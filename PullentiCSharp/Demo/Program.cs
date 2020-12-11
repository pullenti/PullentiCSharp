/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Diagnostics;

namespace Demo
{
    class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            // инициализация - необходимо проводить один раз до обработки текстов
            Console.Write("Initializing SDK Pullenti ver {0} ({1}) ... ", Pullenti.Sdk.Version, Pullenti.Sdk.VersionDate);
            // инициализируются движок и все имеющиеся анализаторы
            Pullenti.Sdk.InitializeAll();
            sw.Stop();
            Console.WriteLine("OK (by {0} ms), version {1}", (int)sw.ElapsedMilliseconds, Pullenti.Ner.ProcessorService.Version);
            // посмотрим, какие анализаторы доступны
            foreach (Pullenti.Ner.Analyzer a in Pullenti.Ner.ProcessorService.Analyzers) 
            {
                Console.WriteLine("   {0} {1} \"{2}\"", (a.IsSpecific ? "Specific analyzer" : "Common analyzer"), a.Name, a.Caption);
            }
            // анализируемый текст
            string txt = "Система разрабатывается с 2011 года российским программистом Михаилом Жуковым, проживающим в Москве на Красной площади в доме номер один на втором этаже. Конкурентов у него много: Abbyy, Yandex, ООО \"Russian Context Optimizer\" (RCO) и другие компании. Он планирует продать SDK за 1.120.000.001,99 (миллиард сто двадцать миллионов один рубль 99 копеек) рублей, без НДС.";
            Console.WriteLine("Text: {0}", txt);
            // запускаем обработку на пустом процессоре (без анализаторов NER)
            Pullenti.Ner.AnalysisResult are = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
            Console.Write("Noun groups: ");
            // перебираем токены
            for (Pullenti.Ner.Token t = are.FirstToken; t != null; t = t.Next) 
            {
                // выделяем именную группу с текущего токена
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                // не получилось
                if (npt == null) 
                    continue;
                // получилось, выводим в нормализованном виде
                Console.Write("[{0}=>{1}] ", npt.GetSourceText(), npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false));
                // указатель на последний токен именной группы
                t = npt.EndToken;
            }
            using (Pullenti.Ner.Processor proc = Pullenti.Ner.ProcessorService.CreateProcessor()) 
            {
                // анализируем текст
                Pullenti.Ner.AnalysisResult ar = proc.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
                // результирующие сущности
                Console.WriteLine("\r\n==========================================\r\nEntities: ");
                foreach (Pullenti.Ner.Referent e in ar.Entities) 
                {
                    Console.WriteLine("{0}: {1}", e.TypeName, e.ToString());
                    foreach (Pullenti.Ner.Slot s in e.Slots) 
                    {
                        Console.WriteLine("   {0}: {1}", s.TypeName, s.Value);
                    }
                }
                // пример выделения именных групп
                Console.WriteLine("\r\n==========================================\r\nNoun groups: ");
                for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next) 
                {
                    // токены с сущностями игнорируем
                    if (t.GetReferent() != null) 
                        continue;
                    // пробуем создать именную группу
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.AdjectiveCanBeLast, 0, null);
                    // не получилось
                    if (npt == null) 
                        continue;
                    Console.WriteLine(npt);
                    // указатель перемещаем на последний токен группы
                    t = npt.EndToken;
                }
            }
            using (Pullenti.Ner.Processor proc = Pullenti.Ner.ProcessorService.CreateSpecificProcessor(Pullenti.Ner.Keyword.KeywordAnalyzer.ANALYZER_NAME)) 
            {
                Pullenti.Ner.AnalysisResult ar = proc.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
                Console.WriteLine("\r\n==========================================\r\nKeywords1: ");
                foreach (Pullenti.Ner.Referent e in ar.Entities) 
                {
                    if (e is Pullenti.Ner.Keyword.KeywordReferent) 
                        Console.WriteLine(e);
                }
                Console.WriteLine("\r\n==========================================\r\nKeywords2: ");
                for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next) 
                {
                    if (t is Pullenti.Ner.ReferentToken) 
                    {
                        Pullenti.Ner.Keyword.KeywordReferent kw = t.GetReferent() as Pullenti.Ner.Keyword.KeywordReferent;
                        if (kw == null) 
                            continue;
                        string kwstr = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(t as Pullenti.Ner.ReferentToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        Console.WriteLine("{0} = {1}", kwstr, kw);
                    }
                }
            }
            Console.WriteLine("Over!");
        }
    }
}