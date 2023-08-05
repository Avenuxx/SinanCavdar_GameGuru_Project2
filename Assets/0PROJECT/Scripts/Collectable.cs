using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    CollectableType collectableTypeEnum;

    private void OnCollect(object value)
    {
        if ((GameObject)value != this.gameObject)
            return;

        Debug.Log("collected");
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCollect, OnCollect);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCollect, OnCollect);
    }
}
