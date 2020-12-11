/*
 * SDK Pullenti Lingvo, version 4.1, december 2020. Copyright (c) 2013, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Org.Internal
{
    static class OrgOwnershipHelper
    {
        public static bool CanBeHigher(Pullenti.Ner.Org.OrganizationReferent higher, Pullenti.Ner.Org.OrganizationReferent lower, bool robust = false)
        {
            if (higher == null || lower == null || higher == lower) 
                return false;
            if (lower.Owner != null) 
                return false;
            Pullenti.Ner.Org.OrganizationKind hk = higher.Kind;
            Pullenti.Ner.Org.OrganizationKind lk = lower.Kind;
            if (higher.CanBeEquals(lower, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                return false;
            if (lower.Higher == null && lower.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_HIGHER, null, true) != null) 
                return false;
            ICollection<string> hTyps = (ICollection<string>)higher.Types;
            ICollection<string> lTyps = (ICollection<string>)lower.Types;
            if (hk != Pullenti.Ner.Org.OrganizationKind.Bank) 
            {
                foreach (string v in hTyps) 
                {
                    if (lTyps.Contains(v)) 
                        return false;
                }
            }
            if (hk != Pullenti.Ner.Org.OrganizationKind.Department && lk == Pullenti.Ner.Org.OrganizationKind.Department) 
            {
                if (_Contains(lTyps, "курс", null) || _Contains(lTyps, "группа", "група")) 
                    return hk == Pullenti.Ner.Org.OrganizationKind.Study || _Contains(hTyps, "институт", "інститут");
                if (_Contains(lTyps, "епархия", "єпархія") || _Contains(lTyps, "патриархия", "патріархія")) 
                    return hk == Pullenti.Ner.Org.OrganizationKind.Church;
                if (hk == Pullenti.Ner.Org.OrganizationKind.Undefined) 
                {
                    if (_Contains(hTyps, "управление", "управління")) 
                        return false;
                }
                return true;
            }
            if (lower.ContainsProfile(Pullenti.Ner.Org.OrgProfile.Unit) || _Contains(lTyps, "department", null)) 
            {
                if (!higher.ContainsProfile(Pullenti.Ner.Org.OrgProfile.Unit) && lk != Pullenti.Ner.Org.OrganizationKind.Department) 
                    return true;
            }
            if (_Contains(hTyps, "правительство", "уряд")) 
            {
                if (lk == Pullenti.Ner.Org.OrganizationKind.Govenment) 
                    return (((lTyps.Contains("агентство") || lTyps.Contains("федеральная служба") || lTyps.Contains("федеральна служба")) || lTyps.Contains("департамент") || lTyps.Contains("комиссия")) || lTyps.Contains("комитет") || lTyps.Contains("комісія")) || lTyps.Contains("комітет");
            }
            if (hk == Pullenti.Ner.Org.OrganizationKind.Govenment) 
            {
                if (lk == Pullenti.Ner.Org.OrganizationKind.Govenment) 
                {
                    if (_Contains(lTyps, "комиссия", "комісія") || _Contains(lTyps, "инспекция", "інспекція") || _Contains(lTyps, "комитет", "комітет")) 
                    {
                        if ((!_Contains(hTyps, "комиссия", "комісія") && !_Contains(hTyps, "инспекция", "інспекція") && !_Contains(lTyps, "государственный комитет", null)) && !_Contains(hTyps, "комитет", "комітет") && ((!_Contains(hTyps, "совет", "рада") || higher.ToString().Contains("Верховн")))) 
                            return true;
                    }
                    if (higher.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME, "ФЕДЕРАЛЬНОЕ СОБРАНИЕ", true) != null || hTyps.Contains("конгресс") || hTyps.Contains("парламент")) 
                    {
                        if ((lower.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME, "СОВЕТ ФЕДЕРАЦИИ", true) != null || lower.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME, "ГОСУДАРСТВЕННАЯ ДУМА", true) != null || lower.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME, "ВЕРХОВНА РАДА", true) != null) || _Contains(lTyps, "палата", null) || _Contains(lTyps, "совет", null)) 
                            return true;
                    }
                    if (higher.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME, "ФСБ", true) != null) 
                    {
                        if (lower.FindSlot(Pullenti.Ner.Org.OrganizationReferent.ATTR_NAME, "ФПС", true) != null) 
                            return true;
                    }
                    if (_Contains(hTyps, "государственный комитет", null)) 
                    {
                        if ((_Contains(lTyps, "комиссия", "комісія") || _Contains(lTyps, "инспекция", "інспекція") || _Contains(lTyps, "комитет", "комітет")) || _Contains(lTyps, "департамент", null)) 
                            return true;
                    }
                }
                else if (lk == Pullenti.Ner.Org.OrganizationKind.Undefined) 
                {
                    if ((_Contains(lTyps, "комиссия", "комісія") || _Contains(lTyps, "инспекция", "інспекція") || _Contains(lTyps, "комитет", "комітет")) || _Contains(lTyps, "управление", "управління") || _Contains(lTyps, "служба", null)) 
                        return true;
                }
                else if (lk == Pullenti.Ner.Org.OrganizationKind.Bank) 
                {
                }
            }
            if (_Contains(hTyps, "министерство", "міністерство")) 
            {
                if ((((((_Contains(lTyps, "институт", "інститут") || _Contains(lTyps, "университет", "університет") || _Contains(lTyps, "училище", null)) || _Contains(lTyps, "школа", null) || _Contains(lTyps, "лицей", "ліцей")) || _Contains(lTyps, "НИИ", "НДІ") || _Contains(lTyps, "Ф", null)) || _Contains(lTyps, "департамент", null) || _Contains(lTyps, "управление", "управління")) || _Contains(lTyps, "комитет", "комітет") || _Contains(lTyps, "комиссия", "комісія")) || _Contains(lTyps, "инспекция", "інспекція") || _Contains(lTyps, "центр", null)) 
                    return true;
                if (_Contains(lTyps, "академия", "академія")) 
                {
                }
                if (_Contains(lTyps, "служба", null) && !_Contains(lTyps, "федеральная служба", "федеральна служба")) 
                    return true;
                if (lk == Pullenti.Ner.Org.OrganizationKind.Culture || lk == Pullenti.Ner.Org.OrganizationKind.Medical) 
                    return true;
            }
            if (_Contains(hTyps, "академия", "академія")) 
            {
                if (_Contains(lTyps, "институт", "інститут") || _Contains(lTyps, "научн", "науков") || _Contains(lTyps, "НИИ", "НДІ")) 
                    return true;
            }
            if (_Contains(hTyps, "факультет", null)) 
            {
                if (_Contains(lTyps, "курс", null) || _Contains(lTyps, "кафедра", null)) 
                    return true;
            }
            if (_Contains(hTyps, "university", null)) 
            {
                if (_Contains(lTyps, "school", null) || _Contains(lTyps, "college", null)) 
                    return true;
            }
            int hr = _militaryRank(hTyps);
            int lr = _militaryRank(lTyps);
            if (hr > 0) 
            {
                if (lr > 0) 
                    return hr < lr;
                else if (hr == 3 && ((lTyps.Contains("войсковая часть") || lTyps.Contains("військова частина")))) 
                    return true;
            }
            else if (hTyps.Contains("войсковая часть") || hTyps.Contains("військова частина")) 
            {
                if (lr >= 6) 
                    return true;
            }
            if (lr >= 6) 
            {
                if (higher.ContainsProfile(Pullenti.Ner.Org.OrgProfile.Policy) || higher.ContainsProfile(Pullenti.Ner.Org.OrgProfile.Union)) 
                    return true;
            }
            if (hk == Pullenti.Ner.Org.OrganizationKind.Study || _Contains(hTyps, "институт", "інститут") || _Contains(hTyps, "академия", "академія")) 
            {
                if (((_Contains(lTyps, "магистратура", "магістратура") || _Contains(lTyps, "аспирантура", "аспірантура") || _Contains(lTyps, "докторантура", null)) || _Contains(lTyps, "факультет", null) || _Contains(lTyps, "кафедра", null)) || _Contains(lTyps, "курс", null)) 
                    return true;
            }
            if (hk != Pullenti.Ner.Org.OrganizationKind.Department) 
            {
                if (((((_Contains(lTyps, "департамент", null) || _Contains(lTyps, "центр", null))) && hk != Pullenti.Ner.Org.OrganizationKind.Medical && hk != Pullenti.Ner.Org.OrganizationKind.Science) && !_Contains(hTyps, "центр", null) && !_Contains(hTyps, "департамент", null)) && !_Contains(hTyps, "управление", "управління")) 
                    return true;
                if (_Contains(hTyps, "департамент", null) || robust) 
                {
                    if (_Contains(lTyps, "центр", null)) 
                        return true;
                    if (lk == Pullenti.Ner.Org.OrganizationKind.Study) 
                        return true;
                }
                if (_Contains(hTyps, "служба", null) || _Contains(hTyps, "штаб", null)) 
                {
                    if (_Contains(lTyps, "управление", "управління")) 
                        return true;
                }
                if (hk == Pullenti.Ner.Org.OrganizationKind.Bank) 
                {
                    if (_Contains(lTyps, "управление", "управління") || _Contains(lTyps, "департамент", null)) 
                        return true;
                }
                if (hk == Pullenti.Ner.Org.OrganizationKind.Party || hk == Pullenti.Ner.Org.OrganizationKind.Federation) 
                {
                    if (_Contains(lTyps, "комитет", "комітет")) 
                        return true;
                }
                if ((lk == Pullenti.Ner.Org.OrganizationKind.Federation && hk != Pullenti.Ner.Org.OrganizationKind.Federation && hk != Pullenti.Ner.Org.OrganizationKind.Govenment) && hk != Pullenti.Ner.Org.OrganizationKind.Party) 
                {
                    if (!_Contains(hTyps, "фонд", null) && hk != Pullenti.Ner.Org.OrganizationKind.Undefined) 
                        return true;
                }
            }
            else if (_Contains(hTyps, "управление", "управління") || _Contains(hTyps, "департамент", null)) 
            {
                if (!_Contains(lTyps, "управление", "управління") && !_Contains(lTyps, "департамент", null) && lk == Pullenti.Ner.Org.OrganizationKind.Department) 
                    return true;
                if (_Contains(hTyps, "главное", "головне") && _Contains(hTyps, "управление", "управління")) 
                {
                    if (_Contains(lTyps, "департамент", null)) 
                        return true;
                    if (_Contains(lTyps, "управление", "управління")) 
                    {
                        if (!lTyps.Contains("главное управление") && !lTyps.Contains("головне управління") && !lTyps.Contains("пограничное управление")) 
                            return true;
                    }
                }
                if (_Contains(hTyps, "управление", "управління") && _Contains(lTyps, "центр", null)) 
                    return true;
                if (_Contains(hTyps, "департамент", null) && _Contains(lTyps, "управление", "управління")) 
                    return true;
            }
            else if ((lk == Pullenti.Ner.Org.OrganizationKind.Govenment && _Contains(lTyps, "служба", null) && higher.Higher != null) && higher.Higher.Kind == Pullenti.Ner.Org.OrganizationKind.Govenment) 
                return true;
            else if (_Contains(hTyps, "отдел", "відділ") && lk == Pullenti.Ner.Org.OrganizationKind.Department && ((_Contains(lTyps, "стол", "стіл") || _Contains(lTyps, "направление", "напрямок") || _Contains(lTyps, "отделение", "відділ")))) 
                return true;
            if (hk == Pullenti.Ner.Org.OrganizationKind.Bank) 
            {
                if (higher.Names.Contains("СБЕРЕГАТЕЛЬНЫЙ БАНК")) 
                {
                    if (lk == Pullenti.Ner.Org.OrganizationKind.Bank && !lower.Names.Contains("СБЕРЕГАТЕЛЬНЫЙ БАНК")) 
                        return true;
                }
            }
            if (lk == Pullenti.Ner.Org.OrganizationKind.Medical) 
            {
                if (hTyps.Contains("департамент")) 
                    return true;
            }
            if (lk == Pullenti.Ner.Org.OrganizationKind.Department) 
            {
                if (hk == Pullenti.Ner.Org.OrganizationKind.Department && higher.Higher != null && hTyps.Count == 0) 
                {
                    if (CanBeHigher(higher.Higher, lower, false)) 
                    {
                        if (_Contains(lTyps, "управление", "управління") || _Contains(lTyps, "отдел", "відділ")) 
                            return true;
                    }
                }
                if (_Contains(lTyps, "офис", "офіс")) 
                {
                    if (_Contains(hTyps, "филиал", "філіал") || _Contains(hTyps, "отделение", "відділення")) 
                        return true;
                }
            }
            if (_Contains(lTyps, "управление", "управління") || _Contains(lTyps, "отдел", "відділ")) 
            {
                string str = higher.ToString(true, null, 0);
                if (str.StartsWith("ГУ", StringComparison.OrdinalIgnoreCase)) 
                    return true;
            }
            return false;
        }
        static int _militaryRank(ICollection<string> li)
        {
            if (_Contains(li, "фронт", null)) 
                return 1;
            if (_Contains(li, "группа армий", "група армій")) 
                return 2;
            if (_Contains(li, "армия", "армія")) 
                return 3;
            if (_Contains(li, "корпус", null)) 
                return 4;
            if (_Contains(li, "округ", null)) 
                return 5;
            if (_Contains(li, "дивизия", "дивізія")) 
                return 6;
            if (_Contains(li, "бригада", null)) 
                return 7;
            if (_Contains(li, "полк", null)) 
                return 8;
            if (_Contains(li, "батальон", "батальйон") || _Contains(li, "дивизион", "дивізіон")) 
                return 9;
            if (_Contains(li, "рота", null) || _Contains(li, "батарея", null) || _Contains(li, "эскадрон", "ескадрон")) 
                return 10;
            if (_Contains(li, "взвод", null) || _Contains(li, "отряд", "загін")) 
                return 11;
            return -1;
        }
        static bool _Contains(ICollection<string> li, string v, string v2 = null)
        {
            foreach (string l in li) 
            {
                if (l.Contains(v)) 
                    return true;
            }
            if (v2 != null) 
            {
                foreach (string l in li) 
                {
                    if (l.Contains(v2)) 
                        return true;
                }
            }
            return false;
        }
    }
}