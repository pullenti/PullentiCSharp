/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Titlepage.Internal
{
    class PersonRelations
    {
        public List<PersonRelation> Rels = new List<PersonRelation>();
        public void Add(Pullenti.Ner.Person.PersonReferent pers, TitleItemToken.Types typ, float coef)
        {
            PersonRelation r = null;
            foreach (PersonRelation rr in Rels) 
            {
                if (rr.Person == pers) 
                {
                    r = rr;
                    break;
                }
            }
            if (r == null) 
                Rels.Add((r = new PersonRelation() { Person = pers }));
            if (!r.Coefs.ContainsKey(typ)) 
                r.Coefs.Add(typ, coef);
            else 
                r.Coefs[typ] += coef;
        }
        public List<Pullenti.Ner.Person.PersonReferent> GetPersons(TitleItemToken.Types typ)
        {
            List<Pullenti.Ner.Person.PersonReferent> res = new List<Pullenti.Ner.Person.PersonReferent>();
            foreach (PersonRelation v in Rels) 
            {
                if (v.Best == typ) 
                    res.Add(v.Person);
            }
            return res;
        }
        public List<TitleItemToken.Types> RelTypes
        {
            get
            {
                List<TitleItemToken.Types> res = new List<TitleItemToken.Types>();
                res.Add(TitleItemToken.Types.Worker);
                res.Add(TitleItemToken.Types.Boss);
                res.Add(TitleItemToken.Types.Editor);
                res.Add(TitleItemToken.Types.Opponent);
                res.Add(TitleItemToken.Types.Consultant);
                res.Add(TitleItemToken.Types.Adopt);
                res.Add(TitleItemToken.Types.Translate);
                return res;
            }
        }
        public string GetAttrNameForType(TitleItemToken.Types typ)
        {
            if (typ == TitleItemToken.Types.Worker) 
                return Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_AUTHOR;
            if (typ == TitleItemToken.Types.Boss) 
                return Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_SUPERVISOR;
            if (typ == TitleItemToken.Types.Editor) 
                return Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_EDITOR;
            if (typ == TitleItemToken.Types.Opponent) 
                return Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_OPPONENT;
            if (typ == TitleItemToken.Types.Consultant) 
                return Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_CONSULTANT;
            if (typ == TitleItemToken.Types.Adopt) 
                return Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_AFFIRMANT;
            if (typ == TitleItemToken.Types.Translate) 
                return Pullenti.Ner.Titlepage.TitlePageReferent.ATTR_TRANSLATOR;
            return null;
        }
        public TitleItemToken.Types CalcTypFromAttrs(Pullenti.Ner.Person.PersonReferent pers)
        {
            foreach (Pullenti.Ner.Slot a in pers.Slots) 
            {
                if (a.TypeName == Pullenti.Ner.Person.PersonReferent.ATTR_ATTR) 
                {
                    string s = a.Value.ToString();
                    if (s.Contains("руководител")) 
                        return TitleItemToken.Types.Boss;
                    if (s.Contains("студент") || s.Contains("слушател")) 
                        return TitleItemToken.Types.Worker;
                    if (s.Contains("редактор") || s.Contains("рецензент")) 
                        return TitleItemToken.Types.Editor;
                    if (s.Contains("консультант")) 
                        return TitleItemToken.Types.Consultant;
                    if (s.Contains("исполнитель")) 
                        return TitleItemToken.Types.Worker;
                }
            }
            return TitleItemToken.Types.Undefined;
        }
    }
}