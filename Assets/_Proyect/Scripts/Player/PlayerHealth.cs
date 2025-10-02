using UnityEngine;
using Fusion;

public class PlayerHealth : NetworkBehaviour
{
    [Networked] public int health { get; set; } = 100;

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputPlayer inputPlayer))
        {
            if (inputPlayer.buttons.IsSet(NetworkInputPlayer.MOUSE_BUTTON_1))
            {
                TakeDamage(10);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Player {name} takes damage {damage}, hasAutho: {HasStateAuthority}");

        if(!HasStateAuthority) return;

        health -= damage;

        Debug.Log($"Player {name} has {health} health");
    }
}
