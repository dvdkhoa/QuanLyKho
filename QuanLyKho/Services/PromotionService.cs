using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models.EF;
using QuanLyKho.Models.Entities;

namespace QuanLyKho.Services
{
    public interface IPromotionService
    {
        public Task UpdatePromotionPriceStartDate();
        public Task UpdatePromotionPriceEndDate();
    }
    public class PromotionService : IPromotionService
    {
        private readonly AppDbContext _context;
        public PromotionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdatePromotionPriceStartDate()
        {
            Console.WriteLine("Running Update promotion price StartDate !!!!!");

            var promotions = await _context.Promotions.Include(prm => prm.ProductPromotions).Where(prm => prm.Status == Status.Show).Where(prm => prm.StartDate.Date <= DateTime.Now.Date).ToListAsync();
            foreach (var promotion in promotions)
            {
                if (promotion.PromotionType == Models.Entities.PromotionType.Discount)
                {
                    if (promotion.StartDate < DateTime.Now && DateTime.Now < promotion.EndDate)
                    {
                        if (promotion.ProductPromotions != null && promotion.ProductPromotions.Count > 0)
                        {
                            foreach (var productPromotion in promotion.ProductPromotions)
                            {
                                var product = await _context.Products.FindAsync(productPromotion.ProductId);
                                product!.PromotionPrice = product.Price - (product.Price * promotion.Percent / 100);
                            }
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePromotionPriceEndDate()
        {
            Console.WriteLine("Running Update promotion price EndDate !!!!!");
            var promotions = await _context.Promotions.Include(prm => prm.ProductPromotions).Where(prm => prm.Status == Status.Show).Where(prm => prm.EndDate.Date <= DateTime.Now.Date).ToListAsync();
            foreach (var promotion in promotions)
            {
                if (DateTime.Now > promotion.EndDate)
                {
                    if (promotion.PromotionType == Models.Entities.PromotionType.Discount)
                    {
                        if (promotion.ProductPromotions != null && promotion.ProductPromotions.Count > 0)
                        {
                            foreach (var productPromotion in promotion.ProductPromotions)
                            {
                                var product = await _context.Products.FindAsync(productPromotion.ProductId);
                                product!.PromotionPrice = null;
                            }
                        }
                    }
                    promotion.Status = Status.Hide;
                }

            }

            await _context.SaveChangesAsync();
        }
    }
}
