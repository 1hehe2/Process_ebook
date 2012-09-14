using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProcessEbook
{
    public class ProcessUL
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


        public ProcessUL(XmlNode root)
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
                            continue;


                        t = int.Parse(cNode.Attributes["rel"].Value);
                        if (t < _sequence)
                            _sequence = t;
                    }
                    break;
                }
            }

            int tempSequence = _sequence;
            List<XmlNode> reserveNodeList = new List<XmlNode>();
            
            foreach (XmlNode childNode in _root.ChildNodes)
            {
                List<XmlNode> removeNodeList1 = new List<XmlNode>();
                foreach (XmlNode cNode in childNode.ChildNodes)
                {
                    if (cNode.Name == "ul")
                    {
                        ProcessUL pUL = new ProcessUL(cNode);
                        pUL.Sequence = _sequence;
                        pUL.Process();
                        _sequence = pUL.Sequence;
                        continue;
                    }
                    else if (cNode.Name == "h3")
                    {
                        ProcessDiv pDiv = new ProcessDiv(cNode);
                        pDiv.Sequence = _sequence;
                        pDiv.Process();
                        _sequence = pDiv.Sequence;
                        continue;
                    }
                    else if (cNode.Name == "table")
                    {
                        ProcessTable pTb = new ProcessTable(cNode);
                        pTb.Sequence = _sequence;
                        pTb.Process();
                        _sequence = pTb.Sequence;
                        continue;
                    }
                    else if (cNode.Name == "td")
                    {
                        ProcessDiv pDiv = new ProcessDiv(cNode);
                        pDiv.Sequence = _sequence;
                        pDiv.Process();
                        _sequence = pDiv.Sequence;
                        continue;
                    }
                    else if (cNode.Name == "div")
                    {
                        ProcessDiv pDiv = new ProcessDiv(cNode);
                        pDiv.Sequence = _sequence;
                        pDiv.Process();
                        _sequence = pDiv.Sequence;
                        continue;
                    }

                    else if (cNode.Name == "p")
                    {
                        ProcessP proP = new ProcessP(cNode);
                        proP.Sequence = _sequence;
                        proP.Process();
                        _sequence = proP.Sequence;
                        continue;
                    }

                    int t = int.MaxValue;

                    if (cNode.Name == "br")
                    {
                        continue;
                    }

                    


                    if (cNode.Attributes["rel"] != null)
                        t = int.Parse(cNode.Attributes["rel"].Value);



                    if (cNode.Attributes == null || cNode.Attributes["rel"] == null)
                    {


                    }
                    else
                    {
                        t = int.Parse(cNode.Attributes["rel"].Value);
                        if (t == _sequence)
                        {
                            _sequence++;
                        }
                        if (t - _sequence > _root.ChildNodes.Count || t < _sequence - 2)
                            removeNodeList1.Add(cNode);

                        List<XmlNode> removeNodeList2 = new List<XmlNode>();
                        foreach (XmlNode ccNode in cNode.ChildNodes)
                        {
                            if (ccNode.Name == "div" || ccNode.Name == "blockquote")
                            {
                                ProcessDiv pDiv = new ProcessDiv(ccNode);
                                pDiv.Sequence = _sequence;
                                pDiv.Process();
                                _sequence = pDiv.Sequence;
                                continue;
                            }
                            else if (ccNode.Name == "ul")
                            {
                                ProcessUL pUL = new ProcessUL(ccNode);
                                pUL.Sequence = _sequence;
                                pUL.Process();
                                _sequence = pUL.Sequence;
                                continue;
                            }
                            else if (ccNode.Name == "table")
                            {
                                ProcessTable pTb = new ProcessTable(ccNode);
                                pTb.Sequence = _sequence;
                                pTb.Process();
                                _sequence = pTb.Sequence;
                                continue;
                            }
                            else if (ccNode.Name == "p")
                            {
                                ProcessP proP = new ProcessP(ccNode);
                                proP.Sequence = _sequence;
                                proP.Process();
                                _sequence = proP.Sequence;
                                continue;
                            }

                            if (ccNode.Attributes == null || ccNode.Attributes["rel"] == null)
                                continue;

                           

                            t = int.Parse(ccNode.Attributes["rel"].Value);
                            if (t == _sequence)
                            {
                                _sequence++;
                            }

                            if (t - _sequence > cNode.ChildNodes.Count || t < _sequence - 2)
                                removeNodeList2.Add(ccNode);

                            List<XmlNode> removeNodeList3 = new List<XmlNode>();
                            foreach (XmlNode cccNode in ccNode.ChildNodes)
                            {
                                if (cccNode.Name == "div" || cccNode.Name == "blockquote")
                                {
                                    ProcessDiv pDiv = new ProcessDiv(cccNode);
                                    pDiv.Sequence = _sequence;
                                    pDiv.Process();
                                    _sequence = pDiv.Sequence;
                                    continue;
                                }

                                if (cccNode.Attributes == null || cccNode.Attributes["rel"] == null)
                                    continue;

                                t = int.Parse(cccNode.Attributes["rel"].Value);
                                if (t == _sequence)
                                {
                                    _sequence++;
                                }

                                if (t - _sequence > ccNode.ChildNodes.Count || t < _sequence - 2)
                                    removeNodeList3.Add(cccNode);

                                List<XmlNode> removeNodeList4 = new List<XmlNode>();
                                foreach (XmlNode ccccNode in cccNode.ChildNodes)
                                {
                                    if (ccccNode.Name == "div" || ccccNode.Name == "blockquote")
                                    {
                                        ProcessDiv pDiv = new ProcessDiv(ccccNode);
                                        pDiv.Sequence = _sequence;
                                        pDiv.Process();
                                        _sequence = pDiv.Sequence;
                                        continue;
                                    }

                                    if (ccccNode.Attributes == null || ccccNode.Attributes["rel"] == null)
                                        continue;

                                    t = int.Parse(ccccNode.Attributes["rel"].Value);
                                    if (t == _sequence)
                                    {
                                        _sequence++;
                                    }

                                    if (t - _sequence > cccNode.ChildNodes.Count || t < _sequence - 2)
                                        removeNodeList4.Add(ccccNode);

                                    List<XmlNode> removeNodeList5 = new List<XmlNode>();
                                    foreach (XmlNode cccccNode in ccccNode.ChildNodes)
                                    {
                                        if (cccccNode.Name == "div" || cccccNode.Name == "blockquote")
                                        {
                                            ProcessDiv pDiv = new ProcessDiv(cccccNode);
                                            pDiv.Sequence = _sequence;
                                            pDiv.Process();
                                            _sequence = pDiv.Sequence;
                                            continue;
                                        }

                                        if (cccccNode.Attributes == null || cccccNode.Attributes["rel"] == null)
                                            continue;

                                        t = int.Parse(cccccNode.Attributes["rel"].Value);
                                        if (t == _sequence)
                                        {
                                            _sequence++;
                                        }

                                        if (t - _sequence > ccccNode.ChildNodes.Count || t < _sequence - 2)
                                            removeNodeList5.Add(cccccNode);
                                    }

                                    foreach (XmlNode rNode in removeNodeList5)
                                    {
                                        ccccNode.RemoveChild(rNode);
                                    }

                                }

                                foreach (XmlNode rNode in removeNodeList4)
                                {
                                    cccNode.RemoveChild(rNode);
                                }

                            }
                            foreach (XmlNode rNode in removeNodeList3)
                            {
                                ccNode.RemoveChild(rNode);
                            }

                        }

                        foreach (XmlNode rNode in removeNodeList2)
                        {
                            cNode.RemoveChild(rNode);
                        }
                    }


                }

                foreach (XmlNode rNode in removeNodeList1)
                {
                    childNode.RemoveChild(rNode);
                }
            }

           


            //Format node
            //foreach (XmlNode childNode in _root.ChildNodes)
            //{
            //    Utility.FormatStringNode(childNode);
            //    foreach (XmlNode cNode in childNode.ChildNodes)
            //    {
            //        Utility.FormatStringNode(cNode);
            //        foreach (XmlNode ccNode in cNode.ChildNodes)
            //        {
            //            Utility.FormatStringNode(ccNode);
            //            foreach (XmlNode cccNode in ccNode.ChildNodes)
            //            {
            //                Utility.FormatStringNode(cccNode);
            //            }
            //        }
            //    }
            //}
        }

    }
}
