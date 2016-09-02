﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes
{
    public class Nodes<T> where T : IComparable
    {
        public T Element { get; set; }
        public Nodes<T> leftChild { get; set; }
        public Nodes<T> rightChild { get; set; }

        public Nodes(T Element)
        {
            this.Element = Element;
        }
        public override string ToString()
        {
            string nodeString = "[" + this.Element + "";
            if (this.leftChild == null && this.rightChild == null)
            {
                nodeString += " (Leaf) ";
            }
            if (this.leftChild != null)
            {
                nodeString += "Left: " + this.leftChild.ToString();
            }
            if (this.rightChild != null)
            {
                nodeString += "Right: " + this.rightChild.ToString();
            }
            nodeString += "]";
            return nodeString;
        }
    }
}
