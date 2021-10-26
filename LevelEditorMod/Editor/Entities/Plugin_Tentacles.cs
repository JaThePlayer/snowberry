﻿using Celeste;
using Microsoft.Xna.Framework;
using Monocle;

namespace LevelEditorMod.Editor.Entities {
    [Plugin("tentacles")]
    public class Plugin_Tentacles : Entity {
        public override void Render() {
            base.Render();

            MTexture icon = GFX.Game["plugins/LevelEditorMod/tentacles"];
            icon.DrawCentered(Position);

            Vector2 prev = Position;
            foreach (Vector2 node in Nodes) {
                icon.DrawCentered(node);
                DrawUtil.DottedLine(prev, node, Color.Red * 0.5f, 8, 4);
                prev = node;
            }
        }
    }
}
