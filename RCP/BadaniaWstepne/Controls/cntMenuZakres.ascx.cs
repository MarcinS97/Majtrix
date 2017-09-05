using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.BadaniaWstepne.Controls
{
    public partial class cntMenuZakres : System.Web.UI.UserControl
    {
        public enum ZakrTypes : int
        {
            Undefined = default(int),
            Week
        };
        public int VisibleItems { get; set; }
        public ZakrTypes ZakrType { get; set; }
        public int DefOffset { get; set; }
        private const int startOffset = 1;
        public bool IsFilterSelected
        {
            get { return Menu1.SelectedValue != "0"; }
        }
        public event EventHandler SelectedItemChanged;

        public string SelectedValue
        {
            get
            {
                if (ViewState["SelectedValue"] == null)
                    ViewState["SelectedValue"] = "";
                return (string)ViewState["SelectedValue"];
            }
            private set
            {
                ViewState["SelectedValue"] = value;
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            tabContent.Items[2].Text = Tools.DateToStr(GetCurrentZakr());
            if (!IsPostBack)
                CreateMenuItems();
        }

        DateTime GetCurrentZakr()
        {
            DateTime r = DateTime.Now;
            switch(ZakrType)
            {
                case ZakrTypes.Week:
                    return r.AddDays(1 - (int)r.DayOfWeek);
                default:
                    throw new NotImplementedException();
            }
        }

        DateTime moveZakr(DateTime dt, int n)
        {
            switch (ZakrType)
            {
                case ZakrTypes.Week:
                    return dt.AddDays(7 * n);
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
        public void SelectedChanged()
        {
            if (SelectedIndex < 0)
            {
                SelectedValue = "";
                SelectedZakr = "";
                tabContent.Items[1].Enabled = tabContent.Items[3].Enabled = false;
            }
            else
            {
                SelectedValue = GetValueFromMVal(Menu1.Items[SelectedIndex + startOffset]);
                SelectedZakr = Tools.DateToStr(DateTime.FromBinary(long.Parse(Menu1.Items[SelectedIndex + startOffset].Text)));
                tabContent.Items[1].Enabled = tabContent.Items[3].Enabled = true;
            }
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, new EventArgs());
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectedIndex = Menu1.Items.IndexOf(e.Item) - startOffset;
            SelectedChanged();
        }

        protected void tabContent_MenuItemClick(object sender, MenuEventArgs e)
        {
            int i = int.Parse(e.Item.Value);
            switch(i)
            {
                case -2:
                    PageLeft();
                    break;
                case -1:
                    MoveLeft();
                    break;
                case 0:
                    MoveNow();
                    break;
                case 1:
                    MoveRight();
                    break;
                case 2:
                    PageRight();
                    break;
            }
        }

        public static DateTime GetZakrFromDate(DateTime date)
        {
            return GetZakrFromDate(date, ZakrTypes.Week);
        }
        public static DateTime GetZakrFromDate(DateTime date, ZakrTypes type)
        {
            switch(type)
            {
                case ZakrTypes.Week:
                    return date.AddDays(1 - (int)date.DayOfWeek);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}