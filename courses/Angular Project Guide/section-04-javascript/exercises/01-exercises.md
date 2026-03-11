# Section 4: Exercises

## Exercise 4.1: Variables and Data Types

### Objective
Practice working with JavaScript variables and data types.

### Tasks
1. Create variables for a product using `const`:
   - Product name (string)
   - Price (number)
   - In stock (boolean)

2. Create an array of products

3. Create an object representing a product

---

## Exercise 4.2: Array Methods

### Objective
Practice using array methods.

### Given Array
```javascript
const numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
```

### Tasks
1. Use `map` to double all numbers
2. Use `filter` to get even numbers only
3. Use `reduce` to find the sum
4. Use `find` to get first number > 5

---

## Exercise 4.3: DOM Manipulation

### Objective
Practice selecting and modifying DOM elements.

### Tasks
1. Select all elements with class "product"
2. Change the text content of the first product
3. Add a new class to all products
4. Create a new button element and add it to the page

---

## Exercise 4.4: Event Handling

### Objective
Practice adding event listeners.

### Tasks
1. Add a click event to a button
2. Add a submit event to a form
3. Add keyboard event listener (keyup)
4. Add mouse events (mouseenter, mouseleave)

---

## Exercise 4.5: Async JavaScript

### Objective
Practice asynchronous JavaScript.

### Tasks
1. Create a function that returns a Promise
2. Use `.then()` to handle the result
3. Convert to async/await syntax
4. Use try/catch for error handling

### Example
```javascript
function fetchData() {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            resolve({ data: "Success!" });
        }, 1000);
    });
}
```

---

## Exercise 4.6: Fetch API

### Objective
Practice making HTTP requests.

### Tasks
1. Make a GET request to fetch products
2. Handle success and error responses
3. Display the data on the page

---

## Exercise 4.7: Build a Calculator

### Objective
Build a simple calculator.

### Requirements
- Two number inputs
- Four buttons (+, -, *, /)
- Display result
- Handle division by zero

---

## Exercise 4.8: Todo List

### Objective
Build a simple todo list.

### Features
- Add new todo
- Mark todo as complete
- Delete todo
- Show count of remaining todos
