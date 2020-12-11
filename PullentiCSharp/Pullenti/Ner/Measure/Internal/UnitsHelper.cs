/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Measure.Internal
{
    public static class UnitsHelper
    {
        public static List<Unit> Units = new List<Unit>();
        public static Pullenti.Ner.Core.TerminCollection Termins = new Pullenti.Ner.Core.TerminCollection();
        public static Unit uGradus;
        public static Unit uGradusC;
        public static Unit uGradusF;
        public static Unit uPercent;
        public static Unit uAlco;
        public static Unit uOm;
        public static Unit uHour;
        public static Unit uMinute;
        public static Unit uSec;
        static bool m_Inited = false;
        static Dictionary<Pullenti.Ner.Measure.MeasureKind, List<string>> m_KindsKeywords;
        public static Unit FindUnit(string v, UnitsFactors fact)
        {
            if (fact != UnitsFactors.No) 
            {
                foreach (Unit u in Units) 
                {
                    if (u.BaseUnit != null && u.Factor == fact) 
                    {
                        if ((u.BaseUnit.FullnameCyr == v || u.BaseUnit.FullnameLat == v || u.BaseUnit.NameCyr == v) || u.BaseUnit.NameLat == v) 
                            return u;
                    }
                }
            }
            foreach (Unit u in Units) 
            {
                if ((u.FullnameCyr == v || u.FullnameLat == v || u.NameCyr == v) || u.NameLat == v) 
                    return u;
            }
            return null;
        }
        public static bool CheckKeyword(Pullenti.Ner.Measure.MeasureKind ki, Pullenti.Ner.Token t)
        {
            if (t == null || ki == Pullenti.Ner.Measure.MeasureKind.Undefined) 
                return false;
            if (t is Pullenti.Ner.MetaToken) 
            {
                for (Pullenti.Ner.Token tt = (t as Pullenti.Ner.MetaToken).BeginToken; tt != null && tt.EndChar <= t.EndChar; tt = tt.Next) 
                {
                    if (CheckKeyword(ki, tt)) 
                        return true;
                }
                return false;
            }
            if (!(t is Pullenti.Ner.TextToken)) 
                return false;
            string term = t.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false);
            foreach (Unit u in Units) 
            {
                if (u.Kind == ki) 
                {
                    if (u.Keywords.Contains(term)) 
                        return true;
                }
            }
            if (m_KindsKeywords.ContainsKey(ki)) 
            {
                if (m_KindsKeywords[ki].Contains(term)) 
                    return true;
            }
            return false;
        }
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Units = new List<Unit>();
            Termins = new Pullenti.Ner.Core.TerminCollection();
            m_KindsKeywords = new Dictionary<Pullenti.Ner.Measure.MeasureKind, List<string>>();
            m_KindsKeywords.Add(Pullenti.Ner.Measure.MeasureKind.Speed, new List<string>(new string[] {"СКОРОСТЬ", "SPEED", "ШВИДКІСТЬ"}));
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = true;
            Unit u;
            Unit uu;
            Pullenti.Ner.Core.Termin t;
            u = new Unit("м", "m", "метр", "meter") { Kind = Pullenti.Ner.Measure.MeasureKind.Length };
            u.Keywords.AddRange(new string[] {"ДЛИНА", "ДЛИННА", "ШИРИНА", "ГЛУБИНА", "ВЫСОТА", "РАЗМЕР", "ГАБАРИТ", "РАССТОЯНИЕ", "РАДИУС", "ПЕРИМЕТР", "ДИАМЕТР", "ТОЛЩИНА", "ПОДАЧА", "НАПОР", "ДАЛЬНОСТЬ", "ТИПОРАЗМЕР", "КАЛИБР", "LENGTH", "WIDTH", "DEPTH", "HEIGHT", "SIZE", "ENVELOPE", "DISTANCE", "RADIUS", "PERIMETER", "DIAMETER", "FLOW", "PRESSURE", "CALIBER", "ДОВЖИНА", "ШИРИНА", "ГЛИБИНА", "ВИСОТА", "РОЗМІР", "ГАБАРИТ", "ВІДСТАНЬ", "РАДІУС", "ДІАМЕТР", "НАТИСК", "КАЛІБР"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("МЕТР") { Tag = u };
            t.AddVariant("МЕТРОВЫЙ", false);
            t.AddVariant("МЕТРОВИЙ", false);
            t.AddVariant("METER", false);
            t.AddAbridge("М.");
            t.AddAbridge("M.");
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Deci, UnitsFactors.Centi, UnitsFactors.Milli, UnitsFactors.Micro, UnitsFactors.Nano}) 
            {
                _addFactor(f, u, "М.", "M.", "МЕТР;МЕТРОВЫЙ", "МЕТР;МЕТРОВИЙ", "METER;METRE");
            }
            uu = new Unit("миль", "mile", "морская миля", "mile") { Kind = Pullenti.Ner.Measure.MeasureKind.Length };
            uu.BaseUnit = u;
            uu.BaseMultiplier = 1852;
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("МИЛЯ") { Tag = uu };
            t.AddVariant("МОРСКАЯ МИЛЯ", false);
            t.AddAbridge("NMI");
            t.AddVariant("MILE", false);
            t.AddVariant("NAUTICAL MILE", false);
            Termins.Add(t);
            uu = new Unit("фут", "ft", "фут", "foot") { BaseUnit = u, BaseMultiplier = 0.304799472, Kind = Pullenti.Ner.Measure.MeasureKind.Length };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ФУТ") { Tag = uu };
            t.AddAbridge("FT.");
            t.AddVariant("FOOT", false);
            Termins.Add(t);
            uu = new Unit("дюйм", "in", "дюйм", "inch") { BaseUnit = u, BaseMultiplier = 0.0254, Kind = Pullenti.Ner.Measure.MeasureKind.Length };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ДЮЙМ") { Tag = uu };
            t.AddAbridge("IN");
            t.AddVariant("INCH", false);
            Termins.Add(t);
            u = new Unit("ар", "are", "ар", "are") { Kind = Pullenti.Ner.Measure.MeasureKind.Area };
            u.Keywords.AddRange(new string[] {"ПЛОЩАДЬ", "ПРОЩИНА", "AREA", "SQWARE", "SPACE"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("АР") { Tag = u };
            t.AddVariant("ARE", false);
            t.AddVariant("СОТКА", false);
            Termins.Add(t);
            uu = new Unit("га", "ga", "гектар", "hectare") { Kind = Pullenti.Ner.Measure.MeasureKind.Area };
            uu.BaseUnit = u;
            uu.BaseMultiplier = 100;
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ГЕКТАР") { Tag = uu };
            t.AddVariant("HECTARE", false);
            t.AddAbridge("ГА");
            t.AddAbridge("GA");
            Termins.Add(t);
            u = new Unit("г", "g", "грамм", "gram") { Kind = Pullenti.Ner.Measure.MeasureKind.Weight };
            u.Keywords.AddRange(new string[] {"ВЕС", "ТЯЖЕСТЬ", "НЕТТО", "БРУТТО", "МАССА", "НАГРУЗКА", "ЗАГРУЗКА", "УПАКОВКА", "WEIGHT", "NET", "GROSS", "MASS", "ВАГА", "ТЯЖКІСТЬ", "МАСА"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ГРАММ") { Tag = u };
            t.AddAbridge("Г.");
            t.AddAbridge("ГР.");
            t.AddAbridge("G.");
            t.AddAbridge("GR.");
            t.AddVariant("ГРАММОВЫЙ", false);
            t.AddVariant("ГРАММНЫЙ", false);
            t.AddVariant("ГРАМОВИЙ", false);
            t.AddVariant("GRAM", false);
            t.AddVariant("GRAMME", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Milli}) 
            {
                _addFactor(f, u, "Г.;ГР;", "G.;GR.", "ГРАМ;ГРАММ;ГРАММНЫЙ", "ГРАМ;ГРАМОВИЙ", "GRAM;GRAMME");
            }
            uu = new Unit("ц", "centner", "центнер", "centner") { BaseUnit = u, BaseMultiplier = 100000, Kind = Pullenti.Ner.Measure.MeasureKind.Weight };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ЦЕНТНЕР") { Tag = uu };
            t.AddVariant("CENTNER", false);
            t.AddVariant("QUINTAL", false);
            t.AddAbridge("Ц.");
            Termins.Add(t);
            uu = new Unit("т", "t", "тонна", "tonne") { BaseUnit = u, BaseMultiplier = 1000000, Kind = Pullenti.Ner.Measure.MeasureKind.Weight };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ТОННА") { Tag = uu };
            t.AddVariant("TONNE", false);
            t.AddVariant("TON", false);
            t.AddAbridge("Т.");
            t.AddAbridge("T.");
            Termins.Add(t);
            _addFactor(UnitsFactors.Mega, uu, "Т", "T", "ТОННА;ТОННЫЙ", "ТОННА;ТОННИЙ", "TONNE;TON");
            u = new Unit("л", "l", "литр", "liter") { Kind = Pullenti.Ner.Measure.MeasureKind.Volume };
            u.Keywords.AddRange(new string[] {"ОБЪЕМ", "ЕМКОСТЬ", "ВМЕСТИМОСЬ", "ОБСЯГ", "ЄМНІСТЬ", "МІСТКІСТЬ", "VOLUME", "CAPACITY"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ЛИТР") { Tag = u };
            t.AddAbridge("Л.");
            t.AddAbridge("L.");
            t.AddVariant("LITER", false);
            t.AddVariant("LITRE", false);
            t.AddVariant("ЛІТР", false);
            t.AddVariant("ЛІТРОВИЙ", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Milli, UnitsFactors.Centi}) 
            {
                _addFactor(f, u, "Л.", "L.", "ЛИТР;ЛИТРОВЫЙ", "ЛІТР;ЛІТРОВИЙ", "LITER;LITRE");
            }
            uu = new Unit("галлон", "gallon", "галлон", "gallon") { BaseUnit = u, BaseMultiplier = 4.5461, Kind = Pullenti.Ner.Measure.MeasureKind.Volume };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ГАЛЛОН") { Tag = u };
            t.AddVariant("ГАЛОН", false);
            t.AddVariant("GALLON", false);
            t.AddAbridge("ГАЛ");
            Termins.Add(t);
            uu = new Unit("баррель", "bbls", "баррель нефти", "barrel") { BaseUnit = u, BaseMultiplier = 158.987, Kind = Pullenti.Ner.Measure.MeasureKind.Volume };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("БАРРЕЛЬ") { Tag = uu };
            t.AddAbridge("BBLS");
            t.AddVariant("БАРРЕЛЬ НЕФТИ", false);
            t.AddVariant("BARRREL", false);
            Termins.Add(t);
            uSec = (u = new Unit("сек", "sec", "секунда", "second") { Kind = Pullenti.Ner.Measure.MeasureKind.Time });
            u.Keywords.AddRange(new string[] {"ВРЕМЯ", "ПРОДОЛЖИТЕЛЬНОСТЬ", "ЗАДЕРЖКА", "ДЛИТЕЛЬНОСТЬ", "ДОЛГОТА", "TIME", "DURATION", "DELAY", "ЧАС", "ТРИВАЛІСТЬ", "ЗАТРИМКА"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("СЕКУНДА") { Tag = u };
            t.AddAbridge("С.");
            t.AddAbridge("C.");
            t.AddAbridge("СЕК");
            t.AddAbridge("СЕК");
            t.AddAbridge("S.");
            t.AddAbridge("SEC");
            t.AddVariant("СЕКУНДНЫЙ", false);
            t.AddVariant("СЕКУНДНИЙ", false);
            t.AddVariant("SECOND", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Milli, UnitsFactors.Micro}) 
            {
                _addFactor(f, u, "С.;СЕК", "C;S.;SEC;", "СЕКУНДА;СЕКУНДНЫЙ", "СЕКУНДА;СЕКУНДНИЙ", "SECOND");
            }
            uMinute = (uu = new Unit("мин", "min", "минута", "minute") { Kind = Pullenti.Ner.Measure.MeasureKind.Time });
            uu.BaseUnit = u;
            uu.BaseMultiplier = 60;
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("МИНУТА") { Tag = uu };
            t.AddAbridge("МИН.");
            t.AddAbridge("MIN.");
            t.AddVariant("МИНУТНЫЙ", false);
            t.AddVariant("ХВИЛИННИЙ", false);
            t.AddVariant("ХВИЛИНА", false);
            t.AddVariant("МІНУТА", false);
            t.AddVariant("MINUTE", false);
            Termins.Add(t);
            u = uu;
            uHour = (uu = new Unit("ч", "h", "час", "hour") { BaseUnit = u, BaseMultiplier = 60, Kind = Pullenti.Ner.Measure.MeasureKind.Time });
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ЧАС") { Tag = uu };
            t.AddAbridge("Ч.");
            t.AddAbridge("H.");
            t.AddVariant("ЧАСОВОЙ", false);
            t.AddVariant("HOUR", false);
            t.AddVariant("ГОДИННИЙ", false);
            t.AddVariant("ГОДИНА", false);
            Termins.Add(t);
            u = new Unit("дн", "d", "день", "day") { Kind = Pullenti.Ner.Measure.MeasureKind.Time };
            u.Keywords.AddRange(uSec.Keywords);
            u.Keywords.AddRange(new string[] {"ПОСТАВКА", "СРОК", "РАБОТА", "ЗАВЕРШЕНИЕ"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ДЕНЬ") { Tag = u };
            t.AddAbridge("ДН.");
            t.AddAbridge("Д.");
            t.AddVariant("DAY", false);
            Termins.Add(t);
            uu = new Unit("сут", "d", "сутки", "day") { Kind = Pullenti.Ner.Measure.MeasureKind.Time };
            uu.Keywords.AddRange(uu.Keywords);
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("СУТКИ") { Tag = uu };
            t.AddAbridge("СУТ.");
            t.AddVariant("DAY", false);
            Termins.Add(t);
            uu = new Unit("нед", "week", "неделя", "week") { BaseUnit = u, BaseMultiplier = 7, Kind = Pullenti.Ner.Measure.MeasureKind.Time };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("НЕДЕЛЯ") { Tag = uu };
            t.AddAbridge("НЕД");
            t.AddVariant("WEEK", false);
            t.AddVariant("ТИЖДЕНЬ", false);
            Termins.Add(t);
            uu = new Unit("мес", "mon", "месяц", "month") { BaseUnit = u, BaseMultiplier = 30, Kind = Pullenti.Ner.Measure.MeasureKind.Time };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("МЕСЯЦ") { Tag = uu };
            t.AddAbridge("МЕС");
            t.AddAbridge("MON");
            t.AddVariant("MONTH", false);
            t.AddVariant("МІСЯЦЬ", false);
            Termins.Add(t);
            uu = new Unit("г", "year", "год", "year") { BaseUnit = u, BaseMultiplier = 365, Kind = Pullenti.Ner.Measure.MeasureKind.Time };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ГОД") { Tag = uu };
            t.AddAbridge("Г.");
            t.AddAbridge("ГД");
            t.AddVariant("YEAR", false);
            t.AddVariant("РІК", false);
            t.AddVariant("ЛЕТ", false);
            Termins.Add(t);
            uGradus = new Unit("°", "°", "градус", "degree");
            uGradus.Keywords.AddRange(new string[] {"ТЕМПЕРАТУРА", "ШИРОТА", "ДОЛГОТА", "АЗИМУТ", "ДОВГОТА", "TEMPERATURE", "LATITUDE", "LONGITUDE", "AZIMUTH"});
            Units.Add(uGradus);
            t = new Pullenti.Ner.Core.Termin("ГРАДУС") { Tag = uGradus };
            t.AddVariant("DEGREE", false);
            Termins.Add(t);
            uGradusC = new Unit("°C", "°C", "градус Цельсия", "celsius degree") { Kind = Pullenti.Ner.Measure.MeasureKind.Temperature };
            uGradusC.Keywords.Add("ТЕМПЕРАТУРА");
            uGradus.Keywords.Add("TEMPERATURE");
            uGradus.Psevdo.Add(uGradusC);
            Units.Add(uGradusC);
            t = new Pullenti.Ner.Core.Termin("ГРАДУС ЦЕЛЬСИЯ") { Tag = uGradusC };
            t.AddVariant("ГРАДУС ПО ЦЕЛЬСИЮ", false);
            t.AddVariant("CELSIUS DEGREE", false);
            Termins.Add(t);
            uGradusF = new Unit("°F", "°F", "градус Фаренгейта", "Fahrenheit degree") { Kind = Pullenti.Ner.Measure.MeasureKind.Temperature };
            uGradusF.Keywords = uGradusC.Keywords;
            uGradus.Psevdo.Add(uGradusF);
            Units.Add(uGradusF);
            t = new Pullenti.Ner.Core.Termin("ГРАДУС ФАРЕНГЕЙТА") { Tag = uGradusF };
            t.AddVariant("ГРАДУС ПО ФАРЕНГЕЙТУ", false);
            t.AddVariant("FAHRENHEIT DEGREE", false);
            Termins.Add(t);
            uPercent = new Unit("%", "%", "процент", "percent") { Kind = Pullenti.Ner.Measure.MeasureKind.Percent };
            Units.Add(uPercent);
            t = new Pullenti.Ner.Core.Termin("ПРОЦЕНТ") { Tag = uPercent };
            t.AddVariant("ПРОЦ", false);
            t.AddVariant("PERC", false);
            t.AddVariant("PERCENT", false);
            Termins.Add(t);
            uAlco = new Unit("%(об)", "%(vol)", "объёмный процент", "volume percent");
            uAlco.Keywords.AddRange(new string[] {"КРЕПОСТЬ", "АЛКОГОЛЬ", "ALCOHOL", "СПИРТ", "АЛКОГОЛЬНЫЙ", "SPIRIT"});
            uPercent.Psevdo.Add(uAlco);
            uGradus.Psevdo.Add(uAlco);
            Units.Add(uAlco);
            t = new Pullenti.Ner.Core.Termin("ОБЪЕМНЫЙ ПРОЦЕНТ") { Tag = uAlco };
            t.AddVariant("ГРАДУС", false);
            Termins.Add(t);
            u = new Unit("об", "rev", "оборот", "revolution");
            uGradus.Keywords.AddRange(new string[] {"ЧАСТОТА", "ВРАЩЕНИЕ", "ВРАЩАТЕЛЬНЫЙ", "СКОРОСТЬ", "ОБОРОТ", "FREQUENCY", "ROTATION", "ROTATIONAL", "SPEED", "ОБЕРТАННЯ", "ОБЕРТАЛЬНИЙ"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ОБОРОТ") { Tag = u };
            t.AddAbridge("ОБ.");
            t.AddAbridge("ROT.");
            t.AddAbridge("REV.");
            t.AddVariant("ROTATION", false);
            t.AddVariant("REVOLUTION", false);
            Termins.Add(t);
            u = new Unit("В", "V", "вольт", "volt");
            u.Keywords.AddRange(new string[] {"ЭЛЕКТРИЧЕСКИЙ", "ПОТЕНЦИАЛ", "НАПРЯЖЕНИЕ", "ЭЛЕКТРОДВИЖУЩИЙ", "ПИТАНИЕ", "ТОК", "ПОСТОЯННЫЙ", "ПЕРЕМЕННЫЙ", "ЕЛЕКТРИЧНИЙ", "ПОТЕНЦІАЛ", "НАПРУГА", "ЕЛЕКТРОРУШІЙНОЇ", "ХАРЧУВАННЯ", "СТРУМ", "ПОСТІЙНИЙ", "ЗМІННИЙ", "ELECTRIC", "POTENTIAL", "TENSION", "ELECTROMOTIVE", "FOOD", "CURRENT", "CONSTANT", "VARIABLE"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ВОЛЬТ") { Tag = u };
            t.AddVariant("VOLT", false);
            t.AddAbridge("V");
            t.AddAbridge("В.");
            t.AddAbridge("B.");
            t.AddVariant("VAC", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Milli, UnitsFactors.Milli, UnitsFactors.Micro}) 
            {
                _addFactor(f, u, "В.", "V.", "ВОЛЬТ;ВОЛЬТНЫЙ", "ВОЛЬТ;ВОЛЬТНІ", "VOLT");
            }
            u = new Unit("Вт", "W", "ватт", "watt");
            u.Keywords.AddRange(new string[] {"МОЩНОСТЬ", "ЭНЕРГИЯ", "ПОТОК", "ИЗЛУЧЕНИЕ", "ЭНЕРГОПОТРЕБЛЕНИЕ", "ПОТУЖНІСТЬ", "ЕНЕРГІЯ", "ПОТІК", "ВИПРОМІНЮВАННЯ", "POWER", "ENERGY", "FLOW", "RADIATION"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ВАТТ") { Tag = u };
            t.AddAbridge("Вт");
            t.AddAbridge("W");
            t.AddVariant("WATT", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Milli}) 
            {
                _addFactor(f, u, "ВТ.", "W.", "ВАТТ;ВАТТНЫЙ", "ВАТ;ВАТНИЙ", "WATT;WATTS");
            }
            uu = new Unit("л.с.", "hp", "лошадиная сила", "horsepower") { BaseUnit = u, BaseMultiplier = 735.49875 };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ЛОШАДИНАЯ СИЛА") { Tag = uu };
            t.AddAbridge("Л.С.");
            t.AddAbridge("ЛОШ.С.");
            t.AddAbridge("ЛОШ.СИЛА");
            t.AddAbridge("HP");
            t.AddAbridge("PS");
            t.AddAbridge("SV");
            t.AddVariant("HORSEPOWER", false);
            Termins.Add(t);
            u = new Unit("Дж", "J", "джоуль", "joule");
            u.Keywords.AddRange(new string[] {"РАБОТА", "ЭНЕРГИЯ", "ТЕПЛОТА", "ТЕПЛОВОЙ", "ТЕПЛОВЫДЕЛЕНИЕ", "МОЩНОСТЬ", "ХОЛОДИЛЬНЫЙ"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ДЖОУЛЬ") { Tag = u };
            t.AddAbridge("ДЖ");
            t.AddAbridge("J");
            t.AddVariant("JOULE", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Tera, UnitsFactors.Milli}) 
            {
                _addFactor(f, u, "ДЖ.", "J.", "ДЖОУЛЬ", "ДЖОУЛЬ", "JOULE");
            }
            uu = new Unit("БТЕ", "BTU", "британская терминальная единица", "british terminal unit");
            uu.Keywords = u.Keywords;
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("БРИТАНСКАЯ ТЕРМИНАЛЬНАЯ ЕДИНИЦА") { Tag = uu };
            t.AddAbridge("БТЕ");
            t.AddAbridge("BTU");
            t.AddVariant("BRITISH TERMINAL UNIT", false);
            Termins.Add(t);
            u = new Unit("К", "K", "кельвин", "kelvin");
            u.Keywords.AddRange(uGradusC.Keywords);
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("КЕЛЬВИН") { Tag = u };
            t.AddAbridge("К.");
            t.AddAbridge("K.");
            t.AddVariant("KELVIN", false);
            t.AddVariant("КЕЛЬВІН", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Milli}) 
            {
                _addFactor(f, u, "К.", "K.", "КЕЛЬВИН", "КЕЛЬВІН", "KELVIN");
            }
            u = new Unit("Гц", "Hz", "герц", "herz");
            u.Keywords.AddRange(new string[] {"ЧАСТОТА", "ЧАСТОТНЫЙ", "ПЕРИОДИЧНОСТЬ", "ПИТАНИЕ", "ЧАСТОТНИЙ", "ПЕРІОДИЧНІСТЬ", "FREQUENCY"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ГЕРЦ") { Tag = u };
            t.AddAbridge("HZ");
            t.AddAbridge("ГЦ");
            t.AddVariant("ГЕРЦОВЫЙ", false);
            t.AddVariant("ГЕРЦОВИЙ", false);
            t.AddVariant("HERZ", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Micro}) 
            {
                _addFactor(f, u, "ГЦ.", "W.", "ГЕРЦ;ГЕРЦОВЫЙ", "ГЕРЦ;ГЕРЦОВИЙ", "HERZ");
            }
            uOm = (u = new Unit("Ом", "Ω", "Ом", "Ohm"));
            u.Keywords.AddRange(new string[] {"СОПРОТИВЛЕНИЕ", "РЕЗИСТОР", "РЕЗИСТНЫЙ", "ИМПЕДАНС", "РЕЗИСТОРНЫЙ", "ОПІР", "РЕЗИСТИВНИЙ", "ІМПЕДАНС", "RESISTANCE", "RESISTOR", "RESISTIVE", "IMPEDANCE"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ОМ") { Tag = uOm };
            t.AddVariant("OHM", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Micro, UnitsFactors.Milli}) 
            {
                _addFactor(f, u, "ОМ", "Ω", "ОМ", "ОМ", "OHM");
            }
            u = new Unit("А", "A", "ампер", "ampere");
            u.Keywords.AddRange(new string[] {"ТОК", "СИЛА", "ЭЛЕКТРИЧЕСКИЙ", "ЭЛЕКТРИЧЕСТВО", "МАГНИТ", "МАГНИТОДВИЖУЩИЙ", "ПОТРЕБЛЕНИЕ", "CURRENT", "POWER", "ELECTRICAL", "ELECTRICITY", "MAGNET", "MAGNETOMOTIVE", "CONSUMPTION", "СТРУМ", "ЕЛЕКТРИЧНИЙ", "ЕЛЕКТРИКА", "МАГНІТ", "МАГНИТОДВИЖУЩИЙ", "СПОЖИВАННЯ"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("АМПЕР") { Tag = u };
            t.AddAbridge("A");
            t.AddAbridge("А");
            t.AddVariant("АМПЕРНЫЙ", false);
            t.AddVariant("AMP", false);
            t.AddVariant("AMPERE", false);
            Termins.Add(t);
            uu = new Unit("Ач", "Ah", "ампер-час", "ampere-hour") { BaseUnit = u, MultUnit = uHour };
            uu.Keywords.AddRange(new string[] {"ЗАРЯД", "АККУМУЛЯТОР", "АККУМУЛЯТОРНЫЙ", "ЗАРЯДКА", "БАТАРЕЯ", "CHARGE", "BATTERY", "CHARGING", "АКУМУЛЯТОР", "АКУМУЛЯТОРНИЙ"});
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("АМПЕР ЧАС") { Tag = uu };
            t.AddAbridge("АЧ");
            t.AddAbridge("AH");
            t.AddVariant("AMPERE HOUR", false);
            t.AddVariant("АМПЕРЧАС", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Micro, UnitsFactors.Milli}) 
            {
                Unit u1 = _addFactor(f, u, "А", "A", "АМПЕР;АМПЕРНЫЙ", "АМПЕР;АМПЕРНИЙ", "AMPERE;AMP");
                Unit uu1 = _addFactor(f, uu, "АЧ", "AH", "АМПЕР ЧАС", "АМПЕР ЧАС", "AMPERE HOUR");
                uu1.BaseUnit = u1;
                uu1.MultUnit = uHour;
            }
            uu = new Unit("ВА", "VA", "вольт-ампер", "volt-ampere");
            uu.MultUnit = u;
            uu.BaseUnit = FindUnit("V", UnitsFactors.No);
            uu.Keywords.AddRange(new string[] {"ТОК", "СИЛА", "МОЩНОСТЬ", "ЭЛЕКТРИЧЕСКИЙ", "ПЕРЕМЕННЫЙ"});
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("ВОЛЬТ-АМПЕР") { Tag = uu };
            t.AddAbridge("BA");
            t.AddAbridge("BA");
            t.AddVariant("VA", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Micro, UnitsFactors.Milli}) 
            {
                Unit u1 = _addFactor(f, uu, "ВА;BA", "VA", "ВОЛЬТ-АМПЕР", "ВОЛЬТ-АМПЕР", "VOLT-AMPERE");
            }
            u = new Unit("лк", "lx", "люкс", "lux");
            u.Keywords.AddRange(new string[] {"СВЕТ", "ОСВЕЩЕННОСТЬ", "ILLUMINANCE", "СВІТЛО", " ОСВІТЛЕНІСТЬ"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ЛЮКС") { Tag = u };
            t.AddAbridge("ЛК");
            t.AddAbridge("LX");
            t.AddVariant("LUX", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Deci, UnitsFactors.Centi, UnitsFactors.Micro, UnitsFactors.Milli, UnitsFactors.Nano}) 
            {
                Unit u1 = _addFactor(f, u, "ЛК", "LX", "ЛЮКС", "ЛЮКС", "LUX");
            }
            u = new Unit("Б", "B", "белл", "bell");
            u.Keywords.AddRange(new string[] {"ЗВУК", "ЗВУКОВОЙ", "ШУМ", "ШУМОВОЙ", "ГРОМКОСТЬ", "ГРОМКИЙ", "СИГНАЛ", "УСИЛЕНИЕ", "ЗАТУХАНИЕ", "ГАРМОНИЧЕСКИЙ", "ПОДАВЛЕНИЕ", "ЗВУКОВИЙ", "ШУМОВИЙ", "ГУЧНІСТЬ", "ГУЧНИЙ", "ПОСИЛЕННЯ", "ЗАГАСАННЯ", "ГАРМОНІЙНИЙ", "ПРИДУШЕННЯ", "SOUND", "NOISE", "VOLUME", "LOUD", "SIGNAL", "STRENGTHENING", "ATTENUATION", "HARMONIC", "SUPPRESSION"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("БЕЛЛ") { Tag = u };
            t.AddAbridge("Б.");
            t.AddAbridge("B.");
            t.AddAbridge("В.");
            t.AddVariant("БЕЛ", false);
            t.AddVariant("BELL", false);
            Termins.Add(t);
            _addFactor(UnitsFactors.Deci, u, "Б", "B", "БЕЛЛ;БЕЛ", "БЕЛЛ;БЕЛ", "BELL");
            u = new Unit("дБи", "dBi", "коэффициент усиления антенны", "dBi");
            u.Keywords.AddRange(new string[] {"УСИЛЕНИЕ", "АНТЕННА", "АНТЕНА", "ПОСИЛЕННЯ", "GAIN", "ANTENNA"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("DBI") { Tag = u };
            t.AddVariant("ДБИ", false);
            Termins.Add(t);
            u = new Unit("дБм", "dBm", "опорная мощность", "dBm");
            u.Keywords.AddRange(new string[] {"МОЩНОСТЬ"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("DBM") { Tag = u };
            t.AddVariant("ДБМ", false);
            t.AddVariant("ДВМ", false);
            Termins.Add(t);
            u = new Unit("Ф", "F", "фарад", "farad");
            u.Keywords.AddRange(new string[] {"ЕМКОСТЬ", "ЭЛЕКТРИЧНСКИЙ", "КОНДЕНСАТОР"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ФАРАД") { Tag = u };
            t.AddAbridge("Ф.");
            t.AddAbridge("ФА");
            t.AddAbridge("F");
            t.AddVariant("FARAD", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Micro, UnitsFactors.Milli, UnitsFactors.Nano, UnitsFactors.Pico}) 
            {
                _addFactor(f, u, "Ф.;ФА.", "F", "ФАРАД", "ФАРАД", "FARAD");
            }
            u = new Unit("Н", "N", "ньютон", "newton");
            u.Keywords.AddRange(new string[] {"СИЛА", "МОМЕНТ", "НАГРУЗКА"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("НЬЮТОН") { Tag = u };
            t.AddAbridge("Н.");
            t.AddAbridge("H.");
            t.AddAbridge("N.");
            t.AddVariant("NEWTON", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Mega, UnitsFactors.Kilo, UnitsFactors.Micro, UnitsFactors.Milli}) 
            {
                _addFactor(f, u, "Н.", "N.", "НЬЮТОН", "НЬЮТОН", "NEWTON");
            }
            u = new Unit("моль", "mol", "моль", "mol");
            u.Keywords.AddRange(new string[] {"МОЛЕКУЛА", "МОЛЕКУЛЯРНЫЙ", "КОЛИЧЕСТВО", "ВЕЩЕСТВО"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("МОЛЬ") { Tag = u };
            t.AddAbridge("МЛЬ");
            t.AddVariant("МОЛ", false);
            t.AddVariant("MOL", false);
            t.AddVariant("ГРАММ МОЛЕКУЛА", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Mega, UnitsFactors.Kilo, UnitsFactors.Micro, UnitsFactors.Milli, UnitsFactors.Nano}) 
            {
                _addFactor(f, u, "МЛЬ", "MOL", "МОЛЬ", "МОЛЬ", "MOL");
            }
            u = new Unit("Бк", "Bq", "беккерель", "becquerel");
            u.Keywords.AddRange(new string[] {"АКТИВНОСТЬ", "РАДИОАКТИВНЫЙ", "ИЗЛУЧЕНИЕ", "ИСТОЧНИК"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("БЕККЕРЕЛЬ") { Tag = u };
            t.AddAbridge("БК.");
            t.AddVariant("BQ.", false);
            t.AddVariant("БЕК", false);
            t.AddVariant("БЕКЕРЕЛЬ", false);
            t.AddVariant("BECQUEREL", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Mega, UnitsFactors.Kilo, UnitsFactors.Micro, UnitsFactors.Milli, UnitsFactors.Nano}) 
            {
                _addFactor(f, u, "БК.", "BQ.", "БЕККЕРЕЛЬ;БЕК", "БЕКЕРЕЛЬ", "BECQUEREL");
            }
            u = new Unit("См", "S", "сименс", "siemens");
            u.Keywords.AddRange(new string[] {"ПРОВОДИМОСТЬ", "ЭЛЕКТРИЧЕСКИЙ", "ПРОВОДНИК"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("СИМЕНС") { Tag = u };
            t.AddAbridge("СМ.");
            t.AddAbridge("CM.");
            t.AddVariant("S.", false);
            t.AddVariant("SIEMENS", false);
            t.AddVariant("СІМЕНС", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Mega, UnitsFactors.Kilo, UnitsFactors.Micro, UnitsFactors.Milli, UnitsFactors.Nano}) 
            {
                _addFactor(f, u, "СМ.", "S.", "СИМЕНС", "СІМЕНС", "SIEMENS");
            }
            u = new Unit("кд", "cd", "кандела", "candela");
            u.Keywords.AddRange(new string[] {"СВЕТ", "СВЕТОВОЙ", "ПОТОК", "ИСТОЧНИК"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("КАНДЕЛА") { Tag = u };
            t.AddAbridge("КД.");
            t.AddVariant("CD.", false);
            t.AddVariant("КАНДЕЛА", false);
            t.AddVariant("CANDELA", false);
            Termins.Add(t);
            u = new Unit("Па", "Pa", "паскаль", "pascal");
            u.Keywords.AddRange(new string[] {"ДАВЛЕНИЕ", "НАПРЯЖЕНИЕ", "ТЯЖЕСТЬ", "PRESSURE", "STRESS", "ТИСК", "НАПРУГА"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ПАСКАЛЬ") { Tag = u };
            t.AddAbridge("ПА");
            t.AddAbridge("РА");
            t.AddVariant("PA", false);
            t.AddVariant("PASCAL", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Micro, UnitsFactors.Milli}) 
            {
                _addFactor(f, u, "ПА", "PA", "ПАСКАЛЬ", "ПАСКАЛЬ", "PASCAL");
            }
            uu = new Unit("бар", "bar", "бар", "bar") { BaseUnit = u, BaseMultiplier = 100000 };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("БАР") { Tag = uu };
            t.AddVariant("BAR", false);
            Termins.Add(t);
            uu = new Unit("мм.рт.ст.", "mm Hg", "миллиметр ртутного столба", "millimeter of mercury") { BaseUnit = u, BaseMultiplier = 133.332 };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("МИЛЛИМЕТР РТУТНОГО СТОЛБА") { Tag = uu };
            t.AddAbridge("ММ.РТ.СТ.");
            t.AddAbridge("MM.PT.CT");
            t.AddAbridge("MM HG");
            t.AddVariant("MMGH", false);
            t.AddVariant("ТОРР", false);
            t.AddVariant("TORR", false);
            t.AddVariant("MILLIMETER OF MERCURY", false);
            Termins.Add(t);
            u = new Unit("бит", "bit", "бит", "bit");
            u.Keywords.AddRange(new string[] {"ОБЪЕМ", "РАЗМЕР", "ПАМЯТЬ", "ЕМКОСТЬ", "ПЕРЕДАЧА", "ПРИЕМ", "ОТПРАВКА", "ОП", "ДИСК", "НАКОПИТЕЛЬ", "КЭШ", "ОБСЯГ", "РОЗМІР", "ВІДПРАВЛЕННЯ", "VOLUME", "SIZE", "MEMORY", "TRANSFER", "SEND", "RECEPTION", "RAM", "DISK", "HDD", "RAM", "ROM", "CD-ROM", "CASHE"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("БИТ") { Tag = u };
            t.AddVariant("BIT", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Tera}) 
            {
                _addFactor(f, u, "БИТ", "BIT", "БИТ", "БИТ", "BIT");
            }
            uu = new Unit("б", "b", "байт", "byte");
            uu.Keywords = u.Keywords;
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("БАЙТ") { Tag = uu };
            t.AddVariant("BYTE", false);
            t.AddAbridge("B.");
            t.AddAbridge("Б.");
            t.AddAbridge("В.");
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Tera}) 
            {
                _addFactor(f, uu, "Б.", "B.", "БАЙТ", "БАЙТ", "BYTE");
            }
            u = new Unit("бод", "Bd", "бод", "baud");
            u.Keywords.AddRange(new string[] {"СКОРОСТЬ", "ПЕРЕДАЧА", "ПРИЕМ", "ДАННЫЕ", "ОТПРАВКА"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("БОД") { Tag = u };
            t.AddAbridge("BD");
            t.AddVariant("BAUD", false);
            Termins.Add(t);
            foreach (UnitsFactors f in new UnitsFactors[] {UnitsFactors.Kilo, UnitsFactors.Mega, UnitsFactors.Giga, UnitsFactors.Tera}) 
            {
                _addFactor(f, uu, "БОД", "BD.", "БОД", "БОД", "BAUD");
            }
            u = new Unit("гс", "gf", "грамм-сила", "gram-force");
            u.Keywords.AddRange(new string[] {"СИЛА", "ДАВЛЕНИЕ"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("ГРАММ СИЛА") { Tag = u };
            t.AddAbridge("ГС");
            t.AddVariant("POND", false);
            t.AddVariant("ГРАМ СИЛА", false);
            t.AddAbridge("GP.");
            t.AddVariant("GRAM POND", false);
            t.AddVariant("GRAM FORCE", false);
            Termins.Add(t);
            uu = new Unit("кгс", "kgf", "килограмм-сила", "kilogram-force") { BaseUnit = u, BaseMultiplier = 1000 };
            Units.Add(uu);
            t = new Pullenti.Ner.Core.Termin("КИЛОГРАММ СИЛА") { Tag = uu };
            t.AddAbridge("КГС");
            t.AddVariant("KILOPOND", false);
            t.AddVariant("КІЛОГРАМ СИЛА", false);
            t.AddAbridge("KP.");
            t.AddVariant("KILOGRAM POND", false);
            Termins.Add(t);
            u = new Unit("dpi", "точек на дюйм", "dpi", "dots per inch");
            u.Keywords.AddRange(new string[] {"РАЗРЕШЕНИЕ", "ЭКРАН", "МОНИТОР"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("DOTS PER INCH") { Tag = u };
            t.AddVariant("DPI", false);
            Termins.Add(t);
            u = new Unit("IP", "IP", "IP", "IP") { Kind = Pullenti.Ner.Measure.MeasureKind.Ip };
            u.Keywords.AddRange(new string[] {"ЗАЩИТА", "КЛАСС ЗАЩИТЫ", "PROTECTION", "PROTACTION RATING"});
            Units.Add(u);
            t = new Pullenti.Ner.Core.Termin("IP") { Tag = u };
            Termins.Add(t);
            Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
        }
        static Unit _addFactor(UnitsFactors f, Unit u0, string abbrCyr, string abbrLat, string namesRu, string namesUa, string namesEn)
        {
            string prefCyr = null;
            string prefLat = null;
            string prefRu = null;
            string prefUa = null;
            string prefEn = null;
            double mult = (double)1;
            switch (f) { 
            case UnitsFactors.Centi:
                prefCyr = "С";
                prefLat = "C";
                prefRu = "САНТИ";
                prefUa = "САНТИ";
                prefEn = "CENTI";
                mult = 0.1;
                break;
            case UnitsFactors.Deci:
                prefCyr = "Д";
                prefLat = "D";
                prefRu = "ДЕЦИ";
                prefUa = "ДЕЦИ";
                prefEn = "DECI";
                mult = 0.01;
                break;
            case UnitsFactors.Giga:
                prefCyr = "Г";
                prefLat = "G";
                prefRu = "ГИГА";
                prefUa = "ГІГА";
                prefEn = "GIGA";
                mult = 1000000000;
                break;
            case UnitsFactors.Kilo:
                prefCyr = "К";
                prefLat = "K";
                prefRu = "КИЛО";
                prefUa = "КІЛО";
                prefEn = "KILO";
                mult = 1000;
                break;
            case UnitsFactors.Mega:
                prefCyr = "М";
                prefLat = "M";
                prefRu = "МЕГА";
                prefUa = "МЕГА";
                prefEn = "MEGA";
                mult = 1000000;
                break;
            case UnitsFactors.Micro:
                prefCyr = "МК";
                prefLat = "MK";
                prefRu = "МИКРО";
                prefUa = "МІКРО";
                prefEn = "MICRO";
                mult = 0.0001;
                break;
            case UnitsFactors.Milli:
                prefCyr = "М";
                prefLat = "M";
                prefRu = "МИЛЛИ";
                prefUa = "МІЛІ";
                prefEn = "MILLI";
                mult = 0.001;
                break;
            case UnitsFactors.Nano:
                prefCyr = "Н";
                prefLat = "N";
                prefRu = "НАНО";
                prefUa = "НАНО";
                prefEn = "NANO";
                mult = 0.0000000001;
                break;
            case UnitsFactors.Pico:
                prefCyr = "П";
                prefLat = "P";
                prefRu = "ПИКО";
                prefUa = "ПІКО";
                prefEn = "PICO";
                mult = 0.0000000000001;
                break;
            case UnitsFactors.Tera:
                prefCyr = "Т";
                prefLat = "T";
                prefRu = "ТЕРА";
                prefUa = "ТЕРА";
                prefEn = "TERA";
                mult = 1000000000000;
                break;
            }
            Unit u = new Unit(prefCyr.ToLower() + u0.NameCyr, prefLat.ToLower() + u0.NameLat, prefRu.ToLower() + u0.FullnameCyr, prefEn.ToLower() + u0.FullnameLat) { Factor = f, BaseMultiplier = mult, BaseUnit = u0, Kind = u0.Kind, Keywords = u0.Keywords };
            if (f == UnitsFactors.Mega || f == UnitsFactors.Tera || f == UnitsFactors.Giga) 
            {
                u.NameCyr = prefCyr + u0.NameCyr;
                u.NameLat = prefLat + u0.NameLat;
            }
            Units.Add(u);
            string[] nams = namesRu.Split(';');
            Pullenti.Ner.Core.Termin t = new Pullenti.Ner.Core.Termin(prefRu + nams[0]) { Tag = u };
            for (int i = 1; i < nams.Length; i++) 
            {
                if (!string.IsNullOrEmpty(nams[i])) 
                    t.AddVariant(prefRu + nams[i], false);
            }
            foreach (string n in nams) 
            {
                if (!string.IsNullOrEmpty(n)) 
                    t.AddVariant(prefCyr + n, false);
            }
            foreach (string n in namesUa.Split(';')) 
            {
                if (!string.IsNullOrEmpty(n)) 
                {
                    t.AddVariant(prefUa + n, false);
                    t.AddVariant(prefCyr + n, false);
                }
            }
            foreach (string n in namesEn.Split(';')) 
            {
                if (!string.IsNullOrEmpty(n)) 
                {
                    t.AddVariant(prefEn + n, false);
                    t.AddVariant(prefLat + n, false);
                }
            }
            foreach (string n in abbrCyr.Split(';')) 
            {
                if (!string.IsNullOrEmpty(n)) 
                    t.AddAbridge(prefCyr + n);
            }
            foreach (string n in abbrLat.Split(';')) 
            {
                if (!string.IsNullOrEmpty(n)) 
                    t.AddAbridge(prefLat + n);
            }
            Termins.Add(t);
            return u;
        }
    }
}