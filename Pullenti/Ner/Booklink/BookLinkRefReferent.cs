/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Booklink
{
    /// <summary>
    /// Ссылка на ССЫЛКУ (BookLinkReferent или DecreeReferent)
    /// </summary>
    public class BookLinkRefReferent : Pullenti.Ner.Referent
    {
        /// <summary>
        /// Имя типа сущности TypeName ("BOOKLINKREF")
        /// </summary>
        public const string OBJ_TYPENAME = "BOOKLINKREF";
        /// <summary>
        /// Имя атрибута - источник (BookLinkReferent или DecreeReferent)
        /// </summary>
        public const string ATTR_BOOK = "BOOK";
        /// <summary>
        /// Имя атрибута - тип (BookLinkRefType)
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - ссылка на страницу или диапазон страниц
        /// </summary>
        public const string ATTR_PAGES = "PAGES";
        /// <summary>
        /// Имя атрибута - порядковый номер в списке
        /// </summary>
        public const string ATTR_NUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - разное
        /// </summary>
        public const string ATTR_MISC = "MISC";
        public BookLinkRefReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Booklink.Internal.MetaBookLinkRef.GlobalMeta;
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            if (Number != null) 
                res.AppendFormat("[{0}] ", Number);
            if (Pages != null) 
                res.AppendFormat("{0} {1}; ", (lang != null && lang.IsEn ? "pages" : "стр."), Pages);
            Pullenti.Ner.Referent book = Book;
            if (book == null) 
                res.Append("?");
            else 
                res.Append(book.ToString(shortVariant, lang, lev));
            return res.ToString();
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return this.GetSlotValue(ATTR_BOOK) as Pullenti.Ner.Referent;
            }
        }
        /// <summary>
        /// Тип ссылки
        /// </summary>
        public BookLinkRefType Typ
        {
            get
            {
                string val = this.GetStringValue(ATTR_TYPE);
                if (val == null) 
                    return BookLinkRefType.Undefined;
                try 
                {
                    return (BookLinkRefType)Enum.Parse(typeof(BookLinkRefType), val, true);
                }
                catch(Exception ex397) 
                {
                }
                return BookLinkRefType.Undefined;
            }
            set
            {
                this.AddSlot(ATTR_TYPE, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Собственно ссылка вовне на источник - BookLinkReferent или DecreeReferent
        /// </summary>
        public Pullenti.Ner.Referent Book
        {
            get
            {
                return this.GetSlotValue(ATTR_BOOK) as Pullenti.Ner.Referent;
            }
            set
            {
                this.AddSlot(ATTR_BOOK, value, true, 0);
            }
        }
        /// <summary>
        /// Порядковый номер в списке
        /// </summary>
        public string Number
        {
            get
            {
                return this.GetStringValue(ATTR_NUMBER);
            }
            set
            {
                string num = value;
                if (num != null && num.IndexOf('-') > 0) 
                    num = num.Replace(" - ", "-");
                this.AddSlot(ATTR_NUMBER, num, true, 0);
            }
        }
        /// <summary>
        /// Ссылка на страницу или диапазон страниц
        /// </summary>
        public string Pages
        {
            get
            {
                return this.GetStringValue(ATTR_PAGES);
            }
            set
            {
                this.AddSlot(ATTR_PAGES, value, true, 0);
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            BookLinkRefReferent r = obj as BookLinkRefReferent;
            if (r == null) 
                return false;
            if (Book != r.Book) 
                return false;
            if (Number != r.Number) 
                return false;
            if (Pages != r.Pages) 
                return false;
            if ((Typ == BookLinkRefType.Inline) != (r.Typ == BookLinkRefType.Inline)) 
                return false;
            return true;
        }
        /// <summary>
        /// Возвращает разницу номеров r2 - r1, иначе null, если номеров нет
        /// </summary>
        /// <param name="r1">первая ссылка</param>
        /// <param name="r2">вторая ссылка</param>
        public static int? GetNumberDiff(Pullenti.Ner.Referent r1, Pullenti.Ner.Referent r2)
        {
            string num1 = r1.GetStringValue(ATTR_NUMBER);
            string num2 = r2.GetStringValue(ATTR_NUMBER);
            if (num1 == null || num2 == null) 
                return null;
            int n1;
            int n2;
            if (!int.TryParse(num1, out n1) || !int.TryParse(num2, out n2)) 
                return null;
            return n2 - n1;
        }
    }
}