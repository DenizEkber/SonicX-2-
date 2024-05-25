import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

function VerifyEmail() {
  const [verificationCode, setVerificationCode] = useState('');
  const [response, setResponse] = useState(null);
  const navigate = useNavigate();
  const {email}=useParams();

  const handleVerifyEmail = async (e) => {
    e.preventDefault();
    const res = await fetch('http://localhost:8001/verify-email', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, verificationCode }),
    });
    const result = await res.json();
    setResponse(result.success);
    if (result.success==true) {
      navigate('/login'); // Doğrulama başarılıysa anasayfaya yönlendir
    }
  };

  return (
    <div className="verify-email">
      <h2>Verify Email</h2>
      <form onSubmit={handleVerifyEmail}>
        <input
          type="email"
          placeholder="Email"
          value={email}
          readOnly
        />
        <input
          type="text"
          placeholder="Verification Code"
          value={verificationCode}
          onChange={e => setVerificationCode(e.target.value)}
        />
        <button type="submit">Verify</button>
      </form>
      {response !== null && (
        <div className="response">
          {response ? 'Verification successful!' : 'Invalid verification code.'}
        </div>
      )}
    </div>
  );
}

export default VerifyEmail;
