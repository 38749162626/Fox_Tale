using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;
public class LSPlayer : MonoBehaviour
{
    public MapPoint currentPoint;

    public float moveSpeed = 10f;

    private bool levelLoading;

    public LSManager theManager;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentPoint.transform.position) < 0.1f && (Input.anyKeyDown || MobileInput.instance != null && MobileInput.instance.isJumpPressed) && !levelLoading)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f || VirtualJoystick.GetAxisRaw("Horizontal") > 0.5f)
            {
                if (currentPoint.right != null)
                {
                    SetNextPoint(currentPoint.right);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.5f || VirtualJoystick.GetAxisRaw("Horizontal") < -0.5f)
            {
                if (currentPoint.left != null)
                {
                    SetNextPoint(currentPoint.left);
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0.5f || VirtualJoystick.GetAxisRaw("Vertical") > 0.5f)
            {
                if (currentPoint.up != null)
                {
                    SetNextPoint(currentPoint.up);
                }
            }
            else if (Input.GetAxisRaw("Vertical") < -0.5f || VirtualJoystick.GetAxisRaw("Vertical") < -0.5f)
            {
                if (currentPoint.down != null)
                {
                    SetNextPoint(currentPoint.down);
                }
            }
            else if (currentPoint.isLevel && currentPoint.levelToLoad != null && !currentPoint.isLocked)
            {
                LSUIController.instance.ShowInfo(currentPoint);

                if (Input.GetButtonDown("Jump") || MobileInput.instance != null && MobileInput.instance.isJumpPressed)
                {
                    levelLoading = true;

                    theManager.LoadLevel();
                }
            }
        }
    }

    public void SetNextPoint(MapPoint nextpoint)
    {
        currentPoint = nextpoint;

        LSUIController.instance.HideInfo();
    }
}
