using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PILib.PIReport;

namespace PIMVC.Models.PIReport
{
	public class ReportModel
	{
		public Report report{get;protected set;}
		public SortedList<string, ReportChartModel> Charts { get; protected set; }
		public int ChartsTableColumns{get;set;}
		public SortedList<string, string> AddTabs { get; set; }
		public ReportRequest Request { get; set; }
		

		public ReportModel(Report report,ReportRequest request) {
			this.report = report;
			this.Request = request;
			ChartsTableColumns = 2;
			Charts=new SortedList<string,ReportChartModel>();
			AddTabs = new SortedList<string, string>();
		}

		public void addChart(string name, string dataXML, string propXML, string width, string height, ChartMode Mode = ChartMode.visifire) {
			ReportChartModel chart=new ReportChartModel(name, dataXML, propXML,Mode,true);
			Charts.Add(name,chart);
		}

		public void addTab(string name,string template) {
			AddTabs.Add(name,template);
		}

		public int getMaxLevel() {
			int maxLevel=0;
			foreach (TagInfo tagInfo in report.tagInfoFullArray) {
				maxLevel = maxLevel < tagInfo.Level ? tagInfo.Level : maxLevel;
			}
			return maxLevel;
		}

		public int getColSpan(TagInfo tagInfo) {
			int count=1;
			foreach (TagInfo child in tagInfo.ChildrenTags) {
				count += getColSpan(child);
			}
			return count;
		}
	}
}