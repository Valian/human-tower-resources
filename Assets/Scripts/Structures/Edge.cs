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
    public bool IsActive { get { return state == EdgeState.ACTIVE; } }

    private EdgeState state = EdgeState.NOT_ACTIVE;
    private float currentHP = 0f;

    public void InitEdge(Node from, Node to)
    {
        from.NodeDamaged += onConnectedNodeDamaged;
        to.NodeDamaged += onConnectedNodeDamaged;
        transform.position = (from.transform.position + to.transform.position) / 2;
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
