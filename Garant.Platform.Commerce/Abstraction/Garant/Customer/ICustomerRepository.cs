﻿using System.Threading.Tasks;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Models.Commerce.Output;

namespace Garant.Platform.Commerce.Abstraction.Garant.Customer
{
    /// <summary>
    /// Абстракция репозитория Гаранта со стороны покупателя для работы с БД.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Метод создаст новый заказ.
        /// </summary>
        /// <param name="amount">Цена.</param>
        /// <param name="description">Объект с описанием платежа.</param>
        /// <param name="iteration">Номер итерации этапа.</param>
        /// <returns>Данные платежа.</returns>
        Task<OrderOutput> CreateOrderAsync(long originalId, double amount, Description description, string orderType, string userId, int iteration);

        /// <summary>
        /// Метод проставит флаг оплаты в true документам покупателя, которые оплачены.
        /// </summary>
        /// <param name="userId">Id покупателя.</param>
        /// <param name="iteration">Номер итерации.</param>
        Task SetDocumentsCustomerPaymentAsync(string userId, int iteration);
    }
}
