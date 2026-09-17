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
        private readonly NotificacionLog notificacionLogic = new NotificacionLog();

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

            model.Notificaciones =    notificacionLogic.ObtenerNotificaciones();

            model.NotificacionesPendientes =  notificacionLogic.ContarNotificacionesPendientes();

            // Obtener estadísticas de Depresión, Ansiedad y Estrés
            model.EstadisticasDASS =
                dassLogic.ObtenerEstadisticasDashboard();

            return View(model);
        }



        [HttpPost]
        public ActionResult MarcarNotificacionAtendida(int id)
        {
            // Seguridad básica por rol
            if (Session["Rol"] == null || Session["Rol"].ToString() != "PSICOLOGO")
            {
                return RedirectToAction("Index", "Login");
            }

            if (id <= 0)
            {
                return RedirectToAction("Index");
            }

            notificacionLogic.MarcarComoAtendida(id);

            return RedirectToAction("Index");
        }


        public ActionResult ResultadosDASS()
        {
            // Seguridad básica por rol
            if (Session["Rol"] == null || Session["Rol"].ToString() != "PSICOLOGO")
            {
                return RedirectToAction("Index", "Login");
            }

            var resultados = dassLogic.ObtenerUltimosTests();

            return View(resultados);
        }

        public ActionResult HistorialDASS(string id)
        {
            // Seguridad básica por rol
            if (Session["Rol"] == null || Session["Rol"].ToString() != "PSICOLOGO")
            {
                return RedirectToAction("Index", "Login");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("ResultadosDASS");
            }

            var historial = dassLogic.ObtenerHistorial(id);

            return View(historial);
        }

        public ActionResult DetalleDASS(int id)
        {
            // Seguridad básica por rol
            if (Session["Rol"] == null || Session["Rol"].ToString() != "PSICOLOGO")
            {
                return RedirectToAction("Index", "Login");
            }

            if (id <= 0)
            {
                return RedirectToAction("ResultadosDASS");
            }

            var test = dassLogic.ObtenerTestPorId(id);

            if (test == null)
            {
                return RedirectToAction("ResultadosDASS");
            }

            var respuestas = dassLogic.ObtenerRespuestas(id);

            var model = new DassDetailViewModel
            {
                Test = test,
                Respuestas = respuestas
            };

            return View(model);
        }
    }
}