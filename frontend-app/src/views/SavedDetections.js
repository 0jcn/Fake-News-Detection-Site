import React, { useEffect, useMemo, useState } from 'react';
import '../styles/ProfilePages.css';

const truncateText = (text, maxLength = 120) => {
  if (!text) {
    return '';
  }

  return text.length > maxLength ? `${text.slice(0, maxLength).trim()}...` : text;
};

const normalizeDetections = (rawData) => {
  if (Array.isArray(rawData)) {
    return rawData;
  }

  if (Array.isArray(rawData?.value)) {
    return rawData.value;
  }

  return [];
};

function SavedDetections() {
  const [savedDetections, setSavedDetections] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState('');
  const [selectedDetection, setSelectedDetection] = useState(null);
  const [deletingIds, setDeletingIds] = useState([]);

  useEffect(() => {
    let isMounted = true;

    const fetchSavedDetections = async () => {
      try {
        setIsLoading(true);
        setErrorMessage('');

        const apiBaseUrl = process.env.REACT_APP_API_BASE_URL;
        const authToken = sessionStorage.getItem('authToken');
        const response = await fetch(`${apiBaseUrl}/SaveDetections`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json', Authorization: `Bearer ${authToken}`
          }
        });

        const data = await response.json().catch(() => null);

        if (!response.ok) {
          throw new Error(data?.message || 'Unable to load saved detections.');
        }

        if (isMounted) {
          setSavedDetections(normalizeDetections(data));
        }
      } catch (error) {
        if (isMounted) {
          setErrorMessage(error instanceof Error ? error.message : 'Unable to load saved detections.');
        }
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    };

    fetchSavedDetections();

    return () => {
      isMounted = false;
    };
  }, []);

  const selectedProbabilities = useMemo(() => {
    if (!selectedDetection) {
      return [];
    }

    return [
      { label: 'True', value: selectedDetection.trueProbability },
      { label: 'Barely True', value: selectedDetection.barelyTrueProbability },
      { label: 'Half True', value: selectedDetection.halfTrueProbability },
      { label: 'False', value: selectedDetection.falseProbability },
      { label: 'Mostly True', value: selectedDetection.mostlyTrueProbability },
      { label: 'Pants Fire', value: selectedDetection.pantsFireProbability }
    ];
  }, [selectedDetection]);

  const handleDeleteDetection = async (detectionId) => {
    try {
      setDeletingIds((prev) => [...prev, detectionId]);

      const apiBaseUrl = process.env.REACT_APP_API_BASE_URL;
      const authToken = sessionStorage.getItem('authToken');
      const response = await fetch(`${apiBaseUrl}/SaveDetections/${detectionId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${authToken}`
        }
      });

      if (!response.ok) {
        const data = await response.json().catch(() => null);
        throw new Error(data?.message || 'Unable to delete detection.');
      }

      setSavedDetections((prev) => prev.filter((item) => item.detectionId !== detectionId));
      setSelectedDetection((prev) => (prev?.detectionId === detectionId ? null : prev));
    } catch (error) {
      setErrorMessage(error instanceof Error ? error.message : 'Unable to delete detection.');
    } finally {
      setDeletingIds((prev) => prev.filter((id) => id !== detectionId));
    }
  };

  return (
    <section className="profile-page">
      <div className="profile-card">
        <h1>Saved Detections</h1>

        {isLoading && <p>Loading saved detections...</p>}

        {!isLoading && errorMessage && <p className="saved-error">{errorMessage}</p>}

        {!isLoading && !errorMessage && savedDetections.length === 0 && (
          <p>No saved detections found yet.</p>
        )}

        {!isLoading && !errorMessage && savedDetections.length > 0 && (
          <div className="saved-detections-grid">
            {savedDetections.map((detection) => (
              <div
                key={detection.detectionId}
                className="saved-detection-card"
                data-detection-id={detection.detectionId}
                onClick={() => setSelectedDetection(detection)}
                role="button"
                tabIndex={0}
                onKeyDown={(event) => {
                  if (event.key === 'Enter' || event.key === ' ') {
                    setSelectedDetection(detection);
                  }
                }}
              >
                <h2>Detection #{detection.detectionId}</h2>
                <p><strong>Date:</strong> {new Date(detection.createdAt).toLocaleString()}</p>
                <p><strong>Result:</strong> {detection.result}</p>
                <p><strong>Input:</strong> {truncateText(detection.input)}</p>
                <button
                  type="button"
                  className="saved-delete-button"
                  onClick={(event) => {
                    event.stopPropagation();
                    handleDeleteDetection(detection.detectionId);
                  }}
                  disabled={deletingIds.includes(detection.detectionId)}
                >
                  {deletingIds.includes(detection.detectionId) ? 'Deleting...' : 'Delete'}
                </button>
              </div>
            ))}
          </div>
        )}
      </div>

      {selectedDetection && (
        <div className="saved-modal-overlay" onClick={() => setSelectedDetection(null)}>
          <div className="saved-modal" onClick={(event) => event.stopPropagation()}>
            <h2>Detection #{selectedDetection.detectionId}</h2>
            <p><strong>Date:</strong> {new Date(selectedDetection.createdAt).toLocaleString()}</p>
            <p><strong>Result:</strong> {selectedDetection.result}</p>
            <p><strong>Full Input:</strong></p>
            <div className="saved-modal-input">{selectedDetection.input}</div>

            <h3>Probabilities</h3>
            <div className="saved-probabilities-list">
              {selectedProbabilities.map((probability) => (
                <p key={probability.label}>
                  <strong>{probability.label}:</strong> {probability.value}
                </p>
              ))}
            </div>

            <button
              type="button"
              className="saved-modal-close"
              onClick={() => setSelectedDetection(null)}
            >
              Close
            </button>
          </div>
        </div>
      )}
    </section>
  );
}

export default SavedDetections;
