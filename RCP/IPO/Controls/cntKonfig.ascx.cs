using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.IPO.Controls
{
    public partial class cntKonfig : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
          Tools.PrepareDicListView(lvAkceptacje, 0);
          Tools.PrepareDicListView(lvOgolne, 0);
          Tools.PrepareDicListView(lvRodzajeProduktow, 0);
          Tools.PrepareDicListView(lvJednostki, 0);
          Tools.PrepareDicListView(lvPodstawoweAkceptacje, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lvOgolne_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
  
        }
        protected void lvAkceptacje_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

        protected void lvRodzajeProduktow_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Usun":
                    string idRodzajuProduktu = lvRodzajeProduktow.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value.ToString();
                    bool rodzajProduktuIsUsed = Boolean.Parse(db.getScalar(@"SELECT 
	                                                                        CASE 
		                                                                        WHEN count('x') > 0 THEN 'True'
		                                                                        ELSE 'False'
	                                                                        END AS rodzajProduktuIsUsed
                                                                         FROM IPO_PozycjeZamowien
                                                                         WHERE IPO_PozycjeZamowien.idRodzajuProduktu = " + idRodzajuProduktu));
                    if (rodzajProduktuIsUsed)
                    {
                        Tools.ShowError("Ten rodzaj produktu nie może zostać usunięty, ponieważ został użyty w conajmniej jednym zamówieniu.");
                    }
                    else
                    {
                        db.execSQL("DELETE FROM [IPO_RodzajeProduktow] WHERE [Id] = " + idRodzajuProduktu);
                        lvRodzajeProduktow.DataBind();
                    }
                    break;
                case "Insert":
                    lvAkceptacje.DataBind();
                    break;
            }
           
        }

        protected void lvRodzajeProduktow_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }
        protected void lvPodstawoweAkceptacje_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }
        protected void lvItemIserting(object sender, ListViewInsertEventArgs e)
        {
            DropDownList RodzajProduktuDropDownList = lvAkceptacje.InsertItem.FindControl("RodzajProduktuDropDownList") as DropDownList;
            SqlDataSource2.InsertParameters["IdRodzajuProduktu"].DefaultValue = RodzajProduktuDropDownList.SelectedValue;

        }

    }
}