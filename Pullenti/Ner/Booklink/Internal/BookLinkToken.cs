/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Booklink.Internal
{
    public class BookLinkToken : Pullenti.Ner.MetaToken
    {
        public BookLinkTyp Typ = BookLinkTyp.Undefined;
        public string Value;
        public Pullenti.Ner.ReferentToken Tok;
        public Pullenti.Ner.Referent Ref;
        public double AddCoef = 0;
        public Pullenti.Ner.Person.Internal.FioTemplateType PersonTemplate = Pullenti.Ner.Person.Internal.FioTemplateType.Undefined;
        public string StartOfName;
        public BookLinkToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public static BookLinkToken TryParseAuthor(Pullenti.Ner.Token t, Pullenti.Ner.Person.Internal.FioTemplateType prevPersTemplate = Pullenti.Ner.Person.Internal.FioTemplateType.Undefined)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.ReferentToken rtp = Pullenti.Ner.Person.Internal.PersonItemToken.TryParsePerson(t, prevPersTemplate);
            if (rtp != null) 
            {
                BookLinkToken re;
                if (rtp.Data == null) 
                    re = new BookLinkToken(t, (rtp == t ? t : rtp.EndToken)) { Typ = BookLinkTyp.Person, Ref = rtp.Referent };
                else 
                    re = new BookLinkToken(t, rtp.EndToken) { Typ = BookLinkTyp.Person, Tok = rtp };
                re.PersonTemplate = (Pullenti.Ner.Person.Internal.FioTemplateType)rtp.MiscAttrs;
                for (Pullenti.Ner.Token tt = rtp.BeginToken; tt != null && tt.EndChar <= rtp.EndChar; tt = tt.Next) 
                {
                    if (!(tt.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
                        continue;
                    Pullenti.Ner.ReferentToken rt = tt as Pullenti.Ner.ReferentToken;
                    if (rt.BeginToken.Chars.IsCapitalUpper && tt != rtp.BeginToken) 
                    {
                        re.StartOfName = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(rt, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        break;
                    }
                    return null;
                }
                return re;
            }
            if (t.IsChar('[')) 
            {
                BookLinkToken re = TryParseAuthor(t.Next, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined);
                if (re != null && re.EndToken.Next != null && re.EndToken.Next.IsChar(']')) 
                {
                    re.BeginToken = t;
                    re.EndToken = re.EndToken.Next;
                    return re;
                }
            }
            if (((t.IsValue("И", null) || t.IsValue("ET", null))) && t.Next != null) 
            {
                if (t.Next.IsValue("ДРУГИЕ", null) || t.Next.IsValue("ДР", null) || t.Next.IsValue("AL", null)) 
                {
                    BookLinkToken res = new BookLinkToken(t, t.Next) { Typ = BookLinkTyp.AndOthers };
                    if (t.Next.Next != null && t.Next.Next.IsChar('.')) 
                        res.EndToken = res.EndToken.Next;
                    return res;
                }
            }
            return null;
        }
        public static BookLinkToken TryParse(Pullenti.Ner.Token t, int lev = 0)
        {
            if (t == null || lev > 3) 
                return null;
            BookLinkToken res = _tryParse(t, lev + 1);
            if (res == null) 
            {
                if (t.IsHiphen) 
                    res = _tryParse(t.Next, lev + 1);
                if (res == null) 
                    return null;
            }
            if (res.EndToken.Next != null && res.EndToken.Next.IsChar('.')) 
                res.EndToken = res.EndToken.Next;
            t = res.EndToken.Next;
            if (t != null && t.IsComma) 
                t = t.Next;
            if (res.Typ == BookLinkTyp.Geo || res.Typ == BookLinkTyp.Press) 
            {
                BookLinkToken re2 = _tryParse(t, lev + 1);
                if (re2 != null && ((re2.Typ == BookLinkTyp.Press || re2.Typ == BookLinkTyp.Year))) 
                    res.AddCoef += 1;
            }
            return res;
        }
        static BookLinkToken _tryParse(Pullenti.Ner.Token t, int lev)
        {
            if (t == null || lev > 3) 
                return null;
            if (t.IsChar('[')) 
            {
                BookLinkToken re = _tryParse(t.Next, lev + 1);
                if (re != null && re.EndToken.Next != null && re.EndToken.Next.IsChar(']')) 
                {
                    re.BeginToken = t;
                    re.EndToken = re.EndToken.Next;
                    return re;
                }
                if (re != null && re.EndToken.IsChar(']')) 
                {
                    re.BeginToken = t;
                    return re;
                }
                if (re != null) 
                {
                    if (re.Typ == BookLinkTyp.Sostavitel || re.Typ == BookLinkTyp.Editors) 
                        return re;
                }
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    if ((br.EndToken.Previous is Pullenti.Ner.NumberToken) && (br.LengthChar < 30)) 
                        return new BookLinkToken(t, br.EndToken) { Typ = BookLinkTyp.Number, Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(br.BeginToken.Next, br.EndToken.Previous, Pullenti.Ner.Core.GetTextAttr.No) };
                }
            }
            Pullenti.Ner.Token t0 = t;
            if (t is Pullenti.Ner.ReferentToken) 
            {
                if (t.GetReferent() is Pullenti.Ner.Person.PersonReferent) 
                    return TryParseAuthor(t, Pullenti.Ner.Person.Internal.FioTemplateType.Undefined);
                if (t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                    return new BookLinkToken(t, t) { Typ = BookLinkTyp.Geo, Ref = t.GetReferent() };
                if (t.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                {
                    Pullenti.Ner.Date.DateReferent dr = t.GetReferent() as Pullenti.Ner.Date.DateReferent;
                    if (dr.Slots.Count == 1 && dr.Year > 0) 
                        return new BookLinkToken(t, t) { Typ = BookLinkTyp.Year, Value = dr.Year.ToString() };
                    if (dr.Year > 0 && t.Previous != null && t.Previous.IsComma) 
                        return new BookLinkToken(t, t) { Typ = BookLinkTyp.Year, Value = dr.Year.ToString() };
                }
                if (t.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) 
                {
                    Pullenti.Ner.Org.OrganizationReferent org = t.GetReferent() as Pullenti.Ner.Org.OrganizationReferent;
                    if (org.Kind == Pullenti.Ner.Org.OrganizationKind.Press) 
                        return new BookLinkToken(t, t) { Typ = BookLinkTyp.Press, Ref = org };
                }
                if (t.GetReferent() is Pullenti.Ner.Uri.UriReferent) 
                {
                    Pullenti.Ner.Uri.UriReferent uri = t.GetReferent() as Pullenti.Ner.Uri.UriReferent;
                    if ((uri.Scheme == "http" || uri.Scheme == "https" || uri.Scheme == "ftp") || uri.Scheme == null) 
                        return new BookLinkToken(t, t) { Typ = BookLinkTyp.Url, Ref = uri };
                }
            }
            Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
            {
                BookLinkTyp typ = (BookLinkTyp)tok.Termin.Tag;
                bool ok = true;
                if (typ == BookLinkTyp.Type || typ == BookLinkTyp.NameTail || typ == BookLinkTyp.ElectronRes) 
                {
                    if (t.Previous != null && ((t.Previous.IsCharOf(".:[") || t.Previous.IsHiphen))) 
                    {
                    }
                    else 
                        ok = false;
                }
                if (ok) 
                    return new BookLinkToken(t, tok.EndToken) { Typ = typ, Value = tok.Termin.CanonicText };
                if (typ == BookLinkTyp.ElectronRes) 
                {
                    for (Pullenti.Ner.Token tt = tok.EndToken.Next; tt != null; tt = tt.Next) 
                    {
                        if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsLetter) 
                            continue;
                        if (tt.GetReferent() is Pullenti.Ner.Uri.UriReferent) 
                            return new BookLinkToken(t, tt) { Typ = BookLinkTyp.ElectronRes, Ref = tt.GetReferent() };
                        break;
                    }
                }
            }
            if (t.IsChar('/')) 
            {
                BookLinkToken res = new BookLinkToken(t, t) { Typ = BookLinkTyp.Delimeter, Value = "/" };
                if (t.Next != null && t.Next.IsChar('/')) 
                {
                    res.EndToken = t.Next;
                    res.Value = "//";
                }
                if (!t.IsWhitespaceBefore && !t.IsWhitespaceAfter) 
                {
                    int coo = 3;
                    bool no = true;
                    for (Pullenti.Ner.Token tt = t.Next; tt != null && coo > 0; tt = tt.Next,coo--) 
                    {
                        BookLinkToken vvv = TryParse(tt, lev + 1);
                        if (vvv != null && vvv.Typ != BookLinkTyp.Number) 
                        {
                            no = false;
                            break;
                        }
                    }
                    if (no) 
                        return null;
                }
                return res;
            }
            if ((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).IntValue != null && (t as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
            {
                BookLinkToken res = new BookLinkToken(t, t) { Typ = BookLinkTyp.Number, Value = (t as Pullenti.Ner.NumberToken).Value.ToString() };
                int val = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                if (val >= 1930 && (val < 2030)) 
                    res.Typ = BookLinkTyp.Year;
                if (t.Next != null && t.Next.IsChar('.')) 
                    res.EndToken = t.Next;
                else if ((t.Next != null && t.Next.LengthChar == 1 && !t.Next.Chars.IsLetter) && t.Next.IsWhitespaceAfter) 
                    res.EndToken = t.Next;
                else if (t.Next is Pullenti.Ner.TextToken) 
                {
                    string term = (t.Next as Pullenti.Ner.TextToken).Term;
                    if (((term == "СТР" || term == "C" || term == "С") || term == "P" || term == "S") || term == "PAGES") 
                    {
                        res.EndToken = t.Next;
                        res.Typ = BookLinkTyp.Pages;
                        res.Value = (t as Pullenti.Ner.NumberToken).Value.ToString();
                    }
                }
                return res;
            }
            if (t is Pullenti.Ner.TextToken) 
            {
                string term = (t as Pullenti.Ner.TextToken).Term;
                if (((((((term == "СТР" || term == "C" || term == "С") || term == "ТОМ" || term == "T") || term == "Т" || term == "P") || term == "PP" || term == "V") || term == "VOL" || term == "S") || term == "СТОР" || t.IsValue("PAGE", null)) || t.IsValue("СТРАНИЦА", "СТОРІНКА")) 
                {
                    Pullenti.Ner.Token tt = t.Next;
                    while (tt != null) 
                    {
                        if (tt.IsCharOf(".:~")) 
                            tt = tt.Next;
                        else 
                            break;
                    }
                    if (tt is Pullenti.Ner.NumberToken) 
                    {
                        BookLinkToken res = new BookLinkToken(t, tt) { Typ = BookLinkTyp.PageRange };
                        Pullenti.Ner.Token tt0 = tt;
                        Pullenti.Ner.Token tt1 = tt;
                        for (tt = tt.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsCharOf(",") || tt.IsHiphen) 
                            {
                                if (tt.Next is Pullenti.Ner.NumberToken) 
                                {
                                    tt = tt.Next;
                                    res.EndToken = tt;
                                    tt1 = tt;
                                    continue;
                                }
                            }
                            break;
                        }
                        res.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt0, tt1, Pullenti.Ner.Core.GetTextAttr.No);
                        return res;
                    }
                }
                if ((term == "M" || term == "М" || term == "СПБ") || term == "K" || term == "К") 
                {
                    if (t.Next != null && t.Next.IsCharOf(":;")) 
                    {
                        BookLinkToken re = new BookLinkToken(t, t.Next) { Typ = BookLinkTyp.Geo };
                        return re;
                    }
                    if (t.Next != null && t.Next.IsCharOf(".")) 
                    {
                        BookLinkToken res = new BookLinkToken(t, t.Next) { Typ = BookLinkTyp.Geo };
                        if (t.Next.Next != null && t.Next.Next.IsCharOf(":;")) 
                            res.EndToken = t.Next.Next;
                        else if (t.Next.Next != null && (t.Next.Next is Pullenti.Ner.NumberToken)) 
                        {
                        }
                        else if (t.Next.Next != null && t.Next.Next.IsComma && (t.Next.Next.Next is Pullenti.Ner.NumberToken)) 
                        {
                        }
                        else 
                            return null;
                        return res;
                    }
                }
                if (term == "ПЕР" || term == "ПЕРЕВ" || term == "ПЕРЕВОД") 
                {
                    Pullenti.Ner.Token tt = t;
                    if (tt.Next != null && tt.Next.IsChar('.')) 
                        tt = tt.Next;
                    if (tt.Next != null && ((tt.Next.IsValue("C", null) || tt.Next.IsValue("С", null)))) 
                    {
                        tt = tt.Next;
                        if (tt.Next == null || tt.WhitespacesAfterCount > 2) 
                            return null;
                        BookLinkToken re = new BookLinkToken(t, tt.Next) { Typ = BookLinkTyp.Translate };
                        return re;
                    }
                }
                if (term == "ТАМ" || term == "ТАМЖЕ") 
                {
                    BookLinkToken res = new BookLinkToken(t, t) { Typ = BookLinkTyp.Tamze };
                    if (t.Next != null && t.Next.IsValue("ЖЕ", null)) 
                        res.EndToken = t.Next;
                    return res;
                }
                if (((term == "СМ" || term == "CM" || term == "НАПР") || term == "НАПРИМЕР" || term == "SEE") || term == "ПОДРОБНЕЕ" || term == "ПОДРОБНО") 
                {
                    BookLinkToken res = new BookLinkToken(t, t) { Typ = BookLinkTyp.See };
                    for (t = t.Next; t != null; t = t.Next) 
                    {
                        if (t.IsCharOf(".:") || t.IsValue("ALSO", null)) 
                        {
                            res.EndToken = t;
                            continue;
                        }
                        if (t.IsValue("В", null) || t.IsValue("IN", null)) 
                        {
                            res.EndToken = t;
                            continue;
                        }
                        BookLinkToken vvv = _tryParse(t, lev + 1);
                        if (vvv != null && vvv.Typ == BookLinkTyp.See) 
                        {
                            res.EndToken = vvv.EndToken;
                            break;
                        }
                        break;
                    }
                    return res;
                }
                if (term == "БОЛЕЕ") 
                {
                    BookLinkToken vvv = _tryParse(t.Next, lev + 1);
                    if (vvv != null && vvv.Typ == BookLinkTyp.See) 
                    {
                        vvv.BeginToken = t;
                        return vvv;
                    }
                }
                Pullenti.Ner.Token no = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t);
                if (no is Pullenti.Ner.NumberToken) 
                    return new BookLinkToken(t, no) { Typ = BookLinkTyp.N };
                if (((term == "B" || term == "В")) && (t.Next is Pullenti.Ner.NumberToken) && (t.Next.Next is Pullenti.Ner.TextToken)) 
                {
                    string term2 = (t.Next.Next as Pullenti.Ner.TextToken).Term;
                    if (((term2 == "Т" || term2 == "T" || term2.StartsWith("ТОМ")) || term2 == "TT" || term2 == "ТТ") || term2 == "КН" || term2.StartsWith("КНИГ")) 
                        return new BookLinkToken(t, t.Next.Next) { Typ = BookLinkTyp.Volume };
                }
            }
            if (t.IsChar('(')) 
            {
                if (((t.Next is Pullenti.Ner.NumberToken) && (t.Next as Pullenti.Ner.NumberToken).IntValue != null && t.Next.Next != null) && t.Next.Next.IsChar(')')) 
                {
                    int num = (t.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                    if (num > 1900 && num <= 2040) 
                    {
                        if (num <= DateTime.Now.Year) 
                            return new BookLinkToken(t, t.Next.Next) { Typ = BookLinkTyp.Year, Value = num.ToString() };
                    }
                }
                if (((t.Next is Pullenti.Ner.ReferentToken) && (t.Next.GetReferent() is Pullenti.Ner.Date.DateReferent) && t.Next.Next != null) && t.Next.Next.IsChar(')')) 
                {
                    int num = (t.Next.GetReferent() as Pullenti.Ner.Date.DateReferent).Year;
                    if (num > 0) 
                        return new BookLinkToken(t, t.Next.Next) { Typ = BookLinkTyp.Year, Value = num.ToString() };
                }
            }
            return null;
        }
        public static bool CheckLinkBefore(Pullenti.Ner.Token t0, string num)
        {
            if (num == null || t0 == null) 
                return false;
            int nn;
            if (t0.Previous != null && (t0.Previous.GetReferent() is Pullenti.Ner.Booklink.BookLinkRefReferent)) 
            {
                if (int.TryParse((t0.Previous.GetReferent() as Pullenti.Ner.Booklink.BookLinkRefReferent).Number ?? "", out nn)) 
                {
                    if (((nn + 1)).ToString() == num) 
                        return true;
                }
            }
            return false;
        }
        public static bool CheckLinkAfter(Pullenti.Ner.Token t1, string num)
        {
            if (num == null || t1 == null) 
                return false;
            if (t1.IsNewlineAfter) 
            {
                BookLinkToken bbb = BookLinkToken.TryParse(t1.Next, 0);
                int nn;
                if (bbb != null && bbb.Typ == BookLinkTyp.Number) 
                {
                    if (int.TryParse(bbb.Value ?? "", out nn)) 
                    {
                        if (((nn - 1)).ToString() == num) 
                            return true;
                    }
                }
            }
            return false;
        }
        public static void Initialize()
        {
            if (m_Termins != null) 
                return;
            m_Termins = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin tt;
            tt = new Pullenti.Ner.Core.Termin("ТЕКСТ") { Tag = BookLinkTyp.NameTail };
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("ЭЛЕКТРОННЫЙ РЕСУРС") { Tag = BookLinkTyp.ElectronRes };
            tt.AddVariant("ЕЛЕКТРОННИЙ РЕСУРС", false);
            tt.AddVariant("MODE OF ACCESS", false);
            tt.AddVariant("URL", false);
            tt.AddVariant("URLS", false);
            tt.AddVariant("ELECTRONIC RESOURCE", false);
            tt.AddVariant("ON LINE", false);
            tt.AddVariant("ONLINE", false);
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("РЕЖИМ ДОСТУПА") { Tag = BookLinkTyp.Misc };
            tt.AddVariant("РЕЖИМ ДОСТУПУ", false);
            tt.AddVariant("AVAILABLE", false);
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("МОНОГРАФИЯ") { Tag = BookLinkTyp.Type };
            tt.AddVariant("МОНОГРАФІЯ", false);
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("УЧЕБНОЕ ПОСОБИЕ") { Tag = BookLinkTyp.Type };
            tt.AddAbridge("УЧ.ПОСОБИЕ");
            tt.AddAbridge("УЧЕБ.");
            tt.AddAbridge("УЧЕБН.");
            tt.AddVariant("УЧЕБНИК", false);
            tt.AddVariant("ПОСОБИЕ", false);
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("НАВЧАЛЬНИЙ ПОСІБНИК") { Tag = BookLinkTyp.Type, Lang = Pullenti.Morph.MorphLang.UA };
            tt.AddAbridge("НАВЧ.ПОСІБНИК");
            tt.AddAbridge("НАВЧ.ПОСІБ");
            tt.AddVariant("ПІДРУЧНИК", false);
            tt.AddVariant("ПІДРУЧ", false);
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("АВТОРЕФЕРАТ") { Tag = BookLinkTyp.Type };
            tt.AddAbridge("АВТОРЕФ.");
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("ДИССЕРТАЦИЯ") { Tag = BookLinkTyp.Type };
            tt.AddVariant("ДИСС", false);
            tt.AddAbridge("ДИС.");
            tt.AddVariant("ДИСЕРТАЦІЯ", false);
            tt.AddVariant("DISSERTATION", false);
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("ДОКЛАД") { Tag = BookLinkTyp.Type };
            tt.AddVariant("ДОКЛ", false);
            tt.AddAbridge("ДОКЛ.");
            tt.AddVariant("ДОПОВІДЬ", false);
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("ПОД РЕДАКЦИЕЙ") { Tag = BookLinkTyp.Editors };
            tt.AddAbridge("ПОД РЕД");
            tt.AddAbridge("ОТВ.РЕД");
            tt.AddAbridge("ОТВ.РЕДАКТОР");
            tt.AddVariant("ПОД ОБЩЕЙ РЕДАКЦИЕЙ", false);
            tt.AddAbridge("ОТВ.РЕД");
            tt.AddAbridge("ОТВ.РЕДАКТОР");
            tt.AddAbridge("ПОД ОБЩ. РЕД");
            tt.AddAbridge("ПОД ОБЩЕЙ РЕД");
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("ПІД РЕДАКЦІЄЮ") { Tag = BookLinkTyp.Editors, Lang = Pullenti.Morph.MorphLang.UA };
            tt.AddAbridge("ПІД РЕД");
            tt.AddAbridge("ОТВ.РЕД");
            tt.AddAbridge("ВІД. РЕДАКТОР");
            tt.AddVariant("ЗА ЗАГ.РЕД", false);
            tt.AddAbridge("ВІДПОВІДАЛЬНИЙ РЕДАКТОР");
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("СОСТАВИТЕЛЬ") { Tag = BookLinkTyp.Sostavitel };
            tt.AddAbridge("СОСТ.");
            m_Termins.Add(tt);
            tt = new Pullenti.Ner.Core.Termin("УКЛАДАЧ") { Tag = BookLinkTyp.Sostavitel, Lang = Pullenti.Morph.MorphLang.UA };
            tt.AddAbridge("УКЛ.");
            m_Termins.Add(tt);
            foreach (string s in new string[] {"Политиздат", "Прогресс", "Мысль", "Просвещение", "Наука", "Физматлит", "Физматкнига", "Инфра-М", "Питер", "Интеллект", "Аспект пресс", "Аспект-пресс", "АСВ", "Радиотехника", "Радио и связь", "Лань", "Академия", "Академкнига", "URSS", "Академический проект", "БИНОМ", "БВХ", "Вильямс", "Владос", "Волтерс Клувер", "Wolters Kluwer", "Восток-Запад", "Высшая школа", "ГЕО", "Дашков и К", "Кнорус", "Когито-Центр", "КолосС", "Проспект", "РХД", "Статистика", "Финансы и статистика", "Флинта", "Юнити-дана"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s.ToUpper()) { Tag = BookLinkTyp.Press });
            }
            tt = new Pullenti.Ner.Core.Termin("ИЗДАТЕЛЬСТВО") { Tag = BookLinkTyp.Press };
            tt.AddAbridge("ИЗ-ВО");
            tt.AddAbridge("ИЗД-ВО");
            tt.AddAbridge("ИЗДАТ-ВО");
            tt.AddVariant("ISSN", false);
            tt.AddVariant("PRESS", false);
            tt.AddVariant("VERLAG", false);
            tt.AddVariant("JOURNAL", false);
            m_Termins.Add(tt);
        }
        static Pullenti.Ner.Core.TerminCollection m_Termins;
        public static Pullenti.Ner.Token ParseStartOfLitBlock(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Core.Internal.BlockLine bl = Pullenti.Ner.Core.Internal.BlockLine.Create(t, null);
            if (bl != null && bl.Typ == Pullenti.Ner.Core.Internal.BlkTyps.Literature) 
                return bl.EndToken;
            return null;
        }
    }
}