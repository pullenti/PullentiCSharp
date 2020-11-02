/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Phone
{
    /// <summary>
    /// Сущность - телефонный номер
    /// </summary>
    public class PhoneReferent : Pullenti.Ner.Referent
    {
        public PhoneReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Phone.Internal.MetaPhone.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("PHONE")
        /// </summary>
        public const string OBJ_TYPENAME = "PHONE";
        /// <summary>
        /// Имя атрибута - номер (слитно, без кода страны)
        /// </summary>
        public const string ATTR_NUNBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - тип (PhoneKind)
        /// </summary>
        public const string ATTR_KIND = "KIND";
        /// <summary>
        /// Имя атрибута - код страны
        /// </summary>
        public const string ATTR_COUNTRYCODE = "COUNTRYCODE";
        /// <summary>
        /// Имя атрибута - добавочный номер
        /// </summary>
        public const string ATTR_ADDNUMBER = "ADDNUMBER";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            if (CountryCode != null) 
                res.AppendFormat("{0}{1} ", (CountryCode != "8" ? "+" : ""), CountryCode);
            string num = Number;
            if (num != null && num.Length >= 9) 
            {
                int cou = 3;
                if (num.Length >= 11) 
                    cou = num.Length - 7;
                res.AppendFormat("({0}) ", num.Substring(0, cou));
                num = num.Substring(cou);
            }
            else if (num != null && num.Length == 8) 
            {
                res.AppendFormat("({0}) ", num.Substring(0, 2));
                num = num.Substring(2);
            }
            if (num == null) 
                res.Append("???-??-??");
            else 
            {
                res.Append(num);
                if (num.Length > 5) 
                {
                    res.Insert(res.Length - 4, '-');
                    res.Insert(res.Length - 2, '-');
                }
            }
            if (AddNumber != null) 
                res.AppendFormat(" (доб.{0})", AddNumber);
            return res.ToString();
        }
        /// <summary>
        /// Основной номер (без кода страны)
        /// </summary>
        public string Number
        {
            get
            {
                return this.GetStringValue(ATTR_NUNBER);
            }
            set
            {
                this.AddSlot(ATTR_NUNBER, value, true, 0);
            }
        }
        /// <summary>
        /// Добавочный номер (если есть)
        /// </summary>
        public string AddNumber
        {
            get
            {
                return this.GetStringValue(ATTR_ADDNUMBER);
            }
            set
            {
                this.AddSlot(ATTR_ADDNUMBER, value, true, 0);
            }
        }
        /// <summary>
        /// Код страны
        /// </summary>
        public string CountryCode
        {
            get
            {
                return this.GetStringValue(ATTR_COUNTRYCODE);
            }
            set
            {
                this.AddSlot(ATTR_COUNTRYCODE, value, true, 0);
            }
        }
        /// <summary>
        /// Тип телефона
        /// </summary>
        public PhoneKind Kind
        {
            get
            {
                string str = this.GetStringValue(ATTR_KIND);
                if (str == null) 
                    return PhoneKind.Undefined;
                try 
                {
                    return (PhoneKind)Enum.Parse(typeof(PhoneKind), str, true);
                }
                catch(Exception ex) 
                {
                    return PhoneKind.Undefined;
                }
            }
            set
            {
                if (value != PhoneKind.Undefined) 
                    this.AddSlot(ATTR_KIND, value.ToString().ToLower(), true, 0);
            }
        }
        public override List<string> GetCompareStrings()
        {
            string num = Number;
            if (num == null) 
                return null;
            if (num.Length > 9) 
                num = num.Substring(9);
            List<string> res = new List<string>();
            res.Add(num);
            string add = AddNumber;
            if (add != null) 
                res.Add(string.Format("{0}*{1}", num, add));
            return res;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            return this._canBeEqual(obj, typ, false);
        }
        bool _canBeEqual(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ, bool ignoreAddNumber)
        {
            PhoneReferent ph = obj as PhoneReferent;
            if (ph == null) 
                return false;
            if (ph.CountryCode != null && CountryCode != null) 
            {
                if (ph.CountryCode != CountryCode) 
                    return false;
            }
            if (ignoreAddNumber) 
            {
                if (AddNumber != null && ph.AddNumber != null) 
                {
                    if (ph.AddNumber != AddNumber) 
                        return false;
                }
            }
            else if (AddNumber != null || ph.AddNumber != null) 
            {
                if (AddNumber != ph.AddNumber) 
                    return false;
            }
            if (Number == null || ph.Number == null) 
                return false;
            if (Number == ph.Number) 
                return true;
            if (typ != Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
            {
                if (Pullenti.Morph.LanguageHelper.EndsWith(Number, ph.Number) || Pullenti.Morph.LanguageHelper.EndsWith(ph.Number, Number)) 
                    return true;
            }
            return false;
        }
        public override bool CanBeGeneralFor(Pullenti.Ner.Referent obj)
        {
            if (!this._canBeEqual(obj, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText, true)) 
                return false;
            PhoneReferent ph = obj as PhoneReferent;
            if (CountryCode != null && ph.CountryCode == null) 
                return false;
            if (AddNumber == null) 
            {
                if (ph.AddNumber != null) 
                    return true;
            }
            else if (ph.AddNumber == null) 
                return false;
            if (Pullenti.Morph.LanguageHelper.EndsWith(ph.Number, Number)) 
                return true;
            return false;
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            PhoneReferent ph = obj as PhoneReferent;
            if (ph == null) 
                return;
            if (ph.CountryCode != null && CountryCode == null) 
                CountryCode = ph.CountryCode;
            if (ph.Number != null && Pullenti.Morph.LanguageHelper.EndsWith(ph.Number, Number)) 
                Number = ph.Number;
        }
        internal string m_Template;
        internal void Correct()
        {
            if (Kind == PhoneKind.Undefined) 
            {
                if (this.FindSlot(ATTR_ADDNUMBER, null, true) != null) 
                    Kind = PhoneKind.Work;
                else if (CountryCode == null || CountryCode == "7") 
                {
                    string num = Number;
                    if (num.Length == 10 && num[0] == '9') 
                        Kind = PhoneKind.Mobile;
                }
            }
        }
    }
}