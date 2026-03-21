using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using Terresquall;
using Unity.VisualScripting;
using UnityEngine;

public class Smasher : MonoBehaviour
{
    public float SmashSpeed, UpSpeed;
    public GameObject Trigger;

    private CheckPlayer checkPlayer;
    private Vector3 startPos;
    private bool isUping;

    void Start()
    {
        startPos = transform.position;

        Trigger.transform.parent = null;

        checkPlayer = Trigger.GetComponent<CheckPlayer>();
    }

    
    void Update()
    {
        if (checkPlayer.playerEnterTrigger && !isUping)
        {
            StartCoroutine(SmasherCo());
        }
    }

    private IEnumerator SmasherCo()
    {
        while (Vector3.Distance(transform.position, Trigger.transform.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, Trigger.transform.position, SmashSpeed * Time.deltaTime);
            yield return null;
        }
        if (GamepadRumbler.IsConnected() || Application.isMobilePlatform)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        yield return new WaitForSeconds(1);
        isUping = true;
        while (Vector3.Distance(transform.position, startPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, UpSpeed * Time.deltaTime);
            yield return null;
        }
        isUping = false;
    }
}
