using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Objects;

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
            TreeLists.Add((TowerTreeTypes)Enum.Parse(typeof(TowerTreeTypes), baseTower.Attribute("Type").Value, true), treeNodes);
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
        Debug.Log(towerType);
        if (towerType != TowerTypes.Unknown)
        {
            Debug.Log(TreeLists.First(x => x.Value.Any(y => y.TowerType == towerType)).Value);
            //Debug.Log();
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

