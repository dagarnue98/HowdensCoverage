using PublishSystem.Domain.Enums.StateManagement;

namespace PublishSystem.Domain.BusinessRules.StateManager
{
    public interface IStateManager
    {
        public State? CurrentState { get; }
        public State InitialiseState(State state);
        public State ChangeState(Trigger trigger);
        public string ToDotGraph();
    }
}
