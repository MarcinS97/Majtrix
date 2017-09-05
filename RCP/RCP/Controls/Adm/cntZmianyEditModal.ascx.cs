using HRRcp.App_Code;
using HRRcp.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Adm
{
    public partial class cntZmianyEditModal : System.Web.UI.UserControl
    {
        public event EventHandler Saved;
        protected void Page_Load(object sender, EventArgs e)
        {

            PrepareColorPicker();
        }

        void PrepareColorPicker()
        {
            Tools.ExecuteJavascript(String.Format("prepareColorPicker('{0}');", hidColor.ClientID));
        }

        public void Show(int id)
        {
            cntModal.Show();
            DropDownList ddl = ddlOd.DdlValue;
            DropDownList ddl2 = ddlDo.DdlValue;
            
            if (ddl2 != null)
            {
                Tools.FillTime2(ddl2, 0, 5);
            }
            if (ddl != null)
            {
                Tools.FillTime2(ddl, 0, 5);
            }


            //if (KolorImageButton != null)
            //{
            //    KolorImageButton.Attributes.Add("onClick", "javascript:jscolor.init2(this);return false;");
            //}

            this.ZmianaId = id.ToString();
            DataRow dr = db.Select.Row(SqlDataSource1, id);
            dbField.FillData(this, dr, 0, 0, 0, dbField.moEdit);


            SymbolLabel.BackColor = GetColorNull(dr["Kolor"].ToString());
            hidColor.Value = dr["Kolor"].ToString();
            
            ShowHideNadg(dr["TypZmiany"].ToString());
            App.FillStawki(ddlStawka.DdlValue, dr["Stawka"].ToString());
            App.FillStawki(ddlNadgodzinyDzien.DdlValue, dr["NadgodzinyDzien"].ToString());
            App.FillStawki(ddlNadgodzinyNoc.DdlValue, dr["NadgodzinyNoc"].ToString());

            hidInneCzasy.Value = dr["InneCzasy"].ToString();

            this.ddlDo.Value = DateTime.Today.Add(TimeSpan.Parse(dr["Do"].ToString())).ToString();
            this.ddlOd.Value = DateTime.Today.Add(TimeSpan.Parse(dr["Od"].ToString())).ToString();
        }

        public void Show()
        {
            cntModal.Show();

            DropDownList ddl = ddlOd.DdlValue;
            DropDownList ddl2 = ddlDo.DdlValue;

            if (ddl2 != null)
            {
                Tools.FillTime2(ddl2, 0, 5);
            }
            if (ddl != null)
            {
                Tools.FillTime2(ddl, 0, 5);
            }


            //if (KolorImageButton != null)
            //{
            //    KolorImageButton.Attributes.Add("onClick", "javascript:jscolor.init2(this);return false;");
            //}
            hidInneCzasy.Value = "";
            this.ZmianaId = null;
            dbField.FillData(this, null, 0, 0, 0, dbField.moEdit);
        }

        public void Hide()
        {
            cntModal.Close();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            if (String.IsNullOrEmpty(ZmianaId)) //insert
            {
                dbField.dbInsert(db.con, this, "Zmiany", "InneCzasy,HideZgoda,ZgodaNadg,Kolor", db.nullParamStr(hidInneCzasy.Value), 1, 1,db.nullParamStr(hidColor.Value));

            }
            else //update
            {
                db.update("Zmiany",0,"InneCzasy,Kolor","Id = " + ZmianaId,db.nullParamStr(hidInneCzasy.Value),db.nullParamStr(hidColor.Value));
                dbField.dbUpdate(db.con, this, "Zmiany", "Id=" + ZmianaId, null, null);
            }
            TriggerSaved();
            
            Hide();
        }

        void TriggerSaved()
        {
            if (Saved != null)
                Saved(null, EventArgs.Empty);
        }

        public String ZmianaId
        {
            get { return ViewState["vZmianaId"] as String; }
            set { ViewState["vZmianaId"] = value; }

        }

        public void Close(int id)
        {
            cntModal.Close();
        }

        protected void ddlTypZmiany_Changed(object sender, EventArgs e)
        {
            dbField dbF = sender as dbField;
            ShowHideNadg(dbF.Value);

        }


        public void ShowHideNadg(string zgodaNadg)
        {
            if (zgodaNadg == "1")
            {
                dvNadgodziny.Visible = true;
            }
            else
            {
                dvNadgodziny.Visible = false;
            }
            if (zgodaNadg == "2")
            {
                dvCzasStawka.Visible = false;
            }
            else
            {
                dvCzasStawka.Visible = true;
            }

        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {


            if (hidInneCzasy != null)
            {
                string zmiana = (sender as LinkButton).CommandArgument;
                if (!String.IsNullOrEmpty(zmiana))
                {

                    string id = zmiana.Split(';')[0];
                    string sidx = zmiana.Split(';')[1];
                    int idx = Convert.ToInt32(sidx);

                    List<String> zmianyList = hidInneCzasy.Value.Split(';').ToList();

                    for (int i = 0; i < zmianyList.Count; i++)
                    {
                        if (zmianyList[i] == id && i == idx)
                        {
                            zmianyList.RemoveAt(i);
                            break;
                        }
                    }

                    hidInneCzasy.Value = String.Join(";", zmianyList.ToArray());

                }
            }
        }

        protected void lnkAddInneCzasy_Click(object sender, EventArgs e)
        {


            if (hidInneCzasy != null)
            {
                if (!String.IsNullOrEmpty(hidInneCzasy.Value))
                {
                    hidInneCzasy.Value = hidInneCzasy.Value + ";" + teTimeIn.Seconds;
                }
                else
                {
                    hidInneCzasy.Value = teTimeIn.Seconds.ToString();
                }
            }
        }


        public Color GetColorNull(string color)
        {
            try
            {
                //return ColorTranslator.FromHtml(color.StartsWith("#") ? color : "#" + color);
                return ColorTranslator.FromHtml(color);
            }
            catch
            {
                return Color.Transparent;
            }
        }

        public string GetColorNoHash(string color)
        {
            try
            {
                return color.StartsWith("#") ? color.Substring(1) : color;
            }
            catch
            {
                return "Transparent";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            db.Execute("delete from ZMIANY WHERE ID = {0}",ZmianaId);
            TriggerSaved();

            Hide();
        }


    }
}