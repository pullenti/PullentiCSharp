/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree.Internal
{
    public class CanonicDecreeRefUri
    {
        public CanonicDecreeRefUri(string txt)
        {
            Text = txt;
        }
        public Pullenti.Ner.Referent Ref;
        public int BeginChar;
        public int EndChar;
        public bool IsDiap = false;
        /// <summary>
        /// Это есть ключ. слово "утв."
        /// </summary>
        public bool IsAdopted = false;
        /// <summary>
        /// Это Закон Челябинской области
        /// </summary>
        public string TypeWithGeo;
        public string Text;
        public override string ToString()
        {
            return (Text == null ? "?" : Text.Substring(BeginChar, (EndChar + 1) - BeginChar));
        }
    }
}