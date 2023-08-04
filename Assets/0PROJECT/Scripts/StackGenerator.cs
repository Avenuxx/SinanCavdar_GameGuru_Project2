using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGenerator : MonoBehaviour
{
    GameObject[] stacks;
    Vector2 stackBounds = new Vector2(bound, bound);

    int score;
    int stackIndex;
    int combo;

    float stackMovement;
    float stackSpeed = 2f;

    const float bound = 3f;
    float gameOverAmount = 0.1f;

    bool _isMoveAxis;

    Vector3 lastStackPos;

    void Start()
    {
        stacks = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            stacks[i] = transform.GetChild(i).gameObject;

        stackIndex = transform.childCount - 1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (PlaceStack())
                SpawnStack();
            else
                GameOver();
        }
        MoveStack();
    }

    void SpawnStack()
    {
        lastStackPos = stacks[stackIndex].transform.localPosition;
        stackIndex--;
        if (stackIndex < 0)
            stackIndex = transform.childCount - 1;
        _isMoveAxis = !_isMoveAxis;
        stacks[stackIndex].transform.localPosition = new Vector3(0, 0, score * 3);
        score++;
    }

    void MoveStack()
    {
        stackMovement += Time.deltaTime * stackSpeed;
        stacks[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(stackMovement) * bound, 0, score * 3);
    }

    bool PlaceStack()
    {
        Transform lastStack = stacks[stackIndex].transform;

        float diff = lastStackPos.x - lastStack.position.x;

        if (Mathf.Abs(diff)>gameOverAmount)
        {
            combo = 0;
            stackBounds.x -= Mathf.Abs(diff);
            if (stackBounds.x <= 0)
                return false;

            float middle = lastStackPos.x + lastStack.localPosition.x / 2;
            lastStack.localScale=(new Vector3(stackBounds.x, 1, stackBounds.y));
        }

        return true;
    }

    void GameOver()
    {

    }
}
