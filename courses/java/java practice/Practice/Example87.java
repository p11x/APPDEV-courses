/*
 * SUB TOPIC: File Operations Basics
 */

import java.io.*;

public class Example87 {
    public static void main(String[] args) throws IOException {
        FileWriter fw = new FileWriter("test.txt");
        fw.write("Hello");
        fw.close();
        
        FileReader fr = new FileReader("test.txt");
        int ch;
        while ((ch = fr.read()) != -1) System.out.print((char)ch);
        fr.close();
    }
}
