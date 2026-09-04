using Logic;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly StudentLog studentLog = new StudentLog();
        private readonly UserLog userLog = new UserLog();
        private readonly PsychologistLog psychologistLog = new PsychologistLog();
        private readonly ResourceLog resourceLog = new ResourceLog();
        private readonly CommentLog commentLog = new CommentLog();

        // ================= DASHBOARD =================
        public ActionResult Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalPsicologos = psychologistLog.CountPsychologists(),
                TotalEstudiantes = studentLog.CountStudents(),
                TotalRecursos = resourceLog.CountResources(),
                TotalComentarios = commentLog.CountComments()
            };

            return View(model);
        }

        // ================= VISTA GESTIÓN PSICÓLOGOS =================
        [HttpGet]
        public ActionResult Psychologists(string search)
        {
            var model = LoadPsychologists(search);

            // Siempre que se entra a esta acción GET
            // se muestra el formulario de REGISTRO.
            model.IsEditing = false;

            return View(model);
        }
        private PsychologistViewModel LoadPsychologists(string search = "")
        {
            var model = new PsychologistViewModel();

            var psychologists = psychologistLog.ListPsychologists();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                psychologists = psychologists
                    .Where(p =>
                        p.Id.ToLower().Contains(search) ||
                        p.Nombre.ToLower().Contains(search) ||
                        p.Apellido.ToLower().Contains(search) ||
                        p.Correo.ToLower().Contains(search))
                    .ToList();
            }

            foreach (var p in psychologists)
            {
                model.Psychologists.Add(new PsychologistViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Correo = p.Correo,
                    Telefono = p.Telefono,
                    Formacion = p.Formacion,
                    Horario = p.Horario,
                    Estado = p.Estado
                });
            }

            return model;

        }
        public ActionResult EditPsychologist(string id)
        {
            var dto = psychologistLog.GetPsychologistById(id);

            if (dto == null)
            {
                TempData["Error"] = "No se encontró el psicólogo.";
                return RedirectToAction("Psychologists");
            }

            var model = LoadPsychologists();

            model.Id = dto.Id;
            model.Nombre = dto.Nombre;
            model.Apellido = dto.Apellido;
            model.Correo = dto.Correo;
            model.Telefono = dto.Telefono;
            model.Formacion = dto.Formacion;
            model.Horario = dto.Horario;
            model.Estado = dto.Estado;

            model.IsEditing = true;

            return View("Psychologists", model);
        }
        public ActionResult ChangePsychologistStatus(string id, string estado)
        {
            bool actualizado =
                psychologistLog.UpdatePsychologistStatus(id, estado);

            if (actualizado)
            {
                TempData["Success"] =
                    $"Estado actualizado a {estado}.";
            }
            else
            {
                TempData["Error"] =
                    "No fue posible actualizar el estado.";
            }

            return RedirectToAction("Psychologists");
        }
        // ================= VISTA GESTIÓN ESTUDIANTES =================
        [HttpGet]
        public ActionResult Students(string search)
        {
            return View(LoadStudents(search));
        }

        private StudentViewModel LoadStudents(string search = "")
        {
            var model = new StudentViewModel();

            var students = studentLog.ListStudents();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                students = students
                    .Where(s =>
                        s.Id.ToLower().Contains(search) ||
                        s.Nombre.ToLower().Contains(search) ||
                        s.Apellido.ToLower().Contains(search) ||
                        s.Usuario.ToLower().Contains(search))
                    .ToList();
            }

            foreach (var s in students)
            {
                model.Students.Add(new StudentViewModel
                {
                    Id = s.Id,
                    Usuario = s.Usuario,
                    Nombre = s.Nombre,
                    Apellido = s.Apellido,
                    Estado = s.Estado
                });
            }

            return model;
        }
        public ActionResult EditStudent(string id)
        {
            var dto = studentLog.GetStudentById(id);

            if (dto == null)
            {
                TempData["Error"] = "No se encontró el estudiante.";
                return RedirectToAction("Students");
            }

            var model = LoadStudents();

            model.Id = dto.Id;
            model.Usuario = dto.Usuario;
            model.Nombre = dto.Nombre;
            model.Apellido = dto.Apellido;
            model.Estado = dto.Estado;

            model.IsEditing = true;

            return View("Students", model);
        }

        public ActionResult ChangeStudentStatus(string id, string estado)
        {
            bool actualizado =
                studentLog.UpdateStudentStatus(id, estado);

            if (actualizado)
            {
                TempData["Success"] =
                    $"Estado actualizado a {estado}.";
            }
            else
            {
                TempData["Error"] =
                    "No fue posible actualizar el estado.";
            }

            return RedirectToAction("Students");
        }
        // ================= REGISTRAR PSICÓLOGO =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Psychologists(PsychologistViewModel model)
        {
            if (model.IsEditing)
            {
                ModelState.Remove("Usuario");
                ModelState.Remove("Password");
            }

            if (model.IsEditing)
            {
                bool actualizado =
                    psychologistLog.UpdatePsychologist(
                        model.Id,
                        model.Nombre,
                        model.Apellido,
                        model.Correo,
                        model.Telefono,
                        model.Formacion,
                        model.Horario
                    );

                if (actualizado)
                {
                    TempData["Success"] =
                        "Psicólogo actualizado correctamente.";
                }
                else
                {
                    TempData["Error"] =
                        "No fue posible actualizar el psicólogo.";
                }

                return RedirectToAction("Psychologists");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Por favor corrige los errores del formulario.";

                // Si estamos registrando, aseguramos que el formulario
                // siga comportándose como formulario de registro.
                if (!model.IsEditing)
                {
                    model.Psychologists = psychologistLog.ListPsychologists()
                        .Select(p => new PsychologistViewModel
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Apellido = p.Apellido,
                            Correo = p.Correo,
                            Telefono = p.Telefono,
                            Formacion = p.Formacion,
                            Horario = p.Horario,
                            Estado = p.Estado
                        })
                        .ToList();
                }

                return View(model);
            }

            // 1. Crear usuario
            bool usuarioCreado = userLog.RegisterUser(
                model.Id,
                model.Usuario,
                model.Password,
                "PSICOLOGO"
            );

            if (!usuarioCreado)
            {
                ViewBag.Error = "No fue posible crear el usuario. Verifique si la identificación o el nombre de usuario ya existen.";
                return View(model);
            }

            // 2. Crear psicólogo
            bool psicologoCreado = psychologistLog.InsertPsychologist(
                model.Id,
                model.Nombre,
                model.Apellido,
                model.Correo,
                model.Telefono,
                model.Formacion,
                model.Horario
            );

            if (!psicologoCreado)
            {
                ViewBag.Error = "El usuario fue creado, pero ocurrió un error al registrar el psicólogo.";
                return View(model);
            }

            ViewBag.Success = "¡Psicólogo registrado correctamente!";

            var viewModel = new PsychologistViewModel();

            var psychologists = psychologistLog.ListPsychologists();

            foreach (var p in psychologists)
            {
                viewModel.Psychologists.Add(new PsychologistViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Correo = p.Correo,
                    Telefono = p.Telefono,
                    Formacion = p.Formacion,
                    Horario = p.Horario
                });
            }

            ModelState.Clear();

            return View(viewModel);
        }
        public ActionResult Resources()
        {
            var recursos = resourceLog.ObtenerRecursos();

            var model = new ResourcesViewModel
            {
                ListaRecursos = recursos
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Students(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(LoadStudents());
            }

            if (model.IsEditing)
            {
                bool actualizado = studentLog.UpdateStudent(
                    model.Id,
                    model.Nombre,
                    model.Apellido);

                if (actualizado)
                    TempData["Success"] = "Estudiante actualizado correctamente.";
                else
                    TempData["Error"] = "No fue posible actualizar el estudiante.";

                return RedirectToAction("Students");
            }

            return RedirectToAction("Students");
        }

        public ActionResult DeleteStudent(string id)
        {
            bool eliminado = studentLog.DeleteStudent(id);

            if (eliminado)
                TempData["Success"] = "Estudiante eliminado correctamente.";
            else
                TempData["Error"] = "No fue posible eliminar el estudiante.";

            return RedirectToAction("Students");
        }
    }
}
