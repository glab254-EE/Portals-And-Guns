using UnityEngine;
using UnityEngine.AI;
using com.cyborgAssets.inspectorButtonPro;
using System.Collections.Generic;
using System;
[RequireComponent(typeof(NavMeshAgent))]
public class AgentMover : MonoBehaviour
{
    [SerializeField] private List<Vector3> targets;
    [SerializeField] private Vector3 target;
    [SerializeField] private int MaxHP = 5;
    [SerializeField] internal int HP = 5;
    
    [Header("Agent Navigation Settings")]
    [SerializeField] private AgentState CurrentBehaivor = AgentState.normal; 
    [SerializeField] private int EnemyMask;
    [SerializeField] private float DefaultEnemyCost = 1.5f;
    [SerializeField] private float AvoidingEnemyCost = 3f;
    [SerializeField] private int Wall;
    [SerializeField] private float DefaultWallCost = 1.5f;
    [SerializeField] private float HidingWallCost = 1f;
    [SerializeField] private float DefaultWalcableCost = 1f;
    [SerializeField] private float HidingWalcableCost = 3f;
    AgentState oldstate = AgentState.normal;
    NavMeshAgent agent;
    int lastpoint = 0;
    bool stopped = false;

    [ProButton]
    private void StartAgentMovement()
    {
        stopped = false;
        agent.SetDestination(target);        
    }
    [ProButton]
    private void StopAgentMovement()
    {
        stopped = true;
        agent.SetDestination(transform.position);        
    }
    private void UpdateTarget()
    {
        lastpoint = lastpoint + 1 < targets.Count ? lastpoint + 1 : 0;
        target = targets[lastpoint];
        agent.SetDestination(target);
    }
    private void UpdateBehaivor()
    {
        if (CurrentBehaivor == AgentState.normal)
        {
            agent.SetAreaCost(EnemyMask,DefaultEnemyCost);
            agent.SetAreaCost(Wall,DefaultWallCost);
            agent.SetAreaCost(0,DefaultWalcableCost);
        } else if (CurrentBehaivor == AgentState.hiding)
        {
            agent.SetAreaCost(EnemyMask,DefaultEnemyCost);
            agent.SetAreaCost(Wall,HidingWallCost);
            agent.SetAreaCost(0,HidingWalcableCost);
        } else if (CurrentBehaivor == AgentState.avoiding)
        {
            agent.SetAreaCost(EnemyMask,AvoidingEnemyCost);
            agent.SetAreaCost(Wall,DefaultWallCost);
            agent.SetAreaCost(0,DefaultWalcableCost);
        }
        stopped = false;
        agent.SetDestination(target);     
        oldstate = CurrentBehaivor;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = targets[lastpoint];
        agent.SetDestination(target);
        UpdateBehaivor();
        HP = MaxHP;
    }
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !stopped)
        {
            UpdateTarget();
        }
        if (HP <= Mathf.RoundToInt(MaxHP/2f) && CurrentBehaivor != AgentState.avoiding)
        {
            CurrentBehaivor = AgentState.avoiding;
        }
        if (CurrentBehaivor != oldstate) UpdateBehaivor();
    }
    #if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Color oldcol = Gizmos.color;
        Gizmos.color = Color.cyan;
        foreach(Vector3 t in targets){
            Gizmos.DrawSphere(t,0.5f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target,.75f);
        Gizmos.color = oldcol;
    }
    #endif
}
public enum AgentState {
    normal,
    hiding,
    avoiding,
}
