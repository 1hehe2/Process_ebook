using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

namespace ProcessEbook
{
    public class ProcessImage
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


        public ProcessImage(XmlNode root)
        {
            _root = root;
            
        }

        public void Process()
        {
           
        }
    }
}
