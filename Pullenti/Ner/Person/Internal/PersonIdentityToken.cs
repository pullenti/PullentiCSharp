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

namespace Pullenti.Ner.Person.Internal
{
    class PersonIdentityToken : Pullenti.Ner.MetaToken
    {
        public PersonIdentityToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public float Coef;
        public PersonMorphCollection Firstname;
        public PersonMorphCollection Lastname;
        public object Middlename;
        public Pullenti.Ner.Person.PersonReferent OntologyPerson;
        public Pullenti.Morph.MorphGender ProbableGender
        {
            get
            {
                if (Morph.Gender == Pullenti.Morph.MorphGender.Feminie || Morph.Gender == Pullenti.Morph.MorphGender.Masculine) 
                    return Morph.Gender;
                int fem = 0;
                int mus = 0;
                for (int i = 0; i < 2; i++) 
                {
                    PersonMorphCollection col = (i == 0 ? Firstname : Lastname);
                    if (col == null) 
                        continue;
                    bool isf = false;
                    bool ism = false;
                    foreach (PersonMorphCollection.PersonMorphVariant v in col.Items) 
                    {
                        if (((v.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                            ism = true;
                        if (((v.Gender & Pullenti.Morph.MorphGender.Feminie)) != Pullenti.Morph.MorphGender.Undefined) 
                            isf = true;
                    }
                    if (ism) 
                        mus++;
                    if (isf) 
                        fem++;
                }
                if (mus > fem) 
                    return Pullenti.Morph.MorphGender.Masculine;
                if (fem > mus) 
                    return Pullenti.Morph.MorphGender.Feminie;
                return Pullenti.Morph.MorphGender.Undefined;
            }
        }
        public FioTemplateType Typ = FioTemplateType.Undefined;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0} {1}: {2}", Coef, Typ.ToString(), (Lastname == null ? "" : Lastname.ToString()));
            res.AppendFormat(" {0} {1}; {2}", (Firstname == null ? "" : Firstname.ToString()), (Middlename == null ? "" : Middlename.ToString()), Morph.ToString());
            return res.ToString();
        }
        public static PersonMorphCollection CreateLastname(PersonItemToken pit, Pullenti.Morph.MorphBaseInfo inf)
        {
            PersonMorphCollection res = new PersonMorphCollection();
            if (pit.Lastname == null) 
                SetValue(res, pit.BeginToken, inf);
            else 
                SetValue2(res, pit.Lastname, inf);
            return res;
        }
        public static Pullenti.Ner.Person.PersonReferent TryAttachLatinSurname(PersonItemToken pit, Pullenti.Ner.Core.IntOntologyCollection ontos)
        {
            if (pit == null) 
                return null;
            if (pit.Lastname != null && ((pit.Lastname.IsInDictionary || pit.Lastname.IsLastnameHasStdTail))) 
            {
                Pullenti.Ner.Person.PersonReferent p = new Pullenti.Ner.Person.PersonReferent();
                p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, pit.Lastname.Vars[0].Value, false, 0);
                return p;
            }
            return null;
        }
        public static Pullenti.Ner.Person.PersonReferent TryAttachOntoForSingle(PersonItemToken pit, Pullenti.Ner.Core.IntOntologyCollection ontos)
        {
            if ((pit == null || ontos == null || pit.Value == null) || pit.Typ == PersonItemToken.ItemType.Initial) 
                return null;
            if (ontos.Items.Count > 30) 
                return null;
            Pullenti.Ner.Person.PersonReferent p0 = null;
            int cou = 0;
            bool fi = false;
            bool sur = true;
            foreach (Pullenti.Ner.Core.IntOntologyItem p in ontos.Items) 
            {
                if (p.Referent is Pullenti.Ner.Person.PersonReferent) 
                {
                    Pullenti.Ner.Person.PersonReferent p00 = null;
                    if (pit.Firstname != null) 
                    {
                        foreach (PersonItemToken.MorphPersonItemVariant v in pit.Firstname.Vars) 
                        {
                            if (p.Referent.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, v.Value, true) != null) 
                            {
                                p00 = p.Referent as Pullenti.Ner.Person.PersonReferent;
                                fi = true;
                                break;
                            }
                        }
                    }
                    if (pit.Lastname != null) 
                    {
                        foreach (PersonItemToken.MorphPersonItemVariant v in pit.Lastname.Vars) 
                        {
                            if (p.Referent.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, v.Value, true) != null) 
                            {
                                p00 = p.Referent as Pullenti.Ner.Person.PersonReferent;
                                sur = true;
                                break;
                            }
                        }
                    }
                    if (p00 == null) 
                    {
                        if (p.Referent.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, pit.Value, true) != null) 
                        {
                            p00 = p.Referent as Pullenti.Ner.Person.PersonReferent;
                            fi = true;
                        }
                        else if (p.Referent.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME, pit.Value, true) != null) 
                        {
                            p00 = p.Referent as Pullenti.Ner.Person.PersonReferent;
                            sur = true;
                        }
                    }
                    if (p00 != null) 
                    {
                        p0 = p00;
                        cou++;
                    }
                }
            }
            if (p0 != null && cou == 1) 
            {
                if (fi) 
                {
                    List<PersonItemToken> li = new List<PersonItemToken>();
                    li.Add(pit);
                    PersonIdentityToken king = TryAttachKing(li, 0, pit.Morph, false);
                    if (king != null) 
                        return null;
                }
                return p0;
            }
            return null;
        }
        public static Pullenti.Ner.Person.PersonReferent TryAttachOntoForDuble(PersonItemToken pit0, PersonItemToken pit1, Pullenti.Ner.Core.IntOntologyCollection ontos)
        {
            if ((pit0 == null || pit0.Firstname == null || pit1 == null) || pit1.Middlename == null || ontos == null) 
                return null;
            if (ontos.Items.Count > 100) 
                return null;
            Pullenti.Ner.Person.PersonReferent p0 = null;
            int cou = 0;
            foreach (Pullenti.Ner.Core.IntOntologyItem p in ontos.Items) 
            {
                if (p.Referent != null) 
                {
                    foreach (PersonItemToken.MorphPersonItemVariant v in pit0.Firstname.Vars) 
                    {
                        if (p.Referent.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, v.Value, true) == null) 
                            continue;
                        if (p.Referent.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME, pit1.Middlename.Vars[0].Value, true) == null) 
                            continue;
                        p0 = p.Referent as Pullenti.Ner.Person.PersonReferent;
                        cou++;
                        break;
                    }
                }
            }
            if (p0 != null && cou == 1) 
                return p0;
            return null;
        }
        public static PersonIdentityToken TryAttachOntoExt(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, Pullenti.Ner.ExtOntology ontos)
        {
            if (ind >= pits.Count || pits[ind].Typ == PersonItemToken.ItemType.Initial || ontos == null) 
                return null;
            if (ontos.Items.Count > 1000) 
                return null;
            List<Pullenti.Ner.Core.IntOntologyToken> otl = ontos.AttachToken(Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME, pits[ind].BeginToken);
            return _TryAttachOnto(pits, ind, inf, otl, false, false);
        }
        public static PersonIdentityToken TryAttachOntoInt(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, Pullenti.Ner.Core.IntOntologyCollection ontos)
        {
            if (ind >= pits.Count || pits[ind].Typ == PersonItemToken.ItemType.Initial) 
                return null;
            if (ontos.Items.Count > 1000) 
                return null;
            List<Pullenti.Ner.Core.IntOntologyToken> otl = ontos.TryAttach(pits[ind].BeginToken, null, false);
            PersonIdentityToken res = _TryAttachOnto(pits, ind, inf, otl, false, false);
            if (res != null) 
                return res;
            return null;
        }
        static PersonIdentityToken _TryAttachOnto(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, List<Pullenti.Ner.Core.IntOntologyToken> otl, bool isLocal, bool isAttrBefore)
        {
            if (otl == null || otl.Count == 0) 
                return null;
            List<PersonIdentityToken> res = new List<PersonIdentityToken>();
            List<Pullenti.Ner.Person.PersonReferent> ontoPersons = new List<Pullenti.Ner.Person.PersonReferent>();
            if (otl != null) 
            {
                foreach (Pullenti.Ner.Core.IntOntologyToken ot in otl) 
                {
                    if (ot.EndToken == pits[ind].EndToken) 
                    {
                        Pullenti.Ner.Person.PersonReferent pers = ot.Item.Referent as Pullenti.Ner.Person.PersonReferent;
                        if (pers == null) 
                            continue;
                        if (ontoPersons.Contains(pers)) 
                            continue;
                        PersonIdentityToken pit;
                        if (ot.Termin.IgnoreTermsOrder) 
                        {
                            if (ind != 0) 
                                continue;
                            pit = TryAttachIdentity(pits, inf);
                            if (pit == null) 
                                continue;
                            Pullenti.Ner.Person.PersonReferent p = new Pullenti.Ner.Person.PersonReferent();
                            p.AddIdentity(pit.Lastname);
                            pit.OntologyPerson = p;
                            ontoPersons.Add(pers);
                            res.Add(pit);
                            continue;
                        }
                        if (inf.Gender == Pullenti.Morph.MorphGender.Masculine) 
                        {
                            if (pers.IsFemale) 
                                continue;
                        }
                        else if (inf.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        {
                            if (pers.IsMale) 
                                continue;
                        }
                        Pullenti.Morph.MorphBaseInfo inf0 = new Pullenti.Morph.MorphBaseInfo() { Case = inf.Case, Gender = inf.Gender };
                        if (!ot.Morph.Case.IsUndefined && inf0.Case == Pullenti.Morph.MorphCase.AllCases && ot.BeginToken == ot.EndToken) 
                            inf0.Case = ot.Morph.Case;
                        if (pers.IsMale) 
                            inf0.Gender = Pullenti.Morph.MorphGender.Masculine;
                        else if (pers.IsFemale) 
                            inf0.Gender = Pullenti.Morph.MorphGender.Feminie;
                        List<PersonIdentityToken> vars = new List<PersonIdentityToken>();
                        if (ind > 1) 
                        {
                            if ((((pit = TryAttachIISurname(pits, ind - 2, inf0)))) != null) 
                                vars.Add(pit);
                            if ((((pit = TryAttachNameSecnameSurname(pits, ind - 2, inf0, false)))) != null) 
                                vars.Add(pit);
                        }
                        if (ind > 0) 
                        {
                            if ((((pit = TryAttachIISurname(pits, ind - 1, inf0)))) != null) 
                                vars.Add(pit);
                            if ((((pit = TryAttachNameSurname(pits, ind - 1, inf0, false, isAttrBefore)))) != null) 
                                vars.Add(pit);
                        }
                        if ((ind + 2) < pits.Count) 
                        {
                            if ((((pit = TryAttachSurnameII(pits, ind, inf0)))) != null) 
                                vars.Add(pit);
                            if ((((pit = TryAttachSurnameNameSecname(pits, ind, inf0, false, false)))) != null) 
                                vars.Add(pit);
                        }
                        if ((ind + 1) < pits.Count) 
                        {
                            if ((((pit = TryAttachSurnameName(pits, ind, inf0, false)))) != null) 
                            {
                                PersonIdentityToken pit0 = null;
                                foreach (PersonIdentityToken v in vars) 
                                {
                                    if (v.Typ == FioTemplateType.SurnameNameSecname) 
                                    {
                                        pit0 = v;
                                        break;
                                    }
                                }
                                if (pit0 == null || (pit0.Coef < pit.Coef)) 
                                    vars.Add(pit);
                            }
                        }
                        if ((((pit = TryAttachAsian(pits, ind, inf0, 3, false)))) != null) 
                            vars.Add(pit);
                        else if ((((pit = TryAttachAsian(pits, ind, inf0, 2, false)))) != null) 
                            vars.Add(pit);
                        pit = null;
                        foreach (PersonIdentityToken v in vars) 
                        {
                            if (v.Coef < 0) 
                                continue;
                            Pullenti.Ner.Person.PersonReferent p = new Pullenti.Ner.Person.PersonReferent();
                            if (v.OntologyPerson != null) 
                                p = v.OntologyPerson;
                            else 
                            {
                                if (v.Typ == FioTemplateType.AsianName) 
                                    pers.AddIdentity(v.Lastname);
                                else 
                                    p.AddFioIdentity(v.Lastname, v.Firstname, v.Middlename);
                                v.OntologyPerson = p;
                            }
                            if (!pers.CanBeEquals(p, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                            {
                                if (pit != null && v.Coef >= pit.Coef) 
                                    pit = null;
                            }
                            else if (pit == null) 
                                pit = v;
                            else if (pit.Coef < v.Coef) 
                                pit = v;
                        }
                        if (pit == null) 
                        {
                            pit = TryAttachSingleSurname(pits, ind, inf0);
                            if (pit == null || (pit.Coef < 2)) 
                                continue;
                            Pullenti.Ner.Person.PersonReferent p = new Pullenti.Ner.Person.PersonReferent();
                            p.AddFioIdentity(pit.Lastname, null, null);
                            pit.OntologyPerson = p;
                        }
                        ontoPersons.Add(pers);
                        res.Add(pit);
                    }
                }
            }
            if (res.Count == 0) 
                return null;
            if (res.Count == 1) 
            {
                res[0].OntologyPerson.MergeSlots(ontoPersons[0], true);
                return res[0];
            }
            return null;
        }
        public static PersonIdentityToken CreateTyp(List<PersonItemToken> pits, FioTemplateType typ, Pullenti.Morph.MorphBaseInfo inf)
        {
            if (typ == FioTemplateType.SurnameNameSecname) 
                return TryAttachSurnameNameSecname(pits, 0, inf, false, true);
            return null;
        }
        public static void Sort(List<PersonIdentityToken> li)
        {
            if (li != null && li.Count > 1) 
            {
                for (int k = 0; k < li.Count; k++) 
                {
                    bool ch = false;
                    for (int i = 0; i < (li.Count - 1); i++) 
                    {
                        if (li[i].Coef < li[i + 1].Coef) 
                        {
                            ch = true;
                            PersonIdentityToken v = li[i];
                            li[i] = li[i + 1];
                            li[i + 1] = v;
                        }
                    }
                    if (!ch) 
                        break;
                }
            }
        }
        public static List<PersonIdentityToken> TryAttachForExtOnto(List<PersonItemToken> pits)
        {
            PersonIdentityToken pit = null;
            if (pits.Count == 3) 
            {
                if (pits[0].Typ == PersonItemToken.ItemType.Value && pits[1].Typ == PersonItemToken.ItemType.Initial && pits[2].Typ == PersonItemToken.ItemType.Value) 
                {
                    pit = new PersonIdentityToken(pits[0].BeginToken, pits[2].EndToken) { Typ = FioTemplateType.NameISurname };
                    ManageFirstname(pit, pits[0], null);
                    ManageLastname(pit, pits[2], null);
                    ManageMiddlename(pit, pits[1], null);
                    pit.Coef = 2;
                }
                else if (pits[0].Typ == PersonItemToken.ItemType.Value && pits[1].Typ == PersonItemToken.ItemType.Value && pits[2].Typ == PersonItemToken.ItemType.Value) 
                {
                    bool ok = false;
                    if (pits[0].Firstname == null && pits[1].Middlename == null && ((pits[1].Firstname != null || pits[2].Middlename != null))) 
                        ok = true;
                    else if (pits[0].Firstname != null && ((pits[0].Firstname.IsLastnameHasStdTail || pits[0].Firstname.IsInDictionary))) 
                        ok = true;
                    if (ok) 
                    {
                        pit = new PersonIdentityToken(pits[0].BeginToken, pits[2].EndToken) { Typ = FioTemplateType.SurnameNameSecname };
                        ManageFirstname(pit, pits[1], null);
                        ManageLastname(pit, pits[0], null);
                        ManageMiddlename(pit, pits[2], null);
                        pit.Coef = 2;
                    }
                }
            }
            else if (pits.Count == 2 && pits[0].Typ == PersonItemToken.ItemType.Value && pits[1].Typ == PersonItemToken.ItemType.Value) 
            {
                PersonItemToken nam = null;
                PersonItemToken sur = null;
                for (int i = 0; i < 2; i++) 
                {
                    if (((pits[i].Firstname != null && pits[i].Firstname.IsInDictionary)) || ((pits[i ^ 1].Lastname != null && ((pits[i ^ 1].Lastname.IsInDictionary || pits[i ^ 1].Lastname.IsLastnameHasStdTail))))) 
                    {
                        nam = pits[i];
                        sur = pits[i ^ 1];
                        break;
                    }
                }
                if (nam != null) 
                {
                    pit = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken) { Typ = (nam == pits[0] ? FioTemplateType.NameSurname : FioTemplateType.SurnameName) };
                    ManageFirstname(pit, nam, null);
                    ManageLastname(pit, sur, null);
                    pit.Coef = 2;
                }
            }
            if (pit == null) 
                return null;
            List<PersonIdentityToken> res = new List<PersonIdentityToken>();
            res.Add(pit);
            return res;
        }
        public static List<PersonIdentityToken> TryAttach(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, Pullenti.Ner.Token firstTok, bool king, bool isAttrBefore)
        {
            List<PersonIdentityToken> res = new List<PersonIdentityToken>();
            FioTemplateType ty = FioTemplateType.Undefined;
            if (firstTok != null) 
            {
                for (Pullenti.Ner.Token t = firstTok.Previous; t != null; t = t.Previous) 
                {
                    Pullenti.Ner.Person.PersonReferent pf = t.GetReferent() as Pullenti.Ner.Person.PersonReferent;
                    if (pf != null) 
                    {
                        ty = pf.m_PersonIdentityTyp;
                        break;
                    }
                    if (t.IsNewlineBefore) 
                        break;
                    if (t.Chars.IsLetter && !t.IsAnd) 
                        break;
                }
            }
            PersonIdentityToken pit;
            PersonIdentityToken pit1;
            if ((((pit = TryAttachGlobal(pits, ind, inf)))) != null) 
            {
                res.Add(pit);
                return res;
            }
            if ((((pit = TryAttachSurnameII(pits, ind, inf)))) != null) 
                res.Add(pit);
            if ((((pit = TryAttachIISurname(pits, ind, inf)))) != null) 
                res.Add(pit);
            if ((((pit = TryAttachAsian(pits, ind, inf, 3, ty == FioTemplateType.AsianName)))) != null) 
                res.Add(pit);
            else 
            {
                if ((((pit = TryAttachNameSurname(pits, ind, inf, ty == FioTemplateType.NameSurname, isAttrBefore)))) != null) 
                    res.Add(pit);
                if ((((pit1 = TryAttachSurnameName(pits, ind, inf, ty == FioTemplateType.SurnameName)))) != null) 
                {
                    res.Add(pit1);
                    if (pit != null && (pit.Coef + 1) >= pit1.Coef && ty != FioTemplateType.SurnameName) 
                        pit1.Coef -= ((float)0.5);
                }
                if ((((pit = TryAttachNameSecnameSurname(pits, ind, inf, ty == FioTemplateType.NameSecnameSurname)))) != null) 
                    res.Add(pit);
                if ((((pit = TryAttachSurnameNameSecname(pits, ind, inf, ty == FioTemplateType.SurnameNameSecname, false)))) != null) 
                    res.Add(pit);
                if ((((pit = TryAttachAsian(pits, ind, inf, 2, ty == FioTemplateType.AsianName)))) != null) 
                    res.Add(pit);
            }
            if (king) 
            {
                if ((((pit = TryAttachNameSecname(pits, ind, inf, ty == FioTemplateType.NameSecname)))) != null) 
                {
                    res.Add(pit);
                    foreach (PersonIdentityToken r in res) 
                    {
                        if (r.Typ == FioTemplateType.NameSurname) 
                            r.Coef = pit.Coef - 1;
                    }
                }
            }
            if ((((pit = TryAttachKing(pits, ind, inf, ty == FioTemplateType.King || king)))) != null) 
                res.Add(pit);
            if (inf.Gender == Pullenti.Morph.MorphGender.Masculine || inf.Gender == Pullenti.Morph.MorphGender.Feminie) 
            {
                foreach (PersonIdentityToken p in res) 
                {
                    if (p.Morph.Gender == Pullenti.Morph.MorphGender.Undefined || p.Morph.Gender == ((Pullenti.Morph.MorphGender.Feminie | Pullenti.Morph.MorphGender.Masculine))) 
                    {
                        p.Morph.Gender = inf.Gender;
                        if (p.Morph.Case.IsUndefined) 
                            p.Morph.Case = inf.Case;
                    }
                }
            }
            foreach (PersonIdentityToken r in res) 
            {
                for (Pullenti.Ner.Token tt = r.BeginToken; tt != r.EndToken; tt = tt.Next) 
                {
                    if (tt.IsNewlineAfter) 
                        r.Coef -= 1;
                }
                Pullenti.Ner.Token ttt = r.BeginToken.Previous;
                if (ttt != null && ttt.Morph.Class == Pullenti.Morph.MorphClass.Verb) 
                {
                    Pullenti.Ner.Token tte = r.EndToken.Next;
                    if (tte == null || tte.IsChar('.') || tte.IsNewlineBefore) 
                    {
                    }
                    else 
                        continue;
                    r.Coef += 1;
                }
                if (r.Coef >= 0 && ind == 0 && r.EndToken == pits[pits.Count - 1].EndToken) 
                    r.Coef += _calcCoefAfter(pits[pits.Count - 1].EndToken.Next);
            }
            if (ty != FioTemplateType.Undefined && ind == 0) 
            {
                foreach (PersonIdentityToken r in res) 
                {
                    if (r.Typ == ty) 
                        r.Coef += ((float)1.5);
                    else if (((r.Typ == FioTemplateType.SurnameName && ty == FioTemplateType.SurnameNameSecname)) || ((r.Typ == FioTemplateType.SurnameNameSecname && ty == FioTemplateType.SurnameName))) 
                        r.Coef += ((float)0.5);
                }
            }
            Sort(res);
            return res;
        }
        public static void ManageLastname(PersonIdentityToken res, PersonItemToken pit, Pullenti.Morph.MorphBaseInfo inf)
        {
            if (pit.Lastname == null) 
            {
                res.Lastname = new PersonMorphCollection();
                SetValue(res.Lastname, pit.BeginToken, inf);
                if (pit.IsInDictionary) 
                    res.Coef--;
                Pullenti.Ner.TextToken tt = pit.BeginToken as Pullenti.Ner.TextToken;
                if ((tt != null && !tt.Chars.IsLatinLetter && tt.Chars.IsCapitalUpper) && tt.LengthChar > 2 && !tt.Chars.IsLatinLetter) 
                {
                    bool ok = true;
                    foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
                    {
                        if ((wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok) 
                        res.Coef += ((float)1);
                }
            }
            else 
            {
                res.Coef++;
                if (!IsAccords(pit.Lastname, inf)) 
                    res.Coef--;
                res.Lastname = new PersonMorphCollection();
                SetValue2(res.Lastname, pit.Lastname, inf);
                if (pit.Lastname.Term != null) 
                {
                    if (res.Morph.Case.IsUndefined || res.Morph.Case.IsNominative) 
                    {
                        if (!pit.Lastname.IsInDictionary && !res.Lastname.Values.Contains(pit.Lastname.Term)) 
                        {
                            if (inf.Case.IsNominative || inf.Case.IsUndefined) 
                            {
                                if (pit.Lastname.Morph.Class.IsAdjective && inf.Gender == Pullenti.Morph.MorphGender.Feminie) 
                                {
                                }
                                else 
                                    res.Lastname.Add(pit.Lastname.Term, null, pit.Morph.Gender, false);
                            }
                        }
                    }
                }
                if (pit.IsInDictionary) 
                    res.Coef--;
                if (pit.Lastname.IsInDictionary || pit.Lastname.IsInOntology) 
                    res.Coef++;
                if (pit.Lastname.IsLastnameHasHiphen) 
                    res.Coef += 1;
                if (pit.Middlename != null && pit.Middlename.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                    res.Coef--;
            }
            if (pit.Firstname != null && !pit.Chars.IsLatinLetter) 
                res.Coef--;
            if (pit.BeginToken is Pullenti.Ner.ReferentToken) 
                res.Coef--;
        }
        public static void ManageFirstname(PersonIdentityToken res, PersonItemToken pit, Pullenti.Morph.MorphBaseInfo inf)
        {
            if (pit.Firstname == null) 
            {
                if (pit.Lastname != null) 
                    res.Coef--;
                res.Firstname = new PersonMorphCollection();
                SetValue(res.Firstname, pit.BeginToken, inf);
                if (pit.IsInDictionary) 
                    res.Coef--;
            }
            else 
            {
                res.Coef++;
                if (!IsAccords(pit.Firstname, inf)) 
                    res.Coef--;
                res.Firstname = new PersonMorphCollection();
                SetValue2(res.Firstname, pit.Firstname, inf);
                if (pit.IsInDictionary && !pit.Firstname.IsInDictionary) 
                    res.Coef--;
            }
            if (pit.Middlename != null && pit.Middlename != pit.Firstname) 
                res.Coef--;
            if (pit.Lastname != null && ((pit.Lastname.IsInDictionary || pit.Lastname.IsInOntology))) 
                res.Coef--;
            if (pit.BeginToken is Pullenti.Ner.ReferentToken) 
                res.Coef -= 2;
        }
        static void ManageMiddlename(PersonIdentityToken res, PersonItemToken pit, Pullenti.Morph.MorphBaseInfo inf)
        {
            PersonMorphCollection mm = new PersonMorphCollection();
            res.Middlename = mm;
            if (pit.Middlename == null) 
                SetValue(mm, pit.BeginToken, inf);
            else 
            {
                res.Coef++;
                if (!IsAccords(pit.Middlename, inf)) 
                    res.Coef--;
                SetValue2(mm, pit.Middlename, inf);
            }
        }
        static PersonIdentityToken TryAttachSingleSurname(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf)
        {
            if (ind >= pits.Count || pits[ind].Lastname == null) 
                return null;
            PersonIdentityToken res = new PersonIdentityToken(pits[ind].BeginToken, pits[ind].EndToken);
            if (ind == 0 && pits.Count == 1) 
                res.Coef++;
            else 
            {
                if (ind > 0 && ((!pits[ind - 1].IsInDictionary || pits[ind - 1].Typ == PersonItemToken.ItemType.Initial || pits[ind - 1].Firstname != null))) 
                    res.Coef--;
                if (((ind + 1) < pits.Count) && ((!pits[ind + 1].IsInDictionary || pits[ind + 1].Typ == PersonItemToken.ItemType.Initial || pits[ind + 1].Firstname != null))) 
                    res.Coef--;
            }
            res.Morph = AccordMorph(inf, pits[ind].Lastname, null, null, pits[ind].EndToken.Next);
            ManageLastname(res, pits[ind], inf);
            return res;
        }
        static PersonIdentityToken TryAttachNameSurname(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, bool prevHasThisTyp = false, bool isAttrBefore = false)
        {
            if ((ind + 1) >= pits.Count || pits[ind + 1].Typ != PersonItemToken.ItemType.Value || pits[ind].Typ != PersonItemToken.ItemType.Value) 
                return null;
            if (pits[ind + 1].Lastname == null) 
            {
                if (!prevHasThisTyp) 
                {
                    if (pits[ind].Chars.IsLatinLetter) 
                    {
                    }
                    else 
                    {
                        if (pits[ind].Firstname == null || pits[ind + 1].Middlename != null) 
                            return null;
                        if (pits[ind + 1].IsNewlineAfter) 
                        {
                        }
                        else if (pits[ind + 1].EndToken.Next != null && pits[ind + 1].EndToken.Next.IsCharOf(",.)")) 
                        {
                        }
                        else 
                            return null;
                    }
                }
            }
            if (pits[ind].IsNewlineAfter || pits[ind].IsHiphenAfter) 
                return null;
            if (pits[ind + 1].Middlename != null && pits[ind + 1].Middlename.IsInDictionary && pits[ind + 1].Middlename.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                return null;
            if (IsBothSurnames(pits[ind], pits[ind + 1])) 
            {
                if (pits.Count == 2 && ind == 0 && isAttrBefore) 
                {
                }
                else 
                    return null;
            }
            PersonIdentityToken res = new PersonIdentityToken(pits[ind].BeginToken, pits[ind + 1].EndToken) { Typ = FioTemplateType.NameSurname };
            res.Coef -= ind;
            res.Morph = AccordMorph(inf, pits[ind + 1].Lastname, pits[ind].Firstname, null, pits[ind + 1].EndToken.Next);
            if (res.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || res.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
            {
                if (pits[ind + 1].Lastname != null && !pits[ind + 1].Lastname.Morph.Case.IsUndefined) 
                {
                    if ((pits[ind].Lastname != null && pits[ind].Lastname.IsLastnameHasStdTail && pits[ind + 1].Firstname != null) && pits[ind + 1].Firstname.IsInDictionary) 
                        res.Coef -= 1;
                    else 
                        res.Coef += 1;
                }
                inf = res.Morph;
            }
            ManageFirstname(res, pits[ind], inf);
            ManageLastname(res, pits[ind + 1], inf);
            if (pits[ind].Firstname != null && (pits[ind + 1].BeginToken is Pullenti.Ner.ReferentToken)) 
                res.Coef++;
            if (pits[ind].BeginToken.GetMorphClassInDictionary().IsVerb) 
            {
                if (pits[ind].BeginToken.Chars.IsCapitalUpper && !Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(pits[ind].BeginToken)) 
                {
                }
                else 
                    res.Coef -= 1;
            }
            if (pits[ind].Firstname != null && ((pits[ind + 1].IsNewlineAfter || ((pits[ind + 1].EndToken.Next != null && (pits[ind + 1].EndToken.Next.IsCharOf(",.")))))) && !pits[ind + 1].IsNewlineBefore) 
            {
                if (pits[ind + 1].Firstname == null && pits[ind + 1].Middlename == null) 
                    res.Coef++;
                else if (pits[ind + 1].Chars.IsLatinLetter && (ind + 2) == pits.Count) 
                    res.Coef++;
            }
            if (pits[ind + 1].Middlename != null) 
            {
                Pullenti.Ner.Core.StatisticWordInfo info = pits[ind].Kit.Statistics.GetWordInfo(pits[ind + 1].BeginToken);
                if (info != null && info.NotCapitalBeforeCount > 0) 
                {
                }
                else 
                {
                    res.Coef -= (1 + ind);
                    if (res.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || res.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                    {
                        if (pits[ind + 1].Lastname != null && ((pits[ind + 1].Lastname.IsInDictionary || pits[ind + 1].Lastname.IsInOntology))) 
                        {
                        }
                        else 
                            foreach (PersonItemToken.MorphPersonItemVariant v in pits[ind + 1].Middlename.Vars) 
                            {
                                if (((v.Gender & res.Morph.Gender)) != Pullenti.Morph.MorphGender.Undefined) 
                                {
                                    res.Coef -= 1;
                                    break;
                                }
                            }
                    }
                }
            }
            if (pits[ind].Chars != pits[ind + 1].Chars) 
            {
                if (pits[ind].Chars.IsCapitalUpper && pits[ind + 1].Chars.IsAllUpper) 
                {
                }
                else if (pits[ind].Chars.IsAllUpper && pits[ind + 1].Chars.IsCapitalUpper && pits[ind].Firstname == null) 
                    res.Coef -= 10;
                else 
                    res.Coef -= 1;
                if (pits[ind].Firstname == null || !pits[ind].Firstname.IsInDictionary || pits[ind].Chars.IsAllUpper) 
                    res.Coef -= 1;
            }
            else if (pits[ind].Chars.IsAllUpper) 
                res.Coef -= ((float)0.5);
            if (pits[ind].IsInDictionary) 
            {
                if (pits[ind + 1].IsInDictionary) 
                {
                    res.Coef -= 2;
                    if (pits[ind + 1].IsNewlineAfter) 
                        res.Coef++;
                    else if (pits[ind + 1].EndToken.Next != null && pits[ind + 1].EndToken.Next.IsCharOf(".,:")) 
                        res.Coef++;
                    if (pits[ind].IsInDictionary && pits[ind].Firstname == null) 
                        res.Coef--;
                }
                else if (pits[ind].Firstname == null || !pits[ind].Firstname.IsInDictionary) 
                {
                    if (inf.Case.IsUndefined) 
                        res.Coef -= 1;
                    else 
                        foreach (Pullenti.Morph.MorphBaseInfo mi in pits[ind].BeginToken.Morph.Items) 
                        {
                            if (!((mi.Case & inf.Case)).IsUndefined) 
                            {
                                if ((mi is Pullenti.Morph.MorphWordForm) && (mi as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                                {
                                    res.Coef -= 1;
                                    break;
                                }
                            }
                        }
                }
            }
            if (!pits[ind].Chars.IsLatinLetter) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(pits[ind].BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.EndChar >= pits[ind + 1].BeginChar) 
                {
                    if (pits[ind].BeginToken.GetMorphClassInDictionary().IsAdjective) 
                        res.Coef -= 2;
                    else if (pits[ind + 1].BeginToken.GetMorphClassInDictionary().IsNoun) 
                        res.Coef -= 2;
                }
            }
            CorrectCoefAfterLastname(res, pits, ind + 2);
            if (ind > 0 && res.Coef > 0 && pits[ind].IsHiphenBefore) 
            {
                Pullenti.Ner.Core.StatisticBigrammInfo b1 = pits[ind].Kit.Statistics.GetBigrammInfo(pits[ind - 1].BeginToken, pits[ind].BeginToken);
                if (b1 != null && b1.SecondCount == b1.PairCount) 
                {
                    PersonIdentityToken res0 = new PersonIdentityToken(pits[ind].BeginToken, pits[ind + 1].EndToken) { Typ = FioTemplateType.NameSurname };
                    ManageFirstname(res0, pits[ind - 1], inf);
                    res.Firstname = PersonMorphCollection.AddPrefix(res0.Firstname, res.Firstname);
                    res.Coef++;
                    res.BeginToken = pits[ind - 1].BeginToken;
                }
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(res.BeginToken.Previous, false, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(res.EndToken.Next, false, null, false)) 
                res.Coef -= 2;
            Pullenti.Ner.Core.StatisticBigrammInfo bi = pits[0].BeginToken.Kit.Statistics.GetInitialInfo(pits[ind].Value, pits[ind + 1].BeginToken);
            if (bi != null && bi.PairCount > 0) 
                res.Coef += 2;
            if ((!pits[0].IsInDictionary && pits[1].Lastname != null && pits[1].Lastname.IsLastnameHasStdTail) && !pits[1].IsInDictionary) 
                res.Coef += 0.5F;
            if (res.Firstname != null && pits[ind].BeginToken.IsValue("СЛАВА", null)) 
                res.Coef -= 3;
            else if (CheckLatinAfter(res) != null) 
                res.Coef += 2;
            if (pits[0].Firstname == null || ((pits[0].Firstname != null && !pits[0].Firstname.IsInDictionary))) 
            {
                if (pits[0].BeginToken.GetMorphClassInDictionary().IsProperGeo && pits[1].Lastname != null && pits[1].Lastname.IsInOntology) 
                    res.Coef -= 2;
            }
            if (ind == 0 && pits.Count == 2 && pits[0].Chars.IsLatinLetter) 
            {
                if (pits[0].Firstname != null) 
                {
                    if (!isAttrBefore && (pits[0].BeginToken.Previous is Pullenti.Ner.TextToken) && pits[0].BeginToken.Previous.Chars.IsCapitalUpper) 
                        res.Coef -= 1;
                    else 
                        res.Coef += 1;
                }
                if (pits[0].Chars.IsAllUpper && pits[1].Chars.IsCapitalUpper) 
                    res.Coef = 0;
            }
            return res;
        }
        static PersonIdentityToken TryAttachNameSecnameSurname(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, bool prevHasThisTyp = false)
        {
            if ((ind + 2) >= pits.Count || pits[ind].Typ != PersonItemToken.ItemType.Value || pits[ind + 2].Typ != PersonItemToken.ItemType.Value) 
                return null;
            if (pits[ind].IsNewlineAfter) 
            {
                if ((pits.Count == 3 && pits[0].Firstname != null && pits[1].Middlename != null) && pits[2].Lastname != null) 
                {
                }
                else 
                    return null;
            }
            if (pits[ind + 2].Lastname == null && !prevHasThisTyp && !pits[ind].Morph.Language.IsEn) 
                return null;
            bool ok = false;
            bool needTestNameSurname = false;
            int addCoef = 0;
            if (pits[ind + 1].Typ == PersonItemToken.ItemType.Initial) 
                ok = true;
            else if (pits[ind + 1].Typ == PersonItemToken.ItemType.Value && pits[ind + 1].Middlename != null) 
                ok = true;
            else if (pits[ind + 1].Typ == PersonItemToken.ItemType.Value && pits[ind + 2].Firstname == null) 
            {
                Pullenti.Ner.Core.StatisticBigrammInfo b1 = pits[0].Kit.Statistics.GetBigrammInfo(pits[ind + 1].BeginToken, pits[ind + 2].BeginToken);
                Pullenti.Ner.Core.StatisticBigrammInfo b2 = pits[0].Kit.Statistics.GetBigrammInfo(pits[ind].BeginToken, pits[ind + 2].BeginToken);
                if (b1 != null) 
                {
                    if (b1.PairCount == b1.FirstCount && b1.PairCount == b1.SecondCount) 
                    {
                        ok = true;
                        Pullenti.Ner.Core.StatisticBigrammInfo b3 = pits[0].Kit.Statistics.GetBigrammInfo(pits[ind].BeginToken, pits[ind + 1].BeginToken);
                        if (b3 != null) 
                        {
                            if (b3.SecondCount > b3.PairCount) 
                                ok = false;
                            else if (b3.SecondCount == b3.PairCount && pits[ind + 2].IsHiphenBefore) 
                                ok = false;
                        }
                    }
                    else if (b2 != null && (b2.PairCount + b1.PairCount) == b1.SecondCount) 
                        ok = true;
                }
                else if ((ind + 3) == pits.Count && pits[ind + 2].Lastname != null && !pits[ind + 2].IsInDictionary) 
                    ok = true;
                if (!ok) 
                {
                    b1 = pits[0].Kit.Statistics.GetInitialInfo(pits[ind].Value, pits[ind + 2].BeginToken);
                    if (b1 != null && b1.PairCount > 0) 
                    {
                        ok = true;
                        addCoef = 2;
                    }
                }
                if (!ok) 
                {
                    Pullenti.Ner.Core.StatisticWordInfo wi = pits[0].Kit.Statistics.GetWordInfo(pits[ind + 2].EndToken);
                    if (wi != null && wi.LowerCount == 0) 
                    {
                        if (wi.MaleVerbsAfterCount > 0 || wi.FemaleVerbsAfterCount > 0) 
                        {
                            ok = true;
                            addCoef = 2;
                            needTestNameSurname = true;
                            if (pits[ind + 1].Firstname != null && pits[ind + 1].Middlename == null) 
                            {
                                if (pits[ind].Firstname == null && pits[ind].Value != null && pits[ind].IsInDictionary) 
                                    ok = false;
                            }
                            if (pits[ind + 1].Lastname != null && ((pits[ind + 1].Lastname.IsInDictionary || pits[ind + 1].Lastname.IsInOntology))) 
                                ok = false;
                        }
                    }
                }
                if (!ok) 
                {
                    if ((ind == 0 && pits.Count == 3 && pits[0].Chars.IsLatinLetter) && pits[1].Chars.IsLatinLetter && pits[2].Chars.IsLatinLetter) 
                    {
                        if (pits[0].Firstname != null && pits[2].Lastname != null) 
                            ok = true;
                    }
                }
            }
            if (!ok) 
                return null;
            if (IsBothSurnames(pits[ind], pits[ind + 2])) 
                return null;
            ok = false;
            for (int i = ind; i < (ind + 3); i++) 
            {
                if (pits[i].Typ == PersonItemToken.ItemType.Initial) 
                    ok = true;
                else if (!pits[i].IsInDictionary) 
                {
                    Pullenti.Morph.MorphClass cla = pits[i].BeginToken.GetMorphClassInDictionary();
                    if (cla.IsProperName || cla.IsProperSurname || cla.IsProperSecname) 
                        ok = true;
                    else if (cla.IsUndefined) 
                        ok = true;
                }
            }
            if (!ok) 
                return null;
            PersonIdentityToken res = new PersonIdentityToken(pits[ind].BeginToken, pits[ind + 2].EndToken);
            res.Typ = (pits[ind + 1].Typ == PersonItemToken.ItemType.Initial ? FioTemplateType.NameISurname : FioTemplateType.NameSecnameSurname);
            res.Coef -= ind;
            res.Morph = AccordMorph(inf, pits[ind + 2].Lastname, pits[ind].Firstname, pits[ind + 1].Middlename, pits[ind + 2].EndToken.Next);
            if (res.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || res.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
            {
                res.Coef += 1;
                inf = res.Morph;
            }
            ManageFirstname(res, pits[ind], inf);
            ManageLastname(res, pits[ind + 2], inf);
            if (pits[ind + 1].Middlename != null && pits[ind + 1].Middlename.Vars.Count > 0) 
            {
                res.Coef++;
                res.Middlename = pits[ind + 1].Middlename.Vars[0].Value;
                if (pits[ind + 1].Middlename.Vars.Count > 1) 
                {
                    res.Middlename = new PersonMorphCollection();
                    SetValue2(res.Middlename as PersonMorphCollection, pits[ind + 1].Middlename, inf);
                }
                if (pits[ind + 2].Lastname != null) 
                {
                    if (pits[ind + 2].Lastname.IsInDictionary || pits[ind + 2].Lastname.IsLastnameHasStdTail || pits[ind + 2].Lastname.IsHasStdPostfix) 
                        res.Coef++;
                }
            }
            else if (pits[ind + 1].Typ == PersonItemToken.ItemType.Initial) 
            {
                res.Middlename = pits[ind + 1].Value;
                res.Coef++;
                if (pits[ind + 2].Lastname != null) 
                {
                }
                else 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(pits[ind + 2].BeginToken, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns | Pullenti.Ner.Core.NounPhraseParseAttr.ParseAdverbs, 0, null);
                    if (npt != null && npt.EndChar > pits[ind + 2].EndChar) 
                        res.Coef -= 2;
                }
            }
            else if (pits[ind + 1].Firstname != null && pits[ind + 2].Middlename != null && pits.Count == 3) 
                res.Coef -= 2;
            else 
            {
                ManageMiddlename(res, pits[ind + 1], inf);
                res.Coef += ((float)0.5);
            }
            if (pits[ind].Chars != pits[ind + 2].Chars) 
            {
                res.Coef -= 1;
                if (pits[ind].Chars.IsAllUpper) 
                    res.Coef -= 1;
            }
            else if (pits[ind + 1].Typ != PersonItemToken.ItemType.Initial && pits[ind].Chars != pits[ind + 1].Chars) 
                res.Coef -= 1;
            CorrectCoefAfterLastname(res, pits, ind + 3);
            res.Coef += addCoef;
            if (pits[ind].IsInDictionary && pits[ind + 1].IsInDictionary && pits[ind + 2].IsInDictionary) 
                res.Coef--;
            return res;
        }
        static PersonIdentityToken TryAttachNameSecname(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, bool prevHasThisTyp = false)
        {
            if ((ind != 0 || (ind + 2) != pits.Count || pits[ind].Typ != PersonItemToken.ItemType.Value) || pits[ind + 1].Typ != PersonItemToken.ItemType.Value) 
                return null;
            if (pits[ind].IsNewlineAfter) 
                return null;
            if (pits[ind].Firstname == null || pits[ind + 1].Middlename == null) 
                return null;
            PersonIdentityToken res = new PersonIdentityToken(pits[ind].BeginToken, pits[ind + 1].EndToken);
            res.Typ = FioTemplateType.NameSecname;
            res.Morph = AccordMorph(inf, null, pits[ind].Firstname, pits[ind + 1].Middlename, pits[ind + 1].EndToken.Next);
            if (res.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || res.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
            {
                res.Coef += 1;
                inf = res.Morph;
            }
            ManageFirstname(res, pits[ind], inf);
            ManageMiddlename(res, pits[ind + 1], inf);
            res.Coef = 2;
            return res;
        }
        static void CorrectCoefAfterLastname(PersonIdentityToken res, List<PersonItemToken> pits, int ind)
        {
            if (!pits[ind - 1].IsNewlineAfter) 
            {
                PersonAttrToken pat = PersonAttrToken.TryAttach(pits[ind - 1].BeginToken, null, PersonAttrToken.PersonAttrAttachAttrs.OnlyKeyword);
                if (pat != null) 
                    res.Coef -= 1;
            }
            if (ind >= pits.Count) 
            {
                if (CheckLatinAfter(res) != null) 
                    res.Coef += 2;
                Pullenti.Ner.Token te = pits[ind - 1].EndToken;
                Pullenti.Ner.Core.StatisticWordInfo stat = te.Kit.Statistics.GetWordInfo(te);
                if (stat != null) 
                {
                    if (stat.HasBeforePersonAttr) 
                        res.Coef++;
                }
                te = pits[ind - 1].EndToken.Next;
                if (te == null) 
                    return;
                if (PersonHelper.IsPersonSayOrAttrAfter(te)) 
                {
                    res.Coef++;
                    if (res.Chars.IsLatinLetter && res.Typ == FioTemplateType.NameSurname) 
                        res.Coef += 2;
                }
                if (!te.Chars.IsLetter && !te.Chars.IsAllLower) 
                    return;
                Pullenti.Ner.Core.StatisticWordInfo wi = te.Kit.Statistics.GetWordInfo(te);
                if (wi != null) 
                {
                    if (wi.LowerCount > 0) 
                        res.Coef--;
                    else if ((wi.FemaleVerbsAfterCount + wi.MaleVerbsAfterCount) > 0) 
                        res.Coef++;
                }
                return;
            }
            if (ind == 0) 
                return;
            if (pits[ind].Typ == PersonItemToken.ItemType.Value && ((pits[ind].Firstname == null || ind == (pits.Count - 1)))) 
            {
                Pullenti.Ner.Core.StatisticBigrammInfo b1 = pits[0].Kit.Statistics.GetBigrammInfo(pits[ind - 1].BeginToken, pits[ind].BeginToken);
                if ((b1 != null && b1.FirstCount == b1.PairCount && b1.SecondCount == b1.PairCount) && b1.PairCount > 0) 
                {
                    bool ok = false;
                    if (b1.PairCount > 1 && pits[ind].WhitespacesBeforeCount == 1) 
                        ok = true;
                    else if (pits[ind].IsHiphenBefore && pits[ind].Lastname != null) 
                        ok = true;
                    if (ok) 
                    {
                        PersonIdentityToken res1 = new PersonIdentityToken(pits[ind].BeginToken, pits[ind].EndToken);
                        ManageLastname(res1, pits[ind], res.Morph);
                        res.Lastname = PersonMorphCollection.AddPrefix(res.Lastname, res1.Lastname);
                        res.EndToken = pits[ind].EndToken;
                        res.Coef++;
                        ind++;
                        if (ind >= pits.Count) 
                            return;
                    }
                }
            }
            if (pits[ind - 1].WhitespacesBeforeCount > pits[ind - 1].WhitespacesAfterCount) 
                res.Coef -= 1;
            else if (pits[ind - 1].WhitespacesBeforeCount == pits[ind - 1].WhitespacesAfterCount) 
            {
                if (pits[ind].Lastname != null || pits[ind].Firstname != null) 
                {
                    if (!pits[ind].IsInDictionary) 
                        res.Coef -= 1;
                }
            }
        }
        static void CorrectCoefForLastname(PersonIdentityToken pit, PersonItemToken it)
        {
            if (it.BeginToken != it.EndToken) 
                return;
            Pullenti.Ner.TextToken tt = it.BeginToken as Pullenti.Ner.TextToken;
            if (tt == null) 
                return;
            bool inDic = false;
            bool hasStd = false;
            foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
            {
                if (wf.Class.IsProperSurname) 
                {
                }
                else if ((wf as Pullenti.Morph.MorphWordForm).IsInDictionary) 
                    inDic = true;
            }
            if (it.Lastname != null) 
                hasStd = it.Lastname.IsLastnameHasStdTail;
            if (!hasStd && inDic) 
                pit.Coef -= 1.5F;
        }
        static PersonIdentityToken TryAttachSurnameName(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, bool prevHasThisTyp = false)
        {
            if ((ind + 1) >= pits.Count || pits[ind + 1].Typ != PersonItemToken.ItemType.Value || pits[ind].Typ != PersonItemToken.ItemType.Value) 
                return null;
            if (pits[ind].Lastname == null && !prevHasThisTyp) 
                return null;
            if (IsBothSurnames(pits[ind], pits[ind + 1])) 
                return null;
            PersonIdentityToken res = new PersonIdentityToken(pits[ind].BeginToken, pits[ind + 1].EndToken) { Typ = FioTemplateType.SurnameName };
            res.Coef -= ind;
            if (pits[ind].IsNewlineAfter) 
            {
                res.Coef--;
                if (pits[ind].WhitespacesAfterCount > 15) 
                    res.Coef--;
            }
            res.Morph = AccordMorph(inf, pits[ind].Lastname, pits[ind + 1].Firstname, null, pits[ind + 1].EndToken.Next);
            if (res.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || res.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
            {
                if (pits[ind].Lastname != null && !pits[ind].Lastname.Morph.Case.IsUndefined) 
                    res.Coef += 1;
                inf = res.Morph;
            }
            ManageLastname(res, pits[ind], inf);
            ManageFirstname(res, pits[ind + 1], inf);
            CorrectCoefForLastname(res, pits[ind]);
            if (pits[ind].Chars != pits[ind + 1].Chars) 
            {
                res.Coef -= 1;
                if (pits[ind + 1].Firstname == null || !pits[ind + 1].Firstname.IsInDictionary || pits[ind + 1].Chars.IsAllUpper) 
                    res.Coef -= 1;
            }
            else if (pits[ind].Chars.IsAllUpper) 
                res.Coef -= ((float)0.5);
            if (pits[ind + 1].IsInDictionary && ((pits[ind + 1].Firstname == null || !pits[ind + 1].Firstname.IsInDictionary))) 
                res.Coef -= 1;
            CorrectCoefAfterName(res, pits, ind + 2);
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(pits[ind + 1].EndToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
            if (npt != null && npt.EndToken != pits[ind + 1].EndToken) 
                res.Coef -= 1;
            if (ind == 0) 
                CorrectCoefSNS(res, pits, ind + 2);
            if (pits[ind].EndToken.Next.IsHiphen) 
                res.Coef -= 2;
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(res.BeginToken.Previous, false, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(res.EndToken.Next, false, null, false)) 
                res.Coef -= 2;
            if (pits[ind].IsInDictionary) 
            {
                Pullenti.Morph.MorphClass mc = pits[ind].BeginToken.GetMorphClassInDictionary();
                if (mc.IsPronoun || mc.IsPersonalPronoun) 
                    return null;
            }
            if (((pits.Count == 2 && ind == 0 && pits[0].Chars.IsAllUpper) && pits[1].Chars.IsCapitalUpper && !pits[1].IsInDictionary) && (res.Coef < 0)) 
                res.Coef = 0;
            return res;
        }
        static void CorrectCoefSNS(PersonIdentityToken res, List<PersonItemToken> pits, int indAfter)
        {
            if (indAfter >= pits.Count) 
                return;
            if (pits[0].Lastname == null || !pits[0].Lastname.IsLastnameHasStdTail) 
            {
                Pullenti.Ner.Core.StatisticWordInfo stat = pits[0].Kit.Statistics.GetWordInfo(pits[1].BeginToken);
                Pullenti.Ner.Core.StatisticWordInfo statA = pits[0].Kit.Statistics.GetWordInfo(pits[2].BeginToken);
                Pullenti.Ner.Core.StatisticWordInfo statB = pits[0].Kit.Statistics.GetWordInfo(pits[0].BeginToken);
                if (stat != null && statA != null && statB != null) 
                {
                    if (stat.LikeCharsAfterWords != null && stat.LikeCharsBeforeWords != null) 
                    {
                        int couA = 0;
                        int couB = 0;
                        stat.LikeCharsAfterWords.TryGetValue(statA, out couA);
                        stat.LikeCharsBeforeWords.TryGetValue(statB, out couB);
                        if (couA == stat.TotalCount && (couB < stat.TotalCount)) 
                            res.Coef -= 2;
                    }
                }
                return;
            }
            if (pits[1].Firstname == null) 
                return;
            PersonItemToken.MorphPersonItem middle = null;
            if (indAfter > 2 && pits[2].Middlename != null) 
                middle = pits[2].Middlename;
            Pullenti.Morph.MorphBaseInfo inf = new Pullenti.Morph.MorphBaseInfo();
            Pullenti.Morph.MorphBaseInfo mi1 = (Pullenti.Morph.MorphBaseInfo)AccordMorph(inf, pits[0].Lastname, pits[1].Firstname, middle, null);
            if (mi1.Case.IsUndefined) 
                res.Coef -= 1;
            if (pits[indAfter].Lastname == null || !pits[indAfter].Lastname.IsLastnameHasStdTail) 
                return;
            Pullenti.Morph.MorphBaseInfo mi2 = (Pullenti.Morph.MorphBaseInfo)AccordMorph(inf, pits[indAfter].Lastname, pits[1].Firstname, middle, pits[indAfter].EndToken.Next);
            if (!mi2.Case.IsUndefined) 
                res.Coef -= 1;
        }
        static PersonIdentityToken TryAttachSurnameNameSecname(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, bool prevHasThisTyp = false, bool always = false)
        {
            if ((ind + 2) >= pits.Count || pits[ind + 1].Typ != PersonItemToken.ItemType.Value || pits[ind].Typ != PersonItemToken.ItemType.Value) 
                return null;
            if (pits[ind].Lastname == null && !prevHasThisTyp) 
            {
                if (ind > 0) 
                    return null;
                if (pits.Count == 3 && !always) 
                {
                    Pullenti.Ner.Token tt1 = pits[2].EndToken.Next;
                    if (tt1 != null && tt1.IsComma) 
                        tt1 = tt1.Next;
                    if (tt1 != null && !tt1.IsNewlineBefore && PersonAttrToken.TryAttach(tt1, null, PersonAttrToken.PersonAttrAttachAttrs.OnlyKeyword) != null) 
                    {
                    }
                    else 
                        return null;
                }
            }
            if (!always) 
            {
                if (IsBothSurnames(pits[ind], pits[ind + 2])) 
                    return null;
                if (IsBothSurnames(pits[ind], pits[ind + 1])) 
                {
                    if (pits.Count == 3 && ind == 0 && pits[2].Middlename != null) 
                    {
                    }
                    else 
                        return null;
                }
            }
            PersonIdentityToken res = new PersonIdentityToken(pits[ind].BeginToken, pits[ind + 2].EndToken) { Typ = FioTemplateType.SurnameNameSecname };
            if (pits[ind + 2].Middlename == null) 
            {
                if ((ind + 2) == (pits.Count - 1) && prevHasThisTyp) 
                    res.Coef += 1;
                else if (pits[ind + 1].Firstname != null && pits[ind + 2].Firstname != null) 
                {
                }
                else if (!always) 
                    return null;
            }
            res.Coef -= ind;
            if (pits[ind].IsNewlineAfter) 
            {
                if (pits[ind].IsNewlineBefore && pits[ind + 2].IsNewlineAfter) 
                {
                }
                else 
                {
                    res.Coef--;
                    if (pits[ind].WhitespacesAfterCount > 15) 
                        res.Coef--;
                }
            }
            if (pits[ind + 1].IsNewlineAfter) 
            {
                if (pits[ind].IsNewlineBefore && pits[ind + 2].IsNewlineAfter) 
                {
                }
                else 
                {
                    res.Coef--;
                    if (pits[ind + 1].WhitespacesAfterCount > 15) 
                        res.Coef--;
                }
            }
            res.Morph = AccordMorph(inf, pits[ind].Lastname, pits[ind + 1].Firstname, pits[ind + 2].Middlename, pits[ind + 2].EndToken.Next);
            if (res.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || res.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
            {
                res.Coef += 1.5F;
                inf = res.Morph;
            }
            ManageLastname(res, pits[ind], inf);
            CorrectCoefForLastname(res, pits[ind]);
            ManageFirstname(res, pits[ind + 1], inf);
            if (pits[ind + 2].Middlename != null && pits[ind + 2].Middlename.Vars.Count > 0) 
            {
                res.Coef++;
                res.Middlename = pits[ind + 2].Middlename.Vars[0].Value;
                if (pits[ind + 2].Middlename.Vars.Count > 1) 
                {
                    res.Middlename = new PersonMorphCollection();
                    SetValue2(res.Middlename as PersonMorphCollection, pits[ind + 2].Middlename, inf);
                }
                if (pits[ind + 1].Firstname != null && pits.Count == 3 && !pits[ind].IsInDictionary) 
                    res.Coef++;
            }
            else 
                ManageMiddlename(res, pits[ind + 2], inf);
            if (pits[ind].Chars != pits[ind + 1].Chars || pits[ind].Chars != pits[ind + 2].Chars) 
            {
                res.Coef -= 1;
                if (pits[ind].Chars.IsAllUpper && pits[ind + 1].Chars.IsCapitalUpper && pits[ind + 2].Chars.IsCapitalUpper) 
                    res.Coef += 2;
            }
            Pullenti.Ner.TextToken tt = pits[ind].BeginToken as Pullenti.Ner.TextToken;
            if (tt != null) 
            {
                if (tt.IsValue("УВАЖАЕМЫЙ", null) || tt.IsValue("ДОРОГОЙ", null)) 
                    res.Coef -= 2;
            }
            CorrectCoefAfterName(res, pits, ind + 3);
            if (ind == 0) 
                CorrectCoefSNS(res, pits, ind + 3);
            if (pits[ind].IsInDictionary && pits[ind + 1].IsInDictionary && pits[ind + 2].IsInDictionary) 
                res.Coef--;
            return res;
        }
        static void CorrectCoefAfterName(PersonIdentityToken res, List<PersonItemToken> pits, int ind)
        {
            if (ind >= pits.Count) 
                return;
            if (ind == 0) 
                return;
            if (pits[ind - 1].WhitespacesBeforeCount > pits[ind - 1].WhitespacesAfterCount) 
                res.Coef -= 1;
            else if (pits[ind - 1].WhitespacesBeforeCount == pits[ind - 1].WhitespacesAfterCount) 
            {
                if (pits[ind].Lastname != null || pits[ind].Firstname != null || pits[ind].Middlename != null) 
                    res.Coef -= 1;
            }
            Pullenti.Ner.Token t = pits[ind - 1].EndToken.Next;
            if (t != null && t.Next != null && t.Next.IsChar(',')) 
                t = t.Next;
            if (t != null) 
            {
                if (PersonAttrToken.TryAttach(t, null, PersonAttrToken.PersonAttrAttachAttrs.OnlyKeyword) != null) 
                    res.Coef += 1;
            }
        }
        static float _calcCoefAfter(Pullenti.Ner.Token tt)
        {
            if (tt != null && tt.IsComma) 
                tt = tt.Next;
            PersonAttrToken attr = PersonAttrToken.TryAttach(tt, null, PersonAttrToken.PersonAttrAttachAttrs.OnlyKeyword);
            if (attr != null && attr.Age != null) 
                return 3;
            if (tt != null && tt.GetReferent() != null && tt.GetReferent().TypeName == "DATE") 
            {
                float co = (float)1;
                if (tt.Next != null && tt.Next.IsValue("Р", null)) 
                    co += 2;
                return co;
            }
            return 0;
        }
        static PersonIdentityToken TryAttachSurnameII(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf)
        {
            if ((ind + 1) >= pits.Count || pits[ind + 1].Typ != PersonItemToken.ItemType.Initial || pits[ind].Typ == PersonItemToken.ItemType.Initial) 
                return null;
            if (pits[ind].IsNewlineAfter) 
                return null;
            if (pits[ind].Lastname == null) 
                return null;
            PersonIdentityToken res = new PersonIdentityToken(pits[ind].BeginToken, pits[ind + 1].EndToken) { Typ = FioTemplateType.SurnameI };
            res.Coef -= ind;
            ManageLastname(res, pits[ind], inf);
            if (pits[ind].IsAsianItem(false) && pits[ind].Lastname != null && pits[ind].Lastname.IsChinaSurname) 
            {
            }
            else if (pits[ind].Firstname != null && pits[ind].Firstname.IsInDictionary) 
            {
                if (pits[ind].Lastname == null || !pits[ind].Lastname.IsLastnameHasStdTail) 
                {
                    if ((ind == 0 && pits.Count == 3 && !pits[1].IsNewlineAfter) && !pits[2].IsWhitespaceAfter) 
                    {
                    }
                    else 
                        res.Coef -= 2;
                }
            }
            res.Morph = (pits[ind].Lastname == null ? pits[ind].Morph : pits[ind].Lastname.Morph);
            if (res.Lastname.Gender != Pullenti.Morph.MorphGender.Undefined) 
                res.Morph.Gender = res.Lastname.Gender;
            if (pits[ind].WhitespacesAfterCount < 2) 
                res.Coef += ((float)0.5);
            res.Firstname = new PersonMorphCollection();
            res.Firstname.Add(pits[ind + 1].Value, null, Pullenti.Morph.MorphGender.Undefined, false);
            int i1 = ind + 2;
            if ((i1 < pits.Count) && pits[i1].Typ == PersonItemToken.ItemType.Initial) 
            {
                res.Typ = FioTemplateType.SurnameII;
                res.EndToken = pits[i1].EndToken;
                res.Middlename = pits[i1].Value;
                if (pits[i1].WhitespacesBeforeCount < 2) 
                    res.Coef += ((float)0.5);
                i1++;
            }
            if (i1 >= pits.Count) 
            {
                if (pits[0].Lastname != null && ((pits[0].Lastname.IsInDictionary || pits[0].Lastname.IsInOntology)) && pits[0].Firstname == null) 
                    res.Coef++;
                return res;
            }
            if (pits[ind].WhitespacesAfterCount > pits[i1].WhitespacesBeforeCount) 
                res.Coef--;
            else if (pits[ind].WhitespacesAfterCount == pits[i1].WhitespacesBeforeCount && pits[i1].Lastname != null) 
            {
                if ((i1 + 3) == pits.Count && pits[i1 + 1].Typ == PersonItemToken.ItemType.Initial && pits[i1 + 2].Typ == PersonItemToken.ItemType.Initial) 
                {
                }
                else 
                {
                    if (pits[i1].IsInDictionary && pits[i1].BeginToken.GetMorphClassInDictionary().IsNoun) 
                    {
                    }
                    else 
                        res.Coef--;
                    bool ok = true;
                    for (Pullenti.Ner.Token tt = pits[ind].BeginToken.Previous; tt != null; tt = tt.Previous) 
                    {
                        if (tt.IsNewlineBefore) 
                            break;
                        else if (tt.GetReferent() != null && !(tt.GetReferent() is Pullenti.Ner.Person.PersonReferent)) 
                        {
                            ok = false;
                            break;
                        }
                        else if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter) 
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok) 
                        res.Coef++;
                }
            }
            return res;
        }
        static PersonIdentityToken TryAttachIISurname(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf)
        {
            if ((ind + 1) >= pits.Count || pits[ind].Typ != PersonItemToken.ItemType.Initial) 
                return null;
            if (ind > 0) 
            {
                if (pits[ind - 1].Typ == PersonItemToken.ItemType.Initial) 
                    return null;
            }
            if (pits[ind].IsNewlineAfter) 
                return null;
            PersonIdentityToken res = new PersonIdentityToken(pits[ind].BeginToken, pits[ind + 1].EndToken) { Typ = FioTemplateType.ISurname };
            res.Coef -= ind;
            res.Firstname = new PersonMorphCollection();
            res.Firstname.Add(pits[ind].Value, null, Pullenti.Morph.MorphGender.Undefined, false);
            int i1 = ind + 1;
            if (pits[i1].Typ == PersonItemToken.ItemType.Initial) 
            {
                res.Typ = FioTemplateType.IISurname;
                res.Middlename = pits[i1].Value;
                if (pits[i1].WhitespacesBeforeCount < 2) 
                    res.Coef += ((float)0.5);
                i1++;
            }
            if (i1 >= pits.Count || pits[i1].Typ != PersonItemToken.ItemType.Value) 
                return null;
            if (pits[i1].IsNewlineBefore) 
                return null;
            res.EndToken = pits[i1].EndToken;
            PersonItemToken prev = null;
            if (!pits[ind].IsNewlineBefore) 
            {
                if (ind > 0) 
                    prev = pits[ind - 1];
                else 
                {
                    prev = PersonItemToken.TryAttach(pits[ind].BeginToken.Previous, null, (pits[i1].Chars.IsLatinLetter ? PersonItemToken.ParseAttr.CanBeLatin : PersonItemToken.ParseAttr.No), null);
                    if (prev != null) 
                    {
                        if (PersonAttrToken.TryAttachWord(prev.BeginToken) != null) 
                        {
                            prev = null;
                            res.Coef++;
                        }
                    }
                }
            }
            ManageLastname(res, pits[i1], inf);
            if (pits[i1].Lastname != null && pits[i1].Lastname.IsInOntology) 
                res.Coef++;
            if (pits[i1].Firstname != null && pits[i1].Firstname.IsInDictionary) 
            {
                if (pits[i1].Lastname == null || ((!pits[i1].Lastname.IsLastnameHasStdTail && !pits[i1].Lastname.IsInOntology))) 
                    res.Coef -= 2;
            }
            if (prev != null) 
            {
                Pullenti.Morph.MorphClass mc = prev.BeginToken.GetMorphClassInDictionary();
                if (mc.IsPreposition || mc.IsAdverb || mc.IsVerb) 
                {
                    res.Coef += ind;
                    if (pits[i1].Lastname != null) 
                    {
                        if (pits[i1].Lastname.IsInDictionary || pits[i1].Lastname.IsInOntology) 
                            res.Coef += 1;
                    }
                }
                if (prev.Lastname != null && ((prev.Lastname.IsLastnameHasStdTail || prev.Lastname.IsInDictionary))) 
                    res.Coef -= 1;
            }
            res.Morph = (pits[i1].Lastname == null ? pits[i1].Morph : pits[i1].Lastname.Morph);
            if (res.Lastname.Gender != Pullenti.Morph.MorphGender.Undefined) 
                res.Morph.Gender = res.Lastname.Gender;
            if (pits[i1].WhitespacesBeforeCount < 2) 
            {
                if (!pits[ind].IsNewlineBefore && (pits[ind].WhitespacesBeforeCount < 2) && prev != null) 
                {
                }
                else 
                    res.Coef += ((float)0.5);
            }
            if (prev == null) 
            {
                if (pits[ind].IsNewlineBefore && pits[i1].IsNewlineAfter) 
                    res.Coef += 1;
                else if (pits[i1].EndToken.Next != null && ((pits[i1].EndToken.Next.IsCharOf(";,.") || pits[i1].EndToken.Next.Morph.Class.IsConjunction))) 
                    res.Coef += 1;
                return res;
            }
            if (prev.WhitespacesAfterCount < pits[i1].WhitespacesBeforeCount) 
                res.Coef--;
            else if (prev.WhitespacesAfterCount == pits[i1].WhitespacesBeforeCount && prev.Lastname != null) 
                res.Coef--;
            return res;
        }
        static PersonIdentityToken TryAttachKing(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, bool prevHasThisTyp = false)
        {
            if (ind > 0 || ind >= pits.Count) 
                return null;
            if (pits[0].Firstname == null || pits[0].IsNewlineAfter) 
                return null;
            if (pits[0].BeginToken.IsValue("ТОМ", null)) 
                return null;
            int i = 0;
            if (pits.Count > 1 && ((pits[1].Firstname != null || pits[1].Middlename != null))) 
                i++;
            if (pits[i].IsNewlineAfter) 
                return null;
            if (pits[i].EndToken.WhitespacesAfterCount > 2) 
                return null;
            int num = 0;
            bool roman = false;
            bool ok = false;
            Pullenti.Ner.Token t = pits[i].EndToken.Next;
            if (t is Pullenti.Ner.NumberToken) 
            {
                if (t.Chars.IsAllLower || (t as Pullenti.Ner.NumberToken).IntValue == null) 
                    return null;
                num = (t as Pullenti.Ner.NumberToken).IntValue.Value;
                if (!t.Morph.Class.IsAdjective) 
                    return null;
            }
            else 
            {
                if (((i + 2) < pits.Count) && pits[i + 1].Typ == PersonItemToken.ItemType.Initial) 
                    return null;
                Pullenti.Ner.NumberToken nt = Pullenti.Ner.Core.NumberHelper.TryParseRoman(t);
                if (nt != null && nt.IntValue != null) 
                {
                    num = nt.IntValue.Value;
                    roman = true;
                    t = nt.EndToken;
                }
            }
            if (num < 1) 
            {
                if (pits[0].Firstname != null && prevHasThisTyp) 
                {
                    if (pits.Count == 1) 
                        ok = true;
                    else if (pits.Count == 2 && pits[0].EndToken.Next.IsHiphen) 
                        ok = true;
                }
                if (!ok) 
                    return null;
            }
            PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[0].EndToken) { Typ = FioTemplateType.King };
            res.Morph = AccordMorph(inf, null, pits[0].Firstname, (pits.Count == 2 ? (pits[1].Middlename ?? pits[1].Firstname) : null), pits[(pits.Count == 2 ? 1 : 0)].EndToken.Next);
            if (res.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || res.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                inf = res.Morph;
            if (inf.Gender != Pullenti.Morph.MorphGender.Feminie && inf.Gender != Pullenti.Morph.MorphGender.Masculine && !roman) 
                return null;
            ManageFirstname(res, pits[0], inf);
            if (num > 0) 
            {
                res.Lastname = new PersonMorphCollection();
                res.Lastname.Number = num;
                res.EndToken = t;
            }
            if (i > 0) 
            {
                ManageMiddlename(res, pits[1], inf);
                res.EndToken = pits[1].EndToken;
            }
            res.Coef = (num > 0 ? 3 : 2);
            return res;
        }
        static PersonIdentityToken TryAttachAsian(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf, int cou, bool prevHasThisTyp = false)
        {
            if (ind > 0 || ind >= pits.Count || ((pits.Count != cou && pits.Count != (cou * 2)))) 
                return null;
            if (pits[0].Lastname != null && pits[0].Lastname.IsChinaSurname && pits[0].Chars.IsCapitalUpper) 
            {
                if (cou == 3) 
                {
                    if (!pits[1].IsAsianItem(false)) 
                        return null;
                    if (!pits[2].IsAsianItem(true)) 
                        return null;
                }
                else if (cou == 2) 
                {
                    if (pits[1].Typ != PersonItemToken.ItemType.Value) 
                        return null;
                }
            }
            else if (cou == 3) 
            {
                if (!pits[0].IsAsianItem(false)) 
                    return null;
                if (!pits[1].IsAsianItem(false)) 
                    return null;
                if (!pits[2].IsAsianItem(true)) 
                    return null;
            }
            else 
            {
                if (!pits[0].IsAsianItem(false)) 
                    return null;
                if (!pits[1].IsAsianItem(true)) 
                    return null;
            }
            cou--;
            bool isChineSur = pits[0].Lastname != null && pits[0].Lastname.IsChinaSurname;
            PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[cou].EndToken) { Typ = FioTemplateType.AsianName };
            if (pits[cou].Lastname != null) 
                res.Morph = AccordMorph(inf, pits[cou].Lastname, null, null, pits[cou].EndToken.Next);
            if (!res.Morph.Case.IsUndefined) 
                inf = res.Morph;
            if (isChineSur) 
            {
                res.Typ = FioTemplateType.AsianSurnameName;
                res.Coef = 2;
                if (pits[1].IsAsianItem(true)) 
                    res.Coef += 1;
                ManageLastname(res, pits[0], inf);
                string tr = Pullenti.Ner.Person.PersonReferent._DelSurnameEnd(pits[0].Value);
                if (tr != pits[0].Value) 
                    res.Lastname.Add(tr, null, Pullenti.Morph.MorphGender.Masculine, false);
                res.Firstname = new PersonMorphCollection();
                string pref = (cou == 2 ? pits[1].Value : "");
                if (pits[cou].IsAsianItem(false)) 
                {
                    res.Firstname.Add(pref + pits[cou].Value, null, Pullenti.Morph.MorphGender.Masculine, false);
                    res.Firstname.Add(pref + pits[cou].Value, null, Pullenti.Morph.MorphGender.Feminie, false);
                    if (pref.Length > 0) 
                    {
                        res.Firstname.Add(pref + "-" + pits[cou].Value, null, Pullenti.Morph.MorphGender.Masculine, false);
                        res.Firstname.Add(pref + "-" + pits[cou].Value, null, Pullenti.Morph.MorphGender.Feminie, false);
                    }
                }
                else 
                {
                    string v = Pullenti.Ner.Person.PersonReferent._DelSurnameEnd(pits[cou].Value);
                    res.Firstname.Add(pref + v, null, Pullenti.Morph.MorphGender.Masculine, false);
                    if (pref.Length > 0) 
                        res.Firstname.Add(pref + "-" + v, null, Pullenti.Morph.MorphGender.Masculine, false);
                    string ss = pits[cou].EndToken.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    if (ss != v && ss.Length <= v.Length) 
                    {
                        res.Firstname.Add(pref + ss, null, Pullenti.Morph.MorphGender.Masculine, false);
                        if (pref.Length > 0) 
                            res.Firstname.Add(pref + "-" + ss, null, Pullenti.Morph.MorphGender.Masculine, false);
                    }
                    inf.Gender = Pullenti.Morph.MorphGender.Masculine;
                }
            }
            else 
            {
                if (inf.Gender == Pullenti.Morph.MorphGender.Masculine) 
                    ManageLastname(res, pits[cou], inf);
                else 
                {
                    res.Lastname = new PersonMorphCollection();
                    if (pits[cou].IsAsianItem(false)) 
                    {
                        res.Lastname.Add(pits[cou].Value, null, Pullenti.Morph.MorphGender.Masculine, false);
                        res.Lastname.Add(pits[cou].Value, null, Pullenti.Morph.MorphGender.Feminie, false);
                    }
                    else 
                    {
                        string v = Pullenti.Ner.Person.PersonReferent._DelSurnameEnd(pits[cou].Value);
                        res.Lastname.Add(v, null, Pullenti.Morph.MorphGender.Masculine, false);
                        string ss = pits[cou].EndToken.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        if (ss != v && ss.Length <= v.Length) 
                            res.Lastname.Add(ss, null, Pullenti.Morph.MorphGender.Masculine, false);
                        inf.Gender = Pullenti.Morph.MorphGender.Masculine;
                    }
                }
                if (cou == 2) 
                {
                    res.Coef = 2;
                    if ((res.WhitespacesAfterCount < 2) && pits.Count > 3) 
                        res.Coef--;
                    res.Lastname.AddPrefixStr(string.Format("{0} {1} ", pits[0].Value, pits[1].Value));
                }
                else 
                {
                    res.Coef = 1;
                    res.Lastname.AddPrefixStr(pits[0].Value + " ");
                }
                for (int i = 0; i < pits.Count; i++) 
                {
                    if (pits[i].IsInDictionary) 
                    {
                        Pullenti.Morph.MorphClass mc = pits[i].BeginToken.GetMorphClassInDictionary();
                        if ((mc.IsConjunction || mc.IsPronoun || mc.IsPreposition) || mc.IsPersonalPronoun) 
                            res.Coef -= 0.5F;
                    }
                }
            }
            if (pits[0].Value == pits[1].Value) 
                res.Coef -= 0.5F;
            if (cou == 2) 
            {
                if (pits[0].Value == pits[2].Value) 
                    res.Coef -= 0.5F;
                if (pits[1].Value == pits[2].Value) 
                    res.Coef -= 0.5F;
            }
            if (!pits[cou].IsWhitespaceAfter) 
            {
                Pullenti.Ner.Token t = pits[cou].EndToken.Next;
                if (t != null && t.IsHiphen) 
                    res.Coef -= 0.5F;
                if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, false, null, false)) 
                    res.Coef -= 0.5F;
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(pits[0].BeginToken.Previous, false, false)) 
                res.Coef -= 0.5F;
            return res;
        }
        public static PersonIdentityToken TryAttachIdentity(List<PersonItemToken> pits, Pullenti.Morph.MorphBaseInfo inf)
        {
            if (pits.Count == 1) 
            {
                if (pits[0].Typ != PersonItemToken.ItemType.Referent) 
                    return null;
            }
            else 
            {
                if (pits.Count != 2 && pits.Count != 3) 
                    return null;
                foreach (PersonItemToken p in pits) 
                {
                    if (p.Typ != PersonItemToken.ItemType.Value) 
                        return null;
                    if (p.Chars != pits[0].Chars) 
                        return null;
                }
            }
            Pullenti.Ner.TextToken begin = pits[0].BeginToken as Pullenti.Ner.TextToken;
            Pullenti.Ner.TextToken end = pits[pits.Count - 1].EndToken as Pullenti.Ner.TextToken;
            if (begin == null || end == null) 
                return null;
            PersonIdentityToken res = new PersonIdentityToken(begin, end);
            res.Lastname = new PersonMorphCollection();
            string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(begin, end, Pullenti.Ner.Core.GetTextAttr.No);
            if (s.Length > 100) 
                return null;
            StringBuilder tmp = new StringBuilder();
            for (Pullenti.Ner.Token t = (Pullenti.Ner.Token)begin; t != null && t.Previous != end; t = t.Next) 
            {
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    continue;
                if (tt.IsHiphen) 
                {
                    tmp.Append('-');
                    continue;
                }
                if (tmp.Length > 0) 
                {
                    if (tmp[tmp.Length - 1] != '-') 
                        tmp.Append(' ');
                }
                if (tt.LengthChar < 3) 
                {
                    tmp.Append(tt.Term);
                    continue;
                }
                string sss = tt.Term;
                foreach (Pullenti.Morph.MorphBaseInfo wff in tt.Morph.Items) 
                {
                    Pullenti.Morph.MorphWordForm wf = wff as Pullenti.Morph.MorphWordForm;
                    if (wf != null && wf.NormalCase != null && (wf.NormalCase.Length < sss.Length)) 
                        sss = wf.NormalCase;
                }
                tmp.Append(sss);
            }
            string ss = tmp.ToString();
            if (inf.Case.IsNominative) 
            {
                res.Lastname.Add(s, null, Pullenti.Morph.MorphGender.Undefined, false);
                if (s != ss) 
                    res.Lastname.Add(ss, null, Pullenti.Morph.MorphGender.Undefined, false);
            }
            else 
            {
                if (s != ss) 
                    res.Lastname.Add(ss, null, Pullenti.Morph.MorphGender.Undefined, false);
                res.Lastname.Add(s, null, Pullenti.Morph.MorphGender.Undefined, false);
            }
            foreach (PersonItemToken p in pits) 
            {
                if (p != pits[0]) 
                {
                    if (p.IsNewlineBefore) 
                        res.Coef -= 1;
                    else if (p.WhitespacesBeforeCount > 1) 
                        res.Coef -= ((float)0.5);
                }
                res.Coef += ((float)0.5);
                if (p.LengthChar > 4) 
                {
                    if (p.IsInDictionary) 
                        res.Coef -= ((float)1.5);
                    if (p.Lastname != null && ((p.Lastname.IsInDictionary || p.Lastname.IsInOntology))) 
                        res.Coef -= 1;
                    if (p.Firstname != null && p.Firstname.IsInDictionary) 
                        res.Coef -= 1;
                    if (p.Middlename != null) 
                        res.Coef -= 1;
                    if (p.Chars.IsAllUpper) 
                        res.Coef -= ((float)0.5);
                }
                else if (p.Chars.IsAllUpper) 
                    res.Coef -= 1;
            }
            if (pits.Count == 2 && pits[1].Lastname != null && ((pits[1].Lastname.IsLastnameHasStdTail || pits[1].Lastname.IsInDictionary))) 
                res.Coef -= 0.5F;
            return res;
        }
        static PersonIdentityToken TryAttachGlobal(List<PersonItemToken> pits, int ind, Pullenti.Morph.MorphBaseInfo inf)
        {
            if (ind > 0 || pits[0].Typ != PersonItemToken.ItemType.Value) 
                return null;
            if ((pits.Count == 4 && pits[0].Value == "АУН" && pits[1].Value == "САН") && pits[2].Value == "СУ" && pits[3].Value == "ЧЖИ") 
            {
                PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[3].EndToken);
                res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_IDENTITY, "АУН САН СУ ЧЖИ", false, 0);
                res.OntologyPerson.IsFemale = true;
                res.Coef = 10;
                return res;
            }
            if (pits.Count == 2 && pits[0].Firstname != null && pits[0].Firstname.IsInDictionary) 
            {
                if (pits[0].BeginToken.IsValue("ИВАН", null) && pits[1].BeginToken.IsValue("ГРОЗНЫЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, "ИВАН", false, 0);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, "ГРОЗНЫЙ", false, 0);
                    res.OntologyPerson.IsMale = true;
                    res.Coef = 10;
                    return res;
                }
                if (pits[0].BeginToken.IsValue("ЮРИЙ", null) && pits[1].BeginToken.IsValue("ДОЛГОРУКИЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME, "ЮРИЙ", false, 0);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, "ДОЛГОРУКИЙ", false, 0);
                    res.OntologyPerson.IsMale = true;
                    res.Coef = 10;
                    return res;
                }
                if (pits[1].BeginToken.IsValue("ВЕЛИКИЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    if (pits[0].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        res.OntologyPerson.IsFemale = true;
                    else if (pits[0].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || ((pits[1].Morph.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                        res.OntologyPerson.IsMale = true;
                    else 
                        return null;
                    ManageFirstname(res, pits[0], pits[1].Morph);
                    res.OntologyPerson.AddFioIdentity(null, res.Firstname, null);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, (res.OntologyPerson.IsMale ? "ВЕЛИКИЙ" : "ВЕЛИКАЯ"), false, 0);
                    res.Coef = 10;
                    return res;
                }
                if (pits[1].BeginToken.IsValue("СВЯТОЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    if (pits[0].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        res.OntologyPerson.IsFemale = true;
                    else if (pits[0].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || ((pits[1].Morph.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                        res.OntologyPerson.IsMale = true;
                    else 
                        return null;
                    ManageFirstname(res, pits[0], pits[1].Morph);
                    res.OntologyPerson.AddFioIdentity(null, res.Firstname, null);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, (res.OntologyPerson.IsMale ? "СВЯТОЙ" : "СВЯТАЯ"), false, 0);
                    res.Coef = 10;
                    return res;
                }
                if (pits[1].BeginToken.IsValue("ПРЕПОДОБНЫЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    if (pits[0].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        res.OntologyPerson.IsFemale = true;
                    else if (pits[0].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || ((pits[1].Morph.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                        res.OntologyPerson.IsMale = true;
                    else 
                        return null;
                    ManageFirstname(res, pits[0], pits[1].Morph);
                    res.OntologyPerson.AddFioIdentity(null, res.Firstname, null);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, (res.OntologyPerson.IsMale ? "ПРЕПОДОБНЫЙ" : "ПРЕПОДОБНАЯ"), false, 0);
                    res.Coef = 10;
                    return res;
                }
                if (pits[1].BeginToken.IsValue("БЛАЖЕННЫЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    if (pits[0].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        res.OntologyPerson.IsFemale = true;
                    else if (pits[0].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || ((pits[1].Morph.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                        res.OntologyPerson.IsMale = true;
                    else 
                        return null;
                    ManageFirstname(res, pits[0], pits[1].Morph);
                    res.OntologyPerson.AddFioIdentity(null, res.Firstname, null);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, (res.OntologyPerson.IsMale ? "БЛАЖЕННЫЙ" : "БЛАЖЕННАЯ"), false, 0);
                    res.Coef = 10;
                    return res;
                }
            }
            if (pits.Count == 2 && pits[1].Firstname != null && pits[1].Firstname.IsInDictionary) 
            {
                if (pits[0].BeginToken.IsValue("СВЯТОЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    if (pits[1].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie || pits[0].Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        res.OntologyPerson.IsFemale = true;
                    else if (pits[1].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || ((pits[0].Morph.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                        res.OntologyPerson.IsMale = true;
                    else 
                        return null;
                    ManageFirstname(res, pits[1], pits[0].Morph);
                    res.OntologyPerson.AddFioIdentity(null, res.Firstname, null);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, (res.OntologyPerson.IsMale ? "СВЯТОЙ" : "СВЯТАЯ"), false, 0);
                    res.Coef = 10;
                    return res;
                }
                if (pits[0].BeginToken.IsValue("ПРЕПОДОБНЫЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    if (pits[1].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        res.OntologyPerson.IsFemale = true;
                    else if (pits[1].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || ((pits[0].Morph.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                        res.OntologyPerson.IsMale = true;
                    else 
                        return null;
                    ManageFirstname(res, pits[1], pits[0].Morph);
                    res.OntologyPerson.AddFioIdentity(null, res.Firstname, null);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, (res.OntologyPerson.IsMale ? "ПРЕПОДОБНЫЙ" : "ПРЕПОДОБНАЯ"), false, 0);
                    res.Coef = 10;
                    return res;
                }
                if (pits[0].BeginToken.IsValue("БЛАЖЕННЫЙ", null)) 
                {
                    PersonIdentityToken res = new PersonIdentityToken(pits[0].BeginToken, pits[1].EndToken);
                    res.OntologyPerson = new Pullenti.Ner.Person.PersonReferent();
                    if (pits[1].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        res.OntologyPerson.IsFemale = true;
                    else if (pits[1].Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine || ((pits[0].Morph.Gender & Pullenti.Morph.MorphGender.Masculine)) != Pullenti.Morph.MorphGender.Undefined) 
                        res.OntologyPerson.IsMale = true;
                    else 
                        return null;
                    ManageFirstname(res, pits[1], pits[0].Morph);
                    res.OntologyPerson.AddFioIdentity(null, res.Firstname, null);
                    res.OntologyPerson.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_NICKNAME, (res.OntologyPerson.IsMale ? "БЛАЖЕННЫЙ" : "БЛАЖЕННАЯ"), false, 0);
                    res.Coef = 10;
                    return res;
                }
            }
            return null;
        }
        static Pullenti.Ner.MorphCollection AccordMorph(Pullenti.Morph.MorphBaseInfo inf, PersonItemToken.MorphPersonItem p1, PersonItemToken.MorphPersonItem p2, PersonItemToken.MorphPersonItem p3, Pullenti.Ner.Token next)
        {
            Pullenti.Ner.MorphCollection res = new Pullenti.Ner.MorphCollection();
            List<PersonItemToken.MorphPersonItem> pp = new List<PersonItemToken.MorphPersonItem>();
            if (p1 != null) 
                pp.Add(p1);
            if (p2 != null) 
                pp.Add(p2);
            if (p3 != null) 
                pp.Add(p3);
            if (pp.Count == 0) 
                return res;
            if (inf != null && p1 != null && ((p1.IsLastnameHasStdTail || p1.IsInDictionary))) 
            {
                if (((inf.Case & p1.Morph.Case)).IsUndefined) 
                    inf = null;
            }
            if (inf != null && p2 != null && p2.IsInDictionary) 
            {
                if (((inf.Case & p2.Morph.Case)).IsUndefined) 
                    inf = null;
            }
            for (int i = 0; i < 2; i++) 
            {
                Pullenti.Morph.MorphGender g = (i == 0 ? Pullenti.Morph.MorphGender.Masculine : Pullenti.Morph.MorphGender.Feminie);
                if (inf != null && inf.Gender != Pullenti.Morph.MorphGender.Undefined && ((inf.Gender & g)) == Pullenti.Morph.MorphGender.Undefined) 
                    continue;
                Pullenti.Morph.MorphCase cas = Pullenti.Morph.MorphCase.AllCases;
                foreach (PersonItemToken.MorphPersonItem p in pp) 
                {
                    Pullenti.Morph.MorphCase ca = new Pullenti.Morph.MorphCase();
                    foreach (PersonItemToken.MorphPersonItemVariant v in p.Vars) 
                    {
                        if (v.Gender != Pullenti.Morph.MorphGender.Undefined) 
                        {
                            if (((v.Gender & g)) == Pullenti.Morph.MorphGender.Undefined) 
                                continue;
                        }
                        if (inf != null && !inf.Case.IsUndefined && !v.Case.IsUndefined) 
                        {
                            if (((inf.Case & v.Case)).IsUndefined) 
                                continue;
                        }
                        if (!v.Case.IsUndefined) 
                            ca |= v.Case;
                        else 
                            ca = Pullenti.Morph.MorphCase.AllCases;
                    }
                    cas &= ca;
                }
                if (!cas.IsUndefined) 
                {
                    if (inf != null && !inf.Case.IsUndefined && !((inf.Case & cas)).IsUndefined) 
                        cas &= inf.Case;
                    res.AddItem(new Pullenti.Morph.MorphBaseInfo() { Gender = g, Case = cas });
                }
            }
            Pullenti.Morph.MorphGender verbGend = Pullenti.Morph.MorphGender.Undefined;
            if ((next != null && (next is Pullenti.Ner.TextToken) && next.Chars.IsAllLower) && next.Morph.Class == Pullenti.Morph.MorphClass.Verb && next.Morph.Number == Pullenti.Morph.MorphNumber.Singular) 
            {
                if (next.Morph.Gender == Pullenti.Morph.MorphGender.Feminie || next.Morph.Gender == Pullenti.Morph.MorphGender.Masculine) 
                {
                    verbGend = next.Morph.Gender;
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if ((npt != null && npt.Morph.Case.IsNominative && npt.Morph.Gender == verbGend) && npt.Morph.Number == Pullenti.Morph.MorphNumber.Singular) 
                        verbGend = Pullenti.Morph.MorphGender.Undefined;
                }
            }
            if (verbGend != Pullenti.Morph.MorphGender.Undefined && res.ItemsCount > 1) 
            {
                int cou = 0;
                foreach (Pullenti.Morph.MorphBaseInfo it in res.Items) 
                {
                    if (it.Case.IsNominative && it.Gender == verbGend) 
                        cou++;
                }
                if (cou == 1) 
                {
                    for (int i = res.ItemsCount - 1; i >= 0; i--) 
                    {
                        if (!res[i].Case.IsNominative || res[i].Gender != verbGend) 
                            res.RemoveItem(i);
                    }
                }
            }
            return res;
        }
        static bool IsAccords(PersonItemToken.MorphPersonItem mt, Pullenti.Morph.MorphBaseInfo inf)
        {
            if (inf == null) 
                return true;
            if (mt.Vars.Count == 0) 
                return true;
            foreach (PersonItemToken.MorphPersonItemVariant wf in mt.Vars) 
            {
                bool ok = true;
                if (!inf.Case.IsUndefined && !wf.Case.IsUndefined) 
                {
                    if (((wf.Case & inf.Case)).IsUndefined) 
                        ok = false;
                }
                if (inf.Gender != Pullenti.Morph.MorphGender.Undefined && wf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                {
                    if (((inf.Gender & wf.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                        ok = false;
                }
                if (ok) 
                    return true;
            }
            return false;
        }
        static bool IsBothSurnames(PersonItemToken p1, PersonItemToken p2)
        {
            if (p1 == null || p2 == null) 
                return false;
            if (p1.Lastname == null || p2.Lastname == null) 
                return false;
            if (!p1.Lastname.IsInDictionary && !p1.Lastname.IsInOntology && !p1.Lastname.IsLastnameHasStdTail) 
                return false;
            if (p1.Firstname != null || p2.Middlename != null) 
                return false;
            if (!p2.Lastname.IsInDictionary && !p2.Lastname.IsInOntology && !p2.Lastname.IsLastnameHasStdTail) 
                return false;
            if (p2.Firstname != null || p2.Middlename != null) 
                return false;
            if (!(p1.EndToken is Pullenti.Ner.TextToken) || !(p2.EndToken is Pullenti.Ner.TextToken)) 
                return false;
            string v1 = (p1.EndToken as Pullenti.Ner.TextToken).Term;
            string v2 = (p2.EndToken as Pullenti.Ner.TextToken).Term;
            if (v1[v1.Length - 1] == v2[v2.Length - 1]) 
                return false;
            return true;
        }
        static string GetValue(PersonItemToken.MorphPersonItem mt, Pullenti.Morph.MorphBaseInfo inf)
        {
            foreach (PersonItemToken.MorphPersonItemVariant wf in mt.Vars) 
            {
                if (inf != null) 
                {
                    if (!inf.Case.IsUndefined && !wf.Case.IsUndefined) 
                    {
                        if (((wf.Case & inf.Case)).IsUndefined) 
                            continue;
                    }
                    if (inf.Gender != Pullenti.Morph.MorphGender.Undefined && wf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        if (((inf.Gender & wf.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                            continue;
                    }
                }
                return wf.Value;
            }
            return mt.Term;
        }
        static void SetValue2(PersonMorphCollection res, PersonItemToken.MorphPersonItem mt, Pullenti.Morph.MorphBaseInfo inf)
        {
            bool ok = false;
            foreach (PersonItemToken.MorphPersonItemVariant wf in mt.Vars) 
            {
                if (inf != null) 
                {
                    if (!inf.Case.IsUndefined && !wf.Case.IsUndefined) 
                    {
                        if (((wf.Case & inf.Case)).IsUndefined) 
                            continue;
                    }
                    if (inf.Gender != Pullenti.Morph.MorphGender.Undefined && wf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                    {
                        if (((inf.Gender & wf.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                            continue;
                    }
                    ok = true;
                }
                res.Add(wf.Value, wf.ShortValue, wf.Gender, false);
            }
            if (res.Values.Count == 0) 
            {
                if ((inf != null && !inf.Case.IsUndefined && mt.Vars.Count > 0) && mt.IsLastnameHasStdTail) 
                {
                    foreach (PersonItemToken.MorphPersonItemVariant wf in mt.Vars) 
                    {
                        res.Add(wf.Value, wf.ShortValue, wf.Gender, false);
                    }
                }
                res.Add(mt.Term, null, inf.Gender, false);
            }
        }
        static void SetValue(PersonMorphCollection res, Pullenti.Ner.Token t, Pullenti.Morph.MorphBaseInfo inf)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null && (t is Pullenti.Ner.MetaToken) && (t as Pullenti.Ner.MetaToken).BeginToken == (t as Pullenti.Ner.MetaToken).EndToken) 
                tt = (t as Pullenti.Ner.MetaToken).BeginToken as Pullenti.Ner.TextToken;
            if (tt == null) 
                return;
            foreach (Pullenti.Morph.MorphBaseInfo wf in tt.Morph.Items) 
            {
                if (wf.Class.IsVerb) 
                    continue;
                if (wf.ContainsAttr("к.ф.", null)) 
                    continue;
                if (inf != null && inf.Gender != Pullenti.Morph.MorphGender.Undefined && wf.Gender != Pullenti.Morph.MorphGender.Undefined) 
                {
                    if (((wf.Gender & inf.Gender)) == Pullenti.Morph.MorphGender.Undefined) 
                        continue;
                }
                if (inf != null && !inf.Case.IsUndefined && !wf.Case.IsUndefined) 
                {
                    if (((wf.Case & inf.Case)).IsUndefined) 
                        continue;
                }
                string str = (t.Chars.IsLatinLetter ? tt.Term : (wf as Pullenti.Morph.MorphWordForm).NormalCase);
                res.Add(str, null, wf.Gender, false);
            }
            res.Add(tt.Term, null, (inf == null ? Pullenti.Morph.MorphGender.Undefined : inf.Gender), false);
        }
        public static bool CorrectXFML(List<PersonIdentityToken> pli0, List<PersonIdentityToken> pli1, List<PersonAttrToken> attrs)
        {
            PersonIdentityToken p0 = null;
            PersonIdentityToken p1 = null;
            foreach (PersonIdentityToken p in pli0) 
            {
                if (p.Typ == FioTemplateType.SurnameNameSecname) 
                {
                    p0 = p;
                    break;
                }
            }
            foreach (PersonIdentityToken p in pli1) 
            {
                if (p.Typ == FioTemplateType.NameSecnameSurname) 
                {
                    p1 = p;
                    break;
                }
            }
            if (p0 == null || p1 == null) 
            {
                foreach (PersonIdentityToken p in pli0) 
                {
                    if (p.Typ == FioTemplateType.SurnameName) 
                    {
                        p0 = p;
                        break;
                    }
                }
                foreach (PersonIdentityToken p in pli1) 
                {
                    if (p.Typ == FioTemplateType.NameSurname) 
                    {
                        p1 = p;
                        break;
                    }
                }
            }
            if (p0 == null || p1 == null) 
                return false;
            if (p1.Coef > p0.Coef) 
                return false;
            for (Pullenti.Ner.Token tt = p1.BeginToken; tt != p1.EndToken; tt = tt.Next) 
            {
                if (tt.IsNewlineAfter) 
                    return false;
            }
            if (!p1.EndToken.IsNewlineAfter) 
            {
                if (PersonItemToken.TryAttach(p1.EndToken.Next, null, PersonItemToken.ParseAttr.No, null) != null) 
                    return false;
            }
            if (p0.Lastname == null || p1.Lastname == null) 
                return false;
            if (p1.Lastname.HasLastnameStandardTail) 
            {
                if (!p0.Lastname.HasLastnameStandardTail) 
                {
                    p1.Coef = p0.Coef + ((float)0.1);
                    return true;
                }
            }
            if (attrs == null || attrs.Count == 0) 
            {
                if (!p1.Lastname.HasLastnameStandardTail && p0.Lastname.HasLastnameStandardTail) 
                    return false;
            }
            Pullenti.Ner.Token t = p1.EndToken.Next;
            if (t != null && !t.Chars.IsCapitalUpper && !t.Chars.IsAllUpper) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(p1.EndToken, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.EndToken != npt.BeginToken) 
                    return false;
                Pullenti.Morph.MorphClass cl1 = p0.BeginToken.GetMorphClassInDictionary();
                Pullenti.Morph.MorphClass cl2 = p1.EndToken.GetMorphClassInDictionary();
                if (cl2.IsNoun && !cl1.IsNoun) 
                    return false;
                p1.Coef = p0.Coef + ((float)0.1);
                return true;
            }
            return false;
        }
        public static PersonIdentityToken CheckLatinAfter(PersonIdentityToken pit)
        {
            if (pit == null) 
                return null;
            Pullenti.Ner.Token t = pit.EndToken.Next;
            if (t == null || !t.IsChar('(')) 
                return null;
            t = t.Next;
            PersonItemToken p1 = PersonItemToken.TryAttachLatin(t);
            if (p1 == null) 
                return null;
            PersonItemToken p2 = PersonItemToken.TryAttachLatin(p1.EndToken.Next);
            if (p2 == null) 
                return null;
            if (p2.EndToken.Next == null) 
                return null;
            PersonItemToken p3 = null;
            Pullenti.Ner.Token et = p2.EndToken.Next;
            if (p2.EndToken.Next.IsChar(')')) 
            {
            }
            else 
            {
                p3 = PersonItemToken.TryAttachLatin(et);
                if (p3 == null) 
                    return null;
                et = p3.EndToken.Next;
                if (et == null || !et.IsChar(')')) 
                    return null;
            }
            PersonItemToken sur = null;
            PersonItemToken nam = null;
            PersonItemToken sec = null;
            if (pit.Typ == FioTemplateType.NameSurname && pit.Firstname != null && pit.Lastname != null) 
            {
                int eq = 0;
                if (p1.Typ == PersonItemToken.ItemType.Value) 
                {
                    if (pit.Firstname.CheckLatinVariant(p1.Value)) 
                        eq++;
                    nam = p1;
                    if (p2.Typ == PersonItemToken.ItemType.Value && p3 == null) 
                    {
                        sur = p2;
                        if (pit.Lastname.CheckLatinVariant(p2.Value)) 
                            eq++;
                    }
                    else if (p2.Typ == PersonItemToken.ItemType.Initial && p3 != null) 
                    {
                        if (pit.Lastname.CheckLatinVariant(p3.Value)) 
                            eq++;
                        sur = p3;
                    }
                }
                if (eq == 0) 
                    return null;
            }
            else if ((pit.Typ == FioTemplateType.NameSecnameSurname && pit.Firstname != null && pit.Middlename != null) && pit.Lastname != null && p3 != null) 
            {
                int eq = 0;
                if (p1.Typ == PersonItemToken.ItemType.Value) 
                {
                    if (pit.Firstname.CheckLatinVariant(p1.Value)) 
                        eq++;
                    nam = p1;
                    if (p2.Typ == PersonItemToken.ItemType.Value) 
                    {
                        sec = p2;
                        if (pit.Middlename is PersonMorphCollection) 
                        {
                            if ((pit.Middlename as PersonMorphCollection).CheckLatinVariant(p2.Value)) 
                                eq++;
                        }
                    }
                    if (p3.Typ == PersonItemToken.ItemType.Value) 
                    {
                        sur = p3;
                        if (pit.Lastname.CheckLatinVariant(p3.Value)) 
                            eq++;
                    }
                }
                if (eq == 0) 
                    return null;
            }
            if (nam == null || sur == null) 
                return null;
            PersonIdentityToken res = new PersonIdentityToken(t, et) { Typ = pit.Typ };
            res.Lastname = new PersonMorphCollection();
            res.Lastname.Add(sur.Value, null, Pullenti.Morph.MorphGender.Undefined, false);
            res.Firstname = new PersonMorphCollection();
            res.Firstname.Add(nam.Value, null, Pullenti.Morph.MorphGender.Undefined, false);
            if (sec != null) 
            {
                res.Middlename = new PersonMorphCollection();
                (res.Middlename as PersonMorphCollection).Add(sec.Value, null, Pullenti.Morph.MorphGender.Undefined, false);
            }
            return res;
        }
    }
}