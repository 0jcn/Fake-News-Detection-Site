import React, { useState } from 'react';
import '../styles/Home.css';

function Home() {
  const [inputText, setInputText] = useState('');
  const [prediction, setPrediction] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleGetPrediction = async () => {
    if (!inputText.trim()) {
      setPrediction('Please enter some text');
      return;
    }

    setIsLoading(true);
    
    try {
      const response = await fetch('https://127.0.0.1:7271/Detector/', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ statement: inputText })
      });
      const data = await response.json();
      console.log(data);
      const predictionResult = `${data.value.prediction}\n\n${data.value.probabilities}}`;
      setPrediction(predictionResult);
    } catch (error) {
      setPrediction('Error: Failed to get prediction');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="Home">
      <div className="container">
        <h1>Fake News Detection</h1>
        
        <div className="input-section">
          <textarea
            value={inputText}
            onChange={(e) => setInputText(e.target.value)}
            placeholder="Enter your text here..."
            rows="4"
            cols="50"
          />
        </div>

        <button 
          onClick={handleGetPrediction}
          disabled={isLoading}
          className="predict-button"
        >
          {isLoading ? 'Processing...' : 'Get Prediction'}
        </button>

        <div className="results-section">
          <h2>Results:</h2>
          <div className="prediction-result">
            {prediction || <span className="placeholder">Prediction will appear here...</span>}
          </div>
        </div>
      </div>
    </div>
  );
}

export default Home;
