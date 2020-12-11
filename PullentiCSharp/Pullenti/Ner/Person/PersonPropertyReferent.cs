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

namespace Pullenti.Ner.Person
{
    /// <summary>
    /// Сущность - свойство персоны (должность, звание...)
    /// </summary>
    public class PersonPropertyReferent : Pullenti.Ner.Referent
    {
        public PersonPropertyReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Person.Internal.MetaPersonProperty.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("PERSONPROPERTY")
        /// </summary>
        public const string OBJ_TYPENAME = "PERSONPROPERTY";
        /// <summary>
        /// Имя атрибута - наименование
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - дополнительный атрибут
        /// </summary>
        public const string ATTR_ATTR = "ATTR";
        /// <summary>
        /// Имя атрибута - ссылка на сущность (GeoReferent, PersonReferent или OrganizationReferent)
        /// </summary>
        public const string ATTR_REF = "REF";
        /// <summary>
        /// Имя атрибута - для составной должности ссылка на обобщающую должность
        /// </summary>
        public const string ATTR_HIGHER = "HIGHER";
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            if (Name != null) 
                res.Append(Name);
            foreach (Pullenti.Ner.Slot r in Slots) 
            {
                if (r.TypeName == ATTR_ATTR && r.Value != null) 
                    res.AppendFormat(", {0}", r.Value.ToString());
            }
            foreach (Pullenti.Ner.Slot r in Slots) 
            {
                if (r.TypeName == ATTR_REF && (r.Value is Pullenti.Ner.Referent) && (lev < 10)) 
                    res.AppendFormat("; {0}", (r.Value as Pullenti.Ner.Referent).ToString(shortVariant, lang, lev + 1));
            }
            PersonPropertyReferent hi = Higher;
            if (hi != null && hi != this && this.CheckCorrectHigher(hi, 0)) 
                res.AppendFormat("; {0}", hi.ToString(shortVariant, lang, lev + 1));
            return res.ToString();
        }
        public override List<string> GetCompareStrings()
        {
            List<string> res = new List<string>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NAME) 
                    res.Add(s.Value.ToString());
            }
            if (res.Count > 0) 
                return res;
            else 
                return base.GetCompareStrings();
        }
        /// <summary>
        /// Наименование свойства
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetStringValue(ATTR_NAME);
            }
            set
            {
                this.AddSlot(ATTR_NAME, value, true, 0);
            }
        }
        /// <summary>
        /// Вышестоящая должность
        /// </summary>
        public PersonPropertyReferent Higher
        {
            get
            {
                return this._getHigher(0);
            }
            set
            {
                if (this.CheckCorrectHigher(value, 0)) 
                    this.AddSlot(ATTR_HIGHER, value, true, 0);
            }
        }
        PersonPropertyReferent _getHigher(int lev)
        {
            PersonPropertyReferent hi = this.GetSlotValue(ATTR_HIGHER) as PersonPropertyReferent;
            if (hi == null) 
                return null;
            if (!this.CheckCorrectHigher(hi, lev + 1)) 
                return null;
            return hi;
        }
        bool CheckCorrectHigher(PersonPropertyReferent hi, int lev)
        {
            if (hi == null) 
                return true;
            if (hi == this) 
                return false;
            if (lev > 20) 
                return false;
            PersonPropertyReferent hii = hi._getHigher(lev + 1);
            if (hii == null) 
                return true;
            if (hii == this) 
                return false;
            List<PersonPropertyReferent> li = new List<PersonPropertyReferent>();
            li.Add(this);
            for (PersonPropertyReferent pr = hi; pr != null; pr = pr._getHigher(lev + 1)) 
            {
                if (li.Contains(pr)) 
                    return false;
                else 
                    li.Add(pr);
            }
            return true;
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return Higher;
            }
        }
        static List<string> m_Bosses0 = new List<string>(new string[] {"глава", "руководитель"});
        static List<string> m_Bosses1 = new List<string>(new string[] {"президент", "генеральный директор", "директор", "председатель"});
        static int _tmpStack = 0;
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            PersonPropertyReferent pr = obj as PersonPropertyReferent;
            if (pr == null) 
                return false;
            string n1 = Name;
            string n2 = pr.Name;
            if (n1 == null || n2 == null) 
                return false;
            bool eqBosses = false;
            if (n1 != n2) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                    return false;
                if (m_Bosses0.Contains(n1) && m_Bosses1.Contains(n2)) 
                    eqBosses = true;
                else if (m_Bosses1.Contains(n1) && m_Bosses0.Contains(n2)) 
                    eqBosses = true;
                else 
                {
                    if (!n1.StartsWith(n2 + " ") && !n2.StartsWith(n1 + " ")) 
                        return false;
                    eqBosses = true;
                }
                for (PersonPropertyReferent hi = Higher; hi != null; hi = hi.Higher) 
                {
                    if ((++_tmpStack) > 20) 
                    {
                    }
                    else if (hi.CanBeEquals(pr, typ)) 
                    {
                        _tmpStack--;
                        return false;
                    }
                    _tmpStack--;
                }
                for (PersonPropertyReferent hi = pr.Higher; hi != null; hi = hi.Higher) 
                {
                    if ((++_tmpStack) > 20) 
                    {
                    }
                    else if (hi.CanBeEquals(this, typ)) 
                    {
                        _tmpStack--;
                        return false;
                    }
                    _tmpStack--;
                }
            }
            if (Higher != null && pr.Higher != null) 
            {
                if ((++_tmpStack) > 20) 
                {
                }
                else if (!Higher.CanBeEquals(pr.Higher, typ)) 
                {
                    _tmpStack--;
                    return false;
                }
                _tmpStack--;
            }
            if (this.FindSlot("@GENERAL", null, true) != null || pr.FindSlot("@GENERAL", null, true) != null) 
                return this.ToString() == pr.ToString();
            if (this.FindSlot(ATTR_REF, null, true) != null || pr.FindSlot(ATTR_REF, null, true) != null) 
            {
                List<object> refs1 = new List<object>();
                List<object> refs2 = new List<object>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_REF) 
                        refs1.Add(s.Value);
                }
                foreach (Pullenti.Ner.Slot s in pr.Slots) 
                {
                    if (s.TypeName == ATTR_REF) 
                        refs2.Add(s.Value);
                }
                bool eq = false;
                bool noeq = false;
                for (int i = 0; i < refs1.Count; i++) 
                {
                    if (refs2.Contains(refs1[i])) 
                    {
                        eq = true;
                        continue;
                    }
                    noeq = true;
                    if (refs1[i] is Pullenti.Ner.Referent) 
                    {
                        foreach (object rr in refs2) 
                        {
                            if (rr is Pullenti.Ner.Referent) 
                            {
                                if ((rr as Pullenti.Ner.Referent).CanBeEquals(refs1[i] as Pullenti.Ner.Referent, typ)) 
                                {
                                    noeq = false;
                                    eq = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < refs2.Count; i++) 
                {
                    if (refs1.Contains(refs2[i])) 
                    {
                        eq = true;
                        continue;
                    }
                    noeq = true;
                    if (refs2[i] is Pullenti.Ner.Referent) 
                    {
                        foreach (object rr in refs1) 
                        {
                            if (rr is Pullenti.Ner.Referent) 
                            {
                                if ((rr as Pullenti.Ner.Referent).CanBeEquals(refs2[i] as Pullenti.Ner.Referent, typ)) 
                                {
                                    noeq = false;
                                    eq = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (eq && !noeq) 
                {
                }
                else if (noeq && ((eq || refs1.Count == 0 || refs2.Count == 0))) 
                {
                    if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts || n1 != n2) 
                        return false;
                    if (Higher != null || pr.Higher != null) 
                        return false;
                }
                else 
                    return false;
            }
            else if (!eqBosses && n1 != n2) 
                return false;
            return true;
        }
        public override bool CanBeGeneralFor(Pullenti.Ner.Referent obj)
        {
            PersonPropertyReferent pr = obj as PersonPropertyReferent;
            if (pr == null) 
                return false;
            string n1 = Name;
            string n2 = pr.Name;
            if (n1 == null || n2 == null) 
                return false;
            if (this.FindSlot(ATTR_REF, null, true) != null || Higher != null) 
            {
                if (n1 != n2 && n1.StartsWith(n2)) 
                    return this.CanBeEquals(obj, Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts);
                return false;
            }
            if (n1 == n2) 
            {
                if (pr.FindSlot(ATTR_REF, null, true) != null || pr.Higher != null) 
                    return true;
                return false;
            }
            if (n2.StartsWith(n1)) 
            {
                if (n2.StartsWith(n1 + " ")) 
                    return this.CanBeEquals(obj, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText);
            }
            return false;
        }
        /// <summary>
        /// Категория свойства
        /// </summary>
        public PersonPropertyKind Kind
        {
            get
            {
                return Pullenti.Ner.Person.Internal.PersonAttrToken.CheckKind(this);
            }
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_NAME) 
                    oi.Termins.Add(new Pullenti.Ner.Core.Termin(a.Value.ToString()));
            }
            return oi;
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            string nam = Name;
            string nam1 = (obj as PersonPropertyReferent).Name;
            base.MergeSlots(obj, mergeStatistic);
            if (nam != nam1 && nam1 != null && nam != null) 
            {
                Pullenti.Ner.Slot s = null;
                if (nam.StartsWith(nam1)) 
                    s = this.FindSlot(ATTR_NAME, nam1, true);
                else if (nam1.StartsWith(nam)) 
                    s = this.FindSlot(ATTR_NAME, nam, true);
                else if (m_Bosses0.Contains(nam) && m_Bosses1.Contains(nam1)) 
                    s = this.FindSlot(ATTR_NAME, nam, true);
                else if (m_Bosses0.Contains(nam1) && m_Bosses1.Contains(nam)) 
                    s = this.FindSlot(ATTR_NAME, nam1, true);
                if (s != null) 
                    Slots.Remove(s);
            }
        }
        // Проверка, что этот референт может выступать в качестве ATTR_REF
        public bool CanHasRef(Pullenti.Ner.Referent r)
        {
            string nam = Name;
            if (nam == null || r == null) 
                return false;
            if (r is Pullenti.Ner.Geo.GeoReferent) 
            {
                Pullenti.Ner.Geo.GeoReferent g = r as Pullenti.Ner.Geo.GeoReferent;
                if (Pullenti.Morph.LanguageHelper.EndsWithEx(nam, "президент", "губернатор", null, null)) 
                    return g.IsState || g.IsRegion;
                if (nam == "мэр" || nam == "градоначальник") 
                    return g.IsCity;
                if (nam == "глава") 
                    return true;
                return false;
            }
            if (r.TypeName == "ORGANIZATION") 
            {
                if ((Pullenti.Morph.LanguageHelper.EndsWith(nam, "губернатор") || nam == "мэр" || nam == "градоначальник") || nam == "президент") 
                    return false;
                if (nam.Contains("министр")) 
                {
                    if (r.FindSlot(null, "министерство", true) == null) 
                        return false;
                }
                if (nam.EndsWith("директор")) 
                {
                    if ((r.FindSlot(null, "суд", true)) != null) 
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}