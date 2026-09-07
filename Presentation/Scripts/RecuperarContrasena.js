console.log("🚨 RECUPERAR CONTRASEÑA JS CARGADO 🚨");
document.addEventListener("DOMContentLoaded", function () {

    // ==========================================
    // MOSTRAR / OCULTAR NUEVA CONTRASEÑA
    // ==========================================

    const nuevaContrasena =
        document.getElementById("nuevaContrasena");

    const toggleNueva =
        document.getElementById("toggleNuevaContrasena");

    const iconoNueva =
        document.getElementById("iconoNuevaContrasena");

    if (nuevaContrasena && toggleNueva && iconoNueva) {

        toggleNueva.addEventListener("click", function () {

            if (nuevaContrasena.type === "password") {

                nuevaContrasena.type = "text";

                iconoNueva.classList.remove("bi-eye");
                iconoNueva.classList.add("bi-eye-slash");

                toggleNueva.setAttribute(
                    "aria-label",
                    "Ocultar contraseña"
                );

                toggleNueva.setAttribute(
                    "title",
                    "Ocultar contraseña"
                );

            } else {

                nuevaContrasena.type = "password";

                iconoNueva.classList.remove("bi-eye-slash");
                iconoNueva.classList.add("bi-eye");

                toggleNueva.setAttribute(
                    "aria-label",
                    "Mostrar contraseña"
                );

                toggleNueva.setAttribute(
                    "title",
                    "Mostrar contraseña"
                );

            }

        });

    }


    // ==========================================
    // MOSTRAR / OCULTAR CONFIRMACIÓN
    // ==========================================

    const confirmarContrasena =
        document.getElementById("confirmarContrasena");

    const toggleConfirmar =
        document.getElementById("toggleConfirmarContrasena");

    const iconoConfirmar =
        document.getElementById("iconoConfirmarContrasena");

    if (confirmarContrasena && toggleConfirmar && iconoConfirmar) {

        toggleConfirmar.addEventListener("click", function () {

            console.log("👁️ CLICK EN CONFIRMAR CONTRASEÑA");

            if (confirmarContrasena.type === "password") {

                confirmarContrasena.type = "text";

                iconoConfirmar.classList.remove("bi-eye");
                iconoConfirmar.classList.add("bi-eye-slash");

                toggleConfirmar.setAttribute(
                    "aria-label",
                    "Ocultar contraseña"
                );

                toggleConfirmar.setAttribute(
                    "title",
                    "Ocultar contraseña"
                );

            } else {

                confirmarContrasena.type = "password";

                iconoConfirmar.classList.remove("bi-eye-slash");
                iconoConfirmar.classList.add("bi-eye");

                toggleConfirmar.setAttribute(
                    "aria-label",
                    "Mostrar contraseña"
                );

                toggleConfirmar.setAttribute(
                    "title",
                    "Mostrar contraseña"
                );

            }

        });

    }

});