﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 5f;
	public float jumpForce = 600;
	private Animator animator;
	private Rigidbody2D rb2d;
	private bool isFacingRight = true;
	private bool jump;
	private bool isOnLand = false;
	private Transform landCheck;
	private float xForce = 0;
	private bool isDead = false;
	private bool isCrouched;
	private bool isAimingUp;
	private bool isReloading;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		landCheck = gameObject.transform.Find("LandCheck");
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDead) {
			isOnLand = Physics2D.Linecast(transform.position, landCheck.position, 1 << LayerMask.NameToLayer("Land"));

			if (isOnLand) {
				animator.SetBool("Jump", false);
			}

			if (Input.GetButtonDown("Jump") && isOnLand && !isReloading) {
				jump = true;
			} else if (Input.GetButtonUp("Jump")) {
				if (rb2d.velocity.y > 0) {
					rb2d.velocity = new Vector2(rb2d.velocity.x, Convert.ToSingle(rb2d.velocity.y * 0.5));
				};
			}

			if (Input.GetButtonDown("Fire1")) {
				animator.SetTrigger("Shoot");
			}

			isAimingUp = Input.GetButton("Up");
			isCrouched = Input.GetButton("Down");

			animator.SetBool("AimUp", isAimingUp);
			animator.SetBool("Crouch", isCrouched);

			if (Input.GetButtonDown("Reload")) {
				animator.SetBool("Reload", true);
			} else {
				animator.SetBool("Reload", false);
			}

			if ((isCrouched || isAimingUp || isReloading) && isOnLand) {
				xForce = 0;
			}
		}
	}

	private void FixedUpdate () {
		if (!isDead) {
			if (!isCrouched && !isAimingUp && !isReloading) {
				xForce = Input.GetAxisRaw("Horizontal");
			}

			animator.SetFloat("Speed", Mathf.Abs(xForce));

			rb2d.velocity = new Vector2(xForce * speed, rb2d.velocity.y);

			if (xForce > 0 && !isFacingRight) {
				Flip();
			} else if (xForce < 0 && isFacingRight) {
				Flip();
			}

			if (jump) {
				animator.SetBool("Jump", true);
				jump = false;
				rb2d.AddForce(Vector2.up * jumpForce);
			}
		}
	}

	void Flip() {
		isFacingRight = !isFacingRight;

		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
