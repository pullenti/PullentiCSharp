/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Internal
{
    public class DelimToken : Pullenti.Ner.MetaToken
    {
        public DelimToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public DelimType Typ = DelimType.Undefined;
        public bool Doublt;
        public override string ToString()
        {
            return string.Format("{0}{1}: {2}", Typ, (Doublt ? "?" : ""), base.ToString());
        }
        public static DelimToken TryParse(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            if (t.IsCommaAnd) 
            {
                DelimToken res0 = TryParse(t.Next);
                if (res0 != null) 
                {
                    res0.BeginToken = t;
                    return res0;
                }
                return null;
            }
            Pullenti.Ner.Core.TerminToken tok = m_Onto.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
            {
                DelimToken res = new DelimToken(t, tok.EndToken);
                res.Typ = (DelimType)tok.Termin.Tag;
                res.Doublt = tok.Termin.Tag2 != null;
                DelimToken res2 = TryParse(res.EndToken.Next);
                if (res2 != null) 
                {
                    if (res2.Typ == res.Typ) 
                    {
                        res.EndToken = res2.EndToken;
                        res.Doublt = false;
                    }
                }
                if (t.Morph.Class.IsPronoun) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParseAdverbs, 0, null);
                    if (npt != null && npt.EndChar > res.EndChar) 
                        return null;
                }
                return res;
            }
            return null;
        }
        static Pullenti.Ner.Core.TerminCollection m_Onto;
        public static void Initialize()
        {
            m_Onto = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("НО") { Tag = DelimType.But };
            t.AddVariant("А", false);
            t.AddVariant("ОДНАКО", false);
            t.AddVariant("ХОТЯ", false);
            m_Onto.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЕСЛИ") { Tag = DelimType.If };
            t.AddVariant("В СЛУЧАЕ ЕСЛИ", false);
            m_Onto.Add(t);
            t = new Pullenti.Ner.Core.Termin("КОГДА") { Tag = DelimType.If, Tag2 = m_Onto };
            m_Onto.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТО") { Tag = DelimType.Then };
            t.AddVariant("ТОГДА", false);
            m_Onto.Add(t);
            t = new Pullenti.Ner.Core.Termin("ИНАЧЕ") { Tag = DelimType.Else };
            t.AddVariant("В ПРОТИВНОМ СЛУЧАЕ", false);
            m_Onto.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТАК КАК") { Tag = DelimType.Because };
            t.AddVariant("ПОТОМУ ЧТО", false);
            t.AddVariant("ПО ПРИЧИНЕ ТОГО ЧТО", false);
            t.AddVariant("ИЗ ЗА ТОГО ЧТО", false);
            t.AddVariant("ИЗЗА ТОГО ЧТО", false);
            t.AddVariant("ИЗ-ЗА ТОГО ЧТО", false);
            t.AddVariant("ТО ЕСТЬ", false);
            m_Onto.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЧТОБЫ") { Tag = DelimType.For };
            t.AddVariant("ДЛЯ ТОГО ЧТОБЫ", false);
            m_Onto.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЧТО") { Tag = DelimType.What };
            m_Onto.Add(t);
        }
    }
}