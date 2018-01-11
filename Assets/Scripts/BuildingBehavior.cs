using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehavior : MonoBehaviour {


    private bool dragging;

    private bool placed;

    private SpriteRenderer spriteRenderer;

    // private GameObject confirmButton;
    // private GameObject cancelButton;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent< SpriteRenderer > ();

        // foreach(Transform child in GetComponentInChildren<Transform>()){
        //     if (child.CompareTag("Confirm")) {
        //         confirmButton = child.gameObject;
        //     }
        //     if (child.CompareTag("Cancel")) {
        //         cancelButton = child.gameObject;
        //     }
        //     //if (child.) 
        // }

        UpdatePlacedDraggingState();
    }
	
    public void ConfirmPlacement() {
        placed = true;
        dragging = false;
        UpdatePlacedDraggingState();
    }

    public void SetDragging(bool dragging) {
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

    private void UpdatePlacedDraggingState() {
        if (placed) {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 1f;
            spriteRenderer.color = spriteColor;
            // confirmButton.SetActive(false);
            // cancelButton.SetActive(false);
            //GetComponent<BoxCollider2D>().SetActive(false);
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        } else if (dragging) {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 0.6f;
            spriteRenderer.color = spriteColor;
            // confirmButton.SetActive(false);
            // cancelButton.SetActive(false);
        } else {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 0.6f;
            spriteRenderer.color = spriteColor;
            // confirmButton.SetActive(true);
            // cancelButton.SetActive(true);
        }
    }

}
