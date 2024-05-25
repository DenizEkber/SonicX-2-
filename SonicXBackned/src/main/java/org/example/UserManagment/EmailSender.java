package org.example.UserManagment;

import javax.mail.*;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.util.Properties;

public class EmailSender {
    private final String host = "smtp.gmail.com";
    private final String port = "465";
    private final String username = "your_email";
    private final String password = "your_app_pass";

    private static EmailSender instance;

    private EmailSender() {
    }

    public static synchronized EmailSender getInstance() {
        if (instance == null) {
            instance = new EmailSender();
        }
        return instance;
    }

    public boolean sendVerificationEmail(String recipientEmail, String verificationCode) {
        Properties props = new Properties();
        props.put("mail.smtp.auth", "true");
        props.put("mail.smtp.starttls.enable", "true");
        props.put("mail.smtp.host", host);
        props.put("mail.smtp.port", port);
        props.put("mail.smtp.socketFactory.class", "javax.net.ssl.SSLSocketFactory");

        Session session = Session.getInstance(props, new Authenticator() {
            protected PasswordAuthentication getPasswordAuthentication() {
                return new PasswordAuthentication(username, password);
            }
        });

        try {
            Message message = new MimeMessage(session);
            message.setFrom(new InternetAddress(username));
            message.setRecipients(Message.RecipientType.TO, InternetAddress.parse(recipientEmail));
            message.setSubject("E-posta Doğrulama Kodu");
            message.setText("Salam,\n\nE-posta doğrulama kodunuz: " + verificationCode);

            Transport.send(message);
            System.out.println("Doğrulama kodu e-posta olarak gönderildi ");
            return true;
        } catch (MessagingException e) {
            System.out.println("E-posta gönderilirken hata oluştu: " + e.getMessage());
            return false;
        }
    }
}
