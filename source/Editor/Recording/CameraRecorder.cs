﻿using System.Collections.Generic;
using System.Linq;
using Celeste;
using Microsoft.Xna.Framework;
using Monocle;

namespace Snowberry.Editor.Recording;

public class CameraRecorder : Recorder{

    private readonly List<(Rectangle camRect, float time)> States = new();

    public override void UpdateInGame(Level l){
        Camera c = l.Camera;
        States.Add((new Rectangle((int)c.X, (int)c.Y, (int)(c.Right - c.X), (int)(c.Bottom - c.Y)), l.TimeActive));
    }

    public override void RenderScreenSpace(float time){}

    public override void RenderWorldSpace(float time){
        foreach (var state in States)
            if (time <= state.time) {
                Draw.HollowRect(state.camRect, Color.Orange);
                break;
            }
    }
}