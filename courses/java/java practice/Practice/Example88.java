/*
 * SUB TOPIC: File Class
 */

import java.io.*;

public class Example88 {
    public static void main(String[] args) {
        File f = new File("test.txt");
        System.out.println("Exists: " + f.exists());
        System.out.println("IsFile: " + f.isFile());
        System.out.println("Name: " + f.getName());
    }
}
