package org.example.DataBase;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class DatabaseConnector {

    private static final String JDBC_URL = "jdbc:mysql://localhost:3306/your_database";
    private static final String USERNAME = "root";
    private static final String PASSWORD = "#123456789#";

    private static DatabaseConnector instance;
    private Connection connection;

    private DatabaseConnector() throws SQLException {
        try {
            Class.forName("com.mysql.cj.jdbc.Driver");
            this.connection = DriverManager.getConnection(JDBC_URL, USERNAME, PASSWORD);
            System.out.println("Veritabanına bağlandı!");
        } catch (ClassNotFoundException e) {
            throw new SQLException("JDBC sürücüsü bulunamadı: " + e.getMessage());
        }
    }

    public static DatabaseConnector getInstance() throws SQLException {
        if (instance == null) {
            instance = new DatabaseConnector();
        } else if (instance.getConnection().isClosed()) {
            instance = new DatabaseConnector();
        }
        return instance;
    }

    public Connection getConnection() {
        return connection;
    }

    public void disconnect() {
        if (connection != null) {
            try {
                connection.close();
                System.out.println("Veritabanı bağlantısı kapatıldı.");
            } catch (SQLException e) {
                System.out.println("Veritabanı bağlantısı kapatılırken hata oluştu: " + e.getMessage());
            }
        }
    }
}
