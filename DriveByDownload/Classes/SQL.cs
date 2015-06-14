using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Configuration;


namespace DriveByDownload.Classes
{


    //class SingleRandom : Random
    //{
    //    static SingleRandom _Instance;
    //    public static SingleRandom Instance
    //    {
    //        get
    //        {
    //            if (_Instance == null) _Instance = new SingleRandom();
    //            return _Instance;
    //        }
    //    }

    //    private SingleRandom() { }
    //}

    public class SQL
    {
        static SQL _Instance;
        //private SqlConnection scon;
        private String connectionString = ConfigurationManager.ConnectionStrings["alexaConnect"].ToString();
        public SQL()
        {
            //System.Diagnostics.Debug.WriteLine("In SQL Constructor" + System.Threading.Thread.CurrentThread.GetHashCode());
            //scon = new SqlConnection(this.connectionString);
            //scon.Open();
        }

        //public static SQL Instance
        //{
        //    get
        //    {
        //        //if (_Instance == null) 
        //        _Instance = new SQL();
        //        return _Instance;
        //    }
        //}

        //~SQL()
        //{
        //    //Not thread safe to close and dispose in destructor apparently 
        //    //http://social.msdn.microsoft.com/Forums/en-US/b23d8492-1696-4ccb-b4d9-3e7fbacab846/internal-net-framework-data-provider-error-1?forum=adodotnetdataproviders

        //}

        private SqlCommand Query(String command, SqlConnection scon, List<SqlParameter> queryParams = null, Boolean storedProcedure = false)
        {
            //try
            //{
            SqlCommand sc = new SqlCommand(command, scon);
            sc.CommandTimeout = 120;

            if (queryParams != null)
            {
                sc.Parameters.AddRange(queryParams.ToArray());
            }
            if(storedProcedure)
                sc.CommandType = CommandType.StoredProcedure;

            return sc;
            //}
            //catch
            //{
                
            //    return Query(command, queryParams);
            //}

            
        }

        /// <summary>
        /// Inserts a value into the database.  
        /// </summary>
        /// <param name="command">The DB query to run.  MUST have OUTPUT inserted.[primary key field], otherwise unpredictable</param>
        /// <param name="queryParams">The parameters to add to the query</param>
        /// <returns></returns>
        public int InsertQuery(String command, List<SqlParameter> queryParams = null)
        {
            using (SqlConnection scon = new SqlConnection(this.connectionString))
            {
                //if (this.scon.State == ConnectionState.Closed)
                scon.Open();
                SqlCommand sc = this.Query(command, scon, queryParams);
                return (int) sc.ExecuteScalar();
            }
        }

        public int UpdateQuery(String command, List<SqlParameter> queryParams = null)
        {
            using (SqlConnection scon = new SqlConnection(this.connectionString))
            {
                //if (this.scon.State == ConnectionState.Closed)
                scon.Open();
                SqlCommand sc = this.Query(command, scon, queryParams);
                return sc.ExecuteNonQuery();
            }
        }

        public DataTable SelectQuery(String command, List<SqlParameter> queryParams = null, Boolean storedProcedure = false)
        {
            try
            {
                using (SqlConnection scon = new SqlConnection(this.connectionString))
                {
                    scon.Open();
                    SqlDataReader sr = this.Query(command, scon, queryParams,storedProcedure).ExecuteReader();
                    DataTable dt = new DataTable();
                    if (sr.HasRows)
                    {
                        dt.Load(sr);
                    }
                    
                    return dt;
                }

            }
            //catch
            //{
            //    Thread.Sleep(new Random().Next(4000));
            //    return this.SelectQuery(command, queryParams);
            //}
            finally
            {
                //this.scon.Close();
                //this.scon.Dispose();
            }
        }

        public Object SingleValueQuery(String command, List<SqlParameter> queryParams = null)
        {
            //try
            //{
            using (SqlConnection scon = new SqlConnection(this.connectionString))
            {
                scon.Open();
                SqlCommand sc = this.Query(command, scon, queryParams);
                return sc.ExecuteScalar();                
            }

            //}
            //finally
            //{
            //    this.scon.Close();
            //    this.scon.Dispose();
            //}
        }


    }
}
