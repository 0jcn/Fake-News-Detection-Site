import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import '../styles/Auth.css';

function CreateAccount({ onCreateAccount }) {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  });
  const [errorMessage, setErrorMessage] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrorMessage('');

    if (!formData.username.trim() || !formData.email.trim() || !formData.password.trim()) {
      setErrorMessage('Please complete all fields.');
      return;
    }

    if (formData.password !== formData.confirmPassword) {
      setErrorMessage('Passwords do not match.');
      return;
    }

    try {
      setIsSubmitting(true);
      const response = await fetch(
        `${process.env.REACT_APP_API_BASE_URL}/Users`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            id:0,
            username: formData.username.trim(),
            password: formData.password,
            email: formData.email.trim(),
            tier: 'User',
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString() 

          })
        }
      );

      const data = await response.json();
      console.log(response);
      console.log(data);
      
      if(response.status === 200){
        if (data?.value?.token) {
          sessionStorage.setItem('authToken', data.value.token);
        }

        onCreateAccount();
        navigate('/');
      }
      else{
        setErrorMessage(data?.message || 'Account creation failed. Please try again.');
        return;
      }
    } catch (error) {
      setErrorMessage('Unable to reach the server. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <section className="auth-page">
      <div className="auth-card">
        <h1>Create Account</h1>
        <p className="auth-subtext">Create an account to save detections</p>

        <form className="auth-form" onSubmit={handleSubmit}>
          <label htmlFor="signup-name">Username</label>
          <input
            id="signup-name"
            type="text"
            value={formData.username}
            onChange={(e) => setFormData({ ...formData, username: e.target.value })}
            required
          />

          <label htmlFor="signup-email">Email</label>
          <input
            id="signup-email"
            type="email"
            value={formData.email}
            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
            required
          />

          <label htmlFor="signup-password">Password</label>
          <input
            id="signup-password"
            type="password"
            value={formData.password}
            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
            required
          />

          <label htmlFor="signup-confirm-password">Confirm Password</label>
          <input
            id="signup-confirm-password"
            type="password"
            value={formData.confirmPassword}
            onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })}
            required
          />

          {errorMessage && <p className="auth-error">{errorMessage}</p>}

          <button type="submit" className="auth-submit-button" disabled={isSubmitting}>
            {isSubmitting ? 'Creating Account...' : 'Create Account'}
          </button>
        </form>

        <p className="auth-switch-text">
          Already have an account? <Link to="/login">Login here</Link>
        </p>
      </div>
    </section>
  );
}

export default CreateAccount;
