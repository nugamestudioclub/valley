using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clockhand : ToggleProp
{
    public int Ticks => 12;

    public int NumberPointing => state == 0 ? 12 : state + 1;

    [SerializeField]
    private int length;
    public int Length => length;
    private CapsuleCollider col;

    private void Awake()
    {
        col = GetComponent<CapsuleCollider>();
        col.radius = Length; 
    }
    protected override int NumStates => Ticks;

    protected override void EnterState(int stateNum)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,
            transform.eulerAngles.y,
            -(float)stateNum / Ticks * 360);
    }

    protected override void Activate()
    {
        if (GameManager.IsTimeFrozen)
        {
            Toggle();
        }
    }
}
