using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertStatus : MonoBehaviour {

    public bool alerted = false;

    public void SetAlert()
    {
        alerted = true;
        gameObject.layer = LayerMask.NameToLayer("AlertEnemy");
    }

    public void SetNotAlert()
    {
        alerted = false;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}
