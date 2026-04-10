# 🎮 Project 17: Gaming Platform

## Browser-Based Gaming Platform

---

## Table of Contents

1. [Game Engine Basics](#game-engine-basics)
2. [Canvas Rendering](#canvas-rendering)
3. [Game Loop](#game-loop)
4. [Input Handling](#input-handling)
5. [Score System](#score-system)

---

## Game Engine Basics

### Simple Game Engine

```javascript
class Game {
  constructor(canvas) {
    this.canvas = canvas;
    this.ctx = canvas.getContext('2d');
    this.entities = [];
    this.running = false;
  }
  
  start() {
    this.running = true;
    this.loop();
  }
  
  stop() {
    this.running = false;
  }
  
  loop() {
    if (!this.running) return;
    
    this.update();
    this.render();
    requestAnimationFrame(() => this.loop());
  }
  
  update() {
    this.entities.forEach(entity => entity.update());
  }
  
  render() {
    this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    this.entities.forEach(entity => entity.render(this.ctx));
  }
  
  add(entity) {
    this.entities.push(entity);
  }
}
```

---

## Canvas Rendering

### Basic Rendering

```javascript
class Entity {
  constructor(x, y, width, height) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
  }
  
  render(ctx) {
    ctx.fillStyle = 'blue';
    ctx.fillRect(this.x, this.y, this.width, this.height);
  }
  
  update() {
    // Override in subclass
  }
}
```

---

## Game Loop

### Frame-Rate Independent Movement

```javascript
class Game {
  constructor() {
    this.lastTime = 0;
  }
  
  loop(timestamp) {
    const deltaTime = timestamp - this.lastTime;
    this.lastTime = timestamp;
    
    this.update(deltaTime);
    this.render();
    
    requestAnimationFrame(t => this.loop(t));
  }
}
```

---

## Input Handling

### Keyboard Input

```javascript
const keys = {};

document.addEventListener('keydown', e => keys[e.code] = true);
document.addEventListener('keyup', e => keys[e.code] = false);

function isKeyPressed(code) {
  return keys[code];
}
```

### Gamepad Support

```javascript
function getGamepad() {
  const gamepads = navigator.getGamepads();
  return gamepads[0];
}
```

---

## Score System

```javascript
class ScoreManager {
  constructor() {
    this.score = 0;
    this.highScore = 0;
  }
  
  addScore(points) {
    this.score += points;
    if (this.score > this.highScore) {
      this.highScore = this.score;
    }
  }
  
  reset() {
    this.score = 0;
  }
  
  save() {
    localStorage.setItem('highScore', this.highScore);
  }
  
  load() {
    this.highScore = parseInt(localStorage.getItem('highScore')) || 0;
  }
}
```

---

## Summary

### Key Takeaways

1. **Game Loop**: requestAnimationFrame
2. **Entities**: Object-oriented design
3. **Input**: Keyboard and gamepad

### Next Steps

- Continue with: [03_PROJECT_EDUCATIONAL_PLATFORM.md](03_PROJECT_EDUCATIONAL_PLATFORM.md)
- Add collision detection
- Implement particle effects

---

## Cross-References

- **Previous**: [01_PROJECT_FINANCE_TRADING.md](01_PROJECT_FINANCE_TRADING.md)
- **Next**: [03_PROJECT_EDUCATIONAL_PLATFORM.md](03_PROJECT_EDUCATIONAL_PLATFORM.md)

---

*Last updated: 2024*