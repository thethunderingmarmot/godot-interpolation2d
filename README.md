# Godot addon - Fixed Timestep Interpolation for 2D
This is my solution to this problem, based on [Calinous's demo project](https://github.com/Calinou/godot-physics-interpolation-demo).  
As for now, Godot only applies Physics Interpolation to `Spatial`s and 3D physics.  
As I needed something for my upcoming C# 2D game, I found [lawnjelly's solution](https://github.com/lawnjelly/smoothing-addon) to be the most useful,  
but I had this itch that wouldn't let me use a GDScript addon in a C# project (just another obsession of mine).  
So I started making this addon in C# with the excuse of learning more about interpolation (which I did).  

## Explaination
You can find [a full explaination here](https://docs.godotengine.org/en/3.6/tutorials/physics/interpolation/physics_interpolation_introduction.html) (thanks Godot!). 

## Diagram
- `Interpolation2D` : `Node2D`  
  - `Interpolate` : `bool` (toggles interpolation)
  - `SkipInterpolation()` : `void` (skips interpolation)

## Usage
`Interpolation2D` works if placed as a child of the Node that actually moves (usually  
the one with the movement logic within `_PhysicalProcess`, like a `KinematicBody2D`),  
and if the Node that you want to move smoothly (usually the one which contains the  
visible part, like a `Sprite`) is placed as a child of `Interpolation2D`.  
The `Interpolate` boolean toggles the interpolation for whatever reason you need,  
while the `SkipInterpoltion()` method will skip the current interpolation operation  
(if you need to instantly move, first change the `Transform` and then call this method).  
