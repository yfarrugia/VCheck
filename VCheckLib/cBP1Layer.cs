﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCheckLib
{
    [Serializable]
    public class cBP1Layer<T> : iBackPropagation<T> where T : IComparable<T>
    {
        private int PreInputNum;
        private int OutputNum;

        private PreInput[] PreInputLayer;
        private Output<T>[] OutputLayer;

        private double learningRate = 0.4;

        public cBP1Layer(int preInputNum, int outputNum, double lr)
        {
            PreInputNum = preInputNum;
            OutputNum = outputNum;
            learningRate = lr;

            PreInputLayer = new PreInput[PreInputNum];
            OutputLayer = new Output<T>[OutputNum];
        }

        #region iBackPropagation<T> Members

        public void BackPropagate()
        {
            try
            {
                //Update The First Layer's Weights
                for (int j = 0; j < OutputNum; j++)
                {
                    for (int i = 0; i < PreInputNum; i++)
                    {
                        PreInputLayer[i].Weights[j] += LearningRate * (OutputLayer[j].Error) * PreInputLayer[i].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double F(double x)
        {
            try
            {

                return (1 / (1 + Math.Exp(-x)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ForwardPropagate(double[] pattern, T output)
        {
            int i, j;
            double total = 0.0;
            try
            {
                //Apply input to the network
                for (i = 0; i < PreInputNum; i++)
                {
                    PreInputLayer[i].Value = pattern[i];
                }

                //Calculate The First(Output) Layer's Inputs, Outputs, Targets and Errors
                for (i = 0; i < OutputNum; i++)
                {
                    total = 0.0;
                    for (j = 0; j < PreInputNum; j++)
                    {
                        total += PreInputLayer[j].Value * PreInputLayer[j].Weights[i];
                    }
                    OutputLayer[i].InputSum = total;
                    OutputLayer[i].output = F(total);
                    OutputLayer[i].Target = OutputLayer[i].Value.CompareTo(output) == 0 ? 1.0 : 0.0;
                    OutputLayer[i].Error = (OutputLayer[i].Target - OutputLayer[i].output) * (OutputLayer[i].output) * (1 - OutputLayer[i].output);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double GetError()
        {
            double total = 0.0;
            try
            {
                for (int j = 0; j < OutputNum; j++)
                {
                    total += Math.Pow((OutputLayer[j].Target - OutputLayer[j].output), 2) / 2;
                }
                return total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InitializeNetwork(Dictionary<T, double[]> TrainingSet)
        {
            Random rand = new Random();
            try
            {
                for (int i = 0; i < PreInputNum; i++)
                {
                    PreInputLayer[i].Weights = new double[OutputNum];
                    for (int j = 0; j < OutputNum; j++)
                    {
                        PreInputLayer[i].Weights[j] = 0.01 + ((double)rand.Next(0, 2) / 100);
                    }
                }

                int k = 0;
                foreach (KeyValuePair<T, double[]> p in TrainingSet)
                {
                    OutputLayer[k++].Value = p.Key;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Recognize(double[] Input, ref T MatchedHigh, ref double OutputValueHigh,
            ref T MatchedLow, ref double OutputValueLow)
        {
            int i, j;
            double total = 0.0;
            double max = -1;
            try
            {
                //Apply Input to Network
                for (i = 0; i < PreInputNum; i++)
                {
                    PreInputLayer[i].Value = Input[i];
                }

                //Find the [Two] Highest Outputs
                for (i = 0; i < OutputNum; i++)
                {
                    total = 0.0;
                    for (j = 0; j < PreInputNum; j++)
                    {
                        total += PreInputLayer[j].Value * PreInputLayer[j].Weights[i];
                    }
                    OutputLayer[i].InputSum = total;
                    OutputLayer[i].output = F(total);
                    if (OutputLayer[i].output > max)
                    {
                        MatchedLow = MatchedHigh;
                        OutputValueLow = max;
                        max = OutputLayer[i].output;
                        MatchedHigh = OutputLayer[i].Value;
                        OutputValueHigh = max;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public double LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }

    }

}
