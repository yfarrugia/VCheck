using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace VCheckLib
{
    public class cSystem
    {
        public DataTable SelectAllSystemTypes()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemType_selectall");

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable dtResult = cDatabase.GetDataTable(sqlCmd);
                if (dtResult.Rows.Count > 0)
                {
                    return dtResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectSystemByUsername(string username)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_selectByUsername");

                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable dtResult = cDatabase.GetDataTable(sqlCmd);
                if (dtResult.Rows.Count > 0)
                {
                    return dtResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectLatLongByUsername(string username)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_selectLatLongByUsername");

                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable dtResult = cDatabase.GetDataTable(sqlCmd);
                if (dtResult.Rows.Count > 0)
                {
                    return dtResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectAllDisabledSystems()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemType_selectDisabled");

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable dtResult = cDatabase.GetDataTable(sqlCmd);
                if (dtResult.Rows.Count > 0)
                {
                    return dtResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectAllEnabledSystems()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemType_selectEnabled");

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable dtResult = cDatabase.GetDataTable(sqlCmd);
                if (dtResult.Rows.Count > 0)
                {
                    return dtResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable CountSystemsActiveAndNotActive()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Account_countSystemUsers");

                //sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable dtResult = cDatabase.GetDataTable(sqlCmd);
                if (dtResult.Rows.Count > 0)
                {
                    return dtResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static int InsertSystem(int UserID, int SystemID, string SystemName, int TrainingSetID, string Address, string Town, int CountryID, decimal Latitude, decimal Longitude)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_insert");

                sqlCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                sqlCmd.Parameters.Add("@SystemID", SqlDbType.Int).Value = SystemID;
                sqlCmd.Parameters.Add("@SystemName", SqlDbType.NVarChar).Value = SystemName;
                sqlCmd.Parameters.Add("@TrainingSetID", SqlDbType.Int).Value = TrainingSetID;
                sqlCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
                sqlCmd.Parameters.Add("@Town", SqlDbType.NVarChar).Value = Town;
                sqlCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                sqlCmd.Parameters.Add("@Latitude", SqlDbType.Decimal).Value = Latitude;
                sqlCmd.Parameters.Add("@Longitude", SqlDbType.Decimal).Value = Longitude;
                
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ReturnID(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    return (int)objResult;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CheckSystemName(string sysName)
        {
            bool result = false;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_checkSystemName");
                sqlCmd.Parameters.Add("@SystemName", SqlDbType.NVarChar).Value = sysName;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public static int EnableSystem(int id)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_updateIsDisabled");

                sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteNonQuery(sqlCmd);

                if (objResult != null)
                {
                    return (int)objResult;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int DeleteSystem(int id)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_delete");

                sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteNonQuery(sqlCmd);

                if (objResult != null)
                {
                    return (int)objResult;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
