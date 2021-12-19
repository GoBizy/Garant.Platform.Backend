﻿using System.Threading.Tasks;
using Garant.Platform.Models.Document.Output;

namespace Garant.Platform.Abstractions.Document
{
    /// <summary>
    /// Абстракция репозитория документов для работы с БД.
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Метод запишет в БД договор от продавца.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="documentItemId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="isDealDocument">Флаг документа сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные документа.</returns>
        Task<DocumentOutput> AddVendorDocumentAsync(string fileName, long documentItemId, string documentType, bool isDealDocument, string account);

        /// <summary>
        /// Метод запишет в true флаг отправки документа на согласование покупателю.
        /// </summary>
        /// <param name="documentItemId">Id документа.</param>
        /// <param name="isDealDocument">Является ли документом сделки.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <returns>Флаг успеха.</returns>
        Task<bool> SetSendStatusDocumentVendorAsync(long documentItemId, bool isDealDocument, string documentType);

        /// <summary>
        /// Метод получит название документа, который отправлен на согласование покупателю.
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// </summary>
        /// <returns>Название документа продавца.</returns>
        Task<DocumentOutput> GetAttachmentNameDocumentVendorDealAsync(long documentItemId);

        /// <summary>
        /// Метод проверит, подтвердил ли покупатель договор продавца.
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// </summary>
        /// <returns>Флаг проверки.</returns>
        Task<bool> CheckApproveDocumentVendorAsync(long documentItemId, string account);

        /// <summary>
        /// Метод подтвердит договор продавца.
        /// </summary>
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// <returns>Флаг проверки.</returns>
        Task<bool> ApproveDocumentVendorAsync(long documentItemId);
    }
}
