using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.Debug;
using Config;
public class WheelController : MonoBehaviour, IPointerDownHandler
{
    float rot_speed;
    bool clicked = false;
    float decay_factor = 0;
    void Start() {

    }
    void Update()
    {
        transform.Rotate(0, 0, this.rot_speed);
        this.rot_speed *= decay_factor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Log("clicked!");
        if(clicked) return; 
        decay_factor = Random.Range(0.9942f, 0.9943f);
        rot_speed = Random.Range(3.0f, 3.15f);
        clicked = true;
    }

    public int result() {
        if(transform.eulerAngles.z <= 180) {
            return (int) Constants.P1_First;
        }else {
            return (int) Constants.P2_First;
        }
    }

    public bool finish() {
        if(rot_speed <= 0.07f && clicked) {
            rot_speed = 0;
            return true;
        }
        return false;
    }
}
