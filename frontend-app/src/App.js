import './App.css';
import { useMemo, useState } from 'react';
import Home from './views/Home';
import Login from './views/Login';
import CreateAccount from './views/CreateAccount';
import SavedDetections from './views/SavedDetections';
import MyAccount from './views/MyAccount';
import Disclaimers from './views/Disclaimers';
import Navbar from './components/Navbar';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(() => sessionStorage.getItem('isLoggedIn') === 'true');

  const authActions = useMemo(
    () => ({
      login: () => {
        setIsLoggedIn(true);
        sessionStorage.setItem('isLoggedIn', 'true');
      },
      logout: () => {
        setIsLoggedIn(false);
        sessionStorage.setItem('isLoggedIn', 'false');
      }
    }),
    []
  );

  return (
    <Router>
      <div className="App">
        <Navbar isLoggedIn={isLoggedIn} onLogout={authActions.logout} />
        <main>
          <Routes>
            <Route path="/" element={<Home isLoggedIn={isLoggedIn} />} />
            <Route
              path="/login"
              element={isLoggedIn ? <Navigate to="/" replace /> : <Login onLogin={authActions.login} />}
            />
            <Route
              path="/create-account"
              element={isLoggedIn ? <Navigate to="/" replace /> : <CreateAccount onCreateAccount={authActions.login} />}
            />
            <Route
              path="/saved-detections"
              element={isLoggedIn ? <SavedDetections /> : <Navigate to="/login" replace />}
            />
            <Route
              path="/my-account"
              element={isLoggedIn ? <MyAccount /> : <Navigate to="/login" replace />}
            />
            <Route path="/disclaimers" element={<Disclaimers />} />
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
