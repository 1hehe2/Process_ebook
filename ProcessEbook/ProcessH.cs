using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ProcessEbook
{
    public class ProcessH
    {
        XmlNode _root;

        public XmlNode Root
        {
            get { return _root; }
            set { _root = value; }
        }


        int _sequence = int.MaxValue;
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }


        public ProcessH(XmlNode root)
        {
            _root = root;
            
        }

        public void Process()
        {
            List<XmlNode> reserveNodeList = new List<XmlNode>();
            foreach (XmlNode childNode in _root.ChildNodes)
            {
                int t = int.MaxValue;

                if (childNode.Name == "br")
                {
                    continue;
                }

                if (childNode.Attributes["rel"] != null)
                    t = int.Parse(childNode.Attributes["rel"].Value);
                if (t < _sequence)
                    _sequence = t;

                if (childNode.Name == "span" || childNode.Name == "img")
                    Utility.ProcessParaph(_root.OwnerDocument, childNode, reserveNodeList, ref _sequence, ref t);
                else if (childNode.Name == "sup" || childNode.Name == "i")
                {
                    Utility.ProcessSubAndItalic(_root.OwnerDocument, childNode, reserveNodeList, ref _sequence);
                }
                else
                    Utility.ProcessParaph(_root.OwnerDocument, childNode, reserveNodeList, ref _sequence, ref t);

            }

            for (int i = _root.ChildNodes.Count - 1; i >= 0; i--)
                _root.RemoveChild(_root.ChildNodes[i]);


            for (int i = 0; i < reserveNodeList.Count; i++)
            {
                foreach (XmlNode rNode in reserveNodeList)
                {

                    if (rNode.Attributes["rel"] == null)
                    {
                        continue;
                    }

                    if (int.Parse(rNode.Attributes["rel"].Value) == _sequence)
                    {
                        if (rNode.ChildNodes.Count > 0)
                            if (rNode.Name == "sup" || rNode.Name == "i" || rNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                            {
                                if (rNode.Name == "sup" || rNode.Name == "i")
                                {
                                    foreach (XmlNode ttNode in rNode.ChildNodes)
                                    {
                                        if (ttNode.Attributes["rel"] != null && int.Parse(ttNode.Attributes["rel"].Value) == _sequence + 1)
                                        {
                                            if (ttNode.ChildNodes.Count > 0)
                                            {
                                                string temp = ttNode.InnerText;
                                                temp = temp.Trim();
                                                if (temp.Length > 1)
                                                    temp = temp + " ";
                                                ttNode.InnerText = temp;
                                            }
                                            _sequence++;
                                        }
                                    }
                                }
                                if (rNode.ChildNodes.Count > 0)
                                {
                                    string temp = rNode.ChildNodes[0].InnerText;
                                    temp = temp.Trim();
                                    if (temp.Length > 1)
                                        temp = temp + " ";
                                    rNode.ChildNodes[0].InnerText = temp;

                                    //process cho ,. ?
                                    if (temp.Length > 1 && (temp[0] == ',' || temp[0] == '.' || temp[0] == '?'))
                                        _root.LastChild.InnerText = _root.LastChild.InnerText.TrimEnd();

                                }

                                _root.AppendChild(rNode);
                            }

                        _sequence++;
                    }
                }
            }
        }

        
    }
}
