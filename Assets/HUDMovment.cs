using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMovment : MonoBehaviour {

    RectTransform myTransform;
    Vector2 targetPosition;
    public float moveAmmount = 20;

	// Use this for initialization
	void Start () {
        myTransform = GetComponent<RectTransform>();
        StartCoroutine(MoveHUD());
    }
	
	// Update is called once per frame
	void Update () {
        print(Input.GetAxis("Mouse X"));
        print(Input.GetAxis("Mouse Y"));

        targetPosition.x = (-Input.GetAxis("Mouse X"))*moveAmmount;
        targetPosition.y = (-Input.GetAxis("Mouse Y"))*moveAmmount;
    }

    IEnumerator MoveHUD()
    {
        while (true)
        {
            myTransform.localPosition = Vector2.Lerp(myTransform.localPosition, targetPosition, Time.deltaTime);
            yield return null;
        }

    }
}
