using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class PsychologistController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            // Seguridad básica por rol
            if (Session["Rol"] == null || Session["Rol"].ToString() != "PSICOLOGO")
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }
}