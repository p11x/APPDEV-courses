# SharedPreferences

## Learning Objectives

1. Using SharedPreferences for simple data
2. Reading and writing preferences
3. Managing preferences with Wrapper classes

```kotlin
package com.android.data.prefs

object SharedPreferences {
    
    // Direct usage
    class PreferenceManager(context: android.content.Context) {
        
        private val prefs = context.getSharedPreferences("my_prefs", android.content.Context.MODE_PRIVATE)
        
        var userName: String
            get() = prefs.getString("user_name", "") ?: ""
            set(value) = prefs.edit().putString("user_name", value).apply()
        
        var userAge: Int
            get() = prefs.getInt("user_age", 0)
            set(value) = prefs.edit().putInt("user_age", value).apply()
        
        var isLoggedIn: Boolean
            get() = prefs.getBoolean("is_logged_in", false)
            set(value) = prefs.edit().putBoolean("is_logged_in", value).apply()
        
        fun clear() {
            prefs.edit().clear().apply()
        }
    }
    
    // Encrypted Preferences
    /*
    val encryptedPrefs = androidx.security.crypto.EncryptedSharedPreferences.create(
        context,
        "secure_prefs",
        MasterKey.Builder(context).build(),
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
    )
    */
}
```