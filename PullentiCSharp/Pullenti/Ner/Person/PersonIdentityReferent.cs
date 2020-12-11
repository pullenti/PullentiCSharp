/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Person
{
    /// <summary>
    /// Удостоверение личности (паспорт и пр.)
    /// </summary>
    public class PersonIdentityReferent : Pullenti.Ner.Referent
    {
        public PersonIdentityReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Person.Internal.MetaPersonIdentity.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("NAMEDENTITY")
        /// </summary>
        public const string OBJ_TYPENAME = "PERSONIDENTITY";
        /// <summary>
        /// Имя атрибута - тип документа
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - серийный номер
        /// </summary>
        public const string ATTR_NUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - дата выдачи
        /// </summary>
        public const string ATTR_DATE = "DATE";
        /// <summary>
        /// Имя атрибута - выдавшая организация (OrganizationReferent)
        /// </summary>
        public const string ATTR_ORG = "ORG";
        /// <summary>
        /// Имя атрибута - географический объект (GeoReferent)
        /// </summary>
        public const string ATTR_STATE = "STATE";
        /// <summary>
        /// Имя атрибута - адрес регистрации (AddressReferent)
        /// </summary>
        public const string ATTR_ADDRESS = "ADDRESS";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            res.Append(Typ ?? "?");
            if (Number != null) 
                res.AppendFormat(" №{0}", Number);
            if (State != null) 
                res.AppendFormat(", {0}", State.ToString(true, lang, lev + 1));
            if (!shortVariant) 
            {
                string dat = this.GetStringValue(ATTR_DATE);
                string org = this.GetStringValue(ATTR_ORG);
                if (dat != null || org != null) 
                {
                    res.Append(", выдан");
                    if (dat != null) 
                        res.AppendFormat(" {0}", dat);
                    if (org != null) 
                        res.AppendFormat(" {0}", org);
                }
            }
            return res.ToString();
        }
        /// <summary>
        /// Тип документа
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
        /// Номер (вместе с серией)
        /// </summary>
        public string Number
        {
            get
            {
                return this.GetStringValue(ATTR_NUMBER);
            }
            set
            {
                this.AddSlot(ATTR_NUMBER, value, true, 0);
            }
        }
        /// <summary>
        /// Государство
        /// </summary>
        public Pullenti.Ner.Referent State
        {
            get
            {
                return this.GetSlotValue(ATTR_STATE) as Pullenti.Ner.Referent;
            }
            set
            {
                this.AddSlot(ATTR_STATE, value, true, 0);
            }
        }
        /// <summary>
        /// Адрес регистрации
        /// </summary>
        public Pullenti.Ner.Referent Address
        {
            get
            {
                return this.GetSlotValue(ATTR_ADDRESS) as Pullenti.Ner.Referent;
            }
            set
            {
                this.AddSlot(ATTR_ADDRESS, value, true, 0);
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            PersonIdentityReferent id = obj as PersonIdentityReferent;
            if (id == null) 
                return false;
            if (Typ != id.Typ) 
                return false;
            if (Number != id.Number) 
                return false;
            if (State != null && id.State != null) 
            {
                if (State != id.State) 
                    return false;
            }
            return true;
        }
    }
}