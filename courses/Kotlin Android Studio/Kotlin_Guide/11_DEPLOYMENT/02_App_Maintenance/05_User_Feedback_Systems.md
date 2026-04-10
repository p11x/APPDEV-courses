# User Feedback Systems

## Learning Objectives

1. Understanding user feedback collection methods
2. Implementing in-app feedback mechanisms
3. Setting up rating prompts and review requests
4. Building feedback management workflows
5. Integrating user feedback with analytics
6. Responding to user feedback effectively

## Prerequisites

- [Analytics Integration](./01_Analytics_Integration.md)
- [Crash Reporting](./02_Crash_Reporting.md)

## Section 1: User Feedback Fundamentals

User feedback is essential for understanding how users perceive your app and identifying areas for improvement. Effective feedback systems capture qualitative insights that complement quantitative metrics like analytics and crash reports.

There are several types of user feedback to consider. In-app feedback forms allow users to submit detailed comments and suggestions. Ratings and reviews on the Play Store provide public feedback that influences other users. Net Promoter Score (NPS) surveys measure user loyalty. Feature requests help prioritize development. Bug reports allow users to report issues they encounter. Social feedback comes from social media mentions and reviews.

Each feedback type serves different purposes and requires different collection methods. A comprehensive feedback strategy incorporates multiple channels to capture the full picture of user sentiment.

```kotlin
// Feedback data models and configuration
package com.example.myapp.feedback

import android.os.Bundle
import com.google.firebase.analytics.FirebaseAnalytics

data class UserFeedback(
    val id: String,
    val userId: String?,
    val feedbackType: FeedbackType,
    val rating: Int? = null,
    val category: FeedbackCategory? = null,
    val title: String? = null,
    val description: String,
    val screenshots: List<String> = emptyList(),
    val deviceInfo: DeviceInfo? = null,
    val appState: AppState? = null,
    val timestamp: Long = System.currentTimeMillis(),
    val sentiment: Sentiment? = null
)

enum class FeedbackType {
    BUG_REPORT,
    FEATURE_REQUEST,
    GENERAL_FEEDBACK,
    RATING,
    REVIEW,
    NPS_SURVEY,
    IN_APP_SURVEY
}

enum class FeedbackCategory {
    UI_USABILITY,
    PERFORMANCE,
    FEATURES,
    CONTENT,
    PRICING,
    CRASH,
    DATA_ISSUES,
    ACCOUNT,
    OTHER
}

enum class Sentiment {
    POSITIVE,
    NEUTRAL,
    NEGATIVE
}

data class DeviceInfo(
    val manufacturer: String,
    val model: String,
    val osVersion: String,
    val appVersion: String,
    val screenSize: String,
    val deviceId: String
)

data class AppState(
    val currentScreen: String,
    val userSegment: String,
    val sessionDuration: Long,
    val previousScreens: List<String>,
    val actions: List<String>
)

// Feedback collector interface
interface FeedbackCollector {
    fun collectFeedback(feedback: UserFeedback)
    fun canShowFeedbackPrompt(): Boolean
    fun onFeedbackSubmitted(success: Boolean)
}

// Feedback submission manager
class FeedbackManager(private val context: Context) {
    
    private val analytics = FirebaseAnalytics.getInstance(context)
    
    // Submit feedback to backend
    suspend fun submitFeedback(feedback: UserFeedback): SubmissionResult {
        return try {
            // Submit to your feedback backend
            val result = submitToBackend(feedback)
            
            // Log analytics event
            logFeedbackSubmission(feedback, result.success)
            
            // If bug report, also submit to crash reporting
            if (feedback.feedbackType == FeedbackType.BUG_REPORT) {
                submitBugReportToCrashlytics(feedback)
            }
            
            result
        } catch (e: Exception) {
            SubmissionResult(false, e.message)
        }
    }
    
    private suspend fun submitToBackend(feedback: UserFeedback): SubmissionResult {
        // Implementation: send to your backend API
        return SubmissionResult(true)
    }
    
    private fun logFeedbackSubmission(feedback: UserFeedback, success: Boolean) {
        val params = Bundle().apply {
            putString("feedback_type", feedback.feedbackType.name)
            putString("feedback_category", feedback.category?.name)
            feedback.rating?.let { putInt("rating", it) }
            putBoolean("success", success)
            feedback.sentiment?.let { putString("sentiment", it.name) }
        }
        
        analytics.logEvent("feedback_submitted", params)
    }
    
    private fun submitBugReportToCrashlytics(feedback: UserFeedback) {
        // Submit to Firebase Crashlytics
    }
}

data class SubmissionResult(
    val success: Boolean,
    val errorMessage: String? = null,
    val feedbackId: String? = null
)
```

## Section 2: In-App Feedback Collection

Implementing in-app feedback forms allows users to provide feedback directly from within the app. The feedback UI should be simple, accessible, and respect user time.

```kotlin
// In-app feedback form implementation
package com.example.myapp.feedback.ui

import android.app.Activity
import android.content.Context
import android.view.LayoutInflater
import androidx.appcompat.app.AlertDialog

class InAppFeedbackForm(private val context: Context) {
    
    interface FeedbackFormListener {
        fun onFeedbackSubmitted(feedback: UserFeedback)
        fun onFeedbackCancelled()
    }
    
    private var currentScreen: String = ""
    private var currentUserId: String? = null
    private var listener: FeedbackFormListener? = null
    
    fun setCurrentScreen(screenName: String) {
        currentScreen = screenName
    }
    
    fun setUserId(userId: String?) {
        currentUserId = userId
    }
    
    fun setListener(listener: FeedbackFormListener) {
        this.listener = listener
    }
    
    fun showFeedbackDialog() {
        val categories = FeedbackCategory.values().map { it.toDisplayString() }.toTypedArray()
        
        AlertDialog.Builder(context)
            .setTitle("Send Feedback")
            .setItems(categories) { _, which ->
                val category = FeedbackCategory.values()[which]
                showFeedbackInputDialog(category)
            }
            .setNegativeButton("Cancel") { dialog, _ ->
                listener?.onFeedbackCancelled()
                dialog.dismiss()
            }
            .show()
    }
    
    private fun showFeedbackInputDialog(category: FeedbackCategory) {
        // In a real implementation, this would show a custom dialog
        // with input fields for title, description, and screenshots
        
        val dialogView = LayoutInflater.from(context)
            .inflate(R.layout.feedback_dialog, null)
        
        val titleInput = dialogView.findViewById<com.google.android.material.textfield.TextInputEditText>(R.id.feedback_title)
        val descriptionInput = dialogView.findViewById<com.google.android.material.textfield.TextInputEditText>(R.id.feedback_description)
        
        AlertDialog.Builder(context)
            .setTitle("Feedback: ${category.toDisplayString()}")
            .setView(dialogView)
            .setPositiveButton("Submit") { dialog, _ ->
                val title = titleInput?.text?.toString() ?: ""
                val description = descriptionInput?.text?.toString() ?: ""
                
                if (description.isNotBlank()) {
                    val feedback = createFeedback(category, title, description)
                    listener?.onFeedbackSubmitted(feedback)
                }
                
                dialog.dismiss()
            }
            .setNegativeButton("Cancel") { dialog, _ ->
                listener?.onFeedbackCancelled()
                dialog.dismiss()
            }
            .show()
    }
    
    private fun createFeedback(
        category: FeedbackCategory,
        title: String,
        description: String
    ): UserFeedback {
        val sentiment = analyzeSentiment(description)
        
        return UserFeedback(
            id = generateFeedbackId(),
            userId = currentUserId,
            feedbackType = FeedbackType.GENERAL_FEEDBACK,
            category = category,
            title = title.ifBlank { null },
            description = description,
            deviceInfo = captureDeviceInfo(),
            appState = captureAppState(),
            sentiment = sentiment
        )
    }
    
    private fun analyzeSentiment(description: String): Sentiment {
        // Simple keyword-based sentiment analysis
        val positiveWords = listOf("great", "love", "excellent", "amazing", "good", "awesome")
        val negativeWords = listOf("bad", "terrible", "hate", "awful", "poor", "worst", "bug", "crash", "slow")
        
        val lowerDesc = description.lowercase()
        
        val positiveCount = positiveWords.count { lowerDesc.contains(it) }
        val negativeCount = negativeWords.count { lowerDesc.contains(it) }
        
        return when {
            positiveCount > negativeCount -> Sentiment.POSITIVE
            negativeCount > positiveCount -> Sentiment.NEGATIVE
            else -> Sentiment.NEUTRAL
        }
    }
    
    private fun generateFeedbackId(): String {
        return java.util.UUID.randomUUID().toString()
    }
    
    private fun captureDeviceInfo(): DeviceInfo {
        val packageInfo = context.packageManager.getPackageInfo(context.packageName, 0)
        val osVersion = android.os.Build.VERSION.RELEASE
        val model = "${android.os.Build.MANUFACTURER} ${android.os.Build.MODEL}"
        
        return DeviceInfo(
            manufacturer = android.os.Build.MANUFACTURER,
            model = android.os.Build.MODEL,
            osVersion = osVersion,
            appVersion = packageInfo.versionName ?: "unknown",
            screenSize = context.resources.configuration.screenLayout.toString(),
            deviceId = android.os.Build.FINGERPRINT
        )
    }
    
    private fun captureAppState(): AppState {
        return AppState(
            currentScreen = currentScreen,
            userSegment = "active",  // Determine from user properties
            sessionDuration = System.currentTimeMillis() - sessionStartTime,
            previousScreens = recentScreens,
            actions = recentActions
        )
    }
    
    private val sessionStartTime: Long = System.currentTimeMillis()
    private val recentScreens: List<String> = emptyList()
    private val recentActions: List<String> = emptyList()
    
    private fun FeedbackCategory.toDisplayString(): String {
        return when (this) {
            FeedbackCategory.UI_USABILITY -> "UI / Usability"
            FeedbackCategory.PERFORMANCE -> "Performance"
            FeedbackCategory.FEATURES -> "Features"
            FeedbackCategory.CONTENT -> "Content"
            FeedbackCategory.PRICING -> "Pricing"
            FeedbackCategory.CRASH -> "Crash / Bug"
            FeedbackCategory.DATA_ISSUES -> "Data Issues"
            FeedbackCategory.ACCOUNT -> "Account"
            FeedbackCategory.OTHER -> "Other"
        }
    }
}
```

## Section 3: Rating Prompts and Review Requests

Prompting users to rate your app or write reviews at the right moment significantly improves your Play Store rating. Timing and context are critical - prompt users when they're most likely to have a positive experience.

```kotlin
// Rating prompt implementation
package comexample.myapp.rating

import android.content.Context
import android.content.SharedPreferences
import com.google.firebase.analytics.FirebaseAnalytics

class RatingPromptManager(private val context: Context) {
    
    private val prefs: SharedPreferences = context.getSharedPreferences(
        "rating_prompts", Context.MODE_PRIVATE
    )
    
    private val analytics = FirebaseAnalytics.getInstance(context)
    
    // Determine if rating prompt should be shown
    fun shouldShowPrompt(): Boolean {
        // Check if user has already rated
        if (hasUserRated()) return false
        
        // Check if we should wait based on time since install
        val daysSinceInstall = getDaysSinceInstall()
        if (daysSinceInstall < MIN_DAYS_BEFORE_PROMPT) return false
        
        // Check if we should wait based on sessions
        val sessionCount = getSessionCount()
        if (sessionCount < MIN_SESSIONS_BEFORE_PROMPT) return false
        
        // Check cooldown since last prompt
        val daysSinceLastPrompt = getDaysSinceLastPrompt()
        if (daysSinceLastPrompt < COOLDOWN_DAYS) return false
        
        // Check if user is engaged enough (based on session duration)
        if (!isUserEngaged()) return false
        
        return true
    }
    
    // Show rating prompt
    fun showRatingPrompt(activity: Activity, callback: RatingPromptCallback) {
        // Record that prompt was shown
        recordPromptShown()
        
        // Show in-app rating dialog
        showRatingDialog(activity) { rating ->
            if (rating >= 4) {
                // Direct to Play Store
                callback.onUserRatedPositively()
                openPlayStoreReview(activity)
            } else if (rating >= 1) {
                // Show feedback form instead
                callback.onUserRatedNegatively()
                showFeedbackForm(activity)
            }
            
            // Track the rating
            trackRating(rating)
        }
    }
    
    private fun showRatingDialog(activity: Activity, onRating: (Int) -> Unit) {
        android.app.AlertDialog.Builder(activity)
            .setTitle("Enjoying our app?")
            .setMessage("Please take a moment to rate your experience")
            .setPositiveButton("Rate") { dialog, _ ->
                // This would show a custom rating UI
                // For now, assume they rate 4 stars
                onRating(4)
                dialog.dismiss()
            }
            .setNegativeButton("Not now") { dialog, _ ->
                onRating(0)
                dialog.dismiss()
            }
            .setNeutralButton("Feedback") { dialog, _ ->
                onRating(2)
                dialog.dismiss()
            }
            .show()
    }
    
    private fun openPlayStoreReview(activity: Activity) {
        try {
            val intent = android.content.Intent(
                android.content.Intent.ACTION_VIEW,
                android.net.Uri.parse("market://details?id=${context.packageName}")
            )
            activity.startActivity(intent)
        } catch (e: Exception) {
            // Fallback to web Play Store
            val intent = android.content.Intent(
                android.content.Intent.ACTION_VIEW,
                android.net.Uri.parse("https://play.google.com/store/apps/details?id=${context.packageName}")
            )
            activity.startActivity(intent)
        }
        
        // Mark user as having rated
        setUserHasRated()
    }
    
    private fun showFeedbackForm(activity: Activity) {
        // Show in-app feedback form
    }
    
    private fun trackRating(rating: Int) {
        val params = android.os.Bundle().apply {
            putInt("rating", rating)
            putInt("days_since_install", getDaysSinceInstall())
            putInt("session_count", getSessionCount())
        }
        analytics.logEvent("rating_prompt_response", params)
    }
    
    // Preference methods
    private fun hasUserRated(): Boolean {
        return prefs.getBoolean(KEY_USER_RATED, false)
    }
    
    private fun setUserHasRated() {
        prefs.edit().putBoolean(KEY_USER_RATED, true).apply()
    }
    
    private fun getDaysSinceInstall(): Int {
        val installTime = prefs.getLong(KEY_INSTALL_TIME, System.currentTimeMillis())
        return ((System.currentTimeMillis() - installTime) / (1000 * 60 * 60 * 24)).toInt()
    }
    
    private fun getSessionCount(): Int {
        return prefs.getInt(KEY_SESSION_COUNT, 0)
    }
    
    fun incrementSessionCount() {
        prefs.edit().putInt(KEY_SESSION_COUNT, getSessionCount() + 1).apply()
    }
    
    private fun getDaysSinceLastPrompt(): Int {
        val lastPromptTime = prefs.getLong(KEY_LAST_PROMPT_TIME, 0)
        if (lastPromptTime == 0L) return Int.MAX_VALUE
        return ((System.currentTimeMillis() - lastPromptTime) / (1000 * 60 * 60 * 24)).toInt()
    }
    
    private fun recordPromptShown() {
        prefs.edit().putLong(KEY_LAST_PROMPT_TIME, System.currentTimeMillis()).apply()
    }
    
    private fun isUserEngaged(): Boolean {
        // Check if user has significant session time
        val totalSessionTime = prefs.getLong(KEY_TOTAL_SESSION_TIME, 0)
        return totalSessionTime > MIN_ENGAGEMENT_TIME_MS
    }
    
    fun addSessionTime(durationMs: Long) {
        val current = prefs.getLong(KEY_TOTAL_SESSION_TIME, 0)
        prefs.edit().putLong(KEY_TOTAL_SESSION_TIME, current + durationMs).apply()
    }
    
    private fun initializeIfNeeded() {
        if (!prefs.contains(KEY_INSTALL_TIME)) {
            prefs.edit().putLong(KEY_INSTALL_TIME, System.currentTimeMillis()).apply()
        }
    }
    
    companion object {
        private const val KEY_USER_RATED = "user_rated"
        private const val KEY_INSTALL_TIME = "install_time"
        private const val KEY_SESSION_COUNT = "session_count"
        private const val KEY_LAST_PROMPT_TIME = "last_prompt_time"
        private const val KEY_TOTAL_SESSION_TIME = "total_session_time"
        
        private const val MIN_DAYS_BEFORE_PROMPT = 3
        private const val MIN_SESSIONS_BEFORE_PROMPT = 5
        private const val COOLDOWN_DAYS = 14
        private const val MIN_ENGAGEMENT_TIME_MS = 5 * 60 * 1000L  // 5 minutes
    }
}

interface RatingPromptCallback {
    fun onUserRatedPositively()
    fun onUserRatedNegatively()
}
```

## Section 4: NPS and In-App Surveys

NPS (Net Promoter Score) surveys measure user loyalty by asking how likely users are to recommend your app to others. In-app surveys gather specific feedback on features or experiences.

```kotlin
// NPS and survey implementation
package com.example.myapp.surveys

import android.content.Context
import android.os.Bundle
import com.google.firebase.analytics.FirebaseAnalytics

class NPSSurveyManager(private val context: Context) {
    
    private val analytics = FirebaseAnalytics.getInstance(context)
    private val prefs = context.getSharedPreferences("nps_surveys", Context.MODE_PRIVATE)
    
    // NPS Question
    // "How likely are you to recommend our app to a friend or colleague?"
    // Scale: 0-10
    
    enum class NPSCategory(val minScore: Int, val maxScore: Int) {
        DETRACTOR(0, 6),   // Unhappy customers
        PASSIVE(7, 8),     // Satisfied but unenthusiastic
        PROMOTER(9, 10)    // Loyal enthusiasts
    }
    
    fun calculateNPS(promoters: Int, detractors: Int, total: Int): Double {
        if (total == 0) return 0.0
        
        val promoterPercentage = (promoters.toDouble() / total) * 100
        val detractorPercentage = (detractors.toDouble() / total) * 100
        
        return promoterPercentage - detractorPercentage
    }
    
    fun categorizeScore(score: Int): NPSCategory {
        return when (score) {
            in 0..6 -> NPSCategory.DETRACTOR
            in 7..8 -> NPSCategory.PASSIVE
            else -> NPSCategory.PROMOTER
        }
    }
    
    fun shouldShowNPS(): Boolean {
        // Show NPS after user has been active for sufficient time
        val daysSinceInstall = getDaysSinceInstall()
        if (daysSinceInstall < NPS_SHOW_AFTER_DAYS) return false
        
        // Don't ask if already answered
        if (hasAnsweredNPS()) return false
        
        // Check cooldown
        val daysSinceLastNPS = getDaysSinceLastNPS()
        if (daysSinceLastNPS < NPS_COOLDOWN_DAYS) return false
        
        return true
    }
    
    fun submitNPSResponse(score: Int, followUp: String?) {
        val category = categorizeScore(score)
        
        // Log analytics
        val params = Bundle().apply {
            putInt("nps_score", score)
            putString("nps_category", category.name)
            followUp?.let { putString("nps_followup", it) }
        }
        analytics.logEvent("nps_response", params)
        
        // Store response
        storeNPSResponse(score, category)
        
        // If detractor, offer feedback
        if (category == NPSCategory.DETRACTOR) {
            // Can show feedback form
        }
    }
    
    private fun getDaysSinceInstall(): Int {
        val installTime = prefs.getLong("install_time", System.currentTimeMillis())
        return ((System.currentTimeMillis() - installTime) / (1000 * 60 * 60 * 24)).toInt()
    }
    
    private fun hasAnsweredNPS(): Boolean {
        return prefs.getBoolean("nps_answered", false)
    }
    
    private fun getDaysSinceLastNPS(): Int {
        val lastTime = prefs.getLong("last_nps_time", 0)
        if (lastTime == 0L) return Int.MAX_VALUE
        return ((System.currentTimeMillis() - lastTime) / (1000 * 60 * 60 * 24)).toInt()
    }
    
    private fun storeNPSResponse(score: Int, category: NPSCategory) {
        prefs.edit()
            .putBoolean("nps_answered", true)
            .putInt("last_nps_score", score)
            .putString("last_nps_category", category.name)
            .putLong("last_nps_time", System.currentTimeMillis())
            .apply()
    }
    
    companion object {
        private const val NPS_SHOW_AFTER_DAYS = 14
        private const val NPS_COOLDOWN_DAYS = 90
    }
}

// Feature satisfaction survey
class FeatureSurveyManager(private val context: Context) {
    
    data class FeatureSurvey(
        val featureId: String,
        val featureName: String,
        val questions: List<SurveyQuestion>
    )
    
    data class SurveyQuestion(
        val id: String,
        val text: String,
        val type: QuestionType,
        val options: List<String>? = null
    )
    
    enum class QuestionType {
        RATING,      // 1-5 stars
        NPS,         // 0-10
        SINGLE_CHOICE,
        MULTIPLE_CHOICE,
        FREE_TEXT
    }
    
    fun showFeatureSurvey(featureId: String, activity: Activity) {
        val survey = getFeatureSurvey(featureId)
        
        if (survey != null && shouldShowSurvey(featureId)) {
            displaySurvey(survey, activity)
        }
    }
    
    private fun getFeatureSurvey(featureId: String): FeatureSurvey? {
        // Return appropriate survey for feature
        return when (featureId) {
            "checkout" -> createCheckoutSurvey()
            "search" -> createSearchSurvey()
            "onboarding" -> createOnboardingSurvey()
            else -> null
        }
    }
    
    private fun createCheckoutSurvey(): FeatureSurvey {
        return FeatureSurvey(
            featureId = "checkout",
            featureName = "Checkout",
            questions = listOf(
                SurveyQuestion(
                    id = "ease_of_use",
                    text = "How easy was the checkout process?",
                    type = QuestionType.RATING
                ),
                SurveyQuestion(
                    id = "speed",
                    text = "How satisfied are you with the checkout speed?",
                    type = QuestionType.RATING
                ),
                SurveyQuestion(
                    id = "issues",
                    text = "Did you experience any issues?",
                    type = QuestionType.SINGLE_CHOICE,
                    options = listOf("No issues", "Payment issue", "UI issue", "Other")
                )
            )
        )
    }
    
    private fun createSearchSurvey(): FeatureSurvey {
        return FeatureSurvey(
            featureId = "search",
            featureName = "Search",
            questions = listOf(
                SurveyQuestion(
                    id = "relevance",
                    text = "Were the search results relevant?",
                    type = QuestionType.RATING
                ),
                SurveyQuestion(
                    id = "feedback",
                    text = "Any suggestions for improvement?",
                    type = QuestionType.FREE_TEXT
                )
            )
        )
    }
    
    private fun createOnboardingSurvey(): FeatureSurvey {
        return FeatureSurvey(
            featureId = "onboarding",
            featureName = "Onboarding",
            questions = listOf(
                SurveyQuestion(
                    id = "clarity",
                    text = "How clear was the onboarding process?",
                    type = QuestionType.RATING
                ),
                SurveyQuestion(
                    id = "nps",
                    text = "How likely are you to recommend this app based on your first impression?",
                    type = QuestionType.NPS
                )
            )
        )
    }
    
    private fun shouldShowSurvey(featureId: String): Boolean {
        // Check if user has used feature enough
        return true
    }
    
    private fun displaySurvey(survey: FeatureSurvey, activity: Activity) {
        // Display survey UI
    }
}
```

## Section 5: Feedback Management and Response

Managing feedback effectively requires organizing feedback, prioritizing issues, and responding to users when appropriate. A good feedback management system helps translate user input into actionable improvements.

```kotlin
// Feedback management and analysis
package com.example.myapp.feedback.management

import com.google.firebase.crashlytics.FirebaseCrashlytics
import com.google.firebase.analytics.FirebaseAnalytics

class FeedbackManager(private val context: Context) {
    
    private val analytics = FirebaseAnalytics.getInstance(context)
    private val crashlytics = FirebaseCrashlytics.getInstance()
    
    // Analyze and categorize incoming feedback
    fun analyzeFeedback(feedback: UserFeedback): FeedbackAnalysis {
        val priority = determinePriority(feedback)
        val rootCause = analyzeRootCause(feedback)
        val relatedIssues = findRelatedIssues(feedback)
        
        return FeedbackAnalysis(
            feedback = feedback,
            priority = priority,
            rootCause = rootCause,
            relatedIssues = relatedIssues,
            recommendedAction = determineAction(priority, feedback)
        )
    }
    
    private fun determinePriority(feedback: UserFeedback): IssuePriority {
        return when {
            // Critical issues have highest priority
            feedback.category == FeedbackCategory.CRASH -> IssuePriority.P0
            feedback.sentiment == Sentiment.NEGATIVE && 
            feedback.description.contains("crash", ignoreCase = true) -> IssuePriority.P0
            
            // High priority for performance issues
            feedback.category == FeedbackCategory.PERFORMANCE -> IssuePriority.P1
            
            // Medium priority for UI/UX feedback
            feedback.category == FeedbackCategory.UI_USABILITY -> IssuePriority.P2
            
            // Lower priority for feature requests
            feedback.feedbackType == FeedbackType.FEATURE_REQUEST -> IssuePriority.P3
            
            else -> IssuePriority.P2
        }
    }
    
    private fun analyzeRootCause(feedback: UserFeedback): RootCause? {
        // Simple pattern matching for common issues
        val description = feedback.description.lowercase()
        
        return when {
            description.contains("crash") || description.contains("force close") -> 
                RootCause("CRASH", "App crashes during specific action")
            
            description.contains("slow") || description.contains("lag") -> 
                RootCause("PERFORMANCE", "Performance issue identified")
            
            description.contains("not work") || description.contains("broken") -> 
                RootCause("FUNCTIONALITY", "Feature not functioning as expected")
            
            description.contains("confus") || description.contains("unclear") -> 
                RootCause("USABILITY", "User confusion with UI/flow")
            
            else -> null
        }
    }
    
    private fun findRelatedIssues(feedback: UserFeedback): List<RelatedIssue> {
        // In production, this would query a database of previous issues
        // For now, return empty list
        return emptyList()
    }
    
    private fun determineAction(
        priority: IssuePriority,
        feedback: UserFeedback
    ): RecommendedAction {
        return when (priority) {
            IssuePriority.P0 -> RecommendedAction.IMMEDIATE_FIX
            IssuePriority.P1 -> RecommendedAction.PRIORITY_FIX
            IssuePriority.P2 -> RecommendedAction.ROADMAP
            IssuePriority.P3 -> RecommendedAction.CONSIDER
        }
    }
    
    // Generate feedback summary for reporting
    fun generateFeedbackSummary(feedbackList: List<UserFeedback>): FeedbackSummary {
        val byCategory = feedbackList.groupBy { it.category }
        val bySentiment = feedbackList.groupBy { it.sentiment }
        
        val categoryBreakdown = byCategory.mapValues { it.value.size }
        val sentimentBreakdown = bySentiment.mapValues { it.value.size }
        
        val avgRating = feedbackList
            .filter { it.rating != null }
            .map { it.rating!! }
            .average()
            .let { if (it.isNaN()) null else it }
        
        val topIssues = feedbackList
            .filter { it.category == FeedbackCategory.CRASH || it.category == FeedbackCategory.PERFORMANCE }
            .groupBy { it.category }
            .mapValues { it.value.size }
            .toList()
            .sortedByDescending { it.second }
            .take(5)
        
        return FeedbackSummary(
            totalFeedback = feedbackList.size,
            categoryBreakdown = categoryBreakdown,
            sentimentBreakdown = sentimentBreakdown,
            averageRating = avgRating,
            topIssues = topIssues,
            periodStart = feedbackList.minOfOrNull { it.timestamp } ?: 0,
            periodEnd = feedbackList.maxOfOrNull { it.timestamp } ?: 0
        )
    }
}

enum class IssuePriority {
    P0,  // Critical - fix immediately
    P1,  // High - prioritize
    P2,  // Medium - plan for fix
    P3   // Low - consider
}

enum class RecommendedAction {
    IMMEDIATE_FIX,
    PRIORITY_FIX,
    ROADMAP,
    CONSIDER,
    WONT_FIX,
    NEEDS_REVIEW
}

data class RootCause(
    val category: String,
    val description: String
)

data class RelatedIssue(
    val id: String,
    val similarity: Double
)

data class FeedbackAnalysis(
    val feedback: UserFeedback,
    val priority: IssuePriority,
    val rootCause: RootCause?,
    val relatedIssues: List<RelatedIssue>,
    val recommendedAction: RecommendedAction
)

data class FeedbackSummary(
    val totalFeedback: Int,
    val categoryBreakdown: Map<FeedbackCategory?, Int>,
    val sentimentBreakdown: Map<Sentiment?, Int>,
    val averageRating: Double?,
    val topIssues: List<Pair<FeedbackCategory?, Int>>,
    val periodStart: Long,
    val periodEnd: Long
)
```

## Best Practices

- Use contextual prompts at moments of high engagement, not random times
- Make feedback forms short and easy to complete
- Follow up with users when their feedback leads to changes
- Prioritize feedback that affects user retention
- Use feedback categorization to identify trends
- Respond to App Store reviews to build community
- Close the feedback loop by communicating changes to users
- Combine feedback with analytics for complete user understanding
- Test feedback prompts with different user segments
- Respect user privacy - don't request unnecessary information

## Common Pitfalls

- **Feedback prompts appear too frequently**
  - Solution: Implement proper cooldown and eligibility checks
  
- **Users submit duplicate feedback**
  - Solution: Check for similar recent submissions before allowing new one
  
- **Feedback not categorized correctly**
  - Solution: Implement AI-assisted categorization or manual review
  
- **Low feedback response rates**
  - Solution: Make forms shorter, offer incentives, explain why feedback matters
  
- **Negative feedback without actionable details**
  - Solution: Include required fields and provide helpful prompts

## Troubleshooting Guide

**Q: Users not seeing feedback prompts**
A: Check eligibility criteria (days since install, sessions, engagement), verify prompt is not disabled.

**Q: Rating prompt shows but doesn't direct to Play Store**
A: Verify package name is correct, check Play Store availability on device.

**Q: Feedback analytics show no data**
A: Verify event logging is working, check that feedback submission is successful.

**Q: NPS responses are too few**
A: Ensure NPS survey appears at appropriate time, reduce survey friction.

## Advanced Tips

- Use ML-based sentiment analysis for large feedback volumes
- Implement automatic issue escalation for critical feedback
- Integrate feedback with product management tools
- Use A/B testing for feedback form optimization
- Build feedback widgets into specific user journeys

## Cross-References

- [Analytics Integration](./01_Analytics_Integration.md) - Analytics for feedback insights
- [Crash Reporting](./02_Crash_Reporting.md) - Bug report integration
- [Google Play Store](../01_App_Distribution/01_Google_Play_Store.md) - Review management
- [Marketing Implementation](./04_Marketing_Implementation.md) - Feedback-driven campaigns