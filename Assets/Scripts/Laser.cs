﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

	LineRenderer lr;
	// Use this for initialization
	void Start ()
	{
		lr = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, GetComponent<Gemini> ().otherPart.transform.position);
	}
}
