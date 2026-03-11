# Objects

## What are Objects?

Objects are collections of key-value pairs that group related data together. Unlike arrays which use numeric indexes, objects use meaningful keys (called properties) to access values. Objects are perfect for representing real-world things with multiple attributes, like a person, a car, or a product.

Objects are fundamental to JavaScript and are used extensively in web development.

## Key Bullet Points

- **Object creation**: `{ name: "John", age: 25 }` - wrapped in curly braces
- **Dot notation**: `person.name` - easiest way to access properties
- **Bracket notation**: `person["name"]` - useful when key is dynamic
- **Adding properties**: `person.email = "john@email.com"` - just assign a value
- **Modifying properties**: Change any property by reassigning
- **Properties can be any type**: Strings, numbers, booleans, arrays, even other objects

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Objects</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #f3e5f5; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #9c27b0;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #9c27b0;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JavaScript Objects</h1>
    
    <button onclick="showObjectCreation()">Creating Objects</button>
    <button onclick="showObjectProperties()">Accessing Properties</button>
    <button onclick="showModifyObject()">Modifying Objects</button>
    
    <div id="output">Click a button to see objects in action!</div>

    <script>
        // Creating an object with properties
        let student = {
            name: "Alice",
            age: 20,
            grade: "A",
            subjects: ["Math", "Science", "English"],
            isEnrolled: true
        };

        // Button handler - creating objects
        function showObjectCreation() {
            let result = "Creating an object:<br><br>";
            result += "let student = {<br>";
            result += "&nbsp;&nbsp;name: \"Alice\",<br>";
            result += "&nbsp;&nbsp;age: 20,<br>";
            result += "&nbsp;&nbsp;grade: \"A\",<br>";
            result += "&nbsp;&nbsp;subjects: [\"Math\", \"Science\", \"English\"],<br>";
            result += "&nbsp;&nbsp;isEnrolled: true<br>";
            result += "};";
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - accessing properties
        function showObjectProperties() {
            let result = "Object: " + JSON.stringify(student) + "<br><br>";
            
            // Dot notation - most common
            result += "Dot notation:<br>";
            result += "student.name = " + student.name + "<br>";
            result += "student.age = " + student.age + "<br>";
            result += "student.grade = " + student.grade + "<br><br>";
            
            // Bracket notation
            result += "Bracket notation:<br>";
            result += "student[\"name\"] = " + student["name"] + "<br>";
            result += "student[\"age\"] = " + student["age"] + "<br><br>";
            
            // Accessing array inside object
            result += "Array in object:<br>";
            result += "student.subjects = " + JSON.stringify(student.subjects);
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - modifying objects
        function showModifyObject() {
            let result = "Original: " + JSON.stringify(student) + "<br><br>";
            
            // Modify existing property
            student.age = 21;
            result += "After student.age = 21: " + JSON.stringify(student) + "<br>";
            
            // Add new property
            student.major = "Computer Science";
            result += "After adding major: " + JSON.stringify(student) + "<br>";
            
            // Delete a property
            delete student.grade;
            result += "After deleting grade: " + JSON.stringify(student);
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **Object literal**: `{ name: "Alice", age: 20 }` - curly braces with key-value pairs. Keys are usually strings (can omit quotes for simple names).
- **Dot notation**: `student.name` - most common way to access properties. Clean and readable.
- **Bracket notation**: `student["name"]` - useful when property name is stored in a variable or has special characters.
- **Adding properties**: `student.major = "Computer Science"` - just assign a value to a new key.
- **Deleting properties**: `delete student.grade` - removes the property completely.
- **Arrays in objects**: Objects can contain arrays as property values.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and three buttons
2. Click "Creating Objects" - shows the syntax for creating an object
3. Click "Accessing Properties" - shows:
   - Dot notation examples (student.name, student.age)
   - Bracket notation examples (student["name"])
   - Accessing the array inside the object
4. Click "Modifying Objects" - shows:
   - Original object
   - After changing age to 21
   - After adding major property
   - After deleting grade property
