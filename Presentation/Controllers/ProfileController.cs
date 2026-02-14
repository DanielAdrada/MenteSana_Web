using Logic;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Models;

namespace Presentation.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProfileLog profileLogic = new ProfileLog();

        public ActionResult Index()
        {
            if(Session["UserId"] == null)
        return RedirectToAction("Index", "Login");

            string id = Session["UserId"].ToString();
            ProfileDTO perfil = profileLogic.GetProfile(id);

            if (perfil == null)
            {
                perfil = new ProfileDTO
                {
                    Id = id,
                    Nombre = "",
                    Apellido = ""
                };
            }

            ProfileViewModel model = new ProfileViewModel
            {
                Id = perfil.Id,
                Usuario = perfil.Usuario,
                Nombre = perfil.Nombre,
                Apellido = perfil.Apellido,
                FotoRuta = perfil.FotoRuta
            };

            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ProfileViewModel model)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
                return View(model);

            bool actualizoPerfil = false;
            bool actualizoFoto = false;
            bool actualizoUsuario = false;


            // Guardar nombre y apellido
            if (profileLogic.SaveProfile(model.Id, model.Nombre, model.Apellido))
            {
                actualizoPerfil = true;
            }
            else
            {
                TempData["Error"] = "No se pudo guardar el perfil";
                return RedirectToAction("Index");
            }

            // Actualizar usuario
            string usuarioActual = profileLogic.GetProfile(model.Id)?.Usuario;

            if (!string.IsNullOrWhiteSpace(model.Usuario) &&
                !model.Usuario.Equals(usuarioActual, StringComparison.OrdinalIgnoreCase))
            {
                actualizoUsuario = profileLogic.UpdateUsername(model.Id, model.Usuario);

                if (!actualizoUsuario)
                {
                    TempData["Error"] = "El nombre de usuario ya existe.";
                    return RedirectToAction("Index");
                }

                Session["Username"] = model.Usuario;
            }

            // Procesar foto
            if (model.Foto != null && model.Foto.ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(model.Foto.FileName).ToLower();
                string[] extensionesPermitidas = { ".jpg", ".jpeg", ".png" };

                if (!extensionesPermitidas.Contains(extension))
                {
                    TempData["Error"] = "Formato de imagen no válido";
                    return RedirectToAction("Index");
                }

                string rutaCarpeta = Server.MapPath("~/Content/Uploads/Profiles/");

                if (!System.IO.Directory.Exists(rutaCarpeta))
                    System.IO.Directory.CreateDirectory(rutaCarpeta);

                string nombreArchivo = $"profile_{model.Id}{extension}";
                string rutaCompleta = System.IO.Path.Combine(rutaCarpeta, nombreArchivo);

                // ✅ Guardar primero la nueva foto
                model.Foto.SaveAs(rutaCompleta);

                // 🔥 Luego borrar fotos anteriores (excepto la nueva)
                string[] extensiones = { ".png", ".jpg", ".jpeg" };
                foreach (var ext in extensiones)
                {
                    if (ext == extension) continue; // no borrar la nueva

                    string archivoViejo = System.IO.Path.Combine(rutaCarpeta, $"profile_{model.Id}{ext}");
                    if (System.IO.File.Exists(archivoViejo))
                    {
                        System.IO.File.Delete(archivoViejo);
                    }
                }

                string rutaBD = $"/Content/Uploads/Profiles/{nombreArchivo}";
                profileLogic.SaveProfilePhoto(model.Id, rutaBD);

                actualizoFoto = true;
            }

            // Mensajes claros
            if (actualizoPerfil && actualizoFoto && actualizoUsuario)
                TempData["Mensaje"] = "Perfil, usuario y foto actualizados correctamente.";
            else if (actualizoUsuario)
                TempData["Mensaje"] = "Nombre de usuario actualizado correctamente.";
            else if (actualizoFoto)
                TempData["Mensaje"] = "Foto de perfil guardada correctamente.";
            else if (actualizoPerfil)
                TempData["Mensaje"] = "Perfil actualizado correctamente.";

            return RedirectToAction("Index");
        }
    }
}