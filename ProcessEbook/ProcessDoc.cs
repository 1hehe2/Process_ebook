using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProcessEbook
{
    public class ProcessDoc
    {
        XmlDocument _doc;
        public XmlDocument Doc
        {
            get { return _doc; }
            set { _doc = value; }
        }

        int _sequence = int.MaxValue;

        public ProcessDoc(XmlDocument doc)
        {
            _doc = doc;
        }

        public void Process()
        {
            XmlNode root = _doc.FirstChild;
            List<XmlNode> removeNodeList = new List<XmlNode>();
            foreach (XmlNode node in root.ChildNodes)
            {
                
                if (node.Name == "span")
                {
                    removeNodeList.Add(node);
                    continue;
                }

                if (node.Name == "h1" || node.Name == "h2" || node.Name == "h3" || node.Name == "h4" || node.Name == "h5" || node.Name == "h6")
                {
                    ProcessP paraph = new ProcessP(node);
                    paraph.Sequence = _sequence;
                    paraph.Process();
                    _sequence = paraph.Sequence;
                }
                else if (node.Name == "p")
                {
                    ProcessP paraph = new ProcessP(node);
                    paraph.Sequence = _sequence;
                    paraph.Process();
                    _sequence = paraph.Sequence;
                }
                else if (node.Name == "ul")
                {
                    ProcessUL pUL = new ProcessUL(node);
                    pUL.Sequence = _sequence;
                    pUL.Process();
                    _sequence = pUL.Sequence;
                }
                else if (node.Name == "table")
                {
                    ProcessTable pTb = new ProcessTable(node);
                    pTb.Sequence = _sequence;
                    pTb.Process();
                    _sequence = pTb.Sequence;
                }
                else if (node.Name == "div" || node.Name == "blockquote")
                {
                    ProcessDiv pDiv = new ProcessDiv(node);
                    pDiv.Sequence = _sequence;
                    pDiv.Process();
                    _sequence = pDiv.Sequence;
                }
                else
                {
                    ProcessDiv pDiv = new ProcessDiv(node);
                    pDiv.Sequence = _sequence;
                    pDiv.Process();
                    _sequence = pDiv.Sequence;
                }
            }

            foreach (XmlNode rNode in removeNodeList)
            {
                root.RemoveChild(rNode);
            }

            //Format
            foreach (XmlNode node in root.ChildNodes)
            {
                Utility.FormatStringNode(node);
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    Utility.FormatStringNode(childNode);
                    foreach (XmlNode cNode in childNode.ChildNodes)
                    {
                        Utility.FormatStringNode(cNode);
                        foreach (XmlNode ccNode in cNode.ChildNodes)
                        {
                            Utility.FormatStringNode(ccNode);
                            foreach (XmlNode cccNode in ccNode.ChildNodes)
                            {
                                Utility.FormatStringNode(cccNode);
                                foreach (XmlNode ccccNode in cccNode.ChildNodes)
                                {
                                    Utility.FormatStringNode(ccccNode);
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
