using Logic;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class PsychologistController : Controller
    {
        private readonly DassLogic dassLogic = new DassLogic();

        [HttpGet]
        public ActionResult Index()
        {
            // Seguridad básica por rol
            if (Session["Rol"] == null || Session["Rol"].ToString() != "PSICOLOGO")
            {
                return RedirectToAction("Index", "Login");
            }

            // Obtener datos reales del DASS para el dashboard
            DashboardViewModel model = new DashboardViewModel();

            model.EstudiantesEvaluados =
                dassLogic.ObtenerEstudiantesEvaluados();

            model.EvaluacionesRealizadas =
                dassLogic.ObtenerEvaluacionesRealizadas();

            model.RequierenSeguimiento =
                dassLogic.ObtenerEstudiantesSeguimiento();

            model.UltimaEvaluacion =
                dassLogic.ObtenerUltimaEvaluacion();

            return View(model);
        }
    }
}