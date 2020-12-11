/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Address.Internal
{
    class MetaAddress : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaAddress();
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_STREET, "Улица", 0, 2);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_HOUSE, "Дом", 0, 1);
            GlobalMeta.HouseTypeFeature = GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_HOUSETYPE, "Тип дома", 0, 1);
            GlobalMeta.HouseTypeFeature.AddValue(Pullenti.Ner.Address.AddressHouseType.Estate.ToString(), "Владение", null, null);
            GlobalMeta.HouseTypeFeature.AddValue(Pullenti.Ner.Address.AddressHouseType.House.ToString(), "Дом", null, null);
            GlobalMeta.HouseTypeFeature.AddValue(Pullenti.Ner.Address.AddressHouseType.HouseEstate.ToString(), "Домовладение", null, null);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_BUILDING, "Строение", 0, 1);
            GlobalMeta.BuildingTypeFeature = GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_BUILDINGTYPE, "Тип строения", 0, 1);
            GlobalMeta.BuildingTypeFeature.AddValue(Pullenti.Ner.Address.AddressBuildingType.Building.ToString(), "Строение", null, null);
            GlobalMeta.BuildingTypeFeature.AddValue(Pullenti.Ner.Address.AddressBuildingType.Construction.ToString(), "Сооружение", null, null);
            GlobalMeta.BuildingTypeFeature.AddValue(Pullenti.Ner.Address.AddressBuildingType.Liter.ToString(), "Литера", null, null);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_CORPUS, "Корпус", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_PORCH, "Подъезд", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_FLOOR, "Этаж", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_FLAT, "Квартира", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_CORPUSORFLAT, "Корпус или квартира", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_OFFICE, "Офис", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_PLOT, "Участок", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_BLOCK, "Блок", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_BOX, "Бокс", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_KILOMETER, "Километр", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_GEO, "Город\\Регион\\Страна", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_ZIP, "Индекс", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_POSTOFFICEBOX, "Абоненский ящик", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_CSP, "ГСП", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_METRO, "Метро", 0, 1);
            Pullenti.Ner.Metadata.Feature detail = GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_DETAIL, "Дополнительный указатель", 0, 1);
            GlobalMeta.DetailFeature = detail;
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.Cross.ToString(), "На пересечении", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.Near.ToString(), "Вблизи", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.Hostel.ToString(), "Общежитие", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.North.ToString(), "Севернее", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.South.ToString(), "Южнее", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.East.ToString(), "Восточнее", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.West.ToString(), "Западнее", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.NorthEast.ToString(), "Северо-восточнее", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.NorthWest.ToString(), "Северо-западнее", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.SouthEast.ToString(), "Юго-восточнее", null, null);
            detail.AddValue(Pullenti.Ner.Address.AddressDetailType.SouthWest.ToString(), "Юго-западнее", null, null);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_MISC, "Разное", 0, 0);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_DETAILPARAM, "Параметр детализации", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_FIAS, "Объект ФИАС", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Address.AddressReferent.ATTR_BTI, "Объект БТИ", 0, 1);
        }
        public Pullenti.Ner.Metadata.Feature DetailFeature;
        public Pullenti.Ner.Metadata.Feature HouseTypeFeature;
        public Pullenti.Ner.Metadata.Feature BuildingTypeFeature;
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Address.AddressReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Адрес";
            }
        }
        public static string AddressImageId = "address";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return AddressImageId;
        }
        internal static MetaAddress GlobalMeta;
    }
}