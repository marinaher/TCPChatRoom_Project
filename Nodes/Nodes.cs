using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes
{
    public class Nodes<T> where T : IComparable
    {
        public T Element { get; set; }
        public Nodes<T> left { get; set; }
        public Nodes<T> right { get; set; }

        public Nodes(T Element)
        {
            this.Element = Element;
        }
        public override string ToString()
        {
            string nodeString = "[" + this.Element + "";
            if (this.left == null && this.right == null)
            {
                nodeString += " (Leaf) ";
            }
            if (this.left != null)
            {
                nodeString += "Left: " + this.left.ToString();
            }
            if (this.right != null)
            {
                nodeString += "Right: " + this.right.ToString();
            }
            nodeString += "]";
            return nodeString;
        }
    }
}
