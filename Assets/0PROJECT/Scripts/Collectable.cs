using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectable : MonoBehaviour
{
    GameManager manager;
    [Header("Enums")]
    public CollectableType collectableTypeEnum;

    [Space(10)]
    [Header("Transforms")]
    public Transform targetStack;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        GetComponent<Animator>().Play("CollectableAnim", 0, Random.Range(0f, 60f));
    }

    private void Update()
    {
        float newPosX = Mathf.Lerp(transform.position.x, targetStack.transform.position.x, Time.deltaTime * 10f);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
    }

    private void OnCollect(object value)
    {
        if ((GameObject)value != this.gameObject)
            return;

        //COLLECT OBJECT PROCESS
        transform.SetParent(manager.objects.player.transform);
        transform.DOLocalJump(Vector3.zero, 2, 1, .5f).OnComplete(() =>
        {
            Destroy(gameObject);
            manager.data.TotalMoney += (int)collectableTypeEnum + 1;
            EventManager.Broadcast(GameEvent.OnEarnMoney, gameObject);
        });
        EventManager.Broadcast(GameEvent.OnPlaySound, "Coin");
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
