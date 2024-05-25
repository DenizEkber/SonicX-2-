// src/pages/Register.js

import React, { useState } from 'react';
import { useHistory, useNavigate } from 'react-router-dom';
import '../Css/Register.css';


function Register() {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [response, setResponse] = useState(null);
  //const history = useHistory();
  const navigation=useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    const res = await fetch('http://localhost:8001/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ firstName, lastName, email, password }),
    });
    const result = await res.json();
    setResponse(result.message);
    if(result.success==true){
      navigation(`/register/verify-email/${email}`);
    }
    else{
      alert("Eposta zaten kayitli.");
      
    }
  };
  

  return (
    <div className="register">
      <h2>Register</h2>
      <input type="text" placeholder="First Name" value={firstName} onChange={e => setFirstName(e.target.value)} />
      <input type="text" placeholder="Last Name" value={lastName} onChange={e => setLastName(e.target.value)} />
      <input type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
      <input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} />
      <button onClick={handleRegister}>Register</button>
    </div>
  );
}

export default Register;
