using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	//	void OnTriggerEnter2D (Collider2D other)
	//	{
	//		if (other.gameObject.tag == "Player") {
	//			other.gameObject.SendMessageUpwards ("ApplyDamage", 100);
	//		}
	//	}
	//
	//	void OnCollisionEnter2D (Collision2D other)
	//	{
	//		if (other.gameObject.tag == "Player") {
	//			other.gameObject.SendMessageUpwards ("ApplyDamage", 100);
	//		}
	//	}

	void OnCollisionStay2D (Collision2D other)
	{
		Debug.Log ("Hello");
		if (other.gameObject.tag == "Player")
			other.gameObject.SendMessageUpwards ("setInvincible", false);
	}
}
