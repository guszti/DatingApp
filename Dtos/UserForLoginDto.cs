namespace DatingApp.API.Dtos
{
    public class UserForLoginDto
    {
        private string username;

        private string password;

        public string Username
        {
            get => username;
            set => username = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }
    }
}