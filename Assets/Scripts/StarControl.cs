using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarControl : MonoBehaviour
{
	public float growSpeed = 1f;

	private float centralRange;
	private Light l;
	private bool growing;
	// Use this for initialization
	void Start ()
	{
		l = GetComponent<Light> ();
		growing = true;
		centralRange = Random.Range (2.3f, 3f);
		StartCoroutine (selfDestruct ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		l.range += ((growing ? 1f : -1f) * Time.unscaledDeltaTime * growSpeed);
		if (l.range >= centralRange + 1f)
			growing = false;
		if (l.range <= centralRange - 1f)
			growing = true;
	}

	IEnumerator selfDestruct ()
	{
		yield return new WaitForSeconds (Random.Range (50f, 80f));
		EnvironmentGenerator.EG.GenerateNewStar ();
		Destroy (gameObject);
	}
}
