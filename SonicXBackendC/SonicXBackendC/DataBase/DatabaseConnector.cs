using System;
using MySql.Data.MySqlClient;

namespace SonicXBackendC.DataBase
{
    public class DatabaseConnector
    {
        private static readonly string ConnectionString = "Server=localhost;Port=3306;Database=your_database;Uid=root;Pwd=your_password";
        private static DatabaseConnector instance;
        private MySqlConnection connection;

        private DatabaseConnector()
        {
            try
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
                Console.WriteLine("Veritabanına bağlandı!");
            }
            catch (MySqlException e)
            {
                throw new Exception("MySQL bağlantı hatası: " + e.Message);
            }
        }

        public static DatabaseConnector GetInstance()
        {
            if (instance == null || instance.connection.State == System.Data.ConnectionState.Closed)
            {
                instance = new DatabaseConnector();
            }
            return instance;
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }

        public void Disconnect()
        {
            if (connection != null)
            {
                try
                {
                    connection.Close();
                    Console.WriteLine("Veritabanı bağlantısı kapatıldı.");
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("Veritabanı bağlantısı kapatılırken hata oluştu: " + e.Message);
                }
            }
        }
    }
}
