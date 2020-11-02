/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Semantic
{
    /// <summary>
    /// Сервис семантического анализа. Основная концепция изложена в документе Pullenti.Semantic. 
    /// В реальных проектах не использовался, слабо отлажен, но ведется доработка данной функциональности.
    /// </summary>
    public static class SemanticService
    {
        /// <summary>
        /// Версия семантики
        /// </summary>
        public static string Version = "0.2";
        /// <summary>
        /// Если собираетесь использовать семантику, то необходимо вызывать в самом начале и только один раз 
        /// (после инициализации ProcessorService.Initialize). Если вызвали Sdk.Initialize(), то там семантика инициализуется, 
        /// и эту функцию вызывать уже не надо. 
        /// Отметим, что для NER семантический анализ не используется.
        /// </summary>
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Semantic.Internal.DelimToken.Initialize();
            Pullenti.Semantic.Internal.AdverbToken.Initialize();
            Pullenti.Ner.Measure.MeasureAnalyzer.Initialize();
            Pullenti.Ner.Money.MoneyAnalyzer.Initialize();
        }
        static bool m_Inited;
        /// <summary>
        /// Сделать семантический анализ поверх результатов морфологического анализа и NER
        /// </summary>
        /// <param name="ar">результат обработки Processor</param>
        /// <param name="pars">дополнительные параметры</param>
        /// <return>результат анализа текста</return>
        public static SemDocument Process(Pullenti.Ner.AnalysisResult ar, SemProcessParams pars = null)
        {
            return Pullenti.Semantic.Internal.AnalyzeHelper.Process(ar, pars ?? new SemProcessParams());
        }
        public static Pullenti.Semantic.Internal.AlgoParams Params = new Pullenti.Semantic.Internal.AlgoParams();
    }
}