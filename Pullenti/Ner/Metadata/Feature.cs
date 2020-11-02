/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Metadata
{
    /// <summary>
    /// Атрибут класса сущностей
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// Внутреннее имя
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Caption
        {
            get;
            set;
        }
        /// <summary>
        /// Минимальное количество
        /// </summary>
        public int LowerBound
        {
            get;
            set;
        }
        /// <summary>
        /// Максимальное количество (0 - неограничено)
        /// </summary>
        public int UpperBound
        {
            get;
            set;
        }
        /// <summary>
        /// Это для внутреннего использования
        /// </summary>
        public bool ShowAsParent
        {
            get;
            set;
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder(Caption ?? Name);
            if (UpperBound > 0 || LowerBound > 0) 
            {
                if (UpperBound == 0) 
                    res.AppendFormat("[{0}..*]", LowerBound);
                else if (UpperBound == LowerBound) 
                    res.AppendFormat("[{0}]", UpperBound);
                else 
                    res.AppendFormat("[{0}..{1}]", LowerBound, UpperBound);
            }
            return res.ToString();
        }
        public List<string> InnerValues = new List<string>();
        public List<string> OuterValues = new List<string>();
        public List<string> OuterValuesEN = new List<string>();
        public List<string> OuterValuesUA = new List<string>();
        public string ConvertInnerValueToOuterValue(string innerValue, Pullenti.Morph.MorphLang lang = null)
        {
            if (innerValue == null) 
                return null;
            string val = innerValue.ToString();
            for (int i = 0; i < InnerValues.Count; i++) 
            {
                if (string.Compare(InnerValues[i], val, true) == 0 && (i < OuterValues.Count)) 
                {
                    if (lang != null) 
                    {
                        if (lang.IsUa && (i < OuterValuesUA.Count) && OuterValuesUA[i] != null) 
                            return OuterValuesUA[i];
                        if (lang.IsEn && (i < OuterValuesEN.Count) && OuterValuesEN[i] != null) 
                            return OuterValuesEN[i];
                    }
                    return OuterValues[i];
                }
            }
            return innerValue;
        }
        public string ConvertOuterValueToInnerValue(string outerValue)
        {
            if (outerValue == null) 
                return null;
            for (int i = 0; i < OuterValues.Count; i++) 
            {
                if (string.Compare(OuterValues[i], outerValue, true) == 0 && (i < InnerValues.Count)) 
                    return InnerValues[i];
                else if ((i < OuterValuesUA.Count) && OuterValuesUA[i] == outerValue) 
                    return InnerValues[i];
            }
            return outerValue;
        }
        public void AddValue(string intVal, string extVal, string extValUa = null, string extValEng = null)
        {
            InnerValues.Add(intVal);
            OuterValues.Add(extVal);
            OuterValuesUA.Add(extValUa);
            OuterValuesEN.Add(extValEng);
        }
    }
}