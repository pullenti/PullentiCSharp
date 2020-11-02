/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Сущность, представляющая ссылку на структурную часть НПА
    /// </summary>
    public class DecreePartReferent : Pullenti.Ner.Referent
    {
        public DecreePartReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Decree.Internal.MetaDecreePart.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("DECREEPART")
        /// </summary>
        public const string OBJ_TYPENAME = "DECREEPART";
        /// <summary>
        /// Имя атрибута - наименование
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - владелец (DecreeReferent)
        /// </summary>
        public const string ATTR_OWNER = "OWNER";
        /// <summary>
        /// Имя атрибута - тип локального владельца (ст.10 этого закона)
        /// </summary>
        public const string ATTR_LOCALTYP = "LOCALTYP";
        /// <summary>
        /// Имя атрибута - часть документа (например, часть 1 Налогового кодекса)
        /// </summary>
        public const string ATTR_DOCPART = "DOCPART";
        /// <summary>
        /// Имя атрибута - приложение
        /// </summary>
        public const string ATTR_APPENDIX = "APPENDIX";
        /// <summary>
        /// Имя атрибута - раздел
        /// </summary>
        public const string ATTR_SECTION = "SECTION";
        /// <summary>
        /// Имя атрибута - подраздел
        /// </summary>
        public const string ATTR_SUBSECTION = "SUBSECTION";
        /// <summary>
        /// Имя атрибута - глава
        /// </summary>
        public const string ATTR_CHAPTER = "CHAPTER";
        /// <summary>
        /// Имя атрибута - статья
        /// </summary>
        public const string ATTR_CLAUSE = "CLAUSE";
        /// <summary>
        /// Имя атрибута - параграф
        /// </summary>
        public const string ATTR_PARAGRAPH = "PARAGRAPH";
        /// <summary>
        /// Имя атрибута - подпараграф
        /// </summary>
        public const string ATTR_SUBPARAGRAPH = "SUBPARAGRAPH";
        /// <summary>
        /// Имя атрибута - часть статьи (не путать с частью документа!)
        /// </summary>
        public const string ATTR_PART = "PART";
        /// <summary>
        /// Имя атрибута - пункт
        /// </summary>
        public const string ATTR_ITEM = "ITEM";
        /// <summary>
        /// Имя атрибута - подпункт
        /// </summary>
        public const string ATTR_SUBITEM = "SUBITEM";
        /// <summary>
        /// Имя атрибута - абзац
        /// </summary>
        public const string ATTR_INDENTION = "INDENTION";
        /// <summary>
        /// Имя атрибута - подабзац
        /// </summary>
        public const string ATTR_SUBINDENTION = "SUBINDENTION";
        /// <summary>
        /// Имя атрибута - преамбула
        /// </summary>
        public const string ATTR_PREAMBLE = "PREAMPLE";
        /// <summary>
        /// Имя атрибута - примечание
        /// </summary>
        public const string ATTR_NOTICE = "NOTICE";
        /// <summary>
        /// Имя атрибута - подпрограмма
        /// </summary>
        public const string ATTR_SUBPROGRAM = "SUBPROGRAM";
        /// <summary>
        /// Имя атрибута - дополнительное соглашение
        /// </summary>
        public const string ATTR_ADDAGREE = "ADDAGREE";
        /// <summary>
        /// Имя атрибута - страница
        /// </summary>
        public const string ATTR_PAGE = "PAGE";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            if (SubIndention != null) 
                res.AppendFormat(" подабз.{0}", SubIndention);
            if (Indention != null) 
                res.AppendFormat(" абз.{0}", Indention);
            if (Notice != null) 
                res.AppendFormat(" прим.{0}", Notice);
            if (SubItem != null) 
                res.AppendFormat(" пп.{0}", SubItem);
            if (Item != null) 
                res.AppendFormat(" п.{0}", Item);
            if (Part != null) 
                res.AppendFormat(" ч.{0}", Part);
            if (Preamble != null) 
                res.AppendFormat(" преамб.{0}", (Preamble == "0" ? "" : Preamble));
            if (Page != null) 
                res.AppendFormat(" стр.{0}", Page);
            if (Clause != null) 
                res.AppendFormat(" ст.{0}", Clause);
            if (SubParagraph != null) 
                res.AppendFormat(" подпар.{0}", SubParagraph);
            if (Paragraph != null) 
                res.AppendFormat(" пар.{0}", Paragraph);
            if (Chapter != null) 
                res.AppendFormat(" гл.{0}", Chapter);
            if (SubSection != null) 
                res.AppendFormat(" подразд.{0}", SubSection);
            if (Section != null) 
                res.AppendFormat(" разд.{0}", Section);
            if (DocPart != null) 
                res.AppendFormat(" док.часть {0}", DocPart);
            string app = Appendix;
            if (app == "0") 
                res.Append(" приложение");
            else if (app != null) 
                res.AppendFormat(" приложение {0}", app);
            if (Subprogram != null) 
                res.AppendFormat(" подпрограмма \"{0}\"", Name ?? "?");
            if (Addagree != null) 
            {
                if (Addagree == "0") 
                    res.AppendFormat(" допсоглашение");
                else 
                    res.AppendFormat(" допсоглашение {0}", Addagree);
            }
            if (((Owner != null || res.Length > 0)) && !shortVariant) 
            {
                if (!shortVariant && Subprogram == null) 
                {
                    string s = this._getShortName();
                    if (s != null) 
                        res.AppendFormat(" ({0})", s);
                }
                if (Owner != null && (lev < 20)) 
                {
                    if (res.Length > 0) 
                        res.Append("; ");
                    res.Append(Owner.ToString(shortVariant, lang, lev + 1));
                }
                else if (LocalTyp != null) 
                    res.AppendFormat("; {0}", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(LocalTyp));
            }
            return res.ToString().Trim();
        }
        /// <summary>
        /// Наименование (если несколько, то самое короткое)
        /// </summary>
        public string Name
        {
            get
            {
                string nam = null;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                    {
                        string n = s.Value.ToString();
                        if (nam == null || nam.Length > n.Length) 
                            nam = n;
                    }
                }
                return nam;
            }
        }
        string _getShortName()
        {
            string nam = Name;
            if (nam == null) 
                return null;
            if (nam.Length > 100) 
            {
                int i = 100;
                for (; i < nam.Length; i++) 
                {
                    if (!char.IsLetter(nam[i])) 
                        break;
                }
                if (i < nam.Length) 
                    nam = nam.Substring(0, i) + "...";
            }
            return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(nam);
        }
        /// <summary>
        /// Локальный тип (при ссылке на текущий документ)
        /// </summary>
        public string LocalTyp
        {
            get
            {
                return this.GetStringValue(ATTR_LOCALTYP);
            }
            set
            {
                this.AddSlot(ATTR_LOCALTYP, value, true, 0);
            }
        }
        public override Pullenti.Ner.Slot AddSlot(string attrName, object attrValue, bool clearOldValue, int statCount = 0)
        {
            string tag = null;
            if (attrValue is Pullenti.Ner.Decree.Internal.PartToken.PartValue) 
            {
                tag = (attrValue as Pullenti.Ner.Decree.Internal.PartToken.PartValue).SourceValue;
                attrValue = (attrValue as Pullenti.Ner.Decree.Internal.PartToken.PartValue).Value;
            }
            Pullenti.Ner.Slot s = base.AddSlot(attrName, attrValue, clearOldValue, statCount);
            if (tag != null) 
                s.Tag = tag;
            return s;
        }
        /// <summary>
        /// Статья
        /// </summary>
        public string Clause
        {
            get
            {
                return this.GetStringValue(ATTR_CLAUSE);
            }
            set
            {
                this.AddSlot(ATTR_CLAUSE, value, true, 0);
            }
        }
        /// <summary>
        /// Часть статьи
        /// </summary>
        public string Part
        {
            get
            {
                return this.GetStringValue(ATTR_PART);
            }
            set
            {
                this.AddSlot(ATTR_PART, value, true, 0);
            }
        }
        /// <summary>
        /// Часть документа (например, часть 2 Налогового кодекса)
        /// </summary>
        public string DocPart
        {
            get
            {
                return this.GetStringValue(ATTR_DOCPART);
            }
            set
            {
                this.AddSlot(ATTR_DOCPART, value, true, 0);
            }
        }
        /// <summary>
        /// Раздел
        /// </summary>
        public string Section
        {
            get
            {
                return this.GetStringValue(ATTR_SECTION);
            }
            set
            {
                this.AddSlot(ATTR_SECTION, value, true, 0);
            }
        }
        /// <summary>
        /// Подраздел
        /// </summary>
        public string SubSection
        {
            get
            {
                return this.GetStringValue(ATTR_SUBSECTION);
            }
            set
            {
                this.AddSlot(ATTR_SUBSECTION, value, true, 0);
            }
        }
        /// <summary>
        /// Приложение
        /// </summary>
        public string Appendix
        {
            get
            {
                return this.GetStringValue(ATTR_APPENDIX);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_APPENDIX, value, true, 0);
            }
        }
        /// <summary>
        /// Глава
        /// </summary>
        public string Chapter
        {
            get
            {
                return this.GetStringValue(ATTR_CHAPTER);
            }
            set
            {
                this.AddSlot(ATTR_CHAPTER, value, true, 0);
            }
        }
        /// <summary>
        /// Параграф
        /// </summary>
        public string Paragraph
        {
            get
            {
                return this.GetStringValue(ATTR_PARAGRAPH);
            }
            set
            {
                this.AddSlot(ATTR_PARAGRAPH, value, true, 0);
            }
        }
        /// <summary>
        /// Подпараграф
        /// </summary>
        public string SubParagraph
        {
            get
            {
                return this.GetStringValue(ATTR_SUBPARAGRAPH);
            }
            set
            {
                this.AddSlot(ATTR_SUBPARAGRAPH, value, true, 0);
            }
        }
        /// <summary>
        /// Пункт
        /// </summary>
        public string Item
        {
            get
            {
                return this.GetStringValue(ATTR_ITEM);
            }
            set
            {
                this.AddSlot(ATTR_ITEM, value, true, 0);
            }
        }
        /// <summary>
        /// Подпункт
        /// </summary>
        public string SubItem
        {
            get
            {
                return this.GetStringValue(ATTR_SUBITEM);
            }
            set
            {
                this.AddSlot(ATTR_SUBITEM, value, true, 0);
            }
        }
        /// <summary>
        /// Абзац
        /// </summary>
        public string Indention
        {
            get
            {
                return this.GetStringValue(ATTR_INDENTION);
            }
            set
            {
                this.AddSlot(ATTR_INDENTION, value, true, 0);
            }
        }
        /// <summary>
        /// Подабзац
        /// </summary>
        public string SubIndention
        {
            get
            {
                return this.GetStringValue(ATTR_SUBINDENTION);
            }
            set
            {
                this.AddSlot(ATTR_SUBINDENTION, value, true, 0);
            }
        }
        /// <summary>
        /// Преамбула
        /// </summary>
        public string Preamble
        {
            get
            {
                return this.GetStringValue(ATTR_PREAMBLE);
            }
            set
            {
                this.AddSlot(ATTR_PREAMBLE, value, true, 0);
            }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Notice
        {
            get
            {
                return this.GetStringValue(ATTR_NOTICE);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_NOTICE, value, true, 0);
            }
        }
        /// <summary>
        /// Страница
        /// </summary>
        public string Page
        {
            get
            {
                return this.GetStringValue(ATTR_PAGE);
            }
            set
            {
                this.AddSlot(ATTR_PAGE, value, true, 0);
            }
        }
        /// <summary>
        /// Подпрограмма
        /// </summary>
        public string Subprogram
        {
            get
            {
                return this.GetStringValue(ATTR_SUBPROGRAM);
            }
            set
            {
                this.AddSlot(ATTR_SUBPROGRAM, value, true, 0);
            }
        }
        /// <summary>
        /// Дополнительное соглашение
        /// </summary>
        public string Addagree
        {
            get
            {
                return this.GetStringValue(ATTR_ADDAGREE);
            }
            set
            {
                this.AddSlot(ATTR_ADDAGREE, value, true, 0);
            }
        }
        /// <summary>
        /// НПА - владелец
        /// </summary>
        public DecreeReferent Owner
        {
            get
            {
                DecreeReferent res = this.GetSlotValue(ATTR_OWNER) as DecreeReferent;
                if (res == null) 
                    return null;
                return res;
            }
            set
            {
                this.AddSlot(ATTR_OWNER, value, true, 0);
                if (value != null && LocalTyp != null) 
                    LocalTyp = null;
            }
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return Owner;
            }
        }
        internal void AddName(string name)
        {
            if (name == null || name.Length == 0) 
                return;
            if (name[name.Length - 1] == '.') 
                name = name.Substring(0, name.Length - 1);
            name = name.Trim().ToUpper();
            this.AddSlot(ATTR_NAME, name, false, 0);
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            base.MergeSlots(obj, mergeStatistic);
            if (Owner != null && LocalTyp != null) 
                LocalTyp = null;
        }
        int _getLevel(string typ)
        {
            if (typ == ATTR_ADDAGREE || typ == ATTR_SUBPROGRAM) 
                return 0;
            if (typ == ATTR_DOCPART) 
                return 1;
            if (typ == ATTR_APPENDIX) 
                return 1;
            if (typ == ATTR_SECTION) 
                return 2;
            if (typ == ATTR_SUBSECTION) 
                return 3;
            if (typ == ATTR_CHAPTER) 
                return 4;
            if (typ == ATTR_PARAGRAPH) 
                return 5;
            if (typ == ATTR_SUBPARAGRAPH) 
                return 6;
            if (typ == ATTR_PAGE) 
                return 6;
            if (typ == ATTR_CLAUSE) 
                return 7;
            if (typ == ATTR_PREAMBLE) 
                return 8;
            if (typ == ATTR_PART) 
                return 8;
            if (typ == ATTR_ITEM) 
                return 9;
            if (typ == ATTR_NOTICE) 
                return 10;
            if (typ == ATTR_SUBITEM) 
                return 11;
            if (typ == ATTR_INDENTION) 
                return 12;
            if (typ == ATTR_SUBINDENTION) 
                return 13;
            return -1;
        }
        bool _hasLessLevelAttr(string typ)
        {
            int l = this._getLevel(typ);
            if (l < 0) 
                return false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l1 = this._getLevel(s.TypeName);
                if (l1 >= 0 && l1 > l) 
                    return true;
            }
            return false;
        }
        // Добавить информацию о вышележащих элементах
        internal void AddHighLevelInfo(DecreePartReferent dp)
        {
            if (dp.Addagree != null && Addagree == null) 
                Addagree = dp.Addagree;
            else if (dp.Addagree != Addagree) 
                return;
            if (dp.Appendix != null && Appendix == null) 
                Appendix = dp.Appendix;
            else if (Appendix != dp.Appendix) 
                return;
            if (dp.DocPart != null && DocPart == null) 
                DocPart = dp.DocPart;
            else if (DocPart != dp.DocPart) 
                return;
            if (dp.Section != null && Section == null && this._hasLessLevelAttr(ATTR_SECTION)) 
                Section = dp.Section;
            else if (Section != dp.Section) 
                return;
            if (dp.SubSection != null && SubSection == null && this._hasLessLevelAttr(ATTR_SUBSECTION)) 
                SubSection = dp.SubSection;
            else if (SubSection != dp.SubSection) 
                return;
            if (dp.Chapter != null && Chapter == null && this._hasLessLevelAttr(ATTR_CHAPTER)) 
                Chapter = dp.Chapter;
            else if (dp.Chapter != Chapter) 
                return;
            if (dp.Paragraph != null && Paragraph == null && this._hasLessLevelAttr(ATTR_PARAGRAPH)) 
                Paragraph = dp.Paragraph;
            else if (Paragraph != dp.Paragraph) 
                return;
            if (dp.SubParagraph != null && SubParagraph == null && this._hasLessLevelAttr(ATTR_SUBPARAGRAPH)) 
                SubParagraph = dp.SubParagraph;
            else if (SubParagraph != dp.SubParagraph) 
                return;
            if (dp.Clause != null && Clause == null && this._hasLessLevelAttr(ATTR_CLAUSE)) 
                Clause = dp.Clause;
            else if (dp.Clause != Clause) 
                return;
            if (dp.Part != null && Part == null && this._hasLessLevelAttr(ATTR_PART)) 
                Part = dp.Part;
            else if (dp.Part != Part) 
                return;
            if (dp.Item != null && Item == null && this._hasLessLevelAttr(ATTR_ITEM)) 
            {
                if (SubItem != null && SubItem.IndexOf('.') > 0) 
                {
                }
                else 
                    Item = dp.Item;
            }
            else if (dp.Item != Item) 
                return;
            if (dp.SubItem != null && SubItem == null && this._hasLessLevelAttr(ATTR_SUBITEM)) 
                SubItem = dp.SubItem;
            else if (dp.SubItem != SubItem) 
                return;
            if (dp.Indention != null && Indention == null && this._hasLessLevelAttr(ATTR_INDENTION)) 
                Indention = dp.Indention;
        }
        // Проверить, что все элементы находятся на более низком уровне, чем у аргумента
        internal bool IsAllItemsLessLevel(Pullenti.Ner.Referent upperParts, bool ignoreEquals)
        {
            if (upperParts is DecreeReferent) 
                return true;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l = this._getLevel(s.TypeName);
                if (l < 0) 
                    continue;
                if (upperParts.FindSlot(s.TypeName, null, true) != null) 
                {
                    if (upperParts.FindSlot(s.TypeName, s.Value, true) == null) 
                        return false;
                    continue;
                }
                foreach (Pullenti.Ner.Slot ss in upperParts.Slots) 
                {
                    int ll = this._getLevel(ss.TypeName);
                    if (ll >= l) 
                        return false;
                }
            }
            return true;
        }
        internal bool IsAllItemsOverThisLevel(Pullenti.Ner.Decree.Internal.PartToken.ItemType typ)
        {
            int l0 = this._getLevel(Pullenti.Ner.Decree.Internal.PartToken._getAttrNameByTyp(typ));
            if (l0 <= 0) 
                return false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l = this._getLevel(s.TypeName);
                if (l <= 0) 
                    continue;
                if (l >= l0) 
                    return false;
            }
            return true;
        }
        internal int GetMinLevel()
        {
            int min = 0;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l = this._getLevel(s.TypeName);
                if (l <= 0) 
                    continue;
                if (min == 0) 
                    min = l;
                else if (min > l) 
                    min = l;
            }
            return min;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            bool b = this._CanBeEquals(obj, typ, false);
            return b;
        }
        bool _CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ, bool ignoreGeo)
        {
            DecreePartReferent dr = obj as DecreePartReferent;
            if (dr == null) 
                return false;
            if (Owner != null && dr.Owner != null) 
            {
                if (Owner != dr.Owner) 
                    return false;
            }
            else if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                return false;
            else 
            {
                string ty1 = (Owner == null ? LocalTyp : Owner.Typ);
                string ty2 = (dr.Owner == null ? dr.LocalTyp : dr.Owner.Typ);
                if (ty1 != ty2) 
                {
                    ty1 = (Owner == null ? LocalTyp : Owner.Typ0);
                    ty2 = (dr.Owner == null ? dr.LocalTyp : dr.Owner.Typ0);
                    if (ty1 != ty2) 
                        return false;
                }
            }
            if (Clause != dr.Clause) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Clause == null || dr.Clause == null))) 
                {
                }
                else 
                    return false;
            }
            if (Part != dr.Part) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Part == null || dr.Part == null))) 
                {
                }
                else 
                    return false;
            }
            if (Paragraph != dr.Paragraph) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Paragraph == null || dr.Paragraph == null))) 
                {
                }
                else 
                    return false;
            }
            if (SubParagraph != dr.SubParagraph) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((SubParagraph == null || dr.SubParagraph == null))) 
                {
                }
                else 
                    return false;
            }
            if (Item != dr.Item) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Item == null || dr.Item == null))) 
                {
                }
                else 
                    return false;
            }
            if (SubItem != dr.SubItem) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((SubItem == null || dr.SubItem == null))) 
                {
                }
                else 
                    return false;
            }
            if (Notice != dr.Notice) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Notice == null || dr.Notice == null))) 
                {
                }
                else 
                    return false;
            }
            if (Indention != dr.Indention) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Indention == null || dr.Indention == null))) 
                {
                }
                else 
                    return false;
            }
            if (SubIndention != dr.SubIndention) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((SubIndention == null || dr.SubIndention == null))) 
                {
                }
                else 
                    return false;
            }
            if (Appendix != dr.Appendix) 
            {
                if (Appendix != null && dr.Appendix != null) 
                    return false;
                if (Clause == null && Paragraph == null && Item == null) 
                    return false;
            }
            if (Chapter != dr.Chapter) 
            {
                if (Chapter != null && dr.Chapter != null) 
                    return false;
                if (Clause == null && Paragraph == null && Item == null) 
                    return false;
            }
            if (Section != dr.Section) 
            {
                if (Section != null && dr.Section != null) 
                    return false;
                if ((Clause == null && Paragraph == null && Item == null) && SubSection == null) 
                    return false;
            }
            if (SubSection != dr.SubSection) 
            {
                if (SubSection != null && dr.SubSection != null) 
                    return false;
                if (Clause == null && Paragraph == null && Item == null) 
                    return false;
            }
            if (Subprogram != null || dr.Subprogram != null) 
            {
                if (Name != dr.Name) 
                    return false;
                return true;
            }
            if (Addagree != null || dr.Addagree != null) 
            {
                if (Addagree != dr.Addagree) 
                    return false;
            }
            if (DocPart != dr.DocPart) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((DocPart == null || dr.DocPart == null))) 
                {
                }
                else 
                    return false;
            }
            if (Page != dr.Page) 
                return false;
            return true;
        }
        public static DecreePartReferent CreateRangeReferent(DecreePartReferent min, DecreePartReferent max)
        {
            DecreePartReferent res = min.Clone() as DecreePartReferent;
            int cou = 0;
            foreach (Pullenti.Ner.Slot s in res.Slots) 
            {
                Pullenti.Ner.Slot ss = max.FindSlot(s.TypeName, null, true);
                if (ss == null) 
                    return null;
                if (ss.Value == s.Value) 
                    continue;
                if (max.FindSlot(s.TypeName, s.Value, true) != null) 
                    continue;
                if ((++cou) > 1) 
                    return null;
                res.UploadSlot(s, string.Format("{0}-{1}", s.Value, ss.Value));
            }
            if (cou != 1) 
                return null;
            return res;
        }
    }
}