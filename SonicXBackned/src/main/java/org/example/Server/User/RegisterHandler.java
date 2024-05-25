package org.example.Server.User;


import com.sun.net.httpserver.HttpHandler;
import com.sun.net.httpserver.HttpExchange;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;
import org.example.Server.RequestData;
import org.example.UserManagment.*;

import java.io.IOException;
import java.io.OutputStream;


public class RegisterHandler implements HttpHandler {
    @Override
    public void handle(HttpExchange exchange) throws IOException {
        if ("POST".equals(exchange.getRequestMethod())) {
            ObjectMapper objectMapper = new ObjectMapper();
            RequestData requestData = objectMapper.readValue(exchange.getRequestBody(), RequestData.class);

            System.out.println("Gelen veri: " + requestData);

            UserService userService = UserService.getInstance();
            boolean isSucces= userService.register(requestData.getFirstName(), requestData.getLastName(), requestData.getEmail(), requestData.getPassword());

            ObjectNode responseJson = objectMapper.createObjectNode();
            responseJson.put("success", isSucces);

            byte[] response = responseJson.toString().getBytes();
            exchange.getResponseHeaders().add("Content-Type", "application/json");
            exchange.sendResponseHeaders(200, response.length);
            OutputStream os = exchange.getResponseBody();
            os.write(response);
            os.close();
        } else {
            exchange.sendResponseHeaders(405, -1); // 405 Method Not Allowed
        }
    }
}
