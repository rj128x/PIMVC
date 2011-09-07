using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMVC.Helpers
{
	using System.Web;

	public static class AppHelper
	{
		public static string getPDIPath(HttpRequest request, string pdi){
			string server = request.Url.Host + (request.Url.Port == 80 ? "" : ":" + request.Url.Port.ToString());
			string path = VirtualPathUtility.ToAbsolute("~/PDI");
			return String.Format("http://{0}{1}/{2}", server, path, pdi);
		}
		

	}
}