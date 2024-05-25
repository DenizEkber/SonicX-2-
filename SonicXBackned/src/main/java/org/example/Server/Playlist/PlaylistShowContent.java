package org.example.Server.Playlist;


import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ArrayNode;
import com.fasterxml.jackson.databind.node.ObjectNode;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import org.example.MusicManagment.MusicFile;
import org.example.PlaylistManagment.Playlist;
import org.example.PlaylistManagment.PlaylistDAO;
import org.example.UserManagment.UserService;

import java.io.IOException;
import java.io.OutputStream;
import java.util.List;

public class PlaylistShowContent implements HttpHandler {
    public void handle(HttpExchange exchange) throws IOException {
        if ("POST".equals(exchange.getRequestMethod())) {
            ObjectMapper objectMapper = new ObjectMapper();
            UserService userService = UserService.getInstance();

            String requestBody = new String(exchange.getRequestBody().readAllBytes());
            ObjectNode jsonRequest = (ObjectNode) objectMapper.readTree(requestBody);
            String playlistName = jsonRequest.get("playlistName").asText();





            Playlist playlist = new Playlist(playlistName);
            List<MusicFile> musicFiles = playlist.getAllMusicFiles(userService);


            ObjectNode responseJson = objectMapper.createObjectNode();
            ArrayNode dataArray = objectMapper.createArrayNode();

            for (MusicFile musicFile : musicFiles) {
                ObjectNode musicNode = objectMapper.createObjectNode();
                musicNode.put("title", musicFile.getTitle());
                musicNode.put("playlistName", playlistName);
                musicNode.put("artist", musicFile.getArtist());
                musicNode.put("album", musicFile.getAlbum());
                musicNode.put("path", musicFile.getPath());


                dataArray.add(musicNode);
            }

            responseJson.put("musicFiles", dataArray);


            byte[] response = responseJson.toString().getBytes();
            exchange.getResponseHeaders().add("Content-Type", "application/json");
            exchange.sendResponseHeaders(200, response.length);
            OutputStream os = exchange.getResponseBody();
            os.write(response);
            os.close();
        } else {
            exchange.sendResponseHeaders(405, -1); // Method Not Allowed
        }
    }

}

