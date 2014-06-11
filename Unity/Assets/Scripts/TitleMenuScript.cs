using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleMenuScript : MonoBehaviour
{
		Vector2 startButtonSize = new Vector2 (84f, 60f);
		Vector2 resetGameDataButtonSize = new Vector2 (154f, 60f);
		Vector2 creditsButtonSize = new Vector2 (84f, 60f);
		Vector2 buttonSpacing = new Vector2 (40f, 40f);
		private float delayBetweenWords = .75f;
		public AudioClip wordFall;
		private IDictionary<WordTypes,AnimatingWord> animatingWords;

		void Start ()
		{
				float delayTime = Time.time;
				animatingWords = new Dictionary<WordTypes,AnimatingWord>
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

		void OnGUI ()
		{
				GUI.BeginGroup (new Rect (Screen.width / 2 - ((startButtonSize.x + buttonSpacing.x + resetGameDataButtonSize.x + buttonSpacing.x + creditsButtonSize.x) / 2f), 
		                          2f * Screen.height / 3f, 
		                          startButtonSize.x + buttonSpacing.x + resetGameDataButtonSize.x + buttonSpacing.x + creditsButtonSize.x, 
		                          startButtonSize.y));
				// Draw a button to start the game
				if (
			GUI.Button (
				// Center in X, 2/3 of the height in Y
				new Rect (
				0,
				0,
				startButtonSize.x,
				startButtonSize.y
				),
				"Start!"
				)
				) {
						// On Click, load the first level.
						// "Stage1" is the name of the first scene we created.
						Application.LoadLevel ("Level1");
				}

				if (
			GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (
			startButtonSize.x + buttonSpacing.x,
			0,
			resetGameDataButtonSize.x,
			resetGameDataButtonSize.y
				),
			"Reset Save Data"
				)
			) {
						PlayerPrefs.DeleteAll ();
				}

				if (
			GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (
			startButtonSize.x + resetGameDataButtonSize.x + (buttonSpacing.x * 2),
			0,
			creditsButtonSize.x,
			creditsButtonSize.y
				),
			"Credits"
				)
			) {
						
				}
				GUI.EndGroup ();
		}

		void Update ()
		{

				foreach (var animatingWord in animatingWords) {
						if (Time.time >= animatingWord.Value.AnimateTime) {
								Transform word;
								switch (animatingWord.Key) {
								case WordTypes.Hell:
										word = GameObject.Find ("HellLettering").transform;
										break;
								case WordTypes.Defends:
										word = GameObject.Find ("DefendsLettering").transform;
										break;
								case WordTypes.Gary:
										word = GameObject.Find ("GaryLettering").transform;
										break;
								case WordTypes.TD:
										word = GameObject.Find ("TDLettering").transform;
										break;
								default:
										throw new UnityException ("Title word does not exist.");
								}
								word.position = Vector2.MoveTowards (word.position, animatingWord.Value.TargetLocation, Time.deltaTime * 7);
								if ((Vector2)word.position == animatingWord.Value.TargetLocation && !animatingWord.Value.HasSoundEffectPlayed) {
										animatingWord.Value.HasSoundEffectPlayed = true;
										audio.PlayOneShot (wordFall);
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