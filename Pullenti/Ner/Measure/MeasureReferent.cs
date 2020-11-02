/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Measure
{
    /// <summary>
    /// Величина или диапазон величин, измеряемая в некоторых единицах
    /// </summary>
    public class MeasureReferent : Pullenti.Ner.Referent
    {
        public MeasureReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Measure.Internal.MeasureMeta.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("MEASURE")
        /// </summary>
        public const string OBJ_TYPENAME = "MEASURE";
        /// <summary>
        /// Имя атрибута - шаблон для значений, например, [1..2], 1x2, 1 ]..1]
        /// </summary>
        public const string ATTR_TEMPLATE = "TEMPLATE";
        /// <summary>
        /// Имя атрибута - значение (м.б. несколько для каждого числа из шаблона)
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - единицы измерения (UnitReferent)
        /// </summary>
        public const string ATTR_UNIT = "UNIT";
        /// <summary>
        /// Имя атрибута - ссылка на уточняющее измерение (MeasureReferent)
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Имя атрибута - наименование перед (если есть)
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - тип (MeasureKind), что измеряется этой величиной
        /// </summary>
        public const string ATTR_KIND = "KIND";
        /// <summary>
        /// Шаблон для значений, например, [1..2], 1x2, 1 ]..1]
        /// </summary>
        public string Template
        {
            get
            {
                return this.GetStringValue(ATTR_TEMPLATE) ?? "1";
            }
            set
            {
                this.AddSlot(ATTR_TEMPLATE, value, true, 0);
            }
        }
        public List<double> DoubleValues
        {
            get
            {
                List<double> res = new List<double>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_VALUE && (s.Value is string)) 
                    {
                        double d;
                        if (Pullenti.Ner.Measure.Internal.MeasureHelper.TryParseDouble(s.Value as string, out d)) 
                            res.Add(d);
                    }
                }
                return res;
            }
        }
        public void AddValue(double d)
        {
            this.AddSlot(ATTR_VALUE, Pullenti.Ner.Core.NumberHelper.DoubleToString(d), false, 0);
        }
        /// <summary>
        /// Список единиц измерения UnitReferent
        /// </summary>
        public List<UnitReferent> Units
        {
            get
            {
                List<UnitReferent> res = new List<UnitReferent>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_UNIT && (s.Value is UnitReferent)) 
                        res.Add(s.Value as UnitReferent);
                }
                return res;
            }
        }
        /// <summary>
        /// Тип, что измеряется этой величиной
        /// </summary>
        public MeasureKind Kind
        {
            get
            {
                string str = this.GetStringValue(ATTR_KIND);
                if (str == null) 
                    return MeasureKind.Undefined;
                try 
                {
                    return (MeasureKind)Enum.Parse(typeof(MeasureKind), str, true);
                }
                catch(Exception ex1736) 
                {
                }
                return MeasureKind.Undefined;
            }
            set
            {
                if (value != MeasureKind.Undefined) 
                    this.AddSlot(ATTR_KIND, value.ToString().ToUpper(), true, 0);
            }
        }
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder(Template);
            List<string> vals = new List<string>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_VALUE) 
                {
                    if (s.Value is string) 
                    {
                        string val = s.Value as string;
                        if (val == "NaN") 
                            val = "?";
                        vals.Add(val);
                    }
                    else if (s.Value is Pullenti.Ner.Referent) 
                        vals.Add((s.Value as Pullenti.Ner.Referent).ToString(true, lang, 0));
                }
            }
            for (int i = res.Length - 1; i >= 0; i--) 
            {
                char ch = res[i];
                if (!char.IsDigit(ch)) 
                    continue;
                int j = (ch - '1');
                if ((j < 0) || j >= vals.Count) 
                    continue;
                res.Remove(i, 1);
                res.Insert(i, vals[j]);
            }
            res.Append(this.OutUnits(lang));
            if (!shortVariant) 
            {
                string nam = this.GetStringValue(ATTR_NAME);
                if (nam != null) 
                    res.AppendFormat(" - {0}", nam);
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_REF && (s.Value is MeasureReferent)) 
                        res.AppendFormat(" / {0}", (s.Value as MeasureReferent).ToString(true, lang, 0));
                }
                MeasureKind ki = Kind;
                if (ki != MeasureKind.Undefined) 
                    res.AppendFormat(" ({0})", ki.ToString().ToUpper());
            }
            return res.ToString();
        }
        /// <summary>
        /// Вывести только единицы измерения
        /// </summary>
        /// <param name="lang">язык</param>
        /// <return>строка с результатом</return>
        public string OutUnits(Pullenti.Morph.MorphLang lang = null)
        {
            List<UnitReferent> uu = Units;
            if (uu.Count == 0) 
                return "";
            StringBuilder res = new StringBuilder();
            res.Append(uu[0].ToString(true, lang, 0));
            for (int i = 1; i < uu.Count; i++) 
            {
                string pow = uu[i].GetStringValue(UnitReferent.ATTR_POW);
                if (!string.IsNullOrEmpty(pow) && pow[0] == '-') 
                {
                    res.AppendFormat("/{0}", uu[i].ToString(true, lang, 1));
                    if (pow != "-1") 
                        res.AppendFormat("<{0}>", pow.Substring(1));
                }
                else 
                    res.AppendFormat("*{0}", uu[i].ToString(true, lang, 0));
            }
            return res.ToString();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            MeasureReferent mr = obj as MeasureReferent;
            if (mr == null) 
                return false;
            if (Template != mr.Template) 
                return false;
            List<string> vals1 = this.GetStringValues(ATTR_VALUE);
            List<string> vals2 = mr.GetStringValues(ATTR_VALUE);
            if (vals1.Count != vals2.Count) 
                return false;
            for (int i = 0; i < vals2.Count; i++) 
            {
                if (vals1[i] != vals2[i]) 
                    return false;
            }
            List<UnitReferent> units1 = Units;
            List<UnitReferent> units2 = mr.Units;
            if (units1.Count != units2.Count) 
                return false;
            for (int i = 0; i < units2.Count; i++) 
            {
                if (units1[i] != units2[i]) 
                    return false;
            }
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_REF || s.TypeName == ATTR_NAME) 
                {
                    if (mr.FindSlot(s.TypeName, s.Value, true) == null) 
                        return false;
                }
            }
            foreach (Pullenti.Ner.Slot s in mr.Slots) 
            {
                if (s.TypeName == ATTR_REF || s.TypeName == ATTR_NAME) 
                {
                    if (this.FindSlot(s.TypeName, s.Value, true) == null) 
                        return false;
                }
            }
            return true;
        }
    }
}