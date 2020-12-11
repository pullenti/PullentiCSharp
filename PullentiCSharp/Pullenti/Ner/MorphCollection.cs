/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Pullenti.Ner
{
    /// <summary>
    /// Коллекция морфологических вариантов
    /// </summary>
    public class MorphCollection : Pullenti.Morph.MorphBaseInfo
    {
        public MorphCollection(MorphCollection source = null)
        {
            if (source == null) 
                return;
            foreach (Pullenti.Morph.MorphBaseInfo it in source.Items) 
            {
                Pullenti.Morph.MorphBaseInfo mi = null;
                if (it is Pullenti.Morph.MorphWordForm) 
                {
                    Pullenti.Morph.MorphWordForm wf = new Pullenti.Morph.MorphWordForm();
                    wf.CopyFromWordForm(it as Pullenti.Morph.MorphWordForm);
                    mi = wf;
                }
                else 
                {
                    mi = new Pullenti.Morph.MorphBaseInfo();
                    mi.CopyFrom(it);
                }
                if (m_Items == null) 
                    m_Items = new List<Pullenti.Morph.MorphBaseInfo>();
                m_Items.Add(mi);
            }
            m_Class = new Pullenti.Morph.MorphClass() { Value = source.m_Class.Value };
            m_Gender = source.m_Gender;
            m_Case = new Pullenti.Morph.MorphCase() { Value = source.m_Case.Value };
            m_Number = source.m_Number;
            m_Language = new Pullenti.Morph.MorphLang() { Value = source.m_Language.Value };
            m_Voice = source.m_Voice;
            m_NeedRecalc = false;
        }
        public override string ToString()
        {
            string res = base.ToString();
            if (Voice != Pullenti.Morph.MorphVoice.Undefined) 
            {
                if (Voice == Pullenti.Morph.MorphVoice.Active) 
                    res += " действ.з.";
                else if (Voice == Pullenti.Morph.MorphVoice.Passive) 
                    res += " страд.з.";
                else if (Voice == Pullenti.Morph.MorphVoice.Middle) 
                    res += " сред. з.";
            }
            return res;
        }
        Pullenti.Morph.MorphClass m_Class = new Pullenti.Morph.MorphClass();
        Pullenti.Morph.MorphGender m_Gender;
        Pullenti.Morph.MorphNumber m_Number;
        Pullenti.Morph.MorphCase m_Case = new Pullenti.Morph.MorphCase();
        Pullenti.Morph.MorphLang m_Language = new Pullenti.Morph.MorphLang();
        Pullenti.Morph.MorphVoice m_Voice;
        bool m_NeedRecalc = true;
        /// <summary>
        /// Создать копию
        /// </summary>
        public MorphCollection Clone()
        {
            MorphCollection res = new MorphCollection();
            if (m_Items != null) 
            {
                res.m_Items = new List<Pullenti.Morph.MorphBaseInfo>();
                try 
                {
                    res.m_Items.AddRange(m_Items);
                }
                catch(Exception ex) 
                {
                }
            }
            if (!m_NeedRecalc) 
            {
                res.m_Class = new Pullenti.Morph.MorphClass() { Value = m_Class.Value };
                res.m_Gender = m_Gender;
                res.m_Case = new Pullenti.Morph.MorphCase() { Value = m_Case.Value };
                res.m_Number = m_Number;
                res.m_Language = new Pullenti.Morph.MorphLang() { Value = m_Language.Value };
                res.m_NeedRecalc = false;
                res.m_Voice = m_Voice;
            }
            return res;
        }
        /// <summary>
        /// Количество морфологических вариантов
        /// </summary>
        public int ItemsCount
        {
            get
            {
                return (m_Items == null ? 0 : m_Items.Count);
            }
        }
        /// <summary>
        /// Пролучить морфологический вариант
        /// </summary>
        public Pullenti.Morph.MorphBaseInfo this[int ind]
        {
            get
            {
                if (m_Items == null || (ind < 0) || ind >= m_Items.Count) 
                    return null;
                else 
                    return m_Items[ind];
            }
        }
        static List<Pullenti.Morph.MorphBaseInfo> m_EmptyItems = new List<Pullenti.Morph.MorphBaseInfo>();
        /// <summary>
        /// Морфологические варианты
        /// </summary>
        public ICollection<Pullenti.Morph.MorphBaseInfo> Items
        {
            get
            {
                return m_Items ?? m_EmptyItems;
            }
        }
        List<Pullenti.Morph.MorphBaseInfo> m_Items = null;
        public void AddItem(Pullenti.Morph.MorphBaseInfo item)
        {
            if (m_Items == null) 
                m_Items = new List<Pullenti.Morph.MorphBaseInfo>();
            m_Items.Add(item);
            m_NeedRecalc = true;
        }
        public void InsertItem(int ind, Pullenti.Morph.MorphBaseInfo item)
        {
            if (m_Items == null) 
                m_Items = new List<Pullenti.Morph.MorphBaseInfo>();
            m_Items.Insert(ind, item);
            m_NeedRecalc = true;
        }
        public void RemoveItem(int i)
        {
            if (m_Items != null && i >= 0 && (i < m_Items.Count)) 
            {
                m_Items.RemoveAt(i);
                m_NeedRecalc = true;
            }
        }
        public void RemoveItem(Pullenti.Morph.MorphBaseInfo item)
        {
            if (m_Items != null && m_Items.Contains(item)) 
            {
                m_Items.Remove(item);
                m_NeedRecalc = true;
            }
        }
        void _recalc()
        {
            m_NeedRecalc = false;
            if (m_Items == null || m_Items.Count == 0) 
                return;
            m_Class = new Pullenti.Morph.MorphClass();
            m_Gender = Pullenti.Morph.MorphGender.Undefined;
            bool g = m_Gender == Pullenti.Morph.MorphGender.Undefined;
            m_Number = Pullenti.Morph.MorphNumber.Undefined;
            bool n = m_Number == Pullenti.Morph.MorphNumber.Undefined;
            m_Case = new Pullenti.Morph.MorphCase();
            bool ca = m_Case.IsUndefined;
            bool la = m_Language == null || m_Language.IsUndefined;
            m_Voice = Pullenti.Morph.MorphVoice.Undefined;
            bool verbHasUndef = false;
            if (m_Items != null) 
            {
                foreach (Pullenti.Morph.MorphBaseInfo it in m_Items) 
                {
                    m_Class.Value |= it.Class.Value;
                    if (g) 
                        m_Gender |= it.Gender;
                    if (ca) 
                        m_Case |= it.Case;
                    if (n) 
                        m_Number |= it.Number;
                    if (la) 
                        m_Language.Value |= it.Language.Value;
                    if (it.Class.IsVerb) 
                    {
                        if (it is Pullenti.Morph.MorphWordForm) 
                        {
                            Pullenti.Morph.MorphVoice v = (it as Pullenti.Morph.MorphWordForm).Misc.Voice;
                            if (v == Pullenti.Morph.MorphVoice.Undefined) 
                                verbHasUndef = true;
                            else 
                                m_Voice |= v;
                        }
                    }
                }
            }
            if (verbHasUndef) 
                m_Voice = Pullenti.Morph.MorphVoice.Undefined;
        }
        public override Pullenti.Morph.MorphClass Class
        {
            get
            {
                if (m_NeedRecalc) 
                    this._recalc();
                return m_Class;
            }
            set
            {
                m_Class = value;
            }
        }
        public override Pullenti.Morph.MorphCase Case
        {
            get
            {
                if (m_NeedRecalc) 
                    this._recalc();
                return m_Case;
            }
            set
            {
                m_Case = value;
            }
        }
        public override Pullenti.Morph.MorphGender Gender
        {
            get
            {
                if (m_NeedRecalc) 
                    this._recalc();
                return m_Gender;
            }
            set
            {
                m_Gender = value;
            }
        }
        public override Pullenti.Morph.MorphNumber Number
        {
            get
            {
                if (m_NeedRecalc) 
                    this._recalc();
                return m_Number;
            }
            set
            {
                m_Number = value;
            }
        }
        public override Pullenti.Morph.MorphLang Language
        {
            get
            {
                if (m_NeedRecalc) 
                    this._recalc();
                return m_Language;
            }
            set
            {
                m_Language = value;
            }
        }
        /// <summary>
        /// Залог (для глаголов)
        /// </summary>
        public Pullenti.Morph.MorphVoice Voice
        {
            get
            {
                if (m_NeedRecalc) 
                    this._recalc();
                return m_Voice;
            }
            set
            {
                if (m_NeedRecalc) 
                    this._recalc();
                m_Voice = value;
            }
        }
        public override bool ContainsAttr(string attrValue, Pullenti.Morph.MorphClass cla = null)
        {
            foreach (Pullenti.Morph.MorphBaseInfo it in Items) 
            {
                if (cla != null && cla.Value != 0 && ((it.Class.Value & cla.Value)) == 0) 
                    continue;
                if (it.ContainsAttr(attrValue, cla)) 
                    return true;
            }
            return false;
        }
        public override bool CheckAccord(Pullenti.Morph.MorphBaseInfo v, bool ignoreGender = false, bool ignoreNumber = false)
        {
            foreach (Pullenti.Morph.MorphBaseInfo it in Items) 
            {
                if (v is MorphCollection) 
                {
                    if (v.CheckAccord(it, ignoreGender, ignoreNumber)) 
                        return true;
                }
                else if (it.CheckAccord(v, ignoreGender, ignoreNumber)) 
                    return true;
            }
            if (Items.Count > 0) 
                return false;
            return base.CheckAccord(v, ignoreGender, ignoreNumber);
        }
        public bool Check(Pullenti.Morph.MorphClass cl)
        {
            return ((Class.Value & cl.Value)) != 0;
        }
        /// <summary>
        /// Удалить элементы, не соответствующие падежу
        /// </summary>
        public void RemoveItems(Pullenti.Morph.MorphCase cas)
        {
            if (m_Items == null) 
                return;
            if (m_Items.Count == 0) 
                m_Case = m_Case & cas;
            for (int i = m_Items.Count - 1; i >= 0; i--) 
            {
                if (((m_Items[i].Case & cas)).IsUndefined) 
                {
                    m_Items.RemoveAt(i);
                    m_NeedRecalc = true;
                }
                else if (((m_Items[i].Case & cas)) != m_Items[i].Case) 
                {
                    if (m_Items[i] is Pullenti.Morph.MorphWordForm) 
                    {
                        Pullenti.Morph.MorphWordForm wf = new Pullenti.Morph.MorphWordForm();
                        wf.CopyFromWordForm(m_Items[i] as Pullenti.Morph.MorphWordForm);
                        wf.Case &= cas;
                        m_Items[i] = wf;
                    }
                    else 
                    {
                        Pullenti.Morph.MorphBaseInfo bi = new Pullenti.Morph.MorphBaseInfo();
                        bi.CopyFrom(m_Items[i]);
                        bi.Case &= cas;
                        m_Items[i] = bi;
                    }
                    m_NeedRecalc = true;
                }
            }
            m_NeedRecalc = true;
        }
        /// <summary>
        /// Удалить элементы, не соответствующие классу
        /// </summary>
        public void RemoveItems(Pullenti.Morph.MorphClass cl, bool eq = false)
        {
            if (m_Items == null) 
                return;
            for (int i = m_Items.Count - 1; i >= 0; i--) 
            {
                bool ok = false;
                if (((m_Items[i].Class.Value & cl.Value)) == 0) 
                    ok = true;
                else if (eq && m_Items[i].Class.Value != cl.Value) 
                    ok = true;
                if (ok) 
                {
                    m_Items.RemoveAt(i);
                    m_NeedRecalc = true;
                }
            }
            m_NeedRecalc = true;
        }
        /// <summary>
        /// Удалить элементы, не соответствующие параметрам
        /// </summary>
        public void RemoveItems(Pullenti.Morph.MorphBaseInfo inf)
        {
            if (m_Items == null) 
                return;
            if (m_Items.Count == 0) 
            {
                if (inf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                    m_Gender &= inf.Gender;
                if (inf.Number != Pullenti.Morph.MorphNumber.Undefined) 
                    m_Number &= inf.Number;
                if (!inf.Case.IsUndefined) 
                    m_Case &= inf.Case;
                return;
            }
            for (int i = m_Items.Count - 1; i >= 0; i--) 
            {
                bool ok = true;
                Pullenti.Morph.MorphBaseInfo it = m_Items[i];
                if (inf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                {
                    if (((it.Gender & inf.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                        ok = false;
                }
                bool chNum = false;
                if (inf.Number != Pullenti.Morph.MorphNumber.Plural && inf.Number != Pullenti.Morph.MorphNumber.Undefined) 
                {
                    if (((it.Number & inf.Number)) == Pullenti.Morph.MorphNumber.Undefined) 
                        ok = false;
                    chNum = true;
                }
                if (!inf.Class.IsUndefined) 
                {
                    if (((inf.Class & it.Class)).IsUndefined) 
                        ok = false;
                }
                if (!inf.Case.IsUndefined) 
                {
                    if (((inf.Case & it.Case)).IsUndefined) 
                        ok = false;
                }
                if (!ok) 
                {
                    m_Items.RemoveAt(i);
                    m_NeedRecalc = true;
                }
                else 
                {
                    if (!inf.Case.IsUndefined) 
                    {
                        if (it.Case != ((inf.Case & it.Case))) 
                        {
                            it.Case = (inf.Case & it.Case);
                            m_NeedRecalc = true;
                        }
                    }
                    if (inf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        if (it.Gender != ((inf.Gender & it.Gender))) 
                        {
                            it.Gender = (inf.Gender & it.Gender);
                            m_NeedRecalc = true;
                        }
                    }
                    if (chNum) 
                    {
                        if (it.Number != ((inf.Number & it.Number))) 
                        {
                            it.Number = (inf.Number & it.Number);
                            m_NeedRecalc = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Убрать элементы, не соответствующие по падежу предлогу
        /// </summary>
        public void RemoveItemsByPreposition(Token prep)
        {
            if (!(prep is TextToken)) 
                return;
            Pullenti.Morph.MorphCase mc = Pullenti.Morph.LanguageHelper.GetCaseAfterPreposition((prep as TextToken).Lemma);
            if (((mc & Case)).IsUndefined) 
                return;
            this.RemoveItems(mc);
        }
        /// <summary>
        /// Удалить элементы не из словаря (если все не из словаря, то ничего не удаляется). 
        /// То есть оставить только словарный вариант.
        /// </summary>
        public void RemoveNotInDictionaryItems()
        {
            if (m_Items == null) 
                return;
            bool hasInDict = false;
            for (int i = m_Items.Count - 1; i >= 0; i--) 
            {
                if ((m_Items[i] is Pullenti.Morph.MorphWordForm) && (m_Items[i] as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                {
                    hasInDict = true;
                    break;
                }
            }
            if (hasInDict) 
            {
                for (int i = m_Items.Count - 1; i >= 0; i--) 
                {
                    if ((m_Items[i] is Pullenti.Morph.MorphWordForm) && !(m_Items[i] as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    {
                        m_Items.RemoveAt(i);
                        m_NeedRecalc = true;
                    }
                }
            }
        }
        public void RemoveProperItems()
        {
            if (m_Items == null) 
                return;
            for (int i = m_Items.Count - 1; i >= 0; i--) 
            {
                if (m_Items[i].Class.IsProper) 
                {
                    m_Items.RemoveAt(i);
                    m_NeedRecalc = true;
                }
            }
        }
        public void RemoveItems(Pullenti.Morph.MorphNumber num)
        {
            if (m_Items == null) 
                return;
            for (int i = m_Items.Count - 1; i >= 0; i--) 
            {
                if (((m_Items[i].Number & num)) == Pullenti.Morph.MorphNumber.Undefined) 
                {
                    m_Items.RemoveAt(i);
                    m_NeedRecalc = true;
                }
            }
        }
        public void RemoveItems(Pullenti.Morph.MorphGender gen)
        {
            if (m_Items == null) 
                return;
            for (int i = m_Items.Count - 1; i >= 0; i--) 
            {
                if (((m_Items[i].Gender & gen)) == Pullenti.Morph.MorphGender.Undefined) 
                {
                    m_Items.RemoveAt(i);
                    m_NeedRecalc = true;
                }
            }
        }
        /// <summary>
        /// Удалить элементы, не соответствующие заданным параметрам
        /// </summary>
        public void RemoveItemsListCla(ICollection<Pullenti.Morph.MorphBaseInfo> bis, Pullenti.Morph.MorphClass cla)
        {
            if (m_Items == null) 
                return;
            for (int i = m_Items.Count - 1; i >= 0; i--) 
            {
                if (cla != null && !cla.IsUndefined) 
                {
                    if (((m_Items[i].Class.Value & cla.Value)) == 0) 
                    {
                        if (((m_Items[i].Class.IsProper || m_Items[i].Class.IsNoun)) && ((cla.IsProper || cla.IsNoun))) 
                        {
                        }
                        else 
                        {
                            m_Items.RemoveAt(i);
                            m_NeedRecalc = true;
                            continue;
                        }
                    }
                }
                bool ok = false;
                foreach (Pullenti.Morph.MorphBaseInfo it in bis) 
                {
                    if (!it.Case.IsUndefined && !m_Items[i].Case.IsUndefined) 
                    {
                        if (((m_Items[i].Case & it.Case)).IsUndefined) 
                            continue;
                    }
                    if (it.Gender != Pullenti.Morph.MorphGender.Undefined && m_Items[i].Gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        if (((it.Gender & m_Items[i].Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                            continue;
                    }
                    if (it.Number != Pullenti.Morph.MorphNumber.Undefined && m_Items[i].Number != Pullenti.Morph.MorphNumber.Undefined) 
                    {
                        if (((it.Number & m_Items[i].Number)) == Pullenti.Morph.MorphNumber.Undefined) 
                            continue;
                    }
                    ok = true;
                    break;
                }
                if (!ok) 
                {
                    m_Items.RemoveAt(i);
                    m_NeedRecalc = true;
                }
            }
        }
        /// <summary>
        /// Удалить элементы, не соответствующие другой морфологической коллекции
        /// </summary>
        public void RemoveItemsEx(MorphCollection col, Pullenti.Morph.MorphClass cla)
        {
            this.RemoveItemsListCla(col.Items, cla);
        }
        public Pullenti.Morph.MorphBaseInfo FindItem(Pullenti.Morph.MorphCase cas, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gen = Pullenti.Morph.MorphGender.Undefined)
        {
            if (m_Items == null) 
                return null;
            Pullenti.Morph.MorphBaseInfo res = null;
            int maxCoef = 0;
            foreach (Pullenti.Morph.MorphBaseInfo it in m_Items) 
            {
                if (!cas.IsUndefined) 
                {
                    if (((it.Case & cas)).IsUndefined) 
                        continue;
                }
                if (num != Pullenti.Morph.MorphNumber.Undefined) 
                {
                    if (((num & it.Number)) == Pullenti.Morph.MorphNumber.Undefined) 
                        continue;
                }
                if (gen != Pullenti.Morph.MorphGender.Undefined) 
                {
                    if (((gen & it.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                        continue;
                }
                Pullenti.Morph.MorphWordForm wf = it as Pullenti.Morph.MorphWordForm;
                if (wf != null && wf.UndefCoef > 0) 
                {
                    if (wf.UndefCoef > maxCoef) 
                    {
                        maxCoef = wf.UndefCoef;
                        res = it;
                    }
                    continue;
                }
                return it;
            }
            return res;
        }
        internal void Serialize(Stream stream)
        {
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, m_Class.Value);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, m_Case.Value);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, (short)m_Gender);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, (short)m_Number);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, (short)m_Voice);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, m_Language.Value);
            if (m_Items == null) 
                m_Items = new List<Pullenti.Morph.MorphBaseInfo>();
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, m_Items.Count);
            foreach (Pullenti.Morph.MorphBaseInfo it in m_Items) 
            {
                this.SerializeItem(stream, it);
            }
        }
        internal void Deserialize(Stream stream)
        {
            m_Class = new Pullenti.Morph.MorphClass() { Value = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream) };
            m_Case = new Pullenti.Morph.MorphCase() { Value = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream) };
            m_Gender = (Pullenti.Morph.MorphGender)Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream);
            m_Number = (Pullenti.Morph.MorphNumber)Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream);
            m_Voice = (Pullenti.Morph.MorphVoice)Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream);
            m_Language = new Pullenti.Morph.MorphLang() { Value = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream) };
            int cou = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            m_Items = new List<Pullenti.Morph.MorphBaseInfo>();
            for (int i = 0; i < cou; i++) 
            {
                Pullenti.Morph.MorphBaseInfo it = this.DeserializeItem(stream);
                if (it != null) 
                    m_Items.Add(it);
            }
            m_NeedRecalc = false;
        }
        void SerializeItem(Stream stream, Pullenti.Morph.MorphBaseInfo bi)
        {
            byte ty = (byte)0;
            if (bi is Pullenti.Morph.MorphWordForm) 
                ty = 1;
            stream.WriteByte(ty);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, bi.Class.Value);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, bi.Case.Value);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, (short)bi.Gender);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, (short)bi.Number);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, bi.Language.Value);
            Pullenti.Morph.MorphWordForm wf = bi as Pullenti.Morph.MorphWordForm;
            if (wf == null) 
                return;
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, wf.NormalCase);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, wf.NormalFull);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeShort(stream, wf.UndefCoef);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, (wf.Misc == null ? 0 : wf.Misc.Attrs.Count));
            if (wf.Misc != null) 
            {
                foreach (string a in wf.Misc.Attrs) 
                {
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, a);
                }
            }
        }
        Pullenti.Morph.MorphBaseInfo DeserializeItem(Stream stream)
        {
            int ty = stream.ReadByte();
            Pullenti.Morph.MorphBaseInfo res = (ty == 0 ? new Pullenti.Morph.MorphBaseInfo() : (Pullenti.Morph.MorphBaseInfo)new Pullenti.Morph.MorphWordForm());
            res.Class = new Pullenti.Morph.MorphClass() { Value = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream) };
            res.Case = new Pullenti.Morph.MorphCase() { Value = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream) };
            res.Gender = (Pullenti.Morph.MorphGender)Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream);
            res.Number = (Pullenti.Morph.MorphNumber)Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream);
            res.Language = new Pullenti.Morph.MorphLang() { Value = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream) };
            if (ty == 0) 
                return res;
            Pullenti.Morph.MorphWordForm wf = res as Pullenti.Morph.MorphWordForm;
            wf.NormalCase = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            wf.NormalFull = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            wf.UndefCoef = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeShort(stream);
            int cou = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            for (int i = 0; i < cou; i++) 
            {
                if (wf.Misc == null) 
                    wf.Misc = new Pullenti.Morph.MorphMiscInfo();
                wf.Misc.Attrs.Add(Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream));
            }
            return res;
        }
    }
}