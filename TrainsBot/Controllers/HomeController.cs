using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainsBot.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("hello")]
        public ActionResult Hello()
        {
            return View("Index");
        }

        [Route("first")]
        public ActionResult First()
        {
            return View();
        }

        [Route("second")]
        public ActionResult Second()
        {
            return View();
        }

        [Route("configure")]
        public ActionResult Configure()
        {
            return View();
        }
        [Route("simple")]
        public ActionResult Simple()
        {
            return View();
        }
        [Route("simplestart")]
        public ActionResult SimpleStart()
        {
            return View();
        }
        [Route("simple_end")]
        public ActionResult SimpleEnd()
        {
            return View();
        }
    }
}