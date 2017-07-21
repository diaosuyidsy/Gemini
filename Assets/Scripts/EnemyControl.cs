﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
	public GameObject Player;
	public float MinDist = 0f;
	public float MoveSpeed = 50f;
	public int Health = 3;
	public GameObject ExplosionEffect;
	public GameObject[] ExplosionEffectPrefabs;
	public GameObject[] OrbsEffect;

	private Transform target;
	private Vector3 playerPos;
	private bool damageLock = false;
	private SpriteRenderer sr;
	private int score;

	void Start ()
	{
		if (Player == null) {
			Player = GameObject.Find ("Player");
		}
		sr = GetComponent<SpriteRenderer> ();
		init ();
	}

	void init ()
	{
		// Set up target
		int rd = Random.Range (1, 3);
		if (rd == 1) {
			target = Player.GetComponent<PlayerControl> ().Black.transform;
		} else {
			target = Player.GetComponent<PlayerControl> ().White.transform;
		}

		// Set up Health and Color
		float rand = Random.Range (1f, 100f);
		Color EnemyColor = new Color ();
		if (rand <= 80f) {
			Health = 1;
		} else if (rand <= 95f && rand > 80f) {
			Health = 2;
		} else {
			Health = 3;
		}
		// Set up Score
		score = Health;
		//Set Up VFX
		OrbsEffect [Health - 1].SetActive (true);
		ExplosionEffect = ExplosionEffectPrefabs [Health - 1];
		sr.color = EnemyColor;
		//Setup Speed
		switch (EnemiesSpawner.ES.difficulty) {
		case 1:
			MoveSpeed = randomAround (2.2f, 0.4f);
			break;
		case 2:
			MoveSpeed = randomAround (2.6f, 0.6f);
			break;
		case 3: 
			MoveSpeed = randomAround (3f, 0.8f);
			break;
		}
	}

	float randomAround (float target, float range)
	{
		return Random.Range (target - range, target + range);
	}

	// Update is called once per frame
	void Update ()
	{
		playerPos = target.position;
		Vector3 diff = playerPos - transform.position;
		diff.Normalize ();

		float rot_z = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, rot_z - 90);

		if (Vector3.Distance (transform.position, playerPos) >= MinDist) {
			transform.position += transform.up * MoveSpeed * Time.deltaTime;
		}
	}

	public void TakeDamage (int dmg)
	{
		if (!damageLock) {
			damageLock = true;
			Instantiate (ExplosionEffect, transform.position, Quaternion.identity);
			StartCoroutine (unlockDamage (0.2f));
			Health -= dmg;
			if (Health <= 0) {
				GameManager.GM.Tryscore (score);
				Destroy (gameObject);
			}
		}
	}

	IEnumerator unlockDamage (float time)
	{
		yield return new WaitForSeconds (time);
		damageLock = false;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			other.gameObject.SendMessageUpwards ("ApplyDamage", Health);
			if (!other.gameObject.GetComponentInParent<PlayerControl> ().isInvincible ())
				TakeDamage (Health + 10);
			else
				TakeDamage (1);
		}
	}
}
