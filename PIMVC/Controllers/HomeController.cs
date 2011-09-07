using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMVC.Models;
using PILib.PIReport;
using PILib.Reports;
using PILib.Chart;
using PIMVC.Models.PIReport;
using PILib.CustomReport;
using PILib.XMLSer;

namespace PIMVC.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		protected ActionResult initNullReport(string name) {
			ReportRequest request = new ReportRequest();
			request.Name = name;
			return View("ReportNull", request);
		}

		protected ActionResult initReport(ReportRequest request, ReportModel model, string view) {			
			if (request.IsExcel) {
				Response.AddHeader("Content-Disposition", "attachment; filename= Report_"+DateTime.Now.ToString()+".xls");
			}
			model.report.readData(request.DateTimeStart, request.DateTimeEnd, request.IntervalLen,request.MaxPointsCount,request.RecordedData);
			ViewData["request"] = request;
			return View(view, model);
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult TestReport() {
			//ChartProperties.createNullXML("d:/prop.xml");
			return initNullReport("Тест");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult TestReport(ReportRequest request) {
			request.init();
			Report report = new Report("Peretok.xml");
			PILib.Debug.start("query", report.debugId);
			ReportModel model=new ReportModel(report, request);
			ActionResult result=initReport(request, model, request.IsExcel ? "ReportExcel" : "ReportTabs");

			PILib.Debug.end("query", report.debugId);
			PILib.Debug.traceAll("query", report.debugId);
			return result;
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult TestOil() {
			//ChartProperties.createNullXML("d:/prop.xml");
			return initNullReport("Масло");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult TestOil(ReportRequest request) {
			request.init();
			Report report = new Report("oil.xml");
			PILib.Debug.start("query",report.debugId);
			ReportModel model=new ReportModel(report,request);
			ActionResult result=initReport(request, model, request.IsExcel?"ReportExcel":"ReportTabs");

			if ((request.RenderCharts)&&(!request.IsExcel)){
				PILib.Debug.start("charts", report.debugId);
				ChartData fullData=new ChartData(report);			
				model.addChart("chart", model.report.writeChartData(fullData), getChartFileName("oil.xml"),"100%","600px",ChartMode.visiblox);
				PILib.Debug.end("charts", report.debugId);
			}
			PILib.Debug.end("query", report.debugId);
			PILib.Debug.traceAll("query", report.debugId);
			return result;			
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult WaterNK() {
			//ChartProperties.createNullXML("d:/prop.xml");
			return initNullReport("Расходы");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult WaterNK(ReportRequest request) {
			request.init();
			request.IsVert = true;
			Report report = new Report("WaterNK.xml");
			PILib.Debug.start("query", report.debugId);
			ReportModel model=new ReportModel(report, request);
			ActionResult result=initReport(request, model, request.IsExcel ? "ReportExcel" : "ReportTabs");

			PILib.Debug.end("query", report.debugId);
			PILib.Debug.traceAll("query", report.debugId);
			return result;
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GeneratorsASKUE() {
			return initNullReport("Генераторы (АСКУЭ)");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult GeneratorsASKUE(ReportRequest request) {
			request.init();
			Report report = new ReportGeneratorsASKUE("GeneratorsASKUE.xml");
			PILib.Debug.start("query", report.debugId);
			ReportModel model=new ReportModel(report, request);
			ActionResult result=initReport(request, model, request.IsExcel ? "ReportExcel" : "ReportTabs");

			if ((request.RenderCharts) && (!request.IsExcel)) {
				PILib.Debug.start("charts", report.debugId);
				ChartData fullData=new ChartData(report);
				ChartData results=new ChartData();
				string[]titles= { "ГА1", "ГА2", "ГА3", "ГА4", "ГА5", "ГА6", "ГА7", "ГА8", "ГА9", "ГА10" };

				report.addChartDataResult(results, "otdacha",
					new string[] { "VOTGES.SUE.GA1.30.Out", "VOTGES.SUE.GA2.30.Out", "VOTGES.SUE.GA3.30.Out", 
									"VOTGES.SUE.GA4.30.Out","VOTGES.SUE.GA5.30.Out","VOTGES.SUE.GA6.30.Out",
									"VOTGES.SUE.GA7.30.Out","VOTGES.SUE.GA8.30.Out","VOTGES.SUE.GA9.30.Out",
									"VOTGES.SUE.GA10.30.Out"},
					titles);

				report.addChartDataResult(results, "priem",
					new string[] { "VOTGES.SUE.GA1.30.In", "VOTGES.SUE.GA2.30.In", "VOTGES.SUE.GA3.30.In", 
									"VOTGES.SUE.GA4.30.In","VOTGES.SUE.GA5.30.In","VOTGES.SUE.GA6.30.In",
									"VOTGES.SUE.GA7.30.In","VOTGES.SUE.GA8.30.In","VOTGES.SUE.GA9.30.In",
									"VOTGES.SUE.GA10.30.In"},
					titles);

				string resultsFileName=model.report.writeChartData(results);

				model.addChart("График", model.report.writeChartData(fullData), getChartFileName("GeneratorsLine.xml"), "100%", "600px");
				model.addChart("Отдача/Прием", resultsFileName, getChartFileName("GeneratorsBar.xml"), "100%", "600px");
				model.addChart("Отдача/Прием (Радар)", resultsFileName, getChartFileName("GeneratorsRadar.xml"), "100%", "600px");
				model.addChart("Отдача (Пирог)", resultsFileName, getChartFileName("GeneratorsOtdachaPie.xml"), "100%", "600px");
				model.addChart("Отдача (Пирамида)", resultsFileName, getChartFileName("GeneratorsOtdachaPyramid.xml"), "100%", "600px");
				PILib.Debug.end("charts", report.debugId);
			}

			PILib.Debug.end("query", report.debugId);
			PILib.Debug.traceAll("query", report.debugId);
			return result;			
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Lines() {
			return initNullReport("Перетоки");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Lines(ReportRequest request) {
			request.init();
			Report report =new ReportLines("Lines.xml");
			PILib.Debug.start("query", report.debugId);
			ReportModel model=new ReportModel(report, request);
			ActionResult result=initReport(request, model, request.IsExcel ? "ReportExcel" : "ReportTabs");

			if ((request.RenderCharts) && (!request.IsExcel)) {
				PILib.Debug.start("charts", report.debugId);
				ChartData results=new ChartData();
				string[]titles= { "6кВ", "110кВ", "220кВ", "500кВ" };

				report.addChartDataResult(results, "otdacha",
					new string[] { "otd_KL_110", "otd_VL_110", "otd_VL_220", "otd_VL_500" },
					titles);

				report.addChartDataResult(results, "priem",
					new string[] { "priem_KL_110", "priem_VL_110", "priem_VL_220", "priem_VL_500" },
					titles);

				report.addChartDataResult(results, "saldo",
					new string[] { "saldo_KL_110", "saldo_VL_110", "saldo_VL_220", "saldo_VL_500" },
					titles);

				string resultsFileName=model.report.writeChartData(results);

				ChartData fullData=new ChartData(report);
				model.addChart("График работы", model.report.writeChartData(fullData), getChartFileName("Lines.xml"), "100%", "600px");
				model.addChart("Отдача/Прием", resultsFileName, getChartFileName("LinesBar.xml"), "100%", "600px");
				model.addChart("Сальдо (Пирог)", resultsFileName, getChartFileName("LinesPie.xml"), "100%", "600px");
				PILib.Debug.end("charts", report.debugId);
			}

			PILib.Debug.end("query", report.debugId);
			PILib.Debug.traceAll("query", report.debugId);

			return result;			
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GeneratorsASUTP() {
			return initNullReport("Генераторы (АСУТП)");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult GeneratorsASUTP(ReportRequest request) {
			request.init();
			Report report =new ReportGeneratorsASUTP("GeneratorsASUTP.xml");
			PILib.Debug.start("query",report.debugId);
			ReportModel model=new ReportModel(report, request);
			ActionResult result=initReport(request, model, request.IsExcel ? "ReportExcel" : "ReportTabs");

			if ((request.RenderCharts) && (!request.IsExcel)) {
				PILib.Debug.start("charts", report.debugId);
				ChartData fullData=new ChartData(report);
				ChartData resultsFull=new ChartData();
				ChartData resultsSK=new ChartData();
				string[]titlesFull= { "ГА1", "ГА2", "ГА3", "ГА4", "ГА5", "ГА6", "ГА7", "ГА8", "ГА9", "ГА10" };

				string[]titlesSK= { "ГА1", "ГА2", "ГА9", "ГА10" };

				report.addChartDataResult(resultsFull, "otdacha",
					new string[] { "ga1", "ga2", "ga3", "ga4", "ga5", "ga6", "ga7", "ga8", "ga9", "ga10" },
					titlesFull);

				report.addChartDataResult(resultsSK, "q_sk",
					new string[] { "ga1_1", "ga2_1", "ga9_1", "ga10_1" },
					titlesSK);

				report.addChartDataResult(resultsSK, "p_sk",
					new string[] { "ga1_2", "ga2_2", "ga9_2", "ga10_2" },
					titlesSK);


				string resultsFileNameFull=model.report.writeChartData(resultsFull);
				string resultsFileNameSK=model.report.writeChartData(resultsSK);

				model.addChart("График работы", model.report.writeChartData(fullData), getChartFileName("GeneratorsLine.xml"), "100%", "600px");
				model.addChart("Отдача", resultsFileNameFull, getChartFileName("GeneratorsASUTPBar.xml"), "100%", "600px");
				model.addChart("P,Q SK", resultsFileNameSK, getChartFileName("GeneratorsASUTP_Radar.xml"), "100%", "600px");
				PILib.Debug.end("charts", report.debugId);
			}

			PILib.Debug.end("query", report.debugId);
			PILib.Debug.traceAll("query", report.debugId);
			return result;	
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Rynok() {
			return initNullReport("График работы");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Rynok(ReportRequest request) {
			request.init();
			Report report = new ReportRynok("Rynok.xml");
			PILib.Debug.start("query",report.debugId);
			ReportModel model = new ReportModel(report, request);
			ActionResult result = initReport(request, model, request.IsExcel ? "ReportExcel" : "ReportTabs");

			if ((request.RenderCharts) && (!request.IsExcel)) {
				PILib.Debug.start("charts", report.debugId);
				ChartData fullData=new ChartData(report);
				model.addChart("График", model.report.writeChartData(fullData), getChartFileName("Rynok.xml"), "100%", "600px");
				model.addTab("График работы", "RynokBriefChart");
				PILib.Debug.end("charts", report.debugId);
			}

			PILib.Debug.end("query", report.debugId);
			PILib.Debug.traceAll("query", report.debugId);
			return result;
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Custom() {
			ReportRequest request = new ReportRequest();
			request.Query = Guid.NewGuid().ToString() + ".xml";
			request.IsCustom = true;
			request.IsVert = true;
			return View("Custom", request);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Custom(ReportRequest request) {
			request.init();
			request.IsCustom = true;
			CustomReportStructure reportStructure=XMLSer<CustomReportStructure>.fromXML(Server.MapPath("/TempData/") + request.Query);
			string reportFileName="report_"+request.Query;
			string chartFileName="chart_" + request.Query;
			reportStructure.createReportXML(reportFileName, Server.MapPath("/TempData/"));

			Report report = new Report(Server.MapPath("/TempData/") + reportFileName,null,false);
			PILib.Debug.start("query", report.debugId);
			ReportModel model=new ReportModel(report, request);
			ActionResult result=initReport(request, model, request.IsExcel ? "ReportExcel" : "ReportTabs");

			if ((request.RenderCharts) && (!request.IsExcel)) {
				reportStructure.createReportChart(chartFileName, Server.MapPath("/TempData/"));
				PILib.Debug.start("charts", report.debugId);
				ChartData fullData=new ChartData(report);
				string fileName=model.report.writeChartData(fullData);
				model.addChart("График 1", fileName, Server.MapPath("/TempData/") + chartFileName, "100%", "600px", ChartMode.visiblox);
				model.addChart("График 2", fileName, Server.MapPath("/TempData/") + chartFileName, "100%", "600px", ChartMode.visifire);
				PILib.Debug.end("charts", report.debugId);
			}

			PILib.Debug.end("query", report.debugId);
			PILib.Debug.traceAll("query", report.debugId);
			return result;
		}

		protected string getChartFileName(string fileName) {
			return Server.MapPath("/Data/Charts/") + fileName;
		}



		public ActionResult About() {
			return View();
		}

		public ActionResult Index() {
			return View();
		}

		public ActionResult ActiveX() {
			return View();
		}

		public ActionResult ActiveXGA9() {
			return View();
		}

		public ActionResult ActiveXSchema() {
			return View();
		}

		public ActionResult ActiveXTrend() {
			return View();
		}

        public ActionResult ActiveXIT()
        {
            return View();
        }
	}
}
