const express = require('express');
const axios = require('axios');
const cors = require('cors');
const bodyParser = require('body-parser');
const querystring = require('querystring');

const app = express();
const port = 8080;
const app2=express();
const port2 = 8001;



<<<<<<< HEAD
const CLIENT_ID = ''; // Spotify Developer Dashboard'dan alın
const CLIENT_SECRET = ''; // Spotify Developer Dashboard'dan alın
=======
const CLIENT_ID = 'your_client-id'; // Spotify Developer Dashboard'dan alın
const CLIENT_SECRET = 'your_client_secret'; // Spotify Developer Dashboard'dan alın
>>>>>>> ca077760bbefc03e1caea75baeb2759449fa3451
const REDIRECT_URI = 'http://localhost:8080/callback';

let accessToken = '';
let refreshToken = '';

const fetchAccessToken = async (code) => {
  try {
    const response = await axios.post('https://accounts.spotify.com/api/token', querystring.stringify({
      grant_type: 'authorization_code',
      code: code,
      redirect_uri: REDIRECT_URI,
      client_id: CLIENT_ID,
      client_secret: CLIENT_SECRET,
    }), {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    });

    accessToken = response.data.access_token;
    refreshToken = response.data.refresh_token;
    console.log('Access token fetched successfully:', accessToken);
  } catch (error) {
    console.error('Error fetching access token:', error.response ? error.response.data : error.message);
  }
};

const fetchData = async (endpoint) => {
  try {
    const response = await axios.get(endpoint, {
      headers: {
        'Authorization': `Bearer ${accessToken}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error(`Error fetching data from ${endpoint}:`, error.response ? error.response.data : error.message);
    return null;
  }
};

app.use(cors());
app.use(bodyParser.json());

app.get('/login', (req, res) => {
  const scope = 'user-read-private user-read-email user-top-read playlist-read-private playlist-modify-public playlist-modify-private';
  res.redirect('https://accounts.spotify.com/authorize?' + querystring.stringify({
    response_type: 'code',
    client_id: CLIENT_ID,
    scope: scope,
    redirect_uri: REDIRECT_URI,
  }));
});

app.get('/callback', async (req, res) => {
  const code = req.query.code || null;
  await fetchAccessToken(code);
  res.redirect('/');
});

app.get('/api/artists', async (req, res) => {
  console.log('Fetching top artists...');
  const data = await fetchData('https://api.spotify.com/v1/me/top/artists');
  if (data) {
    console.log('Top artists data:', data);
    res.json(data);
  } else {
    res.status(500).json({ error: 'Failed to fetch top artists' });
  }
});


app.put('/api/albums', async (req, res) => {
  console.log('Fetching top albums...');
  const data = await fetchData('https://api.spotify.com/v1/me/albums');
  if (data) {
    console.log('Top albums data:', data);
    res.json(data);
  } else {
    res.status(500).json({ error: 'Failed to fetch top albums' });
  }
});

app.get('/api/playlists', async (req, res) => {
  console.log('Fetching playlists...');
  const data = await fetchData('https://api.spotify.com/v1/me/playlists');
  if (data) {
    console.log('Playlists data:', data);
    res.json(data);
  } else {
    res.status(500).json({ error: 'Failed to fetch playlists' });
  }
});

app.get('/api/songs', async (req, res) => {
  console.log('Fetching top songs...');
  const data = await fetchData('https://api.spotify.com/v1/me/top/tracks');
  if (data) {
    console.log('Top songs data:', data);
    res.json(data);
  } else {
    res.status(500).json({ error: 'Failed to fetch top songs' });
  }
});

app.get('/api/search', async (req, res) => {
  const { query } = req.query;
  if (!query) {
    return res.status(400).json({ error: 'Query parameter is required' });
  }
  
  try {
    const response = await axios.get(`https://api.spotify.com/v1/search?q=${query}&type=track`, {
      headers: {
        'Authorization': `Bearer ${accessToken}`,
      },
    });
    res.json(response.data);
  } catch (error) {
    console.error('Error fetching search results:', error.response ? error.response.data : error.message);
    res.status(500).json({ error: 'Failed to fetch search results' });
  }
});



app.listen(port, () => {
  console.log(`Server is running on http://localhost:${port}`);
});


app2.use(cors());
app2.use(bodyParser.json());

// Login endpoint
app2.post('/login', async(req, res) => {
    try {
        const { email, password } = req.body;
        // Front-end'den gelen bilgileri Java API'ye ilet
        const javaResponse = await axios.post('http://localhost:8002/login', { email, password });
        // Java'dan gelen cevabı front-end'e geri gönder
        res.json(javaResponse.data);
      } catch (error) {
        res.status(500).json({ error: 'An error occurred while processing your request.' });
      }
  });

  app2.post('/logout', async (req, res) => {
    try {
      // Java API'de logout işlemini çağır
      const javaResponse = await axios.post('http://localhost:8002/logout');
      res.json(javaResponse.data);
    } catch (error) {
      res.status(500).json({ error: 'An error occurred while processing your request.' });
    }
  });

  app2.post('/profile', async (req, res) => {
    try {
      // Java API'de logout işlemini çağır
      const javaResponse = await axios.post('http://localhost:8002/profile');
      res.json(javaResponse.data);
    } catch (error) {
      res.status(500).json({ error: 'An error occurred while processing your request.' });
    }
  });
  
  // Register endpoint
  app2.post('/register', async (req, res) => {
    try {
      const { firstName, lastName, email, password } = req.body;
      // Front-end'den gelen bilgileri Java API'ye ilet
      const javaResponse = await axios.post('http://localhost:8002/register', { firstName, lastName, email, password });
      // Java'dan gelen cevabı front-end'e geri gönder
      res.json(javaResponse.data);
    } catch (error) {
      res.status(500).json({ error: 'An error occurred while processing your request.' });
    }
  });

  // VerifyEmail endpoint
  app2.post('/verify-email', async (req, res) => {
    try {
      const { email, verificationCode } = req.body;
      // Front-end'den gelen bilgileri Java API'ye ilet
      const javaResponse = await axios.post('http://localhost:8002/verify-email', { email, verificationCode });
      // Java'dan gelen cevabı front-end'e geri gönder
      res.json(javaResponse.data);
    } catch (error) {
      res.status(500).json({ error: 'An error occurred while processing your request.' });
    }
  });
  
  // Playlist oluşturma endpoint'i
  app2.post('/createPlaylist', async (req, res) => {
    try {
      const { playlistName, song } = req.body;
      // Front-end'den gelen bilgileri Java API'ye ilet
      const javaResponse = await axios.post('http://localhost:8002/playlist-create', { playlistName, song });
      // Java'dan gelen cevabı front-end'e geri gönder
      res.json();
    } catch (error) {
      res.status(500).json({ error: 'An error occurred while processing your request.' });
    }
  });

  app2.post('/showPlaylistContent', async (req, res) => {
    try {
      const { playlistName } = req.body;
      // Front-end'den gelen bilgileri Java API'ye ilet
      const javaResponse = await axios.post('http://localhost:8002/playlist-showContent', { playlistName });
      // Java'dan gelen cevabı front-end'e geri gönder
      res.json(javaResponse.data);
    } catch (error) {
      res.status(500).json({ error: 'An error occurred while processing your request.' });
    }
  });
  app2.get('/showPlaylist', async (req, res) => {
    try {
      //const { playlistName } = req.body;
      // Front-end'den gelen bilgileri Java API'ye ilet
      const javaResponse = await axios.get('http://localhost:8002/playlist-show');
      // Java'dan gelen cevabı front-end'e geri gönder
      res.json(javaResponse.data);
    } catch (error) {
      res.status(500).json({ error: 'An error occurred while processing your request.' });
    }
  });
  
  app2.listen(port2, () => {
    console.log(`Server is running on port ${port2}`);
  });
