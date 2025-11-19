# AStar Demo

This project contains a simple and clean implementation of the A* algorithm with real-time visualization.
It includes a minimal user interface to configure the grid, place obstacles, and watch the algorithm work step-by-step.

## Features

- ### Animated A Star Execution
  The algorithm runs with a built-in 100 ms delay between processing steps to provide a smooth visual demonstration.
  You can remove or change this delay inside the ```PathFind``` method of the ```AStarAlgorithm``` class.

- ### Configurable Grid Interface
  The demo includes a primitive UI allowing you to:
    - Set the grid size
    - Choose start and goal positions
    - Mark cells as walkable or blocked
     This makes it easy to test different pathfinding scenarios.

- ### On-The-Fly Rendering
  The grid and algorithm progress are rendered dynamically using ```Graphics.DrawMesh``` once the field is created—no pre-baked meshes or Unity GameObjects required.

## [Playable DEMO](https://adezar.github.io/AStarDemo)
