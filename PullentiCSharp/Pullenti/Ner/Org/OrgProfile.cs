/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Org
{
    /// <summary>
    /// Профили организации, хранятся в атрибутах ATTR_PROFILE, может быть несколько.
    /// </summary>
    public enum OrgProfile : int
    {
        /// <summary>
        /// Неопределённое
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Подразделение, отдел
        /// </summary>
        Unit,
        /// <summary>
        /// Различные объединения людей (фонды, движения, партии, ассоциации)
        /// </summary>
        Union,
        /// <summary>
        /// Соревнование, конкурс, чемпионат
        /// </summary>
        Competition,
        /// <summary>
        /// Группы компаний, холдинги
        /// </summary>
        Holding,
        /// <summary>
        /// Государственные
        /// </summary>
        State,
        /// <summary>
        /// Бизнес, коммерция
        /// </summary>
        Business,
        /// <summary>
        /// Финансы (банки, фонды)
        /// </summary>
        Finance,
        /// <summary>
        /// Образование
        /// </summary>
        Education,
        /// <summary>
        /// Наука
        /// </summary>
        Science,
        /// <summary>
        /// Производство
        /// </summary>
        Industry,
        /// <summary>
        /// Торговля, реализация
        /// </summary>
        Trade,
        /// <summary>
        /// Медицина
        /// </summary>
        Medicine,
        /// <summary>
        /// Политика
        /// </summary>
        Policy,
        /// <summary>
        /// Судебная система
        /// </summary>
        Justice,
        /// <summary>
        /// Силовые структуры
        /// </summary>
        Enforcement,
        /// <summary>
        /// Армейские структуры
        /// </summary>
        Army,
        /// <summary>
        /// Спорт
        /// </summary>
        Sport,
        /// <summary>
        /// Религиозные
        /// </summary>
        Religion,
        /// <summary>
        /// Искусство
        /// </summary>
        Art,
        /// <summary>
        /// Музыка, группы
        /// </summary>
        Music,
        /// <summary>
        /// Театры, выставки, музеи, концерты
        /// </summary>
        Show,
        /// <summary>
        /// Срадства массовой информации
        /// </summary>
        Media,
        /// <summary>
        /// Издательства, газеты, журналы ... (обычно вместе с Media)
        /// </summary>
        Press,
        /// <summary>
        /// пансионаты, отели, дома отдыха
        /// </summary>
        Hotel,
        /// <summary>
        /// Предприятия питания
        /// </summary>
        Food,
        /// <summary>
        /// Железные дороги, авиакомпании ...
        /// </summary>
        Transport,
    }
}