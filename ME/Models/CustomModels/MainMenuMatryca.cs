using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRRcp.Areas.ME.Models.Interfaces;

namespace HRRcp.Areas.ME.Models.CustomModels
{
    
    public class MainMenuMatryca : IMenuSet
    {
        MatrycaMVC bazaMatryca = new MatrycaMVC();
        List<ME_SqlMenu> mainMenu = new List<ME_SqlMenu>();



        public bool setMenu()
        {
            var menuTEMP = bazaMatryca.SqlMenu.Where(x => x.Grupa == "MenuMatryca");

            foreach (var item in menuTEMP)
            {
                mainMenu.Add(item);
            }
            if (mainMenu.Count() > 0) return true;
            else return false;
        }


        public IEnumerable<object> getMenuSet()
        {
            var menu = (IEnumerable<ME_SqlMenu>)mainMenu;
            return menu;
        }
    }
}