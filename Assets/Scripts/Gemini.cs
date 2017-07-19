using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gemini : MonoBehaviour
{
	public GameObject otherPart;
	public float force;
	public float maxV = 500f;

	private Rigidbody2D rb;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate ()
	{
		Vector3 centiVec = transform.position - otherPart.transform.position;
		Vector3 forceNormal = Vector3.Cross (centiVec, Vector3.forward).normalized;
		if (rb.velocity.magnitude < maxV) {
			#if UNITY_EDITOR
			rb.AddForce (forceNormal * Input.GetAxis ("Horizontal") * force * -1f);
			#endif
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Stationary) {
					Camera cam = Camera.main;
					float height = 2f * cam.orthographicSize;
					float width = height * cam.aspect;
					Vector3 touchPos = cam.WorldToScreenPoint (touch.position);
					if (touchPos.x >= width / 2f) {
						rb.AddForce (forceNormal * force * -1f);
					} else {
						rb.AddForce (forceNormal * force);
					}
				}
			}
		}
	}
}
