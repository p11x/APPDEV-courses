# DataStore Implementation

## Learning Objectives

1. Using DataStore for preferences
2. Proto DataStore for typed data
3. Migration from SharedPreferences

```kotlin
package com.android.data.datastore

object DataStoreImplementation {
    
    // Preferences DataStore
    class PreferencesDataStore(private val context: android.content.Context) {
        
        private val Context.dataStore by androidx.datastore.preferences.core.stringPreferencesDataStore("settings")
        
        suspend fun saveSetting(key: String, value: String) {
            context.dataStore.edit { prefs ->
                prefs[androidx.datastore.preferences.core.stringPreferencesKey(key)] = value
            }
        }
        
        suspend fun getSetting(key: String): String? {
            return context.dataStore.data.map { prefs ->
                prefs[androidx.datastore.preferences.core.stringPreferencesKey(key)]
            }.firstOrNull()
        }
    }
    
    // Proto DataStore
    /*
    @androidx.datastore.core.DataStore
    abstract class SettingsDataStore : androidx.datastore.core.Serializer<Settings> {
        override val defaultValue: Settings = Settings.getDefaultInstance()
        
        override suspend fun readFrom(input: java.io.InputStream): Settings {
            return Settings.parseFrom(input)
        }
        
        override suspend fun writeTo(t: Settings, output: java.io.OutputStream) {
            t.writeTo(output)
        }
    }
    
    message Settings {
        string theme = 1;
        bool notifications_enabled = 2;
    }
    */
}
```