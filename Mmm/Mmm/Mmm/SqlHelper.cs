using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mmm
{
    public  class SqlHelper
    {
     static string connStr=System.Configuration.ConfigurationSettings.AppSettings["connectionString"];
     public static int ExecuteNonQuery(string commandText, CommandType type, params SqlParameter[] pms)
     {
         SqlConnection conn = new SqlConnection();
         conn.ConnectionString = connStr;
         conn.Open();
         SqlCommand cmd = new SqlCommand(commandText,conn);
         if (type==CommandType.StoredProcedure)
         {
             cmd.CommandType = CommandType.StoredProcedure;
         }
         if (pms!=null)
         {
             cmd.Parameters.AddRange(pms);
         } 
         int count = cmd.ExecuteNonQuery();
        conn.Close();
         return count;
     }

     //重载2
     public static int ExecuteNonQuery(string commandText, CommandType type)
     {
         return ExecuteNonQuery(commandText, type, null);
     }
     //重载3
     public static int ExecuteNonQuery(string commandText)
     {
         return ExecuteNonQuery(commandText, CommandType.Text);
     }
     //重载4
     public static int ExecuteNonQuery(string commandText, params SqlParameter[] pms)
     {
         return ExecuteNonQuery(commandText, CommandType.Text, pms);
     }




     public static DataTable GetTable(string commandText, CommandType type, params SqlParameter[] pms)
     {
         SqlConnection conn = new SqlConnection(connStr);
         conn.Open();
         SqlCommand cmd = new SqlCommand(commandText,conn);
         if (type==CommandType.StoredProcedure)
         {
             cmd.CommandType = CommandType.StoredProcedure;
         }

         if (pms!=null)
         {
             cmd.Parameters.AddRange(pms);
         }
         SqlDataAdapter sda = new SqlDataAdapter(cmd);      
         DataSet ds = new DataSet();
         sda.Fill(ds, "t");
         DataTable dt = ds.Tables["t"];
         conn.Close();
         return dt;

     }
     //重载2
     public static DataTable GetTable(string commandText, CommandType type)
     {
         return GetTable(commandText, type, null);


     }
     //重载3
     public static DataTable GetTable(string commandText)
     {
         return GetTable(commandText, CommandType.Text);
     }
     //重载4
     public static DataTable GetTable(string commandText, params SqlParameter[] pms)
     {
         return GetTable(commandText, CommandType.Text, pms);
     }




     public static SqlDataReader GetReader(string commandText, CommandType type, params SqlParameter[] pms)
     {
      
  SqlConnection  conn = new SqlConnection(connStr);
   conn.Open();
         SqlCommand cmd = new SqlCommand(commandText, conn);

         if (type == CommandType.StoredProcedure)
         {

             cmd.CommandType = CommandType.StoredProcedure;
         }

         if (pms != null)
         {
             cmd.Parameters.AddRange(pms);
         }

         SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
         return sdr;


     }
       


        //重载2
     public static SqlDataReader GetReader(string commandText, CommandType type )
     {

         return GetReader(commandText, type, null);

     }
        //3
     public static SqlDataReader GetReader(string commandText)
     {
         return GetReader(commandText, CommandType.Text);
     }
        //4
     public static SqlDataReader GetReader(string commandText,params SqlParameter[] pms)
     {
         return GetReader(commandText,CommandType.Text,pms);
     }





     public static bool ExecuteTran(List<string> sqls)
     {

         using (SqlConnection conn = new SqlConnection(connStr))
         {
             conn.Open();
             SqlTransaction st = conn.BeginTransaction();
             SqlCommand cmd = new SqlCommand();
             cmd.Connection = conn;
          
             cmd.Transaction = st;
             try
             {
                 foreach (string sql in sqls)
                 {
                     cmd.CommandText = sql;
                     cmd.ExecuteNonQuery();
                 }
                 //执行循环成功
                 st.Commit();
                 return true;

             }
             catch
             {
                 st.Rollback();//出错回滚
                 return false;

             }

         }

     }

 

     public static bool ExecuteTran(List<string> sqls, params SqlParameter[] pms)
     {
         return ExecuteTran(sqls);
     }




        }
}
