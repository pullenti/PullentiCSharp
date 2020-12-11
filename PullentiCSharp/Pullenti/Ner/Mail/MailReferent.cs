/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Mail
{
    /// <summary>
    /// Сущность - блок письма
    /// </summary>
    public class MailReferent : Pullenti.Ner.Referent
    {
        public MailReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Mail.Internal.MetaLetter.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("MAIL")
        /// </summary>
        public const string OBJ_TYPENAME = "MAIL";
        /// <summary>
        /// Имя атрибута - тип блока (MailKind)
        /// </summary>
        public const string ATTR_KIND = "TYPE";
        /// <summary>
        /// Имя атрибута - текст блока
        /// </summary>
        public const string ATTR_TEXT = "TEXT";
        /// <summary>
        /// Имя атрибута - ссылка на сущность
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Тип блока письма
        /// </summary>
        public MailKind Kind
        {
            get
            {
                string val = this.GetStringValue(ATTR_KIND);
                try 
                {
                    if (val != null) 
                        return (MailKind)Enum.Parse(typeof(MailKind), val, true);
                }
                catch(Exception ex2141) 
                {
                }
                return MailKind.Undefined;
            }
            set
            {
                this.AddSlot(ATTR_KIND, value.ToString().ToUpper(), true, 0);
            }
        }
        /// <summary>
        /// Текст блока
        /// </summary>
        public string Text
        {
            get
            {
                return this.GetStringValue(ATTR_TEXT);
            }
            set
            {
                this.AddSlot(ATTR_TEXT, value, true, 0);
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0}: ", Kind);
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_REF && (s.Value is Pullenti.Ner.Referent)) 
                    res.AppendFormat("{0}, ", (s.Value as Pullenti.Ner.Referent).ToString(true, lang, lev + 1));
            }
            if (res.Length < 100) 
            {
                string str = Text ?? "";
                str = str.Replace('\r', ' ').Replace('\n', ' ');
                if (str.Length > 100) 
                    str = str.Substring(0, 100) + "...";
                res.Append(str);
            }
            return res.ToString();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            return obj == this;
        }
        internal void AddRef(Pullenti.Ner.Referent r, int lev = 0)
        {
            if (r == null || lev > 4) 
                return;
            if ((((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Person.PersonPropertyReferent) || r.TypeName == "ORGANIZATION") || r.TypeName == "PHONE" || r.TypeName == "URI") || (r is Pullenti.Ner.Geo.GeoReferent) || (r is Pullenti.Ner.Address.AddressReferent)) 
                this.AddSlot(ATTR_REF, r, false, 0);
            foreach (Pullenti.Ner.Slot s in r.Slots) 
            {
                if (s.Value is Pullenti.Ner.Referent) 
                    this.AddRef(s.Value as Pullenti.Ner.Referent, lev + 1);
            }
        }
    }
}