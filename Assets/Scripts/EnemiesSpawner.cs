using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TutorialDesigner;

public class EnemiesSpawner : MonoBehaviour
{
	public static EnemiesSpawner ES;
	public GameObject EnemyPrefab;
	public Transform[] SpawnPoints;
	public Transform EnemyParent;
	public int epoch = 1;
	public int difficulty = 1;
	public GameObject HealOrbPrefab;

	private int maxEnemy = 9;
	private bool FirstTimeLock = true;

	void Awake ()
	{
		ES = this;
	}

	void Start ()
	{
		int lastscore = PlayerPrefs.GetInt ("LastScore", 0);
		if (lastscore < 15) {
			
		} else {
			epoch = lastscore;
			difficulty = epoch / 15 + 1;
			maxEnemy += (2 * difficulty);
		}
	}

	public void StartGame ()
	{
		if (FirstTimeLock) {
			FirstTimeLock = false;
			SpecialGenerate (0.7f);
			if (PlayerPrefs.GetInt ("SpawnHeal", 0) == 1)
				StartCoroutine (SpawnHealOrb (0f));
		}
	}

	void SpecialGenerate (float ratio)
	{
		int rd = Random.Range (0, SpawnPoints.Length);
		if (GameObject.Find ("TutorialSystem").GetComponent<SavePoint> ().IsTutorialDone ()) {
			Instantiate (EnemyPrefab, SpawnPoints [rd].position * ratio, Quaternion.identity, EnemyParent);
			StartCoroutine (Generate (2f));
		} else {
			GameObject specialEnemy = (GameObject)Instantiate (EnemyPrefab, SpawnPoints [4].position * 0.5f, Quaternion.identity, EnemyParent);
			specialEnemy.GetComponent<EnemyControl> ().DemandHealth = 1;
			StartCoroutine (TutorialGenerate (2f));
		}
	}

	IEnumerator TutorialGenerate (float time)
	{
		yield return new WaitForSeconds (time);
		GameObject specialEnemy = (GameObject)Instantiate (EnemyPrefab, SpawnPoints [3].position * 0.9f, Quaternion.identity, EnemyParent);
		specialEnemy.GetComponent<EnemyControl> ().DemandHealth = 2;
		StartCoroutine (Generate (4f));
	}

	IEnumerator oneTimeGenerate (float time)
	{
		yield return new WaitForSeconds (time);
		int rd = Random.Range (0, SpawnPoints.Length);
		Instantiate (EnemyPrefab, SpawnPoints [rd].position, Quaternion.identity, EnemyParent);
	}

	IEnumerator SpawnHealOrb (float time)
	{
		yield return new WaitForSeconds (time);
		int rd = Random.Range (0, SpawnPoints.Length / 2);
		Instantiate (HealOrbPrefab, SpawnPoints [rd * 2 + 1].position, Quaternion.identity);
		StartCoroutine (SpawnHealOrb (40f));
	}

	IEnumerator Generate (float time)
	{
		yield return new WaitForSeconds (time);
		if (EnemyParent.childCount < maxEnemy) {
			epoch++;
			if (epoch % 15 == 0) {
				difficulty++;
				maxEnemy += 2;
			}
			int rd = Random.Range (0, SpawnPoints.Length);
			Instantiate (EnemyPrefab, SpawnPoints [rd].position, Quaternion.identity, EnemyParent);
			if (difficulty == 3) {
				StartCoroutine (oneTimeGenerate (1f));
			}
		}
		StartCoroutine (Generate (5f - Mathf.Min (1f * epoch / 7.5f, 3f)));
	}
}
