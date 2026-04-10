# Timeouts and Retries Pattern
## Overview
Timeouts are a fundamental resilience pattern that prevents applications from hanging indefinitely when waiting for responses from remote services or resources. A timeout defines the maximum time an operation can take before it is considered failed, allowing the application to recover and respond quickly rather than waiting indefinitely.  There are several types of timeouts in distributed systems:  **Connection Timeout**: The maximum time to establish a connection to a remote server. This includes the TCP handshake and SSL negotiation time. If the connection cannot be established within this time, the application fails fast and can attempt alternatives.  **Read Timeout (Socket Timeout)**: The maximum time to wait for data after a connection is established. If no data is received within this time, the connection is considered stale and should be terminated.  **Connection Request Timeout**: The maximum time to wait when acquiring a connection from a connection pool.  **Total Request Timeout**: An end-to-end timeout that encompasses the entire operation from request initiation to response completion. ### Why Timeouts Matter In distributed systems, network issues, server overloads, or service failures can cause operations to hang indefinitely. Without timeouts:  - Thread pools become exhausted waiting for responses  - User interfaces freeze  - Resources are wasted on stuck requests  - Cascading failures can occur when services are overwhelmed by waiting requests  Proper timeout configuration is critical for maintaining system responsiveness and preventing resource exhaustion.  ## Flow Chart
```mermaid
flowchart TD
    A[Start Operation] --> B[Start Connection Timer]
    B --> C{Timer Exceeded?}
    C -->|Yes| D[Connection Timeout]
    C -->|No| E{Connection Established?}
    E -->|No| B
    E -->|Yes| F[Start Read Timer]
    F --> G{Timer Exceeded?}
    G -->|Yes| H[Read Timeout]
    G -->|No| I{Data Received?}
    I -->|No| F
    I -->|Yes| J[Process Response]
    J --> K[Success - Return Result]
    D --> L[Close Connection]
    H --> L
    L --> M[Return Timeout Error]
    
    style D fill:#ff6b6b
    style H fill:#ff6b6b
    style M fill:#ffd43b
```  ## Standard Example (Java) ### Socket and Connection Timeout Configuration ```java
import java.net.HttpURLConnection;
import java.net.URL;
import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.SocketTimeoutException;
import java.net.ConnectException;
public class TimeoutExample {
    private static final int CONNECT_TIMEOUT = 5000;   // 5 seconds
    private static final int READ_TIMEOUT = 10000;     // 10 seconds
    
    public static void main(String[] args) {        try {
                String result = callServiceWithTimeout(
                    "http://api.example.com/data",
                    CONNECT_TIMEOUT,
                    READ_TIMEOUT
                );
                System.out.println("Success: " + result);
            } catch (SocketTimeoutException e) {
                System.out.println("Read timeout occurred: " + e.getMessage());
                handleTimeoutFallback();
            } catch (ConnectException e) {
                System.out.println("Connection timeout occurred: " + e.getMessage());
                handleConnectionFallback();
            } catch (Exception e) {
                System.out.println("Error: " + e.getMessage());
            }
        }
        
        private static String callServiceWithTimeout(            String serviceUrl, 
            int connectTimeoutMs, 
            int readTimeoutMs) throws Exception {
            
            URL url = new URL(serviceUrl);
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            
            // Set connection timeout
            connection.setConnectTimeout(connectTimeoutMs);
            
            // Set read timeout
            connection.setReadTimeout(readTimeoutMs);
            
            // Configure request
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Accept", "application/json");
            
            // Execute request
            int responseCode = connection.getResponseCode();
            
            if (responseCode == 200) {
                BufferedReader reader = new BufferedReader(
                    new InputStreamReader(connection.getInputStream())
                );
                StringBuilder response = new StringBuilder();
                String line;
                while ((line = reader.readLine()) != null) {
                    response.append(line);
                }
                reader.close();
                return response.toString();
            } else {
                throw new RuntimeException("HTTP " + responseCode);
            }
        }
        
        private static void handleTimeoutFallback() {
            System.out.println("Executing fallback due to timeout");
        }
        
        private static void handleConnectionFallback() {            System.out.println("Executing fallback due to connection failure");
        }
}  ```  ### Apache HttpClient Timeout Configuration ```java
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.impl.conn.PoolingHttpClientConnectionManager;
import org.apache.http.client.config.RequestConfig;
import org.apache.http.util.EntityUtils;public class ApacheHttpClientTimeoutExample {
    
    public static CloseableHttpClient createHttpClientWithTimeouts() {
        // Connection pool manager
        PoolingHttpClientConnectionManager connectionManager = new PoolingHttpClientConnectionManager();
        connectionManager.setMaxTotal(200);        connectionManager.setDefaultMaxPerRoute(20);
        
        // Timeout configuration
        RequestConfig requestConfig = RequestConfig.custom()            .setConnectTimeout(5000)           // Connection timeout            .setSocketTimeout(10000)         // Read timeout            .setConnectionRequestTimeout(5000)  // Connection request timeout            .setCircularRedirectsAllowed(false)
            .setMaxRedirects(5)
            .build();
        
        return HttpClients.custom()
            .setConnectionManager(connectionManager)
            .setDefaultRequestConfig(requestConfig)
            .setRetryHandler((exception, executionCount) -> {
                if (executionCount > 3) {
                    return false;  // Don't retry more than 3 times
                }
                return exception instanceof org.apache.http.NoHttpResponseException;
            })
            .build();
    }        public static void main(String[] args) throws Exception {
        try (CloseableHttpClient client = createHttpClientWithTimeouts()) {
            HttpGet request = new HttpGet("http://api.example.com/data");
            
            try (CloseableHttpResponse response = client.execute(request)) {                String body = EntityUtils.toString(response.getEntity());
                System.out.println("Response: " + body);
            }        }
    }
}
```  ### OkHttp Timeout Configuration ```java
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import okhttp3.TimeoutException;
import java.util.concurrent.TimeUnit;public class OkHttpTimeoutExample {
    
    private final OkHttpClient httpClient;    
    public OkHttpTimeoutExample() {
        this.httpClient = new OkHttpClient.Builder()
            .connectTimeout(5, TimeUnit.SECONDS)     // Connection timeout            .readTimeout(10, TimeUnit.SECONDS)      // Read timeout            .writeTimeout(10, TimeUnit.SECONDS)     // Write timeout            .pingInterval(30, TimeUnit.SECONDS)     // Keep-alive ping            .retryOnConnectionFailure(true)        // Enable automatic retries
            .addInterceptor(chain -> {                Request original = chain.request();                Request request = original.newBuilder()
                    .header("X-Request-Timeout", "true")
                    .method(original.method(), original.body())
                    .build();                return chain.proceed(request);            })
            .build();
    }        public String fetchData(String url) {
        Request request = new Request.Builder()
            .url(url)
            .get()
            .build();
        
        try (Response response = httpClient.newCall(request).execute()) {            if (!response.isSuccessful()) {
                throw new RuntimeException("Unexpected response: " + response);            }
            return response.body().string();
        } catch (TimeoutException e) {            throw new ServiceTimeoutException("Request timed out: " + e.getMessage());
        } catch (Exception e) {            throw new RuntimeException("Request failed: " + e.getMessage());        }
    }
}  ```  ### Spring RestTemplate Timeout Configuration ```java
import org.springframework.boot.web.client.RestTemplateBuilder;import org.springframework.context.annotation.Bean;import org.springframework.context.annotation.Configuration;
import org.springframework.web.client.RestTemplate;import java.time.Duration;@Configuration
public class RestTemplateConfig {
    
    @Bean
    public RestTemplate restTemplate(RestTemplateBuilder builder) {        return builder
            .setConnectTimeout(Duration.ofSeconds(5))       // Connection timeout            .setReadTimeout(Duration.ofSeconds(10))         // Read timeout            .errorHandler(new CustomResponseErrorHandler())
            .build();    }
}
```  In a service class: ```java
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;import org.springframework.web.client.RestTemplate;@Service
public class ProductService {
    
    @Autowired    private final RestTemplate restTemplate;
    
    public Product getProduct(String productId) {        try {
            return restTemplate.getForObject(
                "http://product-service/api/products/{id}",                Product.class,
                productId            );
        } catch (org.springframework.web.client.ResourceAccessException e) {            // Handle timeout/connection errors
            return getFallbackProduct(productId);
        }    }
    
    private Product getFallbackProduct(String productId) {        // Return cached or default product
        return new Product(productId, "Unknown Product", 0.0);    }
}  ```  ### Resilience4j Timeout Implementation ```java
import io.github.resilience4j.circuitbreaker.CircuitBreaker;import io.github.resilience4j.circuitbreaker.CircuitBreakerConfig;
import io.github.resilience4j.circuitbreaker.CircuitBreakerOpenException;
import io.github.resilience4j.TimeoutTransformer;import io.github.resilience4j.decorators.Decorators;import java.time.Duration;import java.util.concurrent.TimeoutException;
import java.util.function.Supplier;public class Resilience4jTimeoutExample {
    
    public static void main(String[] args) {        // Configure circuit breaker with timeout
        CircuitBreakerConfig config = CircuitBreakerConfig.custom()            .failureRateThreshold(50)
            .waitDurationInOpenState(Duration.ofSeconds(30))            .permittedNumberOfCallsInHalfOpenState(3)
            .build();                CircuitBreaker circuitBreaker = CircuitBreaker.of("myService", config);
        
        Supplier<String> decoratedSupplier = Decorators.decorateSupplier(            circuitBreaker, () -> callServiceWithTimeout());        
        try {            String result = decoratedSupplier.get();            System.out.println("Result: " + result);
        } catch (CircuitBreakerOpenException e) {            System.out.println("Circuit breaker is open!");
        } catch (Exception e) {            System.out.println("Error: " + e.getMessage());        }
    }        private static String callServiceWithTimeout() throws Exception {        // Simulate a slow service call        Thread.sleep(5000);        return "Service response";    }
}  ```  ### JDBC Timeout Configuration ```java
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.SQLException;public class JdbcTimeoutExample {
    
    private static final String JDBC_URL = "jdbc:mysql://localhost:3306/mydb";    private static final String USERNAME = "root";    private static final String PASSWORD = "password";
    
    public static void main(String[] args) {        try (Connection conn = DriverManager.getConnection(JDBC_URL, USERNAME, PASSWORD)) {
            // Set query timeout            int queryTimeoutSeconds = 10;                        String sql = "SELECT * FROM large_table WHERE status = ?";            try (PreparedStatement stmt = conn.prepareStatement(sql)) {                stmt.setString(1, "active");                stmt.setQueryTimeout(queryTimeoutSeconds);                var resultSet = stmt.executeQuery();                                while (resultSet.next()) {                    // Process results                    System.out.println(resultSet.getString("name"));                }
            }        } catch (SQLException e) {            if (e.getErrorCode() == 0) {                System.out.println("Query timeout occurred");            } else {                System.out.println("Database error: " + e.getMessage());            }        }    }
}  ```  ### Setting Timeouts at Different Levels  ```java
public class MultiLevelTimeoutExample {        // 1. Application-level timeout (overall operation)
    private static final Duration OPERATION_TIMEOUT = Duration.ofSeconds(30);
    
    // 2. Circuit breaker timeout
    private static final Duration CB_TIMEOUT = Duration.ofSeconds(20);
    
    // 3. Connection timeout
    private static final Duration CONNECT_TIMEOUT = Duration.ofSeconds(5);
    
    // 4. Read timeout    private static final Duration READ_TIMEOUT = Duration.ofSeconds(10);
    
    public String executeWithMultiLevelTimeout() {        ExecutorService executor = Executors.newSingleThreadExecutor();        
        Future<String> future = executor.submit(() -> {            return performServiceCall();        });                try {            // Operation-level timeout            return future.get(OPERATION_TIMEOUT.getSeconds(), TimeUnit.SECONDS);        } catch (TimeoutException e) {            future.cancel(true);  // Cancel the running task            return getFallbackResponse();        } catch (Exception e) {            return getFallbackResponse();        } finally {            executor.shutdown();        }    }        private String performServiceCall() {        // Actual service call implementation            try {            Thread.sleep(25000);  // Simulate slow call        } catch (InterruptedException e) {            Thread.currentThread().interrupt();        }        return "Response";    }        private String getFallbackResponse() {        return "{\"status\":\"TIMEOUT_FALLBACK\"}";    }
}  ```  ## Real-World Examples  ### Netflix Ribbon (Now in Maintenance) ```java
@FeignClient(name = "product-service", configuration = FeignConfig.class)public interface ProductClient {    @RequestMapping(method = RequestMethod.GET, value = "/products/{id}")
    Product getProduct(@PathVariable("id") String id);}@Configurationclass FeignConfig {    @Bean    public Options feignOptions() {        return new Options(            5000,    // Connect timeout            10000    // Read timeout        );    }}  ```  ### Spring Cloud Gateway Timeout Configuration  ```yaml
spring:  cloud:    gateway:      routes:        - id: product-service          uri: http://localhost:8081          predicates:            - Path=/api/products/**          metadata:            response-timeout: 10000            connect-timeout: 5000      httpclient:        connect-timeout: 5000        response-timeout: 10000        pool:          type: elastic          max-connections: 200          acquire-timeout: 30000```  ### Database Connection Pool Timeouts  ```javapublic class DataSourceConfig {        @Bean    public DataSource dataSource() {        HikariConfig config = new HikariConfig();        config.setJdbcUrl("jdbc:mysql://localhost:3306/mydb");        config.setUsername("user");        config.setPassword("password");        config.setMaximumPoolSize(20);        config.setMinimumIdle(5);        config.setConnectionTimeout(30000);    // Max wait for connection        config.setIdleTimeout(600000);         // Max idle time        config.setMaxLifetime(1800000);       // Max connection lifetime        config.setConnectionTestQuery("SELECT 1");        return new HikariDataSource(config);    }}```  ### Kubernetes Service Timeout Configuration  ```yaml
apiVersion: v1kind: Servicemetadata:  name: my-servicespec:  selector:    app: my-app  ports:    - protocol: TCP      port: 80      targetPort: 8080  sessionAffinity: ClientIP  sessionAffinityConfig:    clientIP:      timeoutSeconds: 10800  ```  ## Output Statement  Timeouts are essential for building resilient distributed systems. They prevent applications from hanging indefinitely, enable fast failure detection, and allow systems to recover gracefully. Key benefits include:  - **Resource Protection**: Preventing thread pool exhaustion and resource consumption - **Fast Failure Detection**: Quickly identifying failing services - **Improved User Experience**: Responding to users within expected timeframes - **System Stability**: Maintaining stability under adverse conditions  Proper timeout configuration requires understanding your services' typical response times, network latency, and the criticality of each operation. Always implement fallback mechanisms to handle timeout scenarios gracefully.  ## Best Practices 1. **Set Appropriate Timeouts**: Configure timeouts based on actual service performance data, not arbitrary values. 2. **Use Layered Timeouts**: Apply timeouts at multiple levels (connection, read, overall operation) for defense in depth.  3. **Implement Fallbacks**: Always have fallback mechanisms when timeouts occur. 4. **Monitor Timeout Events**: Track timeout occurrences to identify problematic services. 5. **Test Timeout Behavior**: Regularly test how your system behaves under timeout conditions. 6. **Use Circuit Breakers**: Combine timeouts with circuit breakers for comprehensive resilience. 7. **Configure Per-Service**: Set different timeouts for different services based on their characteristics. 8. **Set Reasonable Defaults**: Use conservative default timeouts that can be overridden for specific cases. 9. **Handle Interrupted Exceptions**: Properly handle thread interruption when timeouts occur. 10. **Log Timeout Context**: Include sufficient context in timeout logs for debugging. 
