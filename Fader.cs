using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour {

	public float FadeSpeed = 1f;
	public Color ColLight;
	public Color ColDark;

	RawImage ri;

	void Start () {
		ri = GetComponent<RawImage>() as RawImage;
		//Scene scene = SceneManager.GetActiveScene();
		GetComponent<RawImage>().enabled = true;
		MakeLight();
	}

	//0 - Dark, 1 - Light, 2 - Dark + LoadGame, 3 - Dark + Exit, 4 - Dark + Return to MainMenu, 5 - Dark + Reborn + Light
	IEnumerator CorFade(int Mode) 
	{		
		if (Mode != 1)
		{
			gameObject.GetComponent<RawImage>().enabled = true;
		}

		float t = 0f;
		while (t < 1.2f)
		{
			if (Mode != 1)
			{
				ri.color = Color.Lerp(ColLight, ColDark, t);
				t += FadeSpeed;
			}
			
			if (Mode == 1) 
			{
				ri.color = Color.Lerp(ColDark, ColLight, t);
				t += FadeSpeed;
			}
			
			yield return new WaitForEndOfFrame();
		}			

		if (Mode == 1)
		{
			gameObject.GetComponent<RawImage>().enabled = false;
		}

		if (Mode == 2)
		{
			SceneManager.LoadScene("Game01");
		}

		if (Mode == 3)
			Application.Quit();

		if (Mode == 4)
		{			
			SceneManager.LoadScene("Menu");
		}

		if (Mode == 5)
		{			
			GameObject.Find("BALL").GetComponent<BallController>().Respawn();
			StartCoroutine(CorFade(1));
		}
	}

	[ContextMenu("Dark")]
	public void MakeDark()
	{		
		StartCoroutine(CorFade(0));
	}

	[ContextMenu("Light")]
	public void MakeLight()
	{		
		StartCoroutine(CorFade(1));
	}
		
	public void LoadGame()
	{		
		StartCoroutine(CorFade(2));
	}

	public void ExitGame()
	{		
		StartCoroutine(CorFade(3));
	}

	public void LoadMenu()
	{		
		StartCoroutine(CorFade(4));
	}

	public void RespawnF()
	{		
		StartCoroutine(CorFade(5));
	}
}
