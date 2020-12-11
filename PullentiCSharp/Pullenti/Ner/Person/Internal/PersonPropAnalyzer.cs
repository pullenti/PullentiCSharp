/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Person.Internal
{
    class PersonPropAnalyzer : Pullenti.Ner.Analyzer
    {
        public PersonPropAnalyzer() : base()
        {
            IgnoreThisAnalyzer = true;
        }
        public const string ANALYZER_NAME = "PERSONPROPERTY";
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
                return "Используется внутренним образом";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new PersonPropAnalyzer();
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, Pullenti.Ner.Token end)
        {
            PersonAttrToken pat = PersonAttrToken.TryAttach(begin, null, PersonAttrToken.PersonAttrAttachAttrs.InProcess);
            if (pat != null && pat.PropRef != null) 
                return new Pullenti.Ner.ReferentToken(pat.PropRef, pat.BeginToken, pat.EndToken) { Morph = pat.Morph, Tag = pat };
            return null;
        }
    }
}