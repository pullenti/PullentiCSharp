/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Named
{
    /// <summary>
    /// Сущность "тип" + "имя" (планеты, памятники, здания, местоположения, планеты и пр.)
    /// </summary>
    public class NamedEntityReferent : Pullenti.Ner.Referent
    {
        public NamedEntityReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Named.Internal.MetaNamedEntity.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("NAMEDENTITY")
        /// </summary>
        public const string OBJ_TYPENAME = "NAMEDENTITY";
        /// <summary>
        /// Имя атрибута - наименование
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - категория сущности (NamedEntityKind)
        /// </summary>
        public const string ATTR_KIND = "KIND";
        /// <summary>
        /// Имя атрибута - тип
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - ссылка на другую сущность
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Имя атрибута - разное
        /// </summary>
        public const string ATTR_MISC = "MISC";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            string typ = this.GetStringValue(ATTR_TYPE);
            if (typ != null) 
                res.Append(typ);
            string name = this.GetStringValue(ATTR_NAME);
            if (name != null) 
            {
                if (res.Length > 0) 
                    res.Append(' ');
                res.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(name));
            }
            Pullenti.Ner.Referent re = this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            if (re != null) 
            {
                if (res.Length > 0) 
                    res.Append("; ");
                res.Append(re.ToString(shortVariant, lang, lev + 1));
            }
            return res.ToString();
        }
        /// <summary>
        /// Класс сущности
        /// </summary>
        public NamedEntityKind Kind
        {
            get
            {
                string str = this.GetStringValue(ATTR_KIND);
                if (str == null) 
                    return NamedEntityKind.Undefined;
                try 
                {
                    return (NamedEntityKind)Enum.Parse(typeof(NamedEntityKind), str, true);
                }
                catch(Exception ex1746) 
                {
                }
                return NamedEntityKind.Undefined;
            }
            set
            {
                this.AddSlot(ATTR_KIND, value.ToString().ToLower(), true, 0);
            }
        }
        public override string ToSortString()
        {
            return Kind + this.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0);
        }
        public override List<string> GetCompareStrings()
        {
            List<string> res = new List<string>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NAME) 
                {
                    string str = s.Value.ToString();
                    if (!res.Contains(str)) 
                        res.Add(str);
                    if (str.IndexOf(' ') > 0 || str.IndexOf('-') > 0) 
                    {
                        str = str.Replace(" ", "").Replace("-", "");
                        if (!res.Contains(str)) 
                            res.Add(str);
                    }
                }
            }
            if (res.Count == 0) 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_TYPE) 
                    {
                        string t = s.Value.ToString();
                        if (!res.Contains(t)) 
                            res.Add(t);
                    }
                }
            }
            if (res.Count > 0) 
                return res;
            else 
                return base.GetCompareStrings();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            NamedEntityReferent ent = obj as NamedEntityReferent;
            if (ent == null) 
                return false;
            if (ent.Kind != Kind) 
                return false;
            List<string> names = this.GetStringValues(ATTR_NAME);
            List<string> names2 = obj.GetStringValues(ATTR_NAME);
            bool eqNames = false;
            if ((names != null && names.Count > 0 && names2 != null) && names2.Count > 0) 
            {
                foreach (string n in names) 
                {
                    if (names2.Contains(n)) 
                        eqNames = true;
                }
                if (!eqNames) 
                    return false;
            }
            List<string> typs = this.GetStringValues(ATTR_TYPE);
            List<string> typs2 = obj.GetStringValues(ATTR_TYPE);
            bool eqTyps = false;
            if ((typs != null && typs.Count > 0 && typs2 != null) && typs2.Count > 0) 
            {
                foreach (string ty in typs) 
                {
                    if (typs2.Contains(ty)) 
                        eqTyps = true;
                }
                if (!eqTyps) 
                    return false;
            }
            if (!eqTyps && !eqNames) 
                return false;
            Pullenti.Ner.Referent re1 = this.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            Pullenti.Ner.Referent re2 = obj.GetSlotValue(ATTR_REF) as Pullenti.Ner.Referent;
            if (re1 != null && re2 != null) 
            {
                if (!re1.CanBeEquals(re2, typ)) 
                    return false;
            }
            else if (re1 != null || re2 != null) 
            {
            }
            return true;
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            return this._CreateOntologyItem(2, false, false);
        }
        internal Pullenti.Ner.Core.IntOntologyItem _CreateOntologyItem(int minLen, bool onlyNames = false, bool pureNames = false)
        {
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            List<string> vars = new List<string>();
            List<string> typs = this.GetStringValues(ATTR_TYPE) ?? new List<string>();
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_NAME) 
                {
                    string s = a.Value.ToString().ToUpper();
                    if (!vars.Contains(s)) 
                        vars.Add(s);
                    if (!pureNames) 
                    {
                        int sp = 0;
                        for (int jj = 0; jj < s.Length; jj++) 
                        {
                            if (s[jj] == ' ') 
                                sp++;
                        }
                        if (sp == 1) 
                        {
                            s = s.Replace(" ", "");
                            if (!vars.Contains(s)) 
                                vars.Add(s);
                        }
                    }
                }
            }
            if (!onlyNames) 
            {
                if (vars.Count == 0) 
                {
                    foreach (string t in typs) 
                    {
                        string up = t.ToUpper();
                        if (!vars.Contains(up)) 
                            vars.Add(up);
                    }
                }
            }
            int max = 20;
            int cou = 0;
            foreach (string v in vars) 
            {
                if (v.Length >= minLen) 
                {
                    oi.Termins.Add(new Pullenti.Ner.Core.Termin(v));
                    if ((++cou) >= max) 
                        break;
                }
            }
            if (oi.Termins.Count == 0) 
                return null;
            return oi;
        }
    }
}