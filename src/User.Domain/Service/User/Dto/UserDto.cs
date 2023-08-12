using User.SharedKernel.Utils.Enums;

namespace User.Domain.Service.User.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int IdCity { get; set; }
    }
}
