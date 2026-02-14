using Logic;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class RegisterController : Controller
    {
        UserLog userLogic = new UserLog();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool registrado = userLogic.RegistrarUsuario(
    model.Identificacion, // id
    model.Usuario,        // usuario
    model.Identificacion, // identificacion (contraseña)
    model.Nombre,
    model.Apellido
);


                if (registrado)
                {
                    TempData["RegSuccess"] = "Cuenta creada exitosamente. Ya puedes iniciar sesión.";
                    return RedirectToAction("Index", "Login");
                }

                TempData["RegError"] = "No se pudo registrar el usuario. Verifica que no exista previamente.";
                TempData["OpenRegisterModal"] = true;
                return RedirectToAction("Index", "Login");
            }

            TempData["RegError"] = "Datos inválidos. Verifica el formulario.";
            TempData["OpenRegisterModal"] = true;
            return RedirectToAction("Index", "Login");


        }
    }
}