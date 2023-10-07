using System;
using Exiled.API.Features;
using Exiled.CustomRoles.Events;
using System.Data;
using AutoReconnect.Main;
using AutoReconnect.EventHandler;
using Exiled.API.Features;
using Exiled.API.Enums;
using MEC;


namespace AutoReconnect
{
    public class AutoReconnect : Plugin<Config>
    {
        public override string Author => "RedLeaves";
        public override string Name => "AutoReconnect";

        public override Version Version { get; } = new(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new(8, 2, 1);

        public static AutoReconnect Instance;

        public Handler Handler { get; private set; }
        public EventHandlers EventHandlers { get; private set; }



        public override void OnEnabled()
        {
            Instance = this;
            Handler = new Handler();
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Player.Verified += EventHandlers.OnVerified;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Left += EventHandlers.OnDisconnected;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundstarted;

            //EventHandlers._timerHandle = Timing.RunCoroutine(EventHandlers.ATimer());

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Handler = null;
            Instance = null;
            EventHandlers = null;
            Exiled.Events.Handlers.Player.Verified -= EventHandlers.OnVerified;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Left -= EventHandlers.OnDisconnected;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundstarted;
            Timing.KillCoroutines(EventHandlers._timerHandle);

            base.OnDisabled();
        }
    }
}