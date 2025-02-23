﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float lifeTime = 5.0f;
	bool reflected = false;
	Timer lifeTimer;
    Rigidbody2D myRigidbody;
    bool immuneToLauncher = true;
    float maxCos = -1;
	// Use this for initialization
	void Awake () {
		lifeTimer = new Timer (() => {
			Destroy (gameObject);
		}, lifeTime);

		lifeTimer.Start ();
        myRigidbody = GetComponent<Rigidbody2D>();

        maxCos = Mathf.Cos(-Mathf.PI / 4);
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if ((col.CompareTag ("Player") && !GameManager.Instance.Player.IsReflecting) || (col.CompareTag("Launcher") && !immuneToLauncher)|| (!col.CompareTag("Player") && !(col.OnLayer ("Background") || (col.OnLayer ("Interactive") && !col.CompareTag("Block")) || col.CompareTag("Reflective")))) {
			Destroy (gameObject);
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Launcher"))
        {
            immuneToLauncher = false;
        }
    }

    void OnDestroy() {
		if (TimerManager.Instance) {
			lifeTimer.Stop ();
		}
	}

    public bool Reflect(Vector2 fwd)
    {
        if (myRigidbody)
        {
            Vector2 vel = myRigidbody.velocity;
            float dot = vel.normalized.Dot(fwd.normalized);
         
            if (dot <= 0)
            {

                float rot = Mathf.Round(fwd.GetAngle().ToDegrees() / 45) * 45;
                transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
                myRigidbody.velocity = new Vector2(Mathf.Cos(rot.ToRadians()),  Mathf.Sin(rot.ToRadians())) * vel.magnitude;
                return true;
            }
        }
        return false;
    }

	public bool Reflected {
		get {
			return reflected;
		}

		set {
			reflected = value;
		}
	}

}
