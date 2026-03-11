# CodeWhisperer - Use Cases for CS Students

## 1. Academic Projects

### Coursework Assignments

**Database Applications**
```python
# Use CodeWhisperer to quickly write database queries
import sqlite3

def get_student_grades(student_id):
    conn = sqlite3.connect('school.db')
    cursor = conn.cursor()
    
    query = """
    SELECT student_name, subject, grade 
    FROM grades 
    WHERE student_id = ?
    """
    cursor.execute(query, (student_id,))
    results = cursor.fetchall()
    conn.close()
    return results
```

**Web Development**
```javascript
// Express.js API with CodeWhisperer suggestions
app.get('/api/students', async (req, res) => {
  try {
    const students = await Student.findAll();
    res.json({ success: true, data: students });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});
```

---

## 2. Internship Projects

### Real-World Code

**AWS Lambda Functions**
```python
import boto3
import json

def lambda_handler(event, context):
    dynamodb = boto3.resource('dynamodb')
    table = dynamodb.Table('Employees')
    
    # CodeWhisperer suggests proper error handling
    try:
        response = table.scan()
        return {
            'statusCode': 200,
            'body': json.dumps(response['Items'])
        }
    except Exception as e:
        return {
            'statusCode': 500,
            'body': json.dumps({'error': str(e)})
        }
```

---

## 3. Portfolio Development

### Showcase Projects

**Machine Learning Pipeline**
```python
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier

def build_classification_model(data_path):
    # Load and preprocess data
    df = pd.read_csv(data_path)
    X = df.drop('target', axis=1)
    y = df['target']
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Train model
    model = RandomForestClassifier(n_estimators=100)
    model.fit(X_train, y_train)
    
    # Evaluate
    accuracy = model.score(X_test, y_test)
    return model, accuracy
```

---

## 4. Interview Preparation

### Practice Problems

**Algorithm Implementation**
```python
# CodeWhisperer helps with common algorithms
def binary_search(arr, target):
    left, right = 0, len(arr) - 1
    
    while left <= right:
        mid = (left + right) // 2
        if arr[mid] == target:
            return mid
        elif arr[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    
    return -1
```

---

## 5. Security Projects

### Learning Cybersecurity

**Input Validation**
```python
# CodeWhisperer suggests secure coding practices
import re

def validate_input(user_input):
    # Prevent SQL injection
    if re.search(r"[;']", user_input):
        raise ValueError("Invalid input detected")
    
    # Sanitize for XSS
    dangerous_chars = ['<', '>', '"', "'"]
    for char in dangerous_chars:
        user_input = user_input.replace(char, '')
    
    return user_input
```

---

## 6. AWS Cloud Projects

### Cloud-Native Development

**S3 File Operations**
```python
import boto3

def upload_file_to_s3(file_name, bucket_name):
    s3_client = boto3.client('s3')
    
    try:
        s3_client.upload_file(
            file_name, 
            bucket_name, 
            file_name,
            ExtraArgs={'ContentType': 'text/html'}
        )
        return True
    except Exception as e:
        print(f"Error: {e}")
        return False
```

---

## 7. Open Source Contribution

### Contributing to Projects

**Reading Documentation**
- CodeWhisperer helps understand complex codebases
- Suggests proper coding patterns
- Generates documentation comments

---

## 8. Learning New Languages

### Multi-Language Practice

**Learning Go**
```go
// CodeWhisperer helps with Go concurrency
package main

import (
    "fmt"
    "time"
)

func worker(id int, jobs <-chan int, results chan<- int) {
    for j := range jobs {
        fmt.Printf("Worker %d started job %d\n", id, j)
        time.Sleep(time.Second)
        results <- j * 2
    }
}

func main() {
    jobs := make(chan int, 100)
    results := make(chan int, 100)
    
    for w := 1; w <= 3; w++ {
        go worker(w, jobs, results)
    }
    
    for j := 1; j <= 5; j++ {
        jobs <- j
    }
    close(jobs)
    
    for a := 1; a <= 5; a++ {
        <-results
    }
}
```

---

## 9. Unit Testing

### Test Generation

**Python Tests**
```python
import unittest
from myapp import Calculator

class TestCalculator(unittest.TestCase):
    def test_add(self):
        calc = Calculator()
        self.assertEqual(calc.add(2, 3), 5)
    
    def test_divide_by_zero(self):
        calc = Calculator()
        with self.assertRaises(ZeroDivisionError):
            calc.divide(1, 0)
```

---

## 10. Competitive Programming

### LeetCode/HackerRank

**Quick Implementation**
```python
# Two Sum - CodeWhisperer helps with patterns
def two_sum(nums, target):
    seen = {}
    for i, num in enumerate(nums):
        complement = target - num
        if complement in seen:
            return [seen[complement], i]
        seen[num] = i
    return []
```

---

*Back to [08-Coding README](../README.md)*
*Back to [Main README../../README.md)*