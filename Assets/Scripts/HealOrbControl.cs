﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrbControl : MonoBehaviour
{
	public float MoveSpeed = 0.1f;
	public float selfDestructTime = 10f;
	public float RotateSpeed = 5f;
	public GameObject ExplosionEffectPrefab;

	//	private Vector3 PlayerPosition;
	//	private Vector3 MovingPoint;


	void Start ()
	{
//		PlayerPosition = GameObject.Find ("Player").GetComponent<PlayerControl> ().Main.transform.position;
//		MovingPoint = (PlayerPosition - transform.position).normalized;
		StartCoroutine (selfDestruct (selfDestructTime));
	}

	IEnumerator selfDestruct (float time)
	{
		yield return new WaitForSeconds (time);
		Instantiate (ExplosionEffectPrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.MoveTowards (transform.position, Vector3.zero, MoveSpeed * Time.deltaTime);
		transform.RotateAround (Vector3.zero, Vector3.forward, RotateSpeed * Time.deltaTime);
//		transform.Translate (MovingPoint * Time.deltaTime * MoveSpeed);
	}

	public void Absorb ()
	{
		Instantiate (ExplosionEffectPrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			other.gameObject.SendMessageUpwards ("ApplyDamage", -1 * PlayerPrefs.GetInt ("HealAmount", 1));
			Absorb ();
		}
		if (other.gameObject.tag == "DZ") {
			Absorb ();
		}
	}
}
