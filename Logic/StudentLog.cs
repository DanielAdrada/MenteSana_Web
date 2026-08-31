using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class StudentLog
    {
        private readonly StudentDat studentDat = new StudentDat();

        // ================= OBTENER POR ID =================
        public StudentDTO GetStudentById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return studentDat.GetStudentById(id);
        }

        // ================= LISTAR =================
        public List<StudentDTO> ListStudents()
        {
            return studentDat.ListStudents();
        }

        // ================= ACTUALIZAR =================
        public bool UpdateStudent(
            string id,
            string nombre,
            string apellido)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            if (string.IsNullOrWhiteSpace(apellido))
                return false;

            return studentDat.UpdateStudent(
                id.Trim(),
                nombre.Trim(),
                apellido.Trim());
        }

        // ================= CAMBIAR ESTADO =================
        public bool UpdateStudentStatus(string id, string estado)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            if (estado != "ACTIVO" && estado != "INACTIVO")
                return false;

            return studentDat.ChangeStudentStatus(id, estado);
        }

        // ================= ELIMINAR =================
        public bool DeleteStudent(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            if (!studentDat.ExistsStudent(id))
                return false;

            return studentDat.DeleteStudent(id);
        }

        public int CountStudents()
        {
            return studentDat.CountStudents();
        }
    }
}
