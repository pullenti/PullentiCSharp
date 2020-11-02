/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Метатокен - представление последовательности, обрамлённой кавычками (скобками)
    /// </summary>
    public class BracketSequenceToken : Pullenti.Ner.MetaToken
    {
        public BracketSequenceToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        /// <summary>
        /// Внутренние подпоследовательности - список BracketSequenceToken. 
        /// Например, "О внесении изменений (2010-2011)", содержит внутри (2010-2011)
        /// </summary>
        public List<BracketSequenceToken> Internal = new List<BracketSequenceToken>();
        /// <summary>
        /// Признак обрамления кавычками (если false, то м.б. [...], (...), {...})
        /// </summary>
        public bool IsQuoteType
        {
            get
            {
                return "{([".IndexOf(OpenChar) < 0;
            }
        }
        /// <summary>
        /// Открывающий символ
        /// </summary>
        public char OpenChar
        {
            get
            {
                return BeginToken.Kit.GetTextCharacter(BeginToken.BeginChar);
            }
        }
        /// <summary>
        /// Закрывающий символ
        /// </summary>
        public char CloseChar
        {
            get
            {
                return EndToken.Kit.GetTextCharacter(EndToken.BeginChar);
            }
        }
        public override string ToString()
        {
            return base.ToString();
        }
        public override string GetNormalCaseText(Pullenti.Morph.MorphClass mc = null, Pullenti.Morph.MorphNumber num = Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender gender = Pullenti.Morph.MorphGender.Undefined, bool keepChars = false)
        {
            GetTextAttr attr = GetTextAttr.No;
            if (num == Pullenti.Morph.MorphNumber.Singular) 
                attr |= GetTextAttr.FirstNounGroupToNominativeSingle;
            else 
                attr |= GetTextAttr.FirstNounGroupToNominative;
            if (keepChars) 
                attr |= GetTextAttr.KeepRegister;
            return MiscHelper.GetTextValue(BeginToken, EndToken, attr);
        }
    }
}