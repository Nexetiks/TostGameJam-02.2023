using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        MultiMousePointerEventData multiMousePointerEventData = eventData as MultiMousePointerEventData;
        Debug.Log($"Message {multiMousePointerEventData.MouseDeviceId}");
    }
}
