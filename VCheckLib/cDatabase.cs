using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Net;
using Microsoft.Win32;

namespace VCheckLib
{
    public class cDatabase
    {
        private static SqlConnection conn = null;

        public static string DBConnection
        {
            //the below line of code was used when he classes where still residing in the same folder as the web config
            //return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["vcheck"].ConnectionString;
            get
            {
                try
                {
                    return System.Configuration.ConfigurationManager.ConnectionStrings["vcheck"].ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private static SqlConnection GetConnection()
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                }
                conn = new SqlConnection(DBConnection);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void CloseDBConnection()
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Stored procedures general methods to retrieve data

        //for class methods which require to return a data table
        public static DataTable GetDataTable(SqlCommand sqlComm)
        {
            DataTable result = new DataTable();

            try
            {
                GetConnection();
                sqlComm.Connection = conn;

                SqlDataAdapter da = new SqlDataAdapter(sqlComm);
                da.Fill(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDBConnection();
            }

            return result;
        }

        //for class methods which require to return an object
        public static Object ExecuteScalar(SqlCommand sqlComm)
        {
            Object result = new Object();

            try
            {
                GetConnection();
                sqlComm.Connection = conn;

                result = sqlComm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDBConnection();
            }

            return result;
        }

        //for class methods which require the number of rows effected as a result
        public static int ExecuteNonQuery(SqlCommand sqlComm)
        {
            int result;

            try
            {
                GetConnection();
                sqlComm.Connection = conn;

                result = sqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDBConnection();
            }

            return result;
        }

        //for class methods which require the ID
        public static int ReturnID(SqlCommand sqlComm)
        {
            Object result = new Object();

            try
            {
                GetConnection();
                sqlComm.Connection = conn;

                //; SELECT SCOPE_IDENTITY();

                result = sqlComm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDBConnection();
            }

            return Convert.ToInt32(result);
        }


    }
}
