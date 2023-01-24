namespace ProductionScheduler.Application.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
    public class UserDetailsDto : UserDto
    {
        public string Email { get; set; }
    }
}
