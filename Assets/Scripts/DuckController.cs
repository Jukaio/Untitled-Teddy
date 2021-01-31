using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : EnemyController
{
    private Dictionary<string, Finite.State> states = new Dictionary<string, Finite.State>();

    public class Entry : Finite.State
    {
        public Entry(GameObject context)
            : base(context)
        {
            Next = "Idle";
        }
        public override void enter(Finite collection)
        {
            Debug.Log("Entry Enter");
        }

        public override void exit(Finite collection)
        {
            Debug.Log("Entry Exit");
        }

        public override Finite.Mode update(Finite collection)
        {
            Debug.Log("Entry");
            collection.set_next(Next);
            return Finite.Mode.Next;
        }
    }

    public class Idle : Sentient, DamageSystem.Callback
    {
        DamageSystem damage;
        bool in_range = false;
        Finite.Mode mode;

        public Idle(GameObject context)
            : base(context, 1.0f/30.0f, 1.0f/15.0f)
        {
            damage = Context.GetComponent<DamageSystem>();
        }
        public override void enter(Finite collection)
        {
            damage.register(this);
            mode = Finite.Mode.Run;
        }

        public override void exit(Finite collection)
        {
            damage.unregister(this);
        }

        public void on_death()
        {

        }
        public void on_hit(Weapon with, DamageSystem.Type how)
        {

        }

        public override void sense()
        {
            in_range = Vector3.Distance(player.transform.position, Context.transform.position) < 8.0f;
        }

        public override void decide(Finite collection)
        {
            if (in_range)
            {
                Next = "Chase";
                collection.set_next(Next);
                mode = Finite.Mode.Next;
            }
        }

        public override Finite.Mode act(Finite collection)
        {

            return mode;
        }
    }

    public class Chase : Sentient, DamageSystem.Callback
    {
        Finite.Mode mode;
        bool is_reachable;
        List<Vector2Int> debug;
        Stack<Vector2Int> path;
        RoomGenerator.Room room;
        Rigidbody rb;
        public Chase(GameObject context)
            : base(context, 1.0f / 45.0f, 1.0f / 15.0f)
        {
            rb = Context.GetComponent<Rigidbody>();
            var room_index = RG.world.world_to_grid(Context.transform.position);
            room = RG.world.get(room_index.x, room_index.y);
        }

        public override Finite.Mode act(Finite collection)
        {
            if (path.Count == 0)
            {
                collection.set_next("idle");
                return Finite.Mode.Next;
            }

            var from = Context.transform.position;
            var to = room.grid_to_world(path.Peek(), true) - Context.transform.position;
            var angle = Vector3.SignedAngle(from.normalized, to.normalized, Vector3.up);

            rb.rotation = Quaternion.Euler(0.0f, angle + 45.0f, 0.0f);

            to.Normalize();
            to.y = 0;
            to *= 3.0f * Time.deltaTime;
            rb.MovePosition(rb.position + to);


            draw_rect(room.grid_to_world(path.Peek(), true), 1.0f, Color.yellow);
            if(Vector3.Distance(rb.position, room.grid_to_world(path.Peek(), true)) < 0.15f)
            {
                path.Pop();
            }
            foreach(var value in debug)
            {
                draw_rect(room.grid_to_world(value, true), 1.0f, Color.blue);
            }
            return mode;
        }

        public override void decide(Finite collection)
        {
            if (!is_reachable)
            {
                collection.set_next("Idle");
                mode = Finite.Mode.Next;
            }
        }

        public override void enter(Finite collection)
        {

            mode = Finite.Mode.Run;
        }

        public override void exit(Finite collection)
        {
        }

        public void on_death()
        {
        }

        public void on_hit(Weapon with, DamageSystem.Type how)
        {
        }
        private void draw_rect(Vector3 pos, float size, Color color)
        {
            Vector3 half = (size / 2.0f) * Vector2.one;
            Vector3 top_left = new Vector3(-half.x, 0.0f, half.y) + pos;
            Vector3 top_right = new Vector3(half.x, 0.0f, half.y) + pos;
            Vector3 bottom_left = new Vector3(-half.x, 0.0f, -half.y) + pos;
            Vector3 bottom_right = new Vector3(half.x, 0.0f, -half.y) + pos;

            Debug.DrawLine(top_left, top_right, color);
            Debug.DrawLine(top_right, bottom_right, color);
            Debug.DrawLine(bottom_right, bottom_left, color);
            Debug.DrawLine(bottom_left, top_left, color);
        }

        public override void sense()
        {
            var position = Context.transform.position;
            var target_position = player.transform.position;

            var grid = RG.as_bool(RG.world.world_to_grid(Context.transform.position));
            float distance = Vector3.Distance(player.transform.position, Context.transform.position);
            is_reachable = AStar.solvable(out List<Vector2Int> temp, grid, room.world_to_grid(position), room.world_to_grid(target_position));
            path = new Stack<Vector2Int>(temp);
            debug = temp;

            //is_reachable = is_reachable && distance < 8.0f;
        }
    }
    void Start()
    {
        initialise("Entry", new Entry(gameObject));
        add("Idle", new Idle(gameObject));
        add("Chase", new Chase(gameObject));
    }

}
