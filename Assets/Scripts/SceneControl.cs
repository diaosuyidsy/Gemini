using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TutorialDesigner;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
	public static SceneControl SC;
	public Canvas m_Canvas;
	public GAui m_Play;
	public GAui m_PlayeImage;
	public GameObject m_Combo;
	public GameObject LevelExplainer;
	public float scaleSpeed;
	public GameObject levelParticle;
	public GameObject TDEnemyHereDialogue;
	public GameObject TDEnemyHere;
	public GameObject TDDialogue3;
	public GameObject TDDialogue3Image;
	public GameObject TDDialogue4;
	public GameObject TDDIalogue4Image;
	public GameObject TDDialogue5;
	public GameObject TDDialogue6;
	public Text TDDialogue6Text;
	public GameObject TDDialogue7;
	public GameObject TDDialogue8;
	public Text TDDialogue8Text;

	private bool wasPause = false;
	private int lastComboNum = 0;
	//	private IEnumerator comboco;

	void Awake ()
	{
		SC = this;
	}

	void Start ()
	{
//		comboco = comboSelfMoveOut (3);
	}

	public void GameStart ()
	{
		m_Play.MoveOut (GSui.eGUIMove.SelfAndChildren);
		m_PlayeImage.MoveOut (GSui.eGUIMove.SelfAndChildren);
	}

	public void StartMoveIn ()
	{
		m_Play.MoveIn (GSui.eGUIMove.SelfAndChildren);
	}

	public void Combo (int comboNum)
	{
//		StopCoroutine (comboco);
		if (lastComboNum == comboNum)
			return;
		m_Combo.GetComponent<Text> ().text = "x" + comboNum.ToString ();
		switch (comboNum) {
		case -1:
		case 0:
		case 1:
			m_Combo.GetComponent<Text> ().text = "";
			m_Combo.GetComponent<GAui> ().MoveOut (GSui.eGUIMove.SelfAndChildren);
			return;
		case 2:
			m_Combo.GetComponent<Text> ().color = Color.white;
			m_Combo.GetComponent<Text> ().fontSize = 40;
			break;
		case 3:
			m_Combo.GetComponent<Text> ().color = new Color (255f / 255f, 235f / 255f, 0f);
			m_Combo.GetComponent<Text> ().fontSize = 50;
			break;
		case 4:
			m_Combo.GetComponent<Text> ().color = new Color (255f / 255f, 147f / 255f, 0f);
			m_Combo.GetComponent<Text> ().fontSize = 60;
			break;
		case 5:
			m_Combo.GetComponent<Text> ().color = new Color (255f / 255f, 75f / 255f, 0f);
			m_Combo.GetComponent<Text> ().fontSize = 70;
			break;
		}
		m_Combo.GetComponent<GAui> ().MoveIn (GSui.eGUIMove.SelfAndChildren);
//		StartCoroutine (comboco);
		lastComboNum = comboNum;
	}

	IEnumerator comboSelfMoveOut (float time)
	{
		yield return new WaitForSeconds (time);
		m_Combo.GetComponent<GAui> ().MoveOut (GSui.eGUIMove.SelfAndChildren);
	}

	public void Down ()
	{
		EventManager.TriggerEvent ("TouchLevel");
		LevelExplainer.SetActive (true);
		levelParticle.SetActive (false);
		PlayerPrefs.SetInt ("NeedTouch", 0);
		if (Time.timeScale == 0f)
			wasPause = true;
		Time.timeScale = 0f;
	}

	public void Up ()
	{
		if (!wasPause)
			Time.timeScale = 1f;
		wasPause = false;
		LevelExplainer.SetActive (false);
		EventManager.TriggerEvent ("ReleaseLevel");
	}

	public void EnemyEntry ()
	{
		Time.timeScale = 0f;
		if (TDEnemyHere != null) {
			TDEnemyHereDialogue.SetActive (true);
			TDEnemyHereDialogue.transform.position = Camera.main.WorldToScreenPoint (TDEnemyHere.transform.position);
		}
	}

	public void RotateTheOtherBall ()
	{
		TDEnemyHereDialogue.SetActive (false);
		TDDialogue3.SetActive (true);
		TDDialogue3.transform.position = Camera.main.WorldToScreenPoint (GameObject.Find ("Player").GetComponent<PlayerControl> ().White.transform.position);
		TDDialogue3Image.transform.position = Camera.main.WorldToScreenPoint (GameObject.Find ("Player").GetComponent<PlayerControl> ().Black.transform.position);
	}

	public void UseTheGap ()
	{
		TDDialogue3.SetActive (false);
		TDDialogue4.SetActive (true);
		TDDialogue4.transform.position = Camera.main.WorldToScreenPoint ((GameObject.Find ("Player").GetComponent<PlayerControl> ().White.transform.position +
		GameObject.Find ("Player").GetComponent<PlayerControl> ().Black.transform.position) / 2f);
		
		Vector3 diff = Vector3.zero - Camera.main.ScreenToWorldPoint (TDDIalogue4Image.transform.position);
		diff.Normalize ();

		float rot_z = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;
		TDDIalogue4Image.transform.rotation = Quaternion.Euler (0f, 0f, rot_z - 90);
	}

	public void EliminateEnemy ()
	{
		Time.timeScale = 1f;
		TDDialogue4.SetActive (false);
	}

	public void IntroHealth ()
	{
		Time.timeScale = 0f;
		TDDialogue5.SetActive (true);
	}

	public void IntroLevel ()
	{
		TDDialogue5.SetActive (false);
		TDDialogue6.SetActive (true);
	}

	public void HowLevelUP ()
	{
		TDDialogue6Text.text = "Eliminating an enemy will grant you experience to level UP";
	}

	public void TouchLevel ()
	{
		TDDialogue6Text.text = "Hold on the Level Icon to display more Level Information";
	}

	public void TouchedLevel ()
	{
		TDDialogue6.SetActive (false);

	}

	public void GoodToGo (bool open)
	{
		Time.timeScale = 1f;
		TDDialogue7.SetActive (open);
	}

	public void SpecialEnemyEnter ()
	{
		Time.timeScale = 0f;
		if (TDEnemyHere != null) {
			TDDialogue8.SetActive (true);
			TDDialogue8.transform.position = Camera.main.WorldToScreenPoint (TDEnemyHere.transform.position);
			if (Camera.main.WorldToScreenPoint (TDEnemyHere.transform.position).x > Screen.width / 2f) {
				TDDialogue8Text.transform.localPosition = new Vector3 (-130, -26, 0);
			}
		}
	}

	public void DestroOrbIntro ()
	{
		TDDialogue8Text.text = "Destruction Orb(Purple One) takes 3 hits to eliminate";
	}

	public void DoneIntroOrbs ()
	{
		TDDialogue8.SetActive (false);
		Time.timeScale = 1f;
	}


}
