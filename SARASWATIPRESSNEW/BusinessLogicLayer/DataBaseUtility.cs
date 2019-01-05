using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.BusinessLogicLayer
{
    public class DataBaseUtility
    {
        public SqlConnection Conn;
        int flag = 0;
        public DataBaseUtility()
        {
        }
        public int SqlConnection()
        {
            // string ConString = "Data Source=DELL-PC;Initial Catalog=WSMS;Integrated Security=True";
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
            flag = 1;
            return flag;
        }
        public void ConnectDb()
        {
            try
            {
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public void DisconnectDb()
        {
            try
            { Conn.Close(); }
            catch (Exception ex)
            { throw new Exception(); }
        }
        public DataTable GetDataTable(SqlCommand Cmd)
        {
            try
            {
                SqlConnection();
                Cmd.Connection = Conn;
                Cmd.CommandTimeout = 0;
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                Dt.TableName = "dtData";
                Conn.Close();
                return Dt;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { Conn.Close(); }
        }
        public DataSet GetDataSet(SqlCommand Cmd)
        {
            try
            {
                SqlConnection();
                Cmd.Connection = Conn;
                Cmd.CommandTimeout = 0;
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                DataSet DataReturn = new DataSet();
                Da.Fill(DataReturn, "returnTable");
                Conn.Close();
                return DataReturn;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { Conn.Close(); }
        }
        public int ExNonQuery(SqlCommand Cmd)
        {
            SqlTransaction SqlCmdTransaction;
            SqlConnection();
            Conn.Open();

            SqlCmdTransaction = Conn.BeginTransaction();
            try
            {
                Cmd.Connection = Conn;
                Cmd.CommandTimeout = 0;
                Cmd.Transaction = SqlCmdTransaction;
                int result = Cmd.ExecuteNonQuery();
                SqlCmdTransaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                SqlCmdTransaction.Rollback();
                throw new Exception(ex.Message);
                return 0;
            }
            finally
            { Conn.Close(); }
        }
        public string ExScaler(SqlCommand Cmd)
        {
            try
            {
                SqlConnection();
                Cmd.Connection = Conn;
                Conn.Open();
                string result = Cmd.ExecuteScalar().ToString();
                return result;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
            finally
            { Conn.Close(); }
        }
    }

    public class DataBaseUtilityUpdated
    {
        //public SqlConnection Conn;
        //int flag = 0;
        static string ConnStr;
        public DataBaseUtilityUpdated()
        {

        }

        public DataBaseUtilityUpdated(string _connstr)
        {
            ConnStr = _connstr;
        }

        /*public int SqlConnection()
        {
            // string ConString = "Data Source=DELL-PC;Initial Catalog=WSMS;Integrated Security=True";
            //Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
            flag = 1;
            return flag;
        }*/
        public void ConnectDb()
        {
            /*try
            {
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }*/
        }
        public void DisconnectDb()
        {
            try
            { /*Conn.Close();*/ }
            catch (Exception ex)
            { throw new Exception(); }
        }
        public DataTable GetDataTable(SqlCommand Cmd)
        {
            try
            {
                DataTable Dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnStr].ToString()))
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                    cn.Open();
                    Cmd.Connection = cn;
                    Cmd.CommandTimeout = 0;
                    SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                    Da.Fill(Dt);
                    Dt.TableName = "dtData";
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                        cn.Dispose();
                    }
                }
                return Dt;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public DataSet GetDataSet(SqlCommand Cmd)
        {
            try
            {
                DataSet DataReturn = new DataSet();
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnStr].ToString()))
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                    cn.Open();
                    Cmd.Connection = cn;
                    Cmd.CommandTimeout = 0;
                    SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                    Da.Fill(DataReturn, "returnTable");
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                        cn.Dispose();
                    }
                }
                return DataReturn;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int ExNonQuery(SqlCommand Cmd)
        {
            //SqlTransaction SqlCmdTransaction;
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnStr].ToString()))
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                    cn.Open();
                    //SqlCmdTransaction = cn.BeginTransaction();
                    Cmd.Connection = cn;
                    Cmd.CommandTimeout = 0;
                    //Cmd.Transaction = SqlCmdTransaction;
                    int result = Cmd.ExecuteNonQuery();
                    //SqlCmdTransaction.Commit();
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                        cn.Dispose();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                //SqlCmdTransaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public string ExScaler(SqlCommand Cmd)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnStr].ToString()))
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                    cn.Open();
                    Cmd.Connection = cn;
                    string result = Cmd.ExecuteScalar().ToString();
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                        cn.Dispose();
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }

    public enum ConnStringContainer
    {
        [System.ComponentModel.Description("mycon")]
        mycon = 2,
        [System.ComponentModel.Description("mycon17-18")]
        mycon2018 = 1
    }

    public static class UtilityCustom
    {
        public static string GetCustomDescription(Enum en)
        {
            System.Reflection.FieldInfo fieldInfo = en.GetType().GetField(en.ToString());
            System.ComponentModel.DescriptionAttribute[] descriptionAttribute =
                  (System.ComponentModel.DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                  typeof(System.ComponentModel.DescriptionAttribute), false);
            return (descriptionAttribute.Length > 0) ? descriptionAttribute[0].Description : null;
        }
    }
}