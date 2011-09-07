using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMVC.Models.PIReport
{
	public enum ChartMode{visifire,visiblox};
	public class ReportChartModel
	{
		public static int CountCharts=0;
		public string Name { get; protected set; }
		public string DataXML { get; protected set; }
		public string PropXML { get; protected set; }
		public string Width { get; set; }
		public string Height { get; set; }
		public bool IsFullSize { get; set; }
		public int ChartIndex { get; set; }
		public ChartMode Mode { get; set; }

		public ReportChartModel(string name, string dataXML, string propXML, ChartMode Mode=ChartMode.visifire, bool isFullSize = true, string width = "100%", string height = "300px") {
			this.Name = name;
			this.DataXML = dataXML;
			this.PropXML = propXML;
			this.Width = width;
			this.Height = height;
			this.IsFullSize = isFullSize;
			this.ChartIndex = CountCharts++;
			this.Mode = Mode;
		}

		public string FullSizeClass {
			get {
				return IsFullSize ? "resizeFull" : "";
			}
		}
	}
}