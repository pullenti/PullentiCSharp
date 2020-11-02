/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Metadata
{
    /// <summary>
    /// Приходится работать через обёртку, так как ориентируемся на все платформы и языки
    /// </summary>
    public class ImageWrapper
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public string Id;
        /// <summary>
        /// Байтовый поток иконки
        /// </summary>
        public byte[] Content;
        /// <summary>
        /// А здесь Bitmap вы уж сами формируйте, если нужно
        /// </summary>
        public object Image;
    }
}