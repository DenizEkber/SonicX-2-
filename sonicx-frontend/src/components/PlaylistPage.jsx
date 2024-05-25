// PlaylistPage.jsx
import React from 'react';
import { useLocation } from 'react-router-dom';
import PlaylistContent from './PlayListContent';

function PlaylistPage() {
  const location = useLocation();
  const playlistContent = location.state && location.state.playlistContent;
  
  if (!playlistContent) {
    return <div>No playlist content available</div>;
  }
  
  return (
    <div>
      <h1>Playlist Page</h1>
      <PlaylistContent playlistContent={playlistContent} />
    </div>
  );
}

export default PlaylistPage;
