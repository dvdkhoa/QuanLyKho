using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using QuanLyKho.DTO;
using QuanLyKho.Extensions;
using QuanLyKho.Models;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using QuanLyKho.Services;
using System.Text.Json;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Manager,Storekeeper")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        public string PrimaryTitle = "Product";
        private readonly IMapper _mapper;

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public ProductsController(AppDbContext context, IProductService productService, IMapper mapper)
        {
            _context = context;
            _productService = productService;
            _mapper = mapper;
        }

        // GET: Products
        /// <summary>
        /// Action trả về View hiển thị danh sách tất cả các sản phẩm
        /// </summary>
        public async Task<IActionResult> Index(string filter = "All")
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            var productsQuery = _context.Products.Include(p => p.Category).AsQueryable();

            if (filter == "Show")
                productsQuery = productsQuery.Where(product => product.Status == Status.Show).AsQueryable();
            else if (filter == "Hide")
                productsQuery = productsQuery.Where(product => product.Status == Status.Hide).AsQueryable();

            ViewBag.filter = filter;
            return View(await productsQuery.ToListAsync());
        }

        // GET: Products/Details/5
        /// <summary>
        /// Action trả về view thông tin chi tiết sản phẩm
        /// </summary>
        public async Task<IActionResult> Details(string id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productInfo = _productService.getProductInfo(id);

            if (productInfo == null)
            {
                return NotFound();
            }

            return View(productInfo);
        }

        // GET: Products/Create
        /// <summary>
        /// Action trả về View thêm mới sản phẩm(GET)
        /// </summary>
        public IActionResult Create()
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateProductModel createProductModel, IFormFile? iconFile)
        {
            var category = createProductModel.Category;
            if (category != null)
            {
               if(iconFile != null)
                {
                    var imagePath = await CloudinaryHelper.UploadFileToCloudinary(iconFile, "Categories");
                    category.Icon = imagePath;
                    category.SetCreatedTime();

                    _context.AddAsync(category);
                    var kq = await _context.SaveChangesAsync();
                    if (kq > 0)
                    {
                        createProductModel.Category = null;
                        ViewBag.notify = "Add category successfully";
                        ModelState.Clear();
                        return View("Create", createProductModel); ;
                    }
                }            
            }
            ViewBag.notify = "Error: Add category failed";
            
            return View("Create", createProductModel);
        }

        // POST: Products/Create
        /// <summary>
        /// Action thêm mới sản phẩm(POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind()] CreateProductModel createProductModel)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            try
            {
                if (ModelState.IsValid)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        var newProduct = createProductModel.Product;
                        newProduct.SetCreatedTime();
                        await _context.Products.AddAsync(newProduct);
                        var result = await _context.SaveChangesAsync();

                        if (result > 0)
                        {
                            if (createProductModel.Images!.Count > 0)
                            {
                                int i = 1;
                                foreach (var image in createProductModel.Images)
                                {
                                    var imagePath = await CloudinaryHelper.UploadFileToCloudinary(image, newProduct.Id);
                                    ProductImage productImage = new ProductImage
                                    {
                                        Path = imagePath,
                                        ProductId = newProduct.Id,
                                        Status = Status.Show,
                                        CreatedTime = DateTime.Now,
                                        LastUpdated = DateTime.Now,
                                        Product = newProduct,
                                        Order = i
                                    };
                                    _context.ProductImages.Add(productImage);
                                    i++;
                                }
                                await _context.SaveChangesAsync();
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                                //ModelState.AddModelError("", "Images are required");
                            }
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", createProductModel.Product.CategoryId);
                return View(createProductModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Products/Edit/5
        /// <summary>
        /// Action trả về View cập nhật thông tin sản phẩm
        /// </summary>
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

            var brands = _context.CategoryBrands.Include(cb => cb.Brand).Where(cb => cb.CategoryId == product.CategoryId).Select(cb => new { Id = cb.Id, BrandName = cb.Brand.Name }).ToList();

            ViewData["Brands"] = new SelectList(brands, "Id", "BrandName", product.CategoryBrandId);

            var editProductModel = _mapper.Map<EditProductModel>(product);
            return View(editProductModel);
        }

        // POST: Products/Edit/5
        /// <summary>
        /// Action cập nhật thông tin sản phẩm
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditProductModel editProductModel)
        {
            ViewData["PrimaryTitle"] = PrimaryTitle;
            if (id != editProductModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = _context.Products.Find(editProductModel.Id);

                    // 
                    product.Name = editProductModel.Name;
                    product.Description = editProductModel.Description;
                    product.CategoryId = editProductModel.CategoryId;
                    product.CategoryBrandId = editProductModel.CategoryBrandId;
                    product.Price = editProductModel.Price;
                    product.Unit = editProductModel.Unit;
                    product.Supplier = editProductModel.Supplier;
                    product.Origin = editProductModel.Origin;
                    product.Weight = editProductModel.Weight;
                    product.Expiry = editProductModel.Expiry;
                    product.ManufactoringDate = editProductModel.ManufactoringDate;
                    product.PromotionPrice = editProductModel.PromotionPrice;
                    product.LastUpdated = DateTime.Now;
                    //
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(editProductModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", editProductModel.CategoryId);
            return View(editProductModel);
        }

        /// <summary>
        /// Action trả về View danh sách tất các hình ảnh của sản phẩm
        /// </summary>
        public async Task<IActionResult> ImageList(string productId)
        {
            if (productId == null)
                return NotFound();
            var product = _context.Products.Find(productId);

            if (product == null)
                return NotFound();

            var imageList = product.ProductImages.ToList();
            return View("ImageList", imageList);
        }


        /// <summary>
        /// Action xóa sản phẩm
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || _context.Products is null)
                    return BadRequest();

                var product = _context.Products.Find(id);
                if (product is null)
                    return NotFound();

                _context.Products.Remove(product);

                var kq = await _context.SaveChangesAsync();
                if (kq == 0)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Action Ẩn/Hiện sản phẩm
        /// </summary>
        public async Task<IActionResult> Display(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound();

            product.Status = product.Status.ChangeStatus();
            product.SetUpdatedTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }


        /// <summary>
        /// Action trả về View hiển thị danh sách lịch sử Nhập/Xuất/Chuyển sản phẩm
        /// </summary>
        public IActionResult ImportHistory(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var product = _context.Products.Find(id);
            if (product is null)
                return NotFound();

            var inventoryHistorys = _context.ReceiptDetails.Include(rd => rd.Product).Include(rd => rd.Receipt).Include(rd => rd.Receipt.WareHouse).Where(rd => rd.ProductId == id).ToList();

            var model = inventoryHistorys.Select(h => new InventoryHistory { Product = h.Product, WareHouse = h.Receipt.WareHouse, ReceiptDetail = h }).ToList();


            return View(model);
        }

        /// <summary>
        /// Action trả về View danh sách hình ảnh của sản phẩm(GET)
        /// </summary>
        public async Task<IActionResult> EditImages(string id)
        {
            var productImages = await _context.ProductImages.Where(i => i.ProductId == id).OrderBy(i => i.Order).ToListAsync();
            ViewBag.productId = id;
            return View("EditImages", productImages);
        }

        /// <summary>
        /// Action cập nhật thứ tự cho hình ảnh sản phẩm(POST) - Sử dụng thay cho Action ChangeOrder
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditImages(int imageId, int newOrder)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image is null)
                return NotFound();

            var oldImage = await _context.ProductImages.Where(i => i.ProductId == image.ProductId && i.Order == newOrder).FirstOrDefaultAsync();
            if (oldImage is null)
            {
                var count = await _context.ProductImages.Where(i => i.ProductId == image.ProductId).CountAsync();
                if (newOrder < 1 || newOrder > count)
                    return BadRequest();
            }


            if (oldImage != null)
            {
                int temp = image.Order.Value;

                oldImage!.Order = temp; oldImage.SetUpdatedTime();
                _context.ProductImages.Update(oldImage);
            }

            image.Order = newOrder; image.SetUpdatedTime();

            _context.ProductImages.Update(image);

            var kq = await _context.SaveChangesAsync();

            return RedirectToAction("EditImages", new { id = image.ProductId });

        }

        /// <summary>
        /// Action trả về View thêm danh sách hình ảnh cho sản phẩm(GET)
        /// </summary>
        public IActionResult AddImages(string id)
        {

            return View("AddImages", id);
        }

        /// <summary>
        /// Action thêm danh sách hình ảnh cho sản phẩm(POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddImages([FromForm] string id, [FromForm] List<IFormFile> files)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (files.Count == 0) return BadRequest();

            var lastProductImage = await _context.ProductImages.Where(pi => pi.ProductId == id).OrderBy(pi => pi.Order).LastOrDefaultAsync();

            int lastOrder = 0;
            if (lastProductImage != null)
            {
                lastOrder = lastProductImage.Order!.Value;
            }

            for (int i = 0; i < files.Count; i++)
            {
                var imagePath = await CloudinaryHelper.UploadFileToCloudinary(files[i], id);
                ProductImage productImage = new ProductImage
                {
                    Path = imagePath,
                    ProductId = id,
                    Status = Status.Show,
                    CreatedTime = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Order = (i + 1 + lastOrder),
                };
                _context.ProductImages.Add(productImage);
            }

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
                return Ok();
            return BadRequest();
        }

        /// <summary>
        /// Action thay đổi thứ tự hình ảnh sản phẩm(đã được thay thay thế bằng Action EditImages)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ChangeOrder(string productId, int productImageId, int order)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest();

            var productImage = await _context.ProductImages.Where(pi => pi.ProductId == productId && pi.Id == productImageId).FirstOrDefaultAsync();
            if (productImage == null)
                return NotFound();

            productImage.Order = order; // gán thứ tự mới cho ảnh sản phẩm


            // Làm sau => Chuyển thứ tự ban đầu cho ảnh kết tiếp => lần lượt gán các ảnh còn lại như vậy

            /*
             * 1. Lấy vị trí cũ gán cho ảnh tiếp theo
             * 2. Thực hiện gán lần lượt thứ tự cho các ảnh tiếp theo đến khi hết thì thôi
             */



            await _context.SaveChangesAsync();

            return Ok("Change order successfully");
        }

        [HttpPost]
        public IActionResult TestImages([FromForm] List<IFormFile> files)
        {
            return Ok();
        }

        /// <summary>
        /// Action trả về View danh sách các cấu hình chi tiết của sản phẩm
        /// </summary>
        public async Task<IActionResult> ProductConfigs(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();


            var configs = await _context.CategoryDetailedConfigs.Where(cdc => cdc.CategoryId == product.CategoryId).Include(cdc => cdc.DetailedConfig).ToListAsync();

            Dictionary<string, string> models = new Dictionary<string, string>();

            if (_context.ProductDetailedConfigs != null)
            {
                foreach (var config in configs)
                {
                    var productDetailedConfig = await _context.ProductDetailedConfigs.Where(pdc => pdc.ProductId == id && pdc.ConfigId == config.ConfigId).FirstOrDefaultAsync();
                    if (productDetailedConfig != null)
                        models.Add(config.DetailedConfig.Name, productDetailedConfig.Value);
                    else
                        models.Add(config.DetailedConfig.Name, "");
                }
            }
            else
            {
                models.AddRange(configs.Select(cfg => new KeyValuePair<string, string>(cfg.DetailedConfig.Name, ""))
                                        .ToList());
            }

            ViewBag.productId = id;

            return View(models);
        }


        /// <summary>
        /// Action trả về View cập nhật thông tin cấu hình chi tiết của sản phẩm(GET)
        /// </summary>
        public async Task<IActionResult> EditConfigs(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var configs = await _context.CategoryDetailedConfigs.Where(cdc => cdc.CategoryId == product.CategoryId).Include(cdc => cdc.DetailedConfig).ToListAsync();

            Dictionary<DetailedConfig, string> models = new Dictionary<DetailedConfig, string>();

            if (_context.ProductDetailedConfigs != null)
            {
                foreach (var config in configs)
                {
                    var productDetailedConfig = await _context.ProductDetailedConfigs.Where(pdc => pdc.ProductId == id && pdc.ConfigId == config.ConfigId).FirstOrDefaultAsync();
                    if (productDetailedConfig != null)
                        models.Add(config.DetailedConfig, productDetailedConfig.Value);
                    else
                        models.Add(config.DetailedConfig, "");
                }
            }
            else
            {
                models.AddRange(configs.Select(cfg => new KeyValuePair<DetailedConfig, string>(cfg.DetailedConfig, ""))
                                        .ToList());
            }

            ViewBag.productId = id;

            return View(models);
        }


        /// <summary>
        /// Action cập nhật thông tin cấu hình chi tiết cho sản phẩm(POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditConfigs(string productId, string configIds, string[] configValues)
        {
            var configKeys = JsonSerializer.Deserialize<List<int>>(configIds);

            if (configKeys!.Count != configValues.Length)
            {
                return BadRequest();
            }

            for (int i = 0; i < configKeys.Count; i++)
            {
                var productConfig = _context.ProductDetailedConfigs.Where(pdc => pdc.ProductId == productId && pdc.ConfigId == configKeys[i]).FirstOrDefault();
                if (productConfig != null)
                { // Nếu đã tồn tại thì update
                    productConfig.Value = configValues[i];
                    _context.Update(productConfig);

                }
                else // Thêm mới
                {
                    var newProductConfig = new ProductDetailedConfig
                    {
                        ConfigId = configKeys[i],
                        ProductId = productId,
                        Value = configValues[i]
                    };
                    await _context.AddAsync(newProductConfig);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ProductConfigs", new { id = productId });
        }



        /// <summary>
        /// Phương thức lấy tất cả các sản phẩm thuộc kho bất kỳ dựa trên mã kho
        /// </summary>
        public async Task<IActionResult> GetProductByWarehouseId(string id)
        {
            var products = await _context.ProductWareHouses.Include(pw => pw.Product).Where(pw => pw.WareHouseId == id).Select(pw => pw.Product).ToListAsync();

            return Ok(products);
        }



        /// <summary>
        /// Action xóa hình ảnh sản phẩm
        /// </summary>
        public async Task<IActionResult> DeleteImage(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);

            if (productImage == null) return NotFound();

            _context.Remove(productImage);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                await CloudinaryHelper.DeteleImage(productImage.Path, productImage.ProductId);
                return Ok("Delete successfully");
            }
            return BadRequest();
        }


        /// <summary>
        /// Phương thức kiểm tra sản phẩm đã tồn tại trong hệ thống hay chưa
        /// </summary>
        private bool ProductExists(string id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
