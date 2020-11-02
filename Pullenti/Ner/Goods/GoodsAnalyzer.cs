/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Goods
{
    /// <summary>
    /// Анализатор названий товаров (номенклатур) и выделение из них характеристик. 
    /// Специфический анализатор, то есть нужно явно создавать процессор через функцию CreateSpecificProcessor, 
    /// указав имя анализатора. 
    /// Выделение происходит из небольшого фрагмента текста, содержащего только один товар и его характеристики. 
    /// Выделять из большого текста такие фрагменты - это не задача анализатора. 
    /// Примеры текстов: "Плед OXFORD Cashnere Touch Herringbone 1.5-сп с эффектом кашемира", 
    /// "Имплантат размером 5 см х 5 см предназначен для реконструкции твердой мозговой оболочки. Изготовлен из биологически совместимого материала на коллагеновой основе.".
    /// </summary>
    public class GoodsAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("GOODS")
        /// </summary>
        public const string ANALYZER_NAME = "GOODS";
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
                return "Товары и атрибуты";
            }
        }
        public override string Description
        {
            get
            {
                return "Товары и их атрибуты";
            }
        }
        /// <summary>
        /// Этот анализатор является специфическим (IsSpecific = true)
        /// </summary>
        public override bool IsSpecific
        {
            get
            {
                return true;
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new GoodsAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Goods.Internal.AttrMeta.GlobalMeta, Pullenti.Ner.Goods.Internal.GoodMeta.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Goods.Internal.AttrMeta.AttrImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("bullet_ball_glass_grey.png"));
                res.Add(Pullenti.Ner.Goods.Internal.GoodMeta.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("shoppingcart.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == GoodAttributeReferent.OBJ_TYPENAME) 
                return new GoodAttributeReferent();
            if (type == GoodReferent.OBJ_TYPENAME) 
                return new GoodReferent();
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
            List<GoodReferent> goods = new List<GoodReferent>();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (!t.IsNewlineBefore) 
                    continue;
                if (t.BeginChar > nextPos) 
                {
                    nextPos += delta;
                    cur++;
                    if (!this.OnProgress(cur, parts, kit)) 
                        break;
                }
                if (!t.Chars.IsLetter && t.Next != null) 
                    t = t.Next;
                List<Pullenti.Ner.ReferentToken> rts = Pullenti.Ner.Goods.Internal.GoodAttrToken.TryParseList(t);
                if (rts == null || rts.Count == 0) 
                    continue;
                GoodReferent good = new GoodReferent();
                foreach (Pullenti.Ner.ReferentToken rt in rts) 
                {
                    rt.Referent = ad.RegisterReferent(rt.Referent);
                    if (good.FindSlot(GoodReferent.ATTR_ATTR, rt.Referent, true) == null) 
                        good.AddSlot(GoodReferent.ATTR_ATTR, rt.Referent, false, 0);
                    kit.EmbedToken(rt);
                }
                goods.Add(good);
                Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(good, rts[0], rts[rts.Count - 1]);
                kit.EmbedToken(rt0);
                t = rt0;
            }
            foreach (GoodReferent g in goods) 
            {
                ad.Referents.Add(g);
            }
        }
        public override Pullenti.Ner.ReferentToken ProcessOntologyItem(Pullenti.Ner.Token begin)
        {
            if (begin == null) 
                return null;
            GoodAttributeReferent ga = new GoodAttributeReferent();
            if (begin.Chars.IsLatinLetter) 
            {
                if (begin.IsValue("KEYWORD", null)) 
                {
                    ga.Typ = GoodAttrType.Keyword;
                    begin = begin.Next;
                }
                else if (begin.IsValue("CHARACTER", null)) 
                {
                    ga.Typ = GoodAttrType.Character;
                    begin = begin.Next;
                }
                else if (begin.IsValue("PROPER", null)) 
                {
                    ga.Typ = GoodAttrType.Proper;
                    begin = begin.Next;
                }
                else if (begin.IsValue("MODEL", null)) 
                {
                    ga.Typ = GoodAttrType.Model;
                    begin = begin.Next;
                }
                if (begin == null) 
                    return null;
            }
            Pullenti.Ner.ReferentToken res = new Pullenti.Ner.ReferentToken(ga, begin, begin);
            for (Pullenti.Ner.Token t = begin; t != null; t = t.Next) 
            {
                if (t.IsChar(';')) 
                {
                    ga.AddSlot(GoodAttributeReferent.ATTR_VALUE, Pullenti.Ner.Core.MiscHelper.GetTextValue(begin, t.Previous, Pullenti.Ner.Core.GetTextAttr.No), false, 0);
                    begin = t.Next;
                    continue;
                }
                res.EndToken = t;
            }
            if (res.EndChar > begin.BeginChar) 
                ga.AddSlot(GoodAttributeReferent.ATTR_VALUE, Pullenti.Ner.Core.MiscHelper.GetTextValue(begin, res.EndToken, Pullenti.Ner.Core.GetTextAttr.No), false, 0);
            if (ga.Typ == GoodAttrType.Undefined) 
            {
                if (!begin.Chars.IsAllLower) 
                    ga.Typ = GoodAttrType.Proper;
            }
            return res;
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
                Pullenti.Ner.Goods.Internal.AttrMeta.Initialize();
                Pullenti.Ner.Goods.Internal.GoodMeta.Initialize();
                try 
                {
                    Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
                    Pullenti.Ner.Goods.Internal.GoodAttrToken.Initialize();
                    Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
                }
                catch(Exception ex) 
                {
                    throw new Exception(ex.Message, ex);
                }
                Pullenti.Ner.ProcessorService.RegisterAnalyzer(new GoodsAnalyzer());
            }
        }
    }
}