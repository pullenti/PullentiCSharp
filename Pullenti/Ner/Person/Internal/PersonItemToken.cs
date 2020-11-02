/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Person.Internal
{
    public class PersonItemToken : Pullenti.Ner.MetaToken
    {
        private PersonItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        internal static void Initialize()
        {
            MorphPersonItem.Initialize();
        }
        public enum ItemType : int
        {
            Value,
            Initial,
            Referent,
            Suffix,
        }

        public ItemType Typ = ItemType.Value;
        public string Value;
        public bool IsInDictionary;
        public bool IsHiphenBefore;
        public bool IsHiphenAfter;
        public bool IsAsianItem(bool last)
        {
            if (Value == null || Typ != ItemType.Value) 
                return false;
            if (Chars.IsAllLower) 
                return false;
            if (Chars.IsAllUpper && LengthChar > 1) 
                return false;
            int sogl = 0;
            int gl = 0;
            bool prevGlas = false;
            for (int i = 0; i < Value.Length; i++) 
            {
                char ch = Value[i];
                if (!Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch)) 
                    return false;
                else if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(ch)) 
                {
                    if (!prevGlas) 
                    {
                        if (gl > 0) 
                        {
                            if (!last) 
                                return false;
                            if (i == (Value.Length - 1) && ((ch == 'А' || ch == 'У' || ch == 'Е'))) 
                                break;
                            else if (i == (Value.Length - 2) && ch == 'О' && Value[i + 1] == 'М') 
                                break;
                        }
                        gl++;
                    }
                    prevGlas = true;
                }
                else 
                {
                    sogl++;
                    prevGlas = false;
                }
            }
            if (gl != 1) 
            {
                if (last && gl == 2) 
                {
                }
                else 
                    return false;
            }
            if (sogl > 4) 
                return false;
            if (Value.Length == 1) 
            {
                if (!Chars.IsAllUpper) 
                    return false;
            }
            else if (!Chars.IsCapitalUpper) 
                return false;
            return true;
        }
        public MorphPersonItem Firstname;
        public MorphPersonItem Lastname;
        public MorphPersonItem Middlename;
        public Pullenti.Ner.Person.PersonReferent Referent;
        public class MorphPersonItemVariant : Pullenti.Morph.MorphBaseInfo
        {
            public MorphPersonItemVariant(string v, Pullenti.Morph.MorphBaseInfo bi, bool lastname)
            {
                Value = v;
                if (bi != null) 
                    this.CopyFrom(bi);
            }
            public string Value;
            public string ShortValue;
            public override string ToString()
            {
                return string.Format("{0}: {1}", Value ?? "?", base.ToString());
            }
        }

        public class MorphPersonItem
        {
            public Pullenti.Ner.MorphCollection Morph
            {
                get
                {
                    if (m_Morph != null && m_Morph.ItemsCount != Vars.Count) 
                        m_Morph = null;
                    if (m_Morph == null) 
                    {
                        m_Morph = new Pullenti.Ner.MorphCollection();
                        foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in Vars) 
                        {
                            m_Morph.AddItem(v);
                        }
                    }
                    return m_Morph;
                }
            }
            Pullenti.Ner.MorphCollection m_Morph;
            public List<Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant> Vars = new List<Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant>();
            public string Term;
            public bool IsInDictionary;
            public bool IsInOntology;
            public bool IsLastnameHasStdTail;
            public bool IsLastnameHasHiphen;
            public bool IsHasStdPostfix;
            public bool IsChinaSurname
            {
                get
                {
                    string term = Term;
                    if (term == null && Vars.Count > 0) 
                        term = Vars[0].Value;
                    if (term == null) 
                        return false;
                    if (m_LastnameAsian.IndexOf(term) >= 0) 
                        return true;
                    string tr = Pullenti.Ner.Person.PersonReferent._DelSurnameEnd(term);
                    if (m_LastnameAsian.IndexOf(tr) >= 0) 
                        return true;
                    if (m_LastnameAsian.IndexOf(term + "Ь") >= 0) 
                        return true;
                    if (term[term.Length - 1] == 'Ь') 
                    {
                        if (m_LastnameAsian.IndexOf(term.Substring(0, term.Length - 1)) >= 0) 
                            return true;
                    }
                    return false;
                }
            }
            public override string ToString()
            {
                StringBuilder res = new StringBuilder();
                if (Term != null) 
                    res.Append(Term);
                foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in Vars) 
                {
                    res.AppendFormat("; {0}", v.ToString());
                }
                if (IsInDictionary) 
                    res.Append(" - InDictionary");
                if (IsInOntology) 
                    res.Append(" - InOntology");
                if (IsLastnameHasStdTail) 
                    res.Append(" - IsLastnameHasStdTail");
                if (IsHasStdPostfix) 
                    res.Append(" - IsHasStdPostfix");
                if (IsChinaSurname) 
                    res.Append(" - IsChinaSurname");
                return res.ToString();
            }
            public void MergeHiphen(MorphPersonItem second)
            {
                List<Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant> addvars = new List<Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant>();
                foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in Vars) 
                {
                    int ok = 0;
                    foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant vv in second.Vars) 
                    {
                        if (((vv.Gender & v.Gender)) != Pullenti.Morph.MorphGender.Undefined) 
                        {
                            v.Value = string.Format("{0}-{1}", v.Value, vv.Value);
                            ok++;
                            break;
                        }
                    }
                    if (ok > 0) 
                        continue;
                    if (v.Gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant vv in second.Vars) 
                        {
                            if (vv.Gender == Pullenti.Morph.MorphGender.Undefined) 
                            {
                                v.Value = string.Format("{0}-{1}", v.Value, vv.Value);
                                ok++;
                                break;
                            }
                        }
                        if (ok > 0) 
                            continue;
                    }
                    else 
                    {
                        string val0 = v.Value;
                        foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant vv in second.Vars) 
                        {
                            if (vv.Gender != Pullenti.Morph.MorphGender.Undefined) 
                            {
                                if (ok == 0) 
                                {
                                    v.Value = string.Format("{0}-{1}", val0, vv.Value);
                                    v.CopyFrom(vv);
                                }
                                else 
                                    addvars.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant(string.Format("{0}-{1}", val0, vv.Value), vv, false));
                                ok++;
                            }
                        }
                        if (ok > 0) 
                            continue;
                    }
                    if (second.Vars.Count == 0) 
                        continue;
                    v.Value = string.Format("{0}-{1}", v.Value, second.Vars[0].Value);
                }
                Vars.AddRange(addvars);
            }
            public void AddPrefix(string val)
            {
                if (Term != null) 
                    Term = val + Term;
                foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in Vars) 
                {
                    if (v.Value != null) 
                        v.Value = val + v.Value;
                }
            }
            public void AddPostfix(string val, Pullenti.Morph.MorphGender gen)
            {
                if (Term != null) 
                    Term = string.Format("{0}-{1}", Term, val);
                foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in Vars) 
                {
                    if (v.Value != null) 
                    {
                        v.Value = string.Format("{0}-{1}", v.Value, val);
                        if (gen != Pullenti.Morph.MorphGender.Undefined) 
                            v.Gender = gen;
                    }
                }
                IsHasStdPostfix = true;
                IsInDictionary = false;
            }
            public void MergeWithByHiphen(MorphPersonItem pi)
            {
                Term = string.Format("{0}-{1}", Term ?? "", pi.Term ?? "");
                if (pi.IsInDictionary) 
                    IsInDictionary = true;
                if (pi.IsHasStdPostfix) 
                    IsHasStdPostfix = true;
                IsLastnameHasHiphen = true;
                if (pi.Vars.Count == 0) 
                {
                    if (pi.Term != null) 
                        this.AddPostfix(pi.Term, Pullenti.Morph.MorphGender.Undefined);
                    return;
                }
                if (Vars.Count == 0) 
                {
                    if (Term != null) 
                        pi.AddPrefix(Term + "-");
                    Vars = pi.Vars;
                    return;
                }
                List<Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant> res = new List<Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant>();
                foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in Vars) 
                {
                    foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant vv in pi.Vars) 
                    {
                        Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant vvv = new Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant(string.Format("{0}-{1}", v.Value, vv.Value), v, false);
                        res.Add(vvv);
                    }
                }
                Vars = res;
            }
            public void CorrectLastnameVariants()
            {
                IsLastnameHasStdTail = false;
                foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in Vars) 
                {
                    if (v.Value != null && (((EndsWithStdSurname(v.Value) || Pullenti.Morph.LanguageHelper.EndsWith(v.Value, "АЯ") || Pullenti.Morph.LanguageHelper.EndsWith(v.Value, "ОЙ")) || Pullenti.Morph.LanguageHelper.EndsWith(v.Value, "КИЙ") || Pullenti.Morph.LanguageHelper.EndsWith(v.Value, "ЫЙ")))) 
                    {
                        IsLastnameHasStdTail = true;
                        break;
                    }
                }
                if (IsLastnameHasStdTail) 
                {
                    for (int i = Vars.Count - 1; i >= 0; i--) 
                    {
                        if ((((Vars[i].Value != null && !EndsWithStdSurname(Vars[i].Value) && !Pullenti.Morph.LanguageHelper.EndsWith(Vars[i].Value, "АЯ")) && !Pullenti.Morph.LanguageHelper.EndsWith(Vars[i].Value, "ОЙ") && !Pullenti.Morph.LanguageHelper.EndsWith(Vars[i].Value, "КИЙ")) && !Pullenti.Morph.LanguageHelper.EndsWith(Vars[i].Value, "ЫЙ") && !Pullenti.Morph.LanguageHelper.EndsWith(Vars[i].Value, "ИХ")) && !Pullenti.Morph.LanguageHelper.EndsWith(Vars[i].Value, "ЫХ")) 
                        {
                            Vars.RemoveAt(i);
                            continue;
                        }
                        if (Vars[i].Gender == Pullenti.Morph.MorphGender.Undefined) 
                        {
                            bool del = false;
                            for (int j = 0; j < Vars.Count; j++) 
                            {
                                if (j != i && Vars[j].Value == Vars[i].Value && Vars[j].Gender != Pullenti.Morph.MorphGender.Undefined) 
                                {
                                    del = true;
                                    break;
                                }
                            }
                            if (del) 
                            {
                                Vars.RemoveAt(i);
                                continue;
                            }
                            Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail t = FindTail(Vars[i].Value);
                            if (t != null) 
                            {
                                if (t.Gender != Pullenti.Morph.MorphGender.Undefined) 
                                    Vars[i].Gender = t.Gender;
                            }
                            else if (Pullenti.Morph.LanguageHelper.EndsWithEx(Vars[i].Value, "А", "Я", null, null)) 
                                Vars[i].Gender = Pullenti.Morph.MorphGender.Feminie;
                            else 
                                Vars[i].Gender = Pullenti.Morph.MorphGender.Masculine;
                        }
                    }
                }
            }
            public void RemoveNotGenitive()
            {
                bool hasGen = false;
                foreach (Pullenti.Ner.Person.Internal.PersonItemToken.MorphPersonItemVariant v in Vars) 
                {
                    if (v.Case.IsGenitive) 
                        hasGen = true;
                }
                if (hasGen) 
                {
                    for (int i = Vars.Count - 1; i >= 0; i--) 
                    {
                        if (!Vars[i].Case.IsGenitive) 
                            Vars.RemoveAt(i);
                    }
                }
            }
            public static void Initialize()
            {
                m_LastnameStdTails = new List<Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail>();
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ОВ", Pullenti.Morph.MorphGender.Masculine));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ОВА", Pullenti.Morph.MorphGender.Feminie));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ЕВ", Pullenti.Morph.MorphGender.Masculine));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ЕВА", Pullenti.Morph.MorphGender.Feminie));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ЄВ", Pullenti.Morph.MorphGender.Masculine));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ЄВА", Pullenti.Morph.MorphGender.Feminie));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ИН", Pullenti.Morph.MorphGender.Masculine));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ИНА", Pullenti.Morph.MorphGender.Feminie));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ІН", Pullenti.Morph.MorphGender.Masculine));
                m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail("ІНА", Pullenti.Morph.MorphGender.Feminie));
                foreach (string s in new string[] {"ЕР", "РН", "ДЗЕ", "ВИЛИ", "ЯН", "УК", "ЮК", "КО", "МАН", "АНН", "ЙН", "УН", "СКУ", "СКИ", "СЬКІ", "ИЛО", "ІЛО", "АЛО", "ИК", "СОН", "РА", "НДА", "НДО", "ЕС", "АС", "АВА", "ЛС", "ЛЮС", "ЛЬС", "ЙЗ", "ЕРГ", "ИНГ", "OR", "ER", "OV", "IN", "ERG"}) 
                {
                    m_LastnameStdTails.Add(new Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail(s));
                }
                m_LatsnameSexStdTails = new List<string>(new string[] {"ОВ", "ОВА", "ЕВ", "ЄВ", "ЕВА", "ЄВA", "ИН", "ИНА", "ІН", "ІНА", "КИЙ", "КАЯ"});
                m_LastnameAsian = new List<string>();
                foreach (string s in Pullenti.Ner.Person.Internal.ResourceHelper.GetString("chinasurnames.txt").Split('\n')) 
                {
                    string ss = s.Trim().ToUpper().Replace("Ё", "Е");
                    if (!string.IsNullOrEmpty(ss)) 
                        m_LastnameAsian.Add(ss);
                }
                List<string> m_ChinaSurs = new List<string>("Чон Чжао Цянь Сунь Ли Чжоу У Чжэн Ван Фэн Чэнь Чу Вэй Цзян Шэнь Хань Ян Чжу Цинь Ю Сюй Хэ Люй Ши Чжан Кун Цао Янь Хуа Цзинь Тао Ци Се Цзоу Юй Бай Шуй Доу Чжан Юнь Су Пань Гэ Си Фань Пэн Лан Лу Чан Ма Мяо Фан Жэнь Юань Лю Бао Ши Тан Фэй Лянь Цэнь Сюэ Лэй Хэ Ни Тэн Инь Ло Би Хао Ань Чан Лэ Фу Пи Бянь Кан Бу Гу Мэн Пин Хуан Му Сяо Яо Шао Чжань Мао Ди Ми Бэй Мин Ху Хван".Split(' '));
                foreach (string s in m_ChinaSurs) 
                {
                    string ss = s.Trim().ToUpper().Replace("Ё", "Е");
                    if (!string.IsNullOrEmpty(ss)) 
                    {
                        if (!m_LastnameAsian.Contains(ss)) 
                            m_LastnameAsian.Add(ss);
                    }
                }
                m_LastnameAsian.Sort();
            }
            static List<Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail> m_LastnameStdTails;
            static List<string> m_LatsnameSexStdTails;
            static List<string> m_LastnameAsian;
            static Pullenti.Ner.Person.Internal.PersonItemToken.SurnameTail FindTail(string val)
            {
                if (val == null) 
                    return null;
                for (int i = 0; i < m_LastnameStdTails.Count; i++) 
                {
                    if (Pullenti.Morph.LanguageHelper.EndsWith(val, m_LastnameStdTails[i].Tail)) 
                        return m_LastnameStdTails[i];
                }
                return null;
            }
            public static bool EndsWithStdSurname(string val)
            {
                return FindTail(val) != null;
            }
        }

        class SurnameTail
        {
            public SurnameTail(string t, Pullenti.Morph.MorphGender g = Pullenti.Morph.MorphGender.Undefined)
            {
                Tail = t;
                Gender = g;
            }
            public string Tail;
            public Pullenti.Morph.MorphGender Gender;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0} {1}", Typ.ToString(), Value ?? "");
            if (Firstname != null) 
                res.AppendFormat(" (First: {0})", Firstname.ToString());
            if (Middlename != null) 
                res.AppendFormat(" (Middle: {0})", Middlename.ToString());
            if (Lastname != null) 
                res.AppendFormat(" (Last: {0})", Lastname.ToString());
            if (Referent != null) 
                res.AppendFormat(" Ref: {0}", Referent);
            return res.ToString();
        }
        void AddPostfixInfo(string postfix, Pullenti.Morph.MorphGender gen)
        {
            if (Value != null) 
                Value = string.Format("{0}-{1}", Value, postfix);
            if (Lastname != null) 
                Lastname.AddPostfix(postfix, gen);
            if (Firstname != null) 
                Firstname.AddPostfix(postfix, gen);
            else if (Lastname != null) 
                Firstname = Lastname;
            else 
            {
                Firstname = new MorphPersonItem() { IsHasStdPostfix = true };
                Firstname.Vars.Add(new MorphPersonItemVariant(Value, new Pullenti.Morph.MorphBaseInfo() { Gender = gen }, false));
                if (Lastname == null) 
                    Lastname = Firstname;
            }
            if (Middlename != null) 
                Middlename.AddPostfix(postfix, gen);
            else if (Firstname != null && !Chars.IsLatinLetter) 
                Middlename = Firstname;
            IsInDictionary = false;
        }
        public void MergeWithByHiphen(PersonItemToken pi)
        {
            EndToken = pi.EndToken;
            Value = string.Format("{0}-{1}", Value, pi.Value);
            if (Lastname != null) 
            {
                if (pi.Lastname == null || pi.Lastname.Vars.Count == 0) 
                    Lastname.AddPostfix(pi.Value, Pullenti.Morph.MorphGender.Undefined);
                else 
                    Lastname.MergeWithByHiphen(pi.Lastname);
            }
            else if (pi.Lastname != null) 
            {
                pi.Lastname.AddPrefix(Value + "-");
                Lastname = pi.Lastname;
            }
            if (Firstname != null) 
            {
                if (pi.Firstname == null || pi.Firstname.Vars.Count == 0) 
                    Firstname.AddPostfix(pi.Value, Pullenti.Morph.MorphGender.Undefined);
                else 
                    Firstname.MergeWithByHiphen(pi.Firstname);
            }
            else if (pi.Firstname != null) 
            {
                pi.Firstname.AddPrefix(Value + "-");
                Firstname = pi.Firstname;
            }
            if (Middlename != null) 
            {
                if (pi.Middlename == null || pi.Middlename.Vars.Count == 0) 
                    Middlename.AddPostfix(pi.Value, Pullenti.Morph.MorphGender.Undefined);
                else 
                    Middlename.MergeWithByHiphen(pi.Middlename);
            }
            else if (pi.Middlename != null) 
            {
                pi.Middlename.AddPrefix(Value + "-");
                Middlename = pi.Middlename;
            }
        }
        public void RemoveNotGenitive()
        {
            if (Lastname != null) 
                Lastname.RemoveNotGenitive();
            if (Firstname != null) 
                Firstname.RemoveNotGenitive();
            if (Middlename != null) 
                Middlename.RemoveNotGenitive();
        }
        public static PersonItemToken TryAttachLatin(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
            {
                Pullenti.Ner.MetaToken mt = t as Pullenti.Ner.MetaToken;
                if (mt != null && mt.BeginToken == mt.EndToken) 
                {
                    PersonItemToken res00 = TryAttachLatin(mt.BeginToken);
                    if (res00 != null) 
                    {
                        res00.BeginToken = (res00.EndToken = t);
                        return res00;
                    }
                }
                return null;
            }
            if (!tt.Chars.IsLetter) 
                return null;
            if (tt.Term == "THE") 
                return null;
            if (tt.Term == "JR" || tt.Term == "JNR" || tt.Term == "JUNIOR") 
            {
                Pullenti.Ner.Token t1 = (Pullenti.Ner.Token)tt;
                if (tt.Next != null && tt.Next.IsChar('.')) 
                    t1 = tt.Next;
                return new PersonItemToken(tt, t1) { Typ = ItemType.Suffix, Value = "JUNIOR" };
            }
            if ((tt.Term == "SR" || tt.Term == "SNR" || tt.Term == "SENIOR") || tt.Term == "FITZ" || tt.Term == "FILS") 
            {
                Pullenti.Ner.Token t1 = (Pullenti.Ner.Token)tt;
                if (tt.Next != null && tt.Next.IsChar('.')) 
                    t1 = tt.Next;
                return new PersonItemToken(tt, t1) { Typ = ItemType.Suffix, Value = "SENIOR" };
            }
            bool initials = (tt.Term == "YU" || tt.Term == "YA" || tt.Term == "CH") || tt.Term == "SH";
            if (!initials && tt.Term.Length == 2 && tt.Chars.IsCapitalUpper) 
            {
                if (!Pullenti.Morph.LanguageHelper.IsLatinVowel(tt.Term[0]) && !Pullenti.Morph.LanguageHelper.IsLatinVowel(tt.Term[1])) 
                    initials = true;
            }
            if (initials) 
            {
                PersonItemToken rii = new PersonItemToken(tt, tt) { Typ = ItemType.Initial, Value = tt.Term, Chars = tt.Chars };
                if (tt.Next != null && tt.Next.IsChar('.')) 
                    rii.EndToken = tt.Next;
                return rii;
            }
            if (tt.Chars.IsAllLower) 
            {
                if (!m_SurPrefixesLat.Contains(tt.Term)) 
                    return null;
            }
            if (tt.Chars.IsCyrillicLetter) 
                return null;
            if (tt.LengthChar == 1) 
            {
                if (tt.Next == null) 
                    return null;
                if (tt.Next.IsChar('.')) 
                    return new PersonItemToken(tt, tt.Next) { Typ = ItemType.Initial, Value = tt.Term, Chars = tt.Chars };
                if (!tt.Next.IsWhitespaceAfter && !tt.IsWhitespaceAfter && ((tt.Term == "D" || tt.Term == "O" || tt.Term == "M"))) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt.Next, false) && (tt.Next.Next is Pullenti.Ner.TextToken)) 
                    {
                        if (tt.Next.Next.Chars.IsLatinLetter) 
                        {
                            PersonItemToken pit0 = TryAttachLatin(tt.Next.Next);
                            if (pit0 != null && pit0.Typ == ItemType.Value) 
                            {
                                pit0.BeginToken = tt;
                                string val = tt.Term;
                                if (pit0.Value != null) 
                                {
                                    if (val == "M" && pit0.Value.StartsWith("C")) 
                                    {
                                        pit0.Value = "MA" + pit0.Value;
                                        val = "MA";
                                    }
                                    else 
                                        pit0.Value = val + pit0.Value;
                                }
                                if (pit0.Lastname != null) 
                                {
                                    pit0.Lastname.AddPrefix(val);
                                    pit0.Lastname.IsInDictionary = true;
                                }
                                else if (pit0.Firstname != null) 
                                {
                                    pit0.Lastname = pit0.Firstname;
                                    pit0.Lastname.AddPrefix(val);
                                    pit0.Lastname.IsInDictionary = true;
                                }
                                pit0.Firstname = (pit0.Middlename = null);
                                if (!pit0.Chars.IsAllUpper && !pit0.Chars.IsCapitalUpper) 
                                    pit0.Chars.IsCapitalUpper = true;
                                return pit0;
                            }
                        }
                    }
                }
                if (!Pullenti.Morph.LanguageHelper.IsLatinVowel(tt.Term[0]) || tt.WhitespacesAfterCount != 1) 
                {
                    PersonItemToken nex = TryAttachLatin(tt.Next);
                    if (nex != null && nex.Typ == ItemType.Value) 
                        return new PersonItemToken(tt, tt) { Typ = ItemType.Initial, Value = tt.Term, Chars = tt.Chars };
                    return null;
                }
                if (tt.Term == "I") 
                    return null;
                return new PersonItemToken(tt, tt) { Typ = ItemType.Value, Value = tt.Term, Chars = tt.Chars };
            }
            if (!Pullenti.Ner.Core.MiscHelper.HasVowel(tt)) 
                return null;
            PersonItemToken res;
            if (m_SurPrefixesLat.Contains(tt.Term)) 
            {
                Pullenti.Ner.Token te = tt.Next;
                if (te != null && te.IsHiphen) 
                    te = te.Next;
                res = TryAttachLatin(te);
                if (res != null) 
                {
                    res.Value = string.Format("{0}-{1}", tt.Term, res.Value);
                    res.BeginToken = tt;
                    res.Lastname = new MorphPersonItem();
                    res.Lastname.Vars.Add(new MorphPersonItemVariant(res.Value, new Pullenti.Morph.MorphBaseInfo(), true));
                    res.Lastname.IsLastnameHasHiphen = true;
                    return res;
                }
            }
            if (Pullenti.Ner.Mail.Internal.MailLine.IsKeyword(tt)) 
                return null;
            res = new PersonItemToken(tt, tt);
            res.Value = tt.Term;
            Pullenti.Morph.MorphClass cla = tt.GetMorphClassInDictionary();
            if (cla.IsProperName || ((cla.IsProper && ((tt.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || tt.Morph.Gender == Pullenti.Morph.MorphGender.Feminie))))) 
            {
                res.Firstname = new MorphPersonItem() { Term = res.Value };
                foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
                {
                    if ((wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    {
                        if (wf.Class.IsProperName) 
                            res.Firstname.Vars.Add(new MorphPersonItemVariant(res.Value, wf, false));
                    }
                }
                if (res.Firstname.Vars.Count == 0) 
                    res.Firstname.Vars.Add(new MorphPersonItemVariant(res.Value, null, false));
                res.Firstname.IsInDictionary = true;
            }
            if (cla.IsProperSurname) 
            {
                res.Lastname = new MorphPersonItem() { Term = res.Value };
                foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
                {
                    if ((wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    {
                        if (wf.Class.IsProperSurname) 
                            res.Lastname.Vars.Add(new MorphPersonItemVariant(res.Value, wf, false));
                    }
                }
                if (res.Lastname.Vars.Count == 0) 
                    res.Lastname.Vars.Add(new MorphPersonItemVariant(res.Value, null, false));
                res.Lastname.IsInDictionary = true;
            }
            if ((!cla.IsProperName && !cla.IsProper && !cla.IsProperSurname) && !cla.IsUndefined) 
                res.IsInDictionary = true;
            res.Morph = tt.Morph;
            List<Pullenti.Ner.Core.IntOntologyToken> ots = null;
            if (t != null && t.Kit.Ontology != null && ots == null) 
                ots = t.Kit.Ontology.AttachToken(Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME, t);
            if (ots != null) 
            {
                if (ots[0].Termin.IgnoreTermsOrder) 
                    return new PersonItemToken(ots[0].BeginToken, ots[0].EndToken) { Typ = ItemType.Referent, Referent = ots[0].Item.Tag as Pullenti.Ner.Person.PersonReferent, Morph = ots[0].Morph };
                res.Lastname = new MorphPersonItem() { Term = ots[0].Termin.CanonicText, IsInOntology = true };
                foreach (Pullenti.Ner.Core.IntOntologyToken ot in ots) 
                {
                    if (ot.Termin != null) 
                    {
                        Pullenti.Morph.MorphBaseInfo mi = (Pullenti.Morph.MorphBaseInfo)ot.Morph;
                        if (ot.Termin.Gender == Pullenti.Morph.MorphGender.Masculine || ot.Termin.Gender == Pullenti.Morph.MorphGender.Feminie) 
                            mi = new Pullenti.Morph.MorphBaseInfo() { Gender = ot.Termin.Gender };
                        res.Lastname.Vars.Add(new MorphPersonItemVariant(ot.Termin.CanonicText, mi, true));
                    }
                }
            }
            if (res.Value.StartsWith("MC")) 
                res.Value = "MAC" + res.Value.Substring(2);
            if (res.Value.StartsWith("MAC")) 
            {
                res.Firstname = (res.Middlename = null);
                res.Lastname = new MorphPersonItem() { IsHasStdPostfix = true };
                res.Lastname.Vars.Add(new MorphPersonItemVariant(res.Value, new Pullenti.Morph.MorphBaseInfo(), true));
            }
            return res;
        }
        public static PersonItemToken TryAttach(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locOnt, ParseAttr attrs = ParseAttr.No, List<PersonItemToken> prevList = null)
        {
            if (t == null) 
                return null;
            if (t is Pullenti.Ner.TextToken) 
            {
                Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                if (mc.IsPreposition || mc.IsConjunction || mc.IsMisc) 
                {
                    if (t.Next != null && (t.Next is Pullenti.Ner.ReferentToken)) 
                    {
                        if (((attrs & ParseAttr.MustBeItemAlways)) != ParseAttr.No && !t.Chars.IsAllLower) 
                        {
                        }
                        else 
                            return null;
                    }
                }
            }
            if (t is Pullenti.Ner.NumberToken) 
            {
                Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                if (nt.BeginToken == nt.EndToken && nt.Typ == Pullenti.Ner.NumberSpellingType.Words && ((!nt.BeginToken.Chars.IsAllLower || ((attrs & ParseAttr.MustBeItemAlways)) != ParseAttr.No))) 
                {
                    PersonItemToken res00 = TryAttach(nt.BeginToken, locOnt, attrs, prevList);
                    if (res00 != null) 
                    {
                        res00.BeginToken = (res00.EndToken = t);
                        return res00;
                    }
                }
            }
            if (t is Pullenti.Ner.ReferentToken) 
            {
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                if (rt.BeginToken == rt.EndToken && rt.BeginToken.Chars.IsCapitalUpper) 
                {
                    PersonItemToken res00 = TryAttach(rt.BeginToken, locOnt, attrs, prevList);
                    if (res00 != null) 
                    {
                        PersonItemToken res01 = TryAttach(t.Next, locOnt, attrs, prevList);
                        if (res01 != null && res01.Lastname != null && res01.Firstname == null) 
                            return null;
                        res00.BeginToken = (res00.EndToken = t);
                        return res00;
                    }
                }
            }
            if ((((t is Pullenti.Ner.TextToken) && t.LengthChar == 2 && (t as Pullenti.Ner.TextToken).Term == "JI") && t.Chars.IsAllUpper && !t.IsWhitespaceAfter) && t.Next != null && t.Next.IsChar('.')) 
            {
                PersonItemToken re1 = new PersonItemToken(t, t.Next) { Typ = ItemType.Initial, Value = "Л" };
                re1.Chars.IsCyrillicLetter = true;
                re1.Chars.IsAllUpper = true;
                return re1;
            }
            if ((((((t is Pullenti.Ner.TextToken) && t.LengthChar == 1 && (t as Pullenti.Ner.TextToken).Term == "J") && t.Chars.IsAllUpper && !t.IsWhitespaceAfter) && (t.Next is Pullenti.Ner.NumberToken) && (t.Next as Pullenti.Ner.NumberToken).Value == "1") && (t.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit && t.Next.Next != null) && t.Next.Next.IsChar('.')) 
            {
                PersonItemToken re1 = new PersonItemToken(t, t.Next.Next) { Typ = ItemType.Initial, Value = "Л" };
                re1.Chars.IsCyrillicLetter = true;
                re1.Chars.IsAllUpper = true;
                return re1;
            }
            if ((((((t is Pullenti.Ner.TextToken) && t.LengthChar == 1 && (t as Pullenti.Ner.TextToken).Term == "I") && t.Chars.IsAllUpper && !t.IsWhitespaceAfter) && (t.Next is Pullenti.Ner.NumberToken) && (t.Next as Pullenti.Ner.NumberToken).Value == "1") && (t.Next as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit && t.Next.Next != null) && t.Next.Next.IsChar('.')) 
            {
                if (prevList != null && prevList[0].Chars.IsCyrillicLetter) 
                {
                    PersonItemToken re1 = new PersonItemToken(t, t.Next.Next) { Typ = ItemType.Initial, Value = "П" };
                    re1.Chars.IsCyrillicLetter = true;
                    re1.Chars.IsAllUpper = true;
                    return re1;
                }
            }
            if (locOnt != null && locOnt.Items.Count > 1000) 
                locOnt = null;
            PersonItemToken res = _tryAttach(t, locOnt, attrs, prevList);
            if (res != null) 
                return res;
            if (t.Chars.IsLatinLetter && ((attrs & ParseAttr.CanBeLatin)) != ParseAttr.No) 
            {
                List<Pullenti.Ner.Core.IntOntologyToken> ots = null;
                if (locOnt != null) 
                    ots = locOnt.TryAttach(t, Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME, false);
                if (t != null && t.Kit.Ontology != null && ots == null) 
                    ots = t.Kit.Ontology.AttachToken(Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME, t);
                if (ots != null && (t is Pullenti.Ner.TextToken)) 
                {
                    if (ots[0].Termin.IgnoreTermsOrder) 
                        return new PersonItemToken(ots[0].BeginToken, ots[0].EndToken) { Typ = ItemType.Referent, Referent = ots[0].Item.Tag as Pullenti.Ner.Person.PersonReferent, Morph = ots[0].Morph };
                    res = new PersonItemToken(ots[0].BeginToken, ots[0].EndToken) { Value = (t as Pullenti.Ner.TextToken).Term, Chars = ots[0].Chars };
                    res.Lastname = new MorphPersonItem() { Term = ots[0].Termin.CanonicText, IsInOntology = true };
                    foreach (Pullenti.Ner.Core.IntOntologyToken ot in ots) 
                    {
                        if (ot.Termin != null) 
                        {
                            Pullenti.Morph.MorphBaseInfo mi = (Pullenti.Morph.MorphBaseInfo)ot.Morph;
                            if (ot.Termin.Gender == Pullenti.Morph.MorphGender.Masculine || ot.Termin.Gender == Pullenti.Morph.MorphGender.Feminie) 
                                mi = new Pullenti.Morph.MorphBaseInfo() { Gender = ot.Termin.Gender };
                            res.Lastname.Vars.Add(new MorphPersonItemVariant(ot.Termin.CanonicText, mi, true));
                        }
                    }
                    return res;
                }
                res = TryAttachLatin(t);
                if (res != null) 
                    return res;
            }
            if (((t is Pullenti.Ner.NumberToken) && t.LengthChar == 1 && ((attrs & ParseAttr.CanInitialBeDigit)) != ParseAttr.No) && t.Next != null && t.Next.IsCharOf(".„")) 
            {
                if ((t as Pullenti.Ner.NumberToken).Value == "1") 
                    return new PersonItemToken(t, t.Next) { Typ = ItemType.Initial, Value = "І", Chars = new Pullenti.Morph.CharsInfo() { IsCyrillicLetter = true } };
                if ((t as Pullenti.Ner.NumberToken).Value == "0") 
                    return new PersonItemToken(t, t.Next) { Typ = ItemType.Initial, Value = "О", Chars = new Pullenti.Morph.CharsInfo() { IsCyrillicLetter = true } };
                if ((t as Pullenti.Ner.NumberToken).Value == "3") 
                    return new PersonItemToken(t, t.Next) { Typ = ItemType.Initial, Value = "З", Chars = new Pullenti.Morph.CharsInfo() { IsCyrillicLetter = true } };
            }
            if ((((t is Pullenti.Ner.NumberToken) && t.LengthChar == 1 && ((attrs & ParseAttr.CanInitialBeDigit)) != ParseAttr.No) && t.Next != null && t.Next.Chars.IsAllLower) && !t.IsWhitespaceAfter && t.Next.LengthChar > 2) 
            {
                string num = (t as Pullenti.Ner.NumberToken).Value;
                if (num == "3" && t.Next.Chars.IsCyrillicLetter) 
                    return new PersonItemToken(t, t.Next) { Typ = ItemType.Value, Value = "З" + (t.Next as Pullenti.Ner.TextToken).Term, Chars = new Pullenti.Morph.CharsInfo() { IsCyrillicLetter = true, IsCapitalUpper = true } };
                if (num == "0" && t.Next.Chars.IsCyrillicLetter) 
                    return new PersonItemToken(t, t.Next) { Typ = ItemType.Value, Value = "О" + (t.Next as Pullenti.Ner.TextToken).Term, Chars = new Pullenti.Morph.CharsInfo() { IsCyrillicLetter = true, IsCapitalUpper = true } };
            }
            if (((((t is Pullenti.Ner.TextToken) && t.LengthChar == 1 && t.Chars.IsLetter) && t.Chars.IsAllUpper && (t.WhitespacesAfterCount < 2)) && (t.Next is Pullenti.Ner.TextToken) && t.Next.LengthChar == 1) && t.Next.Chars.IsAllLower) 
            {
                int cou = 0;
                Pullenti.Ner.Token t1 = null;
                int lat = 0;
                int cyr = 0;
                char ch = (t as Pullenti.Ner.TextToken).GetSourceText()[0];
                if (t.Chars.IsCyrillicLetter) 
                {
                    cyr++;
                    if (Pullenti.Morph.LanguageHelper.GetLatForCyr(ch) != 0) 
                        lat++;
                }
                else 
                {
                    lat++;
                    if (Pullenti.Morph.LanguageHelper.GetCyrForLat(ch) != 0) 
                        cyr++;
                }
                for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.WhitespacesBeforeCount > 1) 
                        break;
                    if (!(tt is Pullenti.Ner.TextToken) || tt.LengthChar != 1 || !tt.Chars.IsAllLower) 
                        break;
                    t1 = tt;
                    cou++;
                    ch = (tt as Pullenti.Ner.TextToken).GetSourceText()[0];
                    if (tt.Chars.IsCyrillicLetter) 
                    {
                        cyr++;
                        if (Pullenti.Morph.LanguageHelper.GetLatForCyr(ch) != 0) 
                            lat++;
                    }
                    else 
                    {
                        lat++;
                        if (Pullenti.Morph.LanguageHelper.GetCyrForLat(ch) != 0) 
                            cyr++;
                    }
                }
                if (cou < 2) 
                    return null;
                if (cou < 5) 
                {
                    if (prevList != null && prevList.Count > 0 && prevList[prevList.Count - 1].Typ == ItemType.Initial) 
                    {
                    }
                    else 
                    {
                        PersonItemToken ne = TryAttach(t1.Next, locOnt, attrs, null);
                        if (ne == null || ne.Typ != ItemType.Initial) 
                            return null;
                    }
                }
                bool isCyr = cyr >= lat;
                if (cyr == lat && t.Chars.IsLatinLetter) 
                    isCyr = false;
                StringBuilder val = new StringBuilder();
                for (Pullenti.Ner.Token tt = t; tt != null && tt.EndChar <= t1.EndChar; tt = tt.Next) 
                {
                    ch = (tt as Pullenti.Ner.TextToken).GetSourceText()[0];
                    if (isCyr && Pullenti.Morph.LanguageHelper.IsLatinChar(ch)) 
                    {
                        char chh = Pullenti.Morph.LanguageHelper.GetCyrForLat(ch);
                        if (chh != 0) 
                            ch = chh;
                    }
                    else if (!isCyr && Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch)) 
                    {
                        char chh = Pullenti.Morph.LanguageHelper.GetLatForCyr(ch);
                        if (chh != 0) 
                            ch = chh;
                    }
                    val.Append(char.ToUpper(ch));
                }
                res = new PersonItemToken(t, t1) { Typ = ItemType.Value, Value = val.ToString() };
                res.Chars = new Pullenti.Morph.CharsInfo() { IsCapitalUpper = true, IsCyrillicLetter = isCyr, IsLatinLetter = !isCyr, IsLetter = true };
                return res;
            }
            if (((attrs & ParseAttr.MustBeItemAlways)) != ParseAttr.No && (t is Pullenti.Ner.TextToken) && !t.Chars.IsAllLower) 
            {
                res = new PersonItemToken(t, t) { Value = (t as Pullenti.Ner.TextToken).Term };
                return res;
            }
            if (((t.Chars.IsAllUpper && t.LengthChar == 1 && prevList != null) && prevList.Count > 0 && (t.WhitespacesBeforeCount < 2)) && prevList[0].Chars.IsCapitalUpper) 
            {
                PersonItemToken last = prevList[prevList.Count - 1];
                bool ok = false;
                if ((last.Typ == ItemType.Value && last.Lastname != null && last.Lastname.IsInDictionary) && prevList.Count == 1) 
                    ok = true;
                else if (prevList.Count == 2 && last.Typ == ItemType.Initial && prevList[0].Lastname != null) 
                    ok = true;
                if (ok) 
                    return new PersonItemToken(t, t) { Value = (t as Pullenti.Ner.TextToken).Term, Typ = ItemType.Initial };
            }
            return null;
        }
        static PersonItemToken _tryAttach(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locOnt, ParseAttr attrs, List<PersonItemToken> prevList = null)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
            {
                if (t.Chars.IsLetter && t.Chars.IsCapitalUpper && (t is Pullenti.Ner.ReferentToken)) 
                {
                    Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                    if (rt.BeginToken == rt.EndToken && !(rt.Referent is Pullenti.Ner.Person.PersonReferent)) 
                    {
                        PersonItemToken res0 = _tryAttach(rt.BeginToken, locOnt, attrs, null);
                        if (res0 == null) 
                        {
                            res0 = new PersonItemToken(rt, rt) { Value = rt.Referent.ToString(true, t.Kit.BaseLanguage, 0).ToUpper(), Chars = rt.Chars, Morph = rt.Morph };
                            res0.Lastname = new MorphPersonItem() { Term = res0.Value };
                        }
                        else 
                            res0.BeginToken = (res0.EndToken = rt);
                        if ((t.Next != null && t.Next.IsHiphen && (t.Next.Next is Pullenti.Ner.TextToken)) && t.Next.Next.GetMorphClassInDictionary().IsProperSecname) 
                        {
                            PersonItemToken res1 = TryAttach(t.Next.Next, locOnt, ParseAttr.No, null);
                            if (res1 != null && res1.Middlename != null) 
                            {
                                res1.Middlename.AddPrefix(res0.Value + "-");
                                res1.Firstname = res1.Middlename;
                                res1.BeginToken = t;
                                return res1;
                            }
                        }
                        return res0;
                    }
                }
                return null;
            }
            if (!tt.Chars.IsLetter) 
                return null;
            bool canBeAllLower = false;
            if (tt.Chars.IsAllLower && ((attrs & ParseAttr.CanBeLower)) == ParseAttr.No) 
            {
                if (!m_SurPrefixes.Contains(tt.Term)) 
                {
                    Pullenti.Morph.MorphClass mc0 = tt.GetMorphClassInDictionary();
                    if (((tt.Term == "Д" && !tt.IsWhitespaceAfter && Pullenti.Ner.Core.BracketHelper.IsBracket(tt.Next, true)) && !tt.Next.IsWhitespaceAfter && (tt.Next.Next is Pullenti.Ner.TextToken)) && tt.Next.Next.Chars.IsCapitalUpper) 
                    {
                    }
                    else if (mc0.IsProperSurname && !mc0.IsNoun) 
                    {
                        if (tt.Next != null && (tt.WhitespacesAfterCount < 3)) 
                        {
                            Pullenti.Morph.MorphClass mc1 = tt.Next.GetMorphClassInDictionary();
                            if (mc1.IsProperName) 
                                canBeAllLower = true;
                        }
                        if (tt.Previous != null && (tt.WhitespacesBeforeCount < 3)) 
                        {
                            Pullenti.Morph.MorphClass mc1 = tt.Previous.GetMorphClassInDictionary();
                            if (mc1.IsProperName) 
                                canBeAllLower = true;
                        }
                        if (!canBeAllLower) 
                            return null;
                    }
                    else if (mc0.IsProperSecname && !mc0.IsNoun) 
                    {
                        if (tt.Previous != null && (tt.WhitespacesBeforeCount < 3)) 
                        {
                            Pullenti.Morph.MorphClass mc1 = tt.Previous.GetMorphClassInDictionary();
                            if (mc1.IsProperName) 
                                canBeAllLower = true;
                        }
                        if (!canBeAllLower) 
                            return null;
                    }
                    else if (mc0.IsProperName && !mc0.IsNoun) 
                    {
                        if (tt.Next != null && (tt.WhitespacesAfterCount < 3)) 
                        {
                            Pullenti.Morph.MorphClass mc1 = tt.Next.GetMorphClassInDictionary();
                            if (mc1.IsProperSurname || mc1.IsProperSecname) 
                                canBeAllLower = true;
                        }
                        if (tt.Previous != null && (tt.WhitespacesBeforeCount < 3)) 
                        {
                            Pullenti.Morph.MorphClass mc1 = tt.Previous.GetMorphClassInDictionary();
                            if (mc1.IsProperSurname) 
                                canBeAllLower = true;
                        }
                        if (!canBeAllLower) 
                            return null;
                    }
                    else 
                        return null;
                }
            }
            if (tt.LengthChar == 1 || tt.Term == "ДЖ") 
            {
                if (tt.Next == null) 
                    return null;
                string ini = tt.Term;
                Pullenti.Morph.CharsInfo ci = new Pullenti.Morph.CharsInfo() { Value = tt.Chars.Value };
                if (!tt.Chars.IsCyrillicLetter) 
                {
                    char cyr = Pullenti.Morph.LanguageHelper.GetCyrForLat(ini[0]);
                    if (cyr == ((char)0)) 
                        return null;
                    ini = string.Format("{0}", cyr);
                    ci.IsLatinLetter = false;
                    ci.IsCyrillicLetter = true;
                }
                if (tt.Next.IsChar('.')) 
                    return new PersonItemToken(tt, tt.Next) { Typ = ItemType.Initial, Value = ini, Chars = ci };
                if ((tt.Next.IsCharOf(",;„") && prevList != null && prevList.Count > 0) && prevList[prevList.Count - 1].Typ == ItemType.Initial) 
                    return new PersonItemToken(tt, tt) { Typ = ItemType.Initial, Value = ini, Chars = ci };
                if ((tt.Next.WhitespacesAfterCount < 2) && (tt.WhitespacesAfterCount < 2) && ((tt.Term == "Д" || tt.Term == "О" || tt.Term == "Н"))) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt.Next, false) && (tt.Next.Next is Pullenti.Ner.TextToken)) 
                    {
                        if (tt.Next.Next.Chars.IsCyrillicLetter) 
                        {
                            PersonItemToken pit0 = TryAttach(tt.Next.Next, locOnt, attrs | ParseAttr.CanBeLower, prevList);
                            if (pit0 != null) 
                            {
                                pit0.BeginToken = tt;
                                if (pit0.Value != null) 
                                    pit0.Value = ini + pit0.Value;
                                if (pit0.Lastname != null) 
                                {
                                    pit0.Lastname.AddPrefix(ini);
                                    pit0.Lastname.IsInDictionary = true;
                                }
                                else if (pit0.Firstname != null) 
                                {
                                    pit0.Lastname = pit0.Firstname;
                                    pit0.Lastname.AddPrefix(ini);
                                    pit0.Lastname.IsInDictionary = true;
                                }
                                pit0.Firstname = (pit0.Middlename = null);
                                if (!pit0.Chars.IsAllUpper && !pit0.Chars.IsCapitalUpper) 
                                    pit0.Chars.IsCapitalUpper = true;
                                return pit0;
                            }
                        }
                    }
                }
                if (!Pullenti.Morph.LanguageHelper.IsCyrillicVowel(tt.Term[0])) 
                    return null;
                if (tt.WhitespacesAfterCount != 1) 
                {
                    if (tt.Next == null) 
                    {
                    }
                    else if ((!tt.IsWhitespaceAfter && (tt.Next is Pullenti.Ner.TextToken) && !tt.Next.IsChar('.')) && !tt.Next.Chars.IsLetter) 
                    {
                    }
                    else 
                        return null;
                }
                return new PersonItemToken(tt, tt) { Typ = ItemType.Value, Value = tt.Term, Chars = tt.Chars };
            }
            if (!tt.Chars.IsCyrillicLetter) 
                return null;
            if (!Pullenti.Ner.Core.MiscHelper.HasVowel(tt)) 
                return null;
            List<Pullenti.Ner.Core.IntOntologyToken> ots = null;
            if (locOnt != null) 
                ots = locOnt.TryAttach(t, Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME, false);
            if (t != null && t.Kit.Ontology != null && ots == null) 
                ots = t.Kit.Ontology.AttachToken(Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME, t);
            string surPrefix = null;
            PersonItemToken res = null;
            if (ots != null) 
            {
                if (ots[0].Termin.IgnoreTermsOrder) 
                    return new PersonItemToken(ots[0].BeginToken, ots[0].EndToken) { Typ = ItemType.Referent, Referent = ots[0].Item.Tag as Pullenti.Ner.Person.PersonReferent, Morph = ots[0].Morph };
                Pullenti.Morph.MorphClass mc = ots[0].BeginToken.GetMorphClassInDictionary();
                if (ots[0].BeginToken == ots[0].EndToken && mc.IsProperName && !mc.IsProperSurname) 
                    ots = null;
            }
            if (ots != null) 
            {
                res = new PersonItemToken(ots[0].BeginToken, ots[0].EndToken) { Value = tt.Term, Chars = ots[0].Chars };
                res.Lastname = new MorphPersonItem() { IsInOntology = true };
                res.Lastname.Term = ots[0].Termin.CanonicText;
                foreach (Pullenti.Ner.Core.IntOntologyToken ot in ots) 
                {
                    if (ot.Termin != null) 
                    {
                        Pullenti.Morph.MorphBaseInfo mi = (Pullenti.Morph.MorphBaseInfo)ot.Morph;
                        if (ot.Termin.Gender == Pullenti.Morph.MorphGender.Masculine) 
                        {
                            if (((t.Morph.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined) 
                                continue;
                            mi = new Pullenti.Morph.MorphBaseInfo() { Gender = ot.Termin.Gender };
                        }
                        else if (ot.Termin.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        {
                            if (((t.Morph.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                                continue;
                            mi = new Pullenti.Morph.MorphBaseInfo() { Gender = ot.Termin.Gender };
                        }
                        else 
                            continue;
                        res.Lastname.Vars.Add(new MorphPersonItemVariant(ot.Termin.CanonicText, mi, true));
                    }
                }
                if (ots[0].Termin.CanonicText.Contains("-")) 
                    return res;
            }
            else 
            {
                res = new PersonItemToken(t, t) { Value = tt.Term, Chars = tt.Chars, Morph = tt.Morph };
                if (m_SurPrefixes.Contains(tt.Term)) 
                {
                    if (((tt.IsValue("БЕН", null) || tt.IsValue("ВАН", null))) && ((attrs & ParseAttr.AltVar)) != ParseAttr.No && ((tt.Next == null || !tt.Next.IsHiphen))) 
                    {
                    }
                    else 
                    {
                        if (tt.Next != null) 
                        {
                            Pullenti.Ner.Token t1 = tt.Next;
                            if (t1.IsHiphen) 
                                tt = t1.Next as Pullenti.Ner.TextToken;
                            else if (((attrs & ParseAttr.SurnamePrefixNotMerge)) != ParseAttr.No && t1.Chars.IsAllLower) 
                                tt = null;
                            else 
                                tt = t1 as Pullenti.Ner.TextToken;
                            if ((tt == null || tt.IsNewlineBefore || tt.Chars.IsAllLower) || !tt.Chars.IsCyrillicLetter || (tt.LengthChar < 3)) 
                            {
                            }
                            else 
                            {
                                surPrefix = res.Value;
                                res.Value = string.Format("{0}-{1}", res.Value, tt.Term);
                                res.Morph = tt.Morph;
                                res.Chars = tt.Chars;
                                res.EndToken = tt;
                            }
                        }
                        if (surPrefix == null) 
                        {
                            if (t.Chars.IsCapitalUpper || t.Chars.IsAllUpper) 
                                return res;
                            return null;
                        }
                    }
                }
            }
            if (tt.IsValue("ФАМИЛИЯ", "ПРІЗВИЩЕ") || tt.IsValue("ИМЯ", "ІМЯ") || tt.IsValue("ОТЧЕСТВО", "БАТЬКОВІ")) 
                return null;
            if (tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) 
            {
                if (tt.GetMorphClassInDictionary().IsProperName) 
                {
                }
                else if (tt.Next == null || !tt.Next.IsChar('.')) 
                {
                    if (tt.LengthChar > 1 && tt.Chars.IsCapitalUpper && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                    {
                    }
                    else 
                        return null;
                }
            }
            if (((attrs & ParseAttr.MustBeItemAlways)) != ParseAttr.No) 
            {
            }
            else 
            {
                if (tt.Term.Length > 6 && tt.Term.StartsWith("ЗД")) 
                {
                    if (Pullenti.Ner.Core.MiscHelper.IsNotMoreThanOneError("ЗДРАВСТВУЙТЕ", tt)) 
                        return null;
                    if (Pullenti.Ner.Core.MiscHelper.IsNotMoreThanOneError("ЗДРАВСТВУЙ", tt)) 
                        return null;
                }
                if (tt.LengthChar > 6 && tt.Term.StartsWith("ПР")) 
                {
                    if (Pullenti.Ner.Core.MiscHelper.IsNotMoreThanOneError("ПРИВЕТСТВУЮ", tt)) 
                        return null;
                }
                if (tt.LengthChar > 6 && tt.Term.StartsWith("УВ")) 
                {
                    if (tt.IsValue("УВАЖАЕМЫЙ", null)) 
                        return null;
                }
                if (tt.LengthChar > 6 && tt.Term.StartsWith("ДО")) 
                {
                    if (tt.IsValue("ДОРОГОЙ", null)) 
                        return null;
                }
            }
            if (!tt.Chars.IsAllUpper && !tt.Chars.IsCapitalUpper && !canBeAllLower) 
            {
                if (((attrs & ParseAttr.CanInitialBeDigit)) != ParseAttr.No && !tt.Chars.IsAllLower) 
                {
                }
                else if (((attrs & ParseAttr.CanBeLower)) == ParseAttr.No) 
                    return null;
            }
            Pullenti.Morph.MorphWordForm adj = null;
            foreach (Pullenti.Morph.MorphBaseInfo wff in tt.Morph.Items) 
            {
                Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                if (wf == null) 
                    continue;
                if (wf.Class.IsAdjective && wf.ContainsAttr("к.ф.", null)) 
                {
                    if (wf.IsInDictionary) 
                    {
                        if (Pullenti.Morph.LanguageHelper.EndsWith(tt.Term, "НО") || ((tt.Next != null && tt.Next.IsHiphen))) 
                            res.IsInDictionary = true;
                    }
                    continue;
                }
                else if ((wf.Class.IsAdjective && adj == null && !((wf.NormalFull ?? wf.NormalCase)).EndsWith("ОВ")) && !((wf.NormalFull ?? wf.NormalCase)).EndsWith("ИН") && (((wf.IsInDictionary || wf.NormalCase.EndsWith("ЫЙ") || wf.NormalCase.EndsWith("КИЙ")) || wf.NormalCase.EndsWith("АЯ") || wf.NormalCase.EndsWith("ЯЯ")))) 
                    adj = wf;
                if (wf.Class.IsVerb) 
                {
                    if (wf.IsInDictionary) 
                        res.IsInDictionary = true;
                    continue;
                }
                if (wf.IsInDictionary) 
                {
                    if ((wf.Class.IsAdverb || wf.Class.IsPreposition || wf.Class.IsConjunction) || wf.Class.IsPronoun || wf.Class.IsPersonalPronoun) 
                        res.IsInDictionary = true;
                }
                if (wf.Class.IsProperSurname || surPrefix != null) 
                {
                    if (res.Lastname == null) 
                        res.Lastname = new MorphPersonItem() { Term = tt.Term };
                    if (adj != null) 
                    {
                        if (!wf.IsInDictionary && adj.Number == Pullenti.Morph.MorphNumber.Singular) 
                        {
                            string val = adj.NormalCase;
                            res.Lastname.Vars.Add(new MorphPersonItemVariant(val, adj, true));
                            if (val == tt.Term) 
                                break;
                        }
                        adj = null;
                    }
                    if (((attrs & ParseAttr.NominativeCase)) != ParseAttr.No) 
                    {
                        if (!wf.Case.IsUndefined && !wf.Case.IsNominative) 
                            continue;
                    }
                    MorphPersonItemVariant v = new MorphPersonItemVariant(wf.NormalCase, wf, true);
                    if (wf.NormalCase != tt.Term && Pullenti.Morph.LanguageHelper.EndsWith(tt.Term, "ОВ")) 
                    {
                        v.Value = tt.Term;
                        v.Gender = Pullenti.Morph.MorphGender.Masculine;
                    }
                    else if ((wf.Number == Pullenti.Morph.MorphNumber.Plural && wf.NormalFull != null && wf.NormalFull != wf.NormalCase) && wf.NormalFull.Length > 1) 
                    {
                        v.Value = wf.NormalFull;
                        v.Number = Pullenti.Morph.MorphNumber.Singular;
                        if (wf.NormalCase.Length > tt.Term.Length) 
                            v.Value = tt.Term;
                    }
                    res.Lastname.Vars.Add(v);
                    if (wf.IsInDictionary && v.Gender == Pullenti.Morph.MorphGender.Undefined && wf.Gender == Pullenti.Morph.MorphGender.Undefined) 
                    {
                        v.Gender = Pullenti.Morph.MorphGender.Masculine;
                        MorphPersonItemVariant vv = new MorphPersonItemVariant(wf.NormalCase, wf, true);
                        vv.Value = v.Value;
                        vv.ShortValue = v.ShortValue;
                        vv.Gender = Pullenti.Morph.MorphGender.Feminie;
                        res.Lastname.Vars.Add(vv);
                    }
                    if (wf.IsInDictionary) 
                        res.Lastname.IsInDictionary = true;
                    if (tt.Term.EndsWith("ИХ") || tt.Term.EndsWith("ЫХ")) 
                    {
                        if (res.Lastname.Vars[0].Value != tt.Term) 
                            res.Lastname.Vars.Insert(0, new MorphPersonItemVariant(tt.Term, new Pullenti.Morph.MorphBaseInfo() { Case = Pullenti.Morph.MorphCase.AllCases, Gender = Pullenti.Morph.MorphGender.Masculine | Pullenti.Morph.MorphGender.Feminie, Class = new Pullenti.Morph.MorphClass() { IsProperSurname = true } }, true));
                    }
                }
                if (surPrefix != null) 
                    continue;
                if (wf.Class.IsProperName && wf.Number != Pullenti.Morph.MorphNumber.Plural) 
                {
                    bool ok = true;
                    if (t.Morph.Language.IsUa) 
                    {
                    }
                    else if (wf.NormalCase == "ЯКОВ" || wf.NormalCase == "ИОВ" || wf.NormalCase == "ИАКОВ") 
                    {
                    }
                    else if (wf.NormalCase != null && (wf.NormalCase.Length < 5)) 
                    {
                    }
                    else 
                    {
                        ok = !Pullenti.Morph.LanguageHelper.EndsWith(wf.NormalCase, "ОВ") && wf.NormalCase != "АЛЛ";
                        if (ok) 
                        {
                            if (tt.Chars.IsAllUpper && (tt.LengthChar < 4)) 
                                ok = false;
                        }
                    }
                    if (ok) 
                    {
                        if (res.Firstname == null) 
                            res.Firstname = new MorphPersonItem() { Term = tt.Term };
                        res.Firstname.Vars.Add(new MorphPersonItemVariant(wf.NormalCase, wf, false));
                        if (wf.IsInDictionary) 
                        {
                            if (!tt.Chars.IsAllUpper || tt.LengthChar > 4) 
                                res.Firstname.IsInDictionary = true;
                        }
                    }
                }
                if (!MorphPersonItem.EndsWithStdSurname(tt.Term)) 
                {
                    if (wf.Class.IsProperSecname) 
                    {
                        if (res.Middlename == null) 
                            res.Middlename = new MorphPersonItem() { Term = tt.Term };
                        else if (wf.Misc.Form == Pullenti.Morph.MorphForm.Synonym) 
                            continue;
                        MorphPersonItemVariant iii = new MorphPersonItemVariant(wf.NormalCase, wf, false);
                        if (iii.Value == tt.Term) 
                            res.Middlename.Vars.Insert(0, iii);
                        else 
                            res.Middlename.Vars.Add(iii);
                        if (wf.IsInDictionary) 
                            res.Middlename.IsInDictionary = true;
                    }
                    if (!wf.Class.IsProper && wf.IsInDictionary) 
                        res.IsInDictionary = true;
                }
                else if (wf.IsInDictionary && !wf.Class.IsProper && Pullenti.Morph.LanguageHelper.EndsWith(tt.Term, "КО")) 
                    res.IsInDictionary = true;
            }
            if (res.Lastname != null) 
            {
                foreach (MorphPersonItemVariant v in res.Lastname.Vars) 
                {
                    if (MorphPersonItem.EndsWithStdSurname(v.Value)) 
                    {
                        res.Lastname.IsLastnameHasStdTail = true;
                        break;
                    }
                }
                if (!res.Lastname.IsInDictionary) 
                {
                    if (((!res.Lastname.IsInDictionary && !res.Lastname.IsLastnameHasStdTail)) || MorphPersonItem.EndsWithStdSurname(tt.Term)) 
                    {
                        MorphPersonItemVariant v = new MorphPersonItemVariant(tt.Term, null, true);
                        if (Pullenti.Morph.LanguageHelper.EndsWithEx(tt.Term, "ВА", "НА", null, null)) 
                            res.Lastname.Vars.Insert(0, v);
                        else 
                            res.Lastname.Vars.Add(v);
                        if (MorphPersonItem.EndsWithStdSurname(v.Value) && !res.Lastname.IsInDictionary) 
                            res.Lastname.IsLastnameHasStdTail = true;
                    }
                }
                res.Lastname.CorrectLastnameVariants();
                if (surPrefix != null) 
                {
                    res.Lastname.IsLastnameHasHiphen = true;
                    res.Lastname.Term = string.Format("{0}-{1}", surPrefix, res.Lastname.Term);
                    foreach (MorphPersonItemVariant v in res.Lastname.Vars) 
                    {
                        v.Value = string.Format("{0}-{1}", surPrefix, v.Value);
                    }
                }
                if (tt.Morph.Class.IsAdjective && !res.Lastname.IsInOntology) 
                {
                    bool stdEnd = false;
                    foreach (MorphPersonItemVariant v in res.Lastname.Vars) 
                    {
                        if (MorphPersonItem.EndsWithStdSurname(v.Value)) 
                        {
                            stdEnd = true;
                            break;
                        }
                    }
                    if (!stdEnd && (tt.WhitespacesAfterCount < 2)) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.EndToken != npt.BeginToken) 
                        {
                            if ((prevList != null && prevList.Count == 1 && prevList[0].Firstname != null) && prevList[0].Firstname.IsInDictionary && tt.WhitespacesBeforeCount == 1) 
                            {
                            }
                            else 
                            {
                                PersonItemToken nex = _tryAttach(npt.EndToken, locOnt, attrs, null);
                                if (nex != null && nex.Firstname != null) 
                                {
                                }
                                else 
                                    res.Lastname = null;
                            }
                        }
                    }
                }
            }
            else if (tt.LengthChar > 2) 
            {
                res.Lastname = new MorphPersonItem();
                foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
                {
                    if (!wf.Class.IsVerb) 
                    {
                        if (wf.ContainsAttr("к.ф.", null)) 
                            continue;
                        res.Lastname.Vars.Add(new MorphPersonItemVariant((wf as Pullenti.Morph.MorphWordForm).NormalCase, wf, true));
                        if (!res.Lastname.IsLastnameHasStdTail) 
                            res.Lastname.IsLastnameHasStdTail = MorphPersonItem.EndsWithStdSurname((wf as Pullenti.Morph.MorphWordForm).NormalCase);
                    }
                }
                res.Lastname.Vars.Add(new MorphPersonItemVariant(tt.Term, null, true));
                if (!res.Lastname.IsLastnameHasStdTail) 
                    res.Lastname.IsLastnameHasStdTail = MorphPersonItem.EndsWithStdSurname(tt.Term);
                if (surPrefix != null) 
                {
                    res.Lastname.AddPrefix(surPrefix + "-");
                    res.Lastname.IsLastnameHasHiphen = true;
                }
            }
            if (res.BeginToken == res.EndToken) 
            {
                if (res.BeginToken.GetMorphClassInDictionary().IsVerb && res.Lastname != null) 
                {
                    if (!res.Lastname.IsLastnameHasStdTail && !res.Lastname.IsInDictionary) 
                    {
                        if (res.IsNewlineBefore) 
                        {
                        }
                        else if (res.BeginToken.Chars.IsCapitalUpper && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(res.BeginToken)) 
                        {
                        }
                        else 
                            res.Lastname = null;
                    }
                }
                if (res.Lastname != null && res.BeginToken.IsValue("ЗАМ", null)) 
                    return null;
                if (res.Firstname != null && (res.BeginToken is Pullenti.Ner.TextToken)) 
                {
                    if ((res.BeginToken as Pullenti.Ner.TextToken).Term == "ЛЮБОЙ") 
                        res.Firstname = null;
                }
                if (res.BeginToken.GetMorphClassInDictionary().IsAdjective && res.Lastname != null) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null) 
                    {
                        if (npt.BeginToken != npt.EndToken) 
                        {
                            if (!res.Lastname.IsInOntology && !res.Lastname.IsInDictionary) 
                                res.Lastname = null;
                        }
                    }
                }
            }
            if (res.Firstname != null) 
            {
                for (int i = 0; i < res.Firstname.Vars.Count; i++) 
                {
                    string val = res.Firstname.Vars[i].Value;
                    List<ShortNameHelper.ShortnameVar> di = ShortNameHelper.GetNamesForShortname(val);
                    if (di == null) 
                        continue;
                    Pullenti.Morph.MorphGender g = res.Firstname.Vars[i].Gender;
                    if (g != Pullenti.Morph.MorphGender.Masculine && g != Pullenti.Morph.MorphGender.Feminie) 
                    {
                        bool fi = true;
                        foreach (ShortNameHelper.ShortnameVar kp in di) 
                        {
                            if (fi) 
                            {
                                res.Firstname.Vars[i].ShortValue = val;
                                res.Firstname.Vars[i].Value = kp.Name;
                                res.Firstname.Vars[i].Gender = kp.Gender;
                                fi = false;
                            }
                            else 
                            {
                                Pullenti.Morph.MorphBaseInfo mi = new Pullenti.Morph.MorphBaseInfo() { Gender = kp.Gender };
                                res.Firstname.Vars.Add(new MorphPersonItemVariant(kp.Name, mi, false) { ShortValue = val });
                            }
                        }
                    }
                    else 
                    {
                        int cou = 0;
                        foreach (ShortNameHelper.ShortnameVar kp in di) 
                        {
                            if (kp.Gender == g) 
                            {
                                if ((++cou) < 2) 
                                {
                                    res.Firstname.Vars[i].Value = kp.Name;
                                    res.Firstname.Vars[i].ShortValue = val;
                                }
                                else 
                                    res.Firstname.Vars.Insert(i + 1, new MorphPersonItemVariant(kp.Name, res.Firstname.Vars[i], false) { ShortValue = val });
                            }
                        }
                    }
                }
            }
            if ((res != null && res.IsInDictionary && res.Firstname == null) && ((attrs & ParseAttr.MustBeItemAlways)) == ParseAttr.No) 
            {
                Pullenti.Ner.Core.StatisticWordInfo wi = res.Kit.Statistics.GetWordInfo(res.BeginToken);
                if (wi != null && wi.LowerCount > 0) 
                {
                    if (((t.Morph.Class.IsPreposition || t.Morph.Class.IsConjunction || t.Morph.Class.IsPronoun)) && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    {
                    }
                    else 
                        return null;
                }
            }
            if (res.EndToken.Next != null && res.EndToken.Next.IsHiphen && (res.EndToken.Next.Next is Pullenti.Ner.TextToken)) 
            {
                string ter = (res.EndToken.Next.Next as Pullenti.Ner.TextToken).Term;
                if (m_ArabPostfix.Contains(ter) || m_ArabPostfixFem.Contains(ter)) 
                {
                    res.EndToken = res.EndToken.Next.Next;
                    res.AddPostfixInfo(ter, (m_ArabPostfixFem.Contains(ter) ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Masculine));
                    if ((((ter == "ОГЛЫ" || ter == "ОГЛИ" || ter == "КЫЗЫ") || ter == "ГЫЗЫ" || ter == "УГЛИ") || ter == "КЗЫ" || ter == "УЛЫ") || ter == "УУЛУ") 
                    {
                        if (res.Middlename != null) 
                        {
                            res.Firstname = null;
                            res.Lastname = null;
                        }
                    }
                }
                else if ((!res.IsWhitespaceAfter && !res.EndToken.Next.IsWhitespaceAfter && res.EndToken.Next.Next.Chars == res.BeginToken.Chars) && res.BeginToken == res.EndToken) 
                {
                    PersonItemToken res1 = TryAttach(res.EndToken.Next.Next, locOnt, ParseAttr.No, null);
                    if (res1 != null && res1.BeginToken == res1.EndToken) 
                    {
                        if (res1.Lastname != null && res.Lastname != null && ((((res1.Lastname.IsHasStdPostfix || res1.Lastname.IsInDictionary || res1.Lastname.IsInOntology) || res.Lastname.IsHasStdPostfix || res.Lastname.IsInDictionary) || res.Lastname.IsInOntology))) 
                        {
                            res.Lastname.MergeHiphen(res1.Lastname);
                            if (res.Value != null && res1.Value != null) 
                                res.Value = string.Format("{0}-{1}", res.Value, res1.Value);
                            res.Firstname = null;
                            res.Middlename = null;
                            res.EndToken = res1.EndToken;
                        }
                        else if (res.Firstname != null && ((res.Firstname.IsInDictionary || res.Firstname.IsInOntology))) 
                        {
                            if (res1.Firstname != null) 
                            {
                                if (res.Value != null && res1.Value != null) 
                                    res.Value = string.Format("{0}-{1}", res.Value, res1.Value);
                                res.Firstname.MergeHiphen(res1.Firstname);
                                res.Lastname = null;
                                res.Middlename = null;
                                res.EndToken = res1.EndToken;
                            }
                            else if (res1.Middlename != null) 
                            {
                                if (res.Value != null && res1.Value != null) 
                                    res.Value = string.Format("{0}-{1}", res.Value, res1.Value);
                                res.EndToken = res1.EndToken;
                                if (res.Middlename != null) 
                                    res.Middlename.MergeHiphen(res1.Middlename);
                                if (res.Firstname != null) 
                                {
                                    res.Firstname.MergeHiphen(res1.Middlename);
                                    if (res.Middlename == null) 
                                        res.Middlename = res.Firstname;
                                }
                                if (res.Lastname != null) 
                                {
                                    res.Lastname.MergeHiphen(res1.Middlename);
                                    if (res.Middlename == null) 
                                        res.Middlename = res.Firstname;
                                }
                            }
                            else if (res1.Lastname != null && !res1.Lastname.IsInDictionary && !res1.Lastname.IsInOntology) 
                            {
                                if (res.Value != null && res1.Value != null) 
                                    res.Value = string.Format("{0}-{1}", res.Value, res1.Value);
                                res.Firstname.MergeHiphen(res1.Lastname);
                                res.Lastname = null;
                                res.Middlename = null;
                                res.EndToken = res1.EndToken;
                            }
                        }
                        else if ((res.Firstname == null && res.Middlename == null && res.Lastname != null) && !res.Lastname.IsInOntology && !res.Lastname.IsInDictionary) 
                        {
                            if (res.Value != null && res1.Value != null) 
                                res.Value = string.Format("{0}-{1}", res.Value, res1.Value);
                            res.EndToken = res1.EndToken;
                            if (res1.Firstname != null) 
                            {
                                res.Lastname.MergeHiphen(res1.Firstname);
                                res.Firstname = res.Lastname;
                                res.Lastname = (res.Middlename = null);
                            }
                            else if (res1.Middlename != null) 
                            {
                                res.Lastname.MergeHiphen(res1.Middlename);
                                res.Middlename = res.Lastname;
                                res.Firstname = null;
                            }
                            else if (res1.Lastname != null) 
                                res.Lastname.MergeHiphen(res1.Lastname);
                            else if (res1.Value != null) 
                            {
                                foreach (MorphPersonItemVariant v in res.Lastname.Vars) 
                                {
                                    v.Value = string.Format("{0}-{1}", v.Value, res1.Value);
                                }
                            }
                        }
                        else if (((res.Firstname == null && res.Lastname == null && res.Middlename == null) && res1.Lastname != null && res.Value != null) && res1.Value != null) 
                        {
                            res.Lastname = res1.Lastname;
                            res.Lastname.AddPrefix(res.Value + "-");
                            res.Value = string.Format("{0}-{1}", res.Value, res1.Value);
                            res.Firstname = null;
                            res.Middlename = null;
                            res.EndToken = res1.EndToken;
                        }
                        else if (((res.Firstname == null && res.Lastname != null && res.Middlename == null) && res1.Lastname == null && res.Value != null) && res1.Value != null) 
                        {
                            res.Lastname.AddPostfix("-" + res1.Value, Pullenti.Morph.MorphGender.Undefined);
                            res.Value = string.Format("{0}-{1}", res.Value, res1.Value);
                            res.Firstname = null;
                            res.Middlename = null;
                            res.EndToken = res1.EndToken;
                        }
                    }
                }
            }
            while ((res.EndToken.WhitespacesAfterCount < 3) && (res.EndToken.Next is Pullenti.Ner.TextToken)) 
            {
                string ter = (res.EndToken.Next as Pullenti.Ner.TextToken).Term;
                if (((ter != "АЛИ" && ter != "ПАША")) || res.EndToken.Next.Chars.IsAllLower) 
                {
                    if (m_ArabPostfix.Contains(ter) || m_ArabPostfixFem.Contains(ter)) 
                    {
                        if (res.EndToken.Next.Next != null && res.EndToken.Next.Next.IsHiphen) 
                        {
                        }
                        else 
                        {
                            res.EndToken = res.EndToken.Next;
                            res.AddPostfixInfo(ter, (m_ArabPostfixFem.Contains(ter) ? Pullenti.Morph.MorphGender.Feminie : Pullenti.Morph.MorphGender.Masculine));
                            if ((((ter == "ОГЛЫ" || ter == "ОГЛИ" || ter == "КЫЗЫ") || ter == "ГЫЗЫ" || ter == "УГЛИ") || ter == "КЗЫ" || ter == "УЛЫ") || ter == "УУЛУ") 
                            {
                                if (res.Middlename != null) 
                                {
                                    res.Firstname = null;
                                    res.Lastname = null;
                                }
                            }
                            continue;
                        }
                    }
                }
                break;
            }
            return res;
        }
        public static List<string> m_SurPrefixes = new List<string>(new string[] {"АБД", "АБУ", "АЛ", "АЛЬ", "БИН", "БЕН", "ИБН", "ФОН", "ВАН", "ДЕ", "ДИ", "ДА", "ЛА", "ЛЕ", "ЛЯ", "ЭЛЬ"});
        static List<string> m_SurPrefixesLat = new List<string>(new string[] {"ABD", "AL", "BEN", "IBN", "VON", "VAN", "DE", "DI", "LA", "LE", "DA", "DE"});
        public static List<string> m_ArabPostfix = new List<string>(new string[] {"АГА", "АЛИ", "АР", "АС", "АШ", "БЕЙ", "БЕК", "ЗАДЕ", "ОГЛЫ", "ОГЛИ", "УГЛИ", "ОЛЬ", "ООЛ", "ПАША", "УЛЬ", "УЛЫ", "УУЛУ", "ХАН", "ХАДЖИ", "ШАХ", "ЭД", "ЭЛЬ"});
        public static List<string> m_ArabPostfixFem = new List<string>(new string[] {"АСУ", "АЗУ", "ГЫЗЫ", "ЗУЛЬ", "КЫЗЫ", "КЫС", "КЗЫ"});
        public enum ParseAttr : int
        {
            No = 0,
            AltVar = 1,
            CanBeLatin = 2,
            CanInitialBeDigit = 4,
            CanBeLower = 8,
            /// <summary>
            /// Всегда выделять элемент, не делать никакие проверки
            /// </summary>
            MustBeItemAlways = 0x10,
            IgnoreAttrs = 0x20,
            /// <summary>
            /// Известно, что персона в именительном падеже
            /// </summary>
            NominativeCase = 0x40,
            /// <summary>
            /// Для фамилий префиксы (фон, ван) оформлять отдельным элементом
            /// </summary>
            SurnamePrefixNotMerge = 0x80,
            /// <summary>
            /// Ослабленная проверка, когда перед комбинацией находится атрибут персоны
            /// </summary>
            AfterAttribute = 0x100,
        }

        public static List<PersonItemToken> TryAttachList(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locOnt, ParseAttr attrs = ParseAttr.No, int maxCount = 10)
        {
            if (t == null) 
                return null;
            if (((!(t is Pullenti.Ner.TextToken) || !t.Chars.IsLetter)) && ((attrs & ParseAttr.CanInitialBeDigit)) == ParseAttr.No) 
            {
                if ((t is Pullenti.Ner.ReferentToken) && (((t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) || t.GetReferent().TypeName == "ORGANIZATION" || t.GetReferent().TypeName == "TRANSPORT"))) 
                {
                    if ((t as Pullenti.Ner.ReferentToken).BeginToken == (t as Pullenti.Ner.ReferentToken).EndToken) 
                    {
                    }
                    else 
                        return null;
                }
                else if (t is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                    if (nt.BeginToken == nt.EndToken && nt.Typ == Pullenti.Ner.NumberSpellingType.Words && !nt.BeginToken.Chars.IsAllLower) 
                    {
                    }
                    else 
                        return null;
                }
                else 
                    return null;
            }
            PersonItemToken pit = TryAttach(t, locOnt, attrs, null);
            if (pit == null && t.Chars.IsLatinLetter) 
            {
            }
            if (pit == null) 
                return null;
            List<PersonItemToken> res = new List<PersonItemToken>();
            res.Add(pit);
            t = pit.EndToken.Next;
            if ((t != null && t.IsChar('.') && pit.Typ == ItemType.Value) && pit.LengthChar > 3) 
            {
                string str = pit.GetSourceText();
                if (char.IsUpper(str[0]) && char.IsUpper(str[str.Length - 1])) 
                {
                    bool ok = true;
                    for (int i = 1; i < (str.Length - 1); i++) 
                    {
                        if (!char.IsLower(str[i])) 
                            ok = false;
                    }
                    if (ok) 
                    {
                        pit.Value = pit.Value.Substring(0, pit.Value.Length - 1);
                        pit.Firstname = (pit.Middlename = (pit.Lastname = null));
                        PersonItemToken pit2 = new PersonItemToken(t, t) { Typ = ItemType.Initial, Value = str.Substring(str.Length - 1) };
                        res.Add(pit2);
                        t = t.Next;
                    }
                }
            }
            bool zap = false;
            for (; t != null; t = (t == null ? null : t.Next)) 
            {
                if (t.WhitespacesBeforeCount > 15) 
                    break;
                Pullenti.Ner.Token tt = t;
                if (tt.IsHiphen && tt.Next != null) 
                {
                    if (!tt.IsWhitespaceAfter && !tt.IsWhitespaceBefore) 
                        tt = t.Next;
                    else if (tt.Previous.Chars == tt.Next.Chars && !tt.IsNewlineAfter) 
                        tt = tt.Next;
                }
                else if ((tt.IsChar(',') && (tt.WhitespacesAfterCount < 2) && tt.Next != null) && res.Count == 1) 
                {
                    zap = true;
                    tt = tt.Next;
                }
                else if ((tt.IsChar('(') && (tt.Next is Pullenti.Ner.TextToken) && tt.Next.Chars == tt.Previous.Chars) && tt.Next.Next != null && tt.Next.Next.IsChar(')')) 
                {
                    PersonItemToken pit0 = res[res.Count - 1];
                    PersonItemToken pit11 = TryAttach(tt.Next, locOnt, attrs, null);
                    if (pit0.Firstname != null && pit11 != null && pit11.Firstname != null) 
                    {
                        pit0.Firstname.Vars.AddRange(pit11.Firstname.Vars);
                        tt = tt.Next.Next;
                        pit0.EndToken = tt;
                        tt = tt.Next;
                    }
                    else if (pit0.Lastname != null && ((pit0.Lastname.IsInDictionary || pit0.Lastname.IsLastnameHasStdTail || pit0.Lastname.IsHasStdPostfix))) 
                    {
                        if (pit11 != null && pit11.Lastname != null) 
                        {
                            bool ok = false;
                            if ((pit11.Lastname.IsInDictionary || pit11.Lastname.IsLastnameHasStdTail || pit11.Lastname.IsHasStdPostfix)) 
                                ok = true;
                            else if (res.Count == 1) 
                            {
                                PersonItemToken pit22 = TryAttach(tt.Next.Next.Next, locOnt, attrs, null);
                                if (pit22 != null) 
                                {
                                    if (pit22.Firstname != null) 
                                        ok = true;
                                }
                            }
                            if (ok) 
                            {
                                pit0.Lastname.Vars.AddRange(pit11.Lastname.Vars);
                                tt = tt.Next.Next;
                                pit0.EndToken = tt;
                                tt = tt.Next;
                            }
                        }
                    }
                }
                PersonItemToken pit1 = TryAttach(tt, locOnt, attrs, res);
                if (pit1 == null) 
                    break;
                if (pit1.Chars.IsCyrillicLetter != pit.Chars.IsCyrillicLetter) 
                {
                    bool ok = false;
                    if (pit1.Typ == ItemType.Initial) 
                    {
                        if (pit1.Chars.IsCyrillicLetter) 
                        {
                            char v = Pullenti.Morph.LanguageHelper.GetLatForCyr(pit1.Value[0]);
                            if (v != ((char)0)) 
                            {
                                pit1.Value = string.Format("{0}", v);
                                ok = true;
                                pit1.Chars = new Pullenti.Morph.CharsInfo() { IsLatinLetter = true };
                            }
                            else if (pit.Typ == ItemType.Initial) 
                            {
                                v = Pullenti.Morph.LanguageHelper.GetCyrForLat(pit.Value[0]);
                                if (v != ((char)0)) 
                                {
                                    pit.Value = string.Format("{0}", v);
                                    ok = true;
                                    pit.Chars = new Pullenti.Morph.CharsInfo() { IsCyrillicLetter = true };
                                    pit = pit1;
                                }
                            }
                        }
                        else 
                        {
                            char v = Pullenti.Morph.LanguageHelper.GetCyrForLat(pit1.Value[0]);
                            if (v != ((char)0)) 
                            {
                                pit1.Value = string.Format("{0}", v);
                                ok = true;
                                pit1.Chars = new Pullenti.Morph.CharsInfo() { IsCyrillicLetter = true };
                            }
                            else if (pit.Typ == ItemType.Initial) 
                            {
                                v = Pullenti.Morph.LanguageHelper.GetLatForCyr(pit.Value[0]);
                                if (v != ((char)0)) 
                                {
                                    pit.Value = string.Format("{0}", v);
                                    ok = true;
                                    pit.Chars = new Pullenti.Morph.CharsInfo() { IsLatinLetter = true };
                                    pit = pit1;
                                }
                            }
                        }
                    }
                    else if (pit.Typ == ItemType.Initial) 
                    {
                        if (pit.Chars.IsCyrillicLetter) 
                        {
                            char v = Pullenti.Morph.LanguageHelper.GetLatForCyr(pit.Value[0]);
                            if (v != ((char)0)) 
                            {
                                pit.Value = string.Format("{0}", v);
                                ok = true;
                            }
                            else if (pit1.Typ == ItemType.Initial) 
                            {
                                v = Pullenti.Morph.LanguageHelper.GetCyrForLat(pit1.Value[0]);
                                if (v != ((char)0)) 
                                {
                                    pit1.Value = string.Format("{0}", v);
                                    ok = true;
                                    pit = pit1;
                                }
                            }
                        }
                        else 
                        {
                            char v = Pullenti.Morph.LanguageHelper.GetCyrForLat(pit.Value[0]);
                            if (v != ((char)0)) 
                            {
                                pit.Value = string.Format("{0}", v);
                                ok = true;
                            }
                            else if (pit1.Typ == ItemType.Initial) 
                            {
                                v = Pullenti.Morph.LanguageHelper.GetLatForCyr(pit1.Value[0]);
                                if (v != ((char)0)) 
                                {
                                    pit.Value = string.Format("{0}", v);
                                    ok = true;
                                    pit = pit1;
                                }
                            }
                        }
                    }
                    if (!ok) 
                        break;
                }
                if (pit1.Typ == ItemType.Value || ((pit1.Typ == ItemType.Suffix && pit1.IsNewlineBefore))) 
                {
                    if (locOnt != null && ((attrs & ParseAttr.IgnoreAttrs)) == ParseAttr.No) 
                    {
                        PersonAttrToken pat = PersonAttrToken.TryAttach(pit1.BeginToken, locOnt, PersonAttrToken.PersonAttrAttachAttrs.No);
                        if (pat != null) 
                        {
                            if (pit1.IsNewlineBefore) 
                                break;
                            if (pit1.Lastname == null || !pit1.Lastname.IsLastnameHasStdTail) 
                            {
                                Pullenti.Morph.MorphClass ty = pit1.BeginToken.GetMorphClassInDictionary();
                                if (ty.IsNoun) 
                                {
                                    if (pit1.WhitespacesBeforeCount > 1) 
                                        break;
                                    if (pat.Chars.IsCapitalUpper && pat.BeginToken == pat.EndToken) 
                                    {
                                    }
                                    else 
                                        break;
                                }
                            }
                        }
                    }
                }
                if (tt != t) 
                {
                    pit1.IsHiphenBefore = true;
                    res[res.Count - 1].IsHiphenAfter = true;
                }
                res.Add(pit1);
                t = pit1.EndToken;
                if (res.Count > 10) 
                    break;
                if (maxCount > 0 && res.Count >= maxCount) 
                    break;
            }
            if (res[0].IsAsianItem(false) && res[0].Value.Length == 1) 
            {
                if (((attrs & ParseAttr.MustBeItemAlways)) == ParseAttr.No) 
                {
                    if (res.Count < 2) 
                        return null;
                    if (!res[1].IsAsianItem(false) || res[1].Value.Length == 1) 
                        return null;
                }
            }
            if (zap && res.Count > 1) 
            {
                bool ok = false;
                if (res[0].Lastname != null && res.Count == 3) 
                {
                    if (res[1].Typ == ItemType.Initial || res[1].Firstname != null) 
                    {
                        if (res[2].Typ == ItemType.Initial || res[2].Middlename != null) 
                            ok = true;
                    }
                }
                else if (((attrs & ParseAttr.CanInitialBeDigit)) != ParseAttr.No && res[0].Typ == ItemType.Value && res[1].Typ == ItemType.Initial) 
                {
                    if (res.Count == 2) 
                        ok = true;
                    else if (res.Count == 3 && res[2].Typ == ItemType.Initial) 
                        ok = true;
                    else if (res.Count == 3 && res[2].IsInDictionary) 
                        ok = true;
                }
                if (!ok) 
                    res.RemoveRange(1, res.Count - 1);
            }
            if (res.Count == 1 && res[0].IsNewlineBefore && res[0].IsNewlineAfter) 
            {
                if (res[0].Lastname != null && ((res[0].Lastname.IsHasStdPostfix || res[0].Lastname.IsInDictionary || res[0].Lastname.IsLastnameHasStdTail))) 
                {
                    List<PersonItemToken> res1 = TryAttachList(res[0].EndToken.Next, locOnt, ParseAttr.CanBeLatin, maxCount);
                    if (res1 != null && res1.Count > 0) 
                    {
                        if (res1.Count == 2 && ((res1[0].Firstname != null || res1[1].Middlename != null)) && res1[1].IsNewlineAfter) 
                            res.AddRange(res1);
                        else if (res1.Count == 1 && res1[0].IsNewlineAfter) 
                        {
                            List<PersonItemToken> res2 = TryAttachList(res1[0].EndToken.Next, locOnt, ParseAttr.CanBeLatin, maxCount);
                            if (res2 != null && res2.Count == 1 && res2[0].IsNewlineAfter) 
                            {
                                if (res1[0].Firstname != null || res2[0].Middlename != null) 
                                {
                                    res.Add(res1[0]);
                                    res.Add(res2[0]);
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < res.Count; i++) 
            {
                if (res[i].Firstname != null && res[i].BeginToken.IsValue("СВЕТА", null)) 
                {
                    if (i > 0 && res[i - 1].Lastname != null) 
                    {
                    }
                    else if (((i + 1) < res.Count) && ((res[i + 1].Lastname != null || res[i + 1].Middlename != null))) 
                    {
                    }
                    else 
                        continue;
                    res[i].Firstname.Vars[0].Value = "СВЕТЛАНА";
                }
                else if (res[i].Typ == ItemType.Value && ((i + 1) < res.Count) && res[i + 1].Typ == ItemType.Suffix) 
                {
                    res[i].AddPostfixInfo(res[i + 1].Value, Pullenti.Morph.MorphGender.Undefined);
                    res[i].EndToken = res[i + 1].EndToken;
                    if (res[i].Lastname == null) 
                    {
                        res[i].Lastname = new MorphPersonItem() { IsHasStdPostfix = true };
                        res[i].Lastname.Vars.Add(new MorphPersonItemVariant(res[i].Value, new Pullenti.Morph.MorphBaseInfo(), true));
                        res[i].Firstname = null;
                    }
                    res.RemoveAt(i + 1);
                }
            }
            if (res.Count > 1 && res[0].IsInDictionary && ((attrs & ((ParseAttr.MustBeItemAlways | ParseAttr.AfterAttribute)))) == ParseAttr.No) 
            {
                Pullenti.Morph.MorphClass mc = res[0].BeginToken.GetMorphClassInDictionary();
                if (mc.IsPronoun || mc.IsPersonalPronoun) 
                {
                    if (res[0].BeginToken.IsValue("ТОМ", null)) 
                    {
                    }
                    else 
                        return null;
                }
            }
            for (int i = 0; i < (res.Count - 1); i++) 
            {
                if (res[i].Typ == ItemType.Value && res[i + 1].Typ == ItemType.Value && res[i].EndToken.Next.IsHiphen) 
                {
                    bool ok = false;
                    if (i > 0 && res[i - 1].Typ == ItemType.Initial && (i + 2) == res.Count) 
                        ok = true;
                    else if (i == 0 && ((i + 2) < res.Count) && res[i + 2].Typ == ItemType.Initial) 
                        ok = true;
                    if (!ok) 
                        continue;
                    res[i].EndToken = res[i + 1].EndToken;
                    res[i].Value = string.Format("{0}-{1}", res[i].Value, res[i + 1].Value);
                    res[i].Firstname = (res[i].Lastname = (res[i].Middlename = null));
                    res[i].IsInDictionary = false;
                    res.RemoveAt(i + 1);
                    break;
                }
            }
            return res;
        }
        /// <summary>
        /// Это попытка привязать персону со специфического места
        /// </summary>
        /// <param name="prevPersTemplate">шаблон от предыдущей персоны (поможет принять решение в случае ошибки)</param>
        public static Pullenti.Ner.ReferentToken TryParsePerson(Pullenti.Ner.Token t, FioTemplateType prevPersTemplate = FioTemplateType.Undefined)
        {
            if (t == null) 
                return null;
            if (t.GetReferent() is Pullenti.Ner.Person.PersonReferent) 
            {
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                if (rt.BeginToken == rt.EndToken) 
                {
                    Pullenti.Ner.Token tt1 = t.Next;
                    if (tt1 != null && tt1.IsComma) 
                        tt1 = tt1.Next;
                    if (tt1 != null && (tt1.WhitespacesBeforeCount < 2)) 
                    {
                        List<PersonItemToken> pits0 = PersonItemToken.TryAttachList(tt1, null, ParseAttr.CanInitialBeDigit, 10);
                        if (pits0 != null && pits0[0].Typ == ItemType.Initial) 
                        {
                            string str = rt.Referent.GetStringValue(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME);
                            if (str != null && str.StartsWith(pits0[0].Value)) 
                            {
                                Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(rt.Referent, t, pits0[0].EndToken) { MiscAttrs = (int)FioTemplateType.SurnameI };
                                if (pits0.Count > 1 && pits0[1].Typ == ItemType.Initial) 
                                {
                                    str = rt.Referent.GetStringValue(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME);
                                    if (str != null && str.StartsWith(pits0[1].Value)) 
                                    {
                                        res.EndToken = pits0[1].EndToken;
                                        res.MiscAttrs = (int)FioTemplateType.SurnameII;
                                    }
                                }
                                return res;
                            }
                        }
                        if (((((tt1 is Pullenti.Ner.TextToken) && tt1.LengthChar == 1 && tt1.Chars.IsAllUpper) && tt1.Chars.IsCyrillicLetter && (tt1.Next is Pullenti.Ner.TextToken)) && (tt1.WhitespacesAfterCount < 2) && tt1.Next.LengthChar == 1) && tt1.Next.Chars.IsAllUpper && tt1.Next.Chars.IsCyrillicLetter) 
                        {
                            string str = rt.Referent.GetStringValue(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME);
                            if (str != null && str.StartsWith((tt1 as Pullenti.Ner.TextToken).Term)) 
                            {
                                string str2 = rt.Referent.GetStringValue(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME);
                                if (str2 == null || str2.StartsWith((tt1.Next as Pullenti.Ner.TextToken).Term)) 
                                {
                                    Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(rt.Referent, t, tt1.Next) { MiscAttrs = (int)FioTemplateType.NameISurname };
                                    if (str2 == null) 
                                        rt.Referent.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME, (tt1.Next as Pullenti.Ner.TextToken).Term, false, 0);
                                    if (res.EndToken.Next != null && res.EndToken.Next.IsChar('.')) 
                                        res.EndToken = res.EndToken.Next;
                                    return res;
                                }
                            }
                        }
                    }
                }
                return rt;
            }
            if (t.GetReferent() != null && t.GetReferent().TypeName == "ORGANIZATION") 
            {
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                Pullenti.Ner.ReferentToken ppp = TryParsePerson(rt.BeginToken, FioTemplateType.Undefined);
                if (ppp != null && ppp.EndChar == rt.EndChar) 
                {
                    ppp.BeginToken = (ppp.EndToken = rt);
                    return ppp;
                }
            }
            List<PersonItemToken> pits = PersonItemToken.TryAttachList(t, null, ParseAttr.CanInitialBeDigit | ParseAttr.CanBeLatin, 10);
            if ((pits == null && (t is Pullenti.Ner.TextToken) && t.Chars.IsAllLower) && t.LengthChar > 3) 
            {
                PersonItemToken pi = PersonItemToken.TryAttach(t, null, ParseAttr.CanInitialBeDigit | ParseAttr.CanBeLatin | ParseAttr.CanBeLower, null);
                if (pi != null && pi.Lastname != null && ((pi.Lastname.IsInDictionary || pi.Lastname.IsLastnameHasStdTail))) 
                {
                    pits = PersonItemToken.TryAttachList(pi.EndToken.Next, null, ParseAttr.CanInitialBeDigit | ParseAttr.CanBeLatin, 10);
                    if (pits != null && pits[0].Typ == ItemType.Initial && pits[0].Chars.IsLatinLetter == pi.Chars.IsLatinLetter) 
                        pits.Insert(0, pi);
                    else 
                        pits = null;
                }
            }
            if (pits != null && prevPersTemplate != FioTemplateType.Undefined && pits[0].Typ == ItemType.Value) 
            {
                Pullenti.Ner.Token tt1 = null;
                if (pits.Count == 1 && prevPersTemplate == FioTemplateType.SurnameI) 
                    tt1 = pits[0].EndToken.Next;
                if (tt1 != null && tt1.IsComma) 
                    tt1 = tt1.Next;
                if (((tt1 is Pullenti.Ner.TextToken) && tt1.Chars.IsLetter && tt1.Chars.IsAllUpper) && tt1.LengthChar == 1 && (tt1.WhitespacesBeforeCount < 2)) 
                {
                    PersonItemToken ii = new PersonItemToken(tt1, tt1) { Typ = ItemType.Initial, Value = (tt1 as Pullenti.Ner.TextToken).Term, Chars = tt1.Chars };
                    pits.Add(ii);
                }
                if (pits.Count == 1 && pits[0].IsNewlineAfter && ((prevPersTemplate == FioTemplateType.SurnameI || prevPersTemplate == FioTemplateType.SurnameII))) 
                {
                    List<PersonItemToken> ppp = PersonItemToken.TryAttachList(pits[0].EndToken.Next, null, ParseAttr.CanBeLatin, 10);
                    if (ppp != null && ppp[0].Typ == ItemType.Initial) 
                    {
                        pits.Add(ppp[0]);
                        if (ppp.Count > 1 && ppp[1].Typ == ItemType.Initial) 
                            pits.Add(ppp[1]);
                    }
                }
            }
            if (pits != null && pits.Count > 1) 
            {
                FioTemplateType tmpls = FioTemplateType.Undefined;
                PersonItemToken first = null;
                PersonItemToken middl = null;
                PersonItemToken last = null;
                if (pits[0].Typ == ItemType.Value && pits[1].Typ == ItemType.Initial) 
                {
                    if ((t.IsValue("ГЛАВА", null) || t.IsValue("СТАТЬЯ", "СТАТТЯ") || t.IsValue("РАЗДЕЛ", "РОЗДІЛ")) || t.IsValue("ПОДРАЗДЕЛ", "ПІДРОЗДІЛ") || t.IsValue("ЧАСТЬ", "ЧАСТИНА")) 
                        return null;
                    if ((t.IsValue("CHAPTER", null) || t.IsValue("CLAUSE", null) || t.IsValue("SECTION", null)) || t.IsValue("SUBSECTION", null) || t.IsValue("PART", null)) 
                        return null;
                    first = pits[1];
                    last = pits[0];
                    tmpls = FioTemplateType.SurnameI;
                    if (pits.Count > 2 && pits[2].Typ == ItemType.Initial) 
                    {
                        middl = pits[2];
                        tmpls = FioTemplateType.SurnameII;
                    }
                }
                else if (pits[0].Typ == ItemType.Initial && pits[1].Typ == ItemType.Value) 
                {
                    first = pits[0];
                    last = pits[1];
                    tmpls = FioTemplateType.ISurname;
                }
                else if ((pits.Count > 2 && pits[0].Typ == ItemType.Initial && pits[1].Typ == ItemType.Initial) && pits[2].Typ == ItemType.Value) 
                {
                    first = pits[0];
                    middl = pits[1];
                    last = pits[2];
                    tmpls = FioTemplateType.IISurname;
                }
                if (pits.Count == 2 && pits[0].Typ == ItemType.Value && pits[1].Typ == ItemType.Value) 
                {
                    if (pits[0].Chars.IsLatinLetter && ((!pits[0].IsInDictionary || !pits[1].IsInDictionary))) 
                    {
                        if (!Pullenti.Ner.Core.MiscHelper.IsEngArticle(pits[0].BeginToken)) 
                        {
                            first = pits[0];
                            last = pits[1];
                            tmpls = FioTemplateType.NameSurname;
                        }
                    }
                }
                if (last != null) 
                {
                    Pullenti.Ner.Person.PersonReferent pers = new Pullenti.Ner.Person.PersonReferent();
                    pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, last.Value, false, 0);
                    pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, first.Value, false, 0);
                    if (middl != null) 
                        pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME, middl.Value, false, 0);
                    Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(pers, t, last.EndToken);
                    if (first.EndChar > last.EndChar) 
                        res.EndToken = first.EndToken;
                    if (middl != null && middl.EndChar > res.EndChar) 
                        res.EndToken = middl.EndToken;
                    res.Data = t.Kit.GetAnalyzerDataByAnalyzerName(Pullenti.Ner.Person.PersonAnalyzer.ANALYZER_NAME);
                    res.MiscAttrs = (int)tmpls;
                    if ((res.EndToken.WhitespacesAfterCount < 2) && (res.EndToken.Next is Pullenti.Ner.NumberToken)) 
                    {
                        Pullenti.Ner.NumberToken num = res.EndToken.Next as Pullenti.Ner.NumberToken;
                        if (num.Value == "2" || num.Value == "3") 
                        {
                            if (num.Morph.Class.IsAdjective) 
                            {
                                pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, num.Value.ToString(), false, 0);
                                res.EndToken = res.EndToken.Next;
                            }
                        }
                    }
                    return res;
                }
            }
            if (pits != null && pits.Count == 1 && pits[0].Typ == ItemType.Value) 
            {
                Pullenti.Ner.Token tt = pits[0].EndToken.Next;
                bool comma = false;
                if (tt != null && ((tt.IsComma || tt.IsChar('.')))) 
                {
                    tt = tt.Next;
                    comma = true;
                }
                if (((tt is Pullenti.Ner.TextToken) && tt.LengthChar == 2 && tt.Chars.IsAllUpper) && tt.Chars.IsCyrillicLetter) 
                {
                    Pullenti.Ner.Person.PersonReferent pers = new Pullenti.Ner.Person.PersonReferent();
                    pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, pits[0].Value, false, 0);
                    pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, (tt as Pullenti.Ner.TextToken).Term[0], false, 0);
                    pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME, (tt as Pullenti.Ner.TextToken).Term[1], false, 0);
                    Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(pers, t, tt) { MiscAttrs = (int)FioTemplateType.SurnameII };
                    if (tt.Next != null && tt.Next.IsChar('.')) 
                        res.EndToken = (tt = tt.Next);
                    res.Data = t.Kit.GetAnalyzerDataByAnalyzerName(Pullenti.Ner.Person.PersonAnalyzer.ANALYZER_NAME);
                    return res;
                }
                if ((((((tt is Pullenti.Ner.TextToken) && (tt.WhitespacesBeforeCount < 2) && tt.LengthChar == 1) && tt.Chars.IsAllUpper && tt.Chars.IsCyrillicLetter) && (tt.Next is Pullenti.Ner.TextToken) && (tt.WhitespacesAfterCount < 2)) && tt.Next.LengthChar == 1 && tt.Next.Chars.IsAllUpper) && tt.Next.Chars.IsCyrillicLetter) 
                {
                    Pullenti.Ner.Person.PersonReferent pers = new Pullenti.Ner.Person.PersonReferent();
                    pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, pits[0].Value, false, 0);
                    pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, (tt as Pullenti.Ner.TextToken).Term, false, 0);
                    pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME, (tt.Next as Pullenti.Ner.TextToken).Term, false, 0);
                    Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(pers, t, tt.Next) { MiscAttrs = (int)FioTemplateType.SurnameII };
                    if (tt.Next.Next != null && tt.Next.Next.IsChar('.')) 
                        res.EndToken = tt.Next.Next;
                    res.Data = t.Kit.GetAnalyzerDataByAnalyzerName(Pullenti.Ner.Person.PersonAnalyzer.ANALYZER_NAME);
                    return res;
                }
                if (comma && tt != null && (tt.WhitespacesBeforeCount < 2)) 
                {
                    List<PersonItemToken> pits1 = PersonItemToken.TryAttachList(tt, null, ParseAttr.CanInitialBeDigit | ParseAttr.CanBeLatin, 10);
                    if (pits1 != null && pits1.Count > 0 && pits1[0].Typ == ItemType.Initial) 
                    {
                        if (prevPersTemplate != FioTemplateType.Undefined) 
                        {
                            if (prevPersTemplate != FioTemplateType.SurnameI && prevPersTemplate != FioTemplateType.SurnameII) 
                                return null;
                        }
                        Pullenti.Ner.Person.PersonReferent pers = new Pullenti.Ner.Person.PersonReferent();
                        pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, pits[0].Value, false, 0);
                        string nam = pits1[0].Value;
                        if (pits1[0].Chars.IsCyrillicLetter != pits[0].Chars.IsCyrillicLetter) 
                        {
                            char ch;
                            if (pits[0].Chars.IsCyrillicLetter) 
                                ch = Pullenti.Morph.LanguageHelper.GetCyrForLat(nam[0]);
                            else 
                                ch = Pullenti.Morph.LanguageHelper.GetLatForCyr(nam[0]);
                            if (ch != ((char)0)) 
                                nam = string.Format("{0}", ch);
                        }
                        pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, nam, false, 0);
                        Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(pers, t, pits1[0].EndToken) { MiscAttrs = (int)FioTemplateType.SurnameI };
                        if (pits1.Count > 1 && pits1[1].Typ == ItemType.Initial) 
                        {
                            string mid = pits1[1].Value;
                            if (pits1[1].Chars.IsCyrillicLetter != pits[0].Chars.IsCyrillicLetter) 
                            {
                                char ch;
                                if (pits[0].Chars.IsCyrillicLetter) 
                                    ch = Pullenti.Morph.LanguageHelper.GetCyrForLat(mid[0]);
                                else 
                                    ch = Pullenti.Morph.LanguageHelper.GetLatForCyr(mid[0]);
                                if (ch != ((char)0)) 
                                    mid = string.Format("{0}", ch);
                            }
                            pers.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME, mid, false, 0);
                            res.EndToken = pits1[1].EndToken;
                            res.MiscAttrs = (int)FioTemplateType.SurnameII;
                        }
                        res.Data = t.Kit.GetAnalyzerDataByAnalyzerName(Pullenti.Ner.Person.PersonAnalyzer.ANALYZER_NAME);
                        return res;
                    }
                }
            }
            return null;
        }
    }
}