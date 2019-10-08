using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace VCheckLib
{
    public class cRecognize
    {
        public char SelectClassificationValueByCharacterImage(string charImg)
        {            
            char character = '-';

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_TrainingCharacter_selectClassificationValue");
                sqlCmd.Parameters.Add("@CharacterImage", SqlDbType.NVarChar).Value = charImg;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    character = Convert.ToChar(objResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return character;
        }

    }
}
