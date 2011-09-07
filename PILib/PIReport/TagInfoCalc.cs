using System.Xml.Linq;

namespace PILib.PIReport
{
	public class TagInfoCalc:TagInfo
	{
		public delegate double GetTagValDelegate(Tag tag);

		public GetTagValDelegate calcFunction;
		public TagInfoCalc(XElement element, TagModeEnum mode, Report report)
			: base(element,mode,report) {
			calcFunction = defaultCalcFunction;
		}

		public override double getTagComputedVal(Tag tag) {
			return calcFunction(tag);
		}

		public static double defaultCalcFunction(Tag tag) {
			return 0;
			
		}
		
	}
}