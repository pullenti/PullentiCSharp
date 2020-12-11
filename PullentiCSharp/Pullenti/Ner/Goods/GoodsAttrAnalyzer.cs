/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Goods
{
    /// <summary>
    /// Анализатор характеристик товаров. Используется, если нужно выделятть только отдельные характеристики, а не товар в целом. 
    /// Если примеряется GoodsAnalyzer, то данный анализатор задействовать не нужно. 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor, 
    /// указав имя анализатора. 
    /// Выделение происходит из небольшого фрагмента текста, содержащего только характеристики. 
    /// Выделять из большого текста такие фрагменты - это не задача анализатора.
    /// </summary>
    public class GoodsAttrAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("GOODSATTR")
        /// </summary>
        public const string ANALYZER_NAME = "GOODSATTR";
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Атрибуты товара";
            }
        }
        public override string Description
        {
            get
            {
                return "Выделяет только атрибуты (из раздела Характеристик)";
            }
        }
        public override bool IsSpecific
        {
            get
            {
                return true;
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new GoodsAttrAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Goods.Internal.AttrMeta.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Goods.Internal.AttrMeta.AttrImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("bullet_ball_glass_grey.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == GoodAttributeReferent.OBJ_TYPENAME) 
                return new GoodAttributeReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 100;
            }
        }
        public override Pullenti.Ner.Core.AnalyzerData CreateAnalyzerData()
        {
            return new Pullenti.Ner.Core.AnalyzerDataWithOntology();
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            int delta = 100000;
            int parts = (((kit.Sofa.Text.Length + delta) - 1)) / delta;
            if (parts == 0) 
                parts = 1;
            int cur = 0;
            int nextPos = 0;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (t.BeginChar > nextPos) 
                {
                    nextPos += delta;
                    cur++;
                    if (!this.OnProgress(cur, parts, kit)) 
                        break;
                }
                Pullenti.Ner.Goods.Internal.GoodAttrToken at = Pullenti.Ner.Goods.Internal.GoodAttrToken.TryParse(t, null, true, true);
                if (at == null) 
                    continue;
                GoodAttributeReferent attr = at._createAttr();
                if (attr == null) 
                {
                    t = at.EndToken;
                    continue;
                }
                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(attr, at.BeginToken, at.EndToken);
                rt.Referent = ad.RegisterReferent(attr);
                kit.EmbedToken(rt);
                t = rt;
            }
        }
        static bool m_Initialized = false;
        static object m_Lock = new object();
        public static void Initialize()
        {
            lock (m_Lock) 
            {
                if (m_Initialized) 
                    return;
                m_Initialized = true;
                Pullenti.Ner.ProcessorService.RegisterAnalyzer(new GoodsAttrAnalyzer());
            }
        }
    }
}