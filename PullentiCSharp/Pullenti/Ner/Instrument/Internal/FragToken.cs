/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Instrument.Internal
{
    public class FragToken : Pullenti.Ner.MetaToken
    {
        public FragToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
            for (Pullenti.Ner.Token t = EndToken.Next; t != null; t = t.Next) 
            {
                if (t.IsChar((char)7) || t.IsChar((char)0x1F)) 
                    EndToken = t;
                else 
                    break;
            }
        }
        public Pullenti.Ner.Instrument.InstrumentKind Kind;
        public Pullenti.Ner.Instrument.InstrumentKind Kind2;
        public object Value;
        public string Name;
        public int Number;
        public int MinNumber;
        public int SubNumber;
        public int SubNumber2;
        public int SubNumber3;
        public List<Pullenti.Ner.Referent> Referents = null;
        public bool IsExpired;
        public string NumberString
        {
            get
            {
                if (SubNumber == 0) 
                    return Number.ToString();
                StringBuilder tmp = new StringBuilder();
                tmp.AppendFormat("{0}.{1}", Number, SubNumber);
                if (SubNumber2 > 0) 
                    tmp.AppendFormat(".{0}", SubNumber2);
                if (SubNumber3 > 0) 
                    tmp.AppendFormat(".{0}", SubNumber3);
                return tmp.ToString();
            }
        }
        public List<FragToken> Children = new List<FragToken>();
        public void SortChildren()
        {
            for (int k = 0; k < Children.Count; k++) 
            {
                bool ch = false;
                for (int i = 0; i < (Children.Count - 1); i++) 
                {
                    if (Children[i].CompareTo(Children[i + 1]) > 0) 
                    {
                        ch = true;
                        FragToken v = Children[i];
                        Children[i] = Children[i + 1];
                        Children[i + 1] = v;
                    }
                }
                if (!ch) 
                    break;
            }
        }
        public FragToken FindChild(Pullenti.Ner.Instrument.InstrumentKind kind)
        {
            foreach (FragToken ch in Children) 
            {
                if (ch.Kind == kind) 
                    return ch;
            }
            return null;
        }
        public int CompareTo(FragToken other)
        {
            if (BeginChar < other.BeginChar) 
                return -1;
            if (BeginChar > other.BeginChar) 
                return 1;
            if (EndChar < other.EndChar) 
                return -1;
            if (EndChar > other.EndChar) 
                return 1;
            return 0;
        }
        public int MinChildNumber
        {
            get
            {
                foreach (FragToken ch in Children) 
                {
                    if (ch.Number > 0) 
                    {
                        if (ch.Number != 1) 
                        {
                            if (ch.Itok != null && ch.Itok.NumTyp == NumberTypes.Letter) 
                                return 0;
                        }
                        return ch.Number;
                    }
                }
                return 0;
            }
        }
        public int MaxChildNumber
        {
            get
            {
                int max = 0;
                foreach (FragToken ch in Children) 
                {
                    if (ch.Number > max) 
                        max = ch.Number;
                }
                return max;
            }
        }
        internal bool DefVal
        {
            get
            {
                return false;
            }
            set
            {
                string str = this.GetSourceText();
                while (str.Length > 0) 
                {
                    char last = str[str.Length - 1];
                    char first = str[0];
                    if ((last == 0x1E || last == 0x1F || last == 7) || char.IsWhiteSpace(last)) 
                        str = str.Substring(0, str.Length - 1);
                    else if ((first == 0x1E || first == 0x1F || first == 7) || char.IsWhiteSpace(first)) 
                        str = str.Substring(1);
                    else 
                        break;
                }
                Value = str;
            }
        }
        internal bool DefVal2
        {
            get
            {
                return false;
            }
            set
            {
                Value = GetRestoredNameMT(this, false);
            }
        }
        internal static string GetRestoredNameMT(Pullenti.Ner.MetaToken mt, bool indexItem = false)
        {
            return GetRestoredName(mt.BeginToken, mt.EndToken, indexItem);
        }
        internal static string GetRestoredName(Pullenti.Ner.Token b, Pullenti.Ner.Token e, bool indexItem = false)
        {
            Pullenti.Ner.Token e0 = e;
            for (; e != null && e.BeginChar > b.EndChar; e = e.Previous) 
            {
                if (e.IsCharOf("*<") || e.IsTableControlChar) 
                {
                }
                else if ((e.IsCharOf(">") && (e.Previous is Pullenti.Ner.NumberToken) && e.Previous.Previous != null) && e.Previous.Previous.IsChar('<')) 
                    e = e.Previous;
                else if (e.IsCharOf(">") && e.Previous.IsChar('*')) 
                {
                }
                else if ((e is Pullenti.Ner.NumberToken) && ((e == e0 || e.Next.IsTableControlChar)) && indexItem) 
                {
                }
                else if (((e.IsChar('.') || e.IsHiphen)) && indexItem) 
                {
                }
                else 
                    break;
            }
            Pullenti.Ner.Token b0 = b;
            for (; b != null && b.EndChar <= e.EndChar; b = b.Next) 
            {
                if (b.IsTableControlChar) 
                {
                }
                else 
                {
                    b0 = b;
                    break;
                }
            }
            string str = Pullenti.Ner.Core.MiscHelper.GetTextValue(b0, e, Pullenti.Ner.Core.GetTextAttr.RestoreRegister | Pullenti.Ner.Core.GetTextAttr.KeepRegister | Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
            if (!string.IsNullOrEmpty(str)) 
            {
                if (char.IsLower(str[0])) 
                    str = string.Format("{0}{1}", char.ToUpper(str[0]), str.Substring(1));
            }
            return str;
        }
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            if (Kind != Pullenti.Ner.Instrument.InstrumentKind.Undefined) 
            {
                tmp.AppendFormat("{0}:", Kind);
                if (Kind2 != Pullenti.Ner.Instrument.InstrumentKind.Undefined) 
                    tmp.AppendFormat(" ({0}):", Kind2);
            }
            else if (Itok != null) 
                tmp.AppendFormat("{0} ", Itok);
            if (Number > 0) 
            {
                if (MinNumber > 0) 
                    tmp.AppendFormat(" Num=[{0}..{1}]", MinNumber, Number);
                else 
                    tmp.AppendFormat(" Num={0}", Number);
                if (SubNumber > 0) 
                    tmp.AppendFormat(".{0}", SubNumber);
                if (SubNumber2 > 0) 
                    tmp.AppendFormat(".{0}", SubNumber2);
                if (SubNumber3 > 0) 
                    tmp.AppendFormat(".{0}", SubNumber3);
            }
            if (IsExpired) 
                tmp.Append(" Expired");
            if (Children.Count > 0) 
                tmp.AppendFormat(" ChCount={0}", Children.Count);
            if (Name != null) 
                tmp.AppendFormat(" Nam='{0}'", Name);
            if (Value != null) 
                tmp.AppendFormat(" Val='{0}'", Value.ToString());
            if (tmp.Length == 0) 
                tmp.Append(this.GetSourceText());
            return tmp.ToString();
        }
        public Pullenti.Ner.Instrument.InstrumentBlockReferent CreateReferent(Pullenti.Ner.Core.AnalyzerData ad)
        {
            return this._CreateReferent(ad, this);
        }
        public Pullenti.Ner.Instrument.InstrumentBlockReferent _CreateReferent(Pullenti.Ner.Core.AnalyzerData ad, FragToken bas)
        {
            Pullenti.Ner.Instrument.InstrumentBlockReferent res = null;
            if (m_Doc != null) 
                res = m_Doc;
            else 
            {
                res = new Pullenti.Ner.Instrument.InstrumentBlockReferent();
                res.Kind = Kind;
                res.Kind2 = Kind2;
                if (Number > 0) 
                    res.Number = Number;
                if (MinNumber > 0) 
                    res.MinNumber = MinNumber;
                if (SubNumber > 0) 
                    res.SubNumber = SubNumber;
                if (SubNumber2 > 0) 
                    res.SubNumber2 = SubNumber2;
                if (SubNumber3 > 0) 
                    res.SubNumber3 = SubNumber3;
                if (IsExpired) 
                    res.IsExpired = true;
                if (Name != null && Kind != Pullenti.Ner.Instrument.InstrumentKind.Head) 
                {
                    Pullenti.Ner.Slot s = res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, Name.ToUpper(), false, 0);
                    s.Tag = Name;
                }
                if (Value != null && Kind != Pullenti.Ner.Instrument.InstrumentKind.Contact) 
                {
                    if (Value is string) 
                        res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_VALUE, Value, false, 0);
                    else if (Value is Pullenti.Ner.Referent) 
                        res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_REF, Value, false, 0);
                    else if (Value is Pullenti.Ner.ReferentToken) 
                    {
                        Pullenti.Ner.Referent r = (Value as Pullenti.Ner.ReferentToken).Referent;
                        (Value as Pullenti.Ner.ReferentToken).SaveToLocalOntology();
                        res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_REF, (Value as Pullenti.Ner.ReferentToken).Referent, false, 0);
                        res.AddExtReferent(Value as Pullenti.Ner.ReferentToken);
                        Pullenti.Ner.Slot s = bas.m_Doc.FindSlot(null, r, true);
                        if (s != null) 
                            s.Value = (Value as Pullenti.Ner.ReferentToken).Referent;
                    }
                    else if (Value is Pullenti.Ner.Decree.Internal.DecreeToken) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeToken dt = Value as Pullenti.Ner.Decree.Internal.DecreeToken;
                        if (dt.Ref is Pullenti.Ner.ReferentToken) 
                        {
                            Pullenti.Ner.Referent r = (dt.Ref as Pullenti.Ner.ReferentToken).Referent;
                            (dt.Ref as Pullenti.Ner.ReferentToken).SaveToLocalOntology();
                            res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_REF, (dt.Ref as Pullenti.Ner.ReferentToken).Referent, false, 0);
                            res.AddExtReferent(dt.Ref as Pullenti.Ner.ReferentToken);
                            Pullenti.Ner.Slot s = bas.m_Doc.FindSlot(null, r, true);
                            if (s != null) 
                                s.Value = (dt.Ref as Pullenti.Ner.ReferentToken).Referent;
                        }
                        else if (dt.Value != null) 
                            res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_VALUE, dt.Value, false, 0);
                    }
                }
                if (Referents != null) 
                {
                    foreach (Pullenti.Ner.Referent r in Referents) 
                    {
                        res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_REF, r, false, 0);
                    }
                }
                if (Children.Count == 0) 
                {
                    for (Pullenti.Ner.Token t = BeginToken; t != null && (t.BeginChar < EndChar); t = t.Next) 
                    {
                        if (t.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent) 
                            res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_REF, t.GetReferent(), false, 0);
                        if (t.EndChar > EndChar) 
                            break;
                    }
                }
            }
            if (ad != null) 
            {
                if (ad.Referents.Count > 200000) 
                    return null;
                ad.Referents.Add(res);
                res.AddOccurenceOfRefTok(new Pullenti.Ner.ReferentToken(res, BeginToken, EndToken));
            }
            foreach (FragToken ch in Children) 
            {
                Pullenti.Ner.Instrument.InstrumentBlockReferent ich = ch._CreateReferent(ad, bas);
                if (ich != null) 
                    res.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_CHILD, ich, false, 0);
            }
            return res;
        }
        public void FillByContentChildren()
        {
            this.SortChildren();
            if (Children.Count == 0) 
            {
                Children.Add(new FragToken(BeginToken, EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
                return;
            }
            if (BeginChar < Children[0].BeginChar) 
                Children.Insert(0, new FragToken(BeginToken, Children[0].BeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
            for (int i = 0; i < (Children.Count - 1); i++) 
            {
                if (Children[i].EndToken.Next != Children[i + 1].BeginToken && (Children[i].EndToken.Next.EndChar < Children[i + 1].BeginChar)) 
                    Children.Insert(i + 1, new FragToken(Children[i].EndToken.Next, Children[i + 1].BeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
            }
            if (Children[Children.Count - 1].EndChar < EndChar) 
                Children.Add(new FragToken(Children[Children.Count - 1].EndToken.Next, EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
        }
        internal Pullenti.Ner.Instrument.InstrumentReferent m_Doc;
        internal InstrToken1 Itok;
        public static FragToken CreateDocument(Pullenti.Ner.Token t, int maxChar, Pullenti.Ner.Instrument.InstrumentKind rootKind = Pullenti.Ner.Instrument.InstrumentKind.Undefined)
        {
            if (t == null) 
                return null;
            while ((t is Pullenti.Ner.TextToken) && t.Next != null) 
            {
                if (t.IsTableControlChar || !t.Chars.IsLetter) 
                    t = t.Next;
                else 
                    break;
            }
            if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
            {
                Pullenti.Ner.Decree.DecreeReferent dec0 = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                if (dec0.Kind == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                    t = t.Next;
                else 
                    t = t.Kit.DebedToken(t);
            }
            else if (t.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) 
            {
                Pullenti.Ner.Decree.DecreePartReferent dp = t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                if ((dp.Clause != null || dp.Item != null || dp.SubItem != null) || dp.Indention != null) 
                {
                }
                else 
                    t = t.Kit.DebedToken(t);
            }
            if (t == null) 
                return null;
            FragToken res = __createActionQuestion(t, maxChar);
            if (res != null) 
                return res;
            res = new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Document };
            res.m_Doc = new Pullenti.Ner.Instrument.InstrumentReferent();
            bool isApp = false;
            int cou = 0;
            for (Pullenti.Ner.Token ttt = t; ttt != null && (cou < 5); ttt = ttt.Next,cou++) 
            {
                if (ttt.IsNewlineBefore || ttt.Previous.IsTableControlChar) 
                {
                    if (ttt.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                    {
                        isApp = true;
                        break;
                    }
                    if (ttt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                        break;
                    Pullenti.Ner.Decree.Internal.DecreeToken dtt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttt, null, false);
                    if (dtt != null && ((dtt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ || dtt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dtt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr))) 
                        break;
                    if (ttt is Pullenti.Ner.NumberToken) 
                        break;
                }
            }
            FragToken head = (isApp || maxChar > 0 ? null : CreateDocTitle(t, res.m_Doc));
            Pullenti.Ner.Decree.DecreeKind headKind = Pullenti.Ner.Decree.DecreeKind.Undefined;
            if (head != null && (head.Tag is Pullenti.Ner.Decree.DecreeKind)) 
                headKind = (Pullenti.Ner.Decree.DecreeKind)head.Tag;
            FragToken head1 = null;
            Pullenti.Ner.Instrument.InstrumentReferent appDoc = new Pullenti.Ner.Instrument.InstrumentReferent();
            if (maxChar > 0 && !isApp) 
            {
            }
            else 
                head1 = CreateAppendixTitle(t, res, appDoc, true, true);
            if (head1 != null) 
            {
                if (head1.Tag is FragToken) 
                {
                    res.m_Doc = appDoc;
                    res.Children.Add(head1);
                    res.Children.Add(head1.Tag as FragToken);
                    res.EndToken = (head1.Tag as FragToken).EndToken;
                    return res;
                }
                bool ee = false;
                if (head == null) 
                    ee = true;
                else if (head1.EndChar > head.EndChar && ((res.m_Doc.Typ == "ПРИЛОЖЕНИЕ" || res.m_Doc.Typ == "ДОДАТОК"))) 
                    ee = true;
                else if (head1.Children.Count > head.Children.Count) 
                    ee = true;
                if (ee) 
                {
                    head = head1;
                    res.m_Doc = appDoc;
                }
            }
            if (head != null) 
            {
                if (maxChar == 0) 
                    _createJusticeParticipants(head, res.m_Doc);
                head.SortChildren();
                res.Children.Add(head);
                res.EndToken = head.EndToken;
                if (head.BeginChar < res.BeginChar) 
                    res.BeginToken = head.BeginToken;
                t = res.EndToken.Next;
            }
            if (t == null) 
            {
                if (head != null && head.Children.Count > 2) 
                    return res;
                return null;
            }
            bool isContract = false;
            if (res.m_Doc.Typ != null) 
            {
                if (res.m_Doc.Typ.Contains("ДОГОВОР") || res.m_Doc.Typ.Contains("ДОГОВІР") || res.m_Doc.Typ.Contains("КОНТРАКТ")) 
                    isContract = true;
            }
            Pullenti.Ner.Token t0 = t;
            List<InstrToken> li = InstrToken.ParseList(t, maxChar);
            if (li == null || li.Count == 0) 
                return null;
            int i;
            if (isApp) 
            {
                for (i = 0; i < li.Count; i++) 
                {
                    if (li[i].Typ == ILTypes.Approved) 
                        li[i].Typ = ILTypes.Undefined;
                    else if (li[i].Typ == ILTypes.Appendix && li[i].Value != "ПРИЛОЖЕНИЕ" && li[i].Value != "ДОДАТОК") 
                        li[i].Typ = ILTypes.Undefined;
                }
            }
            for (i = 0; i < (li.Count - 1); i++) 
            {
                if (li[i].Typ == ILTypes.Appendix) 
                {
                    if (i > 0 && li[i - 1].Typ == ILTypes.Person) 
                        break;
                    InstrToken1 num1 = InstrToken1.Parse(li[i].BeginToken, true, null, 0, null, false, 0, false, false);
                    int maxNum = i + 7;
                    int i0 = i;
                    for (int j = i + 1; (j < li.Count) && (j < maxNum); j++) 
                    {
                        if (li[j].Typ == ILTypes.Appendix) 
                        {
                            if (li[j].Value != li[i].Value) 
                            {
                                if (li[i].Value == "ПРИЛОЖЕНИЕ" || li[i].Value == "ДОДАТОК") 
                                    li[j].Typ = ILTypes.Undefined;
                                else if (li[j].Value == "ПРИЛОЖЕНИЕ" || li[j].Value == "ДОДАТОК") 
                                {
                                    li[i].Typ = ILTypes.Undefined;
                                    break;
                                }
                            }
                            else 
                            {
                                int le = li[j].BeginChar - li[i0].BeginChar;
                                if (le > 400) 
                                    break;
                                i = j;
                                InstrToken1 num2 = InstrToken1.Parse(li[j].BeginToken, true, null, 0, null, false, 0, false, false);
                                int d = NumberingHelper.CalcDelta(num1, num2, true);
                                if (d == 1) 
                                {
                                    li[i0].Typ = ILTypes.Undefined;
                                    li[j].Typ = ILTypes.Undefined;
                                    i0 = j;
                                }
                                num1 = num2;
                                maxNum = j + 7;
                            }
                        }
                        else if (li[j].Typ == ILTypes.Approved) 
                            li[j].Typ = ILTypes.Undefined;
                    }
                }
            }
            bool hasApp = false;
            for (i = 0; i < li.Count; i++) 
            {
                if (li[i].Typ == ILTypes.Appendix || li[i].Typ == ILTypes.Approved) 
                {
                    if (li[i].Typ == ILTypes.Approved) 
                    {
                        hasApp = true;
                        if (i == 0 || !li[i].IsNewlineAfter) 
                            continue;
                    }
                    if (li[i].Ref is Pullenti.Ner.Decree.DecreeReferent) 
                    {
                        Pullenti.Ner.Decree.DecreeReferent drApp = li[i].Ref as Pullenti.Ner.Decree.DecreeReferent;
                        if (drApp.Typ != res.m_Doc.Typ) 
                            continue;
                        if (drApp.Number != null && res.m_Doc.RegNumber != null) 
                        {
                            if (drApp.Number != res.m_Doc.RegNumber) 
                                continue;
                        }
                    }
                    break;
                }
            }
            int i1 = i;
            if (maxChar == 0 && i1 == li.Count) 
            {
                for (i = 0; i < li.Count; i++) 
                {
                    if (li[i].Typ == ILTypes.Person && li[i].IsNewlineBefore && !li[i].HasTableChars) 
                    {
                        int j;
                        bool dat = false;
                        bool num = false;
                        bool geo = false;
                        int pers = 0;
                        for (j = i + 1; j < li.Count; j++) 
                        {
                            if (li[j].Typ == ILTypes.Geo) 
                                geo = true;
                            else if (li[j].Typ == ILTypes.RegNumber) 
                                num = true;
                            else if (li[j].Typ == ILTypes.Date) 
                                dat = true;
                            else if (li[j].Typ == ILTypes.Person && li[j].IsPurePerson) 
                            {
                                if (((li[j].IsNewlineBefore || ((li[j - 1].Typ == ILTypes.Person || li[j - 1].Typ == ILTypes.Date)))) && ((li[j].IsNewlineAfter || ((((j + 1) < li.Count) && ((li[j + 1].Typ == ILTypes.Person || li[j + 1].Typ == ILTypes.Date))))))) 
                                    pers++;
                                else 
                                    break;
                            }
                            else 
                                break;
                        }
                        int k = pers;
                        if (dat) 
                            k++;
                        if (num) 
                            k++;
                        if (geo) 
                            k++;
                        if ((j < li.Count) && ((li[j].Typ == ILTypes.Appendix || li[j].Typ == ILTypes.Approved))) 
                            k += 2;
                        else if ((li[i].IsPurePerson && li[i].BeginToken.Previous != null && li[i].BeginToken.Previous.IsChar('.')) && li[i].IsNewlineAfter) 
                        {
                            InstrToken1 itt = InstrToken1.Parse(li[i].EndToken.Next, true, null, 0, null, false, 0, false, false);
                            if (itt != null && itt.Numbers.Count > 0 && li[i + 1].Typ == ILTypes.Undefined) 
                            {
                            }
                            else 
                                k += 2;
                        }
                        if (k >= 2) 
                        {
                            i = j;
                            if ((i < li.Count) && ((li[i].Typ == ILTypes.Undefined || li[i].Typ == ILTypes.Typ))) 
                                li[i].Typ = ILTypes.Approved;
                            if ((i > (i1 + 10) && (i1 < li.Count) && li[i1].Typ == ILTypes.Appendix) && li[i1].WhitespacesBeforeCount > 15) 
                            {
                            }
                            else 
                                i1 = i;
                            break;
                        }
                    }
                }
            }
            if ((maxChar == 0 && (i1 < li.Count) && (i1 + 10) > li.Count) && !hasApp && ((li[li.Count - 1].EndChar - li[i1].EndChar) < 200)) 
            {
                for (int ii = li.Count - 1; ii > i; ii--) 
                {
                    if (li[ii].Typ == ILTypes.Person || li[ii].Typ == ILTypes.Date || ((li[ii].Typ == ILTypes.RegNumber && li[ii].IsNewlineBefore))) 
                    {
                        i1 = ii + 1;
                        break;
                    }
                }
            }
            int cMax = i1 - 1;
            FragToken tail = null;
            List<Pullenti.Ner.Referent> persList = new List<Pullenti.Ner.Referent>();
            for (i = i1 - 1; i > 0; i--) 
            {
                if (maxChar > 0) 
                    break;
                InstrToken lii = li[i];
                if (lii.HasTableChars) 
                {
                    if ((i < (i1 - 1)) && lii.Typ != ILTypes.Person) 
                        break;
                    if (isContract) 
                        break;
                }
                if ((lii.Typ == ILTypes.Person || lii.Typ == ILTypes.RegNumber || lii.Typ == ILTypes.Date) || lii.Typ == ILTypes.Geo) 
                {
                    if (persList.Count > 0) 
                    {
                        if (lii.Typ != ILTypes.Person && lii.Typ != ILTypes.Date) 
                            break;
                        if (!lii.IsNewlineBefore && !lii.IsNewlineAfter && !lii.HasTableChars) 
                        {
                            if (!lii.IsNewlineBefore && i > 0 && li[i - 1].Typ == ILTypes.Person) 
                            {
                            }
                            else 
                                break;
                        }
                    }
                    if (lii.Typ == ILTypes.Person && (lii.Ref is Pullenti.Ner.ReferentToken)) 
                    {
                        if (persList.Contains((lii.Ref as Pullenti.Ner.ReferentToken).Referent)) 
                        {
                            if (!lii.IsNewlineBefore) 
                                break;
                        }
                    }
                    if (!lii.IsNewlineBefore && !lii.BeginToken.IsTableControlChar && ((lii.Typ == ILTypes.Geo || li[i].Typ == ILTypes.Person))) 
                    {
                        if (i > 0 && ((li[i - 1].Typ == ILTypes.Undefined && !li[i - 1].EndToken.IsTableControlChar))) 
                            break;
                        if (lii.EndToken.IsCharOf(";.")) 
                            break;
                        if (!lii.IsNewlineAfter) 
                        {
                            if (lii.EndToken.Next != null && !lii.EndToken.Next.IsTableControlChar) 
                                break;
                        }
                    }
                    if (tail == null) 
                    {
                        tail = new FragToken(li[i].BeginToken, li[i1 - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Tail };
                        if ((i1 - 1) > i) 
                        {
                        }
                    }
                    tail.BeginToken = lii.BeginToken;
                    cMax = i - 1;
                    FragToken fr = new FragToken(li[i].BeginToken, li[i].EndToken);
                    tail.Children.Insert(0, fr);
                    if (li[i].Typ == ILTypes.Person) 
                    {
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Signer;
                        if (li[i].Ref is Pullenti.Ner.ReferentToken) 
                        {
                            res.m_Doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SIGNER, (li[i].Ref as Pullenti.Ner.ReferentToken).Referent, false, 0);
                            res.m_Doc.AddExtReferent(li[i].Ref as Pullenti.Ner.ReferentToken);
                            fr.Value = li[i].Ref;
                            persList.Add((li[i].Ref as Pullenti.Ner.ReferentToken).Referent);
                        }
                    }
                    else if (li[i].Typ == ILTypes.RegNumber) 
                    {
                        if (li[i].IsNewlineBefore) 
                        {
                            if (res.m_Doc.RegNumber == null || res.m_Doc.RegNumber == li[i].Value) 
                            {
                                fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Number;
                                fr.Value = li[i].Value;
                                res.m_Doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NUMBER, li[i].Value, false, 0);
                            }
                        }
                    }
                    else if (li[i].Typ == ILTypes.Date) 
                    {
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Date;
                        fr.Value = li[i].Value;
                        if (li[i].Ref != null) 
                        {
                            res.m_Doc.AddDate(li[i].Ref);
                            fr.Value = li[i].Ref;
                        }
                        else if (li[i].Value != null) 
                            res.m_Doc.AddDate(li[i].Value);
                    }
                    else if (li[i].Typ == ILTypes.Geo) 
                    {
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Place;
                        fr.Value = li[i].Ref;
                    }
                    if (fr.Value == null) 
                        fr.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(fr, Pullenti.Ner.Core.GetTextAttr.No);
                }
                else 
                {
                    string ss = Pullenti.Ner.Core.MiscHelper.GetTextValue(li[i].BeginToken, li[i].EndToken, Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
                    if (ss == null || ss.Length == 0) 
                        continue;
                    if (ss[ss.Length - 1] == ':') 
                        ss = ss.Substring(0, ss.Length - 1);
                    if (li[i].IsPodpisStoron && tail != null) 
                    {
                        tail.BeginToken = li[i].BeginToken;
                        tail.Children.Insert(0, new FragToken(li[i].BeginToken, li[i].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = ss });
                        cMax = i - 1;
                        break;
                    }
                    int jj;
                    for (jj = 0; jj < ss.Length; jj++) 
                    {
                        if (char.IsLetterOrDigit(ss[jj])) 
                            break;
                    }
                    if (jj >= ss.Length) 
                        continue;
                    if ((ss.Length < 100) && (((i1 - i) < 3))) 
                        continue;
                    break;
                }
            }
            if (cMax < 0) 
            {
                if (i1 > 0) 
                    return null;
            }
            else 
            {
                FragToken content = new FragToken(li[0].BeginToken, li[cMax].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content };
                res.Children.Add(content);
                content._analizeContent(res, maxChar > 0, rootKind);
                if (maxChar > 0 && cMax == (li.Count - 1) && head == null) 
                    res = content;
            }
            if (tail != null) 
            {
                res.Children.Add(tail);
                for (; i1 < li.Count; i1++) 
                {
                    if (li[i1].BeginToken == li[i1].EndToken && (li[i1].BeginToken.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && (li[i1].BeginToken.GetReferent() as Pullenti.Ner.Decree.DecreeReferent).Kind == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                    {
                        FragToken ap = new FragToken(li[i1].BeginToken, li[i1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
                        ap.Referents = new List<Pullenti.Ner.Referent>();
                        ap.Referents.Add(li[i1].BeginToken.GetReferent() as Pullenti.Ner.Decree.DecreeReferent);
                        tail.Children.Add(ap);
                        tail.EndToken = li[i1].EndToken;
                    }
                    else 
                        break;
                }
                if (tail.Children.Count > 0 && (tail.Children[tail.Children.Count - 1].EndChar < tail.EndChar)) 
                {
                    FragToken unkw = new FragToken(tail.Children[tail.Children.Count - 1].EndToken.Next, tail.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined };
                    tail.EndToken = unkw.BeginToken.Previous;
                    res.Children.Add(unkw);
                }
            }
            bool isAllApps = isApp;
            FragToken app0 = null;
            for (i = i1; i < li.Count; i++) 
            {
                int j;
                FragToken app = new FragToken(li[i].BeginToken, li[i].EndToken);
                FragToken title = CreateAppendixTitle(app.BeginToken, app, res.m_Doc, isAllApps, false);
                for (j = i + 1; j < li.Count; j++) 
                {
                    if (title != null && li[j].EndChar <= title.EndChar) 
                        continue;
                    if (li[j].Typ == ILTypes.Appendix) 
                    {
                        if (li[j].Value == li[i1].Value) 
                            break;
                        if (li[j].Value != null && li[i1].Value == null) 
                            break;
                        continue;
                    }
                    else if (li[j].Typ == ILTypes.Approved) 
                    {
                        if ((li[j].BeginChar - li[i].EndChar) > 200) 
                            break;
                    }
                }
                app.EndToken = li[j - 1].EndToken;
                tail = null;
                if (li[j - 1].Typ == ILTypes.Person && li[j - 1].IsNewlineBefore && li[j - 1].IsNewlineAfter) 
                {
                    tail = new FragToken(li[j - 1].BeginToken, li[j - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Tail };
                    for (int jj = j - 1; jj > i; jj--) 
                    {
                        if (li[jj].Typ != ILTypes.Person || !li[jj].IsNewlineBefore || !li[jj].IsNewlineAfter) 
                            break;
                        else 
                        {
                            FragToken fr = new FragToken(li[jj].BeginToken, li[jj].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Signer };
                            if (li[jj].Ref is Pullenti.Ner.ReferentToken) 
                                fr.Value = li[jj].Ref;
                            tail.Children.Insert(0, fr);
                            tail.BeginToken = fr.BeginToken;
                            app.EndToken = tail.BeginToken.Previous;
                        }
                    }
                }
                if (li[i].Typ == ILTypes.Appendix || ((((i + 1) < li.Count) && li[i + 1].Typ == ILTypes.Appendix))) 
                    app.Kind = Pullenti.Ner.Instrument.InstrumentKind.Appendix;
                else if (app.Kind != Pullenti.Ner.Instrument.InstrumentKind.Appendix) 
                    app.Kind = Pullenti.Ner.Instrument.InstrumentKind.InternalDocument;
                if (title == null) 
                {
                    bool ok = true;
                    if (app.LengthChar < 500) 
                        ok = false;
                    else 
                    {
                        app._analizeContent(app, false, Pullenti.Ner.Instrument.InstrumentKind.Undefined);
                        if (app.Children.Count < 2) 
                            ok = false;
                    }
                    if (ok) 
                        res.Children.Add(app);
                    else 
                    {
                        app.Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined;
                        res.Children[res.Children.Count - 1].Children.Add(app);
                        res.Children[res.Children.Count - 1].EndToken = app.EndToken;
                    }
                }
                else 
                {
                    if (isApp && app.Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix) 
                    {
                        if (res.Children.Count > 0) 
                            res.EndToken = res.Children[res.Children.Count - 1].EndToken;
                        FragToken res0 = new FragToken(res.BeginToken, res.EndToken) { m_Doc = res.m_Doc, Kind = Pullenti.Ner.Instrument.InstrumentKind.Document };
                        res.m_Doc = null;
                        res.Kind = Pullenti.Ner.Instrument.InstrumentKind.Appendix;
                        res0.Children.Insert(0, res);
                        res = res0;
                        isApp = false;
                    }
                    if ((app0 != null && !isApp && app0.Kind == Pullenti.Ner.Instrument.InstrumentKind.InternalDocument) && app.Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix) 
                        app0.Children.Add(app);
                    else 
                        res.Children.Add(app);
                    if (i == i1 && !isApp && app.Kind == Pullenti.Ner.Instrument.InstrumentKind.InternalDocument) 
                        app0 = app;
                    if (title.Name != null) 
                    {
                        app.Name = title.Name;
                        title.Name = null;
                    }
                    app.Children.Add(title);
                    if (app.EndChar < title.EndChar) 
                        app.EndToken = title.EndToken;
                    if (title.EndToken.Next != null) 
                    {
                        if (title.EndToken.EndChar < app.EndToken.BeginChar) 
                        {
                            FragToken acontent = new FragToken(title.EndToken.Next, app.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content };
                            app.Children.Add(acontent);
                            acontent._analizeContent(app, false, Pullenti.Ner.Instrument.InstrumentKind.Undefined);
                        }
                        else 
                        {
                        }
                    }
                    if (app.Children.Count == 1 && app.Kind != Pullenti.Ner.Instrument.InstrumentKind.Appendix) 
                    {
                        app.Children.Clear();
                        app.Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined;
                        app.Name = null;
                    }
                }
                if (tail != null) 
                {
                    app.Children.Add(tail);
                    app.EndToken = tail.EndToken;
                }
                i = j - 1;
            }
            if (res.Children.Count > 0) 
                res.EndToken = res.Children[res.Children.Count - 1].EndToken;
            List<FragToken> appendixes = new List<FragToken>();
            foreach (FragToken ch in res.Children) 
            {
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix) 
                    appendixes.Add(ch);
            }
            for (i = 1; i < appendixes.Count; i++) 
            {
                int maxCoef = 0;
                int ii = -1;
                for (int j = i - 1; j >= 0; j--) 
                {
                    int coef = appendixes[i].CalcOwnerCoef(appendixes[j]);
                    if (coef > maxCoef) 
                    {
                        maxCoef = coef;
                        ii = j;
                    }
                }
                if (ii < 0) 
                    continue;
                appendixes[ii].Children.Add(appendixes[i]);
                res.Children.Remove(appendixes[i]);
            }
            if (maxChar > 0) 
                return res;
            if (!isContract && headKind != Pullenti.Ner.Decree.DecreeKind.Standard) 
            {
                foreach (FragToken ch in res.Children) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.InternalDocument || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Head) 
                    {
                        if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix && res.m_Doc.Name != null) 
                            continue;
                        FragToken hi = (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Head ? ch : ch.FindChild(Pullenti.Ner.Instrument.InstrumentKind.Head));
                        if (hi != null) 
                        {
                            hi = hi.FindChild(Pullenti.Ner.Instrument.InstrumentKind.Name);
                            if (hi != null && hi.Value != null && hi.Value.ToString().Length > 20) 
                                res.m_Doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, hi.Value, false, 0);
                        }
                    }
                }
            }
            if (res.m_Doc.Typ == null) 
            {
                foreach (FragToken ch in res.Children) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix) 
                    {
                        FragToken hi = ch.FindChild(Pullenti.Ner.Instrument.InstrumentKind.Head);
                        if (hi != null) 
                            hi = hi.FindChild(Pullenti.Ner.Instrument.InstrumentKind.DocReference);
                        if (hi != null) 
                        {
                            Pullenti.Ner.Token t1 = hi.BeginToken;
                            if (t1.IsValue("К", "ДО") && t1.Next != null) 
                                t1 = t1.Next;
                            Pullenti.Ner.Decree.DecreeReferent dr = t1.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                            if (dr != null && dr.Number == res.m_Doc.RegNumber) 
                                res.m_Doc.Typ = dr.Typ;
                            else 
                            {
                                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t1, null, false);
                                if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                                    res.m_Doc.Typ = dt.Value;
                            }
                        }
                        break;
                    }
                }
            }
            res._createJusticeResolution();
            if (res.m_Doc.Typ == null && ((res.m_Doc.RegNumber != null || res.m_Doc.CaseNumber != null))) 
            {
                if ((res.Children.Count > 1 && res.Children[1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content && res.Children[1].Children.Count > 0) && res.Children[1].Children[res.Children[1].Children.Count - 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.DocPart) 
                {
                    FragToken part = res.Children[1].Children[res.Children[1].Children.Count - 1];
                    foreach (FragToken ch in part.Children) 
                    {
                        if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Directive && ch.Value != null) 
                        {
                            res.m_Doc.Typ = ch.Value as string;
                            break;
                        }
                    }
                }
            }
            return res;
        }
        static FragToken _createCaseInfo(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (!t.IsNewlineBefore) 
                return null;
            bool rez = false;
            Pullenti.Ner.Token t1 = null;
            if ((t is Pullenti.Ner.ReferentToken) && (t.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)) 
            {
                Pullenti.Ner.Decree.DecreePartReferent dpr = t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                if (dpr.Part == "резолютивная") 
                    t1 = t;
            }
            else if (t.IsValue("РЕЗОЛЮТИВНЫЙ", "РЕЗОЛЮТИВНЫЙ") && t.Next != null && t.Next.IsValue("ЧАСТЬ", "ЧАСТИНА")) 
                t1 = t.Next;
            else if (t.IsValue("ПОЛНЫЙ", "ПОВНИЙ") && t.Next != null && t.Next.IsValue("ТЕКСТ", null)) 
                t1 = t.Next;
            if (t1 != null) 
            {
                rez = true;
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t1.Next, null, false);
                if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                    t1 = dt.EndToken;
            }
            if (!rez) 
            {
                if ((t.IsValue("ПОСТАНОВЛЕНИЕ", "ПОСТАНОВА") || t.IsValue("РЕШЕНИЕ", "РІШЕННЯ") || t.IsValue("ОПРЕДЕЛЕНИЕ", "ВИЗНАЧЕННЯ")) || t.IsValue("ПРИГОВОР", "ВИРОК")) 
                {
                    if (t.IsNewlineAfter && t.Chars.IsAllUpper) 
                        return null;
                    t1 = t;
                }
            }
            if (t1 == null) 
                return null;
            if (t1.Next != null && t1.Next.Morph.Class.IsPreposition) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t1.Next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                    t1 = npt.EndToken;
            }
            if (t1.Next != null && t1.Next.Morph.Class.IsVerb) 
            {
            }
            else 
                return null;
            bool hasDate = false;
            for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
            {
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                    break;
                else 
                {
                    t1 = tt;
                    if (t1.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                        hasDate = true;
                }
            }
            if ((!hasDate && t1.Next != null && (t1.Next.GetReferent() is Pullenti.Ner.Date.DateReferent)) && t1.Next.IsNewlineAfter) 
                t1 = t1.Next;
            return new FragToken(t, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.CaseInfo };
        }
        static FragToken _createApproved(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            FragToken res = null;
            if (((t is Pullenti.Ner.ReferentToken) && (t as Pullenti.Ner.ReferentToken).BeginToken.IsChar('(') && (t as Pullenti.Ner.ReferentToken).EndToken.IsChar(')')) && (t as Pullenti.Ner.ReferentToken).BeginToken.Next.IsValue("ПРОТОКОЛ", null)) 
            {
                res = new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
                res.Referents = new List<Pullenti.Ner.Referent>();
                res.Referents.Add(t.GetReferent());
                return res;
            }
            Pullenti.Ner.Token tt = InstrToken._checkApproved(t);
            if (tt != null) 
                res = new FragToken(t, tt) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
            else if ((t.IsValue("ОДОБРИТЬ", "СХВАЛИТИ") || t.IsValue("ПРИНЯТЬ", "ПРИЙНЯТИ") || t.IsValue("УТВЕРДИТЬ", "ЗАТВЕРДИТИ")) || t.IsValue("СОГЛАСОВАТЬ", null)) 
            {
                if (t.Morph.ContainsAttr("инф.", null) && t.Morph.ContainsAttr("сов.в.", null)) 
                {
                }
                else 
                    res = new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
            }
            else if ((t is Pullenti.Ner.TextToken) && (((t as Pullenti.Ner.TextToken).Term == "ИМЕНЕМ" || (t as Pullenti.Ner.TextToken).Term == "ІМЕНЕМ"))) 
                res = new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
            if (res == null) 
                return null;
            t = res.EndToken;
            if (t.Next == null) 
                return res;
            if (!t.IsNewlineAfter && t.Next.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) == res.BeginToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false)) 
            {
                for (t = t.Next; t != null; t = t.Next) 
                {
                    if (t.IsNewlineBefore || t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) != res.BeginToken.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false)) 
                        break;
                    else 
                        res.EndToken = t;
                }
                if (t.Next == null) 
                    return res;
                Pullenti.Ner.Token tt0 = t.Next;
                for (t = t.Next; t != null; t = t.Next) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeToken dtt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                    if (dtt != null) 
                    {
                        if (dtt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && t != tt0 && t.IsNewlineBefore) 
                            break;
                        res.EndToken = (t = dtt.EndToken);
                        continue;
                    }
                    if (t.NewlinesBeforeCount > 1) 
                        break;
                    res.EndToken = t;
                }
                return res;
            }
            for (t = t.Next; t != null; t = t.Next) 
            {
                if (t.IsAnd || t.Morph.Class.IsPreposition) 
                    continue;
                if (t.IsValue("ВВЕСТИ", null) || t.IsValue("ДЕЙСТВИЕ", "ДІЯ")) 
                {
                    res.EndToken = t;
                    continue;
                }
                break;
            }
            while (t != null) 
            {
                if (t.IsCharOf(":.,") || Pullenti.Ner.Core.BracketHelper.IsBracket(t, true)) 
                    t = t.Next;
                else 
                    break;
            }
            if (t == null) 
                return res;
            List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t, null, 10, false);
            if (dts != null && dts.Count > 0) 
            {
                for (int i = 0; i < dts.Count; i++) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeToken dt = dts[i];
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                        res.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Organization, Value = dt });
                    else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                        res.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Initiator, Value = dt });
                    else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        res.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Date, Value = dt });
                    else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        res.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Number, Value = dt });
                    else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && i == 0) 
                    {
                        if (((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                        {
                            i++;
                            dt = dts[i];
                        }
                    }
                    else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr && res.BeginToken.IsValue("ИМЕНЕМ", "ІМЕНЕМ")) 
                        res.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Place, Value = dt });
                    else 
                        break;
                    res.EndToken = dt.EndToken;
                }
            }
            else if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
            {
                res.Referents = new List<Pullenti.Ner.Referent>();
                for (; t != null; t = t.Next) 
                {
                    if (t.IsCommaAnd) 
                        continue;
                    if (t.IsChar('.')) 
                    {
                        res.EndToken = t;
                        continue;
                    }
                    Pullenti.Ner.Decree.DecreeReferent dr = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                    if (dr == null) 
                        break;
                    if (res.Referents.Count > 0 && t.NewlinesBeforeCount > 1) 
                        break;
                    res.Referents.Add(dr);
                    res.EndToken = t;
                }
            }
            else if ((t.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (t.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
            {
                res.Referents = new List<Pullenti.Ner.Referent>();
                for (; t != null; t = t.Next) 
                {
                    if (t.IsCommaAnd) 
                        continue;
                    if ((t.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (t.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
                    {
                        res.Referents.Add(t.GetReferent());
                        res.EndToken = t;
                    }
                    else 
                        break;
                }
            }
            if (res.Children.Count == 0) 
            {
                if (((!res.BeginToken.IsNewlineBefore && !res.BeginToken.Previous.IsTableControlChar)) || ((!res.EndToken.IsNewlineAfter && !res.EndToken.Next.IsTableControlChar))) 
                    return null;
            }
            if (res.EndToken.Next != null && (res.EndToken.Next.GetReferent() is Pullenti.Ner.Date.DateReferent)) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(res.EndToken.Next, null, false);
                if (dt != null) 
                {
                    res.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Date, Value = dt });
                    res.EndToken = dt.EndToken;
                    dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(res.EndToken.Next, null, false);
                    if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                    {
                        res.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Number, Value = dt });
                        res.EndToken = dt.EndToken;
                    }
                }
            }
            t = res.EndToken.Next;
            if (t != null && t.IsComma) 
                t = t.Next;
            if (t != null && t.IsValue("ПРОТОКОЛ", null)) 
            {
                dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t, null, 10, false);
                if (dts != null && dts.Count > 0) 
                    res.EndToken = dts[dts.Count - 1].EndToken;
                else if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                    res.EndToken = t;
            }
            if (!res.IsNewlineBefore && res.BeginToken.Previous != null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(res.BeginToken.Previous, true, false)) 
                res.BeginToken = res.BeginToken.Previous;
            return res;
        }
        public static FragToken _createMisc(Pullenti.Ner.Token t)
        {
            if (t == null || t.Next == null) 
                return null;
            if (t.IsValue("ФОРМА", null) && t.Next.IsValue("ДОКУМЕНТА", null)) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken num = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t.Next.Next, null, false);
                if (num != null && num.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                    return new FragToken(t, num.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined };
                if ((t.Next.Next is Pullenti.Ner.NumberToken) && t.Next.Next.IsNewlineAfter) 
                    return new FragToken(t, t.Next.Next) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined };
            }
            if (t.IsValue("С", null) && t.Next.IsValue("ИЗМЕНЕНИЕ", null) && t.Next.Next != null) 
            {
                Pullenti.Ner.Token tt = t.Next.Next;
                if (tt.Morph.Class.IsPreposition && tt.Next != null) 
                    tt = tt.Next;
                if (tt.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                {
                    if (tt.Next != null && tt.Next.IsChar('.')) 
                        tt = tt.Next;
                    return new FragToken(t, tt) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined };
                }
            }
            Pullenti.Ner.Token t0 = t;
            while ((t is Pullenti.Ner.TextToken) && t.LengthChar == 1 && t.Next != null) 
            {
                t = t.Next;
            }
            if (t.IsValue("ЗАКАЗ", null)) 
            {
                InstrToken1 itt = InstrToken1.Parse(t, false, null, 0, null, false, 0, false, false);
                if (itt != null) 
                    return new FragToken(t, itt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined };
            }
            int nums = 0;
            int spec = 0;
            int chars = 0;
            int words = 0;
            for (Pullenti.Ner.Token t1 = t0; t1 != null; t1 = t1.Next) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken ddd = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t1, null, false);
                if (ddd != null) 
                    break;
                if (t1.IsTableControlChar) 
                {
                }
                else if (t1 is Pullenti.Ner.NumberToken) 
                    nums++;
                else if (!(t1 is Pullenti.Ner.TextToken) || t1.LengthChar > 7) 
                    break;
                else if (!t1.Chars.IsLetter) 
                    spec++;
                else if (t1.LengthChar < 3) 
                    chars++;
                else if (t1.GetMorphClassInDictionary().IsUndefined) 
                    chars++;
                else 
                    words++;
                if (!t1.IsNewlineAfter) 
                    continue;
                if ((nums + spec + chars) <= 1) 
                    break;
                if ((nums + spec + chars) > (words * 3)) 
                {
                }
                else if ((words < 2) && (nums + spec + chars) > 3) 
                {
                }
                else 
                    break;
                return new FragToken(t0, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined };
            }
            if (t.GetReferent() is Pullenti.Ner.Address.StreetReferent) 
            {
                InstrToken1 lin = InstrToken1.Parse(t0, true, null, 0, null, false, 0, false, false);
                if (lin != null && (lin.LengthChar < 70)) 
                    return new FragToken(t0, lin.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Contact };
            }
            if (t0.IsChar('(')) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t0, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null && br.IsNewlineAfter) 
                    return new FragToken(t0, br.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined };
            }
            return null;
        }
        public static FragToken _createEditions(Pullenti.Ner.Token t)
        {
            if (t == null || t.Next == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            bool isInBracks = false;
            bool ok = false;
            if ((t.IsNewlineBefore && t.IsValue("С", null) && t.Next != null) && t.Next.IsValue("ИЗМЕНЕНИЕ", null)) 
            {
                FragToken eee = new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Editions };
                for (t = t.Next.Next; t != null; t = t.Next) 
                {
                    if (t.IsCommaAnd || (t.GetReferent() is Pullenti.Ner.Date.DateReferent)) 
                        eee.EndToken = t;
                    else if (t.IsValue("ДОПОЛНЕНИЕ", null) || t.IsCharOf(":;.") || t.IsValue("ОТ", null)) 
                        eee.EndToken = t;
                    else 
                    {
                        Pullenti.Ner.Date.Internal.DateItemToken dd = Pullenti.Ner.Date.Internal.DateItemToken.TryAttach(t, null, false);
                        if (dd != null) 
                            eee.EndToken = (t = dd.EndToken);
                        else 
                            break;
                    }
                }
                return eee;
            }
            if (t.IsValue("СПИСОК", null)) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.Noun.IsValue("ДОКУМЕНТ", null)) 
                {
                    t = npt.EndToken.Next;
                    if (t != null && t.IsCharOf(":.")) 
                        t = t.Next;
                    if (t == null) 
                        return null;
                }
            }
            if (!t.IsChar('(') && !t.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
            {
                if (t.IsValue("В", "У") && t.Next != null && ((t.Next.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ") || t.Next.IsValue("РЕД", null)))) 
                {
                }
                else if (t.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeToken dtt0 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t.Next, null, false);
                    if (dtt0 != null) 
                        return new FragToken(t, dtt0.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Editions };
                }
                else 
                    return null;
            }
            else 
            {
                isInBracks = t.IsChar('(');
                t = t.Next;
            }
            Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
            if (dt != null && ((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date))) 
                t = dt.EndToken.Next;
            else if (t is Pullenti.Ner.NumberToken) 
                t = t.Next;
            Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, true);
            if (pt != null) 
                t = pt.EndToken.Next;
            else if (isInBracks && (t.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)) 
                t = t.Next;
            if (t == null) 
                return null;
            bool isDoubt = false;
            while (((t.Morph.Class.IsPreposition || t.Morph.Class.IsAdverb)) && t.Next != null) 
            {
                t = t.Next;
            }
            if (t.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                ok = true;
            else if (t.IsValue("РЕД", null)) 
            {
                ok = true;
                if (t.Next != null && t.Next.IsChar('.')) 
                    t = t.Next;
            }
            else if ((t.IsValue("ИЗМ", null) || t.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА") || t.IsValue("УЧЕТ", "ОБЛІК")) || t.IsValue("ВКЛЮЧИТЬ", "ВКЛЮЧИТИ") || t.IsValue("ДОПОЛНИТЬ", "ДОПОВНИТИ")) 
            {
                if (t.IsValue("УЧЕТ", "ОБЛІК")) 
                    isDoubt = true;
                ok = true;
                for (t = t.Next; t != null; t = t.Next) 
                {
                    if (t.Next == null) 
                        break;
                    if (t.IsCharOf(",.")) 
                        continue;
                    if (t.IsValue("ВНЕСЕННЫЙ", "ВНЕСЕНИЙ") || t.IsValue("ПОПРАВКА", null)) 
                        continue;
                    t = t.Previous;
                    break;
                }
            }
            else if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
            {
                Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken;
                if (tt.IsValue("В", "У") && tt.Next != null) 
                    tt = tt.Next;
                if (tt.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                    ok = true;
                else if (tt.IsValue("РЕД", null)) 
                    ok = true;
                t = t.Previous;
            }
            if (!ok || t == null) 
                return null;
            List<Pullenti.Ner.Decree.DecreeReferent> decrs = new List<Pullenti.Ner.Decree.DecreeReferent>();
            for (t = t.Next; t != null; t = t.Next) 
            {
                if (isInBracks) 
                {
                    if (t.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && (br.LengthChar < 200)) 
                        {
                            t = br.EndToken;
                            continue;
                        }
                    }
                    if (t.IsChar(')')) 
                        break;
                }
                Pullenti.Ner.Decree.DecreeReferent dr = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                if (dr != null) 
                {
                    decrs.Add(dr);
                    continue;
                }
                if (t.IsCommaAnd) 
                    continue;
                if (t.IsNewlineBefore && !isInBracks) 
                {
                    t = t.Previous;
                    break;
                }
            }
            if (t == null) 
                return null;
            ok = false;
            if (isInBracks) 
            {
                ok = t.IsChar(')');
                if (!t.IsNewlineAfter) 
                {
                    if ((t.Next is Pullenti.Ner.TextToken) && t.Next.IsNewlineAfter && !t.Next.Chars.IsLetter) 
                    {
                    }
                    else 
                        isDoubt = true;
                }
            }
            else if (t.IsChar('.') || t.IsNewlineAfter) 
                ok = true;
            if (decrs.Count > 0) 
                isDoubt = false;
            if (ok && !isDoubt) 
            {
                FragToken eds = new FragToken(t0, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Editions };
                eds.Referents = new List<Pullenti.Ner.Referent>();
                foreach (Pullenti.Ner.Decree.DecreeReferent d in decrs) 
                {
                    eds.Referents.Add(d);
                }
                return eds;
            }
            return null;
        }
        static FragToken _createOwner(Pullenti.Ner.Token t)
        {
            if (t == null || !t.IsNewlineBefore) 
                return null;
            if (!t.Chars.IsCyrillicLetter || t.Chars.IsAllLower) 
                return null;
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Token t11 = null;
            bool ignoreCurLine = false;
            bool keyword = false;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore) 
                    t11 = t1;
                Pullenti.Ner.Referent r = tt.GetReferent();
                if ((r is Pullenti.Ner.Decree.DecreeReferent) && keyword) 
                {
                    tt = tt.Kit.DebedToken(tt);
                    r = tt.GetReferent();
                }
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                if (dt != null) 
                {
                    if ((dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner && dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org && dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Unknown) && dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr && dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Misc) 
                        break;
                    t1 = (tt = dt.EndToken);
                    continue;
                }
                if (tt != t && tt.WhitespacesBeforeCount > 15) 
                {
                    if (tt.Previous != null && tt.Previous.IsHiphen) 
                    {
                    }
                    else 
                        break;
                }
                if (((((r is Pullenti.Ner.Date.DateReferent) || (r is Pullenti.Ner.Address.AddressReferent) || (r is Pullenti.Ner.Address.StreetReferent)) || (r is Pullenti.Ner.Phone.PhoneReferent) || (r is Pullenti.Ner.Uri.UriReferent)) || (r is Pullenti.Ner.Person.PersonIdentityReferent) || (r is Pullenti.Ner.Bank.BankDataReferent)) || (r is Pullenti.Ner.Decree.DecreePartReferent) || (r is Pullenti.Ner.Decree.DecreeReferent)) 
                {
                    ignoreCurLine = true;
                    t1 = t11;
                    break;
                }
                if (tt.Morph.Class == Pullenti.Morph.MorphClass.Verb) 
                {
                    ignoreCurLine = true;
                    t1 = t11;
                    break;
                }
                if ((r is Pullenti.Ner.Geo.GeoReferent) && tt.IsNewlineBefore) 
                    break;
                t1 = tt;
                Pullenti.Ner.ReferentToken oo = tt.Kit.ProcessReferent("ORGANIZATION", tt);
                if (oo != null) 
                {
                    t1 = (tt = oo.EndToken);
                    continue;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if (tt == t) 
                    {
                        Pullenti.Ner.Org.Internal.OrgItemTypeToken typ = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(tt, false, null);
                        if (typ != null) 
                        {
                            keyword = true;
                            t1 = (tt = typ.EndToken);
                            continue;
                        }
                    }
                    t1 = (tt = npt.EndToken);
                }
            }
            if (t1 == null) 
                return null;
            FragToken fr = new FragToken(t, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Organization, DefVal2 = true };
            return fr;
        }
        int CalcOwnerCoef(FragToken owner)
        {
            List<string> ownTyps = new List<string>();
            FragToken ownName = null;
            foreach (FragToken ch in owner.Children) 
            {
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Head) 
                {
                    foreach (FragToken chh in ch.Children) 
                    {
                        if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Typ || chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Name || chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Keyword) 
                        {
                            Pullenti.Ner.Token t = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(chh.BeginToken, false);
                            if (t is Pullenti.Ner.TextToken) 
                                ownTyps.Add((t as Pullenti.Ner.TextToken).Lemma);
                            if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Name && ownName == null) 
                                ownName = chh;
                        }
                    }
                }
            }
            foreach (FragToken ch in Children) 
            {
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Head) 
                {
                    foreach (FragToken chh in ch.Children) 
                    {
                        if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.DocReference) 
                        {
                            Pullenti.Ner.Token t = chh.BeginToken;
                            if (t.Morph.Class.IsPreposition) 
                                t = t.Next;
                            Pullenti.Ner.Token tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
                            if (tt is Pullenti.Ner.TextToken) 
                            {
                                string ty = (tt as Pullenti.Ner.TextToken).Lemma;
                                if (ownTyps.Contains(ty)) 
                                    return 1;
                                continue;
                            }
                            Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, false);
                            if (pt != null) 
                            {
                                if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix) 
                                {
                                    if (owner.Number > 0) 
                                    {
                                        foreach (Pullenti.Ner.Decree.Internal.PartToken.PartValue nn in pt.Values) 
                                        {
                                            if (nn.Value == owner.Number.ToString()) 
                                                return 3;
                                        }
                                    }
                                }
                            }
                            if (ownName != null && (ownName.Value is string)) 
                            {
                                string val0 = ownName.Value as string;
                                string val1 = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, chh.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                                if (val1 == val0) 
                                    return 3;
                                if (Pullenti.Ner.Core.MiscHelper.CanBeEquals(val0, val1, true, true, false)) 
                                    return 3;
                                if (val1 != null && ((val1.StartsWith(val0) || val0.StartsWith(val1)))) 
                                    return 1;
                            }
                        }
                    }
                }
            }
            return 0;
        }
        public bool HasChanges
        {
            get
            {
                if (BeginToken.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent) 
                    return true;
                for (Pullenti.Ner.Token t = BeginToken; t != null && t.EndChar <= EndChar; t = t.Next) 
                {
                    if (t.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent) 
                        return true;
                }
                return false;
            }
        }
        public Pullenti.Ner.MetaToken MultilineChangesValue
        {
            get
            {
                for (Pullenti.Ner.Token t = BeginToken; t != null && (t.BeginChar < EndChar); t = t.Next) 
                {
                    if (t.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent) 
                    {
                        Pullenti.Ner.Decree.DecreeChangeReferent dcr = t.GetReferent() as Pullenti.Ner.Decree.DecreeChangeReferent;
                        for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken; tt != null && tt.EndChar <= t.EndChar; tt = tt.Next) 
                        {
                            Pullenti.Ner.Decree.DecreeChangeValueReferent dval = tt.GetReferent() as Pullenti.Ner.Decree.DecreeChangeValueReferent;
                            if (dval == null || dval.Kind != Pullenti.Ner.Decree.DecreeChangeValueKind.Text) 
                                continue;
                            string val = dval.Value;
                            if (val == null || (val.Length < 100)) 
                                continue;
                            if ((val.IndexOf('\r') < 0) && (val.IndexOf('\n') < 0) && !tt.IsNewlineBefore) 
                                continue;
                            Pullenti.Ner.Token t0 = null;
                            for (t = (tt as Pullenti.Ner.MetaToken).BeginToken; t != null && t.EndChar <= tt.EndChar; t = t.Next) 
                            {
                                if (Pullenti.Ner.Core.BracketHelper.IsBracket(t, true) && ((t.IsWhitespaceBefore || t.Previous.IsChar(':')))) 
                                {
                                    t0 = t.Next;
                                    break;
                                }
                                else if (t.Previous != null && t.Previous.IsChar(':') && t.IsNewlineBefore) 
                                {
                                    t0 = t;
                                    break;
                                }
                            }
                            Pullenti.Ner.Token t1 = (tt as Pullenti.Ner.MetaToken).EndToken;
                            if (Pullenti.Ner.Core.BracketHelper.IsBracket(t1, true)) 
                                t1 = t1.Previous;
                            if (t0 != null && ((t0.EndChar + 50) < t1.EndChar)) 
                                return new Pullenti.Ner.MetaToken(t0, t1) { Tag = dcr };
                            return null;
                        }
                    }
                    if (t.EndChar > EndChar) 
                        break;
                }
                return null;
            }
        }
        static FragToken CreateTZTitle(Pullenti.Ner.Token t0, Pullenti.Ner.Instrument.InstrumentReferent doc)
        {
            Pullenti.Ner.Decree.Internal.DecreeToken tz = null;
            int cou = 0;
            Pullenti.Ner.Token t;
            for (t = t0; t != null && (cou < 300); t = t.Next) 
            {
                if ((t is Pullenti.Ner.TextToken) && t.LengthChar > 1) 
                    cou++;
                if (!t.IsNewlineBefore) 
                {
                    if (t.Previous != null && t.Previous.IsTableControlChar) 
                    {
                    }
                    else 
                        continue;
                }
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    if (dt.Value == "ТЕХНИЧЕСКОЕ ЗАДАНИЕ") 
                        tz = dt;
                    break;
                }
            }
            if (tz == null) 
                return null;
            FragToken title = new FragToken(t0, tz.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Head };
            for (t = t0; t != null; t = t.Next) 
            {
                if (!t.IsNewlineBefore) 
                {
                    title.EndToken = t;
                    continue;
                }
                if (_isStartOfBody(t, false)) 
                    break;
                if (t.IsValue("СОДЕРЖИМОЕ", null) || t.IsValue("СОДЕРЖАНИЕ", null) || t.IsValue("ОГЛАВЛЕНИЕ", null)) 
                    break;
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                if (dt != null) 
                {
                    _addTitleAttr(doc, title, dt);
                    title.EndToken = (t = dt.EndToken);
                    if (dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        continue;
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 100);
                    if (br != null && Pullenti.Ner.Core.BracketHelper.IsBracket(t.Next, true)) 
                    {
                        FragToken nam = new FragToken(br.BeginToken, br.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal = true };
                        title.Children.Add(nam);
                        title.EndToken = (t = br.EndToken);
                        continue;
                    }
                    if (t.Next != null && t.Next.IsValue("НА", null)) 
                    {
                        Pullenti.Ner.Token t1 = t.Next;
                        for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsNewlineBefore) 
                            {
                                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                                    break;
                                if (tt.IsValue("СОДЕРЖИМОЕ", null) || tt.IsValue("СОДЕРЖАНИЕ", null) || tt.IsValue("ОГЛАВЛЕНИЕ", null)) 
                                    break;
                            }
                            Pullenti.Ner.Core.BracketSequenceToken br1 = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br1 != null) 
                            {
                                t1 = (tt = br1.EndToken);
                                continue;
                            }
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                            if (npt != null) 
                                tt = npt.EndToken;
                            t1 = tt;
                        }
                        FragToken nam = new FragToken(t.Next, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal = true };
                        title.Children.Add(nam);
                        title.EndToken = (t = t1);
                        continue;
                    }
                }
                FragToken appr1 = _createApproved(t);
                if (appr1 != null) 
                {
                    t = appr1.EndToken;
                    title.Children.Add(appr1);
                    title.EndToken = appr1.EndToken;
                    continue;
                }
                FragToken eds = _createEditions(t);
                if (eds != null) 
                {
                    title.Children.Add(eds);
                    title.EndToken = (t = eds.EndToken);
                    continue;
                }
                appr1 = _createMisc(t);
                if (appr1 != null) 
                {
                    t = appr1.EndToken;
                    title.Children.Add(appr1);
                    title.EndToken = appr1.EndToken;
                    continue;
                }
            }
            return title;
        }
        internal bool _analizeTables()
        {
            if (Children.Count > 0) 
            {
                int abzCount = 0;
                int cou = 0;
                foreach (FragToken ch in Children) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                        abzCount++;
                    if (ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Keyword && ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Number && ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Number) 
                        cou++;
                }
                if (abzCount == cou && cou > 0) 
                {
                    List<FragToken> chs = Children;
                    Children = new List<FragToken>();
                    bool bb = this._analizeTables();
                    Children = chs;
                    if (bb) 
                    {
                        for (int i = 0; i < Children.Count; i++) 
                        {
                            if (Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                            {
                                FragToken ch0 = (i > 0 ? Children[i - 1] : null);
                                if (ch0 != null && ch0.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                                {
                                    ch0.EndToken = Children[i].EndToken;
                                    Children.RemoveAt(i);
                                    i--;
                                }
                                else 
                                    Children[i].Kind = Pullenti.Ner.Instrument.InstrumentKind.Content;
                            }
                        }
                    }
                }
                List<FragToken> changed = new List<FragToken>();
                foreach (FragToken ch in Children) 
                {
                    if (ch._analizeTables()) 
                        changed.Add(ch);
                }
                for (int i = changed.Count - 1; i >= 0; i--) 
                {
                    if (changed[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                    {
                        int j = Children.IndexOf(changed[i]);
                        if (j < 0) 
                            continue;
                        Children.RemoveAt(j);
                        Children.InsertRange(j, changed[i].Children);
                    }
                }
                return false;
            }
            if (((Kind == Pullenti.Ner.Instrument.InstrumentKind.Chapter || Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause || Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) || Kind == Pullenti.Ner.Instrument.InstrumentKind.Item || Kind == Pullenti.Ner.Instrument.InstrumentKind.Subitem) || Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
            {
            }
            else 
                return false;
            if (Itok != null && Itok.HasChanges) 
                return false;
            int endChar = EndChar;
            if (EndToken.Next == null) 
                endChar = Kit.Sofa.Text.Length - 1;
            Pullenti.Ner.Token t0 = BeginToken;
            bool tabs = false;
            for (Pullenti.Ner.Token tt = BeginToken; tt != null && tt.EndChar <= endChar; tt = tt.Next) 
            {
                if (!tt.IsNewlineBefore) 
                    continue;
                if (tt.IsChar((char)0x1E)) 
                {
                }
                List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(tt, endChar, false);
                if (rows == null || (rows.Count < 2)) 
                    continue;
                bool ok = true;
                foreach (Pullenti.Ner.Core.TableRowToken r in rows) 
                {
                    if (r.Cells.Count > 15) 
                        ok = false;
                }
                if (!ok) 
                {
                    tt = rows[rows.Count - 1].EndToken;
                    continue;
                }
                if (t0.EndChar < rows[0].BeginChar) 
                    Children.Add(new FragToken(t0, rows[0].BeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
                FragToken tab = new FragToken(rows[0].BeginToken, rows[rows.Count - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Table };
                Children.Add(tab);
                for (int i = 0; i < rows.Count; i++) 
                {
                    FragToken rr = new FragToken(rows[i].BeginToken, rows[i].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.TableRow, Number = i + 1 };
                    tab.Children.Add(rr);
                    tabs = true;
                    int no = 0;
                    int cols = 0;
                    foreach (Pullenti.Ner.Core.TableCellToken ce in rows[i].Cells) 
                    {
                        FragToken cell = new FragToken(ce.BeginToken, ce.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.TableCell, Number = ++no };
                        if (ce.ColSpan > 1) 
                            cols += (((cell.SubNumber = ce.ColSpan)));
                        else 
                            cols++;
                        if (ce.RowSpan > 1) 
                            cell.SubNumber2 = ce.RowSpan;
                        rr.Children.Add(cell);
                    }
                    if (tab.Number < cols) 
                        tab.Number = cols;
                    tt = rows[i].EndToken;
                }
                if (tab.Number > 1) 
                {
                    int[] rnums = new int[(int)tab.Number];
                    int[] rnumsCols = new int[(int)tab.Number];
                    foreach (FragToken r in tab.Children) 
                    {
                        int no = 0;
                        for (int ii = 0; ii < r.Children.Count; ii++) 
                        {
                            if ((no < rnums.Length) && rnums[no] > 0) 
                            {
                                rnums[no]--;
                                no += rnumsCols[no];
                                ii--;
                                continue;
                            }
                            r.Children[ii].Number = no + 1;
                            if (r.Children[ii].SubNumber2 > 1 && (no < rnums.Length)) 
                            {
                                rnums[no] = r.Children[ii].SubNumber2 - 1;
                                rnumsCols[no] = (r.Children[ii].SubNumber == 0 ? 1 : r.Children[ii].SubNumber);
                            }
                            no += (r.Children[ii].SubNumber == 0 ? 1 : r.Children[ii].SubNumber);
                        }
                    }
                }
                t0 = tt.Next;
            }
            if ((t0 != null && (t0.EndChar < EndChar) && tabs) && t0 != EndToken) 
                Children.Add(new FragToken(t0, EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
            return tabs;
        }
        static FragToken CreateDocTitle(Pullenti.Ner.Token t0, Pullenti.Ner.Instrument.InstrumentReferent doc)
        {
            if (t0 == null) 
                return null;
            FragToken title = CreateContractTitle(t0, doc);
            if (title != null) 
                return title;
            title = CreateGostTitle(t0, doc);
            if (title != null) 
                return title;
            title = CreateZapiskaTitle(t0, doc);
            if (title != null) 
                return title;
            title = CreateTZTitle(t0, doc);
            if (title != null) 
                return title;
            doc.Slots.Clear();
            title = CreateProjectTitle(t0, doc);
            if (title != null) 
                return title;
            doc.Slots.Clear();
            title = _createDocTitle_(t0, doc);
            if (title != null && title.Children.Count == 1 && title.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Name) 
            {
                FragToken title2 = _createDocTitle_(title.EndToken.Next, doc);
                if (title2 != null && doc.Typ != null) 
                {
                    title.Children.AddRange(title2.Children);
                    title.EndToken = title2.EndToken;
                }
            }
            return title;
        }
        static FragToken _createDocTitle_(Pullenti.Ner.Token t0, Pullenti.Ner.Instrument.InstrumentReferent doc)
        {
            for (; t0 != null; t0 = t0.Next) 
            {
                if (!t0.IsTableControlChar) 
                    break;
            }
            if (t0 == null) 
                return null;
            FragToken title = new FragToken(t0, t0) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Head };
            Pullenti.Ner.Decree.Internal.DecreeToken dt0 = null;
            Pullenti.Ner.Token t;
            Pullenti.Ner.Token t1 = null;
            string name = null;
            Pullenti.Ner.Token nT0 = null;
            int emptyLines = 0;
            Pullenti.Ner.Token endEmptyLines = null;
            bool ignoreEmptyLines = false;
            int attrs = 0;
            bool canBeOrgs = true;
            List<FragToken> unknownOrgs = new List<FragToken>();
            bool isContract = false;
            bool startOfName = false;
            t = t0;
            if (t0.GetReferent() != null) 
            {
                if (t0.GetReferent().TypeName == "PERSON") 
                    return null;
            }
            FragToken appr0 = null;
            if (t0.IsValue("УТВЕРДИТЬ", "ЗАТВЕРДИТИ") || t0.IsValue("ПРИНЯТЬ", "ПРИЙНЯТИ") || t0.IsValue("УТВЕРЖДАТЬ", null)) 
            {
                appr0 = _createApproved(t);
                if (appr0 != null && appr0.Referents == null) 
                    appr0 = null;
            }
            if (appr0 != null) 
            {
                t1 = (title.EndToken = appr0.EndToken);
                title.Children.Add(appr0);
                t = t1.Next;
            }
            FragToken edi0 = null;
            if (t0.IsValue("РЕДАКЦИЯ", null)) 
                edi0 = _createEditions(t0);
            if (edi0 != null) 
            {
                t1 = (title.EndToken = edi0.EndToken);
                title.Children.Add(edi0);
                t = t1.Next;
            }
            if (t != null && t.IsValue("ДЕЛО", "СПРАВА")) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t.Next, null, false);
                if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                {
                    dt.BeginToken = t;
                    title.Children.Add(new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, Value = "ДЕЛО" });
                    _addTitleAttr(doc, title, dt);
                    t = dt.EndToken.Next;
                    if (t != null && t.IsValue("КОПИЯ", "КОПІЯ")) 
                        t = t.Next;
                    else if ((t.IsChar('(') && t.Next != null && t.Next.IsValue("КОПИЯ", "КОПІЯ")) && t.Next.Next != null && t.Next.Next.IsChar(')')) 
                        t = t.Next.Next;
                }
            }
            for (; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                    continue;
                if (t.IsNewlineBefore || ((t.Previous != null && t.Previous.IsTableControlChar))) 
                {
                    if ((t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && (t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent).Kind != Pullenti.Ner.Decree.DecreeKind.Publisher) 
                        t = t.Kit.DebedToken(t);
                    if (t.IsValue("О", "ПРО") || t.IsValue("ОБ", null) || t.IsValue("ПО", null)) 
                        break;
                    if (_isStartOfBody(t, false)) 
                        break;
                    if (t.IsCharOf("[") && name == null) 
                        break;
                    InstrToken1 iii = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                    if (iii != null && iii.Typ == InstrToken1.Types.Comment) 
                    {
                        FragToken cmt = new FragToken(iii.BeginToken, iii.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Comment };
                        title.Children.Add(cmt);
                        t = (t1 = (title.EndToken = iii.EndToken));
                        continue;
                    }
                    if (iii != null && iii.EndToken.IsChar('?')) 
                    {
                        FragToken cmt = new FragToken(iii.BeginToken, iii.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name };
                        cmt.Value = FragToken.GetRestoredNameMT(iii, false);
                        title.Children.Add(cmt);
                        t = (t1 = (title.EndToken = iii.EndToken));
                        break;
                    }
                    if ((((t.IsValue("ЗАЯВИТЕЛЬ", "ЗАЯВНИК") || t.IsValue("ИСТЕЦ", "ПОЗИВАЧ") || t.IsValue("ОТВЕТЧИК", "ВІДПОВІДАЧ")) || t.IsValue("ДОЛЖНИК", "БОРЖНИК") || t.IsValue("КОПИЯ", "КОПІЯ"))) && t.Next != null && ((t.Next.IsChar(':') || t.Next.IsTableControlChar))) 
                    {
                        Pullenti.Ner.ReferentToken ptt = _createJustParticipant(t.Next.Next, null);
                        if (ptt != null) 
                        {
                            if (t.IsValue("КОПИЯ", null)) 
                            {
                            }
                            t1 = ptt.EndToken;
                            while (t1.Next != null && t1.Next.IsTableControlChar) 
                            {
                                t1 = t1.Next;
                            }
                            if (t1.Next != null && t1.Next.IsChar('(')) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null) 
                                    t1 = br.EndToken;
                            }
                            FragToken ft = new FragToken(t, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Initiator };
                            title.Children.Add(ft);
                            t = (title.EndToken = t1);
                            continue;
                        }
                    }
                    if (t.IsValue("ЦЕНА", "ЦІНА") && t.Next != null && t.Next.IsValue("ИСК", "ПОЗОВ")) 
                    {
                        bool hasMoney = false;
                        Pullenti.Ner.Token tt;
                        for (tt = t.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.GetReferent() is Pullenti.Ner.Money.MoneyReferent) 
                                hasMoney = true;
                            if (tt.IsNewlineAfter) 
                                break;
                        }
                        if (tt != null && hasMoney) 
                        {
                            while (tt.Next != null && tt.Next.IsTableControlChar) 
                            {
                                tt = tt.Next;
                            }
                            if (tt.Next != null && tt.Next.IsChar('(')) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null) 
                                    tt = br.EndToken;
                            }
                            title.Children.Add(new FragToken(t, tt) { Kind = Pullenti.Ner.Instrument.InstrumentKind.CaseInfo });
                            t = (title.EndToken = (t1 = tt));
                            continue;
                        }
                    }
                    if (t.IsValue("В", "У")) 
                    {
                        Pullenti.Ner.Token tt = t.Next;
                        if (tt != null && tt.IsTableControlChar) 
                            tt = tt.Next;
                        if (tt != null && (tt.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                        {
                            Pullenti.Ner.Referent r = tt.GetReferent();
                            while (tt.Next != null && tt.Next.IsTableControlChar) 
                            {
                                tt = tt.Next;
                            }
                            t1 = tt;
                            if (t1.Next != null && t1.Next.IsChar('(')) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null) 
                                    t1 = br.EndToken;
                            }
                            FragToken ooo = new FragToken(t, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Organization };
                            ooo.Referents = new List<Pullenti.Ner.Referent>();
                            ooo.Referents.Add(r);
                            title.Children.Add(ooo);
                            t = (title.EndToken = t1);
                            continue;
                        }
                    }
                    if (t.LengthChar == 1 && t.Chars.IsLetter && t.IsWhitespaceAfter) 
                    {
                        int ii;
                        for (ii = 0; ii < InstrToken.m_DirectivesNorm.Count; ii++) 
                        {
                            Pullenti.Ner.Token ee = Pullenti.Ner.Core.MiscHelper.TryAttachWordByLetters(InstrToken.m_DirectivesNorm[ii], t, false);
                            if (ee != null && ee.IsNewlineAfter) 
                            {
                                FragToken ooo = new FragToken(t, ee) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, Value = InstrToken.m_DirectivesNorm[ii] };
                                title.Children.Add(ooo);
                                doc.Typ = InstrToken.m_DirectivesNorm[ii];
                                t = (title.EndToken = ee);
                                break;
                            }
                        }
                        if (ii < InstrToken.m_DirectivesNorm.Count) 
                            continue;
                    }
                }
                if (t.IsHiphen || t.IsChar('_')) 
                {
                    char ch = t.GetSourceText()[0];
                    for (; t != null; t = t.Next) 
                    {
                        if (!t.IsChar(ch)) 
                            break;
                    }
                }
                if (t == null) 
                    break;
                FragToken casinf = _createCaseInfo(t);
                if (casinf != null) 
                    break;
                Pullenti.Ner.Decree.DecreeReferent dr0 = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                if (dr0 != null) 
                {
                    if (dr0.Kind == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                        continue;
                }
                else if (t.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) 
                {
                    Pullenti.Ner.Decree.DecreePartReferent dpr = t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                    if (dpr != null) 
                    {
                        if (((dpr.Part == null && dpr.DocPart == null)) || dpr.Slots.Count != 2) 
                            break;
                        if ((t.Next is Pullenti.Ner.TextToken) && (t.Next as Pullenti.Ner.TextToken).IsPureVerb) 
                            break;
                        dr0 = dpr.Owner;
                    }
                }
                if (dr0 != null) 
                {
                    if (doc.Typ == null || doc.Typ == dr0.Typ) 
                    {
                        Pullenti.Ner.Token tt1 = (t as Pullenti.Ner.ReferentToken).BeginToken;
                        List<Pullenti.Ner.Decree.Internal.DecreeToken> li = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(tt1, null, 10, false);
                        if (li != null && li.Count > 0 && li[li.Count - 1].IsNewlineAfter) 
                        {
                            foreach (Pullenti.Ner.Decree.Internal.DecreeToken dd in li) 
                            {
                                _addTitleAttr(doc, title, dd);
                            }
                            Pullenti.Ner.Token ttt = li[li.Count - 1].EndToken;
                            if (ttt.EndChar < t.EndChar) 
                            {
                                nT0 = ttt.Next;
                                name = FragToken.GetRestoredName(ttt.Next, (t as Pullenti.Ner.ReferentToken).EndToken, false);
                            }
                            t1 = t;
                            if (name != null && t1.IsNewlineAfter) 
                            {
                                t = t.Next;
                                break;
                            }
                            if (doc.Typ == "КОДЕКС") 
                            {
                                Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t.Next, null, false, false);
                                if (pt != null) 
                                {
                                    if (((pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart)) || pt.Values.Count != 1) 
                                        pt = null;
                                }
                                if (pt != null && pt.Values.Count > 0) 
                                {
                                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PART, pt.Values[0].Value, false, 0);
                                    title.Children.Add(new FragToken(pt.BeginToken, pt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocPart, Value = pt.Values[0].Value });
                                    t = pt.EndToken;
                                    continue;
                                }
                            }
                            if (doc.Name != null) 
                            {
                                t = t.Next;
                                break;
                            }
                        }
                    }
                    else if (dr0.Typ == "КОДЕКС") 
                    {
                        Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t.Next, null, false, false);
                        string nam = dr0.Name;
                        if (pt != null) 
                        {
                            if (((pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart)) || pt.Values.Count != 1) 
                                pt = null;
                        }
                        if (pt != null && pt.Values.Count > 0) 
                            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PART, pt.Values[0].Value, false, 0);
                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, nam, false, 0);
                        doc.Typ = dr0.Typ;
                        object geo = dr0.GetSlotValue(Pullenti.Ner.Decree.DecreeReferent.ATTR_GEO);
                        if (geo != null) 
                            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_GEO, geo, false, 0);
                        t1 = t;
                        title.Children.Add(new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = nam });
                        if (pt != null && pt.Values.Count > 0) 
                        {
                            title.Children.Add(new FragToken(pt.BeginToken, pt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocPart, Value = pt.Values[0].Value });
                            t1 = pt.EndToken;
                        }
                        t = t1;
                        continue;
                    }
                    t1 = t;
                    ignoreEmptyLines = true;
                    canBeOrgs = false;
                    continue;
                }
                if (_isStartOfBody(t, false)) 
                    break;
                if (t.IsValue("ПРОЕКТ", null) && t.IsNewlineAfter) 
                    continue;
                if (doc.Typ == null) 
                {
                    Pullenti.Ner.Token ttt1 = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
                    if (ttt1 != null && ttt1.IsNewlineAfter) 
                    {
                        string typ = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, ttt1, Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
                        if (doc.Typ == null) 
                            doc.Typ = typ;
                        title.Children.Add(new FragToken(t, ttt1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Typ, Value = typ });
                        dt0 = new Pullenti.Ner.Decree.Internal.DecreeToken(t, ttt1) { Typ = Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ, Value = typ };
                        canBeOrgs = false;
                        t1 = (t = ttt1);
                        continue;
                    }
                    if (t.IsNewlineBefore && ttt1 != null && Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false) == null) 
                    {
                        startOfName = true;
                        break;
                    }
                }
                FragToken appr = _createApproved(t);
                if (appr != null) 
                {
                    t = (t1 = appr.EndToken);
                    title.Children.Add(appr);
                    if (appr.BeginChar < title.BeginChar) 
                        title.BeginToken = appr.BeginToken;
                    continue;
                }
                FragToken edss = _createEditions(t);
                if (edss != null) 
                    break;
                FragToken misc = _createMisc(t);
                if (misc != null) 
                {
                    t = (t1 = misc.EndToken);
                    title.Children.Add(misc);
                    continue;
                }
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, dt0, false);
                if (dt != null) 
                {
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                    {
                        if (dt.LengthChar < 4) 
                            dt = null;
                    }
                }
                if (dt == null && dt0 != null && ((dt0.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dt0.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org))) 
                {
                    if ((t is Pullenti.Ner.NumberToken) && t.IsNewlineAfter && t.IsNewlineBefore) 
                        dt = new Pullenti.Ner.Decree.Internal.DecreeToken(t, t) { Typ = Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number, Value = (t as Pullenti.Ner.NumberToken).Value.ToString() };
                }
                if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Unknown) 
                    dt = null;
                if ((dt == null && (t is Pullenti.Ner.NumberToken) && t.IsNewlineBefore) && t.IsNewlineAfter) 
                {
                    if (dt0 != null && dt0.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org && ((t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit)) 
                        dt = new Pullenti.Ner.Decree.Internal.DecreeToken(t, t) { Typ = Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number, Value = (t as Pullenti.Ner.NumberToken).Value.ToString() };
                }
                if (dt != null && ((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org))) 
                {
                    if (!t.IsNewlineBefore && !t.Previous.IsTableControlChar) 
                        dt = null;
                    else 
                        for (Pullenti.Ner.Token ttt = dt.EndToken.Next; ttt != null; ttt = ttt.Next) 
                        {
                            if (ttt.IsNewlineBefore || ttt.IsTableControlChar) 
                                break;
                            else if ((ttt is Pullenti.Ner.TextToken) && (ttt as Pullenti.Ner.TextToken).IsPureVerb) 
                            {
                                dt = null;
                                break;
                            }
                        }
                }
                if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date && dt0 != null) 
                {
                    if (dt.IsNewlineBefore || dt.IsNewlineAfter) 
                    {
                    }
                    else if (dt0.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dt0.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                    {
                    }
                    else 
                        dt = null;
                }
                if (dt == null) 
                {
                    if (t.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                        continue;
                    if (t.IsValue("ДАТА", null)) 
                    {
                        bool ok = false;
                        for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                        {
                            if ((tt.IsValue("ПОДПИСАНИЕ", "ПІДПИСАННЯ") || tt.IsValue("ВВЕДЕНИЕ", "ВВЕДЕННЯ") || tt.IsValue("ПРИНЯТИЕ", "ПРИЙНЯТТЯ")) || tt.IsValue("ДЕЙСТВИЕ", "ДІЮ") || tt.Morph.Class.IsPreposition) 
                                continue;
                            if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsLetter) 
                                continue;
                            Pullenti.Ner.Date.DateReferent da = tt.GetReferent() as Pullenti.Ner.Date.DateReferent;
                            if (da != null) 
                            {
                                FragToken frdt = new FragToken(t, tt) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Date };
                                title.Children.Add(frdt);
                                t = tt;
                                ok = true;
                                if (doc.Date == null) 
                                    doc.AddDate(da);
                            }
                            break;
                        }
                        if (ok) 
                            continue;
                    }
                    Pullenti.Ner.Referent r = t.GetReferent();
                    if ((r == null && t.LengthChar == 1 && !t.Chars.IsLetter) && (t.Next is Pullenti.Ner.ReferentToken) && !t.IsNewlineAfter) 
                    {
                        t = t.Next;
                        r = t.GetReferent();
                    }
                    if (((r is Pullenti.Ner.Address.AddressReferent) || (r is Pullenti.Ner.Uri.UriReferent) || (r is Pullenti.Ner.Phone.PhoneReferent)) || (r is Pullenti.Ner.Person.PersonIdentityReferent) || (r is Pullenti.Ner.Bank.BankDataReferent)) 
                    {
                        FragToken cnt = new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Contact };
                        cnt.Referents = new List<Pullenti.Ner.Referent>();
                        cnt.Referents.Add(r);
                        title.Children.Add(cnt);
                        for (; t != null; t = t.Next) 
                        {
                            if (t.Next != null && t.Next.IsCharOf(",;.")) 
                                t = t.Next;
                            if (t.Next == null) 
                                break;
                            r = t.Next.GetReferent();
                            if (((r is Pullenti.Ner.Address.AddressReferent) || (r is Pullenti.Ner.Uri.UriReferent) || (r is Pullenti.Ner.Phone.PhoneReferent)) || (r is Pullenti.Ner.Person.PersonIdentityReferent) || (r is Pullenti.Ner.Bank.BankDataReferent)) 
                            {
                                cnt.Referents.Add(r);
                                cnt.EndToken = t.Next;
                            }
                            else if (t.IsNewlineAfter) 
                                break;
                        }
                        continue;
                    }
                    Pullenti.Ner.Decree.Internal.PartToken pt = (t.IsNewlineBefore ? Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, false) : null);
                    if ((pt != null && ((pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part || pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart)) && pt.Values.Count == 1) && pt.IsNewlineAfter) 
                    {
                        bool ok = false;
                        if (dt0 != null && dt0.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                            ok = true;
                        else 
                        {
                            Pullenti.Ner.Decree.Internal.DecreeToken ddd = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(pt.EndToken.Next, null, false);
                            if (ddd != null && ddd.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                                ok = true;
                            else if (_createApproved(pt.EndToken.Next) != null) 
                                ok = true;
                        }
                        if (ok) 
                        {
                            title.Children.Add(new FragToken(pt.BeginToken, pt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocPart, Value = pt.Values[0].Value });
                            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PART, pt.Values[0].Value, false, 0);
                            t = pt.EndToken;
                            continue;
                        }
                    }
                    if (appr0 != null) 
                        break;
                    if (canBeOrgs) 
                    {
                        if (t.GetReferent() is Pullenti.Ner.Person.PersonReferent) 
                        {
                        }
                        else 
                        {
                            FragToken org = _createOwner(t);
                            if (org != null) 
                            {
                                unknownOrgs.Add(org);
                                t1 = (t = org.EndToken);
                                continue;
                            }
                        }
                    }
                    InstrToken stok = InstrToken.Parse(t, 0, null);
                    if (stok != null && ((stok.NoWords || (stok.LengthChar < 5)))) 
                    {
                        if (t0 == t) 
                            t0 = stok.EndToken.Next;
                        t = stok.EndToken;
                        continue;
                    }
                    if ((t.IsNewlineBefore && doc.Typ != null && (t is Pullenti.Ner.TextToken)) && Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null) != null) 
                        break;
                    if (t.IsNewlineBefore && t.IsValue("К", "ДО")) 
                        break;
                    if (((!ignoreEmptyLines && stok != null && stok.Typ == ILTypes.Undefined) && !stok.HasVerb && ((dt0 == null || dt0.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number))) && (emptyLines < 3)) 
                    {
                        if (stok.IsNewlineAfter) 
                            emptyLines++;
                        else if (dt0 != null) 
                            break;
                        t = (endEmptyLines = stok.EndToken);
                        continue;
                    }
                    break;
                }
                if ((!ignoreEmptyLines && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr && endEmptyLines != null) && dt0 == null) 
                {
                    if (dt.IsNewlineAfter) 
                        emptyLines++;
                    t = (endEmptyLines = dt.EndToken);
                    continue;
                }
                if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                {
                    if (isContract) 
                        break;
                    for (Pullenti.Ner.Token ttt = dt.EndToken.Next; ttt != null; ttt = ttt.Next) 
                    {
                        if (ttt.WhitespacesBeforeCount > 15) 
                            break;
                        if (ttt.GetMorphClassInDictionary() == Pullenti.Morph.MorphClass.Verb) 
                        {
                            dt = null;
                            break;
                        }
                        Pullenti.Ner.Decree.Internal.DecreeToken dt1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttt, dt0, false);
                        if (dt1 != null) 
                        {
                            if ((dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ || dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) || dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dt1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                                break;
                            dt.EndToken = dt1.EndToken;
                        }
                        else if (ttt.Chars != dt.BeginToken.Chars && ttt.IsNewlineBefore) 
                            break;
                        else 
                            dt.EndToken = ttt;
                    }
                    if (dt == null) 
                        break;
                }
                if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    Pullenti.Ner.Decree.DecreeKind typ = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dt.Value);
                    if (typ == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                    {
                        for (; t != null; t = t.Next) 
                        {
                            if (t.IsNewlineAfter) 
                                break;
                        }
                        if (t == null) 
                            break;
                        continue;
                    }
                    if (typ == Pullenti.Ner.Decree.DecreeKind.Contract || dt.Value == "ДОВЕРЕННОСТЬ" || dt.Value == "ДОВІРЕНІСТЬ") 
                        isContract = true;
                    else if (dt.Value == "ПРОТОКОЛ" && !dt.IsNewlineAfter) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(dt.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt1 != null) 
                        {
                            for (t = dt.EndToken.Next; t != null; t = t.Next) 
                            {
                                dt.EndToken = t;
                                if (t.IsNewlineAfter) 
                                    break;
                            }
                        }
                    }
                    canBeOrgs = false;
                }
                dt0 = dt;
                if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && unknownOrgs.Count > 0) 
                {
                    foreach (FragToken org in unknownOrgs) 
                    {
                        title.Children.Add(org);
                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SOURCE, org.Value, false, 0);
                    }
                    unknownOrgs.Clear();
                }
                if (!_addTitleAttr(doc, title, dt)) 
                    break;
                else 
                    attrs++;
                t1 = (t = dt.EndToken);
            }
            title.SortChildren();
            if (t == null || (((doc.Typ == null && doc.RegNumber == null && appr0 == null) && !startOfName))) 
            {
                if (t == t0) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeToken nam = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(t0, null, true, false);
                    if (nam != null) 
                    {
                        name = FragToken.GetRestoredName(t0, nam.EndToken, false);
                        if (!string.IsNullOrEmpty(name)) 
                        {
                            t1 = nam.EndToken;
                            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, name.Trim(), true, 0);
                            title.Children.Add(new FragToken(t0, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = name.Trim() });
                            for (; t1.Next != null; t1 = t1.Next) 
                            {
                                if (t1.IsTableControlChar && !t1.IsChar((char)0x1F)) 
                                {
                                }
                                else 
                                    break;
                            }
                            title.EndToken = t1;
                            for (t = t1.Next; t != null; t = t.Next) 
                            {
                                if (_isStartOfBody(t, false)) 
                                    break;
                                if (t.IsTableControlChar) 
                                    continue;
                                FragToken appr1 = _createApproved(t);
                                if (appr1 != null) 
                                {
                                    title.Children.Add(appr1);
                                    t = (title.EndToken = appr1.EndToken);
                                    continue;
                                }
                                FragToken eds = _createEditions(t);
                                if (eds != null) 
                                {
                                    title.Children.Add(eds);
                                    t = (title.EndToken = eds.EndToken);
                                    break;
                                }
                                appr1 = _createMisc(t);
                                if (appr1 != null) 
                                {
                                    title.Children.Add(appr1);
                                    t = (title.EndToken = appr1.EndToken);
                                    continue;
                                }
                                Pullenti.Ner.Decree.Internal.DecreeToken dt00 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                                if (dt00 != null) 
                                {
                                    if (dt00.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dt00.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                                    {
                                        _addTitleAttr(doc, title, dt00);
                                        t = (title.EndToken = dt00.EndToken);
                                        continue;
                                    }
                                }
                                break;
                            }
                            return title;
                        }
                    }
                }
                if (t != null && t.IsValue("О", null)) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeToken nam = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(t, null, true, false);
                    if (nam != null) 
                    {
                        name = FragToken.GetRestoredName(t, nam.EndToken, false);
                        if (!string.IsNullOrEmpty(name)) 
                        {
                            t1 = nam.EndToken;
                            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, name.Trim(), true, 0);
                            title.Children.Add(new FragToken(t, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = name.Trim() });
                        }
                    }
                }
                if (attrs > 0) 
                {
                    title.EndToken = t1;
                    return title;
                }
                return null;
            }
            for (int j = 0; j < unknownOrgs.Count; j++) 
            {
                title.Children.Insert(j, unknownOrgs[j]);
                doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SOURCE, unknownOrgs[j].Value, false, 0);
            }
            if (endEmptyLines != null && doc.FindSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SOURCE, null, true) == null) 
            {
                string val = Pullenti.Ner.Core.MiscHelper.GetTextValue(t0, endEmptyLines, Pullenti.Ner.Core.GetTextAttr.No);
                doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SOURCE, val, false, 0);
                title.Children.Insert(0, new FragToken(t0, endEmptyLines) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Organization, Value = val });
            }
            bool isCase = false;
            foreach (FragToken ch in title.Children) 
            {
                if (ch.Value == null && ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Approved && ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                    ch.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(ch.BeginToken, ch.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.CaseNumber) 
                    isCase = true;
            }
            if ((((name != null || t.IsNewlineBefore || ((t.Previous != null && t.Previous.IsTableControlChar))) || ((!t.IsNewlineBefore && title.Children.Count > 0 && title.Children[title.Children.Count - 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Typ)))) && !isCase) 
            {
                Pullenti.Ner.Token tt0 = t;
                InstrToken1 firstLine = null;
                bool poDelu = false;
                if (t.IsValue("ПО", null) && t.Next != null && t.Next.IsValue("ДЕЛО", "СПРАВА")) 
                    poDelu = true;
                for (; t != null; t = t.Next) 
                {
                    if (_isStartOfBody(t, false)) 
                        break;
                    if ((name != null && t == tt0 && t.IsNewlineBefore) && t.WhitespacesBeforeCount > 15) 
                        break;
                    if (t.IsTableControlChar) 
                        break;
                    if (t.IsNewlineBefore) 
                    {
                        Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, false);
                        if (pt != null && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix) 
                            break;
                        InstrToken1 ltt = InstrToken1.Parse(t, false, null, 0, null, false, 0, true, false);
                        if (ltt == null) 
                            break;
                        if (t != tt0 && t.WhitespacesBeforeCount > 15) 
                        {
                            if (t.NewlinesBeforeCount > 2) 
                                break;
                            if (t.NewlinesBeforeCount > 1 && !t.Chars.IsAllUpper) 
                                break;
                            if (t.IsValue("О", "ПРО") || t.IsValue("ОБ", null)) 
                            {
                            }
                            else if (ltt.AllUpper && !ltt.HasChanges) 
                            {
                            }
                            else 
                                break;
                        }
                        if (ltt.Numbers.Count > 0) 
                            break;
                        FragToken appr = _createApproved(t);
                        if (appr != null) 
                        {
                            if (t.Previous != null && t.Previous.IsChar(',')) 
                            {
                            }
                            else 
                                break;
                        }
                        if (_createEditions(t) != null || _createCaseInfo(t) != null) 
                            break;
                        if (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                        {
                            if (t.IsNewlineAfter) 
                                break;
                            if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Date.DateReferent)) 
                                break;
                        }
                        if (t.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                        {
                            if (t.IsNewlineAfter) 
                                break;
                            if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                                break;
                        }
                        if (t.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) 
                            break;
                        if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                        {
                            Pullenti.Ner.Decree.DecreeReferent dr = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                            if (dr.Kind == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                                break;
                        }
                        if (t.IsChar('(')) 
                        {
                            if (_createEditions(t) != null) 
                                break;
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br != null && !br.IsNewlineAfter) 
                            {
                            }
                            else 
                                break;
                        }
                        if (ltt.HasVerb && !ltt.AllUpper) 
                        {
                            if (t.IsValue("О", "ПРО") && tt0 == t) 
                            {
                            }
                            else if (!poDelu) 
                                break;
                        }
                        if (ltt.Typ == InstrToken1.Types.Directive) 
                            break;
                        string str = ltt.ToString();
                        if (t.Previous != null && t.Previous.IsValue("ИЗМЕНЕНИЕ", null)) 
                        {
                        }
                        else if (str.Contains("В СОСТАВЕ") || str.Contains("В СКЛАДІ") || str.Contains("У СКЛАДІ")) 
                            break;
                        if (t.IsValue("В", null) && t.Next != null && t.Next.IsValue("ЦЕЛЬ", "МЕТА")) 
                            break;
                        if (firstLine == null) 
                            firstLine = ltt;
                        else if (firstLine.AllUpper && !ltt.AllUpper && !Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, false, false)) 
                            break;
                        t1 = (t = ltt.EndToken);
                        if (t1.IsTableControlChar) 
                        {
                            t1 = (t = t1.Previous);
                            break;
                        }
                    }
                    else 
                        t1 = t;
                }
                Pullenti.Ner.Token tt1 = Pullenti.Ner.Decree.Internal.DecreeToken._tryAttachStdChangeName(tt0);
                if (tt1 != null) 
                {
                    if (t1 == null || (t1.EndChar < tt1.EndChar)) 
                        t1 = tt1;
                }
                string val = (t1 != null && t1 != tt0 ? FragToken.GetRestoredName(tt0, t1, false) : null);
                if (!string.IsNullOrEmpty(val) && char.IsLetter(val[0]) && char.IsLower(val[0])) 
                    val = char.ToUpper(val[0]) + val.Substring(1);
                if (name == null && title.Children.Count > 0 && title.Children[title.Children.Count - 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Typ) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt0, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null) 
                    {
                        if (npt.Morph.Case.IsGenitive) 
                        {
                            name = title.Children[title.Children.Count - 1].Value as string;
                            if (Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(title.Children[title.Children.Count - 1].BeginToken, null, false) == null) 
                            {
                                tt0 = title.Children[title.Children.Count - 1].BeginToken;
                                title.Children.RemoveAt(title.Children.Count - 1);
                            }
                        }
                    }
                }
                if (val == null) 
                    val = name;
                else if (name != null) 
                    val = string.Format("{0} {1}", name, val);
                if (val != null) 
                {
                    if (nT0 != null) 
                        tt0 = nT0;
                    val = val.Trim();
                    if (val.StartsWith("[") && val.EndsWith("]")) 
                        val = val.Substring(1, val.Length - 2).Trim();
                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, val.Trim(), true, 0);
                    title.Children.Add(new FragToken(tt0, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = val.Trim() });
                    if (val.Contains("КОДЕКС")) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt0, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.Noun.IsValue("КОДЕКС", null)) 
                            doc.Typ = "КОДЕКС";
                    }
                }
            }
            if (t1 == null) 
                return null;
            title.EndToken = t1;
            for (t1 = t1.Next; t1 != null; t1 = t1.Next) 
            {
                if (t1.IsNewlineBefore && (t1.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && t1.IsNewlineAfter) 
                {
                    Pullenti.Ner.Decree.DecreeReferent dr = t1.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                    title.Children.Add(new FragToken(t1, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Ignored });
                    continue;
                }
                if (t1.IsNewlineBefore && t1.IsValue("ЧАСТЬ", "ЧАСТИНА")) 
                {
                    Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t1, null, false, false);
                    if (pt != null && pt.IsNewlineAfter) 
                    {
                        Pullenti.Ner.Decree.Internal.PartToken pt2 = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(pt.EndToken.Next, null, false, false);
                        if (pt2 != null && (((pt2.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Section || pt2.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubSection || pt2.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Chapter) || pt2.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause))) 
                        {
                        }
                        else 
                        {
                            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PART, pt.Values[0].Value, false, 0);
                            title.Children.Add(new FragToken(t1, pt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocPart, Value = pt.Values[0].Value });
                            t1 = (title.EndToken = pt.EndToken);
                            continue;
                        }
                    }
                }
                if (t1.IsNewlineBefore) 
                {
                    InstrToken1 iii = InstrToken1.Parse(t1, true, null, 0, null, false, 0, false, false);
                    if (iii != null && iii.Typ == InstrToken1.Types.Comment) 
                    {
                        title.Children.Add(new FragToken(t1, iii.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Comment });
                        t1 = iii.EndToken;
                        continue;
                    }
                }
                FragToken appr1 = _createApproved(t1);
                if (appr1 != null) 
                {
                    t1 = appr1.EndToken;
                    title.Children.Add(appr1);
                    title.EndToken = appr1.EndToken;
                    continue;
                }
                FragToken cinf = _createCaseInfo(t1);
                if (cinf != null) 
                {
                    t1 = cinf.EndToken;
                    title.Children.Add(cinf);
                    title.EndToken = cinf.EndToken;
                    continue;
                }
                FragToken eds = _createEditions(t1);
                if (eds != null) 
                {
                    title.Children.Add(eds);
                    title.EndToken = (t1 = eds.EndToken);
                    continue;
                }
                appr1 = _createMisc(t1);
                if (appr1 != null) 
                {
                    t1 = appr1.EndToken;
                    title.Children.Add(appr1);
                    title.EndToken = appr1.EndToken;
                    continue;
                }
                if ((t1.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && (t1.GetReferent() as Pullenti.Ner.Decree.DecreeReferent).Kind == Pullenti.Ner.Decree.DecreeKind.Publisher && t1.IsNewlineAfter) 
                {
                    FragToken pub = new FragToken(t1, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
                    pub.Referents = new List<Pullenti.Ner.Referent>();
                    pub.Referents.Add(t1.GetReferent());
                    title.Children.Add(pub);
                    title.EndToken = t1;
                    continue;
                }
                Pullenti.Ner.Token tt = t1;
                if (tt.Next != null && tt.IsChar(',')) 
                    tt = tt.Next;
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                if (dt != null && ((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr || ((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && ((dt.IsDelo || Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tt) != null))))))) 
                {
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                    {
                        if (doc.Date != null) 
                            break;
                        if (!dt.IsNewlineAfter && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(dt.EndToken.Next)) 
                        {
                            Pullenti.Ner.Token ttt = dt.EndToken.Next;
                            if (ttt != null && (((ttt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) || ttt.IsComma))) 
                            {
                            }
                            else 
                                break;
                        }
                    }
                    if (!dt.IsNewlineAfter) 
                    {
                        InstrToken1 lll = InstrToken1.Parse(tt, true, null, 0, null, false, 0, false, false);
                        if (lll != null && lll.HasVerb) 
                            break;
                    }
                    _addTitleAttr(doc, title, dt);
                    t1 = (title.EndToken = dt.EndToken);
                    continue;
                }
                if (tt.IsCharOf("([") && tt.IsNewlineBefore) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines | Pullenti.Ner.Core.BracketParseAttr.CanContainsVerbs, 100);
                    if (br != null) 
                    {
                        t1 = (title.EndToken = br.EndToken);
                        title.Children.Add(new FragToken(br.BeginToken, br.EndToken) { Kind = (tt.IsChar('[') ? Pullenti.Ner.Instrument.InstrumentKind.Name : Pullenti.Ner.Instrument.InstrumentKind.Comment) });
                        continue;
                    }
                }
                if (tt.IsTableControlChar) 
                {
                    title.EndToken = tt;
                    continue;
                }
                break;
            }
            t1 = title.EndToken.Next;
            if (t1 != null && t1.IsNewlineBefore && doc.Typ == "КОДЕКС") 
            {
                Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t1, null, false, false);
                if (pt != null && ((pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part || pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart)) && pt.Values.Count > 0) 
                {
                    int cou = 0;
                    for (t = pt.EndToken; t != null; t = t.Next) 
                    {
                        if (t.IsNewlineBefore) 
                        {
                            if ((++cou) > 4) 
                                break;
                            FragToken eds = _createEditions(t);
                            if (eds != null) 
                            {
                                title.Children.Add(eds);
                                title.EndToken = (t1 = eds.EndToken);
                                title.Children.Add(new FragToken(pt.BeginToken, pt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocPart, Value = pt.Values[0].Value });
                                if (doc.Name != null && doc.Name.Contains("КОДЕКС")) 
                                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PART, pt.Values[0].Value, false, 0);
                                break;
                            }
                        }
                    }
                }
                else if (t1.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) 
                {
                    Pullenti.Ner.Decree.DecreePartReferent dr0 = t1.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                    if (dr0.Part != null || dr0.DocPart != null) 
                    {
                        int cou = 0;
                        for (t = t1.Next; t != null; t = t.Next) 
                        {
                            if (t.IsNewlineBefore) 
                            {
                                if ((++cou) > 4) 
                                    break;
                                FragToken eds = _createEditions(t);
                                if (eds != null) 
                                {
                                    title.Children.Add(eds);
                                    title.EndToken = (t1 = eds.EndToken);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return title;
        }
        static FragToken CreateAppendixTitle(Pullenti.Ner.Token t0, FragToken app, Pullenti.Ner.Instrument.InstrumentReferent doc, bool isApp, bool start)
        {
            if (t0 == null) 
                return null;
            if (t0 != t0.Kit.FirstToken) 
            {
                if (t0.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) 
                {
                    if ((t0.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent).Appendix != null) 
                        t0 = t0.Kit.DebedToken(t0);
                }
            }
            Pullenti.Ner.Token t = t0;
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Referent rr = t.GetReferent();
            if (rr != null) 
            {
                if (rr.TypeName == "PERSON") 
                    return null;
            }
            FragToken title = new FragToken(t0, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Head };
            bool hasAppKeyword = false;
            FragToken appr0 = _createApproved(t0);
            if (appr0 != null) 
            {
                title.EndToken = appr0.EndToken;
                title.Children.Add(appr0);
                t = appr0.EndToken.Next;
            }
            for (; t != null; t = t.Next) 
            {
                InstrToken1 fr = InstrToken1.Parse(t, true, null, 0, null, false, 0, true, false);
                if (fr == null) 
                    break;
                if (fr.Typ != InstrToken1.Types.Appendix && fr.Typ != InstrToken1.Types.Approved) 
                {
                    if (fr.HasManySpecChars) 
                    {
                        t = fr.EndToken;
                        continue;
                    }
                    if (t.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) 
                    {
                        t = fr.EndToken;
                        continue;
                    }
                    if ((t.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) && (t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent).Appendix != null) 
                    {
                        t = t.Kit.DebedToken(t);
                        fr = InstrToken1.Parse(t, true, null, 0, null, false, 0, true, false);
                        if (fr.Typ != InstrToken1.Types.Appendix) 
                            break;
                    }
                    else 
                        break;
                }
                if (fr.Typ == InstrToken1.Types.Appendix) 
                {
                    hasAppKeyword = true;
                    app.Kind = Pullenti.Ner.Instrument.InstrumentKind.Appendix;
                }
                Pullenti.Ner.Token t2 = t;
                if (t.IsValue("ОСОБЫЙ", "ОСОБЛИВИЙ") && t.Next != null) 
                    t2 = t.Next;
                if (t is Pullenti.Ner.TextToken) 
                    title.Children.Add(new FragToken(t, t2) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, DefVal2 = true });
                title.EndToken = (t = fr.EndToken);
                if (fr.Typ == InstrToken1.Types.Appendix && fr.NumBeginToken == null) 
                {
                    InstrToken1 fr1 = InstrToken1.Parse(t.Next, true, null, 0, null, false, 0, false, false);
                    if (fr1 != null && fr1.Typ == InstrToken1.Types.Approved) 
                    {
                        t = fr1.BeginToken;
                        title.Children.Add(new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, Value = t.GetSourceText().ToUpper() });
                        title.EndToken = (t = fr1.EndToken);
                        fr = fr1;
                    }
                }
                appr0 = _createApproved(t);
                if (appr0 != null) 
                {
                    t = (title.EndToken = appr0.EndToken);
                    title.Children.Add(appr0);
                    continue;
                }
                if (fr.NumBeginToken != null && fr.NumEndToken != null) 
                {
                    FragToken num = new FragToken(fr.NumBeginToken, fr.NumEndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Number, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(fr.NumBeginToken, fr.NumEndToken, Pullenti.Ner.Core.GetTextAttr.KeepRegister) };
                    title.Children.Add(num);
                    if (fr.Numbers.Count > 0) 
                        app.Number = Pullenti.Ner.Decree.Internal.PartToken.GetNumber(fr.Numbers[0]);
                    if (fr.Numbers.Count > 1) 
                    {
                        app.SubNumber = Pullenti.Ner.Decree.Internal.PartToken.GetNumber(fr.Numbers[1]);
                        if (fr.Numbers.Count > 2) 
                            app.SubNumber2 = Pullenti.Ner.Decree.Internal.PartToken.GetNumber(fr.Numbers[2]);
                    }
                    if (isApp) 
                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_APPENDIX, num.Value ?? "1", false, 0);
                }
                else if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                {
                    if ((t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent).Kind == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                    {
                        FragToken ff = new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
                        ff.Referents = new List<Pullenti.Ner.Referent>();
                        ff.Referents.Add(t.GetReferent());
                        title.Children.Add(ff);
                    }
                    else if (fr.Typ == InstrToken1.Types.Approved && title.Children.Count > 0 && title.Children[title.Children.Count - 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Keyword) 
                    {
                        FragToken kw = title.Children[title.Children.Count - 1];
                        FragToken appr = new FragToken(kw.BeginToken, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
                        title.Children.RemoveAt(title.Children.Count - 1);
                        appr.Children.Add(kw);
                        appr.Children.Add(new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocReference });
                        title.Children.Add(appr);
                    }
                    else 
                        title.Children.Add(new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocReference });
                }
                else if (fr.Typ == InstrToken1.Types.Approved && fr.LengthChar > 15 && fr.BeginToken != fr.EndToken) 
                    title.Children.Add(new FragToken(fr.BeginToken.Next, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocReference });
                else 
                {
                    List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t.Next, null, 10, false);
                    if (dts != null && dts.Count > 0 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                    {
                        FragToken dref = new FragToken(dts[0].BeginToken, dts[0].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocReference };
                        for (int i = 1; i < dts.Count; i++) 
                        {
                            if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                                break;
                            else if (dts[i].Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Unknown) 
                                dref.EndToken = dts[i].EndToken;
                        }
                        title.Children.Add(dref);
                        title.EndToken = (t = dref.EndToken);
                    }
                }
                if (fr.Typ == InstrToken1.Types.Appendix) 
                {
                    t = t.Next;
                    if (t != null) 
                    {
                        Pullenti.Ner.Decree.DecreePartReferent dpr = t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                        if (dpr != null && dpr.Appendix != null) 
                        {
                            t = t.Kit.DebedToken(t);
                            t = t.Previous;
                            continue;
                        }
                        if (t.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                        {
                            t = t.Previous;
                            continue;
                        }
                    }
                    break;
                }
            }
            if (t == null) 
                return null;
            bool hasForNpa = false;
            if (t.IsValue("К", "ДО")) 
            {
                hasForNpa = true;
                Pullenti.Ner.Decree.DecreeReferent toDecr = null;
                List<InstrToken> toks = new List<InstrToken>();
                for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                {
                    if (tt != t.Next && tt.IsTableControlChar) 
                        break;
                    if (tt.IsNewlineBefore) 
                    {
                        if (tt.NewlinesBeforeCount > 1) 
                            break;
                        InstrToken1 it1 = InstrToken1.Parse(tt, false, null, 0, null, false, 0, false, false);
                        if (it1 != null && it1.Numbers.Count > 0) 
                            break;
                        if (tt.Chars.IsAllLower) 
                        {
                        }
                        else if (tt.LengthChar > 2) 
                            break;
                    }
                    if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                        toDecr = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                    InstrToken tok = InstrToken.Parse(tt, 0, null);
                    if (tok == null) 
                        break;
                    toks.Add(tok);
                    if (toks.Count > 20) 
                        break;
                    if (tt == t.Next && tok.Typ == ILTypes.Undefined) 
                    {
                        Pullenti.Ner.Token ttt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, false);
                        if (ttt != null) 
                        {
                            tok.EndToken = ttt;
                            tok.Typ = ILTypes.Typ;
                        }
                    }
                    tt = tok.EndToken;
                    Pullenti.Ner.Decree.Internal.DecreeToken dtt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt.Next, null, false);
                    if (dtt != null && dtt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        tt = (tok.EndToken = dtt.EndToken);
                    if (tok.Typ == ILTypes.Typ && !tt.IsNewlineAfter) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeToken nn = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(tt.Next, null, false, true);
                        if (nn != null) 
                        {
                            tt = (tok.EndToken = nn.EndToken);
                            break;
                        }
                    }
                }
                int maxInd = -1;
                for (int ii = 0; ii < toks.Count; ii++) 
                {
                    InstrToken tok = toks[ii];
                    if (tok.Typ == ILTypes.Typ && ((tok.Value == doc.Typ || ii == 0))) 
                        maxInd = ii;
                    else if (tok.Typ == ILTypes.RegNumber && (((tok.Value == doc.RegNumber || tok.Value == "?" || tok.IsNewlineBefore) || tok.IsNewlineAfter || tok.HasTableChars))) 
                        maxInd = ii;
                    else if (tok.Typ == ILTypes.Date && doc.Date != null) 
                    {
                        if ((tok.Ref is Pullenti.Ner.Date.DateReferent) && (tok.Ref as Pullenti.Ner.Date.DateReferent).Dt == doc.Date) 
                            maxInd = ii;
                        else if (tok.Ref is Pullenti.Ner.ReferentToken) 
                        {
                            Pullenti.Ner.Date.DateReferent dre = (tok.Ref as Pullenti.Ner.ReferentToken).Referent as Pullenti.Ner.Date.DateReferent;
                            if (dre != null && dre.Dt != null && doc.Date != null) 
                            {
                                if (dre.Dt.Value == doc.Date.Value) 
                                    maxInd = ii;
                            }
                        }
                    }
                    else if (tok.Typ == ILTypes.Date && tok.BeginToken.Previous != null && tok.BeginToken.Previous.IsValue("ОТ", null)) 
                        maxInd = ii;
                    else if (tok.Typ == ILTypes.Undefined && (tok.BeginToken.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                    {
                        maxInd = ii;
                        break;
                    }
                    else if (ii == 0 && tok.Typ == ILTypes.Undefined && (tok.BeginToken.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)) 
                    {
                        Pullenti.Ner.Decree.DecreePartReferent part = tok.BeginToken.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                        if (part.Appendix != null) 
                        {
                            maxInd = ii;
                            break;
                        }
                    }
                    else if (tok.Typ == ILTypes.Organization && ii == 1) 
                        maxInd = ii;
                    else if (tok.Typ == ILTypes.Undefined) 
                    {
                        if (tok.BeginToken.IsValue("ОТ", null) || !tok.IsNewlineBefore) 
                            maxInd = ii;
                        else if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tok.BeginToken) != null) 
                            maxInd = ii;
                    }
                    else if (tok.Typ == ILTypes.Geo || tok.Typ == ILTypes.Organization) 
                        maxInd = ii;
                }
                if (toks.Count > 0 && Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(toks[toks.Count - 1].EndToken.Next, false) != null) 
                    maxInd = toks.Count - 1;
                Pullenti.Ner.Token te = null;
                if (maxInd >= 0) 
                {
                    te = toks[maxInd].EndToken;
                    if (!te.IsNewlineAfter) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeToken nn = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(te.Next, null, false, true);
                        if (nn != null) 
                            te = nn.EndToken;
                    }
                }
                else if (t.Next != null && (t.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                    te = t.Next;
                if (te != null) 
                {
                    FragToken dr = new FragToken(t, te) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocReference };
                    if (toDecr != null) 
                    {
                        dr.Referents = new List<Pullenti.Ner.Referent>();
                        dr.Referents.Add(toDecr);
                    }
                    title.Children.Add(dr);
                    title.EndToken = te;
                    if ((((t = te.Next))) == null) 
                        return title;
                }
            }
            if (title.Children.Count == 0) 
            {
                if (t != null && t.IsValue("АКТ", null)) 
                {
                }
                else 
                    return null;
            }
            for (int kk = 0; kk < 10; kk++) 
            {
                FragToken ta = _createApproved(t);
                if (ta != null) 
                {
                    title.Children.Add(ta);
                    title.EndToken = (t = ta.EndToken);
                    t = t.Next;
                    if (t == null) 
                        return title;
                    continue;
                }
                FragToken ee = _createEditions(t);
                if (ee != null) 
                {
                    title.Children.Add(ee);
                    title.EndToken = ee.EndToken;
                    t = ee.EndToken.Next;
                    if (t == null) 
                        return title;
                    continue;
                }
                ta = _createMisc(t);
                if (ta != null) 
                {
                    title.Children.Add(ta);
                    title.EndToken = (t = ta.EndToken);
                    t = t.Next;
                    if (t == null) 
                        return title;
                    continue;
                }
                break;
            }
            Pullenti.Ner.Token tt0 = t;
            if ((start && hasForNpa && hasAppKeyword) && tt0.IsNewlineBefore) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken dty = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt0, null, false);
                if (dty != null && dty.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    FragToken sub = FragToken.CreateDocument(tt0, 0, Pullenti.Ner.Instrument.InstrumentKind.Undefined);
                    if (sub != null && sub.Children.Count > 1 && sub.m_Doc.FindSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_APPENDIX, null, true) == null) 
                    {
                        if (sub.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Head && sub.Children[0].Children.Count > 1 && sub.Children[0].Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Typ) 
                        {
                            title.Tag = sub;
                            return title;
                        }
                    }
                }
            }
            Pullenti.Ner.Token nT0 = null;
            for (; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                {
                    if (t == tt0) 
                    {
                        if (t.IsChar((char)0x1E)) 
                        {
                            List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(t, 0, true);
                            if (rows != null && rows.Count > 2) 
                                break;
                            break;
                        }
                        tt0 = t.Next;
                        continue;
                    }
                    break;
                }
                if (t.IsNewlineBefore || t.Previous.IsTableControlChar) 
                {
                    if (_isStartOfBody(t, t == tt0)) 
                        break;
                    if (_createApproved(t) != null) 
                        break;
                    if (_createEditions(t) != null) 
                        break;
                    if (t != tt0 && t.WhitespacesBeforeCount > 15) 
                    {
                        if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t.Previous, false) == null) 
                        {
                            if (!t.Previous.IsValue("ОБРАЗЕЦ", "ЗРАЗОК")) 
                                break;
                        }
                        if (t.WhitespacesBeforeCount > 25) 
                            break;
                    }
                    if (t.GetReferent() is Pullenti.Ner.Instrument.InstrumentParticipantReferent) 
                        break;
                    if (t.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) 
                    {
                        if (t.WhitespacesBeforeCount > 15) 
                            break;
                    }
                    Pullenti.Ner.Decree.Internal.DecreeToken dd = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                    if (dd != null && ((dd.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dd.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr)) && dd.IsNewlineAfter) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt0 = null;
                        if (dd.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr && (t is Pullenti.Ner.ReferentToken)) 
                            npt0 = Pullenti.Ner.Core.NounPhraseHelper.TryParse((t as Pullenti.Ner.ReferentToken).BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt0 != null && !npt0.Morph.Case.IsUndefined && !npt0.Morph.Case.IsNominative) 
                        {
                        }
                        else 
                        {
                            _addTitleAttr(null, title, dd);
                            t = (title.EndToken = dd.EndToken);
                            continue;
                        }
                    }
                    InstrToken1 ltt = InstrToken1.Parse(t, true, null, 0, null, false, 0, true, false);
                    if (ltt == null) 
                        break;
                    if (ltt.Numbers.Count > 0) 
                        break;
                    if (ltt.Typ == InstrToken1.Types.Approved) 
                    {
                        title.Children.Add(new FragToken(ltt.BeginToken, ltt.BeginToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved });
                        if (ltt.BeginToken != ltt.EndToken) 
                            title.Children.Add(new FragToken(ltt.BeginToken.Next, ltt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocReference });
                        t = ltt.EndToken;
                        if (ltt.BeginToken == tt0) 
                        {
                            tt0 = t.Next;
                            continue;
                        }
                        break;
                    }
                    if (ltt.HasVerb && !ltt.AllUpper) 
                    {
                        if (t.Chars.IsLetter && t.Chars.IsAllLower) 
                        {
                        }
                        else if (t.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent) 
                        {
                            Pullenti.Ner.Decree.DecreeChangeReferent dch = t.GetReferent() as Pullenti.Ner.Decree.DecreeChangeReferent;
                            if (dch.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Container && t.IsValue("ИЗМЕНЕНИЕ", null)) 
                            {
                            }
                            else 
                                break;
                        }
                        else if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false) != null) 
                        {
                        }
                        else if ((t == tt0 && ltt.EndToken.Next != null && ltt.EndToken.Next.IsChar((char)0x1E)) && !ltt.EndToken.IsChar(':')) 
                        {
                        }
                        else 
                            break;
                    }
                    if (ltt.Typ == InstrToken1.Types.Directive) 
                        break;
                    if (t.Chars.IsLetter && t != tt0) 
                    {
                        if (!t.Chars.IsAllLower && !t.Chars.IsAllUpper) 
                        {
                            if (!(t.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) && !(t.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                            {
                                if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t.Previous, false) == null) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt != null && npt.Morph.Case.IsGenitive) 
                                    {
                                    }
                                    else 
                                        break;
                                }
                            }
                        }
                    }
                    bool hasWords = false;
                    for (Pullenti.Ner.Token ttt = ltt.BeginToken; ttt != null; ttt = ttt.Next) 
                    {
                        if (ttt.BeginChar > ltt.EndChar) 
                            break;
                        if (ttt.Chars.IsCyrillicLetter) 
                        {
                            hasWords = true;
                            break;
                        }
                        Pullenti.Ner.Referent r = ttt.GetReferent();
                        if ((r is Pullenti.Ner.Org.OrganizationReferent) || (r is Pullenti.Ner.Geo.GeoReferent) || (r is Pullenti.Ner.Decree.DecreeChangeReferent)) 
                        {
                            hasWords = true;
                            break;
                        }
                    }
                    if (!hasWords) 
                        break;
                    FragToken eds = _createEditions(t);
                    if (eds != null) 
                    {
                        if (t != tt0) 
                            break;
                        title.Children.Add(eds);
                        t1 = (t = (title.EndToken = eds.EndToken));
                        tt0 = t.Next;
                        continue;
                    }
                    t1 = (t = ltt.EndToken);
                }
                else 
                    t1 = t;
            }
            string val = (t1 != null && tt0 != null ? FragToken.GetRestoredName(tt0, t1, false) : null);
            if (val != null) 
            {
                if (nT0 != null) 
                    tt0 = nT0;
                title.Children.Add(new FragToken(tt0, t1) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = val.Trim() });
                title.EndToken = t1;
                title.Name = val;
            }
            while (title.EndToken.Next != null) 
            {
                FragToken eds = _createEditions(title.EndToken.Next);
                if (eds != null) 
                {
                    title.Children.Add(eds);
                    title.EndToken = eds.EndToken;
                    continue;
                }
                FragToken appr = _createApproved(title.EndToken.Next);
                if (appr != null) 
                {
                    title.Children.Add(appr);
                    title.EndToken = appr.EndToken;
                    continue;
                }
                break;
            }
            if (isApp) 
            {
                if (doc.FindSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_APPENDIX, null, true) == null) 
                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_APPENDIX, "", false, 0);
                foreach (FragToken ch in title.Children) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.DocReference) 
                    {
                        for (Pullenti.Ner.Token tt = ch.BeginToken; tt != null && tt.EndChar <= ch.EndChar; tt = tt.Next) 
                        {
                            if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                            {
                                foreach (Pullenti.Ner.Slot s in tt.GetReferent().Slots) 
                                {
                                    if (s.TypeName == Pullenti.Ner.Decree.DecreeReferent.ATTR_TYPE) 
                                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_TYPE, s.Value, false, 0);
                                    else if (s.TypeName == Pullenti.Ner.Decree.DecreeReferent.ATTR_NUMBER) 
                                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_REGNUMBER, s.Value, false, 0);
                                    else if (s.TypeName == Pullenti.Ner.Decree.DecreeReferent.ATTR_DATE) 
                                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_DATE, s.Value, false, 0);
                                    else if (s.TypeName == Pullenti.Ner.Decree.DecreeReferent.ATTR_SOURCE) 
                                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SOURCE, s.Value, false, 0);
                                    else if (s.TypeName == Pullenti.Ner.Decree.DecreeReferent.ATTR_GEO) 
                                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_GEO, s.Value, false, 0);
                                }
                                break;
                            }
                            Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                            if (dt != null) 
                            {
                                if (_addTitleAttr(doc, null, dt)) 
                                    tt = dt.EndToken;
                            }
                        }
                        break;
                    }
                }
            }
            if (title.Children.Count == 0 && title.EndToken == title.BeginToken) 
                return null;
            for (t1 = title.EndToken.Next; t1 != null; t1 = t1.Next) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t1, null, false);
                if (dt != null) 
                {
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                    {
                        _addTitleAttr(null, title, dt);
                        t1 = (title.EndToken = dt.EndToken);
                        continue;
                    }
                }
                break;
            }
            while (title.EndToken.Next != null) 
            {
                if (title.EndToken.Next.IsTableControlChar && ((!title.EndToken.Next.IsNewlineBefore || title.EndToken.Next.IsNewlineAfter || ((title.EndToken.Next.Next != null && title.EndToken.Next.Next.IsChar((char)0x1F)))))) 
                    title.EndToken = title.EndToken.Next;
                else 
                    break;
            }
            return title;
        }
        static bool _isStartOfBody(Pullenti.Ner.Token t, bool isAppTitle = false)
        {
            if (t == null || !t.IsNewlineBefore) 
                return false;
            if (!isAppTitle) 
            {
                Pullenti.Ner.Core.Internal.BlockTitleToken bl = Pullenti.Ner.Core.Internal.BlockTitleToken.TryAttach(t, false, null);
                if (bl != null) 
                {
                    if (bl.Typ != Pullenti.Ner.Core.Internal.BlkTyps.Undefined && bl.Typ != Pullenti.Ner.Core.Internal.BlkTyps.Literature) 
                        return true;
                }
            }
            Pullenti.Ner.Mail.Internal.MailLine li = Pullenti.Ner.Mail.Internal.MailLine.Parse(t, 0, 0);
            if (li != null) 
            {
                if (li.Typ == Pullenti.Ner.Mail.Internal.MailLine.Types.Hello) 
                    return true;
            }
            InstrToken1 it1 = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
            if (it1 != null) 
            {
                if (it1.Typ == InstrToken1.Types.Index) 
                    return true;
            }
            bool ok = false;
            if (t.IsValue("ВВЕДЕНИЕ", "ВВЕДЕННЯ") || t.IsValue("АННОТАЦИЯ", "АНОТАЦІЯ") || t.IsValue("ПРЕДИСЛОВИЕ", "ПЕРЕДМОВА")) 
                ok = true;
            else if (t.IsValue("ОБЩИЙ", "ЗАГАЛЬНИЙ") && t.Next != null && t.Next.IsValue("ПОЛОЖЕНИЕ", "ПОЛОЖЕННЯ")) 
            {
                t = t.Next;
                ok = true;
            }
            else if ((t.Next != null && t.Next.Chars.IsAllLower && t.Morph.Class.IsPreposition) && ((t.Next.IsValue("СВЯЗЬ", "ЗВЯЗОК") || t.Next.IsValue("ЦЕЛЬ", "МЕТА") || t.Next.IsValue("СООТВЕТСТВИЕ", "ВІДПОВІДНІСТЬ")))) 
                return true;
            if (ok) 
            {
                Pullenti.Ner.Token t1 = t.Next;
                if (t1 != null && t1.IsChar(':')) 
                    t1 = t1.Next;
                if (t1 == null || t1.IsNewlineBefore) 
                    return true;
                return false;
            }
            InstrToken1 it = InstrToken1.Parse(t, false, null, 0, null, false, 0, false, false);
            if (it != null) 
            {
                if (it.TypContainerRank > 0 || it.Typ == InstrToken1.Types.Directive) 
                {
                    if (t.IsValue("ЧАСТЬ", "ЧАСТИНА") && it.Numbers.Count == 1) 
                    {
                        if (_createApproved(it.EndToken.Next) != null) 
                            return false;
                    }
                    return true;
                }
                if (it.Numbers.Count > 0) 
                {
                    if (it.Numbers.Count > 1 || it.NumSuffix != null) 
                        return true;
                }
            }
            if ((t.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) && t.Next != null) 
            {
                if (t.Next.IsValue("СОСТАВ", "СКЛАД")) 
                    return true;
                if (t.Next.IsValue("В", "У") && t.Next.Next != null && t.Next.Next.IsValue("СОСТАВ", "СКЛАД")) 
                    return true;
            }
            return false;
        }
        static bool _addTitleAttr(Pullenti.Ner.Instrument.InstrumentReferent doc, FragToken title, Pullenti.Ner.Decree.Internal.DecreeToken dt)
        {
            if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
            {
                if (doc != null) 
                {
                    if (doc.Typ != null && dt.Value != doc.Typ) 
                    {
                        if (doc.Typ != "ПРОЕКТ") 
                            return false;
                        if (dt.Value.Contains("ЗАКОН")) 
                            doc.Typ = "ПРОЕКТ ЗАКОНА";
                        else 
                            return false;
                    }
                    else 
                        doc.Typ = dt.Value;
                    if (dt.FullValue != null && dt.FullValue != dt.Value && doc.Name == null) 
                        doc.Name = dt.FullValue;
                }
                if (title != null) 
                    title.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Typ, Value = dt.FullValue ?? dt.Value });
            }
            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
            {
                if (dt.IsDelo) 
                {
                    if (doc != null) 
                    {
                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_CASENUMBER, dt.Value, false, 0);
                        if (doc.RegNumber == dt.Value) 
                            doc.RegNumber = null;
                    }
                    if (title != null) 
                        title.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.CaseNumber, Value = dt.Value });
                }
                else 
                {
                    if (dt.Value != "?" && doc != null) 
                    {
                        if (doc.GetStringValue(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_CASENUMBER) == dt.Value) 
                        {
                        }
                        else 
                            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NUMBER, dt.Value, false, 0);
                    }
                    if (title != null) 
                        title.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Number, Value = dt.Value });
                    if (doc != null && doc.Typ == null && dt.Value != null) 
                    {
                        if (Pullenti.Morph.LanguageHelper.EndsWith(dt.Value, "ФКЗ")) 
                            doc.Typ = "ФЕДЕРАЛЬНЫЙ КОНСТИТУЦИОННЫЙ ЗАКОН";
                        else if (Pullenti.Morph.LanguageHelper.EndsWith(dt.Value, "ФЗ")) 
                            doc.Typ = "ФЕДЕРАЛЬНЫЙ ЗАКОН";
                    }
                }
            }
            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
            {
                if (doc != null) 
                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, dt.Value, false, 0);
                if (title != null) 
                    title.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = dt.Value });
            }
            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
            {
                if (doc == null || doc.AddDate(dt)) 
                {
                    if (title != null) 
                        title.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Date, Value = dt });
                }
            }
            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
            {
                if (title != null) 
                    title.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Place, Value = dt });
                if (doc != null && dt.Ref != null) 
                {
                    string geo = doc.GetStringValue(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_GEO);
                    if (geo == "Россия") 
                    {
                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_GEO, null, true, 0);
                        geo = null;
                    }
                    if (geo == null) 
                        doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_GEO, dt.Ref.Referent.ToString(), false, 0);
                }
            }
            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
            {
                if (title != null) 
                    title.Children.Add(new FragToken(dt.BeginToken, dt.EndToken) { Kind = (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org ? Pullenti.Ner.Instrument.InstrumentKind.Organization : Pullenti.Ner.Instrument.InstrumentKind.Initiator), Value = dt });
                if (doc != null) 
                {
                    if (dt.Ref != null) 
                    {
                        doc.AddSlot(Pullenti.Ner.Decree.DecreeReferent.ATTR_SOURCE, dt.Ref.Referent, false, 0).Tag = dt.GetSourceText();
                        if (dt.Ref.Referent is Pullenti.Ner.Person.PersonPropertyReferent) 
                            doc.AddExtReferent(dt.Ref);
                    }
                    else 
                        doc.AddSlot(Pullenti.Ner.Decree.DecreeReferent.ATTR_SOURCE, Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(dt.Value), false, 0).Tag = dt.GetSourceText();
                }
            }
            else 
                return false;
            return true;
        }
        static FragToken CreateZapiskaTitle(Pullenti.Ner.Token t0, Pullenti.Ner.Instrument.InstrumentReferent doc)
        {
            int cou = 0;
            for (Pullenti.Ner.Token t = t0; t != null && (cou < 30); t = t.Next,cou++) 
            {
                InstrToken1 li = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                if (li == null) 
                    break;
                if (li.Numbers.Count > 0) 
                    break;
                bool ok = false;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.EndToken == li.EndToken) 
                {
                    foreach (string kv in m_ZapiskaKeywords) 
                    {
                        if (npt.EndToken.IsValue(kv, null)) 
                        {
                            ok = true;
                            break;
                        }
                    }
                }
                if (t.IsValue("ОТВЕТ", null)) 
                {
                    if (t.IsNewlineAfter) 
                        ok = true;
                    else if (t.Next != null && t.Next.IsValue("НА", null)) 
                        ok = true;
                }
                if (ok) 
                {
                    FragToken res = new FragToken(t0, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Head };
                    if (li.BeginToken != t0) 
                    {
                        FragToken hh = new FragToken(t0, li.BeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved };
                        res.Children.Add(hh);
                    }
                    res.Children.Add(new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, DefVal = true });
                    return res;
                }
                t = li.EndToken;
            }
            return null;
        }
        static string[] m_ZapiskaKeywords = new string[] {"ЗАЯВЛЕНИЕ", "ЗАПИСКА", "РАПОРТ", "ДОКЛАД", "ОТЧЕТ"};
        static FragToken CreateContractTitle(Pullenti.Ner.Token t0, Pullenti.Ner.Instrument.InstrumentReferent doc)
        {
            if (t0 == null) 
                return null;
            bool isContract = false;
            while ((t0 is Pullenti.Ner.TextToken) && t0.Next != null) 
            {
                if (t0.IsTableControlChar || !t0.Chars.IsLetter) 
                    t0 = t0.Next;
                else 
                    break;
            }
            Pullenti.Ner.Decree.Internal.DecreeToken dt0 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t0, null, false);
            if (dt0 != null && dt0.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                isContract = (dt0.Value.Contains("ДОГОВОР") || dt0.Value.Contains("ДОГОВІР") || dt0.Value.Contains("КОНТРАКТ")) || dt0.Value.Contains("СОГЛАШЕНИЕ") || dt0.Value.Contains("УГОДА");
            int cou = 0;
            Pullenti.Ner.Token t;
            ParticipantToken par1 = null;
            for (t = t0; t != null; t = t.Next) 
            {
                if (t is Pullenti.Ner.ReferentToken) 
                {
                    Pullenti.Ner.ReferentToken rtt = t as Pullenti.Ner.ReferentToken;
                    if (rtt.BeginToken == rtt.EndToken) 
                    {
                        Pullenti.Ner.Referent r = t.GetReferent();
                        if (r is Pullenti.Ner.Person.PersonPropertyReferent) 
                        {
                            string str = r.ToString();
                            if (str.Contains("директор") || str.Contains("начальник")) 
                            {
                            }
                            else 
                                t = t.Kit.DebedToken(t);
                        }
                        else if ((r is Pullenti.Ner.Person.PersonReferent) && (rtt.BeginToken.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
                        {
                            string str = rtt.BeginToken.GetReferent().ToString();
                            if (str.Contains("директор") || str.Contains("начальник")) 
                            {
                            }
                            else 
                            {
                                t = t.Kit.DebedToken(t);
                                t = t.Kit.DebedToken(t);
                            }
                        }
                    }
                }
            }
            int newlines = 0;
            int types = 0;
            for (t = t0; t != null && (cou < 300); t = t.Next,cou++) 
            {
                if (t.IsChar('_')) 
                {
                    cou--;
                    continue;
                }
                if (t.IsNewlineBefore) 
                {
                    newlines++;
                    if (newlines > 10) 
                        break;
                    while (t.IsTableControlChar && t.Next != null) 
                    {
                        t = t.Next;
                    }
                    Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                    if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                    {
                        if (((((dt.Value == "ОПРЕДЕЛЕНИЕ" || dt.Value == "ПОСТАНОВЛЕНИЕ" || dt.Value == "РЕШЕНИЕ") || dt.Value == "ПРИГОВОР" || dt.Value == "ВИЗНАЧЕННЯ") || dt.Value == "ПОСТАНОВА" || dt.Value == "РІШЕННЯ") || dt.Value == "ВИРОК" || dt.Value.EndsWith("ЗАЯВЛЕНИЕ")) || dt.Value.EndsWith("ЗАЯВА")) 
                            return null;
                        types++;
                    }
                    if (t.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) 
                    {
                        Pullenti.Ner.Org.OrganizationKind ki = (t.GetReferent() as Pullenti.Ner.Org.OrganizationReferent).Kind;
                        if (ki == Pullenti.Ner.Org.OrganizationKind.Justice) 
                            return null;
                    }
                }
                if (t.IsValue("ДАЛЕЕ", null)) 
                {
                }
                if (t.IsNewlineAfter) 
                    continue;
                par1 = ParticipantToken.TryAttach(t, null, null, isContract);
                if (par1 != null && ((par1.Kind == ParticipantToken.Kinds.NamedAs || par1.Kind == ParticipantToken.Kinds.NamedAsParts))) 
                {
                    t = par1.EndToken.Next;
                    break;
                }
                par1 = null;
            }
            if (par1 == null) 
                return null;
            ParticipantToken par2 = null;
            cou = 0;
            for (; t != null && (cou < 100); t = t.Next,cou++) 
            {
                if (par1.Kind == ParticipantToken.Kinds.NamedAsParts) 
                    break;
                if (t.IsChar('_')) 
                {
                    cou--;
                    continue;
                }
                if (t.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br2 = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br2 != null) 
                    {
                        t = br2.EndToken;
                        continue;
                    }
                }
                if (t.IsAnd) 
                {
                }
                par2 = ParticipantToken.TryAttach(t, null, null, true);
                if (par2 != null) 
                {
                    if (par2.Kind == ParticipantToken.Kinds.NamedAs && par2.Typ != par1.Typ) 
                        break;
                    if (par2.Kind == ParticipantToken.Kinds.Pure && par2.Typ != par1.Typ) 
                    {
                        if (t.Previous.IsAnd) 
                            break;
                    }
                    t = par2.EndToken;
                }
                par2 = null;
            }
            if (par1 != null && par2 != null && ((par1.Typ == null || par2.Typ == null))) 
            {
                Dictionary<string, int> stat = new Dictionary<string, int>();
                for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
                {
                    Pullenti.Ner.Token ttt = tt;
                    if (tt is Pullenti.Ner.MetaToken) 
                        ttt = (tt as Pullenti.Ner.MetaToken).BeginToken;
                    Pullenti.Ner.Core.TerminToken tok = ParticipantToken.m_Ontology.TryParse(ttt, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok == null || tok.Termin.Tag == null) 
                        continue;
                    string key = tok.Termin.CanonicText;
                    if (key == par1.Typ || key == par2.Typ || key == "СТОРОНА") 
                        continue;
                    if (!stat.ContainsKey(key)) 
                        stat.Add(key, 1);
                    else 
                        stat[key]++;
                }
                int max = 0;
                string bestTyp = null;
                foreach (KeyValuePair<string, int> kp in stat) 
                {
                    if (kp.Value > max) 
                    {
                        max = kp.Value;
                        bestTyp = kp.Key;
                    }
                }
                if (bestTyp != null) 
                {
                    if (par1.Typ == null) 
                        par1.Typ = bestTyp;
                    else if (par2.Typ == null) 
                        par2.Typ = bestTyp;
                }
            }
            List<string> contrTyps = ParticipantToken.GetDocTypes(par1.Typ, (par2 == null ? null : par2.Typ));
            Pullenti.Ner.Token t1 = par1.BeginToken.Previous;
            Pullenti.Ner.Token lastT1 = null;
            for (; t1 != null && t1.BeginChar >= t0.BeginChar; t1 = t1.Previous) 
            {
                if (t1.IsNewlineAfter) 
                {
                    lastT1 = t1;
                    if (t1.IsChar(',')) 
                        continue;
                    if (t1.Next == null) 
                        break;
                    if (t1.Next.Chars.IsLetter && t1.Next.Chars.IsAllLower) 
                        continue;
                    break;
                }
            }
            if (t1 == null) 
                t1 = lastT1;
            if (t1 == null) 
                return null;
            Pullenti.Ner.Instrument.InstrumentParticipantReferent p1 = new Pullenti.Ner.Instrument.InstrumentParticipantReferent() { Typ = par1.Typ };
            if (par1.Parts != null) 
            {
                foreach (Pullenti.Ner.Referent p in par1.Parts) 
                {
                    p1.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, p, false, 0);
                }
            }
            Pullenti.Ner.Instrument.InstrumentParticipantReferent p2 = null;
            List<Pullenti.Ner.Instrument.InstrumentParticipantReferent> allParts = new List<Pullenti.Ner.Instrument.InstrumentParticipantReferent>();
            allParts.Add(p1);
            if (par1.Kind == ParticipantToken.Kinds.NamedAsParts) 
            {
                p1.Typ = "СТОРОНА 1";
                p1.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, par1.Parts[0], false, 0);
                for (int ii = 1; ii < par1.Parts.Count; ii++) 
                {
                    Pullenti.Ner.Instrument.InstrumentParticipantReferent pp = new Pullenti.Ner.Instrument.InstrumentParticipantReferent() { Typ = string.Format("СТОРОНА {0}", ii + 1) };
                    pp.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, par1.Parts[ii], false, 0);
                    if (ii == 1) 
                        p2 = pp;
                    allParts.Add(pp);
                }
                foreach (Pullenti.Ner.Referent pp in par1.Parts) 
                {
                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SOURCE, pp, false, 0);
                }
            }
            FragToken title = new FragToken(t0, t0) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Head };
            bool add = false;
            Pullenti.Ner.Token namBeg = null;
            Pullenti.Ner.Token namEnd = null;
            string dttyp = null;
            Pullenti.Ner.Decree.Internal.DecreeToken dt00 = null;
            Pullenti.Ner.Token namBeg2 = null;
            Pullenti.Ner.Token namEnd2 = null;
            for (t = t0; t != null && t.EndChar <= t1.EndChar; t = t.Next) 
            {
                if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                {
                    if (t.IsNewlineBefore || ((t.Previous != null && t.Previous.IsTableControlChar))) 
                        t = t.Kit.DebedToken(t);
                }
                bool newLineBef = t.IsNewlineBefore;
                if (t.Previous != null && t.Previous.IsTableControlChar) 
                    newLineBef = true;
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, dt00, false);
                if (dt != null) 
                {
                    dt00 = dt;
                    if ((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || ((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && newLineBef))) || ((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr && newLineBef))) 
                    {
                        if (namBeg != null && namEnd == null) 
                            namEnd = t.Previous;
                        if (namBeg2 != null && namEnd2 == null) 
                            namEnd2 = t.Previous;
                        if (((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && doc.Typ != null && !doc.Typ.Contains("ДОГОВОР")) && !doc.Typ.Contains("ДОГОВІР") && !isContract) && dt.Value != null && ((dt.Value.Contains("ДОГОВОР") || dt.Value.Contains("ДОГОВІР")))) 
                        {
                            doc.Typ = null;
                            doc.Number = 0;
                            doc.RegNumber = null;
                            isContract = true;
                            namBeg = (namEnd = null);
                            title.Children.Clear();
                        }
                        _addTitleAttr(doc, title, dt);
                        title.EndToken = (t = dt.EndToken);
                        if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                            dttyp = dt.Value;
                        add = true;
                        continue;
                    }
                }
                dt00 = null;
                if (newLineBef && t != t0) 
                {
                    FragToken edss = _createEditions(t);
                    if (edss != null) 
                    {
                        if (namBeg != null && namEnd == null) 
                            namEnd = t.Previous;
                        if (namBeg2 != null && namEnd2 == null) 
                            namEnd2 = t.Previous;
                        title.Children.Add(edss);
                        title.EndToken = edss.EndToken;
                        break;
                    }
                    InstrToken1 it1 = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                    if (it1 != null && it1.Numbers.Count > 0 && it1.NumTyp == NumberTypes.Digit) 
                    {
                        title.EndToken = t.Previous;
                        if (namBeg != null && namEnd == null) 
                            namEnd = t.Previous;
                        if (namBeg2 != null && namEnd2 == null) 
                            namEnd2 = t.Previous;
                        break;
                    }
                    if ((t.IsValue("О", "ПРО") || t.IsValue("ОБ", null) || t.IsValue("НА", null)) || t.IsValue("ПО", null)) 
                    {
                        if (namBeg == null) 
                        {
                            namBeg = t;
                            continue;
                        }
                        else if (namBeg2 == null && namEnd != null) 
                        {
                            namBeg2 = t;
                            continue;
                        }
                    }
                    if (add) 
                        title.EndToken = t.Previous;
                    add = false;
                    Pullenti.Ner.Referent r = t.GetReferent();
                    if ((r is Pullenti.Ner.Geo.GeoReferent) || (r is Pullenti.Ner.Date.DateReferent) || (r is Pullenti.Ner.Decree.DecreeReferent)) 
                    {
                        if (namBeg != null && namEnd == null) 
                            namEnd = t.Previous;
                        if (namBeg2 != null && namEnd2 == null) 
                            namEnd2 = t.Previous;
                    }
                }
                if ((dttyp != null && namBeg == null && t.Chars.IsCyrillicLetter) && (t is Pullenti.Ner.TextToken)) 
                {
                    if (t.IsValue("МЕЖДУ", "МІЖ")) 
                    {
                        Pullenti.Ner.ReferentToken pp = ParticipantToken.TryAttachToExist(t.Next, p1, p2);
                        if (pp != null && pp.EndToken.Next != null && pp.EndToken.Next.IsAnd) 
                        {
                            Pullenti.Ner.ReferentToken pp2 = ParticipantToken.TryAttachToExist(pp.EndToken.Next.Next, p1, p2);
                            if (pp2 != null) 
                            {
                                FragToken fr = new FragToken(t, pp2.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Place };
                                if (fr.Referents == null) 
                                    fr.Referents = new List<Pullenti.Ner.Referent>();
                                fr.Referents.Add(pp.Referent);
                                fr.Referents.Add(pp2.Referent);
                                title.Children.Add(fr);
                                t = (title.EndToken = fr.EndToken);
                                if (t.Next != null) 
                                {
                                    if (t.Next.IsValue("О", "ПРО") || t.Next.IsValue("ОБ", null)) 
                                    {
                                        namBeg = t.Next;
                                        namEnd = null;
                                        namBeg2 = (namEnd2 = null);
                                    }
                                }
                                continue;
                            }
                        }
                    }
                    namBeg = t;
                }
                else if (t.IsValue("МЕЖДУ", "МІЖ") || t.IsValue("ЗАКЛЮЧИТЬ", "УКЛАСТИ")) 
                {
                    if (namBeg != null && namEnd == null) 
                        namEnd = t.Previous;
                    if (namBeg2 != null && namEnd2 == null) 
                        namEnd2 = t.Previous;
                }
                if (((newLineBef && t.WhitespacesBeforeCount > 15)) || t.IsTableControlChar) 
                {
                    if (namBeg != null && namEnd == null && namBeg != t) 
                        namEnd = t.Previous;
                    if (namBeg2 != null && namEnd2 == null && namBeg2 != t) 
                        namEnd2 = t.Previous;
                }
            }
            if (namBeg != null && namEnd == null && t1 != null) 
                namEnd = t1;
            if (namBeg2 != null && namEnd2 == null && t1 != null) 
                namEnd2 = t1;
            if (namEnd != null && namBeg != null) 
            {
                string val = Pullenti.Ner.Core.MiscHelper.GetTextValue(namBeg, namEnd, Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
                if (val != null && val.Length > 3) 
                {
                    FragToken nam = new FragToken(namBeg, namEnd) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = val };
                    title.Children.Add(nam);
                    title.SortChildren();
                    if (namEnd.EndChar > title.EndChar) 
                        title.EndToken = namEnd;
                    if (dttyp != null && !val.Contains(dttyp)) 
                        val = string.Format("{0} {1}", dttyp, val);
                    if (namBeg2 != null && namEnd2 != null) 
                    {
                        string val2 = Pullenti.Ner.Core.MiscHelper.GetTextValue(namBeg2, namEnd2, Pullenti.Ner.Core.GetTextAttr.KeepQuotes);
                        if (val2 != null && val2.Length > 3) 
                        {
                            nam = new FragToken(namBeg2, namEnd2) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, Value = val2 };
                            title.Children.Add(nam);
                            title.SortChildren();
                            if (namEnd2.EndChar > title.EndChar) 
                                title.EndToken = namEnd2;
                            val = string.Format("{0} {1}", val, val2);
                        }
                    }
                    doc.Name = val;
                }
            }
            if (title.Children.Count > 0 && title.Children[0].BeginChar > title.BeginChar) 
                title.Children.Insert(0, new FragToken(title.BeginToken, title.Children[0].BeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Undefined });
            if (((doc.Typ == "ДОГОВОР" || doc.Typ == "ДОГОВІР")) && par1.Kind != ParticipantToken.Kinds.NamedAsParts) 
            {
                if (title.Children.Count > 0 && title.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Typ) 
                {
                    string addi = null;
                    foreach (FragToken ch in title.Children) 
                    {
                        if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Name) 
                        {
                            if (ch.BeginToken.Morph.Class.IsPreposition) 
                            {
                                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ch.BeginToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt != null) 
                                {
                                    addi = npt.Noun.GetSourceText().ToUpper();
                                    List<Pullenti.Morph.MorphWordForm> vvv = Pullenti.Morph.MorphologyService.GetAllWordforms(addi, null);
                                    foreach (Pullenti.Morph.MorphWordForm fi in vvv) 
                                    {
                                        if (fi.Case.IsGenitive) 
                                        {
                                            addi = fi.NormalCase;
                                            if (addi.EndsWith("НЬЯ")) 
                                                addi = addi.Substring(0, addi.Length - 2) + "ИЯ";
                                            break;
                                        }
                                    }
                                }
                            }
                            else 
                            {
                                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ch.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (npt != null && npt.EndChar <= ch.EndChar) 
                                    addi = npt.Noun.GetSourceText().ToUpper();
                            }
                            break;
                        }
                    }
                    if (addi != null) 
                    {
                        if (addi.StartsWith("ОКАЗАН")) 
                            addi = "УСЛУГ";
                        else if (addi.StartsWith("НАДАН")) 
                            addi = "ПОСЛУГ";
                        doc.Typ = string.Format("{0} {1}", doc.Typ, addi);
                        if (doc.Typ == doc.Name) 
                            doc.Name = null;
                    }
                    else if (contrTyps.Count == 1) 
                    {
                        if (doc.Typ == null || (doc.Typ.Length < contrTyps[0].Length)) 
                            doc.Typ = contrTyps[0];
                    }
                    else if (contrTyps.Count > 0 && doc.Typ == null) 
                        doc.Typ = contrTyps[0];
                }
            }
            if (doc.Typ == "ДОГОВОР УСЛУГ") 
                doc.Typ = "ДОГОВОР ОКАЗАНИЯ УСЛУГ";
            if (doc.Typ == "ДОГОВІР ПОСЛУГ") 
                doc.Typ = "ДОГОВІР НАДАННЯ ПОСЛУГ";
            if (doc.Typ == null && contrTyps.Count > 0) 
                doc.Typ = contrTyps[0];
            Pullenti.Ner.Core.AnalyzerData ad = t0.Kit.GetAnalyzerDataByAnalyzerName(Pullenti.Ner.Instrument.InstrumentAnalyzer.ANALYZER_NAME);
            if (ad == null) 
                return null;
            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PARTICIPANT, p1, false, 0);
            Pullenti.Ner.ReferentToken rt = par1.AttachFirst(p1, title.EndChar + 1, (par2 == null ? 0 : par2.BeginChar - 1));
            if (rt == null) 
                return null;
            if (par2 == null) 
            {
                if (p1.Slots.Count < 2) 
                    return null;
                if (!isContract) 
                    return null;
                Pullenti.Ner.Token tt2 = null;
                for (Pullenti.Ner.Token ttt = rt.EndToken.Next; ttt != null; ttt = ttt.Next) 
                {
                    if (ttt.IsComma || ttt.IsAnd) 
                        continue;
                    if (ttt.Morph.Class.IsPreposition) 
                        continue;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.ParseNumericAsAdjective, 0, null);
                    if (npt != null) 
                    {
                        if (npt.EndToken.IsValue("СТОРОНА", null)) 
                        {
                            ttt = npt.EndToken;
                            continue;
                        }
                    }
                    tt2 = ttt;
                    break;
                }
                if (tt2 != null && par1 != null) 
                {
                    Dictionary<string, int> stat = new Dictionary<string, int>();
                    int cou1 = 0;
                    for (Pullenti.Ner.Token ttt = tt2; ttt != null; ttt = ttt.Next) 
                    {
                        if (ttt.IsValue(par1.Typ, null)) 
                        {
                            cou1++;
                            continue;
                        }
                        Pullenti.Ner.Core.TerminToken tok = ParticipantToken.m_Ontology.TryParse(ttt, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok != null && tok.Termin.Tag != null && tok.Termin.CanonicText != "СТОРОНА") 
                        {
                            if (!stat.ContainsKey(tok.Termin.CanonicText)) 
                                stat.Add(tok.Termin.CanonicText, 1);
                            else 
                                stat[tok.Termin.CanonicText]++;
                        }
                    }
                    string typ2 = null;
                    if (cou1 > 10) 
                    {
                        int minCou = (int)((cou1 * 0.6));
                        int maxCou = (int)((cou1 * 1.4));
                        foreach (KeyValuePair<string, int> kp in stat) 
                        {
                            if (kp.Value >= minCou && kp.Value <= maxCou) 
                            {
                                typ2 = kp.Key;
                                break;
                            }
                        }
                    }
                    if (typ2 != null) 
                        par2 = new ParticipantToken(tt2, tt2) { Typ = typ2 };
                }
            }
            rt.Referent = (p1 = ad.RegisterReferent(p1) as Pullenti.Ner.Instrument.InstrumentParticipantReferent);
            t0.Kit.EmbedToken(rt);
            if (par2 != null) 
            {
                p2 = new Pullenti.Ner.Instrument.InstrumentParticipantReferent() { Typ = par2.Typ };
                if (par2.Parts != null) 
                {
                    foreach (Pullenti.Ner.Referent p in par2.Parts) 
                    {
                        p2.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, p, false, 0);
                    }
                }
                p2 = ad.RegisterReferent(p2) as Pullenti.Ner.Instrument.InstrumentParticipantReferent;
                doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PARTICIPANT, p2, false, 0);
                rt = par2.AttachFirst(p2, rt.EndChar + 1, 0);
                if (rt == null) 
                    return title;
                t0.Kit.EmbedToken(rt);
            }
            else if (allParts.Count > 1) 
            {
                foreach (Pullenti.Ner.Instrument.InstrumentParticipantReferent pp in allParts) 
                {
                    Pullenti.Ner.Instrument.InstrumentParticipantReferent ppp = ad.RegisterReferent(pp) as Pullenti.Ner.Instrument.InstrumentParticipantReferent;
                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PARTICIPANT, ppp, false, 0);
                    if (pp == allParts[1]) 
                        p2 = ppp;
                }
            }
            int reqRegim = 0;
            for (t = rt.Next; t != null; t = (t == null ? null : t.Next)) 
            {
                if (t.BeginChar >= 712 && (t.BeginChar < 740)) 
                {
                }
                if (t.IsNewlineBefore) 
                {
                    InstrToken1 ii = InstrToken1.Parse(t, true, null, 0, null, false, 0, true, false);
                    if (ii != null && ii.TitleTyp == InstrToken1.StdTitleType.Requisites) 
                    {
                        reqRegim = 5;
                        t = ii.EndToken;
                        continue;
                    }
                }
                if (t.IsValue("ПРИЛОЖЕНИЕ", null) && t.IsNewlineBefore) 
                {
                }
                if (reqRegim == 5 && t.IsChar((char)0x1E)) 
                {
                    List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(t, 0, true);
                    if (rows != null && rows.Count > 0 && ((rows[0].Cells.Count == 2 || rows[0].Cells.Count == 3))) 
                    {
                        int i0 = rows[0].Cells.Count - 2;
                        Pullenti.Ner.ReferentToken rt0 = ParticipantToken.TryAttachToExist(rows[0].Cells[i0].BeginToken, p1, p2);
                        Pullenti.Ner.ReferentToken rt1 = ParticipantToken.TryAttachToExist(rows[0].Cells[i0 + 1].BeginToken, p1, p2);
                        if (rt0 != null && rt1 != null && rt1.Referent != rt0.Referent) 
                        {
                            for (int ii = 0; ii < rows.Count; ii++) 
                            {
                                if (rows[ii].Cells.Count == rows[0].Cells.Count) 
                                {
                                    rt = ParticipantToken.TryAttachRequisites(rows[ii].Cells[i0].BeginToken, rt0.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent, rt1.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent, false);
                                    if (rt != null && rt.EndChar <= rows[ii].Cells[i0].EndChar) 
                                        t0.Kit.EmbedToken(rt);
                                    rt = ParticipantToken.TryAttachRequisites(rows[ii].Cells[i0 + 1].BeginToken, rt1.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent, rt0.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent, false);
                                    if (rt != null && rt.EndChar <= rows[ii].Cells[i0 + 1].EndChar) 
                                        t0.Kit.EmbedToken(rt);
                                }
                            }
                            t = rows[rows.Count - 1].EndToken;
                            reqRegim = 0;
                            continue;
                        }
                    }
                }
                rt = ParticipantToken.TryAttachToExist(t, p1, p2);
                if (rt == null && reqRegim > 0) 
                {
                    for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsTableControlChar) 
                        {
                        }
                        else if (tt.IsCharOf(".)") || (tt is Pullenti.Ner.NumberToken)) 
                        {
                        }
                        else 
                        {
                            rt = ParticipantToken.TryAttachToExist(tt, p1, p2);
                            if (rt != null && !t.IsTableControlChar) 
                                rt.BeginToken = t;
                            break;
                        }
                    }
                }
                if (rt == null) 
                {
                    reqRegim--;
                    continue;
                }
                List<Pullenti.Ner.Instrument.InstrumentParticipantReferent> ps = new List<Pullenti.Ner.Instrument.InstrumentParticipantReferent>();
                ps.Add(rt.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent);
                if (reqRegim > 0) 
                {
                    Pullenti.Ner.ReferentToken rt1 = ParticipantToken.TryAttachRequisites(rt.EndToken.Next, ps[0], (ps[0] == p1 ? p2 : p1), false);
                    if (rt1 != null) 
                        rt.EndToken = rt1.EndToken;
                }
                t0.Kit.EmbedToken(rt);
                t = rt;
                if (reqRegim <= 0) 
                {
                    if (t.IsNewlineBefore) 
                    {
                    }
                    else if (t.Previous != null && t.Previous.IsTableControlChar) 
                    {
                    }
                    else 
                        continue;
                }
                else 
                {
                }
                if (rt.EndToken.Next != null && rt.EndToken.Next.IsTableControlChar && !rt.EndToken.Next.IsChar((char)0x1E)) 
                {
                    for (Pullenti.Ner.Token tt = rt.EndToken.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsTableControlChar) 
                        {
                        }
                        else if (tt.IsCharOf(".)") || (tt is Pullenti.Ner.NumberToken)) 
                        {
                        }
                        else 
                        {
                            Pullenti.Ner.ReferentToken rt1 = ParticipantToken.TryAttachRequisites(tt, (ps[0] == p1 ? p2 : p1), ps[0], true);
                            if (rt1 != null) 
                            {
                                ps.Add(rt1.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent);
                                t0.Kit.EmbedToken(rt1);
                                t = rt1;
                            }
                            break;
                        }
                    }
                }
                t = t.Next;
                if (t == null) 
                    break;
                while (t.IsTableControlChar && t.Next != null) 
                {
                    t = t.Next;
                }
                int cur = 0;
                for (; t != null; t = t.Next) 
                {
                    if (t.IsTableControlChar && t.IsChar((char)0x1F)) 
                    {
                        reqRegim = 0;
                        break;
                    }
                    rt = ParticipantToken.TryAttachRequisites(t, ps[cur], (p1 == ps[cur] ? p2 : p1), reqRegim <= 0);
                    if (rt != null) 
                    {
                        reqRegim = 5;
                        t0.Kit.EmbedToken(rt);
                        t = rt;
                    }
                    else 
                    {
                        t = t.Previous;
                        break;
                    }
                    if (ps.Count == 2 && t.Next.IsTableControlChar) 
                    {
                        Pullenti.Ner.Token tt = t.Next;
                        for (; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsTableControlChar && tt.IsChar((char)0x1F)) 
                                break;
                            if (tt.IsTableControlChar) 
                            {
                                cur = 1 - cur;
                                if (Pullenti.Ner.Core.TableHelper.IsCellEnd(tt) && Pullenti.Ner.Core.TableHelper.IsRowEnd(tt.Next)) 
                                    tt = tt.Next;
                                t = tt;
                                continue;
                            }
                            break;
                        }
                        continue;
                    }
                    if (t.IsTableControlChar && ps.Count == 2) 
                    {
                        if (Pullenti.Ner.Core.TableHelper.IsCellEnd(t) && Pullenti.Ner.Core.TableHelper.IsRowEnd(t.Next)) 
                            t = t.Next;
                        cur = 1 - cur;
                        continue;
                    }
                    if (!t.IsNewlineAfter) 
                        continue;
                    InstrToken1 it1 = InstrToken1.Parse(t.Next, true, null, 0, null, false, 0, false, false);
                    if (it1 != null) 
                    {
                        if (it1.AllUpper || it1.TitleTyp != InstrToken1.StdTitleType.Undefined || it1.Numbers.Count > 0) 
                            break;
                    }
                }
            }
            return title;
        }
        static FragToken CreateProjectTitle(Pullenti.Ner.Token t0, Pullenti.Ner.Instrument.InstrumentReferent doc)
        {
            if (t0 == null) 
                return null;
            bool isProject = false;
            bool isEntered = false;
            bool isTyp = false;
            if (t0.IsTableControlChar && t0.Next != null) 
                t0 = t0.Next;
            FragToken title = new FragToken(t0, t0) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Head };
            Pullenti.Ner.Token t;
            for (t = t0; t != null; t = t.Next) 
            {
                if (t.IsTableControlChar) 
                    continue;
                if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                    t = t.Kit.DebedToken(t);
                if ((t is Pullenti.Ner.TextToken) && (((t as Pullenti.Ner.TextToken).Term == "ПРОЕКТ" || (t as Pullenti.Ner.TextToken).Term == "ЗАКОНОПРОЕКТ"))) 
                {
                    if ((t.IsValue("ПРОЕКТ", null) && t == t0 && (t.Next is Pullenti.Ner.ReferentToken)) && (t.Next.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                        return null;
                    isProject = true;
                    title.Children.Add(new FragToken(t, t) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, DefVal2 = true });
                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_TYPE, "ПРОЕКТ", false, 0);
                    continue;
                }
                Pullenti.Ner.Token tt = _attachProjectEnter(t);
                if (tt != null) 
                {
                    isEntered = true;
                    title.Children.Add(new FragToken(t, tt) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Approved });
                    t = tt;
                    continue;
                }
                tt = _attachProjectMisc(t);
                if (tt != null) 
                {
                    title.Children.Add(new FragToken(t, tt) { Kind = (tt.IsValue("ЧТЕНИЕ", "ЧИТАННЯ") ? Pullenti.Ner.Instrument.InstrumentKind.Editions : Pullenti.Ner.Instrument.InstrumentKind.Undefined) });
                    t = tt;
                    continue;
                }
                if (t.IsNewlineBefore && (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && ((isProject || isEntered))) 
                    t = t.Kit.DebedToken(t);
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                if (dt != null) 
                {
                    if ((dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) || dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                    {
                        if (_addTitleAttr(doc, title, dt)) 
                        {
                            if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                                isTyp = true;
                            t = dt.EndToken;
                            continue;
                        }
                    }
                }
                break;
            }
            if (isProject) 
            {
            }
            else if (isEntered && isTyp) 
            {
            }
            else 
                return null;
            title.EndToken = t.Previous;
            Pullenti.Ner.Token t00 = t;
            Pullenti.Ner.Token t11 = null;
            bool isBr = Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t00, false, false);
            for (t = t00; t != null; t = t.Next) 
            {
                if (t.IsNewlineAfter) 
                {
                    if (t.Next != null && t.Next.Chars.IsAllLower) 
                        continue;
                }
                if (t.WhitespacesAfterCount > 15) 
                {
                    t11 = t;
                    break;
                }
                else if (t.IsNewlineAfter && t.Next != null) 
                {
                    if (t.Next.GetMorphClassInDictionary() == Pullenti.Morph.MorphClass.Verb) 
                    {
                        t11 = t;
                        break;
                    }
                    if (t.Next.Chars.IsCapitalUpper && t.Next.Morph.Class.IsVerb) 
                    {
                        t11 = t;
                        break;
                    }
                }
                if (t.IsWhitespaceAfter && isBr && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, false, null, false)) 
                {
                    t11 = t;
                    break;
                }
                if (!t.IsNewlineBefore) 
                    continue;
                InstrToken1 it = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                if (it != null && it.Numbers.Count > 0 && it.LastNumber == 1) 
                {
                    t11 = t.Previous;
                    break;
                }
            }
            if (t11 == null) 
                return null;
            FragToken nam = new FragToken(t00, t11) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true };
            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentBlockReferent.ATTR_NAME, nam.Value, false, 0);
            title.Children.Add(nam);
            title.EndToken = t11;
            FragToken appr1 = _createApproved(t11.Next);
            if (appr1 != null) 
            {
                title.Children.Add(appr1);
                title.EndToken = appr1.EndToken;
            }
            return title;
        }
        static Pullenti.Ner.Token _attachProjectMisc(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            bool br = false;
            if (t.IsChar('(') && t.Next != null) 
            {
                br = true;
                t = t.Next;
            }
            if (t.Morph.Class.IsPreposition) 
                t = t.Next;
            if ((t is Pullenti.Ner.NumberToken) && t.Next != null && t.Next.IsValue("ЧТЕНИЕ", "ЧИТАННЯ")) 
            {
                t = t.Next;
                if (br && t.Next != null && t.Next.IsChar(')')) 
                    t = t.Next;
                return t;
            }
            return null;
        }
        static Pullenti.Ner.Token _attachProjectEnter(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (t.IsValue("ВНОСИТЬ", "ВНОСИТИ") || t.IsValue("ВНЕСТИ", null)) 
            {
            }
            else 
                return null;
            int cou = 0;
            for (t = t.Next; t != null; t = t.Next) 
            {
                if (t.Morph.Class.IsPreposition || t.Morph.Class.IsConjunction) 
                    continue;
                if (((t.IsValue("ПЕРИОД", "ПЕРІОД") || t.IsValue("РАССМОТРЕНИЕ", "РОЗГЛЯД") || t.IsValue("ДЕПУТАТ", null)) || t.IsValue("ПОЛНОМОЧИЕ", "ПОВНОВАЖЕННЯ") || t.IsValue("ПЕРЕДАЧА", null)) || t.IsValue("ИСПОЛНЕНИЕ", "ВИКОНАННЯ")) 
                    continue;
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r is Pullenti.Ner.Org.OrganizationReferent) 
                {
                    if (cou > 0 && t.IsNewlineBefore) 
                        return t.Previous;
                    cou++;
                    continue;
                }
                if ((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Person.PersonPropertyReferent)) 
                {
                    cou++;
                    continue;
                }
                if (t.IsNewlineBefore) 
                    return t.Previous;
            }
            return null;
        }
        static void _createJusticeParticipants(FragToken title, Pullenti.Ner.Instrument.InstrumentReferent doc)
        {
            string typ = doc.Typ;
            bool ok = ((((typ == "ПОСТАНОВЛЕНИЕ" || typ == "РЕШЕНИЕ" || typ == "ОПРЕДЕЛЕНИЕ") || typ == "ПРИГОВОР" || ((typ ?? "")).EndsWith("ЗАЯВЛЕНИЕ")) || typ == "ПОСТАНОВА" || typ == "РІШЕННЯ") || typ == "ВИЗНАЧЕННЯ" || typ == "ВИРОК") || ((typ ?? "")).EndsWith("ЗАЯВА");
            foreach (Pullenti.Ner.Slot s in doc.Slots) 
            {
                if (s.TypeName == Pullenti.Ner.Instrument.InstrumentReferent.ATTR_SOURCE && (s.Value is Pullenti.Ner.Org.OrganizationReferent)) 
                {
                    Pullenti.Ner.Org.OrganizationKind ki = (s.Value as Pullenti.Ner.Org.OrganizationReferent).Kind;
                    if (ki == Pullenti.Ner.Org.OrganizationKind.Justice) 
                        ok = true;
                }
                else if (s.TypeName == Pullenti.Ner.Instrument.InstrumentReferent.ATTR_CASENUMBER) 
                    ok = true;
            }
            Pullenti.Ner.ReferentToken pIst = null;
            Pullenti.Ner.ReferentToken pOtv = null;
            Pullenti.Ner.ReferentToken pZayav = null;
            int cou = 0;
            Pullenti.Ner.Token t;
            StringBuilder tmp = new StringBuilder();
            for (t = title.BeginToken; t != null && t.EndChar <= title.EndChar; t = t.Next) 
            {
                if (t.IsNewlineBefore) 
                {
                }
                else if (t.Previous != null && t.Previous.IsTableControlChar) 
                {
                }
                else 
                    continue;
                if (t.Next != null && ((t.Next.IsChar(':') || t.Next.IsTableControlChar))) 
                {
                    if (t.IsValue("ЗАЯВИТЕЛЬ", "ЗАЯВНИК")) 
                    {
                        pZayav = _createJustParticipant(t.Next, null);
                        if (pZayav != null) 
                        {
                            pZayav.BeginToken = t;
                            (pZayav.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent).Typ = "ЗАЯВИТЕЛЬ";
                        }
                    }
                    else if (t.IsValue("ИСТЕЦ", "ПОЗИВАЧ")) 
                    {
                        pIst = _createJustParticipant(t.Next, null);
                        if (pIst != null) 
                        {
                            pIst.BeginToken = t;
                            (pIst.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent).Typ = "ИСТЕЦ";
                        }
                    }
                    else if (t.IsValue("ОТВЕТЧИК", "ВІДПОВІДАЧ") || t.IsValue("ДОЛЖНИК", "БОРЖНИК")) 
                    {
                        pOtv = _createJustParticipant(t.Next, null);
                        if (pOtv != null) 
                        {
                            pOtv.BeginToken = t;
                            (pOtv.Referent as Pullenti.Ner.Instrument.InstrumentParticipantReferent).Typ = "ОТВЕТЧИК";
                        }
                    }
                }
            }
            for (t = title.EndToken.Next; t != null; t = t.Next) 
            {
                if ((++cou) > 1000) 
                    break;
                if (t.IsValue("ЗАЯВЛЕНИЕ", "ЗАЯВА")) 
                {
                }
                else if (t.IsValue("ИСК", "ПОЗОВ") && t.Previous != null && t.Previous.Morph.Class.IsPreposition) 
                {
                }
                else 
                    continue;
                if (t.Next != null && t.Next.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                        t = br.EndToken;
                }
                if (pIst != null) 
                    break;
                pIst = _createJustParticipant(t.Next, (t.Next.Morph.Language.IsUa ? "ПОЗИВАЧ" : "ИСТЕЦ"));
                if (pIst == null) 
                    break;
                t = pIst.EndToken.Next;
                if (t != null && t.IsChar(',')) 
                    t = t.Next;
                if (t == null) 
                    break;
                if (pOtv != null) 
                    break;
                if (t.IsValue("О", "ПРО") && t.Next != null && t.Next.IsValue("ПРИВЛЕЧЕНИЕ", "ЗАЛУЧЕННЯ")) 
                {
                    if (t.Next.Morph.Language.IsUa) 
                        tmp.Append("ПРО ПРИТЯГНЕННЯ");
                    else 
                        tmp.Append("О ПРИВЛЕЧЕНИИ");
                    t = t.Next.Next;
                    pOtv = _createJustParticipant(t, (t.Next.Morph.Language.IsUa ? "ВІДПОВІДАЧ" : "ОТВЕТЧИК"));
                }
                else if (t.IsValue("О", "ПРО") && t.Next != null && t.Next.IsValue("ПРИЗНАНИЕ", "ВИЗНАННЯ")) 
                {
                    if (t.Next.Morph.Language.IsUa) 
                        tmp.Append("ПРО ВИЗНАННЯ");
                    else 
                        tmp.Append("О ПРИЗНАНИИ");
                    t = t.Next.Next;
                    pOtv = _createJustParticipant(t, (t.Next.Morph.Language.IsUa ? "ВІДПОВІДАЧ" : "ОТВЕТЧИК"));
                }
                else if (t.IsValue("О", "ПРО") && t.Next != null && t.Next.IsValue("ВЗЫСКАНИЕ", "СТЯГНЕННЯ")) 
                {
                    if (t.Next.Morph.Language.IsUa) 
                        tmp.Append("ПРО СТЯГНЕННЯ");
                    else 
                        tmp.Append("О ВЗЫСКАНИИ");
                    t = t.Next.Next;
                    if (t != null && t.Morph.Class.IsPreposition) 
                        t = t.Next;
                    pOtv = _createJustParticipant(t, (t.Next.Morph.Language.IsUa ? "ВІДПОВІДАЧ" : "ОТВЕТЧИК"));
                }
                else 
                {
                    if (t == null || !t.IsValue("К", "ПРО")) 
                        break;
                    pOtv = _createJustParticipant(t.Next, (t.Next.Morph.Language.IsUa ? "ВІДПОВІДАЧ" : "ОТВЕТЧИК"));
                }
                if (pOtv != null) 
                    t = pOtv.EndToken.Next;
                break;
            }
            if (((pIst == null && pZayav == null)) || ((pOtv == null && tmp.Length == 0))) 
                return;
            Pullenti.Ner.Core.AnalyzerData ad = title.Kit.GetAnalyzerDataByAnalyzerName(Pullenti.Ner.Instrument.InstrumentAnalyzer.ANALYZER_NAME);
            if (pZayav != null) 
            {
                pZayav.Referent = ad.RegisterReferent(pZayav.Referent);
                pZayav.Kit.EmbedToken(pZayav);
                doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PARTICIPANT, pZayav.Referent, false, 0);
            }
            if (pIst != null) 
            {
                pIst.Referent = ad.RegisterReferent(pIst.Referent);
                pIst.Kit.EmbedToken(pIst);
                doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PARTICIPANT, pIst.Referent, false, 0);
            }
            if (pOtv != null) 
            {
                pOtv.Referent = ad.RegisterReferent(pOtv.Referent);
                pOtv.Kit.EmbedToken(pOtv);
                doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PARTICIPANT, pOtv.Referent, false, 0);
            }
            if (t != null && t.IsChar(',')) 
                t = t.Next;
            if (t == null) 
                return;
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
            if (npt != null && npt.EndToken.IsValue("ЛИЦО", "ОСОБА")) 
            {
                t = npt.EndToken.Next;
                if (t != null && t.IsChar(':')) 
                    t = t.Next;
                for (; t != null; t = t.Next) 
                {
                    if (t.IsChar(',')) 
                        continue;
                    Pullenti.Ner.ReferentToken tret = _createJustParticipant(t, (t.Morph.Language.IsUa ? "ТРЕТЯ ОСОБА" : "ТРЕТЬЕ ЛИЦО"));
                    if (tret == null) 
                        break;
                    tret.Referent = ad.RegisterReferent(tret.Referent);
                    tret.Kit.EmbedToken(tret);
                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PARTICIPANT, tret.Referent, false, 0);
                    t = tret;
                }
            }
            Pullenti.Ner.Token tt00 = t;
            while (t != null) 
            {
                Pullenti.Ner.Token t0 = t;
                if (!t.IsValue("О", "ПРО") && !t.IsValue("ОБ", null)) 
                {
                    if (tmp.Length == 0) 
                    {
                        if (t != tt00) 
                            break;
                        int cou2 = 0;
                        bool hasIsk = true;
                        for (Pullenti.Ner.Token tt = t.Next; tt != null && (cou2 < 140); tt = tt.Next,cou2++) 
                        {
                            if (tt.IsValue("ЗАЯВЛЕНИЕ", "ЗАЯВА") || tt.IsValue("ИСК", "ПОЗОВ")) 
                            {
                                cou2 = 0;
                                hasIsk = true;
                            }
                            if ((hasIsk && ((tt.IsValue("О", "ПРО") || tt.IsValue("ОБ", null))) && tt.Next.GetMorphClassInDictionary().IsNoun) && tt.Next.Morph.Case.IsPrepositional) 
                            {
                                tmp.Append(Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, tt.Next, Pullenti.Ner.Core.GetTextAttr.No));
                                t0 = tt;
                                t = tt.Next.Next;
                                break;
                            }
                        }
                        if (tmp.Length == 0 || t == null) 
                            break;
                    }
                }
                List<Pullenti.Ner.Referent> arefs = new List<Pullenti.Ner.Referent>();
                Pullenti.Ner.Token t1 = null;
                for (; t != null; t = t.Next) 
                {
                    if (t.IsNewlineBefore && t != t0) 
                    {
                        if (t.WhitespacesBeforeCount > 15) 
                            break;
                    }
                    if (t.IsValue("ПРИ", "ЗА") && t.Next != null && t.Next.IsValue("УЧАСТИЕ", "УЧАСТЬ")) 
                        break;
                    if (t.IsValue("БЕЗ", null) && t.Next != null && t.Next.IsValue("ВЫЗОВ", null)) 
                        break;
                    Pullenti.Ner.Referent r = t.GetReferent();
                    if (r != null) 
                    {
                        if (r is Pullenti.Ner.Money.MoneyReferent) 
                        {
                            arefs.Add(r);
                            if (t.Previous != null && t.Previous.IsValue("СУММА", "СУМА")) 
                            {
                            }
                            else 
                                tmp.AppendFormat(" СУММЫ");
                            t1 = t;
                            continue;
                        }
                        if ((r is Pullenti.Ner.Decree.DecreePartReferent) || (r is Pullenti.Ner.Decree.DecreeReferent)) 
                        {
                            arefs.Add(r);
                            if (t.Previous != null && t.Previous.IsValue("ПО", null)) 
                                tmp.Length -= 3;
                            t1 = t;
                            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                            {
                                if (tt.IsCommaAnd) 
                                    continue;
                                r = tt.GetReferent();
                                if ((r is Pullenti.Ner.Decree.DecreePartReferent) || (r is Pullenti.Ner.Decree.DecreeReferent)) 
                                {
                                    arefs.Add(r);
                                    t1 = (t = tt);
                                    continue;
                                }
                                break;
                            }
                            break;
                        }
                        if (r is Pullenti.Ner.Person.PersonReferent) 
                            continue;
                        break;
                    }
                    if (t.IsCharOf(",.") || t.IsHiphen) 
                        break;
                    if (t is Pullenti.Ner.TextToken) 
                    {
                        string term = (t as Pullenti.Ner.TextToken).Term;
                        if (term == "ИП") 
                            continue;
                    }
                    if (t.IsAnd) 
                    {
                        if (t.Next == null) 
                            break;
                        if (t.Next.IsValue("О", "ПРО") || t.Next.IsValue("ОБ", null)) 
                        {
                            t = t.Next;
                            break;
                        }
                    }
                    if (t.IsNewlineAfter) 
                    {
                        if (t.Next == null) 
                        {
                        }
                        else if (t.Next.Chars.IsAllLower) 
                        {
                        }
                        else 
                            break;
                    }
                    if (t.IsWhitespaceBefore && tmp.Length > 0) 
                        tmp.Append(' ');
                    tmp.Append(Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t, Pullenti.Ner.Core.GetTextAttr.No));
                    t1 = t;
                }
                if (tmp.Length > 10 && t1 != null) 
                {
                    Pullenti.Ner.Instrument.InstrumentArtefactReferent art = new Pullenti.Ner.Instrument.InstrumentArtefactReferent() { Typ = "предмет" };
                    string str = tmp.ToString();
                    str = str.Replace("В РАЗМЕРЕ СУММЫ", "СУММЫ").Trim();
                    if (str.EndsWith("В РАЗМЕРЕ")) 
                        str = str.Substring(0, str.Length - 9) + "СУММЫ";
                    if (str.EndsWith("В СУММЕ")) 
                        str = str.Substring(0, str.Length - 7) + "СУММЫ";
                    art.Value = str;
                    foreach (Pullenti.Ner.Referent a in arefs) 
                    {
                        art.AddSlot(Pullenti.Ner.Instrument.InstrumentArtefactReferent.ATTR_REF, a, false, 0);
                    }
                    Pullenti.Ner.ReferentToken rta = new Pullenti.Ner.ReferentToken(art, t0, t1);
                    rta.Referent = ad.RegisterReferent(rta.Referent);
                    doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_ARTEFACT, rta.Referent, false, 0);
                    rta.Kit.EmbedToken(rta);
                    tmp.Length = 0;
                }
                else 
                    break;
            }
            for (t = (pOtv == null ? t : pOtv.Next); t != null; t = t.Next) 
            {
                Pullenti.Ner.ReferentToken rt = null;
                bool checkDel = false;
                if (t.IsValue("ИСТЕЦ", "ПОЗИВАЧ") && pIst != null) 
                {
                    rt = new Pullenti.Ner.ReferentToken(pIst.Referent, t, t);
                    checkDel = true;
                }
                else if (t.IsValue("ЗАЯВИТЕЛЬ", "ЗАЯВНИК") && pZayav != null) 
                {
                    rt = new Pullenti.Ner.ReferentToken(pZayav.Referent, t, t);
                    checkDel = true;
                }
                else if (((t.IsValue("ОТВЕТЧИК", "ВІДПОВІДАЧ") || t.IsValue("ДОЛЖНИК", "БОРЖНИК"))) && pOtv != null) 
                {
                    rt = new Pullenti.Ner.ReferentToken(pOtv.Referent, t, t);
                    checkDel = true;
                }
                else 
                {
                    Pullenti.Ner.Referent r = t.GetReferent();
                    if (!(r is Pullenti.Ner.Org.OrganizationReferent) && !(r is Pullenti.Ner.Person.PersonReferent)) 
                        continue;
                    if (pIst != null && pIst.Referent.FindSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, r, true) != null) 
                        rt = new Pullenti.Ner.ReferentToken(pIst.Referent, t, t);
                    else if (pZayav != null && pZayav.Referent.FindSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, r, true) != null) 
                        rt = new Pullenti.Ner.ReferentToken(pZayav.Referent, t, t);
                    else if (pOtv != null && pOtv.Referent.FindSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, r, true) != null) 
                        rt = new Pullenti.Ner.ReferentToken(pOtv.Referent, t, t);
                }
                if (rt == null) 
                    continue;
                if (checkDel && t.Previous != null && t.Previous.IsValue("ОТ", null)) 
                {
                    Pullenti.Ner.Token tt = t.Previous;
                    if (tt.Previous != null && tt.Previous.IsHiphen) 
                        tt = tt.Previous;
                    if (tt.IsWhitespaceBefore) 
                    {
                        Pullenti.Ner.Token tt1 = t.Next;
                        if (tt1 != null && ((tt1.IsHiphen || tt1.IsChar(':')))) 
                            tt1 = tt1.Next;
                        if (tt1.GetReferent() is Pullenti.Ner.Person.PersonReferent) 
                        {
                            rt.BeginToken = tt;
                            rt.EndToken = tt1;
                            rt.Referent.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_DELEGATE, tt1.GetReferent() as Pullenti.Ner.Person.PersonReferent, false, 0);
                        }
                    }
                }
                if (rt != null && rt.EndToken.Next != null && rt.EndToken.Next.IsChar('(')) 
                {
                    Pullenti.Ner.Token tt = rt.EndToken.Next.Next;
                    if (tt != null && tt.Next != null && tt.Next.IsChar(')')) 
                    {
                        if (tt.IsValue("ИСТЕЦ", "ПОЗИВАЧ") && pIst != null && rt.Referent == pIst.Referent) 
                            rt.EndToken = tt.Next;
                        else if (tt.IsValue("ЗАЯВИТЕЛЬ", "ЗАЯВНИК") && pZayav != null && rt.Referent == pZayav.Referent) 
                            rt.EndToken = tt.Next;
                        else if (((tt.IsValue("ОТВЕТЧИК", "ВІДПОВІДАЧ") || tt.IsValue("ДОЛЖНИК", "БОРЖНИК"))) && pOtv != null && rt.Referent == pOtv.Referent) 
                            rt.EndToken = tt.Next;
                        else if ((tt.GetReferent() is Pullenti.Ner.Person.PersonReferent) || (tt.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                        {
                            if (pIst != null && rt.Referent == pIst.Referent) 
                            {
                                if (pIst.Referent.FindSlot(null, tt.GetReferent(), true) != null) 
                                    rt.EndToken = tt.Next;
                                else if (pOtv != null && pOtv.Referent.FindSlot(null, tt.GetReferent(), true) == null) 
                                {
                                    rt.EndToken = tt.Next;
                                    pIst.Referent.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, tt.GetReferent(), false, 0);
                                }
                            }
                            else if (pOtv != null && rt.Referent == pOtv.Referent) 
                            {
                                if (pOtv.Referent.FindSlot(null, tt.GetReferent(), true) != null) 
                                    rt.EndToken = tt.Next;
                                else if (pIst != null && pIst.Referent.FindSlot(null, tt.GetReferent(), true) == null) 
                                {
                                    rt.EndToken = tt.Next;
                                    pOtv.Referent.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, tt.GetReferent(), false, 0);
                                }
                            }
                        }
                    }
                }
                t.Kit.EmbedToken(rt);
                t = rt;
            }
        }
        static Pullenti.Ner.ReferentToken _createJustParticipant(Pullenti.Ner.Token t, string typ)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Referent r0 = null;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            bool ok = false;
            bool br = false;
            List<Pullenti.Ner.Referent> refs = new List<Pullenti.Ner.Referent>();
            for (; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore && t != t0) 
                {
                    if (t.WhitespacesBeforeCount > 15) 
                        break;
                }
                if (t.IsHiphen || t.IsCharOf(":,") || t.IsTableControlChar) 
                    continue;
                if (!br) 
                {
                    if (t.IsValue("К", null) || t.IsValue("О", "ПРО")) 
                        break;
                }
                if (t.IsChar('(')) 
                {
                    if (br) 
                        break;
                    br = true;
                    continue;
                }
                if (t.IsChar(')') && br) 
                {
                    br = false;
                    t1 = t;
                    break;
                }
                Pullenti.Ner.Referent r = t.GetReferent();
                if ((r is Pullenti.Ner.Person.PersonReferent) || (r is Pullenti.Ner.Org.OrganizationReferent)) 
                {
                    if (r0 == null) 
                    {
                        refs.Add(r);
                        r0 = r;
                        t1 = t;
                        ok = true;
                        continue;
                    }
                    break;
                }
                if (r is Pullenti.Ner.Uri.UriReferent) 
                {
                    Pullenti.Ner.Uri.UriReferent ur = r as Pullenti.Ner.Uri.UriReferent;
                    if (ur.Scheme == "ИНН" || ur.Scheme == "ИИН" || ur.Scheme == "ОГРН") 
                        ok = true;
                    refs.Add(r);
                    t1 = t;
                    continue;
                }
                if (!br) 
                {
                    if ((r is Pullenti.Ner.Decree.DecreeReferent) || (r is Pullenti.Ner.Decree.DecreePartReferent)) 
                        break;
                }
                if (r != null || br) 
                {
                    if ((r is Pullenti.Ner.Phone.PhoneReferent) || (r is Pullenti.Ner.Address.AddressReferent)) 
                        refs.Add(r);
                    t1 = t;
                    continue;
                }
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken brr = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (brr != null) 
                    {
                        ok = true;
                        t1 = (t = brr.EndToken);
                        continue;
                    }
                }
                if (t.Previous.IsComma && !br) 
                    break;
                if (t.Previous.Morph.Class.IsPreposition && t.IsValue("УЧАСТИЕ", "УЧАСТЬ")) 
                    break;
                if ((t.Previous is Pullenti.Ner.NumberToken) && t.IsValue("ЛИЦО", "ОСОБА")) 
                    break;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if ((npt.Noun.IsValue("УЧРЕЖДЕНИЕ", "УСТАНОВА") || npt.Noun.IsValue("ПРЕДПРИЯТИЕ", "ПІДПРИЄМСТВО") || npt.Noun.IsValue("ОРГАНИЗАЦИЯ", "ОРГАНІЗАЦІЯ")) || npt.Noun.IsValue("КОМПЛЕКС", null)) 
                    {
                        t1 = (t = npt.EndToken);
                        ok = true;
                        continue;
                    }
                }
                Pullenti.Ner.Org.Internal.OrgItemTypeToken ty = Pullenti.Ner.Org.Internal.OrgItemTypeToken.TryAttach(t, true, null);
                if (ty != null) 
                {
                    t1 = (t = ty.EndToken);
                    ok = true;
                    continue;
                }
                if ((t is Pullenti.Ner.TextToken) && t.Chars.IsCyrillicLetter && t.Chars.IsAllLower) 
                {
                    if (t.Morph.Class == Pullenti.Morph.MorphClass.Verb || t.Morph.Class == Pullenti.Morph.MorphClass.Adverb) 
                        break;
                }
                if (t.IsNewlineBefore && typ == null) 
                    break;
                else if (!t.Morph.Class.IsPreposition && !t.Morph.Class.IsConjunction) 
                    t1 = t;
                else if (t.IsNewlineBefore) 
                    break;
            }
            if (!ok) 
                return null;
            Pullenti.Ner.Instrument.InstrumentParticipantReferent pat = new Pullenti.Ner.Instrument.InstrumentParticipantReferent() { Typ = typ };
            foreach (Pullenti.Ner.Referent r in refs) 
            {
                pat.AddSlot(Pullenti.Ner.Instrument.InstrumentParticipantReferent.ATTR_REF, r, false, 0);
            }
            return new Pullenti.Ner.ReferentToken(pat, t0, t1);
        }
        void _createJusticeResolution()
        {
            Pullenti.Ner.Core.AnalyzerData ad = Kit.GetAnalyzerDataByAnalyzerName(Pullenti.Ner.Instrument.InstrumentAnalyzer.ANALYZER_NAME);
            if (ad == null) 
                return;
            List<FragToken> res = this._findResolution();
            if (res == null) 
                return;
            foreach (FragToken r in res) 
            {
                for (Pullenti.Ner.Token t = r.BeginToken; t != null && t.EndChar <= r.EndChar; t = t.Next) 
                {
                    if (t == r.BeginToken) 
                    {
                    }
                    else if (t.Previous != null && t.Previous.IsChar('.') && t.IsWhitespaceBefore) 
                    {
                    }
                    else if (!t.IsValue("ПРИЗНАТЬ", "ВИЗНАТИ")) 
                        continue;
                    if (t.Morph.Class.IsPreposition && t.Next != null) 
                        t = t.Next;
                    List<Pullenti.Ner.ReferentToken> arts = new List<Pullenti.Ner.ReferentToken>();
                    Pullenti.Ner.Token tt = null;
                    Pullenti.Ner.Token te = null;
                    if (t.IsValue("ВЗЫСКАТЬ", "СТЯГНУТИ")) 
                    {
                        bool gosposh = false;
                        Pullenti.Ner.Money.MoneyReferent sum = null;
                        te = null;
                        for (tt = t.Next; tt != null && tt.EndChar <= r.EndChar; tt = tt.Next) 
                        {
                            if (tt.Morph.Class.IsPreposition) 
                                continue;
                            if (tt.IsChar('.')) 
                                break;
                            if (tt.IsValue("ТОМ", "ТОМУ") && tt.Next != null && tt.Next.IsValue("ЧИСЛО", null)) 
                                break;
                            if (tt.IsValue("ГОСПОШЛИНА", "ДЕРЖМИТО")) 
                                gosposh = true;
                            else if (tt.IsValue("ФЕДЕРАЛЬНЫЙ", "ФЕДЕРАЛЬНИЙ") && tt.Next != null && tt.Next.IsValue("БЮДЖЕТ", null)) 
                                gosposh = true;
                            if (tt.GetReferent() is Pullenti.Ner.Money.MoneyReferent) 
                            {
                                te = tt;
                                sum = tt.GetReferent() as Pullenti.Ner.Money.MoneyReferent;
                            }
                        }
                        if (sum != null) 
                        {
                            Pullenti.Ner.Instrument.InstrumentArtefactReferent art = new Pullenti.Ner.Instrument.InstrumentArtefactReferent() { Typ = "РЕЗОЛЮЦИЯ" };
                            if (gosposh) 
                                art.Value = "ВЗЫСКАТЬ ГОСПОШЛИНУ";
                            else 
                                art.Value = "ВЗЫСКАТЬ СУММУ";
                            art.AddSlot(Pullenti.Ner.Instrument.InstrumentArtefactReferent.ATTR_REF, sum, false, 0);
                            arts.Add(new Pullenti.Ner.ReferentToken(art, t, te));
                        }
                    }
                    if ((t.IsValue("ЗАЯВЛЕНИЕ", "ЗАЯВА") || t.IsValue("ИСК", "ПОЗОВ") || t.IsValue("ТРЕБОВАНИЕ", "ВИМОГА")) || t.IsValue("ЗАЯВЛЕННЫЙ", "ЗАЯВЛЕНИЙ") || t.IsValue("УДОВЛЕТВОРЕНИЕ", "ЗАДОВОЛЕННЯ")) 
                    {
                        for (tt = t.Next; tt != null && tt.EndChar <= r.EndChar; tt = tt.Next) 
                        {
                            if (tt.Morph.Class.IsPreposition) 
                                continue;
                            if (tt.IsChar('.')) 
                            {
                                if (tt.IsWhitespaceAfter) 
                                    break;
                            }
                            if (tt.IsValue("УДОВЛЕТВОРИТЬ", "ЗАДОВОЛЬНИТИ")) 
                            {
                                string val = "УДОВЛЕТВОРИТЬ";
                                te = tt;
                                if (tt.Next != null && tt.Next.IsValue("ПОЛНОСТЬЮ", "ПОВНІСТЮ")) 
                                {
                                    val += " ПОЛНОСТЬЮ";
                                    te = tt.Next;
                                }
                                else if (tt.Previous != null && tt.Previous.IsValue("ПОЛНОСТЬЮ", "ПОВНІСТЮ")) 
                                    val += " ПОЛНОСТЬЮ";
                                Pullenti.Ner.Instrument.InstrumentArtefactReferent art = new Pullenti.Ner.Instrument.InstrumentArtefactReferent() { Typ = "РЕЗОЛЮЦИЯ" };
                                art.Value = val;
                                arts.Add(new Pullenti.Ner.ReferentToken(art, t, te));
                                break;
                            }
                            if (tt.IsValue("ОТКАЗАТЬ", "ВІДМОВИТИ")) 
                            {
                                Pullenti.Ner.Instrument.InstrumentArtefactReferent art = new Pullenti.Ner.Instrument.InstrumentArtefactReferent() { Typ = "РЕЗОЛЮЦИЯ" };
                                art.Value = "ОТКАЗАТЬ";
                                arts.Add(new Pullenti.Ner.ReferentToken(art, t, (te = tt)));
                                break;
                            }
                        }
                    }
                    if (t.IsValue("ПРИЗНАТЬ", "ВИЗНАТИ")) 
                    {
                        int zak = -1;
                        int otm = -1;
                        for (tt = t.Next; tt != null && tt.EndChar <= r.EndChar; tt = tt.Next) 
                        {
                            if (tt.Morph.Class.IsPreposition) 
                                continue;
                            if (tt.IsChar('.')) 
                                break;
                            if (tt.IsValue("НЕЗАКОННЫЙ", "НЕЗАКОННИЙ")) 
                            {
                                zak = 0;
                                te = tt;
                            }
                            else if (tt.IsValue("ЗАКОННЫЙ", "ЗАКОННИЙ")) 
                            {
                                zak = 1;
                                te = tt;
                            }
                            else if (tt.IsValue("ОТМЕНИТЬ", "СКАСУВАТИ")) 
                            {
                                otm = 1;
                                te = tt;
                            }
                        }
                        if (zak >= 0) 
                        {
                            string val = string.Format("ПРИЗНАТЬ {0}", (zak > 0 ? "ЗАКОННЫМ" : "НЕЗАКОННЫМ"));
                            if (otm > 0) 
                                val += " И ОТМЕНИТЬ";
                            Pullenti.Ner.Instrument.InstrumentArtefactReferent art = new Pullenti.Ner.Instrument.InstrumentArtefactReferent() { Typ = "РЕЗОЛЮЦИЯ" };
                            art.Value = val;
                            arts.Add(new Pullenti.Ner.ReferentToken(art, t, te));
                        }
                        else 
                            continue;
                    }
                    foreach (Pullenti.Ner.ReferentToken rt in arts) 
                    {
                        rt.Referent = ad.RegisterReferent(rt.Referent);
                        m_Doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_ARTEFACT, rt.Referent, false, 0);
                        if (r.BeginToken == rt.BeginToken) 
                            r.BeginToken = rt;
                        if (r.EndToken == rt.EndToken) 
                            r.EndToken = rt;
                        Kit.EmbedToken(rt);
                        t = rt;
                    }
                }
            }
        }
        List<FragToken> _findResolution()
        {
            if (Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix) 
                return null;
            bool dir = false;
            List<FragToken> res = null;
            for (int i = 0; i < Children.Count; i++) 
            {
                if (Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Directive && ((i + 1) < Children.Count)) 
                {
                    object v = Children[i].Value;
                    if (v == null) 
                        continue;
                    string s = v.ToString();
                    if ((((s == "РЕШЕНИЕ" || s == "ОПРЕДЕЛЕНИЕ" || s == "ПОСТАНОВЛЕНИЕ") || s == "ПРИГОВОР" || s == "РІШЕННЯ") || s == "ВИЗНАЧЕННЯ" || s == "ПОСТАНОВА") || s == "ВИРОК") 
                    {
                        int ii = i + 1;
                        for (int j = ii + 1; j < Children.Count; j++) 
                        {
                            ii = j;
                        }
                        if (res == null) 
                            res = new List<FragToken>();
                        if (ii == (i + 1)) 
                            res.Add(Children[i + 1]);
                        else 
                            res.Add(new FragToken(Children[i + 1].BeginToken, Children[ii].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
                    }
                    else 
                        dir = true;
                }
            }
            if (res != null) 
                return res;
            if (dir) 
                return null;
            foreach (FragToken ch in Children) 
            {
                List<FragToken> re = ch._findResolution();
                if (re != null) 
                {
                    if (res == null) 
                        res = re;
                    else 
                        res.AddRange(re);
                }
            }
            return res;
        }
        static FragToken __createActionQuestion(Pullenti.Ner.Token t, int maxChar)
        {
            List<InstrToken> li = new List<InstrToken>();
            bool ok = false;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                InstrToken it = InstrToken.Parse(tt, maxChar, null);
                if (it == null) 
                    break;
                li.Add(it);
                if (li.Count > 5) 
                    return null;
                if (it.Typ == ILTypes.Question) 
                {
                    ok = true;
                    break;
                }
                tt = it.EndToken;
            }
            if (!ok) 
                return null;
            Pullenti.Ner.Token t1 = li[li.Count - 1].EndToken;
            List<InstrToken> li2 = new List<InstrToken>();
            ok = false;
            for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
            {
                if (!tt.IsNewlineBefore) 
                    continue;
                InstrToken it = InstrToken.Parse(tt, maxChar, null);
                if (it == null) 
                    break;
                li2.Add(it);
                tt = it.EndToken;
                if (it.Typ != ILTypes.Typ) 
                    continue;
                InstrToken1 it1 = InstrToken1.Parse(tt, true, null, 0, null, false, maxChar, false, false);
                if (it1 != null && it1.HasVerb) 
                {
                    tt = it1.EndToken;
                    continue;
                }
                Pullenti.Ner.Token tt2 = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(it.BeginToken, false);
                if (tt2 != null && tt2 == it.EndToken) 
                {
                    ok = true;
                    break;
                }
            }
            if (!ok) 
                return null;
            Pullenti.Ner.Token t2 = li2[li2.Count - 1].BeginToken;
            while (li2.Count > 1 && li2[li2.Count - 2].Typ == ILTypes.Organization) 
            {
                t2 = li2[li2.Count - 2].BeginToken;
                li2.RemoveAt(li2.Count - 1);
            }
            FragToken res = CreateDocument(t2, maxChar, Pullenti.Ner.Instrument.InstrumentKind.Undefined);
            if (res == null) 
                return null;
            FragToken ques = new FragToken(t, t2.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Question };
            res.Children.Insert(0, ques);
            ques.Children.Add(new FragToken(li[li.Count - 1].BeginToken, li[li.Count - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword });
            FragToken content = new FragToken(li[li.Count - 1].EndToken.Next, t2.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content };
            ques.Children.Add(content);
            content._analizeContent(res, maxChar > 0, Pullenti.Ner.Instrument.InstrumentKind.Undefined);
            if (li.Count > 1) 
            {
                FragToken fr = new FragToken(t, li[li.Count - 2].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal = true };
                ques.Children.Insert(0, fr);
            }
            res.BeginToken = t;
            return res;
        }
        static FragToken CreateGostTitle(Pullenti.Ner.Token t0, Pullenti.Ner.Instrument.InstrumentReferent doc)
        {
            if (t0 == null) 
                return null;
            bool ok = false;
            if (t0.IsTableControlChar && t0.Next != null) 
                t0 = t0.Next;
            Pullenti.Ner.Token t;
            int cou = 0;
            for (t = t0; t != null && (cou < 300); t = t.Next,cou++) 
            {
                Pullenti.Ner.Decree.DecreeReferent dr = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                if (dr != null) 
                {
                    if (dr.Kind == Pullenti.Ner.Decree.DecreeKind.Standard) 
                    {
                        t = t.Kit.DebedToken(t);
                        if (t.BeginChar == t0.BeginChar) 
                            t0 = t;
                        ok = true;
                    }
                    break;
                }
                if (t.IsTableControlChar) 
                    continue;
                if (t.IsNewlineBefore || ((t.Previous != null && t.Previous.IsTableControlChar))) 
                {
                    if (_isStartOfBody(t, false)) 
                        break;
                    Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                    if (dt != null) 
                    {
                        if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            if (dt.TypKind == Pullenti.Ner.Decree.DecreeKind.Standard) 
                                ok = true;
                            break;
                        }
                    }
                }
            }
            if (!ok) 
                return null;
            FragToken title = new FragToken(t0, t0) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Head };
            cou = 0;
            bool hasNum = false;
            for (t = t0; t != null && (cou < 100); t = t.Next) 
            {
                if (t.IsNewlineBefore && t != t0) 
                {
                    title.EndToken = t.Previous;
                    if (t.IsValue("ЧАСТЬ", null)) 
                    {
                        t = t.Next;
                        Pullenti.Ner.Token tt1 = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t);
                        if (tt1 != null) 
                            t = tt1;
                        if (t is Pullenti.Ner.NumberToken) 
                        {
                            StringBuilder tmp = new StringBuilder();
                            for (; t != null; t = t.Next) 
                            {
                                if (t is Pullenti.Ner.NumberToken) 
                                    tmp.Append((t as Pullenti.Ner.NumberToken).Value);
                                else if (((t.IsHiphen || t.IsChar('.'))) && !t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.NumberToken)) 
                                    tmp.Append((t as Pullenti.Ner.TextToken).Term);
                                else 
                                    break;
                                if (t.IsWhitespaceAfter) 
                                    break;
                            }
                            doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_PART, tmp.ToString(), true, 0);
                        }
                        continue;
                    }
                    if (_isStartOfBody(t, false)) 
                        break;
                    cou++;
                }
                if (!hasNum) 
                {
                    Pullenti.Ner.Decree.DecreeReferent dr = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                    if (dr != null && dr.Kind == Pullenti.Ner.Decree.DecreeKind.Standard) 
                        t = t.Kit.DebedToken(t);
                }
                title.EndToken = t;
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                if (dt == null) 
                    continue;
                if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    if (dt.TypKind != Pullenti.Ner.Decree.DecreeKind.Standard) 
                        continue;
                    _addTitleAttr(doc, title, dt);
                    t = dt.EndToken;
                    if (!hasNum) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeToken num = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t.Next, dt, false);
                        if (num != null && num.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        {
                            _addTitleAttr(doc, title, num);
                            if (num.NumYear > 0) 
                                doc.AddSlot(Pullenti.Ner.Instrument.InstrumentReferent.ATTR_DATE, num.NumYear, false, 0);
                            t = dt.EndToken;
                            hasNum = true;
                        }
                    }
                    continue;
                }
            }
            title.Tag = Pullenti.Ner.Decree.DecreeKind.Standard;
            return title;
        }
        public void _analizeContent(FragToken topDoc, bool isCitat, Pullenti.Ner.Instrument.InstrumentKind rootKind = Pullenti.Ner.Instrument.InstrumentKind.Undefined)
        {
            Kind = Pullenti.Ner.Instrument.InstrumentKind.Content;
            if (BeginToken.Previous != null && BeginToken.Previous.IsChar((char)0x1E)) 
                BeginToken = BeginToken.Previous;
            ContentAnalyzeWhapper wr = new ContentAnalyzeWhapper();
            wr.Analyze(this, topDoc, isCitat, rootKind);
            foreach (FragToken ch in topDoc.Children) 
            {
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Head) 
                {
                    foreach (FragToken chh in ch.Children) 
                    {
                        if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions && chh.Referents != null) 
                        {
                            if (topDoc.Referents == null) 
                                topDoc.Referents = new List<Pullenti.Ner.Referent>();
                            foreach (Pullenti.Ner.Referent r in chh.Referents) 
                            {
                                if (!topDoc.Referents.Contains(r)) 
                                    topDoc.Referents.Add(r);
                            }
                        }
                    }
                }
            }
        }
    }
}