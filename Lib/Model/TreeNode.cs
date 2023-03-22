using System;
using System.Collections.Generic;

namespace adventofcode.Lib.Model
{
    public class TreeNode<T>
    {
        T Node;
        TreeNode<T> Parent;
        HashSet<TreeNode<T>> Children;

        public TreeNode(T node)
        {
            this.Node = node;
            Children = new();
        }

        public TreeNode(T node, HashSet<TreeNode<T>> children) : this(node)
        {
            this.Children = children;
        }

        public void AddChild(T childNode)
        {
            TreeNode<T> child = new(childNode);
            child.Parent = this;
            Children.Add(child);
        }

        public void AddChild(TreeNode<T> childNode)
        {
            childNode.Parent = this;
            Children.Add(childNode);
        }

        public T GetValue() => this.Node;
        public TreeNode<T> GetParent() => this.Parent;
        public T GetParentValue() => this.Parent.Node;
        public HashSet<TreeNode<T>> GetChildren() => this.Children;

    }
}

