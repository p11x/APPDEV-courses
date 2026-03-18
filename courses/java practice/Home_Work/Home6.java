public class Home6 {
    public static void main(String[] args) {
        
        //7.check whether a character is alphabet or not


        char character = 'A';
        if ((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z')) {
            System.out.println("The character is an alphabet");
        } else {
            System.out.println("The character is not an alphabet");
        }
    }

}
