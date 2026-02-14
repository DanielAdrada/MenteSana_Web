// ===============================
// TEMA CLARO / OSCURO
// ===============================
const themeToggle = document.getElementById('themeToggle');
const root = document.documentElement;

const savedTheme = localStorage.getItem('ms_theme');
if (savedTheme === 'dark') {
    root.classList.add('dark');
}

if (themeToggle) {
    themeToggle.addEventListener('click', () => {
        root.classList.toggle('dark');
        const theme = root.classList.contains('dark') ? 'dark' : 'light';
        localStorage.setItem('ms_theme', theme);
    });
}

// ===============================
// TRANSICIÓN DE PÁGINA 
// ===============================
document.addEventListener("DOMContentLoaded", () => {
    const page = document.getElementById("page-transition");
    if (!sessionStorage.getItem("ms_loaded")) {
        page.classList.add("opacity-0", "translate-y-4", "scale-95");
        sessionStorage.setItem("ms_loaded", "1");

        requestAnimationFrame(() => {
            page.classList.remove("opacity-0", "translate-y-4", "scale-95");
        });
    }
});


// ===============================
// FRASES MOTIVACIONALES
// ===============================
const quoteEl = document.getElementById('quote');
if (quoteEl) {
    const quotes = [
        'Respira profundo: estás haciendo lo mejor que puedes.',
        'Pequeños pasos también son progreso.',
        'Pedir ayuda es un acto de valentía.',
        'Tu mente es única y poderosa.',
        'Hoy es un buen día para empezar de nuevo.'
    ];

    let i = Math.floor(Math.random() * quotes.length);
    quoteEl.textContent = quotes[i];

    setInterval(() => {
        i = (i + 1) % quotes.length;
        quoteEl.textContent = quotes[i];
    }, 8000);
}
