package org.example.Server.User;



import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import org.example.UserManagement.User;
import org.example.UserManagment.UserService;

import java.io.IOException;
import java.io.OutputStream;

public class ProfileHandler implements HttpHandler {

    @Override
    public void handle(HttpExchange exchange) throws IOException {
        if ("GET".equals(exchange.getRequestMethod())) {


            UserService userService = UserService.getInstance();
            User currentUser = userService.getUser();
            if (currentUser != null) {
                ObjectMapper objectMapper = new ObjectMapper();
                ObjectNode userData = objectMapper.createObjectNode();
                userData.put("firstName", currentUser.getFirstName());
                userData.put("lastName", currentUser.getLastName());
                userData.put("email", currentUser.getEmail());

                ObjectNode responseJson = objectMapper.createObjectNode();
                responseJson.put("success", true);
                responseJson.set("data", userData);

                byte[] response = responseJson.toString().getBytes();
                exchange.getResponseHeaders().add("Content-Type", "application/json");
                exchange.sendResponseHeaders(200, response.length);
                OutputStream os = exchange.getResponseBody();
                os.write(response);
                os.close();
            } else {
                exchange.sendResponseHeaders(401, -1);
            }
        }
        else {
            exchange.sendResponseHeaders(405, -1); // Method Not Allowed
        }
    }
}
