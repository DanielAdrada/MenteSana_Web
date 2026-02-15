using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic
{
    public class ResourceLog
    {
        ResourcesDat resourcesDat = new ResourcesDat();

        // ================= INSERTAR RECURSOS EDUCATIVOS  =================
        public bool agregarRecurso(string _titulo, string _descripcion, string _tipo, string _archivo, string _url)
        {
            // Valida que no sea nulo ni espacios en blanco
            if (string.IsNullOrWhiteSpace(_titulo))
                return false;

            // Valida que al menos uno de los dos (Archivo o URL) tenga contenido
            if (string.IsNullOrWhiteSpace(_archivo) && string.IsNullOrWhiteSpace(_url))
                return false;

            _titulo = _titulo.Trim();
            _descripcion = _descripcion.Trim();

            if (_titulo.Length < 5 || _titulo.Length > 100)
                return false;

            return resourcesDat.saveResource(_titulo, _descripcion, _tipo, _archivo, _url);

        }


        // ================= LISTAR TODOS LOS RECURSOS =================
        public List<ResourcesDTO> ObtenerRecursos()
        {
            return resourcesDat.ShowResources();
        }

        // ================= ELIMINAR UN RECURSO EDUCATIVO =================
        public bool eliminarRecurso(int _id)
        {
            if (_id <= 0) return false;

            return resourcesDat.deleteResource(_id);
        }



        // ================= ACTUALIZAR UN RECURSO EDUCATIVO =================
        public bool actualizarRecurso(int _id, string _titulo, string _descripcion, string _tipo, string _archivo, string _url)
        {

            if (_id <= 0) return false;


            _titulo = _titulo.Trim();
            _descripcion = _descripcion?.Trim();


            if (_titulo.Length < 5 || _titulo.Length > 100)
                return false;


            return resourcesDat.updateResource(_id, _titulo, _descripcion, _tipo, _archivo, _url);
        }

    }
}