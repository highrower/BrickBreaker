# Modular Brick Breaker: Technical Prototype

**Project Type:** Systems Engineering & Architecture Prototype 
**Primary Tech:** C#, Unity, UI Toolkit (UXML/USS), ScriptableObjects 
**Focus:** Data-Driven Design, Decoupled Architecture, Developer Tools

## 1. Project Overview

This project is practice for good game architecture. My goal was to move away from hard-coded logic and build a framework where gameplay data is injected dynamically.

The visual presentation is a work-in-progress, but the underlying systems are architected to support rapid iteration and content expansion without code changes.


https://github.com/user-attachments/assets/af2e0444-98f1-41d7-bcca-39fe42ab7cba


## 2. Key Technical Systems
### A. Data-Driven Upgrade System (The Graph Shop)

The game’s progression is built on ScriptableObjects. I designed a upgrade graph that allows designers to "plop in" new power-ups and stat modifications without touching a single line of code.

  <img width="834" height="831" alt="Screenshot 2026-01-15 at 2 35 17 PM" src="https://github.com/user-attachments/assets/33f24239-db73-4581-8a49-255ca97b587e" />

### B. Unique Gameplay System (Ball Bust through)

https://github.com/user-attachments/assets/0bd98d24-33d7-438d-a1fd-0d148d8ee70e


I wanted the ball to feel heavy and powerful. When moving at high speeds, the ball smashes through weaker bricks.

**The Engineering:** I manipulated and took advantage of Unity's physics system to work how I want it to

    Prediction: The ball casts a shape forward every frame to detect collisions before they happen.

    State Swapping: If the ball is strong enough to destroy a target, the system swaps the target's collider to a "Trigger" right before impact.

    Result: The ball passes physically through the object. I then apply a calculated drag force to the velocity, 
    simulating the resistance of smashing through material without halting the gameplay flow.

### C. Decoupled Event Architecture (Observer Pattern)

To maintain clean code I avoid tight coupling between systems. There are no global Singletons managing state in a way that creates dependencies.

    Implementation: I utilize C# Events/Actions as a communication bus.

    Result: The Ball class knows nothing about the Score or Bank or even its own stats. 
      The brick just grabs up to date information from scriptable objects that are manipulated by either the UI or events. 
      This makes debugging and refactoring significantly easier.

### D. UI Architecture (MVC & UI Toolkit)

I utilized Unity’s UI Toolkit (UXML/USS) to separate the layout (View) from the logic (Controller).

    Model-View-Controller (MVC): The UI does not hold game state. 
      It strictly observes data models and updates the view when data changes.

    Web Standards: My experience with CSS-like styling (USS) and hierarchy (UXML) allows for responsive, reusable UI components.

### E. Persistence & Serialization Interface-Driven Save

    System: Implemented an ISaveable interface.
    This allows any ScriptableObject or Component to register itself for persistence. 
    
    JSON Serialization: Uses Unity's JsonUtility to save game state (upgrades, bank balance, etc.) to the local file system.
    This ensures progress is maintained across sessions.


### 3. Code Standards & Philosophy

As a Data Engineer, I use common programming principles in my code:

    Dependency Injection Principles: Systems receive the references they need upon initialization rather than reaching out into the global scope.

    Composition over Inheritance: While I use OOP, I prefer composing behaviors (component-based) to deep inheritance hierarchies to avoid fragility.

    Git Discipline: All features are developed with version control in mind, ensuring atomic commits and organized branches.

### 4. Why This Matters

This project demonstrates my ability to build tools and pipelines. I am ready to join a team environment where I can handle the "plumbing" of a game—connecting UI to data, managing game states, and ensuring the designers have the exposed variables they need to balance the fun.
