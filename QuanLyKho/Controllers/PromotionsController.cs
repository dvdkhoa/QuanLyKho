using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Extensions;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;
using QuanLyKho.Services;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class PromotionsController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public PromotionsController(AppDbContext context, PromotionService promotionService)
        {
            _context = context;
        }

        /// <summary>
        /// Action trả về View danh sách tất cả CTKM có trong hệ thống
        /// </summary>
        public IActionResult Index(string filter = "All")
        {
            if (_context.Promotions == null)
                return Problem("Entity set 'AppDbContext.Promotions'  is null.");

            var promotions = _context.Promotions;

            List<Promotion> promotionsModel = new List<Promotion>();
            if (filter == "Show")
                promotionsModel = promotions.Where(p => p.Status == Status.Show).ToList();
            else if (filter == "Hide")
                promotionsModel = promotions.Where(p => p.Status == Status.Hide).ToList();
            else
                promotionsModel = promotions.ToList();

            ViewBag.filter = filter;

            return View(promotionsModel);
        }

        /// <summary>
        /// Action trả về View thông tin chi tiết của CTKM
        /// </summary>
        public IActionResult Details(int id)
        {
            var promotion = _context.Promotions.Find(id);
            if (promotion is null)
                return NotFound();

            promotion.ProductPromotions = _context.ProductPromotions.Include(p => p.Product).Where(p => p.PromotionId == id).ToList();

            return View(promotion);
        }


        /// <summary>
        /// Action Ẩn/Hiện CTKM
        /// </summary>
        public async Task<IActionResult> Display(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion is null)
                return NotFound();

            promotion.Status = promotion.Status.ChangeStatus();
            promotion.SetUpdatedTime();

            int kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return RedirectToAction("Index");

            }
            return BadRequest();
        }

        /// <summary>
        /// Action trả về View tạo mới CTKM(GET)
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Action tạo mới CTKM(POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (promotion.StartDate == promotion.EndDate)
                    {
                        ModelState.AddModelError("", "'Start date' and 'End date' must be different");
                        return View(promotion);
                    }

                    var kqCompare = promotion.StartDate.CompareTo(promotion.EndDate);

                    if (kqCompare > 0)
                    {
                        ModelState.AddModelError("", "'Start date' must be before 'End date' ");
                        return View(promotion);
                    }

                    if (promotion.PromotionType == PromotionType.Discount && promotion.Percent  == 0)
                    {
                        ModelState.AddModelError("", "Discount promotion needs to be large 0%");
                        return View(promotion);
                    }
                    

                    promotion.SetCreatedTime();
                    await _context.AddAsync(promotion);

                    var kq = await _context.SaveChangesAsync();
                    if (kq > 0)
                    {
                        return RedirectToAction("Details", promotion);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Action trả về View thêm sản phẩm vào CTKM
        /// </summary>
        public async Task<IActionResult> AddPromotionalProducts(int id)
        {
            // Lấy danh sách sản phẩm chưa tồn tại trong khuyến mãi này, đồng thời phải đảm bảm sản phẩm này
            // không được nằm trong khuyến mãi giảm giá nào khác(khuyễn mãi giảm giá và tặng sản phẩm có thể áp dùng
            // đồng thời trên 1 sản phẩm được)

            var productPromotionIds = await _context.ProductPromotions
                                                    .Where(pp => pp.PromotionId == id).Select(pp => pp.ProductId)
                                                    .ToListAsync();

            // Bổ sung thêm: 1 sản phẩm đã thuộc khuyến mãi giảm giá có tác dụng trong thời gian này thì không được thêm vào cho khuyến mãi giảm giá khác
            // Đem xuống phần kiểm trả bên phương thức post
            //var otherPromotionalProductIds = await _context.ProductPromotions.Include(pp => pp.Promotion)
            //                                                .Where(pp => pp.Promotion.PromotionType == PromotionType.Discount
            //                                                    && pp.Promotion.EndDate <= DateTime.Now.Date).Select(pp => pp.ProductId).ToListAsync();

            var products = await _context.Products
                                         .Where(p => !productPromotionIds.Contains(p.Id))
                                         .ToListAsync();

            //var products = await _context.Products
            //                             .Where(p => !productPromotionIds.Contains(p.Id)
            //                                        || !otherPromotionalProductIds.Contains(p.Id))
            //                             .ToListAsync();


            ViewData["ProductId"] = new SelectList(products, "Id", "Name");
            return View(id);
        }

        /// <summary>
        /// Action thêm danh sách sản phẩm vào CTKM
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddPromotionalProducts(int promotionId, string[] productIds)
        {
            if (promotionId == 0 || productIds is null || productIds.Length == 0)
            {
                return BadRequest();
            }

            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion == null)
            {
                return NotFound();
            }


            // Lấy danh sách sản phẩm chưa tồn tại trong khuyến mãi này, đồng thời phải đảm bảm sản phẩm này
            // không được nằm trong khuyến mãi giảm giá nào khác(khuyễn mãi giảm giá và tặng sản phẩm có thể áp dùng
            // đồng thời trên 1 sản phẩm được)

            var productPromotionIds = await _context.ProductPromotions
                                                    .Where(pp => pp.PromotionId == promotionId).Select(pp => pp.ProductId)
                                                    .ToListAsync();

            // Bổ sung thêm: 1 sản phẩm đã thuộc khuyến mãi giảm giá có tác dụng trong thời gian này thì không được thêm vào cho khuyến mãi giảm giá khác
            // Đem xuống phần kiểm trả bên phương thức post
            //var otherPromotionalProductIds = await _context.ProductPromotions.Include(pp => pp.Promotion)
            //                                                .Where(pp => pp.Promotion.PromotionType == PromotionType.Discount
            //                                                    && pp.Promotion.EndDate <= DateTime.Now.Date).Select(pp => pp.ProductId).ToListAsync();

            var selectProducts = await _context.Products
                                         .Where(p => !productPromotionIds.Contains(p.Id))
                                         .ToListAsync();

            //var products = await _context.Products
            //                             .Where(p => !productPromotionIds.Contains(p.Id)
            //                                        || !otherPromotionalProductIds.Contains(p.Id))
            //                             .ToListAsync();


            ViewData["ProductId"] = new SelectList(selectProducts, "Id", "Name");


            // Bổ sung thêm: 1 sản phẩm đã thuộc khuyến mãi giảm giá có tác dụng trong thời gian này thì không được thêm vào cho khuyến mãi giảm giá khác
            if (promotion.PromotionType == PromotionType.Discount)
            {
                // Lấy ra tất cả các id sản phẩm thuộc 1 chương trình khuyến mãi giảm giá bất kỳ ở thời điểm hiện tại - có hiệu lực
                var otherPromotionalProductIds = await _context.ProductPromotions.Include(pp => pp.Promotion)
                                                                .Where(pp => pp.Promotion.PromotionType == PromotionType.Discount
                                                                    //&& pp.Promotion.EndDate.Date >= DateTime.Now.Date).Select(pp => pp.ProductId).ToListAsync();
                                                                    && DateTime.Now < pp.Promotion.EndDate).Select(pp => pp.ProductId).ToListAsync();

                // Check
                List<string> idExists = new List<string>();
                foreach (var id in productIds)
                {
                    if (otherPromotionalProductIds.Contains(id))
                    {
                        idExists.Add(id);
                    }
                }
                if (idExists.Count > 0)
                {
                    ModelState.AddModelError("", $"Products with Id: {string.Join(",", idExists)} already exist in another promotion");

                    return View(promotionId);
                }
            }

            var productPromotions = productIds.Select(id =>
            {
                return new ProductPromotion
                {
                    ProductId = id,
                    PromotionId = promotionId
                };
            });
            await _context.AddRangeAsync(productPromotions);
            int kq = await _context.SaveChangesAsync();

            // So sánh kết quả số dòng insert có bằng số lượng id sản phẩm được gửi đến không, sau cần bắt lỗi chỗ này để rollback
            if (kq > 0 && kq == productIds.Length)
            {
                if (promotion.Status == Status.Show) // Nếu promotion active thì mới check set giá khuyến mãi
                {
                    // Kiểm tra xem đây là loại khuyến mãi giảm giá hay là khuyến mãi tặng sản phẩm
                    if (promotion.PromotionType == PromotionType.Discount)
                    {
                        if (promotion.StartDate.CompareTo(DateTime.Now) <= 0)
                        {
                            // Lấy ra các sản phẩm cần cập nhật giá trị PromotionalPrice
                            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                            products.ForEach(p =>
                            {
                                p.PromotionPrice = p.Price - (p.Price * promotion.Percent / 100); // Tính giá khuyễn mãi :D
                                p.LastUpdated = DateTime.Now;
                            });
                            _context.Products.UpdateRange(products);
                            kq = await _context.SaveChangesAsync();
                            if (kq == 0)
                            {
                                ModelState.AddModelError("", "Updating the promotional price for products failed! ");
                                return View(promotionId);
                            }
                        }
                    }
                    else // Phần này là dành cho PromotionType.Gift, sau nếu triển khai loại khuyến mãi này thì viết tiếp code ở đây
                    {

                    }
                }
            }
            return RedirectToAction("Details", promotion);
        }


        /// <summary>
        /// Action xóa sản phẩm khỏi CTKM
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RemovePromotionalProduct(int promotionId, string productId)
        {
            var productPromotion = await _context.ProductPromotions.Where(pp => pp.ProductId == productId && pp.PromotionId == promotionId).FirstOrDefaultAsync();
            if (productPromotion == null)
            {
                return NotFound();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var product = await _context.Products.FindAsync(productPromotion.ProductId);

                    _context.Remove(productPromotion);

                    int kq = await _context.SaveChangesAsync();
                    if (kq > 0)
                    {
                        product!.PromotionPrice = null;
                        _context.Update(product);

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return Ok();
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest();
                }
            }
        }

        /// <summary>
        /// Action xóa CTKM
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int kq = 0;
                var promotion = await _context.Promotions.FindAsync(id);
                if (promotion == null)
                    return NotFound();

                // Lấy tất cả các sản phẩm thuộc chương trình khuyến mãi này
                var productPromotions = await _context.ProductPromotions.Where(pp => pp.PromotionId == id).ToListAsync();
                // Lấy ra danh sách productId
                var productIds = productPromotions.Select(pp => pp.ProductId).ToList();

                // Kiểm tra xem promotion này có đang hiệu lực hay không ?
                // Nếu còn trong thời hạn hiệu lực hoặc chưa tới thời hạn hiệu lực(phần chưa tới thời hạn - chưa có trong hệ thống-> cần triển khai sau này) thì cập nhật giá lại(xóa giá khuyến mãi).
                if (promotion.StartDate.Date <= DateTime.Now.Date || promotion.EndDate.Date >= DateTime.Now.Date)
                {
                    var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                    if (productIds?.Count > 0)
                    {
                        products.ForEach(p =>
                        {
                            p.PromotionPrice = null;
                        });
                        _context.Products.UpdateRange(products);
                        kq = await _context.SaveChangesAsync();
                        if (kq == 0)
                            return BadRequest();
                    }
                }
                // Nếu không còn trong thời hạn hiệu lực thì chỉ xóa các chi tiết sản phẩm trong promotion này thôi chứ không thay đổi giá

                kq = 0; // reset biến kq để dùng cho việc kiểm tra phía bên dưới

                _context.ProductPromotions.RemoveRange(productPromotions);  // Xóa productPromotions

                _context.Remove(promotion); // Xóa promotion
                kq = await _context.SaveChangesAsync();

                if (kq > 0)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Action trả về View cập nhật thông tin CTKM
        /// </summary>
        public IActionResult Edit(int id)
        {
            return View(_context.Promotions.Find(id));
        }

        /// <summary>
        /// Action cập nhật CTKM
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(Promotion promotion)
        {
            promotion.SetUpdatedTime();
            _context.Update(promotion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", promotion);
        }
    }
}
