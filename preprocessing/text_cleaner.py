import re
from typing import Iterable

import nltk
from nltk.corpus import stopwords
from nltk.stem import SnowballStemmer

# Ensure required resources are available; download quietly if missing.
for resource in ("stopwords", "punkt"):
    try:
        nltk.data.find(f"corpora/{resource}")
    except LookupError:
        nltk.download(resource, quiet=True)


_stop_words = set(stopwords.words("english"))
_stemmer = SnowballStemmer("english")


def _strip_urls(text: str) -> str:
    url_pattern = re.compile(r"https?://\S+|www\.\S+")
    return url_pattern.sub(" ", text)


def _strip_html(text: str) -> str:
    html_pattern = re.compile(r"<.*?>")
    return html_pattern.sub(" ", text)


def _normalize_tokens(tokens: Iterable[str]) -> str:
    cleaned = []
    for token in tokens:
        token = re.sub(r"[^a-zA-Z]", "", token).lower()
        if not token or token in _stop_words:
            continue
        cleaned.append(_stemmer.stem(token))
    return " ".join(cleaned)


def clean_text(text: str) -> str:
    """
    Lightweight normalization for email bodies:
    - strips URLs and HTML tags
    - tokenizes with NLTK
    - removes punctuation/numbers/stopwords
    - applies stemming
    Returns a whitespace-normalized string suitable for vectorizers.
    """
    if not isinstance(text, str):
        text = "" if text is None else str(text)

    text = _strip_urls(text)
    text = _strip_html(text)
    tokens = nltk.word_tokenize(text)
    return _normalize_tokens(tokens)


__all__ = ["clean_text"]
