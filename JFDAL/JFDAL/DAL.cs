using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace JFDAL
{
    public class DAL
    {
        List<SqlParameter> parameters = new List<SqlParameter>();
        SqlParameter param = new SqlParameter();


        private string DynConn;
        private string DynConnVal
        {
            get
            {
                return DynConn;
            }

            set
            {
                DynConn = value;
            }
        }

        public string ConnectionString(string Server_Name, string Db_Name, string UserId, string Password)
        {
            return DynConnVal = string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}"
                                                , Server_Name
                                                , Db_Name
                                                , UserId
                                                , Password
                                             );
        }

        #region Create Parameter
        /*Create Parameter in Model*/
        public List<SqlParameter> CreateParameter(dynamic model)
        {
            List<dynamic> mod = new List<dynamic>();
            parameters = new List<SqlParameter>();
            param = new SqlParameter();

            mod.Add(model);
            foreach (var KeyCol in mod)
            {
                foreach (var col in KeyCol.GetType().GetProperties())
                {
                    param = new SqlParameter();
                    param.ParameterName = col.Name;
                    if (!col.PropertyType.Name.Contains("List"))
                    {
                        if (col.GetValue(KeyCol) != null)
                        {
                            param.Value = col.GetValue(KeyCol).ToString();
                        }
                        parameters.Add(param);
                    }
                }
            }
            return parameters;
        }

        /*Create Parameter in DataTable*/
        public List<SqlParameter> CreateParameterDataTable(DataTable dt)
        {
            parameters = new List<SqlParameter>();
            foreach (DataColumn col in dt.Columns)
            {
                param = new SqlParameter();
                foreach (DataRow row in col.Table.Rows)
                {
                    param.Value = row[col].ToString();
                    param.ParameterName = col.ToString();
                    parameters.Add(param);
                }
            }
            return parameters;
        }
        #endregion


        public DataTable Text_DT(string connectionString, string Query, List<SqlParameter> parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(Query, con))
                {

                    com.CommandType = CommandType.Text;


                    foreach (var param in parameters)
                    {
                        com.Parameters.AddWithValue(param.ParameterName, param.Value);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(com))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;

        }

        public DataTable StoredProc_DT(string connectionString, string StoredProcedure_Name, List<SqlParameter> parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(StoredProcedure_Name, con))
                {

                    com.CommandType = CommandType.StoredProcedure;

                    foreach (var param in parameters)
                    {
                        com.Parameters.AddWithValue(param.ParameterName, param.Value);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(com))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }


    }
}
