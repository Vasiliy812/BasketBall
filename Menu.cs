using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public Color32 DefaultColor;
	public Color32 EnterColor;
	public int MenuItemId;
	Menu_Misc menu_misc;
	Menu_BallsController menu_ballscontroller;
	Text text;
	AudioSource music;
	Fader fader;

	void Start () {		
		gameObject.GetComponent<Text>().color = DefaultColor;
		menu_misc = GameObject.Find("Menu_Misc").GetComponent<Menu_Misc>() as Menu_Misc;
		menu_ballscontroller = GameObject.Find("BallsController").GetComponent<Menu_BallsController>() as Menu_BallsController;
		text = gameObject.GetComponent<Text>() as Text;
		music = GameObject.Find("MUSIC").GetComponent<AudioSource>() as AudioSource;
		fader = GameObject.Find("Fader").GetComponent<Fader>() as Fader;
	}

	public void MouseEnter()
	{
		if (menu_misc.LastMenuID != MenuItemId)
		{			
			menu_ballscontroller.BallsMove(MenuItemId);
			menu_misc.LastMenuID = MenuItemId;
		}
		text.color = EnterColor;
	}

	public void MouseLeave()
	{
		text.color = DefaultColor;
	}	

	public void ExitClick()
	{
		music.Stop();
		menu_ballscontroller.BallsMove(4);
		fader.ExitGame();
	}

	public void GameClick()
	{
		music.Stop();
		menu_ballscontroller.BallsMove(4);
		fader.LoadGame();
	}
}
