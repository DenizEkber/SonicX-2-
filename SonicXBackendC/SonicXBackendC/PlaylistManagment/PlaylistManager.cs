using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using SonicXBackendC.DataBase;
using SonicXBackendC.UserManagement;

namespace SonicXBackendC.PlaylistManagement
{
    public class PlaylistManager
    {
        private readonly MySqlConnection connection;

        public PlaylistManager()
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

        public void CreatePlaylistTableForUser(UserService userService)
        {
            string tableName = "playlist_" + userService.GetUser().Id;
            if (IsTableExists(tableName))
            {
                Console.WriteLine("Playlist table for user " + userService.GetUser().FirstName + " already exists.");
                return;
            }

            string sql = "CREATE TABLE IF NOT EXISTS " + tableName +
                " (id INT PRIMARY KEY IDENTITY(1,1), " +
                "playlist_name VARCHAR(255), " +
                "title VARCHAR(255), " +
                "artist VARCHAR(255), " +
                "album VARCHAR(255), " +
                "duration INT, " +
                "path VARCHAR(300))";

            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Playlist table created for user: " + userService.GetUser().FirstName);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error creating playlist table: " + e.Message);
            }
        }

        public void DropPlaylistTableForUser(User user)
        {
            string tableName = "playlist_" + user.Id;
            if (!IsTableExists(tableName))
            {
                Console.WriteLine("Playlist table for user " + user.FirstName + " does not exist.");
                return;
            }

            string sql = "DROP TABLE " + tableName;

            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Playlist table dropped for user: " + user.FirstName);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error dropping playlist table: " + e.Message);
            }
        }

        public bool IsTableExists(string tableName)
        {
            string sql = "SELECT COUNT(*) as count FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int count = reader.GetInt32(reader.GetOrdinal("count"));
                            return count > 0;
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error checking if table exists: " + e.Message);
            }
            return false;
        }

        public void Close()
        {

            try
            {
                DatabaseConnector.GetInstance().Disconnect();
            }
            catch (MySqlException e)
            {
                throw new Exception("Veritabanı bağlantısı kapatılırken hata oluştu: " + e.Message);
            }
        }
    }
}
