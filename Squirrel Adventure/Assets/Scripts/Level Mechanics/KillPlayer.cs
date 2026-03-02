using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GamepadRumbler.IsConnected() || Application.isMobilePlatform)
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);

            LevelManager.instance.RespawnPlayer();
        }
    }
}
