using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGUIControl : MonoBehaviour
{
	
	private Animator _a;
	private int stage;
	// Use this for initialization
	void Start ()
	{
		stage = 0;
		_a = GetComponent<Animator> ();
	}

	public void TakeDamage (int dmg)
	{
		for (int i = 0; i < dmg; i++) {
			_a.Play ("Health", 0, (1f / 6f) * (++stage));
		}
		if (dmg < 0) {
			_a.Play ("Health", 0, (1f / 6f) * (--stage));
		}
	}
	// Update is called once per frame
	void Update ()
	{
		
	}
}
