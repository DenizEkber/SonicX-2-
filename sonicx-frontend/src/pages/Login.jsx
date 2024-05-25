// src/pages/Login.js

import React, { useState } from 'react';
import { useHistory, useNavigate } from 'react-router-dom';
import { useAuth } from '../components/AuthContext';
import '../Css/Login.css';

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [response, setResponse] = useState(null);
  //const history = useHistory();
  const navigation=useNavigate();
  const { setIsAuthenticated } = useAuth();

  const handleLogin = async (e) => {
    e.preventDefault();
    const res = await fetch('http://localhost:8001/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, password }),
    });
    const result = await res.json();
    setResponse(result.message);
    if(result.success==true){
      setIsAuthenticated(true);
      navigation('/menu');
    }
  };
  

  return (
    <div className="login">
      <h2>Login</h2>
      <input type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
      <input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} />
      <button onClick={handleLogin}>Login</button>
      {response && <p>{response}</p>}
    </div>
  );
}

export default Login;
