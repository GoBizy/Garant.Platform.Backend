﻿using Garant.Platform.Models.User.Output;

namespace Garant.Platform.Configurator.Abstractions
{
    /// <summary>
    /// Абстракция сервиса конфигуратора.
    /// </summary>
    public interface IConfiguratorService
    {
        /// <summary>
        /// Метод заведет нового сотрудника сервиса.
        /// </summary>
        /// <param name="employeeRoleName">Название роли сотрдника.</param>
        /// <param name="employeeRoleSystemName">Системное название роли сотрудника.</param>
        /// <param name="employeeStatus">Статус сотрудника.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="patronymic">Отчество.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="email">Почта сотрудника.</param>
        /// <param name="telegramTag">Тэг в телеграме.</param>
        /// <returns>Данные добавленного сотрудника</returns>
        Task<CreateEmployeeOutput> CreateEmployeeAsync(string employeeRoleName, string employeeRoleSystemName,
            string employeeStatus, string firstName, string lastName, string patronymic, string phoneNumber,
            string email, string telegramTag);
    }
}