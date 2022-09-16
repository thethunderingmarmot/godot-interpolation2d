using Godot;
using System;

[Tool]
public class Interpolation2D : Node2D
{
    [Export]
    private NodePath InputNodePath = new NodePath("..");
    [Export]
    public bool Interpolate = true;

    private Node2D InputNode;
    
    private float InterpolationTimer;
    private Transform2D OldTransform;

    public override void _Ready()
    {
        SetProcess(false);
        SetPhysicsProcess(false);

        if(InputNodePath != null)
        {
            InputNode = GetNode<Node2D>(InputNodePath);

            this.SetAsToplevel(true);
            this.Transform = InputNode.Transform;

            ProcessPriority = InputNode.ProcessPriority - 1;
            Engine.PhysicsJitterFix = 0;
            InputNode.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
            this.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
            OldTransform = InputNode.Transform;

            SetProcess(true);
            SetPhysicsProcess(true);
        }
        else
        {
            GD.PrintErr("No Input or Output selected for Interpolation2D!");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        InterpolationTimer = 0;
        if(Interpolate)
            OldTransform = InputNode.Transform;
    }

    public override void _Process(float delta)
    {
        InterpolationTimer += delta;
        if(Interpolate)
            this.Transform = OldTransform.InterpolateWith(InputNode.Transform, InterpolationTimer * Engine.IterationsPerSecond);
        else
            this.Transform = InputNode.Transform;
    }
}
