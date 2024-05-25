package org.example;


import org.example.UserManagment.UserService;

import java.util.Scanner;

public class Main {

    public static void main(String[] args) {
        UserService userService = UserService.getInstance();
        Scanner scanner = new Scanner(System.in);

        while (true) {
            System.out.println("1. Giriş Yap");
            System.out.println("2. Kayıt Ol");
            System.out.println("3. E-posta Doğrulama");
            System.out.println("4. Çıkış Yap");
            System.out.println("5. Yeniden Giriş Yap");
            System.out.println("0. Çıkış");
            System.out.print("Seçiminizi yapın: ");
            int choice = scanner.nextInt();
            scanner.nextLine();  // Consume newline

            switch (choice) {
                case 1:
                    System.out.print("E-posta adresinizi girin: ");
                    String loginEmail = scanner.nextLine();
                    System.out.print("Şifrenizi girin: ");
                    String loginPassword = scanner.nextLine();
                    userService.login(loginEmail, loginPassword);
                    break;
                case 2:
                    System.out.println("\nHesap oluşturmak için bilgilerinizi girin:");
                    System.out.print("Adınız: ");
                    String firstName = scanner.nextLine();
                    System.out.print("Soyadınız: ");
                    String lastName = scanner.nextLine();
                    System.out.print("E-posta adresiniz: ");
                    String email = scanner.nextLine();
                    System.out.print("Şifreniz: ");
                    String password = scanner.nextLine();
                    userService.register(firstName, lastName, email, password);

                    System.out.print("\nE-posta doğrulama kodunu girin: ");
                    String verificationCode = scanner.nextLine();
                    userService.verifyEmail(email, verificationCode);
                    break;
                case 3:

                    break;
                case 4:
                    userService.logout();
                    break;
                case 5:
                    System.out.print("\nYeniden giriş yapmak için e-posta adresinizi ve şifrenizi girin: ");
                    String emailReLogin = scanner.nextLine();
                    String passwordReLogin = scanner.nextLine();
                    userService.login(emailReLogin, passwordReLogin);
                    break;
                case 0:
                    System.out.println("Çıkılıyor...");
                    scanner.close();
                    return;
                default:
                    System.out.println("Geçersiz seçim, lütfen tekrar deneyin.");
            }
        }
    }
}