using Data.Models;
using Logic;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Presentation.Controllers
{
    public class ResourcesController : Controller
    {

        ResourceLog resourcesLog = new ResourceLog();
        public ActionResult Index()
        {

            List<ResourcesDTO> listaDesdeBD = resourcesLog.ObtenerRecursos();
          
            ResourcesViewModel modeloParaVista = new ResourcesViewModel();

            modeloParaVista.ListaRecursos = listaDesdeBD;

            return View(modeloParaVista);
        }

        [HttpPost]
        public ActionResult Create(ResourcesViewModel model)
        {

            if (model.Archivo == null && string.IsNullOrWhiteSpace(model.Url))
            {
                ModelState.AddModelError("", "Debe adjuntar un archivo o ingresar una URL externa.");
            }

            if (!ModelState.IsValid)
            {
                model.ListaRecursos = resourcesLog.ObtenerRecursos();
                return View("Index", model);
            }

            string nombreArchivoParaBD = model.ArchivoActual;

            if (model.Archivo != null && model.Archivo.ContentLength > 0)
            {
                nombreArchivoParaBD = System.IO.Path.GetFileName(model.Archivo.FileName);
                string rutaFisica = Server.MapPath("~/Uploads/" + nombreArchivoParaBD);
                model.Archivo.SaveAs(rutaFisica);
            }

            bool resultado;

            // ================= CREATE =================
            if (model.Id == 0)
            {
                // Obtener el ID del psicólogo que tiene la sesión iniciada
                string psiId = Session["UserId"]?.ToString();

                if (string.IsNullOrWhiteSpace(psiId))
                {
                    ModelState.AddModelError(
                        "",
                        "No se pudo identificar al psicólogo que publica el recurso."
                    );

                    model.ListaRecursos = resourcesLog.ObtenerRecursos();
                    return View("Index", model);
                }

                resultado = resourcesLog.agregarRecurso(
                    model.Titulo,
                    model.Descripcion,
                    model.Tipo,
                    nombreArchivoParaBD,
                    model.Url,
                    psiId
                );

                TempData["Mensaje"] = "Recurso creado correctamente.";
            }
            else
            {
                // ================= UPDATE =================
                resultado = resourcesLog.actualizarRecurso(
                    model.Id,
                    model.Titulo,
                    model.Descripcion,
                    model.Tipo,
                    nombreArchivoParaBD,
                    model.Url
                );

                TempData["Mensaje"] = "Recurso actualizado correctamente.";
            }

            if (!resultado)
            {
                ModelState.AddModelError("", "No se pudo guardar el recurso.");
                model.ListaRecursos = resourcesLog.ObtenerRecursos();
                return View("Index", model);
            }

            return RedirectToAction("Index");

        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            bool eliminado = resourcesLog.eliminarRecurso(id);

            if (eliminado)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "No se pudo eliminar el recurso.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult SelectResources()
        {
            List<ResourcesDTO> listaDesdeBD = resourcesLog.ObtenerRecursos();

            ResourcesViewModel modeloParaVista = new ResourcesViewModel();

            modeloParaVista.ListaRecursos = listaDesdeBD;

            return View(modeloParaVista);
        }

    }
}