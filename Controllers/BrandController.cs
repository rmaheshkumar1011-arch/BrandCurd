using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.web.models;

namespace WebApp.Controllers;

public class BrandController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        List<Brand> brands = _dbContext.Brand.ToList();
        return View(brands);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Brand brand)
    {
        String webRootPath = _webHostEnvironment.WebRootPath;

        var files = HttpContext.Request.Form.Files;
        if(files.Count > 0)
        {
           String newFilename = Guid.NewGuid().ToString();
           var uploads = Path.Combine(webRootPath, @"images\brand");
           var extenstion = Path.GetExtension(files[0].FileName);
           using(var filestream = new FileStream(Path.Combine(uploads,newFilename+extenstion), FileMode.Create))
           {
                files[0].CopyTo(filestream);
           }
           brand.BrandLogo=@"\images\brand\" + newFilename + extenstion;
        }
        if (ModelState.IsValid)
        {
            _dbContext.Brand.Add(brand);
            _dbContext.SaveChanges();

            TempData["success"] = "Brand created successfully";

            return RedirectToAction( nameof(Index));
        }
        return View();
    }
    [HttpGet]
    public IActionResult Details(Guid id)
    {
        Brand brand= _dbContext.Brand.FirstOrDefault(x => x.ID ==id);
        return View(brand);
    }
    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        Brand brand= _dbContext.Brand.FirstOrDefault(x => x.ID ==id);
        return View(brand);
    }
    [HttpPost]
    public IActionResult Edit(Brand brand)
    {
        String webRootPath = _webHostEnvironment.WebRootPath;

        var files = HttpContext.Request.Form.Files;
        if(files.Count > 0)
        {
           String newFilename = Guid.NewGuid().ToString();
           var uploads = Path.Combine(webRootPath, @"images\brand");
           var extenstion = Path.GetExtension(files[0].FileName);

           //delete old image
           var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.ID == brand.ID);

        if(objFromDb != null)
            {

           var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.TrimStart('\\'));
           if(System.IO.File.Exists(oldImagePath))
           {
                System.IO.File.Delete(oldImagePath);
           }
            }

           using(var filestream = new FileStream(Path.Combine(uploads,newFilename+extenstion), FileMode.Create))
           {
                files[0].CopyTo(filestream);
           }
           brand.BrandLogo=@"\images\brand\" + newFilename + extenstion;
        }
        if(ModelState.IsValid)
        {
            var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.ID == brand.ID);
            objFromDb.Name=brand.Name;
            objFromDb.EstablishedYear=brand.EstablishedYear;
            if(brand.BrandLogo != null)
            {
                objFromDb.BrandLogo=brand.BrandLogo;
            }

            _dbContext.Brand.Update(objFromDb);
            _dbContext.SaveChanges();
            TempData["warning"] = "Brand updated successfully";


            return RedirectToAction(nameof(Index));
        }
        return View(brand);
    }
    [HttpGet]
     public IActionResult Delete(Guid id)
    {
            Brand brand= _dbContext.Brand.FirstOrDefault(x => x.ID ==id);
            return View(brand);
    }

   [HttpPost]
    public IActionResult Delete(Brand brand)
    {
        string webRootpath = _webHostEnvironment.WebRootPath;
        if(!string.IsNullOrEmpty(webRootpath))
        {
            var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.ID == brand.ID);

           if(objFromDb != null)
           {

              var oldImagePath = Path.Combine(webRootpath, objFromDb.BrandLogo.TrimStart('\\'));
              if(System.IO.File.Exists(oldImagePath))
               {
                System.IO.File.Delete(oldImagePath);
               }
            }
            
        }
         _dbContext.Brand.Remove(brand);
         _dbContext.SaveChanges();

         TempData["error"] = "Brand deleted successfully";

         return RedirectToAction(nameof(Index));
    }
}
    

    



