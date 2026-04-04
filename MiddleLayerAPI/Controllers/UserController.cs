using Microsoft.AspNetCore.Mvc;

namespace MiddleLayerAPI.Controllers
{
    public class UserController : Controller
    {
        private AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context; 
        }


   
    }
}
