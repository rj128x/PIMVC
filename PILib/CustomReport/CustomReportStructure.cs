using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using PILib.Chart;

namespace PILib.CustomReport
{
	public class CustomReportStructure
	{
		public List<CustomReportDataString> SelectedTags { get; set; }

		public void createReportXML(string fileName ,string path) {
			Logger.debug("createReportXML");
			try {
				string str="<?xml version='1.0' encoding='utf-8' ?><root xmlns='reportSchema.xsd'>";
				str += "<nameReport>Отчет</nameReport>";
				str += "<items>";
				foreach (CustomReportDataString tag in SelectedTags) {
					if (tag.MinData) {
						str += getItemStr(tag, "asMinimum", "min", " (Мин)");												
					}
					if (tag.MaxData) {
						str += getItemStr(tag, "asMaximum", "max", " (Макс)");
					}
					if (tag.AvgData) {
						str += getItemStr(tag, "asAverage", "avg", " (Средн)");
					}
				}
				str += "</items>";
				str += "</root>";
				StreamWriter myWriter = new StreamWriter(path + fileName);
				myWriter.WriteLine(str);
				myWriter.Close();
			} catch (Exception e) {
				Logger.error(String.Format("createReportXML {0}:\n {1}", e.Message, e.StackTrace));
			}
		}

		public void createReportChart(string fileName, string path) {
			ChartProperties chart=new ChartProperties();
			chart.Series = new List<ChartSerieProperties>();
			chart.XAxisType = XAxisTypeEnum.datetime;
			try {			
				foreach (CustomReportDataString tag in SelectedTags) {
					if (tag.MinData) {
						chart.Series.Add(getItemChartProperties(tag,"min"," (Мин)"));
					}
					if (tag.MaxData) {
						chart.Series.Add(getItemChartProperties(tag, "max", " (Макс)"));
					}
					if (tag.AvgData) {
						chart.Series.Add(getItemChartProperties(tag, "avg", " (Средн)"));
					}
				}
				chart.Axes = new List<ChartAxisProperties>();

				ChartAxisProperties ax=new ChartAxisProperties();
				ax.Auto = true;
				ax.Index = 0;				
				chart.Axes.Add(ax);

				ax=new ChartAxisProperties();
				ax.Auto = true;
				ax.Index = 1;
				chart.Axes.Add(ax);


				chart.toXML(path + fileName);
			} catch (Exception e) {
				Logger.error(String.Format("createReportChart {0}:\n {1}", e.Message, e.StackTrace));
			}
		}

		protected string getItemStr(CustomReportDataString tag, string sumType, string resultMode, string post) {
			string basis=tag.TagName.ToLower().Contains(".sue.") ? "cbEventWeightedExcludeEarliestEvent" : "cbTimeWeighted";			
			string itemstr=String.Format(
				"<item mode='tag' tagName='{0}' title='{1}' summariesType='{2}' resultMode='{3}' calculationBasis ='{4}' id='{5}' toChart='true'  stringFormat='### ### ### ### ##0.##'/>",
				tag.TagName, tag.TagTitle + post, sumType, resultMode, basis, tag.TagName+"_"+resultMode);
			return itemstr;
		}

		protected ChartSerieProperties getItemChartProperties(CustomReportDataString tag, string resultMode, string post) {
			ChartSerieProperties prop=new ChartSerieProperties();
			prop.Enabled = true;
			prop.LineWidth = 2;
			prop.SerieType = ChartSerieType.line;
			prop.TagName = tag.TagName + "_" + resultMode;
			prop.Title = tag.TagTitle+post;
			return prop;
		}
	}
}
