using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PILib.PIReport;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace PILib.Chart
{
	public class ChartDataSerie
	{
		public List<ChartDataPoint> Points { get; set; }
		public string Name{get; set;}		

		public ChartDataSerie() {
			Points = new List<ChartDataPoint>();
		}

	}
}
