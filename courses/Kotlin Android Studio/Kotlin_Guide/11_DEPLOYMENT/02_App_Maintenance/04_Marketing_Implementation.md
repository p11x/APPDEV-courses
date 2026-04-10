# Marketing Implementation

## Learning Objectives

1. Understanding mobile app marketing fundamentals
2. Implementing deep linking for marketing campaigns
3. Setting up Firebase Dynamic Links
4. Configuring app indexing and search
5. Implementing referral tracking
6. Measuring marketing campaign effectiveness

## Prerequisites

- [Analytics Integration](./01_Analytics_Integration.md)
- [Google Play Store](../01_App_Distribution/01_Google_Play_Store.md)

## Section 1: Mobile Marketing Fundamentals

Mobile app marketing encompasses all activities that drive user acquisition, engagement, and retention. Understanding the marketing funnel for apps helps you design effective campaigns that convert users from awareness to loyal users.

The mobile marketing funnel consists of several stages. Discovery is when users find your app through search, ads, or referrals. Installation is when users download and install your app. Activation is when users complete onboarding and start using your core features. Engagement is when users regularly use the app. Retention is when users continue using the app over time. Referral is when users recommend your app to others.

Each stage requires different marketing tactics and metrics. Discoverability involves ASO (App Store Optimization), paid advertising, and organic search. Installation uses install campaigns, deep links, and promotional offers. Activation requires smooth onboarding and clear value proposition. Engagement uses push notifications, in-app messages, and personalized content. Retention involves loyalty programs, rewards, and regular updates. Referral uses referral programs and viral features.

```kotlin
// Marketing campaign tracking models
package com.example.myapp.marketing

import android.os.Bundle

data class Campaign(
    val id: String,
    val name: String,
    val source: String,
    val medium: String,
    val content: String,
    val term: String,
    val customParameters: Map<String, String>
)

data class AttributionData(
    val campaignId: String?,
    val source: String?,
    val medium: String?,
    val term: String?,
    val content: String?,
    val clickTimestamp: Long?,
    val installTimestamp: Long?
)

data class ConversionEvent(
    val eventName: String,
    val timestamp: Long,
    val conversionValue: Double,
    val currency: String,
    val campaignData: AttributionData?
)

// Campaign attribution manager
class CampaignAttributionManager {
    
    fun trackInstall(attributionData: AttributionData) {
        // Store attribution data for future conversion attribution
        storeAttributionData(attributionData)
        
        // Log install event
        logInstallEvent(attributionData)
    }
    
    fun trackConversion(conversionEvent: ConversionEvent) {
        // Associate with stored attribution data
        val attribution = getStoredAttribution()
        
        // Send to analytics with attribution data
        logConversionWithAttribution(conversionEvent, attribution)
    }
    
    private fun storeAttributionData(data: AttributionData) {
        // Store in SharedPreferences or local database
    }
    
    private fun getStoredAttribution(): AttributionData? {
        // Retrieve stored attribution
        return null
    }
    
    private fun logInstallEvent(attribution: AttributionData) {
        // Send to analytics
    }
    
    private fun logConversionWithAttribution(
        event: ConversionEvent,
        attribution: AttributionData?
    ) {
        // Send conversion with attribution to analytics
    }
}
```

## Section 2: Deep Linking Implementation

Deep links allow you to direct users to specific content within your app from external sources like websites, emails, or other apps. Implementing deep links properly is essential for marketing campaign tracking and user experience.

Android deep linking can be implemented in three ways: using custom URL schemes, using App Links (verified deep links), or using Firebase Dynamic Links. Each has different characteristics and use cases.

```kotlin
// Deep link handling implementation
package com.example.myapp.deeplink

import android.content.Context
import android.content.Intent
import android.net.Uri
import androidx.navigation.NavController
import com.google.firebase.dynamiclinks.FirebaseDynamicLinks
import com.google.firebase.dynamiclinks.PendingDynamicLinkData

class DeepLinkManager(private val context: Context) {
    
    private val dynamicLinks = FirebaseDynamicLinks.getInstance()
    
    // Handle incoming deep links
    fun handleDeepLink(uri: Uri, navController: NavController?) {
        when {
            uri.scheme == "myapp" -> {
                // Custom URL scheme: myapp://product/123
                handleCustomScheme(uri)
            }
            uri.host == "example.com" && uri.pathSegments.isNotEmpty() -> {
                // App Links: https://example.com/product/123
                handleAppLink(uri)
            }
            else -> {
                // Check if it's a Firebase Dynamic Link
                handleFirebaseDynamicLink(uri)
            }
        }
    }
    
    private fun handleCustomScheme(uri: Uri) {
        val path = uri.path ?: return
        val params = parseQueryParams(uri)
        
        when {
            path.startsWith("/product") -> {
                val productId = pathSegments.lastOrNull()
                productId?.let { navigateToProduct(it, params) }
            }
            path.startsWith("/category") -> {
                val categoryId = pathSegments.lastOrNull()
                categoryId?.let { navigateToCategory(it, params) }
            }
            path.startsWith("/promo") -> {
                val promoId = params["code"]
                promoId?.let { navigateToPromo(it, params) }
            }
            path.startsWith("/profile") -> {
                navigateToProfile(params)
            }
        }
    }
    
    private fun handleAppLink(uri: Uri) {
        val pathSegments = uri.pathSegments
        val params = parseQueryParams(uri)
        
        when (pathSegments.firstOrNull()) {
            "product" -> {
                val productId = pathSegments.getOrNull(1)
                productId?.let { navigateToProduct(it, params) }
            }
            "category" -> {
                val categoryId = pathSegments.getOrNull(1)
                categoryId?.let { navigateToCategory(it, params) }
            }
            "article" -> {
                val articleId = pathSegments.getOrNull(1)
                articleId?.let { navigateToArticle(it, params) }
            }
            "checkout" -> {
                navigateToCheckout(params)
            }
        }
    }
    
    private fun handleFirebaseDynamicLink(uri: Uri) {
        dynamicLinks.getDynamicLink(uri)
            .addOnSuccessListener { pendingDynamicLinkData ->
                pendingDynamicLinkData?.let {
                    val deepLink = it.link
                    handleDeepLink(deepLink, null)
                    
                    // Track dynamic link click
                    trackDynamicLinkClick(it)
                }
            }
            .addOnFailureListener { e ->
                // Handle failure
            }
    }
    
    private fun parseQueryParams(uri: Uri): Map<String, String> {
        val params = mutableMapOf<String, String>()
        uri.queryParameterNames.forEach { name ->
            uri.getQueryParameter(name)?.let { value ->
                params[name] = value
            }
        }
        return params
    }
    
    private fun navigateToProduct(productId: String, params: Map<String, String>) {
        // Navigate using Navigation component
        val intent = Intent(Intent.ACTION_VIEW).apply {
            data = Uri.parse("myapp://product/$productId")
        }
        context.startActivity(intent)
    }
    
    private fun navigateToCategory(categoryId: String, params: Map<String, String>) {
        // Navigate to category
    }
    
    private fun navigateToPromo(promoId: String, params: Map<String, String>) {
        // Navigate to promo with code
    }
    
    private fun navigateToProfile(params: Map<String, String>) {
        // Navigate to profile
    }
    
    private fun navigateToArticle(articleId: String, params: Map<String, String>) {
        // Navigate to article
    }
    
    private fun navigateToCheckout(params: Map<String, String>) {
        // Navigate to checkout
    }
    
    private fun trackDynamicLinkClick(data: PendingDynamicLinkData) {
        // Log dynamic link click for analytics
    }
    
    // Handle link when app is opened from killed state
    fun getInitialDynamicLink(): android.content.Intent? {
        val pendingLink = FirebaseDynamicLinks.getInstance()
            .getDynamicLink(context.intent)
        
        // Process pending link
        return null
    }
}

// Deep link route definitions
sealed class DeepLinkRoute(val path: String) {
    object Product : DeepLinkRoute("product/{productId}")
    object Category : DeepLinkRoute("category/{categoryId}")
    object Article : DeepLinkRoute("article/{articleId}")
    object Promo : DeepLinkRoute("promo/{promoCode}")
    object Profile : DeepLinkRoute("profile")
    object Checkout : DeepLinkRoute("checkout")
    
    companion object {
        fun fromUri(uri: Uri): DeepLinkRoute? {
            val path = uri.path ?: return null
            val segments = path.split("/").filter { it.isNotEmpty() }
            
            return when (segments.firstOrNull()) {
                "product" -> Product
                "category" -> Category
                "article" -> Article
                "promo" -> Promo
                "profile" -> Profile
                "checkout" -> Checkout
                else -> null
            }
        }
    }
}
```

## Section 3: Firebase Dynamic Links

Firebase Dynamic Links are smart URLs that work whether or not your app is installed. They intelligently direct users to the appropriate destination based on their device and platform, making them perfect for marketing campaigns.

```kotlin
// Firebase Dynamic Links creation and handling
package com.example.myapp.dynamiclinks

import android.content.Context
import android.net.Uri
import com.google.firebase.dynamiclinks.DynamicLink
import com.google.firebase.dynamiclinks.FirebaseDynamicLinks
import com.google.firebase.dynamiclinks.ShortDynamicLink

class DynamicLinkService(private val context: Context) {
    
    private val dynamicLinks = FirebaseDynamicLinks.getInstance()
    private val domain = "example.app.goo.gl"  // Your Dynamic Links domain
    
    // Create a dynamic link for a product
    fun createProductLink(
        productId: String,
        productName: String,
        productImageUrl: String,
        campaignSource: String,
        campaignMedium: String
    ): Uri {
        val baseUri = Uri.parse("https://$domain/product/$productId")
        
        return dynamicLinks.createDynamicLink()
            .setLink(baseUri)
            .setDomainUriPrefix(domain)
            .setAndroidParams(
                DynamicLink.AndroidParameters.Builder("com.example.myapp")
                    .setMinimumVersion(1)
                    .setFallbackUrl(Uri.parse("https://play.google.com/store/apps/details?id=com.example.myapp"))
                    .build()
            )
            .setSocialMetaTagParameters(
                DynamicLink.SocialMetaTagParameters.Builder()
                    .setTitle("Check out $productName")
                    .setDescription("View this product in our app")
                    .setImageUrl(Uri.parse(productImageUrl))
                    .build()
            )
            .setGoogleAnalyticsParams(
                DynamicLink.GoogleAnalyticsParameters.Builder()
                    .setSource(campaignSource)
                    .setMedium(campaignMedium)
                    .setCampaign("product_promo")
                    .build()
            )
            .buildDynamicLink()
            .uri
    }
    
    // Create a short dynamic link for sharing
    suspend fun createShortLink(
        longUri: Uri,
        socialMetaTag: SocialMetaTag? = null
    ): ShortLinkResult {
        return try {
            val shortLinkTask = dynamicLinks.createDynamicLink()
                .setLongLink(longUri)
                .buildShortLinkAsync()
            
            suspendCoroutine { continuation ->
                shortLinkTask.addOnSuccessListener { shortLink ->
                    continuation.resume(ShortLinkResult(
                        success = true,
                        shortLink = shortLink.shortLink,
                        previewLink = shortLink.previewLink,
                        warnings = shortLink.warnings ?: emptyList()
                    ))
                }
                
                shortLinkTask.addOnFailureListener { exception ->
                    continuation.resume(ShortLinkResult(
                        success = false,
                        error = exception.message
                    ))
                }
            }
        } catch (e: Exception) {
            ShortLinkResult(success = false, error = e.message)
        }
    }
    
    // Create referral link for user referral programs
    fun createReferralLink(userId: String, referrerName: String): Uri {
        val baseUri = Uri.parse("https://$domain/invite/$userId")
        
        return dynamicLinks.createDynamicLink()
            .setLink(baseUri)
            .setDomainUriPrefix(domain)
            .setAndroidParams(
                DynamicLink.AndroidParameters.Builder("com.example.myapp")
                    .setMinimumVersion(10)
                    .build()
            )
            .setSocialMetaTagParameters(
                DynamicLink.SocialMetaTagParameters.Builder()
                    .setTitle("$referrerName invited you!")
                    .setDescription("Join me on this app and get a bonus!")
                    .build()
            )
            .setIosParams(
                DynamicLink.IosParameters.Builder("com.example.myapp")
                    .setMinimumVersion("1.0.0")
                    .setAppStoreId("123456789")
                    .setFallbackUrl(Uri.parse("https://apps.apple.com/app/id123456789"))
                    .build()
            )
            .buildDynamicLink()
            .uri
    }
    
    // Handle incoming dynamic link
    fun handleDynamicLink() {
        FirebaseDynamicLinks.getInstance()
            .getDynamicLink(context.intent)
            .addOnSuccessListener { pendingDynamicLinkData ->
                pendingDynamicLinkData?.let { data ->
                    processDynamicLink(data.link, data)
                }
            }
            .addOnFailureListener { exception ->
                // Handle failure
            }
    }
    
    private fun processDynamicLink(
        link: Uri,
        data: PendingDynamicLinkData
    ) {
        // Get attribution data
        val clickTimestamp = data.clickTimestamp
        val minimumAppVersion = data.minimumAppVersion
        
        // Navigate to appropriate content
        navigateFromDeepLink(link)
        
        // Track for analytics
        trackDynamicLinkConversion(link, data)
    }
    
    private fun navigateFromDeepLink(link: Uri) {
        // Use Navigation component to navigate
    }
    
    private fun trackDynamicLinkConversion(
        link: Uri,
        data: PendingDynamicLinkData
    ) {
        // Log conversion event
    }
}

data class ShortLinkResult(
    val success: Boolean,
    val shortLink: Uri? = null,
    val previewLink: Uri? = null,
    val warnings: List<String> = emptyList(),
    val error: String? = null
)

data class SocialMetaTag(
    val title: String,
    val description: String,
    val imageUrl: String? = null
)
```

## Section 4: App Indexing and Search

App indexing allows your app's content to appear in Google search results, driving organic discovery and downloads. Proper implementation helps users find your app content directly from search.

```kotlin
// App indexing implementation
package com.example.myapp.indexing

import android.content.Context
import android.content.Intent
import android.net.Uri
import com.google.android.gms.appindexing.Action
import com.google.android.gms.appindexing.AppIndex
import com.google.android.gms.appindexing Thing

class AppIndexer(private val context: Context) {
    
    private val appIndex = AppIndex.getAppIndex()
    private val appUri = Uri.parse("android-app://${context.packageName}")
    
    // Index a product for search
    fun indexProduct(
        productId: String,
        productName: String,
        productDescription: String,
        productUrl: String
    ) {
        val uri = Uri.parse("android-app://${context.packageName}/product/$productId")
        
        val thing = Thing.Builder()
            .setId(productId)
            .setName(productName)
            .setDescription(productDescription)
            .setUrl(Uri.parse(productUrl))
            .build()
        
        val action = Action.Builder(Action.TYPE_VIEW)
            .setObject(thing)
            .setActionStatus(Action.STATUS_TYPE_COMPLETED)
            .build()
        
        appIndex.start(
            getActionToken(action),
            context.packageName
        )
    }
    
    // Index an article
    fun indexArticle(
        articleId: String,
        title: String,
        description: String,
        publishedDate: String
    ) {
        val thing = Thing.Builder()
            .setId(articleId)
            .setName(title)
            .setDescription(description)
            .build()
        
        val action = Action.Builder(Action.TYPE_VIEW)
            .setObject(thing)
            .setActionStatus(Action.STATUS_TYPE_COMPLETED)
            .build()
        
        appIndex.start(
            getActionToken(action),
            context.packageName
        )
    }
    
    // Batch index multiple items
    fun indexContent(items: List<IndexableContent>) {
        val actions = items.map { item ->
            createIndexableAction(item)
        }
        
        // Submit batch
        appIndex.start(
            getBatchActionToken(actions),
            context.packageName
        )
    }
    
    // Remove content from index
    fun removeFromIndex(contentId: String) {
        // Use delete method to remove indexed content
    }
    
    // Clear all indexed content
    fun clearIndex() {
        appIndex.remove(getActionToken(null), context.packageName)
    }
    
    private fun getActionToken(action: Action?): com.google.android.gms.common.api.PendingResult<*>? {
        // Get action token
        return null
    }
    
    private fun getBatchActionToken(actions: List<Action>): com.google.android.gms.common.api.PendingResult<*>? {
        return null
    }
    
    private fun createIndexableAction(content: IndexableContent): Action {
        val thing = Thing.Builder()
            .setId(content.id)
            .setName(content.title)
            .setDescription(content.description)
            .setUrl(content.url)
            .build()
        
        return Action.Builder(Action.TYPE_VIEW)
            .setObject(thing)
            .setActionStatus(Action.STATUS_TYPE_COMPLETED)
            .build()
    }
}

sealed class IndexableContent {
    abstract val id: String
    abstract val title: String
    abstract val description: String
    abstract val url: String
    
    data class Product(
        override val id: String,
        override val title: String,
        override val description: String,
        override val url: String,
        val price: Double,
        val imageUrl: String
    ) : IndexableContent()
    
    data class Article(
        override val id: String,
        override val title: String,
        override val description: String,
        override val url: String,
        val author: String,
        val publishedDate: Long
    ) : IndexableContent()
}

// App Link verification
class AppLinkVerifier(private val context: Context) {
    
    fun verifyAppLinks(): AppLinkVerification {
        // Android App Links are automatically verified through
        // Digital Asset Links file hosted on your website
        
        // To verify programmatically:
        // 1. Check if links are verified
        // 2. Get verification status
        
        return AppLinkVerification(
            linksVerified = true,
            host = "example.com",
            verifiedPaths = listOf("/product/", "/article/", "/category/")
        )
    }
}

data class AppLinkVerification(
    val linksVerified: Boolean,
    val host: String,
    val verifiedPaths: List<String>
)
```

## Section 5: Marketing Attribution and Analytics

Measuring marketing campaign effectiveness requires proper attribution tracking to understand which channels drive installs, engagement, and conversions.

```kotlin
// Marketing attribution tracking
package com.example.myapp.marketing.attribution

import android.content.Context
import android.os.Bundle
import com.google.firebase.analytics.FirebaseAnalytics
import com.google.firebase.analytics.logEvent

class MarketingAttribution(private val context: Context) {
    
    private val analytics = FirebaseAnalytics.getInstance(context)
    
    // Track installation source attribution
    fun trackInstallAttribution(
        campaign: String?,
        source: String?,
        medium: String?,
        term: String?,
        content: String?
    ) {
        val params = Bundle().apply {
            campaign?.let { putString("campaign", it) }
            source?.let { putString("source", it) }
            medium?.let { putString("medium", it) }
            term?.let { putString("term", it) }
            content?.let { putString("content", it) }
        }
        
        analytics.logEvent("install_attribution", params)
    }
    
    // Track conversion events
    fun trackConversion(
        eventName: String,
        value: Double,
        currency: String,
        campaignData: Map<String, String>?
    ) {
        val params = Bundle().apply {
            putDouble("value", value)
            putString("currency", currency)
            
            campaignData?.forEach { (key, value) ->
                putString(key, value)
            }
        }
        
        analytics.logEvent(eventName, params)
    }
    
    // Track referral events
    fun trackReferral(referrerId: String, newUserId: String) {
        val params = Bundle().apply {
            putString("referrer_id", referrerId)
            putString("new_user_id", newUserId)
            putString("conversion_type", "referral")
        }
        
        analytics.logEvent("referral_conversion", params)
    }
    
    // Set user properties for segmentation
    fun setAttributionUserProperties(
        installSource: String,
        installCampaign: String,
        installDate: Long
    ) {
        analytics.setUserProperty("install_source", installSource)
        analytics.setUserProperty("install_campaign", installCampaign)
        analytics.setUserProperty("install_date", installDate.toString())
    }
}

// Marketing channel definitions
object MarketingChannels {
    const val ORGANIC = "organic"
    const val PAID_SEARCH = "paid_search"
    const val PAID_SOCIAL = "paid_social"
    const val DISPLAY = "display"
    const val EMAIL = "email"
    const val REFERRAL = "referral"
    const val DIRECT = "direct"
    const val ORGANIC_SOCIAL = "organic_social"
    const val AFFILIATE = "affiliate"
}

// Campaign tracking utilities
class CampaignTracker {
    
    data class CampaignMetrics(
        val campaignName: String,
        val channel: String,
        val impressions: Int,
        val clicks: Int,
        val installs: Int,
        val conversions: Int,
        val revenue: Double,
        val cost: Double,
        val roas: Double,
        val cpi: Double,
        val conversionRate: Double
    )
    
    fun calculateCampaignMetrics(
        impressions: Int,
        clicks: Int,
        installs: Int,
        conversions: Int,
        revenue: Double,
        cost: Double
    ): CampaignMetrics {
        val cpi = if (installs > 0) cost / installs else 0.0
        val ctr = if (impressions > 0) (clicks.toDouble() / impressions) * 100 else 0.0
        val conversionRate = if (installs > 0) (conversions.toDouble() / installs) * 100 else 0.0
        val roas = if (cost > 0) revenue / cost else 0.0
        
        return CampaignMetrics(
            campaignName = "",
            channel = "",
            impressions = impressions,
            clicks = clicks,
            installs = installs,
            conversions = conversions,
            revenue = revenue,
            cost = cost,
            roas = roas,
            cpi = cpi,
            conversionRate = conversionRate
        )
    }
}
```

## Best Practices

- Use UTM parameters consistently across all marketing campaigns
- Implement deep links for all marketing touchpoints
- Test deep links on actual devices before launching campaigns
- Use Firebase Dynamic Links for cross-platform sharing
- Implement proper attribution for both installs and in-app conversions
- Track referral program performance with unique referral codes
- Set up regular reporting for marketing channel performance
- Use server-side attribution for better accuracy
- A/B test marketing messages and creative assets
- Monitor fraud and invalid traffic in campaigns

## Common Pitfalls

- **Deep links not working on iOS but working on Android**
  - Solution: Verify iOS URI scheme configuration in Firebase Console
  
- **Attribution data lost when app is reinstalled**
  - Solution: Store attribution in backend tied to device ID
  
- **Campaign tracking shows incorrect source**
  - Solution: Check that UTM parameters are being captured correctly in all flows
  
- **App Links not verified**
  - Solution: Verify Digital Asset Links file is correctly hosted
  
- **Dynamic link opens in browser instead of app**
  - Solution: Check Dynamic Link configuration and minimum app version

## Troubleshooting Guide

**Q: Deep link doesn't open the app**
A: Verify intent filter is configured correctly in AndroidManifest.xml, check scheme matches.

**Q: Campaign source shows as "(not set)"**
A: Ensure UTM parameters are being passed correctly in the install referrer.

**Q: Firebase Dynamic Links not creating short links**
A: Check quota limits and ensure domain is properly configured.

**Q: App indexing not showing in Google results**
A: App indexing requires content to be publicly accessible and properly structured.

## Advanced Tips

- Use server-to-server conversion tracking for better attribution accuracy
- ImplementSKAdNetwork for iOS attribution
- Use deferred deep links to route users to specific content after install
- Implement cross-platform attribution for unified campaign tracking

## Cross-References

- [Analytics Integration](./01_Analytics_Integration.md) - Analytics and attribution
- [Google Play Store](../01_App_Distribution/01_Google_Play_Store.md) - Store listing optimization
- [User Feedback Systems](./05_User_Feedback_Systems.md) - In-app promotion and feedback
- [Firebase App Distribution](../01_App_Distribution/02_Firebase_App_Distribution.md) - Pre-launch testing