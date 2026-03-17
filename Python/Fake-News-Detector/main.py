from fastapi import FastAPI
from joblib import load
from pydantic import BaseModel
import uvicorn

model = load("best_rfc.pkl")

app = FastAPI(title="Fake News Detector API")

class NewsItem(BaseModel):
    statement: str
    subject: str
    context: str

@app.get("/")
def health_check():
    return {"message": "Fake News Detector API is running"}


@app.post("/predict")
def predict(news_item: NewsItem):
    input_data = [[news_item.statement, news_item.subject, news_item.context]]
    prediction = model.predict(input_data)[0]
    return {"prediction": int(prediction)}


if __name__ == "__main__":    
    uvicorn.run(app, port=8080) 