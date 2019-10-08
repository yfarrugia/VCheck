using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VCheckLib
{
    public class cVehicle
    {
        public DataTable SelectAllVehicleTypes()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_VehicleType_selectall");

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
        public DataTable SelectAllRegCountries()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Country_selectall");

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
        public DataTable SelectCountriesByVehicleType(int VehicleTypeId)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Country_selectByVehicleTypeID");
                sqlCmd.Parameters.Add("@VehicleTypeID", SqlDbType.Int).Value = VehicleTypeId;
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
        public DataTable SelectLPRegByCountryIdAndVehicleTypeId(int CountryId, int VehicleTypeId)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_LPSyntax_selectLPSyntaxByCountryIDAndVehicleTypeid");
                sqlCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryId;
                sqlCmd.Parameters.Add("@VehicleTypeID", SqlDbType.Int).Value = VehicleTypeId;
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

        public static int InsertVehicle(string Vehicle, string LicensePlate, int VehicleTypeID, int UserID, int LPSyntaxID, string HashKey)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Vehicle_insert");

                sqlCmd.Parameters.Add("@Vehicle", SqlDbType.NVarChar).Value = Vehicle;
                sqlCmd.Parameters.Add("@LicensePlate", SqlDbType.NChar).Value = LicensePlate;
                sqlCmd.Parameters.Add("@VehicleTypeID", SqlDbType.Int).Value = VehicleTypeID;
                sqlCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                sqlCmd.Parameters.Add("@LPSyntaxID", SqlDbType.Int).Value = LPSyntaxID;
                sqlCmd.Parameters.Add("@HashKey", SqlDbType.NVarChar).Value = HashKey;

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

        public int CheckLP(string lp)
        {
            int result = -1;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Vehicle_checkLicensePlate");
                sqlCmd.Parameters.Add("@LicensePlate", SqlDbType.NVarChar).Value = lp;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    result = (int)objResult;
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
            return result;
        }
        public DataTable SelectVehiclesByUsername(string username)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Vehicle_selectByUsername");

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
        public static int DisableVehicle(int id)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Vehicle_updateIsEnabled");

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

        //ACCESS LOG
        public static int InsertAccessLog(int VehicleID, int SysUserId,string RecognizedLP)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_AccessLog_insert");

                sqlCmd.Parameters.Add("@VehicleID", SqlDbType.Int).Value = VehicleID;
                sqlCmd.Parameters.Add("@SystemUserId", SqlDbType.Int).Value = SysUserId;
                sqlCmd.Parameters.Add("@RecognizedLP", SqlDbType.NVarChar).Value = RecognizedLP;
               
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
        public DataTable SelectAccessLogByUsernameMonth(string username, int month)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_AccessLog_selectByUsername");

                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.Parameters.Add("@month", SqlDbType.Int).Value = month;
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
        public DataTable SelectAccessLogVehiclesByUsernameMonth(string username, int month)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_AccessLog_selectVehiclesByUsername");

                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.Parameters.Add("@month", SqlDbType.Int).Value = month;
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
        public DataTable SelectAccessLogByVehicleMonth(int vehicleId, int month)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_AccessLog_selectByVehicleIDUsernameMonth");

                sqlCmd.Parameters.Add("@VehicleID", SqlDbType.Int).Value = vehicleId;
                sqlCmd.Parameters.Add("@month", SqlDbType.Int).Value = month;
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
    }
}
