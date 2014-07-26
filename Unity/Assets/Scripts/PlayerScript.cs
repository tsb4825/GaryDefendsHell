using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
		public int Lives;
		public bool IsGameOver;
		public float GameOverMessageTime;
		public int Gold;
		public bool AreAllCreepsReleased;
		public bool AreAllWavesReleased;
		public int LevelNumber;
		public bool IsGameSaved;
		public int WaveNumber;
		public int TotalWaves;
		public float TimeOfNextWave;

		void OnGUI ()
		{
				if (IsGameOver) {
						GUI.Label (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "Game Over!");
				}
				if (AreAllCreepsReleased && GameObject.FindGameObjectWithTag ("Enemy") == null && Lives >= 0) {
						GUI.Label (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "You Win!");
						if (!IsGameSaved) {
								IsGameSaved = SaveLevelComplete (1);
						}
				}
				GUI.Label (new Rect (0, 0, 100, 50), "Lives: " + ((Lives >= 0) ? Lives : 0));

				GUI.Label (new Rect (110, 0, 100, 50), "Gold: " + Gold);

				GUI.Label (new Rect (210, 0, 100, 50), "Waves " + WaveNumber + " / " + TotalWaves);

				if (WaveNumber > 0 && TimeOfNextWave > 0 && !AreAllWavesReleased)
				{
					GUI.Label (new Rect (0, 30, 150, 50), "Time to next wave: " + Mathf.Round (TimeOfNextWave - Time.time));
				}
		}

		void Update ()
		{
				if (IsGameOver) {
						if (Time.time >= GameOverMessageTime) {
								Application.LoadLevel ("MainScreen");
						}
				}
				if (AreAllCreepsReleased && GameObject.FindGameObjectWithTag ("Enemy") == null && Lives >= 0) {
						if (GameOverMessageTime == default(float)) {
								GameOverMessageTime = Time.time + 3;
						}
						if (Time.time >= GameOverMessageTime) {
								Application.LoadLevel ("MainScreen");
						}
				}
		}

		public void LoseLives (int lives)
		{
				Lives -= lives;
				if (Lives <= 0) {
						IsGameOver = true;
						GameOverMessageTime = Time.time + 3;
				}
		}

		public void AddGold (int gold)
		{
				Gold += gold;
		}

		public void SubtractGold (int gold)
		{
				Gold -= gold;
		}

		public PlayerSettings GetPlayerSettings ()
		{
				return new PlayerSettings{
					SellRate = 0.5f
				};
		}

		private bool SaveLevelComplete (int starLevel)
		{
				if (PlayerPrefs.HasKey ("Level" + LevelNumber)) {
						if (starLevel > PlayerPrefs.GetInt ("Level" + LevelNumber)) {
								PlayerPrefs.SetInt ("Level" + LevelNumber, starLevel);
						}
				} else {
						PlayerPrefs.SetInt ("Level" + LevelNumber, starLevel);
				}
				UtilityFunctions.DebugMessage (PlayerPrefs.GetInt ("Level" + LevelNumber).ToString ());
			
				return true;
		}
}

public class PlayerSettings
{
		public float SellRate { get; set; }
}