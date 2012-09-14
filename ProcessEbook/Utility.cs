using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;

namespace ProcessEbook
{
    public class Utility
    {
       
        public static void FormatStringNode(XmlNode node)
        {
            
            if (node.NodeType == XmlNodeType.Text)
                return;
            if ((node.ParentNode.Name == "a" || node.ParentNode.Name == "i" || 
                node.ParentNode.Name == "b" || node.ParentNode.Name == "em" ||
                node.ParentNode.Name == "strong" || node.ParentNode.Name == "var") && node.ParentNode.LastChild == node)
            {
                if (node.ParentNode.Attributes["dvc"] != null)
                    return;
                XmlAttribute att = node.OwnerDocument.CreateAttribute("dvc");
                att.Value = "1";
                node.ParentNode.Attributes.Append(att);
                //XmlNode childNode = node.LastChild;
                //if (childNode.NodeType == XmlNodeType.Text)
                //{
                    //childNode.InnerText = childNode.InnerText.Trim();
                //}

                XmlNode nodeTemp;
                XmlText text;

                nodeTemp = node.OwnerDocument.CreateElement("span");
                text = node.OwnerDocument.CreateTextNode(" ");
                nodeTemp.AppendChild(text);
                node.ParentNode.ParentNode.InsertAfter(nodeTemp, node.ParentNode);
               
                if (node.ParentNode.Name == "a")
                {
                    if (node.InnerText.Length > 3)
                    {
                        nodeTemp = node.OwnerDocument.CreateElement("span");
                        text = node.OwnerDocument.CreateTextNode(" ");
                        nodeTemp.AppendChild(text);
                        node.ParentNode.ParentNode.InsertBefore(nodeTemp, node.ParentNode);
                    }
                }
                else
                //if (node.ParentNode.Name == "i" || node.ParentNode.Name == "b")
                {
                   
                    XmlNode prevNode = node.ParentNode.PreviousSibling;
                    
                    string tempText = string.Empty;
                    if (prevNode != null)
                    {
                        XmlNode tNode = prevNode.LastChild;
                        if (tNode != null && tNode.NodeType == XmlNodeType.Text)
                        {
                            tempText = tNode.InnerText;
                            if (tempText.Length == 1)
                                tNode.InnerText = tNode.InnerText.TrimEnd();
                        }
                        else if (tNode != null)
                        {
                            XmlNode ttNode = tNode.LastChild;
                            if (ttNode != null && ttNode.NodeType == XmlNodeType.Text)
                            {
                                tempText = ttNode.InnerText;
                                if (tempText.Length == 1)
                                    ttNode.InnerText = ttNode.InnerText.TrimEnd();
                            }
                            else if (ttNode != null)
                            {
                                XmlNode tttNode = ttNode.LastChild;

                                if (tttNode != null && tttNode.NodeType == XmlNodeType.Text)
                                {
                                    tempText = tttNode.InnerText;
                                    if (tempText.Length == 1)
                                        tttNode.InnerText = tttNode.InnerText.TrimEnd();
                                }
                            }
                        }
                    }

                    if (tempText.Length == 0)
                    {
                        //MessageBox.Show("");
                        return;
                    }
                    
                    if (tempText.Length == 1 || tempText[tempText.Length - 2] == ' ' || tempText[tempText.Length - 1] == ' ' || tempText[tempText.Length - 1] == '\"')
                        return;
                    nodeTemp = node.OwnerDocument.CreateElement("span");
                    text = node.OwnerDocument.CreateTextNode(" ");
                    nodeTemp.AppendChild(text);
                    node.ParentNode.ParentNode.InsertBefore(nodeTemp, node.ParentNode);
                }

                

                return;
            }
            if (node.ChildNodes.Count > 0)
            {
                XmlNode childNode = node.LastChild;
                if (childNode == null)
                    return;
                if (node.Name == "sup" || childNode.NodeType == XmlNodeType.Text)
                {
                    string str = childNode.InnerText;

                    int length = str.Length;

                    //string str = childNode.InnerText.Trim();

                    //int length = str.Length;
                    //if (length > 1)
                    //{
                        

                    //    if (str[length - 2] != ' ' && str[length - 2] != '\"' && str[length - 2] != '\'')
                    //    {
                    //        if (str.Substring(length - 2) != "“i")
                    //            childNode.InnerText = str + " ";
                    //    }
                        
                    //    if(str[length - 2] == ' ')
                    //        if (str[length - 1] == 'y' || str[length - 1] == 'u' || str[length - 1] == 'o' || str[length - 1] == 'ơ' || str[length - 1] == 'ở' || str[length - 1] == 'ô')
                    //            childNode.InnerText = str + " ";
                    //}
                    //else if(length == 1)
                    //{
                       
                    //    if (str[0] == ',' || str[0] == '.' || str[0] == '!' || str[0] == '?')
                    //        childNode.InnerText = str + " ";

                    //    if (str[0] == 'y' || str[0] == 'u' || str[0] == 'o' || str[0] == 'ơ' || str[0] == 'ở' || str[0] == 'ô')
                    //        childNode.InnerText = str + " ";
                        
                    //}

                    //Trim node prev
                    if (length > 0)
                    {
                        if (node.Name == "sup" || str[0] == ',' || str[0] == '.' || str[0] == '?' || str[0] == '!' || str[0] == '\"' || str[0] == '\'' || str.Length == 1)
                        {
                            XmlNode prevNode = node.PreviousSibling;
                            if (prevNode == null)
                                prevNode = node.ParentNode.PreviousSibling;
                            if(prevNode == null)
                                prevNode = node.ParentNode.ParentNode.PreviousSibling;
                            if (prevNode != null)
                            {
                                if (prevNode.ChildNodes.Count > 0)
                                {
                                    XmlNode tNode = prevNode.LastChild;
                                    if (tNode != null && tNode.NodeType == XmlNodeType.Text)
                                    {
                                        tNode.InnerText = tNode.InnerText.TrimEnd();
                                    }
                                    else if(tNode != null)
                                    {
                                        XmlNode ttNode = tNode.LastChild;
                                        if (ttNode != null && ttNode.NodeType == XmlNodeType.Text)
                                        {
                                            ttNode.InnerText = ttNode.InnerText.TrimEnd();
                                        }
                                        else if (ttNode != null)
                                        {
                                            XmlNode tttNode = ttNode.LastChild;

                                            if (tttNode != null && tttNode.NodeType == XmlNodeType.Text)
                                            {
                                                tttNode.InnerText = tttNode.InnerText.TrimEnd();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                
            }
        }
        public static void ProcessParaph(XmlDocument doc, XmlNode childNode, List<XmlNode> reserveNodeList, ref int sequence, ref int t)
        {

            try
            {

                if (childNode.ChildNodes.Count > 0)
                {
                    if ((childNode.Name == "span" || childNode.Name == "img") && childNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                        reserveNodeList.Add(childNode);
                    else
                    {
                        XmlNode tNode = doc.CreateElement("span");
                        XmlAttribute att = doc.CreateAttribute("rel");
                        XmlText textNode = doc.CreateTextNode(" ");
                        att.Value = t.ToString();
                        tNode.Attributes.Append(att);
                        tNode.AppendChild(textNode);
                        reserveNodeList.Add(tNode);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "1");
            }

            if (childNode.ChildNodes.Count > 0 && childNode.NodeType != XmlNodeType.Text)
            {
                foreach (XmlNode cNode in childNode.ChildNodes)
                {
                    try
                    {
                        if (cNode.NodeType == XmlNodeType.Text)
                            continue;
                        if (cNode.Name == "br")
                        {

                            continue;
                        }
                        if (cNode.Attributes["rel"] != null)
                            t = int.Parse(cNode.Attributes["rel"].Value);
                        if (t < sequence)
                            sequence = t;

                        if (cNode.ChildNodes.Count > 0)
                        {
                            if (cNode.Name == "span" && cNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                            {
                                reserveNodeList.Add(cNode);
                            }
                            else
                            {
                                XmlNode tNode = doc.CreateElement("span");
                                XmlAttribute att = doc.CreateAttribute("rel");
                                XmlText textNode = doc.CreateTextNode(" ");
                                att.Value = t.ToString();
                                tNode.Attributes.Append(att);
                                tNode.AppendChild(textNode);
                                reserveNodeList.Add(tNode);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "2");
                    }

                    if (cNode.ChildNodes.Count > 0 && cNode.NodeType != XmlNodeType.Text)
                    {
                        foreach (XmlNode ccNode in cNode.ChildNodes)
                        {
                            try
                            {
                                if (ccNode.NodeType == XmlNodeType.Text)
                                    continue;
                                if (ccNode.Name == "br")
                                {
                                    continue;
                                }

                                if (ccNode.Attributes["rel"] != null)
                                    t = int.Parse(ccNode.Attributes["rel"].Value);
                                if (t < sequence)
                                    sequence = t;

                                if (ccNode.ChildNodes.Count > 0)
                                {
                                    if (ccNode.Name == "span" && ccNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                                    {
                                        reserveNodeList.Add(ccNode);
                                    }
                                    else
                                    {
                                        XmlNode tNode = doc.CreateElement("span");
                                        XmlAttribute att = doc.CreateAttribute("rel");
                                        XmlText textNode = doc.CreateTextNode(" ");
                                        att.Value = t.ToString();
                                        tNode.Attributes.Append(att);
                                        tNode.AppendChild(textNode);
                                        reserveNodeList.Add(tNode);

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "3");
                            }

                        }

                    }
                }

            }
        }

        public static void ProcessSubAndItalic(XmlDocument doc, XmlNode childNode, List<XmlNode> reserveNodeList, ref int sequence)
        {
            int t = int.MaxValue;
           
            try
            {
                reserveNodeList.Add(childNode);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "1");
            }

            if (childNode.ChildNodes.Count > 0 && childNode.NodeType != XmlNodeType.Text)
            {
                List<XmlNode> removeNodeList = new List<XmlNode>();
                foreach (XmlNode cNode in childNode.ChildNodes)
                {
                    try
                    {
                        if (cNode.Name == "br")
                        {
                            continue;
                        }
                        if (cNode.Attributes == null || cNode.Attributes["rel"] == null)
                            continue;

                        t = int.Parse(cNode.Attributes["rel"].Value);

                       

                        if (t == sequence)
                        {
                            sequence++;
                        }

                        if (t - sequence > 20 || t < sequence -2)
                            removeNodeList.Add(cNode);


                        List<XmlNode> removeNodeList2 = new List<XmlNode>();
                        foreach (XmlNode ccNode in cNode.ChildNodes)
                        {
                            if (ccNode.Attributes == null || ccNode.Attributes["rel"] == null)
                                continue;

                            t = int.Parse(ccNode.Attributes["rel"].Value);
                            if (t == sequence)
                            {
                                sequence++;
                            }

                            if (t - sequence > 20 || t < sequence - 2)
                                removeNodeList2.Add(ccNode);
                        }

                        foreach (XmlNode rNode in removeNodeList2)
                        {
                            cNode.RemoveChild(rNode);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "2");
                    }

                

                    
                }
                foreach (XmlNode rNode in removeNodeList)
                {
                    childNode.RemoveChild(rNode);
                }

            }

           
        }
    }
}
