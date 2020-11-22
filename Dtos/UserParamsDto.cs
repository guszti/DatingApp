using DatingApp.API.Enum;

namespace DatingApp.API.Dtos
{
    public class UserParamsDto : GridParamsDto
    {
        public Gender Gender { get; set; }
    }
}