using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGenerator : MonoBehaviour
{
    public Material[] stackMaterials;
    public int score;

    [SerializeField]
    private StackMovement stackPrefab;

    public void SpawnStack()
    {
        var stack = Instantiate(stackPrefab);

        if (StackMovement.LastStack != null && StackMovement.LastStack.gameObject != GameObject.Find("StartStack"))
        {
            int xPos = score % 2==0 ? -4 : 4;
            stack.transform.position = new Vector3(xPos, transform.position.y, StackMovement.LastStack.transform.position.z + stackPrefab.transform.localScale.z);
        }
        else
        {
            stack.transform.position = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, stackPrefab.transform.localScale);
    }
}