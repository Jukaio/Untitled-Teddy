using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sentient : Finite.State
{
    private float sense_timer;
    private float sense_interval;
    private float decide_timer;
    private float decide_interval;

    protected Sentient(GameObject context, float sense_rate, float decide_rate) 
        : base(context)
    {
        sense_timer = sense_rate;
        sense_interval = sense_rate;
        decide_timer = decide_rate;
        decide_interval = decide_rate;
        
    }

    public abstract void sense();
    public abstract void decide(Finite collection);
    public abstract Finite.Mode act(Finite collection);

    public override Finite.Mode update(Finite collection)
    {
        sense_timer -= Time.deltaTime;
        decide_timer -= Time.deltaTime;
        if(sense_timer < 0)
        {
            sense();
            sense_timer = sense_interval;
        }
        if (decide_timer < 0)
        {
            decide(collection);
            decide_timer = decide_interval;
        }
        return act(collection);
    }
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] static public GameObject player;
    [SerializeField] static public RoomGenerator RG;
    private Finite.State.Machine FSM;


    protected void initialise(string name, Finite.State enter_state)
    {
        FSM = new Finite.State.Machine(name, enter_state);
    }

    void Update()
    {
        FSM.update();
    }
    public void add(string name, Finite.State that)
    {
        FSM.add(name, that);
    }
    public void remove(string name)
    {
        FSM.remove(name);
    }
}
