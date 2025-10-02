using Fusion;
using UnityEngine;


public class Proyectile : NetworkBehaviour
{
    [Networked] private TickTimer life { set; get; }
    
    [SerializeField] private float _speed; 

    public void InitProyectile()
    {
        life = TickTimer.CreateFromSeconds(Runner, 5f);
    }

    public override void FixedUpdateNetwork()
    {
        if (!life.Expired(Runner))
        {
            transform.position += transform.forward * Runner.DeltaTime * _speed;
        }
        else
        {
            Runner.Despawn(Object);
        }
    }
}
