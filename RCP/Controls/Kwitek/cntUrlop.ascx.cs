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
    public partial class cntUrlop : System.Web.UI.UserControl
    {
        public const int moPanelPrac = 0;
        public const int moPanelKier = 1;
        int FMode = moPanelPrac;

        protected void Page_Init(object sender, EventArgs e)
        {
#if IQOR
            Grid.Prepare(gvLimity);
#endif
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if IQOR
                //gvLimity.Visible = true;   // jak zool odpalam to i tak grid znika ... -> w aspx 
                SqlDataSource2.SelectParameters["KadryId"].DefaultValue = App.KwitekKadryId;
#endif
            }
        }

        public bool OldWymiaryVisible
        {
            get
            {
#if IQOR
                return true;
                return false;
#else
                return true;
#endif
            }
        }

        public bool AssecoWymiaryVisible
        {
            get
            {
#if IQOR
                return true;
#else
                return false;
#endif
            }
        }

        public bool IsPeselVisible
        {
            get
            {
#if SPX
                return false;
#else
                return true;
#endif
            }
        }
        
        public void Prepare(string data, bool list)
        {
            if (String.IsNullOrEmpty(data))
                data = Tools.DateToStr(DateTime.Today);
            string d1 = data.Substring(0, 4) + "-01-01";

            switch (FMode)
            {
                case moPanelPrac:
                    SqlDataSource1.SelectCommand = RepUrlop.GetSql(d1, data,   //<<<< to powinno się pobrac z Asseco
                        ",UPPER(P.Nazwisko) as Nazwisko, UPPER(P.Imie) as Imie, P.KadryId as LpLogo, P.Nick as PESEL"
#if IQOR
                        + ",U.DataZwiekszenia"
#endif                        
                        ,
#if SPX
                        //String.Format("and P.KadryId + '.' = '{0}.'", App.KwitekKadryId));    //20150114 porównanie zwraca true jak w jednym P.KadryId są spacje na końcu !!!   
                        String.Format("and P.KadryId = '{0}'", App.KwitekKadryId));             //jednak wyczyszcze baze ...
#else
                        String.Format("and P.KadryId = '{0}'", App.KwitekKadryId));
#endif
                    dlHeader.DataBind();
                    if (list)
                        PracUrlop1.Prepare(App.KwitekKadryId);
                    else
                    {
                        lbLista.Visible = false;
                        PracUrlop1.Visible = false;
                    }
                    break;
                case moPanelKier:
                    lbLista.Visible = false;
                    PracUrlop1.Visible = false;
                    break;
            }
        }

        public void _PrepareKier(string data, string pracId)
        {
            IdPracownika = pracId;
            if (String.IsNullOrEmpty(data))
                data = Tools.DateToStr(DateTime.Today);
            string d1 = data.Substring(0, 4) + "-01-01";

            switch (FMode)
            {
                case moPanelPrac:
                    break;
                case moPanelKier:
                    SqlDataSource1.SelectCommand = RepUrlop.GetSql(d1, data,   //<<<< to powinno się pobrac z Asseco
                        ",UPPER(P.Nazwisko) as Nazwisko, UPPER(P.Imie) as Imie, P.KadryId as LpLogo, null as PESEL"
#if IQOR
                        + ",U.DataZwiekszenia"
#endif
                        ,
                        String.Format("and P.Id = {0}", IdPracownika));
                    dlHeader.DataBind();
                    lbLista.Visible = false;
                    PracUrlop1.Visible = false;
                    break;
            }
        }

        //---------------------------
        protected void dlHeader_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string pracId = drv["Id"].ToString();
                //----- kierownik / pracownik ------
                bool kier = Base.getBool(drv["Kierownik"], false);
                Tools.SetControlVisible(e.Item, "PracownikLabel", !kier);
                Tools.SetControlVisible(e.Item, "lbtPracownik", kier);
                //----- wartości -----
                object o1 = drv["UrlopNom"];
                //object o1r= drv["UrlopNomRok"];
                object o2 = drv["UrlopZaleg"];
                object o3 = drv["UrlopWyk"];
                object o4 = drv["WykDoDn"];

                double d1 = db.isNull(o1) ? 0 : Convert.ToDouble(o1);
                //double d1r= db.isNull(o1r)? 0 : Convert.ToDouble(o1r);
                double d2 = db.isNull(o2) ? 0 : Convert.ToDouble(o2);
                double d3 = db.isNull(o3) ? 0 : Convert.ToDouble(o3);
                double d4 = db.isNull(o4) ? 0 : Convert.ToDouble(o4);

                string dd = Worktime.Round05(d1 + d2 - d3, 2).ToString().Replace(".", ",");
                if (dd.EndsWith(".00")) dd = dd.Remove(dd.Length - 3);
                Tools.SetText(e.Item, "lbDoWyk", dd);

                dd = Worktime.Round05(d1 + d2 - d4, 2).ToString().Replace(".", ",");
                if (dd.EndsWith(".00")) dd = dd.Remove(dd.Length - 3);
                Tools.SetText(e.Item, "lbDoWykNaDzien", dd);

                Tools.SetControlVisible(e.Item, "paDaneOsobowe", FMode == moPanelPrac);
#if IQOR
                object odz = drv["DataZwiekszenia"];
                if (db.isNull(odz))
                {
                    paDataZwiekszenia.Visible = false;
                    lbDataZwiekszenia.Text = null;
                }
                else
                {
                    paDataZwiekszenia.Visible = true;
                    lbDataZwiekszenia.Text = Tools.DateToStr(odz);
                }
#endif
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

        public int UMode
        {
            set { PracUrlop1.Mode = value; }
        }
    }
}