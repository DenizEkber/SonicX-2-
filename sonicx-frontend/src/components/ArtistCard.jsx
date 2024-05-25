import React, { useState } from 'react';
import "../Css/Card.css";
import {play} from '../logo/play.svg';

function ArtistCard({ artist }) {
  const [isHovered, setIsHovered] = useState(false);
  const handleMouseEnter = () => setIsHovered(true);
  const handleMouseLeave = () => setIsHovered(false);

  return (
    <div className="artist-card"
    onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
      >
      <img src={artist.image} alt={artist.name} />
      <h3>{artist.name}</h3>
      <button
        className="play-button"
        style={{ display: isHovered ? 'block' : 'none' }}
      >
       {<svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="currentColor" class="bi bi-play-circle-fill" viewBox="0 0 16 16">
  <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M6.79 5.093A.5.5 0 0 0 6 5.5v5a.5.5 0 0 0 .79.407l3.5-2.5a.5.5 0 0 0 0-.814z"/>
</svg>}
      </button>
    </div>
  );
}

export default ArtistCard;
