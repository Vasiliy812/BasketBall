using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {

	public GameObject Line;
	public GameObject RingCenter;
	LineRenderer linerenderer;
	Rigidbody rigidbody;
	CursorController cursorcontroller;
	BoxCollider boxcollider;
	Fader fader;
	public bool BallCaptured = false;
	public float CamDist = 4.3f;
	public float ThrowPower = 1f;
	public float RespawnTime = 3f;
	bool IsRespawning = false;
	public bool IsTouchable = true;

	public GameObject FloorHitSnd;
	public float FloorHitSndMult = 0.2f;
	public GameObject WPHitSnd;
	public GameObject RingHitSnd;
	public GameObject GridHitSnd;
	public GameObject MetalHitSnd;
	int GoalCounter1 = 0;
	int GoalCounter2 = 0;
	bool WasGoal = false;
	GameManager gamemanager;

	[HideInInspector]
	public int ScoresIfGoal = 0;

	Component halo;

	public Text ThrowData;
	public int ThrowsCount = 0;

	void Start()
	{
		linerenderer = Line.GetComponent<LineRenderer>() as LineRenderer;
		rigidbody = GetComponent<Rigidbody>() as Rigidbody;
		cursorcontroller = GameObject.Find("CursorController").GetComponent<CursorController>() as CursorController;
		boxcollider = GameObject.Find("SpawnZone").GetComponent<BoxCollider>() as BoxCollider;
		halo = GetComponent("Halo");
		gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>() as GameManager;
		fader = GameObject.Find("Fader").GetComponent<Fader>();
		ScoresIfGoal = GetBallDistanceForScores();
		Respawn();
	}

	void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (BallCaptured)
			{				
				rigidbody.isKinematic = false;
				rigidbody.AddForce((linerenderer.GetPosition(0) - linerenderer.GetPosition(1)) * ThrowPower, ForceMode.Impulse);
				linerenderer.enabled = false;
				BallCaptured = false;
				cursorcontroller.SetNewCursor(0);
				ThrowData.enabled = false;
				cursorcontroller.ResetCursor();
				IsTouchable = false;
				ThrowsCount++;
			}				
		}

		if (BallCaptured)
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CamDist));
			Vector3 NewPos = new Vector3(transform.position.x, pos.y, pos.z);
			if (linerenderer.GetPosition(1) != NewPos)
			{
				ThrowData.text = GetThrowData(NewPos);
				ThrowData.GetComponent<RectTransform>().transform.position = Input.mousePosition;
			}
			linerenderer.SetPosition(1, NewPos);
		}
		// если мяч улетел вниз
		if (transform.position.y < 0f && !IsRespawning)
		{
			IsRespawning = true;
			Invoke("RespawnFader", RespawnTime);
		}
	}

	void OnMouseDown()
	{
		if (!IsTouchable)
			return;
		BallCaptured = true;
		linerenderer.enabled = true;
		linerenderer.SetPosition(0, gameObject.transform.position);
		cursorcontroller.SetNewCursor(1);
		ThrowData.enabled = true;
	}

	void OnCollisionEnter(Collision c)
	{	// если мяч коснулся пола
		if (c.collider.gameObject.CompareTag("Floor"))
		{
			if (!IsRespawning)
			{
				IsRespawning = true;
				Invoke("RespawnFader", RespawnTime);
			}
			GameObject fs = Instantiate(FloorHitSnd, c.contacts[0].point, Quaternion.identity) as GameObject;
			fs.GetComponent<AudioSource>().volume = rigidbody.velocity.magnitude * FloorHitSndMult;
		}

		if (c.collider.gameObject.CompareTag("WoodenPanel"))
		{
			Instantiate(WPHitSnd, c.contacts[0].point, Quaternion.identity);
		}

		if (c.collider.gameObject.CompareTag("Ring"))
		{
			Instantiate(RingHitSnd, c.contacts[0].point, Quaternion.identity);
		}

		if (c.collider.gameObject.CompareTag("Metal"))
		{
			Instantiate(MetalHitSnd, c.contacts[0].point, Quaternion.identity);
		}

		if (c.collider.CompareTag("Floor"))
		{
			GoalCounter1 = 0;
			GoalCounter2 = 0;
			if (!WasGoal)
				gamemanager.Miss();
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.CompareTag("Grid"))
		{
			Instantiate(GridHitSnd, gameObject.transform.position, Quaternion.identity);
		}

		if (GoalCounter2 == 0 && c.CompareTag("GT1"))
			GoalCounter1++;

		if (GoalCounter1 > 0 && c.CompareTag("GT2"))
			GoalCounter2++;		

		if (GoalCounter1 > 0 && GoalCounter2 > 0 )
		{
			GoalCounter1 = 0;
			GoalCounter2 = 0;
			gamemanager.Goal();
			WasGoal = true;
		}
	}

	void OnMouseEnter()
	{		
		if (!IsTouchable)
			return;
		halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
		cursorcontroller.SetNewCursor(0);
	}

	void OnMouseExit()
	{		
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
		if (!BallCaptured)
			cursorcontroller.ResetCursor();
	}

	public void Respawn()
	{		
		rigidbody.isKinematic = true;
		Vector3 newpos = new Vector3(Random.Range(boxcollider.bounds.min.x, boxcollider.bounds.max.x), Random.Range(boxcollider.bounds.min.y, boxcollider.bounds.max.y), Random.Range(boxcollider.bounds.min.z, boxcollider.bounds.max.z));
		transform.position = newpos;
		IsRespawning = false;
		WasGoal = false;
		IsTouchable = true;
		ScoresIfGoal = GetBallDistanceForScores();
	}

	void RespawnFader()
	{		
		fader.RespawnF();
	}

	string GetThrowData(Vector3 np)
	{
		float BallDist = Vector3.Distance(RingCenter.transform.position, transform.position);
		float ThrowPower = Vector3.Distance(np, transform.position);
		return("Расстояние: " + string.Format("{0:N2}", BallDist) + " m." + "\n" + "Сила броска: " + string.Format("{0:N2}", ThrowPower));
	}

	public int GetBallDistanceForScores()
	{
		int res = Mathf.RoundToInt(Vector3.Distance(RingCenter.transform.position, transform.position) * 100);
		return(res);
	}
}
