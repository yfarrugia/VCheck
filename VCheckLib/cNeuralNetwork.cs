using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Data;
using System.Data.SqlClient;

namespace VCheckLib
{

    public class cNeuralNetwork<T>
       where T : IComparable<T>
    {
        private iBackPropagation<T> NeuralNet;
        private double maximumError = 1.0;
        private int maximumIteration = 2300;
        Dictionary<T, double[]> TrainingSet;

        public double MaximumError
        {
            get { return maximumError; }
            set { maximumError = value; }
        }

        public int MaximumIteration
        {
            get { return maximumIteration; }
            set { maximumIteration = value; }
        }

        public delegate void IterationChangedCallBack(object o, NeuralEventArgs args);
        public event IterationChangedCallBack IterationChanged = null;

        public cNeuralNetwork(iBackPropagation<T> IBackPro, Dictionary<T, double[]> trainingSet, double maxError, int maxIteration)
        {
            maximumError = maxError;
            maximumIteration = maxIteration;

            NeuralNet = IBackPro;
            TrainingSet = trainingSet;
            NeuralNet.InitializeNetwork(TrainingSet);
        }

        public bool Train()
        {
            try
            {
                double currentError = 0;
                int currentIteration = 0;
                NeuralEventArgs Args = new NeuralEventArgs();

                do
                {
                    currentError = 0;
                    foreach (KeyValuePair<T, double[]> p in TrainingSet)
                    {
                        NeuralNet.ForwardPropagate(p.Value, p.Key);
                        NeuralNet.BackPropagate();
                        currentError += NeuralNet.GetError();
                    }

                    currentIteration++;

                    if (IterationChanged != null && currentIteration % 5 == 0)
                    {
                        Args.CurrentError = currentError;
                        Args.CurrentIteration = currentIteration;
                        IterationChanged(this, Args);
                    }

                } while (currentError > maximumError && currentIteration < maximumIteration && !Args.Stop);

                if (IterationChanged != null)
                {
                    Args.CurrentError = currentError;
                    Args.CurrentIteration = currentIteration;
                    IterationChanged(this, Args);
                }

                if (currentIteration >= maximumIteration || Args.Stop)
                    return false;//Training Not Successful

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Recognize(double[] Input, ref T MatchedHigh, ref double OutputValueHight,
            ref T MatchedLow, ref double OutputValueLow)
        {
            try
            {
                NeuralNet.Recognize(Input, ref MatchedHigh, ref OutputValueHight, ref MatchedLow, ref OutputValueLow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveNetwork(string path)
        {
            try
            {
                FileStream FS = new FileStream(path, FileMode.Create);
                BinaryFormatter BF = new BinaryFormatter();
                BF.Serialize(FS, NeuralNet);
                FS.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LoadNetwork(string path)
        {
            try
            {
                FileStream FS = new FileStream(path, FileMode.Open);
                BinaryFormatter BF = new BinaryFormatter();
                NeuralNet = (iBackPropagation<T>)BF.Deserialize(FS);
                FS.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    public class cNetwork
    {

        //NeuralNetwork
        public static int SaveNeuralNetwork(string ServerPath, int TrainingSetID, int Layers, double LearningRate, int Iterations, double MaxError, double CurrentErrorValue, int CurrentIterationValue, int input, int hidden, int output)
        {

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_NeuralNetwork_insert");

                sqlCmd.Parameters.Add("@ServerPath", SqlDbType.NVarChar).Value = ServerPath;
                sqlCmd.Parameters.Add("@TrainingSetID", SqlDbType.Int).Value = TrainingSetID;
                sqlCmd.Parameters.Add("@Layers", SqlDbType.Int).Value = Layers;
                sqlCmd.Parameters.Add("@LearningRate", SqlDbType.Decimal).Value = Convert.ToDecimal(LearningRate);
                sqlCmd.Parameters.Add("@Iterations", SqlDbType.Int).Value = Iterations;
                sqlCmd.Parameters.Add("@MaxError", SqlDbType.Decimal).Value = Convert.ToDecimal(MaxError);
                sqlCmd.Parameters.Add("@NetworkError", SqlDbType.Decimal).Value = Convert.ToDecimal(CurrentErrorValue);
                sqlCmd.Parameters.Add("@ExecutedIterations", SqlDbType.Int).Value = CurrentIterationValue;
                sqlCmd.Parameters.Add("@input", SqlDbType.Int).Value = input;
                sqlCmd.Parameters.Add("@hidden", SqlDbType.Int).Value = hidden;
                sqlCmd.Parameters.Add("@output", SqlDbType.Int).Value = output;

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ReturnID(sqlCmd);

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
        public DataTable SelectNeuralNetworkByTrainingSet(int TrainingSetID)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_NeuralNetwork_selectByTrainingSet");
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@TrainingSetID", SqlDbType.Int).Value = TrainingSetID;
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
        public string SelectNeuralNetworkServerPathByTrainingSet(int TrainingSetID)
        {
            string serverPath = null;

            try
            {
                SqlCommand sqlCmd = new SqlCommand("sp_NeuralNetwork_selectServerPathByTrainingSet");
                sqlCmd.Parameters.Add("@TrainingSetID", SqlDbType.Int).Value = TrainingSetID;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    serverPath = (string)objResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return serverPath;

        }


        public int CheckNetworkRatingExists(int userID, int NetworkID)
        {

            int ratingId = 0;

            try
            {

                SqlCommand sqlCmd = new SqlCommand("sp_NetworkRating_checkExists");
                sqlCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                sqlCmd.Parameters.Add("@NetworkID", SqlDbType.Int).Value = NetworkID;
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                object objResult = cDatabase.ExecuteScalar(sqlCmd);

                if ((objResult != null) && (objResult.ToString() != ""))
                {
                    ratingId = (int)objResult;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ratingId;
        }
        //public int UpdateNetworkRating(int RatingId, int Rating)
        //{
        //    try
        //    {
        //        SqlCommand sqlCmd = new SqlCommand("sp_NetworkRating_updateRating");
        //        sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = RatingId;
        //        sqlCmd.Parameters.Add("@Rating", SqlDbType.Int).Value = Rating;
        //        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        object objResult = cDatabase.ExecuteNonQuery(sqlCmd);

        //        if ((objResult != null) && (objResult.ToString() != ""))
        //        {
        //            return (int)objResult;
        //        }
        //        else
        //        {
        //            return -1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static int SaveNetworkRating(int NetworkID, int UserID, int Rating)
        //{
        //    try
        //    {
        //        SqlCommand sqlCmd = new SqlCommand("sp_NetworkRating_insert");

        //        sqlCmd.Parameters.Add("@NetworkID", SqlDbType.Int).Value = NetworkID;
        //        sqlCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
        //        sqlCmd.Parameters.Add("@Rating", SqlDbType.Int).Value = Rating;

        //        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        object objResult = cDatabase.ReturnID(sqlCmd);

        //        if (objResult != null)
        //        {
        //            return (int)objResult;
        //        }
        //        else
        //        {
        //            return -1;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }

}
