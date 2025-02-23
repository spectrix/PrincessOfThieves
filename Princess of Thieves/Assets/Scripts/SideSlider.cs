﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSlider : ResettableObject,ActivateableObject {
	SliderStatus status;
	[SerializeField]
	bool startOpen = false;
	[SerializeField]
	float travelTime = 0.5f;
	[SerializeField]
	bool openHorizontally = true;

	Vector3 openPos;
	Vector3 closePos;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		if (startOpen) {
			openPos = transform.position;
			if (openHorizontally) {
				closePos = openPos - new Vector3 (gameObject.HalfWidth () * 2, 0);
			} else {
				closePos = openPos - new Vector3 (0, gameObject.HalfHeight () * 2);
			}
			status = SliderStatus.Open;
		} else {
			closePos = transform.position;
			if (openHorizontally) {
				openPos = closePos + new Vector3 (gameObject.HalfWidth () * 2, 0);
			} else {
				openPos = closePos + new Vector3 (0, gameObject.HalfHeight () * 2);
			}
			status = SliderStatus.Closed;
		}
	}

	public void Activate() {
		if (status == SliderStatus.Closed || status == SliderStatus.Closing) {
			StartCoroutine ("Open");
		}
	}

	IEnumerator Open() {
		StopCoroutine ("Close");
		status = SliderStatus.Opening;
 
		do {
			if (openHorizontally) {
				transform.position += new Vector3(gameObject.HalfWidth() * 2.0f * Time.deltaTime / travelTime, 0);
			} else {
				transform.position += new Vector3(0, gameObject.HalfHeight() * 2.0f * Time.deltaTime / travelTime);
			}

			yield return null;
		} while((openHorizontally && transform.position.x < openPos.x) || (!openHorizontally && transform.position.x < openPos.y));
		transform.position = openPos;
		status = SliderStatus.Open;
	}

	IEnumerator Close() {
		StopCoroutine ("Open");
		status = SliderStatus.Closing;

		do {
			if (openHorizontally) {
				transform.position -= new Vector3(gameObject.HalfWidth() * 2.0f * Time.deltaTime / travelTime, 0);;
			} else {
				transform.position -= new Vector3(0, gameObject.HalfHeight() * 2.0f * Time.deltaTime / travelTime);
			}
			yield return null;
		} while((openHorizontally && transform.position.x > closePos.x) || (!openHorizontally && transform.position.y > closePos.y));
		
		transform.position = closePos;
		status = SliderStatus.Closed;
	}
	public void Deactivate() {
		if (status == SliderStatus.Open || status == SliderStatus.Opening) {
			StartCoroutine ("Close");
		}
	}

	public bool IsActive {
		get {
			return status == SliderStatus.Closing || status == SliderStatus.Opening;
		}
	}

	public override void Reset ()
	{
		if (startOpen) {
			transform.position = openPos;
			status = SliderStatus.Open;
		} else {
			transform.position = closePos;
			status = SliderStatus.Closed;
		}
	}

	public float ActivationTime {
		get {
			return travelTime;
		}
	}
}

public enum SliderStatus {
	Open,
	Opening,
	Closed,
	Closing
}
