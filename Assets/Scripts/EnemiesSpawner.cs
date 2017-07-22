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
		epoch++;
		int rd = Random.Range (0, SpawnPoints.Length);
		Instantiate (EnemyPrefab, SpawnPoints [rd].position * 0.8f, Quaternion.identity, EnemyParent);
		StartCoroutine (Generate (2f));
	}

	IEnumerator SpawnHealOrb (float time)
	{
		yield return new WaitForSeconds (time);
		int rd = Random.Range (0, SpawnPoints.Length / 2);
		Instantiate (HealOrbPrefab, SpawnPoints [rd * 2 + 1].position, Quaternion.identity, EnemyParent);
		StartCoroutine (SpawnHealOrb (40f / difficulty));
	}

	IEnumerator Generate (float time)
	{
		yield return new WaitForSeconds (time);
		epoch++;
		if (epoch == 20)
			difficulty++;
		if (epoch == 35)
			difficulty++;
		for (int i = 0; i < (difficulty == 3 ? 2 : 1); i++) {
			int rd = Random.Range (0, SpawnPoints.Length);
			Instantiate (EnemyPrefab, SpawnPoints [rd].position, Quaternion.identity, EnemyParent);
		}
		StartCoroutine (Generate (5f - difficulty));
	}
}
