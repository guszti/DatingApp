namespace DatingApp.API.Model
{
    public class User : IUser
    {
        private int id;

        private string username;

        private byte[] password;

        private byte[] salt;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public byte[] Password
        {
            get => password;
            set => password = value;
        }

        public byte[] Salt
        {
            get => salt;
            set => salt = value;
        }
    }
}