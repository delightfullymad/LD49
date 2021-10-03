using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State {Idle, RunTowards, Attack, RunAway, Dead, Hit};

    public float health = 3f;

    public State currentState = State.Idle;
    public Rigidbody rb;
    private Vector3 m_Velocity = Vector3.zero;
    public Transform target;
    public float turnSpeed = 1f;
    public float moveSpeed = 2f;
    public float attackRate = 2f;
    public float nextAttack;
    public float attackRange = 2f;
    public float damage = 1f;
    public Transform modelObj;
    public SphereCollider lookRange;
    public GameObject explodeParticle;
    public ParticleSystem hitParticle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude > 0.1f && currentState != State.Hit)
        {
            modelObj.GetComponent<Animator>().SetBool("run", true);
        }
        else
        {
            modelObj.GetComponent<Animator>().SetBool("run", false);
        }


        if (target != null && (currentState == State.RunTowards || currentState == State.Attack) && Time.time > nextAttack)
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
            float dist = Vector3.Distance(target.position, transform.position);
            //Debug.Log(dist);
            if (dist > attackRange)
            {
                rb.velocity = (target.transform.position - transform.position).normalized * moveSpeed;
                
                //Debug.Log("Moving");
            }
            else if (Time.time > nextAttack)
            {
                Attack();
                rb.velocity = Vector3.zero;
                nextAttack = Time.time + attackRate;
            }
        }
        if (health <= 0f && currentState != State.Dead)
        {
            modelObj.GetComponent<Animator>().SetTrigger("dead");
            currentState = State.Dead;
            rb.velocity = Vector3.zero;
            Invoke("Explode", 3f);
        }
    }

    public void Attack()
    {
        modelObj.GetComponent<Animator>().SetTrigger("attack");
        
    }

    public void AttackHit()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        if (target != null && dist < 3f)
        {
            target.GetComponent<Player>().TakeDamage(damage);
        }
    }



    public void Damage(float hitDamage, Vector3 pos)
    {
        if (currentState != State.Dead)
        {
            currentState = State.Hit;
            rb.velocity = Vector3.zero;
            health -= hitDamage;
            modelObj.GetComponent<Animator>().SetTrigger("hit");
            rb.AddForce((transform.position - pos) * 25, ForceMode.Impulse);
            //rb.velocity = (transform.position - pos) * 10;
            hitParticle.Play();
        }

    }

    public void Explode()
    {
        Instantiate(explodeParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
