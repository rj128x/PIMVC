using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PILib;

namespace PIMVC
{
	// Примечание: Инструкции по включению классического режима IIS6 или IIS7 
	// см. по ссылке http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			
			routes.MapRoute(
				"Default", // Имя маршрута
				"{controller}/{action}/{id}", // URL-адрес с параметрами
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Параметры по умолчанию
			);

		}

		protected void Application_Start() {
			PILib.PIReport.PIServerInfo.defPath = Server.MapPath("/");
			PILib.PIReport.Report.defPath = Server.MapPath("/");
			PILib.PIReport.Report.defPathTempCharts = Server.MapPath("/TempData/Charts");
			Logger.init(Server.MapPath("/logs/"), "pimvc");			

			PILib.PIReport.PIServerInfo.initServers();

			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);			
		}

		
	}
}