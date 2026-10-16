# MenteSana_Web
Mente Sana es una aplicación web desarrollada para apoyar la identificación de estados emocionales negativos asociados con depresión, ansiedad y estrés mediante la aplicación del cuestionario DASS-42.

La plataforma permite realizar la evaluación, procesar los resultados y generar recomendaciones para la gestión emocional. Además, cuenta con un componente de Machine Learning (Supervisado) encargado de realizar la clasificación de los resultados y una API que permite la comunicación entre los modelos y la aplicación web.

El sistema fue desarrollado utilizando ASP.NET MVC, C#, MySQL, Python y modelos de Machine Learning entrenados previamente.

### Machine Learning

Se incluye el código utilizado para el entrenamiento del modelo y los modelos previamente entrenados para la clasificación de las dimensiones de depresión, ansiedad y estrés a partir de las respuestas del DASS-42.

- `ML_GEE2.py`: código utilizado para el entrenamiento.
- `modelo_depresion.pkl`: modelo entrenado para depresión.
- `modelo_ansiedad.pkl`: modelo entrenado para ansiedad.
- `modelo_estres.pkl`: modelo entrenado para estrés.

Los modelos entrenados son utilizados posteriormente por la API para realizar las predicciones del sistema.
