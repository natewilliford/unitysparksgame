using GameSparks;
using GameSparks.Api.Responses;
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

public class GameScript : MonoBehaviour, TouchGestureListener {

	public GameSparksManager gameSparksManager;
	public UIManager uiManager;

	private InitialiationState initialiationState = InitialiationState.Starting;

	private TouchManager touchManager;

	private bool gsAvailable = false;
	private bool gsAuthenticated = false;


	
    private BuildingBehavior placingBuildingBehavior;

    public BuildingBehavior farmPrefab;

	private GameData gameData = new GameData();
	private Dictionary<object, GameObject> dataGameObjectsMap = new Dictionary<object, GameObject>();
	

	void Start () {

		touchManager = new TouchManager();
		touchManager.AddListener(this);

		GS.GameSparksAvailable += GSAvailableHandler;


		// MyTestClass testClass = MyTestClass.BuildMyTestClass();
		// GSData gsData = new GSData(GSDataHelpers.ObjectToGSData(testClass));

		// // Debug.LogWarning("Size of JSON object: " + GSDataHelpers.SizeOfGSData(gsData) + " bytes");

		// string json = gsData.JSON;
		// // string json = "{\"className\":\"MyTestClass\",\"_id\":{\"$oid\":\"testclassid\"},\"myString\":\"Hello World!\"}";
		// Debug.Log(json);

		// GSData newGSData = new GSRequestData(json);
		
		// // var gsDataList = newGSData.GetGSDataList("myInventory");
		// // Debug.Log(gsDataList);
		// MyTestClass otherTestClass = (MyTestClass)GSDataHelpers.GSDataToObject(newGSData);

		// Debug.Log(otherTestClass._id.oid);



		// List<GSData> dataList = new List<GSData>();
		// dataList.Add(new GSData());
		// GSRequestData gsRequestData = new GSRequestData();
		// gsRequestData.AddObjectList("theList", dataList);

		// var finalList = gsRequestData.GetGSDataList("theList");
		// Debug.Log(finalList);

	}

	void Update() {
		touchManager.UpdateTouches();
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
				LoadPlayer();
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

		Debug.Log("Loading buildings");
		gameSparksManager.GetPlayerBuildings((LogEventResponse response) => {
			if (response.HasErrors) {
				// handle bla
			} else {
				foreach(GSData buildingGSData in response.ScriptData.GetGSDataList("player_buildings")) {
					// GSData buildingGSData = buildingWrapperGSData.GetGSData("building");
					Building buildingData = (Building)GSDataHelpers.GSDataToObject(buildingGSData);
					gameData.AddBuilding(buildingData);
					BuildingBehavior newBuilding = Instantiate<BuildingBehavior>(farmPrefab);
					newBuilding.SetPlaced(true);
					
					newBuilding.transform.position = new Vector2(buildingData.position.x, buildingData.position.y);
					dataGameObjectsMap.Add(buildingData, newBuilding.gameObject);
				}
			}
		});

		Debug.Log("Loading player");
	}

	public void Logout() {
		GS.Reset();
		uiManager.SetLoggedInAnonymously(true);
		// TODO: clear player data.
		InitUser();
	}





	public void OnTouchGesture(TouchGesture gesture) {
		
        if (placingBuildingBehavior != null) {
			// Debug.Log("got a placingBuildingBehavior");

            if (gesture.type == TouchGestureType.Tap) {
                Debug.Log("Tap at " + gesture.endPosition);
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.endPosition), Vector2.zero);
                if (hitInfo) {

                    if (hitInfo.transform.CompareTag("Confirm")) {
                        ConfirmPlacement();
                    } else if (hitInfo.transform.CompareTag("Cancel")) {
                        CancelPlacement();
                    }
                }
            } else if (gesture.type == TouchGestureType.Drag) {

                // Debug.Log("drag at " + gesture.endPosition);
                Vector2 endPoint = Camera.main.ScreenToWorldPoint(gesture.endPosition);
                Vector2 placePoint = new Vector2(endPoint.x, endPoint.y + 0.3f);

                if (placingBuildingBehavior.IsDragging()) {
                    placingBuildingBehavior.gameObject.transform.position = placePoint;
                } else {

                    if (placingBuildingBehavior != null) {
                        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.startPosition), Vector2.zero);
                        if (hitInfo && hitInfo.collider.gameObject == placingBuildingBehavior.gameObject) {
                            placingBuildingBehavior.gameObject.transform.position = placePoint;
                            placingBuildingBehavior.SetDragging(true);
                        }
                    }
                }

            } else if (gesture.type == TouchGestureType.DragEnd) {
                if (placingBuildingBehavior != null) {
                    placingBuildingBehavior.SetDragging(false);
                }
            }
        }
	}
	
    public void PlaceBuilding() {
        Debug.Log("place buliding");
        if (placingBuildingBehavior != null) {
            Debug.Log("canceling previous");
            CancelPlacement();
        }

        placingBuildingBehavior = Instantiate< BuildingBehavior>(farmPrefab);
        if (placingBuildingBehavior.gameObject != null) {
            Debug.Log("got a building to place");
        } else {
            Debug.Log("it's broken, yo");
        }
    }

    private void CancelPlacement() {
        Destroy(placingBuildingBehavior.gameObject);
        placingBuildingBehavior = null;
    }
    private void ConfirmPlacement() {
		placingBuildingBehavior.SetPendingPlacement(true);

		gameSparksManager.BuyBuilding("farm", placingBuildingBehavior.transform.position, (LogEventResponse response) => {
			placingBuildingBehavior.SetPendingPlacement(false);
			if (!response.HasErrors) {
				placingBuildingBehavior.ConfirmPlacement();
        		placingBuildingBehavior = null;


				// Building buildingData = (Building)GSDataHelpers.GSDataToObject(buildingGSData);
				// 	gameData.AddBuilding(buildingData);
				// 	BuildingBehavior newBuilding = Instantiate<BuildingBehavior>(farmPrefab);
				// 	newBuilding.transform.position = new Vector2(buildingData.posX, buildingData.posY);
				// 	dataGameObjectsMap.Add(buildingData, newBuilding.gameObject);
			}
		});
    }
}
