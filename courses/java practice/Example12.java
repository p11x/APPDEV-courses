public class Example12 {
    public static void main(String[] args) {
        int arr[] = new int[] {10,20,30,40,50};
        System.out.println("one dimensional array ");
        for(int i=0;i<arr.length;i++){
            System.out.print(arr[i]+"\t" );

        }
        System.out.println();


        //two dimensional array
        System.out.println("two dimensional array ");
        int arr2[][] = new int[][]{{10,20,30},{40,50,60}};
        for(int i=0;i<arr2.length;i++){
            for(int j=0;j<arr2[i].length;j++){
                System.out.print(arr2[i][j]+"\t");
            }
            System.out.print("\n");
        }


    
    }
}
