using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System.IO;
using TutorialDesigner;

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
	public TextAsset levelInfo;
	public TextAsset ExperienceTxt;
	public GameObject LevelUPText;
	public GameObject HealthCircle;

	private int score = 0;
	private int bestscore;
	private float XPpool;
	private float currentXP;
	private string[] XPpoolstr;
	public int comboCounter = -1;

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
		XPpoolstr = ExperienceTxt.text.Split ("\n" [0]);
		float.TryParse (XPpoolstr [level], out XPpool);
		LevelText.text = level.ToString ();
		FilledLevel.fillAmount = currentXP / XPpool;

		if (level < 1) {
			HealthCircle.SetActive (false);
		}
		setupLevelText (level);
		if (PlayerPrefs.GetInt ("NeedTouch", 0) == 1) {
			levelParticle.SetActive (true);
		} else {
			levelParticle.SetActive (false);
		}
		comboCounter = 0;
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
		PlayerPrefs.SetInt ("LastScore", EnemiesSpawner.ES.epoch);
	}

	public void StartGame ()
	{
		Time.timeScale = 1f;
		EventManager.TriggerEvent ("GameStarted");
		if (GameObject.Find ("TutorialSystem").GetComponent<SavePoint> ().IsTutorialDone ())
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
		score += (num * (comboCounter > 0 ? comboCounter : 1));
		Score.text = score.ToString ();
		AddXP (num);
	}

	void AddXP (int num)
	{
		currentXP += (num * 10 * (comboCounter > 0 ? comboCounter : 1));
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
		StartCoroutine (LevelUpText (2f));
		levelParticle.SetActive (true);
		PlayerPrefs.SetInt ("NeedTouch", 1);
		level++;
		LevelText.text = level.ToString ();
		currentXP = 0;
		float.TryParse (XPpoolstr [level], out XPpool);
		FilledLevel.fillAmount = 0f;
		// Write to disk about it
		PlayerPrefs.SetInt ("Level", level);
		PlayerPrefs.SetFloat ("CurrentXP", currentXP);
		setupLevelText (level);
		switch (level) {
		case 1:
			HealthCircle.SetActive (true);
			PlayerPrefs.SetInt ("DuotsEnable", 1);
			break;
		case 2:
			White.GetComponent<Gemini> ().setDistanceLonger ();
			break;
		case 3:
			White.GetComponent<Gemini> ().setDistanceLonger ();
			break;
		case 4:
			PlayerPrefs.SetInt ("SpawnHeal", 1);
			PlayerPrefs.SetInt ("HealAmount", 2);
			break;
		case 5:
			PlayerPrefs.SetInt ("InvicibleAble", 1);
			break;
		case 6:
			White.GetComponent<Gemini> ().setDistanceLonger ();
			break;
		case 7:
			PlayerPrefs.SetInt ("HealAmount", 3);
			break;
		case 8:
			PlayerPrefs.SetInt ("FrostRing", 1);
			break;
		case 9:
			White.GetComponent<Gemini> ().setDistanceLonger ();
			break;
		case 10:
			PlayerPrefs.SetInt ("invincibleOrb", 1);
			break;
		case 11:
			PlayerPrefs.SetInt ("DestructionRing", 1);
			break;
		case 12:
			PlayerPrefs.SetInt ("FrostRing", 2);
			break;
		case 13:
			PlayerPrefs.SetInt ("DestructionRing", 2);
			break;
		case 14:
			PlayerPrefs.SetInt ("invincibleOrb", 2);
			break;
		default:
			break;
		}
	}

	void setupLevelText (int currentLevel)
	{
		
		string fileData = levelInfo.text;
		string[] lines = fileData.Split ("\n" [0]);
		string[] words = { "" };
		string[] currentPerks = { "" };
		if (currentLevel <= 13) {
			words = lines [currentLevel].Split (',');
		} else {
			words = lines [lines.Length - 1].Split (',');
		}
		currentPerks = words [words.Length - 1].Split (';');
		UnlockLevel.text = words [0];
		NextPerk.text = words [1];
		PerkElaboration.text = words [2];
		CurrentPerks.text = "";
		foreach (string perk in currentPerks) {
			CurrentPerks.text += (perk + "\n");
		}
	}

	IEnumerator LevelUpText (float time)
	{
		LevelUPText.SetActive (true);
		yield return new WaitForSeconds (time);
		LevelUPText.SetActive (false);
	}

}
