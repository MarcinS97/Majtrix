using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Kwitek
{
    public partial class cntImieNazwisko : System.Web.UI.UserControl
    {
        public const int moPanelPrac = 0;
        public const int moPanelKier = 1;
        int FMode = moPanelPrac;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public bool IsPeselVisible
        {
            get
            {
#if IQOR && !CO && !DEMO && !PRON
                return true;
#else
                return false;
#endif
            }
        }
        
        public void Prepare(string pracid)
        {
            SqlDataSource1.SelectCommand = 
@"SELECT Imie,
Nazwisko,
KadryId as LpLogo,
Nick as Pesel
FROM Pracownicy
WHERE KadryId = @par";
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("par", pracid);
            dlHeader.DataBind();
        }

        protected void dlHeader_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DataRowView drv = (DataRowView)e.Item.DataItem;
            }
        }

        public string IdPracownika
        {
            set { ViewState["pracid"] = value; }
            get { return Tools.GetStr(ViewState["pracid"]); }
        }

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

    }
}