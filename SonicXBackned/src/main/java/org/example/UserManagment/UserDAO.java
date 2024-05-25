package org.example.UserManagement;
import org.example.UserManagement.User;

import org.example.DataBase.DatabaseConnector;
import org.example.UserManagment.PasswordUtils;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.sql.DatabaseMetaData;

public class UserDAO {
    private final Connection connection;

    public UserDAO() {
        try {
            this.connection = DatabaseConnector.getInstance().getConnection();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }



    public boolean addUser(User user) {
        if (!isTableExists()) {
            createUsersTable();
            boolean result = addFirstUser(user);
            return result;
        } else {
            boolean result = addRegularUser(user);
            return result;
        }
    }

    public void createUsersTable() {
        String sql = "CREATE TABLE users (" +
                "id INT AUTO_INCREMENT PRIMARY KEY," +
                "first_name VARCHAR(50)," +
                "last_name VARCHAR(50)," +
                "email VARCHAR(100) UNIQUE," +
                "password VARCHAR(100)," +
                "salt VARCHAR(100)" +
                ")";
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.executeUpdate();
            System.out.println("Users table created successfully.");
        } catch (SQLException e) {
            System.out.println("Error creating users table: " + e.getMessage());
        }
    }

    public boolean isTableExists() {
        String sql = "SELECT COUNT(*) as count FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ?";
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setString(1, "users");
            ResultSet resultSet = statement.executeQuery();
            if (resultSet.next()) {
                int count = resultSet.getInt("count");
                return count > 0;
            }
        } catch (SQLException e) {
            System.out.println("Error checking if table exists: " + e.getMessage());
        }
        return false;
    }

    private boolean addRegularUser(User user) {
        String sql = "INSERT INTO users (first_name, last_name, email, password, salt) VALUES (?, ?, ?, ?, ?)";
        String salt = PasswordUtils.getSalt();
        String hashedPassword = PasswordUtils.hashPassword(user.getPassword(), salt);
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setString(1, user.getFirstName());
            statement.setString(2, user.getLastName());
            statement.setString(3, user.getEmail());
            statement.setString(4, hashedPassword);
            statement.setString(5, salt);
            int rowsInserted = statement.executeUpdate();
            if (rowsInserted > 0) {
                System.out.println("User added successfully.");
            } else {
                System.out.println("Failed to add user.");
            }
            return rowsInserted > 0;
        } catch (SQLException e) {
            System.out.println("Error adding user: " + e.getMessage());
            return false;
        }

    }

    public boolean addFirstUser(User user) {

        String sql = "INSERT INTO users (id, first_name, last_name, email, password, salt) VALUES (?, ?, ?, ?, ?, ?)";
        String salt = PasswordUtils.getSalt();
        String hashedPassword = PasswordUtils.hashPassword(user.getPassword(), salt);
        try (PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setInt(1, 100000); // İlk kullanıcı için özel bir ID değeri
            statement.setString(2, user.getFirstName());
            statement.setString(3, user.getLastName());
            statement.setString(4, user.getEmail());
            statement.setString(5, hashedPassword);
            statement.setString(6, salt);
            int rowsInserted = statement.executeUpdate();
            return rowsInserted > 0;
        } catch (SQLException e) {
            System.out.println("Error adding first user: " + e.getMessage());
            return false;
        }
    }

        public User getUserByEmail(String email) {
            String sql = "SELECT * FROM users WHERE email = ?";
            try (PreparedStatement statement = connection.prepareStatement(sql)) {
                statement.setString(1, email);
                ResultSet resultSet = statement.executeQuery();
                if (resultSet.next()) {
                    int id = resultSet.getInt("id");
                    String firstName = resultSet.getString("first_name");
                    String lastName = resultSet.getString("last_name");
                    String password = resultSet.getString("password");
                    String salt = resultSet.getString("salt");
                    return new User(id, firstName, lastName, email, password, salt);
                }
            } catch (SQLException e) {
                System.out.println("Kullanıcı getirilirken hata oluştu: " + e.getMessage());
            }
            return null;
        }
        public boolean updateUser(User user) {
            String sql = "UPDATE users SET first_name = ?, last_name = ?, email = ?, password = ? WHERE id = ?";
            try (PreparedStatement statement = connection.prepareStatement(sql)) {
                statement.setString(1, user.getFirstName());
                statement.setString(2, user.getLastName());
                statement.setString(3, user.getEmail());
                statement.setString(4, user.getPassword());
                statement.setInt(5, user.getId());
                int rowsUpdated = statement.executeUpdate();
                return rowsUpdated > 0;
            } catch (SQLException e) {
                System.out.println("Kullanıcı güncellenirken hata oluştu: " + e.getMessage());
                return false;
            }
        }

        public void deleteUser ( int id){
            String sql = "DELETE FROM users WHERE id = ?";
            try (PreparedStatement statement = connection.prepareStatement(sql)) {
                statement.setInt(1, id);
                int rowsDeleted = statement.executeUpdate();
                if (rowsDeleted > 0) {
                    System.out.println("Kullanıcı silindi: ID=" + id);
                } else {
                    System.out.println("Belirtilen kullanıcı bulunamadı: ID=" + id);
                }
            } catch (SQLException e) {
                System.out.println("Kullanıcı silinirken hata oluştu: " + e.getMessage());
            }
        }

        public List<User> getAllUsers () {
            List<User> userList = new ArrayList<>();
            String sql = "SELECT * FROM users";
            try (PreparedStatement statement = connection.prepareStatement(sql)) {
                ResultSet resultSet = statement.executeQuery();
                while (resultSet.next()) {
                    int id = resultSet.getInt("id");
                    String firstName = resultSet.getString("first_name");
                    String lastName = resultSet.getString("last_name");
                    String email = resultSet.getString("email");
                    String password = resultSet.getString("password");
                    String salt = resultSet.getString("salt");
                    userList.add(new User(id, firstName, lastName, email, password, salt));
                }
            } catch (SQLException e) {
                System.out.println("Kullanıcılar getirilirken hata oluştu: " + e.getMessage());
            }
            return userList;
        }

        public void close () {
            try {
                DatabaseConnector.getInstance().disconnect();
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }
        }
}
