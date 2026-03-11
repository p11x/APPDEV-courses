// AppConfigDemo - Demonstrates Static Config for Angular Integration
// Use case: Application-wide configuration settings

public class AppConfigDemo {
    // Static config - same for all users
    private static String API_BASE_URL = "https://api.example.com";
    private static int TIMEOUT_SECONDS = 30;
    private static boolean DEBUG_MODE = true;
    
    // Instance config - per user
    private String userToken;
    private String currentLanguage;
    
    public AppConfigDemo(String userToken, String language) {
        this.userToken = userToken;
        this.currentLanguage = language;
    }
    
    // Static getters
    public static String getApiBaseUrl() {
        return API_BASE_URL;
    }
    
    public static int getTimeout() {
        return TIMEOUT_SECONDS;
    }
    
    public static boolean isDebugMode() {
        return DEBUG_MODE;
    }
    
    // Static setter - modify global config
    public static void setApiBaseUrl(String url) {
        API_BASE_URL = url;
    }
    
    // Instance getters
    public String getUserToken() {
        return userToken;
    }
    
    public String getCurrentLanguage() {
        return currentLanguage;
    }
    
    public static void main(String[] args) {
        System.out.println("=== STATIC CONFIG FOR ANGULAR ===\n");
        
        // Access static config globally
        System.out.println("API URL: " + AppConfigDemo.getApiBaseUrl());
        System.out.println("Timeout: " + AppConfigDemo.getTimeout());
        System.out.println("Debug: " + AppConfigDemo.isDebugMode());
        
        // Create user-specific instances
        AppConfigDemo user1 = new AppConfigDemo("token-123", "en");
        AppConfigDemo user2 = new AppConfigDemo("token-456", "es");
        
        System.out.println("\n--- User 1 ---");
        System.out.println("Token: " + user1.getUserToken());
        System.out.println("Language: " + user1.getCurrentLanguage());
        
        System.out.println("\n--- User 2 ---");
        System.out.println("Token: " + user2.getUserToken());
        System.out.println("Language: " + user2.getCurrentLanguage());
        
        // Update global config
        AppConfigDemo.setApiBaseUrl("https://new-api.example.com");
        System.out.println("\n--- Updated Global Config ---");
        System.out.println("New API URL: " + AppConfigDemo.getApiBaseUrl());
        
        System.out.println("\n=== USE CASES ===");
        System.out.println("1. Global API endpoints");
        System.out.println("2. Feature flags");
        System.out.println("3. Environment settings");
        System.out.println("4. Per-user tokens (instance)");
    }
}
