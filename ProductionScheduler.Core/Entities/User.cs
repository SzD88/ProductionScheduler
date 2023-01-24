namespace ProductionScheduler.Core.Entities
{
    public class User
    {
     
        // #refactor primitives 
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public string Password   { get; private set; }
        public string FullName { get; private set; }
        public string Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public User(Guid id, string email, string userName, string password, string fullName, string role, DateTime createdAt)
        {
            Id = id;
            Email = email;
            UserName = userName;
            Password = password;
            FullName = fullName;
            Role = role;
            CreatedAt = createdAt;
        }
    }
}
