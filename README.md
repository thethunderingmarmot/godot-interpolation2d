# Godot addon - Fixed Timestep Interpolation for 2D
This is my solution to this problem, based on [Calinous's demo project](https://github.com/Calinou/godot-physics-interpolation-demo).  
As for now, Godot only applies Physics Interpolation to `Spatial`s and 3D physics.  
As I needed something for my upcoming C# 2D game, I found [lawnjelly's solution](https://github.com/lawnjelly/smoothing-addon) to be the most useful,  
but I had this itch that wouldn't let me use a GDScript addon in a C# project (just another obsession of mine).  
So I started making this addon in C# with the excuse of learning more about interpolation (which I did).  

## Explaination
You can find [a full explaination here](https://docs.godotengine.org/en/3.6/tutorials/physics/interpolation/physics_interpolation_introduction.html) (thanks Godot!). 

## Diagram
Nodes added and their properties in the editor:
- `GenericInterpolation2D` : `Node`  
  - Input Node Path : `NodePath`
  - Output Node Path : `NodePath`
  - Interpolate : `bool`
- `Interpolation2D` : `Node2D`  
  - Input Node Path : `NodePath`
  - Interpolate : `bool`

## Usage
`GenericInterpolation2D` doesn't have a `Transform` and has no `Position`,  
it's just something that does the work for you while you leave it somewhere.  
In the editor you can modify its properties once you add it to the Tree,  
you should set Input Node Path to the Node that actually moves (usually  
the one with the the movement logic within `_PhysicalProcess`, like a `KinematicBody2D`).  
You should set Output Node Path to the Node that you want to move smoothly  
(usually the one which contains the visible part, like a `Sprite`).  
The Interpolate boolean toggles the interpolation for whatever reason (usually  
for teleporting the object without graphical glitches).  
`Interpolation2D` is similar but since it extends `Node2D`, it has its own `Transform`  
so it has to be placed as a child of the Node that actually moves while  
the visible part should be placed as a child of the `Interpolation2D`.
