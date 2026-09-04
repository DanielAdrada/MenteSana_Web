console.log("✅ modal.js cargado");

document.addEventListener("DOMContentLoaded", () => {
    console.log("✅ DOM cargado");

    console.log("openLogin:", document.getElementById("openLogin"));
    console.log("openRegister:", document.getElementById("openRegister"));
    console.log("openRegister2:", document.getElementById("openRegister2"));
    console.log("loginModal:", document.getElementById("loginModal"));
    console.log("registerModal:", document.getElementById("registerModal"));

    const loginModal = document.getElementById("loginModal");
    const registerModal = document.getElementById("registerModal");

    const pass = document.getElementById("regPassword");
    const confirm = document.getElementById("regConfirmPassword");
    const passwordError = document.getElementById("regPasswordError");
    const registerForm = document.querySelector("#registerModal form");

    const openLoginBtns = [
        document.getElementById("openLogin")
    ].filter(Boolean);

    const openRegisterBtns = [
        document.getElementById("openRegister"),
        document.getElementById("openRegister2")
    ].filter(Boolean);

    const closeLogin = document.getElementById("closeLogin");
    const closeRegister = document.getElementById("closeRegister");

    function openModal(modal) {
        if (!modal) return;

        modal.style.opacity = "1";
        modal.style.pointerEvents = "auto";
        modal.style.display = "flex";

        const box = modal.querySelector(".modal-box");
        if (box) {
            box.style.transform = "scale(1)";
        }

        console.log("🔥 MODAL ABIERTO:", modal.id);
    }

    function closeModal(modal) {
        if (!modal) return;

        modal.style.opacity = "0";
        modal.style.pointerEvents = "none";

        const box = modal.querySelector(".modal-box");
        if (box) {
            box.style.transform = "scale(0.95)";
        }
    }


    openLoginBtns.forEach(btn => {
        btn.addEventListener("click", () => {
            closeModal(registerModal);
            openModal(loginModal);
        });
    });

    openRegisterBtns.forEach(btn => {
        btn.addEventListener("click", () => {
            closeModal(loginModal);
            openModal(registerModal);

            if (pass && confirm) {
                pass.value = "";
                confirm.value = "";
                setTimeout(validatePassword, 50);
            }
        });
    });

    closeLogin?.addEventListener("click", () => closeModal(loginModal));
    closeRegister?.addEventListener("click", () => closeModal(registerModal));

    window.addEventListener("click", (e) => {
        if (e.target === loginModal) closeModal(loginModal);
        if (e.target === registerModal) closeModal(registerModal);
    });

    document.querySelectorAll(".toggle-password").forEach(button => {
        button.addEventListener("click", () => {
            const input = button.previousElementSibling;
            if (!input || input.tagName !== "INPUT") return;

            input.type = input.type === "password" ? "text" : "password";
            button.textContent = input.type === "password" ? "🙈" : "🙉";
        });
    });

    function updateRule(id, state, text) {
        const el = document.getElementById(id);
        if (!el) return;

        el.classList.remove("text-gray-400", "text-green-600", "text-red-600");

        if (state === "neutral") {
            el.textContent = "⭕ " + text;
            el.classList.add("text-gray-400");
        } else if (state === "ok") {
            el.textContent = "✔️ " + text;
            el.classList.add("text-green-600");
        } else {
            el.textContent = "❌ " + text;
            el.classList.add("text-red-600");
        }
    }

    function validatePassword() {
        if (!pass || !confirm) return;

        const p = pass.value;
        const c = confirm.value;

        updateRule("rule-length",
            p.length === 0 ? "neutral" : p.length < 6 ? "error" : "ok",
            "Mínimo 6 caracteres"
        );

        updateRule("rule-match",
            (!p || !c) ? "neutral" : (p !== c ? "error" : "ok"),
            "Las contraseñas coinciden"
        );
    }

    if (pass && confirm) {
        pass.addEventListener("input", validatePassword);
        confirm.addEventListener("input", validatePassword);
        validatePassword();
    }

    if (registerForm && pass && confirm) {
        registerForm.addEventListener("submit", (e) => {
            if (pass.value !== confirm.value) {
                e.preventDefault();
                passwordError?.classList.remove("hidden");
                confirm.focus();
            } else {
                passwordError?.classList.add("hidden");
            }
        });
    }
});

