using MySql.Data.MySqlClient;
using SonicXBackendC.DataBase;
using SonicXBackendC.MusicManagment;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace SonicXBackendC.MusicManagment
{
    public class MusicFileDAO
    {
        private MySqlConnection connection;
        private readonly string path = "Library" + Path.DirectorySeparatorChar;

        public MusicFileDAO()
        {
            try
            {
                connection = DatabaseConnector.GetInstance().GetConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Veritabanı bağlantısı oluşturulurken hata oluştu: " + e.Message);
            }
        }

        public void AddMusicFile(MusicFile musicFile)
        {
            string sql = "INSERT INTO music_files (title, artist, album, duration, path) VALUES (@Title, @Artist, @Album, @Duration, @Path)";
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Title", musicFile.Title);
                    command.Parameters.AddWithValue("@Artist", musicFile.Artist);
                    command.Parameters.AddWithValue("@Album", musicFile.Album);
                    command.Parameters.AddWithValue("@Duration", musicFile.Duration);
                    command.Parameters.AddWithValue("@Path", path + musicFile.Title + "_" + musicFile.Artist + "_" + musicFile.Album + ".mp3");
                    command.ExecuteNonQuery();

                    Console.WriteLine("Müzik dosyası eklendi: " + musicFile);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Müzik dosyası eklenirken hata oluştu: " + e.Message);
            }
        }


        public List<MusicFile> GetAllMusicFiles()
        {
            List<MusicFile> musicFiles = new List<MusicFile>();
            string sql = "SELECT id, title, artist, album, duration, path FROM music_files";
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString(reader.GetOrdinal("title"));
                            string artist = reader.GetString(reader.GetOrdinal("artist"));
                            string album = reader.GetString(reader.GetOrdinal("album"));
                            int duration = reader.GetInt32(reader.GetOrdinal("duration"));
                            string file = reader.GetString(reader.GetOrdinal("path"));
                            MusicFile musicFile = new MusicFile(title, artist, album, duration, file);
                            musicFiles.Add(musicFile);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Müzik dosyaları getirilirken hata oluştu: " + e.Message);
            }
            return musicFiles;
        }


        public void Close()
        {
            try
            {
                DatabaseConnector.GetInstance().Disconnect();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
