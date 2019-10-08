using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCheckLib
{
    [Serializable]
    public class cBP2Layer<T> : iBackPropagation<T> where T : IComparable<T>
    {
        private int PreInputNum;
        private int InputNum;
        private int OutputNum;

        private PreInput[] PreInputLayer;
        private Input[] InputLayer;
        private Output<T>[] OutputLayer;

        private double learningRate = 0.4;

        public cBP2Layer(int preInputNum, int inputNum, int outputNum, double lr)
        {
            PreInputNum = preInputNum;
            InputNum = inputNum;
            OutputNum = outputNum;
            learningRate = lr;

            PreInputLayer = new PreInput[PreInputNum];
            InputLayer = new Input[InputNum];
            OutputLayer = new Output<T>[OutputNum];
        }

        #region iBackPropagation<T> Members
        public void BackPropagate()
        {
            int i, j;
            double total;
            try
            {
                //Fix Input Layer's Error
                for (i = 0; i < InputNum; i++)
                {
                    total = 0.0;
                    for (j = 0; j < OutputNum; j++)
                    {
                        total += InputLayer[i].Weights[j] * OutputLayer[j].Error;
                    }
                    InputLayer[i].Error = total;
                }

                //Update The First Layer's Weights
                for (i = 0; i < InputNum; i++)
                {
                    for (j = 0; j < PreInputNum; j++)
                    {
                        PreInputLayer[j].Weights[i] +=
                            learningRate * InputLayer[i].Error * PreInputLayer[j].Value;
                    }
                }

                //Update The Second Layer's Weights
                for (i = 0; i < OutputNum; i++)
                {
                    for (j = 0; j < InputNum; j++)
                    {
                        InputLayer[j].Weights[i] +=
                            learningRate * OutputLayer[i].Error * InputLayer[j].Output;
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
                //return Math.Exp(-x * x); 
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

                //Calculate The First(Input) Layer's Inputs and Outputs
                for (i = 0; i < InputNum; i++)
                {
                    total = 0.0;
                    for (j = 0; j < PreInputNum; j++)
                    {
                        total += PreInputLayer[j].Value * PreInputLayer[j].Weights[i];
                    }

                    InputLayer[i].InputSum = total;
                    InputLayer[i].Output = F(total);
                }

                //Calculate The Second(Output) Layer's Inputs, Outputs, Targets and Errors
                for (i = 0; i < OutputNum; i++)
                {
                    total = 0.0;
                    for (j = 0; j < InputNum; j++)
                    {
                        total += InputLayer[j].Output * InputLayer[j].Weights[i];
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
            int i, j;
            Random rand = new Random();
            try
            {

                for (i = 0; i < PreInputNum; i++)
                {
                    PreInputLayer[i].Weights = new double[InputNum];
                    for (j = 0; j < InputNum; j++)
                    {
                        PreInputLayer[i].Weights[j] = 0.01 + ((double)rand.Next(0, 5) / 100);
                    }
                }

                for (i = 0; i < InputNum; i++)
                {
                    InputLayer[i].Weights = new double[OutputNum];
                    for (j = 0; j < OutputNum; j++)
                    {
                        InputLayer[i].Weights[j] = 0.01 + ((double)rand.Next(0, 5) / 100);
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

        public void Recognize(double[] Input, ref T MatchedHigh, ref double OutputValueHigh, ref T MatchedLow, ref double OutputValueLow)
        {

            int i, j;
            double total = 0.0;
            double max = -1;
            try
            {
                //Apply input to the network
                for (i = 0; i < PreInputNum; i++)
                {
                    PreInputLayer[i].Value = Input[i];
                }

                //Calculate Input Layer's Inputs and Outputs
                for (i = 0; i < InputNum; i++)
                {
                    total = 0.0;
                    for (j = 0; j < PreInputNum; j++)
                    {
                        total += PreInputLayer[j].Value * PreInputLayer[j].Weights[i];
                    }
                    InputLayer[i].InputSum = total;
                    InputLayer[i].Output = F(total);
                }

                //Find the [Two] Highest Outputs   
                for (i = 0; i < OutputNum; i++)
                {
                    total = 0.0;
                    for (j = 0; j < InputNum; j++)
                    {
                        total += InputLayer[j].Output * InputLayer[j].Weights[i];
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
