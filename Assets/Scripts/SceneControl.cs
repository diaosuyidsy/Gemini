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

	public void GameStart ()
	{
		m_Play.MoveOut (GSui.eGUIMove.SelfAndChildren);
	}

	public void StartMoveIn ()
	{
		m_Play.MoveIn (GSui.eGUIMove.SelfAndChildren);
	}

	public void Down ()
	{
		LevelExplainer.SetActive (true);
		levelParticle.SetActive (false);
		PlayerPrefs.SetInt ("NeedTouch", 0);

	}

	public void Up ()
	{
		LevelExplainer.SetActive (false);
	}


}
