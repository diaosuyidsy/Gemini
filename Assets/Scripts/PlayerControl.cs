using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
	public GameObject Black;
	public GameObject White;
	public GameObject Main;
	public int Health = 6;
	public int MaxHealth = 6;
	public GameObject HealthO;
	public bool StartLock = true;
	public GameObject CirclePrefab;
	public GameObject[] InvincibleEffect;

	private int turn = 0;
	private bool invincible = false;
	private bool isInivincibleBuffOn = false;
	// 0 is Black freeze, 1 is free all, 2 is White freeze, 3 is free all;

	// Use this for initialization
	void Start ()
	{
		Black.GetComponent<Rigidbody2D > ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
		Main = Black;
	}
	
	// Update is called once per frame
	void Update ()
	{
		#if UNITY_EDITOR
		if (!StartLock) {
			if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject ()) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (hit.collider != null && hit.collider.gameObject.tag == "Health" && PlayerPrefs.GetInt ("DuotsEnable", 0) == 1) {
					// Some VFX
					GameObject a = Instantiate (CirclePrefab, HealthO.transform.position, Quaternion.identity, HealthO.transform);
					a.transform.localPosition = new Vector3 (-0.29f, -0.14f);
					bool invincibleAble = PlayerPrefs.GetInt ("InvicibleAble", 0) == 1;
					switch (turn) {
					case 0:
						Main = null;
						if (invincibleAble) {
							setInvincible (true);
						}
							
						Black.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
						turn = (turn + 1) % 4;
						break;
					case 1:
						Main = White;
						setInvincible (false);
						White.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
						Black.GetComponent<Gemini> ().ApplyBurstForce (5000f);
						turn = (turn + 1) % 4;
						break;
					case 2:
						Main = null;
						if (invincibleAble) {
							setInvincible (true);
						}
						White.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
						turn = (turn + 1) % 4;
						break;
					case 3:
						Main = Black;
						setInvincible (false);
						Black.GetComponent<Rigidbody2D > ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
						White.GetComponent<Gemini> ().ApplyBurstForce (5000f);
						turn = (turn + 1) % 4;
						break;
					}

				}
			}
		}
		#endif
		if (!StartLock) {
			foreach (Touch touch in Input.touches) {
				if (IsPointerOverUIObject ())
					return;
				if (touch.phase == TouchPhase.Ended) {
					RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touch.position), Vector2.zero);
					if (hit.collider != null && hit.collider.gameObject.tag == "Health" && PlayerPrefs.GetInt ("DuotsEnable", 0) == 1) {
						// Some VFX
						GameObject a = Instantiate (CirclePrefab, HealthO.transform.position, Quaternion.identity, HealthO.transform);
						a.transform.localPosition = new Vector3 (-0.29f, -0.14f);
						bool invincibleAble = PlayerPrefs.GetInt ("InvicibleAble", 0) == 1;
						switch (turn) {
						case 0:
							Main = null;
							if (invincibleAble) {
								setInvincible (true);
							}

							Black.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
							turn = (turn + 1) % 4;
							break;
						case 1:
							Main = White;
							setInvincible (false);
							White.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
							Black.GetComponent<Gemini> ().ApplyBurstForce (5000f);
							turn = (turn + 1) % 4;
							break;
						case 2:
							Main = null;
							if (invincibleAble) {
								setInvincible (true);
							}
							White.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
							turn = (turn + 1) % 4;
							break;
						case 3:
							Main = Black;
							setInvincible (false);
							Black.GetComponent<Rigidbody2D > ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
							White.GetComponent<Gemini> ().ApplyBurstForce (5000f);
							turn = (turn + 1) % 4;
							break;
						}
					}
				}
			}
		}
	}

	void FixedUpdate ()
	{
		CheckLaserBeam ();
	}

	void CheckLaserBeam ()
	{
		RaycastHit2D[] hits = Physics2D.LinecastAll (Black.transform.position, White.transform.position);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider != null && hit.collider.gameObject.tag == "Enemy") {
				if (hit.collider.gameObject.GetComponent<EnemyControl> ().Health == 4) {
					setInvincibleTime ();
					Destroy (hit.collider.gameObject);
					return;
				}
				hit.collider.gameObject.SendMessage ("TakeDamage", 1);
			} else if (hit.collider != null && hit.collider.gameObject.tag == "HealOrb") {
				ApplyDamage (-1 * PlayerPrefs.GetInt ("HealAmount", 1));
				hit.collider.gameObject.SendMessage ("Absorb");
			}
				
		}
	}

	public void ApplyDamage (int dmg)
	{
		if (!invincible || dmg > 50 || dmg < 0) {
			Health -= dmg;

			// Apply Damage to GUi
			if (Health <= MaxHealth)
				HealthO.GetComponent<HealthGUIControl> ().TakeDamage (dmg);
			else {
				Health = MaxHealth;
				HealthO.GetComponent<HealthGUIControl> ().ResetGUI ();
			}

			if (Health <= 0f) {
				GameManager.GM.Restart ();
			}
		}
	}

	public bool isInvincible ()
	{
		return invincible;
	}

	public void setInvincible (bool yes)
	{
		if (!isInivincibleBuffOn) {
			invincible = yes;
			foreach (GameObject invicibleeffe in InvincibleEffect) {
				invicibleeffe.SetActive (yes);
			}
		}

	}

	private bool IsPointerOverUIObject ()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	public void setInvincibleTime ()
	{
		StopCoroutine ("setInvincibleForATime");
		StartCoroutine ("setInvincibleForATime");
	}

	IEnumerator setInvincibleForATime ()
	{
		setInvincible (true);
		isInivincibleBuffOn = true;
		yield return new WaitForSeconds (3f * PlayerPrefs.GetInt ("invincibleOrb", 0));
		isInivincibleBuffOn = false;
		setInvincible (false);

	}
}
