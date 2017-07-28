using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TutorialDesigner;

public class EnemyControl : MonoBehaviour
{
	public GameObject Player;
	public float MinDist = 0f;
	public float MoveSpeed = 50f;
	public int Health = 3;
	public GameObject ExplosionEffect;
	public GameObject[] ExplosionEffectPrefabs;
	public GameObject[] OrbsEffect;
	public GameObject ForstRingEffect;
	public GameObject DestructionRightEffect;

	private Transform target;
	private Vector3 playerPos;
	private bool damageLock = false;
	private int score;
	private float epochNum;
	private int maxHealth;
	private bool freezee = false;
	public int DemandHealth = 0;

	void Start ()
	{
		if (Player == null) {
			Player = GameObject.Find ("Player");
		}
		epochNum = (float)EnemiesSpawner.ES.epoch;
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

		// Set up Health
		float rand = Random.Range (1f, 100f);
		if (rand <= 100f - Mathf.Min (epochNum * 2, 20f)) {
			Health = 1;
		} else if (rand <= 100 - 0.25f * Mathf.Min (epochNum * 2, 20f) && rand > 100f - Mathf.Min (epochNum * 2, 20f)) {
			Health = 2;
		} else {
			Health = 3;
		}
		if (DemandHealth != 0)
			Health = DemandHealth;
		maxHealth = Health;
		// Set up Score
		score = Health;
		//Set Up VFX
		OrbsEffect [Health - 1].SetActive (true);
		ExplosionEffect = ExplosionEffectPrefabs [Health - 1];
		//Setup Speed
		switch (EnemiesSpawner.ES.difficulty) {
		case 1:
			MoveSpeed = randomAround (2.4f, 0.4f);
			break;
		case 2:
			MoveSpeed = randomAround (2.8f, 0.4f);
			break;
		case 3: 
			MoveSpeed = randomAround (3f, 0.4f);
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
			if (!freezee)
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
				EventManager.TriggerEvent ("HitEnemy");
				GameManager.GM.Tryscore (score);
				ProduceRing ();
				Destroy (gameObject);
			}
		}
	}

	void OnWillRenderObject ()
	{
		if (Camera.current == Camera.main) {
			SceneControl.SC.TDEnemyHere = gameObject;
			EventManager.TriggerEvent ("EnemyEnter");
		}
		if (maxHealth == 2) {
			SceneControl.SC.TDEnemyHere = gameObject;
			EventManager.TriggerEvent ("SpecialEnemyEnter");
		}
	}

	public void freeze (float time)
	{
		StartCoroutine (froze (time));
	}

	IEnumerator froze (float time)
	{
		freezee = true;
		yield return new WaitForSeconds (time);
		freezee = false;
	}

	void ProduceRing ()
	{
		int frost = PlayerPrefs.GetInt ("FrostRing", 0);
		int dest = PlayerPrefs.GetInt ("DestructionRing", 0);
		if (maxHealth == 2 && frost >= 1) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, 8f * frost);
			foreach (Collider2D collider in colliders) {
				if (collider.gameObject.tag == "Enemy") {
					collider.gameObject.SendMessage ("freeze", 3f);
				}
			}
			Instantiate (ForstRingEffect, transform.position, Quaternion.identity);
		} else if (maxHealth == 3 && dest >= 1) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, 4f * dest);
			foreach (Collider2D collider in colliders) {
				if (collider.gameObject.tag == "Enemy") {
					collider.gameObject.SendMessage ("TakeDamage", 1);
				}
			}
			Instantiate (DestructionRightEffect, transform.position, Quaternion.identity);
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
