using ComBots.Game.Players;
using ComBots.Game.Portals;
using ComBots.Game.Worlds.Rooms;
using ComBots.Global.Audio;
using ComBots.Global.UI;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.Sandbox.Global.UI.Menu;
using ComBots.Utils.StateMachines;
using UnityEngine;

namespace ComBots.Game.StateMachine
{
    public partial class GameStateMachine : MonoStateMachine<GameStateMachine>
    {
        public class State_AreaTransition : State
        {
            private readonly GameStateMachine _stateMachine;

            public State_AreaTransition(GameStateMachine stateMachine) : base("Area Transition")
            {
                _stateMachine = stateMachine;
            }

            public override bool Enter(State previousState, object args)
            {
                bool canEnter = true;
                if (PauseMenu.Instance.IsOpen)
                {
                    canEnter = false;
                }
                if (args == null)
                {
                    canEnter = false;
                    MyLogger<State_AreaTransition>.StaticLog("Invalid arguments for area transition.");
                }
                if (args is not State_AreaTransition_ToRoom_Args && args is not State_AreaTransition_ToPortal_Args)
                {
                    canEnter = false;
                    MyLogger<State_AreaTransition>.StaticLog("Invalid arguments type for area transition.");
                }

                if (canEnter)
                {
                    if (args is State_AreaTransition_ToPortal_Args portalArgs)
                    {
                        portalArgs.Player.Controller.FreezeMovement = true;

                        GlobalUIRefs.I.StageDoorTransitions.DoTransition(
                            onTransitionMidPoint: () =>
                            {
                                // Change background music
                                AudioManager.I.TrySetBackgroundMusic(portalArgs.NewBackgroundMusic);
                                // Teleport player
                                portalArgs.Portal.TeleportPlayer(portalArgs.Player);
                            },
                            onTransitionEnd: () =>
                            {
                                portalArgs.Player.Controller.FreezeMovement = false;
                                _stateMachine.SetState<State_Playing>(null);
                            },
                            bannerLabel: portalArgs.AreaName
                        );
                    }
                    else if (args is State_AreaTransition_ToRoom_Args roomArgs)
                    {
                        roomArgs.Player.Controller.FreezeMovement = true;

                        GlobalUIRefs.I.StageDoorTransitions.DoTransition(
                            onTransitionMidPoint: () =>
                            {
                                // Change background music
                                AudioManager.I.TrySetBackgroundMusic(roomArgs.Room.clip);
                                // Teleport player
                                roomArgs.Room.TeleportPlayerToRoom(roomArgs.Player);
                            },
                            onTransitionEnd: () =>
                            {
                                roomArgs.Player.Controller.FreezeMovement = false;
                                _stateMachine.SetState<State_Playing>(null);
                            },
                            bannerLabel: roomArgs.Room.bannerName
                        );
                    }


                }

                return canEnter;
            }

            public override bool Exit(State nextState)
            {
                return true;
            }
        }
    }

    public class State_AreaTransition_ToRoom_Args
    {
        public Room Room { get; private set; }
        public Player Player { get; private set; }

        public State_AreaTransition_ToRoom_Args(Room room, Player player)
        {
            Room = room;
            this.Player = player;
        }
    }

    public class State_AreaTransition_ToPortal_Args
    {
        public Portal Portal { get; private set; }
        public Player Player { get; private set; }
        public string AreaName { get; private set; }
        public AudioClip NewBackgroundMusic { get; private set; }

        public State_AreaTransition_ToPortal_Args(Portal portal, Player player, string areaName, AudioClip newBackgroundMusic)
        {
            Portal = portal;
            this.Player = player;
            NewBackgroundMusic = newBackgroundMusic;
            AreaName = areaName;
        }
    }
}