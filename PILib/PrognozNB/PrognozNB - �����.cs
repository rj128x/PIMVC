using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PILib.Reports;
using PILib.PBR;
using PILib.Chart;
using PILib.Rashod;

namespace PILib.PrognozNB
{
	public class PrognozNB
	{
		public ReportPrognozNB ReportFakt { get; set; }
		public PBRData PBR { get; protected set; }
		public string FileName { get; protected set; }

		public DateTime DateReportStart { get; protected set; }
		public DateTime DateReportEnd { get; protected set; }
		public DateTime DatePrognozStart { get; protected set; }
		public DateTime DatePrognozEnd { get; protected set; }

		public RashodCalcMode CalcMode { get; protected set; }
		public int FaktMinutes { get; protected set; }
		public string debugId=Guid.NewGuid().ToString();
		protected static NNET.NNET nnet;

		static PrognozNB() {
			nnet = NNET.NNET.getNNET(NNET.NNETMODEL.vges_nb);
		}

		public PrognozNB(string fileName, DateTime dateStart, DateTime dateEnd, RashodCalcMode calcMode=RashodCalcMode.avg, int faktMinutes=10, Dictionary<DateTime, double> userPBR = null,string debugId=null) {
			this.debugId = debugId == null ? Guid.NewGuid().ToString() : debugId;
			Debug.start("prognozNBConstruct",debugId);
			CalcMode=calcMode;
			FaktMinutes=faktMinutes;
			DateReportStart = dateStart;
			DateReportEnd = dateEnd>DateTime.Now?DateTime.Now:dateEnd;
			DatePrognozEnd = dateEnd;
			FileName = fileName;
			
			ReportFakt = new ReportPrognozNB(fileName,CalcMode);
			ReportFakt.debugId = debugId;
			Debug.start("readPBR", debugId);
			if (userPBR == null) {
				PBR = new PBRData(0, dateStart, dateEnd, uralSutki: true, scale: 0.001);
			} else {
				PBR = new PBRData(0, dateStart, dateEnd, uralSutki: true, scale:1, userPBR:userPBR);
			}
			Debug.end("readPBR", debugId);

			Debug.start("readFaktData", debugId);
			ReportFakt.readData(DateReportStart, DateReportEnd, FaktMinutes.ToString()+"m", 20000, false);
			Debug.end("readFaktData", debugId);
			Debug.end("prognozNBConstruct", debugId);
		}

		public SortedList<DateTime, double> getPrognozDataByPBR(DateTime datePrognozStart, ChartData chart) {
			DatePrognozStart=datePrognozStart;
			
			SortedList<DateTime,double> avgPPrognoz=new SortedList<DateTime,double>();

			Debug.start("initPrognoz", debugId);
			DateTime date=datePrognozStart;
			while (date <= DatePrognozEnd) {				
				avgPPrognoz.Add(date, PBR.getAvg(date, 30));
				date = date.AddMinutes(30);
			}
			if (avgPPrognoz.Count % 23 != 0) {
				int addCnt=23-avgPPrognoz.Count % 23;
				for (int i=0; i < addCnt; i++) {
					DateTime last=avgPPrognoz.Last().Key;
					avgPPrognoz.Add(last.AddMinutes(30), avgPPrognoz[last]);
				}
			}
			Debug.end("initPrognoz", debugId);

			Debug.start("getPrognoz", debugId);
			SortedList<DateTime, double> rashods;
			SortedList<DateTime, double> prognoz=getPrognoz(datePrognozStart, avgPPrognoz, null,out rashods);
			//prognoz = getPrognoz(datePrognozStart, avgPPrognoz, prognoz, out rashods);
			Debug.end("getPrognoz", debugId);

			Debug.start("clearPrognoz",debugId);
			while (prognoz.Last().Key > DatePrognozEnd) {
				prognoz.Remove(prognoz.Last().Key);
			}
			while (rashods.Last().Key > DatePrognozEnd) {
				rashods.Remove(rashods.Last().Key);
			}
			Debug.end("clearPrognoz", debugId);


			Debug.start("chartsPrognoz", debugId);
			ChartDataSerie serieNB=new ChartDataSerie();
			serieNB.Name = "nbPrognoz";
			foreach (KeyValuePair<DateTime,double> de in prognoz) {
				serieNB.Points.Add(new ChartDataPoint(de.Key.ToString(), de.Value));
			}
			chart.Series.Add(serieNB);

			ChartDataSerie serieRashod=new ChartDataSerie();
			serieRashod.Name = "rashodAvgPrognoz";
			foreach (KeyValuePair<DateTime,double> de in rashods) {
				serieRashod.Points.Add(new ChartDataPoint(de.Key.ToString(), de.Value));
			}			
			chart.Series.Add(serieRashod);
			Debug.end("chartsPrognoz", debugId);
			return prognoz;
		}

		public SortedList<DateTime, double> getPrognozDataByFakt(DateTime datePrognozStart, ChartData chart, bool isQFakt=false) {
			DatePrognozStart = datePrognozStart;

			ReportPrognozNB data=new ReportPrognozNB(FileName,CalcMode);
			//data.debugId = debugId;
			Debug.start("readFaktData", debugId);
			data.readData(DatePrognozStart, DateReportEnd, "30m", 1000, false);
			Debug.end("readFaktData", debugId);

			SortedList<DateTime,double> avgPPrognoz=new SortedList<DateTime, double>();

			Debug.start("initPrognoz", debugId);
			foreach (DateTime date in data.diffDates) {
				double val=isQFakt ? data.tagsByID["rashodFakt"][date].ComputedVal : data.tagsByID["pFakt"][date].ComputedVal;
				avgPPrognoz.Add(date, val);				
			}
			if (avgPPrognoz.Count % 23 != 0) {
				int addCnt=23 - avgPPrognoz.Count % 23;
				for (int i=0; i < addCnt; i++) {
					DateTime last=avgPPrognoz.Last().Key;
					avgPPrognoz.Add(last.AddMinutes(30), avgPPrognoz[last]);
				}
			}
			Debug.end("initPrognoz", debugId);

			Debug.start("getPrognoz", debugId);
			SortedList<DateTime, double> rashods;
			SortedList<DateTime, double> prognoz=getPrognoz(datePrognozStart, avgPPrognoz, null, out rashods,isQFakt);			
			//prognoz=getPrognoz(datePrognozStart, avgPPrognoz, prognoz, out rashods, isQFakt);
			Debug.end("getPrognoz",debugId);
			//prognoz = getPrognoz(datePrognozStart, avgPPrognoz, prognoz, out rashods, isQFakt);

			Debug.start("clearPrognoz", debugId);
			while (prognoz.Last().Key > DatePrognozEnd) {
				prognoz.Remove(prognoz.Last().Key);
			}
			while (rashods.Last().Key > DatePrognozEnd) {
				rashods.Remove(rashods.Last().Key);
			}
			Debug.end("clearPrognoz", debugId);

			Debug.start("chartsPrognoz",debugId);
			ChartDataSerie serieNB=new ChartDataSerie();
			serieNB.Name = "nbPrognoz";
			foreach (KeyValuePair<DateTime,double> de in prognoz) {
				serieNB.Points.Add(new ChartDataPoint(de.Key.ToString(), de.Value));
			}
			chart.Series.Add(serieNB);

			ChartDataSerie serieRashod=new ChartDataSerie();
			serieRashod.Name = "rashodAvgPrognoz";
			foreach (KeyValuePair<DateTime,double> de in rashods) {
				serieRashod.Points.Add(new ChartDataPoint(de.Key.ToString(), de.Value));
			}
			chart.Series.Add(serieRashod);
			Debug.end("chartsPrognoz", debugId);
			return prognoz;
		}	

		protected ReportPrognozNB readPrev2Hours(DateTime datePrognozStart) {
			Debug.start("readPrev2Hours", debugId);
			ReportPrognozNB report=new ReportPrognozNB(FileName,CalcMode);
			report.readData(datePrognozStart.AddHours(-2).AddMinutes(-30), datePrognozStart, "-30m", 100, false);
			Debug.end("readPrev2Hours", debugId);
			return report;
		}

		protected SortedList<DateTime, double> getPrognoz(DateTime datePrognozStart, SortedList<DateTime, double> pArr,
			SortedList<DateTime, double> prevPrognoz, out SortedList<DateTime, double> rashods, bool isQFakt=false) {
			ReportPrognozNB prevData=readPrev2Hours(datePrognozStart);
			SortedList<int,double>prevDataRashodArray=new SortedList<int, double>();
			SortedList<int,double>prevDataNBArray=new SortedList<int, double>();
			SortedList<int,double>prevDataVBArray=new SortedList<int, double>();
			SortedList<DateTime,double>prognoz=new SortedList<DateTime, double>();

			int index=0;
			foreach (DateTime date in prevData.diffDates) {
				prevDataRashodArray.Add(index,prevData.tagsByID["rashodFakt"][date].ComputedVal);
				prevDataNBArray.Add(index, prevData.tagsByID["nbFakt"][date].ComputedVal);
				prevDataVBArray.Add(index, prevData.tagsByID["vbFakt"][date].ComputedVal);
				index++;
			}


			SortedList<DateTime,double>naporArray=new SortedList<DateTime,double>();
			rashods=new SortedList<DateTime, double>();
			
			
			if (prevPrognoz == null) {
				double napor=prevDataVBArray.Last().Value - prevDataNBArray.Last().Value;
				foreach (KeyValuePair<DateTime,double>de in pArr) {
					naporArray.Add(de.Key, napor);
				}
			} else {
				double vb=prevDataVBArray.Last().Value;
				foreach (KeyValuePair<DateTime,double>de in pArr) {
					naporArray.Add(de.Key, vb-prevPrognoz.First(pr=>pr.Key>=de.Key).Value);
				}
			}

			foreach (KeyValuePair<DateTime,double> de in pArr) {
				double rashod=isQFakt ? de.Value : Rashod.RashodTable.getStationRashod(de.Value, naporArray[de.Key],CalcMode);
				rashods.Add(de.Key, rashod);
				prognoz.Add(de.Key, 0);
			}
			prognoz.Add(rashods.First().Key.AddMinutes(-30), prevDataNBArray[4]);

			double currentNapor=naporArray.First().Value;
			SortedList<DateTime,double> dataForPrognoz=new SortedList<DateTime, double>();
			SortedList<DateTime,double> naporsForPrognoz=new SortedList<DateTime, double>();
			for (int indexPoint=0; indexPoint < pArr.Keys.Count; indexPoint++) {
				DateTime Key=pArr.Keys[indexPoint];
				dataForPrognoz.Add(Key, pArr[Key]);
				naporsForPrognoz.Add(Key, naporArray[Key]);
				if (dataForPrognoz.Count == 23) {
					SortedList<int,double> outputVector=new SortedList<int, double>();
					for (int step=0; step <=3; step++) {
						SortedList<int, double> inputVector=new SortedList<int, double>();
						inputVector[0] = datePrognozStart.Year;
						inputVector[1] = datePrognozStart.DayOfYear;
						inputVector[2] = prevData.resultTags["t"].ComputedVal;

						inputVector[3] = prevDataVBArray[0];
						inputVector[4] = prevDataVBArray[1];
						inputVector[5] = prevDataVBArray[2];
						inputVector[6] = prevDataVBArray[3];

						inputVector[7] = prevDataRashodArray[0];
						inputVector[8] = prevDataRashodArray[1];
						inputVector[9] = prevDataRashodArray[2];
						inputVector[10] = prevDataRashodArray[3];
						inputVector[11] = prevDataRashodArray[4];

						for (int i=0; i < 23; i++) {
							double rashod=0;
							if (!isQFakt) {
								rashod = Rashod.RashodTable.getStationRashod(pArr[dataForPrognoz.Keys[i]], naporsForPrognoz[dataForPrognoz.Keys[i]],CalcMode);
								//Logger.debug(String.Format("{0}: {1} - {2} = {3}", dataForPrognoz.Keys[i], pArr[dataForPrognoz.Keys[i]], naporsForPrognoz[dataForPrognoz.Keys[i]], rashod));
							} else {
								rashod = rashods[dataForPrognoz.Keys[i]];
							}
							
							rashods[dataForPrognoz.Keys[i]] = rashod;
							inputVector[i + 12] = rashod;
						}

						inputVector[35] = prevDataNBArray[0];
						inputVector[36] = prevDataNBArray[1];
						inputVector[37] = prevDataNBArray[2];
						inputVector[38] = prevDataNBArray[3];

						Debug.start("nnet", debugId);												
						outputVector=nnet.calc(inputVector);
						Debug.end("nnet", debugId);

						double k=prevDataNBArray[4] / outputVector[0];
						for (int i=1; i < outputVector.Count; i++) {
							outputVector[i] *= k;
							prognoz[dataForPrognoz.Keys[i - 1]] = outputVector[i];
						}

						for (int i=0; i < 23; i++) {
							naporsForPrognoz[dataForPrognoz.Keys[i]] = prevDataVBArray[4] - prognoz[dataForPrognoz.Keys[i]];							
						}
					}
					
					for (int i=0; i <= 4; i++) {
						prevDataNBArray[i] = outputVector[19 + i];
						prevDataRashodArray[i] = rashods[dataForPrognoz.Keys[18 + i]];
					}
					
					dataForPrognoz.Clear();
				}
			}
			return prognoz;
		}



		public void writeChartDataFakt(ChartData chartData) {
			Debug.start("writeChartDataFakt", debugId);
			ReportFakt.addChartData(chartData, "nbFakt", "nbFakt");
			ReportFakt.addChartData(chartData, "vbFakt", "vbFakt");
			ReportFakt.addChartData(chartData, "rashodFakt", "rashodFakt");
			ReportFakt.addChartData(chartData, "rashodCalc", "rashodCalc");
			ReportFakt.addChartData(chartData, "pFakt", "pFakt");
			ReportFakt.addChartData(chartData, "naporFakt", "naporFakt");
			ReportFakt.addChartData(chartData, "t", "t");
			PBR.addChartDataSerie30(chartData, "pbr");
			Debug.end("writeChartDataFakt", debugId);
		}
	}
}
