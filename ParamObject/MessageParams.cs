namespace DatingApp.API.ParamObject
{
    public class MessageParams : GridParamsDto
    {
        public int UserId { get; set; }

        public string Status { get; set; } = "Unread";
    }
}