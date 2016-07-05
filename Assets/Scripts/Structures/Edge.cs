using UnityEngine;
using System.Collections;

public enum EdgeState
{
    ACTIVE,
    NOT_ACTIVE
}


public class Edge : MonoBehaviour {

    [Range(0, 100)]
    public int MaxHP;
    public int NodeId { get; private set; }
    public bool IsActive { get { return state == EdgeState.ACTIVE; } }

    private EdgeState state = EdgeState.NOT_ACTIVE;
    private float currentHP = 0f;

    public void InitEdge(Node from, Node to, int nodeId)
    {
        from.NodeDamaged += onConnectedNodeDamaged;
        to.NodeDamaged += onConnectedNodeDamaged;
        this.NodeId = nodeId;
    }

    public void FortifyEdge(float amountAdded)
    {
        currentHP += amountAdded;
        if (this.currentHP >= MaxHP)
        {
            this.currentHP = MaxHP;
            if (!IsActive)
            {
                this.finishConstruction();
            }
        }
    }

    private void onConnectedNodeDamaged(Node node, float damageAmount)
    {
        this.takeDamage(damageAmount);
    }

    private void takeDamage(float damageAmount)
    {
        this.currentHP -= damageAmount;
        if (this.currentHP <= 0)
        {
            this.currentHP = 0;
            if (this.IsActive)
            {
                this.destroy();
            }
        }
    }

    private void destroy()
    {
        this.state = EdgeState.NOT_ACTIVE;
    }

    private void finishConstruction()
    {
        this.state = EdgeState.ACTIVE;
    }

    
}
