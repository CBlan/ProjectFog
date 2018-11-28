using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeVisualControl : MonoBehaviour {

    public AlertStatus alert;
    public float openDistance = 5;

    private GameObject player;
    private Animator anim;

    private void Start()
    {
        player = GameManager.instance.player;
        anim = GetComponent<Animator>();
        anim.SetFloat("Open", 0);
    }

    void Update () {
        if (alert.alerted)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < openDistance)
            {
                //if (anim.GetFloat("Open") <= 1)
                //{
                //    anim.SetFloat("Open", anim.GetFloat("Open") + 0.1f);
                //}
                //open
                if (!anim.GetBool("Open 0"))
                {
                    Fabric.EventManager.Instance.PostEvent("Enemy/Melee/Drill", Fabric.EventAction.PlaySound, gameObject);
                    anim.SetBool("Open 0", true);
                }
            }
            else
            {
                //if (anim.GetFloat("Open") >= 0)
                //{
                //    anim.SetFloat("Open", anim.GetFloat("Open") - 0.1f);
                //}
                if (anim.GetBool("Open 0"))
                {
                    Fabric.EventManager.Instance.PostEvent("Enemy/Melee/Drill", Fabric.EventAction.StopSound, gameObject);
                    anim.SetBool("Open 0", false);
                }
            }
        }
	}

    private void OnDisable()
    {
        if (Fabric.EventManager.Instance != null)
        {
            Fabric.EventManager.Instance.PostEvent("Enemy/Melee/Drill", Fabric.EventAction.StopSound, gameObject);
        }
    }
}
