using System;
using System.Xml.Linq;

namespace PILib.PIReport
{
	public class TagInfoExpr : TagInfoPI
	{

		public string Expr { get; protected set; }
		public PISDK.SampleTypeConstants TagSampleType { get; protected set; }
		public string TagSampleInterval { get; protected set; }

		public TagInfoExpr(XElement element, TagModeEnum mode, Report report)
			: base(element, mode, report) {
			XAttribute exprAttr = element.Attribute("expr");

			XAttribute sampleTypeAttr = element.Attribute("sampleType");
			String sampleTypeStr = sampleTypeAttr != null ? sampleTypeAttr.Value : "stInterval";
			TagSampleType = (PISDK.SampleTypeConstants)Enum.Parse(typeof(PISDK.SampleTypeConstants), sampleTypeStr, true);

			TagSampleInterval = element.Attribute("sampleInterval") != null ? element.Attribute("sampleInterval").Value : "1m";

			this.Expr = exprAttr != null ? report.processReplaceString(exprAttr.Value) : "0";
			//this.Expr = "'sinusoid'*'sinusoidu'";
		}

	}
}