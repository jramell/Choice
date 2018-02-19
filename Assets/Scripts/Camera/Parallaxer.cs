using UnityEngine;
[System.Serializable]
public class ParallaxBackground {
	public GameObject background;
	public float depth;
}

/// <summary>
/// Handles 2D parallaxing of the backgrounds added to its background array. Should be added to the camera for which parallax
/// is required.
/// </summary>
public class Parallaxer : MonoBehaviour {

	/// <summary>
	/// Backgrounds that are parallaxed
	/// </summary>
	[Tooltip("Backgrounds that are parallaxed")]
	public ParallaxBackground[] backgrounds;


}