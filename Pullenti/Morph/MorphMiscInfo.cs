/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Morph
{
    /// <summary>
    /// Дополнительная морфологическая информация
    /// </summary>
    public class MorphMiscInfo
    {
        /// <summary>
        /// Дополнительные атрибуты
        /// </summary>
        public ICollection<string> Attrs
        {
            get
            {
                return m_Attrs;
            }
        }
        List<string> m_Attrs = new List<string>();
        public void AddAttr(string a)
        {
            if (!m_Attrs.Contains(a)) 
                m_Attrs.Add(a);
        }
        public short Value;
        bool GetBoolValue(int i)
        {
            return ((((Value >> i)) & 1)) != 0;
        }
        void SetBoolValue(int i, bool val)
        {
            if (val) 
                Value |= ((short)(1 << i));
            else 
                Value &= ((short)(~(1 << i)));
        }
        public void CopyFrom(MorphMiscInfo src)
        {
            Value = src.Value;
            foreach (string a in src.Attrs) 
            {
                m_Attrs.Add(a);
            }
        }
        public MorphMiscInfo Clone()
        {
            MorphMiscInfo res = new MorphMiscInfo();
            res.Value = Value;
            res.m_Attrs.AddRange(m_Attrs);
            return res;
        }
        /// <summary>
        /// Лицо
        /// </summary>
        public MorphPerson Person
        {
            get
            {
                MorphPerson res = MorphPerson.Undefined;
                if (m_Attrs.Contains("1 л.")) 
                    res |= MorphPerson.First;
                if (m_Attrs.Contains("2 л.")) 
                    res |= MorphPerson.Second;
                if (m_Attrs.Contains("3 л.")) 
                    res |= MorphPerson.Third;
                return res;
            }
            set
            {
                if (((value & MorphPerson.First)) != MorphPerson.Undefined) 
                    this.AddAttr("1 л.");
                if (((value & MorphPerson.Second)) != MorphPerson.Undefined) 
                    this.AddAttr("2 л.");
                if (((value & MorphPerson.Third)) != MorphPerson.Undefined) 
                    this.AddAttr("3 л.");
            }
        }
        /// <summary>
        /// Время (для глаголов)
        /// </summary>
        public MorphTense Tense
        {
            get
            {
                if (m_Attrs.Contains("п.вр.")) 
                    return MorphTense.Past;
                if (m_Attrs.Contains("н.вр.")) 
                    return MorphTense.Present;
                if (m_Attrs.Contains("б.вр.")) 
                    return MorphTense.Future;
                return MorphTense.Undefined;
            }
            set
            {
                if (value == MorphTense.Past) 
                    this.AddAttr("п.вр.");
                if (value == MorphTense.Present) 
                    this.AddAttr("н.вр.");
                if (value == MorphTense.Future) 
                    this.AddAttr("б.вр.");
            }
        }
        /// <summary>
        /// Аспект (совершенный - несовершенный)
        /// </summary>
        public MorphAspect Aspect
        {
            get
            {
                if (m_Attrs.Contains("нес.в.")) 
                    return MorphAspect.Imperfective;
                if (m_Attrs.Contains("сов.в.")) 
                    return MorphAspect.Perfective;
                return MorphAspect.Undefined;
            }
            set
            {
                if (value == MorphAspect.Imperfective) 
                    this.AddAttr("нес.в.");
                if (value == MorphAspect.Perfective) 
                    this.AddAttr("сов.в.");
            }
        }
        /// <summary>
        /// Наклонение (для глаголов)
        /// </summary>
        public MorphMood Mood
        {
            get
            {
                if (m_Attrs.Contains("пов.накл.")) 
                    return MorphMood.Imperative;
                return MorphMood.Undefined;
            }
            set
            {
                if (value == MorphMood.Imperative) 
                    this.AddAttr("пов.накл.");
            }
        }
        /// <summary>
        /// Залог (для глаголов)
        /// </summary>
        public MorphVoice Voice
        {
            get
            {
                if (m_Attrs.Contains("дейст.з.")) 
                    return MorphVoice.Active;
                if (m_Attrs.Contains("страд.з.")) 
                    return MorphVoice.Passive;
                return MorphVoice.Undefined;
            }
            set
            {
                if (value == MorphVoice.Active) 
                    this.AddAttr("дейст.з.");
                if (value == MorphVoice.Passive) 
                    this.AddAttr("страд.з.");
            }
        }
        /// <summary>
        /// Форма (краткая, синонимичная)
        /// </summary>
        public MorphForm Form
        {
            get
            {
                if (m_Attrs.Contains("к.ф.")) 
                    return MorphForm.Short;
                if (m_Attrs.Contains("синоним.форма")) 
                    return MorphForm.Synonym;
                if (IsSynonymForm) 
                    return MorphForm.Synonym;
                return MorphForm.Undefined;
            }
        }
        /// <summary>
        /// Синонимическая форма
        /// </summary>
        public bool IsSynonymForm
        {
            get
            {
                return this.GetBoolValue(0);
            }
            set
            {
                this.SetBoolValue(0, value);
            }
        }
        public override string ToString()
        {
            if (m_Attrs.Count == 0 && Value == 0) 
                return "";
            StringBuilder res = new StringBuilder();
            if (IsSynonymForm) 
                res.Append("синоним.форма ");
            for (int i = 0; i < m_Attrs.Count; i++) 
            {
                res.AppendFormat("{0} ", m_Attrs[i]);
            }
            return res.ToString().TrimEnd();
        }
        public int Id;
        internal void Deserialize(Pullenti.Morph.Internal.ByteArrayWrapper str, ref int pos)
        {
            int sh = str.DeserializeShort(ref pos);
            Value = (short)sh;
            while (true) 
            {
                string s = str.DeserializeString(ref pos);
                if (string.IsNullOrEmpty(s)) 
                    break;
                if (!m_Attrs.Contains(s)) 
                    m_Attrs.Add(s);
            }
        }
    }
}