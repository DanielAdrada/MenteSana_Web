using Logic;
using Data;
using Data.Models;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class StudentsController : Controller
    {
        public ActionResult Index()
        {
            if(Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            string id = Session["UserId"].ToString();

            ProfileLog profileLogic = new ProfileLog();
            CommentLog commentLog = new CommentLog();
            ResourceLog resourcesLog = new ResourceLog();
            PsychologistDat psychologistDat = new PsychologistDat();

            var model = new StudentHomeViewModel
            {
                Perfil = profileLogic.GetProfile(id),
                Comentarios = commentLog.ObtenerComentariosPositivos(),
                Recursos = resourcesLog.ObtenerRecursos()
                           .OrderByDescending(r => r.Fecha)
                           .Take(3)
                           .ToList(),
                Psicologos = psychologistDat.ListPsychologists()

            };


            return View(model);
        }
    }
}