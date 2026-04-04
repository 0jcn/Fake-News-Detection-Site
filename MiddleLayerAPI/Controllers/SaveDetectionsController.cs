using Microsoft.AspNetCore.Mvc;

namespace MiddleLayerAPI.Controllers
{
    public class SaveDetectionsController : Controller
    {
        private AppDbContext _context;
        public SaveDetectionsController(AppDbContext context)
        {
            _context = context;
        }
    }
}
