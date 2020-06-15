using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JFDAL;

namespace testjfdal
{
    class Program
    {


        
        static void Main(string[] args)
        {

            try
            {
                List<SqlParameter> parameter = new List<SqlParameter>();
                DAL fjd = new DAL();
                string con = fjd.ConnectionString(@".\SQL2016", "SAPCMA", "sa", "qweasd");
                var dt = fjd.Text_DT(con, @"select * from OITM", parameter);

                foreach (DataColumn col in dt.Columns)
                {
                    Console.WriteLine(col.ColumnName);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
       
           
          
        }
    }
}
