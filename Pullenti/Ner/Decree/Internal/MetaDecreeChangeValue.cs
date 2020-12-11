/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree.Internal
{
    class MetaDecreeChangeValue : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDecreeChangeValue();
            Pullenti.Ner.Metadata.Feature fi = GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeValueReferent.ATTR_KIND, "Тип", 1, 1);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeValueKind.Text.ToString(), "Текст", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeValueKind.Words.ToString(), "Слова", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords.ToString(), "Слова (неточно)", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeValueKind.Numbers.ToString(), "Цифры", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeValueKind.Sequence.ToString(), "Предложение", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeValueKind.Footnote.ToString(), "Сноска", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeValueKind.Block.ToString(), "Блок", null, null);
            KindFeature = fi;
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeValueReferent.ATTR_VALUE, "Значение", 1, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeValueReferent.ATTR_NUMBER, "Номер", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeValueReferent.ATTR_NEWITEM, "Новый структурный элемент", 0, 0);
        }
        public static Pullenti.Ner.Metadata.Feature KindFeature;
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Decree.DecreeChangeValueReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Значение изменения СЭ НПА";
            }
        }
        public static string ImageId = "decreechangevalue";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static MetaDecreeChangeValue GlobalMeta;
    }
}