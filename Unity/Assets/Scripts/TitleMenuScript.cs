using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleMenuScript : MonoBehaviour
{
    Vector2 startButtonSize = new Vector2(84f, 60f);
    Vector2 resetGameDataButtonSize = new Vector2(154f, 60f);
    Vector2 creditsButtonSize = new Vector2(84f, 60f);
    Vector2 buttonSpacing = new Vector2(40f, 40f);
    private float delayBetweenWords = .75f;
    public AudioClip wordFall;
    private IDictionary<WordTypes, AnimatingWord> animatingWords;
    private bool ShowConfirmWindow;

    void Start()
    {
        float delayTime = Time.time;
        animatingWords = new Dictionary<WordTypes, AnimatingWord>
				{ 
					{
						WordTypes.Hell, 
			         	new AnimatingWord{TargetLocation = new Vector2 (0f, .35f), AnimateTime = delayTime}
					},
					{ 
						WordTypes.Defends, 
						new AnimatingWord{TargetLocation = new Vector2 (0f, 1.55f), AnimateTime = delayTime + delayBetweenWords}
					},
					{ 
						WordTypes.Gary, 
						new AnimatingWord{TargetLocation = new Vector2 (0f, 2.85f), AnimateTime = delayTime + delayBetweenWords * 2}
					},
					{ 
						WordTypes.TD, 
						new AnimatingWord{TargetLocation = new Vector2 (2.45f, .15f), AnimateTime = delayTime + delayBetweenWords * 3}
					}
					};
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - ((startButtonSize.x + buttonSpacing.x + (PlayerPrefs.HasKey("Level1") ? resetGameDataButtonSize.x + buttonSpacing.x : 0) + creditsButtonSize.x) / 2f),
                          2f * Screen.height / 3f,
                          startButtonSize.x + buttonSpacing.x + (PlayerPrefs.HasKey("Level1") ? resetGameDataButtonSize.x + buttonSpacing.x : 0) + creditsButtonSize.x,
                          startButtonSize.y));

        if (GUI.Button(new Rect(0, 0, startButtonSize.x, startButtonSize.y), PlayerPrefs.HasKey("Level1") ? "Continue" : "Start!"))
        {
            Application.LoadLevel("Map");
        }

        if (PlayerPrefs.HasKey("Level1"))
        {
            if (GUI.Button( new Rect(startButtonSize.x + buttonSpacing.x, 0, resetGameDataButtonSize.x, resetGameDataButtonSize.y), "Reset Save Data"))
            {
                ShowConfirmWindow = true;
            }
        }

        if (ShowConfirmWindow)
        {
            DrawModalWindow();
        }

        if (GUI.Button(new Rect(startButtonSize.x + (PlayerPrefs.HasKey("Level1") ? resetGameDataButtonSize.x + buttonSpacing.x : 0) + buttonSpacing.x, 0, creditsButtonSize.x, creditsButtonSize.y), 
            "Credits"))
        {
            Application.LoadLevel("Credits");
        }
        GUI.EndGroup();
    }

    void DrawModalWindow()
    {
        GUI.ModalWindow(0, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 75, 400, 150), ResetSaveData, "Are you sure?");
    }

    void ResetSaveData(int windowID)
    {
        GuiDisplayScript.ConfirmModal("Are you sure you want to reset your save data?", 
            () =>
                {
                    PlayerPrefs.DeleteAll();
                    ShowConfirmWindow = false;
                },
            () => ShowConfirmWindow = false);
    }

    void Update()
    {

        foreach (var animatingWord in animatingWords)
        {
            if (Time.time >= animatingWord.Value.AnimateTime)
            {
                Transform word;
                switch (animatingWord.Key)
                {
                    case WordTypes.Hell:
                        word = GameObject.Find("HellLettering").transform;
                        break;
                    case WordTypes.Defends:
                        word = GameObject.Find("DefendsLettering").transform;
                        break;
                    case WordTypes.Gary:
                        word = GameObject.Find("GaryLettering").transform;
                        break;
                    case WordTypes.TD:
                        word = GameObject.Find("TDLettering").transform;
                        break;
                    default:
                        throw new UnityException("Title word does not exist.");
                }
                word.position = Vector2.MoveTowards(word.position, animatingWord.Value.TargetLocation, Time.deltaTime * 7);
                if ((Vector2)word.position == animatingWord.Value.TargetLocation && !animatingWord.Value.HasSoundEffectPlayed)
                {
                    animatingWord.Value.HasSoundEffectPlayed = true;
                    GetComponent<AudioSource>().PlayOneShot(wordFall);
                }
            }
        }
    }
}

public class AnimatingWord
{
    public bool HasSoundEffectPlayed;
    public Vector2 TargetLocation;
    public float AnimateTime;
}

public enum WordTypes
{
    Gary,
    Defends,
    Hell,
    TD
}