using Microsoft.AspNetCore.Mvc;
using Maksiupedia.Models;
using System.Text.Json;

namespace Maksiupedia.Controllers
{
    public class PhotosController : Controller
    {
        public IActionResult Index()
        {
            var json = System.IO.File.ReadAllText("Data/photos.json");
            var photos = JsonSerializer.Deserialize<List<Photo>>(json);
            return View(photos);
        }
    }
}
