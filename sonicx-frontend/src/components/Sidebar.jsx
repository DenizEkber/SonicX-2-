// Sidebar.jsx
import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import '../Css/Sidebar.css';

function Sidebar({ playlists, showCreatePlaylist, playlistName, setPlaylistName, handleCreateNewPlaylist }) {
  const navigation = useNavigate();


  return (
    <div className="sidebar">
      <ul>
        <li>
          <Link to="/menu">Home</Link>
        </li>
        <li>
          <Link to="/search">Search</Link>
        </li>
      </ul>
    </div>
  );
}

export default Sidebar;
