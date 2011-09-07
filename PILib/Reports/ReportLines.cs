using PILib.PIReport;

namespace PILib.Reports
{
	public class ReportLines : Report
	{
		public ReportLines(string fileName,string xmlString=null) : base(fileName,xmlString) { }
		public override void init() {
			this.addCalcFunction("otd_VL_110", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.110.Sve.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Iva.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Kau.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.ChTec.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Ber.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.KShT1.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.KShT2.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Dub.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Vdz1.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Vdz2.30.Out").ComputedVal;
			});
			this.addCalcFunction("otd_KL_110", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.6kv.Filt1.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.6kv.Filt2.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.6kv.Schl1.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.6kv.Schl2.30.Out").ComputedVal;
			});
			this.addCalcFunction("otd_VL_220", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.220.Izh1.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.220.Izh2.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.220.Kau1.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.220.Kau2.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.220.Sve.30.Out").ComputedVal;
			});

			this.addCalcFunction("otd_VL_500", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.500.Eml.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.500.Kar.30.Out").ComputedVal +
						tag.getContextTag("VOTGES.SUE.500.Vtk.30.Out").ComputedVal;
			});
			this.addCalcFunction("otd_VL_110_6", delegate(Tag tag) {
				return tag.getContextTag("otd_VL_110").ComputedVal + tag.getContextTag("otd_KL_110").ComputedVal;
			});

			this.addCalcFunction("otd_VL_220_500", delegate(Tag tag) {
				return tag.getContextTag("otd_VL_220").ComputedVal + tag.getContextTag("otd_VL_500").ComputedVal;
			});
			this.addCalcFunction("otd_VL_110_6_220_500", delegate(Tag tag) {
				return tag.getContextTag("otd_VL_110_6").ComputedVal + tag.getContextTag("otd_VL_220_500").ComputedVal;
			});










			this.addCalcFunction("priem_VL_110", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.110.Sve.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Iva.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Kau.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.ChTec.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Ber.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.KShT1.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.KShT2.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Dub.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Vdz1.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.110.Vdz2.30.In").ComputedVal;
			});

			this.addCalcFunction("priem_KL_110", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.6kv.Filt1.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.6kv.Filt2.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.6kv.Schl1.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.6kv.Schl2.30.In").ComputedVal;
			});

			this.addCalcFunction("priem_VL_220", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.220.Izh1.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.220.Izh2.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.220.Kau1.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.220.Kau2.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.220.Sve.30.In").ComputedVal;
			});

			this.addCalcFunction("priem_VL_500", delegate(Tag tag) {
				return tag.getContextTag("VOTGES.SUE.500.Eml.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.500.Kar.30.In").ComputedVal +
						tag.getContextTag("VOTGES.SUE.500.Vtk.30.In").ComputedVal;
			});
			this.addCalcFunction("priem_VL_110_6", delegate(Tag tag) {
				return tag.getContextTag("priem_VL_110").ComputedVal + tag.getContextTag("priem_KL_110").ComputedVal;
			});

			this.addCalcFunction("priem_VL_220_500", delegate(Tag tag) {
				return tag.getContextTag("priem_VL_220").ComputedVal + tag.getContextTag("priem_VL_500").ComputedVal;
			});
			this.addCalcFunction("priem_VL_110_6_220_500", delegate(Tag tag) {
				return tag.getContextTag("priem_VL_110_6").ComputedVal + tag.getContextTag("priem_VL_220_500").ComputedVal;
			});







			this.addCalcFunction("saldo_VL_110", delegate(Tag tag) {
				return tag.getContextTag("110sve").ComputedVal +
						tag.getContextTag("110iva").ComputedVal +
						tag.getContextTag("110kau").ComputedVal +
						tag.getContextTag("110chTec").ComputedVal +
						tag.getContextTag("110ber").ComputedVal +
						tag.getContextTag("110ksht1").ComputedVal +
						tag.getContextTag("110ksht2").ComputedVal +
						tag.getContextTag("110dub").ComputedVal +
						tag.getContextTag("110vdz1").ComputedVal +
						tag.getContextTag("110vdz2").ComputedVal;
			});

			this.addCalcFunction("saldo_KL_110", delegate(Tag tag) {
				return tag.getContextTag("6filtr1").ComputedVal +
						tag.getContextTag("6filtr2").ComputedVal +
						tag.getContextTag("6schl1").ComputedVal +
						tag.getContextTag("6schl2").ComputedVal;
			});

			this.addCalcFunction("saldo_VL_220", delegate(Tag tag) {
				return tag.getContextTag("220izh1").ComputedVal +
						tag.getContextTag("220izh2").ComputedVal +
						tag.getContextTag("220kau1").ComputedVal +
						tag.getContextTag("220kau2").ComputedVal +
						tag.getContextTag("220sve").ComputedVal;
			});

			this.addCalcFunction("saldo_VL_500", delegate(Tag tag) {
				return tag.getContextTag("500eml").ComputedVal +
						tag.getContextTag("500kar").ComputedVal +
						tag.getContextTag("500vtk").ComputedVal;
			});
			this.addCalcFunction("saldo_VL_110_6", delegate(Tag tag) {
				return tag.getContextTag("saldo_VL_110").ComputedVal + tag.getContextTag("saldo_KL_110").ComputedVal;
			});

			this.addCalcFunction("saldo_VL_220_500", delegate(Tag tag) {
				return tag.getContextTag("saldo_VL_220").ComputedVal + tag.getContextTag("saldo_VL_500").ComputedVal;
			});
			this.addCalcFunction("saldo_VL_110_6_220_500", delegate(Tag tag) {
				return tag.getContextTag("saldo_VL_110_6").ComputedVal + tag.getContextTag("saldo_VL_220_500").ComputedVal;
			});

			

			






		}
	}
}