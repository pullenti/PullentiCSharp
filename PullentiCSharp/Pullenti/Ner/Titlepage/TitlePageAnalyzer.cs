/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Titlepage
{
    /// <summary>
    /// Анализатор титульной информации - название произведения, авторы, год и другие книжные атрибуты. 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor, 
    /// указав имя анализатора.
    /// </summary>
    public class TitlePageAnalyzer : Pullenti.Ner.Analyzer
    {
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        /// <summary>
        /// Имя анализатора ("TITLEPAGE")
        /// </summary>
        public const string ANALYZER_NAME = "TITLEPAGE";
        public override string Caption
        {
            get
            {
                return "Титульный лист";
            }
        }
        public override string Description
        {
            get
            {
                return "Информация из титульных страниц и из заголовков статей, научных работ, дипломов и т.д.";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new TitlePageAnalyzer();
        }
        /// <summary>
        /// Этот анализатор является специфическим (IsSpecific = true)
        /// </summary>
        public override bool IsSpecific
        {
            get
            {
                return true;
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Titlepage.Internal.MetaTitleInfo.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Titlepage.Internal.MetaTitleInfo.TitleInfoImageId, Pullenti.Ner.Booklink.Internal.ResourceHelper.GetBytes("titleinfo.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == TitlePageReferent.OBJ_TYPENAME) 
                return new TitlePageReferent();
            return null;
        }
        public Pullenti.Ner.ReferentToken ProcessReferent1(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            Pullenti.Ner.Token et;
            TitlePageReferent tpr = _process(begin, (end == null ? 0 : end.EndChar), begin.Kit, out et);
            if (tpr == null) 
                return null;
            return new Pullenti.Ner.ReferentToken(tpr, begin, et);
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            Pullenti.Ner.Token et;
            TitlePageReferent tpr = _process(kit.FirstToken, 0, kit, out et);
            if (tpr != null) 
                ad.RegisterReferent(tpr);
        }
        internal static TitlePageReferent _process(Pullenti.Ner.Token begin, int maxCharPos, Pullenti.Ner.Core.AnalysisKit kit, out Pullenti.Ner.Token endToken)
        {
            endToken = begin;
            TitlePageReferent res = new TitlePageReferent();
            Pullenti.Ner.Core.Termin term = null;
            List<Pullenti.Ner.Titlepage.Internal.Line> lines = Pullenti.Ner.Titlepage.Internal.Line.Parse(begin, 30, 1500, maxCharPos);
            if (lines.Count < 1) 
                return null;
            int cou = lines.Count;
            int minNewlinesCount = 10;
            Dictionary<int, int> linesCountStat = new Dictionary<int, int>();
            for (int i = 0; i < lines.Count; i++) 
            {
                if (Pullenti.Ner.Titlepage.Internal.TitleNameToken.CanBeStartOfTextOrContent(lines[i].BeginToken, lines[i].EndToken)) 
                {
                    cou = i;
                    break;
                }
                int j = lines[i].NewlinesBeforeCount;
                if (i > 0 && j > 0) 
                {
                    if (!linesCountStat.ContainsKey(j)) 
                        linesCountStat.Add(j, 1);
                    else 
                        linesCountStat[j]++;
                }
            }
            int max = 0;
            foreach (KeyValuePair<int, int> kp in linesCountStat) 
            {
                if (kp.Value > max) 
                {
                    max = kp.Value;
                    minNewlinesCount = kp.Key;
                }
            }
            int endChar = (cou > 0 ? lines[cou - 1].EndChar : 0);
            if (maxCharPos > 0 && endChar > maxCharPos) 
                endChar = maxCharPos;
            List<Pullenti.Ner.Titlepage.Internal.TitleNameToken> names = new List<Pullenti.Ner.Titlepage.Internal.TitleNameToken>();
            for (int i = 0; i < cou; i++) 
            {
                if (i == 6) 
                {
                }
                for (int j = i; (j < cou) && (j < (i + 5)); j++) 
                {
                    if (i == 6 && j == 8) 
                    {
                    }
                    if (j > i) 
                    {
                        if (lines[j - 1].IsPureEn && lines[j].IsPureRu) 
                            break;
                        if (lines[j - 1].IsPureRu && lines[j].IsPureEn) 
                            break;
                        if (lines[j].NewlinesBeforeCount >= (minNewlinesCount * 2)) 
                            break;
                    }
                    Pullenti.Ner.Titlepage.Internal.TitleNameToken ttt = Pullenti.Ner.Titlepage.Internal.TitleNameToken.TryParse(lines[i].BeginToken, lines[j].EndToken, minNewlinesCount);
                    if (ttt != null) 
                    {
                        if (lines[i].IsPureEn) 
                            ttt.Morph.Language = Pullenti.Morph.MorphLang.EN;
                        else if (lines[i].IsPureRu) 
                            ttt.Morph.Language = Pullenti.Morph.MorphLang.RU;
                        names.Add(ttt);
                    }
                }
            }
            Pullenti.Ner.Titlepage.Internal.TitleNameToken.Sort(names);
            Pullenti.Ner.ReferentToken nameRt = null;
            if (names.Count > 0) 
            {
                int i0 = 0;
                if (names[i0].Morph.Language.IsEn) 
                {
                    for (int ii = 1; ii < names.Count; ii++) 
                    {
                        if (names[ii].Morph.Language.IsRu && names[ii].Rank > 0) 
                        {
                            i0 = ii;
                            break;
                        }
                    }
                }
                term = res.AddName(names[i0].BeginNameToken, names[i0].EndNameToken);
                if (names[i0].TypeValue != null) 
                    res.AddType(names[i0].TypeValue);
                if (names[i0].Speciality != null) 
                    res.Speciality = names[i0].Speciality;
                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(res, names[i0].BeginToken, names[i0].EndToken);
                if (kit != null) 
                    kit.EmbedToken(rt);
                else 
                    res.AddOccurence(new Pullenti.Ner.TextAnnotation(rt.BeginToken, rt.EndToken));
                endToken = rt.EndToken;
                nameRt = rt;
                if (begin.BeginChar == rt.BeginChar) 
                    begin = rt;
            }
            if (term != null && kit != null) 
            {
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    Pullenti.Ner.Core.TerminToken tok = term.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok == null) 
                        continue;
                    Pullenti.Ner.Token t0 = t;
                    Pullenti.Ner.Token t1 = tok.EndToken;
                    if (t1.Next != null && t1.Next.IsChar('.')) 
                        t1 = t1.Next;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0.Previous, false, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1.Next, false, null, false)) 
                    {
                        t0 = t0.Previous;
                        t1 = t1.Next;
                    }
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(res, t0, t1);
                    kit.EmbedToken(rt);
                    t = rt;
                }
            }
            Pullenti.Ner.Titlepage.Internal.PersonRelations pr = new Pullenti.Ner.Titlepage.Internal.PersonRelations();
            Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types persTyp = Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined;
            List<Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types> persTypes = pr.RelTypes;
            for (Pullenti.Ner.Token t = begin; t != null; t = t.Next) 
            {
                if (maxCharPos > 0 && t.BeginChar > maxCharPos) 
                    break;
                if (t == nameRt) 
                    continue;
                Pullenti.Ner.Titlepage.Internal.TitleItemToken tpt = Pullenti.Ner.Titlepage.Internal.TitleItemToken.TryAttach(t);
                if (tpt != null) 
                {
                    persTyp = Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined;
                    if (tpt.Typ == Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Typ) 
                    {
                        if (res.Types.Count == 0) 
                            res.AddType(tpt.Value);
                        else if (res.Types.Count == 1) 
                        {
                            string ty = res.Types[0].ToUpper();
                            if (ty == "РЕФЕРАТ") 
                                res.AddType(tpt.Value);
                            else if (ty == "АВТОРЕФЕРАТ") 
                            {
                                if (tpt.Value == "КАНДИДАТСКАЯ ДИССЕРТАЦИЯ") 
                                    res.AddSlot(TitlePageReferent.ATTR_TYPE, "автореферат кандидатской диссертации", true, 0);
                                else if (tpt.Value == "ДОКТОРСКАЯ ДИССЕРТАЦИЯ") 
                                    res.AddSlot(TitlePageReferent.ATTR_TYPE, "автореферат докторской диссертации", true, 0);
                                else if (tpt.Value == "МАГИСТЕРСКАЯ ДИССЕРТАЦИЯ") 
                                    res.AddSlot(TitlePageReferent.ATTR_TYPE, "автореферат магистерской диссертации", true, 0);
                                else if (tpt.Value == "КАНДИДАТСЬКА ДИСЕРТАЦІЯ") 
                                    res.AddSlot(TitlePageReferent.ATTR_TYPE, "автореферат кандидатської дисертації", true, 0);
                                else if (tpt.Value == "ДОКТОРСЬКА ДИСЕРТАЦІЯ") 
                                    res.AddSlot(TitlePageReferent.ATTR_TYPE, "автореферат докторської дисертації", true, 0);
                                else if (tpt.Value == "МАГІСТЕРСЬКА ДИСЕРТАЦІЯ") 
                                    res.AddSlot(TitlePageReferent.ATTR_TYPE, "автореферат магістерської дисертації", true, 0);
                                else 
                                    res.AddType(tpt.Value);
                            }
                            else if (tpt.Value == "РЕФЕРАТ" || tpt.Value == "АВТОРЕФЕРАТ") 
                            {
                                if (!ty.Contains(tpt.Value)) 
                                    res.AddType(tpt.Value);
                            }
                        }
                    }
                    else if (tpt.Typ == Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Speciality) 
                    {
                        if (res.Speciality == null) 
                            res.Speciality = tpt.Value;
                    }
                    else if (persTypes.Contains(tpt.Typ)) 
                        persTyp = tpt.Typ;
                    t = tpt.EndToken;
                    if (t.EndChar > endToken.EndChar) 
                        endToken = t;
                    if (t.Next != null && t.Next.IsCharOf(":-")) 
                        t = t.Next;
                    continue;
                }
                if (t.EndChar > endChar) 
                    break;
                List<Pullenti.Ner.Referent> rli = t.GetReferents();
                if (rli == null) 
                    continue;
                if (!t.IsNewlineBefore && (t.Previous is Pullenti.Ner.TextToken)) 
                {
                    string s = (t.Previous as Pullenti.Ner.TextToken).Term;
                    if (s == "ИМЕНИ" || s == "ИМ") 
                        continue;
                    if (s == "." && t.Previous.Previous != null && t.Previous.Previous.IsValue("ИМ", null)) 
                        continue;
                }
                foreach (Pullenti.Ner.Referent r in rli) 
                {
                    if (r is Pullenti.Ner.Person.PersonReferent) 
                    {
                        if (r != rli[0]) 
                            continue;
                        Pullenti.Ner.Person.PersonReferent p = r as Pullenti.Ner.Person.PersonReferent;
                        if (persTyp != Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined) 
                        {
                            if (t.Previous != null && t.Previous.IsChar('.')) 
                                persTyp = Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined;
                        }
                        Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types typ = pr.CalcTypFromAttrs(p);
                        if (typ != Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined) 
                        {
                            pr.Add(p, typ, 1);
                            persTyp = typ;
                        }
                        else if (persTyp != Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined) 
                            pr.Add(p, persTyp, 1);
                        else if (t.Previous != null && t.Previous.IsChar('©')) 
                        {
                            persTyp = Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Worker;
                            pr.Add(p, persTyp, 1);
                        }
                        else 
                        {
                            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                            {
                                Pullenti.Ner.Referent rr = tt.GetReferent();
                                if (rr == res) 
                                {
                                    persTyp = Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Worker;
                                    break;
                                }
                                if (rr is Pullenti.Ner.Person.PersonReferent) 
                                {
                                    if (pr.CalcTypFromAttrs(r as Pullenti.Ner.Person.PersonReferent) != Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined) 
                                        break;
                                    else 
                                        continue;
                                }
                                if (rr != null) 
                                    break;
                                tpt = Pullenti.Ner.Titlepage.Internal.TitleItemToken.TryAttach(tt);
                                if (tpt != null) 
                                {
                                    if (tpt.Typ != Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Typ && tpt.Typ != Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.TypAndTheme) 
                                        break;
                                    tt = tpt.EndToken;
                                    if (tt.EndChar > endToken.EndChar) 
                                        endToken = tt;
                                    continue;
                                }
                            }
                            if (persTyp == Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined) 
                            {
                                for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                                {
                                    Pullenti.Ner.Referent rr = tt.GetReferent();
                                    if (rr == res) 
                                    {
                                        persTyp = Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Worker;
                                        break;
                                    }
                                    if (rr != null) 
                                        break;
                                    if ((tt.IsValue("СТУДЕНТ", null) || tt.IsValue("СТУДЕНТКА", null) || tt.IsValue("СЛУШАТЕЛЬ", null)) || tt.IsValue("ДИПЛОМНИК", null) || tt.IsValue("ИСПОЛНИТЕЛЬ", null)) 
                                    {
                                        persTyp = Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Worker;
                                        break;
                                    }
                                    tpt = Pullenti.Ner.Titlepage.Internal.TitleItemToken.TryAttach(tt);
                                    if (tpt != null && tpt.Typ != Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Typ) 
                                        break;
                                }
                            }
                            if (persTyp != Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined) 
                                pr.Add(p, persTyp, 1);
                            else 
                                pr.Add(p, persTyp, (float)0.5);
                            if (t.EndChar > endToken.EndChar) 
                                endToken = t;
                        }
                        continue;
                    }
                    if (r == rli[0]) 
                        persTyp = Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined;
                    if (r is Pullenti.Ner.Date.DateReferent) 
                    {
                        if (res.Date == null) 
                        {
                            res.Date = r as Pullenti.Ner.Date.DateReferent;
                            if (t.EndChar > endToken.EndChar) 
                                endToken = t;
                        }
                    }
                    else if (r is Pullenti.Ner.Geo.GeoReferent) 
                    {
                        if (res.City == null && (r as Pullenti.Ner.Geo.GeoReferent).IsCity) 
                        {
                            res.City = r as Pullenti.Ner.Geo.GeoReferent;
                            if (t.EndChar > endToken.EndChar) 
                                endToken = t;
                        }
                    }
                    if (r is Pullenti.Ner.Org.OrganizationReferent) 
                    {
                        Pullenti.Ner.Org.OrganizationReferent org = r as Pullenti.Ner.Org.OrganizationReferent;
                        if (org.Types.Contains("курс") && org.Number != null) 
                        {
                            int i;
                            if (int.TryParse(org.Number, out i)) 
                            {
                                if (i > 0 && (i < 8)) 
                                    res.StudentYear = i;
                            }
                        }
                        for (; org.Higher != null; org = org.Higher) 
                        {
                            if (org.Kind != Pullenti.Ner.Org.OrganizationKind.Department) 
                                break;
                        }
                        if (org.Kind != Pullenti.Ner.Org.OrganizationKind.Department) 
                        {
                            if (res.Org == null) 
                                res.Org = org;
                            else if (Pullenti.Ner.Org.OrganizationReferent.CanBeHigher(res.Org, org)) 
                                res.Org = org;
                        }
                        if (t.EndChar > endToken.EndChar) 
                            endToken = t;
                    }
                    if ((r is Pullenti.Ner.Uri.UriReferent) || (r is Pullenti.Ner.Geo.GeoReferent)) 
                    {
                        if (t.EndChar > endToken.EndChar) 
                            endToken = t;
                    }
                }
            }
            foreach (Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types ty in persTypes) 
            {
                foreach (Pullenti.Ner.Person.PersonReferent p in pr.GetPersons(ty)) 
                {
                    if (pr.GetAttrNameForType(ty) != null) 
                        res.AddSlot(pr.GetAttrNameForType(ty), p, false, 0);
                }
            }
            if (res.GetSlotValue(TitlePageReferent.ATTR_AUTHOR) == null) 
            {
                foreach (Pullenti.Ner.Person.PersonReferent p in pr.GetPersons(Pullenti.Ner.Titlepage.Internal.TitleItemToken.Types.Undefined)) 
                {
                    res.AddSlot(TitlePageReferent.ATTR_AUTHOR, p, false, 0);
                    break;
                }
            }
            if (res.City == null && res.Org != null) 
            {
                Pullenti.Ner.Slot s = res.Org.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_GEO, null, true);
                if (s != null && (s.Value is Pullenti.Ner.Geo.GeoReferent)) 
                {
                    if ((s.Value as Pullenti.Ner.Geo.GeoReferent).IsCity) 
                        res.City = s.Value as Pullenti.Ner.Geo.GeoReferent;
                }
            }
            if (res.Date == null) 
            {
                for (Pullenti.Ner.Token t = begin; t != null && t.EndChar <= endChar; t = t.Next) 
                {
                    Pullenti.Ner.Geo.GeoReferent city = t.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                    if (city == null) 
                        continue;
                    if (t.Next is Pullenti.Ner.TextToken) 
                    {
                        if (t.Next.IsCharOf(":,") || t.Next.IsHiphen) 
                            t = t.Next;
                    }
                    Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent(Pullenti.Ner.Date.DateAnalyzer.ANALYZER_NAME, t.Next);
                    if (rt != null) 
                    {
                        rt.SaveToLocalOntology();
                        res.Date = rt.Referent as Pullenti.Ner.Date.DateReferent;
                        if (kit != null) 
                            kit.EmbedToken(rt);
                        break;
                    }
                }
            }
            if (res.Slots.Count == 0) 
                return null;
            else 
                return res;
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Titlepage.Internal.MetaTitleInfo.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Titlepage.Internal.TitleItemToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new TitlePageAnalyzer());
        }
    }
}