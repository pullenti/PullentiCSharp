/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Keyword
{
    /// <summary>
    /// Ключевая комбинация
    /// </summary>
    public class KeywordReferent : Pullenti.Ner.Referent
    {
        public KeywordReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Keyword.Internal.KeywordMeta.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("KEYWORD")
        /// </summary>
        public const string OBJ_TYPENAME = "KEYWORD";
        /// <summary>
        /// Имя атрибута - тип (KeywordType)
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - значение
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - нормализованное значение
        /// </summary>
        public const string ATTR_NORMAL = "NORMAL";
        /// <summary>
        /// Имя атрибута - ссылка на сущность, если это она
        /// </summary>
        public const string ATTR_REF = "REF";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            if (lev > 10) 
                return "?";
            double rank = Rank;
            string val = this.GetStringValue(ATTR_VALUE);
            if (val == null) 
            {
                Pullenti.Ner.Referent r = this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
                if (r != null) 
                    val = r.ToString(true, lang, lev + 1);
                else 
                    val = this.GetStringValue(ATTR_NORMAL);
            }
            if (shortVariant) 
                return val ?? "?";
            string norm = this.GetStringValue(ATTR_NORMAL);
            if (norm == null) 
                return val ?? "?";
            else 
                return string.Format("{0} [{1}]", val ?? "?", norm);
        }
        /// <summary>
        /// Вычисляемый ранг (в атрибутах не сохраняется - просто поле!)
        /// </summary>
        public double Rank;
        /// <summary>
        /// Тип ключевой комбинации
        /// </summary>
        public KeywordType Typ
        {
            get
            {
                string str = this.GetStringValue(ATTR_TYPE);
                if (str == null) 
                    return KeywordType.Undefined;
                try 
                {
                    return (KeywordType)Enum.Parse(typeof(KeywordType), str, true);
                }
                catch(Exception ex) 
                {
                    return KeywordType.Undefined;
                }
            }
            set
            {
                this.AddSlot(ATTR_TYPE, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Ненормализованное значение
        /// </summary>
        public string Value
        {
            get
            {
                return this.GetStringValue(ATTR_VALUE);
            }
            set
            {
                this.AddSlot(ATTR_VALUE, value, false, 0);
            }
        }
        /// <summary>
        /// Нормализованное значение
        /// </summary>
        public string NormalValue
        {
            get
            {
                return this.GetStringValue(ATTR_NORMAL);
            }
            set
            {
                this.AddSlot(ATTR_NORMAL, value, false, 0);
            }
        }
        public int ChildWords
        {
            get
            {
                return this._getChildWords(this, 0);
            }
        }
        int _getChildWords(KeywordReferent root, int lev)
        {
            if (lev > 5) 
                return 0;
            int res = 0;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_REF && (s.Value is KeywordReferent)) 
                {
                    if (s.Value == root) 
                        return 0;
                    res += (s.Value as KeywordReferent)._getChildWords(root, lev + 1);
                }
            }
            if (res == 0) 
                res = 1;
            return res;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            KeywordReferent kw = obj as KeywordReferent;
            if (kw == null) 
                return false;
            KeywordType ki = Typ;
            if (ki != kw.Typ) 
                return false;
            if (ki == KeywordType.Referent) 
            {
                Pullenti.Ner.Referent re = this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
                if (re == null) 
                    return false;
                Pullenti.Ner.Referent re2 = kw.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
                if (re2 == null) 
                    return false;
                if (re.CanBeEquals(re2, typ)) 
                    return true;
            }
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NORMAL || s.TypeName == ATTR_VALUE) 
                {
                    if (kw.FindSlot(ATTR_NORMAL, s.Value, true) != null) 
                        return true;
                    if (kw.FindSlot(ATTR_VALUE, s.Value, true) != null) 
                        return true;
                }
            }
            return false;
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            double r1 = Rank + (obj as KeywordReferent).Rank;
            base.MergeSlots(obj, mergeStatistic);
            if (Slots.Count > 50) 
            {
            }
            Rank = r1;
        }
        internal void Union(KeywordReferent kw1, KeywordReferent kw2, string word2)
        {
            Typ = kw1.Typ;
            List<string> tmp = new List<string>();
            StringBuilder tmp2 = new StringBuilder();
            foreach (string v in kw1.GetStringValues(ATTR_VALUE)) 
            {
                this.AddSlot(ATTR_VALUE, string.Format("{0} {1}", v, word2), false, 0);
            }
            List<string> norms1 = kw1.GetStringValues(ATTR_NORMAL);
            if (norms1.Count == 0 && kw1.ChildWords == 1) 
                norms1 = kw1.GetStringValues(ATTR_VALUE);
            List<string> norms2 = kw2.GetStringValues(ATTR_NORMAL);
            if (norms2.Count == 0 && kw2.ChildWords == 1) 
                norms2 = kw2.GetStringValues(ATTR_VALUE);
            foreach (string n1 in norms1) 
            {
                foreach (string n2 in norms2) 
                {
                    tmp.Clear();
                    tmp.AddRange(n1.Split(' '));
                    foreach (string n in n2.Split(' ')) 
                    {
                        if (!tmp.Contains(n)) 
                            tmp.Add(n);
                    }
                    tmp.Sort();
                    tmp2.Length = 0;
                    for (int i = 0; i < tmp.Count; i++) 
                    {
                        if (i > 0) 
                            tmp2.Append(' ');
                        tmp2.Append(tmp[i]);
                    }
                    this.AddSlot(ATTR_NORMAL, tmp2.ToString(), false, 0);
                }
            }
            this.AddSlot(ATTR_REF, kw1, false, 0);
            this.AddSlot(ATTR_REF, kw2, false, 0);
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem res = new Pullenti.Ner.Core.IntOntologyItem(this);
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NORMAL || s.TypeName == ATTR_VALUE) 
                    res.Termins.Add(new Pullenti.Ner.Core.Termin((string)s.Value));
            }
            return res;
        }
    }
}