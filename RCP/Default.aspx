<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HRRcp._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>


<%--

<% @ Import Namespace="HRRcp.App_Code" %>
<% @ Import Namespace="System.Data" %>
<% @ Import Namespace="System.Data.SqlClient" %>

<scr ipt language="C#" type="text/C#" runat="server">
    public static string GetLogin()
    {
        String user = HttpContext.Current.User.Identity.Name;
        if (!String.IsNullOrEmpty(user))
        {
            int i = user.LastIndexOf('\\');     // KOMPUTER\\User -> \User -> User
            if (i >= 0)
                user = user.Remove(0, i + 1);
        }
        return user;
    }

    public void Page_Init(object sender, EventArgs e)
    {
    }
    
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string cs = ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand(String.Format("select * from Pracownicy where Login = '{0}'", GetLogin()), con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();    
            da.Fill(ds);
            con.Close();

            string k = ds.Tables[0].Rows[0]["Kierownik"].ToString().ToLower();
            string a = ds.Tables[0].Rows[0]["Admin"].ToString().ToLower();

            if (k == "false" && a == "false")
                //Response.Redirect("~/Default.aspx");
                Response.Redirect("~/ErrorForm.aspx");
                
            //if (!HRRcp.App_Code.App.User.IsKierownik && !HRRcp.App_Code.App.User.IsAdmin)
            //    Response.Redirect(HRRcp.App_Code.App._StartForm);
        }
    }    
</script>

--%>