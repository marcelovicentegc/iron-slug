using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 5f;
	public float jumpForce = 600;

	private Rigidbody2D rb2d;
	private bool isFacingRight = true;
	private bool jump;
	private bool isOnLand = false;
	private Transform landCheck;

	private float xForce = 0;
	private bool isDead = false;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		landCheck = gameObject.transform.Find("LandCheck");
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDead) {
			isOnLand = Physics2D.linecast(transform.position, landCheck.position, 1 << LayerMask.NameToLayer("Land"));

			if (input.GetButtonDown("Jump") && isOnLand) {
				jump = true;
			} else if (Input.GetButtonUp("Jump")) {
				if (rb2d.velocity.y > 0) {
					rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5);
				};
			}
		}
	}

	private void FixedUpdate () {
		if (!isDead) {
			xForce = Input.GetAxisRaw("Horizontal");

			rb2d.velocity = new Vector2(xForce * speed, rb2d.velocity.y);

			if (xForce > 0 && !isFacingRight) {
				Flip();
			} else if (xForce < 0 && isFacingRight) {
				Flip();
			}

			if (jump) {
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
