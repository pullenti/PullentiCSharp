/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Measure
{
    /// <summary>
    /// Анализатор для измеряемых величин. 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor,
    /// </summary>
    public class MeasureAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("MEASURE")
        /// </summary>
        public const string ANALYZER_NAME = "MEASURE";
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
                return "Измеряемые величины";
            }
        }
        public override string Description
        {
            get
            {
                return "Диапазоны и просто значения в некоторых единицах измерения";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new MeasureAnalyzer();
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
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Measure.Internal.MeasureMeta.GlobalMeta, Pullenti.Ner.Measure.Internal.UnitMeta.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Measure.Internal.MeasureMeta.ImageId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("measure.png"));
                res.Add(Pullenti.Ner.Measure.Internal.UnitMeta.ImageId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("munit.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == MeasureReferent.OBJ_TYPENAME) 
                return new MeasureReferent();
            if (type == UnitReferent.OBJ_TYPENAME) 
                return new UnitReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            Pullenti.Ner.Core.TerminCollection addunits = null;
            if (kit.Ontology != null) 
            {
                addunits = new Pullenti.Ner.Core.TerminCollection();
                foreach (Pullenti.Ner.ExtOntologyItem r in kit.Ontology.Items) 
                {
                    UnitReferent uu = r.Referent as UnitReferent;
                    if (uu == null) 
                        continue;
                    if (uu.m_Unit != null) 
                        continue;
                    foreach (Pullenti.Ner.Slot s in uu.Slots) 
                    {
                        if (s.TypeName == UnitReferent.ATTR_NAME || s.TypeName == UnitReferent.ATTR_FULLNAME) 
                            addunits.Add(new Pullenti.Ner.Core.Termin(s.Value as string) { Tag = uu });
                    }
                }
            }
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.Measure.Internal.MeasureToken mt = Pullenti.Ner.Measure.Internal.MeasureToken.TryParseMinimal(t, addunits, false);
                if (mt == null) 
                    mt = Pullenti.Ner.Measure.Internal.MeasureToken.TryParse(t, addunits, true, false, false, false);
                if (mt == null) 
                    continue;
                List<Pullenti.Ner.ReferentToken> rts = mt.CreateRefenetsTokensWithRegister(ad, true);
                if (rts == null) 
                    continue;
                for (int i = 0; i < rts.Count; i++) 
                {
                    Pullenti.Ner.ReferentToken rt = rts[i];
                    t.Kit.EmbedToken(rt);
                    t = rt;
                    for (int j = i + 1; j < rts.Count; j++) 
                    {
                        if (rts[j].BeginToken == rt.BeginToken) 
                            rts[j].BeginToken = t;
                        if (rts[j].EndToken == rt.EndToken) 
                            rts[j].EndToken = t;
                    }
                }
            }
            if (kit.Ontology != null) 
            {
                foreach (Pullenti.Ner.Referent e in ad.Referents) 
                {
                    UnitReferent u = e as UnitReferent;
                    if (u == null) 
                        continue;
                    foreach (Pullenti.Ner.ExtOntologyItem r in kit.Ontology.Items) 
                    {
                        UnitReferent uu = r.Referent as UnitReferent;
                        if (uu == null) 
                            continue;
                        bool ok = false;
                        foreach (Pullenti.Ner.Slot s in uu.Slots) 
                        {
                            if (s.TypeName == UnitReferent.ATTR_NAME || s.TypeName == UnitReferent.ATTR_FULLNAME) 
                            {
                                if (u.FindSlot(null, s.Value, true) != null) 
                                {
                                    ok = true;
                                    break;
                                }
                            }
                        }
                        if (ok) 
                        {
                            u.OntologyItems = new List<Pullenti.Ner.ExtOntologyItem>();
                            u.OntologyItems.Add(r);
                            break;
                        }
                    }
                }
            }
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            Pullenti.Ner.Measure.Internal.MeasureToken mt = Pullenti.Ner.Measure.Internal.MeasureToken.TryParseMinimal(begin, null, true);
            if (mt != null) 
            {
                List<Pullenti.Ner.ReferentToken> rts = mt.CreateRefenetsTokensWithRegister(null, true);
                if (rts != null) 
                    return rts[rts.Count - 1];
            }
            return null;
        }
        public override Pullenti.Ner.ReferentToken ProcessOntologyItem(Pullenti.Ner.Token begin)
        {
            if (!(begin is Pullenti.Ner.TextToken)) 
                return null;
            Pullenti.Ner.Measure.Internal.UnitToken ut = Pullenti.Ner.Measure.Internal.UnitToken.TryParse(begin, null, null, false);
            if (ut != null) 
                return new Pullenti.Ner.ReferentToken(ut.CreateReferentWithRegister(null), ut.BeginToken, ut.EndToken);
            UnitReferent u = new UnitReferent();
            u.AddSlot(UnitReferent.ATTR_NAME, begin.GetSourceText(), false, 0);
            return new Pullenti.Ner.ReferentToken(u, begin, begin);
        }
        static bool m_Initialized = false;
        static object m_Lock = new object();
        public static void Initialize()
        {
            lock (m_Lock) 
            {
                if (m_Initialized) 
                    return;
                m_Initialized = true;
                Pullenti.Ner.Measure.Internal.MeasureMeta.Initialize();
                Pullenti.Ner.Measure.Internal.UnitMeta.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Measure.Internal.UnitsHelper.Initialize();
                Pullenti.Ner.Measure.Internal.NumbersWithUnitToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
                Pullenti.Ner.ProcessorService.RegisterAnalyzer(new MeasureAnalyzer());
            }
        }
    }
}