using Logic;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class StrategiesController : Controller
    {
        private StrategiesLogic strategiesLogic = new StrategiesLogic();


        // Muestra todas las estrategias
        public ActionResult Index()
        {
            try
            {
                var datos = strategiesLogic.ObtenerEstrategias();

                List<StrategiesViewModel> estrategias =
                    new List<StrategiesViewModel>();

                foreach (var item in datos)
                {
                    estrategias.Add(ConvertirEstrategia(item));
                }

                return View(estrategias);
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "Ocurrió un error al obtener las estrategias: " +
                    ex.Message;

                return View(new List<StrategiesViewModel>());
            }
        }


        // Guarda una nueva estrategia
        [HttpPost]
        public ActionResult Crear(
            string dimension,
            string area,
            string nivel,
            string titulo,
            string descripcion)
        {
            try
            {
                string usuId = Session["UserId"]?.ToString();

                if (string.IsNullOrWhiteSpace(usuId))
                {
                    TempData["Error"] =
                        "No se pudo identificar al usuario.";

                    return RedirectToAction("Index");
                }

                int resultado = strategiesLogic.GuardarEstrategia(
                    dimension,
                    area,
                    nivel,
                    titulo,
                    descripcion,
                    usuId
                );

                if (resultado > 0)
                {
                    TempData["Mensaje"] =
                        "La estrategia fue agregada correctamente.";
                }
                else
                {
                    TempData["Error"] =
                        "No se pudo agregar la estrategia.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "Ocurrió un error al agregar la estrategia: " +
                    ex.Message;

                return RedirectToAction("Index");
            }
        }



        // Actualiza una estrategia
        [HttpPost]
        public ActionResult Editar(
            int estrategiaId,
            string dimension,
            string area,
            string nivel,
            string titulo,
            string descripcion)
        {
            try
            {
                bool actualizado =
                    strategiesLogic.ActualizarEstrategia(
                        estrategiaId,
                        dimension,
                        area,
                        nivel,
                        titulo,
                        descripcion
                    );

                if (actualizado)
                {
                    TempData["Mensaje"] =
                        "La estrategia fue actualizada correctamente.";
                }
                else
                {
                    TempData["Error"] =
                        "No se pudo actualizar la estrategia.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "Ocurrió un error al actualizar la estrategia: " +
                    ex.Message;

                return RedirectToAction("Index");
            }
        }


        // Activa o desactiva una estrategia
        [HttpPost]
        public ActionResult CambiarEstado(
            int estrategiaId,
            int activa)
        {
            try
            {
                bool cambiado =
                    strategiesLogic.CambiarEstadoEstrategia(
                        estrategiaId,
                        activa
                    );

                if (cambiado)
                {
                    TempData["Mensaje"] =
                        activa == 1
                        ? "La estrategia fue activada correctamente."
                        : "La estrategia fue desactivada correctamente.";
                }
                else
                {
                    TempData["Error"] =
                        "No se pudo cambiar el estado de la estrategia.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "Ocurrió un error al cambiar el estado: " +
                    ex.Message;

                return RedirectToAction("Index");
            }
        }


        // Convierte los datos de la base de datos
        // al ViewModel utilizado por las vistas
        private StrategiesViewModel ConvertirEstrategia(
            Dictionary<string, object> item)
        {
            StrategiesViewModel modelo =
                new StrategiesViewModel();

            if (item.ContainsKey("estrategia_id") &&
                item["estrategia_id"] != null)
            {
                modelo.EstrategiaId =
                    Convert.ToInt32(item["estrategia_id"]);
            }

            if (item.ContainsKey("estrategia_dimension"))
            {
                modelo.Dimension =
                    item["estrategia_dimension"]?.ToString();
            }

            if (item.ContainsKey("estrategia_area"))
            {
                modelo.Area =
                    item["estrategia_area"]?.ToString();
            }

            if (item.ContainsKey("estrategia_nivel"))
            {
                modelo.Nivel =
                    item["estrategia_nivel"]?.ToString();
            }

            if (item.ContainsKey("estrategia_titulo"))
            {
                modelo.Titulo =
                    item["estrategia_titulo"]?.ToString();
            }

            if (item.ContainsKey("estrategia_descripcion"))
            {
                modelo.Descripcion =
                    item["estrategia_descripcion"]?.ToString();
            }

            if (item.ContainsKey("estrategia_activa") &&
                item["estrategia_activa"] != null)
            {
                modelo.Activa =
                    Convert.ToBoolean(
                        Convert.ToInt32(item["estrategia_activa"])
                    );
            }

            if (item.ContainsKey("estrategia_usu_id"))
            {
                modelo.UsuId =
                    item["estrategia_usu_id"]?.ToString();
            }

            if (item.ContainsKey("estrategia_fecha_creacion") &&
                item["estrategia_fecha_creacion"] != null)
            {
                modelo.FechaCreacion =
                    Convert.ToDateTime(
                        item["estrategia_fecha_creacion"]
                    );
            }

            if (item.ContainsKey("estrategia_fecha_actualizacion") &&
                item["estrategia_fecha_actualizacion"] != null)
            {
                modelo.FechaActualizacion =
                    Convert.ToDateTime(
                        item["estrategia_fecha_actualizacion"]
                    );
            }

            return modelo;
        }



    }
}