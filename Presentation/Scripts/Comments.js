
function mostrarEdicion(id) {

    const div =
        document.getElementById("editar-" + id);

    div.classList.remove("hidden");
}

function cancelarEdicion(id) {

    const div =
        document.getElementById("editar-" + id);

    div.classList.add("hidden");
}

window.addEventListener("beforeunload", function () {
    sessionStorage.setItem(
        "scrollPosition",
        window.scrollY);
});

window.addEventListener("load", function () {

    const pos =
        sessionStorage.getItem("scrollPosition");

    if (pos) {
        window.scrollTo(0, parseInt(pos));
    }
});