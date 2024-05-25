package org.example.Server;


import com.sun.net.httpserver.HttpServer;
import org.example.Server.Email.VerifyEmailHandler;
import org.example.Server.Playlist.PlaylistCreateHandler;
import org.example.Server.Playlist.PlaylistShowContent;
import org.example.Server.Playlist.PlaylistShowHandler;
import org.example.Server.User.*;

import java.io.IOException;
import java.net.InetSocketAddress;

public class JavaHttpServer {

    public static void main(String[] args) throws IOException {
        HttpServer server = HttpServer.create(new InetSocketAddress(8002), 0);
        try {


            server.createContext("/login", new LoginHandler());
            server.createContext("/register", new RegisterHandler());
            server.createContext("/verify-email", new VerifyEmailHandler());
            server.createContext("/logout", new LogoutHandler()); // Logout context'i ekleyin
            server.createContext("/profile", new ProfileHandler());
            server.createContext("/playlist-create", new PlaylistCreateHandler());
            server.createContext("/playlist-show", new PlaylistShowHandler());
            server.createContext("/playlist-showContent", new PlaylistShowContent());
            server.setExecutor(null); // default executor
            server.start();
        }catch (Exception e){
            System.out.println(e.getMessage());
        }
        System.out.println("Java HTTP sunucusu çalışıyor: http://localhost:8002");
    }
}
