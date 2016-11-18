using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapeController : MonoBehaviour {

	public RawImage [] Imgs;
	public float MoveSpeed = 1f;
	public float [] Limits; //Left start, left end, right start, right end;
	RectTransform L, R;

	void Start()
	{		
		L = Imgs[0].GetComponent<RectTransform>() as RectTransform;
		R = Imgs[1].GetComponent<RectTransform>() as RectTransform;
	}

	void Update () {
		L.transform.Translate(new Vector3(0f, -MoveSpeed * Time.deltaTime, 0f));
		R.transform.Translate(new Vector3(0f, MoveSpeed * Time.deltaTime, 0f));

		if (L.transform.localPosition.y < Limits[1])
			L.transform.localPosition = new Vector3(L.transform.localPosition.x, Limits[0], L.transform.localPosition.z);
		
		if (R.transform.localPosition.y > Limits[3])
			R.transform.localPosition = new Vector3(R.transform.localPosition.x, Limits[2], R.transform.localPosition.z);
	}
}
