// DataProviderDemo - Demonstrates Interfaces for Data Provider Pattern
// Use case: Multiple data sources for Angular app

public class DataProviderDemo {
    
    // Interface for data providers
    interface DataProvider {
        String getData();
        String getName();
    }
    
    // Interface with default method
    interface AdvancedDataProvider extends DataProvider {
        default String processData() {
            return "Processing: " + getData();
        }
        
        // Static method in interface
        static DataProvider create() {
            return new RESTDataProvider();
        }
    }
    
    // REST API implementation
    static class RESTDataProvider implements DataProvider {
        private String endpoint;
        
        public RESTDataProvider() {
            this.endpoint = "/api/data";
        }
        
        @Override
        public String getData() {
            return "REST Data from " + endpoint;
        }
        
        @Override
        public String getName() {
            return "REST Provider";
        }
    }
    
    // Database implementation
    static class DatabaseDataProvider implements DataProvider {
        private String query;
        
        public DatabaseDataProvider() {
            this.query = "SELECT * FROM users";
        }
        
        @Override
        public String getData() {
            return "DB Data: " + query;
        }
        
        @Override
        public String getName() {
            return "Database Provider";
        }
    }
    
    // Cache implementation
    static class CacheDataProvider implements AdvancedDataProvider {
        private String cachedData;
        
        public CacheDataProvider() {
            this.cachedData = "Cached User Data";
        }
        
        @Override
        public String getData() {
            return cachedData;
        }
        
        @Override
        public String getName() {
            return "Cache Provider";
        }
        
        @Override
        public String processData() {
            return "Cache processed: " + cachedData;
        }
    }
    
    // Service using provider
    static class DataService {
        private DataProvider provider;
        
        public DataService(DataProvider provider) {
            this.provider = provider;
        }
        
        public void fetchData() {
            System.out.println("Provider: " + provider.getName());
            System.out.println("Data: " + provider.getData());
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== INTERFACES FOR DATA PROVIDERS ===\n");
        
        // Use REST provider
        System.out.println("--- REST Provider ---");
        DataProvider rest = new RESTDataProvider();
        DataService service1 = new DataService(rest);
        service1.fetchData();
        
        // Use Database provider
        System.out.println("\n--- Database Provider ---");
        DataProvider db = new DatabaseDataProvider();
        DataService service2 = new DataService(db);
        service2.fetchData();
        
        // Use Cache provider (implements AdvancedDataProvider)
        System.out.println("\n--- Cache Provider (Advanced) ---");
        AdvancedDataProvider cache = new CacheDataProvider();
        System.out.println("Name: " + cache.getName());
        System.out.println("Data: " + cache.getData());
        System.out.println("Processed: " + cache.processData());
        
        // Use static factory
        System.out.println("\n--- Static Factory ---");
        DataProvider created = AdvancedDataProvider.create();
        System.out.println("Created: " + created.getName());
        
        // Multiple providers in list
        System.out.println("\n--- All Providers ---");
        DataProvider[] providers = {rest, db, cache};
        for (DataProvider p : providers) {
            System.out.println("- " + p.getName() + ": " + p.getData());
        }
        
        System.out.println("\n=== ANGULAR USE CASES ===");
        System.out.println("1. REST API data service");
        System.out.println("2. Local storage service");
        System.out.println("3. WebSocket data service");
        System.out.println("4. Mock data for testing");
        System.out.println("5. Fallback data providers");
    }
}
