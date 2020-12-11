/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Instrument.Internal
{
    /// <summary>
    /// Поддержка анализа редакций для фрагментов НПА
    /// </summary>
    static class EditionHelper
    {
        public static void AnalizeEditions(FragToken root)
        {
            if (root.Number == 6 && root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Subitem) 
            {
            }
            if (root.SubNumber == 67) 
            {
            }
            if (root.Children.Count > 1 && root.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Number && root.Children[1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
            {
                if (root.Children[1].BeginToken.IsValue("УТРАТИТЬ", "ВТРАТИТИ") && root.Children[1].BeginToken.Next != null && root.Children[1].BeginToken.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
                    root.IsExpired = true;
            }
            if ((!root.IsExpired && root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention && root.BeginToken.IsValue("АБЗАЦ", null)) && root.BeginToken.Next != null && root.BeginToken.Next.IsValue("УТРАТИТЬ", "ВТРАТИТИ")) 
                root.IsExpired = true;
            if (root.IsExpired || ((root.Itok != null && root.Itok.IsExpired))) 
            {
                root.IsExpired = true;
                if (root.Referents == null) 
                    root.Referents = new List<Pullenti.Ner.Referent>();
                for (Pullenti.Ner.Token tt = root.BeginToken; tt != null && tt.EndChar <= root.EndChar; tt = tt.Next) 
                {
                    Pullenti.Ner.Decree.DecreeReferent dec = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                    if (dec != null) 
                    {
                        if (!root.Referents.Contains(dec)) 
                            root.Referents.Add(dec);
                    }
                }
                return;
            }
            int i0;
            for (i0 = 0; i0 < root.Children.Count; i0++) 
            {
                FragToken ch = root.Children[i0];
                if (((ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Keyword || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Number) || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Name || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                {
                }
                else 
                    break;
            }
            if (root.Number > 0) 
            {
                FragToken edt1 = _getLastChild(root);
                if (edt1 != null && edt1.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions && edt1.Tag == null) 
                {
                    if (_canBeEditionFor(root, edt1) > 0) 
                    {
                        if (root.Referents == null) 
                            root.Referents = edt1.Referents;
                        else 
                            foreach (Pullenti.Ner.Referent r in edt1.Referents) 
                            {
                                if (!root.Referents.Contains(r)) 
                                    root.Referents.Add(r);
                            }
                        edt1.Tag = edt1;
                    }
                }
            }
            if (i0 >= root.Children.Count) 
            {
                foreach (FragToken ch in root.Children) 
                {
                    AnalizeEditions(ch);
                }
                return;
            }
            FragToken ch0 = root.Children[i0];
            bool ok = false;
            if (_canBeEditionFor(root, ch0) >= 0) 
            {
                ok = true;
                if (i0 > 0 && ((root.Children[i0 - 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content || root.Children[i0 - 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention)) && ((i0 + 1) < root.Children.Count)) 
                {
                    if (_canBeEditionFor(root.Children[i0 - 1], ch0) >= 0) 
                        ok = false;
                }
            }
            if (((i0 + 1) < root.Children.Count) && _canBeEditionFor(root, root.Children[root.Children.Count - 1]) >= 0 && (_canBeEditionFor(root.Children[root.Children.Count - 1], root.Children[root.Children.Count - 1]) < 0)) 
            {
                ok = true;
                ch0 = root.Children[root.Children.Count - 1];
            }
            if (ok && ch0.Tag == null) 
            {
                if (root.Referents == null) 
                    root.Referents = ch0.Referents;
                else 
                    foreach (Pullenti.Ner.Referent r in ch0.Referents) 
                    {
                        if (!root.Referents.Contains(r)) 
                            root.Referents.Add(r);
                    }
                ch0.Tag = ch0;
            }
            for (int i = 0; i < root.Children.Count; i++) 
            {
                FragToken ch = root.Children[i];
                FragToken edt = null;
                FragToken edt2 = null;
                if (ch.Number > 0 && i > 0) 
                    edt = _getLastChild(root.Children[i - 1]);
                if (((i + 1) < root.Children.Count) && root.Children[i + 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                    edt2 = root.Children[i + 1];
                if (edt != null) 
                {
                    if (_canBeEditionFor(ch, edt) < 1) 
                        edt = null;
                }
                if (edt2 != null) 
                {
                    if (_canBeEditionFor(ch, edt2) < 0) 
                        edt2 = null;
                }
                if (edt != null && edt.Tag == null) 
                {
                    if (ch.Referents == null) 
                        ch.Referents = edt.Referents;
                    else 
                        foreach (Pullenti.Ner.Referent r in edt.Referents) 
                        {
                            if (!ch.Referents.Contains(r)) 
                                ch.Referents.Add(r);
                        }
                    edt.Tag = ch;
                }
                if (edt2 != null && edt2.Tag == null) 
                {
                    if (ch.Referents == null) 
                        ch.Referents = edt2.Referents;
                    else 
                        foreach (Pullenti.Ner.Referent r in edt2.Referents) 
                        {
                            if (!ch.Referents.Contains(r)) 
                                ch.Referents.Add(r);
                        }
                    edt2.Tag = ch;
                }
            }
            foreach (FragToken ch in root.Children) 
            {
                AnalizeEditions(ch);
            }
        }
        static FragToken _getLastChild(FragToken fr)
        {
            if (fr.Children.Count == 0) 
                return fr;
            return _getLastChild(fr.Children[fr.Children.Count - 1]);
        }
        static int _canBeEditionFor(FragToken fr, FragToken edt)
        {
            if (edt == null || edt.Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions || edt.Referents == null) 
                return -1;
            if (fr.SubNumber3 == 67) 
            {
            }
            Pullenti.Ner.Token t = edt.BeginToken;
            if (t.IsChar('(') && t.Next != null) 
                t = t.Next;
            if (t.IsValue("АБЗАЦ", null)) 
                return (fr.Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention ? 1 : -1);
            Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, false);
            if (pt == null) 
                pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, true);
            if (pt == null) 
                return 0;
            if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause) 
            {
                if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Clause) 
                    return -1;
            }
            else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part) 
            {
                if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.ClausePart && fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.DocPart && fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Item) 
                    return -1;
            }
            else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item) 
            {
                if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.ClausePart && fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Item && fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Subitem) 
                    return -1;
            }
            else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubItem) 
            {
                if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Subitem) 
                {
                    if (fr.Kind == Pullenti.Ner.Instrument.InstrumentKind.Item && t.IsValue("ПП", null)) 
                    {
                    }
                    else 
                        return -1;
                }
            }
            else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Chapter) 
            {
                if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Chapter) 
                    return -1;
            }
            else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Paragraph) 
            {
                if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Paragraph) 
                    return -1;
            }
            else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subparagraph) 
            {
                if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Subparagraph) 
                    return -1;
            }
            if (pt.Values.Count == 0) 
                return 0;
            if (fr.Number == 0) 
                return -1;
            if (fr.NumberString == pt.Values[0].Value) 
                return 1;
            if (pt.Values[0].Value.EndsWith("." + fr.NumberString)) 
                return 0;
            if (fr.Number == Pullenti.Ner.Decree.Internal.PartToken.GetNumber(pt.Values[0].Value)) 
            {
                if (fr.SubNumber == 0) 
                    return 1;
            }
            return -1;
        }
    }
}