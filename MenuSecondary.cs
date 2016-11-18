using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSecondary : MonoBehaviour {

	public bool IsMenuActive = false;
	public bool IsMenuControlable = true;
	public GameObject CanvasSec;
	Fader fader;
	BallController ballcontroller;
	CursorController cursorcontroller;

	void Start()
	{
		fader = GameObject.Find("Fader").GetComponent<Fader>() as Fader;
		ballcontroller = GameObject.Find("BALL").GetComponent<BallController>() as BallController;
		cursorcontroller = GameObject.Find("CursorController").GetComponent<CursorController>() as CursorController;
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (ballcontroller.BallCaptured || !IsMenuControlable)
				return;
			InvertMenu(true);
		}
	}

	public void Replay()
	{
		InvertMenu(true);
		fader.LoadGame();
	}

	public void ReturnToMainMenu()
	{
		InvertMenu(true);
		fader.LoadMenu();
	}

	public void ActivateMenu(bool IsActive)
	{
		CanvasSec.SetActive(!CanvasSec.activeInHierarchy);
		IsMenuActive = !IsMenuActive;
		if (IsMenuActive)
		{
			Time.timeScale = 0f;
			cursorcontroller.ResetCursor();
		}
		else
		{
			Time.timeScale = 1f;
		}
		ballcontroller.IsTouchable = !IsMenuActive;
	}

	public void InvertMenu(bool IsControl)
	{
		IsMenuControlable = IsControl;
		ActivateMenu(!IsMenuActive);
	}
}
