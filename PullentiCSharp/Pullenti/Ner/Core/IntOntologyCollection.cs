/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    // Внутренний онтологический словарь. По сути, некоторая надстройка над TerminCollection.
    // Не помню уже, зачем был введён, но для чего-то нужен.
    public class IntOntologyCollection
    {
        public ICollection<IntOntologyItem> Items
        {
            get
            {
                return m_Items;
            }
        }
        public bool IsExtOntology;
        List<IntOntologyItem> m_Items = new List<IntOntologyItem>();
        TerminCollection m_Termins = new TerminCollection();
        public void AddItem(IntOntologyItem di)
        {
            m_Items.Add(di);
            di.Owner = this;
            for (int i = 0; i < di.Termins.Count; i++) 
            {
                if (di.Termins[i] is OntologyTermin) 
                {
                    (di.Termins[i] as OntologyTermin).Owner = di;
                    m_Termins.Add(di.Termins[i]);
                }
                else 
                {
                    OntologyTermin nt = new OntologyTermin() { Owner = di, Tag = di.Termins[i].Tag };
                    di.Termins[i].CopyTo(nt);
                    m_Termins.Add(nt);
                    di.Termins[i] = nt;
                }
            }
        }
        public bool AddReferent(Pullenti.Ner.Referent referent)
        {
            if (referent == null) 
                return false;
            IntOntologyItem oi = null;
            if (referent.IntOntologyItem != null && referent.IntOntologyItem.Owner == this) 
            {
                IntOntologyItem oi1 = referent.CreateOntologyItem();
                if (oi1 == null || oi1.Termins.Count == referent.IntOntologyItem.Termins.Count) 
                    return true;
                foreach (Termin t in referent.IntOntologyItem.Termins) 
                {
                    m_Termins.Remove(t);
                }
                int i = m_Items.IndexOf(referent.IntOntologyItem);
                if (i >= 0) 
                    m_Items.RemoveAt(i);
                oi = oi1;
            }
            else 
                oi = referent.CreateOntologyItem();
            if (oi == null) 
                return false;
            oi.Referent = referent;
            referent.IntOntologyItem = oi;
            this.AddItem(oi);
            return true;
        }
        public void AddTermin(IntOntologyItem di, Termin t)
        {
            OntologyTermin nt = new OntologyTermin() { Owner = di, Tag = t.Tag };
            t.CopyTo(nt);
            m_Termins.Add(nt);
        }
        class OntologyTermin : Pullenti.Ner.Core.Termin
        {
            public Pullenti.Ner.Core.IntOntologyItem Owner;
        }

        public void Add(Termin t)
        {
            m_Termins.Add(t);
        }
        public List<Termin> FindTerminByCanonicText(string text)
        {
            return m_Termins.FindTerminsByCanonicText(text);
        }
        public List<IntOntologyToken> TryAttach(Pullenti.Ner.Token t, string referentTypeName = null, bool canBeGeoObject = false)
        {
            List<TerminToken> tts = m_Termins.TryParseAll(t, (canBeGeoObject ? TerminParseAttr.CanBeGeoObject : TerminParseAttr.No));
            if (tts == null) 
                return null;
            List<IntOntologyToken> res = new List<IntOntologyToken>();
            List<IntOntologyItem> dis = new List<IntOntologyItem>();
            foreach (TerminToken tt in tts) 
            {
                IntOntologyItem di = null;
                if (tt.Termin is OntologyTermin) 
                    di = (tt.Termin as OntologyTermin).Owner;
                if (di != null) 
                {
                    if (di.Referent != null && referentTypeName != null) 
                    {
                        if (di.Referent.TypeName != referentTypeName) 
                            continue;
                    }
                    if (dis.Contains(di)) 
                        continue;
                    dis.Add(di);
                }
                res.Add(new IntOntologyToken(tt.BeginToken, tt.EndToken) { Item = di, Termin = tt.Termin, Morph = tt.Morph });
            }
            return (res.Count == 0 ? null : res);
        }
        public List<IntOntologyItem> TryAttachByItem(IntOntologyItem item)
        {
            if (item == null) 
                return null;
            List<IntOntologyItem> res = null;
            foreach (Termin t in item.Termins) 
            {
                List<Termin> li = m_Termins.FindTerminsByTermin(t);
                if (li != null) 
                {
                    foreach (Termin tt in li) 
                    {
                        if (tt is OntologyTermin) 
                        {
                            IntOntologyItem oi = (tt as OntologyTermin).Owner;
                            if (res == null) 
                                res = new List<IntOntologyItem>();
                            if (!res.Contains(oi)) 
                                res.Add(oi);
                        }
                    }
                }
            }
            return res;
        }
        public List<Pullenti.Ner.Referent> TryAttachByReferent(Pullenti.Ner.Referent referent, IntOntologyItem item = null, bool mustBeSingle = false)
        {
            if (referent == null) 
                return null;
            if (item == null) 
                item = referent.CreateOntologyItem();
            if (item == null) 
                return null;
            List<IntOntologyItem> li = this.TryAttachByItem(item);
            if (li == null) 
                return null;
            List<Pullenti.Ner.Referent> res = null;
            foreach (IntOntologyItem oi in li) 
            {
                Pullenti.Ner.Referent r = oi.Referent ?? (oi.Tag as Pullenti.Ner.Referent);
                if (r != null) 
                {
                    if (referent.CanBeEquals(r, ReferentsEqualType.WithinOneText)) 
                    {
                        if (res == null) 
                            res = new List<Pullenti.Ner.Referent>();
                        if (!res.Contains(r)) 
                            res.Add(r);
                    }
                }
            }
            if (mustBeSingle) 
            {
                if (res != null && res.Count > 1) 
                {
                    for (int i = 0; i < (res.Count - 1); i++) 
                    {
                        for (int j = i + 1; j < res.Count; j++) 
                        {
                            if (!res[i].CanBeEquals(res[j], ReferentsEqualType.ForMerging)) 
                                return null;
                        }
                    }
                }
            }
            return res;
        }
        public void Remove(Pullenti.Ner.Referent r)
        {
            int i;
            for (i = 0; i < m_Items.Count; i++) 
            {
                if (m_Items[i].Referent == r) 
                {
                    IntOntologyItem oi = m_Items[i];
                    oi.Referent = null;
                    r.IntOntologyItem = null;
                    m_Items.RemoveAt(i);
                    foreach (Termin t in oi.Termins) 
                    {
                        m_Termins.Remove(t);
                    }
                    break;
                }
            }
        }
    }
}