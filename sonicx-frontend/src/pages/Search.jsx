import React, { useState } from 'react';
import Header from '../components/Header';
import Sidebar from '../components/Sidebar';
import '../Css/Search.css';

function Search() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);

  const handleSearch = () => {
    fetch(`http://localhost:8080/api/search?query=${query}`)
      .then(response => response.json())
      .then(data => {
        console.log('Search results:', data);
        setResults(data.tracks.items);
        console.log(data.track.items);
      })
      .catch(error => console.error('Error fetching search results:', error));
  };

  return (
    <div className="search">
      
      <div className="main-content">
        <Sidebar />
        <div className="content">
          <h2>Search</h2>
          <input
            type="text"
            placeholder="Search for a song..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <button onClick={handleSearch}>Search</button>
          <div className="results">
            {results.map((track, index) => (
              <div key={index} className="track-card">
                <img src={track.album.images[0]?.url} alt={track.name} />
                <h3>{track.name}</h3>
                <p>{track.artists.map(artist => artist.name).join(', ')}</p>
                <audio controls>
                  <source src={track.preview_url} type="audio/mpeg" />
                  Your browser does not support the audio element.
                </audio>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}

export default Search;
