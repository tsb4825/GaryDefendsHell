using UnityEngine;
using System.Collections;

public class FadeInScript : MonoBehaviour
{
    public float currentOpacity;
    public float animationSpeed;
    public float waitSeconds;
    float loadTime;
    public string nextLevel;
    

    // Use this for initialization
    void Start()
    {
        transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentOpacity <= 1f && !Input.anyKey)
        {
            currentOpacity += animationSpeed;
            transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, currentOpacity);
            loadTime = Time.time + waitSeconds;
        }
        else
        {
            if (Time.time > loadTime || Input.anyKey)
            {
                Application.LoadLevel(nextLevel);
            }
        }
    }
}