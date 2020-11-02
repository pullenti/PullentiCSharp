/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Pullenti.Ner.Geo.Internal
{
    public class CityItemToken : Pullenti.Ner.MetaToken
    {
        public CityItemToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public enum ItemType : int
        {
            ProperName,
            City,
            Noun,
            Misc,
        }

        /// <summary>
        /// Тип элемента
        /// </summary>
        public ItemType Typ;
        /// <summary>
        /// Строковое значение
        /// </summary>
        public string Value;
        /// <summary>
        /// Альтернативное значение
        /// </summary>
        public string AltValue;
        /// <summary>
        /// Ссылка на онтологический элемент (существующий город)
        /// </summary>
        public Pullenti.Ner.Core.IntOntologyItem OntoItem;
        /// <summary>
        /// Признак сомнительности
        /// </summary>
        public bool Doubtful;
        /// <summary>
        /// Есть ли перед элементом некоторый географический объект
        /// </summary>
        public bool GeoObjectBefore;
        /// <summary>
        /// Есть ли после элемента некоторый географический объект
        /// </summary>
        public bool GeoObjectAfter;
        /// <summary>
        /// Столица Чечни
        /// </summary>
        public Pullenti.Ner.Geo.GeoReferent HigherGeo;
        /// <summary>
        /// Это ссылка на организацию (для: посёлок НИИ Радио)
        /// </summary>
        public Pullenti.Ner.ReferentToken OrgRef;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0}", Typ.ToString());
            if (Value != null) 
                res.AppendFormat(" {0}", Value);
            if (OntoItem != null) 
                res.AppendFormat(" {0}", OntoItem.ToString());
            if (Doubtful) 
                res.Append(" (?)");
            if (OrgRef != null) 
                res.AppendFormat(" (Org: {0})", OrgRef.Referent);
            if (GeoObjectBefore) 
                res.Append(" GeoBefore");
            if (GeoObjectAfter) 
                res.Append(" GeoAfter");
            return res.ToString();
        }
        public bool MergeWithNext(CityItemToken ne)
        {
            if (Typ != ItemType.Noun || ne.Typ != ItemType.Noun) 
                return false;
            bool ok = false;
            if (Value == "ГОРОДСКОЕ ПОСЕЛЕНИЕ" && ne.Value == "ГОРОД") 
                ok = true;
            if (!ok) 
                return false;
            EndToken = ne.EndToken;
            Doubtful = false;
            return true;
        }
        public static List<CityItemToken> TryParseList(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection loc, int maxCount)
        {
            CityItemToken ci = CityItemToken.TryParse(t, loc, false, null);
            if (ci == null) 
            {
                if (t == null) 
                    return null;
                if (((t is Pullenti.Ner.TextToken) && t.IsValue("МУНИЦИПАЛЬНЫЙ", null) && t.Next != null) && t.Next.IsValue("ОБРАЗОВАНИЕ", null)) 
                {
                    Pullenti.Ner.Token t1 = t.Next.Next;
                    bool br = false;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t1, false, false)) 
                    {
                        br = true;
                        t1 = t1.Next;
                    }
                    List<CityItemToken> lii = TryParseList(t1, loc, maxCount);
                    if (lii != null && lii[0].Typ == ItemType.Noun) 
                    {
                        lii[0].BeginToken = t;
                        lii[0].Doubtful = false;
                        if (br && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(lii[lii.Count - 1].EndToken.Next, false, null, false)) 
                            lii[lii.Count - 1].EndToken = lii[lii.Count - 1].EndToken.Next;
                        return lii;
                    }
                }
                return null;
            }
            if (ci.Chars.IsLatinLetter && ci.Typ == ItemType.Noun && !t.Chars.IsAllLower) 
                return null;
            List<CityItemToken> li = new List<CityItemToken>();
            li.Add(ci);
            for (t = ci.EndToken.Next; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore) 
                {
                    if (li.Count == 1 && li[0].Typ == ItemType.Noun) 
                    {
                    }
                    else 
                        break;
                }
                CityItemToken ci0 = CityItemToken.TryParse(t, loc, false, ci);
                if (ci0 == null) 
                {
                    if (t.IsNewlineBefore) 
                        break;
                    if (ci.Typ == ItemType.Noun && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if ((br != null && (br.LengthChar < 50) && t.Next.Chars.IsCyrillicLetter) && !t.Next.Chars.IsAllLower) 
                        {
                            ci0 = new CityItemToken(br.BeginToken, br.EndToken) { Typ = ItemType.ProperName };
                            Pullenti.Ner.Token tt = br.EndToken.Previous;
                            string num = null;
                            if (tt is Pullenti.Ner.NumberToken) 
                            {
                                num = (tt as Pullenti.Ner.NumberToken).Value.ToString();
                                tt = tt.Previous;
                                if (tt != null && tt.IsHiphen) 
                                    tt = tt.Previous;
                            }
                            ci0.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken.Next, tt, Pullenti.Ner.Core.GetTextAttr.No);
                            if (tt != br.BeginToken.Next) 
                                ci0.AltValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken.Next, tt, Pullenti.Ner.Core.GetTextAttr.No);
                            if (string.IsNullOrEmpty(ci0.Value)) 
                                ci0 = null;
                            else if (num != null) 
                            {
                                ci0.Value = string.Format("{0}-{1}", ci0.Value, num);
                                if (ci0.AltValue != null) 
                                    ci0.AltValue = string.Format("{0}-{1}", ci0.AltValue, num);
                            }
                        }
                    }
                    if ((ci0 == null && ((ci.Typ == ItemType.ProperName || ci.Typ == ItemType.City)) && t.IsComma) && li[0] == ci) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null) 
                        {
                            for (Pullenti.Ner.Token tt = t.Next; tt != null && tt.EndChar <= npt.EndChar; tt = tt.Next) 
                            {
                                CityItemToken ci00 = CityItemToken.TryParse(tt, loc, false, ci);
                                if (ci00 != null && ci00.Typ == ItemType.Noun) 
                                {
                                    CityItemToken ci01 = CityItemToken.TryParse(ci00.EndToken.Next, loc, false, ci);
                                    if (ci01 == null) 
                                    {
                                        ci0 = ci00;
                                        ci0.AltValue = Pullenti.Ner.Core.MiscHelper.GetTextValue(t.Next, ci00.EndToken, (t.Kit.BaseLanguage.IsEn ? Pullenti.Ner.Core.GetTextAttr.IgnoreArticles : Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle)).ToLower();
                                        break;
                                    }
                                }
                                if (!tt.Chars.IsAllLower) 
                                    break;
                            }
                        }
                    }
                    if (ci0 == null) 
                        break;
                }
                if ((ci0.Typ == ItemType.Noun && ci0.Value != null && Pullenti.Morph.LanguageHelper.EndsWith(ci0.Value, "УСАДЬБА")) && ci.Typ == ItemType.Noun) 
                {
                    ci.Doubtful = false;
                    t = (ci.EndToken = ci0.EndToken);
                    continue;
                }
                if (ci0.Typ == ItemType.Noun && ci.Typ == ItemType.Misc && ci.Value == "АДМИНИСТРАЦИЯ") 
                    ci0.Doubtful = false;
                if (ci.MergeWithNext(ci0)) 
                {
                    t = ci.EndToken;
                    continue;
                }
                ci = ci0;
                li.Add(ci);
                t = ci.EndToken;
                if (maxCount > 0 && li.Count >= maxCount) 
                    break;
            }
            if (li.Count > 1 && li[0].Value == "СОВЕТ") 
                return null;
            if (li.Count > 2 && li[0].Typ == ItemType.Noun && li[1].Typ == ItemType.Noun) 
            {
                if (li[0].MergeWithNext(li[1])) 
                    li.RemoveAt(1);
            }
            if (li.Count > 2 && li[0].IsNewlineAfter) 
                li.RemoveRange(1, li.Count - 1);
            if (!li[0].GeoObjectBefore) 
                li[0].GeoObjectBefore = MiscLocationHelper.CheckGeoObjectBefore(li[0].BeginToken);
            if (!li[li.Count - 1].GeoObjectAfter) 
                li[li.Count - 1].GeoObjectAfter = MiscLocationHelper.CheckGeoObjectAfter(li[li.Count - 1].EndToken, true);
            if ((li.Count == 2 && li[0].Typ == ItemType.Noun && li[1].Typ == ItemType.Noun) && ((li[0].GeoObjectBefore || li[1].GeoObjectAfter))) 
            {
                if (li[0].Chars.IsCapitalUpper && li[1].Chars.IsAllLower) 
                    li[0].Typ = ItemType.ProperName;
                else if (li[1].Chars.IsCapitalUpper && li[0].Chars.IsAllLower) 
                    li[1].Typ = ItemType.ProperName;
            }
            return li;
        }
        static bool CheckDoubtful(Pullenti.Ner.TextToken tt)
        {
            if (tt == null) 
                return true;
            if (tt.Chars.IsAllLower) 
                return true;
            if (tt.LengthChar < 3) 
                return true;
            if (((tt.Term == "СОЧИ" || tt.IsValue("КИЕВ", null) || tt.IsValue("ПСКОВ", null)) || tt.IsValue("БОСТОН", null) || tt.IsValue("РИГА", null)) || tt.IsValue("АСТАНА", null) || tt.IsValue("АЛМАТЫ", null)) 
                return false;
            if (tt.Term.EndsWith("ВО")) 
                return false;
            if ((tt.Next is Pullenti.Ner.TextToken) && (tt.WhitespacesAfterCount < 2) && !tt.Next.Chars.IsAllLower) 
            {
                if (tt.Chars == tt.Next.Chars && !tt.Chars.IsLatinLetter && ((!tt.Morph.Case.IsGenitive && !tt.Morph.Case.IsAccusative))) 
                {
                    Pullenti.Morph.MorphClass mc = tt.Next.GetMorphClassInDictionary();
                    if (mc.IsProperSurname || mc.IsProperSecname) 
                        return true;
                }
            }
            if ((tt.Previous is Pullenti.Ner.TextToken) && (tt.WhitespacesBeforeCount < 2) && !tt.Previous.Chars.IsAllLower) 
            {
                Pullenti.Morph.MorphClass mc = tt.Previous.GetMorphClassInDictionary();
                if (mc.IsProperSurname) 
                    return true;
            }
            bool ok = false;
            foreach (Pullenti.Morph.MorphBaseInfo wff in tt.Morph.Items) 
            {
                Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                if (wf.IsInDictionary) 
                {
                    if (!wf.Class.IsProper) 
                        ok = true;
                    if (wf.Class.IsProperSurname || wf.Class.IsProperName || wf.Class.IsProperSecname) 
                    {
                        if (wf.NormalCase != "ЛОНДОН" && wf.NormalCase != "ЛОНДОНЕ") 
                            ok = true;
                    }
                }
                else if (wf.Class.IsProperSurname) 
                {
                    string val = wf.NormalFull ?? wf.NormalCase ?? "";
                    if (Pullenti.Morph.LanguageHelper.EndsWithEx(val, "ОВ", "ЕВ", "ИН", null)) 
                    {
                        if (val != "БЕРЛИН") 
                        {
                            if (tt.Previous != null && tt.Previous.IsValue("В", null)) 
                            {
                            }
                            else 
                                return true;
                        }
                    }
                }
            }
            if (!ok) 
                return false;
            Pullenti.Ner.Token t0 = tt.Previous;
            if (t0 != null && ((t0.IsChar(',') || t0.Morph.Class.IsConjunction))) 
                t0 = t0.Previous;
            if (t0 != null && (t0.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                return false;
            Pullenti.Ner.Token t1 = tt.Next;
            if (t1 != null && ((t1.IsChar(',') || t1.Morph.Class.IsConjunction))) 
                t1 = t1.Next;
            if (m_Recursive == 0) 
            {
                m_Recursive++;
                CityItemToken cit = _TryParse(t1, null, false, null);
                m_Recursive--;
                if (cit == null) 
                    return true;
                if (cit.Typ == ItemType.Noun || cit.Typ == ItemType.City) 
                    return false;
            }
            return true;
        }
        static int m_Recursive = 0;
        public static CityItemToken TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection loc, bool canBeLowChar = false, CityItemToken prev = null)
        {
            if (t == null) 
                return null;
            if (t.Kit.IsRecurceOverflow) 
                return null;
            t.Kit.RecurseLevel++;
            CityItemToken res = _tryParseInt(t, loc, canBeLowChar, prev);
            t.Kit.RecurseLevel--;
            if (res != null && res.Typ == ItemType.Noun && (res.WhitespacesAfterCount < 2)) 
            {
                Pullenti.Ner.Core.NounPhraseToken nn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (nn != null && ((nn.EndToken.IsValue("ЗНАЧЕНИЕ", "ЗНАЧЕННЯ") || nn.EndToken.IsValue("ТИП", null) || nn.EndToken.IsValue("ХОЗЯЙСТВО", "ХАЗЯЙСТВО")))) 
                    res.EndToken = nn.EndToken;
            }
            if ((res != null && res.Typ == ItemType.ProperName && res.Value != null) && res.BeginToken == res.EndToken && res.Value.Length > 4) 
            {
                if (res.Value.EndsWith("ГРАД") || res.Value.EndsWith("ГОРОД")) 
                {
                    res.AltValue = null;
                    res.Typ = ItemType.City;
                }
                else if (((res.Value.EndsWith("СК") || res.Value.EndsWith("ИНО") || res.Value.EndsWith("ПОЛЬ")) || res.Value.EndsWith("ВЛЬ") || res.Value.EndsWith("АС")) || res.Value.EndsWith("ЕС")) 
                {
                    List<Pullenti.Ner.Address.Internal.StreetItemToken> sits = Pullenti.Ner.Address.Internal.StreetItemToken.TryParseList(res.EndToken.Next, null, 3);
                    if (sits != null) 
                    {
                        if (sits.Count == 1 && sits[0].Typ == Pullenti.Ner.Address.Internal.StreetItemType.Noun) 
                            return res;
                        if (sits.Count == 2 && sits[0].Typ == Pullenti.Ner.Address.Internal.StreetItemType.Number && sits[1].Typ == Pullenti.Ner.Address.Internal.StreetItemType.Noun) 
                            return res;
                    }
                    Pullenti.Morph.MorphClass mc = res.EndToken.GetMorphClassInDictionary();
                    if (mc.IsProperGeo || mc.IsUndefined) 
                    {
                        res.AltValue = null;
                        res.Typ = ItemType.City;
                    }
                }
                else if (res.Value.EndsWith("АНЬ") || res.Value.EndsWith("TOWN") || res.Value.StartsWith("SAN")) 
                    res.Typ = ItemType.City;
                else if (res.EndToken is Pullenti.Ner.TextToken) 
                {
                    string lem = (res.EndToken as Pullenti.Ner.TextToken).Lemma;
                    if ((lem.EndsWith("ГРАД") || lem.EndsWith("ГОРОД") || lem.EndsWith("СК")) || lem.EndsWith("АНЬ") || lem.EndsWith("ПОЛЬ")) 
                    {
                        res.AltValue = res.Value;
                        res.Value = lem;
                        int ii = res.AltValue.IndexOf('-');
                        if (ii >= 0) 
                            res.Value = res.AltValue.Substring(0, ii + 1) + lem;
                        if (!lem.EndsWith("АНЬ")) 
                            res.AltValue = null;
                    }
                }
            }
            return res;
        }
        static CityItemToken _tryParseInt(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection loc, bool canBeLowChar, CityItemToken prev)
        {
            if (t == null) 
                return null;
            CityItemToken res = _TryParse(t, loc, canBeLowChar, prev);
            if ((prev == null && t.Chars.IsCyrillicLetter && t.Chars.IsAllUpper) && t.LengthChar == 2) 
            {
                if (t.IsValue("ТА", null)) 
                {
                    res = _TryParse(t.Next, loc, canBeLowChar, prev);
                    if (res != null) 
                    {
                        if (res.Typ == ItemType.Noun) 
                        {
                            res.BeginToken = t;
                            res.Doubtful = false;
                        }
                        else 
                            res = null;
                    }
                }
            }
            if ((prev != null && prev.Typ == ItemType.Noun && m_Recursive == 0) && ((prev.Value != "ГОРОД" && prev.Value != "МІСТО"))) 
            {
                if (res == null || ((res.Typ != ItemType.Noun && res.Typ != ItemType.Misc && res.Typ != ItemType.City))) 
                {
                    m_Recursive++;
                    Pullenti.Ner.Address.Internal.AddressItemToken det = Pullenti.Ner.Address.Internal.AddressItemToken.TryAttachOrg(t);
                    m_Recursive--;
                    if (det != null) 
                    {
                        int cou = 0;
                        for (Pullenti.Ner.Token ttt = det.BeginToken; ttt != null && ttt.EndChar <= det.EndChar; ttt = ttt.Next) 
                        {
                            if (ttt.Chars.IsLetter) 
                                cou++;
                        }
                        if (cou < 6) 
                        {
                            CityItemToken re = new CityItemToken(det.BeginToken, det.EndToken) { Typ = ItemType.ProperName };
                            if (det.Referent.TypeName == "ORGANIZATION") 
                                re.OrgRef = det.RefToken;
                            else 
                            {
                                re.Value = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(det, Pullenti.Ner.Core.GetTextAttr.No);
                                re.AltValue = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(det, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                            }
                            return re;
                        }
                    }
                }
            }
            if (res != null && res.Typ == ItemType.Noun && (res.WhitespacesAfterCount < 3)) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.EndToken.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    if (npt.EndToken.IsValue("ПОДЧИНЕНИЕ", "ПІДПОРЯДКУВАННЯ")) 
                        res.EndToken = npt.EndToken;
                }
            }
            if ((res != null && t.Chars.IsAllUpper && res.Typ == ItemType.ProperName) && m_Recursive == 0) 
            {
                Pullenti.Ner.Token tt = t.Previous;
                if (tt != null && tt.IsComma) 
                    tt = tt.Previous;
                Pullenti.Ner.Geo.GeoReferent geoPrev = null;
                if (tt != null && (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    geoPrev = tt.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                if (geoPrev != null && ((geoPrev.IsRegion || geoPrev.IsCity))) 
                {
                    m_Recursive++;
                    Pullenti.Ner.Address.Internal.AddressItemToken det = Pullenti.Ner.Address.Internal.AddressItemToken.TryAttachOrg(t);
                    m_Recursive--;
                    if (det != null) 
                        res = null;
                }
            }
            if (res != null && res.Typ == ItemType.ProperName) 
            {
                if ((t.IsValue("ДУМА", "РАДА") || t.IsValue("ГЛАВА", "ГОЛОВА") || t.IsValue("АДМИНИСТРАЦИЯ", "АДМІНІСТРАЦІЯ")) || t.IsValue("МЭР", "МЕР") || t.IsValue("ПРЕДСЕДАТЕЛЬ", "ГОЛОВА")) 
                    return null;
            }
            Pullenti.Ner.Geo.GeoReferent geoAfter = null;
            if (res == null) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        res = _TryParse(t.Next, loc, false, null);
                        if (res != null && ((res.Typ == ItemType.ProperName || res.Typ == ItemType.City))) 
                        {
                            res.BeginToken = t;
                            res.Typ = ItemType.ProperName;
                            res.EndToken = br.EndToken;
                            if (res.EndToken.Next != br.EndToken) 
                            {
                                res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, br.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                                res.AltValue = null;
                            }
                            return res;
                        }
                    }
                }
                if (t is Pullenti.Ner.TextToken) 
                {
                    string txt = (t as Pullenti.Ner.TextToken).Term;
                    if (txt == "ИМ" || txt == "ИМЕНИ") 
                    {
                        Pullenti.Ner.Token t1 = t.Next;
                        if (t1 != null && t1.IsChar('.')) 
                            t1 = t1.Next;
                        res = _TryParse(t1, loc, canBeLowChar, null);
                        if (res != null && ((((res.Typ == ItemType.City && res.Doubtful)) || res.Typ == ItemType.ProperName))) 
                        {
                            res.BeginToken = t;
                            res.Morph = new Pullenti.Ner.MorphCollection();
                            return res;
                        }
                    }
                    if (prev != null && prev.Typ == ItemType.Noun && ((!prev.Doubtful || MiscLocationHelper.CheckGeoObjectBefore(prev.BeginToken)))) 
                    {
                        if (t.Chars.IsCyrillicLetter && t.LengthChar == 1 && t.Chars.IsAllUpper) 
                        {
                            if ((t.Next != null && !t.IsWhitespaceAfter && ((t.Next.IsHiphen || t.Next.IsChar('.')))) && (t.Next.WhitespacesAfterCount < 2)) 
                            {
                                CityItemToken res1 = _TryParse(t.Next.Next, loc, false, null);
                                if (res1 != null && ((res1.Typ == ItemType.ProperName || res1.Typ == ItemType.City))) 
                                {
                                    List<string> adjs = MiscLocationHelper.GetStdAdjFullStr(txt, res1.Morph.Gender, res1.Morph.Number, true);
                                    if (adjs == null && prev != null && prev.Typ == ItemType.Noun) 
                                        adjs = MiscLocationHelper.GetStdAdjFullStr(txt, prev.Morph.Gender, Pullenti.Morph.MorphNumber.Undefined, true);
                                    if (adjs == null) 
                                        adjs = MiscLocationHelper.GetStdAdjFullStr(txt, res1.Morph.Gender, res1.Morph.Number, false);
                                    if (adjs != null) 
                                    {
                                        if (res1.Value == null) 
                                            res1.Value = res1.GetSourceText().ToUpper();
                                        if (res1.AltValue != null) 
                                            res1.AltValue = string.Format("{0} {1}", adjs[0], res1.AltValue);
                                        else if (adjs.Count > 1) 
                                            res1.AltValue = string.Format("{0} {1}", adjs[1], res1.Value);
                                        res1.Value = string.Format("{0} {1}", adjs[0], res1.Value);
                                        res1.BeginToken = t;
                                        res1.Typ = ItemType.ProperName;
                                        return res1;
                                    }
                                }
                            }
                        }
                    }
                }
                Pullenti.Ner.Token tt = (prev == null ? t.Previous : prev.BeginToken.Previous);
                while (tt != null && tt.IsCharOf(",.")) 
                {
                    tt = tt.Previous;
                }
                Pullenti.Ner.Geo.GeoReferent geoPrev = null;
                if (tt != null && (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                    geoPrev = tt.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                Pullenti.Ner.Token tt0 = t;
                bool ooo = false;
                bool hasGeoAfter = false;
                if (geoPrev != null || MiscLocationHelper.CheckNearBefore(t.Previous) != null) 
                    ooo = true;
                else if (MiscLocationHelper.CheckGeoObjectBefore(t)) 
                    ooo = true;
                else if (t.Chars.IsLetter) 
                {
                    tt = t.Next;
                    if (tt != null && tt.IsChar('.')) 
                        tt = tt.Next;
                    if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsAllLower) 
                    {
                        if (MiscLocationHelper.CheckGeoObjectAfter(tt, true)) 
                            ooo = (hasGeoAfter = true);
                        else if (Pullenti.Ner.Address.Internal.AddressItemToken.CheckStreetAfter(tt.Next)) 
                            ooo = true;
                        else 
                        {
                            if (loc != null) 
                            {
                            }
                            CityItemToken cit2 = _TryParse(tt, loc, false, null);
                            if (cit2 != null && cit2.BeginToken != cit2.EndToken && ((cit2.Typ == ItemType.ProperName || cit2.Typ == ItemType.City))) 
                            {
                                if (Pullenti.Ner.Address.Internal.AddressItemToken.CheckStreetAfter(cit2.EndToken.Next)) 
                                    ooo = true;
                            }
                            if (cit2 != null && cit2.Typ == ItemType.City && tt.Previous.IsChar('.')) 
                            {
                                if (cit2.IsWhitespaceAfter || ((cit2.EndToken.Next != null && cit2.EndToken.Next.LengthChar == 1))) 
                                {
                                    ooo = true;
                                    if (cit2.OntoItem != null) 
                                        geoAfter = cit2.OntoItem.Referent as Pullenti.Ner.Geo.GeoReferent;
                                }
                            }
                        }
                    }
                }
                if (ooo) 
                {
                    tt = t;
                    for (Pullenti.Ner.Token ttt = tt; ttt != null; ttt = ttt.Next) 
                    {
                        if (ttt.IsCharOf(",.")) 
                        {
                            tt = ttt.Next;
                            continue;
                        }
                        if (ttt.IsNewlineBefore) 
                            break;
                        Pullenti.Ner.Address.Internal.AddressItemToken det = Pullenti.Ner.Address.Internal.AddressItemToken.TryAttachDetail(ttt);
                        if (det != null) 
                        {
                            ttt = det.EndToken;
                            tt = det.EndToken.Next;
                            continue;
                        }
                        det = Pullenti.Ner.Address.Internal.AddressItemToken.TryAttachOrg(ttt);
                        if (det != null) 
                        {
                            ttt = det.EndToken;
                            tt0 = (tt = det.EndToken.Next);
                            continue;
                        }
                        Pullenti.Ner.Address.Internal.AddressItemToken ait = Pullenti.Ner.Address.Internal.AddressItemToken.TryParse(ttt, null, false, true, null);
                        if (ait != null && ait.Typ == Pullenti.Ner.Address.Internal.AddressItemToken.ItemType.Plot) 
                        {
                            ttt = ait.EndToken;
                            tt0 = (tt = ait.EndToken.Next);
                            continue;
                        }
                        break;
                    }
                    if (tt is Pullenti.Ner.TextToken) 
                    {
                        if (tt0.IsComma && tt0.Next != null) 
                            tt0 = tt0.Next;
                        string txt = (tt as Pullenti.Ner.TextToken).Term;
                        if ((((txt == "Д" || txt == "С" || txt == "C") || txt == "П" || txt == "Х")) && ((tt.Chars.IsAllLower || ((tt.Next != null && tt.Next.IsChar('.')))))) 
                        {
                            Pullenti.Ner.Token tt1 = tt;
                            if (tt1.Next != null && tt1.Next.IsChar('.')) 
                                tt1 = tt1.Next;
                            Pullenti.Ner.Token tt2 = tt1.Next;
                            if ((tt2 != null && tt2.LengthChar == 1 && tt2.Chars.IsCyrillicLetter) && tt2.Chars.IsAllUpper) 
                            {
                                if (tt2.Next != null && ((tt2.Next.IsChar('.') || tt2.Next.IsHiphen)) && !tt2.IsWhitespaceAfter) 
                                    tt2 = tt2.Next.Next;
                            }
                            bool ok = false;
                            if (txt == "Д" && (tt2 is Pullenti.Ner.NumberToken) && !tt2.IsNewlineBefore) 
                                ok = false;
                            else if (((txt == "С" || txt == "C")) && (tt2 is Pullenti.Ner.TextToken) && ((tt2.IsValue("О", null) || tt2.IsValue("O", null)))) 
                                ok = false;
                            else if (tt2 != null && tt2.Chars.IsCapitalUpper && (tt2.WhitespacesBeforeCount < 2)) 
                                ok = tt.Chars.IsAllLower;
                            else if (tt2 != null && tt2.Chars.IsAllUpper && (tt2.WhitespacesBeforeCount < 2)) 
                            {
                                ok = true;
                                if (tt.Chars.IsAllUpper) 
                                {
                                    Pullenti.Ner.ReferentToken rtt = tt.Kit.ProcessReferent("PERSON", tt);
                                    if (rtt != null) 
                                    {
                                        ok = false;
                                        Pullenti.Ner.Token ttt2 = rtt.EndToken.Next;
                                        if (ttt2 != null && ttt2.IsComma) 
                                            ttt2 = ttt2.Next;
                                        if (Pullenti.Ner.Address.Internal.AddressItemToken.CheckHouseAfter(ttt2, false, false) || Pullenti.Ner.Address.Internal.AddressItemToken.CheckStreetAfter(ttt2)) 
                                            ok = true;
                                    }
                                    else if (tt.Previous != null && tt.Previous.IsChar('.')) 
                                        ok = false;
                                }
                                else if (tt1 == tt) 
                                    ok = false;
                                if (!ok && tt1.Next != null) 
                                {
                                    Pullenti.Ner.Token ttt2 = tt1.Next.Next;
                                    if (ttt2 != null && ttt2.IsComma) 
                                        ttt2 = ttt2.Next;
                                    if (Pullenti.Ner.Address.Internal.AddressItemToken.CheckHouseAfter(ttt2, false, false) || Pullenti.Ner.Address.Internal.AddressItemToken.CheckStreetAfter(ttt2)) 
                                        ok = true;
                                }
                            }
                            else if (prev != null && prev.Typ == ItemType.ProperName && (tt.WhitespacesBeforeCount < 2)) 
                            {
                                if (MiscLocationHelper.CheckGeoObjectBefore(prev.BeginToken.Previous)) 
                                    ok = true;
                                if (txt == "П" && tt.Next != null && ((tt.Next.IsHiphen || tt.Next.IsCharOf("\\/")))) 
                                {
                                    Pullenti.Ner.Address.Internal.StreetItemToken sit = Pullenti.Ner.Address.Internal.StreetItemToken.TryParse(tt, null, false, null, false);
                                    if (sit != null && sit.Typ == Pullenti.Ner.Address.Internal.StreetItemType.Noun) 
                                        ok = false;
                                }
                            }
                            else if (prev == null) 
                            {
                                if (MiscLocationHelper.CheckGeoObjectBefore(tt.Previous)) 
                                    ok = true;
                                else if (geoAfter != null || hasGeoAfter) 
                                    ok = true;
                            }
                            if (tt.Previous != null && tt.Previous.IsHiphen && !tt.IsWhitespaceBefore) 
                            {
                                if (tt.Next != null && tt.Next.IsChar('.')) 
                                {
                                }
                                else 
                                    ok = false;
                            }
                            if (ok) 
                            {
                                res = new CityItemToken(tt0, tt1) { Typ = ItemType.Noun, GeoObjectBefore = true };
                                res.Value = (txt == "Д" ? "ДЕРЕВНЯ" : (txt == "П" ? "ПОСЕЛОК" : (txt == "Х" ? "ХУТОР" : "СЕЛО")));
                                if (txt == "П") 
                                    res.AltValue = "ПОСЕЛЕНИЕ";
                                else if (txt == "С" || txt == "C") 
                                {
                                    res.AltValue = "СЕЛЕНИЕ";
                                    if (tt0 == tt1) 
                                    {
                                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1.Next, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns, 0, null);
                                        if (npt != null && npt.Morph.Case.IsInstrumental) 
                                            return null;
                                    }
                                }
                                res.Doubtful = true;
                                return res;
                            }
                        }
                        if ((txt == "СП" || txt == "РП" || txt == "ГП") || txt == "ДП") 
                        {
                            if (tt.Next != null && tt.Next.IsChar('.')) 
                                tt = tt.Next;
                            if (tt.Next != null && tt.Next.Chars.IsCapitalUpper) 
                                return new CityItemToken(tt0, tt) { Typ = ItemType.Noun, GeoObjectBefore = true, Value = (txt == "РП" ? "РАБОЧИЙ ПОСЕЛОК" : (txt == "ГП" ? "ГОРОДСКОЕ ПОСЕЛЕНИЕ" : (txt == "ДП" ? "ДАЧНЫЙ ПОСЕЛОК" : "СЕЛЬСКОЕ ПОСЕЛЕНИЕ"))) };
                        }
                        res = _TryParse(tt, loc, canBeLowChar, null);
                        if (res != null && res.Typ == ItemType.Noun) 
                        {
                            res.GeoObjectBefore = true;
                            res.BeginToken = tt0;
                            return res;
                        }
                        if (tt.Chars.IsAllUpper && tt.LengthChar > 2 && tt.Chars.IsCyrillicLetter) 
                            return new CityItemToken(tt, tt) { Typ = ItemType.ProperName, Value = (tt as Pullenti.Ner.TextToken).Term };
                    }
                }
                if ((t is Pullenti.Ner.NumberToken) && t.Next != null) 
                {
                    Pullenti.Ner.Core.NumberExToken net = Pullenti.Ner.Core.NumberHelper.TryParseNumberWithPostfix(t);
                    if (net != null && net.ExTyp == Pullenti.Ner.Core.NumberExType.Kilometer) 
                        return new CityItemToken(t, net.EndToken) { Typ = ItemType.ProperName, Value = string.Format("{0}КМ", (int)net.RealValue) };
                }
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                if ((rt != null && (rt.Referent is Pullenti.Ner.Geo.GeoReferent) && rt.BeginToken == rt.EndToken) && (rt.Referent as Pullenti.Ner.Geo.GeoReferent).IsState) 
                {
                    if (t.Previous == null) 
                        return null;
                    if (t.Previous.Morph.Number == Pullenti.Morph.MorphNumber.Singular && t.Morph.Case.IsNominative && !t.Morph.Case.IsGenitive) 
                        return new CityItemToken(t, t) { Typ = ItemType.ProperName, Value = rt.GetSourceText().ToUpper() };
                }
                return null;
            }
            if (res.Typ == ItemType.Noun) 
            {
                if (res.Value == "СЕЛО" && (t is Pullenti.Ner.TextToken)) 
                {
                    if (t.Previous == null) 
                    {
                    }
                    else if (t.Previous.Morph.Class.IsPreposition) 
                    {
                    }
                    else 
                        res.Doubtful = true;
                    res.Morph.Gender = Pullenti.Morph.MorphGender.Neuter;
                }
                if (res.AltValue == null && res.BeginToken.IsValue("ПОСЕЛЕНИЕ", null)) 
                {
                    res.Value = "ПОСЕЛЕНИЕ";
                    res.AltValue = "ПОСЕЛОК";
                }
                if (Pullenti.Morph.LanguageHelper.EndsWith(res.Value, "УСАДЬБА") && res.AltValue == null) 
                    res.AltValue = "НАСЕЛЕННЫЙ ПУНКТ";
                if (res.Value == "СТАНЦИЯ" || res.Value == "СТАНЦІЯ") 
                    res.Doubtful = true;
                if (res.EndToken.IsValue("СТОЛИЦА", null) || res.EndToken.IsValue("СТОЛИЦЯ", null)) 
                {
                    res.Doubtful = true;
                    if (res.EndToken.Next != null) 
                    {
                        Pullenti.Ner.Geo.GeoReferent geo = res.EndToken.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                        if (geo != null && ((geo.IsRegion || geo.IsState))) 
                        {
                            res.HigherGeo = geo;
                            res.EndToken = res.EndToken.Next;
                            res.Doubtful = false;
                            res.Value = "ГОРОД";
                            foreach (Pullenti.Ner.Core.Termin it in TerrItemToken.m_CapitalsByState.Termins) 
                            {
                                Pullenti.Ner.Geo.GeoReferent ge = it.Tag as Pullenti.Ner.Geo.GeoReferent;
                                if (ge == null || !ge.CanBeEquals(geo, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                    continue;
                                Pullenti.Ner.Core.TerminToken tok = TerrItemToken.m_CapitalsByState.TryParse(res.EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                                if (tok != null && tok.Termin == it) 
                                    break;
                                res.Typ = ItemType.City;
                                res.Value = it.CanonicText;
                                return res;
                            }
                        }
                    }
                }
                if ((res.BeginToken.LengthChar == 1 && res.BeginToken.Chars.IsAllUpper && res.BeginToken.Next != null) && res.BeginToken.Next.IsChar('.')) 
                {
                    CityItemToken ne = _tryParseInt(res.BeginToken.Next.Next, loc, false, null);
                    if (ne != null && ne.Typ == ItemType.City) 
                    {
                    }
                    else if (ne != null && ne.Typ == ItemType.ProperName && ((ne.Value.EndsWith("К") || ne.Value.EndsWith("О")))) 
                    {
                    }
                    else 
                        return null;
                }
            }
            if (res.Typ == ItemType.ProperName || res.Typ == ItemType.City) 
            {
                string val = res.Value ?? ((res.OntoItem == null ? null : res.OntoItem.CanonicText));
                Pullenti.Ner.Token t1 = res.EndToken;
                if (((!t1.IsWhitespaceAfter && t1.Next != null && t1.Next.IsHiphen) && !t1.Next.IsWhitespaceAfter && (t1.Next.Next is Pullenti.Ner.NumberToken)) && (t1.Next.Next as Pullenti.Ner.NumberToken).IntValue != null && ((t1.Next.Next as Pullenti.Ner.NumberToken).IntValue.Value < 30)) 
                {
                    res.EndToken = t1.Next.Next;
                    res.Value = string.Format("{0}-{1}", val, (t1.Next.Next as Pullenti.Ner.NumberToken).Value);
                    if (res.AltValue != null) 
                        res.AltValue = string.Format("{0}-{1}", res.AltValue, (t1.Next.Next as Pullenti.Ner.NumberToken).Value);
                    res.Typ = ItemType.ProperName;
                }
                else if (t1.WhitespacesAfterCount == 1 && (t1.Next is Pullenti.Ner.NumberToken) && t1.Next.Morph.Class.IsAdjective) 
                {
                    bool ok = false;
                    if (t1.Next.Next == null || t1.Next.IsNewlineAfter) 
                        ok = true;
                    else if (!t1.Next.IsWhitespaceAfter && t1.Next.Next != null && t1.Next.Next.IsCharOf(",")) 
                        ok = true;
                    if (ok) 
                    {
                        res.EndToken = t1.Next;
                        res.Value = string.Format("{0}-{1}", val, (t1.Next as Pullenti.Ner.NumberToken).Value);
                        res.Typ = ItemType.ProperName;
                    }
                }
            }
            if (res.Typ == ItemType.City && res.BeginToken == res.EndToken) 
            {
                if (res.BeginToken.GetMorphClassInDictionary().IsAdjective && (res.EndToken.Next is Pullenti.Ner.TextToken)) 
                {
                    bool ok = false;
                    Pullenti.Ner.Token t1 = null;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.EndToken == res.EndToken.Next) 
                    {
                        t1 = npt.EndToken;
                        if (res.EndToken.Next.Chars == res.BeginToken.Chars) 
                        {
                            ok = true;
                            if (res.BeginToken.Chars.IsAllUpper) 
                            {
                                CityItemToken cii = _tryParseInt(res.EndToken.Next, loc, false, null);
                                if (cii != null && cii.Typ == ItemType.Noun) 
                                    ok = false;
                            }
                        }
                        else if (res.EndToken.Next.Chars.IsAllLower) 
                        {
                            Pullenti.Ner.Token ttt = res.EndToken.Next.Next;
                            if (ttt == null || ttt.IsCharOf(",.")) 
                                ok = true;
                        }
                    }
                    else if (res.EndToken.Next.Chars == res.BeginToken.Chars && res.BeginToken.Chars.IsCapitalUpper) 
                    {
                        Pullenti.Ner.Token ttt = res.EndToken.Next.Next;
                        if (ttt == null || ttt.IsCharOf(",.")) 
                            ok = true;
                        t1 = res.EndToken.Next;
                        npt = null;
                    }
                    if (ok && t1 != null) 
                    {
                        res.Typ = ItemType.ProperName;
                        res.OntoItem = null;
                        res.EndToken = t1;
                        if (npt != null) 
                        {
                            res.Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                            res.Morph = npt.Morph;
                        }
                        else 
                            res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(res.BeginToken, res.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    }
                }
                if ((res.EndToken.Next != null && res.EndToken.Next.IsHiphen && !res.EndToken.Next.IsWhitespaceAfter) && !res.EndToken.Next.IsWhitespaceBefore) 
                {
                    CityItemToken res1 = _TryParse(res.EndToken.Next.Next, loc, false, null);
                    if ((res1 != null && res1.Typ == ItemType.ProperName && res1.BeginToken == res1.EndToken) && res1.BeginToken.Chars == res.BeginToken.Chars) 
                    {
                        if (res1.OntoItem == null && res.OntoItem == null) 
                        {
                            res.Typ = ItemType.ProperName;
                            res.Value = string.Format("{0}-{1}", (res.OntoItem == null ? res.Value : res.OntoItem.CanonicText), res1.Value);
                            if (res.AltValue != null) 
                                res.AltValue = string.Format("{0}-{1}", res.AltValue, res1.Value);
                            res.OntoItem = null;
                            res.EndToken = res1.EndToken;
                            res.Doubtful = false;
                        }
                    }
                    else if ((res.EndToken.Next.Next is Pullenti.Ner.NumberToken) && (res.EndToken.Next.Next as Pullenti.Ner.NumberToken).IntValue != null && ((res.EndToken.Next.Next as Pullenti.Ner.NumberToken).IntValue.Value < 30)) 
                    {
                        res.Typ = ItemType.ProperName;
                        res.Value = string.Format("{0}-{1}", (res.OntoItem == null ? res.Value : res.OntoItem.CanonicText), (res.EndToken.Next.Next as Pullenti.Ner.NumberToken).Value);
                        if (res.AltValue != null) 
                            res.AltValue = string.Format("{0}-{1}", res.AltValue, (res.EndToken.Next.Next as Pullenti.Ner.NumberToken).Value);
                        res.OntoItem = null;
                        res.EndToken = res.EndToken.Next.Next;
                    }
                }
                else if (res.BeginToken.GetMorphClassInDictionary().IsProperName) 
                {
                    if (res.BeginToken.IsValue("КИЇВ", null) || res.BeginToken.IsValue("АСТАНА", null) || res.BeginToken.IsValue("АЛМАТЫ", null)) 
                    {
                    }
                    else if ((res.EndToken is Pullenti.Ner.TextToken) && (res.EndToken as Pullenti.Ner.TextToken).Term.EndsWith("ВО")) 
                    {
                    }
                    else 
                    {
                        res.Doubtful = true;
                        Pullenti.Ner.Token tt = res.BeginToken.Previous;
                        if (tt != null && tt.Previous != null) 
                        {
                            if (tt.IsChar(',') || tt.Morph.Class.IsConjunction) 
                            {
                                Pullenti.Ner.Geo.GeoReferent geo = tt.Previous.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                                if (geo != null && geo.IsCity) 
                                    res.Doubtful = false;
                            }
                        }
                        if (tt != null && tt.IsValue("В", null) && tt.Chars.IsAllLower) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(res.BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt1 == null || npt1.EndChar <= res.EndChar) 
                                res.Doubtful = false;
                        }
                    }
                }
                if ((res.BeginToken == res.EndToken && res.Typ == ItemType.City && res.OntoItem != null) && res.OntoItem.CanonicText == "САНКТ - ПЕТЕРБУРГ") 
                {
                    for (Pullenti.Ner.Token tt = res.BeginToken.Previous; tt != null; tt = tt.Previous) 
                    {
                        if (tt.IsHiphen || tt.IsChar('.')) 
                            continue;
                        if (tt.IsValue("С", null) || tt.IsValue("C", null) || tt.IsValue("САНКТ", null)) 
                            res.BeginToken = tt;
                        break;
                    }
                }
            }
            if ((res.BeginToken == res.EndToken && res.Typ == ItemType.ProperName && res.WhitespacesAfterCount == 1) && (res.EndToken.Next is Pullenti.Ner.TextToken) && res.EndToken.Chars == res.EndToken.Next.Chars) 
            {
                bool ok = false;
                Pullenti.Ner.Token t1 = res.EndToken;
                if (t1.Next.Next == null || t1.Next.IsNewlineAfter) 
                    ok = true;
                else if (!t1.Next.IsWhitespaceAfter && t1.Next.Next != null && t1.Next.Next.IsCharOf(",.")) 
                    ok = true;
                if (ok) 
                {
                    CityItemToken pp = _TryParse(t1.Next, loc, false, null);
                    if (pp != null && pp.Typ == ItemType.Noun) 
                        ok = false;
                    if (ok) 
                    {
                        TerrItemToken te = TerrItemToken.TryParse(t1.Next, null, false, false, null);
                        if (te != null && te.TerminItem != null) 
                            ok = false;
                    }
                }
                if (ok) 
                {
                    res.EndToken = t1.Next;
                    res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(res.BeginToken, res.EndToken, Pullenti.Ner.Core.GetTextAttr.No);
                    res.AltValue = null;
                    res.Typ = ItemType.ProperName;
                }
            }
            return res;
        }
        static CityItemToken _TryParse(Pullenti.Ner.Token t, Pullenti.Ner.Core.IntOntologyCollection loc, bool canBeLowChar = false, CityItemToken prev = null)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
            {
                if ((t is Pullenti.Ner.ReferentToken) && (t.GetReferent() is Pullenti.Ner.Date.DateReferent)) 
                {
                    List<Pullenti.Ner.Address.Internal.StreetItemToken> aii = Pullenti.Ner.Address.Internal.StreetItemToken.TryParseSpec(t, null);
                    if (aii != null) 
                    {
                        if (aii.Count > 1 && aii[0].Typ == Pullenti.Ner.Address.Internal.StreetItemType.Number && aii[1].Typ == Pullenti.Ner.Address.Internal.StreetItemType.StdName) 
                        {
                            CityItemToken res2 = new CityItemToken(t, aii[1].EndToken) { Typ = ItemType.ProperName };
                            res2.Value = string.Format("{0} {1}", (aii[0].Number == null ? aii[0].Value : aii[0].Number.IntValue.Value.ToString()), aii[1].Value);
                            return res2;
                        }
                    }
                }
                return null;
            }
            List<Pullenti.Ner.Core.IntOntologyToken> li = new List<Pullenti.Ner.Core.IntOntologyToken>();
            List<Pullenti.Ner.Core.IntOntologyToken> li0 = null;
            bool isInLocOnto = false;
            if (loc != null) 
            {
                if ((((li0 = loc.TryAttach(t, null, false)))) != null) 
                {
                    li.AddRange(li0);
                    isInLocOnto = true;
                }
            }
            if (t.Kit.Ontology != null && li.Count == 0) 
            {
                if ((((li0 = t.Kit.Ontology.AttachToken(Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME, t)))) != null) 
                {
                    li.AddRange(li0);
                    isInLocOnto = true;
                }
            }
            if (li.Count == 0) 
            {
                li0 = m_Ontology.TryAttach(t, null, false);
                if (li0 != null) 
                    li.AddRange(li0);
            }
            if (li.Count > 0) 
            {
                if (t is Pullenti.Ner.TextToken) 
                {
                    for (int i = li.Count - 1; i >= 0; i--) 
                    {
                        if (li[i].Item != null) 
                        {
                            Pullenti.Ner.Geo.GeoReferent g = li[i].Item.Referent as Pullenti.Ner.Geo.GeoReferent;
                            if (g != null) 
                            {
                                if (!g.IsCity) 
                                {
                                    li.RemoveAt(i);
                                    continue;
                                }
                            }
                        }
                    }
                    Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                    foreach (Pullenti.Ner.Core.IntOntologyToken nt in li) 
                    {
                        if (nt.Item != null && nt.Item.CanonicText == tt.Term) 
                        {
                            if (canBeLowChar || !Pullenti.Ner.Core.MiscHelper.IsAllCharactersLower(nt.BeginToken, nt.EndToken, false)) 
                            {
                                CityItemToken ci = new CityItemToken(nt.BeginToken, nt.EndToken) { Typ = ItemType.City, OntoItem = nt.Item, Morph = nt.Morph };
                                if (nt.BeginToken == nt.EndToken && !isInLocOnto) 
                                    ci.Doubtful = CheckDoubtful(nt.BeginToken as Pullenti.Ner.TextToken);
                                Pullenti.Ner.Token tt1 = nt.EndToken.Next;
                                if ((((tt1 != null && tt1.IsHiphen && !tt1.IsWhitespaceBefore) && !tt1.IsWhitespaceAfter && prev != null) && prev.Typ == ItemType.Noun && (tt1.Next is Pullenti.Ner.TextToken)) && tt1.Previous.Chars == tt1.Next.Chars) 
                                {
                                    li = null;
                                    break;
                                }
                                return ci;
                            }
                        }
                    }
                    if (li != null) 
                    {
                        foreach (Pullenti.Ner.Core.IntOntologyToken nt in li) 
                        {
                            if (nt.Item != null) 
                            {
                                if (canBeLowChar || !Pullenti.Ner.Core.MiscHelper.IsAllCharactersLower(nt.BeginToken, nt.EndToken, false)) 
                                {
                                    CityItemToken ci = new CityItemToken(nt.BeginToken, nt.EndToken) { Typ = ItemType.City, OntoItem = nt.Item, Morph = nt.Morph };
                                    if (nt.BeginToken == nt.EndToken && (nt.BeginToken is Pullenti.Ner.TextToken)) 
                                    {
                                        ci.Doubtful = CheckDoubtful(nt.BeginToken as Pullenti.Ner.TextToken);
                                        string str = (nt.BeginToken as Pullenti.Ner.TextToken).Term;
                                        if (str != nt.Item.CanonicText) 
                                        {
                                            if (Pullenti.Morph.LanguageHelper.EndsWithEx(str, "О", "А", null, null)) 
                                                ci.AltValue = str;
                                        }
                                    }
                                    return ci;
                                }
                            }
                        }
                    }
                }
                if (li != null) 
                {
                    foreach (Pullenti.Ner.Core.IntOntologyToken nt in li) 
                    {
                        if (nt.Item == null) 
                        {
                            ItemType ty = (nt.Termin.Tag == null ? ItemType.Noun : (ItemType)nt.Termin.Tag);
                            CityItemToken ci = new CityItemToken(nt.BeginToken, nt.EndToken) { Typ = ty, Morph = nt.Morph };
                            ci.Value = nt.Termin.CanonicText;
                            if (ty == ItemType.Misc && ci.Value == "ЖИТЕЛЬ" && t.Previous != null) 
                            {
                                if (t.Previous.IsValue("МЕСТНЫЙ", "МІСЦЕВИЙ")) 
                                    return null;
                                if (t.Previous.Morph.Class.IsPronoun) 
                                    return null;
                            }
                            if (ty == ItemType.Noun && !t.Chars.IsAllLower) 
                            {
                                if (t.Morph.Class.IsProperSurname) 
                                    ci.Doubtful = true;
                            }
                            if (nt.BeginToken.Kit.BaseLanguage.IsUa) 
                            {
                                if (nt.BeginToken.IsValue("М", null) || nt.BeginToken.IsValue("Г", null)) 
                                {
                                    if (!nt.BeginToken.Chars.IsAllLower) 
                                        return null;
                                    ci.Doubtful = true;
                                }
                                else if (nt.BeginToken.IsValue("МІС", null)) 
                                {
                                    if ((t as Pullenti.Ner.TextToken).Term != "МІС") 
                                        return null;
                                    ci.Doubtful = true;
                                }
                            }
                            if (nt.BeginToken.Kit.BaseLanguage.IsRu) 
                            {
                                if (nt.BeginToken.IsValue("Г", null)) 
                                {
                                    if (nt.BeginToken.Previous != null && nt.BeginToken.Previous.Morph.Class.IsPreposition) 
                                    {
                                    }
                                    else 
                                    {
                                        if (!nt.BeginToken.Chars.IsAllLower) 
                                            return null;
                                        if ((nt.EndToken == nt.BeginToken && nt.EndToken.Next != null && !nt.EndToken.IsWhitespaceAfter) && ((nt.EndToken.Next.IsCharOf("\\/") || nt.EndToken.Next.IsHiphen))) 
                                            return null;
                                        if (!t.IsWhitespaceBefore && t.Previous != null) 
                                        {
                                            if (t.Previous.IsCharOf("\\/") || t.Previous.IsHiphen) 
                                                return null;
                                        }
                                    }
                                    ci.Doubtful = true;
                                }
                                else if (nt.BeginToken.IsValue("ГОР", null)) 
                                {
                                    if ((t as Pullenti.Ner.TextToken).Term != "ГОР") 
                                    {
                                        if (t.Chars.IsCapitalUpper) 
                                        {
                                            ci = null;
                                            break;
                                        }
                                        return null;
                                    }
                                    ci.Doubtful = true;
                                }
                                else if (nt.BeginToken.IsValue("ПОС", null)) 
                                {
                                    if ((t as Pullenti.Ner.TextToken).Term != "ПОС") 
                                        return null;
                                    ci.Doubtful = true;
                                }
                            }
                            Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t.Previous, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt1 != null && npt1.Adjectives.Count > 0) 
                            {
                                string s = npt1.Adjectives[0].GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                                if ((s == "РОДНОЙ" || s == "ЛЮБИМЫЙ" || s == "РІДНИЙ") || s == "КОХАНИЙ") 
                                    return null;
                            }
                            return ci;
                        }
                    }
                }
            }
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            if ((t as Pullenti.Ner.TextToken).Term == "СПБ" && !t.Chars.IsAllLower && m_StPeterburg != null) 
                return new CityItemToken(t, t) { Typ = ItemType.City, OntoItem = m_StPeterburg, Value = m_StPeterburg.CanonicText };
            if (t.Chars.IsAllLower) 
                return null;
            List<Pullenti.Ner.Core.IntOntologyToken> stds = m_StdAdjectives.TryAttach(t, null, false);
            if (stds != null) 
            {
                CityItemToken cit = _TryParse(stds[0].EndToken.Next, loc, false, null);
                if (cit != null && ((((cit.Typ == ItemType.ProperName && cit.Value != null)) || cit.Typ == ItemType.City))) 
                {
                    string adj = stds[0].Termin.CanonicText;
                    cit.Value = string.Format("{0} {1}", adj, cit.Value ?? cit.OntoItem.CanonicText);
                    if (cit.AltValue != null) 
                        cit.AltValue = string.Format("{0} {1}", adj, cit.AltValue);
                    cit.BeginToken = t;
                    Pullenti.Ner.Core.NounPhraseToken npt0 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt0 != null && npt0.EndToken == cit.EndToken) 
                    {
                        cit.Morph = npt0.Morph;
                        cit.Value = npt0.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    }
                    cit.Typ = ItemType.ProperName;
                    cit.Doubtful = false;
                    return cit;
                }
            }
            Pullenti.Ner.Token t1 = t;
            bool doubt = false;
            StringBuilder name = new StringBuilder();
            StringBuilder altname = null;
            int k = 0;
            bool isPrep = false;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (!(tt is Pullenti.Ner.TextToken)) 
                    break;
                if (!tt.Chars.IsLetter || ((tt.Chars.IsCyrillicLetter != t.Chars.IsCyrillicLetter && !tt.IsValue("НА", null)))) 
                    break;
                if (tt != t) 
                {
                    Pullenti.Ner.Address.Internal.StreetItemToken si = Pullenti.Ner.Address.Internal.StreetItemToken.TryParse(tt, null, false, null, false);
                    if (si != null && si.Typ == Pullenti.Ner.Address.Internal.StreetItemType.Noun) 
                    {
                        if (si.EndToken.Next == null || si.EndToken.Next.IsCharOf(",.")) 
                        {
                        }
                        else 
                            break;
                    }
                    if (tt.LengthChar < 2) 
                        break;
                    if ((tt.LengthChar < 3) && !tt.IsValue("НА", null)) 
                    {
                        if (tt.IsWhitespaceBefore) 
                            break;
                    }
                }
                if (name.Length > 0) 
                {
                    name.Append('-');
                    if (altname != null) 
                        altname.Append('-');
                }
                if ((tt is Pullenti.Ner.TextToken) && ((isPrep || ((k > 0 && !tt.GetMorphClassInDictionary().IsProperGeo))))) 
                {
                    name.Append((tt as Pullenti.Ner.TextToken).Term);
                    if (altname != null) 
                        altname.Append((tt as Pullenti.Ner.TextToken).Term);
                }
                else 
                {
                    string ss = GetNormalGeo(tt);
                    if (ss != (tt as Pullenti.Ner.TextToken).Term) 
                    {
                        if (altname == null) 
                            altname = new StringBuilder();
                        altname.Append(name.ToString());
                        altname.Append((tt as Pullenti.Ner.TextToken).Term);
                    }
                    name.Append(ss);
                }
                t1 = tt;
                isPrep = tt.Morph.Class.IsPreposition;
                if (tt.Next == null || tt.Next.Next == null) 
                    break;
                if (!tt.Next.IsHiphen) 
                    break;
                if (tt.IsWhitespaceAfter || tt.Next.IsWhitespaceAfter) 
                {
                    if (tt.WhitespacesAfterCount > 1 || tt.Next.WhitespacesAfterCount > 1) 
                        break;
                    if (tt.Next.Next.Chars != tt.Chars) 
                        break;
                    Pullenti.Ner.Token ttt = tt.Next.Next.Next;
                    if (ttt != null && !ttt.IsNewlineAfter) 
                    {
                        if (ttt.Chars.IsLetter) 
                            break;
                    }
                }
                tt = tt.Next;
                k++;
            }
            if (k > 0) 
            {
                if (k > 2) 
                    return null;
                CityItemToken reee = new CityItemToken(t, t1) { Typ = ItemType.ProperName, Value = name.ToString(), Doubtful = doubt };
                if (altname != null) 
                    reee.AltValue = altname.ToString();
                return reee;
            }
            if (t == null) 
                return null;
            Pullenti.Ner.Core.NounPhraseToken npt = (t.Chars.IsLatinLetter ? null : Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null));
            if ((npt != null && npt.EndToken != t && npt.Adjectives.Count > 0) && !npt.Adjectives[0].EndToken.Next.IsComma) 
            {
                CityItemToken cit = _TryParse(t.Next, loc, false, null);
                if (cit != null && cit.Typ == ItemType.Noun && ((Pullenti.Morph.LanguageHelper.EndsWithEx(cit.Value, "ПУНКТ", "ПОСЕЛЕНИЕ", "ПОСЕЛЕННЯ", "ПОСЕЛОК") || t.Next.IsValue("ГОРОДОК", null)))) 
                    return new CityItemToken(t, t) { Typ = ItemType.City, Value = t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false), Morph = npt.Morph };
                else 
                {
                    if (npt.EndToken.Chars != t.Chars) 
                    {
                        if (npt.EndToken.Chars.IsAllLower && ((npt.EndToken.Next == null || npt.EndToken.Next.IsComma))) 
                        {
                        }
                        else 
                            return null;
                    }
                    if (npt.Adjectives.Count != 1) 
                        return null;
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(npt.EndToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt1 == null || npt1.Adjectives.Count == 0) 
                    {
                        Pullenti.Ner.Address.Internal.StreetItemToken si = Pullenti.Ner.Address.Internal.StreetItemToken.TryParse(npt.EndToken, null, false, null, false);
                        if (si == null || si.Typ != Pullenti.Ner.Address.Internal.StreetItemType.Noun) 
                        {
                            t1 = npt.EndToken;
                            doubt = CheckDoubtful(t1 as Pullenti.Ner.TextToken);
                            return new CityItemToken(t, t1) { Typ = ItemType.ProperName, Value = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false), Doubtful = doubt, Morph = npt.Morph };
                        }
                    }
                }
            }
            if (t.Next != null && t.Next.Chars == t.Chars && !t.IsNewlineAfter) 
            {
                bool ok = false;
                if (t.Next.Next == null || t.Next.Next.Chars != t.Chars) 
                    ok = true;
                else if (t.Next.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                    ok = true;
                else if (m_Recursive == 0) 
                {
                    m_Recursive++;
                    List<TerrItemToken> tis = TerrItemToken.TryParseList(t.Next.Next, loc, 2);
                    m_Recursive--;
                    if (tis != null && tis.Count > 1) 
                    {
                        if (tis[0].IsAdjective && tis[1].TerminItem != null) 
                            ok = true;
                    }
                }
                if (ok && (t.Next is Pullenti.Ner.TextToken)) 
                {
                    doubt = CheckDoubtful(t.Next as Pullenti.Ner.TextToken);
                    Pullenti.Ner.Core.StatisticBigrammInfo stat = t.Kit.Statistics.GetBigrammInfo(t, t.Next);
                    bool ok1 = false;
                    if ((stat != null && stat.PairCount >= 2 && stat.PairCount == stat.SecondCount) && !stat.SecondHasOtherFirst) 
                    {
                        if (stat.PairCount > 2) 
                            doubt = false;
                        ok1 = true;
                    }
                    else if (m_StdAdjectives.TryAttach(t, null, false) != null && (t.Next is Pullenti.Ner.TextToken)) 
                        ok1 = true;
                    else if (((t.Next.Next == null || t.Next.Next.IsComma)) && t.Morph.Class.IsNoun && ((t.Next.Morph.Class.IsAdjective || t.Next.Morph.Class.IsNoun))) 
                        ok1 = true;
                    if (ok1) 
                    {
                        CityItemToken tne = _tryParseInt(t.Next, loc, false, null);
                        if (tne != null && tne.Typ == ItemType.Noun) 
                        {
                        }
                        else 
                        {
                            name.AppendFormat(" {0}", (t.Next as Pullenti.Ner.TextToken).Term);
                            if (altname != null) 
                                altname.AppendFormat(" {0}", (t.Next as Pullenti.Ner.TextToken).Term);
                            t1 = t.Next;
                            return new CityItemToken(t, t1) { Typ = ItemType.ProperName, Value = name.ToString(), AltValue = (altname == null ? null : altname.ToString()), Doubtful = doubt, Morph = t.Next.Morph };
                        }
                    }
                }
            }
            if (t.LengthChar < 2) 
                return null;
            t1 = t;
            doubt = CheckDoubtful(t as Pullenti.Ner.TextToken);
            if (((t.Next != null && prev != null && prev.Typ == ItemType.Noun) && t.Next.Chars.IsCyrillicLetter && t.Next.Chars.IsAllLower) && t.WhitespacesAfterCount == 1) 
            {
                Pullenti.Ner.Token tt = t.Next;
                bool ok = false;
                if (tt.Next == null || tt.Next.IsCharOf(",;")) 
                    ok = true;
                if (ok && Pullenti.Ner.Address.Internal.AddressItemToken.TryParse(tt.Next, null, false, false, null) == null) 
                {
                    t1 = tt;
                    name.AppendFormat(" {0}", t1.GetSourceText().ToUpper());
                }
            }
            if (Pullenti.Ner.Core.MiscHelper.IsEngArticle(t)) 
                return null;
            CityItemToken res = new CityItemToken(t, t1) { Typ = ItemType.ProperName, Value = name.ToString(), AltValue = (altname == null ? null : altname.ToString()), Doubtful = doubt, Morph = t.Morph };
            if (t1 == t && (t1 is Pullenti.Ner.TextToken) && (t1 as Pullenti.Ner.TextToken).Term0 != null) 
                res.AltValue = (t1 as Pullenti.Ner.TextToken).Term0;
            bool sog = false;
            bool glas = false;
            foreach (char ch in res.Value) 
            {
                if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(ch) || Pullenti.Morph.LanguageHelper.IsLatinVowel(ch)) 
                    glas = true;
                else 
                    sog = true;
            }
            if (!glas || !sog) 
                return null;
            if (t == t1 && (t is Pullenti.Ner.TextToken)) 
            {
                if ((t as Pullenti.Ner.TextToken).Term != res.Value) 
                    res.AltValue = (t as Pullenti.Ner.TextToken).Term;
            }
            return res;
        }
        public static CityItemToken TryParseBack(Pullenti.Ner.Token t)
        {
            while (t != null && ((t.IsCharOf("(,") || t.IsAnd))) 
            {
                t = t.Previous;
            }
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            int cou = 0;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Previous) 
            {
                if (!(tt is Pullenti.Ner.TextToken)) 
                    return null;
                if (!tt.Chars.IsLetter) 
                    continue;
                CityItemToken res = TryParse(tt, null, true, null);
                if (res != null && res.EndToken == t) 
                    return res;
                if ((++cou) > 2) 
                    break;
            }
            return null;
        }
        static string GetNormalGeo(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            if (tt.Term[tt.Term.Length - 1] == 'О') 
                return tt.Term;
            if (tt.Term[tt.Term.Length - 1] == 'Ы') 
                return tt.Term;
            foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
            {
                if (wf.Class.IsProperGeo && (wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    return (wf as Pullenti.Morph.MorphWordForm).NormalCase;
            }
            bool geoEqTerm = false;
            foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
            {
                if (wf.Class.IsProperGeo) 
                {
                    string ggg = (wf as Pullenti.Morph.MorphWordForm).NormalCase;
                    if (ggg == tt.Term) 
                        geoEqTerm = true;
                    else if (!wf.Case.IsNominative) 
                        return ggg;
                }
            }
            if (geoEqTerm) 
                return tt.Term;
            if (tt.Morph.ItemsCount > 0) 
                return (tt.Morph[0] as Pullenti.Morph.MorphWordForm).NormalCase;
            else 
                return tt.Term;
        }
        public static void Initialize()
        {
            if (m_Ontology != null) 
                return;
            m_Ontology = new Pullenti.Ner.Core.IntOntologyCollection();
            m_CityAdjectives = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("ГОРОД");
            t.AddAbridge("ГОР.");
            t.AddAbridge("Г.");
            t.Tag = ItemType.Noun;
            t.AddVariant("ГОРОДОК", false);
            t.AddVariant("ШАХТЕРСКИЙ ГОРОДОК", false);
            t.AddVariant("ПРИМОРСКИЙ ГОРОДОК", false);
            t.AddVariant("МАЛЕНЬКИЙ ГОРОДОК", false);
            t.AddVariant("НЕБОЛЬШОЙ ГОРОДОК", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("CITY");
            t.Tag = ItemType.Noun;
            t.AddVariant("TOWN", false);
            t.AddVariant("CAPITAL", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСТО", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("МІС.");
            t.AddAbridge("М.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОРОД-ГЕРОЙ") { CanonicText = "ГОРОД" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСТО-ГЕРОЙ", Pullenti.Morph.MorphLang.UA) { CanonicText = "МІСТО" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОРОД-КУРОРТ") { CanonicText = "ГОРОД" };
            t.AddAbridge("Г.К.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСТО-КУРОРТ", Pullenti.Morph.MorphLang.UA) { CanonicText = "МІСТО" };
            t.AddAbridge("М.К.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕЛО");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДЕРЕВНЯ");
            t.AddAbridge("ДЕР.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕЛЕНИЕ");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕЛО", Pullenti.Morph.MorphLang.UA);
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОРТ");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОРТ", Pullenti.Morph.MorphLang.UA);
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОСЕЛОК");
            t.AddAbridge("ПОС.");
            t.Tag = ItemType.Noun;
            t.AddVariant("ПОСЕЛЕНИЕ", false);
            t.AddVariant("ЖИЛОЙ ПОСЕЛОК", false);
            t.AddVariant("КОТТЕДЖНЫЙ ПОСЕЛОК", false);
            t.AddVariant("ВАХТОВЫЙ ПОСЕЛОК", false);
            t.AddVariant("ШАХТЕРСКИЙ ПОСЕЛОК", false);
            t.AddVariant("ДАЧНЫЙ ПОСЕЛОК", false);
            t.AddVariant("КУРОРТНЫЙ ПОСЕЛОК", false);
            t.AddVariant("ПОСЕЛОК СОВХОЗА", false);
            t.AddVariant("ПОСЕЛОК КОЛХОЗА", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕЛИЩЕ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("СЕЛ.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОСЕЛОК ГОРОДСКОГО ТИПА");
            t.Acronym = (t.AcronymSmart = "ПГТ");
            t.AddAbridge("ПГТ.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕЛИЩЕ МІСЬКОГО ТИПУ", Pullenti.Morph.MorphLang.UA);
            t.Acronym = (t.AcronymSmart = "СМТ");
            t.AddAbridge("СМТ.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("РАБОЧИЙ ПОСЕЛОК");
            t.AddAbridge("Р.П.");
            t.Tag = ItemType.Noun;
            t.AddAbridge("РАБ.П.");
            t.AddAbridge("Р.ПОС.");
            t.AddAbridge("РАБ.ПОС.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("РОБОЧЕ СЕЛИЩЕ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("Р.С.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДАЧНЫЙ ПОСЕЛОК");
            t.AddAbridge("Д.П.");
            t.Tag = ItemType.Noun;
            t.AddAbridge("ДАЧ.П.");
            t.AddAbridge("Д.ПОС.");
            t.AddAbridge("ДАЧ.ПОС.");
            t.AddVariant("ЖИЛИЩНО ДАЧНЫЙ ПОСЕЛОК", false);
            t.AddVariant("ДАЧНОЕ ПОСЕЛЕНИЕ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДАЧНЕ СЕЛИЩЕ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("Д.С.");
            t.Tag = ItemType.Noun;
            t.AddAbridge("ДАЧ.С.");
            t.AddAbridge("Д.СЕЛ.");
            t.AddAbridge("ДАЧ.СЕЛ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОРОДСКОЕ ПОСЕЛЕНИЕ");
            t.AddAbridge("Г.П.");
            t.Tag = ItemType.Noun;
            t.AddAbridge("Г.ПОС.");
            t.AddAbridge("ГОР.П.");
            t.AddAbridge("ГОР.ПОС.");
            t.AddVariant("ГОРОДСКОЙ ОКРУГ", false);
            t.AddAbridge("ГОР. ОКРУГ");
            t.AddAbridge("Г.О.");
            t.AddAbridge("Г.О.Г.");
            t.AddAbridge("ГОРОДСКОЙ ОКРУГ Г.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПОСЕЛКОВОЕ ПОСЕЛЕНИЕ") { CanonicText = "ПОСЕЛОК", Tag = ItemType.Noun };
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСЬКЕ ПОСЕЛЕННЯ", Pullenti.Morph.MorphLang.UA);
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕЛЬСКОЕ ПОСЕЛЕНИЕ");
            t.Tag = ItemType.Noun;
            t.AddAbridge("С.ПОС.");
            t.AddAbridge("С.П.");
            t.AddVariant("СЕЛЬСОВЕТ", false);
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СІЛЬСЬКЕ ПОСЕЛЕННЯ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("С.ПОС.");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАНИЦА");
            t.Tag = ItemType.Noun;
            t.AddAbridge("СТ-ЦА");
            t.AddAbridge("СТАН-ЦА");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАНИЦЯ", Pullenti.Morph.MorphLang.UA);
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТОЛИЦА") { CanonicText = "ГОРОД" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТОЛИЦЯ", Pullenti.Morph.MorphLang.UA) { CanonicText = "МІСТО" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАНЦИЯ");
            t.AddAbridge("СТАНЦ.");
            t.AddAbridge("СТ.");
            t.AddAbridge("СТАН.");
            t.Tag = ItemType.Noun;
            t.AddVariant("ПЛАТФОРМА", false);
            t.AddAbridge("ПЛАТФ.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАНЦІЯ", Pullenti.Morph.MorphLang.UA);
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЖЕЛЕЗНОДОРОЖНАЯ СТАНЦИЯ");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗАЛІЗНИЧНА СТАНЦІЯ", Pullenti.Morph.MorphLang.UA);
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НАСЕЛЕННЫЙ ПУНКТ");
            t.Tag = ItemType.Noun;
            t.AddAbridge("Н.П.");
            t.AddAbridge("Б.Н.П.");
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("НАСЕЛЕНИЙ ПУНКТ", Pullenti.Morph.MorphLang.UA);
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("РАЙОННЫЙ ЦЕНТР") { CanonicText = "НАСЕЛЕННЫЙ ПУНКТ" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("РАЙОННИЙ ЦЕНТР", Pullenti.Morph.MorphLang.UA) { CanonicText = "НАСЕЛЕНИЙ ПУНКТ" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ГОРОДСКОЙ ОКРУГ") { CanonicText = "НАСЕЛЕННЫЙ ПУНКТ" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МІСЬКИЙ ОКРУГ", Pullenti.Morph.MorphLang.UA) { CanonicText = "НАСЕЛЕНИЙ ПУНКТ" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОБЛАСТНОЙ ЦЕНТР") { CanonicText = "НАСЕЛЕННЫЙ ПУНКТ" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОБЛАСНИЙ ЦЕНТР", Pullenti.Morph.MorphLang.UA) { CanonicText = "НАСЕЛЕНИЙ ПУНКТ" };
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ХУТОР");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АУЛ");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ААЛ");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("АРБАН");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЫСЕЛКИ");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("МЕСТЕЧКО");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("УРОЧИЩЕ");
            t.Tag = ItemType.Noun;
            m_Ontology.Add(t);
            t = new Pullenti.Ner.Core.Termin("УСАДЬБА");
            t.Tag = ItemType.Noun;
            t.AddVariant("ЦЕНТРАЛЬНАЯ УСАДЬБА", false);
            t.AddAbridge("ЦЕНТР.УС.");
            t.AddAbridge("ЦЕНТР.УСАДЬБА");
            t.AddAbridge("Ц/У");
            t.AddAbridge("УС-БА");
            t.AddAbridge("ЦЕНТР.УС-БА");
            m_Ontology.Add(t);
            foreach (string s in new string[] {"ЖИТЕЛЬ", "МЭР"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s) { Tag = ItemType.Misc });
            }
            foreach (string s in new string[] {"ЖИТЕЛЬ", "МЕР"}) 
            {
                m_Ontology.Add(new Pullenti.Ner.Core.Termin(s, Pullenti.Morph.MorphLang.UA) { Tag = ItemType.Misc });
            }
            t = new Pullenti.Ner.Core.Termin("АДМИНИСТРАЦИЯ") { Tag = ItemType.Misc };
            t.AddAbridge("АДМ.");
            m_Ontology.Add(t);
            m_StdAdjectives = new Pullenti.Ner.Core.IntOntologyCollection();
            t = new Pullenti.Ner.Core.Termin("ВЕЛИКИЙ");
            t.AddAbridge("ВЕЛ.");
            t.AddAbridge("ВЕЛИК.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("БОЛЬШОЙ");
            t.AddAbridge("БОЛ.");
            t.AddAbridge("БОЛЬШ.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("МАЛЫЙ");
            t.AddAbridge("МАЛ.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕРХНИЙ");
            t.AddAbridge("ВЕР.");
            t.AddAbridge("ВЕРХ.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("НИЖНИЙ");
            t.AddAbridge("НИЖ.");
            t.AddAbridge("НИЖН.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("СРЕДНИЙ");
            t.AddAbridge("СРЕД.");
            t.AddAbridge("СРЕДН.");
            t.AddAbridge("СР.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАРЫЙ");
            t.AddAbridge("СТ.");
            t.AddAbridge("СТАР.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОВЫЙ");
            t.AddAbridge("НОВ.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕЛИКИЙ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("ВЕЛ.");
            t.AddAbridge("ВЕЛИК.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("МАЛИЙ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("МАЛ.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕРХНІЙ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("ВЕР.");
            t.AddAbridge("ВЕРХ.");
            t.AddAbridge("ВЕРХН.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("НИЖНІЙ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("НИЖ.");
            t.AddAbridge("НИЖН.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЕРЕДНІЙ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("СЕР.");
            t.AddAbridge("СЕРЕД.");
            t.AddAbridge("СЕРЕДН.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("СТАРИЙ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("СТ.");
            t.AddAbridge("СТАР.");
            m_StdAdjectives.Add(t);
            t = new Pullenti.Ner.Core.Termin("НОВИЙ", Pullenti.Morph.MorphLang.UA);
            t.AddAbridge("НОВ.");
            m_StdAdjectives.Add(t);
            m_StdAdjectives.Add(new Pullenti.Ner.Core.Termin("SAN"));
            m_StdAdjectives.Add(new Pullenti.Ner.Core.Termin("LOS"));
            byte[] dat = Pullenti.Ner.Address.Internal.ResourceHelper.GetBytes("c.dat");
            if (dat == null) 
                throw new Exception("Not found resource file c.dat in Analyzer.Location");
            using (MemoryStream tmp = new MemoryStream(MiscLocationHelper.Deflate(dat))) 
            {
                tmp.Position = 0;
                XmlDocument xml = new XmlDocument();
                xml.Load(tmp);
                foreach (XmlNode x in xml.DocumentElement.ChildNodes) 
                {
                    if (x.Name == "bigcity") 
                        LoadBigCity(x);
                    else if (x.Name == "city") 
                        LoadCity(x);
                }
            }
        }
        static void LoadCity(XmlNode xml)
        {
            Pullenti.Ner.Core.IntOntologyItem ci = new Pullenti.Ner.Core.IntOntologyItem(null);
            Pullenti.Ner.Core.IntOntologyCollection onto = m_Ontology;
            Pullenti.Morph.MorphLang lang = Pullenti.Morph.MorphLang.RU;
            if (xml.Attributes["l"] != null && xml.Attributes["l"].InnerText == "ua") 
                lang = Pullenti.Morph.MorphLang.UA;
            foreach (XmlNode x in xml.ChildNodes) 
            {
                if (x.Name == "n") 
                {
                    string v = x.InnerText;
                    Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin();
                    t.InitByNormalText(v, lang);
                    ci.Termins.Add(t);
                    t.AddStdAbridges();
                    if (v.StartsWith("SAINT ")) 
                        t.AddAbridge("ST. " + v.Substring(6));
                    else if (v.StartsWith("SAITNE ")) 
                        t.AddAbridge("STE. " + v.Substring(7));
                }
            }
            onto.AddItem(ci);
        }
        static void LoadBigCity(XmlNode xml)
        {
            Pullenti.Ner.Core.IntOntologyItem ci = new Pullenti.Ner.Core.IntOntologyItem(null);
            ci.MiscAttr = ci;
            string adj = null;
            Pullenti.Ner.Core.IntOntologyCollection onto = m_Ontology;
            Pullenti.Ner.Core.TerminCollection cityAdj = m_CityAdjectives;
            Pullenti.Morph.MorphLang lang = Pullenti.Morph.MorphLang.RU;
            if (xml.Attributes["l"] != null) 
            {
                string la = xml.Attributes["l"].InnerText;
                if (la == "ua") 
                    lang = Pullenti.Morph.MorphLang.UA;
                else if (la == "en") 
                    lang = Pullenti.Morph.MorphLang.EN;
            }
            foreach (XmlNode x in xml.ChildNodes) 
            {
                if (x.Name == "n") 
                {
                    string v = x.InnerText;
                    if (string.IsNullOrEmpty(v)) 
                        continue;
                    Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin();
                    t.InitByNormalText(v, lang);
                    ci.Termins.Add(t);
                    if (v == "САНКТ-ПЕТЕРБУРГ") 
                    {
                        if (m_StPeterburg == null) 
                            m_StPeterburg = ci;
                        t.Acronym = "СПБ";
                        t.AddAbridge("С.ПЕТЕРБУРГ");
                        t.AddAbridge("СП-Б");
                        ci.Termins.Add(new Pullenti.Ner.Core.Termin("ПЕТЕРБУРГ", lang));
                    }
                    else if (v.StartsWith("SAINT ")) 
                        t.AddAbridge("ST. " + v.Substring(6));
                    else if (v.StartsWith("SAITNE ")) 
                        t.AddAbridge("STE. " + v.Substring(7));
                }
                else if (x.Name == "a") 
                    adj = x.InnerText;
            }
            onto.AddItem(ci);
            if (!string.IsNullOrEmpty(adj)) 
            {
                Pullenti.Ner.Core.Termin at = new Pullenti.Ner.Core.Termin();
                at.InitByNormalText(adj, lang);
                at.Tag = ci;
                cityAdj.Add(at);
                bool spb = adj == "САНКТ-ПЕТЕРБУРГСКИЙ" || adj == "САНКТ-ПЕТЕРБУРЗЬКИЙ";
                if (spb) 
                    cityAdj.Add(new Pullenti.Ner.Core.Termin(adj.Substring(6), lang) { Tag = ci });
            }
        }
        static Pullenti.Ner.Core.IntOntologyCollection m_Ontology;
        static Pullenti.Ner.Core.IntOntologyItem m_StPeterburg;
        public static Pullenti.Ner.Core.TerminCollection m_CityAdjectives;
        static Pullenti.Ner.Core.IntOntologyCollection m_StdAdjectives;
    }
}