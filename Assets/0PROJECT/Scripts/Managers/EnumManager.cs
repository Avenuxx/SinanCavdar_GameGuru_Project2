using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Beginning,
    Playing,
    FinishLine,
    GameOver,
}

public enum PlayerState
{
    GoingStack,
    GoingForward,
}

public enum CMCam
{
    CMPlayer, 
    CMFinishLine,
    CMLose,
}

public enum CollectableType
{
    Coin,
    Diamond,
    Star,
}


