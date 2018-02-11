/// <summary>
/// Elements that want to be notified when an entity's health changes implement this interface
/// </summary>
public interface IHealthChangedListener {
	void OnHealthChanged();
}