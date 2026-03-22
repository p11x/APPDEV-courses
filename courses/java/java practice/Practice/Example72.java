/*
 * SUB TOPIC: Super Keyword and this Keyword
 * 
 * DEFINITION:
 * 'this' refers to current object, 'super' refers to parent class. Used to differentiate between class members.
 */

class Parent {
    int value = 10;
    Parent() { System.out.println("Parent constructor"); }
}

class Child extends Parent {
    int value = 20;
    Child() { super(); System.out.println("Child constructor"); }
    void show() {
        System.out.println("this.value: " + this.value);
        System.out.println("super.value: " + super.value);
    }
}

public class Example72 {
    public static void main(String[] args) {
        Child c = new Child();
        c.show();
    }
}
