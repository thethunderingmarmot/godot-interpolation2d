using Godot;
using System;

[Tool]
public class NestedInterpolation2D : Node2D
{
    [Signal]
    delegate void InterpolateToggled();
    [Export]
    public bool Interpolate
    {
        get
        {
            return _interpolate;
        }
        set
        {
            _interpolate = value;
            EmitSignal("InterpolateToggled");
        }
    }
    private bool _interpolate = true;

    private Node2D _inputNode;
    
    private float _interpolationTimer;
    private Transform2D _oldTransform;

    public override void _Ready()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
        
        if(!Engine.EditorHint)
        {
            _inputNode = GetParent<Node2D>();

            this.SetAsToplevel(true);
            this.Transform = _inputNode.Transform;

            ProcessPriority = _inputNode.ProcessPriority - 1;
            Engine.PhysicsJitterFix = 0;
            _inputNode.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
            this.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
            _oldTransform = _inputNode.Transform;

            SetProcess(true);
            SetPhysicsProcess(true);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        _interpolationTimer = 0;
        if(_interpolate)
            _oldTransform = _inputNode.Transform;
    }

    public override void _Process(float delta)
    {
        if(_interpolate)
        {
            _interpolationTimer += delta;
            this.Transform = _oldTransform.InterpolateWith(_inputNode.Transform, _interpolationTimer * Engine.IterationsPerSecond);
        }
        else
        {
            this.Transform = _inputNode.Transform;
        }
    }
}