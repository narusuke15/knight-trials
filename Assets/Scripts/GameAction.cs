using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {Activate, Spawn};

[Serializable]
public class GameAction
{
    public ActionType ActionType;
    public GameObject target;
}
