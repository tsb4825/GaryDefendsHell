using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System;

public class TowerTreeScript : MonoBehaviour
{
    public TextAsset TowerTreeConfiguration;
    public Dictionary<TowerTreeTypes, List<TreeNode>> TreeLists;

    // Use this for initialization
    void Start()
    {
        TreeLists = new Dictionary<TowerTreeTypes, List<TreeNode>>();
        LoadTreeLists();
    }

    private void LoadTreeLists()
    {
        XDocument document = XDocument.Parse(TowerTreeConfiguration.text);
        foreach (var baseTower in document.Descendants("Base"))
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            AddRootAndChildren(baseTower, treeNodes);
            TreeLists.Add((TowerTreeTypes)Enum.Parse(typeof(TowerTreeTypes), baseTower.Attribute("Name").Value, true), treeNodes);
        }
        UtilityFunctions.DebugMessage(TreeLists.Count() + " base towers found.");
    }

    private TreeNode AddRootAndChildren(XElement root, List<TreeNode> treeNodes)
    {
        TreeNode rootNode = new TreeNode((TowerTypes)Enum.Parse(typeof(TowerTypes), root.Attribute("Name").Value, true), int.Parse(root.Attribute("GoldCost").Value));
        treeNodes.Add(rootNode);
        foreach (XElement child in root.Elements("Child"))
        {
            rootNode.AddChild(AddRootAndChildren(child, treeNodes));
        }
        return rootNode;
    }

    public List<TowerSettings> GetUpgradeOptions(TowerTypes towerType)
    {
        UtilityFunctions.DebugMessage("TreeList Key Count: " + TreeLists.Count);
        List<TreeNode> children = null;
        if (towerType != TowerTypes.Unknown)
        {
            children = TreeLists.First(x => x.Value.Any(y => y.TowerType == towerType)).Value.First(z => z.TowerType == towerType).Children;
        }
        else
        {
            children = new List<TreeNode>();
            foreach (var towerTree in TreeLists)
            {
                children.Add(towerTree.Value.ElementAt(0));
            }
        }
        return (children != null)
    ? children.Select(x => new TowerSettings { TowerType = x.TowerType, GoldCost = x.GoldCost }).ToList()
        : null;
    }
}

public class TreeNode
{
    private List<TreeNode> _children;
    private TowerTypes _towerType;
    private int _goldCost;

    public TreeNode(TowerTypes towerType, int goldCost)
    {
        _children = new List<TreeNode>();
        _towerType = towerType;
        _goldCost = goldCost;
    }

    public TowerTypes TowerType { get { return _towerType; } }

    public int GoldCost { get { return _goldCost; } }

    public List<TreeNode> Children
    {
        get { return _children; }
    }

    public void AddChild(TreeNode treeNode)
    {
        _children.Add(treeNode);
    }
}

public enum TowerTypes
{
    Unknown,
    AOE,
    Normal,
    Homing,
    Barracks,
    StunAndDrain,
    Slow
}

public enum TowerTreeTypes
{
    Unknown,
    AOE,
    Normal,
    Homing,
    Barracks,
    StunAndDrain,
    Slow
}
