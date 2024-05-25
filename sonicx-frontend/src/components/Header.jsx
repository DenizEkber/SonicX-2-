import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';
import "../Css/Header.css";

function Header() {
  const { isAuthenticated, user, setIsAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [showProfileMenu, setShowProfileMenu] = useState(false);
  const naviagte =useNavigate();

  const handleLogout = async () => {
    try {
      const res = await fetch('http://localhost:8001/logout', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      });
      const result = await res.json();
      if (result.success) {
        setIsAuthenticated(false);
        localStorage.removeItem('isAuthenticated');
        navigate('/');
      } else {
        console.error("Logout failed");
      }
    } catch (error) {
      console.error("An error occurred during logout:", error);
    }
  };

  return (
    <header className="header">
      <h1>Spotify Clone</h1>
      <div className="auth-buttons">
        {!isAuthenticated ? (
          <>
            <Link to="/login" className="login-button">Login</Link>
            <Link to="/register" className="register-button">Register</Link>
          </>
        ) : (
          <button onClick={() => setShowProfileMenu(!showProfileMenu)} className="profile-button">
            {user && `${user.firstName} ${user.lastName}`}
            {showProfileMenu && (
              <div className="profile-menu">
                <button onClick={handleLogout}>Logout</button>
              </div>
            )}
            
          </button>
        )}
      </div>
    </header>
  );
}

export default Header;
