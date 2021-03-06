﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProcessEbook
{
    public class ProcessDiv
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


        public ProcessDiv(XmlNode root)
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
                    if (childNode.Attributes == null || childNode.Attributes["rel"] == null)
                        continue;


                    t = int.Parse(childNode.Attributes["rel"].Value);
                    if (t < _sequence)
                        _sequence = t;

                }
            }

            if (_root.Attributes != null && _root.Attributes["rel"] != null)
            {
                int t = int.Parse(_root.Attributes["rel"].Value);
                if (t == _sequence)
                    _sequence++;
            }
            

            List<XmlNode> reserveNodeList = new List<XmlNode>();
            List<XmlNode> removeNodeList1 = new List<XmlNode>();
            foreach (XmlNode childNode in _root.ChildNodes)
            {
                int t = int.MaxValue;

                if (childNode.Name == "br")
                {
                    continue;
                }

                if (childNode.Name == "div" || childNode.Name == "blockquote" || childNode.Name == "h3" || childNode.Name == "li")
                {
                    ProcessDiv pDiv = new ProcessDiv(childNode);
                    pDiv.Sequence = _sequence;
                    pDiv.Process();
                    _sequence = pDiv.Sequence;
                }
                else if (childNode.Name == "p")
                {
                    ProcessP paraph = new ProcessP(childNode);
                    paraph.Sequence = _sequence;
                    paraph.Process();
                    _sequence = paraph.Sequence;
                }
                else if (childNode.Name == "ul")
                {
                    ProcessUL pUL = new ProcessUL(childNode);
                    pUL.Sequence = _sequence;
                    pUL.Process();
                    _sequence = pUL.Sequence;
                }
                else if (childNode.Name == "table")
                {
                    ProcessTable pTb = new ProcessTable(childNode);
                    pTb.Sequence = _sequence;
                    pTb.Process();
                    _sequence = pTb.Sequence;
                }
                else
                {
                    //if (childNode.Name == "a")
                    //    Utility.ProcessA(childNode);
                    if (childNode.Attributes== null || childNode.Attributes["rel"] == null)
                    {
                        List<XmlNode> removeNodeList = new List<XmlNode>();
                        foreach (XmlNode cNode in childNode.ChildNodes)
                        {

                        }
                    }
                    else
                    {
                        t = int.Parse(childNode.Attributes["rel"].Value);
                        if (t == _sequence)
                        {
                            _sequence++;
                        }
                        if (t - _sequence > _root.ChildNodes.Count)
                            removeNodeList1.Add(childNode);

                        List<XmlNode> removeNodeList2 = new List<XmlNode>();
                        foreach (XmlNode cNode in childNode.ChildNodes)
                        {
                            //if (cNode.Name == "a")
                            //    Utility.ProcessA(childNode);
                            if (cNode.Attributes == null || cNode.Attributes["rel"] == null)
                                continue;
                            if (cNode.Name == "a" || cNode.Name == "i")
                            {
                                ProcessDiv pDiv = new ProcessDiv(cNode);
                                pDiv.Sequence = _sequence;
                                pDiv.Process();
                                _sequence = pDiv.Sequence;
                                continue;
                            }

                            t = int.Parse(cNode.Attributes["rel"].Value);
                            if (t == _sequence)
                            {
                                _sequence++;
                            }

                            if (t - _sequence > childNode.ChildNodes.Count)
                                removeNodeList2.Add(cNode);

                            List<XmlNode> removeNodeList3 = new List<XmlNode>();
                            foreach (XmlNode ccNode in cNode.ChildNodes)
                            {
                                //if (ccNode.Name == "a")
                                //    Utility.ProcessA(childNode);
                                if (ccNode.Attributes == null || ccNode.Attributes["rel"] == null)
                                    continue;
                                if (ccNode.Name == "a" || ccNode.Name == "i")
                                {
                                    ProcessDiv pDiv = new ProcessDiv(ccNode);
                                    pDiv.Sequence = _sequence;
                                    pDiv.Process();
                                    _sequence = pDiv.Sequence;
                                    continue;
                                }

                                t = int.Parse(ccNode.Attributes["rel"].Value);
                                if (t == _sequence)
                                {
                                    _sequence++;
                                }

                                if (t - _sequence > cNode.ChildNodes.Count || t < _sequence - 2)
                                    removeNodeList3.Add(ccNode);
                            }
                            foreach (XmlNode rNode in removeNodeList3)
                            {
                                cNode.RemoveChild(rNode);
                            }

                        }

                        foreach (XmlNode rNode in removeNodeList2)
                        {
                            childNode.RemoveChild(rNode);
                        }


                        
                       
                        
                    }



                   
                }

            }

            foreach (XmlNode rNode in removeNodeList1)
            {
                _root.RemoveChild(rNode);
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
