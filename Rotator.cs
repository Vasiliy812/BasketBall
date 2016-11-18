using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public Vector3 RotationObject;

	void Update () {
		transform.Rotate(RotationObject * Time.deltaTime);
	}
}
