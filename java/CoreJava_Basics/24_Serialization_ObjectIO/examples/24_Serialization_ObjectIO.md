# Java Serialization and Object I/O

## Table of Contents
1. [Object Serialization](#object-serialization)
2. [Serializable Interface](#serializable-interface)
3. [ObjectInputStream and ObjectOutputStream](#objectinputstream-and-objectoutputstream)
4. [Externalizable Interface](#externalizable-interface)
5. [Code Examples](#code-examples)

---

## 1. Object Serialization

### What is Serialization?

Serialization converts Java objects to a format that can be stored or transmitted.

```
┌─────────────────────────────────────────────────────────────┐
│                    SERIALIZATION                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Object ──────► Serialization ──────► Byte Stream          │
│                 (Convert to bytes)                          │
│                                                              │
│   Byte Stream ──► Deserialization ──► Object              │
│                  (Convert back)                             │
│                                                              │
│   Uses:                                                      │
│   ✓ Save objects to files                                   │
│   ✓ Send objects over network                               │
│   ✓ Store objects in database                              │
│   ✓ Cache objects                                           │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Serializable Interface

### Making a Class Serializable

```java
import java.io.Serializable;

class User implements Serializable {
    private static final long serialVersionUID = 1L;
    
    private String name;
    private int age;
    // transient fields are not serialized
    private transient String password;
}
```

---

## 3. ObjectInputStream and ObjectOutputStream

### Writing Objects

```java
// Serialize (write object to file)
FileOutputStream fileOut = new FileOutputStream("user.ser");
ObjectOutputStream out = new ObjectOutputStream(fileOut);
out.writeObject(user);
out.close();
```

### Reading Objects

```java
// Deserialize (read object from file)
FileInputStream fileIn = new FileInputStream("user.ser");
ObjectInputStream in = new ObjectInputStream(fileIn);
User user = (User) in.readObject();
in.close();
```

---

## 4. Externalizable Interface

### Custom Serialization

```java
class User implements Externalizable {
    private String name;
    private int age;
    
    @Override
    public void writeExternal(ObjectOutput out) throws IOException {
        out.writeObject(name);
        out.writeInt(age);
    }
    
    @Override
    public void readExternal(ObjectInput in) throws IOException, 
                                                ClassNotFoundException {
        name = (String) in.readObject();
        age = in.readInt();
    }
}
```

---

## 5. Code Examples

### SerializationDemo

```java
import java.io.*;

class Person implements Serializable {
    private static final long serialVersionUID = 1L;
    
    private String name;
    private int age;
    private String email;
    
    public Person(String name, int age, String email) {
        this.name = name;
        this.age = age;
        this.email = email;
    }
    
    @Override
    public String toString() {
        return "Person{name='" + name + "', age=" + age + ", email='" + email + "'}";
    }
}

public class SerializationDemo {
    public static void main(String[] args) {
        System.out.println("=== SERIALIZATION DEMO ===\n");
        
        // Create object
        Person person = new Person("John", 30, "john@example.com");
        String filename = "person.ser";
        
        // Serialize (write to file)
        try (ObjectOutputStream out = new ObjectOutputStream(
                new FileOutputStream(filename))) {
            out.writeObject(person);
            System.out.println("Object serialized: " + person);
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        // Deserialize (read from file)
        try (ObjectInputStream in = new ObjectInputStream(
                new FileInputStream(filename))) {
            Person loadedPerson = (Person) in.readObject();
            System.out.println("Object deserialized: " + loadedPerson);
        } catch (IOException | ClassNotFoundException e) {
            e.printStackTrace();
        }
        
        // Clean up
        new File(filename).delete();
        System.out.println("\nDemo complete!");
    }
}
```

---

## Summary

### Key Takeaways

1. **Serializable** - Marker interface for serialization
2. **serialVersionUID** - Version control for serialized objects
3. **transient** - Fields not serialized
4. **ObjectOutputStream** - Write objects
5. **ObjectInputStream** - Read objects

---

*Serialization Complete!*
