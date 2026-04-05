namespace E_Commerce.Interfaces
{
    public interface IPaymentService
    {
        Task SaveOrderAsync(string userId, List<(int ProductId, int Quantity, decimal Price)> items);
    }
}