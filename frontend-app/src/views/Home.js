import React, { useState } from 'react';
import '../styles/Home.css';

const probabilityKeyMap = [
  { label: 'True', key: 'true' },
  { label: 'Barely True', key: 'barely-true' },
  { label: 'Half True', key: 'half-true' },
  { label: 'False', key: 'false' },
  { label: 'Mostly True', key: 'mostly-true' },
  { label: 'Pants Fire', key: 'pants-fire' }
];


const buildSavedDetectionPayload = (latestDetection) => {
  const probabilities = latestDetection?.probabilities || {};
  const prediction = latestDetection?.prediction;
  const timestamp = new Date().toISOString();

  return {
    input: latestDetection?.statement || '',
    result: prediction,
    trueProbability: Number(probabilities['true']),
    barelyTrueProbability: Number(probabilities['barely-true']),
    halfTrueProbability: Number(probabilities['half-true']),
    falseProbability: Number(probabilities['false']),
    mostlyTrueProbability: Number(probabilities['mostly-true']),
    pantsFireProbability: Number(probabilities['pants-fire']),
    createdAt: timestamp,
    updatedAt: timestamp
  };
};

function Home({ isLoggedIn }) {
  const [inputText, setInputText] = useState('');
  const [prediction, setPrediction] = useState('');
  const [latestDetection, setLatestDetection] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [saveStatus, setSaveStatus] = useState('');

  const handleGetPrediction = async () => {
    if (!inputText.trim()) {
      setPrediction('Please enter some text');
      setLatestDetection(null);
      setSaveStatus('');
      return;
    }

    setIsLoading(true);
    setPrediction('');
    setSaveStatus('');
    
    try {
      let apiBaseUrl = process.env.REACT_APP_API_BASE_URL;
      const response = await fetch(`${apiBaseUrl}/Detector/`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${sessionStorage.getItem('authToken')}` },
        body: JSON.stringify({ statement: inputText })
      });
      const data = await response.json();
      console.log(data);
      if (!response.ok) {
        throw new Error(data?.message || 'Failed to get prediction');
      }

      setLatestDetection({
        statement: inputText,
        prediction: data?.value?.prediction,
        probabilities: data?.value?.probabilities,
        detectedAt: new Date().toISOString()
      });
    } catch (error) {
      setPrediction('Error: Failed to get prediction');
      setLatestDetection(null);
    } finally {
      setIsLoading(false);
    }
  };

  const handleSaveDetection = async () => {
    if (!latestDetection) {
      return;
    }

    try {
      setSaveStatus('Saving...');
      sessionStorage.setItem('latestDetection', JSON.stringify(latestDetection));

      const apiBaseUrl = process.env.REACT_APP_API_BASE_URL;
      console.log('API Base URL:', apiBaseUrl);
      console.log('Auth Token:', sessionStorage.getItem('authToken'));
      console.log('Payload:', buildSavedDetectionPayload(latestDetection));
      const authToken = sessionStorage.getItem('authToken');
      const payload = buildSavedDetectionPayload(latestDetection);
      const response = await fetch(`${apiBaseUrl}/SaveDetections`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json','Authorization': `Bearer ${authToken}`
        },
        body: JSON.stringify(payload)
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        throw new Error(errorData?.message || 'Save request failed');
      }

      setSaveStatus('Detection saved to API successfully.');
    } catch (error) {
      setSaveStatus('Unable to save to API right now. Latest detection is still stored locally.');
    }
  };

  const shouldShowSaveButton = isLoggedIn && !!latestDetection;

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

        {shouldShowSaveButton && (
          <button
            type="button"
            onClick={handleSaveDetection}
            className="save-button"
          >
            Save Detection
          </button>
        )}

        {saveStatus && <p className="save-status">{saveStatus}</p>}

        <div className="results-section">
          <h2>Results:</h2>
          <div className="prediction-result">
            {latestDetection ? (
              <>
                <p><strong>Result:</strong> {latestDetection.prediction}</p>
                <h3>Probabilities</h3>
                <div className="probabilities-list">
                  {probabilityKeyMap.map((probability) => (
                    <p key={probability.key}>
                      <strong>{probability.label}:</strong> {latestDetection?.probabilities?.[probability.key]}
                    </p>
                  ))}
                </div>
              </>
            ) : (
              prediction || <span className="placeholder">Prediction will appear here...</span>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

export default Home;
