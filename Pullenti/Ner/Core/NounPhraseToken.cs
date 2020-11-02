/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Метатокен - именная группа (это существительное с возможными прилагательными, морфологичски согласованными). 
    /// Выделяется методом TryParse() класса NounPhraseHelper.
    /// </summary>
    public class NounPhraseToken : Pullenti.Ner.MetaToken
    {
        public NounPhraseToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        /// <summary>
        /// Корень группы (существительное, местоимение или сущность)
        /// </summary>
        public Pullenti.Ner.MetaToken Noun;
        /// <summary>
        /// Прилагательные (и причастия, если задан атрибут NounPhraseParseAttr.ParseVerbs)
        /// </summary>
        public List<Pullenti.Ner.MetaToken> Adjectives = new List<Pullenti.Ner.MetaToken>();
        /// <summary>
        /// Наречия (если задан атрибут NounPhraseParseAttr.ParseAdverbs при выделении)
        /// </summary>
        public List<Pullenti.Ner.TextToken> Adverbs = null;
        /// <summary>
        /// Внутренняя именная группа. Например, для случая "по современным на данный момент представлениям" 
        /// это будет "данный момент"
        /// </summary>
        public NounPhraseToken InternalNoun;
        /// <summary>
        /// Токен с анафорической ссылкой-местоимением (если есть), например: старшего своего брата
        /// </summary>
        public Pullenti.Ner.Token Anafor;
        // Используется внешней системой для нахождения анафоры
        public NounPhraseToken AnaforaRef;
        /// <summary>
        /// Начальный предлог (если задан атрибут NounPhraseParseAttr.ParsePreposition)
        /// </summary>
        public PrepositionToken Preposition;
        /// <summary>
        /// Это когда Noun как бы слепленный для разных прилагательных (грузовой и легковой автомобили)
        /// </summary>
        public bool MultiNouns;
        /// <summary>
        /// Это если MultiNouns = true, то можно как бы расщепить на варианты 
        /// (грузовой и легковой автомобили -> грузовой автомобиль и легковой автомобиль)
        /// </summary>
        /// <return>список NounPhraseMultivarToken</return>
        public List<NounPhraseMultivarToken> GetMultivars()
        {
            List<NounPhraseMultivarToken> res = new List<NounPhraseMultivarToken>();
            for (int i = 0; i < Adjectives.Count; i++) 
            {
                NounPhraseMultivarToken v = new NounPhraseMultivarToken(Adjectives[i].BeginToken, Adjectives[i].EndToken) { Source = this, AdjIndex1 = i, AdjIndex2 = i };
                for (; i < (Adjectives.Count - 1); i++) 
                {
                    if (Adjectives[i + 1].BeginToken == Adjectives[i].EndToken.Next) 
                    {
                        v.EndToken = Adjectives[i + 1].EndToken;
                        v.AdjIndex2 = i + 1;
                    }
                    else 
                        break;
                }
                if (i == (Adjectives.Count - 1)) 
                    v.EndToken = EndToken;
                res.Add(v);
            }
            return res;
        }
        public override string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            StringBuilder res = new StringBuilder();
            if (gender == Pullenti.Morph.MorphGender.Undefined) 
                gender = Morph.Gender;
            if (Adverbs != null && Adverbs.Count > 0) 
            {
                int i = 0;
                if (Adjectives.Count > 0) 
                {
                    for (int j = 0; j < Adjectives.Count; j++) 
                    {
                        for (; i < Adverbs.Count; i++) 
                        {
                            if (Adverbs[i].BeginChar < Adjectives[j].BeginChar) 
                                res.AppendFormat("{0} ", Adverbs[i].GetNormalCaseText(Pullenti.Morph.MorphClass.Adverb, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                            else 
                                break;
                        }
                        string s = Adjectives[j].GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Pronoun, num, gender, keepChars);
                        res.AppendFormat("{0} ", s ?? "?");
                    }
                }
                for (; i < Adverbs.Count; i++) 
                {
                    res.AppendFormat("{0} ", Adverbs[i].GetNormalCaseText(Pullenti.Morph.MorphClass.Adverb, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                }
            }
            else 
                foreach (Pullenti.Ner.MetaToken t in Adjectives) 
                {
                    string s = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Pronoun, num, gender, keepChars);
                    res.AppendFormat("{0} ", s ?? "?");
                }
            string r = null;
            if ((Noun.BeginToken is Pullenti.Ner.ReferentToken) && Noun.BeginToken == Noun.EndToken) 
                r = Noun.BeginToken.GetNormalCaseText(null, num, gender, keepChars);
            else 
            {
                Pullenti.Morph.MorphClass cas = Pullenti.Morph.MorphClass.Noun | Pullenti.Morph.MorphClass.Pronoun;
                if (mc != null && !mc.IsUndefined) 
                    cas = mc;
                r = Noun.GetNormalCaseText(cas, num, gender, keepChars);
            }
            if (r == null || r == "?") 
                r = Noun.GetNormalCaseText(null, num, Pullenti.Morph.MorphGender.Undefined, false);
            res.Append(r ?? Noun.ToString());
            return res.ToString();
        }
        public string GetNormalCaseTextWithoutAdjective(int adjIndex)
        {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < Adjectives.Count; i++) 
            {
                if (i != adjIndex) 
                {
                    string s = Adjectives[i].GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Pronoun, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    res.AppendFormat("{0} ", s ?? "?");
                }
            }
            string r = Noun.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun | Pullenti.Morph.MorphClass.Pronoun, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
            if (r == null) 
                r = Noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
            res.Append(r ?? Noun.ToString());
            return res.ToString();
        }
        /// <summary>
        /// Сгенерировать текст именной группы в нужном падеже и числе
        /// </summary>
        /// <param name="cas">нужный падеж</param>
        /// <param name="plural">нужное число</param>
        /// <return>результирующая строка</return>
        public string GetMorphVariant(Pullenti.Morph.MorphCase cas, bool plural)
        {
            Pullenti.Morph.MorphBaseInfo mi = new Pullenti.Morph.MorphBaseInfo() { Case = cas, Language = Pullenti.Morph.MorphLang.RU };
            if (plural) 
                mi.Number = Pullenti.Morph.MorphNumber.Plural;
            else 
                mi.Number = Pullenti.Morph.MorphNumber.Singular;
            string res = null;
            foreach (Pullenti.Ner.MetaToken a in Adjectives) 
            {
                string tt = MiscHelper.GetTextValueOfMetaToken(a, GetTextAttr.No);
                if (a.BeginToken != a.EndToken || !(a.BeginToken is Pullenti.Ner.TextToken)) 
                {
                }
                else 
                {
                    string tt2 = Pullenti.Morph.MorphologyService.GetWordform(tt, mi);
                    if (tt2 != null) 
                        tt = tt2;
                }
                if (res == null) 
                    res = tt;
                else 
                    res = string.Format("{0} {1}", res, tt);
            }
            if (Noun != null) 
            {
                string tt = MiscHelper.GetTextValueOfMetaToken(Noun, GetTextAttr.No);
                if (Noun.BeginToken != Noun.EndToken || !(Noun.BeginToken is Pullenti.Ner.TextToken)) 
                {
                }
                else 
                {
                    string tt2 = Pullenti.Morph.MorphologyService.GetWordform(tt, mi);
                    if (tt2 != null) 
                        tt = tt2;
                }
                if (res == null) 
                    res = tt;
                else 
                    res = string.Format("{0} {1}", res, tt);
            }
            return res;
        }
        public override string ToString()
        {
            if (InternalNoun == null) 
                return string.Format("{0} {1}", this.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) ?? "?", Morph.ToString());
            else 
                return string.Format("{0} {1} / {2}", this.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false) ?? "?", Morph.ToString(), InternalNoun.ToString());
        }
        public void RemoveLastNounWord()
        {
            if (Noun != null) 
            {
                foreach (Pullenti.Morph.MorphBaseInfo it in Noun.Morph.Items) 
                {
                    Pullenti.Ner.Core.Internal.NounPhraseItemTextVar ii = it as Pullenti.Ner.Core.Internal.NounPhraseItemTextVar;
                    if (ii == null || ii.NormalValue == null) 
                        continue;
                    int j = ii.NormalValue.IndexOf('-');
                    if (j > 0) 
                        ii.NormalValue = ii.NormalValue.Substring(0, j);
                    if (ii.SingleNumberValue != null) 
                    {
                        if ((((j = ii.SingleNumberValue.IndexOf('-')))) > 0) 
                            ii.SingleNumberValue = ii.SingleNumberValue.Substring(0, j);
                    }
                }
            }
        }
    }
}