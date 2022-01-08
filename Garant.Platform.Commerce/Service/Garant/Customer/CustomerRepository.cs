﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Commerce.Output;
using Garant.Platform.Models.Entities.Commerce;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Commerce.Service.Garant.Customer
{
    /// <summary>
    /// Сервис реализует методы репозитория Гаранта со стороны покупателя для работы с БД.
    /// </summary>
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public CustomerRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод создаст новый заказ.
        /// </summary>
        /// <param name="amount">Цена.</param>
        /// <param name="description">Объект с описанием платежа.</param>
        /// <param name="iteration">Номер итерации этапа.</param>
        /// <returns>Данные платежа.</returns>
        public async Task<OrderOutput> CreateOrderAsync(long originalId, double amount, Description description, string orderType, string userId, int iteration)
        {
            try
            {
                var id = 1000000;
                long lastId = 0;
                var optionalType = "DocumentCustomerAct" + iteration;

                if (await _postgreDbContext.Orders.AnyAsync())
                {
                    lastId = await _postgreDbContext.Orders.MaxAsync(o => o.OrderId);
                }

                if (lastId <= 0)
                {
                    lastId = id;
                }

                lastId++;
                var last = lastId;

                // Проверит существование такого заказа.
                var checkOrder = await _postgreDbContext.Orders
                    .AsNoTracking()
                    .Where(o => o.OriginalId == originalId
                                && o.UserId.Equals(userId)
                                && o.OrderType.Equals(orderType)
                                && o.Iteration == iteration
                                && o.OptionalType.Equals(optionalType))
                    .FirstOrDefaultAsync();

                if (checkOrder == null)
                {
                    // Создаст новый заказ.
                    await _postgreDbContext.Orders.AddAsync(new OrderEntity
                    {
                        OrderId = last,
                        Amount = amount,
                        Currency = "RUB",
                        DateCreate = DateTime.Now,
                        ShortOrderName = description.Short,
                        FullOrderName = description.Full,
                        OrderStatus = "Hold",
                        TotalAmount = amount,
                        OrderType = orderType,
                        OriginalId = originalId,
                        ProductCount = 1,
                        UserId = userId,
                        Iteration = iteration,
                        OptionalType = optionalType
                    });

                    await _postgreDbContext.SaveChangesAsync();
                }

                // Обновит заказ.
                else
                {
                    var getOrder = await _postgreDbContext.Orders
                        .AsNoTracking()
                        .FirstOrDefaultAsync(o => o.OriginalId == originalId
                                                  && o.UserId.Equals(userId)
                                                  && o.OrderType.Equals(orderType));

                    if (getOrder != null)
                    {
                        var newOrder = new OrderEntity
                        {
                            OrderId = getOrder.OrderId,
                            Amount = amount,
                            Currency = "RUB",
                            DateCreate = DateTime.Now,
                            ShortOrderName = description.Short,
                            FullOrderName = description.Full,
                            OrderStatus = "Hold",
                            TotalAmount = amount,
                            OrderType = orderType,
                            OriginalId = originalId,
                            ProductCount = 1,
                            UserId = userId,
                            Iteration = iteration,
                            OptionalType = optionalType
                        };

                        _postgreDbContext.Orders.Update(newOrder);
                        await _postgreDbContext.SaveChangesAsync();
                    }
                }

                var result = await _postgreDbContext.Orders
                    .OrderByDescending(o => o.OrderId)
                    .Select(o => new OrderOutput { OrderId = o.OrderId })
                    .FirstOrDefaultAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }
    }
}
