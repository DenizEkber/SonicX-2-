package org.example.PlaylistManagement;

import org.example.DataBase.DatabaseConnector;
import org.example.UserManagement.User;
import org.example.UserManagment.UserService;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class PlaylistManager {

    private final Connection connection;

    public PlaylistManager() throws SQLException {
        this.connection = DatabaseConnector.getInstance().getConnection();
    }

    public void createPlaylistTableForUser(UserService userService) throws SQLException {
        String tableName = "playlist_" + userService.getUser().getId();
        if (isTableExists(tableName)) {
            System.out.println("Playlist table for user " + userService.getUser().getFirstName() + " already exists.");
            return;
        }

        String sql = "CREATE TABLE IF NOT EXISTS " + tableName +
                " (id INT PRIMARY KEY AUTO_INCREMENT, " +
                "playlist_name VARCHAR(255), " +
                "title VARCHAR(255), " +
                "artist VARCHAR(255), " +
                "album VARCHAR(255), " +
                "duration INT, " +
                "path VARCHAR(300))";
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.executeUpdate();
            System.out.println("Playlist table created for user: " + userService.getUser().getFirstName());
        }
        catch (SQLException e) {
            System.out.println("Error creating playlist table: " + e.getMessage());
        }
    }

    public void dropPlaylistTableForUser(User user) throws SQLException {
        String tableName = "playlist_" + user.getId();
        if (!isTableExists(tableName)) {
            System.out.println("Playlist table for user " + user.getFirstName() + " does not exist.");
            return;
        }

        String sql = "DROP TABLE " + tableName;
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.executeUpdate();
            System.out.println("Playlist table dropped for user: " + user.getFirstName());
        }
    }

    public boolean isTableExists(String tableName) throws SQLException {
        String sql = "SELECT COUNT(*) as count FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ?";
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setString(1, tableName);
            ResultSet resultSet = statement.executeQuery();
            if (resultSet.next()) {
                int count = resultSet.getInt("count");
                return count > 0;
            }
        }
        return false;
    }

    public void close()  {
        try {
            DatabaseConnector.getInstance().disconnect();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}
