using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

	public Texture2D [] texs;

	public void SetNewCursor(int ID)
	{
		Cursor.SetCursor(texs[ID], new Vector2(32f, 32f), CursorMode.Auto);
	}

	public void ResetCursor()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
}
