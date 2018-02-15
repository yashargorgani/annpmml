using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PMML
{
    class Node
    {
        public XmlNode xmlNode;
        
        public Node()
        {

        }

        public string Name()
        {
            return xmlNode.Name;
        }

        public string Attribute(string attName)
        {
            if (xmlNode.Attributes[attName] != null)
            {
                return xmlNode.Attributes[attName].Value;
            }
            else
                return null;
        }


        public Node Child(int k, string name)
        {
            int j = 0;
            Node child = new Node();
            XmlNodeList children = xmlNode.ChildNodes;
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Name == name)
                {
                    child.xmlNode = children[i];

                    if (j == k)
                    {
                        break;
                    }
                        j++;
                }
            }

            return child;
        }

        public Node Child(int k)
        {
            Node child = new Node();
            child.xmlNode = xmlNode.ChildNodes[k];

            return child;
        }

        public int ChildCount(string name)
        {
            int j = 0;
            Node child = new Node();
            XmlNodeList children = xmlNode.ChildNodes;
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Name == name)
                {
                    j++;
                }
            }
            
            return j;
        }

        public int ChildCount()
        {
            return xmlNode.ChildNodes.Count;
        }
        public string Value()
        {
            return xmlNode.InnerText;
        }
    }
}
