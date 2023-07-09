using EShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Repository.Interface
{
    public interface IOrderRepository
    {
        public List<Order> GetAllOrders();
        public Order GetOrderDetails(BaseEntry model);
    }
}
