/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Transport
{
    /// <summary>
    /// Анализатор транспортных стредств
    /// </summary>
    public class TransportAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("TRANSPORT")
        /// </summary>
        public const string ANALYZER_NAME = "TRANSPORT";
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
                return "Транспорт";
            }
        }
        public override string Description
        {
            get
            {
                return "Техника, автомобили, самолёты, корабли...";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new TransportAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Transport.Internal.MetaTransport.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(TransportKind.Fly.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("fly.png"));
                res.Add(TransportKind.Ship.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("ship.png"));
                res.Add(TransportKind.Space.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("space.png"));
                res.Add(TransportKind.Train.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("train.png"));
                res.Add(TransportKind.Auto.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("auto.png"));
                res.Add(Pullenti.Ner.Transport.Internal.MetaTransport.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("transport.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == TransportReferent.OBJ_TYPENAME) 
                return new TransportReferent();
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
                List<Pullenti.Ner.Transport.Internal.TransItemToken> its = Pullenti.Ner.Transport.Internal.TransItemToken.TryParseList(t, 10);
                if (its == null) 
                    continue;
                List<Pullenti.Ner.ReferentToken> rts = this.TryAttach(its, false);
                if (rts != null) 
                {
                    foreach (Pullenti.Ner.ReferentToken rt in rts) 
                    {
                        int cou = 0;
                        for (Pullenti.Ner.Token tt = t.Previous; tt != null && (cou < 1000); tt = tt.Previous,cou++) 
                        {
                            TransportReferent tr = tt.GetReferent() as TransportReferent;
                            if (tr == null) 
                                continue;
                            bool ok = true;
                            foreach (Pullenti.Ner.Slot s in rt.Referent.Slots) 
                            {
                                if (tr.FindSlot(s.TypeName, s.Value, true) == null) 
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            if (ok) 
                            {
                                rt.Referent = tr;
                                break;
                            }
                        }
                        rt.Referent = ad.RegisterReferent(rt.Referent);
                        kit.EmbedToken(rt);
                        t = rt;
                        foreach (Pullenti.Ner.Slot s in rt.Referent.Slots) 
                        {
                            if (s.TypeName == TransportReferent.ATTR_MODEL) 
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
                                    string brand = rt.Referent.GetStringValue(TransportReferent.ATTR_BRAND);
                                    if (brand == null) 
                                        break;
                                    mod = string.Format("{0} {1}", brand, mod);
                                }
                            }
                            else if (s.TypeName == TransportReferent.ATTR_NAME) 
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
                    Pullenti.Ner.Transport.Internal.TransItemToken tit = Pullenti.Ner.Transport.Internal.TransItemToken.TryParse(tok.BeginToken.Previous, null, false, true);
                    if (tit != null && tit.Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Brand) 
                    {
                        tr.AddSlot(TransportReferent.ATTR_BRAND, tit.Value, false, 0);
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
            List<Pullenti.Ner.Transport.Internal.TransItemToken> its = Pullenti.Ner.Transport.Internal.TransItemToken.TryParseList(begin, 10);
            if (its == null) 
                return null;
            List<Pullenti.Ner.ReferentToken> rr = this.TryAttach(its, true);
            if (rr != null && rr.Count > 0) 
                return rr[0];
            return null;
        }
        List<Pullenti.Ner.ReferentToken> TryAttach(List<Pullenti.Ner.Transport.Internal.TransItemToken> its, bool attach)
        {
            TransportReferent tr = new TransportReferent();
            int i;
            Pullenti.Ner.Token t1 = null;
            bool brandIsDoubt = false;
            for (i = 0; i < its.Count; i++) 
            {
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Noun) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_TYPE, null, true) != null) 
                        break;
                    if (its[i].Kind != TransportKind.Undefined) 
                    {
                        if (tr.Kind != TransportKind.Undefined && its[i].Kind != tr.Kind) 
                            break;
                        else 
                            tr.Kind = its[i].Kind;
                    }
                    tr.AddSlot(TransportReferent.ATTR_TYPE, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(TransportReferent.ATTR_TYPE, its[i].AltValue, false, 0);
                    if (its[i].State != null) 
                        tr.AddGeo(its[i].State);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Geo) 
                {
                    if (its[i].State != null) 
                        tr.AddGeo(its[i].State);
                    else if (its[i].Ref != null) 
                        tr.AddGeo(its[i].Ref);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Brand) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_BRAND, null, true) != null) 
                    {
                        if (tr.FindSlot(TransportReferent.ATTR_BRAND, its[i].Value, true) == null) 
                            break;
                    }
                    if (its[i].Kind != TransportKind.Undefined) 
                    {
                        if (tr.Kind != TransportKind.Undefined && its[i].Kind != tr.Kind) 
                            break;
                        else 
                            tr.Kind = its[i].Kind;
                    }
                    tr.AddSlot(TransportReferent.ATTR_BRAND, its[i].Value, false, 0);
                    t1 = its[i].EndToken;
                    brandIsDoubt = its[i].IsDoubt;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Model) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_MODEL, null, true) != null) 
                        break;
                    tr.AddSlot(TransportReferent.ATTR_MODEL, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(TransportReferent.ATTR_MODEL, its[i].AltValue, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Class) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_CLASS, null, true) != null) 
                        break;
                    tr.AddSlot(TransportReferent.ATTR_CLASS, its[i].Value, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Name) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_NAME, null, true) != null) 
                        break;
                    tr.AddSlot(TransportReferent.ATTR_NAME, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(TransportReferent.ATTR_NAME, its[i].AltValue, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Number) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_NUMBER, null, true) != null) 
                        break;
                    if (its[i].Kind != TransportKind.Undefined) 
                    {
                        if (tr.Kind != TransportKind.Undefined && its[i].Kind != tr.Kind) 
                            break;
                        else 
                            tr.Kind = its[i].Kind;
                    }
                    tr.AddSlot(TransportReferent.ATTR_NUMBER, its[i].Value, false, 0);
                    if (its[i].AltValue != null) 
                        tr.AddSlot(TransportReferent.ATTR_NUMBER_REGION, its[i].AltValue, false, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Org) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_ORG, null, true) != null) 
                        break;
                    if (!its[i].Morph.Case.IsUndefined && !its[i].Morph.Case.IsGenitive) 
                        break;
                    tr.AddSlot(TransportReferent.ATTR_ORG, its[i].Ref, true, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Date) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_DATE, null, true) != null) 
                        break;
                    tr.AddSlot(TransportReferent.ATTR_DATE, its[i].Ref, true, 0);
                    t1 = its[i].EndToken;
                    continue;
                }
                if (its[i].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Route) 
                {
                    if (tr.FindSlot(TransportReferent.ATTR_ROUTEPOINT, null, true) != null) 
                        break;
                    foreach (object o in its[i].RouteItems) 
                    {
                        tr.AddSlot(TransportReferent.ATTR_ROUTEPOINT, o, false, 0);
                    }
                    t1 = its[i].EndToken;
                    continue;
                }
            }
            if (!tr.Check(attach, brandIsDoubt)) 
                return null;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            res.Add(new Pullenti.Ner.ReferentToken(tr, its[0].BeginToken, t1));
            if ((i < its.Count) && tr.Kind == TransportKind.Ship && its[i - 1].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Name) 
            {
                for (; i < its.Count; i++) 
                {
                    if (its[i].Typ != Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Name || !its[i].IsAfterConjunction) 
                        break;
                    TransportReferent tr1 = new TransportReferent();
                    tr1.MergeSlots(tr, true);
                    tr1.AddSlot(TransportReferent.ATTR_NAME, its[i].Value, true, 0);
                    res.Add(new Pullenti.Ner.ReferentToken(tr1, its[i].BeginToken, its[i].EndToken));
                }
            }
            else if (i == its.Count && its[its.Count - 1].Typ == Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Number) 
            {
                for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
                {
                    if (!tt.IsCommaAnd) 
                        break;
                    Pullenti.Ner.Transport.Internal.TransItemToken nn = Pullenti.Ner.Transport.Internal.TransItemToken._attachRusAutoNumber(tt.Next);
                    if (nn == null) 
                        nn = Pullenti.Ner.Transport.Internal.TransItemToken._attachNumber(tt.Next, false);
                    if (nn == null || nn.Typ != Pullenti.Ner.Transport.Internal.TransItemToken.Typs.Number) 
                        break;
                    TransportReferent tr1 = new TransportReferent();
                    foreach (Pullenti.Ner.Slot s in tr.Slots) 
                    {
                        if (s.TypeName != TransportReferent.ATTR_NUMBER) 
                        {
                            if (s.TypeName == TransportReferent.ATTR_NUMBER_REGION && nn.AltValue != null) 
                                continue;
                            tr1.AddSlot(s.TypeName, s.Value, false, 0);
                        }
                    }
                    tr1.AddSlot(TransportReferent.ATTR_NUMBER, nn.Value, true, 0);
                    if (nn.AltValue != null) 
                        tr1.AddSlot(TransportReferent.ATTR_NUMBER_REGION, nn.AltValue, true, 0);
                    res.Add(new Pullenti.Ner.ReferentToken(tr1, nn.BeginToken, nn.EndToken));
                    tt = nn.EndToken;
                }
            }
            return res;
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Transport.Internal.MetaTransport.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Transport.Internal.TransItemToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new TransportAnalyzer());
        }
    }
}