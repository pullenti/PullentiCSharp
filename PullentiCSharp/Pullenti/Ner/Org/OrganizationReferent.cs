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

namespace Pullenti.Ner.Org
{
    /// <summary>
    /// Сущность - организация
    /// </summary>
    public class OrganizationReferent : Pullenti.Ner.Referent
    {
        public OrganizationReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Org.Internal.MetaOrganization.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("ORGANIZATION")
        /// </summary>
        public const string OBJ_TYPENAME = "ORGANIZATION";
        /// <summary>
        /// Имя атрибута - наименование
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Имя атрибута - тип
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - номер
        /// </summary>
        public const string ATTR_NUMBER = "NUMBER";
        /// <summary>
        /// Имя атрибута - эпоним (имени кого)
        /// </summary>
        public const string ATTR_EPONYM = "EPONYM";
        /// <summary>
        /// Имя атрибута - вышестоящая организация (OrganizationReferent)
        /// </summary>
        public const string ATTR_HIGHER = "HIGHER";
        /// <summary>
        /// Имя атрибута - владелец (PersonReferent)
        /// </summary>
        public const string ATTR_OWNER = "OWNER";
        /// <summary>
        /// Имя атрибута - географический объект (GeoReferent)
        /// </summary>
        public const string ATTR_GEO = "GEO";
        /// <summary>
        /// Имя атрибута - разное
        /// </summary>
        public const string ATTR_MISC = "MISC";
        /// <summary>
        /// Имя атрибута - профиль (OrgProfile)
        /// </summary>
        public const string ATTR_PROFILE = "PROFILE";
        /// <summary>
        /// Имя атрибута - маркер
        /// </summary>
        public const string ATTR_MARKER = "MARKER";
        /// <summary>
        /// При выводе в ToString() первым ставить номер, если есть
        /// </summary>
        public static bool ShowNumberOnFirstPosition = false;
        public override string ToString(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            bool isDep = Kind == OrganizationKind.Department;
            string name = null;
            string altname = null;
            int namesCount = 0;
            int len = 0;
            bool noType = false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_NAME) 
                {
                    string n = s.Value.ToString();
                    namesCount++;
                    len += n.Length;
                }
            }
            if (namesCount > 0) 
            {
                len /= namesCount;
                if (len > 10) 
                    len -= ((len / 7));
                int cou = 0;
                int altcou = 0;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                    {
                        string n = s.Value.ToString();
                        if (n.Length >= len) 
                        {
                            if (s.Count > cou) 
                            {
                                name = n;
                                cou = s.Count;
                            }
                            else if (s.Count == cou) 
                            {
                                if (name == null) 
                                    name = n;
                                else if (name.Length < n.Length) 
                                    name = n;
                            }
                        }
                        else if (s.Count > altcou) 
                        {
                            altname = n;
                            altcou = s.Count;
                        }
                        else if (s.Count == altcou) 
                        {
                            if (altname == null) 
                                altname = n;
                            else if (altname.Length > n.Length) 
                                altname = n;
                        }
                    }
                }
            }
            if (name != null) 
            {
                if (altname != null) 
                {
                    if (name.Replace(" ", "").Contains(altname)) 
                        altname = null;
                }
                if (altname != null && ((altname.Length > 30 || altname.Length > (name.Length / 2)))) 
                    altname = null;
                if (altname == null) 
                {
                    foreach (Pullenti.Ner.Slot s in Slots) 
                    {
                        if (s.TypeName == ATTR_NAME) 
                        {
                            if (Pullenti.Ner.Core.MiscHelper.CanBeEqualCyrAndLatSS(name, (string)s.Value)) 
                            {
                                altname = (string)s.Value;
                                break;
                            }
                        }
                    }
                }
            }
            else 
            {
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_TYPE) 
                    {
                        string nam = s.Value as string;
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken._getKind(nam, null, this) == OrganizationKind.Undefined) 
                            continue;
                        if (name == null || nam.Length > name.Length) 
                            name = nam;
                        noType = true;
                    }
                }
                if (name == null) 
                {
                    foreach (Pullenti.Ner.Slot s in Slots) 
                    {
                        if (s.TypeName == ATTR_TYPE) 
                        {
                            string nam = s.Value as string;
                            if (name == null || nam.Length > name.Length) 
                                name = nam;
                            noType = true;
                        }
                    }
                }
            }
            bool outOwnInName = false;
            if (name != null) 
            {
                res.Append(Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(name));
                if (((!isDep && namesCount == 0 && Higher != null) && Higher.Higher == null && Number == null) && Eponyms.Count == 0) 
                    outOwnInName = true;
            }
            if (Number != null) 
            {
                if (ShowNumberOnFirstPosition) 
                    res.Insert(0, string.Format("{0} ", Number));
                else 
                    res.AppendFormat(" №{0}", Number);
            }
            List<string> fams = null;
            foreach (Pullenti.Ner.Slot r in Slots) 
            {
                if (r.TypeName == ATTR_EPONYM && r.Value != null) 
                {
                    if (fams == null) 
                        fams = new List<string>();
                    fams.Add(r.Value.ToString());
                }
            }
            if (fams != null) 
            {
                fams.Sort();
                res.Append(" имени ");
                for (int i = 0; i < fams.Count; i++) 
                {
                    if (i > 0 && ((i + 1) < fams.Count)) 
                        res.Append(", ");
                    else if (i > 0) 
                        res.Append(" и ");
                    res.Append(fams[i]);
                }
            }
            if (altname != null && !isDep) 
                res.AppendFormat(" ({0})", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(altname));
            if (!shortVariant && Owner != null) 
                res.AppendFormat("; {0}", Owner.ToString(true, lang, lev + 1));
            if (!shortVariant) 
            {
                if (!noType && !isDep) 
                {
                    string typ = null;
                    foreach (string t in Types) 
                    {
                        if (Pullenti.Ner.Org.Internal.OrgItemTypeToken._getKind(t, null, this) == OrganizationKind.Undefined) 
                            continue;
                        if (typ == null || typ.Length > t.Length) 
                            typ = t;
                    }
                    if (typ == null) 
                    {
                        foreach (string t in Types) 
                        {
                            if (typ == null || typ.Length > t.Length) 
                                typ = t;
                        }
                    }
                    if (name != null && !string.IsNullOrEmpty(typ) && !char.IsUpper(typ[0])) 
                    {
                        if (name.ToUpper().Contains(typ.ToUpper())) 
                            typ = null;
                    }
                    if (typ != null) 
                        res.AppendFormat(", {0}", typ);
                }
                foreach (Pullenti.Ner.Slot ss in Slots) 
                {
                    if (ss.TypeName == ATTR_GEO && ss.Value != null) 
                        res.AppendFormat(", {0}", ss.Value.ToString());
                }
            }
            if (!shortVariant) 
            {
                if (isDep || outOwnInName) 
                {
                    foreach (Pullenti.Ner.Slot ss in Slots) 
                    {
                        if (ss.TypeName == ATTR_HIGHER && (ss.Value is Pullenti.Ner.Referent) && (lev < 20)) 
                        {
                            OrganizationReferent hi = ss.Value as OrganizationReferent;
                            if (hi != null) 
                            {
                                List<Pullenti.Ner.Referent> tmp = new List<Pullenti.Ner.Referent>();
                                tmp.Add(this);
                                for (; hi != null; hi = hi.Higher) 
                                {
                                    if (tmp.Contains(hi)) 
                                        break;
                                    else 
                                        tmp.Add(hi);
                                }
                                if (hi != null) 
                                    continue;
                            }
                            res.Append(';');
                            res.AppendFormat(" {0}", (ss.Value as Pullenti.Ner.Referent).ToString(shortVariant, lang, lev + 1));
                            break;
                        }
                    }
                }
            }
            if (res.Length == 0) 
            {
                if (INN != null) 
                    res.AppendFormat("ИНН: {0}", INN);
                if (OGRN != null) 
                    res.AppendFormat(" ОГРН: {0}", INN);
            }
            return res.ToString();
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
                if (s.TypeName == ATTR_NAME || s.TypeName == ATTR_EPONYM) 
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
                else if (s.TypeName == ATTR_NUMBER) 
                    res.Add(string.Format("{0} {1}", Kind, s.Value.ToString()));
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
            if (INN != null) 
                res.Add("ИНН:" + INN);
            if (OGRN != null) 
                res.Add("ОГРН:" + OGRN);
            if (res.Count > 0) 
                return res;
            else 
                return base.GetCompareStrings();
        }
        internal bool CheckCorrection()
        {
            if (Slots.Count < 1) 
                return false;
            string s = this.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0).ToLower();
            if (s.Contains("прокуратура") || s.Contains("штаб") || s.Contains("кабинет")) 
                return true;
            if (Slots.Count == 1) 
            {
                if (Slots[0].TypeName != ATTR_NAME) 
                {
                    if (Kind == OrganizationKind.Govenment || Kind == OrganizationKind.Justice) 
                        return true;
                    return false;
                }
            }
            if (this.FindSlot(ATTR_TYPE, null, true) == null && this.FindSlot(ATTR_NAME, null, true) == null) 
                return false;
            if (s == "государственная гражданская служба" || s == "здравоохранения") 
                return false;
            if (Types.Contains("колония")) 
            {
                if (Number == null) 
                    return false;
            }
            if (s.Contains("конгресс")) 
            {
                if (this.FindSlot(ATTR_GEO, null, true) == null) 
                    return false;
            }
            List<string> nams = Names;
            if (nams.Count == 1 && nams[0].Length == 1 && (Types.Count < 3)) 
                return false;
            if (nams.Contains("ВА")) 
            {
                if (Kind == OrganizationKind.Bank) 
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Номер ИНН
        /// </summary>
        public string INN
        {
            get
            {
                return this._getMiscValue("ИНН:");
            }
            set
            {
                if (value != null) 
                    this.AddSlot(ATTR_MISC, "ИНН:" + value, false, 0);
            }
        }
        /// <summary>
        /// Номер ОГРН
        /// </summary>
        public string OGRN
        {
            get
            {
                return this._getMiscValue("ОГРН");
            }
            set
            {
                if (value != null) 
                    this.AddSlot(ATTR_MISC, "ОГРН:" + value, false, 0);
            }
        }
        string _getMiscValue(string pref)
        {
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_MISC) 
                {
                    if (s.Value is Pullenti.Ner.Referent) 
                    {
                        Pullenti.Ner.Referent r = s.Value as Pullenti.Ner.Referent;
                        if (r.TypeName == "URI") 
                        {
                            string val = r.GetStringValue("SCHEME");
                            if (val == pref) 
                                return r.GetStringValue("VALUE");
                        }
                    }
                    else if (s.Value is string) 
                    {
                        string str = s.Value as string;
                        if (str.StartsWith(pref) && str.Length > (pref.Length + 1)) 
                            return str.Substring(pref.Length + 1);
                    }
                }
            }
            return null;
        }
        static List<string> m_EmptyNames = new List<string>();
        /// <summary>
        /// Список имён организации
        /// </summary>
        public List<string> Names
        {
            get
            {
                List<string> res = null;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NAME) 
                    {
                        if (res == null) 
                            res = new List<string>();
                        res.Add(s.Value.ToString());
                    }
                }
                return res ?? m_EmptyNames;
            }
        }
        string CorrectName(string name, out int num)
        {
            num = 0;
            if (name == null || (name.Length < 1)) 
                return null;
            if (char.IsDigit(name[0]) && name.IndexOf(' ') > 0) 
            {
                int i;
                if (int.TryParse(name.Substring(0, name.IndexOf(' ')), out i)) 
                {
                    if (i > 1) 
                    {
                        num = i;
                        name = name.Substring(name.IndexOf(' ')).Trim();
                    }
                }
            }
            else if (char.IsDigit(name[name.Length - 1])) 
            {
                int i;
                for (i = name.Length - 1; i >= 0; i--) 
                {
                    if (!char.IsDigit(name[i])) 
                        break;
                }
                if (i >= 0 && name[i] == '.') 
                {
                }
                else if (i > 0 && int.TryParse(name.Substring(i + 1), out num) && num > 0) 
                {
                    if (i < 1) 
                        return null;
                    name = name.Substring(0, i).Trim();
                    if (name.Length > 0 && name[name.Length - 1] == '-') 
                        name = name.Substring(0, name.Length - 1).Trim();
                }
            }
            return this.CorrectName0(name);
        }
        string CorrectName0(string name)
        {
            name = name.ToUpper();
            if (name.Length > 2 && !char.IsLetterOrDigit(name[name.Length - 1]) && char.IsWhiteSpace(name[name.Length - 2])) 
                name = name.Substring(0, name.Length - 2) + name.Substring(name.Length - 1);
            if (name.Contains(" НА СТ.")) 
                name = name.Replace(" НА СТ.", " НА СТАНЦИИ");
            return this.CorrectType(name);
        }
        string CorrectType(string name)
        {
            if (name == null) 
                return null;
            if (name.EndsWith(" полок")) 
                name = name.Substring(0, name.Length - 5) + "полк";
            else if (name == "полок") 
                name = "полк";
            StringBuilder tmp = new StringBuilder();
            bool notEmpty = false;
            for (int i = 0; i < name.Length; i++) 
            {
                char ch = name[i];
                if (char.IsLetterOrDigit(ch)) 
                    notEmpty = true;
                else if (ch != '&' && ch != ',' && ch != '.') 
                    ch = ' ';
                if (char.IsWhiteSpace(ch)) 
                {
                    if (tmp.Length == 0) 
                        continue;
                    if (tmp[tmp.Length - 1] != ' ' && tmp[tmp.Length - 1] != '.') 
                        tmp.Append(' ');
                    continue;
                }
                bool isSpBefore = tmp.Length == 0 || tmp[tmp.Length - 1] == ' ';
                if (ch == '&' && !isSpBefore) 
                    tmp.Append(' ');
                if (((ch == ',' || ch == '.')) && isSpBefore && tmp.Length > 0) 
                    tmp.Length--;
                tmp.Append(ch);
            }
            if (!notEmpty) 
                return null;
            while (tmp.Length > 0) 
            {
                char ch = tmp[tmp.Length - 1];
                if ((ch == ' ' || ch == ',' || ch == '.') || char.IsWhiteSpace(ch)) 
                    tmp.Length--;
                else 
                    break;
            }
            return tmp.ToString();
        }
        public void AddName(string name, bool removeLongGovNames = true, Pullenti.Ner.Token t = null)
        {
            int num;
            string s = this.CorrectName(name, out num);
            if (s == null) 
            {
                if (num > 0 && Number == null) 
                    Number = num.ToString();
                return;
            }
            if (s == "УПРАВЛЕНИЕ") 
            {
            }
            int i = s.IndexOf(' ');
            if (i == 2 && s[1] == 'К' && ((i + 3) < s.Length)) 
            {
                this.AddSlot(ATTR_TYPE, s.Substring(0, 2), false, 0);
                s = s.Substring(3).Trim();
            }
            if (Kind == OrganizationKind.Bank || s.Contains("БАНК")) 
            {
                if (s.StartsWith("КБ ")) 
                {
                    this.AddTypeStr("коммерческий банк");
                    s = s.Substring(3);
                }
                else if (s.StartsWith("АКБ ")) 
                {
                    this.AddTypeStr("акционерный коммерческий банк");
                    s = s.Substring(3);
                }
            }
            if (num > 0) 
            {
                if (s.Length > 10) 
                    Number = num.ToString();
                else 
                    s = string.Format("{0}{1}", s, num);
            }
            int cou = 1;
            if (t != null && !t.Chars.IsLetter && Pullenti.Ner.Core.BracketHelper.IsBracket(t, false)) 
                t = t.Next;
            if (((t is Pullenti.Ner.TextToken) && (s.IndexOf(' ') < 0) && s.Length > 3) && s == (t as Pullenti.Ner.TextToken).Term) 
            {
                List<Pullenti.Morph.MorphToken> mt = Pullenti.Morph.MorphologyService.Process(s, t.Morph.Language, null);
                if (mt != null && mt.Count == 1) 
                {
                    string sNorm = mt[0].GetLemma();
                    if (sNorm == s) 
                    {
                        if (m_NameSingleNormalReal == null) 
                        {
                            m_NameSingleNormalReal = s;
                            for (int ii = Slots.Count - 1; ii >= 0; ii--) 
                            {
                                if (Slots[ii].TypeName == ATTR_NAME && (Slots[ii].Value as string) != s) 
                                {
                                    mt = Pullenti.Morph.MorphologyService.Process(Slots[ii].Value as string, t.Morph.Language, null);
                                    if (mt != null && mt.Count == 1) 
                                    {
                                        if (mt[0].GetLemma() == m_NameSingleNormalReal) 
                                        {
                                            cou += Slots[ii].Count;
                                            Slots.RemoveAt(ii);
                                            m_NameVars = null;
                                            m_NameHashs = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (sNorm == m_NameSingleNormalReal && sNorm != null) 
                        s = sNorm;
                }
            }
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_NAME) 
                {
                    string n = a.Value.ToString();
                    if (s == n) 
                    {
                        a.Count += cou;
                        return;
                    }
                }
                else if (a.TypeName == ATTR_TYPE) 
                {
                    string n = a.Value.ToString();
                    if (string.Compare(s, n, true) == 0) 
                        return;
                    if (s.StartsWith(n + " ")) 
                        s = s.Substring(n.Length + 1);
                }
            }
            this.AddSlot(ATTR_NAME, s, false, 1);
            if (Pullenti.Morph.LanguageHelper.EndsWith(s, " ПО")) 
            {
                s = s.Substring(0, s.Length - 2) + "ПРОГРАММНОГО ОБЕСПЕЧЕНИЯ";
                this.AddSlot(ATTR_NAME, s, false, 0);
            }
            this.CorrectData(removeLongGovNames);
        }
        public void AddNameStr(string name, Pullenti.Ner.Org.Internal.OrgItemTypeToken typ, int cou = 1)
        {
            if (typ != null && typ.AltTyp != null && !typ.IsNotTyp) 
                this.AddTypeStr(typ.AltTyp);
            if (name == null) 
            {
                if (typ.IsNotTyp) 
                    return;
                if (typ.Name != null && string.Compare(typ.Name, typ.Typ, true) != 0 && ((typ.Name.Length > typ.Typ.Length || this.FindSlot(ATTR_NAME, null, true) == null))) 
                {
                    int num = 0;
                    string s = this.CorrectName(typ.Name, out num);
                    this.AddSlot(ATTR_NAME, s, false, cou);
                    if (num > 0 && typ.IsDep && Number == null) 
                        Number = num.ToString();
                }
                else if (typ.AltTyp != null) 
                    this.AddSlot(ATTR_NAME, this.CorrectName0(typ.AltTyp), false, cou);
            }
            else 
            {
                string s = this.CorrectName0(name);
                if (typ == null || typ.IsNotTyp) 
                    this.AddSlot(ATTR_NAME, s, false, cou);
                else 
                {
                    this.AddSlot(ATTR_NAME, string.Format("{0} {1}", typ.Typ.ToUpper(), s), false, cou);
                    if (typ.Name != null) 
                    {
                        int num = 0;
                        string ss = this.CorrectName(typ.Name, out num);
                        if (ss != null) 
                        {
                            this.AddTypeStr(ss);
                            this.AddSlot(ATTR_NAME, string.Format("{0} {1}", ss, s), false, cou);
                            if (num > 0 && typ.IsDep && Number == null) 
                                Number = num.ToString();
                        }
                    }
                }
                if (Pullenti.Morph.LanguageHelper.EndsWithEx(name, " ОБЛАСТИ", " РАЙОНА", " КРАЯ", " РЕСПУБЛИКИ")) 
                {
                    int ii = name.LastIndexOf(' ');
                    this.AddNameStr(name.Substring(0, ii), typ, cou);
                }
            }
            this.CorrectData(true);
        }
        /// <summary>
        /// Профиль деятельности (список OrgProfile)
        /// </summary>
        public List<OrgProfile> Profiles
        {
            get
            {
                List<OrgProfile> res = new List<OrgProfile>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_PROFILE) 
                    {
                        try 
                        {
                            string str = s.Value as string;
                            if (str == "Politics") 
                                str = "Policy";
                            else if (str == "PartOf") 
                                str = "Unit";
                            OrgProfile v = (OrgProfile)Enum.Parse(typeof(OrgProfile), str, true);
                            res.Add(v);
                        }
                        catch(Exception ex3582) 
                        {
                        }
                    }
                }
                return res;
            }
        }
        public void AddProfile(OrgProfile prof)
        {
            if (prof != OrgProfile.Undefined) 
                this.AddSlot(ATTR_PROFILE, prof.ToString(), false, 0);
        }
        public bool ContainsProfile(OrgProfile prof)
        {
            return this.FindSlot(ATTR_PROFILE, prof.ToString(), true) != null;
        }
        /// <summary>
        /// Список типов и префиксов организации (ЗАО, компания, институт ...)
        /// </summary>
        public List<string> Types
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_TYPE) 
                        res.Add(s.Value.ToString());
                }
                return res;
            }
        }
        internal bool _typesContains(string substr)
        {
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                if (s.TypeName == ATTR_TYPE) 
                {
                    string val = s.Value as string;
                    if (val != null && val.Contains(substr)) 
                        return true;
                }
            }
            return false;
        }
        public void AddType(Pullenti.Ner.Org.Internal.OrgItemTypeToken typ, bool finalAdd = false)
        {
            if (typ == null) 
                return;
            foreach (OrgProfile p in typ.Profiles) 
            {
                this.AddProfile(p);
            }
            if (typ.IsNotTyp) 
                return;
            for (Pullenti.Ner.Token tt = typ.BeginToken; tt != null && tt.EndChar <= typ.EndChar; tt = tt.Next) 
            {
                Pullenti.Ner.Core.TerminToken tok = Pullenti.Ner.Org.Internal.OrgItemTypeToken.m_Markers.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                    this.AddSlot(ATTR_MARKER, tok.Termin.CanonicText, false, 0);
            }
            if (typ.Typ == "следственный комитет") 
            {
                this.AddTypeStr("комитет");
                this.AddName(typ.Typ, true, null);
            }
            else 
            {
                this.AddTypeStr(typ.Typ);
                if (typ.Number != null) 
                    Number = typ.Number;
                if (typ.Typ == "АКБ") 
                    this.AddTypeStr("банк");
                if (typ.Name != null && typ.Name != "ПОЛОК") 
                {
                    if (typ.NameIsName) 
                        this.AddName(typ.Name, true, null);
                    else if (typ.Typ == "министерство" && typ.Name.StartsWith(typ.Typ + " ", StringComparison.OrdinalIgnoreCase)) 
                        this.AddName(typ.Name, true, null);
                    else if (typ.Typ.EndsWith("электростанция") && typ.Name.EndsWith(" " + typ.Typ, StringComparison.OrdinalIgnoreCase)) 
                        this.AddName(typ.Name, true, null);
                    else if (this.FindSlot(ATTR_NAME, null, true) != null && this.FindSlot(ATTR_NAME, typ.Name, true) == null) 
                        this.AddTypeStr(typ.Name.ToLower());
                    else if (finalAdd) 
                    {
                        string ss = typ.Name.ToLower();
                        if (Pullenti.Morph.LanguageHelper.IsLatin(ss) && ss.EndsWith(" " + typ.Typ)) 
                        {
                            if (typ.Root != null && ((typ.Root.CanHasLatinName || typ.Root.CanHasSingleName)) && !typ.Root.MustBePartofName) 
                            {
                                Pullenti.Ner.Slot sl = this.FindSlot(ATTR_NAME, typ.Name, true);
                                if (sl != null) 
                                    Slots.Remove(sl);
                                this.AddName(ss.Substring(0, ss.Length - typ.Typ.Length - 1).ToUpper(), true, null);
                                this.AddName(ss.ToUpper(), true, null);
                                ss = null;
                            }
                        }
                        if (ss != null) 
                            this.AddTypeStr(ss);
                    }
                    if (typ.AltName != null) 
                        this.AddName(typ.AltName, true, null);
                }
            }
            if (typ.AltTyp != null) 
                this.AddTypeStr(typ.AltTyp);
            if (typ.Number != null) 
                Number = typ.Number;
            if (typ.Root != null) 
            {
                if (typ.Root.Acronym != null) 
                {
                    if (this.FindSlot(ATTR_TYPE, typ.Root.Acronym, true) == null) 
                        this.AddSlot(ATTR_TYPE, typ.Root.Acronym, false, 0);
                }
                if (typ.Root.CanonicText != null && typ.Root.CanonicText != "СБЕРЕГАТЕЛЬНЫЙ БАНК" && typ.Root.CanonicText != typ.Root.Acronym) 
                    this.AddTypeStr(typ.Root.CanonicText.ToLower());
            }
            if (typ.Geo != null) 
            {
                if ((typ.Geo.Referent is Pullenti.Ner.Geo.GeoReferent) && (typ.Geo.Referent as Pullenti.Ner.Geo.GeoReferent).IsRegion && Kind == OrganizationKind.Study) 
                {
                }
                else 
                    this.AddGeoObject(typ.Geo);
            }
            if (typ.Geo2 != null) 
                this.AddGeoObject(typ.Geo2);
            if (finalAdd) 
            {
                if (Kind == OrganizationKind.Bank) 
                    this.AddSlot(ATTR_TYPE, "банк", false, 0);
            }
        }
        public void AddTypeStr(string typ)
        {
            if (typ == null) 
                return;
            typ = this.CorrectType(typ);
            if (typ == null) 
                return;
            bool ok = true;
            foreach (string n in Names) 
            {
                if (n.StartsWith(typ, StringComparison.OrdinalIgnoreCase)) 
                {
                    ok = false;
                    break;
                }
            }
            if (!ok) 
                return;
            this.AddSlot(ATTR_TYPE, typ, false, 0);
            this.CorrectData(true);
        }
        ICollection<string> GetSortedTypes(bool forOntos)
        {
            List<string> res = new List<string>(Types);
            res.Sort();
            for (int i = 0; i < res.Count; i++) 
            {
                if (char.IsLower(res[i][0])) 
                {
                    bool into = false;
                    foreach (string r in res) 
                    {
                        if (r != res[i] && r.Contains(res[i])) 
                        {
                            into = true;
                            break;
                        }
                    }
                    if (!into && !forOntos) 
                    {
                        string v = res[i].ToUpper();
                        foreach (string n in Names) 
                        {
                            if (n.Contains(v)) 
                            {
                                into = true;
                                break;
                            }
                        }
                    }
                    if (into) 
                    {
                        res.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Номер (если есть)
        /// </summary>
        public string Number
        {
            get
            {
                if (!m_NumberCalc) 
                {
                    m_Number = this.GetStringValue(ATTR_NUMBER);
                    m_NumberCalc = true;
                }
                return m_Number;
            }
            set
            {
                this.AddSlot(ATTR_NUMBER, value, true, 0);
            }
        }
        bool m_NumberCalc;
        string m_Number;
        /// <summary>
        /// Типа владелец - (Аппарат Президента)
        /// </summary>
        public Pullenti.Ner.Referent Owner
        {
            get
            {
                return this.GetSlotValue(ATTR_OWNER) as Pullenti.Ner.Referent;
            }
            set
            {
                this.AddSlot(ATTR_OWNER, value as Pullenti.Ner.Referent, true, 0);
            }
        }
        /// <summary>
        /// Вышестоящая организация
        /// </summary>
        public OrganizationReferent Higher
        {
            get
            {
                if (m_ParentCalc) 
                    return m_Parent;
                m_ParentCalc = true;
                m_Parent = this.GetSlotValue(ATTR_HIGHER) as OrganizationReferent;
                if (m_Parent == this || m_Parent == null) 
                    return m_Parent = null;
                Pullenti.Ner.Slot sl = m_Parent.FindSlot(ATTR_HIGHER, null, true);
                if (sl == null) 
                    return m_Parent;
                List<OrganizationReferent> li = new List<OrganizationReferent>();
                li.Add(this);
                li.Add(m_Parent);
                for (OrganizationReferent oo = sl.Value as OrganizationReferent; oo != null; oo = oo.GetSlotValue(ATTR_HIGHER) as OrganizationReferent) 
                {
                    if (li.Contains(oo)) 
                        return m_Parent = null;
                    li.Add(oo);
                }
                return m_Parent;
            }
            set
            {
                if (value != null) 
                {
                    OrganizationReferent d = value;
                    List<OrganizationReferent> li = new List<OrganizationReferent>();
                    for (; d != null; d = d.Higher) 
                    {
                        if (d == this) 
                            return;
                        else if (d.ToString() == this.ToString()) 
                            return;
                        if (li.Contains(d)) 
                            return;
                        li.Add(d);
                    }
                }
                this.AddSlot(ATTR_HIGHER, null, true, 0);
                if (value != null) 
                    this.AddSlot(ATTR_HIGHER, value, true, 0);
                m_ParentCalc = false;
            }
        }
        OrganizationReferent m_Parent;
        bool m_ParentCalc;
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                OrganizationReferent hi = Higher;
                if (hi != null) 
                    return hi;
                return Owner;
            }
        }
        static List<string> m_EmpryEponyms = new List<string>();
        /// <summary>
        /// Список объектов, которым посвящена организации (имени кого)
        /// </summary>
        public List<string> Eponyms
        {
            get
            {
                List<string> res = null;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_EPONYM) 
                    {
                        if (res == null) 
                            res = new List<string>();
                        res.Add(s.Value.ToString());
                    }
                }
                return res ?? m_EmpryEponyms;
            }
        }
        public void AddEponym(string rodPadezSurname)
        {
            if (rodPadezSurname == null) 
                return;
            rodPadezSurname = Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(rodPadezSurname);
            if (this.FindSlot(ATTR_EPONYM, rodPadezSurname, true) == null) 
                this.AddSlot(ATTR_EPONYM, rodPadezSurname, false, 0);
        }
        static List<Pullenti.Ner.Geo.GeoReferent> m_EmptyGeos = new List<Pullenti.Ner.Geo.GeoReferent>();
        /// <summary>
        /// Список географических объектов (GeoReferent)
        /// </summary>
        public List<Pullenti.Ner.Geo.GeoReferent> GeoObjects
        {
            get
            {
                List<Pullenti.Ner.Geo.GeoReferent> res = null;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_GEO && (s.Value is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        if (res == null) 
                            res = new List<Pullenti.Ner.Geo.GeoReferent>();
                        res.Add(s.Value as Pullenti.Ner.Geo.GeoReferent);
                    }
                }
                return res ?? m_EmptyGeos;
            }
        }
        internal bool AddGeoObject(object r)
        {
            if (r is Pullenti.Ner.Geo.GeoReferent) 
            {
                Pullenti.Ner.Geo.GeoReferent geo = r as Pullenti.Ner.Geo.GeoReferent;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_GEO && (s.Value is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        Pullenti.Ner.Geo.GeoReferent gg = s.Value as Pullenti.Ner.Geo.GeoReferent;
                        if (gg.CanBeEquals(geo, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText) || gg.Higher == geo) 
                            return true;
                        if (this.FindSlot(ATTR_TYPE, "посольство", true) != null) 
                            break;
                        if (geo.IsState != gg.IsState) 
                        {
                            if (gg.IsState) 
                            {
                                if (Kind == OrganizationKind.Govenment) 
                                    return false;
                                if (!geo.IsCity) 
                                    return false;
                            }
                        }
                        if (geo.IsCity == gg.IsCity) 
                        {
                            bool sovm = false;
                            foreach (string t in Types) 
                            {
                                if (t.Contains("совместн") || t.Contains("альянс")) 
                                    sovm = true;
                            }
                            if (!sovm) 
                                return false;
                        }
                        if (geo.Higher == gg) 
                        {
                            this.UploadSlot(s, geo);
                            return true;
                        }
                    }
                }
                this.AddSlot(ATTR_GEO, r, false, 0);
                return true;
            }
            else if (r is Pullenti.Ner.ReferentToken) 
            {
                if ((r as Pullenti.Ner.ReferentToken).GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                {
                    if (!this.AddGeoObject((r as Pullenti.Ner.ReferentToken).GetReferent())) 
                        return false;
                    this.AddExtReferent(r as Pullenti.Ner.ReferentToken);
                    return true;
                }
                if ((r as Pullenti.Ner.ReferentToken).GetReferent() is Pullenti.Ner.Address.AddressReferent) 
                    return this.AddGeoObject((r as Pullenti.Ner.ReferentToken).BeginToken.GetReferent());
            }
            return false;
        }
        string m_NameSingleNormalReal;
        Dictionary<string, bool> m_NameVars;
        List<string> m_NameHashs;
        internal Dictionary<string, bool> NameVars
        {
            get
            {
                if (m_NameVars != null) 
                    return m_NameVars;
                m_NameVars = new Dictionary<string, bool>();
                m_NameHashs = new List<string>();
                List<string> nameAbbr = null;
                OrganizationKind ki = Kind;
                foreach (string n in Names) 
                {
                    if (!m_NameVars.ContainsKey(n)) 
                        m_NameVars.Add(n, false);
                }
                foreach (string n in Names) 
                {
                    string a;
                    if (ki == OrganizationKind.Bank) 
                    {
                        if (!n.Contains("БАНК")) 
                        {
                            a = n + "БАНК";
                            if (!m_NameVars.ContainsKey(a)) 
                                m_NameVars.Add(a, false);
                        }
                    }
                    if ((((a = Pullenti.Ner.Core.MiscHelper.GetAbbreviation(n)))) != null && a.Length > 1) 
                    {
                        if (!m_NameVars.ContainsKey(a)) 
                            m_NameVars.Add(a, true);
                        if (nameAbbr == null) 
                            nameAbbr = new List<string>();
                        if (!nameAbbr.Contains(a)) 
                            nameAbbr.Add(a);
                        foreach (Pullenti.Ner.Geo.GeoReferent geo in GeoObjects) 
                        {
                            string aa = string.Format("{0}{1}", a, geo.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0)[0]);
                            if (!m_NameVars.ContainsKey(aa)) 
                                m_NameVars.Add(aa, true);
                            if (!nameAbbr.Contains(aa)) 
                                nameAbbr.Add(aa);
                        }
                    }
                    if ((((a = Pullenti.Ner.Core.MiscHelper.GetTailAbbreviation(n)))) != null) 
                    {
                        if (!m_NameVars.ContainsKey(a)) 
                            m_NameVars.Add(a, true);
                    }
                    int i = n.IndexOf(' ');
                    if (i > 0 && (n.IndexOf(' ', i + 1) < 0)) 
                    {
                        a = n.Replace(" ", "");
                        if (!m_NameVars.ContainsKey(a)) 
                            m_NameVars.Add(a, false);
                    }
                }
                foreach (string e in Eponyms) 
                {
                    foreach (string ty in Types) 
                    {
                        string na = string.Format("{0} {1}", ty, e).ToUpper();
                        if (!m_NameVars.ContainsKey(na)) 
                            m_NameVars.Add(na, false);
                    }
                }
                List<string> newVars = new List<string>();
                foreach (string n in Types) 
                {
                    string a = Pullenti.Ner.Core.MiscHelper.GetAbbreviation(n);
                    if (a == null) 
                        continue;
                    foreach (string v in m_NameVars.Keys) 
                    {
                        if (!v.StartsWith(a)) 
                        {
                            newVars.Add(a + v);
                            newVars.Add(a + " " + v);
                        }
                    }
                }
                foreach (string v in newVars) 
                {
                    if (!m_NameVars.ContainsKey(v)) 
                        m_NameVars.Add(v, true);
                }
                foreach (KeyValuePair<string, bool> kp in m_NameVars) 
                {
                    if (!kp.Value) 
                    {
                        string s = Pullenti.Ner.Core.MiscHelper.GetAbsoluteNormalValue(kp.Key, false);
                        if (s != null && s.Length > 4) 
                        {
                            if (!m_NameHashs.Contains(s)) 
                                m_NameHashs.Add(s);
                        }
                    }
                }
                return m_NameVars;
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            bool ret = this.CanBeEqualsEx(obj, false, typ);
            return ret;
        }
        int m_Level;
        public override bool CanBeGeneralFor(Pullenti.Ner.Referent obj)
        {
            if (m_Level > 10) 
                return false;
            m_Level++;
            bool b = this.CanBeEqualsEx(obj, true, Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts);
            m_Level--;
            if (!b) 
                return false;
            List<Pullenti.Ner.Geo.GeoReferent> geos1 = GeoObjects;
            List<Pullenti.Ner.Geo.GeoReferent> geos2 = (obj as OrganizationReferent).GeoObjects;
            if (geos1.Count == 0 && geos2.Count > 0) 
            {
                if (this._checkEqEponyms(obj as OrganizationReferent)) 
                    return false;
                return true;
            }
            else if (geos1.Count == geos2.Count) 
            {
                if (this._checkEqEponyms(obj as OrganizationReferent)) 
                    return false;
                if (Higher != null && (obj as OrganizationReferent).Higher != null) 
                {
                    m_Level++;
                    b = Higher.CanBeGeneralFor((obj as OrganizationReferent).Higher);
                    m_Level--;
                    if (b) 
                        return true;
                }
            }
            return false;
        }
        bool _checkEqEponyms(OrganizationReferent org)
        {
            if (this.FindSlot(ATTR_EPONYM, null, true) == null && org.FindSlot(ATTR_EPONYM, null, true) == null) 
                return false;
            ICollection<string> eps = (ICollection<string>)Eponyms;
            ICollection<string> eps1 = (ICollection<string>)org.Eponyms;
            foreach (string e in eps) 
            {
                if (eps1.Contains(e)) 
                    return true;
                if (!Pullenti.Morph.LanguageHelper.EndsWith(e, "а")) 
                {
                    if (eps1.Contains(e + "а")) 
                        return true;
                }
            }
            foreach (string e in eps1) 
            {
                if (eps.Contains(e)) 
                    return true;
                if (!Pullenti.Morph.LanguageHelper.EndsWith(e, "а")) 
                {
                    if (eps.Contains(e + "а")) 
                        return true;
                }
            }
            if (this.FindSlot(ATTR_EPONYM, null, true) != null && org.FindSlot(ATTR_EPONYM, null, true) != null) 
                return false;
            string s = org.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0);
            foreach (string e in Eponyms) 
            {
                if (s.Contains(e)) 
                    return true;
            }
            s = this.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0);
            foreach (string e in org.Eponyms) 
            {
                if (s.Contains(e)) 
                    return true;
            }
            return false;
        }
        internal OrganizationReferent m_TempParentOrg;
        public bool CanBeEqualsEx(Pullenti.Ner.Referent obj, bool ignoreGeoObjects, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            if (m_Level > 10) 
                return false;
            m_Level++;
            bool ret = this._canBeEquals(obj, ignoreGeoObjects, typ, 0);
            m_Level--;
            if (!ret) 
            {
            }
            return ret;
        }
        bool _canBeEquals(Pullenti.Ner.Referent obj, bool ignoreGeoObjects, Pullenti.Ner.Core.ReferentsEqualType typ, int lev)
        {
            OrganizationReferent org = obj as OrganizationReferent;
            if (org == null) 
                return false;
            if (org == this) 
                return true;
            if (lev > 4) 
                return false;
            bool empty = true;
            bool geoNotEquals = false;
            OrganizationKind k1 = Kind;
            OrganizationKind k2 = org.Kind;
            List<Pullenti.Ner.Geo.GeoReferent> geos1 = GeoObjects;
            List<Pullenti.Ner.Geo.GeoReferent> geos2 = org.GeoObjects;
            if (geos1.Count > 0 && geos2.Count > 0) 
            {
                geoNotEquals = true;
                foreach (Pullenti.Ner.Geo.GeoReferent g1 in geos1) 
                {
                    bool eq = false;
                    foreach (Pullenti.Ner.Geo.GeoReferent g2 in geos2) 
                    {
                        if (g1.CanBeEquals(g2, typ)) 
                        {
                            geoNotEquals = false;
                            eq = true;
                            break;
                        }
                    }
                    if (!eq) 
                        return false;
                }
                if (geos2.Count > geos1.Count) 
                {
                    foreach (Pullenti.Ner.Geo.GeoReferent g1 in geos2) 
                    {
                        bool eq = false;
                        foreach (Pullenti.Ner.Geo.GeoReferent g2 in geos1) 
                        {
                            if (g1.CanBeEquals(g2, typ)) 
                            {
                                geoNotEquals = false;
                                eq = true;
                                break;
                            }
                        }
                        if (!eq) 
                            return false;
                    }
                }
            }
            if (this.FindSlot(ATTR_MARKER, null, true) != null && org.FindSlot(ATTR_MARKER, null, true) != null) 
            {
                List<string> mrks1 = this.GetStringValues(ATTR_MARKER);
                List<string> mrks2 = obj.GetStringValues(ATTR_MARKER);
                foreach (string m in mrks1) 
                {
                    if (!mrks2.Contains(m)) 
                        return false;
                }
                foreach (string m in mrks2) 
                {
                    if (!mrks1.Contains(m)) 
                        return false;
                }
            }
            string inn = INN;
            string inn2 = org.INN;
            if (inn != null && inn2 != null) 
                return inn == inn2;
            string ogrn = OGRN;
            string ogrn2 = org.OGRN;
            if (ogrn != null && ogrn2 != null) 
                return ogrn == ogrn2;
            OrganizationReferent hi1 = Higher ?? m_TempParentOrg;
            OrganizationReferent hi2 = org.Higher ?? org.m_TempParentOrg;
            bool hiEq = false;
            if (hi1 != null && hi2 != null) 
            {
                if (org.FindSlot(ATTR_HIGHER, hi1, false) == null) 
                {
                    if (hi1._canBeEquals(hi2, ignoreGeoObjects, typ, lev + 1)) 
                    {
                    }
                    else 
                        return false;
                }
                hiEq = true;
            }
            if (Owner != null || org.Owner != null) 
            {
                if (Owner == null || org.Owner == null) 
                    return false;
                if (!Owner.CanBeEquals(org.Owner, typ)) 
                    return false;
                if (this.FindSlot(ATTR_TYPE, "индивидуальное предприятие", true) != null || org.FindSlot(ATTR_TYPE, "индивидуальное предприятие", true) != null) 
                    return true;
                hiEq = true;
            }
            if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts && !hiEq) 
            {
                if (Higher != null || org.Higher != null) 
                    return false;
            }
            if (Pullenti.Ner.Org.Internal.OrgItemTypeToken.IsTypesAntagonisticOO(this, org)) 
                return false;
            if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
            {
                if (k1 == OrganizationKind.Department || k2 == OrganizationKind.Department) 
                {
                    if (hi1 == null && hi2 != null) 
                        return false;
                    if (hi1 != null && hi2 == null) 
                        return false;
                }
                else if (k1 != k2) 
                    return false;
            }
            bool eqEponyms = this._checkEqEponyms(org);
            bool eqNumber = false;
            if (Number != null || org.Number != null) 
            {
                if (org.Number != Number) 
                {
                    if (((org.Number == null || Number == null)) && eqEponyms) 
                    {
                    }
                    else if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((org.Number == null || Number == null))) 
                    {
                    }
                    else 
                        return false;
                }
                else 
                {
                    empty = false;
                    foreach (Pullenti.Ner.Slot a in Slots) 
                    {
                        if (a.TypeName == ATTR_TYPE) 
                        {
                            if (obj.FindSlot(a.TypeName, a.Value, true) != null || obj.FindSlot(ATTR_NAME, (a.Value as string).ToUpper(), true) != null) 
                            {
                                eqNumber = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
            {
                if (Number != null || org.Number != null) 
                {
                    if (!eqNumber && !eqEponyms) 
                        return false;
                }
            }
            if (k1 != OrganizationKind.Undefined && k2 != OrganizationKind.Undefined) 
            {
                if (k1 != k2) 
                {
                    bool oo = false;
                    foreach (string ty1 in Types) 
                    {
                        if (org.Types.Contains(ty1)) 
                        {
                            oo = true;
                            break;
                        }
                    }
                    if (!oo) 
                    {
                        bool hasPr = false;
                        foreach (OrgProfile p in Profiles) 
                        {
                            if (org.ContainsProfile(p)) 
                            {
                                hasPr = true;
                                break;
                            }
                        }
                        if (!hasPr) 
                            return false;
                    }
                }
            }
            else 
            {
                if (k1 == OrganizationKind.Undefined) 
                    k1 = k2;
                if ((k1 == OrganizationKind.Bank || k1 == OrganizationKind.Medical || k1 == OrganizationKind.Party) || k1 == OrganizationKind.Culture) 
                {
                    if (Types.Count > 0 && org.Types.Count > 0) 
                    {
                        if (typ != Pullenti.Ner.Core.ReferentsEqualType.ForMerging) 
                            return false;
                        bool ok = false;
                        foreach (Pullenti.Ner.Slot s in Slots) 
                        {
                            if (s.TypeName == ATTR_NAME) 
                            {
                                if (org.FindSlot(s.TypeName, s.Value, true) != null) 
                                    ok = true;
                            }
                        }
                        if (!ok) 
                            return false;
                    }
                }
            }
            if ((k1 == OrganizationKind.Govenment || k2 == OrganizationKind.Govenment || k1 == OrganizationKind.Military) || k2 == OrganizationKind.Military) 
            {
                List<string> typs = org.Types;
                bool ok = false;
                foreach (string ty in Types) 
                {
                    if (typs.Contains(ty)) 
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok) 
                    return false;
            }
            if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging) 
            {
            }
            else if (this.FindSlot(ATTR_NAME, null, true) != null || org.FindSlot(ATTR_NAME, null, true) != null) 
            {
                if (((eqNumber || eqEponyms)) && ((this.FindSlot(ATTR_NAME, null, true) == null || org.FindSlot(ATTR_NAME, null, true) == null))) 
                {
                }
                else 
                {
                    empty = false;
                    int maxLen = 0;
                    foreach (KeyValuePair<string, bool> v in NameVars) 
                    {
                        if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts && v.Value) 
                            continue;
                        bool b;
                        if (!org.NameVars.TryGetValue(v.Key, out b)) 
                            continue;
                        if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts && b) 
                            continue;
                        if (b && v.Value) 
                            continue;
                        if (b && Names.Count > 1 && (v.Key.Length < 4)) 
                            continue;
                        if (v.Value && org.Names.Count > 1 && (v.Key.Length < 4)) 
                            continue;
                        if (v.Key.Length > maxLen) 
                            maxLen = v.Key.Length;
                    }
                    if (typ != Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                    {
                        foreach (string v in m_NameHashs) 
                        {
                            if (org.m_NameHashs.Contains(v)) 
                            {
                                if (v.Length > maxLen) 
                                    maxLen = v.Length;
                            }
                        }
                    }
                    if ((maxLen < 2) && ((k1 == OrganizationKind.Govenment || typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging)) && typ != Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                    {
                        if (geos1.Count == geos2.Count) 
                        {
                            ICollection<string> nams = (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging ? org.NameVars.Keys : (ICollection<string>)org.Names);
                            ICollection<string> nams0 = (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging ? NameVars.Keys : (ICollection<string>)Names);
                            foreach (string n in nams0) 
                            {
                                foreach (string nn in nams) 
                                {
                                    if (n.StartsWith(nn)) 
                                    {
                                        maxLen = nn.Length;
                                        break;
                                    }
                                    else if (nn.StartsWith(n)) 
                                    {
                                        maxLen = n.Length;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (maxLen < 2) 
                        return false;
                    if (maxLen < 4) 
                    {
                        bool ok = false;
                        if (!ok) 
                        {
                            if (Names.Count == 1 && (Names[0].Length < 4)) 
                                ok = true;
                            else if (org.Names.Count == 1 && (org.Names[0].Length < 4)) 
                                ok = true;
                        }
                        if (!ok) 
                            return false;
                    }
                }
            }
            if (eqEponyms) 
                return true;
            if (this.FindSlot(ATTR_EPONYM, null, true) != null || obj.FindSlot(ATTR_EPONYM, null, true) != null) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((this.FindSlot(ATTR_EPONYM, null, true) == null || obj.FindSlot(ATTR_EPONYM, null, true) == null))) 
                {
                }
                else 
                {
                    bool ok = false;
                    ICollection<string> eps = (ICollection<string>)Eponyms;
                    ICollection<string> eps1 = (ICollection<string>)org.Eponyms;
                    foreach (string e in eps) 
                    {
                        if (eps1.Contains(e)) 
                        {
                            ok = true;
                            break;
                        }
                        if (!Pullenti.Morph.LanguageHelper.EndsWith(e, "а")) 
                        {
                            if (eps1.Contains(e + "а")) 
                            {
                                ok = true;
                                break;
                            }
                        }
                    }
                    if (!ok) 
                    {
                        foreach (string e in eps1) 
                        {
                            if (eps.Contains(e)) 
                            {
                                ok = true;
                                break;
                            }
                            if (!Pullenti.Morph.LanguageHelper.EndsWith(e, "а")) 
                            {
                                if (eps.Contains(e + "а")) 
                                {
                                    ok = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (ok) 
                        return true;
                    if (this.FindSlot(ATTR_EPONYM, null, true) == null || obj.FindSlot(ATTR_EPONYM, null, true) == null) 
                    {
                        string s = obj.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0);
                        foreach (string e in Eponyms) 
                        {
                            if (s.Contains(e)) 
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (!ok) 
                        {
                            s = this.ToString(true, Pullenti.Morph.MorphLang.Unknown, 0);
                            foreach (string e in org.Eponyms) 
                            {
                                if (s.Contains(e)) 
                                {
                                    ok = true;
                                    break;
                                }
                            }
                        }
                        if (ok) 
                            return true;
                        else if (empty) 
                            return false;
                    }
                    else 
                        return false;
                }
            }
            if (geoNotEquals) 
            {
                if (k1 == OrganizationKind.Bank || k1 == OrganizationKind.Govenment || k1 == OrganizationKind.Department) 
                    return false;
            }
            if (k1 != OrganizationKind.Department) 
            {
                if (!empty) 
                    return true;
                if (hiEq) 
                {
                    List<string> typs = org.Types;
                    foreach (string ty in Types) 
                    {
                        if (typs.Contains(ty)) 
                            return true;
                    }
                }
            }
            if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                return this.ToString() == org.ToString();
            if (empty) 
            {
                if (((geos1.Count > 0 && geos2.Count > 0)) || k1 == OrganizationKind.Department || k1 == OrganizationKind.Justice) 
                {
                    List<string> typs = org.Types;
                    foreach (string ty in Types) 
                    {
                        if (typs.Contains(ty)) 
                            return true;
                    }
                }
                bool fullNotEq = false;
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (org.FindSlot(s.TypeName, s.Value, true) == null) 
                    {
                        fullNotEq = true;
                        break;
                    }
                }
                foreach (Pullenti.Ner.Slot s in org.Slots) 
                {
                    if (this.FindSlot(s.TypeName, s.Value, true) == null) 
                    {
                        fullNotEq = true;
                        break;
                    }
                }
                if (!fullNotEq) 
                    return true;
            }
            else if (k1 == OrganizationKind.Department) 
                return true;
            if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging) 
                return true;
            return false;
        }
        public override Pullenti.Ner.Slot AddSlot(string attrName, object attrValue, bool clearOldValue, int statCount = 0)
        {
            if (attrName == ATTR_NAME || attrName == ATTR_TYPE) 
            {
                m_NameVars = null;
                m_NameHashs = null;
            }
            else if (attrName == ATTR_HIGHER) 
                m_ParentCalc = false;
            else if (attrName == ATTR_NUMBER) 
                m_NumberCalc = false;
            m_KindCalc = false;
            Pullenti.Ner.Slot sl = base.AddSlot(attrName, attrValue, clearOldValue, statCount);
            return sl;
        }
        public override void UploadSlot(Pullenti.Ner.Slot slot, object newVal)
        {
            m_ParentCalc = false;
            base.UploadSlot(slot, newVal);
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic)
        {
            OrganizationReferent ownThis = Higher;
            OrganizationReferent ownObj = (obj as OrganizationReferent).Higher;
            base.MergeSlots(obj, mergeStatistic);
            for (int i = Slots.Count - 1; i >= 0; i--) 
            {
                if (Slots[i].TypeName == ATTR_HIGHER) 
                    Slots.RemoveAt(i);
            }
            if (ownThis == null) 
                ownThis = ownObj;
            if (ownThis != null) 
                Higher = ownThis;
            if ((obj as OrganizationReferent).IsFromGlobalOntos) 
                IsFromGlobalOntos = true;
            this.CorrectData(true);
        }
        public bool IsFromGlobalOntos;
        internal void CorrectData(bool removeLongGovNames)
        {
            for (int i = Slots.Count - 1; i >= 0; i--) 
            {
                if (Slots[i].TypeName == ATTR_TYPE) 
                {
                    string ty = Slots[i].ToString().ToUpper();
                    bool del = false;
                    foreach (Pullenti.Ner.Slot s in Slots) 
                    {
                        if (s.TypeName == ATTR_NAME) 
                        {
                            string na = s.Value.ToString();
                            if (Pullenti.Morph.LanguageHelper.EndsWith(ty, na)) 
                                del = true;
                        }
                    }
                    if (del) 
                        Slots.RemoveAt(i);
                }
            }
            foreach (string t in Types) 
            {
                Pullenti.Ner.Slot n = this.FindSlot(ATTR_NAME, t.ToUpper(), true);
                if (n != null) 
                    Slots.Remove(n);
            }
            foreach (string t in Names) 
            {
                if (t.IndexOf('.') > 0) 
                {
                    Pullenti.Ner.Slot n = this.FindSlot(ATTR_NAME, t.Replace('.', ' '), true);
                    if (n == null) 
                        this.AddSlot(ATTR_NAME, t.Replace('.', ' '), false, 0);
                }
            }
            ICollection<string> eps = (ICollection<string>)Eponyms;
            if (eps.Count > 1) 
            {
                foreach (string e in eps) 
                {
                    foreach (string ee in eps) 
                    {
                        if (e != ee && e.StartsWith(ee)) 
                        {
                            Pullenti.Ner.Slot s = this.FindSlot(ATTR_EPONYM, ee, true);
                            if (s != null) 
                                Slots.Remove(s);
                        }
                    }
                }
            }
            List<string> typs = Types;
            List<string> epons = Eponyms;
            foreach (string t in typs) 
            {
                foreach (string e in epons) 
                {
                    Pullenti.Ner.Slot n = this.FindSlot(ATTR_NAME, string.Format("{0} {1}", t.ToUpper(), e.ToUpper()), true);
                    if (n != null) 
                        Slots.Remove(n);
                }
            }
            if (removeLongGovNames && Kind == OrganizationKind.Govenment) 
            {
                List<string> nams = Names;
                for (int i = Slots.Count - 1; i >= 0; i--) 
                {
                    if (Slots[i].TypeName == ATTR_NAME) 
                    {
                        string n = Slots[i].Value.ToString();
                        foreach (string nn in nams) 
                        {
                            if (n.StartsWith(nn) && n.Length > nn.Length) 
                            {
                                Slots.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            if (Types.Contains("фронт")) 
            {
                bool uni = false;
                foreach (string ty in Types) 
                {
                    if (ty.Contains("объединение")) 
                        uni = true;
                }
                if (uni || Profiles.Contains(OrgProfile.Union)) 
                {
                    Pullenti.Ner.Slot ss = this.FindSlot(ATTR_PROFILE, "ARMY", true);
                    if (ss != null) 
                    {
                        Slots.Remove(ss);
                        this.AddProfile(OrgProfile.Union);
                    }
                    if ((((ss = this.FindSlot(ATTR_TYPE, "фронт", true)))) != null) 
                        Slots.Remove(ss);
                }
            }
            m_NameVars = null;
            m_NameHashs = null;
            m_KindCalc = false;
            ExtOntologyAttached = false;
        }
        internal void FinalCorrection()
        {
            List<string> typs = Types;
            if (this.ContainsProfile(OrgProfile.Education) && this.ContainsProfile(OrgProfile.Science)) 
            {
                if (typs.Contains("академия") || typs.Contains("академія") || typs.Contains("academy")) 
                {
                    bool isSci = false;
                    foreach (string n in Names) 
                    {
                        if (n.Contains("НАУЧН") || n.Contains("НАУК") || n.Contains("SCIENC")) 
                        {
                            isSci = true;
                            break;
                        }
                    }
                    Pullenti.Ner.Slot s = null;
                    if (isSci) 
                        s = this.FindSlot(ATTR_PROFILE, OrgProfile.Education.ToString(), true);
                    else 
                        s = this.FindSlot(ATTR_PROFILE, OrgProfile.Science.ToString(), true);
                    if (s != null) 
                        Slots.Remove(s);
                }
            }
            if (this.FindSlot(ATTR_PROFILE, null, true) == null) 
            {
                if (typs.Contains("служба") && Higher != null) 
                    this.AddProfile(OrgProfile.Unit);
            }
            if (typs.Count > 0 && Pullenti.Morph.LanguageHelper.IsLatin(typs[0])) 
            {
                if (this.FindSlot(ATTR_NAME, null, true) == null && typs.Count > 1) 
                {
                    string nam = typs[0];
                    foreach (string v in typs) 
                    {
                        if (v.Length > nam.Length) 
                            nam = v;
                    }
                    if (nam.IndexOf(' ') > 0) 
                    {
                        this.AddSlot(ATTR_NAME, nam.ToUpper(), false, 0);
                        Pullenti.Ner.Slot s = this.FindSlot(ATTR_TYPE, nam, true);
                        if (s != null) 
                            Slots.Remove(s);
                    }
                }
                if ((this.FindSlot(ATTR_NAME, null, true) == null && this.FindSlot(ATTR_GEO, null, true) != null && this.FindSlot(ATTR_NUMBER, null, true) == null) && typs.Count > 0) 
                {
                    Pullenti.Ner.Geo.GeoReferent geo = this.GetSlotValue(ATTR_GEO) as Pullenti.Ner.Geo.GeoReferent;
                    if (geo != null) 
                    {
                        string nam = geo.GetStringValue(Pullenti.Ner.Geo.GeoReferent.ATTR_NAME);
                        if (nam != null && Pullenti.Morph.LanguageHelper.IsLatin(nam)) 
                        {
                            bool nn = false;
                            foreach (string t in typs) 
                            {
                                if (t.ToUpper().Contains(nam)) 
                                {
                                    this.AddSlot(ATTR_NAME, t.ToUpper(), false, 0);
                                    nn = true;
                                    if (typs.Count > 1) 
                                    {
                                        Pullenti.Ner.Slot s = this.FindSlot(ATTR_TYPE, t, true);
                                        if (s != null) 
                                            Slots.Remove(s);
                                    }
                                    break;
                                }
                            }
                            if (!nn) 
                                this.AddSlot(ATTR_NAME, string.Format("{0} {1}", nam, typs[0]).ToUpper(), false, 0);
                        }
                    }
                }
            }
            m_NameVars = null;
            m_NameHashs = null;
            m_KindCalc = false;
            ExtOntologyAttached = false;
        }
        internal List<string> _getPureNames()
        {
            List<string> vars = new List<string>();
            List<string> typs = Types;
            foreach (Pullenti.Ner.Slot a in Slots) 
            {
                if (a.TypeName == ATTR_NAME) 
                {
                    string s = a.Value.ToString().ToUpper();
                    if (!vars.Contains(s)) 
                        vars.Add(s);
                    foreach (string t in typs) 
                    {
                        if (s.StartsWith(t, StringComparison.OrdinalIgnoreCase)) 
                        {
                            if ((s.Length < (t.Length + 4)) || s[t.Length] != ' ') 
                                continue;
                            string ss = s.Substring(t.Length + 1);
                            if (!vars.Contains(ss)) 
                                vars.Add(ss);
                        }
                    }
                }
            }
            return vars;
        }
        internal bool ExtOntologyAttached;
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            return this.CreateOntologyItemEx(2, false, false);
        }
        public Pullenti.Ner.Core.IntOntologyItem CreateOntologyItemEx(int minLen, bool onlyNames = false, bool pureNames = false)
        {
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            List<string> vars = new List<string>();
            List<string> typs = Types;
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
            if (!pureNames) 
            {
                foreach (string v in NameVars.Keys) 
                {
                    if (!vars.Contains(v)) 
                        vars.Add(v);
                }
            }
            if (!onlyNames) 
            {
                if (Number != null) 
                {
                    foreach (Pullenti.Ner.Slot a in Slots) 
                    {
                        if (a.TypeName == ATTR_TYPE) 
                        {
                            string s = a.Value.ToString().ToUpper();
                            if (!vars.Contains(s)) 
                                vars.Add(s);
                        }
                    }
                }
                if (vars.Count == 0) 
                {
                    foreach (string t in Types) 
                    {
                        string up = t.ToUpper();
                        if (!vars.Contains(up)) 
                            vars.Add(up);
                    }
                }
                if (INN != null) 
                    vars.Insert(0, "ИНН:" + INN);
                if (OGRN != null) 
                    vars.Insert(0, "ОГРН:" + OGRN);
            }
            int max = 20;
            int cou = 0;
            foreach (string v in vars) 
            {
                if (v.Length >= minLen) 
                {
                    Pullenti.Ner.Core.Termin term;
                    if (pureNames) 
                    {
                        term = new Pullenti.Ner.Core.Termin();
                        term.InitByNormalText(v, null);
                    }
                    else 
                        term = new Pullenti.Ner.Core.Termin(v);
                    oi.Termins.Add(term);
                    if ((++cou) >= max) 
                        break;
                }
            }
            if (oi.Termins.Count == 0) 
                return null;
            return oi;
        }
        /// <summary>
        /// Категория организации (некоторая экспертная оценка на основе названия и типов)
        /// </summary>
        public OrganizationKind Kind
        {
            get
            {
                if (!m_KindCalc) 
                {
                    m_Kind = Pullenti.Ner.Org.Internal.OrgItemTypeToken.CheckKind(this);
                    if (m_Kind == OrganizationKind.Undefined) 
                    {
                        foreach (OrgProfile p in Profiles) 
                        {
                            if (p == OrgProfile.Unit) 
                            {
                                m_Kind = OrganizationKind.Department;
                                break;
                            }
                        }
                    }
                    m_KindCalc = true;
                }
                return m_Kind;
            }
        }
        OrganizationKind m_Kind = OrganizationKind.Undefined;
        bool m_KindCalc = false;
        public override string GetStringValue(string attrName)
        {
            if (attrName == "KIND") 
            {
                OrganizationKind ki = Kind;
                if (ki == OrganizationKind.Undefined) 
                    return null;
                return ki.ToString();
            }
            return base.GetStringValue(attrName);
        }
        // Проверка, что организация slave может быть дополнительным описанием основной организации
        public static bool CanBeSecondDefinition(OrganizationReferent master, OrganizationReferent slave)
        {
            if (master == null || slave == null) 
                return false;
            List<string> mTypes = master.Types;
            List<string> sTypes = slave.Types;
            bool ok = false;
            foreach (string t in mTypes) 
            {
                if (sTypes.Contains(t)) 
                {
                    ok = true;
                    break;
                }
            }
            if (ok) 
                return true;
            if (master.Kind != OrganizationKind.Undefined && slave.Kind != OrganizationKind.Undefined) 
            {
                if (master.Kind != slave.Kind) 
                    return false;
            }
            if (sTypes.Count > 0) 
                return false;
            if (slave.Names.Count == 1) 
            {
                string acr = slave.Names[0];
                if (Pullenti.Morph.LanguageHelper.EndsWith(acr, "АН")) 
                    return true;
                foreach (string n in master.Names) 
                {
                    if (CheckAcronym(acr, n) || CheckAcronym(n, acr)) 
                        return true;
                    if (CheckLatinAccords(n, acr)) 
                        return true;
                    foreach (string t in mTypes) 
                    {
                        if (CheckAcronym(acr, t.ToUpper() + n)) 
                            return true;
                    }
                }
            }
            return false;
        }
        static bool CheckLatinAccords(string rusName, string latName)
        {
            if (!Pullenti.Morph.LanguageHelper.IsCyrillicChar(rusName[0]) || !Pullenti.Morph.LanguageHelper.IsLatinChar(latName[0])) 
                return false;
            string[] ru = rusName.Split(' ');
            string[] la = latName.Split(' ');
            int i = 0;
            int j = 0;
            while ((i < ru.Length) && (j < la.Length)) 
            {
                if (string.Compare(la[j], "THE", true) == 0 || string.Compare(la[j], "OF", true) == 0) 
                {
                    j++;
                    continue;
                }
                if (Pullenti.Ner.Core.MiscHelper.CanBeEqualCyrAndLatSS(ru[i], la[j])) 
                    return true;
                i++;
                j++;
            }
            if ((i < ru.Length) || (j < la.Length)) 
                return false;
            if (i >= 2) 
                return true;
            return false;
        }
        static bool CheckAcronym(string acr, string text)
        {
            int i = 0;
            int j = 0;
            for (i = 0; i < acr.Length; i++) 
            {
                for (; j < text.Length; j++) 
                {
                    if (text[j] == acr[i]) 
                        break;
                }
                if (j >= text.Length) 
                    break;
                j++;
            }
            return i >= acr.Length;
        }
        // Проверка на отношения "вышестоящий - нижестоящий"
        public static bool CanBeHigher(OrganizationReferent higher, OrganizationReferent lower)
        {
            return Pullenti.Ner.Org.Internal.OrgOwnershipHelper.CanBeHigher(higher, lower, false);
        }
    }
}