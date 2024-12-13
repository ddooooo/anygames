using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapController : MonoBehaviour, IPointerDownHandler
{
    public GameObject mage;
    public GameObject target;
    public GameObject card;
    public void OnPointerDown(PointerEventData eventData)
    {
        Transform temp = mage.transform.parent;
        mage.transform.SetParent(target.transform.parent, false);
        target.transform.SetParent(temp, false);
        GameManager.lastDotClicked(true);
    }
    public void deleteCard()
    {
        if (card) {
            int hand_index = card.GetComponent<DragDrop>().hand_index;
            card.GetComponent<DragDrop>().player_data.hand.RemoveAt(hand_index);
            Destroy(card);
        }
    }
}
