using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSparksManager : MonoBehaviour {

	private static GameSparksManager instance = null;

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		} else {
			Destroy(this.gameObject);
		}
	}

	public void AuthenticateAnonymous(Action<AuthenticationResponse> callback) {
		new DeviceAuthenticationRequest().Send((AuthenticationResponse response) => {
				if (response.HasErrors) {
					Debug.Log("Error logging in" + response.Errors.GetString("DETAILS"));
				} else {
					Debug.Log("Logged in user anonymously: " + response.UserId + " " + response.DisplayName);
					// TODO: Save user info.
					PlayerPrefs.SetString("user_id", response.UserId);
					PlayerPrefs.SetString("display_name", response.DisplayName);
					PlayerPrefs.SetInt("login_type", (int)LoginType.Anonymous);
				}
			callback(response);
		});
	}
	
	public void Authenticate(string username, string password, Action<AuthenticationResponse> callback) {

		Debug.Log("Logging in with username: " + username);
		new AuthenticationRequest()
			.SetUserName(username)
			.SetPassword(password)
			.Send((AuthenticationResponse response) => {
				if (response.HasErrors) {
					Debug.Log("Error logging in" + response.Errors.GetString("DETAILS"));
				} else {
					Debug.Log("Logged in user: " + response.UserId + " " + response.DisplayName);
					// TODO: Save user info.
					PlayerPrefs.SetString("user_id", response.UserId);
					PlayerPrefs.SetString("display_name", response.DisplayName);
					PlayerPrefs.SetInt("login_type", (int)LoginType.UsernamePassword);
				}
				callback(response);
			});
	}

	public void GetAccountDetails() {
		new AccountDetailsRequest().Send((AccountDetailsResponse response) => {
			// response.UserId;
		});
	}

	public void LoadPlayer() { //Action<LogEventResponse> callback
		new LogEventRequest()
			.SetEventKey("getPlayerInventory")
			.Send((LogEventResponse response) => {
				List<GSData> items = response.ScriptData.GetGSDataList("player_inventory");
				foreach(GSData eventData in items) {
					Debug.Log("item: " + eventData.GetGSData("item").GetString("name"));
				}
				// callback(response);
			});
	}


}
