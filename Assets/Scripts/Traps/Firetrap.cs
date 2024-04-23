using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField]private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRen;

    [Header ("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private bool triggered; //when the trap gets triggered
    private bool active; // when the trap is active and can hurt the player

    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerHealth != null && active)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();

            if(!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            if(active)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth = null;
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        // turn the sprite red to notify the player
        triggered = true;
        spriteRen.color = Color.red;

        //Wait for delay, activate trap, turn on animation, return color back to nomal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        active = true;
        spriteRen.color = Color.white; // turn the sprite back to its initial color
        anim.SetBool("activated", true);

        //Wait until X seconds, deactivate trap and reset all variables
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
