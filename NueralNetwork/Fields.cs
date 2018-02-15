using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PMML;

namespace NueralNetwork
{
    // This class represents the input values, transformations and derived fields. 
     
    class Fields
    {   
        public double[] netInputs;

        string targetName;
        NodeList MiningField;    //this list consists of MiningField elements
        NodeList DerivedField;   //this list consists of DerivedField elements
        List<double> derFieldsList = new List<double>();  //this list consists derived fields, the transformed and prepared
        Pmml xmlDoc;
        public List<String> FieldName = new List<string>(); //A list Name of Fields
        public int ar = 0; //Number Of AR Fields
        
        //this class gets the XML Document file and a Dictionary of input values with Field name
        //the data preparations are done

        public Fields(Pmml doc)
        {
            xmlDoc = doc;
            MiningField = doc.getList("MiningField");

            //to get the target field name
            for (int i = 0; i < MiningField.Size(); i++)
            {
                if (MiningField.getNode(i).Attribute("usageType") != null && MiningField.getNode(i).Attribute("usageType") == "predicted")
                {
                    targetName = MiningField.getNode(i).Attribute("name");
                }
                else
                {
                    FieldName.Add(MiningField.getNode(i).Attribute("name"));
                }
            }

            for (int i = 0; i < FieldName.Count; i++)
            {
                for (int j = 1; j <= FieldName.Count; j++)
                {
                    if (FieldName[i].Equals("AR" + j))
                    {
                        ar = ar + 1;
                        break;
                    }
                }
            }
        }

        public Fields(Dictionary<string, object> inputDic, Pmml doc)
        {
            xmlDoc = doc;
            MiningField = doc.getList("MiningField");

            //to get the target field name
            for (int i = 0; i < MiningField.Size(); i++)
            {
                if (MiningField.getNode(i).Attribute("usageType") != null && MiningField.getNode(i).Attribute("usageType") == "predicted")
                {
                    targetName = MiningField.getNode(i).Attribute("name");
                }
                else
                {
                    FieldName.Add(MiningField.getNode(i).Attribute("name"));
                }
            }

            for (int i = 0; i < FieldName.Count; i++)
            {
                for (int j = 1; j <= FieldName.Count; j++)
                {
                    if (FieldName[i].Equals("AR" + j))
                    {
                        ar = ar + 1;
                        break;
                    }
                }
            }

            DerivedField = doc.getList("DerivedField");

            //to add transformed values to list
            for (int i = 0; i < DerivedField.Size() / 2; i++)
            {
                double value = 0; 
                if (DerivedField.getNode(i).Child(0).Attribute("field") != targetName)
                {
                    object inp = inputDic[DerivedField.getNode(i).Child(0).Attribute("field")];
                    switch (DerivedField.getNode(i).Child(0).Name())
                    {
                        case "NormDiscrete": value = NormDiscrete(Convert.ToString(inp), DerivedField.getNode(i));
                            break;
                        case "NormContinuous": value = LinearNorm(Convert.ToDouble(inp), DerivedField.getNode(i));
                            break;
                    }
                    derFieldsList.Add(value);
                }
            }

            netInputs = derFieldsList.ToArray();
        }

        //returns an array of target field values
        public string[] GetTarget()
        {
            string[] targetValues = null;
            List<string> trgtVlu = new List<string>();
            NodeList dataField = xmlDoc.getList("DataField");

            for (int i = 0; i < dataField.Size(); i++)
            {
                if (dataField.getNode(i).Attribute("name") == targetName)
                {
                    for (int j = 0; j < dataField.getNode(i).ChildCount("Value"); j++)
                    {
                        trgtVlu.Add(dataField.getNode(i).Child(j, "Value").Attribute("value"));
                     }
                }
            }

            targetValues = trgtVlu.ToArray();
            return targetValues;
        }


        public double DeLinearNorm(double value, string FieldRef)
        {
            Node field = null;
            for (int i = 0; i < DerivedField.Size(); i++)
            {
                if (DerivedField.getNode(i).Attribute("name").Equals(FieldRef))
                {
                    field = DerivedField.getNode(i);
                    break;
                }
            }

            Node NormContinuous = field.Child(0);

            double norm0 = double.Parse(NormContinuous.Child(0, "LinearNorm").Attribute("norm"));
            double norm1 = double.Parse(NormContinuous.Child(1, "LinearNorm").Attribute("norm"));
            double orig0 = double.Parse(NormContinuous.Child(0, "LinearNorm").Attribute("orig"));
            double orig1 = double.Parse(NormContinuous.Child(1, "LinearNorm").Attribute("orig"));
            double deNormValue = orig0 + (value - norm0) * (orig1 - orig0) / (norm1 - norm0);

            return deNormValue;
        }
     
        
        //this function does the linear Noramalization of double input values
        //this function gets a value to normalizan and a XML node that has the normalization values
        private double LinearNorm(double value, Node field)
        {
            Node NormContinuous = field.Child(0);

            double norm0 = double.Parse(NormContinuous.Child(0, "LinearNorm").Attribute("norm"));
            double norm1 = double.Parse(NormContinuous.Child(1, "LinearNorm").Attribute("norm"));
            double orig0 = double.Parse(NormContinuous.Child(0, "LinearNorm").Attribute("orig"));
            double orig1 = double.Parse(NormContinuous.Child(1, "LinearNorm").Attribute("orig"));

            double normValue = norm0 + (value - orig0) / (orig1 - orig0) * (norm1 - norm0);

            return normValue;
        }


        //this function transforms Categorical inputs to double values.
        ////this function gets a value to transform and a XML node that has the transformation values
        private double NormDiscrete(string value, Node field)
    {
        Node NormDis = field.Child(0);
        double r = 0;
        if (NormDis.Attribute("value").Equals(value))
        {
            r = 1;
        }
        return r;
    }
    }
}
