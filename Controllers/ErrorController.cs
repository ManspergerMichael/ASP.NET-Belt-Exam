using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    public IActionResult Default()
    {
        return View("~/Views/Shared/Errors/Default.cshtml");
    }
}
