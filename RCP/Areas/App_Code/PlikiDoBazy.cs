using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HRRcp.App_Code;


namespace HRRcp.App_Code
{
    public static class PlikiDoBazy
    {
        public static void Zapis(int idSciezki)
        {
            db.execSQL(db.conP,String.Format("DELETE FROM PLIKI WHERE IdSciezki={0}",idSciezki));
            //zapis do bazy z katalogu z bazy danych [PlikiSciezki] z plikami do [Pliki]
            DataTable dt = db.Select.Table(db.conP, 0, String.Format("select * from PlikiSciezki where Id = {0}", idSciezki));    // nie było zawężenia zakresu
            //db.Select.Table();
            int id;
            string sciezka;
            string[] plik;
            string sql;
            foreach (DataRow item in dt.Rows)       // zawsze po zawężeniu zakreu będzie 1 rekord
            {
                // data     
                id = int.Parse(item["Id"].ToString());
                sciezka = item["Sciezka"].ToString();
                plik = Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath(sciezka));
                sql = item["Sql"].ToString();
                string tym;
                //SqlDataSource1.InsertCommandType = SqlDataSourceCommandType.Text;
                DataRow sqq;

                foreach (var pli in plik)
                {

                   // tym = pli.Remove(0, sciezka.Count() + 1);
                    tym = pli.Remove(0, System.Web.HttpContext.Current.Server.MapPath(sciezka).Count()+1);
                    sqq = db.Select.Row(db.conP, sql, tym);

                    StringBuilder cols = new StringBuilder();
                    StringBuilder vals = new StringBuilder();

                    foreach (DataColumn col in sqq.Table.Columns)
                    {
                        cols.AppendFormat(", {0}", col.ColumnName);
                        vals.AppendFormat(", {0}", sqq[col.ColumnName]);
                    }

                    if (idSciezki == id)
                    {
                        db.execSQL(db.conP, String.Format(" insert Pliki (IdSciezki, NazwaPliku{2}) select {0}, '{1}'{3}", id, tym, cols.ToString(), vals.ToString()));

                    }
                   
                    // db.execSQL(db.conP,SqlDataSource1.InsertCommand, id, tym, cols.ToString(), vals.ToString());


                    /*db.Execute("insert into PlikiWSciezkach(IdSciezki, NazwaPliku, Idx, Datax) values ({0}, '{1}', '{2}', '{3}')", id, tym, sqq[1], sqq[0]);*/



                    //SqlDataSource1.InsertCommand = "Insert into PlikiWSciezkach (NazwaPliku,Sciezka) VALUES (@NazwaPliku, @Sciezka)";
                    //SqlDataSource1.InsertParameters.Add("@NazwaPliku", pli);
                    //SqlDataSource1.InsertParameters.Add("@Sciezka", sciezka);
                    //SqlDataSource1.Insert();
                }
            }
              
        }

        /*
        public static void Zapis(int idSciezki)
        {
            db.execSQL(db.conP,String.Format("DELETE FROM PLIKI WHERE IdSciezki={0}",idSciezki));
            //zapis do bazy z katalogu z bazy danych [PlikiSciezki] z plikami do [Pliki]
            DataTable dt = db.Select.Table(db.conP, 0, String.Format("select * from PlikiSciezki"));
            //db.Select.Table();
            int id;
            string sciezka;
            string[] plik;
            string sql;
            foreach (DataRow item in dt.Rows)
            {
                // data     
                id = int.Parse(item["Id"].ToString());
                sciezka = item["Sciezka"].ToString();
                plik = Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath(sciezka));
                sql = item["Sql"].ToString();
                string tym;
                //SqlDataSource1.InsertCommandType = SqlDataSourceCommandType.Text;
                DataRow sqq;

                foreach (var pli in plik)
                {

                   // tym = pli.Remove(0, sciezka.Count() + 1);
                    tym = pli.Remove(0, System.Web.HttpContext.Current.Server.MapPath(sciezka).Count()+1);
                    sqq = db.Select.Row(db.conP, sql, tym);

                    StringBuilder cols = new StringBuilder();
                    StringBuilder vals = new StringBuilder();

                    foreach (DataColumn col in sqq.Table.Columns)
                    {
                        cols.AppendFormat(", {0}", col.ColumnName);
                        vals.AppendFormat(", {0}", sqq[col.ColumnName]);
                    }

                    if (idSciezki == id)
                    {
                        db.execSQL(db.conP, String.Format(" insert Pliki (IdSciezki, NazwaPliku{2}) select {0}, '{1}'{3}", id, tym, cols.ToString(), vals.ToString()));

                    }
                   
                    // db.execSQL(db.conP,SqlDataSource1.InsertCommand, id, tym, cols.ToString(), vals.ToString());


                    //db.Execute("insert into PlikiWSciezkach(IdSciezki, NazwaPliku, Idx, Datax) values ({0}, '{1}', '{2}', '{3}')", id, tym, sqq[1], sqq[0]);



                    //SqlDataSource1.InsertCommand = "Insert into PlikiWSciezkach (NazwaPliku,Sciezka) VALUES (@NazwaPliku, @Sciezka)";
                    //SqlDataSource1.InsertParameters.Add("@NazwaPliku", pli);
                    //SqlDataSource1.InsertParameters.Add("@Sciezka", sciezka);
                    //SqlDataSource1.Insert();
                }
            }
              
        }
         */


    }
}