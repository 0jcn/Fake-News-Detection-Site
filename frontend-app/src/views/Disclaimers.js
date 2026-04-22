import React from 'react';
import '../styles/Disclaimers.css';
import classificationReport from '../imgs/classification_report.png';
import confusionMatrix from '../imgs/confusion_matrix.png';

function Disclaimers() {
  return (
    <div className="disclaimers-page">
      <div className="disclaimers-card">
        <h1>Disclaimers</h1>

        <div className="disclaimer-text-field">
            The model is not 100% accurate and may produce false positives or false negatives. It should be used as a tool to assist in evaluating news articles, not as a definitive authority on their veracity. 
            Users should always verify information from multiple sources before drawing conclusions. 
            Below shows the confusion matrix and classification report for the model, which was trained on the LIAR dataset, this can be found from Kaggle at this link <a href="https://www.kaggle.com/datasets/doanquanvietnamca/liar-dataset/data" target="_blank" rel="noopener noreferrer">here</a>.        
            </div>

        <div className="disclaimer-image-grid">
          <div className="disclaimer-image-slot">
            <img src={classificationReport} alt="Classification report" className="disclaimer-image" />
          </div>
          <div className="disclaimer-image-slot">
            <img src={confusionMatrix} alt="Confusion matrix" className="disclaimer-image" />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Disclaimers;