namespace ComBots.Utils.StateMachines
{
    public abstract class State
    {
        public string Name { get; private set; }

        public State(string name)
        {
            Name = name;
        }

        public abstract bool Enter(State previousState, object args);

        public abstract bool Exit(State nextState);
    }
}