/* array operations */
import java.util.Arrays;
public class Example13 {
    public static void main(String[] args) {
        int a [] = new int [5];
        System.out.println("enter 5 array elements");
        for(int i=0;i<a.length;i++){
            System.out.println("a ["+i+"] = ");
            a[i] = Integer.parseInt(System.console().readLine());
            
        }
        System.out.println("Before sorting array elements are : ");
        for(int i=0;i<a.length;i++){
            System.out.print("a ["+i+"] = "+a[i]);
        }
        System.out.println();


        //sorting array elements
        /*for(int i=0;i<a.length;i++){
            for(int j=i+1;j<a.length;j++){
                if(a[i]>a[j]){
                    int temp = a[i];
                    a[i] = a[j];
                    a[j] = temp;
                }
            }
        }/*
         */
        Arrays.sort(a);

        System.out.println("After sorting array elements are : ");
        for(int i=0;i<a.length;i++){
            System.out.print("a ["+i+"] = "+a[i]+"\t");
        }
        System.out.println("\n");

        //searching an element in array

        System.out.println("Enter the number to be searched : ");
        int key = Integer.parseInt(System.console().readLine());
        int index= Arrays.binarySearch(a, key);

        if (index<0){
            System.out.println("Element"+key+" not found");
        }else{
            System.out.println("Element"+key+" found at index "+index);
        }

    }
    
}
