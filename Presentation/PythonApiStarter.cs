using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Presentation
{
    public class PythonApiStarter
    {
        private static Process _process;

        public static void Start()
        {
            if (_process != null && !_process.HasExited)
                return;

            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "ML_prediccion.py",
                WorkingDirectory = @"C:\GestionEmocional",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _process = Process.Start(psi);

            // Espera 2 segundos para que Flask levante bien
            System.Threading.Thread.Sleep(2000);
        }

        public static void Stop()
        {
            if (_process != null && !_process.HasExited)
            {
                _process.Kill();
                _process.Dispose();
            }
        }
    }
}