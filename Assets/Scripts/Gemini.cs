using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TutorialDesigner;
using UnityEngine.EventSystems;

public class Gemini : MonoBehaviour
{
	public GameObject otherPart;
	public float force;
	public float maxV = 500f;

	private Rigidbody2D rb;
	private float distance = 0.1f;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		distance = PlayerPrefs.GetFloat ("DuotDistance", 0.1f);
		if (gameObject.name == "White") {
			transform.localPosition = new Vector3 (distance, 0f);
		}
	}

	public void setDistanceLonger ()
	{
		float d = PlayerPrefs.GetFloat ("DuotDistance", 0.1f);
		PlayerPrefs.SetFloat ("DuotDistance", d + 0.175f);
	}

	void FixedUpdate ()
	{
		if (GameManager.GM.StartLock)
			return;
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
						EventManager.TriggerEvent ("TouchRight");
						rb.AddForce (forceNormal * force);
					} else {
						EventManager.TriggerEvent ("TouchLeft");
						rb.AddForce (forceNormal * force * -1f);
					}
				}
			}
			#endif
			foreach (Touch touch in Input.touches) {
				if (IsPointerOverUIObject ())
					return;
				if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
					RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touch.position), Vector2.zero);
					if (hit.collider == null || (hit.collider != null && hit.collider.gameObject.tag != "Health")) {
						if (touch.position.x >= Screen.width / 2f) {
							EventManager.TriggerEvent ("TouchRight");
							rb.AddForce (forceNormal * force);
						} else {
							EventManager.TriggerEvent ("TouchLeft");
							rb.AddForce (forceNormal * force * -1f);
						}
					}
				}
			}
		}
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			EventManager.TriggerEvent ("HitAnyWhere");
		}
	}

	public void ApplyBurstForce (float forceNum)
	{
//		rb.velocity = rb.velocity.normalized * maxV;
		rb.AddForce (rb.velocity.normalized * forceNum, ForceMode2D.Impulse);
	}

	private bool IsPointerOverUIObject ()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
		return results.Count > 0;
	}
}
