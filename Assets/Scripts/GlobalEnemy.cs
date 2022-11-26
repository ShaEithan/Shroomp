using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemy : MonoBehaviour
{
    public int Health = 100;
    public int damageTaken = 20;
    public int fireWeakness,iceWeakness;
    public int enemyDamage = 1;
    public float dotRate = 3f;
    public int dotAmmount = 5;
    public float dotPulseDuration= 12.5f;
    public float invulTime = 0.5f;
    private float invulTimeStorage;
    //Status handle script
    private StatusEffectController statusHandler;
    private SpriteRenderer sprite;
    private float defaultSpeed;



    private float invincibleTime;
    private bool isInvincible = false;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        defaultSpeed = GetComponent<AIPath>().maxSpeed;
        statusHandler = FindObjectOfType<StatusEffectController>();
        dotAStorage = dotAmmount;
        dotDurationStorage = dotPulseDuration;
        invulTimeStorage = invulTime;
    }

    // Update is called once per frame
    void Update()
    {
        invulTime -= Time.deltaTime;
    }
    private float iceRate = 0.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && invulTime<=0)
        {
            if (collision.gameObject.GetComponent<ShroompController>().isDashing)
            {
                ChangeHealth(-damageTaken);
                if (statusHandler.fireUp)
                {
                    CancelInvoke("FireDot");
                    dotAmmount = dotAStorage;
                    InvokeRepeating("FireDot", 0f, dotRate);

                }
                if (statusHandler.iceUp)
                {
                    CancelInvoke("IceDot");
                    dotPulseDuration = dotDurationStorage;
                    InvokeRepeating("IceDot", 0f, iceRate);
                }
                invulTime = invulTimeStorage;
            }
                
            if (!collision.gameObject.GetComponent<ShroompController>().isDashing)
                collision.gameObject.GetComponent<ShroompController>().ChangeHealth(-enemyDamage);
           
        }
    }
    private void ChangeHealth(int i)
    {
        Health += i;
        if (Health <= 0)
            Destroy(transform.gameObject);
    }
    private int dotAStorage;
    private void FireDot()
    {
        Debug.Log("FIRE");
        dotAmmount--;
        if(sprite.color != new Color(1f, 0f, 0f, 1f))
        sprite.color = new Color(1f, 0f, 0f, 1f);
        else if(sprite.color == new Color(1f, 0f, 0f, 1f))
            sprite.color = new Color(1f, 0.4f, 0.2f, 1f);

        ChangeHealth(-5 * fireWeakness);
        if (dotAmmount == 0)
        {
            sprite.color = Color.white;
            dotAmmount = dotAStorage;
            CancelInvoke("FireDot");
        }
    }
    private float dotDurationStorage;
    private void IceDot()
    {
        Debug.Log("Ice");

        sprite.color = new Color(0f, 0f, 1f, 1f);
        FindObjectOfType<AIPath>().maxSpeed = Mathf.Clamp(defaultSpeed - iceWeakness, 1, defaultSpeed);
        dotPulseDuration -= iceRate;
        if(dotPulseDuration <= 0)
        {
            FindObjectOfType<AIPath>().maxSpeed = defaultSpeed;
            sprite.color = Color.white;
            dotPulseDuration = dotDurationStorage;
            CancelInvoke("IceDot");
        }
    }
    /**
    void dotHandler()
    {
        if (dotActive && !dotIsInvul)
        {

            if (statusHandler.fireUp)
            {
                sprite.color = new Color(1f, 0f, 0f, sprite.color.a);
                ChangeHealth(-5 * fireWeakness);
                dotInvulTime = dotInvulSetter;
                dotIsInvul = true;
                colorChangeDelay = colorChangeSetter;
                isColorChanging = true;
            }


        }
        if (dotActive)
        {
            if (statusHandler.iceUp)
            {
                colorChangeDelay -= Time.deltaTime;
                if (colorChangeDelay < 0)
                    sprite.color = new Color(0f, 0f, 1f, sprite.color.a);
                //maxVelocity = Mathf.Clamp(6 - iceWeakness, 1, 6);
                FindObjectOfType<AIPath>().maxSpeed = Mathf.Clamp(defaultSpeed - iceWeakness, 1, defaultSpeed);
            }
            dotTime -= Time.deltaTime;
            if (dotTime < 0)
            {
                dotActive = false;
                //Restore max velocity and then color
                FindObjectOfType<AIPath>().maxSpeed = defaultSpeed;
                sprite.color = Color.white;
            }
        }
        if (dotIsInvul)
        {
            dotInvulTime -= Time.deltaTime;
            if (dotInvulTime < 0)
                dotIsInvul = false;
        }

    }
    **/
}
