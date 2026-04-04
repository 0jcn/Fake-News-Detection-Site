using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly AppSettings _appSettings;

        public UserController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }



    }
}
