using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Logic.Services
{
    public class EstrategiaService
    {
        public List<Estrategia> ObtenerEstrategias(EmotionResult resultado)
        {
            var estrategias = new List<Estrategia>();

            if (resultado == null)
                return estrategias;

            if (resultado.areas_prioritarias == null ||
                resultado.areas_prioritarias.Count == 0)
            {
                return estrategias;
            }

            string dimension = resultado.dimension_prioritaria?.ToLower();

            foreach (var area in resultado.areas_prioritarias)
            {
                var estrategiasArea = ObtenerEstrategiasPorArea(
                    dimension,
                    area.area,
                    area.puntaje
                );

                estrategias.AddRange(estrategiasArea);
            }

            // Evita estrategias repetidas
            return estrategias
                .GroupBy(e => e.Titulo)
                .Select(g => g.First())
                .Take(5)
                .ToList();
        }

        private List<Estrategia> ObtenerEstrategiasPorArea(
            string dimension,
            string area,
            double puntaje)
        {
            var estrategias = new List<Estrategia>();

            // ==========================================
            // DEPRESIÓN
            // ==========================================

            if (dimension == "depresion")
            {
                switch (area)
                {
                    case "estado_animo":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Activación emocional positiva",
                            Descripcion = "Dedica unos minutos a realizar una actividad que normalmente disfrutes, aunque inicialmente no tengas muchas ganas.",
                            Area = "Estado de ánimo"
                        });
                        break;

                    case "interes_disfrute":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Retomar actividades agradables",
                            Descripcion = "Elige una actividad sencilla que antes disfrutabas y reserva un pequeño espacio del día para realizarla.",
                            Area = "Interés y disfrute"
                        });
                        break;

                    case "motivacion":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Pequeños pasos",
                            Descripcion = "Divide una tarea pendiente en acciones pequeñas y comienza únicamente con el primer paso.",
                            Area = "Motivación"
                        });
                        break;

                    case "auto_valoracion":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Reconocer tus fortalezas",
                            Descripcion = "Escribe tres cualidades, capacidades o acciones de las que puedas sentirte orgulloso/a.",
                            Area = "Auto valoración"
                        });
                        break;

                    case "esperanza_perspectiva":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Metas a corto plazo",
                            Descripcion = "Establece una meta pequeña y alcanzable para los próximos días y concéntrate en avanzar paso a paso.",
                            Area = "Esperanza y perspectiva"
                        });
                        break;
                }
            }

            // ==========================================
            // ANSIEDAD
            // ==========================================

            if (dimension == "ansiedad")
            {
                switch (area)
                {
                    case "preocupacion_miedo":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Organizar las preocupaciones",
                            Descripcion = "Escribe aquello que te preocupa y separa lo que puedes controlar de lo que no puedes controlar.",
                            Area = "Preocupación y miedo"
                        });
                        break;

                    case "activacion_fisica":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Respiración consciente",
                            Descripcion = "Realiza respiraciones lentas y controladas durante unos minutos, concentrándote en el ritmo de tu respiración.",
                            Area = "Activación física"
                        });
                        break;

                    case "ansiedad_situacional":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Preparación gradual",
                            Descripcion = "Ante una situación que te genere ansiedad, identifica un pequeño paso que puedas realizar de manera gradual.",
                            Area = "Ansiedad situacional"
                        });
                        break;

                    case "panico":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Técnica de anclaje",
                            Descripcion = "Observa tu entorno e identifica cinco cosas que puedas ver, cuatro que puedas tocar y tres que puedas escuchar.",
                            Area = "Pánico"
                        });
                        break;
                }
            }

            // ==========================================
            // ESTRÉS
            // ==========================================

            if (dimension == "estres")
            {
                switch (area)
                {
                    case "irritabilidad":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Pausa antes de reaccionar",
                            Descripcion = "Cuando notes irritación, haz una pausa, respira lentamente y espera unos segundos antes de responder.",
                            Area = "Irritabilidad"
                        });
                        break;

                    case "relajacion":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Espacio de relajación",
                            Descripcion = "Reserva unos minutos para alejarte de las actividades que generan tensión y realizar una actividad tranquila.",
                            Area = "Relajación"
                        });
                        break;

                    case "tension_activacion":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Liberar tensión",
                            Descripcion = "Realiza estiramientos suaves y acompáñalos con respiraciones lentas para ayudar a disminuir la tensión corporal.",
                            Area = "Tensión y activación"
                        });
                        break;

                    case "impaciencia":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Practicar la espera consciente",
                            Descripcion = "Cuando tengas que esperar, dirige tu atención a la respiración y evita anticipar cuánto tiempo falta.",
                            Area = "Impaciencia"
                        });
                        break;

                    case "tolerancia_frustracion":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Replantear la situación",
                            Descripcion = "Cuando algo no salga como esperabas, identifica qué puedes modificar y qué debes aceptar.",
                            Area = "Tolerancia a la frustración"
                        });
                        break;

                    case "recuperacion_emocional":
                        estrategias.Add(new Estrategia
                        {
                            Titulo = "Tiempo para recuperarte",
                            Descripcion = "Después de una situación estresante, dedica unos minutos a respirar, descansar y reconocer cómo te sientes.",
                            Area = "Recuperación emocional"
                        });
                        break;
                }
            }

            return estrategias;
        }
    }
}