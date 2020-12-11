/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti
{
    /// <summary>
    /// Инициализация SDK Pullenti
    /// </summary>
    public static class Sdk
    {
        /// <summary>
        /// Версия SDK Pullenti
        /// </summary>
        public static string Version
        {
            get
            {
                return Pullenti.Ner.ProcessorService.Version;
            }
        }
        /// <summary>
        /// Дата выпуска версии SDK
        /// </summary>
        public static string VersionDate
        {
            get
            {
                return Pullenti.Ner.ProcessorService.VersionDate;
            }
        }
        /// <summary>
        /// Инициализация всего SDK и на всех поддержанных языках. 
        /// Вызывать в самом начале работы. Инициализируется морфология (MorphologyService), 
        /// служба процессоров (ProcessorService), все доступные анализаторы сущностей и 
        /// семантический анализ (SemanticService). Так что больше ничего инициализировать не нужно.
        /// </summary>
        public static void InitializeAll()
        {
            Initialize(Pullenti.Morph.MorphLang.RU | Pullenti.Morph.MorphLang.UA | Pullenti.Morph.MorphLang.EN);
        }
        /// <summary>
        /// Инициализация SDK. 
        /// Вызывать в самом начале работы. Инициализируется морфология (MorphologyService), 
        /// служба процессоров (ProcessorService), все доступные анализаторы сущностей и 
        /// семантический анализ (SemanticService). Так что больше ничего инициализировать не нужно.
        /// </summary>
        /// <param name="lang">по умолчанию, русский и английский</param>
        public static void Initialize(Pullenti.Morph.MorphLang lang = null)
        {
            // сначала инициализация всего сервиса
            Pullenti.Ner.ProcessorService.Initialize(lang);
            // а затем конкретные анализаторы (какие нужно, в данном случае - все)
            Pullenti.Ner.Money.MoneyAnalyzer.Initialize();
            Pullenti.Ner.Uri.UriAnalyzer.Initialize();
            Pullenti.Ner.Phone.PhoneAnalyzer.Initialize();
            Pullenti.Ner.Date.DateAnalyzer.Initialize();
            Pullenti.Ner.Keyword.KeywordAnalyzer.Initialize();
            Pullenti.Ner.Definition.DefinitionAnalyzer.Initialize();
            Pullenti.Ner.Denomination.DenominationAnalyzer.Initialize();
            Pullenti.Ner.Measure.MeasureAnalyzer.Initialize();
            Pullenti.Ner.Bank.BankAnalyzer.Initialize();
            Pullenti.Ner.Geo.GeoAnalyzer.Initialize();
            Pullenti.Ner.Address.AddressAnalyzer.Initialize();
            Pullenti.Ner.Org.OrganizationAnalyzer.Initialize();
            Pullenti.Ner.Person.PersonAnalyzer.Initialize();
            Pullenti.Ner.Mail.MailAnalyzer.Initialize();
            Pullenti.Ner.Transport.TransportAnalyzer.Initialize();
            Pullenti.Ner.Decree.DecreeAnalyzer.Initialize();
            Pullenti.Ner.Instrument.InstrumentAnalyzer.Initialize();
            Pullenti.Ner.Titlepage.TitlePageAnalyzer.Initialize();
            Pullenti.Ner.Booklink.BookLinkAnalyzer.Initialize();
            Pullenti.Ner.Goods.GoodsAnalyzer.Initialize();
            Pullenti.Ner.Named.NamedEntityAnalyzer.Initialize();
            Pullenti.Ner.Weapon.WeaponAnalyzer.Initialize();
            // ещё инициализируем семантическую обработки (в принципе, она не используется для задачи NER)
            Pullenti.Semantic.SemanticService.Initialize();
        }
    }
}