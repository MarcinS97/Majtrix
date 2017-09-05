using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Adm
{
    public partial class cntGrupyUprawnien : System.Web.UI.UserControl
    {
        static bool migrateChecked = false;

        public static void MigrationDB()
        {
            const string MigrationSQL = @"declare @KolumnaIstnieje int
declare @TabelaIstnieje int


select @KolumnaIstnieje = COUNT(COLUMN_NAME)
from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME = 'Pracownicy' and COLUMN_NAME = 'IdRightsGrupy'

select @TabelaIstnieje = COUNT(TABLE_NAME) from INFORMATION_SCHEMA.TABLES
where TABLE_NAME = 'RightsGrupy'


IF @KolumnaIstnieje = 0
BEGIN
	alter table Pracownicy
	add IdRightsGrupy int null
END

IF @TabelaIstnieje = 0
BEGIN
	CREATE TABLE [dbo].[RightsGrupy](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Nazwa] [nvarchar](50) NOT NULL,
		[Rights] [varchar](250) NOT NULL,
	 CONSTRAINT [PK_RightsGrupy] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END";

            if(!migrateChecked && Lic.GrupyUprawnien && App.User.IsAdmin)
            {
                db.Execute(MigrationSQL);
                migrateChecked = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MigrationDB();
            if(ddlGroups.Items.Count > 0 && ddlGroups.SelectedItem == null)
            {
                ddlGroups.SelectedIndex = 0;
            }
            else if(ddlGroups.SelectedItem != null)
            {
                //UpdateCheckBoxes();
            }
        }

        protected void ddlGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCheckBoxes();
        }

        protected void ddlGroups_DataBound(object sender, EventArgs e)
        {
            SelectEditingGroup();
            hfEditingGroup.Value = null;
            UpdateCheckBoxes();
        }

        void UpdateCheckBoxes()
        {
            List<string> items = new List<string>();

            string selectedRights = "";
            if(ddlGroups.SelectedItem != null)
                selectedRights = ddlGroups.SelectedValue.Split('|')[1];
            else for(int i=0; i<AppUser.maxRight+1; i++) selectedRights += "0";

            int rightId = 101;
            char right = rightId - 1 < selectedRights.Length ? selectedRights[rightId] : '0';
            items.Add(string.Format("{0}|{1}|{2}", rightId, "A - Administrator", right));

            for (int i = 0; i < cntPracownicy3.rights.Length / 2; i++)
            {
                rightId = Convert.ToInt32(cntPracownicy3.rights[i, 0]);
                right = rightId - 1 < selectedRights.Length ? selectedRights[rightId] : '0';
                items.Add(string.Format("{0}|{1}|{2}", rightId, cntPracownicy3.rights[i, 1], right));

                if(rightId == 18)
                {
                    rightId = 102;
                    right = rightId - 1 < selectedRights.Length ? selectedRights[rightId] : '0';
                    items.Add(string.Format("{0}|{1}|{2}", rightId, "R - Raporty", right));
                }

            }
            cbRepeater.DataSource = items;
            cbRepeater.DataBind();
            
            if(ddlGroups.Items.Count == 0)
            {
                editButton.Visible = false;
                deleteButton.Visible = false;
            }
        }

        void SelectEditingGroup()
        {
            if (hfEditingGroup.Value != null)
            {
                for(int i=0; i<ddlGroups.Items.Count; i++)
                {
                    if(ddlGroups.Items[i].Text == hfEditingGroup.Value)
                    {
                        ddlGroups.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        void EnableEditor(bool value, bool newGroup)
        {
            saveButton.Visible = value;
            cancelButton.Visible = value;
            editButton.Visible = !value;
            deleteButton.Visible = !value;
            newButton.Visible = !value;
            ddlGroups.Visible = !value;
            tbNazwa.Visible = value;

            if(newGroup)
            {
                tbNazwa.Text = "";

                foreach(RepeaterItem item in cbRepeater.Items)
                {
                    ((CheckBox)item.FindControl("cbR")).Checked = false;
                }
            }
            else
            {
                if(ddlGroups.SelectedItem != null)
                    tbNazwa.Text = ddlGroups.SelectedItem.Text;
            }

            foreach(RepeaterItem item in cbRepeater.Items)
            {
                ((CheckBox)item.FindControl("cbR")).Enabled = value;
            }
        }

        string GetNewRights()
        {
            char[] newRights = new char[AppUser.maxRight+1];
            for(int i=0; i<newRights.Length; i++)
                newRights[i] = '0';

            foreach(RepeaterItem item in cbRepeater.Items)
            {
                CheckBox checkBox = (CheckBox)item.FindControl("cbR");
                string hidValue = ( (HiddenField)item.FindControl("hidRightId") ).Value;
                int rightId;

                if( int.TryParse(hidValue, out rightId) )
                {
                    newRights[rightId] = checkBox.Checked ? '1' : '0';
                }
            }

            return new string(newRights);
        }

        DataSet GetUsersInGroup(string groupId)
        {
            return db.getDataSet( string.Format(dsPowiazaniZGrupa.SelectCommand, db.sqlPut(groupId)) );
        }

        bool GroupNameAlreadyExist(string groupId, string name)
        {
            DataRow dr = db.getDataRow( string.Format(dsPowiazaniZGrupa.UpdateCommand, db.sqlPut(groupId), db.sqlPut(name)) );
            return db.getValue(dr, "value") == "1";
        }

        public static bool UserRightsEqualsGroup(DataRow userInfo, string groupRights)
        {
            char[] userRights = userInfo["Rights"].ToString().ToArray();
            userRights[101] = (bool)userInfo["Admin"]   ? '1' : '0';
            userRights[102] = (bool)userInfo["Raporty"] ? '1' : '0';

            return UserRightsEqualsGroup(new string(userRights), groupRights);
        }

        public static bool UserRightsEqualsGroup(string userRights, string groupRights)
        {
            bool retValue = true;
            for(int i = 0; i < cntPracownicy3.rights.Length/2; i++)
            {
                int rightId = (int)cntPracownicy3.rights[i, 0];
                if(  userRights[rightId]  !=  groupRights[rightId]  )
                    retValue = false;
            }
            
            if(  userRights[101]  !=  groupRights[101]  )
                retValue = false;
            if(  userRights[102]  !=  groupRights[102]  )
                retValue = false;

            return retValue;
        }

        void UpdateUserRight(DataRow userInfo, string groupRights)
        {
            char[] newRight = userInfo["Rights"].ToString().ToArray();

            for(int i = 0; i < cntPracownicy3.rights.Length/2; i++)
            {
                int rightId = (int)cntPracownicy3.rights[i, 0];
                newRight[rightId] = groupRights[rightId];
            }

            db.ExecuteInsert( dsPowiazaniZGrupa, userInfo["Id"], new string(newRight) );
        }

        bool ShowEditConfirm()
        {
            bool warnings = false;

            string groupId = ddlGroups.SelectedValue.Split('|')[0];
            DataSet pracownicy = GetUsersInGroup(groupId);
            DataRowCollection rows = db.getRows(pracownicy);
            
            if( GroupNameAlreadyExist(groupId, tbNazwa.Text) )
            {
                Tools.ShowWarning("Istnieje już grupa o podanej nazwie");
                warnings = true;
            }
            else if (rows.Count > 0 && hfNewGroup.Value != "1")
            {

                string message = "Zmiany w grupie wpłyną na:\n\n";
                for(int i=0; i<rows.Count; i++)
                {
                    if( UserRightsEqualsGroup( rows[i] , ddlGroups.SelectedValue.Split('|')[1] ) )
                    {
                        message += string.Format("\t({0}){1} {2}", rows[i]["KadryId"], rows[i]["Imie"], rows[i]["Nazwisko"]);
                        if(i < rows.Count-1)
                            message += "\n";

                        warnings = true;
                    }
                }
                if(warnings)
                    Tools.ShowConfirm(message, confirmEditButton);

            }

            return warnings;
        }

        void ShowDeleteConfirm()
        {
            string message = string.Format("Czy na pewno chcesz usunąć grupę \"{0}\"?", ddlGroups.SelectedItem.Text);

            string groupId = ddlGroups.SelectedValue.Split('|')[0];
            DataSet pracownicy = GetUsersInGroup(groupId);
            DataRowCollection rows = db.getRows(pracownicy);
            if(rows.Count > 0)
            {
                message += " Następujące osoby utracą przypisanie do grupy:\n\n";
                for(int i=0; i<rows.Count; i++)
                {
                    message += string.Format("\t({0}){1} {2}", rows[i]["KadryId"], rows[i]["Imie"], rows[i]["Nazwisko"]);
                    if(i < rows.Count-1)
                        message += "\n";
                }
            }

            Tools.ShowConfirm(message, confirmDeleteButton);
        }

        protected bool IsChecked(object dataItem)
        {
            if(dataItem != null)
            {
                return ((string)dataItem).Split('|')[2][0] == '1';
            }
            return false;
        }

        void UpdateGroup()
        {
            string newRights = GetNewRights();

            switch(hfNewGroup.Value)
            {
                case "1":
                    if(GroupNameAlreadyExist(null, tbNazwa.Text))
                    {
                        Tools.ShowWarning("Istnieje już grupa o podanej nazwie");
                        return;
                    }
                    else
                        db.ExecuteInsert(SqlDataSource1, newRights, tbNazwa.Text);
                    break;

                case "0":
                    string groupId = ddlGroups.SelectedValue.Split('|')[0];
                    db.ExecuteUpdate(SqlDataSource1, groupId, newRights, tbNazwa.Text);

                    DataSet pracownicy = GetUsersInGroup(groupId);
                    DataRowCollection rows = db.getRows(pracownicy);
                    for(int i=0; i<rows.Count; i++)
                    {
                        if( UserRightsEqualsGroup( rows[i] , ddlGroups.SelectedValue.Split('|')[1] ) )
                        {
                            UpdateUserRight(rows[i], newRights);
                        }
                    }
                    break;
            }

            hfEditingGroup.Value = tbNazwa.Text;
            ddlGroups.DataBind();
            EnableEditor(false, false);
            UpdateCheckBoxes();
        }

        void DeleteGroup()
        {
            string groupId = ddlGroups.SelectedValue.Split('|')[0];
            db.ExecuteDelete(SqlDataSource1, groupId);
            ddlGroups.DataBind();
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            if( !ShowEditConfirm() )
                UpdateGroup();
        }

        protected void cancelButton_Click(object sender, EventArgs e)
        {
            EnableEditor(false, false);
            UpdateCheckBoxes();
        }

        protected void editButton_Click(object sender, EventArgs e)
        {
            hfNewGroup.Value = "0";
            EnableEditor(true, false);
        }

        protected void newButton_Click(object sender, EventArgs e)
        {
            hfNewGroup.Value = "1";
            EnableEditor(true, true);
        }

        protected void deleteButton_Click(object sender, EventArgs e)
        {
            ShowDeleteConfirm();
        }

        protected void confirmEditButton_Click(object sender, EventArgs e)
        {
            UpdateGroup();
        }

        protected void confirmDeleteButton_Click(object sender, EventArgs e)
        {
            DeleteGroup();
        }
    }
}