
function cargarParaEditar(id, titulo, descripcion, tipo, archivo, url) {
    document.getElementById("ResourceId").value = id;
    document.getElementById("Titulo").value = titulo;
    document.getElementById("Descripcion").value = descripcion;
    document.getElementById("Tipo").value = tipo;
    document.getElementById("ArchivoActual").value = archivo || "";
    document.getElementById("Url").value = url || "";

    document.getElementById("btnGuardar").value = "Actualizar recurso";
    document.getElementById("btnCancelar").style.display = "inline-block";

    // Subir automáticamente al formulario
    window.scrollTo({ top: 0, behavior: 'smooth' });
}


function limpiarFormulario() {
    document.getElementById("ResourceId").value = 0;
    document.querySelector("form").reset();

    document.getElementById("btnGuardar").value = "Guardar Recurso";
    document.getElementById("btnCancelar").style.display = "none";
}



function filtrarRecursos(tipo, elemento) {

    const cards = document.querySelectorAll(".recurso-card");
    const botones = document.querySelectorAll(".filtro-btn");

    // activar botón seleccionado
    botones.forEach(b => b.classList.remove("active"));

    if (elemento) {
        elemento.classList.add("active");
    }

    // Lógica de filtrado
    cards.forEach(card => {
        card.style.display = (tipo === "Todos" || card.dataset.tipo === tipo) ? "block" : "none";
    });
}

function buscarRecursos() {

    const texto = document
        .getElementById("buscarRecurso")
        .value
        .trim()
        .toLowerCase();

    const tarjetas = document.querySelectorAll(".recurso-card");

    let encontrados = 0;

    tarjetas.forEach(card => {

        const titulo = card.dataset.titulo.toLowerCase();
        const descripcion = card.dataset.descripcion.toLowerCase();

        if (
            texto === "" ||
            titulo.includes(texto) ||
            descripcion.includes(texto)
        ) {

            card.style.display = "flex";
            encontrados++;

        } else {

            card.style.display = "none";

        }

    });

    const mensaje = document.getElementById("sinResultados");

    if (encontrados === 0) {
        mensaje.classList.remove("hidden");
    } else {
        mensaje.classList.add("hidden");
    }

}