/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Named
{
    /// <summary>
    /// Анализатор именованных сущностей "тип" + "имя": планеты, памятники, здания, местоположения, планеты и пр.
    /// </summary>
    public class NamedEntityAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("NAMEDENTITY")
        /// </summary>
        public const string ANALYZER_NAME = "NAMEDENTITY";
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
                return "Мелкие именованные сущности";
            }
        }
        public override string Description
        {
            get
            {
                return "Планеты, памятники, здания, местоположения, планеты и пр.";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new NamedEntityAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Named.Internal.MetaNamedEntity.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(NamedEntityKind.Monument.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("monument.png"));
                res.Add(NamedEntityKind.Planet.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("planet.png"));
                res.Add(NamedEntityKind.Location.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("location.png"));
                res.Add(NamedEntityKind.Building.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("building.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == NamedEntityReferent.OBJ_TYPENAME) 
                return new NamedEntityReferent();
            return null;
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME, "ORGANIZATION", "PERSON"};
            }
        }
        public override int ProgressWeight
        {
            get
            {
                return 3;
            }
        }
        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new Pullenti.Ner.Core.AnalyzerDataWithOntology();
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerDataWithOntology ad = kit.GetAnalyzerData(this) as Pullenti.Ner.Core.AnalyzerDataWithOntology;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                List<Pullenti.Ner.Named.Internal.NamedItemToken> li = Pullenti.Ner.Named.Internal.NamedItemToken.TryParseList(t, ad.LocalOntology);
                if (li == null || li.Count == 0) 
                    continue;
                Pullenti.Ner.ReferentToken rt = _tryAttach(li);
                if (rt != null) 
                {
                    rt.Referent = ad.RegisterReferent(rt.Referent);
                    kit.EmbedToken(rt);
                    t = rt;
                    continue;
                }
            }
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            List<Pullenti.Ner.Named.Internal.NamedItemToken> li = Pullenti.Ner.Named.Internal.NamedItemToken.TryParseList(begin, null);
            if (li == null || li.Count == 0) 
                return null;
            Pullenti.Ner.ReferentToken rt = _tryAttach(li);
            if (rt == null) 
                return null;
            rt.Data = begin.Kit.GetAnalyzerData(this);
            return rt;
        }
        static bool canBeRef(NamedEntityKind ki, Pullenti.Ner.Referent re)
        {
            if (re == null) 
                return false;
            if (ki == NamedEntityKind.Monument) 
            {
                if (re.TypeName == "PERSON" || re.TypeName == "PERSONPROPERTY") 
                    return true;
            }
            else if (ki == NamedEntityKind.Location) 
            {
                if (re is Pullenti.Ner.Geo.GeoReferent) 
                {
                    Pullenti.Ner.Geo.GeoReferent geo = re as Pullenti.Ner.Geo.GeoReferent;
                    if (geo.IsRegion || geo.IsState) 
                        return true;
                }
            }
            else if (ki == NamedEntityKind.Building) 
            {
                if (re.TypeName == "ORGANIZATION") 
                    return true;
            }
            return false;
        }
        static Pullenti.Ner.ReferentToken _tryAttach(List<Pullenti.Ner.Named.Internal.NamedItemToken> toks)
        {
            Pullenti.Ner.Named.Internal.NamedItemToken typ = null;
            Pullenti.Ner.Named.Internal.NamedItemToken re = null;
            List<Pullenti.Ner.Named.Internal.NamedItemToken> nams = null;
            NamedEntityKind ki = NamedEntityKind.Undefined;
            int i;
            for (i = 0; i < toks.Count; i++) 
            {
                if (toks[i].TypeValue != null) 
                {
                    if (nams != null && toks[i].NameValue != null) 
                        break;
                    if (typ == null) 
                    {
                        typ = toks[i];
                        ki = typ.Kind;
                    }
                    else if (typ.Kind != toks[i].Kind) 
                        break;
                }
                if (toks[i].NameValue != null) 
                {
                    if (typ != null && toks[i].Kind != NamedEntityKind.Undefined && toks[i].Kind != typ.Kind) 
                        break;
                    if (nams == null) 
                        nams = new List<Pullenti.Ner.Named.Internal.NamedItemToken>();
                    else if (nams[0].IsWellknown != toks[i].IsWellknown) 
                        break;
                    if (ki == NamedEntityKind.Undefined) 
                        ki = toks[i].Kind;
                    nams.Add(toks[i]);
                }
                if (toks[i].TypeValue == null && toks[i].NameValue == null) 
                    break;
                if (re == null && canBeRef(ki, toks[i].Ref)) 
                    re = toks[i];
            }
            if ((i < toks.Count) && toks[i].Ref != null) 
            {
                if (canBeRef(ki, toks[i].Ref)) 
                {
                    re = toks[i];
                    i++;
                }
            }
            bool ok = false;
            if (typ != null) 
            {
                if (nams == null) 
                {
                    if (re == null) 
                        ok = false;
                    else 
                        ok = true;
                }
                else if ((nams[0].BeginChar < typ.EndChar) && !nams[0].IsWellknown) 
                {
                    if (re != null) 
                        ok = true;
                    else if ((nams[0].Chars.IsCapitalUpper && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(nams[0].BeginToken) && typ.Morph.Number != Pullenti.Morph.MorphNumber.Plural) && typ.Morph.Case.IsNominative) 
                        ok = true;
                }
                else 
                    ok = true;
            }
            else if (nams != null) 
            {
                if (nams.Count == 1 && nams[0].Chars.IsAllLower) 
                {
                }
                else if (nams[0].IsWellknown) 
                    ok = true;
            }
            if (!ok || ki == NamedEntityKind.Undefined) 
                return null;
            NamedEntityReferent nam = new NamedEntityReferent() { Kind = ki };
            if (typ != null) 
                nam.AddSlot(NamedEntityReferent.ATTR_TYPE, typ.TypeValue.ToLower(), false, 0);
            if (nams != null) 
            {
                if (nams.Count == 1 && nams[0].IsWellknown && nams[0].TypeValue != null) 
                    nam.AddSlot(NamedEntityReferent.ATTR_TYPE, nams[0].TypeValue.ToLower(), false, 0);
                if (typ != null && (typ.EndChar < nams[0].BeginChar)) 
                {
                    string str = Pullenti.Ner.Core.MiscHelper.GetTextValue(nams[0].BeginToken, nams[nams.Count - 1].EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    nam.AddSlot(NamedEntityReferent.ATTR_NAME, str, false, 0);
                }
                StringBuilder tmp = new StringBuilder();
                foreach (Pullenti.Ner.Named.Internal.NamedItemToken n in nams) 
                {
                    if (tmp.Length > 0) 
                        tmp.Append(' ');
                    tmp.Append(n.NameValue);
                }
                nam.AddSlot(NamedEntityReferent.ATTR_NAME, tmp.ToString(), false, 0);
            }
            if (re != null) 
                nam.AddSlot(NamedEntityReferent.ATTR_REF, re.Ref, false, 0);
            return new Pullenti.Ner.ReferentToken(nam, toks[0].BeginToken, toks[i - 1].EndToken);
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            try 
            {
                Pullenti.Ner.Named.Internal.MetaNamedEntity.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Named.Internal.NamedItemToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new NamedEntityAnalyzer());
        }
    }
}