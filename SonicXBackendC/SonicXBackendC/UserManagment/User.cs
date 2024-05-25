namespace SonicXBackendC.UserManagement
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public User(int id, string firstName, string lastName, string email, string password)
            : this(id, firstName, lastName, email, password, null) { }

        public User(int id, string firstName, string lastName, string email, string password, string salt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Salt = salt;
        }
    }
}
