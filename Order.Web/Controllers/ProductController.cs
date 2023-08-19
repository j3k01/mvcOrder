using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.DataAccess.DbContext;
using Order.DataAccess.Repositories.IRepositories;
using Order.Model.Models;
using System.Linq;

namespace Order.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, OrderDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(string searchString)
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            var displayedProducts = products.Take(10).ToList();
            return View(displayedProducts);
        }


        public IActionResult Upsert(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return View(new Product { MainImageId = 0 });
            }
            else
            {
                if (Id.HasValue)
                {
                    int validProductId = Id.Value;
                    var product = _unitOfWork.Product.GetProductWithImages(validProductId);
                    if (product.ProductImages != null && product.ProductImages.Any())
                    {
                        product.MainImageId = product.ProductImages.First().Id;
                    }

                    return View(product);
                }
                else
                {
                    throw new ArgumentNullException(nameof(Id), "Product Id cannot be null.");
                }
            }
        }
        [HttpPost]
        public IActionResult Upsert(Product product, List<IFormFile>? files, ProductImage image, int? MainImageId)
        {
            if (MainImageId == null || MainImageId == 0)
            {
                product.MainImageId = null;
            }
            else
            {
                product.MainImageId = MainImageId;
            }

            if (product.NetPrice == product.GrossPrice)
            {
                ModelState.AddModelError("NetPrice", "The NetPrice cannot exactly match the GrossPrice.");
            }

            if (!ModelState.IsValid)
            {
                double netPrice;
                double grossPrice;

                if (double.TryParse(product.NetPrice.ToString("F2"), out netPrice) && double.TryParse(product.GrossPrice.ToString("F2"), out grossPrice))
                {
                    product.NetPrice = netPrice;
                    product.GrossPrice = grossPrice;
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            string fileName = product.Name + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string createProductPath = Path.Combine(wwwRootPath, "Images", "Product", fileName);
                            string finalProductPath = Path.Combine("Images", "Product", fileName);
                            string url = finalProductPath.Replace("\\", "/");

                            using (var fileStream = new FileStream(createProductPath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }
                            ProductImage productImage = new()
                            {
                                ImageUrl = @"/" + finalProductPath.Replace("\\", "/"),
                                ProductId = product.Id
                            };

                            if (product.ProductImages == null)
                            {
                                product.ProductImages = new List<ProductImage>();
                            }

                            product.ProductImages.Add(productImage);
                        }
                        if(product.Id == 0)
                        {
                            _unitOfWork.Product.Add(product);
                            _unitOfWork.Save(); 

                        }
                        else
                        {
                            _unitOfWork.Product.Update(product);
                            _unitOfWork.Save(); 
                        }


                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("NetPrice", "Invalid Net Price");
                    ModelState.AddModelError("GrossPrice", "Invalid Gross Price");
                }
            }
            return View(product);
        }


        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                var productToUpdate = _unitOfWork.Product.Get(i => i.Id == product.Id);

                if (productToUpdate == null)
                {
                    return NotFound();
                }
                productToUpdate.Name = product.Name;
                productToUpdate.NetPrice = product.NetPrice;
                productToUpdate.GrossPrice = product.GrossPrice;
                productToUpdate.VATRate = product.VATRate;
                productToUpdate.MeasureUnit = product.MeasureUnit;
                productToUpdate.LongDescription = product.LongDescription;
                productToUpdate.ShortDescription = product.ShortDescription;
                productToUpdate.ActiveState = product.ActiveState;
                productToUpdate.Symbol = product.Symbol;
                _unitOfWork.Product.Update(productToUpdate);
                _unitOfWork.Product.Save();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult DeleteImage(int Id)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(i => i.Id == Id);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if(!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.ProductImage.Delete(imageToBeDeleted);
                _unitOfWork.ProductImage.Save();
            }
            return RedirectToAction(nameof(Upsert),new {id= productId});
        }
        public IActionResult Delete(int? Id)
        {
            var productToDelete = _unitOfWork.Product.Get(i => i.Id == Id);
            if (productToDelete == null)
            {
                return NotFound();
            }
            var imagesToDelete = _unitOfWork.ProductImage.GetAll(i => i.ProductId == productToDelete.Id).ToList();
            foreach (var image in imagesToDelete)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }

            _unitOfWork.Product.Delete(productToDelete);
            _unitOfWork.Product.Save();
            return RedirectToAction("Index");
        }
    }
}
