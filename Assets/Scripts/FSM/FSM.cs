public class FSM<T>
{
    private States<T> currentState;

    public FSM() { }

    public FSM(States<T> init)
    {
        if (init != null)
            SetInit(init);
    }

    public void SetInit(States<T> init)
    {
        this.currentState = init;
        this.currentState.Awake();
    }

    public void OnUpdate()
    {
        currentState.Execute();
    }

    public void Transition(T input)
    {
        States<T> newState = currentState.GetState(input);

        if (newState == null) return;

        currentState.Sleep();

        newState.Awake();

        currentState = newState;
    }

}
