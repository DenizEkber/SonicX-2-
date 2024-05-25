package org.example.Server.Playlist;



import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import org.example.MusicManagment.MusicFile;
import org.example.PlaylistManagment.Playlist;
import org.example.PlaylistManagement.PlaylistManager;
import org.example.UserManagment.UserService;

import java.io.IOException;
import java.io.OutputStream;
import java.sql.SQLException;

public class PlaylistCreateHandler implements HttpHandler {

    @Override
    public void handle(HttpExchange exchange) throws IOException {
        if ("POST".equals(exchange.getRequestMethod())) {
            ObjectMapper objectMapper = new ObjectMapper();


            UserService userService = UserService.getInstance();
            String requestBody = new String(exchange.getRequestBody().readAllBytes());
            ObjectNode jsonRequest = (ObjectNode) objectMapper.readTree(requestBody);
            String playlistName = jsonRequest.get("playlistName").asText();
            ObjectNode songNode = (ObjectNode) jsonRequest.get("song");

            String title = songNode.get("title").asText();
            String artist = songNode.get("artist").asText();
            String album = songNode.get("album").asText();
            int duration = songNode.get("duration").asInt();
            String path = songNode.get("path").asText();

            PlaylistManager playlistManager= null;
            try {
                playlistManager = new PlaylistManager();
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }
            try {
                playlistManager.createPlaylistTableForUser(userService);
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }


            MusicFile musicFile = new MusicFile(title, artist, album, duration, path);

            Playlist playlist = new Playlist(playlistName);
            playlist.addMusicFileToPlaylist(musicFile,userService);



            ObjectNode responseJson = objectMapper.createObjectNode();
            responseJson.put("success", true);

            byte[] response = responseJson.toString().getBytes();
            exchange.getResponseHeaders().add("Content-Type", "application/json");
            exchange.sendResponseHeaders(200, response.length);
            OutputStream os = exchange.getResponseBody();
            os.write(response);
            os.close();
        } else {
            exchange.sendResponseHeaders(405, -1);
        }
    }
}


