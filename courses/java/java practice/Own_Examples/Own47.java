import java.util.Scanner;

// Hospital Patient Record System
public class Own47 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Declare arrays for 4 patients
        String[] names = {"John Smith", "Mary Johnson", "Robert Brown", "Patricia Davis"};
        int[] ages = {45, 62, 35, 78};
        String[] diseases = {"Diabetes", "Heart Disease", "Flu", "Arthritis"};
        int[] wardNumbers = {101, 202, 101, 303};
        
        int choice;
        
        System.out.println("=== Hospital Patient Record System ===");
        System.out.println();
        
        // Menu loop
        do {
            System.out.println("===== Hospital Menu =====");
            System.out.println("1. View all patients");
            System.out.println("2. Search patient by name");
            System.out.println("3. Display patients above age 60");
            System.out.println("4. Display patients in a specific ward");
            System.out.println("5. Exit");
            System.out.print("Enter your choice: ");
            
            choice = scanner.nextInt();
            System.out.println();
            
            switch (choice) {
                case 1:
                    // View all patients
                    System.out.println("=== All Patients ===");
                    for (int i = 0; i < 4; i++) {
                        System.out.println("Patient " + (i + 1) + ":");
                        System.out.println("  Name: " + names[i]);
                        System.out.println("  Age: " + ages[i]);
                        System.out.println("  Disease: " + diseases[i]);
                        System.out.println("  Ward: " + wardNumbers[i]);
                        System.out.println();
                    }
                    break;
                    
                case 2:
                    // Search by name
                    System.out.print("Enter patient name to search: ");
                    scanner.nextLine(); // consume newline
                    String searchName = scanner.nextLine();
                    
                    boolean found = false;
                    for (int i = 0; i < 4; i++) {
                        if (names[i].equalsIgnoreCase(searchName)) {
                            System.out.println("Patient Found!");
                            System.out.println("  Name: " + names[i]);
                            System.out.println("  Age: " + ages[i]);
                            System.out.println("  Disease: " + diseases[i]);
                            System.out.println("  Ward: " + wardNumbers[i]);
                            found = true;
                            break;
                        }
                    }
                    
                    if (!found) {
                        System.out.println("Patient not found!");
                    }
                    break;
                    
                case 3:
                    // Patients above age 60
                    System.out.println("=== Patients Above Age 60 ===");
                    for (int i = 0; i < 4; i++) {
                        if (ages[i] > 60) {
                            System.out.println("  " + names[i] + " - Age: " + ages[i] + " - Ward: " + wardNumbers[i]);
                        }
                    }
                    break;
                    
                case 4:
                    // Patients in a specific ward
                    System.out.print("Enter ward number: ");
                    int searchWard = scanner.nextInt();
                    
                    System.out.println("=== Patients in Ward " + searchWard + " ===");
                    for (int i = 0; i < 4; i++) {
                        if (wardNumbers[i] == searchWard) {
                            System.out.println("  " + names[i] + " - Disease: " + diseases[i] + " - Age: " + ages[i]);
                        }
                    }
                    break;
                    
                case 5:
                    System.out.println("Thank you for using Hospital System!");
                    break;
                    
                default:
                    System.out.println("Invalid choice!");
            }
            
            System.out.println();
            
        } while (choice != 5);
        
        scanner.close();
    }
}
