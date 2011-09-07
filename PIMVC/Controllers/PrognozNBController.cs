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
	public class PrognozNBController: Controller
	{
		
		public ActionResult Index() {
			//ChartProperties.createNullXML("d:/prop.xml");
			return View(new PrognozNBModel());
		}

		
	}
}
