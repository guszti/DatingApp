namespace DatingApp.API.Model
{
    public interface IUser
    {
        public int Id { get; }
        
        public string Username { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }
    }
}