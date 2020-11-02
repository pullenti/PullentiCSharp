/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Pullenti.Ner.Person.Internal
{
    public class PersonAttrToken : Pullenti.Ner.ReferentToken
    {
        public static void Initialize()
        {
            if (m_Termins != null) 
                return;
            PersonAttrTermin t;
            m_Termins = new Pullenti.Ner.Core.TerminCollection();
            m_Termins.Add(new PersonAttrTermin("ТОВАРИЩ") { Typ = PersonAttrTerminType.Prefix });
            m_Termins.Add(new PersonAttrTermin("ТОВАРИШ", Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Prefix });
            foreach (string s in new string[] {"ГОСПОДИН", "ГРАЖДАНИН", "УРОЖЕНЕЦ", "МИСТЕР", "СЭР", "СЕНЬОР", "МОНСЕНЬОР", "СИНЬОР", "МЕСЬЕ", "МСЬЕ", "ДОН", "МАЭСТРО", "МЭТР"}) 
            {
                t = new PersonAttrTermin(s) { Typ = PersonAttrTerminType.Prefix, Gender = Pullenti.Morph.MorphGender.Masculine };
                if (s == "ГРАЖДАНИН") 
                {
                    t.AddAbridge("ГР.");
                    t.AddAbridge("ГРАЖД.");
                    t.AddAbridge("ГР-Н");
                }
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"ПАН", "ГРОМАДЯНИН", "УРОДЖЕНЕЦЬ", "МІСТЕР", "СЕР", "СЕНЬЙОР", "МОНСЕНЬЙОР", "МЕСЬЄ", "МЕТР", "МАЕСТРО"}) 
            {
                t = new PersonAttrTermin(s, Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Prefix, Gender = Pullenti.Morph.MorphGender.Masculine };
                if (s == "ГРОМАДЯНИН") 
                {
                    t.AddAbridge("ГР.");
                    t.AddAbridge("ГР-Н");
                }
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"ГОСПОЖА", "ПАНИ", "ГРАЖДАНКА", "УРОЖЕНКА", "СЕНЬОРА", "СЕНЬОРИТА", "СИНЬОРА", "СИНЬОРИТА", "МИСС", "МИССИС", "МАДАМ", "МАДЕМУАЗЕЛЬ", "ФРАУ", "ФРОЙЛЯЙН", "ЛЕДИ", "ДОННА"}) 
            {
                t = new PersonAttrTermin(s) { Typ = PersonAttrTerminType.Prefix, Gender = Pullenti.Morph.MorphGender.Feminie };
                if (s == "ГРАЖДАНКА") 
                {
                    t.AddAbridge("ГР.");
                    t.AddAbridge("ГРАЖД.");
                    t.AddAbridge("ГР-КА");
                }
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"ПАНІ", "ГРОМАДЯНКА", "УРОДЖЕНКА", "СЕНЬЙОРА", "СЕНЬЙОРА", "МІС", "МІСІС", "МАДАМ", "МАДЕМУАЗЕЛЬ", "ФРАУ", "ФРОЙЛЯЙН", "ЛЕДІ"}) 
            {
                t = new PersonAttrTermin(s, Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Prefix, Gender = Pullenti.Morph.MorphGender.Feminie };
                if (s == "ГРОМАДЯНКА") 
                {
                    t.AddAbridge("ГР.");
                    t.AddAbridge("ГР-КА");
                }
                m_Termins.Add(t);
            }
            t = new PersonAttrTermin("MISTER", Pullenti.Morph.MorphLang.EN) { Typ = PersonAttrTerminType.Prefix, Gender = Pullenti.Morph.MorphGender.Masculine };
            t.AddAbridge("MR");
            t.AddAbridge("MR.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("MISSIS", Pullenti.Morph.MorphLang.EN) { Typ = PersonAttrTerminType.Prefix, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("MRS");
            t.AddAbridge("MSR.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("MISS", Pullenti.Morph.MorphLang.EN) { Typ = PersonAttrTerminType.Prefix, Gender = Pullenti.Morph.MorphGender.Feminie };
            t.AddAbridge("MS");
            t.AddAbridge("MS.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("БЕЗРАБОТНЫЙ") { Typ = PersonAttrTerminType.Position };
            t.AddVariant("НЕ РАБОТАЮЩИЙ", false);
            t.AddVariant("НЕ РАБОТАЕТ", false);
            t.AddVariant("ВРЕМЕННО НЕ РАБОТАЮЩИЙ", false);
            t.AddVariant("ВРЕМЕННО НЕ РАБОТАЕТ", false);
            m_Termins.Add(t);
            t = new PersonAttrTermin("БЕЗРОБІТНИЙ", Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Position };
            t.AddVariant("НЕ ПРАЦЮЮЧИЙ", false);
            t.AddVariant("НЕ ПРАЦЮЄ", false);
            t.AddVariant("ТИМЧАСОВО НЕ ПРАЦЮЮЧИЙ", false);
            t.AddVariant("ТИМЧАСОВО НЕ ПРАЦЮЄ", false);
            m_Termins.Add(t);
            t = new PersonAttrTermin("ЗАМЕСТИТЕЛЬ") { CanonicText = "заместитель", Typ2 = PersonAttrTerminType2.Io2, Typ = PersonAttrTerminType.Position };
            t.AddVariant("ЗАМЕСТИТЕЛЬНИЦА", false);
            t.AddAbridge("ЗАМ.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ЗАСТУПНИК", Pullenti.Morph.MorphLang.UA) { CanonicText = "заступник", Typ2 = PersonAttrTerminType2.Io2, Typ = PersonAttrTerminType.Position };
            t.AddVariant("ЗАСТУПНИЦЯ", false);
            t.AddAbridge("ЗАМ.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("УПОЛНОМОЧЕННЫЙ") { CanonicText = "уполномоченный", Typ2 = PersonAttrTerminType2.Io2, Typ = PersonAttrTerminType.Position };
            m_Termins.Add(t);
            t = new PersonAttrTermin("УПОВНОВАЖЕНИЙ", Pullenti.Morph.MorphLang.UA) { CanonicText = "уповноважений", Typ2 = PersonAttrTerminType2.Io2, Typ = PersonAttrTerminType.Position };
            m_Termins.Add(t);
            t = new PersonAttrTermin("ЭКС-УПОЛНОМОЧЕННЫЙ") { CanonicText = "экс-уполномоченный", Typ2 = PersonAttrTerminType2.Io2, Typ = PersonAttrTerminType.Position };
            m_Termins.Add(t);
            t = new PersonAttrTermin("ЕКС-УПОВНОВАЖЕНИЙ", Pullenti.Morph.MorphLang.UA) { CanonicText = "екс-уповноважений", Typ2 = PersonAttrTerminType2.Io2, Typ = PersonAttrTerminType.Position };
            m_Termins.Add(t);
            t = new PersonAttrTermin("ИСПОЛНЯЮЩИЙ ОБЯЗАННОСТИ") { Typ2 = PersonAttrTerminType2.Io, Typ = PersonAttrTerminType.Position };
            t.AddAbridge("И.О.");
            t.CanonicText = (t.Acronym = "ИО");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ВИКОНУЮЧИЙ ОБОВЯЗКИ", Pullenti.Morph.MorphLang.UA) { Typ2 = PersonAttrTerminType2.Io, Typ = PersonAttrTerminType.Position };
            t.AddAbridge("В.О.");
            t.CanonicText = (t.Acronym = "ВО");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ВРЕМЕННО ИСПОЛНЯЮЩИЙ ОБЯЗАННОСТИ") { Typ2 = PersonAttrTerminType2.Io, Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ВР.И.О.");
            t.CanonicText = (t.Acronym = "ВРИО");
            m_TerminVrio = t;
            m_Termins.Add(t);
            t = new PersonAttrTermin("ЗАВЕДУЮЩИЙ") { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ЗАВЕД.");
            t.AddAbridge("ЗАВ.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ЗАВІДУВАЧ", Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ЗАВІД.");
            t.AddAbridge("ЗАВ.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("СОТРУДНИК") { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("СОТРУДН.");
            t.AddAbridge("СОТР.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("СПІВРОБІТНИК", Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("СПІВРОБ.");
            t.AddAbridge("СПІВ.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("АКАДЕМИК") { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("АКАД.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("АКАДЕМІК", Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("АКАД.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ЧЛЕН-КОРРЕСПОНДЕНТ") { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ЧЛ.-КОРР.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ЧЛЕН-КОРЕСПОНДЕНТ", Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ЧЛ.-КОР.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ДОЦЕНТ") { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ДОЦ.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ПРОФЕССОР") { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ПРОФ.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ПРОФЕСОР", Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ПРОФ.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("PROFESSOR", Pullenti.Morph.MorphLang.EN) { Typ = PersonAttrTerminType.Position };
            t.AddAbridge("PROF.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("КАНДИДАТ") { Typ2 = PersonAttrTerminType2.Grade, Typ = PersonAttrTerminType.Position };
            t.AddAbridge("КАНД.");
            t.AddAbridge("КАН.");
            t.AddAbridge("К-Т");
            t.AddAbridge("К.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ДОКТОР") { Typ2 = PersonAttrTerminType2.Grade, Typ = PersonAttrTerminType.Position };
            t.AddAbridge("ДОКТ.");
            t.AddAbridge("ДОК.");
            t.AddAbridge("Д-Р");
            t.AddAbridge("Д.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("DOCTOR", Pullenti.Morph.MorphLang.EN) { Typ = PersonAttrTerminType.Prefix };
            t.AddAbridge("DR");
            t.AddAbridge("DR.");
            m_Termins.Add(t);
            t = new PersonAttrTermin("ДОКТОРАНТ") { Typ = PersonAttrTerminType.Position };
            m_Termins.Add(t);
            t = new PersonAttrTermin("ДОКТОРАНТ", Pullenti.Morph.MorphLang.UA) { Typ = PersonAttrTerminType.Position };
            m_Termins.Add(t);
            foreach (string s in new string[] {"КФН", "КТН", "КХН"}) 
            {
                t = new PersonAttrTermin(s) { CanonicText = "кандидат наук", Typ = PersonAttrTerminType.Position, Typ2 = PersonAttrTerminType2.Abbr };
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"ГЛАВНЫЙ", "МЛАДШИЙ", "СТАРШИЙ", "ВЕДУЩИЙ", "НАУЧНЫЙ"}) 
            {
                t = new PersonAttrTermin(s) { Typ2 = PersonAttrTerminType2.Adj, Typ = PersonAttrTerminType.Position };
                t.AddAllAbridges(0, 0, 2);
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"ГОЛОВНИЙ", "МОЛОДШИЙ", "СТАРШИЙ", "ПРОВІДНИЙ", "НАУКОВИЙ"}) 
            {
                t = new PersonAttrTermin(s) { Typ2 = PersonAttrTerminType2.Adj, Typ = PersonAttrTerminType.Position, Lang = Pullenti.Morph.MorphLang.UA };
                t.AddAllAbridges(0, 0, 2);
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"НЫНЕШНИЙ", "НОВЫЙ", "CURRENT", "NEW"}) 
            {
                t = new PersonAttrTermin(s) { Typ2 = PersonAttrTerminType2.IgnoredAdj, Typ = PersonAttrTerminType.Position };
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"НИНІШНІЙ", "НОВИЙ"}) 
            {
                t = new PersonAttrTermin(s) { Typ2 = PersonAttrTerminType2.IgnoredAdj, Typ = PersonAttrTerminType.Position, Lang = Pullenti.Morph.MorphLang.UA };
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"ТОГДАШНИЙ", "БЫВШИЙ", "ПРЕДЫДУЩИЙ", "FORMER", "PREVIOUS", "THEN"}) 
            {
                t = new PersonAttrTermin(s) { Typ2 = PersonAttrTerminType2.Io, Typ = PersonAttrTerminType.Position };
                m_Termins.Add(t);
            }
            foreach (string s in new string[] {"ТОДІШНІЙ", "КОЛИШНІЙ"}) 
            {
                t = new PersonAttrTermin(s) { Typ2 = PersonAttrTerminType2.Io, Typ = PersonAttrTerminType.Position, Lang = Pullenti.Morph.MorphLang.UA };
                m_Termins.Add(t);
            }
            byte[] dat = ResourceHelper.GetBytes("attr_ru.dat");
            if (dat == null) 
                throw new Exception("Not found resource file attr_ru.dat in Person analyzer");
            LoadAttrs(m_Termins, dat, Pullenti.Morph.MorphLang.RU);
            if ((((dat = ResourceHelper.GetBytes("attr_en.dat")))) == null) 
                throw new Exception("Not found resource file attr_en.dat in Person analyzer");
            LoadAttrs(m_Termins, dat, Pullenti.Morph.MorphLang.EN);
            LoadAttrs(m_Termins, ResourceHelper.GetBytes("attr_ua.dat"), Pullenti.Morph.MorphLang.UA);
        }
        static Pullenti.Ner.Core.TerminCollection m_Termins;
        static PersonAttrTermin m_TerminVrio;
        static byte[] Deflate(byte[] zip)
        {
            using (MemoryStream unzip = new MemoryStream()) 
            {
                MemoryStream data = new MemoryStream(zip);
                data.Position = 0;
                Pullenti.Morph.Internal.MorphDeserializer.DeflateGzip(data, unzip);
                data.Dispose();
                return unzip.ToArray();
            }
        }
        static void LoadAttrs(Pullenti.Ner.Core.TerminCollection termins, byte[] dat, Pullenti.Morph.MorphLang lang)
        {
            if (dat == null || dat.Length == 0) 
                return;
            using (MemoryStream tmp = new MemoryStream(Deflate(dat))) 
            {
                tmp.Position = 0;
                XmlDocument xml = new XmlDocument();
                xml.Load(tmp);
                foreach (XmlNode x in xml.DocumentElement.ChildNodes) 
                {
                    XmlAttribute a = x.Attributes["v"];
                    if (a == null) 
                        continue;
                    string val = a.Value;
                    if (val == null) 
                        continue;
                    string attrs = (x.Attributes["a"] == null ? "" : (x.Attributes["a"].InnerText ?? ""));
                    if (val == "ОТЕЦ") 
                    {
                    }
                    PersonAttrTermin pat = new PersonAttrTermin(val) { Typ = PersonAttrTerminType.Position, Lang = lang };
                    foreach (char ch in attrs) 
                    {
                        if (ch == 'p') 
                            pat.CanHasPersonAfter = 1;
                        else if (ch == 'P') 
                            pat.CanHasPersonAfter = 2;
                        else if (ch == 's') 
                            pat.CanBeSameSurname = true;
                        else if (ch == 'm') 
                            pat.Gender = Pullenti.Morph.MorphGender.Masculine;
                        else if (ch == 'f') 
                            pat.Gender = Pullenti.Morph.MorphGender.Feminie;
                        else if (ch == 'b') 
                            pat.IsBoss = true;
                        else if (ch == 'r') 
                            pat.IsMilitaryRank = true;
                        else if (ch == 'n') 
                            pat.IsNation = true;
                        else if (ch == 'c') 
                            pat.Typ = PersonAttrTerminType.King;
                        else if (ch == 'q') 
                            pat.Typ = PersonAttrTerminType.King;
                        else if (ch == 'k') 
                            pat.IsKin = true;
                        else if (ch == 'a') 
                            pat.Typ2 = PersonAttrTerminType2.Io2;
                        else if (ch == '1') 
                            pat.CanBeIndependant = true;
                        else if (ch == '?') 
                            pat.IsDoubt = true;
                    }
                    if (x.Attributes["alt"] != null) 
                    {
                        pat.AddVariant((val = x.Attributes["alt"].InnerText), false);
                        if (val.IndexOf('.') > 0) 
                            pat.AddAbridge(val);
                    }
                    if (x.ChildNodes.Count > 0) 
                    {
                        foreach (XmlNode xx in x.ChildNodes) 
                        {
                            if (xx.Name == "alt") 
                            {
                                pat.AddVariant((val = xx.InnerText), false);
                                if (val.IndexOf('.') > 0) 
                                    pat.AddAbridge(val);
                            }
                        }
                    }
                    termins.Add(pat);
                }
            }
        }
        public PersonAttrToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(null, begin, end, null)
        {
        }
        public PersonAttrTerminType Typ;
        public Pullenti.Morph.MorphGender Gender;
        public string Value;
        /// <summary>
        /// Например, патриарх Московский и всея Руси - это будет Московский
        /// </summary>
        internal PersonItemToken KingSurname;
        /// <summary>
        /// ВОзраст
        /// </summary>
        public string Age;
        public Pullenti.Ner.Person.PersonPropertyReferent PropRef
        {
            get
            {
                return Referent as Pullenti.Ner.Person.PersonPropertyReferent;
            }
            set
            {
                Referent = value;
            }
        }
        public PersonAttrToken HigherPropRef;
        public bool AddOuterOrgAsRef = false;
        public Pullenti.Ner.Token Anafor;
        public bool CanBeIndependentProperty
        {
            get
            {
                if (PropRef == null) 
                    return false;
                if (Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                    return false;
                if (HigherPropRef != null && HigherPropRef.CanBeIndependentProperty) 
                    return true;
                if (CanBeSinglePerson) 
                    return true;
                if (Typ != PersonAttrTerminType.Position) 
                    return false;
                if (!m_CanBeIndependentProperty) 
                {
                    if (PropRef.Kind == Pullenti.Ner.Person.PersonPropertyKind.Boss) 
                        return true;
                    return false;
                }
                if (PropRef.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) != null) 
                {
                    if (PropRef.Name != "член") 
                        return true;
                }
                return false;
            }
            set
            {
                m_CanBeIndependentProperty = value;
            }
        }
        bool m_CanBeIndependentProperty;
        public bool CanBeSinglePerson;
        public int CanHasPersonAfter = 0;
        public bool CanBeSameSurname;
        /// <summary>
        /// Сомнительный атрибут
        /// </summary>
        public bool IsDoubt;
        public override string ToString()
        {
            if (Referent != null) 
                return base.ToString();
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0}: {1}", Typ.ToString(), Value ?? "");
            if (PropRef != null) 
                res.AppendFormat(" Ref: {0}", PropRef.ToString());
            if (Gender != Pullenti.Morph.MorphGender.Undefined) 
                res.AppendFormat("; {0}", Gender);
            if (CanHasPersonAfter >= 0) 
                res.AppendFormat("; MayBePersonAfter={0}", CanHasPersonAfter);
            if (CanBeSameSurname) 
                res.Append("; CanHasLikeSurname");
            if (m_CanBeIndependentProperty) 
                res.Append("; CanBeIndependent");
            if (IsDoubt) 
                res.Append("; Doubt");
            if (Age != null) 
                res.AppendFormat("; Age={0}", Age);
            if (!Morph.Case.IsUndefined) 
                res.AppendFormat("; {0}", Morph.Case.ToString());
            return res.ToString();
        }
        public override void SaveToLocalOntology()
        {
            Pullenti.Ner.Core.AnalyzerData ad = Data;
            if (ad == null || PropRef == null || HigherPropRef == null) 
            {
                base.SaveToLocalOntology();
                return;
            }
            List<PersonAttrToken> li = new List<PersonAttrToken>();
            for (PersonAttrToken pr = this; pr != null && pr.PropRef != null; pr = pr.HigherPropRef) 
            {
                li.Insert(0, pr);
            }
            for (int i = 0; i < li.Count; i++) 
            {
                li[i].Data = ad;
                li[i].HigherPropRef = null;
                li[i].SaveToLocalOntology();
                if ((i + 1) < li.Count) 
                    li[i + 1].PropRef.Higher = li[i].PropRef;
            }
        }
        public enum PersonAttrAttachAttrs : int
        {
            No = 0,
            AfterZamestitel = 1,
            OnlyKeyword = 2,
            InProcess = 4,
        }

        public static PersonAttrToken TryAttach(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locOnto, PersonAttrAttachAttrs attrs = PersonAttrAttachAttrs.No)
        {
            if (t == null) 
                return null;
            object olev = null;
            int lev = 0;
            if (!t.Kit.MiscData.TryGetValue("pat", out olev)) 
                t.Kit.MiscData.Add("pat", (lev = 1));
            else 
            {
                lev = (int)olev;
                if (lev > 2) 
                    return null;
                lev++;
                t.Kit.MiscData["pat"] = lev;
            }
            PersonAttrToken res = _TryAttach(t, locOnto, attrs);
            lev--;
            if (lev < 0) 
                lev = 0;
            t.Kit.MiscData["pat"] = lev;
            if (res == null) 
            {
                if (t.Morph.Class.IsNoun) 
                {
                    Pullenti.Ner.Geo.GeoAnalyzer aterr = t.Kit.Processor.FindAnalyzer("GEO") as Pullenti.Ner.Geo.GeoAnalyzer;
                    if (aterr != null) 
                    {
                        Pullenti.Ner.ReferentToken rt = aterr.ProcessCitizen(t);
                        if (rt != null) 
                        {
                            res = new PersonAttrToken(rt.BeginToken, rt.EndToken) { Morph = rt.Morph };
                            res.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent();
                            res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_NAME, (t.Kit.BaseLanguage.IsUa ? "громадянин" : "гражданин"), true, 0);
                            res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, rt.Referent, true, 0);
                            res.PropRef.AddExtReferent(rt);
                            res.Typ = PersonAttrTerminType.Position;
                            if ((res.EndToken.Next != null && res.EndToken.Next.IsValue("ПО", null) && res.EndToken.Next.Next != null) && res.EndToken.Next.Next.IsValue("ПРОИСХОЖДЕНИЕ", null)) 
                                res.EndToken = res.EndToken.Next.Next;
                            return res;
                        }
                    }
                }
                if ((((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).Term == "АК" && t.Next != null) && t.Next.IsChar('.') && t.Next.Next != null) && !t.Next.Next.Chars.IsAllLower) 
                {
                    res = new PersonAttrToken(t, t.Next) { Typ = PersonAttrTerminType.Position };
                    res.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent() { Name = "академик" };
                    return res;
                }
                if ((t is Pullenti.Ner.TextToken) && t.Next != null) 
                {
                    if (((t.IsValue("ВИЦЕ", "ВІЦЕ") || t.IsValue("ЭКС", "ЕКС") || t.IsValue("ГЕН", null)) || t.IsValue("VICE", null) || t.IsValue("EX", null)) || t.IsValue("DEPUTY", null)) 
                    {
                        Pullenti.Ner.Token tt = t.Next;
                        if (tt.IsHiphen || tt.IsChar('.')) 
                            tt = tt.Next;
                        res = _TryAttach(tt, locOnto, attrs);
                        if (res != null && res.PropRef != null) 
                        {
                            res.BeginToken = t;
                            if (t.IsValue("ГЕН", null)) 
                                res.PropRef.Name = string.Format("генеральный {0}", res.PropRef.Name);
                            else 
                                res.PropRef.Name = string.Format("{0}-{1}", (t as Pullenti.Ner.TextToken).Term.ToLower(), res.PropRef.Name);
                            return res;
                        }
                    }
                }
                if (t.IsValue("ГВАРДИИ", "ГВАРДІЇ")) 
                {
                    res = _TryAttach(t.Next, locOnto, attrs);
                    if (res != null) 
                    {
                        if (res.PropRef != null && res.PropRef.Kind == Pullenti.Ner.Person.PersonPropertyKind.MilitaryRank) 
                        {
                            res.BeginToken = t;
                            return res;
                        }
                    }
                }
                Pullenti.Ner.Token tt1 = t;
                if (tt1.Morph.Class.IsPreposition && tt1.Next != null) 
                    tt1 = tt1.Next;
                if ((tt1.Next != null && tt1.IsValue("НАЦИОНАЛЬНОСТЬ", "НАЦІОНАЛЬНІСТЬ")) || tt1.IsValue("ПРОФЕССИЯ", "ПРОФЕСІЯ") || tt1.IsValue("СПЕЦИАЛЬНОСТЬ", "СПЕЦІАЛЬНІСТЬ")) 
                {
                    tt1 = tt1.Next;
                    if (tt1 != null) 
                    {
                        if (tt1.IsHiphen || tt1.IsChar(':')) 
                            tt1 = tt1.Next;
                    }
                    res = _TryAttach(tt1, locOnto, attrs);
                    if (res != null) 
                    {
                        res.BeginToken = t;
                        return res;
                    }
                }
                return null;
            }
            if (res.Typ == PersonAttrTerminType.Other && res.Age != null && res.Value == null) 
            {
                PersonAttrToken res1 = _TryAttach(res.EndToken.Next, locOnto, attrs);
                if (res1 != null) 
                {
                    res1.BeginToken = res.BeginToken;
                    res1.Age = res.Age;
                    res = res1;
                }
            }
            if (res.BeginToken.IsValue("ГЛАВА", null)) 
            {
                if (t.Previous is Pullenti.Ner.NumberToken) 
                    return null;
            }
            else if (res.BeginToken.IsValue("АДВОКАТ", null)) 
            {
                if (t.Previous != null) 
                {
                    if (t.Previous.IsValue("РЕЕСТР", "РЕЄСТР") || t.Previous.IsValue("УДОСТОВЕРЕНИЕ", "ПОСВІДЧЕННЯ")) 
                        return null;
                }
            }
            Pullenti.Morph.MorphClass mc = res.BeginToken.GetMorphClassInDictionary();
            if (mc.IsAdjective) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.EndChar > res.EndChar) 
                {
                    if (m_Termins.TryParse(npt.EndToken, Pullenti.Ner.Core.TerminParseAttr.No) == null && npt.EndToken.Chars.IsAllLower) 
                        return null;
                }
            }
            if (res.Typ == PersonAttrTerminType.Prefix && (((((res.Value == "ГРАЖДАНИН" || res.Value == "ГРАЖДАНКА" || res.Value == "УРОЖЕНЕЦ") || res.Value == "УРОЖЕНКА" || res.Value == "ГРОМАДЯНИН") || res.Value == "ГРОМАДЯНКА" || res.Value == "УРОДЖЕНЕЦЬ") || res.Value == "УРОДЖЕНКА")) && res.EndToken.Next != null) 
            {
                Pullenti.Ner.Token tt = res.EndToken.Next;
                if (((tt != null && tt.IsChar('(') && tt.Next != null) && tt.Next.IsValue("КА", null) && tt.Next.Next != null) && tt.Next.Next.IsChar(')')) 
                {
                    res.EndToken = tt.Next.Next;
                    tt = res.EndToken.Next;
                }
                Pullenti.Ner.Referent r = (tt == null ? null : tt.GetReferent());
                if (r != null && r.TypeName == ObjNameGeo) 
                {
                    res.EndToken = tt;
                    res.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent();
                    res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_NAME, res.Value.ToLower(), true, 0);
                    res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, true, 0);
                    res.Typ = PersonAttrTerminType.Position;
                    for (Pullenti.Ner.Token ttt = tt.Next; ttt != null; ttt = ttt.Next) 
                    {
                        if (!ttt.IsCommaAnd || ttt.Next == null) 
                            break;
                        ttt = ttt.Next;
                        r = ttt.GetReferent();
                        if (r == null || r.TypeName != ObjNameGeo) 
                            break;
                        res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, false, 0);
                        res.EndToken = (tt = ttt);
                        if (ttt.Previous.IsAnd) 
                            break;
                    }
                    if (((res.EndToken.Next is Pullenti.Ner.ReferentToken) && (res.WhitespacesAfterCount < 3) && res.EndToken.Next.GetReferent() != null) && res.EndToken.Next.GetReferent().TypeName == ObjNameGeo) 
                    {
                        if (Pullenti.Ner.Geo.Internal.GeoOwnerHelper.CanBeHigher(r as Pullenti.Ner.Geo.GeoReferent, res.EndToken.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent)) 
                        {
                            res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, res.EndToken.Next.GetReferent(), false, 0);
                            res.EndToken = res.EndToken.Next;
                        }
                    }
                }
                else if ((tt != null && tt.IsAnd && tt.Next != null) && tt.Next.IsValue("ЖИТЕЛЬ", null)) 
                {
                    PersonAttrToken aaa = _TryAttach(tt.Next, locOnto, attrs);
                    if (aaa != null && aaa.PropRef != null) 
                    {
                        aaa.BeginToken = res.BeginToken;
                        aaa.Value = res.Value;
                        aaa.PropRef.Name = aaa.Value.ToLower();
                        res = aaa;
                    }
                }
                else 
                {
                    Pullenti.Ner.Token tt2 = tt;
                    if (tt2.IsCommaAnd) 
                        tt2 = tt2.Next;
                    PersonAttrToken nex = _TryAttach(tt2, locOnto, attrs);
                    if (nex != null && nex.PropRef != null) 
                    {
                        foreach (Pullenti.Ner.Slot sss in nex.PropRef.Slots) 
                        {
                            if (sss.Value is Pullenti.Ner.Geo.GeoReferent) 
                            {
                                if (res.PropRef == null) 
                                    res.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent();
                                res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_NAME, res.Value.ToLower(), false, 0);
                                res.PropRef.AddSlot(sss.TypeName, sss.Value, false, 0);
                                res.Typ = PersonAttrTerminType.Position;
                            }
                        }
                    }
                }
            }
            if (res.Typ == PersonAttrTerminType.King || res.Typ == PersonAttrTerminType.Position) 
            {
                if (res.BeginToken == res.EndToken && res.Chars.IsCapitalUpper && res.WhitespacesAfterCount == 1) 
                {
                    PersonItemToken pit = PersonItemToken.TryAttach(t, locOnto, PersonItemToken.ParseAttr.IgnoreAttrs, null);
                    if (pit != null && pit.Lastname != null && pit.Lastname.IsLastnameHasStdTail) 
                    {
                        Pullenti.Ner.ReferentToken rt1 = t.Kit.ProcessReferent("PERSON", t.Next);
                        if (rt1 != null && (rt1.Referent is Pullenti.Ner.Person.PersonReferent)) 
                        {
                        }
                        else if (((attrs & PersonAttrAttachAttrs.InProcess)) != PersonAttrAttachAttrs.No) 
                        {
                        }
                        else 
                            return null;
                    }
                }
            }
            if (res.PropRef == null) 
                return res;
            if (res.Chars.IsLatinLetter) 
            {
                Pullenti.Ner.Token tt = res.EndToken.Next;
                if (tt != null && tt.IsHiphen) 
                    tt = tt.Next;
                if (tt != null && tt.IsValue("ELECT", null)) 
                    res.EndToken = tt;
            }
            if (!res.BeginToken.Chars.IsAllLower) 
            {
                PersonItemToken pat = PersonItemToken.TryAttach(res.BeginToken, locOnto, PersonItemToken.ParseAttr.IgnoreAttrs, null);
                if (pat != null && pat.Lastname != null) 
                {
                    if (pat.Lastname.IsInDictionary || pat.Lastname.IsInOntology) 
                    {
                        if (CheckKind(res.PropRef) != Pullenti.Ner.Person.PersonPropertyKind.King) 
                            return null;
                    }
                }
            }
            string s = res.PropRef.ToString();
            if (s == "глава книги") 
                return null;
            if (s == "глава" && res.PropRef.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) == null) 
                return null;
            if (((s == "королева" || s == "король" || s == "князь")) && res.Chars.IsCapitalUpper) 
            {
                List<PersonItemToken> pits = PersonItemToken.TryAttachList(res.EndToken.Next, locOnto, PersonItemToken.ParseAttr.No, 10);
                if (pits != null && pits.Count > 0) 
                {
                    if (pits[0].Typ == PersonItemToken.ItemType.Initial) 
                        return null;
                    if (pits[0].Firstname != null) 
                    {
                        if (pits.Count == 1) 
                            return null;
                        if (pits.Count == 2 && pits[1].Middlename != null) 
                            return null;
                    }
                }
                if (!Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                    return null;
            }
            if (s == "друг" || s.StartsWith("друг ")) 
            {
                if (t.Previous != null) 
                {
                    if (t.Previous.IsValue("ДРУГ", null)) 
                        return null;
                    if (t.Previous.Morph.Class.IsPreposition && t.Previous.Previous != null && t.Previous.Previous.IsValue("ДРУГ", null)) 
                        return null;
                }
                if (t.Next != null) 
                {
                    if (t.Next.IsValue("ДРУГ", null)) 
                        return null;
                    if (t.Next.Morph.Class.IsPreposition && t.Next.Next != null && t.Next.Next.IsValue("ДРУГ", null)) 
                        return null;
                }
            }
            if (res.Chars.IsLatinLetter && ((res.IsDoubt || s == "senior")) && (res.WhitespacesAfterCount < 2)) 
            {
                if (res.PropRef != null && res.PropRef.Slots.Count == 1) 
                {
                    Pullenti.Ner.Token tt2 = res.EndToken.Next;
                    if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(tt2)) 
                        tt2 = tt2.Next.Next;
                    PersonAttrToken res2 = _TryAttach(tt2, locOnto, attrs);
                    if ((res2 != null && res2.Chars.IsLatinLetter && res2.Typ == res.Typ) && res2.PropRef != null) 
                    {
                        res2.PropRef.Name = string.Format("{0} {1}", res.PropRef.Name ?? "", res2.PropRef.Name ?? "").Trim();
                        res2.BeginToken = res.BeginToken;
                        res = res2;
                    }
                }
            }
            if (res.PropRef.Name == "министр") 
            {
                Pullenti.Ner.ReferentToken rt1 = res.Kit.ProcessReferent("ORGANIZATION", res.EndToken.Next);
                if (rt1 != null && rt1.Referent.FindSlot("TYPE", "министерство", true) != null) 
                {
                    Pullenti.Ner.Token t1 = rt1.EndToken;
                    if (t1.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                        t1 = t1.Previous;
                    if (rt1.BeginChar < t1.EndChar) 
                    {
                        string addStr = Pullenti.Ner.Core.MiscHelper.GetTextValue(rt1.BeginToken, t1, Pullenti.Ner.Core.GetTextAttr.No);
                        if (addStr != null) 
                        {
                            res.PropRef.Name += (" " + addStr.ToLower());
                            res.EndToken = t1;
                        }
                    }
                }
            }
            for (Pullenti.Ner.Person.PersonPropertyReferent p = res.PropRef; p != null; p = p.Higher) 
            {
                if (p.Name != null && p.Name.Contains(" - ")) 
                    p.Name = p.Name.Replace(" - ", "-");
            }
            if (res.BeginToken.Morph.Class.IsAdjective) 
            {
                Pullenti.Ner.ReferentToken r = res.Kit.ProcessReferent("GEO", res.BeginToken);
                if (r != null) 
                {
                    res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r.Referent, false, 0);
                    res.PropRef.AddExtReferent(r);
                    int i = res.PropRef.Name.IndexOf(' ');
                    if (i > 0) 
                        res.PropRef.Name = res.PropRef.Name.Substring(i).Trim();
                }
            }
            bool containsGeo = false;
            foreach (Pullenti.Ner.Slot ss in res.PropRef.Slots) 
            {
                if (ss.Value is Pullenti.Ner.Referent) 
                {
                    if ((ss.Value as Pullenti.Ner.Referent).TypeName == ObjNameGeo) 
                    {
                        containsGeo = true;
                        break;
                    }
                }
            }
            if (!containsGeo && (res.EndToken.WhitespacesAfterCount < 2)) 
            {
                if ((res.EndToken.Next is Pullenti.Ner.ReferentToken) && res.EndToken.Next.GetReferent().TypeName == ObjNameGeo) 
                {
                    res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, res.EndToken.Next.GetReferent(), false, 0);
                    res.EndToken = res.EndToken.Next;
                }
            }
            if (res.EndToken.WhitespacesAfterCount < 2) 
            {
                Pullenti.Ner.Token te = res.EndToken.Next;
                if (te != null && te.IsValue("В", null)) 
                {
                    te = te.Next;
                    if ((te is Pullenti.Ner.ReferentToken) && ((te.GetReferent().TypeName == ObjNameDate || te.GetReferent().TypeName == ObjNameDateRange))) 
                        res.EndToken = te;
                }
                else if (te != null && te.IsChar('(')) 
                {
                    te = te.Next;
                    if (((te is Pullenti.Ner.ReferentToken) && ((te.GetReferent().TypeName == ObjNameDate || te.GetReferent().TypeName == ObjNameDateRange)) && te.Next != null) && te.Next.IsChar(')')) 
                        res.EndToken = te.Next;
                    else if (te is Pullenti.Ner.NumberToken) 
                    {
                        Pullenti.Ner.ReferentToken rt1 = te.Kit.ProcessReferent("DATE", te);
                        if (rt1 != null && rt1.EndToken.Next != null && rt1.EndToken.Next.IsChar(')')) 
                            res.EndToken = rt1.EndToken.Next;
                    }
                }
            }
            if (res.PropRef != null && res.PropRef.Name == "отец") 
            {
                bool isKing = false;
                Pullenti.Ner.Token tt = res.EndToken.Next;
                if ((tt is Pullenti.Ner.TextToken) && tt.GetMorphClassInDictionary().IsProperName) 
                {
                    if (!((res.Morph.Case & tt.Morph.Case)).IsUndefined) 
                    {
                        if (!tt.Morph.Case.IsGenitive) 
                            isKing = true;
                    }
                }
                if (isKing) 
                    res.PropRef.Name = "священник";
            }
            if (res.PropRef != null && res.PropRef.Kind == Pullenti.Ner.Person.PersonPropertyKind.King) 
            {
                Pullenti.Ner.Token t1 = res.EndToken.Next;
                if (res.PropRef.Name == "отец") 
                {
                    if (t1 == null || !t1.Chars.IsCapitalUpper) 
                        return null;
                    if (((res.Morph.Case & t1.Morph.Case)).IsUndefined) 
                        return null;
                    res.PropRef.Name = "священник";
                    return res;
                }
                if (t1 != null && t1.Chars.IsCapitalUpper && t1.Morph.Class.IsAdjective) 
                {
                    if ((((res.KingSurname = PersonItemToken.TryAttach(t1, locOnto, PersonItemToken.ParseAttr.IgnoreAttrs, null)))) != null) 
                    {
                        res.EndToken = t1;
                        if ((t1.Next != null && t1.Next.IsAnd && t1.Next.Next != null) && t1.Next.Next.IsValue("ВСЕЯ", null)) 
                        {
                            t1 = t1.Next.Next.Next;
                            Pullenti.Ner.Geo.GeoReferent geo = ((t1 == null ? null : t1.GetReferent())) as Pullenti.Ner.Geo.GeoReferent;
                            if (geo != null) 
                            {
                                res.EndToken = t1;
                                res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, geo, false, 0);
                            }
                        }
                    }
                }
            }
            if (res.CanHasPersonAfter > 0 && res.PropRef.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) == null) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    {
                        Pullenti.Ner.Token tt0 = res.BeginToken;
                        bool have = false;
                        if ((tt0 is Pullenti.Ner.TextToken) && tt0.Morph.Class.IsPersonalPronoun && ((tt0.IsValue("ОН", null) || tt0.IsValue("ОНА", null)))) 
                        {
                        }
                        else 
                        {
                            tt0 = tt0.Previous;
                            if ((tt0 is Pullenti.Ner.TextToken) && tt0.Morph.Class.IsPersonalPronoun && ((tt0.IsValue("ОН", null) || tt0.IsValue("ОНА", null)))) 
                            {
                            }
                            else if ((tt0 is Pullenti.Ner.TextToken) && tt0.Morph.Class.IsPronoun && tt0.IsValue("СВОЙ", null)) 
                            {
                            }
                            else if ((tt0 is Pullenti.Ner.TextToken) && ((tt0.IsValue("ИМЕТЬ", null) || (tt0 as Pullenti.Ner.TextToken).IsVerbBe))) 
                                have = true;
                            else 
                                tt0 = null;
                        }
                        if (tt0 != null) 
                        {
                            Pullenti.Morph.MorphGender gen = Pullenti.Morph.MorphGender.Undefined;
                            int cou = 0;
                            if (!have) 
                            {
                                foreach (Pullenti.Morph.MorphBaseInfo wf in tt0.Morph.Items) 
                                {
                                    if (wf.Class.IsPersonalPronoun || wf.Class.IsPronoun) 
                                    {
                                        if ((((gen = wf.Gender))) == Pullenti.Morph.MorphGender.Neuter) 
                                            gen = Pullenti.Morph.MorphGender.Masculine;
                                        break;
                                    }
                                }
                            }
                            for (Pullenti.Ner.Token tt = tt0.Previous; tt != null && (cou < 200); tt = tt.Previous,cou++) 
                            {
                                Pullenti.Ner.Person.PersonPropertyReferent pr = tt.GetReferent() as Pullenti.Ner.Person.PersonPropertyReferent;
                                if (pr != null) 
                                {
                                    if (((tt.Morph.Gender & gen)) == Pullenti.Morph.MorphGender.Undefined) 
                                        continue;
                                    break;
                                }
                                Pullenti.Ner.Person.PersonReferent p = tt.GetReferent() as Pullenti.Ner.Person.PersonReferent;
                                if (p == null) 
                                    continue;
                                if (have && (cou < 10)) 
                                {
                                }
                                else if (gen == Pullenti.Morph.MorphGender.Feminie) 
                                {
                                    if (p.IsMale && !p.IsFemale) 
                                        continue;
                                }
                                else if (gen == Pullenti.Morph.MorphGender.Masculine) 
                                {
                                    if (p.IsFemale && !p.IsMale) 
                                        continue;
                                }
                                else 
                                    break;
                                res.BeginToken = (have ? tt0.Next : tt0);
                                res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, p, false, 0);
                                res.CanBeIndependentProperty = true;
                                if (res.Morph.Number != Pullenti.Morph.MorphNumber.Plural) 
                                    res.CanBeSinglePerson = true;
                                npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt0, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt != null && npt.BeginToken != npt.EndToken) 
                                    res.Morph = npt.Morph;
                                break;
                            }
                        }
                        else if (res.WhitespacesAfterCount == 1) 
                        {
                            Pullenti.Ner.Person.PersonAnalyzer pa = res.Kit.Processor.FindAnalyzer("PERSON") as Pullenti.Ner.Person.PersonAnalyzer;
                            if (pa != null) 
                            {
                                Pullenti.Ner.Token t1 = res.EndToken.Next;
                                Pullenti.Ner.ReferentToken pr = Pullenti.Ner.Person.PersonAnalyzer.TryAttachPerson(t1, res.Kit.GetAnalyzerData(pa) as Pullenti.Ner.Person.PersonAnalyzer.PersonAnalyzerData, false, 0, true);
                                if (pr != null && res.CanHasPersonAfter == 1) 
                                {
                                    if (pr.BeginToken == t1) 
                                    {
                                        if (!pr.Morph.Case.IsGenitive && !pr.Morph.Case.IsUndefined) 
                                            pr = null;
                                        else if (!pr.Morph.Case.IsUndefined && !((res.Morph.Case & pr.Morph.Case)).IsUndefined) 
                                        {
                                            if (Pullenti.Ner.Person.PersonAnalyzer.TryAttachPerson(pr.EndToken.Next, res.Kit.GetAnalyzerData(pa) as Pullenti.Ner.Person.PersonAnalyzer.PersonAnalyzerData, false, 0, true) != null) 
                                            {
                                            }
                                            else 
                                                pr = null;
                                        }
                                    }
                                    else if (pr.BeginToken.Previous == t1) 
                                    {
                                        pr = null;
                                        res.PropRef.Name = string.Format("{0} {1}", res.PropRef.Name, t1.GetSourceText().ToLower());
                                        res.EndToken = t1;
                                    }
                                    else 
                                        pr = null;
                                }
                                else if (pr != null && res.CanHasPersonAfter == 2) 
                                {
                                    List<PersonItemToken> pits = PersonItemToken.TryAttachList(t1, null, PersonItemToken.ParseAttr.No, 10);
                                    if (((pits != null && pits.Count > 1 && pits[0].Firstname != null) && pits[1].Firstname != null && pr.EndChar > pits[0].EndChar) && pits[0].Morph.Case.IsGenitive) 
                                    {
                                        pr = null;
                                        int cou = 100;
                                        for (Pullenti.Ner.Token tt = t1.Previous; tt != null && cou > 0; tt = tt.Previous,cou--) 
                                        {
                                            Pullenti.Ner.Person.PersonReferent p0 = tt.GetReferent() as Pullenti.Ner.Person.PersonReferent;
                                            if (p0 == null) 
                                                continue;
                                            foreach (PersonItemToken.MorphPersonItemVariant v in pits[0].Firstname.Vars) 
                                            {
                                                if (p0.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, v.Value, true) != null) 
                                                {
                                                    pr = new Pullenti.Ner.ReferentToken(p0, t1, pits[0].EndToken);
                                                    break;
                                                }
                                            }
                                            if (pr != null) 
                                                break;
                                        }
                                    }
                                }
                                if (pr != null) 
                                {
                                    res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, pr, false, 0);
                                    res.EndToken = pr.EndToken;
                                    res.CanBeIndependentProperty = true;
                                    if (res.Morph.Number != Pullenti.Morph.MorphNumber.Plural) 
                                        res.CanBeSinglePerson = true;
                                }
                            }
                        }
                    }
            }
            if (res.PropRef.Higher == null && res.PropRef.Kind == Pullenti.Ner.Person.PersonPropertyKind.Boss && res.PropRef.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) == null) 
            {
                Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(res.BeginToken, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null && tok.EndToken == res.EndToken) 
                {
                    int cou = 0;
                    List<Pullenti.Ner.Referent> refs = new List<Pullenti.Ner.Referent>();
                    for (Pullenti.Ner.Token tt = tok.BeginToken.Previous; tt != null; tt = tt.Previous) 
                    {
                        if (tt.WhitespacesAfterCount > 15) 
                            break;
                        if (tt.IsNewlineAfter) 
                            cou += 10;
                        if ((++cou) > 1000) 
                            break;
                        if (!(tt is Pullenti.Ner.ReferentToken)) 
                            continue;
                        List<Pullenti.Ner.Referent> li = tt.GetReferents();
                        if (li == null) 
                            continue;
                        bool breaks = false;
                        foreach (Pullenti.Ner.Referent r in li) 
                        {
                            if (((r.TypeName == "ORGANIZATION" || r.TypeName == "GEO")) && r.ParentReferent == null) 
                            {
                                if (!refs.Contains(r)) 
                                {
                                    if (res.PropRef.CanHasRef(r)) 
                                        refs.Add(r);
                                }
                            }
                            else if (r is Pullenti.Ner.Person.PersonPropertyReferent) 
                            {
                                if ((r as Pullenti.Ner.Person.PersonPropertyReferent).FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) != null) 
                                    breaks = true;
                            }
                            else if (r is Pullenti.Ner.Person.PersonReferent) 
                                breaks = true;
                        }
                        if (refs.Count > 1 || breaks) 
                            break;
                    }
                    if (refs.Count == 1) 
                    {
                        res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, refs[0], false, 0);
                        res.AddOuterOrgAsRef = true;
                    }
                }
            }
            if (res.Chars.IsLatinLetter && res.PropRef != null && res.PropRef.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) == null) 
            {
                if (res.BeginToken.Previous != null && res.BeginToken.Previous.IsValue("S", null)) 
                {
                    if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(res.BeginToken.Previous.Previous) && (res.BeginToken.Previous.Previous.Previous is Pullenti.Ner.ReferentToken)) 
                    {
                        res.BeginToken = res.BeginToken.Previous.Previous.Previous;
                        res.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, res.BeginToken.GetReferent(), false, 0);
                    }
                }
            }
            if (res.Chars.IsLatinLetter && res.PropRef != null && (res.WhitespacesAfterCount < 2)) 
            {
                PersonAttrToken rnext = TryAttach(res.EndToken.Next, locOnto, PersonAttrAttachAttrs.No);
                if ((rnext != null && rnext.Chars.IsLatinLetter && rnext.PropRef != null) && rnext.PropRef.Slots.Count == 1 && rnext.CanHasPersonAfter > 0) 
                {
                    res.EndToken = rnext.EndToken;
                    res.PropRef.Name = string.Format("{0} {1}", res.PropRef.Name, rnext.PropRef.Name);
                }
            }
            return res;
        }
        static PersonAttrToken _TryAttach(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection locOnto, PersonAttrAttachAttrs attrs)
        {
            if (t == null) 
                return null;
            if (t.Morph.Class.IsPronoun && (((t.IsValue("ЕГО", "ЙОГО") || t.IsValue("ЕЕ", "ЇЇ") || t.IsValue("HIS", null)) || t.IsValue("HER", null)))) 
            {
                PersonAttrToken res1 = TryAttach(t.Next, locOnto, attrs);
                if (res1 != null && res1.PropRef != null) 
                {
                    int k = 0;
                    for (Pullenti.Ner.Token tt2 = t.Previous; tt2 != null && (k < 10); tt2 = tt2.Previous,k++) 
                    {
                        Pullenti.Ner.Referent r = tt2.GetReferent();
                        if (r == null) 
                            continue;
                        if (r.TypeName == ObjNameOrg || (r is Pullenti.Ner.Person.PersonReferent)) 
                        {
                            bool ok = false;
                            if (t.IsValue("ЕЕ", "ЇЇ") || t.IsValue("HER", null)) 
                            {
                                if (tt2.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                                    ok = true;
                            }
                            else if (((tt2.Morph.Gender & ((Pullenti.Morph.MorphGender.Masculine | Pullenti.Morph.MorphGender.Neuter)))) != Pullenti.Morph.MorphGender.Undefined) 
                                ok = true;
                            if (ok) 
                            {
                                res1.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, false, 0);
                                res1.BeginToken = t;
                                return res1;
                            }
                            break;
                        }
                    }
                }
                return null;
            }
            Pullenti.Ner.NumberToken nta = Pullenti.Ner.Core.NumberHelper.TryParseAge(t);
            if (nta != null) 
            {
                if (nta.Morph.Class.IsAdjective || ((t.Previous != null && t.Previous.IsComma)) || ((nta.EndToken.Next != null && nta.EndToken.Next.IsCharOf(",.")))) 
                    return new PersonAttrToken(t, nta.EndToken) { Typ = PersonAttrTerminType.Other, Age = nta.Value.ToString(), Morph = nta.Morph };
            }
            if (t.IsNewlineBefore) 
            {
                Pullenti.Ner.Mail.Internal.MailLine li = Pullenti.Ner.Mail.Internal.MailLine.Parse(t, 0, 0);
                if (li != null && li.Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.BestRegards) 
                    return new PersonAttrToken(li.BeginToken, li.EndToken) { Typ = PersonAttrTerminType.BestRegards, Morph = new Pullenti.Ner.MorphCollection() { Case = Pullenti.Morph.MorphCase.Nominative } };
            }
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
            {
                Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
                if (nt != null) 
                {
                    if (((nt.Value == "1" || nt.Value == "2" || nt.Value == "3")) && nt.Morph.Class.IsAdjective) 
                    {
                        PersonAttrToken pat0 = _TryAttach(t.Next, locOnto, attrs);
                        if (pat0 != null && pat0.PropRef != null) 
                        {
                            pat0.BeginToken = t;
                            foreach (Pullenti.Ner.Slot s in pat0.PropRef.Slots) 
                            {
                                if (s.TypeName == Pullenti.Ner.Person.PersonPropertyReferent.ATTR_NAME) 
                                {
                                    if (s.Value.ToString().Contains("глава")) 
                                        return null;
                                    pat0.PropRef.UploadSlot(s, string.Format("{0} {1}", (pat0.Morph.Gender == Pullenti.Morph.MorphGender.Feminie || t.Morph.Gender == Pullenti.Morph.MorphGender.Feminie ? (nt.Value == "1" ? "первая" : (nt.Value == "2" ? "вторая" : "третья")) : (nt.Value == "1" ? "первый" : (nt.Value == "2" ? "второй" : "третий"))), s.Value));
                                }
                            }
                            return pat0;
                        }
                    }
                }
                Pullenti.Ner.Referent rr = null;
                if (t != null) 
                    rr = t.GetReferent();
                if (rr != null && (((rr is Pullenti.Ner.Geo.GeoReferent) || rr.TypeName == "ORGANIZATION"))) 
                {
                    Pullenti.Ner.Token ttt = t.Next;
                    if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(ttt)) 
                        ttt = ttt.Next.Next;
                    if ((ttt is Pullenti.Ner.TextToken) && ttt.Morph.Language.IsEn && (ttt.WhitespacesBeforeCount < 2)) 
                    {
                        PersonAttrToken res0 = _TryAttach(ttt, locOnto, attrs);
                        if (res0 != null && res0.PropRef != null) 
                        {
                            res0.BeginToken = t;
                            res0.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, t.GetReferent(), false, 0);
                            return res0;
                        }
                    }
                }
                if ((rr is Pullenti.Ner.Person.PersonReferent) && Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(t.Next)) 
                {
                    PersonAttrToken res0 = _TryAttach(t.Next.Next.Next, locOnto, attrs);
                    if (res0 != null && res0.PropRef != null && res0.Chars.IsLatinLetter) 
                    {
                        res0.BeginToken = t;
                        res0.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, t.GetReferent(), false, 0);
                        return res0;
                    }
                }
                return null;
            }
            if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(tt)) 
            {
                PersonAttrToken res0 = _TryAttach(t.Next, locOnto, attrs);
                if (res0 != null) 
                {
                    res0.BeginToken = t;
                    return res0;
                }
            }
            if ((tt.Term == "Г" || tt.Term == "ГР" || tt.Term == "М") || tt.Term == "Д") 
            {
                if (tt.Next != null && tt.Next.IsHiphen && (tt.Next.Next is Pullenti.Ner.TextToken)) 
                {
                    string pref = tt.Term;
                    string tail = (tt.Next.Next as Pullenti.Ner.TextToken).Term;
                    List<Pullenti.Morph.MorphWordForm> vars = null;
                    if (pref == "Г") 
                        vars = GetStdForms(tail, "ГОСПОДИН", "ГОСПОЖА");
                    else if (pref == "ГР") 
                        vars = GetStdForms(tail, "ГРАЖДАНИН", "ГРАЖДАНКА");
                    else if (pref == "М") 
                        vars = GetStdForms(tail, "МИСТЕР", null);
                    else if (pref == "Д") 
                    {
                        if (_findGradeLast(tt.Next.Next.Next, tt) != null) 
                        {
                        }
                        else 
                            vars = GetStdForms(tail, "ДОКТОР", null);
                    }
                    if (vars != null) 
                    {
                        PersonAttrToken res = new PersonAttrToken(tt, tt.Next.Next) { Typ = PersonAttrTerminType.Prefix };
                        foreach (Pullenti.Morph.MorphWordForm v in vars) 
                        {
                            res.Morph.AddItem(v);
                            if (res.Value == null) 
                            {
                                res.Value = v.NormalCase;
                                res.Gender = v.Gender;
                            }
                        }
                        return res;
                    }
                }
            }
            if (tt.Term == "ГР" || tt.Term == "ГРАЖД") 
            {
                Pullenti.Ner.Token t1 = (Pullenti.Ner.Token)tt;
                if (tt.Next != null && tt.Next.IsChar('.')) 
                    t1 = tt.Next;
                if (t1.Next is Pullenti.Ner.NumberToken) 
                    return null;
                return new PersonAttrToken(tt, t1) { Typ = PersonAttrTerminType.Prefix, Value = (tt.Morph.Language.IsUa ? "ГРОМАДЯНИН" : "ГРАЖДАНИН") };
            }
            Pullenti.Ner.Core.NounPhraseToken npt0 = null;
            for (int step = 0; step < 2; step++) 
            {
                List<Pullenti.Ner.Core.TerminToken> toks = m_Termins.TryParseAll(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (toks == null && t.IsValue("ВРИО", null)) 
                {
                    toks = new List<Pullenti.Ner.Core.TerminToken>();
                    toks.Add(new Pullenti.Ner.Core.TerminToken(t, t) { Termin = m_TerminVrio });
                }
                else if (toks == null && (t is Pullenti.Ner.TextToken) && t.Morph.Language.IsEn) 
                {
                    string str = (t as Pullenti.Ner.TextToken).Term;
                    if (str.EndsWith("MAN") || str.EndsWith("PERSON") || str.EndsWith("MIST")) 
                    {
                        toks = new List<Pullenti.Ner.Core.TerminToken>();
                        toks.Add(new Pullenti.Ner.Core.TerminToken(t, t) { Termin = new PersonAttrTermin(str, t.Morph.Language) { Typ = PersonAttrTerminType.Position } });
                    }
                    else if (str == "MODEL" && (t.WhitespacesAfterCount < 2)) 
                    {
                        Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("PERSON", t.Next);
                        if (rt != null && (rt.Referent is Pullenti.Ner.Person.PersonReferent)) 
                        {
                            toks = new List<Pullenti.Ner.Core.TerminToken>();
                            toks.Add(new Pullenti.Ner.Core.TerminToken(t, t) { Termin = new PersonAttrTermin(str, t.Morph.Language) { Typ = PersonAttrTerminType.Position } });
                        }
                    }
                }
                if ((toks == null && step == 0 && t.Chars.IsLatinLetter) && (t.WhitespacesAfterCount < 2)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 != null && npt1.BeginToken != npt1.EndToken) 
                    {
                        List<PersonItemToken> pits = PersonItemToken.TryAttachList(t, locOnto, PersonItemToken.ParseAttr.CanBeLatin | PersonItemToken.ParseAttr.IgnoreAttrs, 10);
                        if (pits != null && pits.Count > 1 && pits[0].Firstname != null) 
                            npt1 = null;
                        int k = 0;
                        if (npt1 != null) 
                        {
                            for (Pullenti.Ner.Token tt2 = npt1.BeginToken; tt2 != null && tt2.EndChar <= npt1.EndChar; tt2 = tt2.Next) 
                            {
                                List<Pullenti.Ner.Core.TerminToken> toks1 = m_Termins.TryParseAll(tt2, Pullenti.Ner.Core.TerminParseAttr.No);
                                if (toks1 != null) 
                                {
                                    step = 1;
                                    toks = toks1;
                                    npt0 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, toks1[0].EndChar, null);
                                    if (!(toks[0].Termin as PersonAttrTermin).IsDoubt) 
                                    {
                                        if (toks[0].Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                                        {
                                        }
                                        else 
                                            break;
                                    }
                                }
                                k++;
                                if (k >= 3 && t.Chars.IsAllLower) 
                                {
                                    if (!Pullenti.Ner.Core.MiscHelper.IsEngArticle(t.Previous)) 
                                        break;
                                }
                            }
                        }
                    }
                    else if (((npt1 == null || npt1.EndToken == t)) && t.Chars.IsCapitalUpper) 
                    {
                        Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                        if ((mc.IsMisc || mc.IsPreposition || mc.IsConjunction) || mc.IsPersonalPronoun || mc.IsPronoun) 
                        {
                        }
                        else 
                        {
                            Pullenti.Ner.Token tt1 = null;
                            if ((t.Next != null && t.Next.IsHiphen && !t.IsWhitespaceAfter) && !t.Next.IsWhitespaceAfter) 
                                tt1 = t.Next.Next;
                            else if (npt1 == null) 
                                tt1 = t.Next;
                            List<Pullenti.Ner.Core.TerminToken> toks1 = m_Termins.TryParseAll(tt1, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (toks1 != null && (toks1[0].Termin as PersonAttrTermin).Typ == PersonAttrTerminType.Position && (tt1.WhitespacesBeforeCount < 2)) 
                            {
                                step = 1;
                                toks = toks1;
                            }
                        }
                    }
                }
                if (toks != null) 
                {
                    foreach (Pullenti.Ner.Core.TerminToken tok in toks) 
                    {
                        if (((tok.Morph.Class.IsPreposition || tok.Morph.ContainsAttr("к.ф.", null))) && tok.EndToken == tok.BeginToken) 
                            continue;
                        PersonAttrTermin pat = tok.Termin as PersonAttrTermin;
                        if ((tok.EndToken is Pullenti.Ner.TextToken) && pat.CanonicText.StartsWith((tok.EndToken as Pullenti.Ner.TextToken).Term)) 
                        {
                            if (tok.LengthChar < pat.CanonicText.Length) 
                            {
                                if (tok.EndToken.Next != null && tok.EndToken.Next.IsChar('.')) 
                                    tok.EndToken = tok.EndToken.Next;
                            }
                        }
                        if (pat.Typ == PersonAttrTerminType.Prefix) 
                        {
                            if (step == 0 || ((pat.CanonicText != "ГРАЖДАНИН" && pat.CanonicText != "ГРОМАДЯНИН"))) 
                                return new PersonAttrToken(tok.BeginToken, tok.EndToken) { Typ = PersonAttrTerminType.Prefix, Value = pat.CanonicText, Morph = tok.Morph, Gender = pat.Gender };
                        }
                        if (pat.Typ == PersonAttrTerminType.BestRegards) 
                        {
                            Pullenti.Ner.Token end = tok.EndToken;
                            if (end.Next != null && end.Next.IsCharOf(",")) 
                                end = end.Next;
                            return new PersonAttrToken(tok.BeginToken, end) { Typ = PersonAttrTerminType.BestRegards, Morph = new Pullenti.Ner.MorphCollection() { Case = Pullenti.Morph.MorphCase.Nominative } };
                        }
                        if (pat.Typ == PersonAttrTerminType.Position || pat.Typ == PersonAttrTerminType.Prefix || pat.Typ == PersonAttrTerminType.King) 
                        {
                            PersonAttrToken res = CreateAttrPosition(tok, locOnto, attrs);
                            if (res != null) 
                            {
                                if (pat.Typ == PersonAttrTerminType.King) 
                                    res.Typ = pat.Typ;
                                if (pat.Gender != Pullenti.Morph.MorphGender.Undefined && res.Gender == Pullenti.Morph.MorphGender.Undefined) 
                                    res.Gender = pat.Gender;
                                if (pat.CanHasPersonAfter > 0) 
                                {
                                    if (res.EndToken.IsValue(pat.CanonicText, null)) 
                                        res.CanHasPersonAfter = pat.CanHasPersonAfter;
                                    else 
                                        for (int ii = pat.CanonicText.Length - 1; ii > 0; ii--) 
                                        {
                                            if (!char.IsLetter(pat.CanonicText[ii])) 
                                            {
                                                if (res.EndToken.IsValue(pat.CanonicText.Substring(ii + 1), null)) 
                                                    res.CanHasPersonAfter = pat.CanHasPersonAfter;
                                                break;
                                            }
                                        }
                                }
                                if (pat.CanBeSameSurname) 
                                    res.CanBeSameSurname = true;
                                if (pat.CanBeIndependant) 
                                    res.CanBeIndependentProperty = true;
                                if (pat.IsDoubt) 
                                {
                                    res.IsDoubt = true;
                                    if (res.PropRef != null && (res.PropRef.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) != null)) 
                                        res.IsDoubt = false;
                                }
                                if ((t.EndChar < res.BeginChar) && res.PropRef != null) 
                                {
                                    Pullenti.Ner.Token tt1 = res.BeginToken.Previous;
                                    if (tt1.IsHiphen) 
                                        res.PropRef.Name = string.Format("{0} {1}", res.PropRef.Name, Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tt1.Previous, Pullenti.Ner.Core.GetTextAttr.No).ToLower());
                                    else 
                                        res.PropRef.Name = string.Format("{0} {1}", Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tt1, Pullenti.Ner.Core.GetTextAttr.No).ToLower(), res.PropRef.Name);
                                    res.BeginToken = t;
                                }
                            }
                            if (res != null) 
                            {
                                PersonItemToken pit = PersonItemToken.TryAttach(t, null, PersonItemToken.ParseAttr.IgnoreAttrs, null);
                                if (pit != null && pit.Typ == PersonItemToken.ItemType.Initial) 
                                {
                                    bool ok = false;
                                    pit = PersonItemToken.TryAttach(pit.EndToken.Next, null, PersonItemToken.ParseAttr.IgnoreAttrs, null);
                                    if (pit != null && pit.Typ == PersonItemToken.ItemType.Initial) 
                                    {
                                        pit = PersonItemToken.TryAttach(pit.EndToken.Next, null, PersonItemToken.ParseAttr.IgnoreAttrs, null);
                                        if (pit != null && pit.Typ == PersonItemToken.ItemType.Initial) 
                                            ok = true;
                                    }
                                    if (!ok) 
                                    {
                                        if (_TryAttach(tok.EndToken.Next, locOnto, attrs) != null) 
                                            ok = true;
                                    }
                                    if (!ok) 
                                        return null;
                                }
                                if (npt0 != null) 
                                {
                                    Pullenti.Ner.Token ttt1 = (npt0.Adjectives.Count > 0 ? npt0.Adjectives[0].BeginToken : npt0.BeginToken);
                                    if (ttt1.BeginChar < res.BeginChar) 
                                        res.BeginToken = ttt1;
                                    res.Anafor = npt0.Anafor;
                                    string emptyAdj = null;
                                    for (int i = 0; i < npt0.Adjectives.Count; i++) 
                                    {
                                        int j;
                                        for (j = 0; j < m_EmptyAdjs.Length; j++) 
                                        {
                                            if (npt0.Adjectives[i].IsValue(m_EmptyAdjs[j], null)) 
                                                break;
                                        }
                                        if (j < m_EmptyAdjs.Length) 
                                        {
                                            emptyAdj = m_EmptyAdjs[j].ToLower();
                                            npt0.Adjectives.RemoveAt(i);
                                            break;
                                        }
                                    }
                                    string na0 = npt0.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                                    string na1 = res.PropRef.Name;
                                    for (int i = 1; i < (na0.Length - 1); i++) 
                                    {
                                        if (na1.StartsWith(na0.Substring(i))) 
                                        {
                                            res.PropRef.Name = string.Format("{0} {1}", na0.Substring(0, i).Trim(), na1);
                                            break;
                                        }
                                    }
                                    if (emptyAdj != null) 
                                    {
                                        PersonAttrToken res1 = new PersonAttrToken(res.BeginToken, res.EndToken) { Morph = npt0.Morph, HigherPropRef = res };
                                        res1.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent();
                                        res1.PropRef.Name = emptyAdj;
                                        res1.PropRef.Higher = res.PropRef;
                                        res1.CanBeIndependentProperty = res.CanBeIndependentProperty;
                                        res1.Typ = res.Typ;
                                        if (res.BeginToken != res.EndToken) 
                                            res.BeginToken = res.BeginToken.Next;
                                        res = res1;
                                    }
                                }
                                if (res != null) 
                                    res.Morph.RemoveNotInDictionaryItems();
                                return res;
                            }
                        }
                    }
                }
                if (step > 0 || t.Chars.IsLatinLetter) 
                    break;
                if (t.Morph.Class.IsAdjective || t.Chars.IsLatinLetter) 
                {
                }
                else if (t.Next != null && t.Next.IsHiphen) 
                {
                }
                else 
                    break;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt == null || npt.EndToken == t || npt.InternalNoun != null) 
                    break;
                if (npt.EndToken.IsValue("ВИЦЕ", "ВІЦЕ")) 
                    break;
                t = npt.EndToken;
                npt0 = npt;
            }
            if ((t is Pullenti.Ner.TextToken) && (((t.IsValue("ВИЦЕ", "ВІЦЕ") || t.IsValue("ЭКС", "ЕКС") || t.IsValue("VICE", null)) || t.IsValue("EX", null) || t.IsValue("DEPUTY", null))) && t.Next != null) 
            {
                Pullenti.Ner.Token te = t.Next;
                if (te.IsHiphen) 
                    te = te.Next;
                PersonAttrToken ppp = _TryAttach(te, locOnto, attrs);
                if (ppp != null) 
                {
                    if (t.BeginChar < ppp.BeginChar) 
                    {
                        ppp.BeginToken = t;
                        if (ppp.PropRef != null && ppp.PropRef.Name != null) 
                            ppp.PropRef.Name = string.Format("{0}-{1}", (t as Pullenti.Ner.TextToken).Term.ToLower(), ppp.PropRef.Name);
                    }
                    return ppp;
                }
                if ((te != null && te.Previous.IsHiphen && !te.IsWhitespaceAfter) && !te.IsWhitespaceBefore) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(te, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(te, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && (te is Pullenti.Ner.TextToken)) 
                        {
                            ppp = new PersonAttrToken(t, br.EndToken) { Morph = br.EndToken.Previous.Morph };
                            ppp.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent();
                            ppp.PropRef.Name = string.Format("{0}-{1}", (t as Pullenti.Ner.TextToken).Term, Pullenti.Ner.Core.MiscHelper.GetTextValue(te.Next, br.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative)).ToLower();
                            return ppp;
                        }
                    }
                }
            }
            if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLatinLetter) 
            {
                if (t.IsValue("STATE", null)) 
                {
                    Pullenti.Ner.Token tt1 = t.Next;
                    if (Pullenti.Ner.Core.MiscHelper.IsEngAdjSuffix(tt1)) 
                        tt1 = tt1.Next.Next;
                    PersonAttrToken res1 = _TryAttach(tt1, locOnto, attrs);
                    if (res1 != null && res1.PropRef != null) 
                    {
                        res1.BeginToken = t;
                        res1.PropRef.Name = string.Format("{0} {1}", (t as Pullenti.Ner.TextToken).Term.ToLower(), res1.PropRef.Name);
                        return res1;
                    }
                }
            }
            return null;
        }
        static string[] m_EmptyAdjs = new string[] {"УСПЕШНЫЙ", "ИЗВЕСТНЫЙ", "ЗНАМЕНИТЫЙ", "ИЗВЕСТНЕЙШИЙ", "ПОПУЛЯРНЫЙ", "ГЕНИАЛЬНЫЙ", "ТАЛАНТЛИВЫЙ", "МОЛОДОЙ", "УСПІШНИЙ", "ВІДОМИЙ", "ЗНАМЕНИТИЙ", "ВІДОМИЙ", "ПОПУЛЯРНИЙ", "ГЕНІАЛЬНИЙ", "ТАЛАНОВИТИЙ", "МОЛОДИЙ"};
        static Dictionary<string, List<Pullenti.Morph.MorphWordForm>> m_StdForms = new Dictionary<string, List<Pullenti.Morph.MorphWordForm>>();
        static List<Pullenti.Morph.MorphWordForm> GetStdForms(string tail, string w1, string w2)
        {
            List<Pullenti.Morph.MorphWordForm> res = new List<Pullenti.Morph.MorphWordForm>();
            List<Pullenti.Morph.MorphWordForm> li1 = null;
            List<Pullenti.Morph.MorphWordForm> li2 = null;
            if (!m_StdForms.TryGetValue(w1, out li1)) 
            {
                li1 = Pullenti.Morph.MorphologyService.GetAllWordforms(w1, null);
                m_StdForms.Add(w1, li1);
            }
            foreach (Pullenti.Morph.MorphWordForm v in li1) 
            {
                if (Pullenti.Morph.LanguageHelper.EndsWith(v.NormalCase, tail)) 
                    res.Add(v);
            }
            if (w2 != null) 
            {
                if (!m_StdForms.TryGetValue(w2, out li2)) 
                {
                    li2 = Pullenti.Morph.MorphologyService.GetAllWordforms(w2, null);
                    m_StdForms.Add(w2, li2);
                }
            }
            if (li2 != null) 
            {
                foreach (Pullenti.Morph.MorphWordForm v in li2) 
                {
                    if (Pullenti.Morph.LanguageHelper.EndsWith(v.NormalCase, tail)) 
                        res.Add(v);
                }
            }
            return (res.Count > 0 ? res : null);
        }
        static PersonAttrToken CreateAttrPosition(Pullenti.Ner.Core.TerminToken tok, Pullenti.Ner.Core.IntOntologyCollection locOnto, PersonAttrAttachAttrs attrs)
        {
            PersonAttrTerminType2 ty2 = (tok.Termin as PersonAttrTermin).Typ2;
            if (ty2 == PersonAttrTerminType2.Abbr) 
            {
                Pullenti.Ner.Person.PersonPropertyReferent pr0 = new Pullenti.Ner.Person.PersonPropertyReferent();
                pr0.Name = tok.Termin.CanonicText;
                return new PersonAttrToken(tok.BeginToken, tok.EndToken) { PropRef = pr0, Typ = PersonAttrTerminType.Position };
            }
            if (ty2 == PersonAttrTerminType2.Io || ty2 == PersonAttrTerminType2.Io2) 
            {
                for (int k = 0; ; k++) 
                {
                    if (k > 0) 
                    {
                        if (ty2 == PersonAttrTerminType2.Io) 
                            return null;
                        if (((tok.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                            return null;
                        break;
                    }
                    Pullenti.Ner.Token tt = tok.EndToken.Next;
                    if (tt != null && tt.Morph.Class.IsPreposition) 
                        tt = tt.Next;
                    PersonAttrToken resPat = new PersonAttrToken(tok.BeginToken, tok.EndToken) { Typ = PersonAttrTerminType.Position };
                    resPat.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent();
                    if (tt != null && (tt.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
                    {
                        resPat.EndToken = tt;
                        resPat.PropRef.Higher = tt.GetReferent() as Pullenti.Ner.Person.PersonPropertyReferent;
                    }
                    else 
                    {
                        PersonAttrAttachAttrs aa = attrs;
                        if (ty2 == PersonAttrTerminType2.Io2) 
                            aa |= PersonAttrAttachAttrs.AfterZamestitel;
                        PersonAttrToken pat = TryAttach(tt, locOnto, aa);
                        if (pat == null) 
                        {
                            if (!(tt is Pullenti.Ner.TextToken)) 
                                continue;
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt == null || npt.EndToken == tok.EndToken.Next) 
                                continue;
                            pat = TryAttach(npt.EndToken, locOnto, PersonAttrAttachAttrs.No);
                            if (pat == null || pat.BeginToken != tt) 
                                continue;
                        }
                        if (pat.Typ != PersonAttrTerminType.Position) 
                            continue;
                        resPat.EndToken = pat.EndToken;
                        resPat.PropRef.Higher = pat.PropRef;
                        resPat.HigherPropRef = pat;
                    }
                    string nam = tok.Termin.CanonicText;
                    Pullenti.Ner.Token ts = resPat.EndToken.Next;
                    Pullenti.Ner.Token te = null;
                    for (; ts != null; ts = ts.Next) 
                    {
                        if (ts.Morph.Class.IsPreposition) 
                        {
                            if (ts.IsValue("В", null) || ts.IsValue("ПО", null)) 
                            {
                                if (ts.Next is Pullenti.Ner.ReferentToken) 
                                {
                                    Pullenti.Ner.Referent r = ts.Next.GetReferent();
                                    if (r.TypeName == ObjNameGeo || r.TypeName == ObjNameOrg) 
                                    {
                                        resPat.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, false, 0);
                                        resPat.EndToken = ts.Next;
                                    }
                                    else 
                                        te = ts.Next;
                                    ts = ts.Next;
                                    continue;
                                }
                                Pullenti.Ner.ReferentToken rt11 = ts.Kit.ProcessReferent("NAMEDENTITY", ts.Next);
                                if (rt11 != null) 
                                {
                                    resPat.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, rt11, false, 0);
                                    resPat.EndToken = rt11.EndToken;
                                    ts = rt11.EndToken;
                                    continue;
                                }
                            }
                            if (ts.IsValue("ПО", null) && ts.Next != null) 
                            {
                                Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ts.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (nnn != null) 
                                    ts = (te = nnn.EndToken);
                                else if ((ts.Next is Pullenti.Ner.TextToken) && ((!ts.Next.Chars.IsAllLower && !ts.Next.Chars.IsCapitalUpper))) 
                                    ts = (te = ts.Next);
                                else 
                                    break;
                                if (ts.Next != null && ts.Next.IsAnd && nnn != null) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken nnn2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ts.Next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (nnn2 != null && !((nnn2.Morph.Case & nnn.Morph.Case)).IsUndefined) 
                                        ts = (te = nnn2.EndToken);
                                }
                                continue;
                            }
                            break;
                        }
                        if (ts != resPat.EndToken.Next && ts.Chars.IsAllLower) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ts, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (nnn == null) 
                                break;
                            ts = (te = nnn.EndToken);
                            continue;
                        }
                        break;
                    }
                    if (te != null) 
                    {
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(resPat.EndToken.Next, te, Pullenti.Ner.Core.GetTextAttr.No);
                        if (!string.IsNullOrEmpty(s)) 
                        {
                            nam = string.Format("{0} {1}", nam, s);
                            resPat.EndToken = te;
                        }
                        if ((resPat.HigherPropRef != null && (te.WhitespacesAfterCount < 4) && te.Next.GetReferent() != null) && te.Next.GetReferent().TypeName == ObjNameOrg) 
                        {
                            resPat.EndToken = (resPat.HigherPropRef.EndToken = te.Next);
                            resPat.HigherPropRef.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, te.Next.GetReferent(), false, 0);
                        }
                    }
                    resPat.BeginToken = _analizeVise(resPat.BeginToken, ref nam);
                    resPat.PropRef.Name = nam.ToLower();
                    resPat.Morph = tok.Morph;
                    return resPat;
                }
            }
            if (ty2 == PersonAttrTerminType2.Adj) 
            {
                PersonAttrToken pat = _TryAttach(tok.EndToken.Next, locOnto, attrs);
                if (pat == null || pat.Typ != PersonAttrTerminType.Position) 
                    return null;
                if (tok.BeginChar == tok.EndChar && !tok.BeginToken.Morph.Class.IsUndefined) 
                    return null;
                pat.BeginToken = tok.BeginToken;
                pat.PropRef.Name = string.Format("{0} {1}", tok.Termin.CanonicText.ToLower(), pat.PropRef.Name);
                pat.Morph = tok.Morph;
                return pat;
            }
            if (ty2 == PersonAttrTerminType2.IgnoredAdj) 
            {
                PersonAttrToken pat = _TryAttach(tok.EndToken.Next, locOnto, attrs);
                if (pat == null || pat.Typ != PersonAttrTerminType.Position) 
                    return null;
                pat.BeginToken = tok.BeginToken;
                pat.Morph = tok.Morph;
                return pat;
            }
            if (ty2 == PersonAttrTerminType2.Grade) 
            {
                PersonAttrToken gr = CreateAttrGrade(tok);
                if (gr != null) 
                    return gr;
                if (tok.BeginToken.IsValue("КАНДИДАТ", null)) 
                {
                    Pullenti.Ner.Token tt = tok.EndToken.Next;
                    if (tt != null && tt.IsValue("В", null)) 
                        tt = tt.Next;
                    else if ((tt != null && tt.IsValue("НА", null) && tt.Next != null) && ((tt.Next.IsValue("ПОСТ", null) || tt.Next.IsValue("ДОЛЖНОСТЬ", null)))) 
                        tt = tt.Next.Next;
                    else 
                        tt = null;
                    if (tt != null) 
                    {
                        PersonAttrToken pat2 = _TryAttach(tt, locOnto, PersonAttrAttachAttrs.No);
                        if (pat2 != null) 
                        {
                            PersonAttrToken res0 = new PersonAttrToken(tok.BeginToken, pat2.EndToken) { Typ = PersonAttrTerminType.Position };
                            res0.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent() { Name = "кандидат" };
                            res0.PropRef.Higher = pat2.PropRef;
                            res0.HigherPropRef = pat2;
                            res0.Morph = tok.Morph;
                            return res0;
                        }
                    }
                }
                if (!tok.BeginToken.IsValue("ДОКТОР", null) && !tok.BeginToken.IsValue("КАНДИДАТ", null)) 
                    return null;
            }
            string name = tok.Termin.CanonicText.ToLower();
            Pullenti.Ner.Token t0 = tok.BeginToken;
            Pullenti.Ner.Token t1 = tok.EndToken;
            t0 = _analizeVise(t0, ref name);
            Pullenti.Ner.Person.PersonPropertyReferent pr = new Pullenti.Ner.Person.PersonPropertyReferent();
            if ((t1.Next != null && t1.Next.IsHiphen && !t1.IsWhitespaceAfter) && !t1.Next.IsWhitespaceAfter) 
            {
                if (t1.Next.Next.Chars == t1.Chars || m_Termins.TryParse(t1.Next.Next, Pullenti.Ner.Core.TerminParseAttr.No) != null || ((t1.Next.Next.Chars.IsAllLower && t1.Next.Next.Chars.IsCyrillicLetter))) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.EndToken == t1.Next.Next) 
                    {
                        name = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                        t1 = npt.EndToken;
                    }
                }
            }
            Pullenti.Ner.Token tname0 = t1.Next;
            Pullenti.Ner.Token tname1 = null;
            string category = null;
            Pullenti.Ner.Core.NounPhraseToken npt0 = null;
            for (Pullenti.Ner.Token t = t1.Next; t != null; t = t.Next) 
            {
                if (((attrs & PersonAttrAttachAttrs.OnlyKeyword)) != PersonAttrAttachAttrs.No) 
                    break;
                if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t) != null) 
                    break;
                if (t.IsNewlineBefore) 
                {
                    bool ok = false;
                    if (t.GetReferent() != null) 
                    {
                        if (t.GetReferent().TypeName == ObjNameOrg || (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        {
                            if (pr.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) == null) 
                                ok = true;
                        }
                    }
                    if (t.NewlinesBeforeCount > 1 && !t.Chars.IsAllLower) 
                    {
                        if (!ok) 
                            break;
                        if ((t.NewlinesAfterCount < 3) && tok.BeginToken.IsNewlineBefore) 
                        {
                        }
                        else 
                            break;
                    }
                    if (tok.IsNewlineBefore) 
                    {
                        if (m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                            break;
                        else 
                            ok = true;
                    }
                    if (t0.Previous != null && t0.Previous.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br0 = Pullenti.Ner.Core.BracketHelper.TryParse(t0.Previous, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 10);
                        if (br0 != null && br0.EndChar > t.EndChar) 
                            ok = true;
                    }
                    if (!ok) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt00 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                        if (npt00 != null && npt00.EndToken.Next != null && !_isPerson(t)) 
                        {
                            Pullenti.Ner.Token tt1 = npt00.EndToken;
                            bool zap = false;
                            bool and = false;
                            for (Pullenti.Ner.Token ttt = tt1.Next; ttt != null; ttt = ttt.Next) 
                            {
                                if (!ttt.IsCommaAnd) 
                                    break;
                                npt00 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt00 == null) 
                                    break;
                                tt1 = npt00.EndToken;
                                if (ttt.IsChar(',')) 
                                    zap = true;
                                else 
                                {
                                    and = true;
                                    break;
                                }
                                ttt = npt00.EndToken;
                            }
                            if (zap && !and) 
                            {
                            }
                            else if (tt1.Next == null) 
                            {
                            }
                            else 
                            {
                                if (_isPerson(tt1.Next)) 
                                    ok = true;
                                else if (tt1.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                                {
                                    if (_isPerson(tt1.Next.Next)) 
                                        ok = true;
                                    else 
                                    {
                                        string ccc;
                                        Pullenti.Ner.Token ttt = TryAttachCategory(tt1.Next.Next, out ccc);
                                        if (ttt != null) 
                                            ok = true;
                                    }
                                }
                                if (ok) 
                                {
                                    t = (t1 = (tname1 = tt1));
                                    continue;
                                }
                            }
                        }
                        break;
                    }
                }
                if (t.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        t = br.EndToken;
                        bool ok = true;
                        for (Pullenti.Ner.Token ttt = br.BeginToken; ttt != br.EndToken; ttt = ttt.Next) 
                        {
                            if (ttt.Chars.IsLetter) 
                            {
                                if (!ttt.Chars.IsAllLower) 
                                {
                                    ok = false;
                                    break;
                                }
                            }
                        }
                        if (!ok) 
                            break;
                        continue;
                    }
                    else 
                        break;
                }
                Pullenti.Ner.Token tt2 = _analyzeRomanNums(t);
                if (tt2 != null) 
                {
                    t1 = (t = tt2);
                    if (t.IsValue("СОЗЫВ", null) && t.Next != null && t.Next.IsValue("ОТ", null)) 
                    {
                        t = t.Next;
                        continue;
                    }
                    break;
                }
                PersonAttrToken pat = null;
                if (((attrs & PersonAttrAttachAttrs.OnlyKeyword)) == PersonAttrAttachAttrs.No) 
                    pat = _TryAttach(t, locOnto, PersonAttrAttachAttrs.OnlyKeyword);
                if (pat != null) 
                {
                    if (pat.Morph.Number == Pullenti.Morph.MorphNumber.Plural && !pat.Morph.Case.IsNominative) 
                    {
                    }
                    else if (((tok.Termin is PersonAttrTermin) && (tok.Termin as PersonAttrTermin).IsDoubt && pat.PropRef != null) && pat.PropRef.Slots.Count == 1 && tok.Chars.IsLatinLetter == pat.Chars.IsLatinLetter) 
                    {
                        t1 = (tname1 = (t = pat.EndToken));
                        continue;
                    }
                    else if ((!tok.Morph.Case.IsGenitive && (tok.Termin is PersonAttrTermin) && (tok.Termin as PersonAttrTermin).CanHasPersonAfter == 1) && pat.Morph.Case.IsGenitive) 
                    {
                        Pullenti.Ner.ReferentToken rr = null;
                        if (!t.Kit.MiscData.ContainsKey("IgnorePersons")) 
                        {
                            t.Kit.MiscData.Add("IgnorePersons", null);
                            rr = t.Kit.ProcessReferent("PERSON", t);
                            if (t.Kit.MiscData.ContainsKey("IgnorePersons")) 
                                t.Kit.MiscData.Remove("IgnorePersons");
                        }
                        if (rr != null && rr.Morph.Case.IsGenitive) 
                        {
                            pr.AddExtReferent(rr);
                            pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, rr.Referent, false, 0);
                            t1 = (t = rr.EndToken);
                        }
                        else 
                            t1 = (tname1 = (t = pat.EndToken));
                        continue;
                    }
                    else if (t.IsValue("ГР", null) && (pat.EndToken.Next is Pullenti.Ner.TextToken) && !pat.EndToken.Next.Chars.IsAllLower) 
                    {
                        Pullenti.Ner.ReferentToken ppp = t.Kit.ProcessReferent("PERSON", pat.EndToken.Next.Next);
                        if (ppp != null) 
                        {
                            t1 = (tname1 = (t = pat.EndToken));
                            continue;
                        }
                        break;
                    }
                    else 
                        break;
                }
                Pullenti.Ner.Token te = t;
                if (te.Next != null && te.IsCharOf(",в") && ((attrs & PersonAttrAttachAttrs.AfterZamestitel)) == PersonAttrAttachAttrs.No) 
                {
                    te = te.Next;
                    if (te.IsValue("ОРГАНИЗАЦИЯ", null) && (te.Next is Pullenti.Ner.ReferentToken) && te.Next.GetReferent().TypeName == ObjNameOrg) 
                        te = te.Next;
                }
                else if (te.Next != null && te.Morph.Class.IsPreposition) 
                {
                    if (((attrs & PersonAttrAttachAttrs.AfterZamestitel)) == PersonAttrAttachAttrs.AfterZamestitel) 
                        break;
                    if (((te.IsValue("ИЗ", null) || te.IsValue("ПРИ", null) || te.IsValue("ПО", null)) || te.IsValue("НА", null) || te.IsValue("ОТ", null)) || te.IsValue("OF", null)) 
                        te = te.Next;
                }
                else if ((te.IsHiphen && te.Next != null && !te.IsWhitespaceBefore) && !te.IsWhitespaceAfter && te.Previous.Chars == te.Next.Chars) 
                    continue;
                else if (te.IsValue("REPRESENT", null) && (te.Next is Pullenti.Ner.ReferentToken)) 
                    te = te.Next;
                Pullenti.Ner.Referent r = te.GetReferent();
                Pullenti.Ner.Referent r1;
                if ((te.Chars.IsLatinLetter && te.LengthChar > 1 && !t0.Chars.IsLatinLetter) && !te.Chars.IsAllLower) 
                {
                    if (r == null || r.TypeName != ObjNameOrg) 
                    {
                        Pullenti.Ner.Token tt = TryAttachCategory(t, out category);
                        if (tt != null && name != null) 
                        {
                            t = (t1 = tt);
                            continue;
                        }
                        for (; te != null; te = te.Next) 
                        {
                            if (te.Chars.IsLetter) 
                            {
                                if (!te.Chars.IsLatinLetter) 
                                    break;
                                t1 = (tname1 = (t = te));
                            }
                        }
                        continue;
                    }
                }
                if (r != null) 
                {
                    if ((r.TypeName == ObjNameGeo && te.Previous != null && te.Previous.IsValue("ДЕЛО", "СПРАВІ")) && te.Previous.Previous != null && te.Previous.Previous.IsValue("ПО", null)) 
                    {
                        t1 = (tname1 = (t = te));
                        continue;
                    }
                    if ((r.TypeName == ObjNameGeo || r.TypeName == ObjNameAddr || r.TypeName == ObjNameOrg) || r.TypeName == ObjNameTransport) 
                    {
                        if (t0.Previous != null && t0.Previous.IsValue("ОТ", null) && t.IsNewlineBefore) 
                            break;
                        t1 = te;
                        pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, false, 0);
                        bool posol = ((r.TypeName == ObjNameGeo || r.TypeName == ObjNameOrg)) && Pullenti.Morph.LanguageHelper.EndsWithEx(name, "посол", "представитель", null, null);
                        if (posol) 
                        {
                            t = t1;
                            continue;
                        }
                        if ((((r.TypeName == ObjNameGeo && t1.Next != null && t1.Next.Morph.Class.IsPreposition) && t1.Next.Next != null && !t1.Next.IsValue("О", null)) && !t1.Next.IsValue("ОБ", null) && ((attrs & PersonAttrAttachAttrs.AfterZamestitel)) == PersonAttrAttachAttrs.No) && !(tok.Termin as PersonAttrTermin).IsBoss) 
                        {
                            if ((((r1 = t1.Next.Next.GetReferent()))) != null) 
                            {
                                if (r1.TypeName == ObjNameOrg) 
                                {
                                    pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r1, false, 0);
                                    t = (t1 = t1.Next.Next);
                                }
                            }
                        }
                        if (r.TypeName == ObjNameOrg) 
                        {
                            for (t = te.Next; t != null; t = t.Next) 
                            {
                                if (!t.IsCommaAnd || !(t.Next is Pullenti.Ner.ReferentToken)) 
                                    break;
                                r = t.Next.GetReferent();
                                if (r == null) 
                                    break;
                                if (r.TypeName != ObjNameOrg) 
                                    break;
                                pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, false, 0);
                                t = t.Next;
                                t1 = t;
                                if (t.Previous.IsAnd) 
                                {
                                    t = t.Next;
                                    break;
                                }
                            }
                            for (; t != null; t = t.Next) 
                            {
                                if (t.IsNewlineBefore) 
                                    break;
                                tt2 = _analyzeRomanNums(t);
                                if (tt2 != null) 
                                {
                                    t1 = (t = tt2);
                                    if (t.IsValue("СОЗЫВ", null) && t.Next != null && t.Next.IsValue("ОТ", null)) 
                                        t = t.Next;
                                    else 
                                        break;
                                }
                                if (t.IsValue("В", null) || t.IsValue("ОТ", null) || t.IsAnd) 
                                    continue;
                                if (t.Morph.Language.IsUa) 
                                {
                                    if (t.IsValue("ВІД", null)) 
                                        continue;
                                }
                                if (((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && !t.Chars.IsAllLower) && t.Previous.IsValue("ОТ", "ВІД")) 
                                {
                                    tname0 = t.Previous;
                                    tname1 = (t1 = t);
                                    continue;
                                }
                                if ((t is Pullenti.Ner.TextToken) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false) && t.Previous.IsValue("ОТ", "ВІД")) 
                                {
                                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                    if (br != null && (br.LengthChar < 100)) 
                                    {
                                        tname0 = t.Previous;
                                        tname1 = (t1 = (t = br.EndToken));
                                        continue;
                                    }
                                }
                                r = t.GetReferent();
                                if (r == null) 
                                    break;
                                if (r.TypeName != ObjNameGeo) 
                                {
                                    if (r.TypeName == ObjNameOrg && t.Previous != null && ((t.Previous.IsValue("ОТ", null) || t.Previous.IsValue("ВІД", null)))) 
                                    {
                                    }
                                    else 
                                        break;
                                }
                                pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, r, false, 0);
                                t1 = t;
                            }
                        }
                    }
                    if ((t1.Next != null && (t1.WhitespacesAfterCount < 2) && t1.Next.Chars.IsLatinLetter) && !t1.Next.Chars.IsAllLower && Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t1.Next) == null) 
                    {
                        for (t = t1.Next; t != null; t = t.Next) 
                        {
                            if (!(t is Pullenti.Ner.TextToken)) 
                                break;
                            if (!t.Chars.IsLetter) 
                                break;
                            if (!t.Chars.IsLatinLetter) 
                                break;
                            if (t.Kit.BaseLanguage.IsEn) 
                                break;
                            t1 = (tname1 = t);
                        }
                    }
                    t = t1;
                    if (((tname0 == t && tname1 == null && t.Next != null) && ((attrs & PersonAttrAttachAttrs.AfterZamestitel)) == PersonAttrAttachAttrs.No && name != "президент") && t.Next.IsValue("ПО", null)) 
                    {
                        tname0 = t.Next;
                        continue;
                    }
                    break;
                }
                if (category == null) 
                {
                    Pullenti.Ner.Token tt = TryAttachCategory(t, out category);
                    if (tt != null && name != null) 
                    {
                        t = (t1 = tt);
                        continue;
                    }
                }
                if (name == "премьер") 
                    break;
                if (t is Pullenti.Ner.TextToken) 
                {
                    if (t.IsValue("ИМЕНИ", "ІМЕНІ")) 
                        break;
                }
                if (!t.Chars.IsAllLower) 
                {
                    PersonItemToken pit = PersonItemToken.TryAttach(t, locOnto, PersonItemToken.ParseAttr.CanBeLatin | PersonItemToken.ParseAttr.IgnoreAttrs, null);
                    if (pit != null) 
                    {
                        if (pit.Referent != null) 
                            break;
                        if (pit.Lastname != null && ((pit.Lastname.IsInDictionary || pit.Lastname.IsInOntology))) 
                            break;
                        if (pit.Firstname != null && pit.Firstname.IsInDictionary) 
                            break;
                        List<PersonItemToken> pits = PersonItemToken.TryAttachList(t, locOnto, PersonItemToken.ParseAttr.No | PersonItemToken.ParseAttr.IgnoreAttrs, 6);
                        if (pits != null && pits.Count > 0) 
                        {
                            if (pits.Count == 2) 
                            {
                                if (pits[1].Lastname != null && pits[1].Lastname.IsInDictionary) 
                                    break;
                                if (pits[1].Typ == PersonItemToken.ItemType.Initial && pits[0].Lastname != null) 
                                    break;
                            }
                            if (pits.Count == 3) 
                            {
                                if (pits[2].Lastname != null) 
                                {
                                    if (pits[1].Middlename != null) 
                                        break;
                                    if (pits[0].Firstname != null && pits[0].Firstname.IsInDictionary) 
                                        break;
                                }
                                if (pits[1].Typ == PersonItemToken.ItemType.Initial && pits[2].Typ == PersonItemToken.ItemType.Initial && pits[0].Lastname != null) 
                                    break;
                            }
                            if (pits[0].Typ == PersonItemToken.ItemType.Initial) 
                                break;
                        }
                    }
                }
                bool testPerson = false;
                if (!t.Chars.IsAllLower) 
                {
                    if (t.Kit.MiscData.ContainsKey("TestAttr")) 
                    {
                    }
                    else 
                    {
                        List<PersonItemToken> pits = PersonItemToken.TryAttachList(t, null, PersonItemToken.ParseAttr.IgnoreAttrs, 10);
                        if (pits != null && pits.Count > 1) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            int iii = 1;
                            if (nnn != null && nnn.Adjectives.Count > 0) 
                                iii += nnn.Adjectives.Count;
                            testPerson = true;
                            t.Kit.MiscData.Add("TestAttr", null);
                            List<PersonIdentityToken> li = PersonIdentityToken.TryAttach(pits, 0, new Pullenti.Morph.MorphBaseInfo() { Case = Pullenti.Morph.MorphCase.AllCases }, null, false, false);
                            t.Kit.MiscData.Remove("TestAttr");
                            if (li.Count > 0 && li[0].Coef > 1) 
                            {
                                t.Kit.MiscData.Add("TestAttr", null);
                                List<PersonIdentityToken> li1 = PersonIdentityToken.TryAttach(pits, iii, new Pullenti.Morph.MorphBaseInfo() { Case = Pullenti.Morph.MorphCase.AllCases }, null, false, false);
                                t.Kit.MiscData.Remove("TestAttr");
                                if (li1.Count == 0) 
                                    break;
                                if (li1[0].Coef <= li[0].Coef) 
                                    break;
                            }
                            else 
                            {
                                t.Kit.MiscData.Add("TestAttr", null);
                                List<PersonIdentityToken> li1 = PersonIdentityToken.TryAttach(pits, 1, new Pullenti.Morph.MorphBaseInfo() { Case = Pullenti.Morph.MorphCase.AllCases }, null, false, false);
                                t.Kit.MiscData.Remove("TestAttr");
                                if (li1.Count > 0 && li1[0].Coef >= 1 && li1[0].BeginToken == t) 
                                    continue;
                            }
                        }
                    }
                }
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if ((br != null && t.Next.GetReferent() != null && t.Next.GetReferent().TypeName == ObjNameOrg) && t.Next.Next == br.EndToken) 
                    {
                        pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, t.Next.GetReferent(), false, 0);
                        t1 = br.EndToken;
                        break;
                    }
                    else if (br != null && (br.LengthChar < 40)) 
                    {
                        t = (t1 = (tname1 = br.EndToken));
                        continue;
                    }
                }
                if ((t is Pullenti.Ner.NumberToken) && t.Previous.IsValue("ГЛАВА", null)) 
                    break;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if ((npt == null && (t is Pullenti.Ner.NumberToken) && (t.WhitespacesAfterCount < 3)) && (t.WhitespacesBeforeCount < 3)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt00 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt00 != null) 
                    {
                        if (npt00.EndToken.IsValue("ОРДЕН", null) || npt00.EndToken.IsValue("МЕДАЛЬ", null)) 
                            npt = npt00;
                    }
                }
                bool test = false;
                if (npt != null) 
                {
                    if (_existsInDoctionary(npt.EndToken) && ((npt.Morph.Case.IsGenitive || npt.Morph.Case.IsInstrumental))) 
                        test = true;
                    else if (npt.BeginToken == npt.EndToken && t.LengthChar > 1 && ((t.Chars.IsAllUpper || t.Chars.IsLastLower))) 
                        test = true;
                }
                else if (t.Chars.IsAllUpper || t.Chars.IsLastLower) 
                    test = true;
                if (test) 
                {
                    Pullenti.Ner.ReferentToken rto = t.Kit.ProcessReferent("ORGANIZATION", t);
                    if (rto != null) 
                    {
                        string str = rto.Referent.ToString().ToUpper();
                        if (str.StartsWith("ГОСУДАРСТВЕННАЯ ГРАЖДАНСКАЯ СЛУЖБА")) 
                            rto = null;
                    }
                    if (rto != null && rto.EndChar >= t.EndChar && rto.BeginChar == t.BeginChar) 
                    {
                        pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, rto.Referent, false, 0);
                        pr.AddExtReferent(rto);
                        t = (t1 = rto.EndToken);
                        if (((attrs & PersonAttrAttachAttrs.AfterZamestitel)) != PersonAttrAttachAttrs.No) 
                            break;
                        npt0 = npt;
                        if (t.Next != null && t.Next.IsAnd) 
                        {
                            Pullenti.Ner.ReferentToken rto2 = t.Kit.ProcessReferent("ORGANIZATION", t.Next.Next);
                            if (rto2 != null && rto2.BeginChar == t.Next.Next.BeginChar) 
                            {
                                pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, rto2.Referent, false, 0);
                                pr.AddExtReferent(rto2);
                                t = (t1 = rto2.EndToken);
                            }
                        }
                        continue;
                    }
                    if (npt != null) 
                    {
                        t = (t1 = (tname1 = npt.EndToken));
                        npt0 = npt;
                        continue;
                    }
                }
                if (t.Morph.Class.IsPreposition) 
                {
                    npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt == null && t.Next != null && t.Next.Morph.Class.IsAdverb) 
                        npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && _existsInDoctionary(npt.EndToken)) 
                    {
                        bool ok = false;
                        if ((t.IsValue("ПО", null) && npt.Morph.Case.IsDative && !npt.Noun.IsValue("ИМЯ", "ІМЯ")) && !npt.Noun.IsValue("ПРОЗВИЩЕ", "ПРІЗВИСЬКО") && !npt.Noun.IsValue("ПРОЗВАНИЕ", "ПРОЗВАННЯ")) 
                        {
                            ok = true;
                            if (npt.Noun.IsValue("РАБОТА", "РОБОТА") || npt.Noun.IsValue("ПОДДЕРЖКА", "ПІДТРИМКА") || npt.Noun.IsValue("СОПРОВОЖДЕНИЕ", "СУПРОВІД")) 
                            {
                                Pullenti.Ner.Core.NounPhraseToken npt2 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(npt.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                                if (npt2 != null) 
                                    npt = npt2;
                            }
                        }
                        else if (npt.Noun.IsValue("ОТСТАВКА", null) || npt.Noun.IsValue("ВІДСТАВКА", null)) 
                            ok = true;
                        else if (name == "кандидат" && t.IsValue("В", null)) 
                            ok = true;
                        if (ok) 
                        {
                            t = (t1 = (tname1 = npt.EndToken));
                            npt0 = npt;
                            continue;
                        }
                    }
                    if (t.IsValue("OF", null)) 
                        continue;
                }
                else if (t.IsAnd && npt0 != null) 
                {
                    npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && !((npt.Morph.Class & npt0.Morph.Class)).IsUndefined) 
                    {
                        if (npt0.Chars == npt.Chars) 
                        {
                            t = (t1 = (tname1 = npt.EndToken));
                            npt0 = null;
                            continue;
                        }
                    }
                }
                else if (t.IsCommaAnd && ((!t.IsNewlineAfter || tok.IsNewlineBefore)) && npt0 != null) 
                {
                    npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && !((npt.Morph.Class & npt0.Morph.Class)).IsUndefined) 
                    {
                        if (npt0.Chars == npt.Chars && npt.EndToken.Next != null && npt.EndToken.Next.IsAnd) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(npt.EndToken.Next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt1 != null && !((npt1.Morph.Class & npt.Morph.Class & npt0.Morph.Class)).IsUndefined) 
                            {
                                if (npt0.Chars == npt1.Chars) 
                                {
                                    t = (t1 = (tname1 = npt1.EndToken));
                                    npt0 = null;
                                    continue;
                                }
                            }
                        }
                    }
                }
                else if (t.Morph.Class.IsAdjective && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null && (br.LengthChar < 100)) 
                    {
                        t = (t1 = (tname1 = br.EndToken));
                        npt0 = null;
                        continue;
                    }
                }
                if (t.Chars.IsLatinLetter && t.Previous.Chars.IsCyrillicLetter) 
                {
                    for (; t != null; t = t.Next) 
                    {
                        if (!t.Chars.IsLatinLetter || t.IsNewlineBefore) 
                            break;
                        else 
                            t1 = (tname1 = t);
                    }
                    break;
                }
                if (((t.Chars.IsAllUpper || ((!t.Chars.IsAllLower && !t.Chars.IsCapitalUpper)))) && t.LengthChar > 1 && !t0.Chars.IsAllUpper) 
                {
                    t1 = (tname1 = t);
                    continue;
                }
                if (t.Chars.IsLastLower && t.LengthChar > 2 && !t0.Chars.IsAllUpper) 
                {
                    t1 = (tname1 = t);
                    continue;
                }
                if (((t.Chars.IsLetter && (t.Next is Pullenti.Ner.ReferentToken) && (t.Next.GetReferent() is Pullenti.Ner.Person.PersonReferent)) && !t.Morph.Class.IsPreposition && !t.Morph.Class.IsConjunction) && !t.Morph.Class.IsVerb) 
                {
                    t1 = (tname1 = t);
                    break;
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    if ((t as Pullenti.Ner.NumberToken).BeginToken.IsValue("МИЛЛИОНОВ", null) || (t as Pullenti.Ner.NumberToken).BeginToken.IsValue("МІЛЬЙОНІВ", null)) 
                    {
                        t1 = (tname1 = t);
                        break;
                    }
                }
                if (testPerson) 
                {
                    if (t.Next == null) 
                        break;
                    te = t.Next;
                    if (((te.IsCharOf(",в") || te.IsValue("ИЗ", null))) && te.Next != null) 
                        te = te.Next;
                    if ((((r = te.GetReferent()))) != null) 
                    {
                        if (r.TypeName == ObjNameGeo || r.TypeName == ObjNameOrg || r.TypeName == ObjNameTransport) 
                        {
                            t1 = (tname1 = t);
                            continue;
                        }
                    }
                    break;
                }
                if (t.Morph.Language.IsEn) 
                    break;
                if (t.Morph.Class.IsNoun && t.GetMorphClassInDictionary().IsUndefined && (t.WhitespacesBeforeCount < 2)) 
                {
                    t1 = (tname1 = t);
                    continue;
                }
                if (t.Morph.Class.IsPronoun) 
                    continue;
                break;
            }
            if (tname1 != null) 
            {
                if (pr.FindSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true) == null && (((((tname1.IsValue("КОМПАНИЯ", "КОМПАНІЯ") || tname1.IsValue("ФИРМА", "ФІРМА") || tname1.IsValue("ПРЕДПРИЯТИЕ", "ПІДПРИЄМСТВО")) || tname1.IsValue("ПРЕЗИДИУМ", "ПРЕЗИДІЯ") || tname1.IsValue("ЧАСТЬ", "ЧАСТИНА")) || tname1.IsValue("ФЕДЕРАЦИЯ", "ФЕДЕРАЦІЯ") || tname1.IsValue("ВЕДОМСТВО", "ВІДОМСТВО")) || tname1.IsValue("БАНК", null) || tname1.IsValue("КОРПОРАЦИЯ", "КОРПОРАЦІЯ")))) 
                {
                    if (tname1 == tname0 || ((tname0.IsValue("ЭТОТ", "ЦЕЙ") && tname0.Next == tname1))) 
                    {
                        Pullenti.Ner.Referent org = null;
                        int cou = 0;
                        for (Pullenti.Ner.Token tt0 = t0.Previous; tt0 != null; tt0 = tt0.Previous) 
                        {
                            if (tt0.IsNewlineAfter) 
                                cou += 10;
                            if ((++cou) > 500) 
                                break;
                            List<Pullenti.Ner.Referent> rs0 = tt0.GetReferents();
                            if (rs0 == null) 
                                continue;
                            bool hasOrg = false;
                            foreach (Pullenti.Ner.Referent r0 in rs0) 
                            {
                                if (r0.TypeName == ObjNameOrg) 
                                {
                                    hasOrg = true;
                                    if (tname1.IsValue("БАНК", null)) 
                                    {
                                        if (r0.FindSlot("TYPE", "банк", true) == null) 
                                            continue;
                                    }
                                    if (tname1.IsValue("ЧАСТЬ", "ЧАСТИНА")) 
                                    {
                                        bool ok1 = false;
                                        foreach (Pullenti.Ner.Slot s in r0.Slots) 
                                        {
                                            if (s.TypeName == "TYPE") 
                                            {
                                                if (((string)s.Value).EndsWith("часть") || ((string)s.Value).EndsWith("частина")) 
                                                    ok1 = true;
                                            }
                                        }
                                        if (!ok1) 
                                            continue;
                                    }
                                    org = r0;
                                    break;
                                }
                            }
                            if (org != null || hasOrg) 
                                break;
                        }
                        if (org != null) 
                        {
                            pr.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, org, false, 0);
                            tname1 = null;
                        }
                    }
                }
            }
            if (tname1 != null) 
            {
                string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(tname0, tname1, Pullenti.Ner.Core.GetTextAttr.No);
                if (s != null) 
                    name = string.Format("{0} {1}", name, s.ToLower());
            }
            if (category != null) 
                name = string.Format("{0} {1}", name, category);
            else 
            {
                Pullenti.Ner.Token tt = TryAttachCategory(t1.Next, out category);
                if (tt != null) 
                {
                    name = string.Format("{0} {1}", name, category);
                    t1 = tt;
                }
            }
            pr.Name = name;
            PersonAttrToken res = new PersonAttrToken(t0, t1) { Typ = PersonAttrTerminType.Position, PropRef = pr, Morph = tok.Morph };
            res.CanBeIndependentProperty = (tok.Termin as PersonAttrTermin).CanBeUniqueIdentifier;
            int i = name.IndexOf("заместитель ");
            if (i < 0) 
                i = name.IndexOf("заступник ");
            if (i >= 0) 
            {
                i += 11;
                PersonAttrToken res1 = new PersonAttrToken(t0, t1) { Typ = PersonAttrTerminType.Position, Morph = tok.Morph };
                res1.PropRef = new Pullenti.Ner.Person.PersonPropertyReferent();
                res1.PropRef.Name = name.Substring(0, i);
                res1.PropRef.Higher = res.PropRef;
                res1.HigherPropRef = res;
                res.PropRef.Name = name.Substring(i + 1);
                return res1;
            }
            return res;
        }
        static bool _existsInDoctionary(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return false;
            foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
            {
                if ((wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    return true;
            }
            return false;
        }
        static bool _isPerson(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (t is Pullenti.Ner.ReferentToken) 
                return t.GetReferent() is Pullenti.Ner.Person.PersonReferent;
            if (!t.Chars.IsLetter || t.Chars.IsAllLower) 
                return false;
            Pullenti.Ner.ReferentToken rt00 = t.Kit.ProcessReferent("PERSON", t);
            return rt00 != null && (rt00.Referent is Pullenti.Ner.Person.PersonReferent);
        }
        static Pullenti.Ner.Token _analyzeRomanNums(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.Token tt2 = t;
            if (tt2.IsValue("В", null) && tt2.Next != null) 
                tt2 = tt2.Next;
            Pullenti.Ner.NumberToken lat = Pullenti.Ner.Core.NumberHelper.TryParseRoman(tt2);
            if (lat == null) 
                return null;
            tt2 = lat.EndToken;
            if (tt2.Next != null && tt2.Next.IsHiphen) 
            {
                Pullenti.Ner.NumberToken lat2 = Pullenti.Ner.Core.NumberHelper.TryParseRoman(tt2.Next.Next);
                if (lat2 != null) 
                    tt2 = lat2.EndToken;
            }
            if (tt2.Next != null && ((tt2.Next.IsValue("ВЕК", null) || tt2.Next.IsValue("СТОЛЕТИЕ", null) || tt2.Next.IsValue("СОЗЫВ", null)))) 
                return tt2.Next;
            if (tt2.Next != null && tt2.Next.IsValue("В", null)) 
            {
                tt2 = tt2.Next;
                if (tt2.Next != null && tt2.Next.IsChar('.')) 
                    tt2 = tt2.Next;
                return tt2;
            }
            return null;
        }
        static Pullenti.Ner.Token _analizeVise(Pullenti.Ner.Token t0, ref string name)
        {
            if (t0 == null) 
                return null;
            if (t0.Previous != null && t0.Previous.IsHiphen && (t0.Previous.Previous is Pullenti.Ner.TextToken)) 
            {
                if (t0.Previous.Previous.IsValue("ВИЦЕ", "ВІЦЕ")) 
                {
                    t0 = t0.Previous.Previous;
                    name = ((t0.Kit.BaseLanguage.IsUa ? "віце-" : "вице-")) + name;
                }
                if (t0.Previous != null && t0.Previous.Previous != null) 
                {
                    if (t0.Previous.Previous.IsValue("ЭКС", "ЕКС")) 
                    {
                        t0 = t0.Previous.Previous;
                        name = ((t0.Kit.BaseLanguage.IsUa ? "екс-" : "экс-")) + name;
                    }
                    else if (t0.Previous.Previous.Chars == t0.Chars && !t0.IsWhitespaceBefore && !t0.Previous.IsWhitespaceBefore) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt00 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t0.Previous.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt00 != null) 
                        {
                            name = npt00.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                            t0 = t0.Previous.Previous;
                        }
                    }
                }
            }
            return t0;
        }
        static Pullenti.Ner.Token TryAttachCategory(Pullenti.Ner.Token t, out string cat)
        {
            cat = null;
            if (t == null || t.Next == null) 
                return null;
            Pullenti.Ner.Token tt = null;
            int num = -1;
            if (t is Pullenti.Ner.NumberToken) 
            {
                if ((t as Pullenti.Ner.NumberToken).IntValue == null) 
                    return null;
                num = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                tt = t;
            }
            else 
            {
                Pullenti.Ner.NumberToken npt = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t);
                if (npt != null && npt.IntValue != null) 
                {
                    num = npt.IntValue.Value;
                    tt = npt.EndToken;
                }
            }
            if ((num < 0) && ((t.IsValue("ВЫСШИЙ", null) || t.IsValue("ВЫСШ", null) || t.IsValue("ВИЩИЙ", null)))) 
            {
                num = 0;
                tt = t;
                if (tt.Next != null && tt.Next.IsChar('.')) 
                    tt = tt.Next;
            }
            if (tt == null || tt.Next == null || (num < 0)) 
                return null;
            tt = tt.Next;
            if (tt.IsValue("КАТЕГОРИЯ", null) || tt.IsValue("КАТЕГОРІЯ", null) || tt.IsValue("КАТ", null)) 
            {
                if (tt.Next != null && tt.Next.IsChar('.')) 
                    tt = tt.Next;
                if (num == 0) 
                    cat = (tt.Kit.BaseLanguage.IsUa ? "вищої категорії" : "высшей категории");
                else 
                    cat = (tt.Kit.BaseLanguage.IsUa ? string.Format("{0} категорії", num) : string.Format("{0} категории", num));
                return tt;
            }
            if (tt.IsValue("РАЗРЯД", null) || tt.IsValue("РОЗРЯД", null)) 
            {
                if (num == 0) 
                    cat = (tt.Kit.BaseLanguage.IsUa ? "вищого розряду" : "высшего разряда");
                else 
                    cat = (tt.Kit.BaseLanguage.IsUa ? string.Format("{0} розряду", num) : string.Format("{0} разряда", num));
                return tt;
            }
            if (tt.IsValue("КЛАСС", null) || tt.IsValue("КЛАС", null)) 
            {
                if (num == 0) 
                    cat = (tt.Kit.BaseLanguage.IsUa ? "вищого класу" : "высшего класса");
                else 
                    cat = (tt.Kit.BaseLanguage.IsUa ? string.Format("{0} класу", num) : string.Format("{0} класса", num));
                return tt;
            }
            if (tt.IsValue("РАНГ", null)) 
            {
                if (num == 0) 
                    return null;
                else 
                    cat = string.Format("{0} ранга", num);
                return tt;
            }
            if (tt.IsValue("СОЗЫВ", null) || tt.IsValue("СКЛИКАННЯ", null)) 
            {
                if (num == 0) 
                    return null;
                else 
                    cat = (tt.Kit.BaseLanguage.IsUa ? string.Format("{0} скликання", num) : string.Format("{0} созыва", num));
                return tt;
            }
            return null;
        }
        const string ObjNameGeo = "GEO";
        const string ObjNameAddr = "ADDRESS";
        const string ObjNameOrg = "ORGANIZATION";
        const string ObjNameTransport = "TRANSPORT";
        const string ObjNameDate = "DATE";
        const string ObjNameDateRange = "DATERANGE";
        static PersonAttrToken CreateAttrGrade(Pullenti.Ner.Core.TerminToken tok)
        {
            Pullenti.Ner.Token t1 = _findGradeLast(tok.EndToken.Next, tok.BeginToken);
            if (t1 == null) 
                return null;
            Pullenti.Ner.Person.PersonPropertyReferent pr = new Pullenti.Ner.Person.PersonPropertyReferent();
            pr.Name = string.Format("{0} наук", tok.Termin.CanonicText.ToLower());
            return new PersonAttrToken(tok.BeginToken, t1) { Typ = PersonAttrTerminType.Position, PropRef = pr, Morph = tok.Morph, CanBeIndependentProperty = false };
        }
        static Pullenti.Ner.Token _findGradeLast(Pullenti.Ner.Token t, Pullenti.Ner.Token t0)
        {
            int i = 0;
            Pullenti.Ner.Token t1 = null;
            for (; t != null; t = t.Next) 
            {
                if (t.IsValue("НАУК", null)) 
                {
                    t1 = t;
                    i++;
                    break;
                }
                if (t.IsValue("Н", null)) 
                {
                    if (t0.LengthChar > 1 || t0.Chars != t.Chars) 
                        return null;
                    if ((t.Next != null && t.Next.IsHiphen && t.Next.Next != null) && t.Next.Next.IsValue("К", null)) 
                    {
                        t1 = t.Next.Next;
                        break;
                    }
                    if (t.Next != null && t.Next.IsChar('.')) 
                    {
                        t1 = t.Next;
                        break;
                    }
                }
                if (!t.Chars.IsAllLower && t0.Chars.IsAllLower) 
                    break;
                if ((++i) > 2) 
                    break;
                if (t.Next != null && t.Next.IsChar('.')) 
                    t = t.Next;
                if (t.Next != null && t.Next.IsHiphen) 
                    t = t.Next;
            }
            if (t1 == null || i == 0) 
                return null;
            return t1;
        }
        public static Pullenti.Ner.Person.PersonPropertyKind CheckKind(Pullenti.Ner.Person.PersonPropertyReferent pr)
        {
            if (pr == null) 
                return Pullenti.Ner.Person.PersonPropertyKind.Undefined;
            string n = pr.GetStringValue(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_NAME);
            if (n == null) 
                return Pullenti.Ner.Person.PersonPropertyKind.Undefined;
            n = n.ToUpper();
            foreach (string nn in n.Split(' ', '-')) 
            {
                List<Pullenti.Ner.Core.Termin> li = m_Termins.FindTerminsByString(nn, Pullenti.Morph.MorphLang.RU);
                if (li == null || li.Count == 0) 
                    li = m_Termins.FindTerminsByString(n, Pullenti.Morph.MorphLang.UA);
                if (li != null && li.Count > 0) 
                {
                    PersonAttrTermin pat = li[0] as PersonAttrTermin;
                    if (pat.IsBoss) 
                        return Pullenti.Ner.Person.PersonPropertyKind.Boss;
                    if (pat.IsKin) 
                        return Pullenti.Ner.Person.PersonPropertyKind.Kin;
                    if (pat.Typ == PersonAttrTerminType.King) 
                    {
                        if (n != "ДОН") 
                            return Pullenti.Ner.Person.PersonPropertyKind.King;
                    }
                    if (pat.IsMilitaryRank) 
                    {
                        if (nn == "ВИЦЕ") 
                            continue;
                        if (nn == "КАПИТАН" || nn == "CAPTAIN" || nn == "КАПІТАН") 
                        {
                            Pullenti.Ner.Referent org = pr.GetSlotValue(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF) as Pullenti.Ner.Referent;
                            if (org != null && org.TypeName == "ORGANIZATION") 
                                continue;
                        }
                        return Pullenti.Ner.Person.PersonPropertyKind.MilitaryRank;
                    }
                    if (pat.IsNation) 
                        return Pullenti.Ner.Person.PersonPropertyKind.Nationality;
                }
            }
            return Pullenti.Ner.Person.PersonPropertyKind.Undefined;
        }
        public static Pullenti.Ner.Core.TerminToken TryAttachWord(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if ((tok != null && tok.BeginToken == tok.EndToken && t.LengthChar == 1) && t.IsValue("Д", null)) 
            {
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(t.Next, true) && !t.IsWhitespaceAfter) 
                    return null;
            }
            if (tok != null && tok.Termin.CanonicText == "ГРАФ") 
            {
                tok.Morph = new Pullenti.Ner.MorphCollection(t.Morph);
                tok.Morph.RemoveItems(Pullenti.Morph.MorphGender.Masculine);
            }
            if (tok != null) 
            {
                PersonAttrTermin pat = tok.Termin as PersonAttrTermin;
                if (pat.Typ2 != PersonAttrTerminType2.Undefined && pat.Typ2 != PersonAttrTerminType2.Grade) 
                    return null;
            }
            return tok;
        }
        public static Pullenti.Ner.Core.TerminToken TryAttachPositionWord(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null) 
                return null;
            PersonAttrTermin pat = tok.Termin as PersonAttrTermin;
            if (pat == null) 
                return null;
            if (pat.Typ != PersonAttrTerminType.Position) 
                return null;
            if (pat.Typ2 != PersonAttrTerminType2.Io2 && pat.Typ2 != PersonAttrTerminType2.Undefined) 
                return null;
            return tok;
        }
    }
}