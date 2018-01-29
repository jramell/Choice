/// <summary>
/// Base class of a buff or a debuff, i.e. a class that defines behavior such that it alters the state 
/// </summary>
public abstract class Buff : Wearable {

	/// <summary>
	/// Applies the buff to the player by altering its state. Buff duration is then managed by the BuffManager.
	/// </summary>
	public abstract void Apply();

	/// <summary>
	/// Buff expires, nullifying its influence on the player's state.
	/// </summary>
	public abstract void Expire();
}
