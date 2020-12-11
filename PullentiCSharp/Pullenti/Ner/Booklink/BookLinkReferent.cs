/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Booklink
{
    /// <summary>
    /// Ссылка на внешний литературный источник (статью, книгу и пр.)
    /// </summary>
    public class BookLinkReferent : Pullenti.Ner.Referent
    {
        /// <summary>
        /// Имя типа сущности TypeName ("BOOKLINK")
        /// </summary>
        public const string OBJ_TYPENAME = "BOOKLINK";
        /// <summary>
        /// Имя атрибута - автор (обычно PersonReferent)
        /// </summary>
        public const string ATTR_AUTHOR = "AUTHOR";
        /// <summary>
        /// Имя атрибута - наименование
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - год
        /// </summary>
        public const string ATTR_YEAR = "YEAR";
        /// <summary>
        /// Имя атрибута - язык
        /// </summary>
        public const string ATTR_LANG = "LANG";
        /// <summary>
        /// Имя атрибута - география (обычно GeoReferent)
        /// </summary>
        public const string ATTR_GEO = "GEO";
        /// <summary>
        /// Имя атрибута - Url, ISDN И пр. (UriReferent)
        /// </summary>
        public const string ATTR_URL = "URL";
        /// <summary>
        /// Имя атрибута - мелочи
        /// </summary>
        public const string ATTR_MISC = "MISC";
        /// <summary>
        /// Имя атрибута - тип
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        public BookLinkReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Booklink.Internal.MetaBookLink.GlobalMeta;
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            object a = this.GetSlotValue(ATTR_AUTHOR);
            if (a != null) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_AUTHOR) 
                    {
                        if (a != s.Value) 
                            res.Append(", ");
                        if (s.Value is Pullenti.Ner.Referent) 
                            res.Append((s.Value as Pullenti.Ner.Referent).ToString(true, lang, lev + 1));
                        else if (s.Value is string) 
                            res.Append(s.Value as string);
                    }
                }
                if (AuthorsAndOther) 
                    res.Append(" и др.");
            }
            string nam = Name;
            if (nam != null) 
            {
                if (res.Length > 0) 
                    res.Append(' ');
                if (nam.Length > 200) 
                    nam = nam.Substring(0, 200) + "...";
                res.AppendFormat("\"{0}\"", nam);
            }
            Pullenti.Ner.Uri.UriReferent uri = this.GetSlotValue(ATTR_URL) as Pullenti.Ner.Uri.UriReferent;
            if (uri != null) 
                res.AppendFormat(" [{0}]", uri.ToString());
            if (Year > 0) 
                res.AppendFormat(", {0}", Year);
            return res.ToString();
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
        /// Язык
        /// </summary>
        public string Lang
        {
            get
            {
                return this.GetStringValue(ATTR_LANG);
            }
            set
            {
                this.AddSlot(ATTR_LANG, value, true, 0);
            }
        }
        /// <summary>
        /// Тип
        /// </summary>
        public string Typ
        {
            get
            {
                return this.GetStringValue(ATTR_TYPE);
            }
            set
            {
                this.AddSlot(ATTR_TYPE, value, true, 0);
            }
        }
        /// <summary>
        /// URL
        /// </summary>
        public Pullenti.Ner.Uri.UriReferent Url
        {
            get
            {
                return this.GetSlotValue(ATTR_URL) as Pullenti.Ner.Uri.UriReferent;
            }
        }
        /// <summary>
        /// Год
        /// </summary>
        public int Year
        {
            get
            {
                int year;
                if (int.TryParse(this.GetStringValue(ATTR_YEAR) ?? "", out year)) 
                    return year;
                else 
                    return 0;
            }
            set
            {
                this.AddSlot(ATTR_YEAR, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Есть ли признак среди списка авторов "и др."
        /// </summary>
        public bool AuthorsAndOther
        {
            get
            {
                return this.FindSlot(ATTR_MISC, "и др.", true) != null;
            }
            set
            {
                this.AddSlot(ATTR_MISC, "и др.", false, 0);
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            BookLinkReferent br = obj as BookLinkReferent;
            if (br == null) 
                return false;
            int eq = 0;
            if (Year > 0 && br.Year > 0) 
            {
                if (Year == br.Year) 
                    eq++;
                else 
                    return false;
            }
            if (Typ != null && br.Typ != null) 
            {
                if (Typ != br.Typ) 
                    return false;
            }
            bool eqAuth = false;
            if (this.FindSlot(ATTR_AUTHOR, null, true) != null && br.FindSlot(ATTR_AUTHOR, null, true) != null) 
            {
                bool ok = false;
                foreach (Pullenti.Ner.Slot a in Slots) 
                {
                    if (a.TypeName == ATTR_AUTHOR) 
                    {
                        if (br.FindSlot(ATTR_AUTHOR, a.Value, true) != null) 
                        {
                            eq++;
                            ok = true;
                            eqAuth = true;
                        }
                    }
                }
                if (!ok) 
                    return false;
            }
            if (br.Name != Name) 
            {
                if (Name == null || br.Name == null) 
                    return false;
                if (Name.StartsWith(br.Name) || br.Name.StartsWith(Name)) 
                    eq += 1;
                else if (eqAuth && Pullenti.Ner.Core.MiscHelper.CanBeEquals(Name, br.Name, false, true, false)) 
                    eq += 1;
                else 
                    return false;
            }
            else 
                eq += 2;
            return eq > 2;
        }
    }
}