import React from 'react';

function MusicBox({ song, onSongSelect }) {
  const handleSongClick = () => {
    onSongSelect(song);
  };

  return (
    <div className="song-card" onClick={handleSongClick}>
      {song.album.images[0]?.url && <img src={song.album.images[0].url} alt={song.name} />}
      <h3>{song.name}</h3>
      <p>{song.artists.map(artist => artist.name).join(', ')}</p>
    </div>
  );
}

export default MusicBox;
