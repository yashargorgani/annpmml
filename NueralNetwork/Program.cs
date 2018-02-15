using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PMML;

namespace NueralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {

            NNModel model = new NNModel("E:/IT/PMML/Neural Networks/Model Files/Drug.xml");

            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("Age", 23);
            input.Add("Sex", "F");
            input.Add("BP", "HIGH");
            input.Add("Cholesterol", "HIGH");
            input.Add("Na", 0.792535);
            input.Add("K", 0.031258);

            Console.WriteLine(model.Predict(input));
            Console.ReadKey();
        }
    }
}
