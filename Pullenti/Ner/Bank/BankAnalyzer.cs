/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Bank
{
    /// <summary>
    /// Анализатор банковских данных (счетов, платёжных реквизитов...)
    /// </summary>
    public class BankAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("BANKDATA")
        /// </summary>
        public const string ANALYZER_NAME = "BANKDATA";
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
                return "Банковские данные";
            }
        }
        public override string Description
        {
            get
            {
                return "Банковские реквизиты, счета и пр.";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new BankAnalyzer();
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
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Bank.Internal.MetaBank.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Bank.Internal.MetaBank.ImageId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("dollar.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == BankDataReferent.OBJ_TYPENAME) 
                return new BankDataReferent();
            return null;
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {"URI", "ORGANIZATION"};
            }
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.ReferentToken rt = null;
                if (t.Chars.IsLetter) 
                {
                    Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null) 
                    {
                        Pullenti.Ner.Token tt = tok.EndToken.Next;
                        if (tt != null && tt.IsChar(':')) 
                            tt = tt.Next;
                        rt = this.TryAttach(tt, true);
                        if (rt != null) 
                            rt.BeginToken = t;
                    }
                }
                if (rt == null && (((t is Pullenti.Ner.ReferentToken) || t.IsNewlineBefore))) 
                    rt = this.TryAttach(t, false);
                if (rt != null) 
                {
                    rt.Referent = ad.RegisterReferent(rt.Referent);
                    kit.EmbedToken(rt);
                    t = rt;
                }
            }
        }
        static bool _isBankReq(string txt)
        {
            if (((((((txt == "Р/С" || txt == "К/С" || txt == "Л/С") || txt == "ОКФС" || txt == "ОКАТО") || txt == "ОГРН" || txt == "БИК") || txt == "SWIFT" || txt == "ОКПО") || txt == "ОКВЭД" || txt == "ОКОНХ") || txt == "КБК" || txt == "ИНН") || txt == "КПП") 
                return true;
            else 
                return false;
        }
        Pullenti.Ner.ReferentToken TryAttach(Pullenti.Ner.Token t, bool keyWord)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            List<string> urisKeys = null;
            List<Pullenti.Ner.Uri.UriReferent> uris = null;
            Pullenti.Ner.Referent org = null;
            Pullenti.Ner.Referent corOrg = null;
            bool orgIsBank = false;
            int empty = 0;
            Pullenti.Ner.Uri.UriReferent lastUri = null;
            for (; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar && t != t0) 
                    break;
                if (t.IsComma || t.Morph.Class.IsPreposition || t.IsCharOf("/\\")) 
                    continue;
                bool bankKeyword = false;
                if (t.IsValue("ПОЛНЫЙ", null) && t.Next != null && ((t.Next.IsValue("НАИМЕНОВАНИЕ", null) || t.Next.IsValue("НАЗВАНИЕ", null)))) 
                {
                    t = t.Next.Next;
                    if (t == null) 
                        break;
                }
                if (t.IsValue("БАНК", null)) 
                {
                    if ((t is Pullenti.Ner.ReferentToken) && t.GetReferent().TypeName == "ORGANIZATION") 
                        bankKeyword = true;
                    Pullenti.Ner.Token tt = t.Next;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null) 
                        tt = npt.EndToken.Next;
                    if (tt != null && tt.IsChar(':')) 
                        tt = tt.Next;
                    if (tt != null) 
                    {
                        if (!bankKeyword) 
                        {
                            t = tt;
                            bankKeyword = true;
                        }
                        else if (tt.GetReferent() != null && tt.GetReferent().TypeName == "ORGANIZATION") 
                            t = tt;
                    }
                }
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r != null && r.TypeName == "ORGANIZATION") 
                {
                    bool isBank = false;
                    int kk = 0;
                    for (Pullenti.Ner.Referent rr = r; rr != null && (kk < 4); rr = rr.ParentReferent,kk++) 
                    {
                        isBank = string.Compare(rr.GetStringValue("KIND") ?? "", "Bank", true) == 0;
                        if (isBank) 
                            break;
                    }
                    if (!isBank && bankKeyword) 
                        isBank = true;
                    if (!isBank && uris != null && urisKeys.Contains("ИНН")) 
                        return null;
                    if ((lastUri != null && lastUri.Scheme == "К/С" && t.Previous != null) && t.Previous.IsValue("В", null)) 
                    {
                        corOrg = r;
                        t1 = t;
                    }
                    else if (org == null || ((!orgIsBank && isBank))) 
                    {
                        org = r;
                        t1 = t;
                        orgIsBank = isBank;
                        if (isBank) 
                            continue;
                    }
                    if (uris == null && !keyWord) 
                        return null;
                    continue;
                }
                if (r is Pullenti.Ner.Uri.UriReferent) 
                {
                    Pullenti.Ner.Uri.UriReferent u = r as Pullenti.Ner.Uri.UriReferent;
                    if (uris == null) 
                    {
                        if (!_isBankReq(u.Scheme)) 
                            return null;
                        if (u.Scheme == "ИНН" && t.IsNewlineAfter) 
                            return null;
                        uris = new List<Pullenti.Ner.Uri.UriReferent>();
                        urisKeys = new List<string>();
                    }
                    else 
                    {
                        if (!_isBankReq(u.Scheme)) 
                            break;
                        if (urisKeys.Contains(u.Scheme)) 
                            break;
                        if (u.Scheme == "ИНН") 
                        {
                            if (empty > 0) 
                                break;
                        }
                    }
                    urisKeys.Add(u.Scheme);
                    uris.Add(u);
                    lastUri = u;
                    t1 = t;
                    empty = 0;
                    continue;
                }
                else if (uris == null && !keyWord && !orgIsBank) 
                    return null;
                if (r != null && ((r.TypeName == "GEO" || r.TypeName == "ADDRESS"))) 
                {
                    empty++;
                    continue;
                }
                if (t is Pullenti.Ner.TextToken) 
                {
                    if (t.IsValue("ПОЛНЫЙ", null) || t.IsValue("НАИМЕНОВАНИЕ", null) || t.IsValue("НАЗВАНИЕ", null)) 
                    {
                    }
                    else if (t.Chars.IsLetter) 
                    {
                        Pullenti.Ner.Core.TerminToken tok = m_Ontology.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok != null) 
                        {
                            t = tok.EndToken;
                            empty = 0;
                        }
                        else 
                        {
                            empty++;
                            if (t.IsNewlineBefore) 
                            {
                                Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (nnn != null && nnn.EndToken.Next != null && nnn.EndToken.Next.IsChar(':')) 
                                    break;
                            }
                        }
                        if (uris == null) 
                            break;
                    }
                }
                if (empty > 2) 
                    break;
                if (empty > 0 && t.IsChar(':') && t.IsNewlineAfter) 
                    break;
                if (((t is Pullenti.Ner.NumberToken) && t.IsNewlineBefore && t.Next != null) && !t.Next.Chars.IsLetter) 
                    break;
            }
            if (uris == null) 
                return null;
            if (!urisKeys.Contains("Р/С") && !urisKeys.Contains("Л/С")) 
                return null;
            bool ok = false;
            if ((uris.Count < 2) && org == null) 
                return null;
            BankDataReferent bdr = new BankDataReferent();
            foreach (Pullenti.Ner.Uri.UriReferent u in uris) 
            {
                bdr.AddSlot(BankDataReferent.ATTR_ITEM, u, false, 0);
            }
            if (org != null) 
                bdr.AddSlot(BankDataReferent.ATTR_BANK, org, false, 0);
            if (corOrg != null) 
                bdr.AddSlot(BankDataReferent.ATTR_CORBANK, corOrg, false, 0);
            Pullenti.Ner.Referent org0 = (t0.Previous == null ? null : t0.Previous.GetReferent());
            if (org0 != null && org0.TypeName == "ORGANIZATION") 
            {
                foreach (Pullenti.Ner.Slot s in org0.Slots) 
                {
                    if (s.Value is Pullenti.Ner.Uri.UriReferent) 
                    {
                        Pullenti.Ner.Uri.UriReferent u = s.Value as Pullenti.Ner.Uri.UriReferent;
                        if (_isBankReq(u.Scheme)) 
                        {
                            if (!urisKeys.Contains(u.Scheme)) 
                                bdr.AddSlot(BankDataReferent.ATTR_ITEM, u, false, 0);
                        }
                    }
                }
            }
            return new Pullenti.Ner.ReferentToken(bdr, t0, t1);
        }
        static Pullenti.Ner.Core.TerminCollection m_Ontology;
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            Pullenti.Ner.Bank.Internal.MetaBank.Initialize();
            m_Ontology = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin("БАНКОВСКИЕ РЕКВИЗИТЫ", null, true);
            t.AddVariant("ПЛАТЕЖНЫЕ РЕКВИЗИТЫ", false);
            t.AddVariant("РЕКВИЗИТЫ", false);
            m_Ontology.Add(t);
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new BankAnalyzer());
        }
    }
}