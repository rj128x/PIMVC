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

namespace PIMVC.Controllers
{
	[HandleError]
	public class RynokController: Controller
	{

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult IndexMonth() {
			RynokModel model=new RynokModel();
			model.Month = DateTime.Now.Month;
			return View("IndexMonth", model);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult IndexMonth(RynokModel model) {
			model.processDataMonth();
			return View("IndexMonth", model);
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult IndexKvartal() {
			RynokKvartalModel model=new RynokKvartalModel();					
			return View("IndexKvartal", model);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult IndexKvartal(RynokKvartalModel model) {
			model.processData();
			return View("IndexKvartal", model);
		}


		
	}
}
