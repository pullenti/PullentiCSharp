/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Denomination
{
    /// <summary>
    /// Анализатор деноминаций и обозначений (типа C#, A-320) 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor, 
    /// указав имя анализатора.
    /// </summary>
    public class DenominationAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("DENOMINATION")
        /// </summary>
        public const string ANALYZER_NAME = "DENOMINATION";
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Деноминации";
            }
        }
        public override string Description
        {
            get
            {
                return "Деноминации и обозначения типа СС-300, АН-24, С++";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new DenominationAnalyzer();
        }
        public override int ProgressWeight
        {
            get
            {
                return 5;
            }
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
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Denomination.Internal.MetaDenom.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Denomination.Internal.MetaDenom.DenomImageId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("denom.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == DenominationReferent.OBJ_TYPENAME) 
                return new DenominationReferent();
            return null;
        }
        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new Pullenti.Ner.Core.AnalyzerDataWithOntology();
        }
        // Основная функция выделения объектов
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerDataWithOntology ad = kit.GetAnalyzerData(this) as Pullenti.Ner.Core.AnalyzerDataWithOntology;
            for (int k = 0; k < 2; k++) 
            {
                bool detectNewDenoms = false;
                DateTime dt = DateTime.Now;
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    if (t.IsWhitespaceBefore) 
                    {
                    }
                    else if (t.Previous != null && ((t.Previous.IsCharOf(",") || Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Previous, false, false)))) 
                    {
                    }
                    else 
                        continue;
                    Pullenti.Ner.ReferentToken rt0 = this.TryAttachSpec(t);
                    if (rt0 != null) 
                    {
                        rt0.Referent = ad.RegisterReferent(rt0.Referent);
                        kit.EmbedToken(rt0);
                        t = rt0;
                        continue;
                    }
                    if (!t.Chars.IsLetter) 
                        continue;
                    if (!this.CanBeStartOfDenom(t)) 
                        continue;
                    if (((DateTime.Now - dt)).TotalMinutes > 1) 
                        break;
                    List<Pullenti.Ner.Core.IntOntologyToken> ot = null;
                    ot = ad.LocalOntology.TryAttach(t, null, false);
                    if (ot != null && (ot[0].Item.Referent is DenominationReferent)) 
                    {
                        if (this.CheckAttach(ot[0].BeginToken, ot[0].EndToken)) 
                        {
                            DenominationReferent cl = ot[0].Item.Referent.Clone() as DenominationReferent;
                            cl.Occurrence.Clear();
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(cl, ot[0].BeginToken, ot[0].EndToken);
                            kit.EmbedToken(rt);
                            t = rt;
                            continue;
                        }
                    }
                    if (k > 0) 
                        continue;
                    if (t != null && t.Kit.Ontology != null) 
                    {
                        if ((((ot = t.Kit.Ontology.AttachToken(DenominationReferent.OBJ_TYPENAME, t)))) != null) 
                        {
                            if (this.CheckAttach(ot[0].BeginToken, ot[0].EndToken)) 
                            {
                                DenominationReferent dr = new DenominationReferent();
                                dr.MergeSlots(ot[0].Item.Referent, true);
                                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(dr), ot[0].BeginToken, ot[0].EndToken);
                                kit.EmbedToken(rt);
                                t = rt;
                                continue;
                            }
                        }
                    }
                    rt0 = this.TryAttach(t, false);
                    if (rt0 != null) 
                    {
                        rt0.Referent = ad.RegisterReferent(rt0.Referent);
                        kit.EmbedToken(rt0);
                        detectNewDenoms = true;
                        t = rt0;
                        if (ad.LocalOntology.Items.Count > 1000) 
                            break;
                    }
                }
                if (!detectNewDenoms) 
                    break;
            }
        }
        bool CanBeStartOfDenom(Pullenti.Ner.Token t)
        {
            if ((t == null || !t.Chars.IsLetter || t.Next == null) || t.IsNewlineAfter) 
                return false;
            if (!(t is Pullenti.Ner.TextToken)) 
                return false;
            if (t.LengthChar > 4) 
                return false;
            t = t.Next;
            if (t.Chars.IsLetter) 
                return false;
            if (t is Pullenti.Ner.NumberToken) 
                return true;
            if (t.IsCharOf("/\\") || t.IsHiphen) 
                return t.Next is Pullenti.Ner.NumberToken;
            if (t.IsCharOf("+*&^#@!_")) 
                return true;
            return false;
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            return this.TryAttach(begin, false);
        }
        public Pullenti.Ner.ReferentToken TryAttach(Pullenti.Ner.Token t, bool forOntology = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.ReferentToken rt0 = this.TryAttachSpec(t);
            if (rt0 != null) 
                return rt0;
            if (t.Chars.IsAllLower) 
            {
                if (!t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.NumberToken)) 
                {
                    if (t.Previous == null || t.IsWhitespaceBefore || t.Previous.IsCharOf(",:")) 
                    {
                    }
                    else 
                        return null;
                }
                else 
                    return null;
            }
            StringBuilder tmp = new StringBuilder();
            Pullenti.Ner.Token t1 = t;
            bool hiph = false;
            bool ok = true;
            int nums = 0;
            int chars = 0;
            for (Pullenti.Ner.Token w = t1.Next; w != null; w = w.Next) 
            {
                if (w.IsWhitespaceBefore && !forOntology) 
                    break;
                if (w.IsCharOf("/\\_") || w.IsHiphen) 
                {
                    hiph = true;
                    tmp.Append('-');
                    continue;
                }
                hiph = false;
                Pullenti.Ner.NumberToken nt = w as Pullenti.Ner.NumberToken;
                if (nt != null) 
                {
                    if (nt.Typ != Pullenti.Ner.NumberSpellingType.Digit) 
                        break;
                    t1 = nt;
                    tmp.Append(nt.GetSourceText());
                    nums++;
                    continue;
                }
                Pullenti.Ner.TextToken tt = w as Pullenti.Ner.TextToken;
                if (tt == null) 
                    break;
                if (tt.LengthChar > 3) 
                {
                    ok = false;
                    break;
                }
                if (!char.IsLetter(tt.Term[0])) 
                {
                    if (tt.IsCharOf(",:") || Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, false, null, false)) 
                        break;
                    if (!tt.IsCharOf("+*&^#@!")) 
                    {
                        ok = false;
                        break;
                    }
                    chars++;
                }
                t1 = tt;
                tmp.Append(tt.GetSourceText());
            }
            if (!forOntology) 
            {
                if ((tmp.Length < 1) || !ok || hiph) 
                    return null;
                if (tmp.Length > 12) 
                    return null;
                char last = tmp[tmp.Length - 1];
                if (last == '!') 
                    return null;
                if ((nums + chars) == 0) 
                    return null;
                if (!this.CheckAttach(t, t1)) 
                    return null;
            }
            DenominationReferent newDr = new DenominationReferent();
            newDr.AddValue(t, t1);
            return new Pullenti.Ner.ReferentToken(newDr, t, t1);
        }
        // Некоторые специфические случаи
        Pullenti.Ner.ReferentToken TryAttachSpec(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
            if (nt != null && nt.Typ == Pullenti.Ner.NumberSpellingType.Digit && nt.Value == "1") 
            {
                if (t.Next != null && t.Next.IsHiphen) 
                    t = t.Next;
                if ((t.Next is Pullenti.Ner.TextToken) && !t.Next.IsWhitespaceBefore) 
                {
                    if (t.Next.IsValue("C", null) || t.Next.IsValue("С", null)) 
                    {
                        DenominationReferent dr = new DenominationReferent();
                        dr.AddSlot(DenominationReferent.ATTR_VALUE, "1С", false, 0);
                        dr.AddSlot(DenominationReferent.ATTR_VALUE, "1C", false, 0);
                        return new Pullenti.Ner.ReferentToken(dr, t0, t.Next);
                    }
                }
            }
            if (((nt != null && nt.Typ == Pullenti.Ner.NumberSpellingType.Digit && (t.Next is Pullenti.Ner.TextToken)) && !t.IsWhitespaceAfter && !t.Next.Chars.IsAllLower) && t.Next.Chars.IsLetter) 
            {
                DenominationReferent dr = new DenominationReferent();
                dr.AddSlot(DenominationReferent.ATTR_VALUE, string.Format("{0}{1}", nt.GetSourceText(), (t.Next as Pullenti.Ner.TextToken).Term), false, 0);
                return new Pullenti.Ner.ReferentToken(dr, t0, t.Next);
            }
            return null;
        }
        bool CheckAttach(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            for (Pullenti.Ner.Token t = begin; t != null && t != end.Next; t = t.Next) 
            {
                if (t != begin) 
                {
                    int co = t.WhitespacesBeforeCount;
                    if (co > 0) 
                    {
                        if (co > 1) 
                            return false;
                        if (t.Chars.IsAllLower) 
                            return false;
                        if (t.Previous.Chars.IsAllLower) 
                            return false;
                    }
                }
            }
            if (!end.IsWhitespaceAfter && end.Next != null) 
            {
                if (!end.Next.IsCharOf(",;") && !Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(end.Next, false, null, false)) 
                    return false;
            }
            return true;
        }
        static bool m_Inites = false;
        public static void Initialize()
        {
            if (m_Inites) 
                return;
            m_Inites = true;
            Pullenti.Ner.Denomination.Internal.MetaDenom.Initialize();
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new DenominationAnalyzer());
        }
    }
}