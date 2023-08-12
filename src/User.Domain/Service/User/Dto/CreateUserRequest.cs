using User.SharedKernel.Utils.Enums;

namespace User.Domain.Service.User.Dto
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int IdCity { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
