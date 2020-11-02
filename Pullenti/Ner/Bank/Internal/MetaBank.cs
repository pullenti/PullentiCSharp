/*
 * Copyright (c) 2013, Pullenti. All rights reserved. Non-Commercial Freeware.
 * This class is generated using the converter UniSharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Bank.Internal
{
    class MetaBank : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaBank();
            GlobalMeta.AddFeature(Pullenti.Ner.Bank.BankDataReferent.ATTR_ITEM, "Элемент", 0, 0).ShowAsParent = true;
            GlobalMeta.AddFeature(Pullenti.Ner.Bank.BankDataReferent.ATTR_BANK, "Банк", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Bank.BankDataReferent.ATTR_CORBANK, "Банк К/С", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Bank.BankDataReferent.ATTR_MISC, "Разное", 0, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Bank.BankDataReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Банковские реквизиты";
            }
        }
        public static string ImageId = "bankreq";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        internal static MetaBank GlobalMeta;
    }
}