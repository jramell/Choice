using UnityEngine;

/// <summary>
/// Behave as platforming spikes. They may be electrified. If they are, 
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Spikes : MonoBehaviour {

	[Tooltip("If checked, object tag will become Spikes and BoxCollider will become a trigger automatically. This is done so it can interact with the rest of the system")]
	public bool autoSetup;

	[Tooltip("Should these Spikes shock objects on contact?")]
	public bool electrified = false;

	[Tooltip("If this object is electrified, what is the strength of its shock?")]
	public float shockStrength = 10f;

	void Start() {
		if(autoSetup) {
			gameObject.tag = "Spikes";
			GetComponent<BoxCollider2D>().isTrigger = true;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if(electrified) {
			IShockable shockableObject = col.gameObject.GetComponent<IShockable>();
			if (shockableObject != null) {
				shockableObject.ReceiveShock(shockStrength);
			}
		}
	}
}
