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

namespace Pullenti.Ner.Instrument
{
    /// <summary>
    /// Представление фрагмента документа. Фрагменты образуют дерево с вершиной в InstrumentReferent.
    /// </summary>
    public class InstrumentBlockReferent : Pullenti.Ner.Referent
    {
        public InstrumentBlockReferent(string typename = null) : base(typename ?? OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Instrument.Internal.MetaInstrumentBlock.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("INSTRBLOCK")
        /// </summary>
        public const string OBJ_TYPENAME = "INSTRBLOCK";
        /// <summary>
        /// Имя атрибута - тип фрагмента (InstrumentKind)
        /// </summary>
        public const string ATTR_KIND = "KIND";
        public const string ATTR_KIND2 = "KIND_SEC";
        /// <summary>
        /// Имя атрибута - ссылки на дочерние фрагменты (InstrumentBlockReferent)
        /// </summary>
        public const string ATTR_CHILD = "CHILD";
        /// <summary>
        /// Имя атрибута - значение (например, текст)
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - ссылка на сущность (если есть)
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Имя атрибута - признак утраты силы
        /// </summary>
        public const string ATTR_EXPIRED = "EXPIRED";
        /// <summary>
        /// Имя атрибута - наименование фрагмента
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - номер фрагмента (для диапазона - максимальный номер)
        /// </summary>
        public const string ATTR_NUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - для диапазона - минимальный номер
        /// </summary>
        public const string ATTR_MINNUMBER = "MINNUMBER";
        /// <summary>
        /// Имя атрибута - подномер
        /// </summary>
        public const string ATTR_SUBNUMBER = "ADDNUMBER";
        /// <summary>
        /// Имя атрибута - второй подномер
        /// </summary>
        public const string ATTR_SUB2NUMBER = "ADDSECNUMBER";
        /// <summary>
        /// Имя атрибута - третий подномер
        /// </summary>
        public const string ATTR_SUB3NUMBER = "ADDTHIRDNUMBER";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            InstrumentKind ki = Kind;
            string str;
            str = Pullenti.Ner.Instrument.Internal.MetaInstrumentBlock.GlobalMeta.KindFeature.ConvertInnerValueToOuterValue(ki.ToString(), lang) as string;
            if (str != null) 
            {
                res.Append(str);
                if (Kind2 != InstrumentKind.Undefined) 
                {
                    str = Pullenti.Ner.Instrument.Internal.MetaInstrumentBlock.GlobalMeta.KindFeature.ConvertInnerValueToOuterValue(Kind2.ToString(), lang) as string;
                    if (str != null) 
                        res.AppendFormat(" ({0})", str);
                }
            }
            if (Number > 0) 
            {
                if (ki == InstrumentKind.Table) 
                    res.AppendFormat(" {0} строк, {1} столбцов", Children.Count, Number);
                else 
                {
                    res.AppendFormat(" №{0}", Number);
                    if (SubNumber > 0) 
                    {
                        res.AppendFormat(".{0}", SubNumber);
                        if (SubNumber2 > 0) 
                        {
                            res.AppendFormat(".{0}", SubNumber2);
                            if (SubNumber3 > 0) 
                                res.AppendFormat(".{0}", SubNumber3);
                        }
                    }
                    if (MinNumber > 0) 
                    {
                        for (int i = res.Length - 1; i >= 0; i--) 
                        {
                            if (res[i] == ' ' || res[i] == '.') 
                            {
                                res.Insert(i + 1, string.Format("{0}-", MinNumber));
                                break;
                            }
                        }
                    }
                }
            }
            bool ignoreRef = false;
            if (IsExpired) 
            {
                res.Append(" (утратить силу)");
                ignoreRef = true;
            }
            else if (ki != InstrumentKind.Editions && ki != InstrumentKind.Approved && (Ref is Pullenti.Ner.Decree.DecreeReferent)) 
            {
                res.Append(" (*)");
                ignoreRef = true;
            }
            if ((((str = this.GetStringValue(ATTR_NAME)))) == null) 
                str = this.GetStringValue(ATTR_VALUE);
            if (str != null) 
            {
                if (str.Length > 100) 
                    str = str.Substring(0, 100) + "...";
                res.AppendFormat(" \"{0}\"", str);
            }
            else if (!ignoreRef && (Ref is Pullenti.Ner.Referent) && (lev < 30)) 
                res.AppendFormat(" \"{0}\"", Ref.ToString(shortVariant, lang, lev + 1));
            return res.ToString().Trim();
        }
        /// <summary>
        /// Тип фрагмента
        /// </summary>
        public InstrumentKind Kind
        {
            get
            {
                string s = this.GetStringValue(ATTR_KIND);
                if (s == null) 
                    return InstrumentKind.Undefined;
                try 
                {
                    if (s == "Part" || s == "Base" || s == "Special") 
                        return InstrumentKind.Undefined;
                    object res = Enum.Parse(typeof(InstrumentKind), s, true);
                    if (res is InstrumentKind) 
                        return (InstrumentKind)res;
                }
                catch(Exception ex2123) 
                {
                }
                return InstrumentKind.Undefined;
            }
            set
            {
                if (value != InstrumentKind.Undefined) 
                    this.AddSlot(ATTR_KIND, value.ToString().ToUpper(), true, 0);
            }
        }
        public InstrumentKind Kind2
        {
            get
            {
                string s = this.GetStringValue(ATTR_KIND2);
                if (s == null) 
                    return InstrumentKind.Undefined;
                try 
                {
                    object res = Enum.Parse(typeof(InstrumentKind), s, true);
                    if (res is InstrumentKind) 
                        return (InstrumentKind)res;
                }
                catch(Exception ex2124) 
                {
                }
                return InstrumentKind.Undefined;
            }
            set
            {
                if (value != InstrumentKind.Undefined) 
                    this.AddSlot(ATTR_KIND2, value.ToString().ToUpper(), true, 0);
            }
        }
        /// <summary>
        /// Значение фрагмента
        /// </summary>
        public string Value
        {
            get
            {
                return this.GetStringValue(ATTR_VALUE);
            }
            set
            {
                this.AddSlot(ATTR_VALUE, value, true, 0);
            }
        }
        /// <summary>
        /// Ссылка на сущность
        /// </summary>
        public Pullenti.Ner.Referent Ref
        {
            get
            {
                return this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            }
        }
        /// <summary>
        /// Признак утраты силы
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return this.GetStringValue(ATTR_EXPIRED) == "true";
            }
            set
            {
                this.AddSlot(ATTR_EXPIRED, (value ? "true" : null), true, 0);
            }
        }
        /// <summary>
        /// Номер (для диапазона - максимальный номер)
        /// </summary>
        public int Number
        {
            get
            {
                string str = this.GetStringValue(ATTR_NUMBER);
                if (str == null) 
                    return 0;
                int i;
                if (int.TryParse(str, out i)) 
                    return i;
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_NUMBER, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Дополнительный номер (через точку за основным)
        /// </summary>
        public int SubNumber
        {
            get
            {
                string str = this.GetStringValue(ATTR_SUBNUMBER);
                if (str == null) 
                    return 0;
                int i;
                if (int.TryParse(str, out i)) 
                    return i;
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_SUBNUMBER, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Дополнительный второй номер (через точку за дополнительным)
        /// </summary>
        public int SubNumber2
        {
            get
            {
                string str = this.GetStringValue(ATTR_SUB2NUMBER);
                if (str == null) 
                    return 0;
                int i;
                if (int.TryParse(str, out i)) 
                    return i;
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_SUB2NUMBER, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Дополнительный третий номер (через точку за вторым дополнительным)
        /// </summary>
        public int SubNumber3
        {
            get
            {
                string str = this.GetStringValue(ATTR_SUB3NUMBER);
                if (str == null) 
                    return 0;
                int i;
                if (int.TryParse(str, out i)) 
                    return i;
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_SUB3NUMBER, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Минимальный номер, если задан диапазон
        /// </summary>
        public int MinNumber
        {
            get
            {
                string str = this.GetStringValue(ATTR_MINNUMBER);
                if (str == null) 
                    return 0;
                int i;
                if (int.TryParse(str, out i)) 
                    return i;
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_MINNUMBER, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetStringValue(ATTR_NAME);
            }
            set
            {
                this.AddSlot(ATTR_NAME, value, true, 0);
            }
        }
        /// <summary>
        /// Дочерние узлы: список InstrumentBlockReferent
        /// </summary>
        public List<InstrumentBlockReferent> Children
        {
            get
            {
                if (m_Children == null) 
                {
                    m_Children = new List<InstrumentBlockReferent>();
                    foreach (Pullenti.Ner.Slot s in Slots) 
                    {
                        if (s.TypeName == ATTR_CHILD) 
                        {
                            if (s.Value is InstrumentBlockReferent) 
                                m_Children.Add(s.Value as InstrumentBlockReferent);
                        }
                    }
                }
                return m_Children;
            }
        }
        List<InstrumentBlockReferent> m_Children;
        public override Pullenti.Ner.Slot AddSlot(string attrName, object attrValue, bool clearOldValue, int statCount = 0)
        {
            m_Children = null;
            return base.AddSlot(attrName, attrValue, clearOldValue, statCount);
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            return obj == this;
        }
        /// <summary>
        /// Представить тип строкой русского языка.
        /// </summary>
        /// <param name="typ">тип</param>
        /// <param name="shortVal">сокращённый или полный (например, ст. или статья)</param>
        /// <return>слово</return>
        public static string KindToRusString(InstrumentKind typ, bool shortVal)
        {
            if (typ == InstrumentKind.Appendix) 
                return (shortVal ? "прил." : "Приложение");
            if (typ == InstrumentKind.Clause) 
                return (shortVal ? "ст." : "Статья");
            if (typ == InstrumentKind.Chapter) 
                return (shortVal ? "гл." : "Глава");
            if (typ == InstrumentKind.Item) 
                return (shortVal ? "п." : "Пункт");
            if (typ == InstrumentKind.Paragraph) 
                return (shortVal ? "§" : "Параграф");
            if (typ == InstrumentKind.Subparagraph) 
                return (shortVal ? "подпарагр." : "Подпараграф");
            if (typ == InstrumentKind.DocPart) 
                return (shortVal ? "ч." : "Часть");
            if (typ == InstrumentKind.Section) 
                return (shortVal ? "раздел" : "Раздел");
            if (typ == InstrumentKind.InternalDocument) 
                return "Документ";
            if (typ == InstrumentKind.Subitem) 
                return (shortVal ? "пп." : "Подпункт");
            if (typ == InstrumentKind.Subsection) 
                return (shortVal ? "подразд." : "Подраздел");
            if (typ == InstrumentKind.ClausePart) 
                return (shortVal ? "ч." : "Часть");
            if (typ == InstrumentKind.Indention) 
                return (shortVal ? "абз." : "Абзац");
            if (typ == InstrumentKind.Preamble) 
                return (shortVal ? "преамб." : "Преамбула");
            return null;
        }
    }
}