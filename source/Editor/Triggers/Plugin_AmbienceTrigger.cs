﻿using Microsoft.Xna.Framework;

namespace Snowberry.Editor.Triggers;

[Plugin("everest/ambienceTrigger")]
public class Plugin_AmbienceTrigger : Trigger {

    [Option("track")] public string Track = "";
    [Option("resetOnLeave")] public bool ResetOnLeave = true;

    public override void Render() {
        base.Render();

        var str = (Track == "") ? "" : $"({Track})";
        Fonts.Pico8.Draw(str, Center + Vector2.UnitY * 6, Vector2.One, new(0.5f), Color.Black);
    }

    public new static void AddPlacements() {
        Placements.EntityPlacementProvider.Create("Ambience Trigger (Everest)", "everest/ambienceTrigger", trigger: true);
    }
}