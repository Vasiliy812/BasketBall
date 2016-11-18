using UnityEngine;
using System.Collections;

public class Menu_BallsController : MonoBehaviour {

	public GameObject [] Balls;
	public GameObject [] Snds;

	public float SideBallsShift = 200f;
	public float HeightBallsShift1 = 70f;
	public float HeightBallsShift2 = -10f;
	public float HeightBallsShift3 = -70f;

	public float BallsInOutSpeed = 0.01f;
	public float BallsInOutDelay = 0.05f;

	//LeftBall positions
	Vector3 OutPosLeft;
	Vector3 InPosLeft1;
	Vector3 InPosLeft2;
	Vector3 InPosLeft3;

	//RightBall positions
	Vector3 OutPosRight;
	Vector3 InPosRight1;
	Vector3 InPosRight2;
	Vector3 InPosRight3;


	void Start () {		
		//LeftBall positions
		OutPosLeft = Balls[0].transform.position;
		InPosLeft1 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 - SideBallsShift, Screen.height / 2 + HeightBallsShift1, 1f));
		InPosLeft2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 - SideBallsShift, Screen.height / 2 + HeightBallsShift2, 1f));
		InPosLeft3 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 - SideBallsShift, Screen.height / 2 + HeightBallsShift3, 1f));

		//RightBall positions
		OutPosRight = Balls[1].transform.position;
		InPosRight1 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 + SideBallsShift, Screen.height / 2 + HeightBallsShift1, 1f));
		InPosRight2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 + SideBallsShift, Screen.height / 2 + HeightBallsShift2, 1f));
		InPosRight3 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 + SideBallsShift, Screen.height / 2 + HeightBallsShift3, 1f));
	}

	IEnumerator CorBallsMove(Vector3 L2, Vector3 R2)
	{
		float t = 0f;

		Vector3 L1 = Balls[0].transform.position;
		Vector3 R1 = Balls[1].transform.position;

		while (t < 1f)
		{			
			t += BallsInOutSpeed;
			Balls[0].transform.position = Vector3.Lerp(L1, L2, t);
			Balls[1].transform.position = Vector3.Lerp(R1, R2, t);
			yield return new WaitForSeconds(BallsInOutDelay);
		}
	}

	public void BallsMove(int Pos)
	{
		if (Pos >= 1 && Pos <= 3)
		{
			Instantiate(Snds[0], Vector3.zero, Quaternion.identity);
		}
		
		switch (Pos)
		{
			case 1:
				StartCoroutine(CorBallsMove(InPosLeft1, InPosRight1));
				break;
			case 2:
				StartCoroutine(CorBallsMove(InPosLeft2, InPosRight2));
				break;
			case 3:
				StartCoroutine(CorBallsMove(InPosLeft3, InPosRight3));
				break;
			case 4:
				Instantiate(Snds[1], Vector3.zero, Quaternion.identity);
				StartCoroutine(CorBallsMove(OutPosLeft, OutPosRight));
				break;
		}
	}		
}
