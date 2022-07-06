namespace GenericStateMachine
{
    public abstract class State
    {
        public StateMachine stateMachine { get; private set; }

        public void SetStateMachine(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        /// <summary>
        /// <para> Called when the state is created. </para>
        /// <para> Can get ride of base.Init() </para>
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// <para> Called after Init. </para>
        /// <para> Need to keep base.Enter() it call RegisterInput() </para>
        /// </summary>
        public virtual void Enter()
        {
            RegisterInput();
        }

        /// <summary>
        /// <para> Called when the state is destroyed. </para>
        /// <para> Need to keep base.Exit() it call UnregisterInput() </para>
        /// </summary>
        public virtual void Exit()
        {
            UnregisterInput();
        }

        /// <summary>
        /// <para> Called when the is resumed (stacked beaviour). </para>
        /// <para> Need to keep base.Resume() it call RegisterInput() </para>
        /// </summary>
        public virtual void Resume()
        {
            RegisterInput();
        }

        /// <summary>
        /// <para> Called when the is paused (stacked beaviour). </para>
        /// <para> Need to keep base.Pause() it call UnregisterInput() </para>
        /// </summary>
        public virtual void Pause()
        {
            UnregisterInput();
        }

        /// <summary>
        /// <para> Called before the update. (each frame) </para>
        /// <para> Can get ride of base.PreUpdate() </para>
        /// </summary>
        public virtual void PreUpdate()
        {
        }

        /// <summary>
        /// <para> Called each frame. </para>
        /// <para> Can get ride of base.Update() </para>
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// <para> Called after the update. (each frame) </para>
        /// <para> Can get ride of base.LateUpdate() </para>
        /// </summary>
        public virtual void LateUpdate()
        {
        }

        /// <summary>
        /// <para> Called each time the physic is updated </para>
        /// <para> Can get ride of base.FixedUpdate() </para>
        /// </summary>
        public virtual void FixedUpdate()
        {
        }

        /// <summary>
        /// <para> Called in Enter() and Resume() </para>
        /// <para> Can get ride of base.RegisterInput() </para>
        /// </summary>
        public virtual void RegisterInput()
        {
        }

        /// <summary>
        /// <para> Called in Exit() and Pause() </para>
        /// <para> Can get ride of base.RegisterInput() </para>
        /// </summary>
        public virtual void UnregisterInput()
        {
        }
    }
}