# SQLite Implementation

## Section 2: SQLite Implementation

SQLite - Direct SQLite usage (legacy but useful to know)

```kotlin
package com.android.data.sqlite

object SQLiteImplementation {
    
    // SQLiteOpenHelper
    class MyDatabaseHelper(
        context: android.content.Context
    ) : android.database.sqlite.SQLiteOpenHelper(
        context,
        "mydb.db",
        null,
        1
    ) {
        override fun onCreate(db: android.database.sqlite.SQLiteDatabase) {
            db.execSQL("CREATE TABLE users (id INTEGER PRIMARY KEY, name TEXT, email TEXT)")
        }
        
        override fun onUpgrade(db: android.database.sqlite.SQLiteDatabase, oldVersion: Int, newVersion: Int) {
            db.execSQL("DROP TABLE IF EXISTS users")
            onCreate(db)
        }
    }
    
    // CRUD Operations
    class UserDao(private val dbHelper: MyDatabaseHelper) {
        
        fun insert(name: String, email: String): Long {
            val db = dbHelper.writableDatabase
            val values = android.content.ContentValues().apply {
                put("name", name)
                put("email", email)
            }
            return db.insert("users", null, values)
        }
        
        fun getAll(): List<User> {
            val db = dbHelper.readableDatabase
            val cursor = db.query("users", null, null, null, null, null, null)
            val users = mutableListOf<User>()
            while (cursor.moveToNext()) {
                users.add(User(cursor.getLong(0), cursor.getString(1), cursor.getString(2)))
            }
            cursor.close()
            return users
        }
        
        data class User(val id: Long, val name: String, val email: String)
    }
}
```

## Section 3: Type Converters

```kotlin
package com.android.data.converters

object TypeConverters {
    
    // Custom TypeConverter
    class Converters {
        
        @androidx.room.TypeConverter
        fun fromTimestamp(value: Long?): java.util.Date? {
            return value?.let { java.util.Date(it) }
        }
        
        @androidx.room.TypeConverter
        fun dateToTimestamp(date: java.util.Date?): Long? {
            return date?.time
        }
        
        @androidx.room.TypeConverter
        fun fromStringList(value: String): List<String> {
            return value.split(",").map { it.trim() }
        }
        
        @androidx.room.TypeConverter
        fun toStringList(list: List<String>): String {
            return list.joinToString(",")
        }
    }
    
    // Usage in Entity
    /*
    @androidx.room.Entity
    data class Event(
        val date: java.util.Date,
        val tags: List<String>
    )
    */
}
```

## Section 4: Database Migrations

```kotlin
package com.android.data.migrations

object DatabaseMigrations {
    
    // Migration example
    val MIGRATION_1_2 = object : androidx.room.migration.Migration(1, 2) {
        override fun migrate(database: androidx.database.sqlite.SQLiteDatabase) {
            database.execSQL("ALTER TABLE users ADD COLUMN phone TEXT")
        }
    }
    
    // Add more migrations
    val MIGRATION_2_3 = object : androidx.room.migration.Migration(2, 3) {
        override fun migrate(database: androidx.database.sqlite.SQLiteDatabase) {
            database.execSQL("CREATE TABLE posts (id INTEGER PRIMARY KEY, user_id INTEGER, title TEXT)")
        }
    }
    
    // Fallback migration
    /*
    val db = Room.databaseBuilder(context, AppDatabase::class.java, "db")
        .addMigrations(MIGRATION_1_2, MIGRATION_2_3)
        .build()
    */
}
```

## Section 5: Database Testing

```kotlin
package com.android.data.testing

object DatabaseTesting {
    
    // Test with in-memory database
    /*
    @Test
    fun testDao() {
        val db = Room.inMemoryDatabaseBuilder(
            ApplicationProvider.getApplicationContext(),
            AppDatabase::class.java
        ).build()
        
        val dao = db.userDao()
        dao.insertUser(User(name = "Test", email = "test@test.com"))
        
        val users = dao.getAllUsers()
        assert(users.size == 1)
    }
    */
}
```