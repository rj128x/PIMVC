using PILib.PIReport;
using System;

namespace PILib.Reports
{
	public class ReportRynokSN:Report
	{
		public enum type{minus,plus,abs,normal};
		public ReportRynokSN(string fileName, string xmlString = null) : base(fileName, xmlString) { }
		public override void init() {
			this.addCalcFunctions("gesFakt", "gesPlan", "gesDiff", "gesDiffMinus", "gesDiffPlus", "gesDiffAbs", "gesDiffAbsProc",
				"gesDiffSI", "gesDiffSIMinus", "gesDiffSIPlus", "gesDiffSIAbs", "gesDiffSIAbsProc");
			this.addCalcFunctions("gtp1Fakt", "gtp1Plan", "gtp1Diff", "gtp1DiffMinus", "gtp1DiffPlus", "gtp1DiffAbs", "gtp1DiffAbsProc",
				"gtp1DiffSI", "gtp1DiffSIMinus", "gtp1DiffSIPlus", "gtp1DiffSIAbs", "gtp1DiffSIAbsProc");
			this.addCalcFunctions("gtp2Fakt", "gtp2Plan", "gtp2Diff", "gtp2DiffMinus", "gtp2DiffPlus", "gtp2DiffAbs", "gtp2DiffAbsProc",
				"gtp2DiffSI", "gtp2DiffSIMinus", "gtp2DiffSIPlus", "gtp2DiffSIAbs", "gtp2DiffSIAbsProc");		
		}

		protected void addCalcFunctions(string faktStr, string planStr, 
			 string diffStr, string diffMinusStr, string diffPlusStr, string diffAbsStr, string diffAbsProcStr, 
			 string diffSIStr, string diffSIMinusStr, string diffSIPlusStr, string diffSIAbsStr, string diffSIAbsProcStr) {
			this.addCalcFunction(diffStr, delegate(Tag tag) {
				return tag.getContextTag(faktStr).ComputedVal - tag.getContextTag(planStr).ComputedVal;
			});

			this.addCalcFunction(diffMinusStr, delegate(Tag tag) {
				return tag.getContextTag(diffStr).ComputedVal < 0 ? tag.getContextTag(diffStr).ComputedVal : 0;
			});

			this.addCalcFunction(diffPlusStr, delegate(Tag tag) {
				return tag.getContextTag(diffStr).ComputedVal > 0 ? tag.getContextTag(diffStr).ComputedVal : 0;
			});

			this.addCalcFunction(diffAbsStr, delegate(Tag tag) {
				return Math.Abs(tag.getContextTag(diffStr).ComputedVal);
			});

			this.addCalcFunction(diffAbsProcStr, delegate(Tag tag) {
				return getDiffProc(tag.getContextTag(diffAbsStr).ComputedVal, tag.getContextTag(planStr).ComputedVal);
			});

		

			this.addCalcFunction(diffSIStr, delegate(Tag tag) {
				return getDiffSI(tag.getContextTag(faktStr).ComputedVal, tag.getContextTag(planStr).ComputedVal, type.normal);
			});

			this.addCalcFunction(diffSIMinusStr, delegate(Tag tag) {
				return getDiffSI(tag.getContextTag(faktStr).ComputedVal, tag.getContextTag(planStr).ComputedVal, type.minus);
			});

			this.addCalcFunction(diffSIPlusStr, delegate(Tag tag) {
				return getDiffSI(tag.getContextTag(faktStr).ComputedVal, tag.getContextTag(planStr).ComputedVal, type.plus);
			});

			this.addCalcFunction(diffSIAbsStr, delegate(Tag tag) {
				return getDiffSI(tag.getContextTag(faktStr).ComputedVal, tag.getContextTag(planStr).ComputedVal, type.abs);
			});

			this.addCalcFunction(diffSIAbsProcStr, delegate(Tag tag) {
				return getDiffProc(tag.getContextTag(diffSIAbsStr).ComputedVal, tag.getContextTag(planStr).ComputedVal);
			});


		}

		protected double getDiffSI(double fakt,double plan,type mode){
			double diff=fakt-plan;
			double diffProc=plan == 0 ? (fakt == 0 ? 0 : 100) : diff / plan * 100;
			diff= Math.Abs(diffProc) < 3 ? diff : 0;
			switch (mode) {
				case type.abs:
					return Math.Abs(diff);
				case type.minus:
					return diff<0?diff:0;
				case type.plus:
					return diff > 0 ? diff : 0;
				case type.normal:
					return diff;
			}
			return diff;			
		}

		protected double getDiffProc(double diff,double plan) {			
			double diffProc=plan == 0 ? (diff == 0 ? 0 : 100) : diff / plan * 100;
			return diffProc;
		}

		public double GTP1KSI {
			get {
				return resultTags["gtp1DiffSIAbsProc"].ComputedVal / 100;
			}
		}

		public double GTP2KSI {
			get {
				return resultTags["gtp2DiffSIAbsProc"].ComputedVal / 100;
			}
		}

		public double GTP1SI {
			get {
				return resultTags["gtp1DiffSIAbs"].ComputedVal;
			}
		}

		public double GTP2SI {
			get {
				return resultTags["gtp2DiffSIAbs"].ComputedVal;
			}
		}

		public double SUMSI {
			get {
				return GTP1KSI * 220 + GTP2KSI * 800;
			}
		}

	}
}