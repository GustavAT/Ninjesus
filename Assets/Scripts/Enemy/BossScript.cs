﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossScript : MonoBehaviour {
	
	Animator animator;

	//animation states - the values in the animator conditions
	const int STATE_LEFT = 0;
	const int STATE_RIGHT = 1;
	int _currentAnimationState = STATE_LEFT;

	string _currentDirection = "left";

	// 1 - Designer variables

	public float X_WallOffset = 2.0f;
	public float Y_WallOffset = 2.0f;
	
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(2, 2);

	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, -1);

	private Vector2 movement;
	private Rigidbody2D rigidbodyComponent;

	void Start()
	{
		animator = this.GetComponent<Animator>();
	}

	void Update()
	{
		// 2 - Movement
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);
		
		//RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left);
		var raycastLeft = Physics2D.RaycastAll(transform.position, Vector2.left,X_WallOffset);
		var raycastRight = Physics2D.RaycastAll(transform.position, Vector2.right,X_WallOffset);
		var raycastTop = Physics2D.RaycastAll(transform.position, Vector2.up,Y_WallOffset);
		var raycastBot = Physics2D.RaycastAll(transform.position, Vector2.down,Y_WallOffset);
		if (raycastLeft.Length >= 1)
		{ 
        	if (raycastLeft.Any(x=>x.collider.CompareTag("Wall"))||raycastLeft.Any(x=>x.collider.CompareTag("Door")))
			{
				direction = new Vector2(1,direction.y);
				_currentDirection = "right";
				changeState(STATE_RIGHT);
			}
		}
		if (raycastRight.Length >=1)
		{
			if (raycastRight.Any(x=>x.collider.CompareTag("Wall"))||raycastRight.Any(x=>x.collider.CompareTag("Door")))
			{
				direction = new Vector2(-1,direction.y);
				_currentDirection = "left";
				changeState(STATE_LEFT);
			}
		}
		if (raycastTop.Length >=1)
		{
			if (raycastTop.Any(x=>x.collider.CompareTag("Wall"))||raycastTop.Any(x=>x.collider.CompareTag("Door")))
			{
				direction = new Vector2(direction.x,-1);
				if (_currentDirection == "left")
				{
					changeState(STATE_LEFT);
				}

				if (_currentDirection == "right")
				{
					changeState(STATE_RIGHT);
				}
			}
		}
		if (raycastBot.Length >=1)
		{
			if (raycastBot.Any(x=>x.collider.CompareTag("Wall"))||raycastBot.Any(x=>x.collider.CompareTag("Door")))
			{
				direction = new Vector2(direction.x,1);
				if (_currentDirection == "left")
				{
					changeState(STATE_LEFT);
				}

				if (_currentDirection == "right")
				{
					changeState(STATE_RIGHT);
				}
			}
		}
	}

	void FixedUpdate()
	{
		if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

		// Apply movement to the rigidbody
		rigidbodyComponent.velocity = movement;
		
	}
	
	void changeState(int state)
	{

		if (_currentAnimationState == state)
			return;

		switch (state)
		{

			case STATE_LEFT:
				animator.SetInteger("state", STATE_LEFT);
				break;

			case STATE_RIGHT:
				animator.SetInteger("state", STATE_RIGHT);
				break; 

		}

		_currentAnimationState = state;
	}
}
