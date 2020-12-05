using DatingApp.API.Enum;

namespace DatingApp.API
{
    public class UserParamsDto : GridParamsDto
    {
        public Gender Gender { get; set; }

        public int MinAge { get; set; } = 18;

        public int MaxAge { get; set; } = 150;

        public string SortBy { get; set; } = "lastActive";
    }
}