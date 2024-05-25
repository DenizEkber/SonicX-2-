import { useNavigate, useParams } from 'react-router-dom';
import '../Css/Playlist.css';


export default function Playlist( { playlists, showCreatePlaylist, playlistName, setPlaylistName, handleCreateNewPlaylist }){
    const navigation=useNavigate();


    const handlePlaylistClick = async (playlistName) => {
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
          navigation(`/menu/playlist/${playlistName}`, { state: { playlistContent: data } });
        } catch (error) {
          console.error('Error fetching playlist content:', error.message);
        }
      };

    return(
        <div className="playlist">
      <ul>
        <li className="playlists">
          <span>Playlists</span>
          <ul>
            {playlists.map((playlist, index) => (
              <button key={index} onClick={() => handlePlaylistClick(playlist)}>
                {playlist}
              </button>
            ))}
          </ul>
          <ul>
          {showCreatePlaylist && (
          <div className="create-playlist-container">
            <input type="text" value={playlistName} onChange={(e) => setPlaylistName(e.target.value)} placeholder="Playlist Name" />
            <button onClick={handleCreateNewPlaylist}>Create Playlist</button>
          </div>
        )}
          </ul>
        </li>
      </ul>
    </div>
    );
}