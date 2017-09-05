using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Adm
{
    public partial class cntKartyRcp : System.Web.UI.UserControl
    {
        public const int moShowAll = 0;
        public const int moNoRcpId = 1;    // nr karty Jabil
        public const int moNoNrKarty = 2;  // napis Siemens

#if SIEMENS
        public const int defMode = moNoRcpId;
#else
        //public const int defMode = moNoNrKarty;  //dla ZG oba widoczne
        public const int defMode = moShowAll;
#endif

        int FMode = defMode;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //----------------------------------
        public string x_GetCurrent()
        {
            string data = DateTime.Today.ToStringDb();
#if SIEMENS
            DataRow dr = db.getDataRow(String.Format("select top 1 NrKarty from PracownicyKarty where IdPracownika = {0} and {1} between Od and ISNULL(Do, '20990909')", PracId, data));
#else
            DataRow dr = db.getDataRow(String.Format("select top 1 RcpId from PracownicyKarty where IdPracownika = {0} and {1} between Od and ISNULL(Do, '20990909')", PracId, data));
#endif
            if (dr != null)
                return db.getValue(dr, 0);
            else
                return null;
        }

        public void GetCurrent(out string rcpid, out string nrkarty)
        {
            string data = DateTime.Today.ToStringDb();
            DataRow dr = db.getDataRow(String.Format("select top 1 RcpId, NrKarty from PracownicyKarty where IdPracownika = {0} and {1} between Od and ISNULL(Do, '20990909')", PracId, data));
            if (dr != null)
            {
                switch (FMode)
                {
                    default:
                    case moNoNrKarty:
                        rcpid = db.getValue(dr, 0);
                        nrkarty = null;
                        break;
                    case moNoRcpId:
                        rcpid = null;
                        nrkarty = db.getValue(dr, 1);
                        break;
                    case moShowAll:
                        rcpid = db.getValue(dr, 0);
                        nrkarty = db.getValue(dr, 1);
                        break;
                }
            }
            else
            {
                rcpid = null;
                nrkarty = null;
            }
        }
        //----------------------------------
        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {
            //bool b = FMode != moNoRcpId;
            //Tools.SetControlVisible(ListView1, "thRcpId", b);
            //b = FMode != moNoNrKarty;
            //Tools.SetControlVisible(ListView1, "thNrKarty", b);

            switch (FMode)
            {
                case moNoNrKarty:
                    Tools.SetControlVisible(ListView1, "thRcpId", true);
                    Tools.SetControlVisible(ListView1, "thNrKarty", false);
                    break;
                case moNoRcpId:
                    Tools.SetControlVisible(ListView1, "thRcpId", false);
                    Tools.SetControlVisible(ListView1, "thNrKarty", true);
                    break;
                case moShowAll:
                    Tools.SetControlVisible(ListView1, "thRcpId",   true);
                    Tools.SetControlVisible(ListView1, "thNrKarty", true);
                    Tools.SetControlVisible(ListView1, "ltRcpId",   false);
                    Tools.SetControlVisible(ListView1, "ltNrKarty", false);
                    Tools.SetControlVisible(ListView1, "ltRcpId1",  true);
                    Tools.SetControlVisible(ListView1, "ltNrKarty1",true);
                    break;
            }
        }

        private void SetControlsVisible(ListViewItem item)
        {
            bool b = FMode != moNoRcpId;
            Tools.SetControlVisible(item, "tdRcpId", b);
            Tools.SetControlEnabled(item, "RequiredFieldValidator2", b);
            AjaxControlToolkit.FilteredTextBoxExtender cnt = item.FindControl("tbFilter") as AjaxControlToolkit.FilteredTextBoxExtender;
            if (cnt != null) cnt.Enabled = b;
            b = FMode != moNoNrKarty;
            Tools.SetControlVisible(item, "tdNrKarty", b);
            Tools.SetControlEnabled(item, "RequiredFieldValidator1", b && FMode != moShowAll);   //20150104 zezwalam na pusty nr karty
            switch (FMode)
            {
                case moNoNrKarty:
                    break;
                case moNoRcpId:
                    break;
                case moShowAll:
                    break;
            }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                SetControlsVisible(e.Item);
            }
        }

        protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                SetControlsVisible(e.Item);
                Tools.SetChecked(e.Item, "cbClosePrev", true);
            }
        }
        //----------------------------------
        private static bool ValidateOverlapped(string tbName, string fldName, int typ, string pracId, string dataOd, string dataDo, string id, out string msg)
        {
            return ValidateOverlapped(tbName, "Od", "Do", fldName, typ, pracId, dataOd, dataDo, id, out msg);
        }

        private static bool ValidateOverlapped(string tbName, string fldOd, string fldDo, string fldName, int typ, string pracId, string dataOd, string dataDo, string id, out string msg)
        {
            DataRow dr;
            if (String.IsNullOrEmpty(id))                    // fld, od, do                                               do, od               od
                dr = db.getDataRow(String.Format("select top 1 {4}, {6}, {7} from {3} where IdPracownika = {0} and ISNULL({7}, {6}) >= {1} and {6} <= {2}",               pracId, dataOd, dataDo, tbName, fldName, null, fldOd, fldDo));  // jak jest 20990909 to zawsze spełnia
            else
                dr = db.getDataRow(String.Format("select top 1 {4}, {6}, {7} from {3} where IdPracownika = {0} and ISNULL({7}, {6}) >= {1} and {6} <= {2} and Id <> {5}", pracId, dataOd, dataDo, tbName, fldName, id, fldOd, fldDo));  // jak jest 20990909 to zawsze spełnia
            if (dr != null)
            {
                object d = dr[fldDo];
                msg = String.Format("Okres obowiązywania pokrywa się z już istniejącym ({1} - {2}).",
                    db.getValue(dr, 0),         // nie dodaję, trzeba by link do opisu robić np dla algorytmów
                    Tools.DateToStr(dr[fldOd]),
                    db.isNull(d) ? "bez terminu" : Tools.DateToStr(d));
                return false;
            }
            else
            {
                msg = null;
                return true;
            }
        }

        private static bool ValidateDuplicate(string tbName, string fldName, int typ, string pracId, string dataOd, string dataDo, string id, string rcpIdKarta, out string msg)
        {
            DataSet ds = db.getDataSet(String.Format(@"
declare @od datetime, @do datetime
set @od = {5}
set @do = {6}
select P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') + ': ' + convert(varchar(10), PK.Od, 20) + ' - ' + case when PK.Do is null then 'bez terminu' else convert(varchar(10), PK.Do, 20) end 
from {0} PK
inner join Przypisania R on R.IdPracownika = PK.IdPracownika and R.Status = 1 and GETDATE() between R.Od and ISNULL(R.Do, '20990909')
left join Pracownicy P on P.Id = PK.IdPracownika
--where GETDATE() between PK.Od and ISNULL(PK.Do, '20990909')
where @od <= ISNULL(PK.Do, '20990909') and PK.Od <= @do
and PK.{1} = '{2}'
and PK.Id != {4}
--and PK.IdPracownika != {3}
                ", tbName, fldName, rcpIdKarta, pracId, String.IsNullOrEmpty(id) ? "-1" : id, dataOd, dataDo));   // daty są już z '', do=null -> MaxDate 99991231
            if (db.getCount(ds) > 0)
            {
                msg = String.Format("Identyfikator RCP ({0}) jest już przypisany do:\\n{1}", rcpIdKarta, db.Join(ds, 0, "\\n"));
                return false;
            }
            else
            {
                msg = null;
                return true;
            }
        }

        public static bool Validate(string tbName, string fldName, int typ, string pracId, object dOd, object dDo, string id, object rcpIdKarta)  // fldName - póki co dowolne, ale być musi
        {
            return Validate(tbName, "Od", "Do", fldName, typ, pracId, dOd, dDo, id, rcpIdKarta);
        }

        public static bool Validate(string tbName, string fldOd, string fldDo, string fldName, int typ, string pracId, object dOd, object dDo, string id, object rcpIdKarta)  // fldName - póki co dowolne, ale być musi
        {
            if (db.isNull(dOd)) Tools.ShowError("Data początkowa nie może być pusta.");
            else
            {
                if (db.isNull(dDo)) dDo = DateTime.MaxValue;
                if ((DateTime)dOd > (DateTime)dDo) Tools.ShowError("Data końcowa nie może być wcześniejsza niż początkowa.");
                else
                {
                    string dataOd = ((DateTime)dOd).ToStringDb();
                    string dataDo = ((DateTime)dDo).ToStringDb();
                    switch (typ)
                    {
                        default:
                        case 0:     // okres nie może się pokrywać
                            string msg;
                            bool ok = ValidateOverlapped(tbName, fldOd, fldDo, fldName, typ, pracId, dataOd, dataDo, id, out msg);
                            if (!ok) Tools.ShowError(msg);
                            return ok; 
                        case 1:     // pracownik może mieć 2 karty w tym samym czasie
                            ok = ValidateDuplicate(tbName, fldName, typ, pracId, dataOd, dataDo, id, rcpIdKarta.ToString(), out msg);
                            if (!ok) Tools.ShowError(msg);
                            else
                            {
                                bool b1 = ValidateOverlapped(tbName, fldName, typ, pracId, dataOd, dataDo, id, out msg);
                                if (!b1) Tools.ShowWarning(msg);
                            }
                            return ok;
                    }
                }
            }
            return false;
        }
        //---------------------------------------
        public static bool xx_Validate(string tbName, string fldName, int typ, string pracId, object dOd, object dDo, string id)  // fldName - póki co dowolne, ale być musi
        {
            if (db.isNull(dOd)) Tools.ShowError("Data początkowa nie może być pusta.");
            else
            {
                if (db.isNull(dDo)) dDo = DateTime.MaxValue;
                if ((DateTime)dOd > (DateTime)dDo) Tools.ShowError("Data końcowa nie może być wcześniejsza niż początkowa.");
                else
                {
                    string dataOd = ((DateTime)dOd).ToStringDb();
                    string dataDo = ((DateTime)dDo).ToStringDb();
                    DataRow dr;
                    if (String.IsNullOrEmpty(id))
                        dr = db.getDataRow(String.Format("select top 1 {4}, Od, Do from {3} where IdPracownika = {0} and ISNULL(Do, Od) >= {1} and Od <= {2}", pracId, dataOd, dataDo, tbName, fldName));  // jak jest 20990909 to zawsze spełnia
                    else
                        dr = db.getDataRow(String.Format("select top 1 {4}, Od, Do from {3} where IdPracownika = {0} and ISNULL(Do, Od) >= {1} and Od <= {2} and Id <> {5}", pracId, dataOd, dataDo, tbName, fldName, id));  // jak jest 20990909 to zawsze spełnia
                    if (dr != null)
                    {
                        object d = dr["Do"];
                        switch (typ)
                        {
                            case 0:     // okres nie może się pokrywać
                                Tools.ShowError("Okres obowiązywania pokrywa się z już istniejącym ({1} - {2}).",
                                    db.getValue(dr, 0),         // nie dodaję, trzeba by link do opisu robić np dla algorytmów
                                    Tools.DateToStr(dr["Od"]),
                                    db.isNull(d) ? "bez terminu" : Tools.DateToStr(d));
                                break;
                            case 1:     // pracownik może mieć 2 karty w tym samym czasie
                                Tools.ShowWarning("Okres obowiązywania pokrywa się z już istniejącym ({1} - {2}).",
                                    db.getValue(dr, 0),         // nie dodaję, trzeba by link do opisu robić np dla algorytmów
                                    Tools.DateToStr(dr["Od"]),
                                    db.isNull(d) ? "bez terminu" : Tools.DateToStr(d));
                                return true;    // <<<<< ok
                        }
                    }
                    else return true;   // <<<<< ok
                }
            }
            return false;
        }

        private bool Validate(ListViewItem item, IOrderedDictionary values, string id)
        {
#if SIEMENS
            return Validate("PracownicyKarty", "NrKarty", 1, PracId, values["Od"], values["Do"], id, values["NrKarty"]);
#else
            return Validate("PracownicyKarty", "RcpId", 1, PracId, values["Od"], values["Do"], id, values["RcpId"]);
#endif
        }

        private bool Validate_x(ListViewItem item, IOrderedDictionary values, string id)
        {
            DateTime? dOd = (DateTime?)values["Od"];
            DateTime? dDo = (DateTime?)values["Do"];
            //string rcpid = values["RcpId"].ToString();
            if (db.isNull(dOd)) Tools.ShowError("Data początkowa nie może być pusta.");
            else
            {
                if (db.isNull(dDo)) dDo = DateTime.MaxValue;
                if ((DateTime)dOd > (DateTime)dDo) Tools.ShowError("Data końcowa nie może być wcześniejsza niż początkowa.");
                else
                {
                    string dataOd = ((DateTime)dOd).ToStringDb();
                    string dataDo = ((DateTime)dDo).ToStringDb();
                    DataRow dr;
                    if (String.IsNullOrEmpty(id))
                        dr = db.getDataRow(String.Format("select top 1 RcpId from PracownicyKarty where IdPracownika = {0} and ISNULL(Do, Od) >= {1} and Od <= {2}", PracId, dataOd, dataDo));  // jak jest 20990909 to zawsze spełnia
                    else
                        dr = db.getDataRow(String.Format("select top 1 RcpId from PracownicyKarty where IdPracownika = {0} and ISNULL(Do, Od) >= {1} and Od <= {2} and Id <> {3}", PracId, dataOd, dataDo, id));  // jak jest 20990909 to zawsze spełnia
                    if (dr != null) Tools.ShowError("Okres obowiązywania pokrywa się z już istniejącym ({0}).", db.getValue(dr, 0));
                    else return true;   // <<<<< ok
                }
                
                
                //if (db.isNull(dDo))
                //{
                //    dr = db.getDataRow(String.Format("select top 1 Id from PracownicyKarty where IdPracownika = {0} and ISNULL(Do, Od) >= {1}", PracId, dataOd));
                //    if (dr != null) Tools.ShowError("Okres obowiązywania pokrywa się z już istniejącym.");
                //    else return true;   // <<<<< ok
                //}
                //else
                //{
                //    if ((DateTime)dOd > (DateTime)dDo) Tools.ShowError("Data końcowa wcześniejsza nież początkowa.");
                //    else
                //    {
                //        string dataDo = ((DateTime)dDo).ToStringDb();
                //        dr = db.getDataRow(String.Format("select top 1 Id from PracownicyKarty where IdPracownika = {0} and ISNULL(Do, Od) >= {1} and Od < {2}", PracId, dataOd, dataDo));  // jak jest 20990909 to zawsze spełnia
                //        if (dr != null) Tools.ShowError("Okres obowiązywania pokrywa się z już istniejącym.");
                //        else return true;   // <<<<< ok
                //    }
                //}
            }
            return false;
        }
        //----------------------------------
        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
#if SIEMENS
            e.Values["RcpId"] = PracId;
#endif
            e.Cancel = !Validate(e.Item, e.Values, null);
        }

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
#if SIEMENS
            e.NewValues["RcpId"] = PracId;
#endif
            e.Cancel = !Validate(ListView1.EditItem, e.NewValues, Tools.GetDataKey((ListView)sender, e));
        }
        //------
        protected void ListView1_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            Updated = true;
        }

        protected void ListView1_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            Updated = true;
        }

        protected void ListView1_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            Updated = true;
        }
        //----------------------------------
       
        public bool InEdit
        {
            get { return ListView1.EditIndex != -1 || ListView1.InsertItemPosition != InsertItemPosition.None; }
        }

        /*
        public bool Update()
        {
            if (ListView1.EditIndex != -1)
                ListView1.UpdateItem(ListView1.EditIndex, true);
            return false;
        }
        */

        //-----------------------------------
        public string PracId
        {
            set 
            {
                ViewState["pracid"] = value;
                hidPracId.Value = value;
                Updated = false;
                ListView1.InsertItemPosition = InsertItemPosition.None;
                ListView1.EditIndex = -1;
                ListView1.SelectedIndex = -1;
            }
            get { return Tools.GetStr(ViewState["pracid"]); }
        }

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

        public bool Updated
        {
            set { ViewState["updated"] = value; }
            get { return Tools.GetBool(ViewState["updated"], false); }
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }

    }
}