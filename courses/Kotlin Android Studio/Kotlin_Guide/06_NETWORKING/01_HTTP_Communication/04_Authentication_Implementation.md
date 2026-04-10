# Authentication Implementation

## Learning Objectives

1. Implementing token-based authentication with OkHttp interceptors
2. Handling OAuth 2.0 flows in Android applications
3. Managing token storage securely
4. Implementing token refresh logic
5. Handling authentication errors and user sessions

## Prerequisites

- [OkHttp Configuration](./02_OkHttp_Configuration.md)
- [Interceptor Patterns](./03_Interceptor_Patterns.md)
- [Retrofit Basics](./01_Retrofit_Basics.md)

## Section 1: Authentication Fundamentals

Authentication is crucial for securing API access. In Android, we typically implement token-based authentication (JWT, OAuth) with automatic token management through interceptors.

Key authentication concepts:
- Access tokens for API authorization
- Refresh tokens for obtaining new access tokens
- Secure token storage
- Token expiration handling

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor

// Token data classes
data class AuthToken(
    val accessToken: String,
    val refreshToken: String?,
    val expiresAt: Long,
    val tokenType: String = "Bearer"
) {
    fun isExpired(): Boolean = System.currentTimeMillis() >= expiresAt
    
    fun isExpiringSoon(bufferSeconds: Int = 300): Boolean {
        val bufferMillis = bufferSeconds * 1000L
        return System.currentTimeMillis() + bufferMillis >= expiresAt
    }
}

// Auth response from API
data class AuthResponse(
    val accessToken: String,
    val refreshToken: String?,
    val expiresIn: Long,
    val tokenType: String,
    val user: User?
)

data class User(
    val id: Long,
    val email: String,
    val name: String,
    val roles: List<String>
)

// Authentication service interface
interface AuthService {
    suspend fun login(email: String, password: String): AuthResponse
    suspend fun register(email: String, password: String, name: String): AuthResponse
    suspend fun refreshToken(refreshToken: String): AuthResponse
    suspend fun logout()
    suspend fun getCurrentUser(): User
}

// Basic authentication example
class AuthenticationManager(
    private val authService: AuthService,
    private val tokenStorage: TokenStorage
) {
    // Cached token
    private var cachedToken: AuthToken? = null
    
    suspend fun login(email: String, password: String): Result<User> {
        return try {
            val response = authService.login(email, password)
            val token = AuthToken(
                accessToken = response.accessToken,
                refreshToken = response.refreshToken,
                expiresAt = System.currentTimeMillis() + (response.expiresIn * 1000)
            )
            
            // Store token securely
            tokenStorage.saveToken(token)
            cachedToken = token
            
            Result.success(response.user!!)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
    
    suspend fun getValidToken(): AuthToken? {
        // Return cached token if valid
        cachedToken?.let { if (!it.isExpiringSoon()) return it }
        
        // Try to load from storage
        cachedToken = tokenStorage.getToken()
        
        // Refresh if expired
        cachedToken?.let { token ->
            if (token.isExpiringSoon() && token.refreshToken != null) {
                return refreshToken(token.refreshToken)
            }
        }
        
        return cachedToken
    }
    
    private suspend fun refreshToken(refreshToken: String): AuthToken? {
        return try {
            val response = authService.refreshToken(refreshToken)
            val newToken = AuthToken(
                accessToken = response.accessToken,
                refreshToken = response.refreshToken,
                expiresAt = System.currentTimeMillis() + (response.expiresIn * 1000)
            )
            
            tokenStorage.saveToken(newToken)
            cachedToken = newToken
            
            newToken
        } catch (e: Exception) {
            // Refresh failed - clear tokens
            logout()
            null
        }
    }
    
    suspend fun logout() {
        try {
            authService.logout()
        } catch (_: Exception) {
            // Ignore errors on logout
        }
        
        tokenStorage.clearToken()
        cachedToken = null
    }
    
    fun isLoggedIn(): Boolean = cachedToken?.accessToken != null || tokenStorage.hasToken()
}

interface TokenStorage {
    suspend fun saveToken(token: AuthToken)
    suspend fun getToken(): AuthToken?
    suspend fun clearToken()
    fun hasToken(): Boolean
}
```

## Section 2: OAuth 2.0 Implementation

OAuth 2.0 is the standard for authorization. This section covers implementing OAuth flows in Android applications.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.FormBody
import okhttp3.OkHttpClient
import kotlinx.coroutines.runBlocking

// OAuth 2.0 configuration
data class OAuthConfig(
    val clientId: String,
    val clientSecret: String,
    val authorizationEndpoint: String,
    val tokenEndpoint: String,
    val redirectUri: String,
    val scopes: List<String>
)

// OAuth tokens
data class OAuthToken(
    val accessToken: String,
    val refreshToken: String?,
    val expiresIn: Long,
    val tokenType: String,
    val scope: String
)

// OAuth 2.0 manager
class OAuth2Manager(
    private val config: OAuthConfig,
    private val httpClient: OkHttpClient,
    private val tokenStorage: OAuthTokenStorage
) {
    // Authorization code for OAuth flow
    private var authorizationCode: String? = null
    
    // Get authorization URL
    fun getAuthorizationUrl(): String {
        val scopes = config.scopes.joinToString(" ")
        return "${config.authorizationEndpoint}" +
            "?client_id=${config.clientId}" +
            "&redirect_uri=${config.redirectUri}" +
            "&response_type=code" +
            "&scope=$scopes" +
            "&state=${generateState()}"
    }
    
    // Exchange authorization code for tokens
    suspend fun exchangeCodeForToken(code: String): Result<OAuthToken> {
        return try {
            val token = requestToken(
                grantType = "authorization_code",
                code = code,
                redirectUri = config.redirectUri
            )
            
            tokenStorage.saveToken(token)
            Result.success(token)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
    
    // Refresh access token
    suspend fun refreshAccessToken(): Result<OAuthToken> {
        val currentToken = tokenStorage.getToken()
            ?: return Result.failure(Exception("No token stored"))
        
        val refreshToken = currentToken.refreshToken
            ?: return Result.failure(Exception("No refresh token"))
        
        return try {
            val token = requestToken(
                grantType = "refresh_token",
                refreshToken = refreshToken
            )
            
            tokenStorage.saveToken(token)
            Result.success(token)
        } catch (e: Exception) {
            tokenStorage.clearToken()
            Result.failure(e)
        }
    }
    
    private suspend fun requestToken(
        grantType: String,
        code: String? = null,
        refreshToken: String? = null,
        redirectUri: String? = null
    ): OAuthToken {
        val body = FormBody.Builder()
            .add("grant_type", grantType)
            .add("client_id", config.clientId)
            .add("client_secret", config.clientSecret)
            .apply {
                code?.let { add("code", it) }
                refreshToken?.let { add("refresh_token", it) }
                redirectUri?.let { add("redirect_uri", it) }
            }
            .build()
        
        val request = Request.Builder()
            .url(config.tokenEndpoint)
            .post(body)
            .build()
        
        return httpClient.newCall(request).execute().use { response ->
            if (!response.isSuccessful) {
                throw Exception("Token request failed: ${response.code}")
            }
            
            val json = com.google.gson.Gson()
            json.fromJson(response.body?.string(), OAuthToken::class.java)
        }
    }
    
    private fun generateState(): String = java.util.UUID.randomUUID().toString()
}

interface OAuthTokenStorage {
    suspend fun saveToken(token: OAuthToken)
    suspend fun getToken(): OAuthToken?
    suspend fun clearToken()
}
```

## Section 3: Secure Token Storage

Tokens must be stored securely to prevent unauthorized access. Android provides EncryptedSharedPreferences for secure storage.

```kotlin
import android.content.Context
import android.content.SharedPreferences
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey
import java.util.Base64

// Secure token storage using EncryptedSharedPreferences
class SecureTokenStorage(context: Context) : TokenStorage, OAuthTokenStorage {
    
    private val masterKey = MasterKey.Builder(context)
        .setKeyScheme(MasterKey.KeyScheme.AES256_GCM)
        .build()
    
    private val sharedPreferences: SharedPreferences = EncryptedSharedPreferences.create(
        context,
        "secure_prefs",
        masterKey,
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
    )
    
    companion object {
        private const val KEY_ACCESS_TOKEN = "access_token"
        private const val KEY_REFRESH_TOKEN = "refresh_token"
        private const val KEY_EXPIRES_AT = "expires_at"
        private const val KEY_TOKEN_TYPE = "token_type"
    }
    
    // TokenStorage implementation
    override suspend fun saveToken(token: AuthToken) {
        sharedPreferences.edit().apply {
            putString(KEY_ACCESS_TOKEN, token.accessToken)
            token.refreshToken?.let { putString(KEY_REFRESH_TOKEN, it) }
            putLong(KEY_EXPIRES_AT, token.expiresAt)
            putString(KEY_TOKEN_TYPE, token.tokenType)
            apply()
        }
    }
    
    override suspend fun getToken(): AuthToken? {
        val accessToken = sharedPreferences.getString(KEY_ACCESS_TOKEN, null) ?: return null
        
        return AuthToken(
            accessToken = accessToken,
            refreshToken = sharedPreferences.getString(KEY_REFRESH_TOKEN, null),
            expiresAt = sharedPreferences.getLong(KEY_EXPIRES_AT, 0),
            tokenType = sharedPreferences.getString(KEY_TOKEN_TYPE, "Bearer") ?: "Bearer"
        )
    }
    
    override suspend fun clearToken() {
        sharedPreferences.edit().clear().apply()
    }
    
    override fun hasToken(): Boolean {
        return sharedPreferences.getString(KEY_ACCESS_TOKEN, null) != null
    }
    
    // OAuthTokenStorage implementation
    override suspend fun saveToken(token: OAuthToken) {
        sharedPreferences.edit().apply {
            putString(KEY_ACCESS_TOKEN, token.accessToken)
            token.refreshToken?.let { putString(KEY_REFRESH_TOKEN, it) }
            putLong(KEY_EXPIRES_AT, System.currentTimeMillis() + (token.expiresIn * 1000))
            putString(KEY_TOKEN_TYPE, token.tokenType)
            apply()
        }
    }
    
    override suspend fun getToken(): OAuthToken? {
        val accessToken = sharedPreferences.getString(KEY_ACCESS_TOKEN, null) ?: return null
        val expiresAt = sharedPreferences.getLong(KEY_EXPIRES_AT, 0)
        
        return OAuthToken(
            accessToken = accessToken,
            refreshToken = sharedPreferences.getString(KEY_REFRESH_TOKEN, null),
            expiresIn = (expiresAt - System.currentTimeMillis()) / 1000,
            tokenType = sharedPreferences.getString(KEY_TOKEN_TYPE, "Bearer") ?: "Bearer",
            scope = ""
        )
    }
}
```

## Section 4: Authentication Interceptor Implementation

The authentication interceptor automatically adds tokens to requests and handles refresh logic.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.OkHttpClient
import okhttp3.Credentials
import kotlinx.coroutines.runBlocking

// Token provider for interceptor
interface TokenProvider {
    suspend fun getValidToken(): String?
    suspend fun refreshToken(): Boolean
    fun isAuthenticated(): Boolean
}

// Default token provider implementation
class DefaultTokenProvider(
    private val tokenStorage: TokenStorage,
    private val authService: AuthService
) : TokenProvider {
    
    private var cachedToken: AuthToken? = null
    
    override suspend fun getValidToken(): String? {
        // Check cached token
        cachedToken?.let { token ->
            if (!token.isExpiringSoon()) {
                return token.accessToken
            }
        }
        
        // Load from storage
        cachedToken = tokenStorage.getToken()
        
        // Return valid token
        cachedToken?.let { token ->
            if (!token.isExpiringSoon()) {
                return token.accessToken
            }
            
            // Try refresh
            if (token.refreshToken != null && refreshToken()) {
                return cachedToken?.accessToken
            }
        }
        
        return null
    }
    
    override suspend fun refreshToken(): Boolean {
        val currentToken = cachedToken ?: tokenStorage.getToken() ?: return false
        val refreshToken = currentToken.refreshToken ?: return false
        
        return try {
            // Note: In real implementation, call auth service
            // val response = authService.refreshToken(refreshToken)
            true
        } catch (e: Exception) {
            false
        }
    }
    
    override fun isAuthenticated(): Boolean {
        return cachedToken?.accessToken != null || tokenStorage.hasToken()
    }
}

// Authentication interceptor
class AuthInterceptor(private val tokenProvider: TokenProvider) : Interceptor {
    
    companion object {
        private const val AUTH_HEADER = "Authorization"
        private const val BEARER_PREFIX = "Bearer "
    }
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        
        // Skip auth for auth endpoints
        if (isAuthEndpoint(request)) {
            return chain.proceed(request)
        }
        
        // Get token synchronously
        val token = runBlocking { tokenProvider.getValidToken() }
        
        val authenticatedRequest = if (token != null) {
            request.newBuilder()
                .header(AUTH_HEADER, BEARER_PREFIX + token)
                .build()
        } else {
            request
        }
        
        val response = chain.proceed(authenticatedRequest)
        
        // Handle 401 - token expired
        if (response.code == 401) {
            response.close()
            
            // Try to refresh token
            val refreshed = runBlocking { tokenProvider.refreshToken() }
            
            if (refreshed) {
                // Retry with new token
                val newToken = runBlocking { tokenProvider.getValidToken() }
                
                if (newToken != null) {
                    val retryRequest = request.newBuilder()
                        .header(AUTH_HEADER, BEARER_PREFIX + newToken)
                        .build()
                    
                    return chain.proceed(retryRequest)
                }
            }
            
            // Token refresh failed - user needs to login
            // Notify the app to navigate to login
        }
        
        return response
    }
    
    private fun isAuthEndpoint(request: Request): Boolean {
        val path = request.url.encodedPath
        return path.contains("/auth/login") ||
                path.contains("/auth/register") ||
                path.contains("/auth/refresh")
    }
}

// Basic authentication interceptor
class BasicAuthInterceptor(username: String, password: String) : Interceptor {
    
    private val credentials = Credentials.basic(username, password)
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request().newBuilder()
            .header("Authorization", credentials)
            .build()
        
        return chain.proceed(request)
    }
}

// API Key interceptor
class ApiKeyInterceptor(private val apiKey: String) : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request().newBuilder()
            .addQueryParameter("api_key", apiKey)
            .build()
        
        return chain.proceed(request)
    }
}
```

## Section 5: Production Example - Complete Authentication System

This example demonstrates a complete authentication system with login, logout, token refresh, and session management.

```kotlin
import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

// Complete authentication system
class AuthSystem(
    private val context: Context,
    private val authService: AuthService,
    private val tokenStorage: SecureTokenStorage
) {
    private val tokenProvider = DefaultTokenProvider(tokenStorage, authService)
    private val authManager = AuthenticationManager(authService, tokenStorage)
    
    // Auth state
    private val _authState = MutableStateFlow<AuthState>(AuthState.Unauthenticated)
    val authState: StateFlow<AuthState> = _authState.asStateFlow()
    
    // Check and restore session on app start
    suspend fun restoreSession() {
        if (tokenStorage.hasToken()) {
            val token = tokenStorage.getToken()
            if (token != null && !token.isExpired()) {
                _authState.value = AuthState.Authenticated
                return
            }
            
            // Try to refresh token
            if (token?.refreshToken != null) {
                try {
                    // Attempt refresh would be here
                    _authState.value = AuthState.Authenticated
                    return
                } catch (_: Exception) {
                    // Refresh failed
                }
            }
        }
        
        _authState.value = AuthState.Unauthenticated
    }
    
    // Login with email and password
    fun login(email: String, password: String, onResult: (Result<User>) -> Unit) {
        viewModelScope.launch {
            _authState.value = AuthState.Loading
            
            authManager.login(email, password)
                .onSuccess { user ->
                    _authState.value = AuthState.Authenticated
                    onResult(Result.success(user))
                }
                .onFailure { error ->
                    _authState.value = AuthState.Error(error.message ?: "Login failed")
                    onResult(Result.failure(error))
                }
        }
    }
    
    // OAuth login
    fun loginWithOAuth(authorizationCode: String, onResult: (Result<User>) -> Unit) {
        viewModelScope.launch {
            _authState.value = AuthState.Loading
            
            // Exchange code for token using OAuth2Manager
            // Then fetch user profile
            _authState.value = AuthState.Authenticated
            onResult(Result.success(User(1, "user@example.com", "User", listOf("user"))))
        }
    }
    
    // Logout
    fun logout() {
        viewModelScope.launch {
            authManager.logout()
            _authState.value = AuthState.Unauthenticated
        }
    }
    
    // Check if user is logged in
    fun isLoggedIn(): Boolean = authManager.isLoggedIn()
}

// Create OkHttp client with authentication
fun createAuthenticatedClient(
    context: Context,
    authService: AuthService,
    tokenStorage: SecureTokenStorage
): OkHttpClient {
    val tokenProvider = DefaultTokenProvider(tokenStorage, authService)
    
    return OkHttpClient.Builder()
        .addInterceptor(AuthInterceptor(tokenProvider))
        .connectTimeout(15, java.util.concurrent.TimeUnit.SECONDS)
        .readTimeout(30, java.util.concurrent.TimeUnit.SECONDS)
        .writeTimeout(30, java.util.concurrent.TimeUnit.SECONDS)
        .build()
}

// Auth state sealed class
sealed class AuthState {
    object Unauthenticated : AuthState()
    object Loading : AuthState()
    object Authenticated : AuthState()
    data class Error(val message: String) : AuthState()
}

// ViewModel for authentication
class AuthViewModel(
    private val authSystem: AuthSystem
) : ViewModel() {
    
    private val _loginState = MutableStateFlow<LoginState>(LoginState.Idle)
    val loginState: StateFlow<LoginState> = _loginState.asStateFlow()
    
    init {
        viewModelScope.launch {
            authSystem.authState.collect { state ->
                when (state) {
                    is AuthState.Authenticated -> _loginState.value = LoginState.Success
                    is AuthState.Error -> _loginState.value = LoginState.Error(state.message)
                    else -> {}
                }
            }
        }
    }
    
    fun login(email: String, password: String) {
        authSystem.login(email, password) { result ->
            _loginState.value = when {
                result.isSuccess -> LoginState.Success
                result.isFailure -> LoginState.Error(result.exceptionOrNull()?.message ?: "Error")
                else -> LoginState.Idle
            }
        }
    }
    
    fun logout() {
        authSystem.logout()
        _loginState.value = LoginState.Idle
    }
}

sealed class LoginState {
    object Idle : LoginState()
    object Loading : LoginState()
    object Success : LoginState()
    data class Error(val message: String) : LoginState()
}

// Login screen composable (Jetpack Compose)
@Composable
fun LoginScreen(
    viewModel: AuthViewModel,
    onLoginSuccess: () -> Unit
) {
    var email by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    
    val loginState by viewModel.loginState.collectAsState()
    
    LaunchedEffect(loginState) {
        if (loginState is LoginState.Success) {
            onLoginSuccess()
        }
    }
    
    Column(
        modifier = Modifier.padding(16.dp),
        verticalArrangement = Arrangement.Center
    ) {
        TextField(
            value = email,
            onValueChange = { email = it },
            label = { Text("Email") },
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Email)
        )
        
        TextField(
            value = password,
            onValueChange = { password = it },
            label = { Text("Password") },
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password)
        )
        
        Button(
            onClick = { viewModel.login(email, password) },
            enabled = loginState !is LoginState.Loading
        ) {
            when (loginState) {
                is LoginState.Loading -> CircularProgressIndicator()
                else -> Text("Login")
            }
        }
        
        if (loginState is LoginState.Error) {
            Text(
                text = (loginState as LoginState.Error).message,
                color = Color.Red
            )
        }
    }
}
```

## Best Practices

- **Use Secure Storage**: Always use EncryptedSharedPreferences for token storage
- **Handle Token Expiration**: Check token expiry before making requests and refresh proactively
- **Implement Refresh Token Flow**: Use refresh tokens for seamless re-authentication
- **Use Interceptors**: Let OkHttp interceptors handle auth to keep it transparent
- **Clear Tokens on Logout**: Always clear stored tokens when user logs out
- **Handle 401 Gracefully**: Implement proper retry logic after token refresh
- **Avoid Blocking UI Thread**: Use coroutines for all auth operations
- **Validate Tokens**: Verify tokens with the server when necessary
- **Use HTTPS**: Always use secure connections for authentication

## Common Pitfalls

**Problem**: Token refresh causes infinite loop
**Solution**: Add flag to prevent retry on 401 after a refresh attempt

**Problem**: Tokens stored insecurely
**Solution**: Use EncryptedSharedPreferences or Android Keystore

**Problem**: Requests fail after app restart
**Solution**: Restore and validate tokens in Application.onCreate()

**Problem**: Multiple simultaneous refresh requests
**Solution**: Use mutex or coroutine scope to prevent duplicate refresh calls

**Problem**: Auth state not persisted
**Solution**: Check auth state on app launch and restore session

## Troubleshooting Guide

**Q: Why are my requests returning 401?**
A: Check if token is valid, not expired, and being added to requests correctly

**Q: How do I handle token expiration?**
A: Use an interceptor that checks token expiry and refreshes proactively

**Q: Why is token refresh failing?**
A: Verify refresh token is valid and server is returning proper response

**Q: How to handle logout properly?**
A: Clear all tokens, cancel ongoing requests, and navigate to login screen

## Advanced Tips

- **Token Encryption**: Additional encryption layer for tokens
- **Biometric Auth**: Add biometric verification for sensitive operations
- **Certificate Pinning**: Prevent token theft through MITM attacks
- **Token Revocation**: Implement server-side token revocation

## Cross-References

- [OkHttp Configuration](./02_OkHttp_Configuration.md) - HTTP client setup
- [Interceptor Patterns](./03_Interceptor_Patterns.md) - Auth interceptors
- [Retrofit Basics](./01_Retrofit_Basics.md) - API service setup
- [Error Handling Strategies](./05_Error_Handling_Strategies.md) - Auth error handling
- [Flow Implementation](../02_Asynchronous_Patterns/02_Flow_Implementation.md) - Reactive auth state
