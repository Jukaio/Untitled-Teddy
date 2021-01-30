using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Finite.State.Machine FSM;

    protected void initialise(Finite.State enter_state)
    {
        FSM = new Finite.State.Machine(enter_state);
    }

    void Update()
    {
        FSM.update();
    }
}
