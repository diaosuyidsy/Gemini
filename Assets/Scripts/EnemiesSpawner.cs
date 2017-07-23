using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
	public static EnemiesSpawner ES;
	public GameObject EnemyPrefab;
	public Transform[] SpawnPoints;
	public Transform EnemyParent;
	public int epoch = 1;
	public int difficulty = 1;
	public GameObject HealOrbPrefab;

	private bool FirstTimeLock = true;

	void Awake ()
	{
		ES = this;
	}

	void Start ()
	{
		Debug.Log (PlayerPrefs.GetInt ("LastScore"));
		if (PlayerPrefs.GetInt ("LastScore", 0) >= 30) {
			epoch = 14;
		}
	}

	public void StartGame ()
	{
		if (FirstTimeLock) {
			FirstTimeLock = false;
			SpecialGenerate ();
			StartCoroutine (SpawnHealOrb (0f));
		}
	}

	void SpecialGenerate ()
	{
		int rd = Random.Range (0, SpawnPoints.Length);
		Instantiate (EnemyPrefab, SpawnPoints [rd].position * 0.7f, Quaternion.identity, EnemyParent);
		StartCoroutine (Generate (2f));
	}

	IEnumerator SpawnHealOrb (float time)
	{
		yield return new WaitForSeconds (time);
		int rd = Random.Range (0, SpawnPoints.Length / 2);
		Instantiate (HealOrbPrefab, SpawnPoints [rd * 2 + 1].position, Quaternion.identity);
		StartCoroutine (SpawnHealOrb (40f / difficulty));
	}

	IEnumerator Generate (float time)
	{
		yield return new WaitForSeconds (time);
		Debug.Log (EnemyParent.childCount);
		if (EnemyParent.childCount < 9) {
			epoch++;
			if (epoch == 15)
				difficulty++;
			if (epoch == 30)
				difficulty++;
			for (int i = 0; i < (difficulty == 3 ? 2 : 1); i++) {
				int rd = Random.Range (0, SpawnPoints.Length);
				Instantiate (EnemyPrefab, SpawnPoints [rd].position, Quaternion.identity, EnemyParent);
			}
		}
		StartCoroutine (Generate (5f - difficulty));
	}
}
