
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls all of the player-based movement that occurs over the course of the game.  This is a subclass of CharacterBaseActionClass, 
 * yet most of the base classes methods are overridden, as well as a few new methods that allow for shooting, slashing, etc.  
 * 
 * 
 */


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerHealthPanelManager))]

public class Player : Character, ICanHoldItems {

	//Instance data for the Player.  
	private bool playerCoroutinesCurrentlyActive = true;

	//Used so when the player is in between two close walls, he/she automatically goes up by just pressing the up arrow. 

	IEnumerator weaponInputCoroutine, arrowMovementCoroutine;

	protected override void InitializeCharacter() {
		//Start the coroutines required for the player.  
		weaponInputCoroutine = CheckForWeaponInput ();
		arrowMovementCoroutine = ListenForArrowMovement ();
		StartCoroutine (weaponInputCoroutine);
		StartCoroutine (arrowMovementCoroutine);
	}

	//Used to check whether or not player is grounded, touching a wall, etc.  Defines movements.  
	protected override IEnumerator CheckCharacterPhysics() {
		while (true) {
			//Update the grounded boolean.  
			grounded = CheckWhetherGrounded();

			//The ground checks should be extremely close to the player, or it appears as grounded on the next frame.  
			if (grounded && jumpInEffect != 0 && jumpInEffect != 4) {
				InitializeJump (0);
			} 

			//In case the player is in the air (not jumping, just falling)
			if (grounded == false && jumpInEffect == 0) {
				//No force should be added, so this is done manually. 
				jumpInEffect = 1;
				anim.SetInteger ("Jump", 1);
			}

			//When the player wants to jump.  
			if (playerCoroutinesCurrentlyActive) {
				if (jumpInEffect != 4) {
					if (Input.GetKeyDown (KeyCode.UpArrow)) {
						//The order of these conditions is important.  
						if (jumpInEffect == 0)
							InitializeJump (1);
						else if (jumpInEffect == 1)
							//Double jump
							InitializeJump (2);
					} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
						if (jumpInEffect == 1 || jumpInEffect == 2) 
							//Dive and roll.  
							InitializeJump (3);
						if (jumpInEffect == 0 && Math.Abs(rb2d.velocity.x) < 0.1f)
							InitializeJump (4);
						if (jumpInEffect == 0 && Math.Abs (rb2d.velocity.x) > 0.1f)
							anim.SetTrigger ("Slide");
					} 
				}

				if (Input.GetKeyUp(KeyCode.DownArrow) && jumpInEffect == 4) {
					InitializeJump(0);
				}
			}


			//Every frame.  
			yield return null;
		}

	}

	protected override void InitializeJump(int jumpStyle) {
		//Jumping parameters
		anim.SetInteger("Jump", jumpStyle);
		jumpInEffect = jumpStyle;

		//Add forces based on the jump style.  
		switch (jumpStyle) {
		case 0: 
			break;
		case 1: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
			break;
		case 2: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
			break;
		case 3: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, -jumpForce);
			break;
		case 4: 
			//Ducking, nothing happens.  
			break;
		default: 
			Debug.LogError ("Invalid jumpStyle of " + jumpStyle + " input");
			break;
		}

	}

	//Movement based on arrow keys
	IEnumerator ListenForArrowMovement () { 
		//Prevent constant boxing and unboxing.  
		float h = 0;

		//Constantly
		while (true) {
			//This gets the current state of the pressed keys.  
			h = Input.GetAxis ("Horizontal");
			//Make it so the player cannot move if the character is ducking.  
			if (jumpInEffect == 4) {
				h = 0;
				rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
			}

			//Movement
			if (Mathf.Abs (h) > 0)
				anim.SetBool ("Running", true);
			else
				anim.SetBool ("Running", false);

			rb2d.AddForce (Vector2.right * moveForce * h * 1 / (2f * jumpInEffect + 1));

			rb2d.velocity = new Vector2 (Mathf.Clamp (rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);

			//Control flipping based on the arrow keys.  
			if (h > 0 && !facingRight) 
				Flip ();
			else if (h < 0 && facingRight) 
				Flip ();

			//Tell the camera that the player is moving (should be changed at some point.  
			//transform.FindChild("Main Camera").FindChild("Background").GetComponent <BackgroundManager> ().MoveBackground(rb2d.velocity.x / maxSpeed);

			yield return new WaitForFixedUpdate();

		}
	}

	//Used for weapons.  
	public void ExternalJumpAction (int num) {
		InitializeJump (num);
	}

	/************************************************ ITEM STUFF *************************************************************/
	//This dictionary contains the possible weapon moves for the player.  The first entry contains the required action to trigger the action, and the second
	//includes a string of the method.  
	private MovementAndMethod[] possibleWeaponMoves;

	private Item itemInUseByCharacter;

	//This will be called by the item management part of the costume manager script
	public void OnRefreshCurrentWeaponMoves(Item ctorItemInUseByCharacter) {
		itemInUseByCharacter = ctorItemInUseByCharacter;
		if (ctorItemInUseByCharacter != null) {
			possibleWeaponMoves = itemInUseByCharacter.GetPossibleActionsForItem ();
		} else {
			possibleWeaponMoves = null;
		}
	}
		
	//The coroutine method that will check whether the dictionary requirements for some attack have been met.  The code that sets the array (above) 
	//is in the costume manager class.  
	IEnumerator CheckForWeaponInput() {
		//Unless the possible attack dictionary is empty,
		while (true) {
			if (itemInUseByCharacter != null) {
				//Works due to short-circuiting.  
				if (possibleWeaponMoves != null && possibleWeaponMoves.Length != 0) {
					if (currentlyInAttackAnimation == false) {
						for (int i = 0; i < possibleWeaponMoves.Length; i++) {
							//If the can be used while midair is false, then it will only work while grounded is true.  Vice versa is also the case.  
							if (possibleWeaponMoves [i].GetCanBeUsedWhileMidair () == ! grounded) {
								if (possibleWeaponMoves [i].GetTriggerHasOccurred ()) {
									if (!currentlyInAttackAnimation) {
										anim.SetTrigger (possibleWeaponMoves[i].GetActionKey());
										itemInUseByCharacter.InfluenceEnvironment (possibleWeaponMoves[i].GetActionEnum());
									} else {
										Debug.Log("Was in attack animation, did not attack");
									}
								}
							}
						}
					}
				} else {
					Debug.LogError ("Possible weapon moves of " + itemInUseByCharacter.gameObject.name + " is null");
				}
			}

			//For every frame.  
			yield return null;
		}
	}

	/**************************************** OTHER STUFF ******************************************/
	public void DisablePlayerActions() {
		if (playerCoroutinesCurrentlyActive) {
			//Disable the coroutines.  
			StopCoroutine (arrowMovementCoroutine);
			StopCoroutine (weaponInputCoroutine);
			playerCoroutinesCurrentlyActive = false;
			anim.SetBool ("Running", false);
		} else {
			Debug.Log ("Cannot disable coroutines: Coroutines are already disabled");
		}
	}

	public void EnablePlayerActions() {
		if (playerCoroutinesCurrentlyActive == false) {
			//Enable the coroutines.  
			arrowMovementCoroutine = ListenForArrowMovement ();
			weaponInputCoroutine = CheckForWeaponInput ();
			StartCoroutine (arrowMovementCoroutine);
			StartCoroutine (weaponInputCoroutine);
			playerCoroutinesCurrentlyActive = true;
		} else {
			Debug.Log ("Cannot enable coroutines: Coroutines are already active.");
		}
	}

	/************************** HEALTH PANEL MANAGER ***********************************/
	public CharacterHealthPanelManager GetHealthController() {
		return GetComponent <CharacterHealthPanelManager> ();
	}
}
