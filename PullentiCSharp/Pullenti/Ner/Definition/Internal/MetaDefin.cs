/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Definition.Internal
{
    class MetaDefin : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDefin();
            GlobalMeta.AddFeature(Pullenti.Ner.Definition.DefinitionReferent.ATTR_TERMIN, "Термин", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Definition.DefinitionReferent.ATTR_TERMIN_ADD, "Дополнение термина", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Definition.DefinitionReferent.ATTR_VALUE, "Значение", 1, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Definition.DefinitionReferent.ATTR_MISC, "Мелочь", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Definition.DefinitionReferent.ATTR_DECREE, "Ссылка на НПА", 0, 0);
            Pullenti.Ner.Metadata.Feature fi = GlobalMeta.AddFeature(Pullenti.Ner.Definition.DefinitionReferent.ATTR_KIND, "Тип", 1, 1);
            fi.AddValue(Pullenti.Ner.Definition.DefinitionKind.Assertation.ToString(), "Утверждение", null, null);
            fi.AddValue(Pullenti.Ner.Definition.DefinitionKind.Definition.ToString(), "Определение", null, null);
            fi.AddValue(Pullenti.Ner.Definition.DefinitionKind.Negation.ToString(), "Отрицание", null, null);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Definition.DefinitionReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Тезис";
            }
        }
        public static string ImageDefId = "defin";
        public static string ImageAssId = "assert";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            if (obj is Pullenti.Ner.Definition.DefinitionReferent) 
            {
                Pullenti.Ner.Definition.DefinitionKind ki = (obj as Pullenti.Ner.Definition.DefinitionReferent).Kind;
                if (ki == Pullenti.Ner.Definition.DefinitionKind.Definition) 
                    return ImageDefId;
            }
            return ImageAssId;
        }
        internal static MetaDefin GlobalMeta;
    }
}