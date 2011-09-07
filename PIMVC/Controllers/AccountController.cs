using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PIMVC.Controllers
{
	public class AccountController : Controller
	{
		//
		// GET: /Account/

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult LogOn() {
			return View();
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult LogOut() {
			string returnUrl = Url.Action("Index", "Home");
			FormsAuthentication.SignOut();
			return Redirect(returnUrl);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult LogOn(string name, string password, string returnUrl) {
			if (FormsAuthentication.Authenticate(name, password)) {
				returnUrl = returnUrl ?? Url.Action("Index", "Home");
				FormsAuthentication.SetAuthCookie(name, false);				
				return Redirect(returnUrl);
			} else {
				ViewData["lastLoginFailed"] = true;
				return View();
			}
		}

	}
}
