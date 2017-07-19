using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
	public GameObject EnemyPrefab;
	public Transform[] SpawnPoints;
	public Transform EnemyParent;

	private bool FirstTimeLock = true;

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
		int rd = Random.Range (0, SpawnPoints.Length);
		Instantiate (EnemyPrefab, SpawnPoints [rd].position, Quaternion.identity, EnemyParent);
		StartCoroutine (Generate (3f));
	}
}
