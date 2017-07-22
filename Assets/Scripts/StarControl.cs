using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarControl : MonoBehaviour
{
	public float growSpeed = 1f;
	public float dimSpeed = 0.5f;

	private float intentistyRange;
	private Light l;
	private bool dimming = false;
	// Use this for initialization
	void Start ()
	{
		l = GetComponent<Light> ();
		intentistyRange = Random.Range (1.7f, 2.2f);
		l.range = Random.Range (1.25f, 3f);
		StartCoroutine (selfDestruct ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		// first light up
		if (!dimming && l.intensity <= intentistyRange)
			l.intensity += (Time.unscaledDeltaTime * growSpeed);
		
		if (dimming) {
			l.intensity -= (Time.unscaledDeltaTime * dimSpeed);
		}

		if (l.intensity <= 0f)
			DimWayOut ();
	}

	IEnumerator selfDestruct ()
	{
		yield return new WaitForSeconds (Random.Range (20f, 30f));
		dimming = true;
	}

	void DimWayOut ()
	{
		transform.position = EnvironmentGenerator.EG.RandomPosition ();
		dimming = false;
	}
}
