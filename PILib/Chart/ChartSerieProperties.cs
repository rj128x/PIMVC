using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PILib.Chart
{
	public enum ChartSerieType { line, bar, pie, column, radar, stepLine, spline, quickLine, 
		area, stackedArea, stackedArea100, stackedColumn, stackedColumn100, stackedBar, stackedBar100, doughnut,
		sectionFunnel, streamlineFunnel, pyramid}
	public class ChartSerieProperties
	{
		public bool SecondaryXAxis { get; set; }
		public string TagName { get; set; }
		public string Title { get; set; }
		public bool Enabled { get; set; }
		public ChartSerieType SerieType { get; set; }
		public int LineWidth { get; set; }
		public string Color { get; set; }
		public bool Marker { get; set; }
		public int YAxisIndex { get; set; }
		

		public ChartSerieProperties() {
			Enabled = true;
			LineWidth = 2;
			Color = null;
			Marker = true;
			YAxisIndex = 0;
		}

		
	}
}
