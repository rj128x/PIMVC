using System;
using System.Collections.Generic;
using System.Linq;
using PILib.PIReport;
using System.Text;
using PILib.Rashod;

namespace PILib.Reports
{
	public class ReportPrognozNB:Report
	{
		RashodCalcMode calcMode;
		Random random=new Random();
		public ReportPrognozNB(string fileName, string xmlString = null) : base(fileName, xmlString) { }
		public ReportPrognozNB(string fileName, RashodCalcMode calcMode=RashodCalcMode.avg) : base(fileName) {
			this.calcMode = calcMode;
		}
		public override void init() {
			this.addCalcFunction("naporFakt", delegate(Tag tag) {
				return tag.getContextTag("vbFakt").ComputedVal - tag.getContextTag("nbFakt").ComputedVal;
			});
			this.addCalcFunction("rashodCalc", delegate(Tag tag) {
				double pFakt=tag.getContextTag("pFakt").ComputedVal;
				double naporFakt=tag.getContextTag("naporFakt").ComputedVal;
				double rashodCalc = RashodTable.getStationRashod(pFakt, naporFakt, calcMode);
				//double rashodCalc=0;
				return rashodCalc;
			});
		}

		/*protected override void preCalc(){
			foreach (DateTime date in diffDates) {
				int mins=(date - date.Date).Minutes + (date - date.Date).Hours * 60;
				tagsByID["nbFakt"][date].RealVal = 66.80 + mins/1440.0;
				tagsByID["vbFakt"][date].RealVal = 87.80 + mins / 1440.0;
				tagsByID["rashodFakt"][date].RealVal = 3000 + 1000 * mins / 1440.0;
				tagsByID["pFakt"][date].RealVal = 500 + 300 * mins / 1440.0;
			}
		}*/
	}
}
