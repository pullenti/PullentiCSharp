/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Метатокен - представление союзов и других служебных слов. Они могут быть из нескольких токенов, например, "из-за того что". 
    /// Получить можно с помощью ConjunctionHelper.TryParse(t)
    /// </summary>
    public class ConjunctionToken : Pullenti.Ner.MetaToken
    {
        public ConjunctionToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        /// <summary>
        /// Нормализованное значение
        /// </summary>
        public string Normal;
        /// <summary>
        /// Возможный тип союза
        /// </summary>
        public ConjunctionType Typ;
        /// <summary>
        /// Это когда союз простой (запятая, и, ...), а не такой "а также", "также, как и"
        /// </summary>
        public bool IsSimple;
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