document.addEventListener("DOMContentLoaded", function () {

    // ==========================================
    // MOSTRAR / OCULTAR CONTRASEÑA
    // ==========================================

    const passwordInput = document.getElementById("Password");
    const togglePassword = document.getElementById("togglePassword");
    const passwordIcon = document.getElementById("passwordIcon");

    if (passwordInput && togglePassword && passwordIcon) {

        togglePassword.addEventListener("click", function () {

            if (passwordInput.type === "password") {

                passwordInput.type = "text";

                passwordIcon.classList.remove("bi-eye");
                passwordIcon.classList.add("bi-eye-slash");

                togglePassword.setAttribute(
                    "aria-label",
                    "Ocultar contraseña"
                );

                togglePassword.setAttribute(
                    "title",
                    "Ocultar contraseña"
                );

            } else {

                passwordInput.type = "password";

                passwordIcon.classList.remove("bi-eye-slash");
                passwordIcon.classList.add("bi-eye");

                togglePassword.setAttribute(
                    "aria-label",
                    "Mostrar contraseña"
                );

                togglePassword.setAttribute(
                    "title",
                    "Mostrar contraseña"
                );
            }

        });

    }


    // ==========================================
    // TEMA CLARO / OSCURO
    // ==========================================

    const themeToggle = document.getElementById("themeToggle");

    // Aplicar el tema guardado
    const savedTheme = localStorage.getItem("theme");

    if (savedTheme === "dark") {
        document.documentElement.classList.add("dark");
    } else {
        document.documentElement.classList.remove("dark");
    }


    // Botón de cambio de tema
    if (themeToggle) {

        themeToggle.addEventListener("click", function () {

            document.documentElement.classList.toggle("dark");

            const isDark =
                document.documentElement.classList.contains("dark");

            localStorage.setItem(
                "theme",
                isDark ? "dark" : "light"
            );

        });

    }

});