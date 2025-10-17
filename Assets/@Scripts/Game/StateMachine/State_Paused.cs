//using ComBots.Global.UI;
//using ComBots.Inputs;
//using ComBots.Sandbox.Global.UI.Menu;
//using ComBots.Utils.StateMachines;
//using UnityEngine;

//namespace ComBots.Game.StateMachine
//{
//    public partial class GameStateMachine : MonoStateMachine<GameStateMachine>
//    {
//        public class State_Paused : State
//        {
//            private readonly GameStateMachine _stateMachine;

//            public State_Paused(GameStateMachine stateMachine) : base("Paused")
//            {
//                _stateMachine = stateMachine;
//            }

//            public override bool Enter(State previousState, object args = null)
//            {
//                bool canEnter = previousState is State_Playing;
//                if (canEnter)
//                {
//                    Debug.Log("Entering Paused state...");
//                    //InputManager.I.PushContext(_stateMachine._pauseContextData, );
//                    PauseMenu.Instance.Open();
//                }
//                return canEnter;
//            }

//            public override bool Exit(State nextState)
//            {
//                Debug.Log("Exiting Paused state...");
//                //InputManager.I.PopContext(_stateMachine._pauseContextData.contextName);
//                PauseMenu.Instance.Close();
//                return true;
//            }
//        }
//    }
//}