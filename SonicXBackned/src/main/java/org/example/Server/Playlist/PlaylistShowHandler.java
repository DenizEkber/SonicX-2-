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
import java.sql.SQLException;
import java.util.List;

public class PlaylistShowHandler implements HttpHandler {
    @Override
    public void handle(HttpExchange exchange) throws IOException {
        if ("GET".equals(exchange.getRequestMethod())) {
            ObjectMapper objectMapper = new ObjectMapper();
            UserService userService = UserService.getInstance();
            PlaylistDAO playlistDAO = null;
            try {
                playlistDAO = new PlaylistDAO();
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }

            List<String> playlists = null;
            try {
                playlists = playlistDAO.getAllPlaylistNames(userService);
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }

            ArrayNode arrayNode = objectMapper.createArrayNode();
            for (String playlist : playlists) {
                arrayNode.add(playlist);
            }

            ObjectNode responseJson = objectMapper.createObjectNode();
            responseJson.set("playlists", arrayNode);

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
