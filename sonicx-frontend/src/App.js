import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import Search from './pages/Search';
import Login from './pages/Login'; // Login sayfasını import et
import Register from './pages/Register'; // Register sayfasını import et
import './App.css';
import VerifyEmail from './pages/VerifyEmail';
import Menu from './pages/Menu';
import { AuthProvider } from './components/AuthContext';
import Header from './components/Header';
import Profile from './components/Profile';
import PlaylistPage from './components/PlaylistPage';


function App() {
  return (
    <AuthProvider>
      <Header/>
    <div className="App">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/search" element={<Search />} />
        <Route path="/login" element={<Login />} /> {/* Login sayfasına yönlendirme */}
        <Route path="/register/*" element={<Register />} /> {/* Register sayfasına yönlendirme */}
        <Route path="/profile" element={<Profile/>} />
        <Route path="/register/verify-email/:email" element={<VerifyEmail />} /> {/* Register sayfasına yönlendirme */}
        <Route path="/menu/playlist/:playlistName" element={<PlaylistPage />} />
        <Route path="/menu" element={<Menu />} />
      </Routes>
    </div>
    </AuthProvider>
  );
}

export default App;
