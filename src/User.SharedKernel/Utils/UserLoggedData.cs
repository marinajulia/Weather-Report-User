using User.SharedKernel.Utils.Enums;

namespace User.SharedKernel.Utils
{
    public class UserLoggedData
    {
        private readonly List<DataToken> _data;
        public UserLoggedData()
        {
            _data = new List<DataToken>();
        }

        public void Add(int idUsuario, UserProfileEnum userProfile)
        {
            _data.Add(new DataToken
            {
                Id_Usuario = idUsuario,
                UserProfile = userProfile,
            });
        }

        public DataToken GetData()
        {
            return _data.FirstOrDefault();
        }
    }

    public class DataToken
    {
        public int Id_Usuario { get; set; }
        public UserProfileEnum UserProfile { get; set; }
    }
}
