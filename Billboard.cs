using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

	public GameObject Target;
	public bool AutoFindMainCamera = true;

	void Start()
	{
		if (AutoFindMainCamera)
			Target = GameObject.Find("Main Camera") as GameObject;
	}

	void Update () {
		if (Target)
			transform.LookAt(Target.transform.position);
	}
}
