using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Nodes
{
    class BinarySearchTree<TKey, TValue> : IEnumerable<Nodes<TKey, TValue>> where TKey : IComparable<TKey>
    {
        Dictionary<string, Socket> Clients = new Dictionary<string, Socket>();

        private Nodes<TKey, TValue> _root;

        public bool Search(TKey key, out TValue value)
        {
            Nodes<TKey, TValue> node = _root;

            while (node != null)
            {
                if (key.CompareTo(node.Key) < 0)
                {
                    node = node.Left;
                }
                else if (key.CompareTo(node.Key) > 0)
                {
                    node = node.Right;
                }
                else
                {
                    value = node.Value;
                    return true;
                }
            }

            value = default(TValue);

            return false;
        }

        public void Add(TKey key, TValue value)
        {
            if (_root == null)
            {
                _root = new Nodes<TKey, TValue>(key, value);
            }
            else
            {
                Nodes<TKey, TValue> node = _root;

                while (node != null)
                {
                    int comaprisonResult = node.Key.CompareTo(key);

                    if (comaprisonResult < 0)
                    {
                        Nodes<TKey, TValue> left = node.Left;

                        if (left == null)
                        {
                            node.Left = new Nodes<TKey, TValue>(key, value, node);
                            return;
                        }
                        node = left;
                    }
                    else if (comaprisonResult > 0)
                    {
                        Nodes<TKey, TValue> right = node.Right;

                        if (right == null)
                        {
                            node.Right = new Nodes<TKey, TValue>(key, value, node);
                            return;
                        }
                        node = right;
                    }
                    else
                    {
                        node.Key = key;
                        node.Value = value;
                        return;
                    }
                }
            }
        }
        private static void Replace(Nodes<TKey, TValue> target, Nodes<TKey, TValue> source)
        {
            Nodes<TKey, TValue> left = source.Left;
            Nodes<TKey, TValue> right = source.Right;

            target.Key = source.Key;
            target.Value = source.Value;
            target.Left = left;
            target.Right = right;

            if (left != null)
            {
                left.Parent = target;
            }

            if (right != null)
            {
                right.Parent = target;
            }
        }
        IEnumerator<Nodes<TKey, TValue>> IEnumerable<Nodes<TKey, TValue>>.GetEnumerator()
        {
            return _root.GetEnumerator();
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_root).GetEnumerator();
        }
    }
}