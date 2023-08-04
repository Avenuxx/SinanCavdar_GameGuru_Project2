using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private StackGenerator stackGenerator;
    public bool _isGameOver;

    void Start()
    {
        stackGenerator = FindObjectOfType<StackGenerator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (StackMovement.CurrentStack != null)
                StackMovement.CurrentStack.Stop();

            if (_isGameOver == true)
                return;

            stackGenerator.SpawnStack();
        }
    }
}
