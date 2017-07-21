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

			if (Input.GetMouseButton (0)) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (hit.collider == null || (hit.collider != null && hit.collider.gameObject.tag != "Health")) {
					if (Input.mousePosition.x >= Screen.width / 2f) {
						rb.AddForce (forceNormal * force);
					} else {
						rb.AddForce (forceNormal * force * -1f);
					}
				}
			}
			#endif
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
					RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touch.position), Vector2.zero);
					if (hit.collider == null || (hit.collider != null && hit.collider.gameObject.tag != "Health")) {
						if (Input.mousePosition.x >= Screen.width / 2f) {
							rb.AddForce (forceNormal * force);
						} else {
							rb.AddForce (forceNormal * force * -1f);
						}
					}
				}
			}
		}
	}

	public void ApplyBurstForce (float forceNum)
	{
//		rb.velocity = rb.velocity.normalized * maxV;
		rb.AddForce (rb.velocity.normalized * forceNum, ForceMode2D.Impulse);
	}
}
