/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core.Internal
{
    static class GeneralRelationHelper
    {
        public static void RefreshGenerals(Pullenti.Ner.Processor proc, Pullenti.Ner.Core.AnalysisKit kit)
        {
            Dictionary<string, Dictionary<string, List<Pullenti.Ner.Referent>>> all = new Dictionary<string, Dictionary<string, List<Pullenti.Ner.Referent>>>();
            List<Node> allRefs = new List<Node>();
            foreach (Pullenti.Ner.Analyzer a in proc.Analyzers) 
            {
                Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(a);
                if (ad == null) 
                    continue;
                foreach (Pullenti.Ner.Referent r in ad.Referents) 
                {
                    Node nod = new Node() { Ref = r, Ad = ad };
                    allRefs.Add(nod);
                    r.Tag = nod;
                    Dictionary<string, List<Pullenti.Ner.Referent>> si;
                    if (!all.TryGetValue(a.Name, out si)) 
                        all.Add(a.Name, (si = new Dictionary<string, List<Pullenti.Ner.Referent>>()));
                    List<string> strs = r.GetCompareStrings();
                    if (strs == null || strs.Count == 0) 
                        continue;
                    foreach (string s in strs) 
                    {
                        if (s == null) 
                            continue;
                        List<Pullenti.Ner.Referent> li;
                        if (!si.TryGetValue(s, out li)) 
                            si.Add(s, (li = new List<Pullenti.Ner.Referent>()));
                        li.Add(r);
                    }
                }
            }
            foreach (Node r in allRefs) 
            {
                foreach (Pullenti.Ner.Slot s in r.Ref.Slots) 
                {
                    if (s.Value is Pullenti.Ner.Referent) 
                    {
                        Pullenti.Ner.Referent to = s.Value as Pullenti.Ner.Referent;
                        Node tn = to.Tag as Node;
                        if (tn == null) 
                            continue;
                        if (tn.RefsFrom == null) 
                            tn.RefsFrom = new List<Node>();
                        tn.RefsFrom.Add(r);
                        if (r.RefsTo == null) 
                            r.RefsTo = new List<Node>();
                        r.RefsTo.Add(tn);
                    }
                }
            }
            foreach (Dictionary<string, List<Pullenti.Ner.Referent>> ty in all.Values) 
            {
                foreach (List<Pullenti.Ner.Referent> li in ty.Values) 
                {
                    if (li.Count < 2) 
                        continue;
                    if (li.Count > 3000) 
                        continue;
                    for (int i = 0; i < li.Count; i++) 
                    {
                        for (int j = i + 1; j < li.Count; j++) 
                        {
                            Node n1 = null;
                            Node n2 = null;
                            if (li[i].CanBeGeneralFor(li[j]) && !li[j].CanBeGeneralFor(li[i])) 
                            {
                                n1 = li[i].Tag as Node;
                                n2 = li[j].Tag as Node;
                            }
                            else if (li[j].CanBeGeneralFor(li[i]) && !li[i].CanBeGeneralFor(li[j])) 
                            {
                                n1 = li[j].Tag as Node;
                                n2 = li[i].Tag as Node;
                            }
                            if (n1 != null && n2 != null) 
                            {
                                if (n1.GenFrom == null) 
                                    n1.GenFrom = new List<Node>();
                                if (!n1.GenFrom.Contains(n2)) 
                                    n1.GenFrom.Add(n2);
                                if (n2.GenTo == null) 
                                    n2.GenTo = new List<Node>();
                                if (!n2.GenTo.Contains(n1)) 
                                    n2.GenTo.Add(n1);
                            }
                        }
                    }
                }
            }
            foreach (Node n in allRefs) 
            {
                if (n.GenTo != null && n.GenTo.Count > 1) 
                {
                    for (int i = n.GenTo.Count - 1; i >= 0; i--) 
                    {
                        Node p = n.GenTo[i];
                        bool del = false;
                        for (int j = 0; j < n.GenTo.Count; j++) 
                        {
                            if (j != i && n.GenTo[j].IsInGenParentsOrHigher(p)) 
                                del = true;
                        }
                        if (del) 
                        {
                            p.GenFrom.Remove(n);
                            n.GenTo.RemoveAt(i);
                        }
                    }
                }
            }
            foreach (Node n in allRefs) 
            {
                if (!n.Deleted && n.GenTo != null && n.GenTo.Count == 1) 
                {
                    Node p = n.GenTo[0];
                    if (p.GenFrom.Count == 1) 
                    {
                        n.Ref.MergeSlots(p.Ref, true);
                        p.Ref.Tag = n.Ref;
                        p.ReplaceValues(n);
                        foreach (Pullenti.Ner.TextAnnotation o in p.Ref.Occurrence) 
                        {
                            n.Ref.AddOccurence(o);
                        }
                        p.Deleted = true;
                    }
                    else 
                        n.Ref.GeneralReferent = p.Ref;
                }
            }
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                _correctReferents(t);
            }
            foreach (Node n in allRefs) 
            {
                if (n.Deleted) 
                    n.Ad.RemoveReferent(n.Ref);
                n.Ref.Tag = null;
            }
        }
        static void _correctReferents(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
            if (rt == null) 
                return;
            if (rt.Referent != null && (rt.Referent.Tag is Pullenti.Ner.Referent)) 
                rt.Referent = rt.Referent.Tag as Pullenti.Ner.Referent;
            for (Pullenti.Ner.Token tt = rt.BeginToken; tt != null && tt.EndChar <= rt.EndChar; tt = tt.Next) 
            {
                _correctReferents(tt);
            }
        }
        class Node
        {
            public Pullenti.Ner.Referent Ref;
            public Pullenti.Ner.Core.AnalyzerData Ad;
            public List<Node> RefsTo;
            public List<Node> RefsFrom;
            public List<Node> GenTo;
            public List<Node> GenFrom;
            public bool Deleted;
            public override string ToString()
            {
                return Ref.ToString();
            }
            public bool IsInGenParentsOrHigher(Node n)
            {
                if (GenTo == null) 
                    return false;
                foreach (Node p in GenTo) 
                {
                    if (p == n) 
                        return true;
                    else if (p.IsInGenParentsOrHigher(n)) 
                        return true;
                }
                return false;
            }
            public void ReplaceValues(Node newNode)
            {
                if (RefsFrom != null) 
                {
                    foreach (Node fr in RefsFrom) 
                    {
                        bool ch = false;
                        foreach (Pullenti.Ner.Slot s in fr.Ref.Slots) 
                        {
                            if (s.Value == Ref) 
                            {
                                fr.Ref.UploadSlot(s, newNode.Ref);
                                ch = true;
                            }
                        }
                        if (!ch) 
                            continue;
                        for (int i = 0; i < (fr.Ref.Slots.Count - 1); i++) 
                        {
                            for (int j = i + 1; j < fr.Ref.Slots.Count; j++) 
                            {
                                if (fr.Ref.Slots[i].TypeName == fr.Ref.Slots[j].TypeName && fr.Ref.Slots[i].Value == fr.Ref.Slots[j].Value) 
                                {
                                    fr.Ref.Slots.RemoveAt(j);
                                    j--;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}