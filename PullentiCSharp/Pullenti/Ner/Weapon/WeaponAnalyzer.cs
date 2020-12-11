/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Weapon
{
    /// <summary>
    /// Анализатор оружия
    /// </summary>
    public class WeaponAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("WEAPON")
        /// </summary>
        public const string ANALYZER_NAME = "WEAPON";
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
                return "Оружие";
            }
        }
        public override string Description
        {
            get
            {
                return "Оружие (пистолеты, пулемёты)";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new WeaponAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Weapon.Internal.MetaWeapon.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Weapon.Internal.MetaWeapon.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("weapon.jpg"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == WeaponReferent.OBJ_TYPENAME) 
                return new WeaponReferent();
            return null;
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME, "ORGANIZATION"};
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 5;
            }
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            Pullenti.Ner.Core.TerminCollection models = new Pullenti.Ner.Core.TerminCollection();
            Dictionary<string, List<Pullenti.Ner.Referent>> objsByModel = new Dictionary<string, List<Pullenti.Ner.Referent>>();
            Pullenti.Ner.Core.TerminCollection objByNames = new Pullenti.Ner.Core.TerminCollection();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                List<Pullenti.Ner.Weapon.Internal.WeaponItemToken> its = Pullenti.Ner.Weapon.Internal.WeaponItemToken.TryParseList(t, 10);
                if (its == null) 
                    continue;
                List<Pullenti.Ner.ReferentToken> rts = this.TryAttach(its, false);
                if (rts != null) 
                {
                    foreach (Pullenti.Ner.ReferentToken rt in rts) 
                    {
                        rt.Referent = ad.RegisterReferent(rt.Referent);
                        kit.EmbedToken(rt);
                        t = rt;
                        foreach (Pullenti.Ner.Slot s in rt.Referent.Slots) 
                        {
                            if (s.TypeName == WeaponReferent.ATTR_MODEL) 
                            {
                                string mod = s.Value.ToString();
                                for (int k = 0; k < 2; k++) 
                                {
                                    if (!char.IsDigit(mod[0])) 
                                    {
                                        List<Pullenti.Ner.Referent> li;
                                        if (!objsByModel.TryGetValue(mod, out li)) 
                                            objsByModel.Add(mod, (li = new List<Pullenti.Ner.Referent>()));
                                        if (!li.Contains(rt.Referent)) 
                                            li.Add(rt.Referent);
                                        models.AddString(mod, li, null, false);
                                    }
                                    if (k > 0) 
                                        break;
                                    string brand = rt.Referent.GetStringValue(WeaponReferent.ATTR_BRAND);
                                    if (brand == null) 
                                        break;
                                    mod = string.Format("{0} {1}", brand, mod);
                                }
                            }
                            else if (s.TypeName == WeaponReferent.ATTR_NAME) 
                                objByNames.Add(new Pullenti.Ner.Core.Termin(s.Value.ToString()) { Tag = rt.Referent });
                        }
                    }
                }
            }
            if (objsByModel.Count == 0 && objByNames.Termins.Count == 0) 
                return;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 10);
                if (br != null) 
                {
                    Pullenti.Ner.Core.TerminToken toks = objByNames.TryParse(t.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (toks != null && toks.EndToken.Next == br.EndToken) 
                    {
                        Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(toks.Termin.Tag as Pullenti.Ner.Referent, br.BeginToken, br.EndToken);
                        kit.EmbedToken(rt0);
                        t = rt0;
                        continue;
                    }
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                    continue;
                if (!t.Chars.IsLetter) 
                    continue;
                Pullenti.Ner.Core.TerminToken tok = models.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok == null) 
                {
                    if (!t.Chars.IsAllLower) 
                        tok = objByNames.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok == null) 
                        continue;
                }
                if (!tok.IsWhitespaceAfter) 
                {
                    if (tok.EndToken.Next == null || !tok.EndToken.Next.IsCharOf(",.)")) 
                    {
                        if (!Pullenti.Ner.Core.BracketHelper.IsBracket(tok.EndToken.Next, false)) 
                            continue;
                    }
                }
                Pullenti.Ner.Referent tr = null;
                List<Pullenti.Ner.Referent> li = tok.Termin.Tag as List<Pullenti.Ner.Referent>;
                if (li != null && li.Count == 1) 
                    tr = li[0];
                else 
                    tr = tok.Termin.Tag as Pullenti.Ner.Referent;
                if (tr != null) 
                {
                    Pullenti.Ner.Weapon.Internal.WeaponItemToken tit = Pullenti.Ner.Weapon.Internal.WeaponItemToken.TryParse(tok.BeginToken.Previous, null, false, true);
                    if (tit != null && tit.Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Brand) 
                    {
                        tr.AddSlot(WeaponReferent.ATTR_BRAND, tit.Value, false, 0);
                        tok.BeginToken = tit.BeginToken;
                    }
                    Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(tr, tok.BeginToken, tok.EndToken);
                    kit.EmbedToken(rt0);
                    t = rt0;
                    continue;
                }
            }
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            List<Pullenti.Ner.Weapon.Internal.WeaponItemToken> its = Pullenti.Ner.Weapon.Internal.WeaponItemToken.TryParseList(begin, 10);
            if (its == null) 
                return null;
            List<Pullenti.Ner.ReferentToken> rr = this.TryAttach(its, true);
            if (rr != null && rr.Count > 0) 
                return rr[0];
            return null;
        }
        List<Pullenti.Ner.ReferentToken> TryAttach(List<Pullenti.Ner.Weapon.Internal.WeaponItemToken> its, bool attach)
        {
            WeaponReferent tr = new WeaponReferent();
            int i;
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Weapon.Internal.WeaponItemToken noun = null;
            Pullenti.Ner.Weapon.Internal.WeaponItemToken brand = null;
            Pullenti.Ner.Weapon.Internal.WeaponItemToken model = null;
            for (i = 0; i < its.Count; i++) 
            {
                if (its[i].Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Noun) 
                {
                    if (its.Count == 1) 
                        return null;
                    if (tr.FindSlot(WeaponReferent.ATTR_TYPE, null, true) != null) 
                    {
                        if (tr.FindSlot(WeaponReferent.ATTR_TYPE, its[i].Value, true) == null) 
                            break;
                    }
                    if (!its[i].IsInternal) 
                        noun = its[i];
                    tr.AddSlot(WeaponReferent.ATTR_TYPE, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(WeaponReferent.ATTR_TYPE, its[i].AltValue, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Brand) 
                {
                    if (tr.FindSlot(WeaponReferent.ATTR_BRAND, null, true) != null) 
                    {
                        if (tr.FindSlot(WeaponReferent.ATTR_BRAND, its[i].Value, true) == null) 
                            break;
                    }
                    if (!its[i].IsInternal) 
                    {
                        if (noun != null && noun.IsDoubt) 
                            noun.IsDoubt = false;
                    }
                    brand = its[i];
                    tr.AddSlot(WeaponReferent.ATTR_BRAND, its[i].Value, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Model) 
                {
                    if (tr.FindSlot(WeaponReferent.ATTR_MODEL, null, true) != null) 
                    {
                        if (tr.FindSlot(WeaponReferent.ATTR_MODEL, its[i].Value, true) == null) 
                            break;
                    }
                    model = its[i];
                    tr.AddSlot(WeaponReferent.ATTR_MODEL, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(WeaponReferent.ATTR_MODEL, its[i].AltValue, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Name) 
                {
                    if (tr.FindSlot(WeaponReferent.ATTR_NAME, null, true) != null) 
                        break;
                    tr.AddSlot(WeaponReferent.ATTR_NAME, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(WeaponReferent.ATTR_NAME, its[i].AltValue, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Number) 
                {
                    if (tr.FindSlot(WeaponReferent.ATTR_NUMBER, null, true) != null) 
                        break;
                    tr.AddSlot(WeaponReferent.ATTR_NUMBER, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(WeaponReferent.ATTR_NUMBER, its[i].AltValue, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Caliber) 
                {
                    if (tr.FindSlot(WeaponReferent.ATTR_CALIBER, null, true) != null) 
                        break;
                    tr.AddSlot(WeaponReferent.ATTR_CALIBER, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(WeaponReferent.ATTR_CALIBER, its[i].AltValue, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Developer) 
                {
                    tr.AddSlot(WeaponReferent.ATTR_REF, its[i].Ref, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Weapon.Internal.WeaponItemToken.Typs.Date) 
                {
                    if (tr.FindSlot(WeaponReferent.ATTR_DATE, null, true) != null) 
                        break;
                    tr.AddSlot(WeaponReferent.ATTR_DATE, its[i].Ref, true, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
            }
            bool hasGoodNoun = (noun == null ? false : !noun.IsDoubt);
            WeaponReferent prev = null;
            if (noun == null) 
            {
                for (Pullenti.Ner.Token tt = its[0].BeginToken.Previous; tt != null; tt = tt.Previous) 
                {
                    if ((((prev = tt.GetReferent() as WeaponReferent))) != null) 
                    {
                        List<Pullenti.Ner.Slot> addSlots = new List<Pullenti.Ner.Slot>();
                        foreach (Pullenti.Ner.Slot s in prev.Slots) 
                        {
                            if (s.TypeName == WeaponReferent.ATTR_TYPE) 
                                tr.AddSlot(s.TypeName, s.Value, false, 0);
                            else if (s.TypeName == WeaponReferent.ATTR_BRAND || s.TypeName == WeaponReferent.ATTR_BRAND || s.TypeName == WeaponReferent.ATTR_MODEL) 
                            {
                                if (tr.FindSlot(s.TypeName, null, true) == null) 
                                    addSlots.Add(s);
                            }
                        }
                        foreach (Pullenti.Ner.Slot s in addSlots) 
                        {
                            tr.AddSlot(s.TypeName, s.Value, false, 0);
                        }
                        hasGoodNoun = true;
                        break;
                    }
                    else if ((tt is Pullenti.Ner.TextToken) && ((!tt.Chars.IsLetter || tt.Morph.Class.IsConjunction))) 
                    {
                    }
                    else 
                        break;
                }
            }
            if (noun == null && model != null) 
            {
                int cou = 0;
                for (Pullenti.Ner.Token tt = its[0].BeginToken.Previous; tt != null && (cou < 100); tt = tt.Previous,cou++) 
                {
                    if ((((prev = tt.GetReferent() as WeaponReferent))) != null) 
                    {
                        if (prev.FindSlot(WeaponReferent.ATTR_MODEL, model.Value, true) == null) 
                            continue;
                        List<Pullenti.Ner.Slot> addSlots = new List<Pullenti.Ner.Slot>();
                        foreach (Pullenti.Ner.Slot s in prev.Slots) 
                        {
                            if (s.TypeName == WeaponReferent.ATTR_TYPE) 
                                tr.AddSlot(s.TypeName, s.Value, false, 0);
                            else if (s.TypeName == WeaponReferent.ATTR_BRAND || s.TypeName == WeaponReferent.ATTR_BRAND) 
                            {
                                if (tr.FindSlot(s.TypeName, null, true) == null) 
                                    addSlots.Add(s);
                            }
                        }
                        foreach (Pullenti.Ner.Slot s in addSlots) 
                        {
                            tr.AddSlot(s.TypeName, s.Value, false, 0);
                        }
                        hasGoodNoun = true;
                        break;
                    }
                }
            }
            if (hasGoodNoun) 
            {
            }
            else if (noun != null) 
            {
                if (model != null || ((brand != null && !brand.IsDoubt))) 
                {
                }
                else 
                    return null;
            }
            else 
            {
                if (model == null) 
                    return null;
                int cou = 0;
                bool ok = false;
                for (Pullenti.Ner.Token tt = t1.Previous; tt != null && (cou < 20); tt = tt.Previous,cou++) 
                {
                    if ((tt.IsValue("ОРУЖИЕ", null) || tt.IsValue("ВООРУЖЕНИЕ", null) || tt.IsValue("ВЫСТРЕЛ", null)) || tt.IsValue("ВЫСТРЕЛИТЬ", null)) 
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok) 
                    return null;
            }
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            res.Add(new Pullenti.Ner.ReferentToken(tr, its[0].BeginToken, t1));
            return res;
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Measure.MeasureAnalyzer.Initialize();
            Pullenti.Ner.Weapon.Internal.MetaWeapon.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Weapon.Internal.WeaponItemToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new WeaponAnalyzer());
        }
    }
}