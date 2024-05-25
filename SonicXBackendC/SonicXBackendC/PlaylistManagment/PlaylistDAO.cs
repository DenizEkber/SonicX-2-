using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SonicXBackendC.DataBase;
using SonicXBackendC.MusicManagment;
using SonicXBackendC.UserManagement;
using MySql.Data.MySqlClient;

namespace SonicXBackendC.PlaylistManagment
{
   
    public class PlaylistDAO
    {
        private readonly MySqlConnection connection;

        public PlaylistDAO()
        {
            try
            {
                this.connection = DatabaseConnector.GetInstance().GetConnection();
            }
            catch (Exception e)
            {
                throw new Exception("Veritabanı bağlantısı oluşturulurken hata oluştu: " + e.Message);
            }
        }

        public void AddMusicFileToPlaylist(string playlistName, MusicFile musicFile, UserService userService)
        {
            string sql = "INSERT INTO playlist_" + userService.GetUser().Id + " (playlist_name, title, artist, album, duration, path) VALUES (@PlaylistName, @Title, @Artist, @Album, @Duration, @Path)";
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PlaylistName", playlistName);
                    command.Parameters.AddWithValue("@Title", musicFile.Title);
                    command.Parameters.AddWithValue("@Artist", musicFile.Artist);
                    command.Parameters.AddWithValue("@Album", musicFile.Album);
                    command.Parameters.AddWithValue("@Duration", musicFile.Duration);
                    command.Parameters.AddWithValue("@Path", musicFile.Path);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Müzik dosyası çalma listesine eklendi: " + musicFile.Title);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Müzik dosyası çalma listesine eklenirken hata oluştu: " + e.Message);
            }
        }

        public void RemoveMusicFileFromPlaylist(string playlistName, string musicTitle, UserService userService)
        {
            string sql = "DELETE FROM playlist_files WHERE playlist_name = @PlaylistName AND title = @Title AND user_id = @UserId";
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PlaylistName", playlistName);
                    command.Parameters.AddWithValue("@Title", musicTitle);
                    command.Parameters.AddWithValue("@UserId", userService.GetUser().Id);
                    int rowsDeleted = command.ExecuteNonQuery();
                    if (rowsDeleted > 0)
                    {
                        Console.WriteLine("Müzik dosyası çalma listesinden silindi: " + musicTitle);
                    }
                    else
                    {
                        Console.WriteLine("Belirtilen müzik dosyası çalma listesinde bulunamadı: " + musicTitle);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Müzik dosyası çalma listesinden silinirken hata oluştu: " + e.Message);
            }
        }

        public List<MusicFile> GetMusicFilesInPlaylist(string playlistName, UserService userService)
        {
            List<MusicFile> musicFiles = new List<MusicFile>();
            string sql = "SELECT title, artist, album, duration, path FROM playlist_" + userService.GetUser().Id + " WHERE playlist_name = @PlaylistName";
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PlaylistName", playlistName);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString(reader.GetOrdinal("title"));
                            string artist = reader.GetString(reader.GetOrdinal("artist"));
                            string album = reader.GetString(reader.GetOrdinal("album"));
                            int duration = reader.GetInt32(reader.GetOrdinal("duration"));
                            string path = reader.GetString(reader.GetOrdinal("path"));
                            MusicFile musicFile = new MusicFile(title, artist, album, duration, path);
                            musicFiles.Add(musicFile);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Çalma listesi yenilenirken hata oluştu: " + e.Message);
            }
            return musicFiles;
        }

        public List<string> GetAllPlaylistNames(UserService userService)
        {
            List<string> playlistNames = new List<string>();
            string sql = "SELECT DISTINCT playlist_name FROM playlist_" + userService.GetUser().Id;
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string playlistName = reader.GetString(reader.GetOrdinal("playlist_name"));
                            playlistNames.Add(playlistName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Çalma listeleri getirilirken hata oluştu: " + e.Message);
            }
            return playlistNames;
        }

        public void Close()
        {
            try
            {
                DatabaseConnector.GetInstance().Disconnect();
            }
            catch (SqlException e)
            {
                throw new Exception("Veritabanı bağlantısı kapatılırken hata oluştu: " + e.Message);
            }
        }
    }
}
