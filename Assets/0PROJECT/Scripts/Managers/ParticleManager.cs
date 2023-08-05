using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public List<ParticleSystem> collectableParticles = new List<ParticleSystem>();

    private void OnCollect(object value)
    {
        GameObject collectable = (GameObject)value;
        int particleIndex = (int)collectable.GetComponent<Collectable>().collectableTypeEnum;
        ParticleSystem newParticle = Instantiate(collectableParticles[particleIndex], collectable.transform.position, Quaternion.identity, null);
        Destroy(newParticle, 2f);
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
