using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PILib.Chart
{
	public class ChartAnswer
	{
		public ChartData Data { get; set; }
		public ChartProperties Properties { get; set; }
		
		
		public static ChartAnswer getChart(string dataFileName, string propFileName) {
			ChartAnswer chart=new ChartAnswer();
			chart.Data=ChartData.fromXML(dataFileName);
			chart.Properties = ChartProperties.fromXML(propFileName);
			return chart;
		}
	}
}
