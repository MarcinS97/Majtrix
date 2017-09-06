using HRRcp.Areas.ME.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.Areas.ME.Models.CustomModels.Oceny
{
    public class WartoscOceny : IOcenaValue
    {

        
        public double getWartoscOceny(Models.Oceny[] oceny, int Integer = 0, int Typ = 0)
        {
            double wartosc = 0;
            if (Integer == 0 || Typ == 0)
            {
                foreach (var item in oceny)
                {
                    if (item.Wartosc != null)
                    {
                        wartosc += (double)item.Wartosc;
                    }
                }
                return wartosc;
            }
            else
            {
                foreach (var item in oceny)
                {
                    if (item.Wartosc != null)
                    {
                        switch (Typ)
                        {
                            case 1:
                                {
                                    wartosc += (double)item.Wartosc - Integer;
                                } break;
                            case 2:
                                {
                                    wartosc += (double)item.Wartosc * Integer;
                                }
                                break;
                            case 3:
                                {
                                    wartosc += (double)item.Wartosc / Integer;
                                }
                                break;
                            default:
                                return 0;
                                break;
                        }
                    }
                }
                return wartosc;
            }
            
        }
        public Dictionary<int, double> getWartoscOceny(Models.Oceny[] oceny, Pracownicy[] pracownicy, int Integer = 0, int Typ = 0)
        {
            double wartosc = 0;
            Dictionary<int, double> dict = new Dictionary<int, double>();


            if (Integer == 0 || Typ == 0)
            {
                foreach (var item in pracownicy)
                {
                    var prac = oceny.Where(m => m.Id_Pracownicy == item.Id_Pracownicy);
                    wartosc = (double)prac.Sum(m => m.Wartosc);
                    dict.Add(item.Id_Pracownicy, wartosc);
                }
                return dict;
            }
            else
            {

                foreach (var item in pracownicy)
                {
                    var prac = oceny.Where(m => m.Id_Pracownicy == item.Id_Pracownicy);
                    switch (Typ)
                    {
                        case 1:
                            {
                                wartosc = (double)prac.Sum(m => (m.Wartosc - Integer));
                                dict.Add(item.Id_Pracownicy, wartosc);
                            }
                            break;
                        case 2:
                            {
                                wartosc = (double)prac.Sum(m => (m.Wartosc * Integer));
                            }
                            break;
                        case 3:
                            {
                                wartosc = (double)prac.Sum(m => (m.Wartosc / Integer));
                            }
                            break;
                        default:
                            {
                                dict.Add(0, 0);
                                return dict;
                            }
                            break;
                    }
                    dict.Add(item.Id_Pracownicy, wartosc);


                }
                return dict;
            }
        }


    }
}