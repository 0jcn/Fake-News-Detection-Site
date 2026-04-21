# python-api/Dockerfile
FROM python:3.11-slim

WORKDIR /app

# Copy requirements first (for better caching)
COPY requirements.txt .

# Install dependencies
RUN pip install --no-cache-dir -r requirements.txt

# Copy joblib model files (adjust paths as needed)
COPY best_rfc.pkl ./
COPY tfidf_vectorizer.joblib ./

# Copy application code
COPY . .

# Expose port
EXPOSE 8000

# Run the application
CMD ["fastapi", "run", "main.py", "--port", "8000"]
