using DatingApp.API.Enum;

namespace DatingApp.API.Dtos
{
    public class UserParamsDto : GridParamsDto
    {
        public Gender Gender { get; set; }

        private int minAge = 18;

        private int maxAge = 150;

        public int MinAge { get; set; }

        public int MaxAge { get; set; }
    }
}