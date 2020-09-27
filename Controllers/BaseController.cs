using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected DataContext context;
        
        public BaseController(DataContext context)
        {
            this.context = context;
        }
        
        public async Task<ActionResult<IEnumerable<T>>> IndexAction<T>() where T : class, IEntity
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task<ActionResult<T>> ShowAction<T>(int id) where T : class, IEntity
        {
            return await this.context.Set<T>().FirstOrDefaultAsync(
                o => o.Id == id
                );
        }

        public async Task<IActionResult> DeleteAction<T>(T entity) where T : class
        {
            this.context.Remove(entity);

            await this.context.SaveChangesAsync();

            return NoContent();
        }
    }
}