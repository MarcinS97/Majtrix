using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using System.Text;

namespace HRRcp.Portal.Controls
{
    public partial class cntKalendarz : System.Web.UI.UserControl
    {

        Pliki pl = new Pliki();


        public class DisplayEvent
        {
            public int id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string text { get; set; }
            public string end { get; set; }
            public string reminder { get; set; }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
            String a = Report.DecryptQueryString("u1Z6s1QTr2CmBoG5oAK6ptCOPF7H08h06YgFvnmMctE=", Grid.key, Grid.salt);
            String b = Report.DecryptQueryString("jA4Ax+Q7MPjwvNW8Lm0oV569Hy1JtjRkW6qO9fqrTAs=", Grid.key, Grid.salt);
            if (!IsPostBack)
            {

            }
            DataTable dt = db.Select.Table(dsCalendar);
            //  var s = dt.Rows["DataStart"].ToString();
            foreach (DataRow item in dt.Rows)
            {
                DisplayEvent de = new DisplayEvent();
                de.id = int.Parse(item["Id"].ToString());
                de.title = item["Nazwa"].ToString();
                de.text = item["Opis"].ToString();
                de.start = item["DataStart"].ToString();
                de.end = item["DataKoniec"].ToString();
                de.reminder = item["DataPrzypomnienia"].ToString();


            }

        }

        protected void btnAsd_Click(object sender, EventArgs e)
        {

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
          //  PlikiDoBazy.Zapis();
            //DataTable dt = db.Select.Table(SqlDataSource1);
            //int id;
            //string sciezka;
            //string[] plik;
            //string sql;
            //foreach (DataRow item in dt.Rows)
            //{
            //    id = int.Parse(item["Id"].ToString());
            //    sciezka = item["Sciezka"].ToString();
            //    plik = Directory.GetFiles(@sciezka);
            //    sql = item["Sql"].ToString();
            //    string tym;
            //    //SqlDataSource1.InsertCommandType = SqlDataSourceCommandType.Text;
            //    DataRow sqq;
                
            //    foreach (var pli in plik)
            //    {

            //        tym = pli.Remove(0, sciezka.Count()+1);
            //        sqq = db.Select.Row(sql,tym);

            //        StringBuilder cols = new StringBuilder();
            //        StringBuilder vals = new StringBuilder();

            //        foreach (DataColumn col in sqq.Table.Columns)
            //        {
            //            cols.AppendFormat(", {0}", col.ColumnName);
            //            vals.AppendFormat(", {0}", sqq[col.ColumnName]);
            //        }

            //       // db.execSQL(db.conP,SqlDataSource1.InsertCommand, id, tym, cols.ToString(), vals.ToString());
            //        db.execSQL(db.conP,String.Format(SqlDataSource1.InsertCommand, id, tym, cols.ToString(), vals.ToString()));
                    

            //        /*db.Execute("insert into PlikiWSciezkach(IdSciezki, NazwaPliku, Idx, Datax) values ({0}, '{1}', '{2}', '{3}')", id, tym, sqq[1], sqq[0]);*/



            //        //SqlDataSource1.InsertCommand = "Insert into PlikiWSciezkach (NazwaPliku,Sciezka) VALUES (@NazwaPliku, @Sciezka)";
            //        //SqlDataSource1.InsertParameters.Add("@NazwaPliku", pli);
            //        //SqlDataSource1.InsertParameters.Add("@Sciezka", sciezka);
            //        //SqlDataSource1.Insert();
            //    }
            //}
        }
    }
}