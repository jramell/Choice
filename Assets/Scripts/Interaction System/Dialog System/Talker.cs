using UnityEngine;

/// <summary>
/// Component an entity that talks (this is, has conversations with dialog) must implement. Each Talker maintains a current conversation,
/// and decides which conversations are appropriate to have according to game state. Additionally, talkers control their conversation
/// loop. Being a separate entity from an NPC allows a Talker to represent conversations with multiple NPCs in them.
/// </summary>
public abstract class Talker : MonoBehaviour {
	/// <summary>
	/// Returns next piece of dialog of the current conversation. If the current conversation is
	/// over, returns an null. If called again, it may loop the last conversation or start another one.
	/// </summary>
	public abstract Dialog Talk();
}
