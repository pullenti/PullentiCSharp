/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Метатокен - предлог (они могут быть из нескольких токенов, например, 
    /// "несмотря на", "в соответствии с"). 
    /// Создаётся методом PrepositionHelper.TryParse(t).
    /// </summary>
    public class PrepositionToken : Pullenti.Ner.MetaToken
    {
        public PrepositionToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        /// <summary>
        /// Нормализованное значение (ПОДО -> ПОД,  ОБО, ОБ -> О ...)
        /// </summary>
        public string Normal;
        /// <summary>
        /// Падежи для слов, используемых с предлогом
        /// </summary>
        public Pullenti.Morph.MorphCase NextCase;
        public override string ToString()
        {
            return Normal;
        }
        public override string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            string res = Normal;
            if (keepChars) 
            {
                if (Chars.IsAllLower) 
                    res = res.ToLower();
                else if (Chars.IsAllUpper) 
                {
                }
                else if (Chars.IsCapitalUpper) 
                    res = MiscHelper.ConvertFirstCharUpperAndOtherLower(res);
            }
            return res;
        }
    }
}