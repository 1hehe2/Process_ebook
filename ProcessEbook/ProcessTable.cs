using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProcessEbook
{
    public class ProcessTable
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


        public ProcessTable(XmlNode root)
        {
            _root = root;

        }

        public void Process()
        {

            if (_sequence == int.MaxValue)
            {
                int t = int.MaxValue;
                foreach (XmlNode childNode in _root.ChildNodes)
                {
                    foreach (XmlNode cNode in childNode.ChildNodes)
                    {
                        if (cNode.Attributes == null || cNode.Attributes["rel"] == null)
                        {
                            foreach (XmlNode ccNode in cNode.ChildNodes)
                            {
                                if (ccNode.Attributes == null || ccNode.Attributes["rel"] == null)
                                    continue;


                                t = int.Parse(ccNode.Attributes["rel"].Value);
                                if (t < _sequence)
                                    _sequence = t;
                            }
                        }

                        if (cNode.Attributes["rel"] != null)
                        {
                            t = int.Parse(cNode.Attributes["rel"].Value);
                            if (t < _sequence)
                                _sequence = t;
                        }
                    }
                    break;
                }
            }


            int tempSequence = _sequence;
            List<XmlNode> reserveNodeList = new List<XmlNode>();

            foreach (XmlNode childNode in _root.ChildNodes)
            {
               
                ProcessUL pUL = new ProcessUL(childNode);
                pUL.Sequence = _sequence;
                pUL.Process();
                _sequence = pUL.Sequence;
               
            }

        }

    }
}
