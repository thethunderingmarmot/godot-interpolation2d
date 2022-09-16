using Godot;
using System;

[Tool]
public class Interpolation2D : Node
{
    [Export]
    private NodePath InputNodePath;
    [Export]
    private NodePath OutputNodePath;
    [Export]
    private bool Interpolate = true;

    private Node2D InputNode;
    private Node2D OutputNode;
    
    private float InterpolationTimer;
    private Transform2D OldTransform;

    public override void _Ready()
    {
        SetProcess(false);
        SetPhysicsProcess(false);

        if(InputNodePath != null && OutputNodePath != null)
        {
            if(!Engine.EditorHint)
            {
                InputNode = GetNode<Node2D>(InputNodePath);
                OutputNode = GetNode<Node2D>(OutputNodePath);

                if (InputNode.IsAParentOf(OutputNode))
                {
                    OutputNode.SetAsToplevel(true);
                    OutputNode.Transform = InputNode.Transform;
                }
                else if(OutputNode.IsAParentOf(InputNode))
                {
                    InputNode.SetAsToplevel(true);
                    InputNode.Transform = OutputNode.Transform;
                }

                ProcessPriority = InputNode.ProcessPriority - 1;
                Engine.PhysicsJitterFix = 0;
                InputNode.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
                OutputNode.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
                OldTransform = InputNode.Transform;

                SetProcess(true);
                SetPhysicsProcess(true);
            }
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
            OutputNode.Transform = OldTransform.InterpolateWith(InputNode.Transform, InterpolationTimer * Engine.IterationsPerSecond);
        else
            OutputNode.Transform = InputNode.Transform;
    }
}