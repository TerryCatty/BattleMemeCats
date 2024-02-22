using UnityEngine;

public class Unit_StateMachine : MonoBehaviour
{
    public UnitAttack stateAttack { get; private set; }
    public UnitWalk stateWalk { get; private set; }
    public UnitState currentState;

    private UnitWarrior unit;
    public Unit_StateMachine(UnitWarrior unit)
    {
        this.unit = unit;
        InitializeMachine();
    }

    
    private void InitializeMachine()
    {

        stateAttack = new UnitAttack();
        stateWalk = new UnitWalk();
        currentState = new UnitState();

    }

    public void ChangeState(UnitState state)
    {
        currentState.Exit();

        currentState = state;
        currentState.InitState(unit);
    }

    public void DoAction()
    {
        currentState.UpdateAction();
    }
}
