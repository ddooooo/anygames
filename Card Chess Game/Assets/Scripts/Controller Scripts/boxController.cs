using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoxController : MonoBehaviour
{
    public Sprite trashcan_close;
    public Sprite trashcan_open;


    //public cardGenerator cardScript;
    private void OnTriggerEnter2D(Collider2D other) {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = trashcan_open;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        gameObject.GetComponent<SpriteRenderer>().sprite = trashcan_close;
    }
}
