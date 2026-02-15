document.addEventListener("DOMContentLoaded", () => {
    const toast = document.getElementById('toast');

    if (!toast) return;

    setTimeout(() => {
        toast.classList.add('opacity-0', 'translate-x-10');

        setTimeout(() => {
            toast.remove();
        }, 500);
    }, 3500);
});
