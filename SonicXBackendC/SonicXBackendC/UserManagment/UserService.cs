using System;
using System.Collections.Generic;
using SonicXBackendC.UserManagement;

namespace SonicXBackendC.UserManagement
{
    public class UserService
    {
        private static UserService instance;
        private User currentUser;
        private UserDAO userDAO;
        private EmailSender emailSender;

        private readonly Dictionary<string, User> pendingUsers;
        private readonly Dictionary<string, string> verificationCodes;

        private UserService()
        {
            userDAO = new UserDAO();
            emailSender = EmailSender.GetInstance();
            verificationCodes = new Dictionary<string, string>();
            pendingUsers = new Dictionary<string, User>();
        }

        public User GetUser()
        {
            return currentUser;
        }

        public static UserService GetInstance()
        {
            if (instance == null)
            {
                instance = new UserService();
            }
            return instance;
        }

        public bool Login(string email, string password)
        {
            User user = userDAO.GetUserByEmail(email);
            if (user != null)
            {
                string hashedPassword = PasswordUtils.HashPassword(password, user.Salt);
                if (user.Password.Equals(hashedPassword))
                {
                    currentUser = user;
                    Console.WriteLine("Giriş başarılı. Hoş geldiniz, " + currentUser.FirstName + " " + currentUser.LastName);
                    return true;
                }
            }
            Console.WriteLine("Hatalı e-posta veya şifre. Lütfen tekrar deneyin.");
            return false;
        }

        public bool Register(string firstName, string lastName, string email, string password)
        {
            Console.WriteLine("Register started");
            if (userDAO.GetUserByEmail(email) != null)
            {
                Console.WriteLine("Bu e-posta adresi zaten kullanımda.");
                return false;
            }

            if (userDAO.IsTableExists())
            {
                userDAO.CreateUsersTable();
            }

            string verificationCode = GenerateVerificationCode();

            verificationCodes.Add(email, verificationCode);
            pendingUsers.Add(email, new User(0, firstName, lastName, email, password));

            emailSender.SendVerificationEmail(email, verificationCode);
            Console.WriteLine("Doğrulama kodu gönderildi. Lütfen e-posta adresinizi doğrulayın.");
            return true;
        }

        private bool AddFirstUser(string firstName, string lastName, string email, string password)
        {
            User newUser = new User(100000, firstName, lastName, email, password);
            bool success = userDAO.AddUser(newUser);
            if (success)
            {
                Console.WriteLine("Kullanıcı eklendi. ID: " + newUser.Id);
                return true;
            }
            else
            {
                Console.WriteLine("Kullanıcı eklenirken bir hata oluştu.");
                return false;
            }
        }

        public bool VerifyEmail(string email, string code)
        {
            if (verificationCodes.TryGetValue(email, out string correctCode) && correctCode.Equals(code))
            {
                User newUser = pendingUsers[email];
                bool success = userDAO.AddUser(newUser);
                if (success)
                {
                    Console.WriteLine("E-posta doğrulaması başarılı. Hesap aktifleştirildi.");
                    verificationCodes.Remove(email); // Kod doğrulandığı için kaldır
                    pendingUsers.Remove(email); // Geçici kullanıcıyı kaldır
                    return true;
                }
                else
                {
                    Console.WriteLine("Hesap oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Geçersiz doğrulama kodu. Lütfen doğru kodu girin.");
                return false;
            }
        }

        public void Logout()
        {
            currentUser = null;
            Console.WriteLine("Çıkış yapıldı. Lütfen tekrar giriş yapın.");
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            int code = 1000 + random.Next(9000);
            return code.ToString();
        }
    }
}
