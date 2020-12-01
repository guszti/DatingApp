using AutoMapper;
using DatingApp.API.Repository;

namespace DatingApp.API.Controllers
{
    public class MessageController : BaseController
    {
        public MessageController(IBaseRepository baseRepositoryInterface, IMapper mapper)
            : base(baseRepositoryInterface, mapper)
        {
        }
    }
}