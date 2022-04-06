using Microsoft.Extensions.Logging;
using PublishSystem.Domain.Enums.StateManagement;
using Stateless;
using Stateless.Graph;
using State = PublishSystem.Domain.Enums.StateManagement.State;

namespace PublishSystem.Domain.BusinessRules.StateManager
{
    public class StateManager : IStateManager
    {
        private readonly ILogger<StateManager> _logger;
        private StateMachine<State, Trigger> _stateMachine;

        public StateManager(ILogger<StateManager> logger)
        {
            this._logger = logger;
        }

        public State? CurrentState
        {
            get { return _stateMachine?.State; }
        }

        public State InitialiseState(State state)
        {
            this._stateMachine = new StateMachine<State, Trigger>(state);
            ConfigureStateMachine(_stateMachine);
            return _stateMachine.State;
        }

        public State ChangeState(Trigger trigger)
        {
            _stateMachine.Fire(trigger);
            return _stateMachine.State;
        }

        public string ToDotGraph()
        {
            if (_stateMachine == null) return string.Empty;
            return UmlDotGraph.Format(_stateMachine.GetInfo());
        }

        private void ConfigureStateMachine(StateMachine<State, Trigger> stateMachine)
        {
            stateMachine.Configure(State.Requested)
                        .Permit(Trigger.Send, State.QueuedForRendering)
                        .Permit(Trigger.Error, State.QueuedForRenderingError);
            stateMachine.Configure(State.QueuedForRendering)
                        .Permit(Trigger.Subscribe, State.Rendering)
                        .Permit(Trigger.Error, State.SubscribedToRenderingError)
                        .SubstateOf(State.Render);
            stateMachine.Configure(State.Rendering)
                        .Permit(Trigger.Completed, State.Rendered)
                        .Permit(Trigger.Error, State.RenderingError)
                        .SubstateOf(State.Render);
            stateMachine.Configure(State.Rendered)
                        .Permit(Trigger.Send, State.QueuedForEncoding)
                        .Permit(Trigger.Error, State.QueuedForEncodingError)
                        .SubstateOf(State.Render);
            stateMachine.Configure(State.QueuedForEncoding)
                        .Permit(Trigger.Subscribe, State.Encoding)
                        .Permit(Trigger.Error, State.SubscribedToEncodingError)
                        .SubstateOf(State.Encode);
            stateMachine.Configure(State.Encoding)
                        .Permit(Trigger.Completed, State.Encoded)
                        .Permit(Trigger.Error, State.EncodingError)
                        .SubstateOf(State.Encode);
            stateMachine.Configure(State.Encoded)
                        .Permit(Trigger.Send, State.Published)
                        .Permit(Trigger.Error, State.PublishedError)
                        .SubstateOf(State.Encode);
            stateMachine.Configure(State.Published)
                        .Permit(Trigger.Completed, State.EmailSent)
                        .Permit(Trigger.Error, State.EmailError)
                        .SubstateOf(State.Publish);
            stateMachine.Configure(State.EmailSent)
                        .SubstateOf(State.Publish);
            // Errors
            stateMachine.Configure(State.QueuedForRenderingError)
                        .SubstateOf(State.Error);
            stateMachine.Configure(State.SubscribedToRenderingError)
                        .SubstateOf(State.Error);
            stateMachine.Configure(State.RenderingError)
                        .SubstateOf(State.Error);
            stateMachine.Configure(State.QueuedForEncodingError)
                        .SubstateOf(State.Error);
            stateMachine.Configure(State.SubscribedToEncodingError)
                        .SubstateOf(State.Error);
            stateMachine.Configure(State.EncodingError)
                        .SubstateOf(State.Error);
            stateMachine.Configure(State.PublishedError)
                        .SubstateOf(State.Error);
            stateMachine.Configure(State.EmailError)
                        .SubstateOf(State.Error);
            stateMachine.Configure(State.Error);
            stateMachine.OnUnhandledTrigger((state, trigger) => LogUnhandledTrigger(state, trigger));
            stateMachine.OnTransitioned(transition => LogTransition(transition));
        }

        private void LogUnhandledTrigger(State state, Trigger trigger)
        {
            _logger.LogError($"Unhandled trigger occurred. State: {state}. Trigger: {trigger}");
        }

        private void LogTransition(StateMachine<State, Trigger>.Transition transition)
        {
            _logger.LogDebug($"OnTransitioned: {transition.Source} -> " +
                $"{transition.Destination} via {transition.Trigger}({string.Join(", ", transition.Parameters)})");
        }
    }
}
