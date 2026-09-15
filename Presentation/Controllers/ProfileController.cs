using Logic;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Models;
using System.Diagnostics;

namespace Presentation.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProfileLog profileLogic = new ProfileLog();

        // PERFIL DEL ESTUDIANTE
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
                Grado = perfil.Grado,
                Curso = perfil.Curso,
                FechaNacimiento = perfil.FechaNacimiento,
                FotoRuta = perfil.FotoRuta
            };

            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];

            return View(model);
        }


        // PERFIL DEL PSICÓLOGO - GET
        public ActionResult Index_Psychologist()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            string id = Session["UserId"].ToString();

            PsychologistDTO perfil =
                profileLogic.GetPsychologistProfile(id);

            if (perfil == null)
            {
                return HttpNotFound("No se encontró el perfil del psicólogo.");
            }

            ProfilePsychologistViewModel model =
                new ProfilePsychologistViewModel
                {
                    Id = perfil.Id,
                    Nombre = perfil.Nombre,
                    Apellido = perfil.Apellido,
                    Correo = perfil.Correo,
                    Telefono = perfil.Telefono,
                    Formacion = perfil.Formacion,
                    Horario = perfil.Horario,
                    Estado = perfil.Estado,
                    Usuario = profileLogic.GetUsername(id),
                };

            model.FotoRuta = profileLogic.GetProfile(id)?.FotoRuta;

            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];

            return View(model);
        }


        // PERFIL DEL ESTUDIANTE - POST
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


            // Guardar datos del estudiante
            if (profileLogic.SaveProfile(model.Id, model.Nombre, model.Apellido, model.Grado, model.Curso, model.FechaNacimiento))
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

                // ✅ Guardar la nueva foto
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

            // Mensajes 
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


        // PERFIL DEL PSICOLOGO - POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index_Psychologist(
           ProfilePsychologistViewModel model)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
                return View(model);

            string id = Session["UserId"].ToString();

            bool actualizoPerfil = false;
            bool actualizoFoto = false;
            bool actualizoUsuario = false;


            // -----------------------------------------------------
            // Guardar datos profesionales del psicólogo
            // -----------------------------------------------------

            actualizoPerfil =
                profileLogic.SavePsychologistProfile(
                    id,
                    model.Nombre,
                    model.Apellido,
                    model.Correo,
                    model.Telefono,
                    model.Formacion,
                    model.Horario);

            if (!actualizoPerfil)
            {
                TempData["Error"] =
                    "No se pudo guardar el perfil del psicólogo.";

                return RedirectToAction(
                    "Index_Psychologist");
            }


            // -----------------------------------------------------
            // Actualizar nombre de usuario
            // -----------------------------------------------------

            ProfileDTO informacionUsuario =
                profileLogic.GetProfile(id);

            string usuarioActual =
                informacionUsuario?.Usuario;

            if (!string.IsNullOrWhiteSpace(model.Usuario) &&
                !model.Usuario.Equals(
                    usuarioActual,
                    StringComparison.OrdinalIgnoreCase))
            {
                actualizoUsuario =
                    profileLogic.UpdateUsername(
                        id,
                        model.Usuario);

                if (!actualizoUsuario)
                {
                    TempData["Error"] =
                        "El nombre de usuario ya existe.";

                    return RedirectToAction(
                        "Index_Psychologist");
                }

                Session["Username"] = model.Usuario;
            }


            // -----------------------------------------------------
            // Procesar foto
            // -----------------------------------------------------

            if (model.Foto != null &&
                model.Foto.ContentLength > 0)
            {
                string extension =
                    System.IO.Path
                        .GetExtension(model.Foto.FileName)
                        .ToLower();

                string[] extensionesPermitidas =
                {
                    ".jpg",
                    ".jpeg",
                    ".png"
                };

                if (!extensionesPermitidas.Contains(extension))
                {
                    TempData["Error"] =
                        "Formato de imagen no válido.";

                    return RedirectToAction(
                        "Index_Psychologist");
                }

                string rutaCarpeta =
                    Server.MapPath(
                        "~/Content/Uploads/Profiles/");

                if (!System.IO.Directory.Exists(
                    rutaCarpeta))
                {
                    System.IO.Directory.CreateDirectory(
                        rutaCarpeta);
                }

                string nombreArchivo =
                    $"profile_{id}{extension}";

                string rutaCompleta =
                    System.IO.Path.Combine(
                        rutaCarpeta,
                        nombreArchivo);

                // Guardar nueva foto
                model.Foto.SaveAs(rutaCompleta);


                // Eliminar fotos anteriores
                string[] extensiones =
                {
                    ".png",
                    ".jpg",
                    ".jpeg"
                };

                foreach (var ext in extensiones)
                {
                    if (ext == extension)
                        continue;

                    string archivoViejo =
                        System.IO.Path.Combine(
                            rutaCarpeta,
                            $"profile_{id}{ext}");

                    if (System.IO.File.Exists(
                        archivoViejo))
                    {
                        System.IO.File.Delete(
                            archivoViejo);
                    }
                }


                // Guardar ruta en BD
                string rutaBD =
                    $"/Content/Uploads/Profiles/{nombreArchivo}";

                profileLogic.SaveProfilePhoto(
                    id,
                    rutaBD);

                actualizoFoto = true;
            }

            // Mensajes finales

            if (actualizoPerfil &&
                actualizoFoto &&
                actualizoUsuario)
            {
                TempData["Mensaje"] =
                    "Perfil, usuario y foto actualizados correctamente.";
            }
            else if (actualizoPerfil &&
                     actualizoFoto)
            {
                TempData["Mensaje"] =
                    "Perfil y foto actualizados correctamente.";
            }
            else if (actualizoPerfil &&
                     actualizoUsuario)
            {
                TempData["Mensaje"] =
                    "Perfil y usuario actualizados correctamente.";
            }
            else if (actualizoFoto &&
                     actualizoUsuario)
            {
                TempData["Mensaje"] =
                    "Foto y usuario actualizados correctamente.";
            }
            else if (actualizoUsuario)
            {
                TempData["Mensaje"] =
                    "Nombre de usuario actualizado correctamente.";
            }
            else if (actualizoFoto)
            {
                TempData["Mensaje"] =
                    "Foto de perfil guardada correctamente.";
            }
            else if (actualizoPerfil)
            {
                TempData["Mensaje"] =
                    "Perfil actualizado correctamente.";
            }

            return RedirectToAction(
                "Index_Psychologist");
        }


    }
}