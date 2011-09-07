using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PILib.Rashod
{
	public enum RashodCalcMode{avg,min,max};
	
	public class RashodTable
	{
		protected double[] napors;
		protected double[] powers;
		protected double[][] rashods;
		protected SortedList<double,SortedList<double,double>> rashodsByNapor, rashodsByPower;
		protected double minPower;
		protected double maxPower;
		protected double minNapor;
		protected double maxNapor;
		protected double stepNapor;
		protected double stepPower;

		protected static RashodTable[] rashodTables;
		public static string debugId=Guid.NewGuid().ToString();

		static RashodTable() {
			rashodTables = new RashodTable[13];
			
		}

		protected RashodTable(int ga) {
			Debug.start("initRashodTable_"+ga,debugId);
			string str=null;
			int len=111;
			switch (ga) {
				case 12:
					str = PILib.rashods.opt;
					len = 1035;
					break;
				case 11:
					str = PILib.rashods.avg;
					len = 1035;
					break;
				case 1:
					str = PILib.rashods._1;
					break;
				case 2:
					str = PILib.rashods._2;
					break;
				case 3:
					str = PILib.rashods._3;
					break;
				case 4:
					str = PILib.rashods._4;
					break;
				case 5:
					str = PILib.rashods._5;
					break;
				case 6:
					str = PILib.rashods._6;
					break;
				case 7:
					str = PILib.rashods._7;
					break;
				case 8:
					str = PILib.rashods._8;
					break;
				case 9:
					str = PILib.rashods._9;
					break;
				case 10:
					str = PILib.rashods._10;
					break;
			}
			string[]rows=str.Split('\n');
			int Row = 0;
			foreach (string strLine in rows) {
				try {
					string[] Line = strLine.Split(';');
					if (Row == 0) {
						//strArray = new string[Line.Length, 111];
						napors = new double[Line.Length - 1];
						rashods = new double[len][];
						powers = new double[len];
						for (int column=1; column < Line.Length; column++) {
							napors[column - 1] = Double.Parse(Line[column]);
						}
					} else {
						rashods[Row-1]=new double[Line.Length - 1];
						powers[Row - 1] = Double.Parse(Line[0]);
						for (int column=1; column < Line.Length; column++) {
							rashods[Row - 1][column - 1] = Double.Parse(Line[column]);
						}
					}
					Row++;
				} catch { }
			}
			minPower = powers[0];
			maxPower = powers[powers.Length - 1];
			minNapor = napors[0];
			maxNapor = napors[napors.Length - 1];
			stepPower = powers[1] - powers[0];
			stepNapor = napors[1] - napors[0];

			rashodsByNapor = new SortedList<double, SortedList<double, double>>();
			rashodsByPower = new SortedList<double, SortedList<double, double>>();

			for (int naporIndex=0; naporIndex < napors.Length; naporIndex++) {
				double napor=napors[naporIndex];
				rashodsByNapor.Add(napor, new SortedList<double, double>());
				for (int powerIndex=0; powerIndex < powers.Length; powerIndex++) {
					try {
						double rashod=rashods[powerIndex][naporIndex];
						double power=powers[powerIndex];
						rashodsByNapor[napor].Add(power, rashod);
					}catch{}
				}
			}

			for (int powerIndex=0; powerIndex < powers.Length; powerIndex++) {
				double power=powers[powerIndex];
				try {
					rashodsByPower.Add(power, new SortedList<double, double>());
					for (int naporIndex=0; naporIndex < napors.Length; naporIndex++) {
						try {
							double rashod=rashods[powerIndex][naporIndex];
							double napor=napors[powerIndex];
							rashodsByPower[power].Add(napor, rashod);
						} catch { }
					}
				} catch { };
			}
			Debug.end("initRashodTable_" + ga, debugId);
			Debug.traceAll("initRashodTable_" + ga, debugId);
		}

		public static RashodTable getRashodTable(int ga) {
			if (rashodTables[ga] == null) {				
				rashodTables[ga] = new RashodTable(ga);				
			}
			return rashodTables[ga];
		}

		protected double getRashod(double power, double napor) {
			try {								
				KeyValuePair<double,SortedList<double,double>> naporRashod1;
				KeyValuePair<double,SortedList<double,double>> naporRashod2;

							

				if ((napor < maxNapor)&&(napor>minNapor)) {
					naporRashod1 = rashodsByNapor.Last(de => de.Key <= napor);
					naporRashod2 = rashodsByNapor.First(de => de.Key >= napor);
				} else if (napor>minNapor){
					naporRashod1 = rashodsByNapor.Last(de => de.Key < maxNapor);
					naporRashod2 = rashodsByNapor.First(de => de.Key == maxNapor);
				} else{
					naporRashod1 = rashodsByNapor.First(de => de.Key == minNapor);
					naporRashod2 = rashodsByNapor.First(de => de.Key > minNapor);
				}
				
				
				KeyValuePair<double,double>powerRashod11=naporRashod1.Value.Last(de => de.Key <= power);
				KeyValuePair<double,double>powerRashod12=naporRashod1.Value.First(de => de.Key >= power);
				KeyValuePair<double,double>powerRashod21=naporRashod2.Value.Last(de => de.Key <= power);
				KeyValuePair<double,double>powerRashod22=naporRashod2.Value.First(de => de.Key >= power);

				double naporK=(napor - naporRashod1.Key) / (naporRashod2.Key - naporRashod1.Key);
				naporK = Double.IsNaN(naporK) ? 0 : naporK;
				double powerK=(power - powerRashod11.Key) / (powerRashod12.Key - powerRashod11.Key);
				powerK = Double.IsNaN(powerK) ? 0 : powerK;

				double rashod1=powerRashod11.Value + (powerRashod12.Value - powerRashod11.Value) * powerK;
				double rashod2=powerRashod21.Value + (powerRashod22.Value - powerRashod21.Value) * powerK;
				double rashod=rashod1 + (rashod2 - rashod1) * naporK;
				//Logger.debug(String.Format("Расход мощность: {0} napor: {1} ({2})", power, napor, rashod));
				return rashod;
			} catch (Exception e) {
				Logger.error(String.Format("Ошибка получения расхода мощность: {0} napor: {1} ({2})",power,napor,e.Message));
				return 0;
			}
		}

		protected static double getOptimRashod(double power, double napor, bool min=true) {
			try {
				if (power < 35) 
					return 0;
				double minRashod=10e6;
				double maxRashod=0;
				for (int count=1; count <= 10; count++) {
					double divPower=(double) power / (double)count;
					if ((divPower<35)||(divPower>110))
						continue;
					SortedList<double,int>rashods=new SortedList<double,int>();
					for (int ga=1; ga <= 10; ga++) {
						double rashodGA=RashodTable.getRashod(ga, divPower, napor);
						while (rashods.Keys.Contains(rashodGA))
							rashodGA += 10e-20;
						rashods.Add(rashodGA,ga);
					}

					if (min) {
						double fullRashod=0;
						for (int i=0; i < count; i++) {
							fullRashod += rashods.Keys[i];
						}
						if (minRashod > fullRashod)
							minRashod = fullRashod;
					} else {
						double fullRashod=0;
						for (int i=0; i < count; i++) {
							fullRashod += rashods.Keys[9 - i];
						}
						if (maxRashod < fullRashod)
							maxRashod = fullRashod;
					}
					
				}
				return min?minRashod:maxRashod;
			} catch (Exception e) {
				Logger.error(String.Format("Ошибка получения оптимального расхода мощность: {0} napor: {1} ({2})", power, napor, e.Message));
				Logger.error(e.StackTrace);
				return 0;
			}
		}

		protected double getPower(double rashod, double napor) {
			try {
				KeyValuePair<double,SortedList<double,double>> naporRashod1;
				KeyValuePair<double,SortedList<double,double>> naporRashod2;



				if ((napor < maxNapor) && (napor > minNapor)) {
					naporRashod1 = rashodsByNapor.Last(de => de.Key <= napor);
					naporRashod2 = rashodsByNapor.First(de => de.Key >= napor);
				} else if (napor > minNapor) {
					naporRashod1 = rashodsByNapor.Last(de => de.Key < maxNapor);
					naporRashod2 = rashodsByNapor.First(de => de.Key == maxNapor);
				} else {
					naporRashod1 = rashodsByNapor.First(de => de.Key == minNapor);
					naporRashod2 = rashodsByNapor.First(de => de.Key > minNapor);
				}

				KeyValuePair<double,double>powerRashod11=naporRashod1.Value.Last(de => de.Value <= rashod);
				KeyValuePair<double,double>powerRashod12=naporRashod1.Value.First(de => de.Value >= rashod);
				KeyValuePair<double,double>powerRashod21=naporRashod2.Value.Last(de => de.Value <= rashod);
				KeyValuePair<double,double>powerRashod22=naporRashod2.Value.First(de => de.Value >= rashod);

				double naporK=(napor - naporRashod1.Key) / (naporRashod2.Key - naporRashod1.Key);
				naporK = Double.IsNaN(naporK) ? 0 : naporK;
				double rashod1K=(rashod - powerRashod11.Value) / (powerRashod12.Value - powerRashod11.Value);
				rashod1K = Double.IsNaN(rashod1K) ? 0 : rashod1K;
				double rashod2K=(rashod - powerRashod21.Value) / (powerRashod22.Value - powerRashod21.Value);
				rashod2K = Double.IsNaN(rashod2K) ? 0 : rashod2K;

				double power1=powerRashod11.Key + (powerRashod12.Key - powerRashod11.Key) * rashod1K;
				double power2=powerRashod21.Key + (powerRashod22.Key - powerRashod21.Key) * rashod2K;
				double power=power1 + (power2 - power1) * naporK;

				//Logger.debug(String.Format("Мощность расход: {0} napor: {1} ({2})", rashod, napor, power));
				return rashod;
			} catch (Exception e) {
				Logger.error(String.Format("Ошибка получения мощности расход: {0} napor: {1} ({2})", rashod, napor, e.Message));
				return 0;
			}
		}

		public static double getRashod(int ga, double power, double napor) {
			return getRashodTable(ga).getRashod(power, napor);
		}

		public static double getStationRashod(double power,double napor, RashodCalcMode mode){
			if (napor<16)
				mode=RashodCalcMode.min;
			switch (mode){
				case RashodCalcMode.avg:
					return getRashodTable(11).getRashod(power,napor);
				case RashodCalcMode.min:
					return getOptimRashod(power, napor,true);
				case RashodCalcMode.max:
					return getOptimRashod(power, napor,false);
				default:
					return getRashodTable(12).getRashod(power, napor);
			}
		}

		
	}
}
