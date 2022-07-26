﻿using System;

namespace Garant.Platform.Models.Franchise.Output
{
    /// <summary>
    /// Класс выходной модели франшизы.
    /// </summary>
    public class CreateUpdateFranchiseOutput
    {
        /// <summary>
        /// Массив с названиями файлов изображений франшизы.
        /// </summary>
        public string[] UrlsDetailsNames { get; set; }

        /// <summary>
        /// Массив с путями изображений франшизы.
        /// </summary>
        public string[] UrlsDetails { get; set; }

        /// <summary>
        /// Путь файла логотипа франшизы.
        /// </summary>
        public string FileLogoUrl { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Текст до цены.
        /// </summary>
        public string TextDoPrice { get; set; }

        /// <summary>
        /// Категория франшизы.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Подкатегория.
        /// </summary>
        public string SubCategory { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        public string ViewBusiness { get; set; }

        /// <summary>
        /// Покупка через гарант.
        /// </summary>
        public bool IsGarant { get; set; }

        public string City { get; set; }

        /// <summary>
        /// Желаемая прибыль в мес.
        /// </summary>
        public double ProfitPrice { get; set; }

        /// <summary>
        /// Статус или должность.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Сумма общих инвестиций (включая паушальный взнос).
        /// </summary>
        public double GeneralInvest { get; set; }

        /// <summary>
        /// Паушальный взнос (зависит от выбранного пакета).
        /// </summary>
        public double LumpSumPayment { get; set; }

        /// <summary>
        /// Роялти (от валовой выручки).
        /// </summary>
        public double Royalty { get; set; }

        /// <summary>
        /// Окупаемость (средняя и планируемая). Кол-во мес.
        /// </summary>
        public int Payback { get; set; }

        /// <summary>
        /// Месячная прибыль (планируемая чистая прибыль).
        /// </summary>
        public double ProfitMonth { get; set; }

        /// <summary>
        /// Срок запуска (средний срок открытия бизнеса).
        /// </summary>
        public int LaunchDate { get; set; }

        /// <summary>
        /// Описание деятельности.
        /// </summary>
        public string ActivityDetail { get; set; }

        /// <summary>
        /// Входит в инвестиции (json).
        /// </summary>
        public string InvestInclude { get; set; }

        /// <summary>
        /// Год основания.
        /// </summary>
        public int BaseDate { get; set; }

        /// <summary>
        /// Год запуска.
        /// </summary>
        public int YearStart { get; set; }

        /// <summary>
        /// Кол-во точек.
        /// </summary>
        public int DotCount { get; set; }

        /// <summary>
        /// Кол-во собственных предприятий.
        /// </summary>
        public int BusinessCount { get; set; }

        /// <summary>
        /// Особенность франшизы.
        /// </summary>
        public string Peculiarity { get; set; }

        /// <summary>
        /// Путь файла финансовой модели.
        /// </summary>
        public string FinModelFileUrl { get; set; }

        /// <summary>
        /// Путь файла презентации.
        /// </summary>
        public string PresentFileUrl { get; set; }

        /// <summary>
        /// Путь к фото франшизы.
        /// </summary>
        public string FranchisePhotoUrl { get; set; }

        /// <summary>
        /// Описание расчета.
        /// </summary>
        public string PaymentDetail { get; set; }

        /// <summary>
        /// Название финансовых показателей.
        /// </summary>
        public string NameFinIndicators { get; set; }

        /// <summary>
        /// Список фин.показателей (json).
        /// </summary>
        public string FinIndicators { get; set; }

        /// <summary>
        /// Описание обучения.
        /// </summary>
        public string TrainingDetails { get; set; }

        /// <summary>
        /// Путь к фото обучения.
        /// </summary>
        public string TrainingPhotoUrl { get; set; }

        /// <summary>
        /// Пакеты франшизы (json).
        /// </summary>
        public string FranchisePacks { get; set; }

        /// <summary>
        /// Ссылка на видео о франшизе.
        /// </summary>
        public string UrlVideo { get; set; }

        /// <summary>
        /// Отзывы о франшизе (json).
        /// </summary>
        public string Reviews { get; set; }

        /// <summary>
        /// Id франшизы.
        /// </summary>
        public long FranchiseId { get; set; }
    }
}
