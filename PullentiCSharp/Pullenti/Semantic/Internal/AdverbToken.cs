/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Internal
{
    class AdverbToken : Pullenti.Ner.MetaToken
    {
        public AdverbToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public Pullenti.Semantic.SemAttributeType Typ = Pullenti.Semantic.SemAttributeType.Undefined;
        public bool Not;
        public string Spelling
        {
            get
            {
                if (m_Spelling != null) 
                    return m_Spelling;
                return Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(this, Pullenti.Ner.Core.GetTextAttr.No);
            }
            set
            {
                m_Spelling = value;
            }
        }
        string m_Spelling;
        public override string ToString()
        {
            if (Typ == Pullenti.Semantic.SemAttributeType.Undefined) 
                return Spelling;
            return string.Format("{0}: {1}{2}", Typ, (Not ? "НЕ " : ""), Spelling);
        }
        public static AdverbToken TryParse(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if ((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).Term == "НЕ") 
            {
                AdverbToken nn = TryParse(t.Next);
                if (nn != null) 
                {
                    nn.Not = true;
                    nn.BeginToken = t;
                    return nn;
                }
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1;
            if (t.Next != null && t.Morph.Class.IsPreposition) 
                t = t.Next;
            if (t.IsValue("ДРУГ", null) || t.IsValue("САМ", null)) 
            {
                t1 = t.Next;
                if (t1 != null && t1.Morph.Class.IsPreposition) 
                    t1 = t1.Next;
                if (t1 != null) 
                {
                    if (t1.IsValue("ДРУГ", null) && t.IsValue("ДРУГ", null)) 
                        return new AdverbToken(t0, t1) { Typ = Pullenti.Semantic.SemAttributeType.EachOther };
                    if (t1.IsValue("СЕБЯ", null) && t.IsValue("САМ", null)) 
                        return new AdverbToken(t0, t1) { Typ = Pullenti.Semantic.SemAttributeType.Himelf };
                }
            }
            Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
            {
                AdverbToken res = new AdverbToken(t0, tok.EndToken) { Typ = (Pullenti.Semantic.SemAttributeType)tok.Termin.Tag };
                t = res.EndToken.Next;
                if (t != null && t.IsComma) 
                    t = t.Next;
                if (res.Typ == Pullenti.Semantic.SemAttributeType.Less || res.Typ == Pullenti.Semantic.SemAttributeType.Great) 
                {
                    if (t != null && t.IsValue("ЧЕМ", null)) 
                        res.EndToken = t;
                }
                return res;
            }
            Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
            if (mc.IsAdverb) 
                return new AdverbToken(t, t);
            if (t.IsValue("ВСТРЕЧА", null) && t.Previous != null && t.Previous.IsValue("НА", null)) 
            {
                AdverbToken ne = TryParse(t.Next);
                if (ne != null && ne.Typ == Pullenti.Semantic.SemAttributeType.EachOther) 
                    return new AdverbToken(t.Previous, t);
            }
            return null;
        }
        static Pullenti.Ner.Core.TerminCollection m_Termins;
        public static void Initialize()
        {
            if (m_Termins != null) 
                return;
            m_Termins = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("ЕЩЕ") { Tag = Pullenti.Semantic.SemAttributeType.Still };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("УЖЕ") { Tag = Pullenti.Semantic.SemAttributeType.Already };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВСЕ") { Tag = Pullenti.Semantic.SemAttributeType.All };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЛЮБОЙ") { Tag = Pullenti.Semantic.SemAttributeType.Any };
            t.AddVariant("ЛЮБОЙ", false);
            t.AddVariant("КАЖДЫЙ", false);
            t.AddVariant("ЧТО УГОДНО", false);
            t.AddVariant("ВСЯКИЙ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("НЕКОТОРЫЙ") { Tag = Pullenti.Semantic.SemAttributeType.Some };
            t.AddVariant("НЕКИЙ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДРУГОЙ") { Tag = Pullenti.Semantic.SemAttributeType.Other };
            t.AddVariant("ИНОЙ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВЕСЬ") { Tag = Pullenti.Semantic.SemAttributeType.Whole };
            t.AddVariant("ЦЕЛИКОМ", false);
            t.AddVariant("ПОЛНОСТЬЮ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОЧЕНЬ") { Tag = Pullenti.Semantic.SemAttributeType.Very };
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("МЕНЬШЕ") { Tag = Pullenti.Semantic.SemAttributeType.Less };
            t.AddVariant("МЕНЕЕ", false);
            t.AddVariant("МЕНЕЕ", false);
            t.AddVariant("МЕНЬШЕ", false);
            m_Termins.Add(t);
            t = new Pullenti.Ner.Core.Termin("БОЛЬШЕ") { Tag = Pullenti.Semantic.SemAttributeType.Great };
            t.AddVariant("БОЛЕЕ", false);
            t.AddVariant("СВЫШЕ", false);
            m_Termins.Add(t);
        }
    }
}