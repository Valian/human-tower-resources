using UnityEngine;

public class PlayerBase : MonoBehaviour {

    public int PlayerId;

    public PlayerLinearMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }

    void Awake()
    {
        Movement = GetComponent<PlayerLinearMovement>();
        Attack = GetComponent<PlayerAttack>();
    }

}
