using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class backGround : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    private bool notMove;
    public void OnDrag(PointerEventData eventData)
    {
        if(notMove)
            notMove = false;
        GameObject.Find("Scene").transform.localPosition += new Vector3(eventData.delta.x,eventData.delta.y,0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        notMove = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(notMove)
            GameObject.Find("Scene").GetComponent<GameScene>().ClickNothing();
    }
}
