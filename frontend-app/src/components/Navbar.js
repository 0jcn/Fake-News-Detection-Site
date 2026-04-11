import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import '../styles/Navbar.css';

function Navbar({ isLoggedIn, onLogout }) {
  const navigate = useNavigate();

  const handleLogout = () => {
    sessionStorage.removeItem('authToken');
    onLogout();
    navigate('/');
  };

  return (
    <nav className="site-navbar">
      <div className="site-navbar-inner">
        <NavLink to="/" className="site-brand">
          Fake News Detector
        </NavLink>

        <div className="site-nav-links">
          {!isLoggedIn ? (
            <>
            <NavLink
                to="/"
                className={({ isActive }) => (isActive ? 'site-nav-link active-link' : 'site-nav-link')}
              >
                Home
              </NavLink>
              <NavLink
                to="/login"
                className={({ isActive }) => (isActive ? 'site-nav-link active-link' : 'site-nav-link')}
              >
                Login
              </NavLink>
              <NavLink
                to="/create-account"
                className={({ isActive }) =>
                  isActive ? 'site-nav-link site-nav-cta active-link' : 'site-nav-link site-nav-cta'
                }
              >
                Create Account
              </NavLink>
            </>
          ) : (
            <>
            <NavLink
                to="/"
                className={({ isActive }) => (isActive ? 'site-nav-link active-link' : 'site-nav-link')}
              >
                Home
              </NavLink>
              <NavLink
                to="/saved-detections"
                className={({ isActive }) => (isActive ? 'site-nav-link active-link' : 'site-nav-link')}
              >
                Saved Detections
              </NavLink>
              <NavLink
                to="/my-account"
                className={({ isActive }) => (isActive ? 'site-nav-link active-link' : 'site-nav-link')}
              >
                My Account
              </NavLink>
              <button type="button" className="site-nav-link logout-button" onClick={handleLogout}>
                Logout
              </button>
            </>
          )}
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
