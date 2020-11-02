/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree.Internal
{
    class MetaDecreeChange : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDecreeChange();
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, "Структурный элемент", 1, 0);
            Pullenti.Ner.Metadata.Feature fi = GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_KIND, "Тип", 1, 1);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeKind.Append.ToString(), "Дополнить", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeKind.Expire.ToString(), "Утратить силу", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeKind.New.ToString(), "В редакции", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeKind.Exchange.ToString(), "Заменить", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeKind.Remove.ToString(), "Исключить", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeKind.Consider.ToString(), "Считать", null, null);
            fi.AddValue(Pullenti.Ner.Decree.DecreeChangeKind.Container.ToString(), "Внести изменение", null, null);
            KindFeature = fi;
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_CHILD, "Дочернее изменение", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_VALUE, "Значение", 0, 1).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_PARAM, "Параметр", 0, 1).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_MISC, "Разное", 0, 0);
        }
        public static Pullenti.Ner.Metadata.Feature KindFeature;
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Decree.DecreeChangeReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Изменение СЭ НПА";
            }
        }
        public static string ImageId = "decreechange";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static MetaDecreeChange GlobalMeta;
    }
}