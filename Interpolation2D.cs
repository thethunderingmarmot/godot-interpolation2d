using Godot;
using System;

[Tool]
public class Interpolation2D : Node
{
    [Export]
    public NodePath InputNodePath;
    [Export]
    public NodePath OutputNodePath;
    
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
    private Node2D _outputNode;
    
    private float _interpolationTimer;
    private Transform2D _oldTransform;

    public override void _Ready()
    {
        SetProcess(false);
        SetPhysicsProcess(false);

        if(InputNodePath != null && OutputNodePath != null)
        {
            if(!Engine.EditorHint)
            {
                _inputNode = GetNode<Node2D>(InputNodePath);
                _outputNode = GetNode<Node2D>(OutputNodePath);

                if (_inputNode.IsAParentOf(_outputNode))
                {
                    _outputNode.SetAsToplevel(true);
                    _outputNode.Transform = _inputNode.Transform;
                }
                else if(_outputNode.IsAParentOf(_inputNode))
                {
                    _inputNode.SetAsToplevel(true);
                    _inputNode.Transform = _outputNode.Transform;
                }

                _oldTransform = _inputNode.Transform;
                ProcessPriority = _inputNode.ProcessPriority - 1;

                Engine.PhysicsJitterFix = 0;
                _inputNode.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
                _outputNode.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;

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
        _interpolationTimer = 0;
        if(_interpolate)
            _oldTransform = _inputNode.Transform;
    }

    public override void _Process(float delta)
    {
        if(_interpolate)
        {
            _interpolationTimer += delta;
            _outputNode.Transform = _oldTransform.InterpolateWith(_inputNode.Transform, _interpolationTimer * Engine.IterationsPerSecond);
        }
        else
        {
            _outputNode.Transform = _inputNode.Transform;
        }
    }
}