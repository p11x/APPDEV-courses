// UserAccessControlDemo - Demonstrates Control Flow for Access Control
// Use case: Role-based access control for Angular apps

public class UserAccessControlDemo {
    
    // Enum for user roles
    enum Role { ADMIN, USER, GUEST, MODERATOR }
    
    // Method demonstrating control flow for access control
    public static String checkAccess(Role role, String resource) {
        System.out.println("\n=== ACCESS CONTROL CHECK ===");
        System.out.println("User Role: " + role);
        System.out.println("Requested Resource: " + resource);
        
        // Nested if-else for access levels
        if (role == Role.ADMIN) {
            // Admin has full access
            if (resource.equals("settings") || resource.equals("users") || resource.equals("reports")) {
                return "ACCESS GRANTED - Full permissions";
            } else {
                return "ACCESS GRANTED - All resources";
            }
        } else if (role == Role.MODERATOR) {
            // Moderator has limited access
            if (resource.equals("users")) {
                return "ACCESS GRANTED - View and edit users";
            } else if (resource.equals("reports")) {
                return "ACCESS GRANTED - View reports only";
            } else {
                return "ACCESS DENIED - Insufficient permissions";
            }
        } else if (role == Role.USER) {
            // Regular user access
            if (resource.equals("profile") || resource.equals("dashboard")) {
                return "ACCESS GRANTED - Own data only";
            } else {
                return "ACCESS DENIED - User cannot access " + resource;
            }
        } else {
            // Guest has minimal access
            if (resource.equals("public")) {
                return "ACCESS GRANTED - Public content";
            } else {
                return "ACCESS DENIED - Login required";
            }
        }
    }
    
    // Switch statement for feature flags
    public static String getFeatureLevel(Role role) {
        System.out.println("\n=== FEATURE FLAGS ===");
        
        switch (role) {
            case ADMIN:
                return "ALL_FEATURES";
            case MODERATOR:
                return "MODERATOR_FEATURES";
            case USER:
                return "STANDARD_FEATURES";
            case GUEST:
                return "BASIC_FEATURES";
            default:
                return "NO_FEATURES";
        }
    }
    
    // Loop example for processing permissions
    public static String[] getPermissions(Role role) {
        System.out.println("\n=== PERMISSION LIST ===");
        
        String[] permissions;
        
        switch (role) {
            case ADMIN:
                permissions = new String[] {"read", "write", "delete", "admin", "audit"};
                break;
            case MODERATOR:
                permissions = new String[] {"read", "write", "edit_users", "view_reports"};
                break;
            case USER:
                permissions = new String[] {"read", "write_own", "edit_profile"};
                break;
            default:
                permissions = new String[] {"read_public"};
        }
        
        // Enhanced for loop
        for (String perm : permissions) {
            System.out.println("- " + perm);
        }
        
        return permissions;
    }
    
    public static void main(String[] args) {
        System.out.println("=== CONTROL FLOW FOR ACCESS CONTROL ===\n");
        
        // Test different roles
        Role[] roles = {Role.ADMIN, Role.MODERATOR, Role.USER, Role.GUEST};
        String[] resources = {"settings", "users", "dashboard", "public"};
        
        // Nested loops for testing
        for (Role role : roles) {
            for (String resource : resources) {
                String result = checkAccess(role, resource);
                System.out.println("Result: " + result);
            }
        }
        
        // Feature flags
        for (Role role : roles) {
            System.out.println("\nRole: " + role + " -> Features: " + getFeatureLevel(role));
        }
        
        // Permissions list
        getPermissions(Role.ADMIN);
        
        System.out.println("\n=== ANGULAR USE CASE ===");
        System.out.println("1. Route guards (*ngIf based on roles)");
        System.out.println("2. Button visibility (*ngIf='canEdit')");
        System.out.println("3. Feature flags for A/B testing");
        System.out.println("4. Permission-based UI rendering");
    }
}
