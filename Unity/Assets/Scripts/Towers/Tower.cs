using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class Tower : MonoBehaviour
{
    public List<Transform> Targets;
    public Transform Target;
    public float AttackCooldown;
    public float NextFireTime;
    public TowerTypes TowerType;
    public List<TowerSettings> TowerChoices;
    public bool IsTowerSelected;
    public int GoldCost;
    const float BoxWidth = 150;
    const float BoxHeight = 50;
    const float ButtonWidth = 30;
    const float ButtonHeight = 30;
    const float ButtonSpacingWidth = 6;
    const float ButtonSpacingHeight = 6;

    void OnGUI()
    {
        if (IsTowerSelected)
        {
            if (TowerChoices != null)
            {
                BuildButtons();
            }
        }
    }

    private void BuildButtons()
    {
        Vector3 point = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
        float guiY = Screen.height - point.y;
        GUI.Box(new Rect(point.x - (BoxWidth / 2), guiY - (BoxHeight / 2), BoxWidth, BoxHeight), "");
        var player = GameObject.FindObjectOfType<PlayerScript>();

        for (var index = 0; index < TowerChoices.Count; index++)
        {
            if (GUI.Button(
    new Rect(point.x - (BoxWidth / 2) + (ButtonSpacingWidth * (index + 1)) + (ButtonWidth * index), guiY - (BoxHeight / 2) + ButtonSpacingHeight, ButtonWidth, ButtonHeight),
    (Texture)Resources.Load(GetTextureName(TowerChoices[index].TowerType))))
            {
                if (player.Gold >= TowerChoices[index].GoldCost)
                {
                    player.SubtractGold(TowerChoices[index].GoldCost);
                    Transform tower = (Transform)Instantiate(Resources.Load<Transform>(GetTransformName(TowerChoices[index].TowerType)), transform.position, Quaternion.identity);
                    tower.GetComponent<Tower>().GoldCost += TowerChoices[index].GoldCost;
                    Destroy(this.gameObject);
                }
            }
        }

        if (TowerType != TowerTypes.Unknown)
        {
            GUI.Box(
    new Rect(point.x - (BoxWidth / 2), guiY + (BoxHeight / 2) + transform.GetComponent<SpriteRenderer>().sprite.texture.height, BoxWidth, BoxHeight), "");
            if (GUI.Button(
    new Rect(point.x - (BoxWidth / 2) + ButtonSpacingWidth, guiY + (BoxHeight / 2) + transform.GetComponent<SpriteRenderer>().sprite.texture.height + ButtonSpacingHeight, ButtonWidth, ButtonHeight),
                (Texture)Resources.Load("Sell")))
            {
                PlayerSettings playerSettings = GameObject.FindObjectOfType<PlayerScript>().GetPlayerSettings();
                player.AddGold(Mathf.FloorToInt(GoldCost * playerSettings.SellRate));
                Transform tower = (Transform)Instantiate(Resources.Load<Transform>("TowerBase"), transform.position, Quaternion.identity);
                tower.GetComponent<Tower>().GoldCost = 0;
                Destroy(this.gameObject);
            }
        }
    }

    private string GetTextureName(TowerTypes towerType)
    {
        switch (towerType)
        {
            case TowerTypes.Normal:
                return "Normal";
            case TowerTypes.Homing:
                return "Homing";
            case TowerTypes.AOE:
                return "Sphere";
            case TowerTypes.Barracks:
                return "Barracks";
            default:
                throw new UnityException("TowerType not supported.");
        }
    }

    private string GetTransformName(TowerTypes towerType)
    {
        switch (towerType)
        {
            case TowerTypes.Normal:
                return "NormalTower";
            case TowerTypes.Homing:
                return "HomingTower";
            case TowerTypes.AOE:
                return "AOETower";
            case TowerTypes.Barracks:
                return "BarracksTower";
            default:
                throw new UnityException("TowerType not supported.");
        }
    }

    private void CloseOtherWindows()
    {
        foreach (var towerBase in GameObject.FindGameObjectsWithTag("Tower"))
        {
            towerBase.GetComponent<Tower>().IsTowerSelected = false;
        }
    }

    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.Any(x => x.collider is BoxCollider2D && x.transform == transform))
            {
                UtilityFunctions.DebugMessage("Tower selector hit");
                if (IsTowerSelected)
                {
                    IsTowerSelected = false;
                }
                else
                {
                    CloseOtherWindows();
                    IsTowerSelected = true;
                    TowerChoices = GameObject.FindObjectOfType<TowerTreeScript>().GetUpgradeOptions(TowerType);
                    UtilityFunctions.DebugMessage("TowerChoices: " + TowerChoices.Count);
                }
            }
        }
        if (Target != null)
        {
            if (Time.time >= NextFireTime)
            {
                Fire();
                NextFireTime = Time.time + AttackCooldown;
            }
        }
        else if (Targets.Count > 0)
        {
            Targets.Remove(Target);
        }
        if (Target == null && Targets.Count > 0)
        {
            UtilityFunctions.DebugMessage("Searching for target through update");
            List<Transform> targets = FindClosestTargetsToBase(1);
            Target = targets.Any() ? targets[0] : null;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" && collider is BoxCollider2D)
        {
            Targets.Add(collider.transform);
            UtilityFunctions.DebugMessage("Searching for target through trigger");
            List<Transform> targets = FindClosestTargetsToBase(1);
            Target = targets.Any() ? targets[0] : null;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" && collider is BoxCollider2D)
        {
            Targets.Remove(collider.transform);
            UtilityFunctions.DebugMessage("Searching for target through exit trigger");
            List<Transform> targets = FindClosestTargetsToBase(1);
            Target = targets.Any() ? targets[0] : null;
        }
    }

    public abstract void Fire();

    protected List<Transform> FindClosestTargetsToBase(int targets)
    {
        RemoveNullTargets();
        UtilityFunctions.DebugMessage("Targets: " + Targets.Count);
        var basePosition = UseUnitZPosition(GameObject.FindGameObjectsWithTag("Base")[0].transform.position);
        // Iterate through them and find the closest to base
        List<Creep> creeps = Targets.Select(x =>
            new Creep
            {
                Transform = x,
                Distance = (UseUnitZPosition(x.transform.position) - basePosition).sqrMagnitude
            }).ToList();

        return creeps.OrderBy(x => x.Distance).Take(targets).Select(x => x.Transform).ToList();
    }

    protected IEnumerable<Transform> FindClosestTargets(int targets)
    {
        RemoveNullTargets();
        if (targets >= Targets.Count())
            return Targets.ToList();
        else
        {
            List<Creep> creeps = Targets.Select(x =>
                new Creep
                {
                    Transform = x,
                    Distance = (UtilityFunctions.UseUnitZPosition(x, x.transform.position) - UtilityFunctions.UseUnitZPosition(x, transform.position)).sqrMagnitude
                }).ToList();
            return creeps.OrderBy(x => x.Distance).Take(targets).Select(x => x.Transform).ToList();
        }
    }

    private class Creep
    {
        public Transform Transform { get; set; }
        public float Distance { get; set; }
    }

    private Vector3 UseUnitZPosition(Vector3 position)
    {
        position.z = 0;
        return position;
    }

    private void RemoveNullTargets()
    {
        Targets.RemoveAll(x => x == null);
    }
}
