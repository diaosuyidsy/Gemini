using System;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
	public GameObject target;
	public float damping = 1;
	public float lookAheadFactor = 3;
	public float lookAheadReturnSpeed = 0.5f;
	public float lookAheadMoveThreshold = 0.1f;

	// private variables
	float m_OffsetZ;
	Vector3 m_LastTargetPosition;
	Vector3 m_CurrentVelocity;
	Vector3 m_LookAheadPos;
	Vector3 targetPos;

	// Update is called once per frame
	private void Update ()
	{
//		if (target == null) {
//			GameObject Player = GameObject.FindGameObjectWithTag ("Player");
//			targetPos = (Player.GetComponent<PlayerControl> ().Black.transform.position + Player.GetComponent<PlayerControl> ().White.transform.position) / 2;
//		} else {
//			targetPos = target.transform.position;
//		}

		if (target.GetComponent<PlayerControl> ().Main != null) {
			targetPos = target.GetComponent<PlayerControl> ().Main.transform.position;
		} else {
			targetPos = (target.GetComponent<PlayerControl> ().Black.transform.position + target.GetComponent<PlayerControl> ().White.transform.position) / 2;

		}

		// only update lookahead pos if accelerating or changed direction
		float xMoveDelta = (targetPos - m_LastTargetPosition).x;

		bool updateLookAheadTarget = Mathf.Abs (xMoveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadTarget) {
			m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign (xMoveDelta);
		} else {
			m_LookAheadPos = Vector3.MoveTowards (m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
		}

		Vector3 aheadTargetPos = targetPos + m_LookAheadPos + Vector3.forward * m_OffsetZ;
		Vector3 newPos = Vector3.SmoothDamp (transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

		newPos = new Vector3 (newPos.x, newPos.y, transform.position.z);
		transform.position = newPos;

		m_LastTargetPosition = targetPos;
	}
	//
	//	public void setTarget (Transform target)
	//	{
	//		this.target = target;
	//		m_LastTargetPosition = target.position;
	//		m_OffsetZ = (transform.position - target.position).z;
	//		transform.parent = null;
	//	}


}

