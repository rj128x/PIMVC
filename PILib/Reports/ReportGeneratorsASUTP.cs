using PILib.PIReport;

namespace PILib.Reports
{
	public class ReportGeneratorsASUTP:Report
	{
		public ReportGeneratorsASUTP(string fileName, string xmlString = null) : base(fileName,xmlString) { }
		public override void init() {

			this.addCalcFunction("otdP", delegate(Tag tag) {
				return tag.getContextTag("ga10").ComputedVal +
					tag.getContextTag("ga1").ComputedVal +
					tag.getContextTag("ga2").ComputedVal +
					tag.getContextTag("ga3").ComputedVal +
					tag.getContextTag("ga4").ComputedVal +
					tag.getContextTag("ga5").ComputedVal +
					tag.getContextTag("ga6").ComputedVal +
					tag.getContextTag("ga7").ComputedVal +
					tag.getContextTag("ga8").ComputedVal +
					tag.getContextTag("ga9").ComputedVal;					
			});

		}
	}
}