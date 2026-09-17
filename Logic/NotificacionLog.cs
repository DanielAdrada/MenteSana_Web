using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic
{
    public class NotificacionLog
    {
        private NotificacionDat notificacionDat = new NotificacionDat();

        public List<NotificacionDTO> ObtenerNotificaciones()
        {
            return notificacionDat.ObtenerNotificaciones();
        }

        public bool MarcarComoAtendida(int notificacionId)
        {
            return notificacionDat.MarcarComoAtendida(notificacionId);
        }

        public int ContarNotificacionesPendientes()
        {
            return notificacionDat.ContarNotificacionesPendientes();
        }
    }
}