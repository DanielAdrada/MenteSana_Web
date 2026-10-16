# -*- coding: utf-8 -*-
"""
Created on Tue May 19 17:47:20 2026

@author: linaM
"""

import pandas as pd
import joblib
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report, accuracy_score, confusion_matrix

# Carga del dataset
df = pd.read_csv(
    "C:/Proyectos Software/Dataset-DASS-42/dataset_dass42.csv")

# Definicion de preguntas DASS-42 para cada dimension
depresion = [
    'Q3A','Q5A','Q10A','Q13A','Q16A','Q17A',
    'Q21A','Q24A','Q26A','Q31A','Q34A',
    'Q37A','Q38A','Q42A']

ansiedad = [
    'Q2A','Q4A','Q7A','Q9A','Q15A','Q19A',
    'Q20A','Q23A','Q25A','Q28A','Q30A',
    'Q36A','Q40A','Q41A']

estres = [
    'Q1A','Q6A','Q8A','Q11A','Q12A','Q14A',
    'Q18A','Q22A','Q27A','Q29A','Q32A',
    'Q33A','Q35A','Q39A']

# Calculo de Scores
df['Depresion_score'] = df[depresion].sum(axis=1)
df['Ansiedad_score'] = df[ansiedad].sum(axis=1)
df['Estres_score'] = df[estres].sum(axis=1)

# Funcion de clasificación para depresion
def clasificar_depresion(x):

    if x <= 9:
        return "Normal"

    elif x <= 13:
        return "Leve"

    elif x <= 20:
        return "Moderado"

    elif x <= 27:
        return "Severo"

    else:
        return "Extremadamente Severo"

# Funcion de clasificacion para ansiedad
def clasificar_ansiedad(x):

    if x <= 7:
        return "Normal"

    elif x <= 9:
        return "Leve"

    elif x <= 14:
        return "Moderado"

    elif x <= 19:
        return "Severo"

    else:
        return "Extremadamente Severo"

# Funcion de clasificacion para estres
def clasificar_estres(x):

    if x <= 14:
        return "Normal"

    elif x <= 18:
        return "Leve"

    elif x <= 25:
        return "Moderado"

    elif x <= 33:
        return "Severo"

    else:
        return "Extremadamente Severo"

# Crea etiquetas 
df['Depresion_nivel'] = (
    df['Depresion_score']
    .apply(clasificar_depresion))

df['Ansiedad_nivel'] = (
    df['Ansiedad_score']
    .apply(clasificar_ansiedad))

df['Estres_nivel'] = (
    df['Estres_score']
    .apply(clasificar_estres))


# Variables de entrada 
X = df[[f'Q{i}A' for i in range(1, 43)]]


# Funcion de entrenamiento
def entrenar_modelo(target, nombre_modelo):
    print(f"ENTRENANDO: {nombre_modelo}")
    
    #Target
    y = df[target]

    # División train/test
    X_train, X_test, y_train, y_test = train_test_split(
        X,
        y,
        test_size=0.2,
        random_state=42,
        stratify=y)

    # Random Forest
    modelo = RandomForestClassifier(

        n_estimators=300,
        max_depth=15,
        min_samples_split=5,
        min_samples_leaf=2,
        class_weight='balanced',
        random_state=42)
    

    # Validacion cruzada
    cv_scores = cross_val_score(
        modelo,
        X_train,
        y_train,
        cv=5)

    print("VALIDACIÓN CRUZADA:")
    print(cv_scores)
    print("PROMEDIO CV:", cv_scores.mean())

    # Entrenamiento final
    modelo.fit(X_train, y_train)

    # Prediccion
    y_pred = modelo.predict(X_test)

    # Evaluacion
    print("\nExactitud:")
    print(accuracy_score(y_test,y_pred))

    print("\nReporte de clasificacion:")
    print(classification_report(
            y_test,
            y_pred))

    print("\nMatriz de confusion:")
    print(confusion_matrix(
            y_test,
            y_pred))


    # Importancia variables
    importancia = pd.DataFrame({

        'Pregunta': X.columns,
        'Importancia': modelo.feature_importances_  })

    importancia = importancia.sort_values(
        by='Importancia',
        ascending=False)

    print("\n PREGUNTAS MÁS IMPORTANTES:\n")
    print(importancia.head(10))


    # Guarda el modelo
    joblib.dump(modelo, nombre_modelo)
    print(f"\nMODELO GUARDADO: {nombre_modelo}")



# Entrena los tres modelos
entrenar_modelo(
    'Depresion_nivel',
    'modelo_depresion.pkl')

entrenar_modelo(
    'Ansiedad_nivel',
    'modelo_ansiedad.pkl')

entrenar_modelo(
    'Estres_nivel',
    'modelo_estres.pkl')

print("\nTODOS LOS MODELOS FUERON ENTRENADOS")





