using System;
using UnityEngine;

public abstract class LevelData : MonoBehaviour
{
    public event EventHandler playerWon;

    protected void RaisePlayerWon() => playerWon?.Invoke(this, EventArgs.Empty);
}