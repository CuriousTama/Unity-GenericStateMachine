public interface IState
{
    public void Enter(StateMachine stateMachine);
    public void Exit(StateMachine stateMachine);
    public void PreUpdate(StateMachine stateMachine);
    public void Update(StateMachine stateMachine);
    public void LateUpdate(StateMachine stateMachine);
    public void FixedUpdate(StateMachine stateMachine);
}
