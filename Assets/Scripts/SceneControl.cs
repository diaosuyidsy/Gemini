using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneControl : MonoBehaviour
{
	public Canvas m_Canvas;
	public GAui m_Play;

	public GameObject LevelExplainer;
	public float scaleSpeed;
	public GameObject levelParticle;

	private bool scaleup = false;

	public void GameStart ()
	{
		m_Play.MoveOut (GSui.eGUIMove.SelfAndChildren);
	}

	public void StartMoveIn ()
	{
		m_Play.MoveIn (GSui.eGUIMove.SelfAndChildren);
	}
	//
	//	void Update ()
	//	{
	//		if (LevelExplainer.transform.localScale.x <= 0f) {
	//			LevelExplainer.SetActive (false);
	//		} else {
	//			LevelExplainer.SetActive (true);
	//		}
	//		if (!scaleup && LevelExplainer.transform.localScale.x > 0f) {
	//			LevelExplainer.transform.localScale -= new Vector3 (Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);
	//		} else if (scaleup && LevelExplainer.transform.localScale.x <= 0.9f) {
	//			LevelExplainer.transform.localScale += new Vector3 (Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);
	//		} else if (LevelExplainer.transform.localScale.x > 1f) {
	//			LevelExplainer.transform.localScale = new Vector3 (1f, 1f);
	//		}
	//	}

	public void Down ()
	{
		if (GameManager.GM.StartLock) {
			LevelExplainer.SetActive (true);
			levelParticle.SetActive (false);
			PlayerPrefs.SetInt ("NeedTouch", 0);
		}
	}

	public void Up ()
	{
		if (GameManager.GM.StartLock) {
			LevelExplainer.SetActive (false);
		}
	}


}
