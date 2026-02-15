using Logic;
using Data.Models;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class CommunityController : Controller
    {
        CommentLog commentLog = new CommentLog();

        // GET: Community
        public ActionResult Index()
        {
            // Validar sesión
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            // 1. Traer DTOs desde lógica
            List<CommentDTO> comentariosDTO = commentLog.ObtenerComentarios();

            // 2. Mapear DTO → ViewModel
            List<CommentViewModel> comentariosVM = comentariosDTO.Select(c => new CommentViewModel
            {
                UserId = c.Id,
                Usuario = c.Usuario,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                FotoPerfil = c.FotoRuta,
                Contenido = c.Contenido,
                Fecha = c.Fecha
            }).ToList();

            // 3. Enviar ViewModel a la vista
            return View(comentariosVM);
        }

        // POST: Community/Create
        [HttpPost]
        public ActionResult Create(string contenido)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");

            string usuId = Session["UserId"].ToString();

            commentLog.AgregarComentario(usuId, contenido);

            return RedirectToAction("Index");
        }
    }
}