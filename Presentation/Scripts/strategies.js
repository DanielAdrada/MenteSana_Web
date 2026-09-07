function editarEstrategia(id, dimension, area, nivel, titulo, descripcion) {

    document.getElementById("tituloFormulario").innerText =
        "✏️ Editar estrategia";

    document.getElementById("formEstrategia").action =
        "/Strategies/Editar";

    // Seleccionar dimensión
    document.getElementById("dimension").value = dimension;

    // Cargar las áreas correspondientes a esa dimensión
    cargarAreas();

    // Después de cargar las opciones, seleccionar el área
    document.getElementById("area").value = area;

    // Seleccionar nivel
    document.getElementById("nivel").value = nivel;

    // Cargar título y descripción
    document.getElementById("titulo").value = titulo;
    document.getElementById("descripcion").value = descripcion;

    // Crear/obtener el ID de la estrategia
    let inputId = document.getElementById("estrategiaId");

    if (inputId == null) {

        inputId = document.createElement("input");

        inputId.type = "hidden";
        inputId.id = "estrategiaId";
        inputId.name = "estrategiaId";

        document.getElementById("formEstrategia")
            .appendChild(inputId);
    }

    inputId.value = id;

    // Cambiar texto del botón
    document.getElementById("btnGuardar").innerText =
        "Actualizar estrategia";

    // Subir al formulario
    document.getElementById("formEstrategia")
        .scrollIntoView({
            behavior: "smooth",
            block: "start"
        });
}



function cargarAreas() {

    const dimension = document.getElementById("dimension").value;
    const area = document.getElementById("area");

    area.innerHTML = "";

    if (!dimension) {

        const opcion = document.createElement("option");
        opcion.value = "";
        opcion.textContent = "Seleccione primero una dimensión";

        area.appendChild(opcion);

        return;
    }

    const areas = {

        DEPRESION: [
            {
                valor: "estado_animo",
                texto: "Estado de ánimo"
            },
            {
                valor: "interes_disfrute",
                texto: "Interés y disfrute"
            },
            {
                valor: "motivacion",
                texto: "Motivación"
            },
            {
                valor: "auto_valoracion",
                texto: "Autovaloración"
            },
            {
                valor: "esperanza_perspectiva",
                texto: "Esperanza y perspectiva"
            }
        ],

        ANSIEDAD: [
            {
                valor: "preocupacion_miedo",
                texto: "Preocupación y miedo"
            },
            {
                valor: "activacion_fisica",
                texto: "Activación física"
            },
            {
                valor: "ansiedad_situacional",
                texto: "Ansiedad situacional"
            },
            {
                valor: "panico",
                texto: "Pánico"
            }
        ],

        ESTRES: [
            {
                valor: "irritabilidad",
                texto: "Irritabilidad"
            },
            {
                valor: "relajacion",
                texto: "Relajación"
            },
            {
                valor: "tension_activacion",
                texto: "Tensión y activación"
            },
            {
                valor: "impaciencia",
                texto: "Impaciencia"
            },
            {
                valor: "tolerancia_frustracion",
                texto: "Tolerancia a la frustración"
            },
            {
                valor: "recuperacion_emocional",
                texto: "Recuperación emocional"
            }
        ]
    };

    const opcionInicial = document.createElement("option");

    opcionInicial.value = "";
    opcionInicial.textContent = "Seleccione un área";

    area.appendChild(opcionInicial);

    areas[dimension].forEach(function (item) {

        const opcion = document.createElement("option");

        opcion.value = item.valor;
        opcion.textContent = item.texto;

        area.appendChild(opcion);
    });
}
