# Java Integration with rJava

## Learning Objectives

- Set up rJava for R-Java communication
- Create Java objects from R
- Call Java methods from R
- Pass data between R and Java
- Use JRI (Java R Interface)

## Theoretical Background

### rJava Package

rJava provides a low-level interface between R and Java, enabling:

1. **JRI** - Java R Interface (run R from Java)
2. **JNI** - Java Native Interface (run Java from R)
3. **Direct method calls** - Call Java methods directly from R

### Installation Requirements

rJava requires:
- Java Development Kit (JDK) installed
- JAVA_HOME environment variable set
- Appropriate system configuration

### Architecture

```
R <--> rJava <--> JVM <--> Java Code
```

This allows:
- Creating Java objects in R
- Calling Java methods
- Accessing Java libraries
- Running R from Java

## Code Examples

### Standard Example: Basic rJava Setup

```r
# ===== RJAVA SETUP =====

# Install rJava if needed
# install.packages("rJava")

cat("===== LOADING RJAVA =====\n")

# Load rJava library
library(rJava)

# Initialize JVM (usually automatic, but can be explicit)
.jinit()

# Check Java version
cat("Java version:\n")
print(.jcall("java/lang/System", "S", "getProperty", "java.version"))

# Check Java vendor
cat("\nJava vendor:\n")
print(.jcall("java/lang/System", "S", "getProperty", "java.vendor"))

# Check classpath
cat("\nCurrent classpath:\n")
print(.jclassPath())
```

### Standard Example: Creating Java Objects

```r
# ===== JAVA OBJECTS =====
cat("\n===== CREATE JAVA OBJECTS =====\n")

# Create a Java String
jstr <- .jnew("java/lang/String", "Hello from Java!")
class(jstr)
cat("Created Java String:\n")
print(.jstr(jstr))

# Create Java ArrayList
jal <- .jnew("java/util/ArrayList")
cat("\nCreated ArrayList, is empty:", .jcall(jal, "Z", "isEmpty"), "\n")

# Add elements to ArrayList
.jcall(jal, "Ljava/util/Collection;", "add", .jnew("java/lang/String", "item1"))
.jcall(jal, "Ljava/util/Collection;", "add", .jnew("java/lang/String", "item2"))
.jcall(jal, "Ljava/util/Collection;", "add", .jnew("java/lang/String", "item3"))

cat("ArrayList size after adding:", .jcall(jal, "I", "size"), "\n")

# Get elements
cat("\nElements in ArrayList:\n")
elements <- .jcall(jal, "[Ljava/lang/Object;", "toArray")
print(sapply(elements, .jstr))

# Create Java HashMap
jmap <- .jnew("java/util/HashMap")
.jcall(jmap, "Ljava/lang/Object;", "put", 
       .jnew("java/lang/String", "key1"),
       .jnew("java/lang/String", "value1"))
.jcall(jmap, "Ljava/lang/Object;", "put", 
       .jnew("java/lang/String", "key2"),
       .jnew("java/lang/String", "value2"))

cat("\nHashMap contents:\n")
print(.jstr(.jcall(jmap, "Ljava/lang/String;", "toString")))
```

### Standard Example: Java Method Calls

```r
# ===== JAVA METHOD CALLS =====
cat("\n===== CALL JAVA METHODS =====\n")

# String operations
text <- .jnew("java/lang/String", "  Hello Java World!  ")
cat("Original: '", .jstr(text), "'\n", sep = "")

# trim()
cat("After trim(): '", .jstr(.jcall(text, "S", "trim")), "'\n")

# toUpperCase()
cat("Uppercase: '", .jstr(.jcall(text, "S", "toUpperCase")), "'\n")

# substring(7, 12)
cat("Substring(7,12): '", .jstr(.jcall(text, "S", "substring", as.integer(7), as.integer(12))), "'\n")

# Math operations
cat("\n===== JAVA MATH =====\n")

# Math.pow()
result <- .jcall("java/lang/Math", "D", "pow", as.double(2), as.integer(8))
cat("Math.pow(2, 8) =", result, "\n")

# Math.sqrt()
result <- .jcall("java/lang/Math", "D", "sqrt", as.double(16))
cat("Math.sqrt(16) =", result, "\n")

# Math.abs()
result <- .jcall("java/lang/Math", "D", "abs", as.double(-5.5))
cat("Math.abs(-5.5) =", result, "\n")
```

### Standard Example: Converting R and Java Objects

```r
# ===== DATA CONVERSION =====
cat("\n===== DATA CONVERSION =====\n")

# R vector to Java array
cat("R vector to Java array:\n")
r_vec <- c(1, 2, 3, 4, 5)

# Create Java int array from R vector
j_arr <- .jarray(as.integer(r_vec))
cat("Java int array created\n")

# Java array to R vector
cat("Java array to R vector:\n")
r_from_j <- .jevalArray(j_arr)
print(r_from_j)

# R matrix to Java 2D array
cat("\nR matrix to Java 2D array:\n")
r_mat <- matrix(1:6, nrow = 2, ncol = 3)

# Convert to Java array
j_mat <- .jarray(as.integer(r_mat))
class(j_mat) <- "[I"  # int[][]
cat("Java 2D array created\n")

# Java 2D array to R matrix
cat("Java 2D array to R:\n")
r_from_mat <- .jevalArray(j_mat)
print(matrix(r_from_mat, nrow = 2))

# Java List to R vector
cat("\nJava List to R vector:\n")
jlist <- .jnew("java/util/Arrays", "asList", 
               .jarray(as.integer(c(10, 20, 30))))
r_from_list <- .jevalArray(.jcall(jlist, "[Ljava/lang/Object;", "toArray"))
print(r_from_list)
```

### Standard Example: Custom Java Classes

```r
# ===== CUSTOM JAVA CLASSES =====
cat("\n===== USING CUSTOM CLASSES =====\n")

# Example: Using your own Java class
# First, create the Java class:

java_class_code <- '
// SimplePerson.java
public class SimplePerson {
  private String name;
  private int age;
  
  public SimplePerson(String name, int age) {
    this.name = name;
    this.age = age;
  }
  
  public String getName() {
    return name;
  }
  
  public int getAge() {
    return age;
  }
  
  public void setName(String name) {
    this.name = name;
  }
  
  public void setAge(int age) {
    this.age = age;
  }
  
  public String introduce() {
    return "Hello, I am " + name + " and I am " + age + " years old.";
  }
}
'

cat("Example Java class:\n")
cat(java_class_code)

# Compile and use:
# javac SimplePerson.java
# Then in R:

cat("\n\n===== USING COMPILED CLASS =====\n")
usage_example <- "
# Add compiled class to classpath
.jaddClassPath(\"/path/to/SimplePerson.class\")

# Create Person object
person <- .jnew(\"SimplePerson\", \"Alice\", 30)

# Call methods
cat(\"Name:\", .jcall(person, \"S\", \"getName\"))
cat(\"Age:\", .jcall(person, \"I\", \"getAge\"))
cat(\"Introduction:\", .jcall(person, \"S\", \"introduce\"))

# Set new values
.jcall(person, \"V\", \"setName\", \"Bob\")
.jcall(person, \"V\", \"setAge\", 25)
"
cat(usage_example)
```

### Standard Example: JRI (Java R Interface)

```r
# ===== JRI BASICS =====
cat("\n===== JRI (RUN R FROM JAVA) =====\n")

jri_example <- '
# JRI allows running R from within Java
# This requires rJRI package (JRInterface)

# Java code to run R:
/*
import org.renmin.base.JR;
import org.renmin.base.REXP;
import org.renmin.base.RFactor;

public class JRIExample {
  public static void main(String[] args) {
    // Create R engine
    REXP x;
    
    // Simple calculation
    x = R.eval(\"1 + 2\");
    System.out.println(\"1 + 2 = \" + x.asInteger());
    
    // Plot (saved to file)
    R.eval(\"png('plot.png')\");
    R.eval(\"plot(1:10, main='R Plot from Java')\");
    R.eval(\"dev.off()\");
    
    // Data frame operations
    R.eval(\"df <- data.frame(x=1:5, y=6:10)\");
    R.eval(\"summary(df)\");
  }
}
*/
cat("JRI Java code example (runs inside JVM):\n")
cat(jri_example)

cat("\n\n===== COMMON ERRORS =====\n")
error_handling <- "
# Common rJava errors and fixes:

# 1. JAVA_HOME not set
# Fix: Set environment variable
Sys.setenv(JAVA_HOME = \"/path/to/jdk\")

# 2. No JVM found
# Fix: Reinitialize
.jinit()

# 3. Class not found
# Fix: Add to classpath
.jaddClassPath(\"/path/to/class\")

# 4. Method not found
# Fix: Check method signature
.jmethods(class_object)
"
cat(error_handling)
```