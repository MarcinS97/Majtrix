using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HRRcp.BadaniaWstepne.Controls
{
    public partial class cntMenuZakres3 : System.Web.UI.UserControl
    {
        public event EventHandler SelectedItemChanged;

        public const string ALL = "ALL";

        public enum ZakrTypes : int
        {
            Undefined = default(int),
            Week,
            Day
        };
        public ZakrTypes ZakrType { get; set; }
        public int VisibleItems { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateMenuItems();
            }
        }

        private void CreateMenuItems()
        {
            DateTime curr = GetCurrentZakr();
            int sel = CreateMenuItems(curr);
            SelectedIndex = sel;
        }

        /*
        private int CreateMenuItems(int first)   // first:0,1...; -VisibleItems -> max-first, ret: SelectedIndex po zmianie
        {
            DateTime[] days = Days;
            int sel = SelectedIndex;
            Menu1.Items.Clear();
            if (days != null)
            {
                Menu1.Items.Add(new MenuItem(String.Format(tab, "Wszystko"), null));
                int max = days.Length;
                int vis = max > VisibleItems ? VisibleItems : max;  // ilosc zakładek
                if (first == -VisibleItems) first = max + first;
                else if (first < 0)
                {
                    //sel += first; 
                    sel = 1;
                    first = 0;
                }                
                int last = first + vis;
                if (last > max)
                {
                    //sel += first - (max - vis);
                    sel = vis;
                    first = max - vis;  // zawsze będzie >= 0
                    last = max;
                }
                for (int i = first; i < last; i++)
                {
                    DateTime dt = days[i];
                    string d = Tools.DateToStr(dt);
                    //string v = Tools.DateToStrDb(dt);   // nie może być bo jest robione parse
                    Menu1.Items.Add(new MenuItem(String.Format(item, d), d));
                }
                First = first;
            }
            return sel;
        }
        */

        private int CreateMenuItems(int first, DateTime? selZakr)
        {
            const string tab = "<div class=\"tabCaption\"><div class=\"tabLeft\"><div class=\"tabRight\">{0}</div></div></div>";

            DateTime[] days = Days;
            int sel = SelectedIndex;
            Menu1.Items.Clear();
            
            //Menu1.Items.Add(new MenuItem(String.Format(tab, "Wszystko"), null));
            
            Menu1.Items.Add(new MenuItem(String.Format(tab, "Wszystko"), ALL));
            
            //MenuItem itemW = new MenuItem(String.Format(tab, "Wszystko"), null);
            //itemW.Value = null;
            //Menu1.Items.Add(itemW);

            if (days != null)
            {
                int max = days.Length;
                int vis = VisibleItems < max ? VisibleItems : max;
                if (first == -VisibleItems) first = max - vis;
                if (first < 0)
                {
                    sel = 1;
                    first = 0;
                }
                int last = first + vis;
                if (last > max)
                {
                    sel = vis;
                    first = max - vis;  // zawsze będzie >= 0
                    last = max;
                }
                int idx = 1;    // Wszyscy
                for (int i = first; i < last; i++)
                {
                    DateTime dt = days[i];
                    string d = Tools.DateToStr(dt);
                    //string v = Tools.DateToStrDb(dt);   // nie może być bo jest robione parse
                    MenuItem item = new MenuItem(String.Format(tab, d), d);
                    if (selZakr != null && (DateTime)selZakr == dt)
                    {
                        //item.Selected = true;
                        sel = idx;   
                    }
                    Menu1.Items.Add(item);
                    idx++;
                }
                First = first;
            }
            else
            {
                First = -1;
            }
            return sel;
        }

        private int CreateMenuItems(DateTime selZakr)   
        {
            DateTime[] days = Days;
            int first = First;
            if (days != null)
            {
                int max = days.Length;
                int vis = max > VisibleItems ? VisibleItems : max;
                int idx = -1;
                for (int i = max - 1; i >= 0; i--)
                    if (days[i] == selZakr)
                    {
                        idx = i;
                        break;
                    }
                first = idx - vis + 2;  // przedostatnia zakładka jak się da
                if (first + vis > max)
                    first = max - vis;
            }
            else
            {
                first = 0;
            }
            return CreateMenuItems(first, selZakr);
        }

        private DateTime GetCurrentZakr()
        {
            DateTime[] days = Days;
            DateTime today = DateTime.Today;
            DateTime curr = today;
            int curridx = -1;
            if (days != null && days.Length > 0)
            {
                for (int i = days.Length - 1; i >= 0; i--)
                {
                    DateTime dt = days[i];
                    if (curridx == -1 || dt >= today)
                    {
                        curridx = i;
                        curr = dt;
                    }
                    else
                        break;
                }
            }
            SetCurrentTab(Tools.DateToStr(curr));
            DaneCurrentIndex = curridx;
            DaneCurrent = curr;
            //if (select) SelectedIndex = curridx - First + 1;  // scroll to index jeszcze powinno być
            return curr;
        }

        public void SelectAll()
        {
            SelectedIndex = 0;
        }

        public void CheckReload(object zakr)  // zakr-nowa data do zaznaczenia zakładki
        {
            string z1 = zakr.ToString().Replace("'", "");       // przychodzi w ''
            DateTime znew = (DateTime)Tools.StrToDateTime(z1);  // musi być
            string z2 = _SelectedZakr;
            //if (String.IsNullOrEmpty(z2) || (DateTime)Tools.StrToDateTime(z2) != znew)
            if (z2 == ALL || (DateTime)Tools.StrToDateTime(z2) != znew)
            {
                Days = null;
                int sel = CreateMenuItems(znew);
                SelectedIndex = sel;
            }
        }


        //public string GetTextFromMVal(object obj)
        //{
        //    MenuItem m = (MenuItem)obj;
        //    long l = long.Parse(m.Text);
        //    if (l == 0)
        //        return "Wszystkie";
        //    return Tools.DateToStr(DateTime.FromBinary(l));
        //}
        //-------------------
        /*
        dane:   0 1 2 3 4 5 6 7 8 9
        zakładki:   all 4 5 6 7 8 9
        Items.Index   0 1 2 3 4 5 6
                                  ^
        First:          4
        SelctedIndex:             6            
        DaneSelIndex:             9 (First + SelectedIndex - 1
        CurrentIndex:             9 (w dane)         
        */
        private void CheckEnabled()
        {
            bool p3, p2, p1, n1, n2, n3;
            int first, sel, max, count;
            first = First;
            sel = SelectedIndex;
            max = Days.Length;
            count = Menu1.Items.Count - 1;  // Wszystkie
            p2 = first > 0 && sel != 0;
            p1 = (first > 0 || sel > 1) && sel != 0;
            p3 = first > 0 && count < max;
            n2 = first + count < max && sel != 0;
            n1 = (first + count < max || sel < count) && sel != 0;
            n3 = first + count < max && count < max;
            tabContent.Items[0].Enabled = p3;
            tabContent.Items[1].Enabled = p2;
            tabContent.Items[2].Enabled = p1;
            tabContent.Items[4].Enabled = n1;
            tabContent.Items[5].Enabled = n2;
            tabContent.Items[6].Enabled = n3;
        }

        private void SetCurrentTab(string text)
        {
            tabContent.Items[3].Text = text;
        }

        private void TriggerSelectedChanged()
        {
            CheckEnabled();
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, new EventArgs());
        }

        //--------------------------------------
        protected void Menu1_DataBinding(object sender, EventArgs e)
        {

        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            //SelectedIndex = Menu1.Items.IndexOf(e.Item) - startOffset;
            TriggerSelectedChanged();
        }

        protected void tabContent_MenuItemClick(object sender, MenuEventArgs e)
        {
            Move(Tools.StrToInt(e.Item.Value, -9));
        }

        //-------------------------------------
        private void Move(int dir)
        {
            int idx, sel;
            switch (dir)
            {
                case -3:
                    sel = CreateMenuItems(0, null);
                    SelectedIndex = 1;
                    break;
                case -2:
                    sel = CreateMenuItems(First - VisibleItems, null);
                    SelectedIndex = sel;
                    break;
                case -1:
                    idx = SelectedIndex - 1;
                    if (idx == 0)  // Wszystko
                    {
                        sel = CreateMenuItems(First - 1, null);
                        SelectedIndex = sel;
                    }
                    else
                        SelectedIndex = idx;
                    break;
                case 0:     // pytanie czy nie zrobić reload - odświeży ... ?
                    sel = CreateMenuItems(DaneCurrent);
                    SelectedIndex = sel;
                    //SelectedIndex = DaneCurrentIndex - First + 1;
                    break;
                case 1:
                    idx = SelectedIndex + 1;
                    if (idx == Menu1.Items.Count)
                    {
                        sel = CreateMenuItems(First + 1, null);
                        SelectedIndex = sel;
                    }
                    else
                        SelectedIndex = idx;
                    break;
                case 2:
                    sel = CreateMenuItems(First + VisibleItems, null);
                    SelectedIndex = sel;
                    break;
                case 3:
                    sel = CreateMenuItems(-VisibleItems, null);
                    SelectedIndex = Menu1.Items.Count - 1;
                    break;
            }
        }

        //-----------------------------------
        private int FFirst = -1;
        public int First
        {
            set
            {
                FFirst = value;
                ViewState["first"] = value;
            }
            get
            {
                if (FFirst == -1)
                    FFirst = Tools.GetInt(ViewState["first"], 0);
                return FFirst;
            }
        }

        private int SelectedIndex    //index w Items
        {
            get 
            { 
                for (int i = 0; i < Menu1.Items.Count; i++)
                    if (Menu1.Items[i].Selected)
                        return i;
                return -1;
            }
            set
            {
                if (value < 0) value = 0;
                else if (value >= Menu1.Items.Count) value = Menu1.Items.Count - 1;
                if (!Menu1.Items[value].Selected)
                {
                    Menu1.Items[value].Selected = true;
                    TriggerSelectedChanged();
                }
            }
        }

        private int DaneSelectedIndex
        {
            get { return First + SelectedIndex - 1; }
        }

        public int DaneCurrentIndex   //w skali wszystich, jak nie ma to -1 - zaznacza "Wszystko"
        {
            set { ViewState["curridx"] = value; }
            get { return Tools.GetInt(ViewState["curridx"], -1); }
        }

        public DateTime DaneCurrent   
        {
            set { ViewState["curr"] = value; }
            get { return Tools.GetDateTime(ViewState["curr"], DateTime.Today); }
        }
        //---------------------------
        public DateTime[] Days
        {
            get 
            { 
                DateTime[] days = Tools.GetDateTimeA(ViewState["days"]);
                if (days == null)
                {
                    DataSet ds = db.getDataSet("select distinct Zakr from BadaniaWst order by Zakr");
                    int cnt = db.getCount(ds);
                    DateTime today = DateTime.Today;
                    DateTime curr = DateTime.Today;
                    if (cnt > 0)
                    {
                        days = new DateTime[cnt];
                        for (int i = 0; i < cnt; i++)
                        {
                            DateTime d = (DateTime)db.getDateTime(db.getRow(ds, i), 0);
                            days[i] = d;
                            //if (d <= today) curr = d;
                        }
                        Days = days;
                    }
                }
                return days;
            }
            set { ViewState["days"] = value; }
        }

        //-----------------------
        public string _SelectedValue     // zwraca fragment do filtra
        {
            get 
            { 
                if (Menu1.Items.Count == 0 || Menu1.Items[0].Selected)   // ALL
                    return "";   // nie może być null
                else
                    return string.Format("{{0}} = '{0}'", Menu1.SelectedValue);
            }
        }

        public string _SelectedZakr
        {
            get { return Menu1.SelectedValue; }
        }

        public bool IsFilterSelected   // dla kompatybilności
        {
            //get { return Menu1.SelectedValue != "0"; }
            get { return !Menu1.Items[0].Selected; }
        }
    }
}




















/*


        public int DefOffset { get; set; }
        private const int startOffset = 1;



        public bool IsFilterSelected
        {
            get { return Menu1.SelectedValue != "0"; }
        }
        
        public string SelectedValue
        {
            get { return Menu1.SelectedValue; } 
        }

        public string SelectedZakr
        {
            get
            {
                if (ViewState["SelectedZakr"] == null)
                    ViewState["SelectedZakr"] = "";
                return (string)ViewState["SelectedZakr"];
            }
            private set
            {
                ViewState["SelectedZakr"] = value;
            }
        }
        
        public int SelectedIndex
        {
            get
            {
                if (ViewState["SelectedIndex"] == null)
                    ViewState["SelectedIndex"] = -1;
                int i = (int)ViewState["SelectedIndex"];
#if DEBUG
                var test = Menu1.Items.Cast<MenuItem>().Select((a, ii) => new { Item = a, Index = ii }).FirstOrDefault(a => a.Item.Selected);
#endif
                if (i < startOffset || !Menu1.Items[i].Selected)
                    return -1;
                return (int)ViewState["SelectedIndex"] - startOffset;
            }
            private set
            {
                if(value >= 0)
                    ViewState["SelectedIndex"] = value + startOffset;
                else
                    ViewState["SelectedIndex"] = -1;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        DateTime GetCurrentZakr()
        {
            DateTime r = DateTime.Now;
            switch(ZakrType)
            {
                case ZakrTypes._Week:
                    return r.AddDays(1 - (int)r.DayOfWeek);
                case ZakrTypes.Day:
                    DateTime[] dt = Days;  // załaduje tu
                    return CurrentZakr;
                default:
                    throw new NotImplementedException();
            }
        }

        DateTime moveZakr(DateTime dt, int n)
        {
            switch (ZakrType)
            {
                case ZakrTypes._Week:
                    return dt.AddDays(7 * n);
                case ZakrTypes.Day:
                    DateTime[] d = Days;
                    if (d != null)
                    {
                        int idx = -1;
                        for (int i = 0; i < d.Length; i++)
                            if (d[i] == dt)
                            {
                                idx = i;
                                break;
                            }
                        idx = idx + n;
                        if (idx < 0) idx = 0;
                        else if (idx >= d.Length) idx = d.Length - 1;
                        return d[idx];
                    }
                    else
                        return dt;                    
                default:
                    throw new NotImplementedException();
            }
        }

        void CreateMenuItems()
        {
            //SelectedIndex = (VisibleItems - 1) / 2 + 1;
            //CreateMenuItems(moveZakr(GetCurrentZakr(), ((VisibleItems - 1) / -2)));

            if (DefOffset == 0)
            {
                SelectedIndex = 0;
                CreateMenuItems(GetCurrentZakr());
            }
            else if (DefOffset < 0)
            {
                int i = VisibleItems + DefOffset;
                SelectedIndex = i;
                CreateMenuItems(moveZakr(GetCurrentZakr(), -i));
            }
            else
            {
                SelectedIndex = DefOffset;
                CreateMenuItems(moveZakr(GetCurrentZakr(), -DefOffset));
            }
        }
        void CreateMenuItems(DateTime first)
        {
            Menu1.Items.Clear();
            DateTime j = first;
            Menu1.Items.Add(new MenuItem("0", "0"));
            for (int i = 0; i < VisibleItems; i++)
            {
                DateTime dtFrom = j;
                j = moveZakr(j, 1);
                DateTime dtTo = j;
                MenuItem Item = new MenuItem(dtFrom.ToBinary().ToString(), dtTo.ToBinary().ToString());
                Menu1.Items.Add(Item);
            }
            if (DefOffset == 0)
            {
                Menu1.Items[startOffset].Selected = true;
            }
            else if (DefOffset < 0)
            {
                Menu1.Items[VisibleItems + DefOffset + startOffset].Selected = true;
            }
            else
            {
                Menu1.Items[DefOffset + startOffset].Selected = true;
            }
            SelectedChanged();
        }
        void ChangeMenuItems()
        {
            if(DefOffset == 0)
            {
                SelectedIndex = 0;
                ChangeMenuItems(GetCurrentZakr(), 0);
            }
            else if(DefOffset < 0)
            {
                int i = VisibleItems + DefOffset;
                SelectedIndex = i;
                ChangeMenuItems(moveZakr(GetCurrentZakr(), -i), i);
            }
            else
            {
                SelectedIndex = DefOffset;
                ChangeMenuItems(moveZakr(GetCurrentZakr(), -DefOffset), DefOffset);
            }
        }
        void ChangeMenuItems(DateTime first, int IndexToSel)
        {
            DateTime j = first;
            for (int i = 1; i < VisibleItems + 1; i++)
            {
                DateTime dtFrom = j;
                j = moveZakr(j, 1);
                DateTime dtTo = j;
                Menu1.Items[i].Text = dtFrom.ToBinary().ToString();
                Menu1.Items[i].Value = dtTo.ToBinary().ToString();
            }
            Menu1.Items[IndexToSel + startOffset].Selected = true;
            SelectedChanged();
        }

        public string GetTextFromMVal(object obj)
        {
            MenuItem m = (MenuItem)obj;
            long l = long.Parse(m.Text);
            if (l == 0)
                return "Wszystkie";
            return Tools.DateToStr(DateTime.FromBinary(l));
        }
        public string GetValueFromMVal(object obj)
        {
            MenuItem m = (MenuItem)obj;
            long l = long.Parse(m.Value);
            if (l == 0)
                return "";
            return string.Format("{{0}} >= '{0}' AND {{0}} < '{1}'",
                Tools.DateToStr(DateTime.FromBinary(long.Parse(m.Text))),
                Tools.DateToStr(DateTime.FromBinary(long.Parse(m.Value))));
        }

        public void SelectAll()
        {
            Menu1.Items[0].Selected = true;
            SelectedIndex = -1;
            SelectedChanged();
        }
        public void PageLeft()
        {
            MenuItem m = Menu1.Items[1];
            DateTime dt = moveZakr(DateTime.FromBinary(long.Parse(m.Value)), -VisibleItems - 1);
            ChangeMenuItems(dt, SelectedIndex);
        }
        public void PageRight()
        {
            MenuItem m = Menu1.Items[VisibleItems];
            DateTime dt = moveZakr(DateTime.FromBinary(long.Parse(m.Value)), 0);
            ChangeMenuItems(dt, SelectedIndex);
        }
        public void MoveLeft()
        {
            if (SelectedIndex < 0)
            {
                return;
            }
            int ni = startOffset + SelectedIndex - 1;
            if (ni < startOffset + 1)
            {
                DateTime dtFrom = moveZakr(DateTime.FromBinary(long.Parse(Menu1.Items[startOffset].Text)), -1);
                DateTime dtTo = moveZakr(DateTime.FromBinary(long.Parse(Menu1.Items[startOffset].Value)), -1);

                for (int i = VisibleItems + startOffset - 1; i > startOffset; i--)
                {
                    Menu1.Items[i].Text = Menu1.Items[i - 1].Text;
                    Menu1.Items[i].Value = Menu1.Items[i - 1].Value;
                }
                Menu1.Items[startOffset].Text = dtFrom.ToBinary().ToString();
                Menu1.Items[startOffset].Value = dtTo.ToBinary().ToString();
            }
            else
            {
                SelectedIndex--;
                Menu1.Items[ni].Selected = true;
            }
            SelectedChanged();
        }
        public void MoveRight()
        {
            if (SelectedIndex < 0)
            {
                return;
            }
            int ni = startOffset + SelectedIndex + 1;
            int tt = startOffset + VisibleItems - 1;
            if (ni > tt - 1)
            {
                DateTime dtFrom = moveZakr(DateTime.FromBinary(long.Parse(Menu1.Items[tt].Text)), 1);
                DateTime dtTo = moveZakr(DateTime.FromBinary(long.Parse(Menu1.Items[tt].Value)), 1);

                for (int i = startOffset; i < tt; i++)
                {
                    Menu1.Items[i].Text = Menu1.Items[i + 1].Text;
                    Menu1.Items[i].Value = Menu1.Items[i + 1].Value;
                }
                Menu1.Items[tt].Text = dtFrom.ToBinary().ToString();
                Menu1.Items[tt].Value = dtTo.ToBinary().ToString();
            }
            else
            {
                SelectedIndex++;
                Menu1.Items[ni].Selected = true;
            }
            SelectedChanged();
        }
        public void MoveNow()
        {
            ChangeMenuItems();
            SelectedChanged();
        }














        public DateTime _GetZakrFromDate(DateTime date)
        {
            return _GetZakrFromDate(date, ZakrType); //ZakrTypes.Week);
        }

        public DateTime _GetZakrFromDate(DateTime date, ZakrTypes type)
        {
            switch(type)
            {
                case ZakrTypes._Week:
                    return date.AddDays(1 - (int)date.DayOfWeek);
                case ZakrTypes.Day:
                    /*
                    DateTime[] d = Days;
                    DateTime zakr = date;
                    if (d != null)
                    {
                        int idx = -1;
                        for (int i = 0; i < d.Length; i++)
                            if (d[i] == dt)
                            {
                                idx = i;
                                break;
                            }
                        idx = idx + n;
                        if (idx < 0) idx = 0;
                        else if (idx >= d.Length) idx = d.Length - 1;
                        return d[idx];
                    }
                    else
                        * /
                        return date;                    
                default:
                    throw new NotImplementedException();
            }
        }

*/