using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VCheckLib
{
    public class cTrain
    {
        //Training Set
        public static int InsertTrainingSetReturnId(string Name, int NoCharacters, string ServerDir)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingSet_insert");

                sqlCmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                sqlCmd.Parameters.Add("@NoCharacters", SqlDbType.Int).Value = NoCharacters;
                sqlCmd.Parameters.Add("@ServerDirectory", SqlDbType.NVarChar).Value = ServerDir;

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
        public DataTable SelectAllTrainingSets()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingSet_selectall");
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
        public int SelectNoCharactersById(int Id)
        {
            int numOfCharacters = 0;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingSet_selectnoCharactersbyid");
                sqlCmd.Parameters.Add("@ID", SqlDbType.Int).Value = Id;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    numOfCharacters = (int)objResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return numOfCharacters;
        }
        public static int UpdateTrainingSet(int NoCharacters, int AvHeight, int AvWidth, int Id)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingSet_update");

                sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                sqlCmd.Parameters.Add("@NoCharacters", SqlDbType.Int).Value = NoCharacters;
                sqlCmd.Parameters.Add("@AvHeight", SqlDbType.Int).Value = AvHeight;
                sqlCmd.Parameters.Add("@AvWidth", SqlDbType.Int).Value = AvWidth;

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
        public static int UpdateTrainingSetNoCharacters(int NoCharacters, int Id)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingSet_updateNoCharacters");

                sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                sqlCmd.Parameters.Add("@NoCharacters", SqlDbType.Int).Value = NoCharacters;

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
        public DataTable SelectAverageTrainingSetDimensionsById(int Id)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingSet_selectAverageDimensions");
                
                sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
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
        public string SelectTrainingSetServerDirectoryById(int Id)
        {
            string path = null;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingSet_selectServerDirectoryById");
                sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    path = (string)objResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return path;
        }

        //Training Character
        public static int InsertTrainingCharacter(int TrainingSetID, string CharacterImageName, int ClassificationValueID)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingCharacter_insert");

                sqlCmd.Parameters.Add("@TrainingSetID", SqlDbType.Int).Value = TrainingSetID;
                sqlCmd.Parameters.Add("@CharacterImage", SqlDbType.NVarChar).Value = CharacterImageName;
                sqlCmd.Parameters.Add("@ClassificationValueID", SqlDbType.Int).Value = ClassificationValueID;

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

        //Classification Value  
        public DataTable SelectClassificationValues()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_ClassificationValue_selectall");
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
        public DataTable SelectClassificationValuesWidthHeightById(int cvId)
        {
            
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_ClassificationValue_selectSizebyId");
                sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = cvId;
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
