using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCheckLib
{
    public interface iBackPropagation<T>
    {
        void BackPropagate();
        double F(double x);
        void ForwardPropagate(double[] pattern, T output);
        double GetError();
        void InitializeNetwork(Dictionary<T, double[]> TrainingSet);
        void Recognize(double[] Input, ref T MatchedHigh, ref double OutputValueHigh,
                                        ref T MatchedLow, ref double OutputValueLow);

    }
}
