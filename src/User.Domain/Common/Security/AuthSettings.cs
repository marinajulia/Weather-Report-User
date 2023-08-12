namespace User.Domain.Common.Security
{
    public class AuthSettings
    {
        public string Secret { get; set; }
        public int ExpireIn { get; set; }
    }
}
