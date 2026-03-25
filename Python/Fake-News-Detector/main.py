import re
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from joblib import load
from pydantic import BaseModel
import uvicorn
import pandas as pd
import numpy as np

model = load("best_rfc.pkl")
vectorizer = load("tfidf_vectorizer.joblib")

app = FastAPI(title="Fake News Detector API")
app.add_middleware(
    CORSMiddleware,
    allow_origins=['*'],
    allow_credentials=True,
    allow_methods=['*'],
    allow_headers=['*'],
)

class NewsItem(BaseModel):
    statement: str

@app.get("/")
def health_check():
    return {"message": "Fake News Detector API is running"}

@app.post("/predict")
def predict(news_item: NewsItem):
    try:
        # Clean the statement
        statement = clean_text(news_item.statement)
        statement_vector = vectorizer.transform([statement]).toarray()[0]
        
        input_features = [
            statement_vector  # This is already a 1D array
           
        ]
        prediction = model.predict(input_features)[0]
        prediction_proba = model.predict_proba(input_features)[0]
        
        return {
            "prediction": prediction,
            "probabilities": prediction_proba.tolist()
        }
        
    except Exception as e:
        return {"error": str(e)}

def clean_text(text):
    if pd.isna(text):
        return ""
    text = str(text).lower()
    text = re.sub(r'https?://\S+|www\.\S+', '', text, flags=re.MULTILINE)
    text = re.sub(r'<.*?>', '', text)
    text = re.sub(r'\s+', ' ', text).strip()
    text = re.sub(r'[^\w\s\']', ' ', text)
    text = re.sub(r'\b\d+\b', '', text)
    text = re.sub(r'\s+', ' ', text).strip()
    
    return text

if __name__ == "__main__":    
    uvicorn.run(app, port=8080)
