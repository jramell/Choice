using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPrototype : MonoBehaviour {

	[Header("UI Settings")]
	public Image endScreenUI;
	public float screenUIFadeInTime = 0.5f;
	public float changeSceneDelay = 0.1f;

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Player") {
			StartCoroutine(GoToCredits());	
		}
	}

	private IEnumerator GoToCredits() {
		SystemManager.Instance.RegisterActiveSystem(GameSystem.Type.PauseMenu);
		endScreenUI.gameObject.SetActive(true);
		UIUtils.Instance.FadeGraphic(endScreenUI, screenUIFadeInTime);
		yield return new WaitForSecondsRealtime(screenUIFadeInTime + changeSceneDelay);
		SceneManager.LoadScene("Credits");
	}
}
