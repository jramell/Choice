using UnityEngine;

public class ElectricProjectile : Projectile {

	public float shockStrength;

	void OnTriggerEnter2D(Collider2D col) {
		IShockable shockableComponent = col.GetComponent<IShockable>();
		if(shockableComponent != null) {
			shockableComponent.ReceiveShock(shockStrength);
		}
		//Dissappear();
	}

	private void Dissappear() {
		//spawn particles, play a sound or something
		Destroy(gameObject);
	}
}
