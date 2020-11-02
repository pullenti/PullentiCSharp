/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Semantic.Internal
{
    static class AnaforHelper
    {
        public static bool ProcessAnafors(List<Pullenti.Semantic.SemObject> objs)
        {
            for (int i = objs.Count - 1; i >= 0; i--) 
            {
                Pullenti.Semantic.SemObject it = objs[i];
                if (it.Typ == Pullenti.Semantic.SemObjectType.PersonalPronoun) 
                {
                }
                else if (it.Morph.NormalFull == "КОТОРЫЙ" && it.LinksFrom.Count == 0) 
                {
                }
                else 
                    continue;
                List<AnaforLink> vars = new List<AnaforLink>();
                for (int j = i - 1; j >= 0; j--) 
                {
                    AnaforLink a = AnaforLink.TryCreate(it, objs[j]);
                    if (a == null) 
                        continue;
                    vars.Add(a);
                    a.Correct();
                }
                if (vars.Count < 1) 
                    continue;
                AnaforLink.Sort(vars);
                if (vars[0].Coef <= 0.1) 
                    continue;
                if (vars[0].TargetList != null) 
                {
                    foreach (Pullenti.Semantic.SemObject tgt in vars[0].TargetList) 
                    {
                        it.Graph.AddLink(Pullenti.Semantic.SemLinkType.Anafor, it, tgt, null, false, null);
                    }
                }
                else 
                {
                    Pullenti.Semantic.SemLink li = it.Graph.AddLink(Pullenti.Semantic.SemLinkType.Anafor, it, vars[0].Target, null, false, null);
                    if (vars.Count > 1 && vars[0].Coef <= (vars[1].Coef * 2) && vars[1].TargetList == null) 
                    {
                        Pullenti.Semantic.SemLink li1 = it.Graph.AddLink(Pullenti.Semantic.SemLinkType.Anafor, it, vars[1].Target, null, false, null);
                        li1.AltLink = li;
                        li.AltLink = li1;
                    }
                }
            }
            return false;
        }
        class AnaforLink : IComparable<AnaforLink>
        {
            public double Coef;
            public Pullenti.Semantic.SemObject Target;
            public List<Pullenti.Semantic.SemObject> TargetList = null;
            public override string ToString()
            {
                if (TargetList == null) 
                    return string.Format("{0}: {1}", Coef, Target);
                StringBuilder tmp = new StringBuilder();
                tmp.AppendFormat("{0}: ", Coef);
                foreach (Pullenti.Semantic.SemObject v in TargetList) 
                {
                    tmp.AppendFormat("{0}; ", v);
                }
                return tmp.ToString();
            }
            public static AnaforLink TryCreate(Pullenti.Semantic.SemObject src, Pullenti.Semantic.SemObject tgt)
            {
                if (tgt.Typ != Pullenti.Semantic.SemObjectType.Noun) 
                    return null;
                if (((src.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) == Pullenti.Morph.MorphNumber.Plural) 
                {
                    if (((tgt.Morph.Number & Pullenti.Morph.MorphNumber.Plural)) != Pullenti.Morph.MorphNumber.Undefined) 
                        return new AnaforLink() { Coef = 1, Target = tgt };
                    AnaforLink res = new AnaforLink() { Coef = 0.5, Target = tgt };
                    res.TargetList = new List<Pullenti.Semantic.SemObject>();
                    foreach (Pullenti.Semantic.SemLink li in tgt.LinksTo) 
                    {
                        Pullenti.Semantic.SemObject frm = li.Source;
                        for (int i = 0; i < frm.LinksFrom.Count; i++) 
                        {
                            res.TargetList.Clear();
                            Pullenti.Semantic.SemLink li0 = frm.LinksFrom[i];
                            if (li0.Target.Typ != Pullenti.Semantic.SemObjectType.Noun) 
                                continue;
                            res.TargetList.Add(li0.Target);
                            for (int j = i + 1; j < frm.LinksFrom.Count; j++) 
                            {
                                Pullenti.Semantic.SemLink li1 = frm.LinksFrom[j];
                                if (li1.Typ == li0.Typ && li1.Preposition == li0.Preposition && li1.Target.Typ == li0.Target.Typ) 
                                    res.TargetList.Add(li1.Target);
                            }
                            if (res.TargetList.Count > 1) 
                                return res;
                        }
                    }
                    return null;
                }
                if (tgt.Morph.Number != Pullenti.Morph.MorphNumber.Undefined && ((tgt.Morph.Number & Pullenti.Morph.MorphNumber.Singular)) == Pullenti.Morph.MorphNumber.Undefined) 
                    return null;
                if (tgt.Morph.Gender != Pullenti.Morph.MorphGender.Undefined) 
                {
                    if (((tgt.Morph.Gender & src.Morph.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                        return null;
                    return new AnaforLink() { Coef = 1, Target = tgt };
                }
                return new AnaforLink() { Coef = 0.1, Target = tgt };
            }
            public static void Sort(List<AnaforLink> li)
            {
                for (int i = 0; i < li.Count; i++) 
                {
                    bool ch = false;
                    for (int j = 0; j < (li.Count - 1); j++) 
                    {
                        if (li[j].CompareTo(li[j + 1]) > 0) 
                        {
                            AnaforLink a = li[j];
                            li[j] = li[j + 1];
                            li[j + 1] = a;
                            ch = true;
                        }
                    }
                    if (!ch) 
                        break;
                }
            }
            public void Correct()
            {
                foreach (Pullenti.Semantic.SemLink li in Target.LinksTo) 
                {
                    if (li.Typ == Pullenti.Semantic.SemLinkType.Naming) 
                        Coef = 0;
                    else if (li.Typ == Pullenti.Semantic.SemLinkType.Agent) 
                        Coef *= 2;
                    else if (li.Typ == Pullenti.Semantic.SemLinkType.Pacient) 
                    {
                        if (li.AltLink == null) 
                            Coef *= 2;
                    }
                    else if (!string.IsNullOrEmpty(li.Preposition)) 
                        Coef /= 2;
                }
            }
            public int CompareTo(AnaforLink other)
            {
                if (Coef > other.Coef) 
                    return -1;
                if (Coef < other.Coef) 
                    return 1;
                return 0;
            }
        }

    }
}