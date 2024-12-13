using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropInGame : MonoBehaviour
{
    private float firstPosX;
    private float firstPosY;
    private float startPosX;
    private float startPosY;
    private bool isBeingHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        firstPosX = this.transform.localPosition.x;
        firstPosY = this.transform.localPosition.y;

    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingHeld == true) {

            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            
            gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
        }
    }

    private void OnMouseDown() {
        if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos); // convert screen mouse to world(game)

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            isBeingHeld = true;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

    private void OnMouseUp() {
        isBeingHeld = false;
        gameObject.transform.localPosition = new Vector3(firstPosX, firstPosY, 0);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        
    }
}
