using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PILib.PIReport;

namespace PILib.CustomReport
{
	public enum CustomReportTagMode { min, max, avg }
	public class CustomReportSection
	{
		public String Title { get; set; }
		public string FindString { get; set; }
		public List<CustomReportSection> Children { get; set; }

		public CustomReportSection() {
			Children = new List<CustomReportSection>();
		}
	}

	public class CustomReport
	{
		public CustomReportSection MainSection;
		public CustomReport() {			
		}
	}

	public class CustomReportDataString
	{
		public string TagName { get; set; }
		public string TagTitle { get; set; }
		public bool MinData { get; set; }
		public bool MaxData { get; set; }
		public bool AvgData { get; set; }				

	}
}