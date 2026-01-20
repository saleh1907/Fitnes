using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication4.Context;
using WebApplication4.Models;
using WebApplication4.ViewModels.TrainerViewModels;

namespace WebApplication4.Areas.admin.Controllers;
[Area("Admin")]
public class TrainersController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _folderPath;

    public TrainersController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _folderPath = Path.Combine(_environment.WebRootPath, "images");
    }

    public async Task<IActionResult> Index()
    {
        var trainer=await _context.Trainers.Select(x=>new TrainerGetVM
        {
            Id=x.Id,
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
        if(!ModelState.IsValid) 
            return View(vm);

        var departament= await _context.Departaments.AnyAsync(x => x.Id == vm.DepartamentId);

        if(!departament )
        {
            ModelState.AddModelError("DepartamentId","This Departament is not found");
            return View(vm);
        }
        if (vm.Image.Length > 2 * 1024 * 1024)
        {

            ModelState.AddModelError("image", "seklin olcusu 2 mbden cox ola bilmez");
            return View(vm);
        }
        if (!vm.Image.ContentType.Contains("image"))
        {
            ModelState.AddModelError("image", "ancaq sekil formatinda data daxil ede bilersiz");
            return View(vm);
        }

        string uniqueFileName=Guid.NewGuid().ToString()+vm.Image.FileName;
        //string folderPath = Path.Combine(_environment.WebRootPath, "images");
        
        string path=Path.Combine(_folderPath, uniqueFileName);


        using FileStream stream = new(path, FileMode.Create);

        await vm.Image.CopyToAsync(stream);

      

       


        Trainer trainer = new()
        {
            Name = vm.Name,
            Description = vm.Description,
            DepartamenId = vm.DepartamentId,
            ImagePath =uniqueFileName,
        };

        await _context.Trainers.AddAsync(trainer);

        await _context.SaveChangesAsync();


        return RedirectToAction("Index");

    }

    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
            return View();

        var trainer= await _context.Trainers.FindAsync(id);
        if (trainer is null)
            return NotFound();

        _context.Trainers.Remove(trainer);
        await _context.SaveChangesAsync();


        string deleteImagePath = Path.Combine(_folderPath, trainer.ImagePath);

        if(System.IO.File.Exists(deleteImagePath))
        System.IO.File.Delete(deleteImagePath);

        return RedirectToAction(nameof(Index));


    }

}
