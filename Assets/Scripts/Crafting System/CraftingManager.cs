using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour {

	/// <summary>
	/// Color the text in the Input Field where player is crafting turns when it's a word in the dictionary
	/// </summary>
	[Tooltip("Color the text in the Input Field where player is crafting turns when it's a word in the dictionary")]
	public Color craftableTextColor = Color.green;

	/// <summary>
	/// Color the text in the Input Field where player is crafting turns when it's not a word in the dictionary
	/// </summary>
	[Tooltip("Color the text in the Input Field where player is crafting turns when it's not a word in the dictionary")]
	public Color uncraftableTextColor = Color.red;

	/// <summary>
	/// Sound effect that plays when the word currently written in the Input Field where player is crafting is in the player's dictionary
	/// </summary>
	[Tooltip("Sound effect that plays when the word currently written in the Input Field where player is crafting is in the player's dictionary")]
	public AudioSource writtenCraftableSFX;

	/// <summary>
	/// Sound effect that plays when the word currently written in the Input Field where player is crafting isn't in the player's dictionary
	/// </summary>
	[Tooltip("Sound effect that plays when the word currently written in the Input Field where player is crafting isn't in the player's dictionary")]
	public AudioSource writtenUncraftableSFX;

	/// <summary>
	/// Sound effect that plays when the player tries to craft a word that can be crafted, i.e., they actually craft it.
	/// </summary>
	[SerializeField]
	[Tooltip("Sound effect that plays when the player tries to craft a word that can be crafted, i.e., they actually craft it")]
	private AudioSource craftedSFX;

	/// <summary>
	/// Sound effect that plays when the player tries to craft a word that can't be crafted.
	/// </summary>
	[SerializeField]
	[Tooltip("Sound effect that plays when the player tries to craft a word that can't be crafted")]
	private AudioSource uncraftedSFX;

	private static CraftingManager instance;
	private CraftingController craftingController;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static CraftingManager Instance {
		get { return instance; }
	}

	/// <summary>
	/// Enables the crafting system and tells it to listen to input on the Input Field passed as a parameter. Does not
	/// handle anything about the display of such Input Field. If the Crafting System was already enabled, this 
	/// </summary>
	public void EnableCrafting(InputField craftingInputField) {
		if(craftingController == null) {
			craftingController = GameObject.FindGameObjectWithTag("Player").GetComponent<CraftingController>();
		}
		craftingController.Enable(craftingInputField);
	}

	/// <summary>
	/// Disables the Crafting System, meaning it will stop listening for the player's input. Does not handle 
	/// anything about the display of the Input Field that was being listened to before disabling. If the Crafting
	/// System was already disabled, does nothing.
	/// </summary>
	public void DisableCrafting() {
		if(craftingController == null) {
			craftingController = GameObject.FindGameObjectWithTag("Player").GetComponent<CraftingController>();
		}
		craftingController.Disable();
	}
		
	/// <returns><c>true</c> if the word passed as a parameter could be crafted, <c>false</c> otherwise.</returns>
	public bool Craft(string word) {
		Word craftedWord = DictionaryManager.Instance.GetWordWithName(word);
		if(craftedWord != null) {
			if(craftedSFX != null) {
				craftedSFX.Play();
			}
			craftedWord.Craft();
			return true;
		} else {
			if(uncraftedSFX != null) {
				uncraftedSFX.Play();
			}
			return false;
		}
	}

	public bool IsCraftable(string word) {
		return DictionaryManager.Instance.Contains(word);
	}
}
