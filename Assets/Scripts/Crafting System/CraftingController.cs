using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Listens for the input inside a certain Input Field and determines whether it can be crafted or not. Does not actually
/// listen whether the player wants to craft a certain word or not, since that is responsibility of the controller of the 
/// window the Input Field belongs to.
/// </summary>
public class CraftingController : MonoBehaviour {

	/// <summary>
	/// Input Field whose input is being listened to and checked for a match against 
	/// </summary>
	private InputField currentCraftingInputField;

	/// <summary>
	/// Text the current crafting input field had when CraftingController last checked its content.
	/// </summary>
	private string previousCraftingText;

	private bool isInputTextValidWord;

	void Start() {
		enabled = false;
	}

	void Update() {
		//check words written on input field
		if(ShouldCheckIfInputTextValid()) {
			if(_IsInputTextValidWord()) {
				currentCraftingInputField.textComponent.color = CraftingManager.Instance.craftableTextColor;
				if(CraftingManager.Instance.writtenCraftableSFX != null) {
					CraftingManager.Instance.writtenCraftableSFX.Play();
				}
			} else {
				currentCraftingInputField.textComponent.color = CraftingManager.Instance.uncraftableTextColor;
				if(CraftingManager.Instance.writtenUncraftableSFX != null) {
					CraftingManager.Instance.writtenUncraftableSFX.Play();
				}
			}
		}
	}
	
	public void Enable(InputField craftingInputField) {
		previousCraftingText = "";
		currentCraftingInputField = craftingInputField;
		enabled = true;
	}

	public void Disable() {
		enabled = false;
		previousCraftingText = "";
		currentCraftingInputField = null;
	}

	private bool ShouldCheckIfInputTextValid() {
		return currentCraftingInputField.text != previousCraftingText;
	}

	private bool _IsInputTextValidWord() {
		previousCraftingText = currentCraftingInputField.text;
		isInputTextValidWord = DictionaryManager.Instance.Contains(previousCraftingText);
		return isInputTextValidWord;
	}

	public bool IsInputTextValidWord {
		get { return isInputTextValidWord; }
	}
}
