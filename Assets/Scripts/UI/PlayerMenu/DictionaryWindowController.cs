using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages input while in the dictionary UI menu, including button presses and word "highlighting".
/// </summary>
public class DictionaryWindowController : MonoBehaviour, IController {

	/// <summary>
	/// Game Object containing the Player Menu's UI elements.
	/// </summary>
	[Tooltip("Object containing the Player Menu's UI elements")]
	public GameObject playerMenu;

	#region Craft Button Settings definition
	[Header("Craft Button Settings")]
	/// <summary>
	/// Button that's pressed for crafting options to pop up
	/// </summary>
	[Tooltip("Button that's pressed for crafting options to pop up")]
	public Button craftButton;

	/// <summary>
	/// Icon the crafting button will have while the crafting options are displayed
	/// </summary>
	[Tooltip("Icon the crafting button will have while the crafting options are displayed")]
	public Sprite craftButtonActiveIcon;

	#endregion

	#region Crafting Options Settings definition
	[Header("Crafting Options Settings")]
	/// <summary>
	/// Game Object that contains UI elements that display the selected word's icon.
	/// </summary>
	[Tooltip("Object that contains UI elements that display the selected word's icon")]
	public GameObject selectedWordIcon;

	/// <summary>
	/// Game Object that contains UI elements that display the selected word's definition.
	/// </summary>
	[Tooltip("Object that contains UI elements that display the selected word's icon")]
	public GameObject selectedWordDefinition;

	/// <summary>
	/// Game Object that contains UI elements that display the Text Field where the player must write a word.
	/// </summary>
	[Tooltip("Object that contains UI elements that display the Text Field where the player must write a word")]
	public GameObject craftingInputFieldObject;

	#endregion

	#region Internal variables definition
	/// <summary>
	/// True if the crafting options in the Dictionary Menu are visible and interactable for the player, false otherwise.
	/// </summary>
	private bool craftWindowOpen;

	private InputField craftingInputField;

	private Sprite craftButtonOriginalIcon;

	//TODO: Handle navigation only with arrows.
	#endregion

	void Start() {
		craftButton.onClick.AddListener(() => OnCraftButtonClicked());
		craftButtonOriginalIcon = craftButton.transform.Find("Icon").GetComponent<Image>().sprite;
		craftingInputField = craftingInputFieldObject.GetComponent<InputField>();
		enabled = false; //dictionary window starts off
	}

	void Update() {
		HandleNavigationInput();
		HandleCraftingInput();
	}

	private void HandleCraftingInput() {
		if(PlayerWantsToCraft()) {
			if(craftWindowOpen) {
				string word = craftingInputField.text;
				if(CraftingManager.Instance.IsCraftable(word)) {
					ExitPlayerMenu(); //opens the way for the Craftable UI to handle setting icons and other stuff
					CraftingManager.Instance.Craft(word);
				}
			} else {
				OpenCraftWindow();
			}
		}
	}

	private void HandleNavigationInput() {
		if(PlayerWantsToReturnInWindow()) {
			ReturnInWindow();
		} else if(PlayerWantsToExitMenu()) {
			ExitPlayerMenu();
		}
	}

	private void ReturnInWindow() {
		if(craftWindowOpen) {
			CloseCraftWindow();
		} else {
			ExitPlayerMenu();
		}
	}

	/// <summary>
	/// Exits menu where the player's dictionary and (future) other options are, resuming
	/// normal game.
	/// </summary>
	private void ExitPlayerMenu() {
		if(craftWindowOpen) {
			CloseCraftWindow();
		}
		MenuNavigationManager.Instance.ExitPlayerMenu();
		//window.setbool(close) ...
		playerMenu.SetActive(false);
	}

	private bool PlayerWantsToReturnInWindow() {
		return Input.GetKeyDown(KeyCode.Escape);
	}

	private bool PlayerWantsToExitMenu() {
		return Input.GetKeyDown(KeyCode.Tab);
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}

	private void OnCraftButtonClicked() {
		if(!craftWindowOpen) {
			OpenCraftWindow();
		} else {
			CloseCraftWindow();
		}
	}

	private bool PlayerWantsToCraft() {
		return Input.GetKeyDown(KeyCode.Return);
	}

	private void OpenCraftWindow() {
		selectedWordIcon.SetActive(false);
		selectedWordDefinition.SetActive(false);
		CraftingManager.Instance.EnableCrafting(craftingInputField);
		//TODO: Change when button image is actually button image. Just use craftButton.image
		craftingInputFieldObject.SetActive(true);
		craftWindowOpen = true;
		craftButton.transform.Find("Icon").GetComponent<Image>().sprite = craftButtonActiveIcon;
	}

	private void CloseCraftWindow() {
		selectedWordIcon.SetActive(true);
		selectedWordDefinition.SetActive(true);
		CraftingManager.Instance.DisableCrafting();
		craftingInputFieldObject.SetActive(false);
		craftingInputField.text = "";
		craftWindowOpen = false;
		craftButton.transform.Find("Icon").GetComponent<Image>().sprite = craftButtonOriginalIcon;
	}
}
