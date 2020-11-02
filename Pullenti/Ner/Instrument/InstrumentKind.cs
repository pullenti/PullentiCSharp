/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Instrument
{
    /// <summary>
    /// Классы фрагментов документа
    /// </summary>
    public enum InstrumentKind : int
    {
        /// <summary>
        /// Неизвестно
        /// </summary>
        Undefined,
        /// <summary>
        /// Корневой документ
        /// </summary>
        Document,
        /// <summary>
        /// Внутренний документ (например, который утверждается)
        /// </summary>
        InternalDocument,
        /// <summary>
        /// Заголовочная часть
        /// </summary>
        Head,
        /// <summary>
        /// Элемент с основным содержимым
        /// </summary>
        Content,
        /// <summary>
        /// Хвостовая часть
        /// </summary>
        Tail,
        /// <summary>
        /// Приложение
        /// </summary>
        Appendix,
        /// <summary>
        /// Часть документа (деление самого верхнего уровня)
        /// </summary>
        DocPart,
        /// <summary>
        /// Раздел
        /// </summary>
        Section,
        /// <summary>
        /// Подраздел
        /// </summary>
        Subsection,
        /// <summary>
        /// Глава
        /// </summary>
        Chapter,
        /// <summary>
        /// Параграф
        /// </summary>
        Paragraph,
        /// <summary>
        /// Подпараграф
        /// </summary>
        Subparagraph,
        /// <summary>
        /// Статья
        /// </summary>
        Clause,
        /// <summary>
        /// Часть статьи
        /// </summary>
        ClausePart,
        /// <summary>
        /// Пункт
        /// </summary>
        Item,
        /// <summary>
        /// Подпункт
        /// </summary>
        Subitem,
        /// <summary>
        /// Абзац
        /// </summary>
        Indention,
        /// <summary>
        /// Элемент списка
        /// </summary>
        ListItem,
        /// <summary>
        /// Заголовок списка (первый абзац перед перечислением)
        /// </summary>
        ListHead,
        /// <summary>
        /// Преамбула
        /// </summary>
        Preamble,
        /// <summary>
        /// Оглавление
        /// </summary>
        Index,
        /// <summary>
        /// Элемент оглавления
        /// </summary>
        IndexItem,
        /// <summary>
        /// Примечание
        /// </summary>
        Notice,
        /// <summary>
        /// Номер
        /// </summary>
        Number,
        /// <summary>
        /// Номер дела (для судебных документов)
        /// </summary>
        CaseNumber,
        /// <summary>
        /// Дополнительная информация (для судебных документов)
        /// </summary>
        CaseInfo,
        /// <summary>
        /// Наименование
        /// </summary>
        Name,
        /// <summary>
        /// Тип
        /// </summary>
        Typ,
        /// <summary>
        /// Подписант
        /// </summary>
        Signer,
        /// <summary>
        /// Организация
        /// </summary>
        Organization,
        /// <summary>
        /// Место
        /// </summary>
        Place,
        /// <summary>
        /// Дата-время
        /// </summary>
        Date,
        /// <summary>
        /// Контактные данные
        /// </summary>
        Contact,
        /// <summary>
        /// Инициатор
        /// </summary>
        Initiator,
        /// <summary>
        /// Директива
        /// </summary>
        Directive,
        /// <summary>
        /// Редакции
        /// </summary>
        Editions,
        /// <summary>
        /// Одобрен, утвержден
        /// </summary>
        Approved,
        /// <summary>
        /// Ссылка на документ
        /// </summary>
        DocReference,
        /// <summary>
        /// Ключевое слово (типа Приложение и т.п.)
        /// </summary>
        Keyword,
        /// <summary>
        /// Комментарий
        /// </summary>
        Comment,
        /// <summary>
        /// Цитата
        /// </summary>
        Citation,
        /// <summary>
        /// Вопрос
        /// </summary>
        Question,
        /// <summary>
        /// Ответ
        /// </summary>
        Answer,
        /// <summary>
        /// Таблица
        /// </summary>
        Table,
        /// <summary>
        /// Строка таблицы
        /// </summary>
        TableRow,
        /// <summary>
        /// Ячейка таблицы
        /// </summary>
        TableCell,
        /// <summary>
        /// Для внутреннего использования
        /// </summary>
        Ignored,
    }
}