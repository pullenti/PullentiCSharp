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
    /// Категории организаций. Не хранятся, а вычисляются на основе других атрибутов.
    /// </summary>
    public enum OrganizationKind : int
    {
        /// <summary>
        /// Неопределённая
        /// </summary>
        Undefined,
        /// <summary>
        /// Правительственная
        /// </summary>
        Govenment,
        /// <summary>
        /// Политическая
        /// </summary>
        Party,
        /// <summary>
        /// Образовательная
        /// </summary>
        Study,
        /// <summary>
        /// Научно-исследовательская
        /// </summary>
        Science,
        /// <summary>
        /// Пресса
        /// </summary>
        Press,
        /// <summary>
        /// Масс-медиа
        /// </summary>
        Media,
        /// <summary>
        /// Производственная
        /// </summary>
        Factory,
        /// <summary>
        /// Банковская
        /// </summary>
        Bank,
        /// <summary>
        /// Культурная
        /// </summary>
        Culture,
        /// <summary>
        /// Медицинская
        /// </summary>
        Medical,
        /// <summary>
        /// Торговая
        /// </summary>
        Trade,
        /// <summary>
        /// Холдинг
        /// </summary>
        Holding,
        /// <summary>
        /// Подразделение
        /// </summary>
        Department,
        /// <summary>
        /// Федерация, Союз и т.п. непонятность
        /// </summary>
        Federation,
        /// <summary>
        /// Отели, Санатории, Пансионаты ...
        /// </summary>
        Hotel,
        /// <summary>
        /// Суды, тюрьмы ...
        /// </summary>
        Justice,
        /// <summary>
        /// Церкви, религиозное
        /// </summary>
        Church,
        /// <summary>
        /// Военные
        /// </summary>
        Military,
        /// <summary>
        /// Аэропорт
        /// </summary>
        Airport,
        /// <summary>
        /// Морские порты
        /// </summary>
        Seaport,
        /// <summary>
        /// События (фестиваль, чемпионат)
        /// </summary>
        Festival,
    }
}