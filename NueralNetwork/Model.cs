using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PMML;

namespace NueralNetwork
{
    class NNModel
    {
        Pmml doc;
        public Fields fields;
        double[] netInput;
        //This list will contain Layers
        List<Layer> layers = new List<Layer>();
        double[] result;
        //list of NeuralNetwork
        NodeList nNet;
        //default Activation Function
        public string defualtFunc = null;

        //the constructor of Model gets the derivedField from XML file, the default Activation Function
        //and inserts the layers of network to a dynamic list
        public NNModel(string dir)
        {
            doc = new Pmml(dir);
            fields = new Fields(doc);

            nNet = doc.getList("NeuralNetwork");
            defualtFunc = nNet.getNode(0).Attribute("activationFunction");

            for (int i = 0; i < doc.getList("NeuralLayer").Size(); i++)
            {
                Layer layer = new Layer(doc.getList("NeuralLayer").getNode(i));
                layer.defFunc = defualtFunc;
                layers.Add(layer);
            }
        }
        
        //this function adds the raw input values to a dictionary, this dictionary is the input of Fields class constructor
        //and gets the derived fields from Fields object

        //this function gets the Activation Function name and returns the proper value
        private double Activation(double z, string FuncName)
        {
            double actZ = 0;
            switch (FuncName)
            {
                case "tanh": actZ = Math.Tanh(z);
                    break;
                case "identity": actZ = z;
                    break;
            }
            return actZ;
        }

        //this method returns normalization value of  output layer
        private double[] Normalize(double[] nOutput, string normName)
        {
            double[] normValue = new double[nOutput.GetLength(0)];

            switch (normName)
            {
                case "softmax":
                    {
                        double sumValue = 0;
                        for (int i = 0; i < nOutput.GetLength(0); i++)
                        {
                            sumValue = sumValue + Math.Exp(nOutput[i]);
                        }
                        for (int j = 0; j < nOutput.GetLength(0); j++)
                        {
                            normValue[j] = Math.Exp(nOutput[j]) / sumValue;
                        }
                    }
                    break;

                case "simplemax":
                    {
                        double sumValue = 0;
                        for (int i = 0; i < nOutput.GetLength(0); i++)
                        {
                            sumValue = sumValue + nOutput[i];
                        }
                        for (int k = 0; k < nOutput.GetLength(0); k++)
                        {
                            normValue[k] = nOutput[k] / sumValue;
                        }
                    }
                    break;
            }

            return normValue;
        }

        //this method computes the neuron output of ONE neuron
        private double ComputeOutput(double[] weight, double[] nOutput, string funcName)
        {
            double z = 0;
            for (int i = 0; i < nOutput.Length; i++)
            {
                z = z + weight[i] * nOutput[i];
            }
            z = z + weight[weight.Length - 1];
            z = Activation(z, funcName);
            return z;
        }


        //the first step gets the outputs of first layer, this step needs Network input values.
        //for the other layers, gets the output values of previous layer and computes the outputs of current layer.

        public double Regression(Dictionary<string, object> input)
        {
            NodeList outLayer = doc.getList("NeuralOutput");
            Node FieldRef = outLayer.getNode(0).Child(0, "DerivedField").Child(0, "FieldRef");
            string trgtFieldRef = FieldRef.Attribute("field");

            fields = new Fields(input, doc);
            netInput = fields.netInputs;
            double prediction = 0;


            for (int l = 0; l < layers[0].weights.GetLength(0); l++)
            {
                double[] w = new double[layers[0].weights.GetLength(1)];

                for (int i = 0; i < layers[0].weights.GetLength(1); i++)
                {
                    w[i] = layers[0].weights[l, i];
                }
                layers[0].output[l] = ComputeOutput(w, netInput, layers[0].ActFunction());
            }

            for (int y = 1; y < layers.Count; y++)
            {
                for (int m = 0; m < layers[y].weights.GetLength(0); m++)
                {
                    double[] w = new double[layers[y].weights.GetLength(1)];

                    for (int j = 0; j < layers[y].weights.GetLength(1); j++)
                    {
                        w[j] = layers[y].weights[m, j];
                    }

                    layers[y].output[m] = ComputeOutput(w, layers[y - 1].output, layers[y].ActFunction());
                }

                result = layers[y].output;
            }

            prediction = fields.DeLinearNorm(result[0], trgtFieldRef);
            return prediction;
        }
        

        public string Predict(Dictionary<string, object> input)
        {
            fields = new Fields(input, doc);
            netInput = fields.netInputs;
            string[] targetV = fields.GetTarget();
            string prediction = null;

            
            for (int l = 0; l < layers[0].weights.GetLength(0); l++)
            {
                double[] w = new double[layers[0].weights.GetLength(1)];

                for (int i = 0; i < layers[0].weights.GetLength(1); i++)
                {
                    w[i] = layers[0].weights[l, i];
                }
                    layers[0].output[l] = ComputeOutput(w, netInput, layers[0].ActFunction());
                }

            for (int y = 1; y < layers.Count; y++)
            {
                for (int m = 0; m < layers[y].weights.GetLength(0); m++)
                {
                    double[] w = new double[layers[y].weights.GetLength(1)];

                    for (int j = 0; j < layers[y].weights.GetLength(1); j++)
                    {
                        w[j] = layers[y].weights[m, j];
                    }

                    layers[y].output[m] = ComputeOutput(w, layers[y-1].output, layers[y].ActFunction());

            }
                                
                    result = Normalize(layers[y].output, layers[y].NormFunction());
            }

            

            for (int r = 0; r < result.GetLength(0); r++)
            {
                Console.WriteLine(result[r]);
                if (result[r] == result.Max())
                {
                    prediction = targetV[r];
                }
            }

            return prediction;
       }

    }
}
