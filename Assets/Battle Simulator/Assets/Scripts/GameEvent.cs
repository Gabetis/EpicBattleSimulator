using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvent
{
    public static Action Attack;
    public static void OnAttack() => Attack?.Invoke();
}
