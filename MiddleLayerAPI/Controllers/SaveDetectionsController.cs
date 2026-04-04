using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
    public class SaveDetectionsController : Controller
    {
        private readonly AppSettings _appSettings;
        public SaveDetectionsController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
    }
}
