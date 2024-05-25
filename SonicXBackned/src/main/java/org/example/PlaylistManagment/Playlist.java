package org.example.PlaylistManagment;

import org.example.MusicManagment.MusicFile;
import org.example.UserManagment.UserService;

import java.sql.SQLException;
import java.util.List;

public class Playlist {
    private String name;
    private PlaylistDAO playlistDAO;

    public Playlist(String name) {
        this.name = name;
        try {
            this.playlistDAO = new PlaylistDAO();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public String getName() {
        return name;
    }

    public void addMusicFileToPlaylist(MusicFile musicFile, UserService userService) {
        try {
            playlistDAO.addMusicFileToPlaylist(this.name, musicFile,userService);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public void removeMusicFileFromPlaylist(String musicTitle,UserService userService) {
        try {
            playlistDAO.removeMusicFileFromPlaylist(this.name, musicTitle,userService);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public List<MusicFile> getAllMusicFiles(UserService userService) {
        try {
            return playlistDAO.getMusicFilesInPlaylist(this.name,userService);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public void close() {
        playlistDAO.close();
    }

}
