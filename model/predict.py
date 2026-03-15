import argparse
import sys
from pathlib import Path
from typing import Tuple

import joblib
import numpy as np


def load_model(model_path: Path):
    if not model_path.exists():
        raise FileNotFoundError(f"Model not found at {model_path}. Train it first.")
    return joblib.load(model_path)


def classify_email(text: str, model_path: Path = Path("model/phishing_model.joblib")) -> Tuple[str, float]:
    """
    Returns predicted label and probability for the positive class ('phishing' if present).
    """
    model = load_model(model_path)
    proba = model.predict_proba([text])[0]

    # Determine phishing class index if labels are strings.
    classes = list(model.classes_)
    phishing_index = classes.index("phishing") if "phishing" in classes else int(np.argmax(proba))
    label = classes[phishing_index]
    score = float(proba[phishing_index])
    return label, score


def parse_args():
    parser = argparse.ArgumentParser(description="Classify a single email as phishing or legitimate.")
    parser.add_argument("--text", type=str, help="Raw email text to classify. If omitted, reads stdin.")
    parser.add_argument(
        "--model",
        type=Path,
        default=Path("model/phishing_model.joblib"),
        help="Path to a trained joblib model file.",
    )
    return parser.parse_args()


def main():
    args = parse_args()
    email_text = args.text if args.text is not None else sys.stdin.read()
    label, score = classify_email(email_text, args.model)
    print(f"Prediction: {label} (p={score:.3f})")


if __name__ == "__main__":
    main()
