/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Bank
{
    /// <summary>
    /// Банковские данные (реквизиты)
    /// </summary>
    public class BankDataReferent : Pullenti.Ner.Referent
    {
        public BankDataReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Bank.Internal.MetaBank.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("BANKDATA")
        /// </summary>
        public const string OBJ_TYPENAME = "BANKDATA";
        /// <summary>
        /// Имя атрибута - реквизит (обычно UriReferent)
        /// </summary>
        public const string ATTR_ITEM = "ITEM";
        /// <summary>
        /// Имя атрибута - банк (обычно OrganizationReferent)
        /// </summary>
        public const string ATTR_BANK = "BANK";
        /// <summary>
        /// Имя атрибута - банк К/С
        /// </summary>
        public const string ATTR_CORBANK = "CORBANK";
        /// <summary>
        /// Имя атрибута - разное
        /// </summary>
        public const string ATTR_MISC = "MISC";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.Value is Pullenti.Ner.Uri.UriReferent) 
                {
                    if ((s.Value as Pullenti.Ner.Uri.UriReferent).Scheme == "Р/С") 
                    {
                        res.Append(s.Value.ToString());
                        break;
                    }
                }
            }
            if (res.Length == 0) 
                res.Append(this.GetStringValue(ATTR_ITEM) ?? "?");
            if (ParentReferent != null && !shortVariant && (lev < 20)) 
                res.AppendFormat(", {0}", ParentReferent.ToString(true, lang, lev + 1));
            return res.ToString();
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return this.GetSlotValue(ATTR_BANK) as Pullenti.Ner.Referent;
            }
        }
        public string FindValue(string schema)
        {
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.Value is Pullenti.Ner.Uri.UriReferent) 
                {
                    Pullenti.Ner.Uri.UriReferent ur = s.Value as Pullenti.Ner.Uri.UriReferent;
                    if (ur.Scheme == schema) 
                        return ur.Value;
                }
            }
            return null;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            BankDataReferent bd = obj as BankDataReferent;
            if (bd == null) 
                return false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_ITEM) 
                {
                    Pullenti.Ner.Uri.UriReferent ur = s.Value as Pullenti.Ner.Uri.UriReferent;
                    string val = bd.FindValue(ur.Scheme);
                    if (val != null) 
                    {
                        if (val != ur.Value) 
                            return false;
                    }
                }
                else if (s.TypeName == ATTR_BANK) 
                {
                    Pullenti.Ner.Referent b1 = s.Value as Pullenti.Ner.Referent;
                    Pullenti.Ner.Referent b2 = bd.GetSlotValue(ATTR_BANK) as Pullenti.Ner.Referent;
                    if (b2 != null) 
                    {
                        if (b1 != b2 && !b1.CanBeEquals(b2, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            return false;
                    }
                }
            }
            return true;
        }
    }
}