using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text3DController : MonoBehaviour {

	public Vector3 Movement;
	public float KillTime = 2f;

	void Start () {
		Destroy(gameObject, KillTime);
	}

	void Update () {
		transform.Translate(Movement * Time.deltaTime);	
	}
}
