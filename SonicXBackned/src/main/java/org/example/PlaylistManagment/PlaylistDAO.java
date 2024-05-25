package org.example.PlaylistManagment;

import org.example.DataBase.DatabaseConnector;
import org.example.MusicManagment.MusicFile;
import org.example.UserManagement.User;
import org.example.UserManagment.UserService;

import javax.management.RuntimeErrorException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class PlaylistDAO {

    private final Connection connection;

    public PlaylistDAO() throws SQLException {
        this.connection = DatabaseConnector.getInstance().getConnection();
    }

    public void addMusicFileToPlaylist(String playlistName, MusicFile musicFile, UserService userService) throws SQLException {
        String sql = "INSERT INTO playlist_"+userService.getUser().getId() +"(playlist_name, title, artist, album, duration, path) VALUES (?, ?, ?, ?, ?, ?)";
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setString(1, playlistName);
            statement.setString(2, musicFile.getTitle());
            statement.setString(3, musicFile.getArtist());
            statement.setString(4, musicFile.getAlbum());
            statement.setInt(5, musicFile.getDuration());
            statement.setString(6, musicFile.getPath());
            statement.executeUpdate();
            System.out.println("Müzik dosyası çalma listesine eklendi: " + musicFile.getTitle());
        }
    }

    public void removeMusicFileFromPlaylist(String playlistName, String musicTitle, UserService userService) throws SQLException {
        String sql = "DELETE FROM playlist_files WHERE playlist_name = ? AND title = ? AND user_id = ?";
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setString(1, playlistName);
            statement.setString(2, musicTitle);
            statement.setInt(3, userService.getUser().getId());
            int rowsDeleted = statement.executeUpdate();
            if (rowsDeleted > 0) {
                System.out.println("Müzik dosyası çalma listesinden silindi: " + musicTitle);
            } else {
                System.out.println("Belirtilen müzik dosyası çalma listesinde bulunamadı: " + musicTitle);
            }
        }
    }

    public List<MusicFile> getMusicFilesInPlaylist(String playlistName, UserService userService) throws SQLException {
        List<MusicFile> musicFiles = new ArrayList<>();
        String sql = "SELECT title, artist, album, duration, path FROM playlist_"+ userService.getUser().getId()+" WHERE playlist_name = ?";
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setString(1, playlistName);
            ResultSet resultSet = statement.executeQuery();
            while (resultSet.next()) {
                String title = resultSet.getString("title");
                String artist = resultSet.getString("artist");
                String album = resultSet.getString("album");
                int duration = resultSet.getInt("duration");
                String file=resultSet.getString("path");
                MusicFile musicFile = new MusicFile(title, artist, album, duration,file);
                musicFiles.add(musicFile);

            }
        } catch (SQLException e) {
            System.out.println("Çalma listesi yenilenirken hata oluştu: " + e.getMessage());
        }

        return musicFiles;
    }

    public List<String> getAllPlaylistNames(UserService userService) throws SQLException {
        List<String> playlistNames = new ArrayList<>();
        String sql = "SELECT DISTINCT playlist_name FROM playlist_" + userService.getUser().getId();
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            ResultSet resultSet = statement.executeQuery();
            while (resultSet.next()) {
                String playlistName = resultSet.getString("playlist_name");
                playlistNames.add(playlistName);
            }
            resultSet.close();
        }catch (Exception e){
            System.out.println(e.getMessage());
        }
        return playlistNames;
    }

    public void close()  {
        try {
            DatabaseConnector.getInstance().disconnect();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}
