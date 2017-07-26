using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager GM;
	public Text Score;
	public Text BestScore;
	public GameObject Player;
	public bool StartLock = true;
	public Image FilledLevel;
	public Text LevelText;
	public int level;
	public Text NextPerk;
	public Text PerkElaboration;
	public Text UnlockLevel;
	public Text CurrentPerks;
	public GameObject White;
	public GameObject levelParticle;

	private int score = 0;
	private int bestscore;
	private float XPpool;
	private float currentXP;
	private static string LevelInfoPath = "Assets/Resources/LevelInfo.txt";

	void Awake ()
	{
		GM = this;
	}

	void Start ()
	{
		StartLock = true;
		bestscore = PlayerPrefs.GetInt ("BestScore", 0);
		BestScore.text = bestscore.ToString ();

		//Extract up Level Info
		level = PlayerPrefs.GetInt ("Level", 0);
		currentXP = PlayerPrefs.GetFloat ("CurrentXP", 0);

		//Set up Level Info
		XPpool = (level + 1) * 100f;
		LevelText.text = level.ToString ();
		FilledLevel.fillAmount = currentXP / XPpool;

		setupLevelText (level);
		if (PlayerPrefs.GetInt ("NeedTouch", 0) == 1) {
			levelParticle.SetActive (true);
		} else {
			levelParticle.SetActive (false);
		}
	}

	public void Restart ()
	{
		GameOver ();
		Time.timeScale = 1f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	private void GameOver ()
	{
		if (score > bestscore) {
			Debug.Log ("Change Best Score");
			PlayerPrefs.SetInt ("BestScore", score);
		}
		PlayerPrefs.SetInt ("LastScore", score);
	}

	public void StartGame ()
	{
		Time.timeScale = 1f;
		GetComponent<EnemiesSpawner> ().StartGame ();
		StartLock = false;
		// Unlock Player Control
		StartCoroutine (Unlock ());
	}

	IEnumerator Unlock ()
	{
		yield return new WaitForSeconds (0f);
		Player.GetComponent<PlayerControl> ().StartLock = false;
		
	}

	public void ResetPlayerPrefs ()
	{
		PlayerPrefs.DeleteAll ();
	}

	public void PauseGame ()
	{
		Time.timeScale = 0f;
	}

	public void Tryscore (int num)
	{
		score += num;
		Score.text = score.ToString ();
		AddXP (num);
	}

	void AddXP (int num)
	{
		currentXP += (num * 50);
		if (currentXP >= XPpool) {
			// Level UP	
			LevelUP ();
		} else {
			PlayerPrefs.SetFloat ("CurrentXP", currentXP);
			FilledLevel.fillAmount = currentXP / XPpool;
		}
	}

	void LevelUP ()
	{
		levelParticle.SetActive (true);
		PlayerPrefs.SetInt ("NeedTouch", 1);
		level++;
		LevelText.text = level.ToString ();
		currentXP = 0;
		XPpool += 100f;
		FilledLevel.fillAmount = 0f;
		// Write to disk about it
		PlayerPrefs.SetInt ("Level", level);
		PlayerPrefs.SetFloat ("CurrentXP", currentXP);
		setupLevelText (level);
		switch (level) {
		case 1:
			White.GetComponent<Gemini> ().setDistanceLonger ();
			break;
		case 3:
			White.GetComponent<Gemini> ().setDistanceLonger ();
			break;
		case 4:
			PlayerPrefs.SetInt ("SpawnHeal", 1);
			PlayerPrefs.SetInt ("HealAmount", 1);
			break;
		case 7:
			PlayerPrefs.SetInt ("InvicibleAble", 1);
			break;
		case 9:
			White.GetComponent<Gemini> ().setDistanceLonger ();
			break;
		case 11:
			PlayerPrefs.SetInt ("HealAmount", 2);
			break;
		case 14:
			break;
		case 15:
			White.GetComponent<Gemini> ().setDistanceLonger ();
			break;
		case 16:
			PlayerPrefs.SetInt ("HealAmount", 3);
			break;
		case 20:
			break;
		default:
			break;
		}
	}

	void setupLevelText (int currentLevel)
	{
		string fileData = System.IO.File.ReadAllText (LevelInfoPath);
		string[] lines = fileData.Split ("\n" [0]);
		string[] words = { "" };
		string[] currentPerks = { "" };
		if (currentLevel < 1) {
			words = lines [0].Split (',');
		} else if (currentLevel >= 1 && currentLevel < 3) {
			words = lines [1].Split (',');
		} else if (currentLevel >= 3 && currentLevel < 4) {
			words = lines [2].Split (',');
		} else if (currentLevel >= 4 && currentLevel < 7) {
			words = lines [3].Split (',');
		} else if (currentLevel >= 7 && currentLevel < 9) {
			words = lines [4].Split (',');
		} else if (currentLevel >= 9 && currentLevel < 11) {
			words = lines [5].Split (',');
		} else if (currentLevel >= 11 && currentLevel < 14) {
			words = lines [6].Split (',');
		} else if (currentLevel >= 14 && currentLevel < 15) {
			words = lines [7].Split (',');
		} else if (currentLevel >= 15 && currentLevel < 16) {
			words = lines [8].Split (',');
		} else if (currentLevel >= 16 && currentLevel < 20) {
			words = lines [9].Split (',');
		} else {
			
		}
		currentPerks = words [words.Length - 1].Split (';');
		UnlockLevel.text = words [0];
		NextPerk.text = words [1];
		PerkElaboration.text = words [2];
		foreach (string perk in currentPerks) {
			CurrentPerks.text += (perk + "\n");
		}

	}
}
