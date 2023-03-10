using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour
{
    [HideInInspector] public Commander commander;
    [HideInInspector] public Planet planet = default;

    Rigidbody2D rb;

    [SerializeField] int power = 1;
    [SerializeField] float speed = 1;
    [SerializeField] SpriteRenderer sprite = default;

    [SerializeField] Planet destination = default;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        planet = transform.parent.GetComponent<Planet>();
        sprite.sprite = commander.spriteUnit;
        commander.shipCount++;
    }

    [SerializeField] float jumpDelay = 0.2f;
    [SerializeField] float jumpSpeed = 2f; //2 units persecond
    [SerializeField] float jumpDuration = 0.1f;
    [SerializeField] float nextJumpTime = 0f;
    private void FixedUpdate() {
        rb.velocity = Vector2.zero;
        if(GameManager.gamePaused) {
            nextJumpTime += Time.fixedDeltaTime;
            return;
        }
        Normal();
        // Jumpy();
    }

    void Normal(){
        Vector2 direction = ((Vector2)destination.transform.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        Vector2 diff = (Vector2)destination.transform.position - rb.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    void Jumpy(){
        if(Time.time >= nextJumpTime){
            if(destination == null) return;
            StartCoroutine(Jump());
            nextJumpTime = Time.time + jumpDuration + jumpDelay + Random.Range(-.1f, .1f);
        }
    }

    IEnumerator Jump(){
        float percentage = 0f;
        while(percentage < 1f){
            if(GameManager.gamePaused) yield return new WaitForFixedUpdate();
            Vector2 direction = ((Vector2)destination.transform.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * jumpSpeed * Time.fixedDeltaTime);

            Vector2 diff = (Vector2)destination.transform.position - rb.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

            percentage += Time.fixedDeltaTime / jumpDuration;
            yield return new WaitForFixedUpdate();
        }
    }

    public void SetDestination(Planet _destination) {
        destination = _destination;
    }

    private void Die() {
        commander.shipCount--;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Planet otherPlanet = other.gameObject.GetComponent<Planet>();
        if(otherPlanet != null && otherPlanet.Equals(destination)){
            //collided with planets
            if(otherPlanet.commander.Equals(commander)){
                otherPlanet.resourceManager.GatherResources(power);
            }else{
                if(otherPlanet.resourceManager.TryConquer(power)){
                    otherPlanet.ChangeCommander(commander);
                }
            }
            Die();
        }
    }
}
