using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

    public int PlayerId;

    public PlayerLinearMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }

    void Start()
    {
        Movement = GetComponent<PlayerLinearMovement>();
        Attack = GetComponent<PlayerAttack>();
    }

}
