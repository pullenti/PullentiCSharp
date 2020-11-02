/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    // Данные, полученные в ходе обработки, причём с поддержкой механизма онтологий
    public class AnalyzerDataWithOntology : AnalyzerData
    {
        public IntOntologyCollection LocalOntology = new IntOntologyCollection();
        public AnalyzerDataWithOntology()
        {
        }
        public override Pullenti.Ner.Referent RegisterReferent(Pullenti.Ner.Referent referent)
        {
            Pullenti.Ner.Referent res;
            List<Pullenti.Ner.Referent> li = LocalOntology.TryAttachByReferent(referent, null, true);
            if (li != null) 
            {
                for (int i = li.Count - 1; i >= 0; i--) 
                {
                    if (li[i].CanBeGeneralFor(referent) || referent.CanBeGeneralFor(li[i])) 
                        li.RemoveAt(i);
                }
            }
            if (li != null && li.Count > 0) 
            {
                res = li[0];
                if (res != referent) 
                    res.MergeSlots(referent, true);
                if (li.Count > 1 && Kit != null) 
                {
                    for (int i = 1; i < li.Count; i++) 
                    {
                        li[0].MergeSlots(li[i], true);
                        foreach (Pullenti.Ner.TextAnnotation ta in li[i].Occurrence) 
                        {
                            li[0].AddOccurence(ta);
                        }
                        Kit.ReplaceReferent(li[i], li[0]);
                        LocalOntology.Remove(li[i]);
                    }
                }
                if (res.m_ExtReferents != null) 
                    res = base.RegisterReferent(res);
                LocalOntology.AddReferent(res);
                return res;
            }
            res = base.RegisterReferent(referent);
            if (res == null) 
                return null;
            LocalOntology.AddReferent(res);
            return res;
        }
        public override void RemoveReferent(Pullenti.Ner.Referent r)
        {
            LocalOntology.Remove(r);
            base.RemoveReferent(r);
        }
    }
}