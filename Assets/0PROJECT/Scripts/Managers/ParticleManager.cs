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
        var newParticle = Instantiate(collectableParticles[particleIndex], collectable.transform.position, Quaternion.identity);
        Destroy(newParticle.gameObject, 2f);
    }

    private void OnPerfectPlaceStack(object value)
    {
        GameObject stack = (GameObject)value;
        var particlePos = stack.transform.position - new Vector3(0, 0, stack.transform.localScale.z / 2);
        var newParticle = Instantiate(Resources.Load<ParticleSystem>("PerfectParticle"), particlePos, Quaternion.identity);
        Destroy(newParticle.gameObject, 2f);
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCollect, OnCollect);
        EventManager.AddHandler(GameEvent.OnPerfectPlaceStack, OnPerfectPlaceStack);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCollect, OnCollect);
        EventManager.RemoveHandler(GameEvent.OnPerfectPlaceStack, OnPerfectPlaceStack);
    }
}
