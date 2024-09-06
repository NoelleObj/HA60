using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchscript : MonoBehaviour
{
    public playerAnimation playeranm;
    public playerC1 playerc1;
    public playerC0 playerc0;
    public Animator switchanimator;
    public Transform player;
    public Transform playermodel;
    public Transform playertarget;
    public float activationdistance;

    bool switching;
    bool switchend;

    public bool switched;
    // Start is called before the first frame update
    void Start()
    {
        switched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("switch") && Vector3.Distance(player.position, playertarget.position) < activationdistance && Mathf.Abs(player.position.y - playertarget.position.y) < 1f && switched == false)
        {
            switching = true;
        }
        if (switching)
        {
            StartCoroutine(wait());

            playerc0.unturnable = true;
            playerc1.enabled = false;

            player.position = Vector3.Slerp(player.position, playertarget.position, Time.deltaTime * 3f);
            playermodel.forward = Vector3.Slerp(playermodel.forward, playertarget.up, Time.deltaTime * 10f);

            if(Vector3.Distance(player.position, playertarget.position) < 0.1f && switchend == false)
            {
                if(switched == false)
                {
                    switched = true;
                    playeranm.switche();
                    switchanimator.SetTrigger("switch");
                    switchend = true;
                }
            }
        }

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);

        switching = false;
        playerc0.unturnable = false;
        playerc1.enabled = true;
        switchend = false;
    }
}
