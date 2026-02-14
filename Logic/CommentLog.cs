using Data.Models;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic
{
    public class CommentLog
    {
        CommentDat commentDat = new CommentDat();

        // ================= INSERTAR COMENTARIO =================
        public bool AgregarComentario(string usuId, string contenido)
        {
            // 1. Validar sesión
            if (string.IsNullOrEmpty(usuId))
                return false;

            // 2. Validar contenido
            if (string.IsNullOrWhiteSpace(contenido))
                return false;

            contenido = contenido.Trim();

            // 3. Validar longitud
            if (contenido.Length < 5 || contenido.Length > 300)
                return false;

            // 4. Por defecto: comentario positivo
            bool positivo = true;

            // 5. Llamar a Data
            return commentDat.InsertComentario(usuId, contenido, positivo);
        }

        // ================= LISTAR TODOS =================
        public List<CommentDTO> ObtenerComentarios()
        {
            return commentDat.GetComentarios();
        }

        // ================= LISTAR POSITIVOS =================
        public List<CommentDTO> ObtenerComentariosPositivos()
        {
            return commentDat.GetComentariosPositivos();
        }

        // ================= DESACTIVAR (PSICÓLOGO) =================
        public bool DesactivarComentario(int comentarioId, string rol)
        {
            // Solo psicólogo puede desactivar
            if (rol != "PSICOLOGO")
                return false;

            if (comentarioId <= 0)
                return false;

            return commentDat.DesactivarComentario(comentarioId);
        }
    }
}