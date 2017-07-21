using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrbControl : MonoBehaviour
{
	public float MoveSpeed = 0.1f;
	public float selfDestructTime = 10f;
	public float RotateSpeed = 5f;
	public GameObject ExplosionEffectPrefab;

	private bool isQuitting = false;

	void Start ()
	{
		StartCoroutine (selfDestruct (selfDestructTime));
	}

	IEnumerator selfDestruct (float time)
	{
		yield return new WaitForSeconds (time);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.MoveTowards (transform.position, Vector3.zero, MoveSpeed * Time.deltaTime);
		transform.RotateAround (Vector3.zero, Vector3.forward, RotateSpeed * Time.deltaTime);
	}

	void OnDestroy ()
	{
		if (!isQuitting)
			Instantiate (ExplosionEffectPrefab, transform.position, Quaternion.identity);
	}

	void OnApplicationQuit ()
	{
		isQuitting = true;
	}
}
