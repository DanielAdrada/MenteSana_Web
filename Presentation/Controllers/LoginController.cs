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

            var resultado = userLogic.IniciarSesion(model.Usuario, model.Password, model.Rol);

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

            else if (resultado.Sesion.Rol.Equals("ADMINISTRADOR", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Administrator");
            }

            return RedirectToAction("Index", "Login");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SolicitarRecuperacion(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                ViewBag.RecoveryError =
                    "Por favor ingresa tu correo electrónico.";

                ViewBag.OpenRecoveryModal = true;

                return View("Index");
            }

            string token;

            bool solicitado = userLogic.SolicitarRecuperacion(
                correo,
                out token
            );

            if (!solicitado)
            {
                ViewBag.RecoveryError =
                    "No se encontró una cuenta asociada a ese correo electrónico.";

                ViewBag.OpenRecoveryModal = true;

                return View("Index");
            }

            string enlaceRecuperacion = Url.Action(
                "RecuperarContrasena",
                "Login",
                new { token = token },
                protocol: Request.Url.Scheme
            );

            EmailService emailService = new EmailService();

            bool correoEnviado =
                emailService.EnviarCorreoRecuperacion(
                    correo,
                    enlaceRecuperacion
                );

            if (!correoEnviado)
            {
                ViewBag.RecoveryError =
                    "La solicitud fue registrada, pero no se pudo enviar el correo de recuperación.";

                ViewBag.OpenRecoveryModal = true;

                return View("Index");
            }

            ViewBag.RecoverySuccess =
                "Se ha enviado un enlace de recuperación a tu correo electrónico.";

            ViewBag.OpenRecoveryModal = true;

            return View("Index");
        }
        // ================= RECUPERACIÓN DE CONTRASEÑA =================

        [HttpGet]
        public ActionResult RecuperarContrasena(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ViewBag.Error =
                    "El enlace de recuperación no es válido.";

                return View();
            }

            var recuperacion =
                userLogic.ValidarTokenRecuperacion(token);

            if (recuperacion == null)
            {
                ViewBag.Error =
                    "El enlace de recuperación no es válido o ha expirado.";

                return View();
            }

            ViewBag.Token = token;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarContrasena(
            string token,
            string nuevaContrasena,
            string confirmarContrasena)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ViewBag.Error =
                    "El enlace de recuperación no es válido.";

                return View();
            }

            if (string.IsNullOrWhiteSpace(nuevaContrasena) ||
                string.IsNullOrWhiteSpace(confirmarContrasena))
            {
                ViewBag.Error =
                    "Debes completar todos los campos.";

                ViewBag.Token = token;

                return View();
            }

            if (nuevaContrasena.Length < 8)
            {
                ViewBag.Error =
                    "La contraseña debe tener mínimo 8 caracteres.";

                ViewBag.Token = token;

                return View();
            }

            if (nuevaContrasena != confirmarContrasena)
            {
                ViewBag.Error =
                    "Las contraseñas no coinciden.";

                ViewBag.Token = token;

                return View();
            }

            bool cambiada =
                userLogic.CambiarContrasena(
                    token,
                    nuevaContrasena
                );

            if (!cambiada)
            {
                ViewBag.Error =
                    "No fue posible cambiar la contraseña. " +
                    "El enlace puede haber expirado o ya fue utilizado.";

                return View();
            }

            ViewBag.Success =
                "Tu contraseña ha sido cambiada correctamente.";

            return View();
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