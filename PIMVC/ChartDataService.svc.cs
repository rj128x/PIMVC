using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;
using PILib.Chart;
using PILib.PIReport;
using PILib.Reports;
using System.Web;
using PILib;
using PILib.PBR;
using PILib.PrognozNB;
using PILib.Rashod;
using System.Collections.Generic;

namespace PIMVC
{
	[ServiceContract(Namespace = "")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class ChartDataService
	{

		[OperationContract]
		public ChartAnswer getChartData(string fileNameData,string fileNameProperties) {
			Logger.debug(String.Format("Сервис: getChartData {0}, {1}",fileNameData,fileNameProperties));
			try {
				ChartAnswer chart=ChartAnswer.getChart(fileNameData, fileNameProperties);
				Logger.debug(String.Format("Сервис: getChartData Data:{0}({1}), Prop:{2}({3})",
					chart.Data.ToString(),chart.Data.Series.Count.ToString(),chart.Properties.ToString(),chart.Properties.Series.Count.ToString()));
				return chart;
			}catch(Exception e){
				Logger.error(String.Format("Сервис: getChartData {0}",e.Message));				
				return null;
			}
		}

		[OperationContract]
		public ChartAnswer getRynokChart(DateTime dateStart, DateTime dateEnd, string interval, bool recordedData,string fileNameProperties){
			Logger.debug(String.Format("Сервис: getRynokChart {0}-{1}, {2}, {3}, {4}", dateStart, dateEnd, interval, recordedData,fileNameProperties));
			try {
				Report report=new Report("RynokBrief.xml");
				report.readData(dateStart, dateEnd, interval,2000, recordedData);
				ChartData fullData=new ChartData(report);
				//string fileName=report.writeChartData(fullData);
				ChartAnswer chart=new ChartAnswer();
				chart.Data = fullData;
				chart.Properties = ChartProperties.fromXML(fileNameProperties);
				//ChartAnswer chart=ChartAnswer.getChart(fileName, fileNameProperties);
				Logger.debug(String.Format("Сервис: getChartData Data:{0}({1}), Prop:{2}({3})",
					chart.Data.ToString(),chart.Data.Series.Count.ToString(),chart.Properties.ToString(),chart.Properties.Series.Count.ToString()));
				return chart;
			}
			catch(Exception e){
				Logger.error(String.Format("Сервис: getRynokChart {0}",e.Message));				
				return null;
			}
		}

		[OperationContract]
		public GraphVyrabAnswer getGraphVyrabData(DateTime dateStart, DateTime dateEnd, string fileNameProperties) {
			Logger.debug(String.Format("Сервис: getGraphVyrabData {0}-{1}, {2}", dateStart, dateEnd, fileNameProperties));
			String idObj=Guid.NewGuid().ToString();
			try {
				Debug.start("getGraphVyrabData", idObj);
				DateTime date=DateTime.Now.AddHours(-2);

				Debug.start("report", idObj);
				Report report=new Report("RynokFakt.xml");
				report.readData(dateStart, date, "1m", 2000, false);
				ChartData fullData=new ChartData(report);
				Debug.end("report", idObj);

				Debug.start("getPBR", idObj);
				PBRData gtp1Data=new PBRData(1, dateStart, dateEnd);
				gtp1Data.addChartDataSerie30(fullData, "gtp1Plan");

				PBRData gtp2Data=new PBRData(2, dateStart, dateEnd);
				gtp2Data.addChartDataSerie30(fullData, "gtp2Plan");

				PBRData gesData=new PBRData(0, dateStart, dateEnd);
				gesData.addChartDataSerie30(fullData, "gesPlan");

				PBRData gesVyrab=new PBRData(0, dateStart, dateEnd, true, 1.0 / 60.0);
				gesVyrab.addChartDataSerie1(fullData, "vyrabPlan");
				Debug.end("getPBR", idObj);

				Debug.start("report2-3", idObj);
				report.addChartData(fullData,"gesFakt","vyrabFakt",true,1.0/60.0);
				
				Report report3=new Report("VyrabFakt.xml");
				report3.readData(dateStart, date, "1d", 2000, false);
				Debug.end("report2-3", idObj);


				double secs=(double)(date.Ticks - dateStart.Ticks) / 10000000;
				double mins=secs / 60.0;

				Debug.start("answer", idObj);
				GraphVyrabAnswer answer=new GraphVyrabAnswer();
				
				answer.date = date;
				answer.CurrentVals.Add(new GraphVyrabData("ГТП1",gtp1Data.getCurrentVal(date), report.getLastTag("gtp1Fakt",date).ComputedVal,date));
				answer.CurrentVals.Add(new GraphVyrabData("ГТП2", gtp2Data.getCurrentVal(date), report.getLastTag("gtp2Fakt", date).ComputedVal, date));
				answer.CurrentVals.Add(new GraphVyrabData("ГЭС", gesData.getCurrentVal(date), report.getLastTag("gesFakt", date).ComputedVal, date));
				answer.CurrentVals.Add(new GraphVyrabData("Выработка", gesVyrab.getCurrentVal(date), report3.resultTags["gesFakt"].ComputedVal*mins/60.0, date));


				try {
					DateTime hourStart=date.AddMinutes(-date.Minute);
					Report report2=new Report("RynokFakt.xml");
					report2.readData(hourStart, date, "1h", 2000, false);
					answer.HourVals.Add(new GraphVyrabData("ГТП1", gtp1Data.getAvgVal(date), report2.getLastTag("gtp1Fakt", date).ComputedVal, date));
					answer.HourVals.Add(new GraphVyrabData("ГТП2", gtp2Data.getAvgVal(date), report2.getLastTag("gtp2Fakt", date).ComputedVal, date));
					answer.HourVals.Add(new GraphVyrabData("ГЭС", gesData.getAvgVal(date), report2.getLastTag("gesFakt", date).ComputedVal, date));
				} catch {
					answer.HourVals.Add(new GraphVyrabData("ГТП1", 0, 0, date));
					answer.HourVals.Add(new GraphVyrabData("ГТП2", 0, 0, date));
					answer.HourVals.Add(new GraphVyrabData("ГЭС", 0, 0, date));
				}

				
				//string fileName=report.writeChartData(fullData);
				//ChartAnswer chart=ChartAnswer.getChart(fileName, fileNameProperties);
				ChartAnswer chart=new ChartAnswer();
				chart.Data = fullData;
				chart.Properties = ChartProperties.fromXML(fileNameProperties);
				Logger.debug(String.Format("Сервис: getGraphVyrabData Data:{0}({1}), Prop:{2}({3})",
					chart.Data.ToString(), chart.Data.Series.Count.ToString(), chart.Properties.ToString(), chart.Properties.Series.Count.ToString()));
				answer.chartAnswer = chart;

				Debug.end("answer", idObj);
				Debug.end("getGraphVyrabData", idObj);
				Debug.traceAll("getGraphVyrabData", idObj);
				return answer;
			} catch (Exception e) {
				Logger.error(String.Format("Сервис: getGraphVyrabData {0}", e.Message));
				return null;
			}
		}

		[OperationContract]
		public PrognozNBAnswer getPrognozNBFakt(DateTime dateFakt, bool now, int hourStart, int minuteStart,
			PrognozDataType prognozType, Dictionary<DateTime, double> userPBR, int sutkiCount = 1, 
			PrognozRashodCalcType rashodCalcMode = PrognozRashodCalcType.avg, int faktMinutes = 10) {
			Logger.debug(String.Format("Сервис: getPrognozNBFakt {0}: now:{1}, {2},{3}   {4}-{5}", dateFakt,now,hourStart,minuteStart,prognozType.ToString(),sutkiCount));

			RashodCalcMode calcMode=RashodCalcMode.avg;
			switch (rashodCalcMode) {
				case PrognozRashodCalcType.avg:
					calcMode = RashodCalcMode.avg;
					break;
				case PrognozRashodCalcType.max:
					calcMode = RashodCalcMode.max;
					break;
				case PrognozRashodCalcType.min:
					calcMode = RashodCalcMode.min;
					break;
			}
			
			DateTime dateStart=dateFakt.Date;
			DateTime dateEnd=dateFakt.Date.AddDays(sutkiCount);
			hourStart = now ? DateTime.Now.Hour : hourStart;
			minuteStart = now ? DateTime.Now.Minute : minuteStart;
			DateTime prognozStart=dateFakt.Date.AddHours(hourStart).AddMinutes(minuteStart);

			userPBR = prognozType == PrognozDataType.PBRUser ? userPBR : null;

			String idObj=Guid.NewGuid().ToString();
			Debug.start("prognozNB", idObj);
			try {
				PrognozNBAnswer prognozNBAnswer=new PrognozNBAnswer();
				prognozNBAnswer.Date = prognozStart;
				prognozNBAnswer.Chart = new ChartAnswer();
				prognozNBAnswer.Chart.Properties = ChartProperties.fromXML(HttpContext.Current.Server.MapPath("/Data/Charts/prognozNBFakt.xml"));
				prognozNBAnswer.Chart.Data = new ChartData();

				Debug.start("getPrognozDataByPBR", idObj);
				PrognozNB prognozNB=new PrognozNB("PrognozNBData.xml", dateStart, dateEnd,calcMode,faktMinutes,userPBR,idObj);				
				switch (prognozType) {
					case PrognozDataType.PBRGekon:
						prognozNB.getPrognozDataByPBR(prognozStart, prognozNBAnswer.Chart.Data);
						break;
					case PrognozDataType.PBRUser:
						prognozNB.getPrognozDataByPBR(prognozStart, prognozNBAnswer.Chart.Data);
						break;
					case PrognozDataType.PFakt:
						prognozNB.getPrognozDataByFakt(prognozStart, prognozNBAnswer.Chart.Data,rashodCalcMode==PrognozRashodCalcType.fakt);
						break;
				}
				Debug.end("getPrognozDataByPBR", idObj);
				
				prognozNB.writeChartDataFakt(prognozNBAnswer.Chart.Data);

				prognozNBAnswer.PBR30 = prognozNB.PBR.Data30;

				Logger.debug(String.Format("Сервис: getPrognozNBFakt Data:{0}({1}), Prop:{2}({3})",
					prognozNBAnswer.Chart.Data.ToString(), prognozNBAnswer.Chart.Data.Series.Count.ToString(), prognozNBAnswer.Chart.Properties.ToString(), prognozNBAnswer.Chart.Properties.Series.Count.ToString()));

				Debug.end("prognozNB", idObj);
				Debug.traceAll("prognozNB", idObj);
				return prognozNBAnswer;
			} catch (Exception e) {
				Logger.error(String.Format("Сервис: getPrognozNBFakt {0}", e.Message));
				Logger.error(String.Format("Сервис: getPrognozNBFakt {0}", e.StackTrace));
				return null;
			}
		}
		

		// Добавьте здесь дополнительные операции и отметьте их атрибутом [OperationContract]
	}

}
