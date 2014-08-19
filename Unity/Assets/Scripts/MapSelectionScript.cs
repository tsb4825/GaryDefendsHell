using UnityEngine;
using System.Collections;
using System.Linq;

public class MapSelectionScript : MonoBehaviour
{
    public bool IsLevelSelected;
    public Transform LevelSelected;
    public int MaxLevel;
    
    void Start()
    {
        HideInactiveMaps(MaxLevel);
    }

    private void HideInactiveMaps(int maxLevel)
    {
        for (int index = maxLevel; index > 1; index--)
        {
            if (PlayerPrefs.HasKey("Level" + index))
            {
                GameObject.Find("Level" + index).renderer.enabled = true;
                GameObject.Find("Level" + index).GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }
    }

    void OnGUI()
    {
        if (IsLevelSelected)
        {
            BuildTowerPopup();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.Any(x => x.collider is BoxCollider2D && !x.rigidbody.isKinematic))
            {
                if (LevelSelected == hit.First().transform)
                {
                    IsLevelSelected = false;
                    LevelSelected = null;
                }
                else
                {
                    IsLevelSelected = true;
                    LevelSelected = hit.First().transform;
                }

            }
        }
    }

    private void BuildTowerPopup()
    {
        var screenPosition = Camera.main.WorldToScreenPoint(LevelSelected.position + Vector3.up);
        float guiY = Screen.height - screenPosition.y;
        GUI.BeginGroup(new Rect(screenPosition.x - 50f, guiY - 75f, 100f, 100f));
        GUI.Box(new Rect(0, 0, 100f, 100f), "");
        if (GUI.Button(new Rect(10f, 10f, 50f, 50f), "Level " + LevelSelected.name.Replace("Level", "")))
        {
            Application.LoadLevel(LevelSelected.name);
        }
        GUI.EndGroup();
    }
}
