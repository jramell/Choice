using UnityEngine;

public class Word : MonoBehaviour {

	public enum EffectType {
		Buff, Equip
	}

	#region Word Settings variable declarations
	[Header("Word Settings")]

	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private string word;

	[TextArea(3, 10)]
	[SerializeField]
	private string definition;

	[TextArea(3, 10)]
	[SerializeField]
	private string pickupDescription;

	[SerializeField]
	private Word.EffectType type;

	[Tooltip("Buff to be applied to the player when this word is used. If word doesn't have EffectType Buff, nothing is done with this")]
	public Buff buff;

	[Tooltip("Equippable to be equipped to the player when this word is used. If word doesn't have EffectType Equiṕ, nothing is done with this")]
	public Equippable equippable;
	#endregion

	public Sprite Icon {
		get { return icon; }
	}

	public string WordName {
		get { return word; }
	}

	public string Definition {
		get { return definition; }
	}


	public Word.EffectType Type {
		get { return type; }
	}

	public string PickupDescription {
		get { return pickupDescription; }
	}

	public void Craft() {
		if(type == Word.EffectType.Buff) {
			//tell player controller's BuffManager to apply buff
		} else if(type == Word.EffectType.Equip) {
			//tell the player controller's EquipmentManager to equip equipment
			EquipmentManager.Instance.Equip(equippable);
		}
	}
}
