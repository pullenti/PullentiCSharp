/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Вариант расщепления именной группы, у которой слиплись существительные. 
    /// Получается методом GetMultivars() у NounPhraseToken, у которой MultiNouns = true.
    /// </summary>
    public class NounPhraseMultivarToken : Pullenti.Ner.MetaToken
    {
        public NounPhraseMultivarToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        /// <summary>
        /// Исходная именная группа
        /// </summary>
        public NounPhraseToken Source;
        /// <summary>
        /// Начальный индекс прилагательных (из списка Adjectives у Source), который относится к расщеплённой группе
        /// </summary>
        public int AdjIndex1;
        /// <summary>
        /// Конечный индекс прилагательных (из списка Adjectives у Source), который относится к расщеплённой группе
        /// </summary>
        public int AdjIndex2;
        public override string ToString()
        {
            return string.Format("{0} {1}", Source.Adjectives[AdjIndex1], Source.Noun);
        }
        public override string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            if (gender == Pullenti.Morph.MorphGender.Undefined) 
                gender = Source.Morph.Gender;
            StringBuilder res = new StringBuilder();
            for (int k = AdjIndex1; k <= AdjIndex2; k++) 
            {
                string adj = Source.Adjectives[k].GetNormalCaseText(Pullenti.Morph.MorphClass.Adjective | Pullenti.Morph.MorphClass.Pronoun, num, gender, keepChars);
                if (adj == null || adj == "?") 
                    adj = MiscHelper.GetTextValueOfMetaToken(Source.Adjectives[k], (keepChars ? GetTextAttr.KeepRegister : GetTextAttr.No));
                res.AppendFormat("{0} ", adj ?? "?");
            }
            string noun = null;
            if ((Source.Noun.BeginToken is Pullenti.Ner.ReferentToken) && Source.BeginToken == Source.Noun.EndToken) 
                noun = Source.Noun.BeginToken.GetNormalCaseText(null, num, gender, keepChars);
            else 
            {
                Pullenti.Morph.MorphClass cas = Pullenti.Morph.MorphClass.Noun | Pullenti.Morph.MorphClass.Pronoun;
                if (mc != null && !mc.IsUndefined) 
                    cas = mc;
                noun = Source.Noun.GetNormalCaseText(cas, num, gender, keepChars);
            }
            if (noun == null || noun == "?") 
                noun = Source.Noun.GetNormalCaseText(null, num, Pullenti.Morph.MorphGender.Undefined, false);
            res.Append(noun ?? "?");
            return res.ToString();
        }
    }
}