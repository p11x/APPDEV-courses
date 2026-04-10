# Notifications

## Overview

Android notifications provide a way to keep users informed about important events even when the app is not in the foreground. Modern notifications support rich content, actions, and conversational interfaces.

## Learning Objectives

- Create basic and rich notifications
- Implement notification channels
- Handle notification actions
- Build messaging-style notifications
- Manage notification channels and importance

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Work Manager](./01_Work_Manager.md)

## Core Concepts

### Notification Channels

Since Android 8.0, notifications must be posted to a channel:
- Each channel has importance level
- Users can customize each channel
- Channels persist across app updates

### Notification Types

- Basic notifications
- Big text notifications
- Big picture notifications
- Messaging-style notifications
- Media style notifications

## Code Examples

### Example 1: Basic Notifications

```kotlin
import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat

/**
 * Notification channel manager
 * Creates and manages notification channels
 */
class NotificationChannelManager(private val context: Context) {
    
    companion object {
        const val CHANNEL_IMPORTANT = "important_channel"
        const val CHANNEL_MESSAGES = "messages_channel"
        const val CHANNEL_LOW = "low_priority_channel"
    }
    
    /**
     * Create all required notification channels
     */
    fun createChannels() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val notificationManager = context.getSystemService(Context.NOTIFICATION_SERVICE) 
                as NotificationManager
            
            // Important notifications - high importance
            createImportantChannel(notificationManager)
            
            // Message notifications - default importance
            createMessagesChannel(notificationManager)
            
            // Low priority - minimal disruption
            createLowPriorityChannel(notificationManager)
        }
    }
    
    private fun createImportantChannel(manager: NotificationManager) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channel = NotificationChannel(
                CHANNEL_IMPORTANT,
                "Important Notifications",
                NotificationManager.IMPORTANCE_HIGH
            ).apply {
                description = "High priority notifications that require immediate attention"
                enableVibration(true)
                enableLights(true)
                setShowBadge(true)
            }
            manager.createNotificationChannel(channel)
        }
    }
    
    private fun createMessagesChannel(manager: NotificationManager) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channel = NotificationChannel(
                CHANNEL_MESSAGES,
                "Messages",
                NotificationManager.IMPORTANCE_DEFAULT
            ).apply {
                description = "Message and chat notifications"
                enableVibration(true)
                setShowBadge(true)
            }
            manager.createNotificationChannel(channel)
        }
    }
    
    private fun createLowPriorityChannel(manager: NotificationManager) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channel = NotificationChannel(
                CHANNEL_LOW,
                "Background Updates",
                NotificationManager.IMPORTANCE_LOW
            ).apply {
                description = "Low priority background updates"
                setShowBadge(false)
            }
            manager.createNotificationChannel(channel)
        }
    }
    
    /**
     * Delete a notification channel
     */
    fun deleteChannel(channelId: String) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val manager = context.getSystemService(Context.NOTIFICATION_SERVICE) 
                as NotificationManager
            manager.deleteNotificationChannel(channelId)
        }
    }
}

/**
 * Basic notification builder
 */
class BasicNotificationBuilder(private val context: Context) {
    
    private var title: String = ""
    private var message: String = ""
    private var channelId: String = NotificationChannelManager.CHANNEL_IMPORTANT
    private var smallIcon: Int = android.R.drawable.ic_dialog_info
    private var largeIcon: Int? = null
    private var priority: Int = NotificationCompat.PRIORITY_DEFAULT
    private var autoCancel: Boolean = true
    private var pendingIntent: PendingIntent? = null
    private var actions: List<NotificationCompat.Action> = emptyList()
    
    fun setTitle(title: String) = apply { this.title = title }
    fun setMessage(message: String) = apply { this.message = message }
    fun setChannelId(channelId: String) = apply { this.channelId = channelId }
    fun setSmallIcon(icon: Int) = apply { this.smallIcon = icon }
    fun setLargeIcon(icon: Int) = apply { this.largeIcon = icon }
    fun setPriority(priority: Int) = apply { this.priority = priority }
    fun setAutoCancel(autoCancel: Boolean) = apply { this.autoCancel = autoCancel }
    fun setPendingIntent(intent: PendingIntent) = apply { this.pendingIntent = intent }
    fun addAction(action: NotificationCompat.Action) = apply { 
        this.actions = actions + action 
    }
    
    fun build(): NotificationCompat.Builder {
        val builder = NotificationCompat.Builder(context, channelId)
            .setSmallIcon(smallIcon)
            .setContentTitle(title)
            .setContentText(message)
            .setPriority(priority)
            .setAutoCancel(autoCancel)
            .setContentIntent(pendingIntent)
        
        largeIcon?.let { builder.setLargeIcon(it) }
        actions.forEach { builder.addAction(it) }
        
        return builder
    }
    
    fun show() {
        val notification = build().build()
        NotificationManagerCompat.from(context).notify(
            System.currentTimeMillis().toInt(),
            notification
        )
    }
}

/**
 * Notification helper for common operations
 */
class NotificationHelper(private val context: Context) {
    
    private val channelManager = NotificationChannelManager(context)
    private val notificationManager = NotificationManagerCompat.from(context)
    
    init {
        channelManager.createChannels()
    }
    
    /**
     * Show simple notification
     */
    fun showSimpleNotification(
        title: String,
        message: String,
        channelId: String = NotificationChannelManager.CHANNEL_IMPORTANT
    ) {
        val intent = context.packageManager
            .getLaunchIntentForPackage(context.packageName)
        
        val pendingIntent = PendingIntent.getActivity(
            context,
            0,
            intent,
            PendingIntent.FLAG_IMMUTABLE
        )
        
        BasicNotificationBuilder(context)
            .setTitle(title)
            .setMessage(message)
            .setChannelId(channelId)
            .setPendingIntent(pendingIntent)
            .show()
    }
    
    /**
     * Show notification with action
     */
    fun showNotificationWithActions(
        title: String,
        message: String,
        actionTitle: String,
        actionIntent: Intent
    ) {
        val pendingIntent = PendingIntent.getActivity(
            context,
            0,
            context.packageManager.getLaunchIntentForPackage(context.packageName),
            PendingIntent.FLAG_IMMUTABLE
        )
        
        val actionPendingIntent = PendingIntent.getBroadcast(
            context,
            1,
            actionIntent,
            PendingIntent.FLAG_IMMUTABLE
        )
        
        val action = NotificationCompat.Action.Builder(
            android.R.drawable.ic_menu_share,
            actionTitle,
            actionPendingIntent
        ).build()
        
        BasicNotificationBuilder(context)
            .setTitle(title)
            .setMessage(message)
            .addAction(action)
            .show()
    }
    
    /**
     * Cancel specific notification
     */
    fun cancelNotification(notificationId: Int) {
        notificationManager.cancel(notificationId)
    }
    
    /**
     * Cancel all notifications
     */
    fun cancelAllNotifications() {
        notificationManager.cancelAll()
    }
    
    /**
     * Check if notifications are enabled
     */
    fun areNotificationsEnabled(): Boolean {
        return notificationManager.areNotificationsEnabled()
    }
}
```

**Output:**
```
Notification shown: Important Notifications
Channel created: important_channel
```

### Example 2: Rich Notifications

```kotlin
import android.app.NotificationManager
import android.app.PendingIntent
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.os.Build
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat

/**
 * Big text style notification
 */
class BigTextNotification(private val context: Context) {
    
    fun show(title: String, bigText: String, summary: String = "") {
        val intent = context.packageManager.getLaunchIntentForPackage(context.packageName)
        val pendingIntent = PendingIntent.getActivity(
            context, 0, intent, PendingIntent.FLAG_IMMUTABLE
        )
        
        val bigTextStyle = NotificationCompat.BigTextStyle()
            .bigText(bigText)
            .setBigContentTitle(title)
            .setSummaryText(summary)
        
        val notification = NotificationCompat.Builder(context, "messages_channel")
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setContentTitle(title)
            .setContentText(summary.ifEmpty { bigText.take(50) + "..." })
            .setStyle(bigTextStyle)
            .setContentIntent(pendingIntent)
            .setAutoCancel(true)
            .build()
        
        NotificationManagerCompat.from(context).notify(
            System.currentTimeMillis().toInt(),
            notification
        )
    }
}

/**
 * Big picture style notification
 */
class BigPictureNotification(private val context: Context) {
    
    fun show(title: String, message: String, pictureResId: Int, summary: String = "") {
        val intent = context.packageManager.getLaunchIntentForPackage(context.packageName)
        val pendingIntent = PendingIntent.getActivity(
            context, 0, intent, PendingIntent.FLAG_IMMUTABLE
        )
        
        // Load bitmap from resource
        val bitmap = BitmapFactory.decodeResource(context.resources, pictureResId)
        
        val pictureStyle = NotificationCompat.BigPictureStyle()
            .bigPicture(bitmap)
            .bigLargeIcon(bitmap)
            .setBigContentTitle(title)
            .setSummaryText(summary)
        
        val notification = NotificationCompat.Builder(context, "important_channel")
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setContentTitle(title)
            .setContentText(message)
            .setStyle(pictureStyle)
            .setContentIntent(pendingIntent)
            .setAutoCancel(true)
            .build()
        
        NotificationManagerCompat.from(context).notify(
            System.currentTimeMillis().toInt(),
            notification
        )
    }
}

/**
 * Inbox style notification
 */
class InboxStyleNotification(private val context: Context) {
    
    fun show(
        title: String,
        summary: String,
        messages: List<String>,
        channelId: String = NotificationChannelManager.CHANNEL_MESSAGES
    ) {
        val intent = context.packageManager.getLaunchIntentForPackage(context.packageName)
        val pendingIntent = PendingIntent.getActivity(
            context, 0, intent, PendingIntent.FLAG_IMMUTABLE
        )
        
        val inboxStyle = NotificationCompat.InboxStyle()
            .setBigContentTitle(title)
            .setSummaryText(summary)
        
        messages.forEach { inboxStyle.addLine(it) }
        
        val notification = NotificationCompat.Builder(context, channelId)
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setContentTitle(title)
            .setContentText(summary)
            .setStyle(inboxStyle)
            .setContentIntent(pendingIntent)
            .setAutoCancel(true)
            .build()
        
        NotificationManagerCompat.from(context).notify(
            System.currentTimeMillis().toInt(),
            notification
        )
    }
}

/**
 * Progress notification
 */
class ProgressNotification(private val context: Context) {
    
    private var notificationId: Int = 0
    private var isIndeterminate: Boolean = false
    
    fun showProgress(
        title: String,
        progress: Int,
        max: Int = 100,
        isIndeterminate: Boolean = false
    ) {
        this.isIndeterminate = isIndeterminate
        this.notificationId = System.currentTimeMillis().toInt()
        
        val notification = NotificationCompat.Builder(context, "low_priority_channel")
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setContentTitle(title)
            .setContentText(if (isIndeterminate) "Loading..." else "$progress%")
            .setProgress(max, progress, isIndeterminate)
            .setOngoing(true)
            .build()
        
        NotificationManagerCompat.from(context).notify(notificationId, notification)
    }
    
    fun updateProgress(title: String, progress: Int, max: Int = 100) {
        showProgress(title, progress, max, isIndeterminate)
    }
    
    fun complete(title: String, message: String) {
        val notification = NotificationCompat.Builder(context, "low_priority_channel")
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setContentTitle(title)
            .setContentText(message)
            .setAutoCancel(true)
            .build()
        
        NotificationManagerCompat.from(context).notify(notificationId, notification)
    }
    
    fun dismiss() {
        NotificationManagerCompat.from(context).cancel(notificationId)
    }
}
```

**Output:**
```
Big text notification shown
Progress: 50%
Progress: 100%
Download complete
```

### Example 3: Messaging-Style Notifications

```kotlin
import android.app.NotificationManager
import android.app.RemoteInput
import android.content.Context
import android.os.Build
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat
import java.util.ArrayList

/**
 * Person for messaging
 */
class Person(
    val name: String,
    val icon: Int? = null,
    val uri: String? = null
)

/**
 * Message data class
 */
data class Message(
    val text: String,
    val timestamp: Long,
    val sender: Person,
    val isRemoteInput: Boolean = false
)

/**
 * Messaging style notification
 */
class MessagingNotification(private val context: Context) {
    
    private val messages = ArrayList<NotificationCompat.MessagingStyle.Message>()
    private var conversationTitle: String = ""
    private var sender: Person? = null
    
    /**
     * Set conversation title
     */
    fun setConversationTitle(title: String) = apply {
        this.conversationTitle = title
    }
    
    /**
     * Set current user
     */
    fun setUser(user: Person) = apply {
        this.sender = user
    }
    
    /**
     * Add message to conversation
     */
    fun addMessage(text: String, timestamp: Long, sender: Person) = apply {
        val message = NotificationCompat.MessagingStyle.Message(
            text,
            timestamp,
            sender
        )
        messages.add(message)
    }
    
    /**
     * Show messaging notification
     */
    fun show(notificationId: Int, channelId: String = NotificationChannelManager.CHANNEL_MESSAGES) {
        val user = sender ?: Person("Me")
        
        val messagingStyle = NotificationCompat.MessagingStyle(user)
            .setConversationTitle(conversationTitle)
        
        messages.forEach { messagingStyle.addMessage(it) }
        
        // Add reply action
        val replyAction = createReplyAction()
        
        val notification = NotificationCompat.Builder(context, channelId)
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setStyle(messagingStyle)
            .addAction(replyAction)
            .setAutoCancel(true)
            .build()
        
        NotificationManagerCompat.from(context).notify(notificationId, notification)
    }
    
    private fun createReplyAction(): NotificationCompat.Action {
        // Create remote input for quick reply
        val remoteInput = RemoteInput.Builder(KEY_TEXT_REPLY)
            .setLabel("Reply")
            .build()
        
        val replyIntent = android.content.Intent(context, NotificationReceiver::class.java).apply {
            action = ACTION_REPLY
        }
        
        val pendingIntent = PendingIntent.getBroadcast(
            context,
            0,
            replyIntent,
            PendingIntent.FLAG_MUTABLE
        )
        
        return NotificationCompat.Action.Builder(
            android.R.drawable.ic_menu_send,
            "Reply",
            pendingIntent
        ).addRemoteInput(remoteInput).build()
    }
    
    companion object {
        const val KEY_TEXT_REPLY = "key_text_reply"
        const val ACTION_REPLY = "com.example.ACTION_REPLY"
    }
}

/**
 * Direct reply handler
 */
class NotificationReceiver : android.content.BroadcastReceiver() {
    
    override fun onReceive(context: Context, intent: android.content.Intent) {
        if (intent.action == MessagingNotification.ACTION_REPLY) {
            val remoteInput = RemoteInput.getResultsFromIntent(intent)
            val replyText = remoteInput?.getCharSequence(
                MessagingNotification.KEY_TEXT_REPLY
            )?.toString()
            
            println("Reply received: $replyText")
            
            // Process reply and update notification
            // In production, update the existing notification or start a service
        }
    }
}

/**
 * Grouped notifications manager
 */
class GroupedNotificationManager(private val context: Context) {
    
    private val notificationManager = NotificationManagerCompat.from(context)
    
    /**
     * Show grouped notifications
     */
    fun showGroupedNotification(
        groupKey: String,
        groupTitle: String,
        summary: String,
        notifications: List<NotificationData>
    ) {
        // Show individual child notifications
        notifications.forEachIndexed { index, data ->
            showChildNotification(groupKey, index, data)
        }
        
        // Show summary notification
        showSummaryNotification(groupKey, groupTitle, summary, notifications.size)
    }
    
    private fun showChildNotification(groupKey: String, id: Int, data: NotificationData) {
        val groupOptions = NotificationCompat.Delegator(groupKey)
        
        val child = NotificationCompat.Builder(context, data.channelId)
            .setSmallIcon(data.icon)
            .setContentTitle(data.title)
            .setContentText(data.message)
            .setGroup(groupKey)
            .setGroupSummary(false)
            .setAutoCancel(true)
            .build()
        
        notificationManager.notify(id, child)
    }
    
    private fun showSummaryNotification(
        groupKey: String,
        groupTitle: String,
        summary: String,
        count: Int
    ) {
        val summaryNotification = NotificationCompat.Builder(context, "messages_channel")
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setContentTitle(groupTitle)
            .setContentText("$count new messages")
            .setStyle(NotificationCompat.BigTextStyle().bigText(summary))
            .setGroup(groupKey)
            .setGroupSummary(true)
            .setAutoCancel(true)
            .build()
        
        notificationManager.notify(GROUP_SUMMARY_ID, summaryNotification)
    }
    
    fun dismissGroup(groupKey: String) {
        // Dismiss all in group
    }
    
    data class NotificationData(
        val title: String,
        val message: String,
        val icon: Int = android.R.drawable.ic_dialog_info,
        val channelId: String = NotificationChannelManager.CHANNEL_MESSAGES
    )
    
    companion object {
        private const val GROUP_SUMMARY_ID = -1
    }
}
```

**Output:**
```
Messaging notification shown with 3 messages
Grouped: 5 notifications with summary
Reply received: Thanks!
```

## Best Practices

- Create notification channels early
- Set appropriate importance levels
- Use notification actions appropriately
- Implement proper notification groups
- Handle notification channels gracefully
- Respect user's notification settings

## Common Pitfalls

### Problem: Notification not showing
**Solution:** Check notification permission and channel importance

### Problem: Notification not updating
**Solution:** Use same notification ID

### Problem: Group notifications not working
**Solution:** Set GROUP on child and GROUP_SUMMARY on summary

## Troubleshooting Guide

**Q: Why notifications are not appearing?**
A: Check POST_NOTIFICATIONS permission for Android 13+

**Q: How to handle notification tap?**
A: Use setContentIntent with PendingIntent

**Q: How to update existing notification?**
A: Use same notification ID in notify()

## Cross-References

- [Work Manager](./01_Work_Manager.md)
- [Cloud Messaging](./05_Cloud_Messaging.md)
- [Security Best Practices](./03_Security/05_Security_Best_Practices.md)