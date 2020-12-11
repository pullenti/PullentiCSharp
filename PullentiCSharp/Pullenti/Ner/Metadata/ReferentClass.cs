/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Metadata
{
    /// <summary>
    /// Описатель класса сущностей
    /// </summary>
    public class ReferentClass
    {
        /// <summary>
        /// Строковый идентификатор
        /// </summary>
        public virtual string Name
        {
            get
            {
                return "?";
            }
        }
        /// <summary>
        /// Заголовок (зависит от текущего языка)
        /// </summary>
        public virtual string Caption
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// Не выводить на графе объектов
        /// </summary>
        public bool HideInGraph = false;
        public override string ToString()
        {
            return Caption ?? Name;
        }
        /// <summary>
        /// Атрибуты класса
        /// </summary>
        public List<Feature> Features
        {
            get
            {
                return m_Features;
            }
        }
        List<Feature> m_Features = new List<Feature>();
        /// <summary>
        /// Добавить атрибут
        /// </summary>
        public Feature AddFeature(string attrName, string attrCaption, int lowBound = 0, int upBound = 0)
        {
            Feature res = new Feature() { Name = attrName, Caption = attrCaption, LowerBound = lowBound, UpperBound = upBound };
            int ind = m_Features.Count;
            m_Features.Add(res);
            if (!m_Hash.ContainsKey(attrName)) 
                m_Hash.Add(attrName, ind);
            else 
                m_Hash[attrName] = ind;
            return m_Features[ind];
        }
        /// <summary>
        /// Найти атрибут по его системному имени
        /// </summary>
        public Feature FindFeature(string name)
        {
            int ind;
            if (!m_Hash.TryGetValue(name, out ind)) 
                return null;
            else 
                return m_Features[ind];
        }
        Dictionary<string, int> m_Hash = new Dictionary<string, int>();
        /// <summary>
        /// Вычислить картинку
        /// </summary>
        /// <param name="obj">если null, то общая картинка для типа</param>
        /// <return>идентификатор картинки, саму картинку можно будет получить через ProcessorService.GetImageById</return>
        public virtual string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return null;
        }
    }
}