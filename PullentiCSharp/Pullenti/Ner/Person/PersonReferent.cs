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
    /// Сущность - персона
    /// </summary>
    public class PersonReferent : Pullenti.Ner.Referent
    {
        public PersonReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Person.Internal.MetaPerson.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("PERSON")
        /// </summary>
        public const string OBJ_TYPENAME = "PERSON";
        /// <summary>
        /// Имя атрибута - пол
        /// </summary>
        public const string ATTR_SEX = "SEX";
        /// <summary>
        /// Имя атрибута - слитно полное имя, если не удалось разбить на ФИО по отдельности
        /// </summary>
        public const string ATTR_IDENTITY = "IDENTITY";
        /// <summary>
        /// Имя атрибута - имя
        /// </summary>
        public const string ATTR_FIRSTNAME = "FIRSTNAME";
        /// <summary>
        /// Имя атрибута - отчество
        /// </summary>
        public const string ATTR_MIDDLENAME = "MIDDLENAME";
        /// <summary>
        /// Имя атрибута - фамилия
        /// </summary>
        public const string ATTR_LASTNAME = "LASTNAME";
        /// <summary>
        /// Имя атрибута - кличка или номер
        /// </summary>
        public const string ATTR_NICKNAME = "NICKNAME";
        /// <summary>
        /// Имя атрибута - свойство (PersonPropertyReferent)
        /// </summary>
        public const string ATTR_ATTR = "ATTRIBUTE";
        /// <summary>
        /// Имя атрибута - возраст
        /// </summary>
        public const string ATTR_AGE = "AGE";
        /// <summary>
        /// Имя атрибута - дата рождения
        /// </summary>
        public const string ATTR_BORN = "BORN";
        /// <summary>
        /// Имя атрибута - дата смерти
        /// </summary>
        public const string ATTR_DIE = "DIE";
        /// <summary>
        /// Имя атрибута - контактная информация
        /// </summary>
        public const string ATTR_CONTACT = "CONTACT";
        /// <summary>
        /// Имя атрибута - удостоверяющий документ (PersonIdentityReferent)
        /// </summary>
        public const string ATTR_IDDOC = "IDDOC";
        /// <summary>
        /// Это мужчина
        /// </summary>
        public bool IsMale
        {
            get
            {
                return this.GetStringValue(ATTR_SEX) == Pullenti.Ner.Person.Internal.MetaPerson.ATTR_SEXMALE;
            }
            set
            {
                this.AddSlot(ATTR_SEX, Pullenti.Ner.Person.Internal.MetaPerson.ATTR_SEXMALE, true, 0);
            }
        }
        /// <summary>
        /// Это женщина
        /// </summary>
        public bool IsFemale
        {
            get
            {
                return this.GetStringValue(ATTR_SEX) == Pullenti.Ner.Person.Internal.MetaPerson.ATTR_SEXFEMALE;
            }
            set
            {
                this.AddSlot(ATTR_SEX, Pullenti.Ner.Person.Internal.MetaPerson.ATTR_SEXFEMALE, true, 0);
            }
        }
        /// <summary>
        /// Возраст
        /// </summary>
        public int Age
        {
            get
            {
                int i = this.GetIntValue(ATTR_AGE, 0);
                if (i > 0) 
                    return i;
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_AGE, value.ToString(), true, 0);
            }
        }
        internal void AddContact(Pullenti.Ner.Referent contact)
        {
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_CONTACT) 
                {
                    Pullenti.Ner.Referent r = s.Value as Pullenti.Ner.Referent;
                    if (r != null) 
                    {
                        if (r.CanBeGeneralFor(contact)) 
                        {
                            this.UploadSlot(s, contact);
                            return;
                        }
                        if (r.CanBeEquals(contact, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            return;
                    }
                }
            }
            this.AddSlot(ATTR_CONTACT, contact, false, 0);
        }
        string _getPrefix()
        {
            if (IsMale) 
                return "г-н ";
            if (IsFemale) 
                return "г-жа ";
            return string.Empty;
        }
        string _findForSurname(string attrName, string surname, bool findShortest = false)
        {
            bool rus = Pullenti.Morph.LanguageHelper.IsCyrillicChar(surname[0]);
            string res = null;
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == attrName) 
                {
                    string v = a.Value.ToString();
                    if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(v[0]) != rus) 
                        continue;
                    if (res == null) 
                        res = v;
                    else if (findShortest && (v.Length < res.Length)) 
                        res = v;
                }
            }
            return res;
        }
        string _findShortestValue(string attrName)
        {
            string res = null;
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == attrName) 
                {
                    string v = a.Value.ToString();
                    if (res == null || (v.Length < res.Length)) 
                        res = v;
                }
            }
            return res;
        }
        string _findShortestKingTitul(bool doName = false)
        {
            string res = null;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.Value is PersonPropertyReferent) 
                {
                    PersonPropertyReferent pr = s.Value as PersonPropertyReferent;
                    if (pr.Kind != PersonPropertyKind.King) 
                        continue;
                    foreach (Pullenti.Ner.Slot ss in pr.Slots) 
                    {
                        if (ss.TypeName == PersonPropertyReferent.ATTR_NAME) 
                        {
                            string n = ss.Value as string;
                            if (res == null) 
                                res = n;
                            else if (res.Length > n.Length) 
                                res = n;
                        }
                    }
                }
            }
            if (res != null || !doName) 
                return res;
            return null;
        }
        public override string ToSortString()
        {
            string sur = null;
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_IDENTITY) 
                    return a.Value.ToString();
                else if (a.TypeName == ATTR_LASTNAME) 
                {
                    sur = a.Value.ToString();
                    break;
                }
            }
            if (sur == null) 
            {
                string tit = this._findShortestKingTitul(false);
                if (tit == null) 
                    return "?";
                string s = this.GetStringValue(ATTR_FIRSTNAME);
                if (s == null) 
                    return "?";
                return string.Format("{0} {1}", tit, s);
            }
            string n = this._findForSurname(ATTR_FIRSTNAME, sur, false);
            if (n == null) 
                return sur;
            else 
                return string.Format("{0} {1}", sur, n);
        }
        public override List<string> GetCompareStrings()
        {
            List<string> res = new List<string>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_LASTNAME || s.TypeName == ATTR_IDENTITY) 
                    res.Add(s.Value.ToString());
            }
            string tit = this._findShortestKingTitul(false);
            if (tit != null) 
            {
                string nam = this.GetStringValue(ATTR_FIRSTNAME);
                if (nam != null) 
                    res.Add(string.Format("{0} {1}", tit, nam));
            }
            if (res.Count > 0) 
                return res;
            else 
                return base.GetCompareStrings();
        }
        /// <summary>
        /// При выводе в ToString() первым ставить фамилию, а не имя
        /// </summary>
        public static bool ShowLastnameOnFirstPosition = false;
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            if (shortVariant) 
                return this.toShortString(lang);
            else 
            {
                string res = this.toFullString(ShowLastnameOnFirstPosition, lang);
                if (this.FindSlot(ATTR_NICKNAME, null, true) == null) 
                    return res;
                List<string> niks = this.GetStringValues(ATTR_NICKNAME);
                if (niks.Count == 1) 
                    return string.Format("{0} ({1})", res, Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(niks[0]));
                StringBuilder tmp = new StringBuilder();
                tmp.Append(res);
                tmp.Append(" (");
                foreach (string s in niks) 
                {
                    if (s != niks[0]) 
                        tmp.Append(", ");
                    tmp.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(s));
                }
                tmp.Append(")");
                return tmp.ToString();
            }
        }
        string toShortString(Pullenti.Morph.MorphLang lang)
        {
            string id = null;
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_IDENTITY) 
                {
                    string s = a.Value.ToString();
                    if (id == null || (s.Length < id.Length)) 
                        id = s;
                }
            }
            if (id != null) 
                return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(id);
            string n = this.GetStringValue(ATTR_LASTNAME);
            if (n != null) 
            {
                StringBuilder res = new StringBuilder();
                res.Append(n);
                string s = this._findForSurname(ATTR_FIRSTNAME, n, true);
                if (s != null) 
                {
                    res.AppendFormat(" {0}.", s[0]);
                    s = this._findForSurname(ATTR_MIDDLENAME, n, false);
                    if (s != null) 
                        res.AppendFormat("{0}.", s[0]);
                }
                return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(res.ToString());
            }
            string tit = this._findShortestKingTitul(true);
            if (tit != null) 
            {
                string nam = this.GetStringValue(ATTR_FIRSTNAME);
                if (nam != null) 
                    return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(string.Format("{0} {1}", tit, nam));
            }
            return this.toFullString(false, lang);
        }
        string toFullString(bool lastNameFirst, Pullenti.Morph.MorphLang lang)
        {
            string id = null;
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_IDENTITY) 
                {
                    string s = a.Value.ToString();
                    if (id == null || s.Length > id.Length) 
                        id = s;
                }
            }
            if (id != null) 
                return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(id);
            string sss = this.GetStringValue("NAMETYPE");
            if (sss == "china") 
                lastNameFirst = true;
            string n = this.GetStringValue(ATTR_LASTNAME);
            if (n != null) 
            {
                StringBuilder res = new StringBuilder();
                if (lastNameFirst) 
                    res.AppendFormat("{0} ", n);
                string s = this._findForSurname(ATTR_FIRSTNAME, n, false);
                if (s != null) 
                {
                    res.AppendFormat("{0}", s);
                    if (IsInitial(s)) 
                        res.Append('.');
                    else 
                        res.Append(' ');
                    s = this._findForSurname(ATTR_MIDDLENAME, n, false);
                    if (s != null) 
                    {
                        res.AppendFormat("{0}", s);
                        if (IsInitial(s)) 
                            res.Append('.');
                        else 
                            res.Append(' ');
                    }
                }
                if (!lastNameFirst) 
                    res.Append(n);
                else if (res[res.Length - 1] == ' ') 
                    res.Length--;
                if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(n[0])) 
                {
                    string nl = null;
                    foreach (Pullenti.Ner.Slot sl in Slots) 
                    {
                        if (sl.TypeName == ATTR_LASTNAME) 
                        {
                            string ss = sl.Value as string;
                            if (ss.Length > 0 && Pullenti.Morph.LanguageHelper.IsLatinChar(ss[0])) 
                            {
                                nl = ss;
                                break;
                            }
                        }
                    }
                    if (nl != null) 
                    {
                        string nal = this._findForSurname(ATTR_FIRSTNAME, nl, false);
                        if (nal == null) 
                            res.AppendFormat(" ({0})", nl);
                        else if (ShowLastnameOnFirstPosition) 
                            res.AppendFormat(" ({0} {1})", nl, nal);
                        else 
                            res.AppendFormat(" ({0} {1})", nal, nl);
                    }
                }
                return Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(res.ToString());
            }
            else if ((((n = this.GetStringValue(ATTR_FIRSTNAME)))) != null) 
            {
                string s = this._findForSurname(ATTR_MIDDLENAME, n, false);
                if (s != null) 
                    n = string.Format("{0} {1}", n, s);
                n = Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(n);
                string nik = this.GetStringValue(ATTR_NICKNAME);
                string tit = this._findShortestKingTitul(false);
                if (tit != null) 
                    n = string.Format("{0} {1}", tit, n);
                if (nik != null) 
                    n = string.Format("{0} {1}", n, nik);
                return n;
            }
            return "?";
        }
        internal Pullenti.Ner.Person.Internal.FioTemplateType m_PersonIdentityTyp;
        List<Pullenti.Ner.Person.Internal.PersonMorphCollection> m_SurnameOccurs = new List<Pullenti.Ner.Person.Internal.PersonMorphCollection>();
        List<Pullenti.Ner.Person.Internal.PersonMorphCollection> m_NameOccurs = new List<Pullenti.Ner.Person.Internal.PersonMorphCollection>();
        List<Pullenti.Ner.Person.Internal.PersonMorphCollection> m_SecOccurs = new List<Pullenti.Ner.Person.Internal.PersonMorphCollection>();
        List<Pullenti.Ner.Person.Internal.PersonMorphCollection> m_IdentOccurs = new List<Pullenti.Ner.Person.Internal.PersonMorphCollection>();
        internal void AddFioIdentity(Pullenti.Ner.Person.Internal.PersonMorphCollection lastName, Pullenti.Ner.Person.Internal.PersonMorphCollection firstName, object middleName)
        {
            if (lastName != null) 
            {
                if (lastName.Number > 0) 
                {
                    string num = Pullenti.Ner.Core.NumberHelper.GetNumberRoman(lastName.Number);
                    if (num == null) 
                        num = lastName.Number.ToString();
                    this.AddSlot(ATTR_NICKNAME, num, false, 0);
                }
                else 
                {
                    lastName.Correct();
                    m_SurnameOccurs.Add(lastName);
                    foreach (string v in lastName.Values) 
                    {
                        this.AddSlot(ATTR_LASTNAME, v, false, 0);
                    }
                }
            }
            if (firstName != null) 
            {
                firstName.Correct();
                if (firstName.Head != null && firstName.Head.Length > 2) 
                    m_NameOccurs.Add(firstName);
                foreach (string v in firstName.Values) 
                {
                    this.AddSlot(ATTR_FIRSTNAME, v, false, 0);
                }
                if (middleName is string) 
                    this.AddSlot(ATTR_MIDDLENAME, middleName, false, 0);
                else if (middleName is Pullenti.Ner.Person.Internal.PersonMorphCollection) 
                {
                    Pullenti.Ner.Person.Internal.PersonMorphCollection mm = middleName as Pullenti.Ner.Person.Internal.PersonMorphCollection;
                    if (mm.Head != null && mm.Head.Length > 2) 
                        m_SecOccurs.Add(mm);
                    foreach (string v in mm.Values) 
                    {
                        this.AddSlot(ATTR_MIDDLENAME, v, false, 0);
                    }
                }
            }
            this.CorrectData();
        }
        internal void AddIdentity(Pullenti.Ner.Person.Internal.PersonMorphCollection ident)
        {
            if (ident == null) 
                return;
            m_IdentOccurs.Add(ident);
            foreach (string v in ident.Values) 
            {
                this.AddSlot(ATTR_IDENTITY, v, false, 0);
            }
            this.CorrectData();
        }
        static bool IsInitial(string str)
        {
            if (str == null) 
                return false;
            if (str.Length == 1) 
                return true;
            if (str == "ДЖ") 
                return true;
            return false;
        }
        public void AddAttribute(object attr)
        {
            this.AddSlot(ATTR_ATTR, attr, false, 0);
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            PersonReferent p = obj as PersonReferent;
            if (p == null) 
                return false;
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_IDENTITY) 
                {
                    foreach (Pullenti.Ner.Slot aa in p.Slots) 
                    {
                        if (aa.TypeName == a.TypeName) 
                        {
                            if (_DelSurnameEnd(a.Value as string) == _DelSurnameEnd(aa.Value as string)) 
                                return true;
                        }
                    }
                }
            }
            string nick1 = this.GetStringValue(ATTR_NICKNAME);
            string nick2 = obj.GetStringValue(ATTR_NICKNAME);
            if (nick1 != null && nick2 != null) 
            {
                if (nick1 != nick2) 
                    return false;
            }
            if (this.FindSlot(ATTR_LASTNAME, null, true) != null && p.FindSlot(ATTR_LASTNAME, null, true) != null) 
            {
                if (!this.CompareSurnamesPers(p)) 
                    return false;
                if (this.FindSlot(ATTR_FIRSTNAME, null, true) != null && p.FindSlot(ATTR_FIRSTNAME, null, true) != null) 
                {
                    if (!this.CheckNames(ATTR_FIRSTNAME, p)) 
                        return false;
                    if (this.FindSlot(ATTR_MIDDLENAME, null, true) != null && p.FindSlot(ATTR_MIDDLENAME, null, true) != null) 
                    {
                        if (!this.CheckNames(ATTR_MIDDLENAME, p)) 
                            return false;
                    }
                    else if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                    {
                        if (this.FindSlot(ATTR_MIDDLENAME, null, true) != null || p.FindSlot(ATTR_MIDDLENAME, null, true) != null) 
                            return this.ToString() == p.ToString();
                        List<string> names1 = new List<string>();
                        List<string> names2 = new List<string>();
                        foreach (Pullenti.Ner.Slot s in Slots) 
                        {
                            if (s.TypeName == ATTR_FIRSTNAME) 
                            {
                                string nam = s.Value.ToString();
                                if (!IsInitial(nam)) 
                                    names1.Add(nam);
                            }
                        }
                        foreach (Pullenti.Ner.Slot s in p.Slots) 
                        {
                            if (s.TypeName == ATTR_FIRSTNAME) 
                            {
                                string nam = s.Value.ToString();
                                if (!IsInitial(nam)) 
                                {
                                    if (names1.Contains(nam)) 
                                        return true;
                                    names2.Add(nam);
                                }
                            }
                        }
                        if (names1.Count == 0 && names2.Count == 0) 
                            return true;
                        return false;
                    }
                }
                else if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts && ((this.FindSlot(ATTR_FIRSTNAME, null, true) != null || p.FindSlot(ATTR_FIRSTNAME, null, true) != null))) 
                    return false;
                return true;
            }
            string tit1 = this._findShortestKingTitul(false);
            string tit2 = p._findShortestKingTitul(false);
            if (((tit1 != null || tit2 != null)) || ((nick1 != null && nick1 == nick2))) 
            {
                if (tit1 == null || tit2 == null) 
                {
                    if (nick1 != null && nick1 == nick2) 
                    {
                    }
                    else 
                        return false;
                }
                else if (tit1 != tit2) 
                {
                    if (!tit1.Contains(tit2) && !tit2.Contains(tit1)) 
                        return false;
                }
                if (this.FindSlot(ATTR_FIRSTNAME, null, true) != null && p.FindSlot(ATTR_FIRSTNAME, null, true) != null) 
                {
                    if (!this.CheckNames(ATTR_FIRSTNAME, p)) 
                        return false;
                    return true;
                }
            }
            return false;
        }
        public override bool CanBeGeneralFor(Pullenti.Ner.Referent obj)
        {
            if (!this.CanBeEquals(obj, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                return false;
            PersonReferent p = obj as PersonReferent;
            if (p == null) 
                return false;
            if (this.FindSlot(ATTR_LASTNAME, null, true) == null || p.FindSlot(ATTR_LASTNAME, null, true) == null) 
                return false;
            if (!this.CompareSurnamesPers(p)) 
                return false;
            if (this.FindSlot(ATTR_FIRSTNAME, null, true) == null) 
            {
                if (p.FindSlot(ATTR_FIRSTNAME, null, true) != null) 
                    return true;
                else 
                    return false;
            }
            if (p.FindSlot(ATTR_FIRSTNAME, null, true) == null) 
                return false;
            if (!this.CheckNames(ATTR_FIRSTNAME, p)) 
                return false;
            if (this.FindSlot(ATTR_MIDDLENAME, null, true) != null && p.FindSlot(ATTR_MIDDLENAME, null, true) == null) 
            {
                if (!IsInitial(this.GetStringValue(ATTR_FIRSTNAME))) 
                    return false;
            }
            int nameInits = 0;
            int nameFulls = 0;
            int secInits = 0;
            int secFulls = 0;
            int nameInits1 = 0;
            int nameFulls1 = 0;
            int secInits1 = 0;
            int secFulls1 = 0;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_FIRSTNAME) 
                {
                    if (IsInitial(s.Value as string)) 
                        nameInits++;
                    else 
                        nameFulls++;
                }
                else if (s.TypeName == ATTR_MIDDLENAME) 
                {
                    if (IsInitial(s.Value as string)) 
                        secInits++;
                    else 
                        secFulls++;
                }
            }
            foreach (Pullenti.Ner.Slot s in p.Slots) 
            {
                if (s.TypeName == ATTR_FIRSTNAME) 
                {
                    if (IsInitial(s.Value as string)) 
                        nameInits1++;
                    else 
                        nameFulls1++;
                }
                else if (s.TypeName == ATTR_MIDDLENAME) 
                {
                    if (IsInitial(s.Value as string)) 
                        secInits1++;
                    else 
                        secFulls1++;
                }
            }
            if (secFulls > 0) 
                return false;
            if (nameInits == 0) 
            {
                if (nameInits1 > 0) 
                    return false;
            }
            else if (nameInits1 > 0) 
            {
                if ((secInits + secFulls) > 0) 
                    return false;
            }
            if (secInits == 0) 
            {
                if ((secInits1 + secFulls1) == 0) 
                {
                    if (nameInits1 == 0 && nameInits > 0) 
                        return true;
                    else 
                        return false;
                }
            }
            else if (secInits1 > 0) 
                return false;
            return true;
        }
        bool CompareSurnamesPers(PersonReferent p)
        {
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_LASTNAME) 
                {
                    string s = a.Value.ToString();
                    foreach (Pullenti.Ner.Slot aa in p.Slots) 
                    {
                        if (aa.TypeName == a.TypeName) 
                        {
                            string ss = aa.Value.ToString();
                            if (this.CompareSurnamesStrs(s, ss)) 
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        // Сравнение с учётом возможных окончаний
        bool CompareSurnamesStrs(string s1, string s2)
        {
            if (s1.StartsWith(s2) || s2.StartsWith(s1)) 
                return true;
            if (_DelSurnameEnd(s1) == _DelSurnameEnd(s2)) 
                return true;
            string n1 = Pullenti.Ner.Core.MiscHelper.GetAbsoluteNormalValue(s1, false);
            if (n1 != null) 
            {
                if (n1 == Pullenti.Ner.Core.MiscHelper.GetAbsoluteNormalValue(s2, false)) 
                    return true;
            }
            if (Pullenti.Ner.Core.MiscHelper.CanBeEquals(s1, s2, true, true, false)) 
                return true;
            return false;
        }
        internal static string _DelSurnameEnd(string s)
        {
            if (s.Length < 3) 
                return s;
            if (Pullenti.Morph.LanguageHelper.EndsWithEx(s, "А", "У", "Е", null)) 
                return s.Substring(0, s.Length - 1);
            if (Pullenti.Morph.LanguageHelper.EndsWith(s, "ОМ") || Pullenti.Morph.LanguageHelper.EndsWith(s, "ЫМ")) 
                return s.Substring(0, s.Length - 2);
            if (Pullenti.Morph.LanguageHelper.EndsWithEx(s, "Я", "Ю", null, null)) 
            {
                char ch1 = s[s.Length - 2];
                if (ch1 == 'Н' || ch1 == 'Л') 
                    return s.Substring(0, s.Length - 1) + "Ь";
            }
            return s;
        }
        bool CheckNames(string attrName, PersonReferent p)
        {
            List<string> names1 = new List<string>();
            List<string> inits1 = new List<string>();
            List<string> normn1 = new List<string>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == attrName) 
                {
                    string n = s.Value.ToString();
                    if (IsInitial(n)) 
                        inits1.Add(n);
                    else 
                    {
                        names1.Add(n);
                        string sn = Pullenti.Ner.Core.MiscHelper.GetAbsoluteNormalValue(n, false);
                        if (sn != null) 
                            normn1.Add(sn);
                    }
                }
            }
            List<string> names2 = new List<string>();
            List<string> inits2 = new List<string>();
            List<string> normn2 = new List<string>();
            foreach (Pullenti.Ner.Slot s in p.Slots) 
            {
                if (s.TypeName == attrName) 
                {
                    string n = s.Value.ToString();
                    if (IsInitial(n)) 
                        inits2.Add(n);
                    else 
                    {
                        names2.Add(n);
                        string sn = Pullenti.Ner.Core.MiscHelper.GetAbsoluteNormalValue(n, false);
                        if (sn != null) 
                            normn2.Add(sn);
                    }
                }
            }
            if (names1.Count > 0 && names2.Count > 0) 
            {
                foreach (string n in names1) 
                {
                    if (names2.Contains(n)) 
                        return true;
                }
                foreach (string n in normn1) 
                {
                    if (normn2.Contains(n)) 
                        return true;
                }
                return false;
            }
            if (inits1.Count > 0) 
            {
                foreach (string n in inits1) 
                {
                    if (inits2.Contains(n)) 
                        return true;
                    foreach (string nn in names2) 
                    {
                        if (nn.StartsWith(n)) 
                            return true;
                    }
                }
            }
            if (inits2.Count > 0) 
            {
                foreach (string n in inits2) 
                {
                    if (inits1.Contains(n)) 
                        return true;
                    foreach (string nn in names1) 
                    {
                        if (nn.StartsWith(n)) 
                            return true;
                    }
                }
            }
            return false;
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            base.MergeSlots(obj, mergeStatistic);
            PersonReferent p = obj as PersonReferent;
            m_SurnameOccurs.AddRange(p.m_SurnameOccurs);
            m_NameOccurs.AddRange(p.m_NameOccurs);
            m_SecOccurs.AddRange(p.m_SecOccurs);
            m_IdentOccurs.AddRange(p.m_IdentOccurs);
            if (p.m_PersonIdentityTyp != Pullenti.Ner.Person.Internal.FioTemplateType.Undefined) 
                m_PersonIdentityTyp = p.m_PersonIdentityTyp;
            this.CorrectData();
        }
        internal void CorrectData()
        {
            Pullenti.Morph.MorphGender g = Pullenti.Morph.MorphGender.Undefined;
            while (true) 
            {
                bool ch = false;
                if (Pullenti.Ner.Person.Internal.PersonMorphCollection.Intersect(m_SurnameOccurs)) 
                    ch = true;
                if (Pullenti.Ner.Person.Internal.PersonMorphCollection.Intersect(m_NameOccurs)) 
                    ch = true;
                if (Pullenti.Ner.Person.Internal.PersonMorphCollection.Intersect(m_SecOccurs)) 
                    ch = true;
                if (Pullenti.Ner.Person.Internal.PersonMorphCollection.Intersect(m_IdentOccurs)) 
                    ch = true;
                if (!ch) 
                    break;
                if (g == Pullenti.Morph.MorphGender.Undefined && m_SurnameOccurs.Count > 0 && m_SurnameOccurs[0].Gender != Pullenti.Morph.MorphGender.Undefined) 
                    g = m_SurnameOccurs[0].Gender;
                if (g == Pullenti.Morph.MorphGender.Undefined && m_NameOccurs.Count > 0 && m_NameOccurs[0].Gender != Pullenti.Morph.MorphGender.Undefined) 
                    g = m_NameOccurs[0].Gender;
                if (g == Pullenti.Morph.MorphGender.Undefined && m_IdentOccurs.Count > 0 && m_IdentOccurs[0].Gender != Pullenti.Morph.MorphGender.Undefined) 
                    g = m_IdentOccurs[0].Gender;
                if (g != Pullenti.Morph.MorphGender.Undefined) 
                {
                    Pullenti.Ner.Person.Internal.PersonMorphCollection.SetGender(m_SurnameOccurs, g);
                    Pullenti.Ner.Person.Internal.PersonMorphCollection.SetGender(m_NameOccurs, g);
                    Pullenti.Ner.Person.Internal.PersonMorphCollection.SetGender(m_SecOccurs, g);
                    Pullenti.Ner.Person.Internal.PersonMorphCollection.SetGender(m_IdentOccurs, g);
                }
            }
            if (g != Pullenti.Morph.MorphGender.Undefined) 
            {
                if (!IsFemale && !IsMale) 
                {
                    if (g == Pullenti.Morph.MorphGender.Masculine) 
                        IsMale = true;
                    else 
                        IsFemale = true;
                }
            }
            this.CorrectSurnames();
            this.CorrectIdentifiers();
            this.CorrectAttrs();
            this.RemoveSlots(ATTR_LASTNAME, m_SurnameOccurs);
            this.RemoveSlots(ATTR_FIRSTNAME, m_NameOccurs);
            this.RemoveSlots(ATTR_MIDDLENAME, m_SecOccurs);
            this.RemoveSlots(ATTR_IDENTITY, m_IdentOccurs);
            this.RemoveInitials(ATTR_FIRSTNAME);
            this.RemoveInitials(ATTR_MIDDLENAME);
        }
        void CorrectSurnames()
        {
            if (!IsMale && !IsFemale) 
                return;
            for (int i = 0; i < Slots.Count; i++) 
            {
                if (Slots[i].TypeName == ATTR_LASTNAME) 
                {
                    string s = Slots[i].Value.ToString();
                    for (int j = i + 1; j < Slots.Count; j++) 
                    {
                        if (Slots[j].TypeName == ATTR_LASTNAME) 
                        {
                            string s1 = Slots[j].Value.ToString();
                            if (s != s1 && _DelSurnameEnd(s) == _DelSurnameEnd(s1) && s1.Length != s.Length) 
                            {
                                if (IsMale) 
                                {
                                    this.UploadSlot(Slots[i], (s = _DelSurnameEnd(s)));
                                    Slots.RemoveAt(j);
                                    j--;
                                }
                                else 
                                {
                                    Slots.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        void CorrectIdentifiers()
        {
            if (IsFemale) 
                return;
            for (int i = 0; i < Slots.Count; i++) 
            {
                if (Slots[i].TypeName == ATTR_IDENTITY) 
                {
                    string s = Slots[i].Value.ToString();
                    for (int j = i + 1; j < Slots.Count; j++) 
                    {
                        if (Slots[j].TypeName == ATTR_IDENTITY) 
                        {
                            string s1 = Slots[j].Value.ToString();
                            if (s != s1 && _DelSurnameEnd(s) == _DelSurnameEnd(s1)) 
                            {
                                this.UploadSlot(Slots[i], (s = _DelSurnameEnd(s)));
                                Slots.RemoveAt(j);
                                j--;
                                IsMale = true;
                            }
                        }
                    }
                }
            }
        }
        void RemoveSlots(string attrName, List<Pullenti.Ner.Person.Internal.PersonMorphCollection> cols)
        {
            List<string> vars = new List<string>();
            foreach (Pullenti.Ner.Person.Internal.PersonMorphCollection col in cols) 
            {
                foreach (string v in col.Values) 
                {
                    if (!vars.Contains(v)) 
                        vars.Add(v);
                }
            }
            if (vars.Count < 1) 
                return;
            for (int i = Slots.Count - 1; i >= 0; i--) 
            {
                if (Slots[i].TypeName == attrName) 
                {
                    string v = Slots[i].Value.ToString();
                    if (!vars.Contains(v)) 
                    {
                        for (int j = 0; j < Slots.Count; j++) 
                        {
                            if (j != i && Slots[j].TypeName == Slots[i].TypeName) 
                            {
                                if (attrName == ATTR_LASTNAME) 
                                {
                                    bool ee = false;
                                    foreach (string vv in vars) 
                                    {
                                        if (this.CompareSurnamesStrs(v, vv)) 
                                            ee = true;
                                    }
                                    if (!ee) 
                                        continue;
                                }
                                Slots.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
        }
        void RemoveInitials(string attrName)
        {
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == attrName) 
                {
                    if (IsInitial(s.Value.ToString())) 
                    {
                        foreach (Pullenti.Ner.Slot ss in Slots) 
                        {
                            if (ss.TypeName == s.TypeName && s != ss) 
                            {
                                string v = ss.Value.ToString();
                                if (!IsInitial(v) && v.StartsWith(s.Value.ToString())) 
                                {
                                    if (attrName == ATTR_FIRSTNAME && v.Length == 2 && this.FindSlot(ATTR_MIDDLENAME, v.Substring(1), true) != null) 
                                        Slots.Remove(ss);
                                    else 
                                        Slots.Remove(s);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        void CorrectAttrs()
        {
            List<PersonPropertyReferent> attrs = new List<PersonPropertyReferent>();
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_ATTR && (s.Value is PersonPropertyReferent)) 
                    attrs.Add(s.Value as PersonPropertyReferent);
            }
            if (attrs.Count < 2) 
                return;
            foreach (PersonPropertyReferent a in attrs) 
            {
                a.Tag = null;
            }
            for (int i = 0; i < (attrs.Count - 1); i++) 
            {
                for (int j = i + 1; j < attrs.Count; j++) 
                {
                    if (attrs[i].GeneralReferent == attrs[j] || attrs[j].CanBeGeneralFor(attrs[i])) 
                        attrs[j].Tag = attrs[i];
                    else if (attrs[j].GeneralReferent == attrs[i] || attrs[i].CanBeGeneralFor(attrs[j])) 
                        attrs[i].Tag = attrs[j];
                }
            }
            for (int i = Slots.Count - 1; i >= 0; i--) 
            {
                if (Slots[i].TypeName == ATTR_ATTR && (Slots[i].Value is PersonPropertyReferent)) 
                {
                    if ((Slots[i].Value as PersonPropertyReferent).Tag != null) 
                    {
                        PersonPropertyReferent pr = (Slots[i].Value as PersonPropertyReferent).Tag as PersonPropertyReferent;
                        if (pr != null && pr.GeneralReferent == null) 
                            pr.GeneralReferent = Slots[i].Value as PersonPropertyReferent;
                        Slots.RemoveAt(i);
                    }
                }
            }
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            string tit = this._findShortestKingTitul(false);
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_IDENTITY) 
                    oi.Termins.Add(new Pullenti.Ner.Core.Termin(a.Value.ToString()) { IgnoreTermsOrder = true });
                else if (a.TypeName == ATTR_LASTNAME) 
                {
                    Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin(a.Value.ToString());
                    if (t.Terms.Count > 20) 
                    {
                    }
                    if (IsMale) 
                        t.Gender = Pullenti.Morph.MorphGender.Masculine;
                    else if (IsFemale) 
                        t.Gender = Pullenti.Morph.MorphGender.Feminie;
                    oi.Termins.Add(t);
                }
                else if (a.TypeName == ATTR_FIRSTNAME && tit != null) 
                {
                    Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin(string.Format("{0} {1}", tit, a.Value.ToString()));
                    if (IsMale) 
                        t.Gender = Pullenti.Morph.MorphGender.Masculine;
                    else if (IsFemale) 
                        t.Gender = Pullenti.Morph.MorphGender.Feminie;
                    oi.Termins.Add(t);
                }
            }
            return oi;
        }
    }
}