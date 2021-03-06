using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour, DamageSystem.Callback
{
    // Start is called before the first frame update
    [SerializeField] float speed = 2.0f;
    [SerializeField] RoomGenerator RG;
    [SerializeField] SwordHandle weapon;
    [SerializeField] private Animator anim;

    private const int CURRENT = 0;
    private const int PREVIOUS = 1;

    DamageSystem damage;

    private Vector3[] movement_axes = { new Vector3(), new Vector3() };

    private float view_angle = 0.0f;
    private Rigidbody rb;

    private void Awake()
    {
        /* Global for player access */
        EnemyController.player = gameObject;
        weapon = GetComponent<SwordHandle>();
        rb = GetComponent<Rigidbody>();
        damage = GetComponent<DamageSystem>();
        damage.register(this);
    }
    void Start()
    {
        transform.position = RG.world.grid_to_world(RG.PlayerSpawnPosition, true);
        transform.Translate(Vector3.up * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(movement_axes[CURRENT].magnitude > 0.0f)
            movement_axes[PREVIOUS] = movement_axes[CURRENT];

        view_angle = Vector3.SignedAngle(Vector3.forward, movement_axes[PREVIOUS], Vector3.up);

        movement_axes[CURRENT].x = Input.GetAxis("Horizontal");
        movement_axes[CURRENT].z = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        var use = movement_axes[CURRENT];
        use.Normalize();

        anim.SetFloat("velocity", use.magnitude);
        rb.velocity = use * speed;
        transform.localRotation = Quaternion.Euler(0, view_angle, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            damage.damage(10);
            if(weapon.HasWeapon) weapon.lose_weapon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible") && (!weapon.HasWeapon))
        {
            weapon.get_weapon(other.gameObject.name);
            other.gameObject.SetActive(false);
        }
    }

    public void on_hit(Weapon with, DamageSystem.Type how)
    {
    }

    public void on_death()
    {
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }
}
