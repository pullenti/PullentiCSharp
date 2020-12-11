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

namespace Pullenti.Ner.Goods
{
    /// <summary>
    /// Атрибут товара
    /// </summary>
    public class GoodAttributeReferent : Pullenti.Ner.Referent
    {
        public GoodAttributeReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Goods.Internal.AttrMeta.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("GOODATTR")
        /// </summary>
        public const string OBJ_TYPENAME = "GOODATTR";
        /// <summary>
        /// Имя атрибута - тип атрибута (GoodAttrType)
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - значение атрибута
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - альтернативное значение атрибута
        /// </summary>
        public const string ATTR_ALTVALUE = "ALTVALUE";
        /// <summary>
        /// Имя атрибута - единица измерения
        /// </summary>
        public const string ATTR_UNIT = "UNIT";
        /// <summary>
        /// Имя атрибута - наименование атрибута
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - сслыка на сущность (Referent)
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Тип атрибута
        /// </summary>
        public GoodAttrType Typ
        {
            get
            {
                string str = this.GetStringValue(ATTR_TYPE);
                if (str == null) 
                    return GoodAttrType.Undefined;
                try 
                {
                    return (GoodAttrType)Enum.Parse(typeof(GoodAttrType), str, true);
                }
                catch(Exception ex1796) 
                {
                }
                return GoodAttrType.Undefined;
            }
            set
            {
                this.AddSlot(ATTR_TYPE, value.ToString().ToUpper(), true, 0);
            }
        }
        /// <summary>
        /// Значения (список string)
        /// </summary>
        public List<string> Values
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_VALUE && (s.Value is string)) 
                    {
                        string v = s.Value as string;
                        if (v.IndexOf('(') > 0) 
                        {
                            if (Typ == GoodAttrType.Numeric) 
                                v = v.Substring(0, v.IndexOf('(')).Trim();
                        }
                        res.Add(v);
                    }
                }
                return res;
            }
        }
        /// <summary>
        /// Альтернативное представление значений (список string). Например, для значение ИКЕЯ здесь 
        /// будут варианты написаний на латинице типа IKEA, IKEYA ...
        /// </summary>
        public List<string> AltValues
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_ALTVALUE && (s.Value is string)) 
                        res.Add(s.Value as string);
                }
                return res;
            }
        }
        /// <summary>
        /// Единицы измерения (список string)
        /// </summary>
        public List<string> Units
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_UNIT && (s.Value is string)) 
                        res.Add(s.Value as string);
                }
                return res;
            }
        }
        /// <summary>
        /// Ссылка на внешнюю сущность
        /// </summary>
        public Pullenti.Ner.Referent Ref
        {
            get
            {
                return this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            }
            set
            {
                this.AddSlot(ATTR_REF, value, true, 0);
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            GoodAttrType typ = Typ;
            string nam = this.GetStringValue(ATTR_NAME);
            if (!shortVariant) 
            {
                if (typ != GoodAttrType.Undefined) 
                    res.AppendFormat("{0}{1}: ", Pullenti.Ner.Goods.Internal.AttrMeta.GlobalMeta.TypAttr.ConvertInnerValueToOuterValue(typ.ToString(), lang), (nam == null ? "" : string.Format(" ({0})", nam.ToLower())));
            }
            string s = this.GetStringValue(ATTR_VALUE);
            if (s != null) 
            {
                if (typ == GoodAttrType.Keyword || typ == GoodAttrType.Character) 
                    res.Append(s.ToLower());
                else if (typ == GoodAttrType.Numeric) 
                {
                    List<string> vals = Values;
                    List<string> units = Units;
                    for (int i = 0; i < vals.Count; i++) 
                    {
                        if (i > 0) 
                            res.Append(" x ");
                        res.Append(vals[i]);
                        if (vals.Count == units.Count) 
                            res.Append(units[i].ToLower());
                        else if (units.Count > 0) 
                            res.Append(units[0].ToLower());
                    }
                }
                else 
                    res.Append(s);
            }
            Pullenti.Ner.Referent re = Ref;
            if (re != null) 
                res.Append(re.ToString(shortVariant, lang, 0));
            return res.ToString();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            GoodAttributeReferent a = obj as GoodAttributeReferent;
            if (a == null) 
                return false;
            if (a.Typ != Typ) 
                return false;
            string u1 = this.GetStringValue(ATTR_UNIT);
            string u2 = a.GetStringValue(ATTR_UNIT);
            if (u1 != null && u2 != null) 
            {
                if (u1 != u2) 
                {
                    if (u1.Length == (u2.Length + 1) && u1 == (u2 + ".")) 
                    {
                    }
                    else if (u2.Length == (u1.Length + 1) && u2 == (u1 + ".")) 
                    {
                    }
                    return false;
                }
            }
            string nam1 = this.GetStringValue(ATTR_NAME);
            string nam2 = a.GetStringValue(ATTR_NAME);
            if (nam1 != null || nam2 != null) 
            {
                if (nam1 != nam2) 
                    return false;
            }
            bool eq = false;
            if (Ref != null || a.Ref != null) 
            {
                if (Ref == null || a.Ref == null) 
                    return false;
                if (!Ref.CanBeEquals(a.Ref, typ)) 
                    return false;
                eq = true;
            }
            if (Typ != GoodAttrType.Numeric) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_VALUE || s.TypeName == ATTR_ALTVALUE) 
                    {
                        if (a.FindSlot(ATTR_VALUE, s.Value, true) != null || a.FindSlot(ATTR_ALTVALUE, s.Value, true) != null) 
                        {
                            eq = true;
                            break;
                        }
                    }
                }
            }
            else 
            {
                List<string> vals1 = Values;
                List<string> vals2 = a.Values;
                if (vals1.Count != vals2.Count) 
                    return false;
                foreach (string v in vals1) 
                {
                    if (!vals2.Contains(v)) 
                        return false;
                }
            }
            if (!eq) 
                return false;
            return true;
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            base.MergeSlots(obj, mergeStatistic);
            for (int i = Slots.Count - 1; i >= 0; i--) 
            {
                if (Slots[i].TypeName == ATTR_ALTVALUE) 
                {
                    if (this.FindSlot(ATTR_VALUE, Slots[i].Value, true) != null) 
                        Slots.RemoveAt(i);
                }
            }
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem re = new Pullenti.Ner.Core.IntOntologyItem(this);
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_VALUE || s.TypeName == ATTR_ALTVALUE) 
                    re.Termins.Add(new Pullenti.Ner.Core.Termin(s.Value as string));
            }
            return re;
        }
    }
}