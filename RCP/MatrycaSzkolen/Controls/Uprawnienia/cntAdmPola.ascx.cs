using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.MatrycaSzkolen.App_Code;

namespace KDR.Controls.Uprawnienia
{

    public partial class cntAdmPola : System.Web.UI.UserControl
    {
        //const int poLastId = 18;

        [Serializable]
        public class Field
        {
            public Field(int No, bool Active, string Description)
            {
                this.No = No;
                this.Active = Active;
                this.Description = Description;
                this.Checked = false;
            }
            public int No { get; set; }
            public bool Active { get; set; }
            public string Description { get; set; }
            public bool Checked { get; set; }
        }


        Field[] StartFields = {
                         new Field(Pola.po0, false, "po0")
                         ,new Field(Pola.po1, false, "po1")
                         ,new Field(Pola.poNumer, true, "Numer")
                         ,new Field(Pola.poDataWaznosci, true, "Data ważności")
                         ,new Field(Pola.po4, false, "po4")
                         ,new Field(Pola.poKategoria, true, "Kategoria")
                         ,new Field(Pola.poDataZdobycia, true, "Data zdobycia")
                         ,new Field(Pola.poDataWaznosciPsychotestow, true, "Data ważności psychotestów")
                         ,new Field(Pola.poDataWaznosciBadanLekarskich, true, "Data ważności badań lekarskich")
                         ,new Field(Pola.poDataWaznosciUmowy, false, "Data ważności umowy")
                         ,new Field(Pola.poUmowaLojalnosciowa, false, "Umowa lojalnościowa")
                         ,new Field(Pola.po11, false, "po11")
                         ,new Field(Pola.po12, false, "po12")
                         ,new Field(Pola.poUwagi, true, "Uwagi")

                         ,new Field(Pola.poDataRozpoczecia, true, "Data rozpoczęcia")
                         ,new Field(Pola.poDataZakonczenia, true, "Data zakończenia")
                         ,new Field(Pola.poNazwa, true, "Nazwa")
                         ,new Field(Pola.poDodatkoweWarunki, true, "Dodatkowe warunki")
                         ,new Field(Pola.poSymbol, true, "Symbol")
                         ,new Field(Pola.poStatus, true, "Status")
                         ,new Field(Pola.poTermin, true, "Termin")
                         ,new Field(Pola.poEwaluacjaMonitDni, true, "Monit")
                         ,new Field(Pola.poPrzeszkolono, true, "Przeszkolono")
                         ,new Field(Pola.poTrener, true, "Trener")
                              };




        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        public void Prepare(string uprId, bool editable)
        {
            this.UprId = uprId;
            this.Fields = StartFields;
            this.Editable = editable;
            divImport.Visible = editable;

            SetFields(uprId);
        }

        void SetFields(string uprId)
        {
            this.StrFields = db.Select.Scalar(dsSelectFields, uprId);
            for (int i = 0; i < StrFields.Length && i < Fields.Length; i++)
            {
                Fields[i].Checked = (StrFields[i] == '1');
            }
            rpFields.DataSource = Fields;
            rpFields.DataBind();
        }

        protected string BuildFields()
        {
            string fields = string.Empty;
            for (int i = 0; i < Fields.Length; i++)
            {
                fields += (Fields[i].Active && Fields[i].Checked) ? "1" : "0";
            }
            return fields;
        }

        protected void cbCheked_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (cb != null)
            {
                HiddenField hidNo = cb.Parent.FindControl("hidNo") as HiddenField;
                string no = hidNo.Value;//Tools.GetValue(cb.Parent, "hidNo");
                if (!String.IsNullOrEmpty(no))
                {
                    int ino = -1;
                    if (int.TryParse(no, out ino))
                    {
                        if (ino != -1)
                        {
                            Fields[ino].Checked = cb.Checked;
                            string newFields = BuildFields();
                            StrFields = newFields;
                            db.Execute(dsSave, UprId, db.strParam(StrFields));
                        }
                    }
                }
            }
        }

        protected void ddlImport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(ddlImport.SelectedValue))
                Import(ddlImport.SelectedValue);
            ddlImport.SelectedValue = null;
        }

        void Import(string uprId)
        {
            db.Execute(dsImport, uprId, UprId);
            SetFields(uprId);
        }

        public Field[] Fields
        {
            get { return (Field[])ViewState["vFields"]; }
            set { ViewState["vFields"] = value; }

        }

        public bool Editable
        {
            get { return Tools.GetViewStateBool(ViewState["vEditable"], false); }
            set { ViewState["vEditable"] = value; }
        }

        public string StrFields
        {
            get { return (String)ViewState["vStrFields"]; }
            set { ViewState["vStrFields"] = value; }
        }

        public string UprId
        {
            get { return hidUprId.Value; }
            set { hidUprId.Value = value; }
        }

    }
}