using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class LetterDataPager : System.Web.UI.UserControl
    {
        string FGetSql = null;
        string FTbName = null;
        string FLetter = null;
        string FWhere = null;
        string FParName1 = null;
        string FParField1 = null;
        string FOffset = null;  // sql ile trzeba dodać do pozycji zeby sie zgadzało - jak np kierownicy z przodu listy Pracowników
        
        int FPageSize = 0;      // <<< do wycięcia ???
        
        //bool FEnabled = true;

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(FTbName) && (String.IsNullOrEmpty(Letters) || !IsPostBack))   // FTbName == null kiedy ustawiam przez Update(SqlDataSource)
                Letters = GetLetters();
            CreateLetters(Letters);
        }

        public void Reset()                             // wywoływać przy konieczności odświeżenia, dalej się wypełni w Update wywołanym w lv_OnDataBound
        {
            Letters = null;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public void Update(ListView lv, bool create)     // do podpięcia w lv_OnDataBound
        {
            if (String.IsNullOrEmpty(FTbName) && String.IsNullOrEmpty(Letters))      // kompatybilność z I wersją
            {
                SqlDataSource sqlDS = Tools.GetSqlDataSource(lv);

                DataSourceSelectArguments args = new DataSourceSelectArguments();
                DataView view = (DataView)sqlDS.Select(args);
                DataTable dt = view.ToTable();

                var query = (from r in dt.AsEnumerable()
                             group r by new
                             {
                                 Letter = r.Field<string>(FLetter)
                             } into g
                             select new
                             {
                                 g.Key.Letter,
                                 Count = g.Count()
                             }).OrderBy(r => r.Letter);

                int count = query.Count();
                string[] LA = new string[count * 2];    // litera, pozycja, litera, pozycja ...
                int i = 0;
                int cnt = GetOffset();                  // 0;
                foreach (var item in query)
                {
                    LA[i++] = item.Letter;
                    LA[i++] = cnt.ToString();
                    cnt += item.Count;
                }
                Letters = String.Join("|", LA);
            }
        
            
            
            if (create) CreateLetters(Letters);
        }






















        //-------------------------
        private string GetWhere()
        {
            if (!String.IsNullOrEmpty(FWhere))
            {
                if (!String.IsNullOrEmpty(FParName1) &&
                    !String.IsNullOrEmpty(FParField1))
                {
                    HiddenField hid = (HiddenField)this.Parent.TemplateControl.FindControl(FParField1);
                    if (hid != null && !String.IsNullOrEmpty(hid.Value))
                        return " where " + FWhere.Replace("@" + FParName1, hid.Value) + " ";
                    else
                        return null;  
                }
                return " where " + FWhere + " ";
            }
            return null;
        }

        private int GetOffset()
        {
            if (!String.IsNullOrEmpty(FOffset))
            {
                if (!String.IsNullOrEmpty(FParName1) &&
                    !String.IsNullOrEmpty(FParField1))
                {
                    HiddenField hid = (HiddenField)this.Parent.TemplateControl.FindControl(FParField1);
                    if (hid != null && !String.IsNullOrEmpty(hid.Value))
                    {
                        string sql = FOffset.Replace("@" + FParName1, hid.Value);
                        return Base.StrToIntDef(Base.getScalar(sql), 0);
                    }
                }
                else
                    return Base.StrToIntDef(Base.getScalar(FOffset), 0);
            }
            return 0;
        }

        private string GetLetters()
        {
            if (!String.IsNullOrEmpty(FTbName) && !String.IsNullOrEmpty(FLetter))
                FGetSql = "select " + FLetter + ",count(*) from " + TbName + 
                            GetWhere() +
                            " group by " + FLetter + 
                            " order by " + FLetter;
            if (!String.IsNullOrEmpty(FGetSql))
            {
                DataSet ds = Base.getDataSet(FGetSql);
                string[] LA = new string[ds.Tables[0].Rows.Count * 2];  // litera, pozycja, litera, pozycja ...
                int i = 0;
                int cnt = GetOffset();  // 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LA[i++] = dr[0].ToString();
                    LA[i++] = cnt.ToString();
                    cnt += Base.getInt(dr, 1, 0);
                }
                if (FPageSize > 0 && cnt <= FPageSize)  // jak wszystko sie miesci na jednej stronie to nie ma sensu dawac
                    return null;
                else /**/
                    return String.Join("|", LA);
            }
            else return null;
        }
        //-------------------------------------------
        private void CreateLetters(string lts)
        {
            phLetters.Controls.Clear();
            string[] LA = lts.Split('|');
            int cnt = LA.Count() / 2;
            int p = 0;
            for (int i = 0; i < cnt; i++)
            {
                if (i > 0)
                {
                    Literal lt = new Literal();
                    lt.Text = " ";
                    phLetters.Controls.Add(lt);
                }
                LinkButton lbt = new LinkButton();
                lbt.ID = "ldp" + i.ToString();
                lbt.Text = LA[p++];
                lbt.CommandArgument = LA[p++];
                lbt.CommandName = "JUMP";
                lbt.Enabled = Enabled;
                phLetters.Controls.Add(lbt);
            }
        }

        public void Prepare()
        {
            Letters = GetLetters();
            CreateLetters(Letters);
        }

        public void Reload()
        {
            Letters = GetLetters();
            CreateLetters(Letters);
        }
        //-------------------------
        public string GetSql
        {
            get { return FGetSql; }
            set { FGetSql = value; }
        }

        public string TbName
        {
            get { return FTbName; }
            set { FTbName = value; }
        }

        public string Letter
        {
            get { return FLetter; }
            set { FLetter = value; }
        }

        public string Where
        {
            get { return FWhere; }
            set { FWhere = value; }
        }

        public string Offset
        {
            get { return FOffset; }
            set { FOffset = value; }
        }
       
        public string ParName1
        {
            get { return FParName1; }
            set { FParName1 = value; }
        }

        public string ParField1
        {
            get { return FParField1; }
            set { FParField1 = value; }
        }

        public int PageSize
        {
            get { return FPageSize; }
            set { FPageSize = value; }
        }

        /*
        public bool Enabled
        {
            get { return FEnabled; }
            set { FEnabled = value; }
        }
        */
        public bool Enabled
        {
            get { return Tools.GetBool(ViewState["enabled"], true); }
            set 
            {
                if (Enabled != value)
                {
                    ViewState["enabled"] = value;
                    foreach (Control c in phLetters.Controls)
                        if (c is LinkButton)
                            ((LinkButton)c).Enabled = value;
                }
            }
        }

        public string Letters
        {
            set { ViewState["letters"] = value; }
            get { return Tools.GetStr(ViewState["letters"]); }
        }
    }
}