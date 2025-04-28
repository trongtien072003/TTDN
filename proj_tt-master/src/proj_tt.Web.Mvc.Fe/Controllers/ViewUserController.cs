using Microsoft.AspNetCore.Mvc;
using proj_tt.Controllers;

namespace proj_tt.Web.Controllers
{
    public class ViewUserController : proj_ttControllerBase
    {
        public ViewUserController()
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    
    
    }
}
