using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order = ECommerce.Api.Orders.Db.Order;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider

    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem() { OrderId = 1, ProductId = 1, Quantity = 3, UnitPrice = 1000 },
                        new Db.OrderItem() { OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 5000 },
                        new Db.OrderItem() { OrderId = 1, ProductId = 3, Quantity = 1, UnitPrice = 3000 }
                    },
                    Total = 11000
                });
                dbContext.Orders.Add(new Order()
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem() { OrderId = 2, ProductId = 4, Quantity = 5, UnitPrice = 100 },
                        new Db.OrderItem() { OrderId = 2, ProductId = 5, Quantity = 2, UnitPrice = 50 },
                        new Db.OrderItem() { OrderId = 2, ProductId = 6, Quantity = 1, UnitPrice = 700 }
                    },
                    Total = 1300
                });
                dbContext.Orders.Add(new Order()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.Now,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem() { OrderId = 3, ProductId = 7, Quantity = 3, UnitPrice = 1000 },
                        new Db.OrderItem() { OrderId = 3, ProductId = 8, Quantity = 1, UnitPrice = 5000 },
                        new Db.OrderItem() { OrderId = 3, ProductId = 9, Quantity = 1, UnitPrice = 3000 }
                    },
                    Total = 11000
                });

                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var order = await dbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Items)
                    .ToListAsync();

                if (order != null && order.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(order);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
