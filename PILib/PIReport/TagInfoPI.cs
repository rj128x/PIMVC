using System;
using System.Xml.Linq;

namespace PILib.PIReport
{	
	public class TagInfoPI:TagInfo
	{		
		public double ScaleMult { get; protected set; }
		public double ScaleDiv { get; protected set; }
		public string ServerName { get; protected set; }
		public PISDK.Server server { get; protected set; }
		public PISDK.ArchiveSummariesTypeConstants TagSummariesType { get; protected set; }
		public PISDK.CalculationBasisConstants TagCalculationBasis { get; protected set; }

		public TagInfoPI(XElement element, TagModeEnum mode, Report report)
			: base(element,mode,report) {
				XAttribute serverNameAttr = element.Attribute("serverName");
				XAttribute scaleMultAttr = element.Attribute("scaleMult");
				XAttribute scaleDivAttr = element.Attribute("scaleDiv");

				XAttribute summariesTypeAttr = element.Attribute("summariesType");
			String summariesTypeStr = summariesTypeAttr != null ? summariesTypeAttr.Value : "asAverage";
			TagSummariesType = (PISDK.ArchiveSummariesTypeConstants)Enum.Parse(typeof(PISDK.ArchiveSummariesTypeConstants), summariesTypeStr, true);


			XAttribute calculationBasisAttr = element.Attribute("calculationBasis");
			String calculationBasisStr = calculationBasisAttr != null ? calculationBasisAttr.Value : "cbEventWeightedExcludeEarliestEvent";
			TagCalculationBasis = (PISDK.CalculationBasisConstants)Enum.Parse(typeof(PISDK.CalculationBasisConstants), calculationBasisStr, true);

			this.ServerName = serverNameAttr != null ? serverNameAttr.Value : "DEFAULT";
			this.ScaleDiv = scaleDivAttr != null ? Double.Parse(scaleDivAttr.Value) : 1.0;
			this.ScaleMult = scaleMultAttr != null ? Double.Parse(scaleMultAttr.Value) : 1.0;

			server = PIServerInfo.getPIServer(ServerName);
		}


		public override double getTagComputedVal(Tag tag) {
			return tag.RealVal * ScaleMult / ScaleDiv;
		}
	}
}