namespace DatingApp.API.Model
{
    public interface IUserRole
    {
        public int Id { get; set; }
        
        public User User { get; set; }
        
        public Role Role { get; set; }
    }
}