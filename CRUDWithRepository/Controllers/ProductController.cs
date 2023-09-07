using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUDWithRepository.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAll();
            return View(products);
        }

        [HttpGet]
        public async Task <IActionResult> CreateorEdit(int Id = 0)
        {
            if (Id == 0) 
            {
                return View(new Product());
            }
            else
            {
                try
                {
                    Product product = await _productRepo.GetById(Id);
                    if (product != null)
                    {
                        return View(product);
                    }
                   

                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] =ex.Message;
                    return RedirectToAction("Index");

                }
                TempData["errorMessage"] = $"Product details not found in this Id: {Id}";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public async Task <IActionResult> CreateorEdit(Product model) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id==0)
                    {
                        await _productRepo.Add(model);
                        TempData["successMessage"] = "Product Inserted.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        await _productRepo.Update(model);
                        TempData["successMessage"] = "Product details updated Successfully.";
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["errorMessage"] = "Invalid model state.";
                    return View();
                }
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
           
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                Product product = await _productRepo.GetById(id);
                if (product != null)
                {
                    return View(product);
                }
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
            TempData["errorMessage"] = $"Product details not found with Id:{id}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id) 
        {
            try
            {
                await _productRepo.Delete(id);
                TempData["successMessage"] = "Product Deleted.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

    }
}
