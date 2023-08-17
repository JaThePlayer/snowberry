﻿using Celeste.Mod;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Reflection;
using Snowberry.Editor;
using Snowberry.Editor.Tools;

namespace Snowberry;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PluginAttribute : Attribute {
    internal readonly string Name;

    public PluginAttribute(string entityName) {
        Name = entityName;
    }
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class OptionAttribute : Attribute {
    internal readonly string Name;

    public OptionAttribute(string optionName) {
        Name = optionName;
    }
}

public abstract class Plugin {
    public PluginInfo Info { get; internal set; }
    public string Name { get; internal set; }

    // overriden by generic plugins
    public virtual void Set(string option, object value) {
        if (Info.Options.TryGetValue(option, out PluginOption f)) {
            object v;
            if (f.FieldType == typeof(char))
                v = value.ToString()[0];
            else if (f.FieldType == typeof(Tileset))
                v = Tileset.ByKey(value.ToString()[0], false);
            else
                v = value is string str ? StrToObject(f.FieldType, str) : Convert.ChangeType(value, f.FieldType);
            try {
                f.SetValue(this, v);
            } catch (ArgumentException e) {
                Snowberry.Log(LogLevel.Warn, "Tried to set field " + option + " to an invalid value " + v);
                Snowberry.Log(LogLevel.Warn, e.ToString());
            }
        }
    }

    public virtual object Get(string option) {
        if (Info.Options.TryGetValue(option, out PluginOption f))
            return ObjectToStr(f.GetValue(this));
        return null;
    }

    public string GetTooltipFor(string option) {
        return Info.Options.TryGetValue(option, out PluginOption f)  ? f.Tooltip : null;
    }

    protected static object StrToObject(Type targetType, string raw){
        if(targetType.IsEnum)
            try {
                return Enum.Parse(targetType, raw);
            } catch {
                return null;
            }

        if(targetType == typeof(Color))
            return Monocle.Calc.HexToColor(raw);
        if(targetType == typeof(char))
            return raw[0];
        if(targetType == typeof(Tileset))
            return Tileset.ByKey(raw[0], false);

        return raw;
    }

    protected static object ObjectToStr(object obj) {
        return obj switch {
            Color color => BitConverter.ToString(new[] { color.R, color.G, color.B }).Replace("-", string.Empty),
            Enum => obj.ToString(),
            char ch => ch.ToString(),
            Tileset ts => ts.Key.ToString(),
            _ => obj
        };
    }
}