using UnityEngine;
using System.Collections;

public class PlayerBuilding : MonoBehaviour {

    public int BuildSpeedPerSecond;
    public bool CanBuild { get { return !movement.IsMoving; } }

    private PlayerLinearMovement movement;
    

    void Start()
    {
        this.movement = this.GetComponent<PlayerLinearMovement>();
    }
    
    public void FortifyNode(Node targetNode)
    {
        if(CanBuild)
        {
            var manager = this.GetComponentInParent<NodeManager>();
            var hpRestored = BuildSpeedPerSecond * Time.deltaTime;
            manager.FortifyEdge(movement.CurrentNode.NodeId, targetNode.NodeId, hpRestored);
        }
    }
}
