using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using SilverChart.ChartDataServiceReference;

namespace SilverChart
{
	public class SilverChartSerieTypes
	{
		public static Dictionary<string,ChartSerieType> TYPES;
		public static Dictionary<ChartSerieType,string> TYPESID;
		public static List<string> TypeNames;



		static SilverChartSerieTypes() {
			TYPES = new Dictionary<string, ChartSerieType>();
			TYPESID = new Dictionary<ChartSerieType, string>();
			TypeNames = new List<string>();
			TYPES.Add("Область", ChartSerieType.area);
			TYPES.Add("Область стэк", ChartSerieType.stackedArea);
			TYPES.Add("Область стэк 100%", ChartSerieType.stackedArea100);

			TYPES.Add("Гориз. колонки", ChartSerieType.bar);
			TYPES.Add("Гориз. колонки стэк", ChartSerieType.stackedBar);
			TYPES.Add("Гориз. колонки стэк 100%", ChartSerieType.stackedBar100);

			TYPES.Add("Верт. колонки", ChartSerieType.column);
			TYPES.Add("Верт. колонки стэк", ChartSerieType.stackedColumn);
			TYPES.Add("Верт. колонки стэк 100%", ChartSerieType.stackedColumn100);

			TYPES.Add("Линия", ChartSerieType.line);
			TYPES.Add("Быстрая линия", ChartSerieType.quickLine);
			TYPES.Add("Сплайн", ChartSerieType.spline);
			TYPES.Add("Ступени", ChartSerieType.stepLine);

			TYPES.Add("Пирог", ChartSerieType.pie);
			TYPES.Add("Радар", ChartSerieType.radar);

			TYPES.Add("Воронка", ChartSerieType.sectionFunnel);
			TYPES.Add("Воронка сорт.", ChartSerieType.streamlineFunnel);
			TYPES.Add("Пирамида", ChartSerieType.pyramid);


			foreach (KeyValuePair<String,ChartSerieType> de in TYPES) {
				TYPESID.Add(de.Value, de.Key);
				TypeNames.Add(de.Key);
			}
		}

		public static ChartSerieType getByName(string name) {
			return TYPES[name];
		}

		public static string getByID(ChartSerieType id) {
			return TYPESID[id];
		}
	}

}
