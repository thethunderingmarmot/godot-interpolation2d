using Godot;
using System;

[Tool]
public class NestedInterpolation2D : Node2D
{
    [Export]
    private bool Interpolate = true;

    private Node2D InputNode;
    
    private float InterpolationTimer;
    private Transform2D OldTransform;

    public override void _Ready()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
        
        if(!Engine.EditorHint)
        {
            InputNode = GetParent<Node2D>();

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