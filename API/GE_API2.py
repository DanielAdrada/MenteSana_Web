from flask import Flask, request, jsonify
import joblib
import numpy as np
import os

app = Flask(__name__)


# ==========================================
# CARGAR MODELOS
# ==========================================

BASE_DIR = os.path.dirname(os.path.abspath(__file__))

modelo_depresion = joblib.load(
    os.path.join(BASE_DIR, "modelo_depresion.pkl")
)

modelo_ansiedad = joblib.load(
    os.path.join(BASE_DIR, "modelo_ansiedad.pkl")
)

modelo_estres = joblib.load(
    os.path.join(BASE_DIR, "modelo_estres.pkl")
)


# ==========================================
# ENDPOINT DASS-42
# ==========================================

@app.route("/predecir_dass42", methods=["POST"])
def predecir_dass42():

    data = request.get_json(silent=True)

    if not data:
        return jsonify({
            "error": "No se recibió JSON válido"
        }), 400

    respuestas = data.get("respuestas")

    if not isinstance(respuestas, list):
        return jsonify({
            "error": "El campo respuestas debe ser una lista."
        }), 400

    if len(respuestas) != 42:
        return jsonify({
            "error": "Se requieren exactamente 42 respuestas."
        }), 400

    try:
        respuestas = [int(x) for x in respuestas]

    except (ValueError, TypeError):
        return jsonify({
            "error": "Todas las respuestas deben ser enteros."
        }), 400


    # ==========================================
    # VALIDAR VALORES
    # ==========================================

    if any(x < 0 or x > 3 for x in respuestas):

        return jsonify({
            "error": "Las respuestas deben estar entre 0 y 3."
        }), 400


    # ==========================================
    # PREPARAR DATOS
    # ==========================================

    X = np.array(respuestas).reshape(1, -1)


    # ==========================================
    # PREDICCIONES
    # ==========================================

    depresion = modelo_depresion.predict(X)[0]

    ansiedad = modelo_ansiedad.predict(X)[0]

    estres = modelo_estres.predict(X)[0]


    # ==========================================
    # RESPUESTA
    # ==========================================

    resultado = {

        "depresion": depresion,

        "ansiedad": ansiedad,

        "estres": estres

    }

    return jsonify(resultado)


# ==========================================
# INICIAR API
# ==========================================

if __name__ == "__main__":

    app.run(
        port=5000,
        debug=False,
        use_reloader=False
    )  