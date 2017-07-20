﻿using System.Collections;
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

	private bool FirstTimeLock = true;

	void Awake ()
	{
		ES = this;
	}

	public void StartGame ()
	{
		if (FirstTimeLock) {
			FirstTimeLock = false;
			StartCoroutine (Generate (0f));
		}
	}

	IEnumerator Generate (float time)
	{
		yield return new WaitForSeconds (time);
		epoch++;
		if (epoch >= 15)
			difficulty++;
		if (epoch >= 30)
			difficulty++;
		for (int i = 0; i < difficulty; i++) {
			int rd = Random.Range (0, SpawnPoints.Length);
			Instantiate (EnemyPrefab, SpawnPoints [rd].position, Quaternion.identity, EnemyParent);
		}
		StartCoroutine (Generate (3f));
	}
}
