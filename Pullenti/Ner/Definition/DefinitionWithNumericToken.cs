/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Definition
{
    // Для поддержки выделений тезисов с числовыми данными
    public class DefinitionWithNumericToken : Pullenti.Ner.MetaToken
    {
        /// <summary>
        /// Значение
        /// </summary>
        public int Number;
        /// <summary>
        /// Начальная позиция числового значения в тексте
        /// </summary>
        public int NumberBeginChar;
        /// <summary>
        /// Конечная позиция числового значения в тексте
        /// </summary>
        public int NumberEndChar;
        /// <summary>
        /// Существительное (или именная группа) в единственном числе
        /// </summary>
        public string Noun;
        /// <summary>
        /// Это же существительное во множественном числе и родительном падеже
        /// </summary>
        public string NounsGenetive;
        /// <summary>
        /// Подстрока из Text, содержащая числовое значение вместе с существительным 
        /// (чтобы потом при формировании вопроса можно было бы её заменить на что-угодно)
        /// </summary>
        public string NumberSubstring;
        /// <summary>
        /// Текст тезиса
        /// </summary>
        public string Text;
        public override string ToString()
        {
            return string.Format("{0} {1} ({2})", Number, Noun ?? "?", NounsGenetive ?? "?");
        }
        public DefinitionWithNumericToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        /// <summary>
        /// Выделить определение с указанного токена
        /// </summary>
        /// <param name="t">токен</param>
        public static DefinitionWithNumericToken TryParse(Pullenti.Ner.Token t)
        {
            if (!Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(t)) 
                return null;
            Pullenti.Ner.Token tt = t;
            Pullenti.Ner.Core.NounPhraseToken noun = null;
            Pullenti.Ner.NumberToken num = null;
            for (; tt != null; tt = tt.Next) 
            {
                if (tt != t && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                    return null;
                if (!(tt is Pullenti.Ner.NumberToken)) 
                    continue;
                if (tt.WhitespacesAfterCount > 2 || tt == t) 
                    continue;
                if (tt.Morph.Class.IsAdjective) 
                    continue;
                Pullenti.Ner.Core.NounPhraseToken nn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (nn == null) 
                    continue;
                num = tt as Pullenti.Ner.NumberToken;
                noun = nn;
                break;
            }
            if (num == null || num.IntValue == null) 
                return null;
            DefinitionWithNumericToken res = new DefinitionWithNumericToken(t, noun.EndToken);
            res.Number = num.IntValue.Value;
            res.NumberBeginChar = num.BeginChar;
            res.NumberEndChar = num.EndChar;
            res.Noun = noun.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
            res.NounsGenetive = noun.GetMorphVariant(Pullenti.Morph.MorphCase.Genitive, true) ?? res.Noun;
            res.Text = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, num.Previous, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
            if (num.IsWhitespaceBefore) 
                res.Text += " ";
            res.NumberSubstring = Pullenti.Ner.Core.MiscHelper.GetTextValue(num, noun.EndToken, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
            res.Text += res.NumberSubstring;
            for (tt = noun.EndToken; tt != null; tt = tt.Next) 
            {
                if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                    break;
                res.EndToken = tt;
            }
            if (res.EndToken != noun.EndToken) 
            {
                if (noun.IsWhitespaceAfter) 
                    res.Text += " ";
                res.Text += Pullenti.Ner.Core.MiscHelper.GetTextValue(noun.EndToken.Next, res.EndToken, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
            }
            return res;
        }
    }
}