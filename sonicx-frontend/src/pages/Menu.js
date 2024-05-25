import React, { useState, useEffect } from 'react';
import Header from '../components/Header';
import Sidebar from '../components/Sidebar';
import MusicBox from '../components/MusicBox';
import '../Css/Menu.css';
import Playlist from './Playlist';

function Menu() {
  const [songs, setSongs] = useState([]);
  const [selectedSong, setSelectedSong] = useState(null);
  const [backgroundColor, setBackgroundColor] = useState('#f8f8f8');
  const [playlists, setPlaylists] = useState([]);
  const [playlistName, setPlaylistName] = useState('');
  const [selectedPlaylist, setSelectedPlaylist] = useState(null);
  const [showCreatePlaylist, setShowCreatePlaylist] = useState(false);
  const [songMenus, setSongMenus] = useState({});
  const [hoveredSong, setHoveredSong] = useState(null);

  useEffect(() => {
    fetchSongs();
    fetchPlaylists();
  }, []);

  const fetchSongs = async () => {
    try {
      const response = await fetch('http://localhost:8080/api/songs');
      if (!response.ok) {
        throw new Error('Failed to fetch songs');
      }
      const data = await response.json();
      setSongs(data.items || []);
    } catch (error) {
      console.error('Error fetching songs:', error.message);
    }
  };

  const fetchPlaylists = async () => {
    try {
      const response = await fetch('http://localhost:8001/showPlaylist');
      if (!response.ok) {
        throw new Error('Failed to fetch playlists');
      }
      const data = await response.json();
      setPlaylists(data.playlists || []);
      console.log(data.playlists);
    } catch (error) {
      console.error('Error fetching playlists:', error.message);
    }
  };

  const handleMouseEnter = () => {
    const randomColor = generateRandomColor();
    setBackgroundColor(randomColor);
  };

  const generateRandomColor = () => {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  };

  const handleSongSelect = (song) => {
    setSelectedSong(song);
  };

  const handleAddToPlaylist = (songId) => {
    setSongMenus((prevMenus) => ({
      ...prevMenus,
      [songId]: true
    }));
  };

  const handlePlaylistSelect = (playlist) => {
    setSelectedPlaylist(playlist);
    setSongMenus({});
  };

  const handleCreateNewPlaylist = async () => {
    try {
      if (!selectedSong) {
        console.error('No song selected');
        alert('Please select a song before creating a playlist.');
        return;
      }

      const response = await fetch('http://localhost:8001/createPlaylist', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          playlistName,
          song: {
            title: selectedSong.name,
            artist: selectedSong.artists.map(artist => artist.name).join(', '),
            album: selectedSong.album.name,
            duration: 100,
            path: selectedSong.preview_url
          }
        }),
      });

      if (!response.ok) {
        throw new Error('Failed to create playlist');
      }

      const data = await response.json();
      console.log('Playlist created:', data);
      fetchPlaylists();
    } catch (error) {
      console.error('Error creating playlist:', error.message);
    }
  };

  const handleShowPlaylistContent = async (playlistName) => {
    try {
      const response = await fetch('http://localhost:8001/showPlaylistContent', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          playlistName
        }),
      });

      if (!response.ok) {
        throw new Error('Failed to fetch playlist content');
      }

      const data = await response.json();
      console.log('Playlist content:', data);
      // Burada playlist içeriğini göstermek için bir işlem yapabilirsiniz
    } catch (error) {
      console.error('Error fetching playlist content:', error.message);
    }
  };

  const handleAddToExistingPlaylist = async (playlistName, selectedSong) => {
    try {
      if (!selectedSong) {
        console.error('No song selected');
        alert('Please select a song before adding to playlist.');
        return;
      }
  
      const response = await fetch('http://localhost:8001/createPlaylist', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          playlistName,
          song: {
            title: selectedSong.name,
            artist: selectedSong.artists.map(artist => artist.name).join(', '),
            album: selectedSong.album.name,
            duration: 100,
            path: selectedSong.preview_url
          }
        }),
      });
  
      if (!response.ok) {
        throw new Error('Failed to add to playlist');
      }
  
      const data = await response.json();
      console.log('Song added to playlist:', data);
    } catch (error) {
      console.error('Error adding to playlist:', error.message);
    }
  };
  
  

  return (
    <div className="menu" style={{ backgroundColor }}>
      <div className="main-layout" onMouseEnter={handleMouseEnter}>
        <Sidebar className='sidebar'
          playlists={playlists}
          setPlaylistName={setPlaylistName}
          selectedPlaylist={selectedPlaylist}
          showCreatePlaylist={showCreatePlaylist}
          handleCreateNewPlaylist={handleCreateNewPlaylist}
          handleShowPlaylistContent={handleShowPlaylistContent}
        />
        <Playlist className='playlist' 
        playlists={playlists}
          setPlaylistName={setPlaylistName}
          selectedPlaylist={selectedPlaylist}
          showCreatePlaylist={showCreatePlaylist}
          handleCreateNewPlaylist={handleCreateNewPlaylist}
          handleShowPlaylistContent={handleShowPlaylistContent}/>
        <div className="content">
          <h2>Songs</h2>
          <div className="songs">
            {songs.map((song, index) => (
              <div className='song'
                key={index}
                onMouseEnter={() => setHoveredSong(song.id)}
                onMouseLeave={() => setHoveredSong(null)}
              >
                {hoveredSong === song.id && (
                  <button className="play-button" song={song} onClick={() => setSelectedSong(song)}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="currentColor" className="bi bi-play-circle-fill" viewBox="0 0 16 16">
                      <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M6.79 5.093A.5.5 0 0 0 6 5.5v5a.5.5 0 0 0 .79.407l3.5-2.5a.5.5 0 0 0 0-.814z"/>
                    </svg>
                  </button>
                )}
                <MusicBox song={song} onSongSelect={handleSongSelect} />
                <div className="add-to-playlist-container">
                  <button onClick={() => handleAddToPlaylist(song.id)}>Add to Playlist</button>
                  {songMenus[song.id] && (
                    <div className="add-to-playlist-menu">
                    <h3>Add to Playlist</h3>
                    {playlists.map((playlist, index) => (
                      <div key={index} onClick={() => handleAddToExistingPlaylist(playlist,selectedSong)}>
                        {playlist}
                      </div>
                    ))}
                    <button onClick={() => setShowCreatePlaylist(true)}>Create New Playlist</button>
                  </div>
                  
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
        <div className="selected-song">
          {selectedSong && (
            <div className="song-info">
              <h2>{selectedSong.name}</h2>
              <p>{selectedSong.artists.map(artist => artist.name).join(', ')}</p>
              <p>Album: {selectedSong.album.name}</p>
              <audio controls>
                <source src={selectedSong.preview_url} type="audio/mpeg" />
                  Your browser does not support the audio element.
                </audio>
            </div>
          )}
        </div>
    </div>
  </div>
  );
}

export default Menu;
