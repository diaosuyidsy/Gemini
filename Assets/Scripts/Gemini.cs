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
		if (rb.velocity.magnitude > maxV) {
			Vector2 v = rb.velocity.normalized;
			rb.velocity = v * maxV;
		}
		if (rb.velocity.magnitude < maxV && GetComponentInParent<PlayerControl> ().Main != null) {
			#if UNITY_EDITOR
			rb.AddForce (forceNormal * Input.GetAxis ("Horizontal") * force);
			#endif
			if (Input.GetMouseButton (0)) {

				if (Input.mousePosition.x >= Screen.width / 2f) {
					rb.AddForce (forceNormal * force);
				} else {
					rb.AddForce (forceNormal * force * -1f);
				}
			}
		}
	}
}
