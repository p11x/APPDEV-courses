# WebAssembly Rust Integration

## What You'll Learn

- How to integrate Rust code with Node.js using WebAssembly
- Using wasm-bindgen for seamless interoperability
- Passing complex data types between Rust and JavaScript
- Best practices for Rust + Node.js projects

---

## Layer 1: Rust to JavaScript Integration

### Basic Function Exports

```rust
// src/lib.rs
use wasm_bindgen::prelude::*;

// Simple function export
#[wasm_bindgen]
pub fn greet(name: &str) -> String {
    format!("Hello, {}!", name)
}

// Number crunching
#[wasm_bindgen]
pub fn calculate_stats(numbers: &[f64]) -> Stats {
    let sum: f64 = numbers.iter().sum();
    let mean = sum / numbers.len() as f64;
    let variance = numbers.iter()
        .map(|x| (x - mean).powi(2))
        .sum::<f64>() / numbers.len() as f64;
    
    Stats { mean, variance, sum }
}

#[wasm_bindgen]
pub struct Stats {
    pub mean: f64,
    pub variance: f64,
    pub sum: f64,
}
```

### Working with Strings

```rust
use wasm_bindgen::prelude::*;

// String handling
#[wasm_bindgen]
pub fn process_text(text: &str) -> String {
    text.to_uppercase()
        .chars()
        .filter(|c| c.is_alphabetic())
        .collect()
}

// Return string array
#[wasm_bindgen]
pub fn tokenize(text: &str) -> Vec<String> {
    text.split_whitespace()
        .map(String::from)
        .collect()
}
```

---

## Layer 2: Complex Data Types

### Custom Structs

```rust
use wasm_bindgen::prelude::*;
use serde::{Serialize, Deserialize};

#[wasm_bindgen]
#[derive(Serialize, Deserialize)]
pub struct User {
    pub id: u32,
    pub name: String,
    pub email: String,
}

#[wasm_bindgen]
impl User {
    #[wasm_bindgen(constructor)]
    pub fn new(id: u32, name: String, email: String) -> User {
        User { id, name, email }
    }
    
    pub fn validate(&self) -> bool {
        self.email.contains('@') && !self.name.is_empty()
    }
}
```

### Error Handling

```rust
use wasm_bindgen::prelude::*;

#[wasm_bindgen]
pub enum MathError {
    DivisionByZero = 0,
    Overflow = 1,
    InvalidInput = 2,
}

#[wasm_bindgen]
pub fn divide(a: f64, b: f64) -> Result<f64, MathError> {
    if b == 0.0 {
        Err(MathError::DivisionByZero)
    } else {
        Ok(a / b)
    }
}
```

---

## Layer 3: JavaScript to Rust

### Passing Arrays

```rust
use wasm_bindgen::prelude::*;

#[wasm_bindgen]
pub fn sum_array(arr: &[i32]) -> i32 {
    arr.iter().sum()
}

#[wasm_bindgen]
pub fn filter_array(arr: &[i32], predicate: i32) -> Vec<i32> {
    arr.iter().filter(|&&x| x > predicate).copied().collect()
}

#[wasm_bindgen]
pub fn sort_array(mut arr: Vec<i32>) -> Vec<i32> {
    arr.sort();
    arr
}
```

### Callbacks

```rust
use wasm_bindgen::prelude::*;

type Callback = Box<dyn FnMut(i32) -> i32>;

#[wasm_bindgen]
pub struct Processor {
    callback: Option<Callback>,
}

#[wasm_bindgen]
impl Processor {
    #[wasm_bindgen(constructor)]
    pub fn new() -> Processor {
        Processor { callback: None }
    }
    
    #[wasm_bindgen]
    pub fn set_callback(&mut self, callback: JsValue) {
        // Callback implementation
    }
    
    #[wasm_bindgen]
    pub fn process(&mut self, value: i32) -> i32 {
        value * 2
    }
}
```

---

## Next Steps

Continue to [WASM TypeScript](./05-wasm-typescript.md)