namespace User.Domain.Common.Generators
{
    public class Generator : IGenerator
    {
        public string Generate() => Guid.NewGuid().ToString("N");
    }
}
