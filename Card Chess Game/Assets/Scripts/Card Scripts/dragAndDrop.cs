using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

using Config; 
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 startPos;
    public int index;
    public int cardType; // 무슨 카드인지
    public bool canMove = true; // 드래그할수 있는지
    private Card card; 
    private GameObject trashCan;
    private int player; 
    public GameManager player_data;
    private bool lock_dragging = false;
    void Start() {
        startPos = transform.position;
        trashCan = GameObject.Find("TrashCan");
    }
    void Update()
    {

    }

    public void init(Card card, int player, int index) {
        this.card = card; 
        this.player = player; 
        this.index = index;
        if (this.player == 1)
        {
            player_data = GameManager.player1;
        }
        else
        {
            player_data = GameManager.player2;
        }
        createNewCard();
    }

    private void changeCard() {
        this.card = player_data.replaceCard(index);
        createNewCard();
        lock_dragging = true;
        GetComponent<Outline>().enabled = true;
    }

    private void createNewCard() {
        Sprite card_sprite = Config.Helper.generateSprite(card.ToString());
        transform.GetChild(0).GetComponent<Image>().sprite = card_sprite;
        transform.GetChild(1).GetComponent<Text>().text = card.ToString(); 
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(lock_dragging) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(GetComponent<BoxCollider2D>().IsTouching(trashCan.GetComponent<BoxCollider2D>())) {
            changeCard();
        }
        
        transform.position = startPos;
    }
}