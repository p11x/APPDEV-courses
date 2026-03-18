# ConApp2 - File I/O and JSON

This document covers the file operations and JSON handling taught in ConApp2.

## Table of Contents
1. [File Operations](#file-operations)
2. [Directory Operations](#directory-operations)
3. [JSON Handling](#json-handling)
4. [Exception Handling](#exception-handling)

---

## File Operations

### Writing to Files (StreamWriter)

```csharp
using StreamWriter sw = new StreamWriter("sample.txt");
sw.WriteLine("Hello World");
sw.Close();
```

**Source:** [Class49.cs](../../ConsoleApplication/ConApp2/Class49.cs)

### Reading from Files (StreamReader)

```csharp
using StreamReader sr = new StreamReader("sample.txt");
string content = sr.ReadToEnd();
Console.WriteLine(content);
sr.Close();
```

**Source:** [Class49.cs](../../ConsoleApplication/ConApp2/Class49.cs)

### Writing Multiple Lines

```csharp
string[] names = new string[5];
StreamWriter sw = new StreamWriter("names.txt", true);  // append mode

for (int i = 0; i < names.Length; i++)
{
    Console.Write($"Names[{i}]=");
    names[i] = Console.ReadLine();
    sw.WriteLine(names[i]);
}
sw.Close();
```

**Source:** [Class52.cs](../../ConsoleApplication/ConApp2/Class52.cs)

### Reading Line by Line

```csharp
StreamReader sr = new StreamReader("names.txt");
while (sr.Peek() != -1)
{
    string line = sr.ReadLine();
    Console.WriteLine(line);
}
sr.Close();
```

**Source:** [Class53.cs](../../ConsoleApplication/ConApp2/Class53.cs)

### File Information (FileInfo)

```csharp
FileInfo fileInfo = new FileInfo(path);
Console.WriteLine("Full Name: " + fileInfo.FullName);
Console.WriteLine("Created: " + fileInfo.CreationTime);
Console.WriteLine("Size: " + fileInfo.Length);
Console.WriteLine("Last Modified: " + fileInfo.LastWriteTime);
```

**Source:** [Class51.cs](../../ConsoleApplication/ConApp2/Class51.cs)

---

## Directory Operations

### Get Drive Information

```csharp
DriveInfo[] drives = DriveInfo.GetDrives();
foreach (DriveInfo d in drives)
{
    Console.WriteLine("Drive: " + d.Name);
    if (d.IsReady)
    {
        Console.WriteLine("  Free Space: " + d.AvailableFreeSpace + " bytes");
        Console.WriteLine("  Total Size: " + d.TotalSize + " bytes");
    }
}
```

**Source:** [Class46.cs](../../ConsoleApplication/ConApp2/Class46.cs)

### Enumerate Files

```csharp
var files = Directory.GetFiles("C:\\MyFolder");
foreach (string file in files)
{
    Console.WriteLine(file);
}
```

**Source:** [Class47.cs](../../ConsoleApplication/ConApp2/Class47.cs)

### Recursive Directory Traversal

```csharp
public static void PrintSubDirectories(string src)
{
    string[] directories = Directory.GetDirectories(src);
    string[] files = Directory.GetFiles(src);
    
    foreach (var file in files)
    {
        Console.WriteLine("\t\t" + file);
    }
    
    foreach (var dir in directories)
    {
        Console.WriteLine(dir);
        PrintSubDirectories(dir);  // Recursive call
    }
}
```

**Source:** [Class48.cs](../../ConsoleApplication/ConApp2/Class48.cs)

### Dynamic Directory Creation

```csharp
string path = @"D:\MyApp\";
path = path + DateTime.Now.Year.ToString();
if (!Directory.Exists(path))
{
    Directory.CreateDirectory(path);
}

path = path + "\\" + DateTime.Now.Month.ToString();
if (!Directory.Exists(path))
{
    Directory.CreateDirectory(path);
}
```

**Source:** [Class50.cs](../../ConsoleApplication/ConApp2/Class50.cs)

---

## JSON Handling

### Using Newtonsoft.Json

```csharp
using Newtonsoft.Json;

// Deserialize JSON to object
string json = File.ReadAllText("employees.json");
List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(json);

// Serialize object to JSON
string output = JsonConvert.SerializeObject(employees);
File.WriteAllText("output.json", output);
```

**Source:** [Class54.cs](../../ConsoleApplication/ConApp2/Class54.cs)

### Multiple JSON Approaches

**Method 1: JsonConvert**
```csharp
var data = JsonConvert.DeserializeObject<List<Student>>(json);
```

**Method 2: JsonTextReader with JsonSerializer**
```csharp
var serializer = new Newtonsoft.Json.JsonSerializer();
using var streamReader = new StreamReader(path);
using var textReader = new JsonTextReader(streamReader);
var students = serializer.Deserialize<List<StudentTraining>>(textReader);
```

**Method 3: JArray.Parse**
```csharp
var jarray = JArray.Parse(json);
List<StudentTraining> students = new List<StudentTraining>();
foreach (var item in jarray)
{
    StudentTraining student = item.ToObject<StudentTraining>();
    students.Add(student);
}
```

**Method 4: System.Text.Json**
```csharp
var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var students = JsonSerializer.Deserialize<List<Student>>(json, options);
```

**Source:** [Class55.cs](../../ConsoleApplication/ConApp2/Class55.cs)

---

## Exception Handling

### Try-Catch-Finally

```csharp
try
{
    int[] arr = new int[5];
    arr[10] = 100;  // This will throw exception
}
catch (IndexOutOfRangeException ex)
{
    Console.WriteLine("Index error: " + ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine("General error: " + ex.Message);
}
finally
{
    Console.WriteLine("Cleanup code always runs");
}
```

### Custom Exceptions

```csharp
class MyException : Exception
{
    public MyException() { }
    public MyException(string message) : base(message) { }
    public override string Message => GetMessage();
}

// Throw custom exception
if (a == b)
    throw new MyException();
```

**Source:** [Class19.cs](../../ConsoleApplication/ConApp2/Class19.cs)

---

## Key Takeaways

| Class | Topic |
|-------|-------|
| Class46 | Drive Information |
| Class47 | File Enumeration |
| Class48 | Recursive Directory |
| Class49 | Basic File I/O |
| Class50 | Dynamic Folders |
| Class51 | FileInfo |
| Class52 | Writing Lines |
| Class53 | Reading Lines |
| Class54 | JSON Deserialize |
| Class55 | Multiple JSON Methods |
| Class19 | Exception Handling |

---

## Related Documentation

- [HTTP & Networking](./HTTP_NETWORKING.md) - REST API consumption
- [OOP Concepts](./OOP_CONCEPTS.md) - Exception handling context

---

*Last Updated: 2026-03-11*