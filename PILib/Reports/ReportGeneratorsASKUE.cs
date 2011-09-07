using PILib.PIReport;

namespace PILib.Reports
{
	public class ReportGeneratorsASKUE:Report
	{
		public ReportGeneratorsASKUE(string fileName, string xmlString = null) : base(fileName,xmlString) { }
		public override void init() {
			this.addCalcFunction("otdP", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.GA1.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA2.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA3.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA4.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA5.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA6.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA7.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA8.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA9.30.Out").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA10.30.Out").ComputedVal;				
			});

			this.addCalcFunction("priemP", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.GA1.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA2.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA3.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA4.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA5.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA6.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA7.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA8.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA9.30.In").ComputedVal +
					tag.getContextTag("VOTGES.SUE.GA10.30.In").ComputedVal;
			});

		}
	}
}