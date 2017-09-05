using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Requests
{
    public partial class cntCreateRequest : System.Web.UI.UserControl
    {
        public event EventHandler Created;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String ObserverId)
        {
            this.ObserverId = ObserverId;
        }

        protected void Create(object sender, EventArgs e)
        {
            Boolean Ok = db.Execute(dsCreateRequest, ObserverId, db.strParam(deDate.Date.ToString()), db.strParam(deDateW.Date.ToString()), db.strParam(tbName.Text));

            if (Ok)
            {
                Tools.ShowMessage("Wniosek został utworzony");
                if (Created != null) Created(null, EventArgs.Empty);
            }
            else
            {
            }
        }

        public Button CloseButton
        {
            get { return btnClose; }
        }

        public Button CreateButton
        {
            get { return btnCreate; }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }
    }
}