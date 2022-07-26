﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Document.Output;
using Garant.Platform.Models.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Garant.Platform.Services.Document
{
    /// <summary>
    /// Репозитория документов.
    /// </summary>
    public sealed class DocumentRepository : IDocumentRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;

        public DocumentRepository(PostgreDbContext postgreDbContext, IUserRepository userRepository)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Метод запишет в БД договор от продавца.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="documentItemId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="isDealDocument">Флаг документа сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные документа.</returns>
        public async Task<DocumentOutput> AddVendorDocumentAsync(string fileName, long documentItemId, string documentType, bool isDealDocument, string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                // Поищет такой документ в БД и обновит если он уже был добавлен.
                var getDocument = await _postgreDbContext.Documents
                    .AsNoTracking()
                    .Where(d => d.DocumentItemId == documentItemId && d.DocumentType.Equals(documentType))
                    .FirstOrDefaultAsync();

                var document = new DocumentEntity
                {
                    DateCreate = DateTime.Now,
                    DocumentItemId = documentItemId,
                    DocumentName = fileName,
                    DocumentType = documentType,
                    IsDealDocument = isDealDocument,
                    IsApproveDocument = false,
                    IsRejectDocument = false,
                    UserId = userId,
                    IsSend = false,
                    IsPay = null
                };

                // Обновит документ.
                if (getDocument != null)
                {
                    getDocument.DocumentId = document.DocumentId;

                    _postgreDbContext.Documents.Update(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

                // Добавит новый документ.
                else
                {
                    await _postgreDbContext.Documents.AddAsync(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

                var jsonString = JsonConvert.SerializeObject(document);
                var result = JsonConvert.DeserializeObject<DocumentOutput>(jsonString);

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

        /// <summary>
        /// Метод запишет в true флаг отправки документа на согласование покупателю.
        /// </summary>
        /// <param name="documentItemId">Id документа.</param>
        /// <param name="isDealDocument">Является ли документом сделки.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<bool> SetSendStatusDocumentVendorAsync(long documentItemId, bool isDealDocument, string documentType)
        {
            try
            {
                var getDocument = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && d.DocumentType.Equals(documentType)
                                && d.IsDealDocument.Equals(isDealDocument))
                    .FirstOrDefaultAsync();

                // Если такой документ не найден.
                if (getDocument == null)
                {
                    return false;
                }

                getDocument.IsSend = true;

                await _postgreDbContext.SaveChangesAsync();

                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит название документа, который отправлен на согласование покупателю.
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// </summary>
        /// <returns>Название документа продавца.</returns>
        public async Task<DocumentOutput> GetAttachmentNameDocumentVendorDealAsync(long documentItemId)
        {
            try
            {
                var result = await _postgreDbContext.Documents
                    .Where(d => d.IsSend.Equals(true) 
                                && d.DocumentItemId == documentItemId 
                                && d.DocumentType.Equals("DocumentVendor"))
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName,
                        DocumentId = d.DocumentId
                    })
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

        /// <summary>
        /// Метод проверит, подтвердил ли покупатель договор продавца.
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// </summary>
        /// <returns>Флаг проверки.</returns>
        public async Task<bool> CheckApproveDocumentVendorAsync(long documentItemId)
        {
            try
            {
                var checkApprove = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId && d.DocumentType.Equals("DocumentVendor"))
                    .Select(d => d.IsApproveDocument)
                    .FirstOrDefaultAsync();

                return checkApprove ?? false;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод подтвердит договор продавца.
        /// </summary>
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Флаг проверки.</returns>
        public async Task<bool> ApproveDocumentVendorAsync(long documentItemId, string account)
        {
            try
            {
                // Если не передан Id документа предмета сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId 
                                && d.IsSend.Equals(true))
                    .FirstOrDefaultAsync();

                // Если документ найден, то подтвердит его.
                if (result != null)
                {
                    result.IsApproveDocument = true;
                    await _postgreDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод запишет в true флаг отправки согласованного покупателем документа продавца.
        /// </summary>
        /// <param name="documentItemId">Id документа.</param>
        /// <param name="isDealDocument">Является ли документом сделки.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<bool> SetSendStatusDocumentCustomerAsync(long documentItemId, bool isDealDocument, string documentType)
        {
            try
            {
                var getDocument = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && d.DocumentType.Equals(documentType)
                                && d.IsDealDocument.Equals(isDealDocument))
                    .FirstOrDefaultAsync();

                // Если такой документ не найден.
                if (getDocument == null)
                {
                    return false;
                }

                getDocument.IsSend = true;

                await _postgreDbContext.SaveChangesAsync();

                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод запишет в БД согласованный покупателем договор от продавца.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="documentItemId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="isDealDocument">Флаг документа сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные документа.</returns>
        public async Task<DocumentOutput> AddCustomerDocumentAsync(string fileName, long documentItemId, string documentType, bool isDealDocument, string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                // Поищет такой документ в БД и обновит если он уже был добавлен.
                var getDocument = await _postgreDbContext.Documents
                    .AsNoTracking()
                    .Where(d => d.DocumentItemId == documentItemId && d.DocumentType.Equals(documentType))
                    .FirstOrDefaultAsync();

                var document = new DocumentEntity
                {
                    DateCreate = DateTime.Now,
                    DocumentItemId = documentItemId,
                    DocumentName = fileName,
                    DocumentType = documentType,
                    IsDealDocument = isDealDocument,
                    IsApproveDocument = false,
                    IsRejectDocument = false,
                    UserId = userId,
                    IsSend = false
                };

                // Обновит документ.
                if (getDocument != null)
                {
                    document.DocumentId = getDocument.DocumentId;

                    _postgreDbContext.Documents.Update(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

                // Добавит новый документ.
                else
                {
                    await _postgreDbContext.Documents.AddAsync(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

                var jsonString = JsonConvert.SerializeObject(document);
                var result = JsonConvert.DeserializeObject<DocumentOutput>(jsonString);

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

        /// <summary>
        /// Метод подтвердит договор покупателя.
        /// </summary>
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// <returns>Флаг проверки.</returns>
        public async Task<bool> ApproveDocumentCustomerAsync(long documentItemId)
        {
            try
            {
                // Если не передан Id документа предмета сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId 
                                && d.IsSend.Equals(true)
                                && d.DocumentType.Equals("DocumentCustomer"))
                    .FirstOrDefaultAsync();

                // Если документ найден, то подтвердит его.
                if (result != null)
                {
                    result.IsApproveDocument = true;
                    await _postgreDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит название документа, который отправлен на согласование продавцу.
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// </summary>
        /// <returns>Название документа продавца.</returns>
        public async Task<DocumentOutput> GetAttachmentNameDocumentCustomerDealAsync(long documentItemId)
        {
            try
            {
                var result = await _postgreDbContext.Documents
                    .Where(d => d.IsSend.Equals(true)
                                && d.DocumentItemId == documentItemId
                                && d.DocumentType.Equals("DocumentCustomer"))
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName,
                        DocumentId = d.DocumentId
                    })
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

        /// <summary>
        /// Метод проверит, подтвердил ли продавец договор покупателя.
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// </summary>
        /// <returns>Флаг проверки.</returns>
        public async Task<bool> CheckApproveDocumentCustomerAsync(long documentItemId)
        {
            try
            {
                var checkApprove = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId && d.DocumentType.Equals("DocumentCustomer"))
                    .Select(d => d.IsApproveDocument)
                    .FirstOrDefaultAsync();

                return checkApprove ?? false;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        ///  Метод получит список документов сделки.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <returns>Список документов.</returns>
        public async Task<IEnumerable<DocumentOutput>> GetDocumentsDealAsync(long documentItemId)
        {
            try
            {
                // Если не передан Id сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                var documents = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && new[] { "DocumentVendor", "DocumentCustomer" }.Contains(d.DocumentType))
                    .OrderBy(d => d.DateCreate)
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName,
                        DateCreate = d.DateCreate.ToString("d")
                    })
                    .ToListAsync();

                return documents;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод запишет в БД документ акта.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="documentItemId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="isDealDocument">Флаг документа сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные документа.</returns>
        public async Task<DocumentOutput> AddDocumentActAsync(string fileName, long documentItemId, string documentType, bool isDealDocument, string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                // Поищет такой документ в БД и обновит если он уже был добавлен.
                var getDocument = await _postgreDbContext.Documents
                    .AsNoTracking()
                    .Where(d => d.DocumentItemId == documentItemId && d.DocumentType.Equals(documentType))
                    .FirstOrDefaultAsync();

                var document = new DocumentEntity
                {
                    DateCreate = DateTime.Now,
                    DocumentItemId = documentItemId,
                    DocumentName = fileName,
                    DocumentType = documentType,
                    IsDealDocument = isDealDocument,
                    IsApproveDocument = false,
                    IsRejectDocument = false,
                    UserId = userId,
                    IsSend = true,
                    IsPay = false
                };

                // Обновит документ.
                if (getDocument != null)
                {
                    getDocument.DocumentId = document.DocumentId;

                    _postgreDbContext.Documents.Update(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

                // Добавит новый документ.
                else
                {
                    await _postgreDbContext.Documents.AddAsync(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

                var jsonString = JsonConvert.SerializeObject(document);
                var result = JsonConvert.DeserializeObject<DocumentOutput>(jsonString);

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

        /// <summary>
        /// Метод получит список актов продавца.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <returns>Список актов продавца.</returns>
        public async Task<IEnumerable<DocumentOutput>> GetVendorActsAsync(long documentItemId)
        {
            try
            {
                // Если не передан Id сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && d.IsSend == true
                                && d.IsDealDocument == true
                                && new[] { "DocumentVendorAct1", "DocumentVendorAct2", "DocumentVendorAct3", "DocumentVendorAct4", "DocumentVendorAct5", "DocumentVendorAct6", "DocumentVendorAct7", "DocumentVendorAct8", "DocumentVendorAct9", "DocumentVendorAct10" }.Contains(d.DocumentType))
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName,
                        IsPay = d.IsPay,
                        DocumentId = d.DocumentId,
                        DateCreate = d.DateCreate.ToString("d"),
                        IsAcceptDocument = d.IsApproveDocument,
                        DocumentType = d.DocumentType
                    })
                    .ToListAsync();

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

        /// <summary>
        /// Метод получит список актов покупателя.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <returns>Список актов покупателя.</returns>
        public async Task<IEnumerable<DocumentOutput>> GetCustomerActsAsync(long documentItemId)
        {
            try
            {
                // Если не передан Id сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && d.IsSend == true
                                && d.IsDealDocument == true
                                && new[] { "DocumentCustomerAct1", "DocumentCustomerAct2", "DocumentCustomerAct3", "DocumentCustomerAct4", "DocumentCustomerAct5", "DocumentCustomerAct6", "DocumentCustomerAct7", "DocumentCustomerAct8", "DocumentCustomerAct9", "DocumentCustomerAct10" }.Contains(d.DocumentType))
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName,
                        IsPay = d.IsPay,
                        DocumentId = d.DocumentId,
                        DateCreate = d.DateCreate.ToString("d"),
                        IsAcceptDocument = d.IsApproveDocument,
                        DocumentType = d.DocumentType
                    })
                    .ToListAsync();

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

        /// <summary>
        /// Метод подтвердит акт продавца.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <param name="documentType">Тип документа, который нужно подтвердить.</param>
        /// <returns>Флаг подтверждения.</returns>
        public async Task<bool> ApproveActVendorAsync(long documentItemId, string documentType)
        {
            try
            {
                // Если не передан Id документа предмета сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                if (!new[] { "DocumentVendorAct1", "DocumentVendorAct2", "DocumentVendorAct3", "DocumentVendorAct4", "DocumentVendorAct5", "DocumentVendorAct6", "DocumentVendorAct7", "DocumentVendorAct8", "DocumentVendorAct9", "DocumentVendorAct10" }.Contains(documentType))
                {
                    throw new ErrorApproveDocumentTypeException("Тип документа отличается от акта продавца");
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && d.IsSend == true
                                && d.DocumentType.Equals(documentType)
                                && d.IsApproveDocument == false
                                && d.IsPay == false
                                && d.IsRejectDocument == false
                                && d.IsDealDocument == true)
                    .FirstOrDefaultAsync();

                // Если акт найден, то подтвердит его.
                if (result != null)
                {
                    result.IsApproveDocument = true;
                    await _postgreDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит список подтвержденных актов продавца.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <returns>Список актов.</returns>
        public async Task<IEnumerable<DocumentOutput>> GetApproveVendorActsAsync(long documentItemId)
        {
            try
            {
                // Если не передан Id документа предмета сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && new[]
                                {
                                    "DocumentVendorAct1", "DocumentVendorAct2", "DocumentVendorAct3",
                                    "DocumentVendorAct4", "DocumentVendorAct5", "DocumentVendorAct6",
                                    "DocumentVendorAct7", "DocumentVendorAct8", "DocumentVendorAct9",
                                    "DocumentVendorAct10"
                                }.Contains(d.DocumentType)
                                && d.IsDealDocument == true
                                && d.IsSend == true
                                && d.IsApproveDocument == true
                                && d.IsPay == false
                                && d.IsRejectDocument == false)
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName,
                        DocumentType = d.DocumentType
                    })
                    .ToListAsync();

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

        /// <summary>
        /// Метод подтвердит акт покупателя.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <param name="documentType">Тип документа, который нужно подтвердить.</param>
        /// <returns>Флаг подтверждения.</returns>
        public async Task<bool> ApproveActCustomerAsync(long documentItemId, string documentType)
        {
            try
            {
                // Если не передан Id документа предмета сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                if (!new[] { "DocumentCustomerAct1", "DocumentCustomerAct2", "DocumentCustomerAct3", "DocumentCustomerAct4", "DocumentCustomerAct5", "DocumentCustomerAct6", "DocumentCustomerAct7", "DocumentCustomerAct8", "DocumentCustomerAct9", "DocumentCustomerAct10" }.Contains(documentType))
                {
                    throw new ErrorApproveDocumentTypeException("Тип документа отличается от акта покупателя");
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && d.IsSend == true
                                && d.DocumentType.Equals(documentType)
                                && d.IsApproveDocument == false
                                && d.IsPay == false
                                && d.IsRejectDocument == false
                                && d.IsDealDocument == true)
                    .FirstOrDefaultAsync();

                // Если акт найден, то подтвердит его.
                if (result != null)
                {
                    result.IsApproveDocument = true;
                    await _postgreDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит список подтвержденных актов покупателя.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <returns>Список актов.</returns>
        public async Task<IEnumerable<DocumentOutput>> GetApproveCustomerActsAsync(long documentItemId)
        {
            try
            {
                // Если не передан Id документа предмета сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && new[]
                                {
                                    "DocumentCustomerAct1", "DocumentCustomerAct2", "DocumentCustomerAct3", "DocumentCustomerAct4", "DocumentCustomerAct5", "DocumentCustomerAct6", "DocumentCustomerAct7", "DocumentCustomerAct8", "DocumentCustomerAct9", "DocumentCustomerAct10"
                                }.Contains(d.DocumentType)
                                && d.IsDealDocument == true
                                && d.IsSend == true
                                && d.IsApproveDocument == true
                                && d.IsPay == false
                                && d.IsRejectDocument == false)
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName,
                        DocumentType = d.DocumentType
                    })
                    .ToListAsync();

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
