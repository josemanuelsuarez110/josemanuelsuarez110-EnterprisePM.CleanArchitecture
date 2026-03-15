# phishing-email-detector

Pequeño prototipo en Python para entrenar y probar un detector de correos de phishing usando `pandas`, `nltk` y `scikit-learn`.

## Estructura
- `dataset/emails.csv`: ejemplos etiquetados (`label`, `text`).
- `preprocessing/text_cleaner.py`: limpieza y normalización con NLTK.
- `model/train_model.py`: entrena y guarda el pipeline TF‑IDF + Regresión Logística.
- `model/predict.py`: carga el modelo y clasifica texto individual.

## Requisitos
```bash
python -m venv .venv
.\.venv\Scripts\activate  # Windows
pip install -U pip
pip install scikit-learn pandas nltk joblib
```
La primera ejecución descargará `stopwords` y `punkt` de NLTK si no están disponibles.

## Entrenamiento
```bash
python model/train_model.py --data dataset/emails.csv --out model/phishing_model.joblib
```
Parámetros opcionales: `--test-size 0.2`, `--seed 42`.

## Predicción
```bash
python model/predict.py --text "Revisa tu cuenta urgentemente en http://malicioso.test"
# o
echo "mensaje aquí" | python model/predict.py
```
Salida ejemplo: `Prediction: phishing (p=0.91)`

## Notas
- El pipeline ya incluye la limpieza, vectorización y el clasificador, por lo que `predict.py` solo necesita el texto en crudo.
- El dataset incluido es mínimo y sirve de placeholder; usa tus propios correos etiquetados para un rendimiento realista.
