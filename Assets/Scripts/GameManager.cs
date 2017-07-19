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

	private int score = 0;
	private int bestscore;

	void Awake ()
	{
		GM = this;
	}

	void Start ()
	{
		bestscore = PlayerPrefs.GetInt ("BestScore", 0);
		BestScore.text = bestscore.ToString ();
	}

	public void Restart ()
	{
		GameOver ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	private void GameOver ()
	{
		if (score > bestscore) {
			Debug.Log ("Change Best Score");
			PlayerPrefs.SetInt ("BestScore", score);
		}
	}

	public void StartGame ()
	{
		Time.timeScale = 1f;
		GetComponent<EnemiesSpawner> ().StartGame ();
		// Unlock Player Control
		StartCoroutine (Unlock ());
	}

	IEnumerator Unlock ()
	{
		yield return new WaitForSeconds (0f);
		Player.GetComponent<PlayerControl> ().StartLock = false;
		
	}

	public void PauseGame ()
	{
		Time.timeScale = 0f;
	}

	public void Tryscore (int num)
	{
		score += num;
		Score.text = score.ToString ();
	}
}
