using Godot;
using System;

[Tool]
public class PhysicsInterpolation2D : EditorPlugin
{
    public override void _EnterTree()
    {
        Script Interpolation2DScript = ResourceLoader.Load("res://addons/PhysicsInterpolation2D/Interpolation2D.cs") as Script;
        Script GenericInterpolation2DScript = ResourceLoader.Load("res://addons/PhysicsInterpolation2D/GenericInterpolation2D.cs") as Script;
        Texture Interpolation2DTexture = ResourceLoader.Load("res://addons/PhysicsInterpolation2D/Icon.png") as Texture;
        Texture GenericInterpolation2DTexture = ResourceLoader.Load("res://addons/PhysicsInterpolation2D/IconGeneric.png") as Texture;
        AddCustomType("Interpolation2D", "Node2D", Interpolation2DScript, Interpolation2DTexture);
        AddCustomType("GenericInterpolation2D", "Node", GenericInterpolation2DScript, GenericInterpolation2DTexture);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("Interpolation2D");
        RemoveCustomType("GenericInterpolation2D");
    }
}
