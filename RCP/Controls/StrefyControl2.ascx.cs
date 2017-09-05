using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using System.Collections.Specialized;

namespace HRRcp.Controls
{
    public static class CustomExtensions
    {
        public static void SetControlsVisible(this Repeater rp, bool b, params string[] ctrl)
        {
            if (ctrl != null)
            {
                foreach (RepeaterItem item in rp.Items)
                {
                    foreach (string c in ctrl)
                    {
                        Control toSet = Tools.FindControl(item, c);
                        if (toSet != null)
                            toSet.Visible = b;
                    }
                }
            }
        }

        public static void SetControlsVisible(this Control cnt, bool b, params string[] ctrl)
        {
            foreach (string c in ctrl)
            {
                Control toSet = Tools.FindControl(cnt, c);
                if (toSet != null)
                    toSet.Visible = b;
            }
        }
    }
    public partial class StrefyControl2 : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvStrefy, Tools.ListViewMode.Bootstrap);
            //Tools.PrepareSorting2(lvTasks, 1, 10);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //lvStrefy.SelectedIndex = 0;
                //fillDatyOd("1");
                Tools.MakeConfirmButton(btDelete, "Potwierdź usunięcie konfiguracji strefy.");
                //Tools.MakeConfirmButton(btDelete, "Potwierdź zapis konfiguracji strefy.");
            }
        }
        //-------------------------------------
        private void AddStrefaReaders()
        {
            foreach (ListViewItem item in cntReaders.List.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("Select");
                if (cb != null)
                {
                    if (cb.Checked)
                    {
                        Label lb = (Label)item.FindControl("IdLabel");
                        string rid = lb.Text;  // nie moze go nie być !!!
                        lb = (Label)item.FindControl("InOutLabel");
                        string io;
                        switch (lb.Text)
                        {
                            default:
                                io = "-";  // out
                                lb = (Label)item.FindControl("NazwaLabel");
                                if (lb != null)
                                {
                                    string n = lb.Text.ToLower();
                                    if (n.EndsWith(" in") || n.EndsWith("_in") || n.Contains(" in ") || n.Contains("_in_"))
                                        io = "";  // in (+)
                                }
                                break;
                            case "IN": 
                                io = "";
                                break;
                            case "OUT":
                                io = "-";
                                break;
                        }
                        string r = cntReaders.Readers;
                        r += (String.IsNullOrEmpty(r) ? "" : ",") + io + rid;
                        updateReaders(r);
                    }
                }
            }
        }

        private void RemoveStrefaReaders()
        {
            List<string> data = null;
            bool f = false;            
            foreach (ListViewItem item in cntStrefaReaders.List.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("Select");
                if (cb != null)
                {
                    if (cb.Checked)
                    {
                        if (!f)  // tworze liste dopiero jak cos jest zaznaczone
                        {
                            f = true;
                            string[] ra = cntStrefaReaders.Readers.Split(',');
                            data = new List<string>(ra.AsEnumerable());
                        }
                        HiddenField hid = (HiddenField)item.FindControl("hidId");
                        string v = hid.Value;
                        if (!data.Remove(v))
                            data.Remove("-" + v);
                    }
                }
            }
            if (f)
            {
                string r = String.Join(",", data.ToArray());
                updateReaders(r);
            }
        }

        
        //-------------------------------------
        /*
        private void AddStrefaReaders()
        {
            string sid = Readers.StrefaId;
            SqlConnection con = Base.Connect();
            
            
            
            foreach (ListViewItem item in Readers.List.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("Select");
                if (cb != null)
                {
                    if (cb.Checked)
                    {
                        Label lb = (Label)item.FindControl("IdLabel");
                        string rid = lb.Text;  // nie moze go nie być !!!
                        lb = (Label)item.FindControl("InOutLabel");
                        string io;
                        switch (lb.Text)
                        {
                            default:
                                io = "1";
                                lb = (Label)item.FindControl("NazwaLabel");
                                if (lb != null)
                                {
                                    string n = lb.Text.ToLower();
                                    if (n.EndsWith(" in") || n.EndsWith("_in") || n.Contains(" in ") || n.Contains("_in_"))
                                        io = "0";
                                }
                                break;
                            case "IN": 
                                io = "0";
                                break;
                            case "OUT":
                                io = "1";
                                break;
                        }

                        
                        Base.insertSQL(con, "insert into StrefyReaders (IdStrefy, ReaderId, InOut) values (" +
                                Base.insertParam(sid) +
                                Base.insertParam(rid) +
                                Base.insertParamLast(io) +
                            ")");
                    }
                }
            }
            Base.Disconnect(con);
        }

        private void RemoveStrefaReaders()
        {
            string ToDelete = null;
            foreach (ListViewItem item in StrefaReaders.List.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("Select");
                if (cb != null)
                {
                    if (cb.Checked)
                    {
                        HiddenField hid = (HiddenField)item.FindControl("hidId");
                        ToDelete += "," + hid.Value;  // nie moze go nie być !!!
                    }
                }
            }
            if (!String.IsNullOrEmpty(ToDelete))
            {
                ToDelete = ToDelete.Substring(1);
                Base.execSQL("delete from StrefyReaders where Id in (" + ToDelete + ")");
            }
        }
         
         */      
        //-------------------------------------
        private void updateReaders(string readers)
        {
            cntReaders.Readers = readers;
            cntReaders.SetSelectSql();
            cntReaders.List.DataBind();
            cntStrefaReaders.Readers = readers;
            cntStrefaReaders.SetSelectSql();
            cntStrefaReaders.List.DataBind();
        }
        
        private void selectReaders()
        {
            string r;
            if (lbDatyOd.SelectedIndex != -1)
            {
                r = Tools.GetLineParam(lbDatyOd.SelectedValue, 1);
                string t = lbDatyOd.SelectedItem.Text;
                tbData.Text = t.Substring(0, 10);
                tbCzas.Text = t.Substring(11, 8);
            }
            else
            {
                r = null;
                DateTime dt = DateTime.Now;
                tbData.Text = Base.DateToStr(dt);
                tbCzas.Text = Base.TimeToStr(dt);
            }
            updateReaders(r);
        }

        private void fillDatyOd(string selDataOd)
        {
            lbDatyOd.Items.Clear();
            if (lvStrefy.SelectedIndex != -1)
            {
                string sid = lvStrefy.DataKeys[lvStrefy.SelectedIndex].Value.ToString();
                StrefaId = sid;
                DataSet ds = Base.getDataSet("select * from StrefyReaders where IdStrefy = " + sid + " order by DataOd desc");
                Tools.BindData(lbDatyOd, ds, "DataOd", "Id", "Readers", null, selDataOd);
                if (lbDatyOd.Items.Count > 0 && lbDatyOd.SelectedIndex == -1)
                    lbDatyOd.SelectedIndex = 0;
            }
            else StrefaId = null;
            selectReaders();
        }

        protected void lvStrefy_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvStrefy.EditIndex = -1;
            fillDatyOd(null);
        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            AddStrefaReaders();
            cntReaders.List.DataBind();
            cntStrefaReaders.List.DataBind();
        }

        protected void btRemove_Click(object sender, EventArgs e)
        {
            RemoveStrefaReaders();
            cntReaders.List.DataBind();
            cntStrefaReaders.List.DataBind();
        }

        protected void lbDatyOd_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvStrefy.EditIndex = -1;
            selectReaders();
        }

        protected void lvStrefy_DataBound(object sender, EventArgs e)
        {
            if (lvStrefy.DataKeys.Count > 0)
            {
                if (lvStrefy.SelectedIndex == -1)
                {
                    lvStrefy.SelectedIndex = 0;
                    fillDatyOd(null);
                }
            }
        }

        protected void lvStrefy_Load(object sender, EventArgs e)
        {
        }

        protected void lvStrefy_PreRender(object sender, EventArgs e)
        {
        }

        protected void lvStrefy_LayoutCreated(object sender, EventArgs e)
        {
            
        }

        protected void lvStrefy_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvStrefy.SelectedIndex = e.NewEditIndex;
            selectReaders();
        }
        //--------------------------
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(StrefaId))  // zabezpieczenie, pozniej uwzglednic jak dodaje nowy
            {
                string dt = tbData.Text + " " + tbCzas.Text;
                SqlConnection con = Base.Connect();
                string id = Base.getScalar(con, "select Id from StrefyReaders where IdStrefy = " + StrefaId + " and DataOd = " + Base.strParam(dt));
                if (String.IsNullOrEmpty(id))
                {
                    Base.execSQL("insert into StrefyReaders (IdStrefy, DataOd, Readers) values (" +
                            Base.insertParam(StrefaId) +
                            Base.insertStrParam(dt) +
                            Base.insertStrParamLast(cntStrefaReaders.GetReadersInOut()) +
                        ")");
                    fillDatyOd(dt);
                }
                else
                {
                    Base.execSQL("update StrefyReaders set Readers = " + Base.strParam(cntStrefaReaders.GetReadersInOut()) +
                        " where Id = " + id);
                    fillDatyOd(dt);  // update selected ale juz niech bedzie tak
                }
                Base.Disconnect(con);
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            selectReaders();
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string id = Tools.GetLineParam(lbDatyOd.SelectedValue, 0);
            if (!String.IsNullOrEmpty(id))
            {
                Base.execSQL("delete from StrefyReaders where Id = " + id);
                fillDatyOd(null);
            }
        }
        //----------------------------
        public string StrefaId
        {
            get { return hidStrefaId.Value; }
            set { hidStrefaId.Value = value; }
        }


      

        private bool UpdateItem(ListViewItem Item, IOrderedDictionary OldValues, IOrderedDictionary Values, EventArgs e)
        {
            // sprawdzanie czy istnieje


            Values["IdCC"] = Tools.GetDdlSelectedValueInt(Item, "ddlCC");
            //Values["Czas"] = Tools.GetText(Item, "tbCzas").Replace(',', '.');
            return true;
        }

        protected void lvStrefy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvStrefy.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvStrefy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        protected void lvStrefy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv = null;
            int li = Tools.GetListItemMode(e, lvStrefy, out drv);
            if (li == Tools.limEdit && drv != null)
            {
                Tools.SelectItem(e.Item, "ddlCC", drv["IdCC"]);
                //Tools.SelectItem(e.Item, "ddlQC", drv["QC"]);
                //Tools.SelectItem(e.Item, "ddlProd", drv["Produktywnosc"]);
            }
        }


    }
} 