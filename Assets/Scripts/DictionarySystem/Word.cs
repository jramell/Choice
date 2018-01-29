using UnityEngine;
using UnityEditor;

public class Word : MonoBehaviour {

	public enum EffectType {
		Buff, Equip
	}

	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private string word;

	[TextArea(3, 10)]
	[SerializeField]
	private string definition;

	[SerializeField]
	private Word.EffectType type;

	[Tooltip("Buff to be applied to the player when this word is used. If word doesn't have EffectType Buff, nothing is done with this")]
	public Buff buff;

	[Tooltip("Equipment to be equipped to the player when this word is used. If word doesn't have EffectType Equiṕ, nothing is done with this")]
	public Equipment equipment;

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

	public void Use() {
		if(type == Word.EffectType.Buff) {
			//tell player controller's BuffManager to apply buff
		} else if(type == Word.EffectType.Equip) {
			//tell the player controller's EquipmentManager to equip equipment
		}
	}
}
