/*
 * Author: Makiah Bennett
 * Last edited: 13 September 2015
 * 
 * This script controls the UI component of the slot, and manages the stacking, moving, combining, and adding items to this slot.  
 * Each slot instance has it's own SlotScript.  This class also serves as a base class to HotbarSlotScript.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	/******************************* INITIALIZATION *******************************/

	protected void OnEnable() {
		UIInitializationSequence.InitializeSlots += ReferenceChildren;
	}

	protected void OnDisable() {
		UIInitializationSequence.InitializeSlots -= ReferenceChildren;
	}


	/******************************** SLOT SCRIPT *******************************/

	// Main properties of each slot.  
	protected ResourceReferenceWithStack currentlyAssigned;
	private bool thisSlotHasACombinationPending = false;

	//Components
	protected Image childIcon;
	private RectTransform selectionIndicator;
	private Transform tooltip;
	protected Text stackIndicator;

	public virtual void ReferenceChildren() {
		childIcon = transform.FindChild ("Icon").GetComponent <Image> ();
		childIcon.enabled = false;
		tooltip = transform.FindChild ("Tooltip");
		selectionIndicator = transform.FindChild ("Selection Indicator").GetComponent <RectTransform> ();
		stackIndicator = transform.FindChild ("Stack Indicator").GetComponent <Text> ();
		UpdateStackIndicator ();
	}


	/******************************* MOUSE CLICK MANAGER *******************************/

	public void OnPointerClick(PointerEventData data) {
		Debug.Log ("Got click");
		if (!thisSlotHasACombinationPending) {
			if (data.button == PointerEventData.InputButton.Left) {
				if (Input.GetKey (KeyCode.LeftShift) && currentlyAssigned != null) {
					ItemCombinationHandler ();
				} else {
					MouseItemMovementAndStackHandler ();
				}
			} else if (data.button == PointerEventData.InputButton.Right) {
				if (SlotMouseInputControl.GetItemInControlByMouse() == null) {
					if (currentlyAssigned != null) {
						int amountOfItemToAssign = (currentlyAssigned.stack - currentlyAssigned.stack % 2) / 2;
						if (amountOfItemToAssign != 0) {
							SlotMouseInputControl.AssignItemToMouseControl(new ResourceReferenceWithStack(currentlyAssigned.uiSlotContent, amountOfItemToAssign));
						}
						currentlyAssigned.stack -= amountOfItemToAssign;
						UpdateStackIndicator();
						Debug.Log("Assigned " + amountOfItemToAssign + " of " + currentlyAssigned.uiSlotContent.itemScreenName + " to mouse control.");
					} 
				} else if (SlotMouseInputControl.GetItemInControlByMouse() != null) {
					if (currentlyAssigned == null) {
						if (SlotMouseInputControl.GetItemInControlByMouse().stack == 1) {
							AssignNewItem(SlotMouseInputControl.DeAssignItemFromMouseControl());
						} else {
							AssignNewItem(new ResourceReferenceWithStack(SlotMouseInputControl.GetItemInControlByMouse().uiSlotContent, 1));
							SlotMouseInputControl.ChangeStackOfItemInControlByMouse(SlotMouseInputControl.GetItemInControlByMouse().stack - 1);
						}
					} else if (currentlyAssigned != null) {
						if (currentlyAssigned.Equals(SlotMouseInputControl.GetItemInControlByMouse())) {
							currentlyAssigned.stack += 1;
							UpdateStackIndicator();
							SlotMouseInputControl.ChangeStackOfItemInControlByMouse(SlotMouseInputControl.GetItemInControlByMouse().stack - 1);
						}
					}
				}
			}
		}
	}
	
	public virtual void MouseItemMovementAndStackHandler() {
		SlotMouseInputControl.ResetPendingCombinationSequence ();

		if (currentlyAssigned == null) {
			if (SlotMouseInputControl.GetItemInControlByMouse() != null) {
				AssignNewItem (SlotMouseInputControl.DeAssignItemFromMouseControl());
				UpdateStackIndicator();
			} 
		} else if (currentlyAssigned != null) {
			if (SlotMouseInputControl.GetItemInControlByMouse() == null) {
				SlotMouseInputControl.AssignItemToMouseControl(DeAssignItem ());
				UpdateStackIndicator();
			} else {
				if (SlotMouseInputControl.GetItemInControlByMouse().uiSlotContent.itemType == currentlyAssigned.uiSlotContent.itemType) {
					if (SlotMouseInputControl.GetItemInControlByMouse().uiSlotContent.localGroupID == currentlyAssigned.uiSlotContent.localGroupID) {
						currentlyAssigned.stack += SlotMouseInputControl.DeAssignItemFromMouseControl().stack;
						UpdateStackIndicator();
					}
				}
			}
		}
	}

	public void ModifyCurrentItemStack(int newStackValue) {
		currentlyAssigned.stack += newStackValue;
		if (currentlyAssigned.stack <= 0) {
			DeAssignItem();
		}
		UpdateStackIndicator ();
	}

	/******************************* ASSIGNING *******************************/

	public virtual void AssignNewItem(ResourceReferenceWithStack itemToAssign) {
		if (itemToAssign.stack != 0) {
			Sprite itemWithoutPivotPoint = ScriptingUtilities.GetSpriteWithoutPivotPoint(itemToAssign.uiSlotContent.itemIcon);
			childIcon.enabled = true;
			currentlyAssigned = itemToAssign;
			childIcon.sprite = itemWithoutPivotPoint;
			UpdateStackIndicator();
		} else {
			Debug.LogError("Could not assign item with 0 stack!");
		}
	}
	
	public virtual ResourceReferenceWithStack DeAssignItem() {
		ResourceReferenceWithStack toReturn = currentlyAssigned;
		currentlyAssigned = null;
		childIcon.sprite = null;
		childIcon.enabled = false;
		UpdateStackIndicator ();
		return toReturn;
	}
	
	public ResourceReferenceWithStack GetCurrentlyAssigned() {
		return currentlyAssigned;
	}


	/******************************* MOUSE INPUT *******************************/

	public void OnPointerEnter(PointerEventData data) {
		if (currentlyAssigned != null) {
			ShowTooltip ();
		}
	}

	public void OnPointerExit(PointerEventData data) {
		if (tooltip.gameObject.activeSelf == true) {
			HideTooltip();
		}
	}


	/******************************* COMBINATIONS *******************************/

	void ItemCombinationHandler() {
		if (SlotMouseInputControl.GetItemInControlByMouse () == null) {
			SlotMouseInputControl.AddIngredient (currentlyAssigned, this);
		} else {
			Debug.Log("Main Slot Manager had an item assigned, did not add item.");
		}
	}

	public void SetCombinationPending() {
		selectionIndicator.gameObject.SetActive (true);
		thisSlotHasACombinationPending = true;
		StartCoroutine (IndicateCombinationSelection());
	}

	public void DisableCombinationPending() {
		selectionIndicator.gameObject.SetActive (false);
		thisSlotHasACombinationPending = false;
		StopCoroutine (IndicateCombinationSelection());
		selectionIndicator.localScale = new Vector3 (1, 1, 1);
	}

	IEnumerator IndicateCombinationSelection() {
		while (true) {
			float scaleValue = Mathf.PingPong ((Time.time * 100), 20);
			selectionIndicator.localScale = new Vector3 ((80+scaleValue) / 100, (80+scaleValue) / 100, 1f);
			yield return null;
		}
	}

	/******************************* TOOLTIP *******************************/

	void ShowTooltip() {
		tooltip.gameObject.SetActive (true);
		tooltip.FindChild ("Title").GetComponent <Text> ().text = currentlyAssigned.uiSlotContent.itemScreenName;
		tooltip.FindChild ("Description").GetComponent <Text> ().text = currentlyAssigned.uiSlotContent.itemDescription;

		//Otherwise this panel is not visible.  
		transform.SetAsLastSibling();
	}

	void HideTooltip() {
		tooltip.gameObject.SetActive (false);
	}


	/******************************* STACK MANAGER *******************************/
	
	public void UpdateStackIndicator() {
		if (currentlyAssigned != null) {
			if (currentlyAssigned.stack <= 1) {
				stackIndicator.text = "";
				stackIndicator.gameObject.SetActive (false);
			} else {
				stackIndicator.gameObject.SetActive (true);
				stackIndicator.text = currentlyAssigned.stack.ToString ();
			}
		} else {
			stackIndicator.text = "";
			stackIndicator.gameObject.SetActive(false);
		}
	}


}
