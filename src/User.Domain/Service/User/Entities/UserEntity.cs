namespace User.Domain.Service.User.Entities
{
    public class UserEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdCity { get; set; }
    }
}
