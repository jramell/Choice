﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Handlers are objects that handle both a controller's job (listening for world input) and a manager's (maintaining and updating
/// world state). The Tutorial Handler does that for the tutorial part of the game.
/// </summary>
public class TutorialHandler : MonoBehaviour {

	[Header("UI Settings")]
	public Text tutorialTextComponent;
	public CanvasGroup tutorialCanvasGroup;

	public GameObject buttonPointingArrow;
	public Button dictionaryCraftingButton;
	public DictionaryWindowController dictionaryController;
	public CraftingController craftingController;

	[Header("Tutorial Settings")]
	[Tooltip("Time in seconds the tutorial text takes to fade in")]
	public float fadeInTime;

	[Tooltip("Time in seconds the tutorial text takes to fade out")]
	public float fadeOutTime;

	public TutorialConfig movementTutorialConfig;
	public TutorialConfig jumpTutorialConfig;
	public TutorialConfig clickTutorialConfig;
	public TutorialConfig useEquipmentTutorialConfig;

	private enum TutorialState {
		Standby,
		Movement, //teaching the player how to move
		Jump, //teaching the player how to jump
		Click, //telling the player to click on orange words
		CraftMode, //telling the player to push the button in the interface
		WriteWord, //telling the player to write a word into the crafting input field
		//Craft, //telling the player to click the craft button because he has some working shit
		UseEquipment,
		Finished//
	}

	private TutorialState tutorialState = TutorialState.Movement;

	private BoolWrapper playerHasMovedHorizontally;
	private BoolWrapper playerHasJumped;
	private BoolWrapper playerHasClickedWord;
	private BoolWrapper playerHasUsedEquipment;

	void Start() {
		playerHasMovedHorizontally = new BoolWrapper(false);
		playerHasJumped = new BoolWrapper(false);
		playerHasClickedWord = new BoolWrapper(false);
		playerHasUsedEquipment = new BoolWrapper(false);
		CurrentTutorialState = TutorialState.Standby;
		AdvanceTutorial();
	}

	void Update() {
		HandleCurrentStateInput();
	}

	private TutorialState CurrentTutorialState {
		get { return tutorialState; }
		set { tutorialState = value;
			HideTutorialTextComponent();
		}
	}

	private void HideTutorialTextComponent() {
		UIUtils.Instance.FadeCanvasGroup(tutorialCanvasGroup, fadeOutTime, 0);
	}

	private void HandleCurrentStateInput() {
		if(CurrentTutorialState == TutorialState.Movement) {
			if(!playerHasMovedHorizontally.value) {
				playerHasMovedHorizontally.value = PlayerManager.Instance.PlayerState.PlayerVelocity.x != 0;
			} else {
				AdvanceTutorial();
			}
		} else if(CurrentTutorialState == TutorialState.Jump) {
			if(!playerHasJumped.value) {
				playerHasJumped.value = PlayerManager.Instance.PlayerState.PlayerVelocity.y > 0;
			} else {
				AdvanceTutorial();
			}
		} else if(CurrentTutorialState == TutorialState.Click) {
			if(!playerHasClickedWord.value) {
				playerHasClickedWord.value = DictionaryManager.Instance.WordCount() > 0;
			} else {
				AdvanceTutorial();
			}
		} else if (CurrentTutorialState == TutorialState.WriteWord) {
			dictionaryController.Disable();
			if (craftingController.IsInputTextValidWord) {
				dictionaryCraftingButton.interactable = true;
				buttonPointingArrow.SetActive(true);
			} else {
				dictionaryCraftingButton.interactable = false;
				buttonPointingArrow.SetActive(false);
			}
		} else if(CurrentTutorialState == TutorialState.UseEquipment) {
			if (!playerHasUsedEquipment.value) {
				playerHasUsedEquipment.value = Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0);
			} else {
				AdvanceTutorial();
			}
		}
	}

	private void AdvanceTutorial() {
		CurrentTutorialState++;
		Debug.Log("CurrenTutorialState = " + CurrentTutorialState);
		if (CurrentTutorialState == TutorialState.Movement) {
			DisplayTutorialWithConfig(movementTutorialConfig, playerHasMovedHorizontally, false);
		} else if (CurrentTutorialState == TutorialState.Jump) {
			DisplayTutorialWithConfig(jumpTutorialConfig, playerHasJumped, false);
		} else if (CurrentTutorialState == TutorialState.Click) {
			DisplayTutorialWithConfig(clickTutorialConfig, playerHasClickedWord, false);
		} else if (CurrentTutorialState == TutorialState.CraftMode) {
			buttonPointingArrow.SetActive(true);
			dictionaryCraftingButton.onClick.AddListener(() => AdvanceTutorial());
			//Display arrow pointing to button. Activate it, but it'll only show when menu is opened
			//Deactivate option to get out of menu
			//Suscribe AdvanceTutorial() to that button's onClick
		} else if (CurrentTutorialState == TutorialState.WriteWord) {
			buttonPointingArrow.SetActive(false);
			dictionaryCraftingButton.onClick.RemoveListener(() => AdvanceTutorial());
			dictionaryCraftingButton.interactable = false;
			//Unsuscribe AdvanceTutorial() from button's click
			//Display prompt to write word and then press the button again
			//Make button to go back from craft mode non-interactable, and "Enter" shortcut unavailable as well
			//when exiting from this
		} else if (CurrentTutorialState == TutorialState.UseEquipment) {
			DisplayTutorialWithConfig(useEquipmentTutorialConfig, playerHasUsedEquipment, false);
		} else if (CurrentTutorialState == TutorialState.Finished) {
			dictionaryCraftingButton.interactable = true;
			buttonPointingArrow.SetActive(false);
			enabled = false;
			//Reactivate option to get out of menu, and make "Enter" shortcut and button to go back from craft mode
			//interactable again
		}
	}

	private void DisplayTutorialWithConfig(TutorialConfig tutorialConfig, BoolWrapper condition, bool conditionEndState) {
		StartCoroutine(_DisplayTutorialWithConfig(tutorialConfig, condition, conditionEndState));
	}
	/// <summary>
	/// Waits tutorialConfig.startDelay seconds, and if the condition passed as a parameter is the same as
	/// conditionEndState, fades in the tutorial text component with the message tutorialConfig.message
	/// </summary>
	private IEnumerator _DisplayTutorialWithConfig(TutorialConfig tutorialConfig, BoolWrapper condition, bool conditionEndState=false) {
		yield return new WaitForSeconds(tutorialConfig.startDelay);
		if(condition.value == conditionEndState) {
			tutorialTextComponent.gameObject.SetActive(true);
			tutorialTextComponent.text = tutorialConfig.message;
			UIUtils.Instance.FadeCanvasGroup(tutorialCanvasGroup, fadeInTime, 1);
		}
	}
}

[System.Serializable]
public class TutorialConfig {
	[Tooltip("Time in seconds this tutorial message will take to appear after the previous tutorial ended")]
	public float startDelay;

	[TextArea(3, 10)]
	public string message;
}

public class BoolWrapper {
	public bool value;

	public BoolWrapper() {
		value = false;
	}

	public BoolWrapper(bool initialValue) {
		value = initialValue;
	}
}