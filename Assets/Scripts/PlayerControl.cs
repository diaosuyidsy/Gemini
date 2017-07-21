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

	private int turn = 0;
	private float touchTimer = 0f;
	private bool invincible = false;
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
		if (!StartLock) {
			if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject ()) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (hit.collider != null && hit.collider.gameObject.tag == "Health") {
					// Some VFX
					GameObject a = Instantiate (CirclePrefab, HealthO.transform.position, Quaternion.identity, HealthO.transform);
					a.transform.localPosition = new Vector3 (-0.29f, -0.14f);
					if (touchTimer <= 0.2f) {
						switch (turn) {
						case 0:
							Main = null;
							invincible = true;
							Black.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
							turn = (turn + 1) % 4;
							break;
						case 1:
							Main = White;
							invincible = false;
							White.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
							Black.GetComponent<Gemini> ().ApplyBurstForce (5000f);
							turn = (turn + 1) % 4;
							break;
						case 2:
							Main = null;
							invincible = true;
							White.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
							turn = (turn + 1) % 4;
							break;
						case 3:
							Main = Black;
							invincible = false;
							Black.GetComponent<Rigidbody2D > ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
							White.GetComponent<Gemini> ().ApplyBurstForce (5000f);
							turn = (turn + 1) % 4;
							break;
						}
					}
					touchTimer = 0f;
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
			if (hit.collider != null && hit.collider.gameObject.tag == "Enemy")
				hit.collider.gameObject.SendMessage ("TakeDamage", 1);
			else if (hit.collider != null && hit.collider.gameObject.tag == "HealOrb") {
				ApplyDamage (-3);
				hit.collider.gameObject.SendMessage ("Absorb");
			}
				
		}
	}

	public void ApplyDamage (int dmg)
	{
		if (!invincible || dmg > 50) {
			Health -= dmg;

			// Apply Damage to GUi
			if (Health <= MaxHealth)
				HealthO.GetComponent<HealthGUIControl> ().TakeDamage (dmg);
			else {
				Health = MaxHealth;
				HealthO.GetComponent<HealthGUIControl> ().ResetGUI ();
			}

			if (Health <= 0f) {
				Debug.Log ("Dead");
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
		invincible = yes;
	}
}
