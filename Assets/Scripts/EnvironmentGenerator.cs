using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
	public GameObject StarPrefab;
	public int StarNum = 50;
	public static EnvironmentGenerator EG;
	public GameObject StarHolder;

	void Awake ()
	{
		EG = this;
	}
	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < StarNum; i++) {
			GenerateNewStar ();
		}
	}

	public void GenerateNewStar ()
	{
		Instantiate (StarPrefab, RandomPosition (), Quaternion.identity, StarHolder.transform);
	}

	Vector3 RandomPosition ()
	{
		Vector3 up = EnemiesSpawner.ES.SpawnPoints [1].position;
		Vector3 left = EnemiesSpawner.ES.SpawnPoints [3].position;
		Vector3 down = EnemiesSpawner.ES.SpawnPoints [5].position;
		Vector3 right = EnemiesSpawner.ES.SpawnPoints [7].position;
		float RandomX = Random.Range (left.x, right.x);
		float RandomY = Random.Range (down.y, up.y);
		return new Vector3 (RandomX, RandomY, Random.Range (99.9f, 99.99f));
	}
}
