using Logic;
using Presentation.Models;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class LoginController : Controller
    {
        private UserLog userLogic = new UserLog();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.RegSuccess = TempData["RegSuccess"];
            ViewBag.RegError = TempData["RegError"];

            ViewBag.Error = TempData["LoginError"];
            ViewBag.Success = TempData["LoginSuccess"];

            return View();
        }

        // ✅ PROCESA el login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LoginError = "Por favor completa todos los campos.";
                ViewBag.OpenLoginModal = true;
                return View("Index");
            }

            var resultado = userLogic.IniciarSesion(model.Usuario, model.Identificacion, model.Rol);

            if (!resultado.Exitoso)
            {
                ViewBag.LoginError = resultado.Mensaje;
                ViewBag.OpenLoginModal = true;
                return View("Index");
            }

            //  LOGIN EXITOSO 
            Session["UserId"] = resultado.Sesion.Id;
            Session["Usuario"] = resultado.Sesion.Usuario;
            Session["Rol"] = resultado.Sesion.Rol;

            if (resultado.Sesion.Rol.Equals("ESTUDIANTE", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Students");
            }
            else if (resultado.Sesion.Rol.Equals("PSICOLOGO", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Psychologist");
            }

            return RedirectToAction("Index", "Login");

        }
        // Acción para cerrar sesión
        [HttpGet] 
        public ActionResult Logout()
        {
            // Limpiar toda la sesión
            Session.Clear();
            Session.Abandon();

            // Redirigir al login
            return RedirectToAction("Index", "Login");
        }
    }
}