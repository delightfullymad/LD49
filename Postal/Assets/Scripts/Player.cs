using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static public Player player;

    public float health;
    public Transform mouse;
    public Transform movementAxisObj;
    public float defaultMoveSpeed = 5f;
    public float sprintSpeed = 7f;
    float moveSpeed = 5f;
    Vector3 moveDir;
    Rigidbody rb;
    public Transform charModel;
    Animator anim;
    public SphereCollider hitCollider;
    float defaultDamage = 1f;
    public float currentDamage = 1f;
    public float attackRate = 2f;
    public float nextAttack;

    public bool holdingThrowable;
    public bool holdingWeapon;
    public Transform handSlot;
    public GameObject weapon;
    public int weaponDurability;
    public bool invincible;


    private void Awake()
    {
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = charModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //anim.SetFloat("moveSpeed", moveSpeed);
        


        
        anim.SetBool("holdingThrow", holdingThrowable);
        anim.SetBool("holdingWeapon", holdingWeapon);
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        //Ta Daaa
        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle-45f, 0f));

        //moveDir.x = Input.GetAxisRaw("Horizontal");
        //moveDir.y = Input.GetAxisRaw("Vertical");


        Vector3 moveX = movementAxisObj.transform.right * Input.GetAxisRaw("Horizontal") * moveSpeed;
        Vector3 moveZ = movementAxisObj.transform.forward * Input.GetAxisRaw("Vertical") * moveSpeed;
        moveDir = moveX + moveZ;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouse.position.x >= transform.position.x && Input.GetAxisRaw("Horizontal") <0f)
        {
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = defaultMoveSpeed;
            }
            anim.SetFloat("moveSpeed", -defaultMoveSpeed);
        }
        else if (mouse.position.x < transform.position.x && Input.GetAxisRaw("Horizontal") > 0f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = defaultMoveSpeed;
            }
            anim.SetFloat("moveSpeed", -defaultMoveSpeed);
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = defaultMoveSpeed;
            }
            anim.SetFloat("moveSpeed", defaultMoveSpeed);
        }


        if (moveDir.magnitude > 0.1f)
        {
            anim.SetBool("run", true);
        }
        else 
        {
            anim.SetBool("run", false);
        }

        

        if (!holdingThrowable && Input.GetMouseButtonDown(0) && Time.time > nextAttack)
        {
            charModel.GetComponent<Animator>().SetTrigger("attack");
            nextAttack = Time.time + attackRate;
        }
        if ((holdingThrowable || holdingWeapon) && Input.GetMouseButtonDown(1) && Time.time > nextAttack)
        {
            charModel.GetComponent<Animator>().SetTrigger("throw");
            //Throw();
            nextAttack = Time.time + attackRate;
        }


    }

    public IEnumerator Swing()
    {
        Debug.Log("Hitting");
        hitCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        hitCollider.enabled = false;
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log("Hit for " + damage);
        if(!invincible)
        {
            invincible = true;
            anim.SetTrigger("hit");
            health -= damage;
            Invoke("EndInvincible", 2f);
        }
        
    }



    public void Equip(float damage, int durability)
    {
        currentDamage = damage;
        holdingWeapon = true;
        weaponDurability = durability;
    }

    public void Holding()
    {
        holdingThrowable = true;
    }

    public void Throw()
    {
        holdingThrowable = false;
        holdingWeapon = false;
        weapon.transform.parent = null;
        weapon.GetComponent<Weapon>().pickedUp = false;
        weapon.GetComponent<Weapon>().thrown = true;
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<Rigidbody>().AddForce(transform.forward*10, ForceMode.Impulse);
        weapon.GetComponent<Rigidbody>().AddRelativeTorque(weapon.transform.up * 10f);
        weapon.layer = 9;

        weapon = null;
    }

    public void WeaponBreak()
    {
        currentDamage = defaultDamage;
        holdingWeapon = false;
        Destroy(weapon);
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void EndInvincible()
    {
        invincible = false;
    }
}
