using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    public Image healthBar;
    public Image jetFuelBar;
    public Image dashCooldownBar;

    public Image damageImage;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public float flashSpeed = 5f;

    public PlayerMoveV3 playerScript;

    private bool dashed;

    //private void Start()
    //{
    //    StartCoroutine(DashCooldownBar());
    //}

    private void Update()
    {
        healthBar.fillAmount = GameManager.instance.playerHP / GameManager.instance.maxPlayerHP;

        jetFuelBar.fillAmount = playerScript.jetFuel / playerScript.maxJetpackFuel;

        if (playerScript.dash && !dashed)
        {
            StartCoroutine(DashCooldownBar());
            dashed = true;
        }

        if (GameManager.instance.playerDamaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    IEnumerator DashCooldownBar()
    {
        dashCooldownBar.fillAmount = 0;
        float timeRunning = 0;
        while (true)
        {
            timeRunning += Time.deltaTime;
            dashCooldownBar.fillAmount = timeRunning / playerScript.dashCooldown;
            //print(timeRunning + " / " + playerScript.dashCooldown + " = " + dashCooldownBar.fillAmount);

            if(dashCooldownBar.fillAmount >= 1)
            {
                dashed = false;
                yield break;
            }
            yield return null;
        }

    }
}
