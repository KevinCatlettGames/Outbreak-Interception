using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempDashUI : MonoBehaviour
{
    bool increment;
    [SerializeField] float dashCooldownTime;
    [SerializeField] float incrementSpeed;

    float currentDashCoolDownTime;

    [SerializeField] Image radialImage;
    [SerializeField] Image radialBackground;

    Rigidbody playerRigidBody;
    private void Start()
    {
       playerRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        currentDashCoolDownTime = dashCooldownTime;
        radialImage.fillAmount = 0;
        radialBackground.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && increment == false && playerRigidBody.velocity.magnitude > .2f)
        {
            increment = true;
            radialBackground.enabled = true;
        }

        if(increment)
        {
            currentDashCoolDownTime -= Time.deltaTime;
            radialImage.fillAmount += incrementSpeed * Time.deltaTime;
            if (currentDashCoolDownTime <= 0)
            {
                increment = false;
                radialBackground.enabled = false;
                currentDashCoolDownTime = dashCooldownTime;
                radialImage.fillAmount = 0;
            }
        }
    }
}
