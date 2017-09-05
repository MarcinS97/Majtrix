using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public class WnButton : Button
    {
        string FStVisible = null;
        string FTypVisible = null;          //"1,3,7"
        string FConfirmMsg = null;

        //stVisible/mode:   0-normal    1-query     2-edit      + - visible
        //      0           -           -           -   
        //      1           +           +           +
        //      2           +           -           -           np. [Edit]
        //    3/E           -           -           +           np. [Save][Cancel]
        //      e           -           -           +           np. [Save][Cancel]

        public static readonly int[,] stateMatrix = {
        /*                          0-norm  1-query 2-edit */
        /* visUnvisible  = '0' */ { 0,      0,      0 },
        /* visVisible    = '1' */ { 1,      1,      1 },
        /* visEdit       = '2' */ { 1,      0,      0 },
        /* visEditAct    = 3/E */ { 0,      0,      1 },
        /* visEditAct_e  = 'e' */ { 0,      0,      1 } 
        };

        public static int getState(int mode, char v)
        {
            switch (v)
            {
                default:
                case dbField.visUnvisible:
                    return stateMatrix[0, mode];
                    break;
                case dbField.visVisible:
                    return stateMatrix[1, mode];
                    break;
                case dbField.visEdit:
                    return stateMatrix[2, mode];
                    break;
                case dbField.visEditAct:
                case dbField.visEditAct_E:
                    return stateMatrix[3, mode];
                    break;
                case dbField.visEditAct_e:
                    return stateMatrix[4, mode];
                    break;
            }
        }
        
        public bool SetVisible(int typ, int vindex, int index, int mode)    // 0-niewidoczny, 1-visible 2-visible 3-visible też w query mode
        {
            if (dbField.IsTypVisible(FTypVisible, null, typ))
            {
                int state;
                bool visible;
                char v = dbField.getState(mode, vindex, index, out state, FStVisible);
                visible = getState(mode, v) != 0;
                Visible = visible;
                StVisibleValue = v;
                return visible;
            }
            else
            {
                Visible = false;
                return false;
            }
        }

        public bool SetMode(int mode)
        {
            bool visible = getState(mode, StVisibleValue) != 0;
            Visible = visible;
            return Visible;
        }

        public string StVisible
        {
            set { FStVisible = value; }
            get { return FStVisible; }
        }

        public string TypVisible
        {
            set { FTypVisible = value; }
            get { return FTypVisible; }
        }

        public string ConfirmMsg
        {
            set 
            { 
                FConfirmMsg = value;
                Tools.MakeConfirmButton(this, value);
            }
            get { return FConfirmMsg; }
        }

        public char StVisibleValue    // bieżąca wartość StVisible na podstawie osoby i statusu wniosku
        {
            set { ViewState["stvis"] = value; }
            get { return Tools.GetChar(ViewState["stvis"], dbField.visUnvisible); }
        }
    }
    //------------------------------
    public delegate bool VisibleEventHandler(Control container, bool typVisible);

    public class WnVisible : Label
    {
        public event VisibleEventHandler CheckVisible;

        string FStVisible = null;
        string FTypVisible = null;          //"1,3,7"
        string FControls = null;

        public bool SetVisible(int mode, int typ, int vindex, int index)
        {
            bool visible;
            bool tv;
            tv = dbField.IsTypVisible(FTypVisible, null, typ);
            tv = IsCheckVisible(tv);
            if (tv)
            {
                int state;
                dbField.getState(mode, vindex, index, out state, FStVisible);
                visible = state != dbField.stUnvisible;
                State = state;
            }
            else
            {
                visible = false;
                State = dbField.stUnvisible;
            }
            dbField.setVisible(Parent, FControls, visible);
            Visible = false;
            return visible;
        }

        private bool IsCheckVisible(bool typVisible)
        {
            if (CheckVisible != null)
                return CheckVisible(this, typVisible);
            else
                return typVisible;
        }

        public void SetMode(int mode)
        {
            //
        }

        public string StVisible
        {
            set { FStVisible = value; }
            get { return FStVisible; }
        }

        public string TypVisible
        {
            set { FTypVisible = value; }
            get { return FTypVisible; }
        }

        public string StControls
        {
            set { FControls = value; }
            get { return FControls; }
        }

        public int State
        {
            set { ViewState["state"] = value; }
            get { return Tools.GetInt(ViewState["state"], dbField.stQuery); }
        }
    }
    //------------------------------
    public class dbLabel : Label
    {
        string FVis = null;

        public string Vis
        {
            set
            {
                FVis = value;
            }

            get { return FVis; }
        }
    }


    public class Portal
    {
        string FVis = null;

        public string Vis
        {
            set
            {
                /*
                string v = value.ToLower();
                switch (v)
                {
                    case "true":
                        base.Visible = true;
                        FVisible = null;
                        break;
                    case "false":
                        base.Visible = true;
                        FVisible = null;
                        break;

                }
                */
                FVis = value;
            }

            get { return FVis; }
        }

    }



}

