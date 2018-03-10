using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Displays name of currently attached Word, and holds a reference to it. Additionally, checks if it's currently selected/highlighted
/// to tell the WordDefinitionUIController which word's definition and icon should be currently displayed.
/// </summary>
[RequireComponent(typeof(Image))]
public class WordSlot : MonoBehaviour, IPointerEnterHandler {

	private enum TransitionType {
		ColorTint, SpriteSwap
	}

	private Image wordBackground;

	[Header("Word Settings")]
	[SerializeField]
	private Word word;

	[SerializeField]
	private Text wordText;

	[SerializeField]
	private TransitionType transitionType;

	[Header("Color Tint Settings")]
	[Tooltip("Color that the image will have when selected. When unselected, color will return to the original. " +
		"If Transition Type isn't Sprite Swap, does nothing")]
	[SerializeField]
	private Color selectedColor = Color.yellow;

	private Color originalColor;

	[Header("Sprite Swap Settings")]
	[Tooltip("Sprite that the image will have when selected. When unselected, sprite will return to the original. " +
		"If Transition Type isn't Sprite Swap, does nothing")]
	[SerializeField]
	private Sprite selectedSprite;

	private Sprite originalSprite;

	void Start() {
		wordBackground = GetComponent<Image>();
		originalColor = wordBackground.color;
		originalSprite = wordBackground.sprite;
	}

	public Word Word {
		get { return word; }
		set { 
			word = value; 
			UpdateDisplayedWord();
		}
	}

	void UpdateDisplayedWord() {
		if(word == null) {
			wordText.text = "";
		} else {
			wordText.text = "<b>" + word.WordName + "</b>";
		}
	}

	public void Select() {
		if(transitionType == WordSlot.TransitionType.ColorTint) {
			wordBackground.color = selectedColor;
		} else if(transitionType == WordSlot.TransitionType.SpriteSwap) {
			wordBackground.sprite = selectedSprite;
		}
	}

	public void Deselect() {
		if(transitionType == WordSlot.TransitionType.ColorTint) {
			wordBackground.color = originalColor;
		} else if(transitionType == WordSlot.TransitionType.SpriteSwap) {
			wordBackground.sprite = originalSprite;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		DictionaryWindowManager.Instance.RegisterHoveredWordSlot(this);
	}
}
