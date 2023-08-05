using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public List<GameObject> collectableList = new List<GameObject>();

    void OnSpawnCollectable(object value)
    {
        if (Random.value > .9f)
            return;

        GameObject stack = (GameObject)value;
        int collectableType = Random.Range(0, 3);

        Vector3 pos = stack.transform.position + new Vector3(0, 1, 0);
        var newCollectable = Instantiate(collectableList[collectableType], pos, Quaternion.identity, null);
        newCollectable.GetComponent<Collectable>().targetStack = stack.transform;
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSpawnCollectable, OnSpawnCollectable);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSpawnCollectable, OnSpawnCollectable);
    }
}
