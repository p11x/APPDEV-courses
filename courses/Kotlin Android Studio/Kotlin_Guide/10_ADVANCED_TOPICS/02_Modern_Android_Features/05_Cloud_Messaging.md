# Cloud Messaging

## Overview

Cloud messaging enables real-time communication between your server and Android devices. Firebase Cloud Messaging (FCM) is the primary solution for push notifications, delivering messages reliably even when your app is not running.

## Learning Objectives

- Implement FCM for push notifications
- Handle upstream and downstream messaging
- Manage device tokens
- Implement topic messaging
- Handle message payloads and notification customization

## Prerequisites

- [Notifications](./04_Notifications.md)
- [Coroutines Basics](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)

## Core Concepts

### FCM Architecture

FCM consists of:
- Your app: Receives messages
- FCM: Handles routing and delivery
- Your server: Sends messages via HTTP or XMPP

### Message Types

- Notification messages: Handled by FCM
- Data messages: Handled by your app
- Hybrid messages: Both notification and data

## Code Examples

### Example 1: Basic FCM Setup

```kotlin
import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.os.Build
import com.google.firebase.messaging.FirebaseMessaging
import com.google.firebase.messaging.FirebaseMessagingService
import com.google.firebase.messaging.RemoteMessage

/**
 * FCM Service for receiving messages
 */
class FCMService : FirebaseMessagingService() {
    
    override fun onNewToken(token: String) {
        super.onNewToken(token)
        println("FCM Token: $token")
        
        // Send token to server
        sendTokenToServer(token)
    }
    
    override fun onMessageReceived(message: RemoteMessage) {
        super.onMessageReceived(message)
        
        println("From: ${message.from}")
        
        // Check message type
        if (message.notification != null) {
            handleNotificationMessage(message.notification!!)
        }
        
        if (message.data.isNotEmpty()) {
            handleDataMessage(message.data)
        }
    }
    
    private fun handleNotificationMessage(notification: RemoteMessage.Notification) {
        val title = notification.title ?: "Notification"
        val body = notification.body ?: ""
        
        println("Notification: $title - $body")
        
        // Show notification
        NotificationHelper(this).showSimpleNotification(title, body)
    }
    
    private fun handleDataMessage(data: Map<String, String>) {
        val action = data["action"]
        val payload = data["payload"]
        
        println("Data action: $action, payload: $payload")
        
        when (action) {
            ACTION_UPDATE -> handleUpdate(payload)
            ACTION_SYNC -> handleSync(payload)
            ACTION_ALERT -> handleAlert(payload)
        }
    }
    
    private fun handleUpdate(payload: String?) {
        // Handle update action
    }
    
    private fun handleSync(payload: String?) {
        // Handle sync action
    }
    
    private fun handleAlert(payload: String?) {
        // Handle alert action
    }
    
    private fun sendTokenToServer(token: String) {
        // Send token to your server
        println("Token sent to server: $token")
    }
    
    companion object {
        const val ACTION_UPDATE = "update"
        const val ACTION_SYNC = "sync"
        const val ACTION_ALERT = "alert"
    }
}

/**
 * FCM topic manager
 */
class TopicManager(private val context: Context) {
    
    /**
     * Subscribe to a topic
     */
    fun subscribeToTopic(topic: String) {
        FirebaseMessaging.getInstance().subscribeToTopic(topic)
            .addOnCompleteListener { task ->
                if (task.isSuccessful) {
                    println("Subscribed to topic: $topic")
                } else {
                    println("Failed to subscribe: ${task.exception}")
                }
            }
    }
    
    /**
     * Unsubscribe from a topic
     */
    fun unsubscribeFromTopic(topic: String) {
        FirebaseMessaging.getInstance().unsubscribeFromTopic(topic)
            .addOnCompleteListener { task ->
                if (task.isSuccessful) {
                    println("Unsubscribed from topic: $topic")
                }
            }
    }
    
    /**
     * Subscribe to multiple topics
     */
    fun subscribeToTopics(topics: List<String>) {
        topics.forEach { topic ->
            FirebaseMessaging.getInstance().subscribeToTopic(topic)
        }
    }
}

/**
 * Token manager for handling FCM tokens
 */
class TokenManager(private val context: Context) {
    
    private val prefs = context.getSharedPreferences("fcm_prefs", Context.MODE_PRIVATE)
    
    /**
     * Save FCM token
     */
    fun saveToken(token: String) {
        prefs.edit().putString(KEY_FCM_TOKEN, token).apply()
    }
    
    /**
     * Get saved token
     */
    fun getToken(): String? {
        return prefs.getString(KEY_FCM_TOKEN, null)
    }
    
    /**
     * Check if token is saved
     */
    fun hasToken(): Boolean {
        return prefs.contains(KEY_FCM_TOKEN)
    }
    
    /**
     * Clear saved token
     */
    fun clearToken() {
        prefs.edit().remove(KEY_FCM_TOKEN).apply()
    }
    
    /**
     * Get token (async)
     */
    fun getTokenAsync(callback: (String?) -> Unit) {
        FirebaseMessaging.getInstance().token
            .addOnSuccessListener { token ->
                saveToken(token)
                callback(token)
            }
            .addOnFailureListener { e ->
                println("Failed to get token: ${e.message}")
                callback(null)
            }
    }
    
    companion object {
        private const val KEY_FCM_TOKEN = "fcm_token"
    }
}
```

**Output:**
```
FCM Token: dQw4w9WgXcQ...
Subscribed to topic: updates
Notification: New Message - Hello!
```

### Example 2: Sending Messages

```kotlin
import com.google.firebase.messaging.Message
import com.google.firebase.messaging.Notification
import com.google.firebase.messaging.AndroidConfig
import com.google.firebase.messaging.AndroidNotification

/**
 * Message sender for downstream messaging
 */
class MessageSender {
    
    /**
     * Send notification message
     */
    fun sendNotification(
        token: String,
        title: String,
        body: String
    ) {
        val notification = Notification.builder()
            .setTitle(title)
            .setBody(body)
            .build()
        
        val message = Message.builder()
            .setToken(token)
            .setNotification(notification)
            .build()
        
        sendMessage(message)
    }
    
    /**
     * Send data message
     */
    fun sendDataMessage(
        token: String,
        data: Map<String, String>
    ) {
        val message = Message.builder()
            .setToken(token)
            .setData(data)
            .build()
        
        sendMessage(message)
    }
    
    /**
     * Send message with notification and data
     */
    fun sendHybridMessage(
        token: String,
        title: String,
        body: String,
        data: Map<String, String>
    ) {
        val notification = Notification.builder()
            .setTitle(title)
            .setBody(body)
            .build()
        
        val androidConfig = AndroidConfig.builder()
            .setPriority(AndroidConfig.Priority.HIGH)
            .setNotification(AndroidNotification.builder()
                .setChannelId("important_channel")
                .build())
            .build()
        
        val message = Message.builder()
            .setToken(token)
            .setNotification(notification)
            .setData(data)
            .setAndroidConfig(androidConfig)
            .build()
        
        sendMessage(message)
    }
    
    private fun sendMessage(message: Message) {
        // In production, call your backend API to send message
        println("Message prepared: ${message.token}")
    }
}

/**
 * Topic message sender
 */
class TopicMessageSender {
    
    /**
     * Send to topic
     */
    fun sendToTopic(topic: String, title: String, body: String) {
        val message = Message.builder()
            .setTopic(topic)
            .setNotification(Notification.builder()
                .setTitle(title)
                .setBody(body)
                .build())
            .build()
        
        println("Topic message sent: $topic")
    }
    
    /**
     * Send to multiple topics (condition)
     */
    fun sendToCondition(
        condition: String, // e.g., "'updates' in topics && 'news' in topics"
        title: String,
        body: String
    ) {
        val message = Message.builder()
            .setCondition(condition)
            .setNotification(Notification.builder()
                .setTitle(title)
                .setBody(body)
                .build())
            .build()
        
        println("Conditional message sent: $condition")
    }
}

/**
 * FCM payload builder
 */
class PayloadBuilder {
    
    /**
     * Build notification payload
     */
    fun buildNotificationPayload(title: String, body: String): Map<String, Any> {
        return mapOf(
            "notification" to mapOf(
                "title" to title,
                "body" to body
            )
        )
    }
    
    /**
     * Build data payload
     */
    fun buildDataPayload(action: String, payload: String): Map<String, Any> {
        return mapOf(
            "action" to action,
            "payload" to payload,
            "timestamp" to System.currentTimeMillis()
        )
    }
    
    /**
     * Build notification with custom sound
     */
    fun buildWithSound(
        title: String,
        body: String,
        sound: String = "default"
    ): Map<String, Any> {
        return mapOf(
            "notification" to mapOf(
                "title" to title,
                "body" to body,
                "sound" to sound
            ),
            "android" to mapOf(
                "notification" to mapOf(
                    "sound" to sound
                )
            )
        )
    }
    
    /**
     * Build with badge
     */
    fun buildWithBadge(
        title: String,
        body: String,
        badge: Int
    ): Map<String, Any> {
        return mapOf(
            "notification" to mapOf(
                "title" to title,
                "body" to body
            ),
            "android" to mapOf(
                "notification" to mapOf(
                    "badge" to badge
                )
            )
        )
    }
}

/**
 * Message priority manager
 */
class MessagePriorityManager {
    
    /**
     * Send high priority message
     */
    fun sendHighPriority(token: String, title: String, body: String) {
        val androidConfig = AndroidConfig.builder()
            .setPriority(AndroidConfig.Priority.HIGH)
            .setTtl(3600) // 1 hour in seconds
            .build()
        
        val message = Message.builder()
            .setToken(token)
            .setNotification(Notification.builder()
                .setTitle(title)
                .setBody(body)
                .build())
            .setAndroidConfig(androidConfig)
            .build()
        
        println("High priority message sent")
    }
    
    /**
     * Send normal priority message
     */
    fun sendNormalPriority(token: String, title: String, body: String) {
        val androidConfig = AndroidConfig.builder()
            .setPriority(AndroidConfig.Priority.NORMAL)
            .setTtl(86400) // 24 hours
            .build()
        
        val message = Message.builder()
            .setToken(token)
            .setNotification(Notification.builder()
                .setTitle(title)
                .setBody(body)
                .build())
            .setAndroidConfig(androidConfig)
            .build()
        
        println("Normal priority message sent")
    }
}
```

**Output:**
```
High priority message sent
Topic message sent: updates
Data message sent to device
```

### Example 3: Advanced FCM Implementation

```kotlin
import android.content.Context
import com.google.firebase.messaging.FirebaseMessaging

/**
 * FCM manager with advanced features
 */
class FCMManager(private val context: Context) {
    
    private val topicManager = TopicManager(context)
    private val tokenManager = TokenManager(context)
    
    /**
     * Initialize FCM - get token and subscribe to topics
     */
    fun initialize() {
        tokenManager.getTokenAsync { token ->
            token?.let {
                // Subscribe to default topics
                subscribeToDefaultTopics()
            }
        }
    }
    
    private fun subscribeToDefaultTopics() {
        topicManager.subscribeToTopics(listOf(
            TOPIC_NOTIFICATIONS,
            TOPIC_UPDATES,
            TOPIC_NEWS
        ))
    }
    
    /**
     * Handle token refresh
     */
    fun handleTokenRefresh(newToken: String) {
        val oldToken = tokenManager.getToken()
        
        if (oldToken != newToken) {
            println("Token changed, updating server")
            tokenManager.saveToken(newToken)
            updateServerToken(newToken)
        }
    }
    
    private fun updateServerToken(token: String) {
        // Call your server to update the token
        println("Server token updated: $token")
    }
    
    /**
     * Handle deep link from message
     */
    fun handleDeepLink(data: Map<String, String>): String? {
        val deepLink = data["deep_link"]
        val screen = data["screen"]
        
        return when {
            deepLink != null -> deepLink
            screen != null -> "app://$screen"
            else -> null
        }
    }
    
    companion object {
        const val TOPIC_NOTIFICATIONS = "notifications"
        const val TOPIC_UPDATES = "updates"
        const val TOPIC_NEWS = "news"
    }
}

/**
 * Message handler for processing incoming messages
 */
class MessageHandler(private val context: Context) {
    
    /**
     * Process message based on type
     */
    fun processMessage(message: Map<String, String>): MessageAction {
        val type = message["type"]
        
        return when (type) {
            TYPE_NAVIGATE -> handleNavigation(message)
            TYPE_UPDATE -> handleUpdate(message)
            TYPE_SYNC -> handleSync(message)
            TYPE_CUSTOM -> handleCustom(message)
            else -> MessageAction.ShowNotification(message["title"] ?: "Message", message["body"] ?: "")
        }
    }
    
    private fun handleNavigation(message: Map<String, String>): MessageAction {
        val screen = message["screen"]
        val params = message["params"]
        return MessageAction.Navigate(screen ?: "", params ?: "")
    }
    
    private fun handleUpdate(message: Map<String, String>): MessageAction {
        val updateType = message["update_type"]
        return MessageAction.RefreshData(updateType ?: "")
    }
    
    private fun handleSync(message: Map<String, String>): MessageAction {
        return MessageAction.SyncData
    }
    
    private fun handleCustom(message: Map<String, String>): MessageAction {
        val action = message["action"]
        val payload = message["payload"]
        return MessageAction.Custom(action ?: "", payload ?: "")
    }
    
    sealed class MessageAction {
        data class Navigate(val screen: String, val params: String) : MessageAction()
        data class RefreshData(val updateType: String) : MessageAction()
        object SyncData : MessageAction()
        data class Custom(val action: String, val payload: String) : MessageAction()
        data class ShowNotification(val title: String, val body: String) : MessageAction()
    }
}

/**
 * Notification customization based on message data
 */
class NotificationCustomizer(private val context: Context) {
    
    /**
     * Customize notification based on message
     */
    fun customize(message: Map<String, String>): NotificationConfig {
        val priority = message["priority"]?.toIntOrNull() ?: NotificationCompat.PRIORITY_DEFAULT
        val sound = message["sound"] ?: "default"
        val badge = message["badge"]?.toIntOrNull() ?: 0
        val color = message["color"]?.let { parseColor(it) }
        val icon = message["icon"]
        
        return NotificationConfig(
            priority = priority,
            sound = sound,
            badge = badge,
            color = color,
            icon = icon
        )
    }
    
    private fun parseColor(colorString: String): Int {
        return try {
            android.graphics.Color.parseColor(colorString)
        } catch (e: Exception) {
            0
        }
    }
    
    data class NotificationConfig(
        val priority: Int,
        val sound: String,
        val badge: Int,
        val color: Int?,
        val icon: String?
    )
}

/**
 * Message statistics tracking
 */
class MessageStats {
    
    private var received = 0
    private var shown = 0
    private var dismissed = 0
    private var clicked = 0
    
    /**
     * Record received message
     */
    fun recordReceived() {
        received++
        println("Message received: $received")
    }
    
    /**
     * Record notification shown
     */
    fun recordShown() {
        shown++
        println("Notification shown: $shown")
    }
    
    /**
     * Record notification dismissed
     */
    fun recordDismissed() {
        dismissed++
    }
    
    /**
     * Record notification clicked
     */
    fun recordClicked() {
        clicked++
        println("Notification clicked: $clicked")
    }
    
    /**
     * Get statistics summary
     */
    fun getSummary(): Map<String, Int> {
        return mapOf(
            "received" to received,
            "shown" to shown,
            "dismissed" to dismissed,
            "clicked" to clicked,
            "clickRate" to if (shown > 0) (clicked * 100 / shown) else 0
        )
    }
}

/**
 * FCM in production app
 */
class ProductionFCMManager(private val context: Context) {
    
    private val fcmManager = FCMManager(context)
    private val messageHandler = MessageHandler(context)
    private val stats = MessageStats()
    
    /**
     * Initialize on app start
     */
    fun onAppStart() {
        fcmManager.initialize()
    }
    
    /**
     * Process incoming message
     */
    fun processMessage(data: Map<String, String>) {
        stats.recordReceived()
        
        val action = messageHandler.processMessage(data)
        
        when (action) {
            is MessageHandler.MessageAction.Navigate -> handleNavigation(action)
            is MessageHandler.MessageAction.RefreshData -> handleRefresh(action)
            is MessageHandler.MessageAction.SyncData -> handleSync()
            is MessageHandler.MessageAction.Custom -> handleCustom(action)
            is MessageHandler.MessageAction.ShowNotification -> showNotification(action)
        }
    }
    
    private fun handleNavigation(action: MessageHandler.MessageAction.Navigate) {
        println("Navigate to: ${action.screen}")
    }
    
    private fun handleRefresh(action: MessageHandler.MessageAction.RefreshData) {
        println("Refresh data: ${action.updateType}")
    }
    
    private fun handleSync() {
        println("Sync data")
    }
    
    private fun handleCustom(action: MessageHandler.MessageAction.Custom) {
        println("Custom action: ${action.action}")
    }
    
    private fun showNotification(action: MessageHandler.MessageAction.ShowNotification) {
        NotificationHelper(context).showSimpleNotification(
            action.title,
            action.body
        )
        stats.recordShown()
    }
    
    /**
     * Handle notification click
     */
    fun onNotificationClicked(data: Map<String, String>) {
        stats.recordClicked()
        
        // Navigate to appropriate screen based on message data
        val deepLink = data["deep_link"]
        println("Notification clicked, deep link: $deepLink")
    }
    
    /**
     * Get message statistics
     */
    fun getStats(): Map<String, Int> = stats.getSummary()
}
```

**Output:**
```
FCM initialized
Message received: 1
Notification shown: 1
Notification clicked, deep link: app://home
Stats: {received=1, shown=1, clicked=1}
```

## Best Practices

- Use appropriate message types for your needs
- Handle message payload efficiently
- Implement token refresh handling
- Use topics for targeted messaging
- Set appropriate TTL for messages
- Track message delivery and engagement

## Common Pitfalls

### Problem: Messages not received
**Solution:** Check FCM service registration and message payload

### Problem: Token expired
**Solution:** Implement token refresh in onNewToken

### Problem: Notification not showing
**Solution:** Check notification channel and importance settings

## Troubleshooting Guide

**Q: Why aren't messages being delivered?**
A: Check device token, server key, and message payload

**Q: How to handle background messages?**
A: Use FirebaseMessagingService to handle data messages

**Q: How to implement topic messaging?**
A: Use subscribeToTopic and send to topic from server

## Cross-References

- [Notifications](./04_Notifications.md)
- [Work Manager](./01_Work_Manager.md)
- [Network Security](./03_Network_Security.md)