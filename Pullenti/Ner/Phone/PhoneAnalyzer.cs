/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Phone
{
    /// <summary>
    /// Анализатор для выделения телефонных номеров
    /// </summary>
    public class PhoneAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("PHONE")
        /// </summary>
        public const string ANALYZER_NAME = "PHONE";
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
                return "Телефоны";
            }
        }
        public override string Description
        {
            get
            {
                return "Телефонные номера";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new PhoneAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Phone.Internal.MetaPhone.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Phone.Internal.MetaPhone.PhoneImageId, Pullenti.Ner.Bank.Internal.ResourceHelper.GetBytes("phone.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == PhoneReferent.OBJ_TYPENAME) 
                return new PhoneReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 2;
            }
        }
        class PhoneAnalizerData : Pullenti.Ner.Core.AnalyzerData
        {
            Dictionary<string, List<Pullenti.Ner.Phone.PhoneReferent>> m_PhonesHash = new Dictionary<string, List<Pullenti.Ner.Phone.PhoneReferent>>();
            public override Pullenti.Ner.Referent RegisterReferent(Pullenti.Ner.Referent referent)
            {
                Pullenti.Ner.Phone.PhoneReferent phone = referent as Pullenti.Ner.Phone.PhoneReferent;
                if (phone == null) 
                    return null;
                string key = phone.Number;
                if (key.Length >= 10) 
                    key = key.Substring(3);
                List<Pullenti.Ner.Phone.PhoneReferent> phLi;
                if (!m_PhonesHash.TryGetValue(key, out phLi)) 
                    m_PhonesHash.Add(key, (phLi = new List<Pullenti.Ner.Phone.PhoneReferent>()));
                foreach (Pullenti.Ner.Phone.PhoneReferent p in phLi) 
                {
                    if (p.CanBeEquals(phone, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                    {
                        p.MergeSlots(phone, true);
                        return p;
                    }
                }
                phLi.Add(phone);
                m_Referents.Add(phone);
                return phone;
            }
        }

        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new PhoneAnalizerData();
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            PhoneAnalizerData ad = kit.GetAnalyzerData(this) as PhoneAnalizerData;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                List<Pullenti.Ner.Phone.Internal.PhoneItemToken> pli = Pullenti.Ner.Phone.Internal.PhoneItemToken.TryAttachAll(t, 15);
                if (pli == null || pli.Count == 0) 
                    continue;
                PhoneReferent prevPhone = null;
                int kkk = 0;
                for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                {
                    if (tt.GetReferent() is PhoneReferent) 
                    {
                        prevPhone = tt.GetReferent() as PhoneReferent;
                        break;
                    }
                    else if (tt is Pullenti.Ner.ReferentToken) 
                    {
                    }
                    else if (tt.IsChar(')')) 
                    {
                        Pullenti.Ner.Token ttt = tt.Previous;
                        int cou = 0;
                        for (; ttt != null; ttt = ttt.Previous) 
                        {
                            if (ttt.IsChar('(')) 
                                break;
                            else if ((++cou) > 100) 
                                break;
                        }
                        if (ttt == null || !ttt.IsChar('(')) 
                            break;
                        tt = ttt;
                    }
                    else if (!tt.IsCharOf(",;/\\") && !tt.IsAnd) 
                    {
                        if ((++kkk) > 5) 
                            break;
                        if (tt.IsNewlineBefore || tt.IsNewlineAfter) 
                            break;
                    }
                }
                int j = 0;
                bool isPhoneBefore = false;
                bool isPref = false;
                PhoneKind ki = PhoneKind.Undefined;
                while (j < pli.Count) 
                {
                    if (pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix) 
                    {
                        if (ki == PhoneKind.Undefined) 
                            ki = pli[j].Kind;
                        isPref = true;
                        isPhoneBefore = true;
                        j++;
                        if ((j < pli.Count) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) 
                            j++;
                    }
                    else if (((j + 1) < pli.Count) && pli[j + 1].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix && j == 0) 
                    {
                        if (ki == PhoneKind.Undefined) 
                            ki = pli[0].Kind;
                        isPref = true;
                        pli.RemoveAt(0);
                    }
                    else 
                        break;
                }
                if (prevPhone != null) 
                    isPhoneBefore = true;
                if (pli.Count == 1 && pli[0].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) 
                {
                    Pullenti.Ner.Token tt = t.Previous;
                    if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsLetter) 
                        tt = tt.Previous;
                    if (tt is Pullenti.Ner.TextToken) 
                    {
                        if (Pullenti.Ner.Uri.UriAnalyzer.m_Schemes.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                            continue;
                    }
                }
                List<Pullenti.Ner.ReferentToken> rts = this.TryAttach(pli, j, isPhoneBefore, prevPhone);
                if (rts == null) 
                {
                    for (j = 1; j < pli.Count; j++) 
                    {
                        if (pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix) 
                        {
                            pli.RemoveRange(0, j);
                            rts = this.TryAttach(pli, 1, true, prevPhone);
                            break;
                        }
                    }
                }
                if (rts == null) 
                    t = pli[pli.Count - 1].EndToken;
                else 
                {
                    if ((ki == PhoneKind.Undefined && prevPhone != null && !isPref) && prevPhone.Kind != PhoneKind.Mobile && kkk == 0) 
                        ki = prevPhone.Kind;
                    foreach (Pullenti.Ner.ReferentToken rt in rts) 
                    {
                        PhoneReferent ph = rt.Referent as PhoneReferent;
                        if (ki != PhoneKind.Undefined) 
                            ph.Kind = ki;
                        else 
                        {
                            if (rt == rts[0] && (rt.WhitespacesBeforeCount < 3)) 
                            {
                                Pullenti.Ner.Token tt1 = rt.BeginToken.Previous;
                                if (tt1 != null && tt1.IsTableControlChar) 
                                    tt1 = tt1.Previous;
                                if ((tt1 is Pullenti.Ner.TextToken) && ((tt1.IsNewlineBefore || ((tt1.Previous != null && tt1.Previous.IsTableControlChar))))) 
                                {
                                    string term = (tt1 as Pullenti.Ner.TextToken).Term;
                                    if (term == "T" || term == "Т") 
                                        rt.BeginToken = tt1;
                                    else if (term == "Ф" || term == "F") 
                                    {
                                        ph.Kind = (ki = PhoneKind.Fax);
                                        rt.BeginToken = tt1;
                                    }
                                    else if (term == "M" || term == "М") 
                                    {
                                        ph.Kind = (ki = PhoneKind.Mobile);
                                        rt.BeginToken = tt1;
                                    }
                                }
                            }
                            ph.Correct();
                        }
                        rt.Referent = ad.RegisterReferent(rt.Referent);
                        kit.EmbedToken(rt);
                        t = rt;
                    }
                }
            }
        }
        List<Pullenti.Ner.ReferentToken> TryAttach(List<Pullenti.Ner.Phone.Internal.PhoneItemToken> pli, int ind, bool isPhoneBefore, PhoneReferent prevPhone)
        {
            Pullenti.Ner.ReferentToken rt = this._TryAttach_(pli, ind, isPhoneBefore, prevPhone, 0);
            if (rt == null) 
                return null;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            res.Add(rt);
            for (int i = 0; i < 5; i++) 
            {
                PhoneReferent ph0 = rt.Referent as PhoneReferent;
                if (ph0.AddNumber != null) 
                    return res;
                Pullenti.Ner.Phone.Internal.PhoneItemToken alt = Pullenti.Ner.Phone.Internal.PhoneItemToken.TryAttachAlternate(rt.EndToken.Next, ph0, pli);
                if (alt == null) 
                    break;
                PhoneReferent ph = new PhoneReferent();
                foreach (Pullenti.Ner.Slot s in rt.Referent.Slots) 
                {
                    ph.AddSlot(s.TypeName, s.Value, false, 0);
                }
                string num = ph.Number;
                if (num == null || num.Length <= alt.Value.Length) 
                    break;
                ph.Number = num.Substring(0, num.Length - alt.Value.Length) + alt.Value;
                ph.m_Template = ph0.m_Template;
                Pullenti.Ner.ReferentToken rt2 = new Pullenti.Ner.ReferentToken(ph, alt.BeginToken, alt.EndToken);
                res.Add(rt2);
                rt = rt2;
            }
            Pullenti.Ner.Phone.Internal.PhoneItemToken add = Pullenti.Ner.Phone.Internal.PhoneItemToken.TryAttachAdditional(rt.EndToken.Next);
            if (add != null) 
            {
                foreach (Pullenti.Ner.ReferentToken rr in res) 
                {
                    (rr.Referent as PhoneReferent).AddNumber = add.Value;
                }
                res[res.Count - 1].EndToken = add.EndToken;
            }
            return res;
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            List<Pullenti.Ner.Phone.Internal.PhoneItemToken> pli = Pullenti.Ner.Phone.Internal.PhoneItemToken.TryAttachAll(begin, 15);
            if (pli == null || pli.Count == 0) 
                return null;
            int i = 0;
            for (; i < pli.Count; i++) 
            {
                if (pli[i].ItemType != Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix) 
                    break;
            }
            Pullenti.Ner.ReferentToken rt = this._TryAttach_(pli, i, true, null, 0);
            if (rt != null) 
            {
                rt.BeginToken = begin;
                return rt;
            }
            return null;
        }
        Pullenti.Ner.ReferentToken _TryAttach_(List<Pullenti.Ner.Phone.Internal.PhoneItemToken> pli, int ind, bool isPhoneBefore, PhoneReferent prevPhone, int lev = 0)
        {
            if (ind >= pli.Count || lev > 4) 
                return null;
            string countryCode = null;
            string cityCode = null;
            int j = ind;
            if (prevPhone != null && prevPhone.m_Template != null && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) 
            {
                StringBuilder tmp = new StringBuilder();
                for (int jj = j; jj < pli.Count; jj++) 
                {
                    if (pli[jj].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) 
                        tmp.Append(pli[jj].Value.Length);
                    else if (pli[jj].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) 
                    {
                        if (pli[jj].Value == " ") 
                            break;
                        tmp.Append(pli[jj].Value);
                        continue;
                    }
                    else 
                        break;
                    string templ0 = tmp.ToString();
                    if (templ0 == prevPhone.m_Template) 
                    {
                        if ((jj + 1) < pli.Count) 
                        {
                            if (pli[jj + 1].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix && (jj + 2) == pli.Count) 
                            {
                            }
                            else 
                                pli.RemoveRange(jj + 1, pli.Count - jj - 1);
                        }
                        break;
                    }
                }
            }
            if ((j < pli.Count) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.CountryCode) 
            {
                countryCode = pli[j].Value;
                if (countryCode != "8") 
                {
                    string cc = Pullenti.Ner.Phone.Internal.PhoneHelper.GetCountryPrefix(countryCode);
                    if (cc != null && (cc.Length < countryCode.Length)) 
                    {
                        cityCode = countryCode.Substring(cc.Length);
                        countryCode = cc;
                    }
                }
                j++;
            }
            else if ((j < pli.Count) && pli[j].CanBeCountryPrefix) 
            {
                int k = j + 1;
                if ((k < pli.Count) && pli[k].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) 
                    k++;
                Pullenti.Ner.ReferentToken rrt = this._TryAttach_(pli, k, isPhoneBefore, null, lev + 1);
                if (rrt != null) 
                {
                    if ((((isPhoneBefore && pli[j + 1].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim && pli[j + 1].BeginToken.IsHiphen) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number && pli[j].Value.Length == 3) && ((j + 2) < pli.Count) && pli[j + 2].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) && pli[j + 2].Value.Length == 3) 
                    {
                    }
                    else 
                    {
                        countryCode = pli[j].Value;
                        j++;
                    }
                }
            }
            if (((j < pli.Count) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number && ((pli[j].Value[0] == '8' || pli[j].Value[0] == '7'))) && countryCode == null) 
            {
                if (pli[j].Value.Length == 1) 
                {
                    countryCode = pli[j].Value;
                    j++;
                }
                else if (pli[j].Value.Length == 4) 
                {
                    countryCode = pli[j].Value.Substring(0, 1);
                    if (cityCode == null) 
                        cityCode = pli[j].Value.Substring(1);
                    else 
                        cityCode += pli[j].Value.Substring(1);
                    j++;
                }
                else if (pli[j].Value.Length == 11 && j == (pli.Count - 1) && isPhoneBefore) 
                {
                    PhoneReferent ph0 = new PhoneReferent();
                    if (pli[j].Value[0] != '8') 
                        ph0.CountryCode = pli[j].Value.Substring(0, 1);
                    ph0.Number = pli[j].Value.Substring(1, 3) + pli[j].Value.Substring(4);
                    return new Pullenti.Ner.ReferentToken(ph0, pli[0].BeginToken, pli[j].EndToken);
                }
                else if (cityCode == null && pli[j].Value.Length > 3 && ((j + 1) < pli.Count)) 
                {
                    int sum = 0;
                    foreach (Pullenti.Ner.Phone.Internal.PhoneItemToken it in pli) 
                    {
                        if (it.ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) 
                            sum += it.Value.Length;
                    }
                    if (sum == 11) 
                    {
                        cityCode = pli[j].Value.Substring(1);
                        j++;
                    }
                }
            }
            if ((j < pli.Count) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.CityCode) 
            {
                if (cityCode == null) 
                    cityCode = pli[j].Value;
                else 
                    cityCode += pli[j].Value;
                j++;
            }
            if ((j < pli.Count) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) 
                j++;
            if ((countryCode == "8" && cityCode == null && ((j + 3) < pli.Count)) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) 
            {
                if (pli[j].Value.Length == 3 || pli[j].Value.Length == 4) 
                {
                    cityCode = pli[j].Value;
                    j++;
                    if ((j < pli.Count) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) 
                        j++;
                }
            }
            int normalNumLen = 0;
            if (countryCode == "421") 
                normalNumLen = 9;
            StringBuilder num = new StringBuilder();
            StringBuilder templ = new StringBuilder();
            List<int> partLength = new List<int>();
            string delim = null;
            bool ok = false;
            string additional = null;
            bool std = false;
            if (countryCode != null && ((j + 4) < pli.Count) && j > 0) 
            {
                if (((((pli[j - 1].Value == "-" || pli[j - 1].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.CountryCode)) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number && pli[j + 1].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) && pli[j + 2].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number && pli[j + 3].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) && pli[j + 4].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) 
                {
                    if ((((pli[j].Value.Length + pli[j + 2].Value.Length) == 6 || ((pli[j].Value.Length == 4 && pli[j + 2].Value.Length == 5)))) && ((pli[j + 4].Value.Length == 4 || pli[j + 4].Value.Length == 1))) 
                    {
                        num.Append(pli[j].Value);
                        num.Append(pli[j + 2].Value);
                        num.Append(pli[j + 4].Value);
                        templ.AppendFormat("{0}{1}{2}{3}{4}", pli[j].Value.Length, pli[j + 1].Value, pli[j + 2].Value.Length, pli[j + 3].Value, pli[j + 4].Value.Length);
                        std = true;
                        ok = true;
                        j += 5;
                    }
                }
            }
            for (; j < pli.Count; j++) 
            {
                if (std) 
                    break;
                if (pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) 
                {
                    if (pli[j].IsInBrackets) 
                        continue;
                    if (j > 0 && pli[j - 1].IsInBrackets) 
                        continue;
                    if (templ.Length > 0) 
                        templ.Append(pli[j].Value);
                    if (delim == null) 
                        delim = pli[j].Value;
                    else if (pli[j].Value != delim) 
                    {
                        if ((partLength.Count == 2 && ((partLength[0] == 3 || partLength[0] == 4)) && cityCode == null) && partLength[1] == 3) 
                        {
                            cityCode = num.ToString().Substring(0, partLength[0]);
                            num.Remove(0, partLength[0]);
                            partLength.RemoveAt(0);
                            delim = pli[j].Value;
                            continue;
                        }
                        if (isPhoneBefore && ((j + 1) < pli.Count) && pli[j + 1].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) 
                        {
                            if (num.Length < 6) 
                                continue;
                            if (normalNumLen > 0 && (num.Length + pli[j + 1].Value.Length) == normalNumLen) 
                                continue;
                        }
                        break;
                    }
                    else 
                        continue;
                    ok = false;
                }
                else if (pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Number) 
                {
                    if (num.Length == 0 && pli[j].BeginToken.Previous != null && pli[j].BeginToken.Previous.IsTableControlChar) 
                    {
                        Pullenti.Ner.Token tt = pli[pli.Count - 1].EndToken.Next;
                        if (tt != null && tt.IsCharOf(",.")) 
                            tt = tt.Next;
                        if (tt is Pullenti.Ner.NumberToken) 
                            return null;
                    }
                    if ((num.Length + pli[j].Value.Length) > 13) 
                    {
                        if (j > 0 && pli[j - 1].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Delim) 
                            j--;
                        ok = true;
                        break;
                    }
                    num.Append(pli[j].Value);
                    partLength.Add(pli[j].Value.Length);
                    templ.Append(pli[j].Value.Length);
                    ok = true;
                    if (num.Length > 10) 
                    {
                        j++;
                        if ((j < pli.Count) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.AddNumber) 
                        {
                            additional = pli[j].Value;
                            j++;
                        }
                        break;
                    }
                }
                else if (pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.AddNumber) 
                {
                    additional = pli[j].Value;
                    j++;
                    break;
                }
                else 
                    break;
            }
            if ((j == (pli.Count - 1) && pli[j].IsInBrackets && ((pli[j].Value.Length == 3 || pli[j].Value.Length == 4))) && additional == null) 
            {
                additional = pli[j].Value;
                j++;
            }
            if ((j < pli.Count) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix && pli[j].IsInBrackets) 
            {
                isPhoneBefore = true;
                j++;
            }
            if ((countryCode == null && cityCode != null && cityCode.Length > 3) && (num.Length < 8) && cityCode[0] != '8') 
            {
                if ((cityCode.Length + num.Length) == 10) 
                {
                }
                else 
                {
                    string cc = Pullenti.Ner.Phone.Internal.PhoneHelper.GetCountryPrefix(cityCode);
                    if (cc != null) 
                    {
                        if (cc.Length > 1 && (cityCode.Length - cc.Length) > 1) 
                        {
                            countryCode = cc;
                            cityCode = cityCode.Substring(cc.Length);
                        }
                    }
                }
            }
            if (countryCode == null && cityCode != null && cityCode.StartsWith("00")) 
            {
                string cc = Pullenti.Ner.Phone.Internal.PhoneHelper.GetCountryPrefix(cityCode.Substring(2));
                if (cc != null) 
                {
                    if (cityCode.Length > (cc.Length + 3)) 
                    {
                        countryCode = cc;
                        cityCode = cityCode.Substring(cc.Length + 2);
                    }
                }
            }
            if (num.Length == 0 && cityCode != null) 
            {
                if (cityCode.Length == 10) 
                {
                    num.Append(cityCode.Substring(3));
                    partLength.Add(num.Length);
                    cityCode = cityCode.Substring(0, 3);
                    ok = true;
                }
                else if (((cityCode.Length == 9 || cityCode.Length == 11 || cityCode.Length == 8)) && ((isPhoneBefore || countryCode != null))) 
                {
                    num.Append(cityCode);
                    partLength.Add(num.Length);
                    cityCode = null;
                    ok = true;
                }
            }
            if (num.Length < 4) 
                ok = false;
            if (num.Length < 7) 
            {
                if (cityCode != null && (cityCode.Length + num.Length) > 7) 
                {
                    if (!isPhoneBefore && cityCode.Length == 3) 
                    {
                        int ii;
                        for (ii = 0; ii < partLength.Count; ii++) 
                        {
                            if (partLength[ii] == 3) 
                            {
                            }
                            else if (partLength[ii] > 3) 
                                break;
                            else if ((ii < (partLength.Count - 1)) || (partLength[ii] < 2)) 
                                break;
                        }
                        if (ii >= partLength.Count) 
                        {
                            if (countryCode == "61") 
                            {
                            }
                            else 
                                ok = false;
                        }
                    }
                }
                else if (((num.Length == 6 || num.Length == 5)) && ((partLength.Count >= 1 && partLength.Count <= 3)) && isPhoneBefore) 
                {
                    if (pli[0].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix && pli[0].Kind == PhoneKind.Home) 
                        ok = false;
                }
                else if (prevPhone != null && prevPhone.Number != null && ((prevPhone.Number.Length == num.Length || prevPhone.Number.Length == (num.Length + 3) || prevPhone.Number.Length == (num.Length + 4)))) 
                {
                }
                else if (num.Length > 4 && prevPhone != null && templ.ToString() == prevPhone.m_Template) 
                    ok = true;
                else 
                    ok = false;
            }
            if (delim == "." && countryCode == null && cityCode == null) 
                ok = false;
            if ((isPhoneBefore && countryCode == null && cityCode == null) && num.Length > 10) 
            {
                string cc = Pullenti.Ner.Phone.Internal.PhoneHelper.GetCountryPrefix(num.ToString());
                if (cc != null) 
                {
                    if ((num.Length - cc.Length) == 9) 
                    {
                        countryCode = cc;
                        num.Remove(0, cc.Length);
                        ok = true;
                    }
                }
            }
            if (ok) 
            {
                if (std) 
                {
                }
                else if (prevPhone != null && prevPhone.Number != null && (((prevPhone.Number.Length == num.Length || prevPhone.Number.Length == (num.Length + 3) || prevPhone.Number.Length == (num.Length + 4)) || prevPhone.m_Template == templ.ToString()))) 
                {
                }
                else if ((partLength.Count == 3 && partLength[0] == 3 && partLength[1] == 2) && partLength[2] == 2) 
                {
                }
                else if (partLength.Count == 3 && isPhoneBefore) 
                {
                }
                else if ((partLength.Count == 4 && ((partLength[0] + partLength[1]) == 3) && partLength[2] == 2) && partLength[3] == 2) 
                {
                }
                else if ((partLength.Count == 4 && partLength[0] == 3 && partLength[1] == 3) && partLength[2] == 2 && partLength[3] == 2) 
                {
                }
                else if (partLength.Count == 5 && (partLength[1] + partLength[2]) == 4 && (partLength[3] + partLength[4]) == 4) 
                {
                }
                else if (partLength.Count > 4) 
                    ok = false;
                else if (partLength.Count > 3 && cityCode != null) 
                    ok = false;
                else if ((isPhoneBefore || cityCode != null || countryCode != null) || additional != null) 
                    ok = true;
                else 
                {
                    ok = false;
                    if (((num.Length == 6 || num.Length == 7)) && (partLength.Count < 4) && j > 0) 
                    {
                        PhoneReferent nextPh = this.GetNextPhone(pli[j - 1].EndToken.Next, lev + 1);
                        if (nextPh != null) 
                        {
                            int d = nextPh.Number.Length - num.Length;
                            if (d == 0 || d == 3 || d == 4) 
                                ok = true;
                        }
                    }
                }
            }
            Pullenti.Ner.Token end = (j > 0 ? pli[j - 1].EndToken : null);
            if (end == null) 
                ok = false;
            if ((ok && cityCode == null && countryCode == null) && prevPhone == null && !isPhoneBefore) 
            {
                if (!end.IsWhitespaceAfter && end.Next != null) 
                {
                    Pullenti.Ner.Token tt = end.Next;
                    if (tt.IsCharOf(".,)") && tt.Next != null) 
                        tt = tt.Next;
                    if (!tt.IsWhitespaceBefore) 
                        ok = false;
                }
            }
            if (!ok) 
                return null;
            if (templ.Length > 0 && !char.IsDigit(templ[templ.Length - 1])) 
                templ.Length--;
            if ((countryCode == null && cityCode != null && cityCode.Length > 3) && num.Length > 6) 
            {
                string cc = Pullenti.Ner.Phone.Internal.PhoneHelper.GetCountryPrefix(cityCode);
                if (cc != null && ((cc.Length + 1) < cityCode.Length)) 
                {
                    countryCode = cc;
                    cityCode = cityCode.Substring(cc.Length);
                }
            }
            if (pli[0].BeginToken.Previous != null) 
            {
                if (pli[0].BeginToken.Previous.IsValue("ГОСТ", null) || pli[0].BeginToken.Previous.IsValue("ТУ", null)) 
                    return null;
            }
            PhoneReferent ph = new PhoneReferent();
            if (countryCode != null) 
                ph.CountryCode = countryCode;
            string number = num.ToString();
            if ((cityCode == null && num.Length > 7 && partLength.Count > 0) && (partLength[0] < 5)) 
            {
                cityCode = number.Substring(0, partLength[0]);
                number = number.Substring(partLength[0]);
            }
            if (cityCode == null && num.Length == 11 && num[0] == '8') 
            {
                cityCode = number.Substring(1, 3);
                number = number.Substring(4);
            }
            if (cityCode == null && num.Length == 10) 
            {
                cityCode = number.Substring(0, 3);
                number = number.Substring(3);
            }
            if (cityCode != null) 
                number = cityCode + number;
            else if (countryCode == null && prevPhone != null) 
            {
                bool ok1 = false;
                if (prevPhone.Number.Length >= (number.Length + 2)) 
                    ok1 = true;
                else if (templ.Length > 0 && prevPhone.m_Template != null && Pullenti.Morph.LanguageHelper.EndsWith(prevPhone.m_Template, templ.ToString())) 
                    ok1 = true;
                if (ok1 && prevPhone.Number.Length > number.Length) 
                    number = prevPhone.Number.Substring(0, prevPhone.Number.Length - number.Length) + number;
            }
            if (ph.CountryCode == null && prevPhone != null && prevPhone.CountryCode != null) 
            {
                if (prevPhone.Number.Length == number.Length) 
                    ph.CountryCode = prevPhone.CountryCode;
            }
            ok = false;
            foreach (char d in number) 
            {
                if (d != '0') 
                {
                    ok = true;
                    break;
                }
            }
            if (!ok) 
                return null;
            if (countryCode != null) 
            {
                if (number.Length < 7) 
                    return null;
            }
            else 
            {
                string s = Pullenti.Ner.Phone.Internal.PhoneHelper.GetCountryPrefix(number);
                if (s != null) 
                {
                    string num2 = number.Substring(s.Length);
                    if (num2.Length >= 10 && num2.Length <= 11) 
                    {
                        number = num2;
                        if (s != "7") 
                            ph.CountryCode = s;
                    }
                }
                if (number.Length == 8 && prevPhone == null) 
                    return null;
            }
            if (number.Length > 11) 
            {
                if ((number.Length < 14) && ((countryCode == "1" || countryCode == "43"))) 
                {
                }
                else 
                    return null;
            }
            ph.Number = number;
            if (additional != null) 
                ph.AddSlot(PhoneReferent.ATTR_ADDNUMBER, additional, true, 0);
            if (!isPhoneBefore && end.Next != null && !end.IsNewlineAfter) 
            {
                if (end.Next.IsCharOf("+=") || end.Next.IsHiphen) 
                    return null;
            }
            if (countryCode != null && countryCode == "7") 
            {
                if (number.Length != 10) 
                    return null;
            }
            ph.m_Template = templ.ToString();
            if (j == (pli.Count - 1) && pli[j].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix && !pli[j].IsNewlineBefore) 
            {
                end = pli[j].EndToken;
                if (pli[j].Kind != PhoneKind.Undefined) 
                    ph.Kind = pli[j].Kind;
            }
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(ph, pli[0].BeginToken, end);
            if (pli[0].ItemType == Pullenti.Ner.Phone.Internal.PhoneItemToken.PhoneItemType.Prefix && pli[0].EndToken.Next.IsTableControlChar) 
                res.BeginToken = pli[1].BeginToken;
            return res;
        }
        PhoneReferent GetNextPhone(Pullenti.Ner.Token t, int lev)
        {
            if (t != null && t.IsChar(',')) 
                t = t.Next;
            if (t == null || lev > 3) 
                return null;
            List<Pullenti.Ner.Phone.Internal.PhoneItemToken> its = Pullenti.Ner.Phone.Internal.PhoneItemToken.TryAttachAll(t, 15);
            if (its == null) 
                return null;
            Pullenti.Ner.ReferentToken rt = this._TryAttach_(its, 0, false, null, lev + 1);
            if (rt == null) 
                return null;
            return rt.Referent as PhoneReferent;
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Phone.Internal.MetaPhone.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                Pullenti.Ner.Phone.Internal.PhoneHelper.Initialize();
                Pullenti.Ner.Phone.Internal.PhoneItemToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new PhoneAnalyzer());
        }
    }
}