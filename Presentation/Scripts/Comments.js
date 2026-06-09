function mostrarEdicion(id) {
    var div =
        document.getElementById(
            "editar-" + id);

    if (div.style.display === "none") {
        div.style.display = "block";
    }
    else {
        div.style.display = "none";
    }
}

function cancelarEdicion(id) {
    const div =
        document.getElementById("editar-" + id);

    div.style.display = "none";
}