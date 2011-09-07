using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PILib.PIReport;
using PISDK;
using PISDKCommon;

namespace PILib.CustomReport
{
	public class CustomReportTags
	{
		public List<CustomReportDataString> Tags{get;set;}
		
		public void readTags(CustomReportSection section) {
			PISDK.Server server=PIServerInfo.getPIServer("DEFAULT");
			IGetPoints2 srv=server as IGetPoints2;
			NamedValues nvs=new NamedValues();
			PointList points=srv.GetPoints2(section.FindString, nvs,GetPointsRetrievalTypes.useGetPoints);
			Tags = new List<CustomReportDataString>();
			foreach (PIPoint point in points) {
				CustomReportDataString tag=new CustomReportDataString();
				tag.TagName = point.Name;
				tag.TagTitle = point.PointAttributes["descriptor"].Value;
				tag.AvgData = false;
				tag.MaxData = false;
				tag.MinData = false;
				Tags.Add(tag);
			}
		}
	}
}
