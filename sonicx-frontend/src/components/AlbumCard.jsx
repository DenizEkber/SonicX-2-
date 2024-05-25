import React from 'react';
import "../Css/Card.css";

function AlbumCard({ album }) {
  return (
    <div className="album-card">
      <img src={album.image} alt={album.title} />
      <h3>{album.title}</h3>
      <button className="play-button" style={{ display: 'none' }}>
        Play
      </button>
    </div>
  );
}

export default AlbumCard;
