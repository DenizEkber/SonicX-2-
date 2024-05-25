// PlaylistContent.jsx
import React from 'react';

function PlaylistContent({ playlistContent }) {
    if (!playlistContent || !playlistContent.musicFiles) {
      console.log(playlistContent);
      return <div>No playlist content available </div>;
    }
  
    return (
      <div>
        <h2>{playlistContent[0]?.playlistName}</h2>
        <ul>
          {playlistContent.musicFiles.map((music, index) => (
            <li key={index}><div className="song-info">
            <h2>{music.title}</h2>
            <p>{music.artist}</p>
            <p>Album: {music.album}</p>
            <audio controls>
              <source src={music.path} type="audio/mpeg" />
                Your browser does not support the audio element.
              </audio>
            </div></li>
            
          ))}
        </ul>
      </div>
    );
  }
  export default PlaylistContent;
