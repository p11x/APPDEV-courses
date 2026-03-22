/*Array examples
 * 1. one dimensional array
 * 2. two dimensional array
 * 3. jagged array
 * 4. array of objects
 * 5. array of interfaces
 * 6. array of abstract classes
 * 7. array of enums
 * 8. array of strings
 * 9. array of characters
 * 10. array of booleans
 * 11. array of bytes
 * 12. array of shorts
 * 13. array of ints
 * 14. array of longs
 * 15. array of floats
 * 16. array of doubles
 */
public class Example14 {
    public static void main(String[] args) {
        //jagged array
        System.out.println("jagged array");

        int arr[][] = new int[][]{{10,20,30,40,50},{10,20,30}};
        for(int i=0;i<arr.length;i++){
            for(int j=0;j<arr[i].length;j++){
                System.out.print(arr[i][j]+"\t");
            }
            System.out.print("\n");
        }

        System.out.println();




        //array of objects
        System.out.println("array of objects");
        String names[] = new String[] {"Alice","Bob","Charlie"}; //String is a class
        for(String name:names){  
            System.out.println(name);
        }
        System.out.println();




        //array of interfaces
        System.out.println("array of interfaces");
        Comparable<String> names1[] = new Comparable[] {"Alice","Bob","Charlie"};  //Comparable is an interface
        for(Comparable<String> name:names1){
            System.out.println(name);
        }

    }
    
}
