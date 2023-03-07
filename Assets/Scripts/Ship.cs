using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour
{
    [HideInInspector] public Commander commander;
    [HideInInspector] public Planet planet;
    Rigidbody2D rb;
    [SerializeField] int power = 1;
    [SerializeField] int speed = 1;
    Planet destination = default;

    float curHealth = 0f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        Vector2 direction = ((Vector2)destination.transform.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        Vector2 diff = (Vector2)destination.transform.position - rb.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public void SetDestination(Planet _destination) {
        destination = _destination;
    }

    private void Die() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Planet otherPlanet = other.gameObject.GetComponent<Planet>();
        if(otherPlanet != null && otherPlanet.Equals(destination)){
            //collided with planets
            if(otherPlanet.commander == commander){
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
