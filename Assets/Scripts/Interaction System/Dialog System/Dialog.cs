using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains information necessary to say one thing. This is: who is saying the thing, what SFX should be played
/// when the dialog is displayed, and of course the sentence that is said. Is attached to a dummy object.
/// </summary>
public class Dialog : MonoBehaviour {
	/// <summary>
	/// The NPC that says the dialog
	/// </summary>
	[SerializeField]
	[Tooltip("The NPC that says the dialog. Could be empty.")]
	private NPC interlocutor;

	[SerializeField]
	[TextArea(3, 10)]
	[Tooltip("What is said in this dialog")]
	private string sentence;

	/// <summary>
	/// The sound effect that accompanies the dialog. Could be voice over.
	/// </summary>
	[SerializeField]
	[Tooltip("The sound effect that accompanies the dialog. Could be voice over")]
	private AudioSource soundEffect;

	/// <summary>
	/// If true, the sound effect used will be the NPC's voice SFX.
	/// </summary>
	[Tooltip("If true, the sound effect used will be the NPC's voice SFX")]
	[SerializeField]
	private bool useNPCVoiceSFX = true;

	[Tooltip("Type of custom action this dialog does")]
	public CustomAction.Type customActionType = CustomAction.Type.None;

	[Tooltip("If Custom Action Type is EnableButton, this is the Button to be enabled. Otherwise, does nothing")]
	public Button buttonToEnable;

	public string Sentence {
		get { return sentence; }
	}

	public NPC Interlocutor {
		get { return interlocutor; }
	}

	public void PlaySoundEffect() {
		if (useNPCVoiceSFX && interlocutor.voiceSFX != null) {
			interlocutor.voiceSFX.Play(); //SFX is NPC's voice
		} else if(soundEffect != null) {
			soundEffect.Play(); //SFX is the custom one defined in the dialog
		}
	}
}
