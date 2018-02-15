using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PMML;

namespace NueralNetwork
{
    //this class defines one layer of the network. its weight values and Activation Function and Normalization Function
    class Layer
    {
        public double[,] weights;   //weights of neurons from previous layer
        public double[] output; //the output value of the layer
        public string defFunc; //default activation function
        Node layer;

        public Layer(Node netlayer)
        {
            layer = netlayer;
            weights = getWeights();
            output = new double[weights.GetLength(0)];

    }
        public string ActFunction()
        {
            string actFunc = null;
            if (layer.Attribute("activationFunction") == null)
            {
                actFunc = defFunc;
            }
            else
            {
                actFunc = layer.Attribute("activationFunction");
            }


            return actFunc;
        }
        public string NormFunction()
        {
            string normFunc = null;

            if (layer.Attribute("normalizationMethod") != null)
            {
                normFunc = layer.Attribute("normalizationMethod");
            }

            return normFunc;
          }


        //This Method gets a XML Node List of neurons of one layer, in result it returns an array of weights from previous layer nodes. the last element of 
        //each array is the bias
        private double[,] getWeights()
        {
            int m = layer.ChildCount("Neuron");
            int n = layer.Child(0, "Neuron").ChildCount("Con");
            double[,] weights = new double[m, n + 1];

            for (int i = 0; i < m; i++)
            {
                weights[i, n] = double.Parse(layer.Child(i, "Neuron").Attribute("bias"));
                for (int j = 0; j < n; j++)
                {
                    weights[i, j] = double.Parse(layer.Child(i, "Neuron").Child(j, "Con").Attribute("weight"));
                }
            }
            return weights;
        }

    }
}
