using Backend.Models;

namespace Backend.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Soda", Category = "Beverages", Price = 1.5M, IsAvailable = true },
            new Product { Id = 2, Name = "Sandwich", Category = "Foods", Price = 4.0M, IsAvailable = true },
            new Product { Id = 3, Name = "Beer", Category = "Beverages", Price = 2.5M, IsAvailable = false },
        };

        public IEnumerable<Product> GetAllProducts() => _products;

        public Product GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void AddProduct(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Category = product.Category;
                existing.Price = product.Price;
                existing.IsAvailable = product.IsAvailable;
            }
        }

        public void DeleteProduct(int id) => _products.RemoveAll(p => p.Id == id);
    }
}
