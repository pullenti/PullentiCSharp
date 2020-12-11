/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Semantic.Internal
{
    static class OptimizerHelper
    {
        public static void Optimize(Pullenti.Semantic.SemDocument doc, Pullenti.Semantic.SemProcessParams pars)
        {
            foreach (Pullenti.Semantic.SemBlock blk in doc.Blocks) 
            {
                foreach (Pullenti.Semantic.SemFragment fr in blk.Fragments) 
                {
                    _optimizeGraph(fr.Graph);
                }
                List<Pullenti.Semantic.SemObject> objs = new List<Pullenti.Semantic.SemObject>();
                objs.AddRange(blk.Graph.Objects);
                foreach (Pullenti.Semantic.SemFragment fr in blk.Fragments) 
                {
                    objs.AddRange(fr.Graph.Objects);
                }
                foreach (Pullenti.Semantic.SemFragment fr in blk.Fragments) 
                {
                    for (int i = fr.Graph.Links.Count - 1; i >= 0; i--) 
                    {
                        Pullenti.Semantic.SemLink li = fr.Graph.Links[i];
                        if (!objs.Contains(li.Source) || !objs.Contains(li.Target)) 
                            fr.Graph.RemoveLink(li);
                    }
                    _processParticiples(fr.Graph);
                    _processLinks(fr.Graph);
                }
                _sortObjects(objs);
                _processPointers(objs);
                _processFormulas(objs);
                if (pars.DontCreateAnafor) 
                {
                }
                else 
                {
                    AnaforHelper.ProcessAnafors(objs);
                    foreach (Pullenti.Semantic.SemFragment fr in blk.Fragments) 
                    {
                        _collapseAnafors(fr.Graph);
                    }
                }
            }
        }
        static void _optimizeGraph(Pullenti.Semantic.SemGraph gr)
        {
            foreach (Pullenti.Semantic.SemObject o in gr.Objects) 
            {
                _optimizeTokens(o);
            }
            _sortObjects(gr.Objects);
        }
        static int _compareToks(Pullenti.Ner.Token t1, Pullenti.Ner.Token t2)
        {
            if (t1.BeginChar < t2.BeginChar) 
                return -1;
            if (t1.BeginChar > t2.BeginChar) 
                return 1;
            if (t1.EndChar < t2.EndChar) 
                return -1;
            if (t1.EndChar > t2.EndChar) 
                return 1;
            return 0;
        }
        static void _optimizeTokens(Pullenti.Semantic.SemObject o)
        {
            for (int i = 0; i < o.Tokens.Count; i++) 
            {
                bool ch = false;
                for (int j = 0; j < (o.Tokens.Count - 1); j++) 
                {
                    if (_compareToks(o.Tokens[j], o.Tokens[j + 1]) > 0) 
                    {
                        Pullenti.Ner.MetaToken t = o.Tokens[j];
                        o.Tokens[j] = o.Tokens[j + 1];
                        o.Tokens[j + 1] = t;
                        ch = true;
                    }
                }
                if (!ch) 
                    break;
            }
            for (int i = 0; i < (o.Tokens.Count - 1); i++) 
            {
                if (o.Tokens[i].EndToken.Next == o.Tokens[i + 1].BeginToken) 
                {
                    o.Tokens[i] = new Pullenti.Ner.MetaToken(o.Tokens[i].BeginToken, o.Tokens[i + 1].EndToken);
                    o.Tokens.RemoveAt(i + 1);
                    i--;
                }
            }
        }
        static void _sortObjects(List<Pullenti.Semantic.SemObject> objs)
        {
            for (int i = 0; i < objs.Count; i++) 
            {
                bool ch = false;
                for (int j = 0; j < (objs.Count - 1); j++) 
                {
                    if (objs[j].CompareTo(objs[j + 1]) > 0) 
                    {
                        Pullenti.Semantic.SemObject o = objs[j];
                        objs[j] = objs[j + 1];
                        objs[j + 1] = o;
                        ch = true;
                    }
                }
                if (!ch) 
                    break;
            }
        }
        static bool _processParticiples(Pullenti.Semantic.SemGraph gr)
        {
            bool ret = false;
            for (int i = 0; i < gr.Objects.Count; i++) 
            {
                Pullenti.Semantic.SemObject obj = gr.Objects[i];
                if (obj.Typ != Pullenti.Semantic.SemObjectType.Participle) 
                    continue;
                Pullenti.Semantic.SemLink own = null;
                bool has = false;
                foreach (Pullenti.Semantic.SemLink li in obj.LinksTo) 
                {
                    if (li.Typ == Pullenti.Semantic.SemLinkType.Participle) 
                        own = li;
                    else 
                        has = true;
                }
                if (!has) 
                    continue;
                if (own == null) 
                {
                    Pullenti.Semantic.SemObject dum = new Pullenti.Semantic.SemObject(gr) { Typ = Pullenti.Semantic.SemObjectType.Noun };
                    if (obj.Morph != null) 
                        dum.Morph = new Pullenti.Morph.MorphWordForm() { Class = Pullenti.Morph.MorphClass.Noun, Number = obj.Morph.Number, Gender = obj.Morph.Gender, Case = obj.Morph.Case };
                    gr.Objects.Add(dum);
                    own = gr.AddLink(Pullenti.Semantic.SemLinkType.Participle, dum, obj, "какой", false, null);
                    ret = true;
                }
                for (int j = obj.LinksTo.Count - 1; j >= 0; j--) 
                {
                    Pullenti.Semantic.SemLink li = obj.LinksTo[j];
                    if (li.Typ == Pullenti.Semantic.SemLinkType.Participle) 
                        continue;
                    bool exi = false;
                    foreach (Pullenti.Semantic.SemLink ll in li.Source.LinksFrom) 
                    {
                        if (ll.Target == own.Source) 
                            exi = true;
                    }
                    if (exi) 
                        gr.RemoveLink(li);
                    else 
                    {
                        obj.LinksTo.RemoveAt(j);
                        li.m_Target = own.Source;
                    }
                    ret = true;
                }
            }
            return ret;
        }
        static bool _processLinks(Pullenti.Semantic.SemGraph gr)
        {
            bool ret = false;
            for (int i = 0; i < gr.Objects.Count; i++) 
            {
                Pullenti.Semantic.SemObject obj = gr.Objects[i];
                for (int j = obj.LinksFrom.Count - 1; j >= 0; j--) 
                {
                    Pullenti.Semantic.SemLink li = obj.LinksFrom[j];
                    if (li.Typ != Pullenti.Semantic.SemLinkType.Pacient) 
                        continue;
                    bool exi = false;
                    foreach (Pullenti.Semantic.SemLink ll in obj.LinksFrom) 
                    {
                        if (ll != li && ll.Typ == Pullenti.Semantic.SemLinkType.Agent && ll.Target == li.Target) 
                            exi = true;
                    }
                    if (exi) 
                    {
                        if (obj.BeginChar > li.Target.BeginChar) 
                        {
                            gr.RemoveLink(li);
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }
        static bool _collapseAnafors(Pullenti.Semantic.SemGraph gr)
        {
            bool ret = false;
            for (int i = 0; i < gr.Objects.Count; i++) 
            {
                Pullenti.Semantic.SemObject obj = gr.Objects[i];
                if (obj.Typ == Pullenti.Semantic.SemObjectType.PersonalPronoun || obj.Morph.NormalFull == "КОТОРЫЙ") 
                {
                }
                else 
                    continue;
                if (obj.Attrs.Count > 0 || obj.Quantity != null) 
                    continue;
                if (obj.LinksFrom.Count == 1 && obj.LinksFrom[0].Typ == Pullenti.Semantic.SemLinkType.Anafor) 
                {
                }
                else if (obj.LinksFrom.Count == 2 && obj.LinksFrom[0].Typ == Pullenti.Semantic.SemLinkType.Anafor && obj.LinksFrom[0].AltLink == obj.LinksFrom[1]) 
                {
                }
                else 
                    continue;
                Pullenti.Semantic.SemLink alink = obj.LinksFrom[0];
                foreach (Pullenti.Semantic.SemLink li in obj.LinksTo) 
                {
                    Pullenti.Semantic.SemLink nli = gr.AddLink(li.Typ, li.Source, alink.Target, li.Question, li.IsOr, li.Preposition);
                    if (alink.AltLink != null) 
                    {
                        Pullenti.Semantic.SemLink nli2 = gr.AddLink(li.Typ, li.Source, alink.AltLink.Target, li.Question, li.IsOr, li.Preposition);
                        nli2.AltLink = nli;
                        nli.AltLink = nli2;
                    }
                }
                gr.RemoveObject(obj);
                i--;
                ret = true;
            }
            return ret;
        }
        static bool _processFormulas(List<Pullenti.Semantic.SemObject> objs)
        {
            bool ret = false;
            for (int i = 0; i < objs.Count; i++) 
            {
                Pullenti.Semantic.SemObject o = objs[i];
                if (o.Typ != Pullenti.Semantic.SemObjectType.Noun || !o.IsValue("РАЗ", Pullenti.Semantic.SemObjectType.Undefined)) 
                    continue;
                if (o.Quantity == null) 
                    continue;
                if (o.LinksFrom.Count == 0 && o.LinksTo.Count == 1) 
                {
                }
                else 
                    continue;
                Pullenti.Semantic.SemObject frm = o.LinksTo[0].Source;
                for (int k = 0; k < 5; k++) 
                {
                    bool brek = false;
                    foreach (Pullenti.Semantic.SemLink li in frm.LinksFrom) 
                    {
                        if (((li.Typ == Pullenti.Semantic.SemLinkType.Detail || li.Typ == Pullenti.Semantic.SemLinkType.Pacient)) && li.Target != o) 
                        {
                            if (o.BeginChar > frm.EndChar && (o.BeginChar < li.Target.BeginChar)) 
                            {
                                brek = true;
                                o.Graph.AddLink(Pullenti.Semantic.SemLinkType.Detail, o, li.Target, "чего", false, null);
                                o.Graph.RemoveLink(li);
                            }
                            else 
                                frm = li.Target;
                            break;
                        }
                    }
                    if (brek) 
                        break;
                }
            }
            return ret;
        }
        static bool _processPointers(List<Pullenti.Semantic.SemObject> objs)
        {
            bool ret = false;
            for (int i = 0; i < objs.Count; i++) 
            {
                Pullenti.Semantic.SemObject o = objs[i];
                if (o.Typ != Pullenti.Semantic.SemObjectType.Noun) 
                    continue;
                if (o.Quantity != null && o.Quantity.Spelling == "1") 
                {
                }
                else 
                    continue;
                if (o.LinksFrom.Count > 0) 
                    continue;
                bool ok = false;
                for (int j = i - 1; j >= 0; j--) 
                {
                    Pullenti.Semantic.SemObject oo = objs[j];
                    if (oo.Typ != Pullenti.Semantic.SemObjectType.Noun) 
                        continue;
                    if (oo.Morph.NormalFull != o.Morph.NormalFull) 
                        continue;
                    if (oo.Quantity != null && oo.Quantity.Spelling != "1") 
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok) 
                {
                    for (int j = i + 1; j < objs.Count; j++) 
                    {
                        Pullenti.Semantic.SemObject oo = objs[j];
                        if (oo.Typ != Pullenti.Semantic.SemObjectType.Noun) 
                            continue;
                        if (oo.Morph.NormalFull != o.Morph.NormalFull) 
                            continue;
                        if (oo.FindFromObject("ДРУГОЙ", Pullenti.Semantic.SemLinkType.Undefined, Pullenti.Semantic.SemObjectType.Undefined) != null || oo.FindFromObject("ВТОРОЙ", Pullenti.Semantic.SemLinkType.Undefined, Pullenti.Semantic.SemObjectType.Undefined) != null) 
                        {
                            ok = true;
                            break;
                        }
                    }
                }
                if (!ok) 
                    continue;
                Pullenti.Semantic.SemObject first = new Pullenti.Semantic.SemObject(o.Graph) { Typ = Pullenti.Semantic.SemObjectType.Adjective };
                first.Tokens.Add(o.Tokens[0]);
                first.Morph.NormalFull = "ПЕРВЫЙ";
                first.Morph.NormalCase = (((o.Morph.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined ? "ПЕРВАЯ" : (((o.Morph.Gender & Pullenti.Morph.MorphGender.Neuter)) != Pullenti.Morph.MorphGender.Undefined ? "ПЕРВОЕ" : "ПЕРВЫЙ"));
                first.Morph.Gender = o.Morph.Gender;
                o.Graph.Objects.Add(first);
                o.Graph.AddLink(Pullenti.Semantic.SemLinkType.Detail, o, first, "какой", false, null);
                o.Quantity = null;
                ret = true;
            }
            for (int i = 0; i < objs.Count; i++) 
            {
                Pullenti.Semantic.SemObject o = objs[i];
                if (o.Typ != Pullenti.Semantic.SemObjectType.Noun) 
                    continue;
                if (o.Quantity != null && o.Quantity.Spelling == "1") 
                {
                }
                else 
                    continue;
                Pullenti.Semantic.SemObject other = o.FindFromObject("ДРУГОЙ", Pullenti.Semantic.SemLinkType.Undefined, Pullenti.Semantic.SemObjectType.Undefined);
                if (other == null) 
                    continue;
                bool ok = false;
                for (int j = i - 1; j >= 0; j--) 
                {
                    Pullenti.Semantic.SemObject oo = objs[j];
                    if (oo.Typ != Pullenti.Semantic.SemObjectType.Noun) 
                        continue;
                    if (oo.Morph.NormalFull != o.Morph.NormalFull) 
                        continue;
                    if (oo.FindFromObject("ПЕРВЫЙ", Pullenti.Semantic.SemLinkType.Undefined, Pullenti.Semantic.SemObjectType.Undefined) != null) 
                    {
                        ok = true;
                        break;
                    }
                }
                if (ok) 
                {
                    other.Morph.NormalFull = "ВТОРОЙ";
                    other.Morph.NormalCase = (((o.Morph.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined ? "ВТОРАЯ" : (((o.Morph.Gender & Pullenti.Morph.MorphGender.Neuter)) != Pullenti.Morph.MorphGender.Undefined ? "ВТОРОЕ" : "ВТОРОЙ"));
                }
            }
            return ret;
        }
    }
}