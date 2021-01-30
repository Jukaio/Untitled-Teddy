using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : EnemyController
{
    public class Entry : Finite.State
    {
        public Entry(GameObject context)
            : base(context)
        {
            
        }
        public override void enter(Finite collection)
        {
            /* Add States here */
        }

        public override void exit(Finite collection)
        {

        }

        public override Finite.Mode update(Finite collection)
        {
            return Finite.Mode.Run;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        initialise(new Entry(gameObject));
    }

}
