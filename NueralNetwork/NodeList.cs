using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PMML
{
    class NodeList
    {
        public XmlNodeList xmlNodeList;

        public NodeList()
        {

        }

        public int Size()
        {
            return xmlNodeList.Count;
        }

        public Node getNode(int i)
        {
            Node node = new Node();
            XmlNode xNode = xmlNodeList[i];

            node.xmlNode = xNode;

            return node;
        }
    }
}
