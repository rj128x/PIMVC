using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using PILib.CustomReport;
using PILib.XMLSer;
using System.Web;
using PILib;
using System.Collections.Generic;
using PILib.PIReport;
using System.IO;

namespace PIMVC
{
	[ServiceContract(Namespace = "")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class CustomReportService
	{
		[OperationContract]
		public CustomReport getReport() {
			Logger.debug("GetReport");
			try {
				CustomReport report;
				report = XMLSer<CustomReport>.fromXML(HttpContext.Current.Server.MapPath("/Data/CustomReport/") + "report.xml");
				Logger.debug("ok");
				return report;
			} catch (Exception e) {
				Logger.error(String.Format("GetReport {0}:\n {1}", e.Message, e.StackTrace));
				return null;
			}
		}

		[OperationContract]
		public CustomReportTags getReportTags(CustomReportSection section) {
			Logger.debug("GetReportTags");
			try {
				CustomReportTags tags=new CustomReportTags();
				tags.readTags(section);				
				Logger.debug("ok");
				return tags;
			} catch (Exception e) {
				Logger.error(String.Format("GetReportTags {0}:\n {1}", e.Message, e.StackTrace));
				return null;
			}
		}

		[OperationContract]
		public bool createReportXML(string fileName,Dictionary<string,CustomReportDataString> selectedTags) {
			Logger.debug("createReportXML");
			try {
				CustomReportStructure report=new CustomReportStructure();
				report.SelectedTags = new List<CustomReportDataString>(selectedTags.Values);
				XMLSer<CustomReportStructure>.toXML(report,HttpContext.Current.Server.MapPath("/TempData/") + fileName);
				return true;
			} catch (Exception e) {
				Logger.error(String.Format("createReportXML {0}:\n {1}", e.Message, e.StackTrace));
				return false;
			}
		}

		[OperationContract]
		public CustomReportStructure createReportFromXML(string fileName) {
			Logger.debug("createReportFromXML");
			try {
				CustomReportStructure report;
				report=XMLSer<CustomReportStructure>.fromXML(HttpContext.Current.Server.MapPath("/TempData/") + fileName);
				if (report == null) {
					report=new CustomReportStructure();
					report.SelectedTags = new List<CustomReportDataString>();
				}
				return report;
			} catch (Exception e) {
				Logger.error(String.Format("createReportFromXML {0}:\n {1}", e.Message, e.StackTrace));
				return null;
			}
		}
	}
}
