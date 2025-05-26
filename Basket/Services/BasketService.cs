global using Microsoft.Extensions.Caching.Distributed;
global using System.Text.Json;
namespace Basket.Services
{
    public class BasketService(IDistributedCache cache, CatalogApiClient catalogApiClient)
    {
        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            var basket = await cache.GetStringAsync(userName);
            return string.IsNullOrEmpty(basket) ? null : 
                JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task UpdateBasket(ShoppingCart basket)
        {
            foreach(var item in basket.Items)
            {
                var product = await catalogApiClient.GetProductById(item.ProductId);
                item.Price = product.Price;
                item.ProductName = product.Name;
            }

            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
        }

        public async Task DeleteBasket(string userName)
        {
            await cache.RemoveAsync(userName);
        }
    }
}
