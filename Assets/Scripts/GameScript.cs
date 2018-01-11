using GameSparks;
using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



enum InitialiationState {
	Starting,
	WaitingForAvailable,
	Available,
	Authenticating,
	AuthError,
	Authenticated,
	Loading,
	Ready,
}

public enum LoginType : int {
	Unknown = 0,
	Anonymous = 1,
	UsernamePassword = 2,
}

public class GameScript : MonoBehaviour {

	public GameSparksManager gameSparksManager;
	public UIManager uiManager;

	private InitialiationState initialiationState = InitialiationState.Starting;


	private bool gsAvailable = false;
	private bool gsAuthenticated = false;

	void Start () {

		// GS.GameSparksAvailable += GSAvailableHandler;


		MyTestClass testClass = MyTestClass.BuildMyTestClass();
		GSData gsData = new GSData(GSDataHelpers.ObjectToGSData(testClass));

		Debug.LogWarning("Size of JSON object: " + GSDataHelpers.SizeOfGSData(gsData) + " bytes");

		string json = gsData.JSON;

		GSData newGSData = new GSRequestData(json);
		
		// var gsDataList = newGSData.GetGSDataList("myInventory");
		// Debug.Log(gsDataList);
		MyTestClass otherTestClass = (MyTestClass)GSDataHelpers.GSDataToObject(newGSData);

		Debug.Log(otherTestClass.myInventory[1].name);



		// List<GSData> dataList = new List<GSData>();
		// dataList.Add(new GSData());
		// GSRequestData gsRequestData = new GSRequestData();
		// gsRequestData.AddObjectList("theList", dataList);

		// var finalList = gsRequestData.GetGSDataList("theList");
		// Debug.Log(finalList);

	}

	private void InitUser() {
		
		gsAvailable = GS.Available;
		if (GS.Available) {
			Debug.Log("GS Available");
			initialiationState = InitialiationState.Available;
		} else {
			Debug.Log("GS Unavailable");
		}

		if (GS.Available && initialiationState == InitialiationState.Available) {
			initialiationState = InitialiationState.Authenticating;
			gsAuthenticated = GS.Authenticated;
			if (GS.Authenticated) {
				initialiationState = InitialiationState.Authenticated;
				Debug.Log("Authenticated");
			} else {
				Debug.Log("Not authenticated");
				if (PlayerPrefs.GetInt("login_type") == (int)LoginType.UsernamePassword) {
					// The user should be logged in with a real account, but isn't. should show an error and prompt to login here.
					Debug.LogWarning("User should be logged in with user/pass, but isn't authenticated.");
					initialiationState = InitialiationState.AuthError;
				} else {
					gameSparksManager.AuthenticateAnonymous((response) => {
						if (response.HasErrors) {
							// TODO: Error screen or something.
							Debug.Log("Error logging in anonymously");
							initialiationState = InitialiationState.AuthError;
						} else {
							// MMMk we're logged in kinda.
							initialiationState = InitialiationState.Authenticated;
							LoadPlayer();
						}
					});
				}
			}
		
			// GS.GameSparksAvailable -= GSAvailableHandler;
		}
	}

	private void GSAvailableHandler(bool available) {
		// if (initialiationState == InitialiationState.Available) {
		// 	// Already available.

		// }
		InitUser();

	}

	// private void GSAuthenticatedHandler(string thing) {

	// 	Debug.Log("auth changed: "+ thing);
	// 	// gsAuthenticated = authenticated;
	// 	// if (GS.Authenticated) {
	// 	// 	Debug.Log("Authenticated");
			
	// 	// } else {
	// 	// 	Debug.Log("Not authenticated");
			
	// 	// }
	// }
	
	// private void InitializeGame() {
	// 	if (initialiationState != InitialiationState.NotInitialized) {
	// 		return;
	// 	}
	// 	// initialiationState = InitialiationState.Initializing;




	// }

	public void Login(string username, string password, Action<bool, string> callback) {

		// TODO: Clear out old player data and GS.

		gameSparksManager.Authenticate(username, password, (response) => {
			if (response.HasErrors) {
				callback(false, "Wrong username or password.");
			} else {
				initialiationState = InitialiationState.Authenticated;
				LoadPlayer();
				callback(true, "");
				uiManager.SetLoggedInAnonymously(false);
				// uiManager
			}
		});
	}


	private void LoadPlayer() {
		if (initialiationState != InitialiationState.Authenticated) {
			Debug.LogWarning("Trying to load player when not authenticated.");
			return;
		}
		initialiationState = InitialiationState.Loading;

		Debug.Log("Loading player");
	}

	public void Logout() {
		GS.Reset();
		uiManager.SetLoggedInAnonymously(true);
		// TODO: clear player data.
		InitUser();
	}
}
