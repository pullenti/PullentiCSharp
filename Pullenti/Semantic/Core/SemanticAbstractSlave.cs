/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic.Core
{
    public class SemanticAbstractSlave : Pullenti.Ner.MetaToken
    {
        public SemanticAbstractSlave(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public string Preposition;
        public Pullenti.Ner.MetaToken Source;
        public static SemanticAbstractSlave CreateFromNoun(Pullenti.Ner.Core.NounPhraseToken npt)
        {
            SemanticAbstractSlave res = new SemanticAbstractSlave(npt.BeginToken, npt.EndToken);
            if (npt.Preposition != null) 
                res.Preposition = npt.Preposition.Normal;
            res.Morph = npt.Morph;
            res.Source = npt;
            return res;
        }
        public override string ToString()
        {
            if (Preposition != null) 
                return string.Format("{0}: {1}", Preposition, this.GetSourceText());
            return this.GetSourceText();
        }
        public bool HasPronoun
        {
            get
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Source as Pullenti.Ner.Core.NounPhraseToken;
                if (npt == null) 
                    return false;
                foreach (Pullenti.Ner.MetaToken a in npt.Adjectives) 
                {
                    if (a.BeginToken.Morph.Class.IsPronoun) 
                        return true;
                }
                return false;
            }
        }
    }
}