using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PILib.Piramida3000DataSetTableAdapters;
using System.Data;
using PILib.Chart;

namespace PILib.PBR
{
	public class PBRData
	{
		public DateTime DateStart { get; protected set; }
		public DateTime DateEnd { get; protected set; }
		public int GTPIndex { get; protected set; }
		public SortedList<DateTime, double> Data30 { get; protected set; }
		public SortedList<DateTime, double> ProcessData30 { get; protected set; }
		public SortedList<DateTime, double> ProcessData1 { get; protected set; }
		public bool Integrate { get; set; }
		public double Scale { get; set; }
		public bool UralSutki { get; set; }

		public PBRData(int GTPIndex,DateTime dateStart, DateTime dateEnd,bool integrate=false, double scale=1,bool uralSutki=false,Dictionary<DateTime,double> userPBR=null) {
			this.DateStart = dateStart;
			this.DateEnd = dateEnd;
			this.UralSutki = uralSutki;			
			this.GTPIndex = GTPIndex;
			this.Integrate = integrate;
			this.Scale = scale;
			if (userPBR == null) {
				readData();
			} else {
				convertUserPBR(userPBR);
			}
			processData();
		}

		protected void convertUserPBR(Dictionary<DateTime, double> userPBR) {
			bool first=true;
			double prevVal=0;
			Data30 = new SortedList<DateTime, double>();
			foreach (KeyValuePair<DateTime,double> de in userPBR) {
				double val= de.Value * Scale;
				if (Data30.Keys.Contains(de.Key))
					continue;
				Data30.Add(de.Key, de.Value * Scale);
				if (!first) {
					double mid=(val + prevVal) / 2.0;
					Data30.Add(de.Key.AddMinutes(-30), mid);
				}
				prevVal = val;
				first = false;
			}
			longData(DateStart, DateEnd);
		}

		protected void readData() {
			int index=0;
			switch (GTPIndex) {
				case 1:
					index = 2;
					break;
				case 2:
					index = 3;
					break;
				case 0:
					index = 1;
					break;
			}
			DateTime dateStart=UralSutki ? DateStart.AddHours(-2) : DateStart;
			DateTime dateEnd=UralSutki ? DateEnd.AddHours(-2) : DateEnd;
			Data30 = new SortedList<DateTime, double>();
			Piramida3000DataSetTableAdapters.DATATableAdapter adapter=new Piramida3000DataSetTableAdapters.DATATableAdapter();			
			Piramida3000DataSet.DATADataTable table=adapter.GetGTPPBR(index, dateStart, dateEnd);
			//adapter.FillGTPPBR(table, 1, DateStart, DateEnd);
			foreach (DataRow Row in table.Rows) {
				DateTime date=DateTime.Parse(Row[table.DATA_DATEColumn].ToString());
				if (UralSutki) {
					date = date.AddHours(2);
				}
				double val=(double)Row[table.VALUE0Column];
				val = val * Scale;

				Data30.Add(date, val);				
			}
			longData(DateStart, DateEnd);
		}

		protected void longData(DateTime dateStart, DateTime dateEnd) {
			DateTime date=dateStart;
			while (date <= dateEnd) {
				if (!Data30.Keys.Contains(date)) {
					if (Data30.Keys.Contains(date.AddHours(-24))){
						Data30.Add(date,Data30[date.AddHours(-24)]);
					}else{
						Data30.Add(date, 0);
					}
				}
				date = date.AddMinutes(30);
			}
		}


		protected void processData() {
			ProcessData30 = new SortedList<DateTime, double>();
			ProcessData1 = new SortedList<DateTime, double>();
			DateTime lastDate=Data30.Last().Key;
			double sum=0;
			foreach (KeyValuePair<DateTime,double> de in Data30){
				bool last=de.Key == lastDate;
				DateTime dt=de.Key.AddMinutes(-15);
				double val=de.Value;


				for (int minute=0; minute < 30; minute++) {
					DateTime dtMin=dt.AddMinutes(minute);
					if ((dtMin >= DateStart) && (dtMin <= DateEnd)) {
						sum += val;
						ProcessData1.Add(dtMin, Integrate?sum:val);
					}
				}

				dt=dt<DateStart?DateStart:dt;
				dt = last? DateEnd : dt;
				ProcessData30.Add(dt, val);
			}			

		}

		public void addChartDataSerie1(ChartData chartData, string serieName){
			ChartDataSerie serie=new ChartDataSerie();
			serie.Name = serieName;
			foreach (KeyValuePair<DateTime,double>de in ProcessData1) {
				serie.Points.Add(new ChartDataPoint(de.Key.ToString(),de.Value));
			}
			chartData.Series.Add(serie);
		}

		public void addChartDataSerie30(ChartData chartData, string serieName) {
			ChartDataSerie serie=new ChartDataSerie();
			serie.Name = serieName;
			foreach (KeyValuePair<DateTime,double>de in ProcessData30) {
				serie.Points.Add(new ChartDataPoint(de.Key.ToString(), de.Value));
			}
			chartData.Series.Add(serie);
		}		

		public double getCurrentVal(DateTime date) {
			KeyValuePair<DateTime,double> first= ProcessData1.First(de => de.Key > date);
			return first.Value;
		}

		public double getAvgVal(DateTime date) {
			double sum=0;
			int count=0;
			foreach (KeyValuePair <DateTime,double> de in ProcessData1) {
				if ((de.Key.Hour == date.Hour)&&(de.Key<=date)) {
					sum += de.Value;
					count++;
				}
			}
			
			return sum/count;
		}

		public double getAvg(DateTime date, int minutes) {
			DateTime ds=date.AddMinutes(minutes);
			DateTime dt=date;
			double sum=0;
			int count=0;
			foreach (KeyValuePair <DateTime,double> de in ProcessData1) {
				if ((de.Key >= ds) && (de.Key <= date) || (de.Key <= ds) && (de.Key >= date)) {
					sum += de.Value;
					count++;
				}
			}
			return sum / count;
		}

	}
}
