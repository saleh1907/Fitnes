using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication4.Context;
using WebApplication4.Models;
using WebApplication4.ViewModels.TrainerViewModels;

namespace WebApplication4.Areas.admin.Controllers;
[Area("Admin")]
public class TrainersController(AppDbContext _context,IWebHostEnvironment _envireonment) : Controller
{
    public async Task<IActionResult> Index()
    {
        var trainer=await _context.Trainers.Select(x=>new TrainerGetVM
        {
            Name = x.Name,
            Description = x.Description,
            ImagePath = x.ImagePath,
            DepartamentName=x.Departament.Name
        }).ToListAsync();
        return View(trainer);
    }
    public async Task<IActionResult> Create()
    {
        await _sendDepartamentsWithvViewBag();
        return View();
    }

    private async Task _sendDepartamentsWithvViewBag()
    {
        var departaments = await _context.Departaments.Select(d => new SelectListItem()
        {

            Value = d.Id.ToString(),
            Text = d.Name,
        }).ToListAsync();

        ViewBag.Departaments = departaments;
    }

    [HttpPost]

    public async Task<IActionResult> Create(TrainerCreateVM vm)
    {
        await _sendDepartamentsWithvViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm); 
        }

        var isExitsDepartamen = await _context.Departaments.AnyAsync(x => x.Id == vm.DepartamentId);
        if (!isExitsDepartamen)
        {
            ModelState.AddModelError("DepartamentId", "This Departamen is not found");
            return View(vm);
        }

        if (vm.Image.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("Image", "Image's maximun size must be 2 mb");
            return View(vm);
        }
        if (!vm.Image.ContentType.Contains("image"))
        {
            ModelState.AddModelError("Image", "You can upload file in only image format");
            return View(vm);
        }

        string uniqueFileName = Guid.NewGuid().ToString() + vm.Image.FileName;

        string foldePath = Path.Combine(_envireonment.WebRootPath, "images");

        string path=Path.Combine(foldePath, uniqueFileName);


        using FileStream stream = new(path, FileMode.Create);

        vm.Image.CopyToAsync(stream);

        Trainer trainer = new()
        {
            Name = vm.Name,
            Description = vm.Description,
            DepartamenId = vm.DepartamentId,
            ImagePath=uniqueFileName

        };
        await _context.Trainers.AddAsync(trainer);

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }

}
