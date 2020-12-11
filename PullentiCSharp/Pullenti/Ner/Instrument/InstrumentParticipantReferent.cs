/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Instrument
{
    /// <summary>
    /// Участник НПА (для договора: продавец, агент, исполнитель и т.п.)
    /// </summary>
    public class InstrumentParticipantReferent : Pullenti.Ner.Referent
    {
        public InstrumentParticipantReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Instrument.Internal.InstrumentParticipantMeta.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("INSTRPARTICIPANT")
        /// </summary>
        public const string OBJ_TYPENAME = "INSTRPARTICIPANT";
        /// <summary>
        /// Имя атрибута - тип участника (например, продавец, арендатор, ответчик...)
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - ссылка на сущность (PersonReferent или OrganizationReferent)
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Имя атрибута - представитель участника (PersonReferent)
        /// </summary>
        public const string ATTR_DELEGATE = "DELEGATE";
        /// <summary>
        /// Имя атрибута - основание (на основании чего действует)
        /// </summary>
        public const string ATTR_GROUND = "GROUND";
        /// <summary>
        /// Тип участника
        /// </summary>
        public string Typ
        {
            get
            {
                return this.GetStringValue(ATTR_TYPE);
            }
            set
            {
                this.AddSlot(ATTR_TYPE, (value == null ? null : value.ToUpper()), true, 0);
            }
        }
        /// <summary>
        /// Основание
        /// </summary>
        public object Ground
        {
            get
            {
                return this.GetSlotValue(ATTR_GROUND);
            }
            set
            {
                this.AddSlot(ATTR_GROUND, value, false, 0);
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            res.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(Typ ?? "?"));
            Pullenti.Ner.Referent org = this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            Pullenti.Ner.Referent del = this.GetSlotValue(ATTR_DELEGATE) as Pullenti.Ner.Referent;
            if (org != null) 
            {
                res.AppendFormat(": {0}", org.ToString(shortVariant, lang, 0));
                if (!shortVariant && del != null) 
                    res.AppendFormat(" (в лице {0})", del.ToString(true, lang, lev + 1));
            }
            else if (del != null) 
                res.AppendFormat(": в лице {0}", del.ToString(shortVariant, lang, lev + 1));
            return res.ToString();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            InstrumentParticipantReferent p = obj as InstrumentParticipantReferent;
            if (p == null) 
                return false;
            if (Typ != p.Typ) 
                return false;
            Pullenti.Ner.Referent re1 = this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            Pullenti.Ner.Referent re2 = obj.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            if (re1 != null && re2 != null) 
            {
                if (!re1.CanBeEquals(re2, typ)) 
                    return false;
            }
            return true;
        }
        internal bool ContainsRef(Pullenti.Ner.Referent r)
        {
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (((s.TypeName == ATTR_REF || s.TypeName == ATTR_DELEGATE)) && (s.Value is Pullenti.Ner.Referent)) 
                {
                    if (r == s.Value || r.CanBeEquals(s.Value as Pullenti.Ner.Referent, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                        return true;
                }
            }
            return false;
        }
    }
}