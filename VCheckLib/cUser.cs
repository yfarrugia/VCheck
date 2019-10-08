using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VCheckLib
{
    public class cUser
    {

        public bool CheckUsername(string username)
        {
            bool result = false;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Account_checkUsername");
                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
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
        public static int InsertUser(string Username, string Password, string Email, int UserTypeID, bool IsEnabled)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Account_insert");

                sqlCmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username;
                sqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Password;
                sqlCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
                sqlCmd.Parameters.Add("@UserTypeID", SqlDbType.Int).Value = UserTypeID;
                sqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = IsEnabled;

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
        public DataTable SelectAccountByUsername(string username)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Account_selectByUsername");

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
        public DataTable SelectAccountByUserTypeaAndDisabled(int userTypeId, bool isEnabled)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Account_selectByUsername");

                sqlCmd.Parameters.Add("@username", SqlDbType.Int).Value = userTypeId;
                sqlCmd.Parameters.Add("@username", SqlDbType.Bit).Value = isEnabled;
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
        public int SelectAccountIDByUsername(string username)
        {
            int id = 0;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Account_selectIdByUsername");
                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    id = (int)(objResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }
        public int SelectSystemUserIDByUsername(string username)
        {
            int id = 0;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_selectIDByUsername");
                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    id = (int)(objResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }


        
        public string SelectAccountPasswordbyUsername(string username)
        {
            string password = null;
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Account_selectPasswordByUsername");
                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    password = (string)(objResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return password;
        }
        public int SelectUserTypeIDByUsername(string username)
        {
            int UserTypeID = 0;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_Account_selectUserTypeByUsername");
                sqlCmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    UserTypeID = (int)(objResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return UserTypeID;
        }
       
        
        public static int UpdateDefaultNetwork(int UserId, int DefaultNetworkID)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_updateDefaultNetwork");

                sqlCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                sqlCmd.Parameters.Add("@DefaultNetworkID", SqlDbType.Int).Value = DefaultNetworkID;

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteNonQuery(sqlCmd);

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
        public static int UpdateDefaultTrainingSet(int UserId, int TrainingSetID)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_updateDefaultTrainingSet");

                sqlCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                sqlCmd.Parameters.Add("@TrainingSetID", SqlDbType.Int).Value = TrainingSetID;

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteNonQuery(sqlCmd);

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
        public int SelectDefaultTrainingSetByUsername(string Username)
        {
            int UserTypeID = 0;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_SystemUser_selectTrainingSetByUsername");
                sqlCmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    UserTypeID = (int)(objResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return UserTypeID;
        }

    }

}
