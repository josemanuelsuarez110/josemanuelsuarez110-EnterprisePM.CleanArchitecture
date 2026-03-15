import argparse
from pathlib import Path

import joblib
import pandas as pd
from sklearn import metrics
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.pipeline import Pipeline
from sklearn.linear_model import LogisticRegression

from preprocessing.text_cleaner import clean_text


def build_pipeline() -> Pipeline:
    # TfidfVectorizer uses the cleaner as a preprocessor; stemming already done there.
    vectorizer = TfidfVectorizer(
        preprocessor=clean_text,
        ngram_range=(1, 2),
        max_features=5000,
        min_df=2,
    )
    classifier = LogisticRegression(
        max_iter=200,
        n_jobs=-1,
        class_weight="balanced",
    )
    return Pipeline([("tfidf", vectorizer), ("clf", classifier)])


def load_data(csv_path: Path) -> tuple[pd.Series, pd.Series]:
    df = pd.read_csv(csv_path)
    if "text" not in df.columns or "label" not in df.columns:
        raise ValueError("CSV must contain 'text' and 'label' columns.")
    return df["text"], df["label"]


def train(csv_path: Path, model_path: Path, test_size: float = 0.2, random_state: int = 42):
    texts, labels = load_data(csv_path)
    X_train, X_test, y_train, y_test = train_test_split(
        texts, labels, test_size=test_size, random_state=random_state, stratify=labels
    )

    pipeline = build_pipeline()
    pipeline.fit(X_train, y_train)

    preds = pipeline.predict(X_test)
    report = metrics.classification_report(y_test, preds, digits=3)
    print("Evaluation report:\n", report)

    model_path.parent.mkdir(parents=True, exist_ok=True)
    joblib.dump(pipeline, model_path)
    print(f"Model saved to {model_path}")


def parse_args():
    parser = argparse.ArgumentParser(description="Train phishing email detector.")
    parser.add_argument(
        "--data",
        type=Path,
        default=Path("dataset/emails.csv"),
        help="Path to CSV with 'text' and 'label' columns.",
    )
    parser.add_argument(
        "--out",
        type=Path,
        default=Path("model/phishing_model.joblib"),
        help="Where to store the trained model.",
    )
    parser.add_argument("--test-size", type=float, default=0.2, help="Holdout size between 0 and 1.")
    parser.add_argument("--seed", type=int, default=42, help="Random seed for reproducibility.")
    return parser.parse_args()


if __name__ == "__main__":
    args = parse_args()
    train(csv_path=args.data, model_path=args.out, test_size=args.test_size, random_state=args.seed)
