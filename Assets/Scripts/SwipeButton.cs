using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position=transform.localPosition;

        transform.localPosition=new Vector3(Mathf.Clamp(position.x+eventData.delta.x,-79,79),position.y,position.z); 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 position=transform.localPosition;

        if(position.x< 79 && position.x >= -79)
        {
            transform.localPosition=new Vector3(-79,position.y,position.z);

            
        }
        else
        {
            GameManager.Instance.OnPlayButtonClicked();
            transform.localPosition = new Vector3(-79, position.y, position.z);
        }
       
    }
}
