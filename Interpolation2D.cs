using Godot;
using System;

[Tool]
public class Interpolation2D : Node2D
{
    [Export]
    public bool Interpolate
    {
        get => _interpolate;
        set
        {
            _interpolate = value;
            if(_interpolate)
                EnableInterpolation();
            else
                DisableInterpolation();
        }
    }
    private bool _interpolate = true;

    private Node2D _nodeToInterpolate;
    private float _interpolationTimer;
    private Transform2D _oldTransform;

    public override void _Ready()
    {
        this.SetProcess(false);
        this.SetPhysicsProcess(false);

        if(!Engine.EditorHint)
        {
            _nodeToInterpolate = GetParent<Node2D>();
            ProcessPriority = _nodeToInterpolate.ProcessPriority - 1;
            
            Engine.PhysicsJitterFix = 0;
            _nodeToInterpolate.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
            this.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
            
            if(_interpolate)
                EnableInterpolation();
            else
                DisableInterpolation();
        }
    }

    private void EnableInterpolation()
    {
        this.SetAsToplevel(true);
        this.Transform = _nodeToInterpolate.Transform;
        _interpolationTimer = 0;
        _oldTransform = _nodeToInterpolate.Transform;
        this.Transform = _nodeToInterpolate.Transform;

        this.SetProcess(true);
        this.SetPhysicsProcess(true);
    }

    private void DisableInterpolation()
    {
        this.SetAsToplevel(false);
        this.Transform = Transform2D.Identity;
        _interpolationTimer = 0;
        _oldTransform = _nodeToInterpolate.Transform;
        this.Transform = _nodeToInterpolate.Transform;

        this.SetProcess(false);
        this.SetPhysicsProcess(false);
    }

    public void SkipInterpolation()
    {
        _interpolationTimer = 0;
        _oldTransform = _nodeToInterpolate.Transform;
        this.Transform = _nodeToInterpolate.Transform;
    }

    public override void _PhysicsProcess(float delta)
    {
        _interpolationTimer = 0;
        _oldTransform = _nodeToInterpolate.Transform;
    }

    public override void _Process(float delta)
    {
        _interpolationTimer += delta;
        this.Transform = _oldTransform.InterpolateWith(_nodeToInterpolate.Transform, _interpolationTimer * Engine.IterationsPerSecond);
    }
}