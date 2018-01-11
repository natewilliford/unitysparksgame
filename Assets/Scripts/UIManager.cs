using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Button loginButton;
	public Button logoutButton;

	public GameObject overlayPanel;
	public GameObject loginDialog;

	public void OpenLoginDialog() {
		OpenDialog(loginDialog);
	}

	private void OpenDialog(GameObject dialog) {
		overlayPanel.active = true;
		dialog.active = true;
	}

	public void CloseDialog(GameObject dialog) {
		overlayPanel.active = false;
		dialog.active = false;
	}

	public void SetLoggedInAnonymously(bool loggedInAnonymously) {
		if (loggedInAnonymously) {
			loginButton.gameObject.SetActive(true);
			logoutButton.gameObject.SetActive(false);
		} else {
			loginButton.gameObject.SetActive(false);
			logoutButton.gameObject.SetActive(true);
		}
	}
}
