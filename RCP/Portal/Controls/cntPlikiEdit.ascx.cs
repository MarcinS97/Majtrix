using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntPlikiEdit : System.Web.UI.UserControl
    {
        public event EventHandler Saved;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(String GroupId)
        {
            this.GroupId = GroupId;
            cntModal.Show();

            if (String.IsNullOrEmpty(GroupId))
            {
                dbField.FillData(this, null, 0, 0, 0, dbField.moEdit);
            }
            else
            {
                DataRow dr = db.Select.Row(dsData, GroupId);
                dbField.FillData(this, dr, 0, 0, 0, dbField.moEdit);
                cntIconPicker._SelectedIcon = db.getValue(dr, "Image");
            }
        }

        public void Close()
        {
            cntModal.Close();
        }

        public String GroupId
        {
            get { return hidGroupId.Value; }
            set { hidGroupId.Value = value; }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(GroupId))
            {
                dbField.dbUpdate(db.conP, this, "SqlMenu", "Id=" + GroupId, "Image", db.nullStrParam(cntIconPicker._SelectedIcon));
            }
            else
            {
                dbField.dbInsert(db.conP, this, "SqlMenu", "Grupa, Image", db.strParam(Request.QueryString["p1"]), db.nullStrParam(cntIconPicker._SelectedIcon));
            }
            if (Saved != null)
                Saved(sender, e);
            Close();
        }
    }
}