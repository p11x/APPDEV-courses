/*
 * SUB TOPIC: Custom Exception
 */

class MyException extends Exception {
    MyException(String msg) { super(msg); }
}

public class Example84 {
    public static void main(String[] args) {
        try {
            throw new MyException("Custom error");
        } catch (MyException e) {
            System.out.println(e.getMessage());
        }
    }
}
