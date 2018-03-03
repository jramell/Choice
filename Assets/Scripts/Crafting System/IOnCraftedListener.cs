/// <summary>
/// Entities that implement this interface want to be notified of crafting events
/// </summary>
public interface IOnCraftedListener {
	void OnWordCrafted();
}