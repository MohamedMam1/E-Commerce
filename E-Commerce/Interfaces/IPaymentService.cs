using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Interfaces
{
    public interface IPaymentService
    {
        Task SaveOrderAsync(
            string userId,
            List<(int ProductVariantId, int Quantity, decimal Price)> items);
    }
}

