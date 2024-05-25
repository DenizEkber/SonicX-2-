package org.example.UserManagment;

import org.example.UserManagement.User;
import org.example.UserManagement.UserDAO;

import java.util.HashMap;
import java.util.Map;
import java.util.Random;

public class UserService {
    private static UserService instance;
    private User currentUser;
    private UserDAO userDAO;
    private EmailSender emailSender;

    private Map<String, User> pendingUsers;

    private Map<String, String> verificationCodes;

    private UserService() {
        userDAO = new UserDAO();
        emailSender = EmailSender.getInstance();
        verificationCodes = new HashMap<>();
        pendingUsers = new HashMap<>();
    }

    public User getUser() {
        return currentUser;
    }

    public static UserService getInstance() {
        if (instance == null) {
            instance = new UserService();
        }
        return instance;
    }

    public boolean login(String email, String password) {
        User user = userDAO.getUserByEmail(email);
        if (user != null) {
            String hashedPassword = PasswordUtils.hashPassword(password, user.getSalt());
            if (user.getPassword().equals(hashedPassword)) {
                currentUser = user;
                System.out.println("Giriş başarılı. Hoş geldiniz, " + currentUser.getFirstName() + " " + currentUser.getLastName());
                return true;
            }
        }
        System.out.println("Hatalı e-posta veya şifre. Lütfen tekrar deneyin.");
        return false;
    }

    public boolean register(String firstName, String lastName, String email, String password) {
        if (userDAO.getUserByEmail(email) != null) {
            System.out.println("Bu e-posta adresi zaten kullanımda.");
            return false;
        }

        if (userDAO.isTableExists()) {
            userDAO.createUsersTable();
            String verificationCode = generateVerificationCode();

            verificationCodes.put(email, verificationCode);
            pendingUsers.put(email, new User(0, firstName, lastName, email, password));

            emailSender.sendVerificationEmail(email, verificationCode);
            System.out.println("Doğrulama kodu gönderildi. Lütfen e-posta adresinizi doğrulayın.");
            return true;
        } else {

            String verificationCode = generateVerificationCode();

            verificationCodes.put(email, verificationCode);
            pendingUsers.put(email, new User(0, firstName, lastName, email, password));

            emailSender.sendVerificationEmail(email, verificationCode);
            System.out.println("Doğrulama kodu gönderildi. Lütfen e-posta adresinizi doğrulayın.");
            return true;
        }
    }

    private boolean addFirstUser(String firstName, String lastName, String email, String password) {

        User newUser = new User(100000, firstName, lastName, email, password);
        boolean success = userDAO.addUser(newUser);
        if (success) {
            System.out.println("Kullanıcı eklendi. ID: " + newUser.getId());
            return true;
        } else {
            System.out.println("Kullanıcı eklenirken bir hata oluştu.");
            return false;
        }
    }

    public boolean verifyEmail(String email, String code) {
        String correctCode = verificationCodes.get(email);
        System.out.println(correctCode);
        if (correctCode != null && correctCode.equals(code)) {

            User newUser = pendingUsers.get(email);
            boolean success = userDAO.addUser(newUser);
            if (success) {
                System.out.println("E-posta doğrulaması başarılı. Hesap aktifleştirildi.");
                verificationCodes.remove(email); // Kod doğrulandığı için kaldır
                pendingUsers.remove(email); // Geçici kullanıcıyı kaldır
                return true;
            } else {
                System.out.println("Hesap oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");
                return false;
            }
        } else {
            System.out.println("Geçersiz doğrulama kodu. Lütfen doğru kodu girin.");
            return false;
        }
    }

    public void logout() {
        currentUser = null;
        System.out.println("Çıkış yapıldı. Lütfen tekrar giriş yapın.");
    }

    private String generateVerificationCode() {
        Random random = new Random();
        int code = 1000 + random.nextInt(9000);
        return String.valueOf(code);
    }
}
