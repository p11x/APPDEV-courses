import java.util.Scanner;

// Simple Library Book Tracker
public class Own17 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Declare arrays for 5 books
        String[] bookTitles = new String[5];
        boolean[] availability = new boolean[5];
        
        // Initialize books and their availability
        bookTitles[0] = "Harry Potter";
        bookTitles[1] = "The Great Gatsby";
        bookTitles[2] = "To Kill a Mockingbird";
        bookTitles[3] = "1984";
        bookTitles[4] = "Pride and Prejudice";
        
        // All books initially available
        for (int i = 0; i < 5; i++) {
            availability[i] = true;
        }
        
        int choice;
        
        // Menu loop
        do {
            System.out.println();
            System.out.println("=== Library Book Tracker ===");
            System.out.println("1. View all books");
            System.out.println("2. Issue a book");
            System.out.println("3. Return a book");
            System.out.println("4. Exit");
            System.out.print("Enter your choice: ");
            
            choice = scanner.nextInt();
            System.out.println();
            
            switch (choice) {
                case 1:
                    // View all books
                    System.out.println("=== All Books ===");
                    for (int i = 0; i < 5; i++) {
                        System.out.print((i + 1) + ". " + bookTitles[i]);
                        if (availability[i]) {
                            System.out.println(" - Available");
                        } else {
                            System.out.println(" - Not Available");
                        }
                    }
                    break;
                    
                case 2:
                    // Issue a book
                    System.out.println("=== Issue a Book ===");
                    System.out.println("Available books:");
                    for (int i = 0; i < 5; i++) {
                        if (availability[i]) {
                            System.out.println((i + 1) + ". " + bookTitles[i]);
                        }
                    }
                    System.out.print("Enter book number to issue: ");
                    int issueBook = scanner.nextInt();
                    
                    if (issueBook >= 1 && issueBook <= 5) {
                        if (availability[issueBook - 1]) {
                            availability[issueBook - 1] = false;
                            System.out.println("Book '" + bookTitles[issueBook - 1] + "' issued successfully!");
                        } else {
                            System.out.println("Book is not available!");
                        }
                    } else {
                        System.out.println("Invalid book number!");
                    }
                    break;
                    
                case 3:
                    // Return a book
                    System.out.println("=== Return a Book ===");
                    System.out.println("Issued books:");
                    for (int i = 0; i < 5; i++) {
                        if (!availability[i]) {
                            System.out.println((i + 1) + ". " + bookTitles[i]);
                        }
                    }
                    System.out.print("Enter book number to return: ");
                    int returnBook = scanner.nextInt();
                    
                    if (returnBook >= 1 && returnBook <= 5) {
                        if (!availability[returnBook - 1]) {
                            availability[returnBook - 1] = true;
                            System.out.println("Book '" + bookTitles[returnBook - 1] + "' returned successfully!");
                        } else {
                            System.out.println("Book was not issued!");
                        }
                    } else {
                        System.out.println("Invalid book number!");
                    }
                    break;
                    
                case 4:
                    // Exit
                    System.out.println("Thank you for using Library Tracker!");
                    break;
                    
                default:
                    System.out.println("Invalid choice! Please enter 1-4.");
            }
            
        } while (choice != 4);
        
        scanner.close();
    }
}
