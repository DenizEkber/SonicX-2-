using System;
using System.Collections.Generic;
using SonicXBackendC.DataBase;
using System.Data.SqlClient;
using SonicXBackendC.UserManagement;
using MySql.Data.MySqlClient;

namespace SonicXBackendC.UserManagement
{
    public class UserDAO
    {
        private readonly MySqlConnection connection;

        public UserDAO()
        {
            try
            {
                this.connection = DatabaseConnector.GetInstance().GetConnection();
            }
            catch (MySqlException e)
            {
                throw new Exception("Veritabanı işlemi sırasında bir hata oluştu.", e);
            }
        }


        public bool AddUser(User user)
        {
            if (!IsTableExists())
            {
                CreateUsersTable();
                return AddFirstUser(user);
            }
            else
            {
                return AddRegularUser(user);
            }
        }

        public void CreateUsersTable()
        {
            string sql = "CREATE TABLE users (" +
                         "id INT PRIMARY KEY IDENTITY," + // AUTO_INCREMENT PRIMARY KEY yerine PRIMARY KEY IDENTITY kullanıldı
                         "first_name VARCHAR(50)," +
                         "last_name VARCHAR(50)," +
                         "email VARCHAR(100) UNIQUE," +
                         "password VARCHAR(100)," +
                         "salt VARCHAR(100)" +
                         ")";
            try
            {
                
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                       
                        command.ExecuteNonQuery();
                        Console.WriteLine("Users table created successfully.");
                    }
                
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error creating users table: " + e.Message);
            }
        }


        public bool IsTableExists()
        {
            string sql = "SELECT COUNT(*) as count FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";
            try
            {
                
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", "users");
                        
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int count = reader.GetInt32(0);
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


        private bool AddRegularUser(User user)
        {
            string sql = "INSERT INTO users (first_name, last_name, email, password, salt) VALUES (@FirstName, @LastName, @Email, @Password, @Salt)";
            string salt = PasswordUtils.GetSalt();
            string hashedPassword = PasswordUtils.HashPassword(user.Password, salt);

            
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Salt", salt);

                    try
                    {
                        
                        int rowsInserted = command.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            Console.WriteLine("User added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add user.");
                        }
                        return rowsInserted > 0;
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine("Error adding user: " + e.Message);
                        return false;
                    }
                }
            
        }


        public bool AddFirstUser(User user)
        {
            string sql = "INSERT INTO users (id, first_name, last_name, email, password, salt) VALUES (@Id, @FirstName, @LastName, @Email, @Password, @Salt)";
            string salt = PasswordUtils.GetSalt();
            string hashedPassword = PasswordUtils.HashPassword(user.Password, salt);

            
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", 100000); // İlk kullanıcı için özel bir ID değeri
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Salt", salt);

                    try
                    {
                        connection.Open();
                        int rowsInserted = command.ExecuteNonQuery();
                        return rowsInserted > 0;
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine("Error adding first user: " + e.Message);
                        return false;
                    }
                }
            
        }


        public User GetUserByEmail(string email)
    {
        string sql = "SELECT id, first_name, last_name, password, salt FROM users WHERE email = @Email";

        
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        string password = reader.GetString(3);
                        string salt = reader.GetString(4);

                        return new User(id, firstName, lastName, email, password, salt);
                    }
                }
            
        }

        return null;
    }

        // Diğer metotlar buraya eklenecek

        public void Close()
        {
            try
            {
                DatabaseConnector.GetInstance().Disconnect();
            }
            catch (MySqlException e)
            {
                throw new Exception("Veritabanı işlemi sırasında bir hata oluştu.", e);
            }
        }
    }
}
