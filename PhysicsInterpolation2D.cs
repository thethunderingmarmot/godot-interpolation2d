using Godot;
using System;

[Tool]
public class PhysicsInterpolation2D : EditorPlugin
{
    public override void _EnterTree()
    {
        Script Interpolation2DScript = ResourceLoader.Load("res://addons/godot-interpolation2d/Interpolation2D.cs") as Script;
        Texture Interpolation2DTexture = ResourceLoader.Load("res://addons/godot-interpolation2d/Icon.png") as Texture;
        AddCustomType("Interpolation2D", "Node2D", Interpolation2DScript, Interpolation2DTexture);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("Interpolation2D");
    }
}
