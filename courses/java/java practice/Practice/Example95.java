/*
 * SUB TOPIC: Properties Class in Java
 * 
 * DEFINITION:
 * Properties is a subclass of Hashtable that is used to maintain lists of values in which 
 * both keys and values are strings. It is commonly used for configuration files and 
 * preferences settings. Properties can be loaded from and saved to a stream, making it 
 * ideal for handling application configuration.
 * 
 * FUNCTIONALITIES:
 * 1. setProperty() - Set string key-value pair
 * 2. getProperty() - Get value by key (returns null if not found)
 * 3. getProperty(key, defaultValue) - Get with default value
 * 4. load() - Load properties from InputStream/Reader
 * 5. store() - Save properties to OutputStream/Writer
 * 6. stringPropertyNames() - Get all property names
 * 7. contains() / containsValue() - Check existence
 * 8. list() - Print to PrintStream for debugging
 */

import java.util.*;
import java.io.*;

public class Example95 {
    public static void main(String[] args) {
        
        // Creating a Properties object
        Properties config = new Properties();
        
        // setProperty() - Adding properties
        config.setProperty("username", "admin");
        config.setProperty("password", "secret123");
        config.setProperty("host", "localhost");
        config.setProperty("port", "8080");
        config.setProperty("database", "mydb");
        
        System.out.println("=== Basic Properties Operations ===");
        System.out.println("All properties: " + config);
        
        // getProperty() - Getting values
        System.out.println("\nusername: " + config.getProperty("username"));
        System.out.println("port: " + config.getProperty("port"));
        System.out.println("missing: " + config.getProperty("missing")); // Returns null
        
        // getProperty with default value
        System.out.println("\nWith default value:");
        System.out.println("timeout: " + config.getProperty("timeout", "30"));
        System.out.println("maxConnections: " + config.getProperty("maxConnections", "100"));
        
        // contains() - Check key existence
        System.out.println("\nContains 'host': " + config.contains("host"));
        System.out.println("Contains 'email': " + config.contains("email"));
        
        // containsValue() - Check value existence
        System.out.println("\nContains value 'admin': " + config.containsValue("admin"));
        
        // stringPropertyNames() - Get all keys
        System.out.println("\nProperty names: " + config.stringPropertyNames());
        
        // System.getProperties() - System properties
        System.out.println("\n=== System Properties ===");
        Properties sysProps = System.getProperties();
        System.out.println("Java version: " + System.getProperty("java.version"));
        System.out.println("OS name: " + System.getProperty("os.name"));
        System.out.println("User home: " + System.getProperty("user.home"));
        
        // Real-time Example 1: Database configuration
        System.out.println("\n=== Example 1: Database Configuration ===");
        Properties dbConfig = new Properties();
        dbConfig.setProperty("db.url", "jdbc:mysql://localhost:3306/mydb");
        dbConfig.setProperty("db.driver", "com.mysql.cj.jdbc.Driver");
        dbConfig.setProperty("db.username", "root");
        dbConfig.setProperty("db.password", "password");
        dbConfig.setProperty("db.pool.size", "10");
        
        System.out.println("Database Configuration:");
        dbConfig.list(System.out);
        
        // Real-time Example 2: Application settings
        System.out.println("\n=== Example 2: Application Settings ===");
        Properties appSettings = new Properties();
        appSettings.setProperty("app.name", "MyApplication");
        appSettings.setProperty("app.version", "1.0.0");
        appSettings.setProperty("app.environment", "development");
        appSettings.setProperty("debug.mode", "true");
        appSettings.setProperty("max.upload.size", "10485760"); // 10MB in bytes
        
        String env = appSettings.getProperty("app.environment");
        System.out.println("Running in: " + env);
        
        if (Boolean.parseBoolean(appSettings.getProperty("debug.mode"))) {
            System.out.println("Debug mode enabled");
        }
        
        // Real-time Example 3: Server configuration
        System.out.println("\n=== Example 3: Server Configuration ===");
        Properties serverProps = new Properties();
        serverProps.setProperty("server.port", "8080");
        serverProps.setProperty("server.host", "0.0.0.0");
        serverProps.setProperty("server.ssl.enabled", "false");
        serverProps.setProperty("server.timeout", "30000");
        serverProps.setProperty("server.maxThreads", "200");
        
        System.out.println("Server starting on port: " + serverProps.getProperty("server.port"));
        System.out.println("Max threads: " + serverProps.getProperty("server.maxThreads"));
        
        int timeout = Integer.parseInt(serverProps.getProperty("server.timeout"));
        System.out.println("Timeout: " + timeout + "ms");
        
        // Real-time Example 4: Email configuration
        System.out.println("\n=== Example 4: Email Configuration ===");
        Properties emailProps = new Properties();
        emailProps.setProperty("mail.smtp.host", "smtp.gmail.com");
        emailProps.setProperty("mail.smtp.port", "587");
        emailProps.setProperty("mail.smtp.auth", "true");
        emailProps.setProperty("mail.smtp.starttls.enable", "true");
        emailProps.setProperty("mail.from", "noreply@example.com");
        
        System.out.println("SMTP Host: " + emailProps.getProperty("mail.smtp.host"));
        System.out.println("SMTP Port: " + emailProps.getProperty("mail.smtp.port"));
        
        // Real-time Example 5: Logging configuration
        System.out.println("\n=== Example 5: Logging Configuration ===");
        Properties logProps = new Properties();
        logProps.setProperty("log.level", "INFO");
        logProps.setProperty("log.file", "application.log");
        logProps.setProperty("log.max.size", "10MB");
        logProps.setProperty("log.pattern", "%d{yyyy-MM-dd} %-5p %c{1}:%L - %m%n");
        
        String logLevel = logProps.getProperty("log.level");
        System.out.println("Log level set to: " + logLevel);
        
        // Real-time Example 6: Feature flags
        System.out.println("\n=== Example 6: Feature Flags ===");
        Properties features = new Properties();
        features.setProperty("feature.dark.mode", "true");
        features.setProperty("feature.new.ui", "false");
        features.setProperty("feature.beta.search", "true");
        features.setProperty("feature.analytics", "false");
        
        System.out.println("Feature flags:");
        for (String key : features.stringPropertyNames()) {
            boolean enabled = Boolean.parseBoolean(features.getProperty(key));
            System.out.println("  " + key + ": " + (enabled ? "ENABLED" : "DISABLED"));
        }
        
        // Additional operations
        System.out.println("\n=== Additional Operations ===");
        Properties testProps = new Properties();
        testProps.setProperty("A", "1");
        testProps.setProperty("B", "2");
        
        // Using put() and putAll()
        testProps.put("C", "3");
        System.out.println("After put: " + testProps);
        
        // Remove property
        testProps.remove("C");
        System.out.println("After remove: " + testProps);
        
        // Clear all
        testProps.clear();
        System.out.println("After clear, isEmpty: " + testProps.isEmpty());
        
        // Default properties
        System.out.println("\n=== Default Properties ===");
        Properties defaults = new Properties();
        defaults.setProperty("theme", "light");
        defaults.setProperty("language", "en");
        
        Properties userPrefs = new Properties(defaults); // Set defaults
        userPrefs.setProperty("theme", "dark"); // Override default
        
        System.out.println("Theme: " + userPrefs.getProperty("theme")); // dark
        System.out.println("Language: " + userPrefs.getProperty("language")); // en (from defaults)
    }
}
