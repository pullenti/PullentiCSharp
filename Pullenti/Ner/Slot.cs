/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner
{
    /// <summary>
    /// Значение атрибута в конкретном экземпляре сущности
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// Имя атрибута
        /// </summary>
        public string TypeName
        {
            get;
            set;
        }
        public bool IsInternal
        {
            get
            {
                return TypeName != null && TypeName[0] == '@';
            }
        }
        /// <summary>
        /// Ссылка на сущность-владельца
        /// </summary>
        public Referent Owner
        {
            get;
            set;
        }
        object m_Value;
        /// <summary>
        /// Значение атрибута
        /// </summary>
        public object Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
                if (m_Value != null) 
                {
                    if (m_Value is Referent) 
                    {
                    }
                    else if (m_Value is Token) 
                    {
                    }
                    else if (m_Value is string) 
                    {
                    }
                    else 
                        m_Value = m_Value.ToString();
                }
                else 
                {
                }
            }
        }
        /// <summary>
        /// Статистика встречаемости в сущности, когда сущность в нескольких местах текста. 
        /// Используется, например, для имён организаций, чтобы статистически определить 
        /// правильное написание имени.
        /// </summary>
        public int Count
        {
            get;
            set;
        }
        /// <summary>
        /// Ссылка на атрибут метамодели
        /// </summary>
        public Pullenti.Ner.Metadata.Feature DefiningFeature
        {
            get
            {
                if (Owner == null) 
                    return null;
                if (Owner.InstanceOf == null) 
                    return null;
                return Owner.InstanceOf.FindFeature(TypeName);
            }
        }
        public override string ToString()
        {
            return this.ToString(Pullenti.Morph.MorphLang.Unknown);
        }
        public string ToString(Pullenti.Morph.MorphLang lang)
        {
            StringBuilder res = new StringBuilder();
            Pullenti.Ner.Metadata.Feature attr = DefiningFeature;
            if (attr != null) 
            {
                if (Count > 0) 
                    res.AppendFormat("{0} ({1}): ", attr.Caption, Count);
                else 
                    res.AppendFormat("{0}: ", attr.Caption);
            }
            else 
                res.AppendFormat("{0}: ", TypeName);
            if (Value != null) 
            {
                if (Value is Referent) 
                    res.Append((Value as Referent).ToString(false, lang, 0));
                else if (attr == null) 
                    res.Append(Value.ToString());
                else 
                    res.Append(attr.ConvertInnerValueToOuterValue(Value.ToString(), null));
            }
            return res.ToString();
        }
        /// <summary>
        /// Преобразовать внутреннее значение в строку указанного языка
        /// </summary>
        /// <param name="lang">язык</param>
        /// <return>значение</return>
        public string ConvertValueToString(Pullenti.Morph.MorphLang lang)
        {
            if (Value == null) 
                return null;
            Pullenti.Ner.Metadata.Feature attr = DefiningFeature;
            if (attr == null) 
                return Value.ToString();
            object v = (object)attr.ConvertInnerValueToOuterValue(Value.ToString(), lang);
            if (v == null) 
                return null;
            if (v is string) 
                return v as string;
            else 
                return v.ToString();
        }
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag
        {
            get;
            set;
        }
    }
}