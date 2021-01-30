using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Finite               -> Collection
    Finite.State         -> Polymorphic State
    Finite.State.Machine -> Execution of States 
    Why nested?          -> to ensure privacy 
*/

public class Finite
{
    private Dictionary<string, IState> states = new Dictionary<string, IState>();
    private IState reset = null;
    private IState current = null;
    private IState next = null;
    public bool IsValid { get { return current != null; } }
    public IState Current { get { return current; } }

    public enum Mode
    {
        Run,
        Next,
        Reset
    }
    public interface IState
    {
        public abstract void enter(Finite collection);
        public abstract Mode update(Finite collection);
        public abstract void exit(Finite collection);
    }
    public abstract class State : IState
    {
        private GameObject context;
        public string Next { get; set; }
        public GameObject Context { get { return context; } }
        public State(GameObject context)
        {
            this.context = context;
            
        }

        public abstract void enter(Finite collection);
        public abstract Mode update(Finite collection);
        public abstract void exit(Finite collection);

        public class Machine
        {
            private Finite collection;

            public void add(string name, IState state)
            {
                collection.add(name, state);
            }
            public void remove(string name)
            {
                collection.remove(name);
                
            }

            public Machine(string name, IState state)
            {
                collection = new Finite(name, state);
                collection.current.enter(collection);
            }

            public void update()
            {
                if (!collection.IsValid)
                    return;

                switch (collection.Current.update(collection))
                {
                    case Mode.Next:
                        change();
                        break;
                    case Mode.Reset:
                        collection.next = null;
                        change();
                        break;
                }
            }

            private void change()
            {
                if (collection.next != null)
                {
                    collection.current.exit(collection);
                    collection.current = collection.next;
                    collection.current.enter(collection);
                    return;
                }

                collection.current.exit(collection);
                collection.current = collection.reset;
                collection.current.enter(collection);
            }
        }
    }

    public Finite(string name, IState state)
    {
        current = state;
        reset = state;
        states.Add(name, state);
    }
    public void set_reset(string name)
    {
        if (states.ContainsKey(name))
        {
            reset = states[name];
        }
    }

    public void add(string name, IState state)
    {
        states.Add(name, state);
    }
    public void remove(string name)
    {
        states.Remove(name);
    }
    public void set_next(string name)
    {
        if(states.ContainsKey(name))
        {
            next = states[name];
        }
    }

}
