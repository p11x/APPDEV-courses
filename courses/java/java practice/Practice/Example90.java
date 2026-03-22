/*
 * SUB TOPIC: Comparable Interface
 */

import java.util.*;

class Student implements Comparable<Student> {
    String name;
    int age;
    Student(String name, int age) { this.name = name; this.age = age; }
    public int compareTo(Student s) { return this.age - s.age; }
}

public class Example90 {
    public static void main(String[] args) {
        List<Student> list = new ArrayList<>();
        list.add(new Student("John", 20));
        list.add(new Student("Jane", 18));
        Collections.sort(list);
        for (Student s : list) System.out.println(s.name + " " + s.age);
    }
}
