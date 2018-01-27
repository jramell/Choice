using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public AudioSource soundEffect;

	/// <summary>
	/// If true, the sound effect used will be the NPC's voice SFX.
	/// </summary>
	[Tooltip("If true, the sound effect used will be the NPC's voice SFX")]
	public bool useNPCVoiceSFX = true;

	public string Sentence {
		get { return sentence; }
	}

	public NPC Interlocutor {
		get { return interlocutor; }
	}
}
