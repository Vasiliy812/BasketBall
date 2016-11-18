using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour {

	public static class RunManager
	{
		public static int SceneRuns = 0;
		public static int SceneTexturesID = -1;
	}

	public GameObject Text3D;
	public Text TimerText;
	public Text ScoreText;
	public GameObject FinalWhistle;
	public GameObject Applause;
	[Space(30)]
	public int Timer;
	public float Multiplier;
	public float MultiplierStep;
	public int Score = 0;
	public int ScoreBase = 50;
	public int HiScore;
	[HideInInspector]
	public bool LastWasGoal = false;
	Text timertext;
	public string [] GoalPhrases;
	int GoalsInline = 0;
	XML xml;
	GameObject ball;
	BallController ballcontroller;
	Text scoreresult;
	MenuSecondary menusecondary;
	AudioSource audiosource;
	RawImage trophyimage;
	RectTransform rectimage;

	[Space(30)]
	[Header("MaterialPacks")]
	public Material [] Field1;
	public Material [] Field2;
	public Material [] Ball1;
	public Material [] Ball2;
	public Material [] Board1;
	public Material [] Board2;

	void Start () {
		
		RunManager.SceneRuns++;
		RunManager.SceneTexturesID++;
		if (RunManager.SceneTexturesID > 1)
			RunManager.SceneTexturesID = 0;

		ApplyTextures(RunManager.SceneTexturesID);

		xml = GameObject.Find("XMLManager").GetComponent<XML>() as XML;

		Timer = int.Parse(xml.GetParam("params", "TimeSec"));
		MultiplierStep = float.Parse(xml.GetParam("params", "Multiplier"));
		HiScore = int.Parse(xml.GetParam("params", "HiScore"));
		timertext = TimerText.GetComponent<Text>() as Text;
		ball = GameObject.Find("BALL") as GameObject;
		ballcontroller = ball.GetComponent<BallController>() as BallController;
		scoreresult = GameObject.Find("ScoreResult").GetComponent<Text>() as Text;
		menusecondary = GetComponent<MenuSecondary>() as MenuSecondary;
		audiosource = GetComponent<AudioSource>() as AudioSource;
		trophyimage = GameObject.Find("Trophy").GetComponent<RawImage>() as RawImage;
		rectimage = GameObject.Find("Trophy").GetComponent<RectTransform>() as RectTransform;

		StartCoroutine("OnTimer");
		Multiplier = 1f;
	}

	public void Goal()
	{
		GoalsInline++;
		if (LastWasGoal)
			Multiplier += MultiplierStep;
		LastWasGoal = true;
		AddScore(ballcontroller.ScoresIfGoal);
		Instantiate(Applause, ball.transform.position, Quaternion.identity);
	}

	public void Miss()
	{
		Multiplier = 1f;
		LastWasGoal = false;
		GoalsInline = 0;
	}

	public void AddScore(int ScoreValue)
	{				
		GameObject txt = Instantiate(Text3D, ball.transform.position, Quaternion.identity) as GameObject;
		int ScoreToAdd = Mathf.RoundToInt(ScoreValue * Multiplier);
		Score += ScoreToAdd;
		string s1 = "";
		if (GoalsInline >= 2)
			s1 = GoalsInline.ToString() + " подряд !!! (" + ScoreValue + " x" + Multiplier.ToString() + " ) ";
		string s2 = GoalPhrases[Random.Range(0, GoalPhrases.Length - 1)] + "\n+" + ScoreToAdd.ToString();
		txt.GetComponentInChildren<TextMesh>().text = s1 + s2;
		ScoreText.GetComponent<Text>().text = "Очки: " + Score;
	}

	IEnumerator OnTimer()
	{
		Timer++;
		while (Timer > 0)
		{			
			Timer--;
			timertext.text = "Время: " + Timer;
			yield return new WaitForSeconds(1f);
		}
		// Окончание матча
		Instantiate(FinalWhistle, ball.transform.position, Quaternion.identity);
		menusecondary.InvertMenu(false);
		scoreresult.enabled = true;
		scoreresult.text = "Ваш результат: " + Score;

		if (Score > HiScore)
		{
			StartCoroutine("TrophyEffect");
			xml.SetParam("params", "HiScore", Score.ToString());
		}			

		audiosource.Play();

		SendAnalytics();
	}

	IEnumerator TrophyEffect()
	{		
		Vector3 Pos1 = new Vector3(0.01f, 0.01f, 0f);
		Vector3 Pos2 = new Vector3(1f, 1f, 0f);
		trophyimage.enabled = true;
		float t = 0f;
		float EffectSpeed = 0.01f;
		while (t < 1f)
		{						
			rectimage.localScale = Vector3.Lerp(Pos1, Pos2, t);
			t += EffectSpeed;
			yield return new WaitForEndOfFrame();
		}
	}

	public void SendAnalytics()
	{		
		Analytics.CustomEvent("BasketBallStatistics", new Dictionary<string, object>
		{
			{"BallThrowsCount", ballcontroller.ThrowsCount}
		});
	}

	public void ApplyTextures(int Index)
	{
		if (RunManager.SceneRuns <= 1)
			return;		
		GameObject.Find("Leather").GetComponent<MeshRenderer>().material = Ball1[Index];
		GameObject.Find("Seam").GetComponent<MeshRenderer>().material = Ball2[Index];

		if (Index == 0)
			GameObject.Find("WoodenPanel").GetComponent<MeshRenderer>().materials = Board1;

		if (Index == 1)
			GameObject.Find("WoodenPanel").GetComponent<MeshRenderer>().materials = Board2;

		GameObject.Find("Pole").GetComponent<MeshRenderer>().material = Field1[Index];
		GameObject.Find("Strip").GetComponent<MeshRenderer>().material = Field2[Index];
	}
		
}
