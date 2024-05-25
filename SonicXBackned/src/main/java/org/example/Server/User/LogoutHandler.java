package org.example.Server.User;


import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import org.example.UserManagment.UserService;

import java.io.IOException;
import java.io.OutputStream;

public class LogoutHandler implements HttpHandler {

    @Override
    public void handle(HttpExchange exchange) throws IOException {
        if ("POST".equals(exchange.getRequestMethod())) {

            ObjectMapper objectMapper = new ObjectMapper();

            UserService userService = UserService.getInstance();
            userService.logout();


            ObjectNode responseJson = objectMapper.createObjectNode();
            responseJson.put("success", true);


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

