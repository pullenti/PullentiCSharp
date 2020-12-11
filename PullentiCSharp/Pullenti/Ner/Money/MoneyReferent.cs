/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Money
{
    /// <summary>
    /// Сущность - денежная сумма
    /// </summary>
    public class MoneyReferent : Pullenti.Ner.Referent
    {
        public MoneyReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Money.Internal.MoneyMeta.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("MONEY")
        /// </summary>
        public const string OBJ_TYPENAME = "MONEY";
        /// <summary>
        /// Имя атрибута - валюта (3-х значный код ISO 4217)
        /// </summary>
        public const string ATTR_CURRENCY = "CURRENCY";
        /// <summary>
        /// Имя атрибута - значение (целая часть)
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - альтернативное значение (когда в скобках ошибочно указано другле число)
        /// </summary>
        public const string ATTR_ALTVALUE = "ALTVALUE";
        /// <summary>
        /// Имя атрибута - дробная часть ("копейки")
        /// </summary>
        public const string ATTR_REST = "REST";
        /// <summary>
        /// Имя атрибута - альтернативная дробная часть (когда в скобках указано другое число)
        /// </summary>
        public const string ATTR_ALTREST = "ALTREST";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            string v = this.GetStringValue(ATTR_VALUE);
            int r = Rest;
            if (v != null || r > 0) 
            {
                res.Append(v ?? "0");
                int cou = 0;
                for (int i = res.Length - 1; i > 0; i--) 
                {
                    if ((++cou) == 3) 
                    {
                        res.Insert(i, '.');
                        cou = 0;
                    }
                }
            }
            else 
                res.Append("?");
            if (r > 0) 
                res.AppendFormat(",{0}", r.ToString("D02"));
            res.AppendFormat(" {0}", Currency);
            return res.ToString();
        }
        /// <summary>
        /// Тип валюты (3-х значный код ISO 4217)
        /// </summary>
        public string Currency
        {
            get
            {
                return this.GetStringValue(ATTR_CURRENCY);
            }
            set
            {
                this.AddSlot(ATTR_CURRENCY, value, true, 0);
            }
        }
        /// <summary>
        /// Значение целой части
        /// </summary>
        public double Value
        {
            get
            {
                string val = this.GetStringValue(ATTR_VALUE);
                if (val == null) 
                    return 0;
                double v;
                if (!double.TryParse(val, out v)) 
                    return 0;
                return v;
            }
        }
        /// <summary>
        /// Альтернативное значение (если есть, то значит неправильно написали сумму 
        /// числом и далее прописью в скобках)
        /// </summary>
        public double? AltValue
        {
            get
            {
                string val = this.GetStringValue(ATTR_ALTVALUE);
                if (val == null) 
                    return null;
                double v;
                if (!double.TryParse(val, out v)) 
                    return null;
                return v;
            }
        }
        /// <summary>
        /// Остаток (от 0 до 99) - копеек, центов и т.п.
        /// </summary>
        public int Rest
        {
            get
            {
                string val = this.GetStringValue(ATTR_REST);
                if (val == null) 
                    return 0;
                int v;
                if (!int.TryParse(val, out v)) 
                    return 0;
                return v;
            }
        }
        /// <summary>
        /// Альтернативный остаток (от 0 до 99) - копеек, центов и т.п.
        /// </summary>
        public int? AltRest
        {
            get
            {
                string val = this.GetStringValue(ATTR_ALTREST);
                if (val == null) 
                    return null;
                int v;
                if (!int.TryParse(val, out v)) 
                    return null;
                return v;
            }
        }
        /// <summary>
        /// Действительное значение (вместе с копейками)
        /// </summary>
        public double RealValue
        {
            get
            {
                return ((double)Value) + ((((double)Rest) / 100));
            }
            set
            {
                string val = Pullenti.Ner.Core.NumberHelper.DoubleToString(value);
                int ii = val.IndexOf('.');
                if (ii > 0) 
                    val = val.Substring(0, ii);
                this.AddSlot(ATTR_VALUE, val, true, 0);
                double re = ((value - Value)) * 100;
                this.AddSlot(ATTR_REST, ((int)((re + 0.0001))).ToString(), true, 0);
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            MoneyReferent s = obj as MoneyReferent;
            if (s == null) 
                return false;
            if (s.Currency != Currency) 
                return false;
            if (s.Value != Value) 
                return false;
            if (s.Rest != Rest) 
                return false;
            if (s.AltValue != AltValue) 
                return false;
            if (s.AltRest != AltRest) 
                return false;
            return true;
        }
    }
}