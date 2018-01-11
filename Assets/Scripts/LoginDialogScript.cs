using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Responses;

public class LoginDialogScript : MonoBehaviour {
	public UIManager uiManager;
	public GameScript gameScript;
	public Text errorText;
	public InputField usernameInput;
	public InputField passwordField;
	public Button loginButton;
	public Button cancelButton;


	public void HandleLoginButtonTap() {

		errorText.gameObject.SetActive(false);
		
		// TODO: validate.
		string username = usernameInput.text;
		string password = passwordField.text;

		gameScript.Login(username, password, (succeeded, errorMessage) => {
			if (succeeded) {
				CloseDialog();
			} else {
				// TODO error messages.
				errorText.text = "Could not log in";
				errorText.gameObject.SetActive(true);
			}
		});

		// gsManager.Authenticate(username, password, (AuthenticationResponse response) => {
		// 	if (response.HasErrors) {
		// 		// TODO error messages.
		// 		errorText.text = "Could not log in";
		// 		errorText.gameObject.active = true;
		// 	} else {
		// 		// success!
		// 		CloseDialog();
		// 	}
		// });
	}

	public void HandleCancelButtonTap() {
		CloseDialog();
	}

	private void CloseDialog() {
		usernameInput.text = "";
		passwordField.text = "";
		errorText.gameObject.SetActive(false);

		uiManager.CloseDialog(gameObject);
	}


}
