namespace GenericStateMachine
{
    public abstract class State
    {
        public StateMachine stateMachine { get; private set; }

        public void SetStateMachine(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Init()
        {
        }

        public virtual void Enter()
        {
            RegisterInput();
        }

        public virtual void Exit()
        {
            UnregisterInput();
        }

        public virtual void Resume()
        {
            RegisterInput();
        }

        public virtual void Pause()
        {
            UnregisterInput();
        }

        public virtual void PreUpdate()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void LateUpdate()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void RegisterInput()
        {
        }

        public virtual void UnregisterInput()
        {
        }
    }
}