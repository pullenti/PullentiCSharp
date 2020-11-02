/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Выделение именных групп - это существительное с согласованными прилагательными (если они есть).
    /// </summary>
    public class NounPhraseHelper
    {
        /// <summary>
        /// Попробовать создать именную группу с указанного токена
        /// </summary>
        /// <param name="t">начальный токен</param>
        /// <param name="attrs">атрибуты (можно битовую маску)</param>
        /// <param name="maxCharPos">максимальная позиция в тексте, до которой выделять (если 0, то без ограничений)</param>
        /// <param name="noun">это если нужно выделить только прилагательные для ранее выделенного существительного (из другой группы)</param>
        /// <return>именная группа или null</return>
        public static NounPhraseToken TryParse(Pullenti.Ner.Token t, NounPhraseParseAttr attrs = NounPhraseParseAttr.No, int maxCharPos = 0, Pullenti.Ner.MetaToken noun = null)
        {
            NounPhraseToken res = _NounPraseHelperInt.TryParse(t, attrs, maxCharPos, noun as Pullenti.Ner.Core.Internal.NounPhraseItem);
            if (res != null) 
            {
                if (((attrs & NounPhraseParseAttr.ParsePreposition)) != NounPhraseParseAttr.No) 
                {
                    if (res.BeginToken == res.EndToken && t.Morph.Class.IsPreposition) 
                    {
                        PrepositionToken prep = PrepositionHelper.TryParse(t);
                        if (prep != null) 
                        {
                            NounPhraseToken res2 = _NounPraseHelperInt.TryParse(t.Next, attrs, maxCharPos, noun as Pullenti.Ner.Core.Internal.NounPhraseItem);
                            if (res2 != null) 
                            {
                                if (!((prep.NextCase & res2.Morph.Case)).IsUndefined) 
                                {
                                    res2.Morph.RemoveItems(prep.NextCase);
                                    res2.Preposition = prep;
                                    res2.BeginToken = t;
                                    return res2;
                                }
                            }
                        }
                    }
                }
                return res;
            }
            if (((attrs & NounPhraseParseAttr.ParsePreposition)) != NounPhraseParseAttr.No) 
            {
                PrepositionToken prep = PrepositionHelper.TryParse(t);
                if (prep != null && (prep.NewlinesAfterCount < 2)) 
                {
                    res = _NounPraseHelperInt.TryParse(prep.EndToken.Next, attrs, maxCharPos, noun as Pullenti.Ner.Core.Internal.NounPhraseItem);
                    if (res != null) 
                    {
                        res.Preposition = prep;
                        res.BeginToken = t;
                        if (!((prep.NextCase & res.Morph.Case)).IsUndefined) 
                            res.Morph.RemoveItems(prep.NextCase);
                        else if (t.Morph.Class.IsAdverb) 
                            return null;
                        return res;
                    }
                }
            }
            return null;
        }
    }
}