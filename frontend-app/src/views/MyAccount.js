import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/ProfilePages.css';

function MyAccount() {
  const navigate = useNavigate();
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [isUpdating, setIsUpdating] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [statusMessage, setStatusMessage] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  useEffect(() => {
    let isMounted = true;

    const loadUser = async () => {
      try {
        setIsLoading(true);
        setErrorMessage('');
        setStatusMessage('');

        const apiBaseUrl = process.env.REACT_APP_API_BASE_URL;
        const authToken = localStorage.getItem('authToken');
        const response = await fetch(`${apiBaseUrl}/Users`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            ...(authToken ? { Authorization: `Bearer ${authToken}` } : {})
          }
        });

        const data = await response.json().catch(() => null);

        if (!response.ok) {
          throw new Error(data?.message || 'Unable to load account details.');
        }

        const user = data?.value || data;

        if (isMounted) {
          setUsername(user?.username || '');
          setEmail(user?.email || '');
        }
      } catch (error) {
        if (isMounted) {
          setErrorMessage(error instanceof Error ? error.message : 'Unable to load account details.');
        }
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    };

    loadUser();

    return () => {
      isMounted = false;
    };
  }, []);

  const handleUpdateAccount = async () => {
    if (!username.trim() || !email.trim()) {
      setErrorMessage('Username and email are required.');
      return;
    }

    try {
      setIsUpdating(true);
      setErrorMessage('');
      setStatusMessage('');

      const apiBaseUrl = process.env.REACT_APP_API_BASE_URL;
      const authToken = localStorage.getItem('authToken');
      const response = await fetch(`${apiBaseUrl}/Users`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          ...(authToken ? { Authorization: `Bearer ${authToken}` } : {})
        },
        body: JSON.stringify({
          username: username.trim(),
          email: email.trim()
        })
      });

      const data = await response.json().catch(() => null);

      if (!response.ok) {
        throw new Error(data?.message || 'Unable to update account.');
      }

      setStatusMessage('Account details updated successfully.');
    } catch (error) {
      setErrorMessage(error instanceof Error ? error.message : 'Unable to update account.');
    } finally {
      setIsUpdating(false);
    }
  };

  const handleDeleteAccount = async () => {
    const shouldDelete = window.confirm('Are you sure you want to delete your account? This cannot be undone.');
    if (!shouldDelete) {
      return;
    }

    try {
      setIsDeleting(true);
      setErrorMessage('');
      setStatusMessage('');

      const apiBaseUrl = process.env.REACT_APP_API_BASE_URL;
      const authToken = localStorage.getItem('authToken');
      const response = await fetch(`${apiBaseUrl}/Users`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          ...(authToken ? { Authorization: `Bearer ${authToken}` } : {})
        }
      });

      if (!response.ok) {
        const data = await response.json().catch(() => null);
        throw new Error(data?.message || 'Unable to delete account.');
      }

      localStorage.removeItem('authToken');
      localStorage.setItem('isLoggedIn', 'false');
      localStorage.removeItem('latestDetection');
      navigate('/login');
    } catch (error) {
      setErrorMessage(error instanceof Error ? error.message : 'Unable to delete account.');
    } finally {
      setIsDeleting(false);
    }
  };

  return (
    <section className="profile-page">
      <div className="profile-card">
        <h1>My Account</h1>

        {isLoading && <p>Loading account details...</p>}

        {!isLoading && (
          <div className="account-form">
            <label htmlFor="account-username">Username</label>
            <input
              id="account-username"
              type="text"
              value={username}
              onChange={(event) => setUsername(event.target.value)}
            />

            <label htmlFor="account-email">Email</label>
            <input
              id="account-email"
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
            />

            {errorMessage && <p className="saved-error">{errorMessage}</p>}
            {statusMessage && <p className="account-success">{statusMessage}</p>}

            <div className="account-actions">
              <button
                type="button"
                className="account-update-button"
                onClick={handleUpdateAccount}
                disabled={isUpdating || isDeleting}
              >
                {isUpdating ? 'Updating...' : 'Update Details'}
              </button>

              <button
                type="button"
                className="account-delete-button"
                onClick={handleDeleteAccount}
                disabled={isDeleting || isUpdating}
              >
                {isDeleting ? 'Deleting...' : 'Delete Account'}
              </button>
            </div>
          </div>
        )}
      </div>
    </section>
  );
}

export default MyAccount;
