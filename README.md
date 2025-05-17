# OptimizationDemo
This project is a showcase of deep Unity optimization, featured in my Fiverr gig video (Currently on my Fiverr gig the video might be under review). It demonstrates real performance problems and how I solved them. No profiler tricks, no fake stats. Everything shown is reproducible. You can download the project and run it yourself to verify the results.

## Overview
I implemented **Conway's Game of Life** and **Procedural Generation** using different techniques but the same core algorithms to show how bad performance can get, and how much it can be improved. Both are CPU-heavy by default. Note that most of those mini allocations that you see in the profiler are caused by Unity itself, not by me.

### Before
- Conway's Game Of Life: 1 FPS, +200 MB allocated per frame
- Procedural Generation: <30 FPS, 1 MB allocated per frame

### After
- Conway's Game Of Life: +1000 FPS, zero allocations per frame
- Procedural Generation: +140 FPS, near-zero allocations per frame

### Key Improvements
- Practically zero garbage collection
- Memory reuse & pooling
- Offloaded work to GPU
- Profier-driven decisions

### Ideal For
- Mobile Game Development
- Large Environment Optimization
- Performance Bottleneck Analysis
- CPU-Intense Systems

## How to Use
Unity doesn't support multi-project setups, so I used two assembly definitions: one for Procedural Generation and one for Conway's Game of Life.

### Open:
- **ConwayScene** for the Game of Life
- **GenerationScene** for procedural generation

Each scene has a bootstrap script with an **_isOptimized** toggle in the inspector:

- **Enabled:** Runs with the optimizations shown in the video
- **Disabled:** Runs the unoptimized version (also shown)

Hit **Play**, then press **K** to start the simulation in either scene.
In **ConwayScene**, press **L** to animate the camera.
