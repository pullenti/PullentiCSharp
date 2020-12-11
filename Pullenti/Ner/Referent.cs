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
using System.Text;
using System.Xml;

namespace Pullenti.Ner
{
    /// <summary>
    /// Базовый класс для всех именованных сущностей
    /// </summary>
    public class Referent
    {
        public Referent(string typ)
        {
            m_ObjectType = typ;
        }
        string m_ObjectType;
        /// <summary>
        /// Имя типа (= InstanceOf.Name)
        /// </summary>
        public string TypeName
        {
            get
            {
                return m_ObjectType;
            }
        }
        public override string ToString()
        {
            return this.ToString(false, Pullenti.Morph.MorphLang.Unknown, 0);
        }
        /// <summary>
        /// Специализированное строковое представление сущности
        /// </summary>
        /// <param name="shortVariant">Сокращённый вариант</param>
        /// <param name="lang">Язык</param>
        public virtual string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            return TypeName;
        }
        // По этой строке можно осуществлять сортировку среди сущностей одного типа
        public virtual string ToSortString()
        {
            return this.ToString(false, Pullenti.Morph.MorphLang.Unknown, 0);
        }
        /// <summary>
        /// Ссылка на описание из модели данных
        /// </summary>
        public Pullenti.Ner.Metadata.ReferentClass InstanceOf
        {
            get;
            set;
        }
        /// <summary>
        /// Привязка к элементам внешней онтологии, если таковые были заданы - 
        /// когда в Process(...) класса Processor был передан словарь "внешней онтологии" ExtOntology. 
        /// В принципе, может привязаться к нескольким элементам "онтологии".
        /// </summary>
        public List<ExtOntologyItem> OntologyItems;
        /// <summary>
        /// Значения атрибутов - список элементов типа Slot
        /// </summary>
        public List<Slot> Slots
        {
            get
            {
                return m_Slots;
            }
        }
        List<Slot> m_Slots = new List<Slot>();
        /// <summary>
        /// Добавить значение атрибута
        /// </summary>
        /// <param name="attrName">имя</param>
        /// <param name="attrValue">значение</param>
        /// <param name="clearOldValue">если true и слот существует, то значение перезапишется</param>
        /// <return>слот(атрибут)</return>
        public virtual Slot AddSlot(string attrName, object attrValue, bool clearOldValue, int statCount = 0)
        {
            if (clearOldValue) 
            {
                for (int i = Slots.Count - 1; i >= 0; i--) 
                {
                    if (Slots[i].TypeName == attrName) 
                        Slots.RemoveAt(i);
                }
            }
            if (attrValue == null) 
                return null;
            foreach (Slot r in Slots) 
            {
                if (r.TypeName == attrName) 
                {
                    if (this.CompareValues(r.Value, attrValue, true)) 
                    {
                        r.Count += statCount;
                        return r;
                    }
                }
            }
            Slot res = new Slot();
            res.Owner = this;
            res.Value = attrValue;
            res.TypeName = attrName;
            res.Count = statCount;
            Slots.Add(res);
            return res;
        }
        public virtual void UploadSlot(Slot slot, object newVal)
        {
            if (slot != null) 
                slot.Value = newVal;
        }
        int m_Level;
        /// <summary>
        /// Найти слот (атрибут)
        /// </summary>
        /// <param name="attrName">имя атрибута</param>
        /// <param name="val">возможное значение</param>
        /// <param name="useCanBeEqualsForReferents">для значений-сущностей использовать метод CanBeEquals для сравнения</param>
        /// <return>подходящий слот или null</return>
        public Slot FindSlot(string attrName, object val = null, bool useCanBeEqualsForReferents = true)
        {
            if (m_Level > 10) 
                return null;
            if (attrName == null) 
            {
                if (val == null) 
                    return null;
                m_Level++;
                foreach (Slot r in Slots) 
                {
                    if (this.CompareValues(val, r.Value, useCanBeEqualsForReferents)) 
                    {
                        m_Level--;
                        return r;
                    }
                }
                m_Level--;
                return null;
            }
            foreach (Slot r in Slots) 
            {
                if (r.TypeName == attrName) 
                {
                    if (val == null) 
                        return r;
                    m_Level++;
                    if (this.CompareValues(val, r.Value, useCanBeEqualsForReferents)) 
                    {
                        m_Level--;
                        return r;
                    }
                    m_Level--;
                }
            }
            return null;
        }
        bool CompareValues(object val1, object val2, bool useCanBeEqualsForReferents)
        {
            if (val1 == null) 
                return val2 == null;
            if (val2 == null) 
                return val1 == null;
            if (val1 == val2) 
                return true;
            if ((val1 is Referent) && (val2 is Referent)) 
            {
                if (useCanBeEqualsForReferents) 
                    return (val1 as Referent).CanBeEquals(val2 as Referent, Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts);
                else 
                    return false;
            }
            if (val1 is string) 
            {
                if (!(val2 is string)) 
                    return false;
                string s1 = (string)val1;
                string s2 = (string)val2;
                int i = string.Compare(s1, s2, true);
                return i == 0;
            }
            return val1 == val2;
        }
        /// <summary>
        /// Получить значение слота-атрибута (если их несколько, то вернёт первое)
        /// </summary>
        /// <param name="attrName">имя слота</param>
        /// <return>значение (поле Value)</return>
        public object GetSlotValue(string attrName)
        {
            foreach (Slot v in Slots) 
            {
                if (v.TypeName == attrName) 
                    return v.Value;
            }
            return null;
        }
        /// <summary>
        /// Получить строковое значение (если их несколько, то вернёт первое)
        /// </summary>
        /// <param name="attrName">имя атрибута</param>
        /// <return>значение или null</return>
        public virtual string GetStringValue(string attrName)
        {
            foreach (Slot v in Slots) 
            {
                if (v.TypeName == attrName) 
                    return (v.Value == null ? null : v.Value.ToString());
            }
            return null;
        }
        /// <summary>
        /// Получить все строовые значения заданного атрибута
        /// </summary>
        /// <param name="attrName">имя атрибута</param>
        /// <return>список значений string</return>
        public virtual List<string> GetStringValues(string attrName)
        {
            List<string> res = new List<string>();
            foreach (Slot v in Slots) 
            {
                if (v.TypeName == attrName && v.Value != null) 
                {
                    if (v.Value is string) 
                        res.Add(v.Value as string);
                    else 
                        res.Add(v.ToString());
                }
            }
            return res;
        }
        /// <summary>
        /// Получить числовое значение (если их несколько, то вернёт первое)
        /// </summary>
        /// <param name="attrName">имя атрибута</param>
        /// <param name="defValue">дефолтовое значение, если не найдено</param>
        /// <return>число</return>
        public int GetIntValue(string attrName, int defValue)
        {
            string str = this.GetStringValue(attrName);
            if (string.IsNullOrEmpty(str)) 
                return defValue;
            int res;
            if (!int.TryParse(str, out res)) 
                return defValue;
            return res;
        }
        List<TextAnnotation> m_Occurrence;
        /// <summary>
        /// Вхождение сущности в исходный текст (список аннотаций TextAnnotation)
        /// </summary>
        public List<TextAnnotation> Occurrence
        {
            get
            {
                if (m_Occurrence == null) 
                    m_Occurrence = new List<TextAnnotation>();
                return m_Occurrence;
            }
        }
        /// <summary>
        /// Найти ближайшую к токену аннотацию
        /// </summary>
        /// <param name="t">токен</param>
        /// <return>ближайшая аннотация</return>
        public TextAnnotation FindNearOccurence(Token t)
        {
            int min = -1;
            TextAnnotation res = null;
            foreach (TextAnnotation oc in Occurrence) 
            {
                if (oc.Sofa == t.Kit.Sofa) 
                {
                    int len = oc.BeginChar - t.BeginChar;
                    if (len < 0) 
                        len = -len;
                    if ((min < 0) || (len < min)) 
                    {
                        min = len;
                        res = oc;
                    }
                }
            }
            return res;
        }
        public void AddOccurenceOfRefTok(ReferentToken rt)
        {
            this.AddOccurence(new TextAnnotation() { Sofa = rt.Kit.Sofa, BeginChar = rt.BeginChar, EndChar = rt.EndChar, OccurenceOf = rt.Referent });
        }
        /// <summary>
        /// Добавить аннотацию
        /// </summary>
        /// <param name="anno">аннотация</param>
        public void AddOccurence(TextAnnotation anno)
        {
            foreach (TextAnnotation l in Occurrence) 
            {
                Pullenti.Ner.Core.Internal.TextsCompareType typ = l.CompareWith(anno);
                if (typ == Pullenti.Ner.Core.Internal.TextsCompareType.Noncomparable) 
                    continue;
                if (typ == Pullenti.Ner.Core.Internal.TextsCompareType.Equivalent || typ == Pullenti.Ner.Core.Internal.TextsCompareType.Contains) 
                    return;
                if (typ == Pullenti.Ner.Core.Internal.TextsCompareType.In || typ == Pullenti.Ner.Core.Internal.TextsCompareType.Intersect) 
                {
                    l.Merge(anno);
                    return;
                }
            }
            if (anno.OccurenceOf != this && anno.OccurenceOf != null) 
                anno = new TextAnnotation() { BeginChar = anno.BeginChar, EndChar = anno.EndChar, Sofa = anno.Sofa };
            if (m_Occurrence == null) 
                m_Occurrence = new List<TextAnnotation>();
            anno.OccurenceOf = this;
            if (m_Occurrence.Count == 0) 
            {
                anno.EssentialForOccurence = true;
                m_Occurrence.Add(anno);
                return;
            }
            if (anno.BeginChar < m_Occurrence[0].BeginChar) 
            {
                m_Occurrence.Insert(0, anno);
                return;
            }
            if (anno.BeginChar >= m_Occurrence[m_Occurrence.Count - 1].BeginChar) 
            {
                m_Occurrence.Add(anno);
                return;
            }
            for (int i = 0; i < (m_Occurrence.Count - 1); i++) 
            {
                if (anno.BeginChar >= m_Occurrence[i].BeginChar && anno.BeginChar <= m_Occurrence[i + 1].BeginChar) 
                {
                    m_Occurrence.Insert(i + 1, anno);
                    return;
                }
            }
            m_Occurrence.Add(anno);
        }
        /// <summary>
        /// Проверка, что ссылки на элемент имеются на заданном участке текста
        /// </summary>
        /// <param name="beginChar">начальная позиция</param>
        /// <param name="endChar">конечная позиция</param>
        /// <return>да или нет</return>
        public bool CheckOccurence(int beginChar, int endChar)
        {
            foreach (TextAnnotation loc in Occurrence) 
            {
                Pullenti.Ner.Core.Internal.TextsCompareType cmp = loc.Compare(beginChar, endChar);
                if (cmp != Pullenti.Ner.Core.Internal.TextsCompareType.Early && cmp != Pullenti.Ner.Core.Internal.TextsCompareType.Later && cmp != Pullenti.Ner.Core.Internal.TextsCompareType.Noncomparable) 
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Используется произвольным образом
        /// </summary>
        public object Tag
        {
            get;
            set;
        }
        public virtual Referent Clone()
        {
            Referent res = ProcessorService.CreateReferent(TypeName);
            if (res == null) 
                res = new Referent(TypeName);
            res.Occurrence.AddRange(Occurrence);
            res.OntologyItems = OntologyItems;
            foreach (Slot r in Slots) 
            {
                Slot rr = new Slot() { TypeName = r.TypeName, Value = r.Value, Count = r.Count };
                rr.Owner = res;
                res.Slots.Add(rr);
            }
            return res;
        }
        /// <summary>
        /// Проверка возможной тождественности сущностей
        /// </summary>
        /// <param name="obj">другая сущность</param>
        /// <param name="typ">тип сравнения</param>
        /// <return>результат</return>
        public virtual bool CanBeEquals(Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            if (obj == null || obj.TypeName != TypeName) 
                return false;
            foreach (Slot r in Slots) 
            {
                if (r.Value != null && obj.FindSlot(r.TypeName, r.Value, false) == null) 
                    return false;
            }
            foreach (Slot r in obj.Slots) 
            {
                if (r.Value != null && this.FindSlot(r.TypeName, r.Value, true) == null) 
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Объединение значений атрибутов со значениями атрибутов другой сущности
        /// </summary>
        /// <param name="obj">Другая сущшность, считающаяся эквивалентной</param>
        /// <param name="mergeStatistic">Объединять ли вместе со статистикой</param>
        public virtual void MergeSlots(Referent obj, bool mergeStatistic = true)
        {
            if (obj == null) 
                return;
            foreach (Slot r in obj.Slots) 
            {
                Slot s = this.FindSlot(r.TypeName, r.Value, true);
                if (s == null && r.Value != null) 
                    s = this.AddSlot(r.TypeName, r.Value, false, 0);
                if (s != null && mergeStatistic) 
                    s.Count += r.Count;
            }
            this._mergeExtReferents(obj);
        }
        /// <summary>
        /// Ссылка на родительскую сущность. Для разных типов сущностей здесь могут быть свои сущности, 
        /// например, для организаций - вышестоящая организация, для пункта закона - сам закон и т.д.
        /// </summary>
        public virtual Referent ParentReferent
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// Получить идентификатор иконки. Саму иконку ImageWrapper можно получить через функцию 
        /// GetImageById(imageId) статического класса ProcessorService.
        /// </summary>
        /// <return>идентификатор иконки</return>
        public virtual string GetImageId()
        {
            if (InstanceOf == null) 
                return null;
            return InstanceOf.GetImageId(this);
        }
        public const string ATTR_GENERAL = "GENERAL";
        /// <summary>
        /// Проверка, может ли текущая сущность быть обобщением для другой сущности
        /// </summary>
        /// <param name="obj">более частная сущность</param>
        /// <return>да-нет</return>
        public virtual bool CanBeGeneralFor(Referent obj)
        {
            return false;
        }
        /// <summary>
        /// Ссылка на сущность-обобщение
        /// </summary>
        public Referent GeneralReferent
        {
            get
            {
                Referent res = this.GetSlotValue(ATTR_GENERAL) as Referent;
                if (res == null || res == this) 
                    return null;
                return res;
            }
            set
            {
                if (value == GeneralReferent) 
                    return;
                if (value == this) 
                    return;
                this.AddSlot(ATTR_GENERAL, value, true, 0);
            }
        }
        // Создать элемент онтологии
        public virtual Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            return null;
        }
        // Используется внутренним образом (напрямую не устанавливать!)
        internal Pullenti.Ner.Core.IntOntologyItem IntOntologyItem;
        // Используется внутренним образом
        public virtual List<string> GetCompareStrings()
        {
            List<string> res = new List<string>();
            res.Add(this.ToString());
            string s = this.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0);
            if (s != res[0]) 
                res.Add(s);
            return res;
        }
        /// <summary>
        /// Сериализация в строку XML. 
        /// Последующая десериализация делается через Processor.DeserializeReferent.
        /// </summary>
        /// <return>строка</return>
        public string Serialize()
        {
            StringBuilder res = new StringBuilder();
            using (XmlWriter xml = XmlWriter.Create(res)) 
            {
                this.Serialize(xml, null);
            }
            int i = res.ToString().IndexOf('>');
            if (i > 10 && res[1] == '?') 
                res.Remove(0, i + 1);
            for (i = 0; i < res.Length; i++) 
            {
                char ch = res[i];
                int cod = (int)ch;
                if ((cod < 0x80) && cod >= 0x20) 
                    continue;
                if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch)) 
                    continue;
                res.Remove(i, 1);
                res.Insert(i, string.Format("&#x{0};", cod.ToString("X04")));
            }
            return res.ToString();
        }
        /// <summary>
        /// Прямая сериализация в XML. 
        /// Последующая десериализация делается через Processor.DeserializeReferentFromXml.
        /// </summary>
        public void Serialize(XmlWriter xml, Dictionary<string, string> attrs = null)
        {
            xml.WriteStartElement(TypeName);
            if (attrs != null) 
            {
                foreach (KeyValuePair<string, string> kp in attrs) 
                {
                    xml.WriteAttributeString(kp.Key, Pullenti.Ner.Core.MiscHelper._corrXmlText(kp.Value));
                }
            }
            else if (Tag != null) 
                xml.WriteAttributeString("id", Pullenti.Ner.Core.MiscHelper._corrXmlText(Tag.ToString()));
            List<string> refs = null;
            foreach (Slot s in Slots) 
            {
                if (s.Value != null) 
                {
                    string nam = s.TypeName;
                    if (nam[0] == '@') 
                        nam = "ATCOM_" + nam.Substring(1);
                    if (!(s.Value is Referent) && !(s.Value is ProxyReferent)) 
                    {
                        xml.WriteStartElement(nam);
                        if (s.Count > 0) 
                            xml.WriteAttributeString("count", s.Count.ToString());
                        try 
                        {
                            xml.WriteValue(Pullenti.Ner.Core.MiscHelper._corrXmlText(s.Value.ToString()));
                        }
                        catch(Exception ex4251) 
                        {
                        }
                        xml.WriteEndElement();
                    }
                    else 
                    {
                        string str = s.TypeName + s.Value;
                        if (refs == null) 
                            refs = new List<string>();
                        if (refs.Contains(str)) 
                            continue;
                        refs.Add(str);
                        xml.WriteStartElement(nam);
                        xml.WriteAttributeString("ref", "true");
                        if (s.Count > 0) 
                            xml.WriteAttributeString("count", s.Count.ToString());
                        string id = null;
                        if (s.Value is ProxyReferent) 
                            id = (s.Value as ProxyReferent).Identity;
                        else if (s.Value is Referent) 
                        {
                            Referent rr = s.Value as Referent;
                            if (rr.RepositoryItemId != 0) 
                                id = rr.RepositoryItemId.ToString();
                            else if (rr.Tag != null) 
                                id = rr.Tag.ToString();
                        }
                        if (!string.IsNullOrEmpty(id)) 
                            xml.WriteAttributeString("id", id);
                        else 
                        {
                        }
                        xml.WriteValue(Pullenti.Ner.Core.MiscHelper._corrXmlText(s.Value.ToString()));
                        xml.WriteEndElement();
                    }
                }
            }
            xml.WriteEndElement();
        }
        // Используется внутренним образом (при сохранении сущностей в репозитории)
        public int RepositoryItemId;
        internal Referent RepositoryReferent;
        internal List<ReferentToken> m_ExtReferents;
        public void AddExtReferent(ReferentToken rt)
        {
            if (rt == null) 
                return;
            if (m_ExtReferents == null) 
                m_ExtReferents = new List<ReferentToken>();
            if (!m_ExtReferents.Contains(rt)) 
                m_ExtReferents.Add(rt);
            if (m_ExtReferents.Count > 100) 
            {
            }
        }
        public void MoveExtReferent(Referent target, Referent r)
        {
            if (m_ExtReferents != null) 
            {
                foreach (ReferentToken rt in m_ExtReferents) 
                {
                    if (rt.Referent == r) 
                    {
                        target.AddExtReferent(rt);
                        m_ExtReferents.Remove(rt);
                        break;
                    }
                }
            }
        }
        protected void _mergeExtReferents(Referent obj)
        {
            if (obj.m_ExtReferents != null) 
            {
                foreach (ReferentToken rt in obj.m_ExtReferents) 
                {
                    this.AddExtReferent(rt);
                }
            }
        }
        public void Serialize(Stream stream)
        {
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, TypeName);
            Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, m_Slots.Count);
            foreach (Slot s in m_Slots) 
            {
                Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, s.TypeName);
                Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, s.Count);
                if ((s.Value is Referent) && ((s.Value as Referent).Tag is int)) 
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, -((int)(s.Value as Referent).Tag));
                else if (s.Value is string) 
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, s.Value as string);
                else if (s.Value == null) 
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, 0);
                else 
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeString(stream, s.Value.ToString());
            }
            if (m_Occurrence == null) 
                Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, 0);
            else 
            {
                Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, m_Occurrence.Count);
                foreach (TextAnnotation o in m_Occurrence) 
                {
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, o.BeginChar);
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, o.EndChar);
                    int attr = 0;
                    if (o.EssentialForOccurence) 
                        attr = 1;
                    Pullenti.Ner.Core.Internal.SerializerHelper.SerializeInt(stream, attr);
                }
            }
        }
        public void Deserialize(Stream stream, List<Referent> all, SourceOfAnalysis sofa)
        {
            string typ = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
            int cou = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            for (int i = 0; i < cou; i++) 
            {
                typ = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
                int c = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
                int id = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
                object val = null;
                if ((id < 0) && all != null) 
                {
                    int id1 = (-id) - 1;
                    if (id1 < all.Count) 
                        val = all[id1];
                }
                else if (id > 0) 
                {
                    stream.Position -= 4;
                    val = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeString(stream);
                }
                this.AddSlot(typ, val, false, c);
            }
            cou = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
            m_Occurrence = new List<TextAnnotation>();
            for (int i = 0; i < cou; i++) 
            {
                TextAnnotation a = new TextAnnotation() { Sofa = sofa, OccurenceOf = this };
                m_Occurrence.Add(a);
                a.BeginChar = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
                a.EndChar = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
                int attr = Pullenti.Ner.Core.Internal.SerializerHelper.DeserializeInt(stream);
                if (((attr & 1)) != 0) 
                    a.EssentialForOccurence = true;
            }
        }
    }
}