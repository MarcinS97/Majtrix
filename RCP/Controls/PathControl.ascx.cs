using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PathControl : System.Web.UI.UserControl
    {
        public event EventHandler SelectPath;
        //-----------------------------------------------------
        [Serializable]
        public class PathEntry
        {
            public string Text; //{ get; set; }
            public string Param; //{ get; set; }
            public string Y;

            public PathEntry(string text, string param, string posy)
            {
                Text = text;
                Param = param;
                Y = posy;
            }
        }

        private List<PathEntry> FPath = null;
        //-----------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            RenderPath();
        }
        //--------------------------------------
        protected void OnPathCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Back")
            {
                int idx = Convert.ToInt32(e.CommandArgument);
                if (0 <= idx && idx < Path.Count)
                    Back(idx);
            }
        }

        public void RenderPath()
        {
            phPath.Controls.Clear();
            if (Path.Count > 1)  // 1 jak root jest
            {
                paPath.Visible = true;
                for (int i = 0; i < Path.Count; i++)
                {
                    string sep = i > 0 ? "» " : "";
                    if (i < Path.Count - 1)
                    {
                        if (Enabled)
                        {

                            LinkButton lbt = new LinkButton();
                            lbt.ID = "lbtPath_" + i.ToString();
                            lbt.Text = Path[i].Text;
                            lbt.CommandName = "Back";
                            lbt.CommandArgument = i.ToString();
                            lbt.Command += new CommandEventHandler(OnPathCommand);
                            Tools.AddControl(phPath, "<span>" + sep, lbt, "</span> ");
                        }
                        else
                        {
                            Tools.AddLiteral(phPath, "<span class=\"disabled\">" + sep + Path[i].Text + "</span>");
                        }
                    }
                    else
                    {
                        if (Enabled)
                            //Tools.AddLiteral(phPath, "<span>" + sep + "<span>" + Path[i].Text + "</span></span>");
                            Tools.AddLiteral(phPath, "<span class=\"selected\">" + sep + Path[i].Text + "</span>");
                        else
                            Tools.AddLiteral(phPath, "<span class=\"selected disabled\">" + sep + Path[i].Text + "</span>");
                    }
                }
            }
            else paPath.Visible = false;
        }

        //--------------------------------------
        public int AddPath(string path, string param, string posY)
        {
            /*
            if (Path.Count == 0)
                Path.Add(new PathEntry(user.NazwiskoImie, user.Id, posY)); */
            Path.Add(new PathEntry(path, param, posY));
            RenderPath();
            return Path.Count - 1;
        }
    
        public string RemovePath(int index, out string pracId, out string posY)
        {
            if (Path.Count > 1) // > 1 bo ostatnia pozycja to ta w ktorej jestesmy
            {
                if (index == -1)
                    if (Path.Count > 1)
                        index = Path.Count - 2;
                    else 
                        index = 0;
                string parent = Path[index].Param;
                if (index < Path.Count - 1)
                {
                    pracId = Path[index + 1].Param;     // ten będzie do zaznaczenia
                    posY = Path[index + 1].Y;           // on tez pamieta scrollTop
                }
                else
                {
                    pracId = null;
                    posY = "0";
                }
                for (int i = Path.Count - 1; i > index; i--)    // usuwamy wszystko za
                    Path.RemoveAt(i);
                /*
                if (Path.Count == 1)                            // usuwamy pierwszy  !!! nie usuwamy, root zawsze zostaje !!!
                    Path.RemoveAt(0);  */
                
                RenderPath();
                return parent;
            }
            else
            {
                pracId = null;
                posY = "0";
                RenderPath();  // tu schowa
                return null;
            }
        }

        public void Back(int idx)  // -1 o jeden
        {
            string sel;
            string posY;
            hidPar.Value = RemovePath(idx, out sel, out posY);
            hidPosY.Value = posY;
            if (SelectPath != null)
                SelectPath(this, EventArgs.Empty);                            
        }

        public void Prepare(string root, string par, string posY)
        {
            Path.Clear();
            if (!String.IsNullOrEmpty(root))
                AddPath(root, par, posY);
        }
        //--------------------------------------
        public List<PathEntry> Path
        {
            get
            {
                if (FPath == null)
                {
                    FPath = (List<PathEntry>)ViewState[ID + "_path"];
                    if (FPath == null)
                    {
                        FPath = new List<PathEntry>();
                        ViewState[ID + "_path"] = FPath;
                    }
                }
                return FPath;
            }        
        }

        public PathEntry LastItem
        {
            get 
            {
                if (Path.Count > 0)
                    return Path[FPath.Count-1];
                else
                    return null;
            }
        }

        public string SelParam
        {
            get { return hidPar.Value; }
        }

        public string SelPosY
        {
            get { return hidPosY.Value; }
        }

        public bool Enabled
        {
            get 
            { 
                object e = ViewState[ID + "_enabled"];
                return e == null ? true : (bool)e;
            }
            set 
            {
                if (value != Enabled)
                {
                    ViewState[ID + "_enabled"] = value;
                    RenderPath();
                }
            }
        }
   }
}