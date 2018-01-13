using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehavior : MonoBehaviour {


    private bool dragging;

    private bool placed;

    private bool pendingPlacement;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;

    private GameObject confirmButton;
    private GameObject cancelButton;

    private bool started = false;

    void Awake() {
        Debug.Log("Awake");
    }

    void OnEnable() {
        Debug.Log("OnEnable");
    }

	// Use this for initialization
	void Start () {
        Debug.Log("Start");


        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        foreach(Transform child in GetComponentInChildren<Transform>()){
            if (child.CompareTag("Confirm")) {
                confirmButton = child.gameObject;
            }
            if (child.CompareTag("Cancel")) {
                cancelButton = child.gameObject;
            }
            //if (child.) 
        }

        started = true;
        UpdatePlacedDraggingState();
    }
	
    public void ConfirmPlacement() {
        placed = true;
        dragging = false;
        UpdatePlacedDraggingState();
    }

    public void SetDragging(bool dragging) {
        Debug.Log("SetDragging");
        this.dragging = dragging;
        UpdatePlacedDraggingState();
    }

    public bool IsDragging() {
        return dragging;
    }


    public void SetPlaced(bool placed) {
        this.placed = placed;
        UpdatePlacedDraggingState();
    }

    public void SetPendingPlacement(bool pendingPlacement) {
        this.pendingPlacement = pendingPlacement;
        UpdatePlacedDraggingState();
    }

    private void UpdatePlacedDraggingState() {
        Debug.Log("UpdatePlacedDraggingState");

        if (!started) {
            Debug.Log("Not Started yet so skipping.");
            return;
        }

        if (placed) {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 1f;
            spriteRenderer.color = spriteColor;
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
            collider.enabled = false;
        } else if (dragging || pendingPlacement) {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 0.6f;
            spriteRenderer.color = spriteColor;
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
        } else {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 0.6f;
            spriteRenderer.color = spriteColor;
            confirmButton.SetActive(true);
            cancelButton.SetActive(true);
        }
    }
}
