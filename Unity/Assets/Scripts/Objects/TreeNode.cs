using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Objects
{
    public class TreeNode
    {
        private List<TreeNode> _children;
        private TowerTypes _towerType;
        private int _goldCost;
        private string _description;

        public TreeNode(TowerTypes towerType, int goldCost, string description)
        {
            _children = new List<TreeNode>();
            _towerType = towerType;
            _goldCost = goldCost;
            _description = description;
        }

        public TowerTypes TowerType { get { return _towerType; } }

        public int GoldCost { get { return _goldCost; } }

        public string Description { get { return _description; } }

        public List<TreeNode> Children
        {
            get { return _children; }
        }

        public void AddChild(TreeNode treeNode)
        {
            _children.Add(treeNode);
        }
    }
}
