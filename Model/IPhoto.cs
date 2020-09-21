namespace DatingApp.API.Model
{
    public interface IPhoto : IEntity
    {
        public string Url { get; set; }
        
        public bool IsMain { get; set; }
        
        public string Description { get; set; }
        
        public int UserId { get; set; }
        
        public User User { get; set; }
    }
}