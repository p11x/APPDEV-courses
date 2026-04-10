# Flow Navigation

## Learning Objectives

1. Understanding flow navigation patterns
2. Implementing linear navigation flows
3. Managing flow state with navigation
4. Creating multi-step forms
5. Implementing wizard-style navigation
6. Handling flow completion and cancellation
7. Preserving flow state across navigation
8. Building guided user journeys

## Section 1: Flow Navigation Overview

Flow navigation guides users through a sequence of screens in a specific order. It's ideal for multi-step processes, wizards, and guided experiences.

```kotlin
/**
 * Flow Navigation Overview
 * 
 * Flow navigation is used when:
 * - Multi-step processes (checkout, onboarding)
 * - Guided user journeys
 * - Sequential screens that must be followed
 * - Forms with multiple steps
 * 
 * Flow characteristics:
 * - Linear progression
 * - Forward and backward navigation
 * - State preservation within flow
 * - Flow completion handling
 */
object FlowNavigationOverview {
    
    // Common flow types
    object FlowTypes {
        // Checkout flow
        const val CHECKOUT = "checkout"
        // cart -> shipping -> payment -> confirmation
        
        // Onboarding flow
        const val ONBOARDING = "onboarding"
        // welcome -> features -> permissions -> complete
        
        // Registration flow
        const val REGISTRATION = "registration"
        // email -> profile -> preferences -> verify
        
        // Settings flow
        const val SETTINGS = "settings"
        // category -> setting -> configure -> save
    }
    
    // Flow state
    object FlowState {
        // Flow is in progress
        const val IN_PROGRESS = "in_progress"
        
        // Flow completed successfully
        const val COMPLETED = "completed"
        
        // Flow was cancelled
        const val CANCELLED = "cancelled"
        
        // Flow failed at some step
        const val FAILED = "failed"
    }
}
```

## Section 2: Linear Navigation Flows

Implementing linear flows where users progress step by step through the navigation.

```kotlin
/**
 * Linear Navigation Flows
 * 
 * Linear flows:
 * - Sequential steps
 * - Step indicator
 * - Progress tracking
 * - Back navigation
 */
class LinearNavigationFlows {
    
    // Step definition
    data class FlowStep(
        val stepNumber: Int,
        val title: String,
        val route: String
    )
    
    // Checkout flow steps
    object CheckoutFlow {
        val steps = listOf(
            FlowStep(1, "Cart", "cart"),
            FlowStep(2, "Shipping", "shipping"),
            FlowStep(3, "Payment", "payment"),
            FlowStep(4, "Review", "review"),
            FlowStep(5, "Confirmation", "confirmation")
        )
        
        fun getStepIndex(route: String): Int {
            return steps.indexOfFirst { it.route == route }
        }
        
        fun canProceed(currentRoute: String): Boolean {
            val index = getStepIndex(currentRoute)
            return index in 0 until steps.size - 1
        }
        
        fun canGoBack(currentRoute: String): Boolean {
            return getStepIndex(currentRoute) > 0
        }
    }
    
    // Checkout flow in Navigation Compose
    @Composable
    fun CheckoutFlowNavigation() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Track current step
        var currentStep by androidx.compose.runtime.remember { 
            androidx.compose.runtime.mutableIntStateOf(0) 
        }
        
        // Flow start destination
        val startDestination = CheckoutFlow.steps.first().route
        
        // NavHost with nested navigation
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = startDestination
        ) {
            // Cart step
            androidx.navigation.compose.composable(CheckoutFlow.steps[0].route) {
                CartScreen(
                    step = 1,
                    totalSteps = CheckoutFlow.steps.size,
                    onProceed = {
                        currentStep = 1
                        navController.navigate(CheckoutFlow.steps[1].route)
                    },
                    onCancel = {
                        // Cancel flow and go back
                        navController.popBackStack("home", false)
                    }
                )
            }
            
            // Shipping step
            androidx.navigation.compose.composable(CheckoutFlow.steps[1].route) {
                ShippingScreen(
                    step = 2,
                    totalSteps = CheckoutFlow.steps.size,
                    onProceed = {
                        currentStep = 2
                        navController.navigate(CheckoutFlow.steps[2].route)
                    },
                    onBack = {
                        currentStep = 0
                        navController.popBackStack()
                    }
                )
            }
            
            // Payment step
            androidx.navigation.compose.composable(CheckoutFlow.steps[2].route) {
                PaymentScreen(
                    step = 3,
                    totalSteps = CheckoutFlow.steps.size,
                    onProceed = {
                        currentStep = 3
                        navController.navigate(CheckoutFlow.steps[3].route)
                    },
                    onBack = {
                        currentStep = 1
                        navController.popBackStack()
                    }
                )
            }
            
            // Review step
            androidx.navigation.compose.composable(CheckoutFlow.steps[3].route) {
                ReviewScreen(
                    step = 4,
                    totalSteps = CheckoutFlow.steps.size,
                    onPlaceOrder = {
                        currentStep = 4
                        navController.navigate(CheckoutFlow.steps[4].route)
                    },
                    onBack = {
                        currentStep = 2
                        navController.popBackStack()
                    }
                )
            }
            
            // Confirmation step
            androidx.navigation.compose.composable(CheckoutFlow.steps[4].route) {
                ConfirmationScreen(
                    orderId = "12345",
                    onDone = {
                        // Clear flow and go to home
                        navController.navigate("home") {
                            popUpTo("home") { inclusive = true }
                        }
                    }
                )
            }
        }
    }
    
    // Cart screen
    @Composable
    fun CartScreen(
        step: Int,
        totalSteps: Int,
        onProceed: () -> Unit,
        onCancel: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally
        ) {
            // Step indicator
            StepIndicator(currentStep = step, totalSteps = totalSteps)
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            
            androidx.compose.material3.Text(
                "Cart",
                style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
            )
            
            androidx.compose.material3.Text("Your cart items will appear here")
            
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            
            androidx.compose.material3.Button(onClick = onProceed) {
                androidx.compose.material3.Text("Proceed to Shipping")
            }
            
            androidx.compose.material3.TextButton(onClick = onCancel) {
                androidx.compose.material3.Text("Cancel")
            }
        }
    }
    
    // Shipping screen
    @Composable
    fun ShippingScreen(
        step: Int,
        totalSteps: Int,
        onProceed: () -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            StepIndicator(currentStep = step, totalSteps = totalSteps)
            
            androidx.compose.material3.Text(
                "Shipping Address",
                style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            
            androidx.compose.material3.OutlinedTextField(
                value = "",
                onValueChange = { },
                label = { androidx.compose.material3.Text("Name") },
                modifier = Modifier.fillMaxWidth()
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            
            androidx.compose.material3.OutlinedTextField(
                value = "",
                onValueChange = { },
                label = { androidx.compose.material3.Text("Address") },
                modifier = Modifier.fillMaxWidth()
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            
            androidx.compose.material3.Button(onClick = onProceed) {
                androidx.compose.material3.Text("Continue to Payment")
            }
            
            androidx.compose.material3.TextButton(onClick = onBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    // Payment screen
    @Composable
    fun PaymentScreen(
        step: Int,
        totalSteps: Int,
        onProceed: () -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            StepIndicator(currentStep = step, totalSteps = totalSteps)
            androidx.compose.material3.Text("Payment", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.Button(onClick = onProceed) { androidx.compose.material3.Text("Review Order") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    // Review screen
    @Composable
    fun ReviewScreen(
        step: Int,
        totalSteps: Int,
        onPlaceOrder: () -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            StepIndicator(currentStep = step, totalSteps = totalSteps)
            androidx.compose.material3.Text("Review Order", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.Button(onClick = onPlaceOrder) { androidx.compose.material3.Text("Place Order") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    // Confirmation screen
    @Composable
    fun ConfirmationScreen(
        orderId: String,
        onDone: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text(
                "Order Confirmed!",
                style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
            )
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Text("Order ID: ${orderId}")
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            androidx.compose.material3.Button(onClick = onDone) {
                androidx.compose.material3.Text("Done")
            }
        }
    }
    
    // Step indicator composable
    @Composable
    fun StepIndicator(currentStep: Int, totalSteps: Int) {
        androidx.compose.foundation.layout.Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = androidx.compose.foundation.layout.Arrangement.SpaceEvenly
        ) {
            for (i in 1..totalSteps) {
                val isCompleted = i < currentStep
                val isCurrent = i == currentStep
                
                androidx.compose.foundation.layout.Column(
                    horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally
                ) {
                    // Step circle
                    androidx.compose.foundation.layout.size(32.dp),
                    androidx.compose.foundation.background(
                        when {
                            isCompleted -> androidx.compose.material3.MaterialTheme.colorScheme.primary
                            isCurrent -> androidx.compose.material3.MaterialTheme.colorScheme.primaryContainer
                            else -> androidx.compose.material3.MaterialTheme.colorScheme.surfaceVariant
                        },
                        shape = androidx.compose.foundation.shape.CircleShape
                    ),
                    androidx.compose.foundation.layout.Box(
                        contentAlignment = androidx.compose.ui.Alignment.Center
                    ) {
                        if (isCompleted) {
                            androidx.compose.material3.Icon(
                                androidx.compose.material.icons.Icons.Default.Check,
                                contentDescription = "Completed",
                                tint = androidx.compose.material3.MaterialTheme.colorScheme.onPrimary,
                                modifier = Modifier.size(16.dp)
                            )
                        } else {
                            androidx.compose.material3.Text(
                                i.toString(),
                                style = androidx.compose.material3.MaterialTheme.typography.labelMedium
                            )
                        }
                    }
                }
            }
        }
    }
}
```

## Section 3: Flow State Management

Managing state across flow steps including data persistence and completion handling.

```kotlin
/**
 * Flow State Management
 * 
 * Managing flow state:
 * - ViewModel for flow data
 * - State preservation
 * - Flow completion handling
 * - Error recovery
 */
class FlowStateManagement {
    
    // Checkout flow ViewModel
    class CheckoutViewModel : androidx.lifecycle.ViewModel() {
        
        // Cart items
        private val _cartItems = androidx.lifecycle.MutableLiveData<List<CartItem>>(emptyList())
        val cartItems: androidx.lifecycle.LiveData<List<CartItem>> = _cartItems
        
        // Shipping info
        private val _shippingInfo = androidx.lifecycle.MutableLiveData<ShippingInfo?>(null)
        val shippingInfo: androidx.lifecycle.LiveData<ShippingInfo?> = _shippingInfo
        
        // Payment info
        private val _paymentInfo = androidx.lifecycle.MutableLiveData<PaymentInfo?>(null)
        val paymentInfo: androidx.lifecycle.LiveData<PaymentInfo?> = _paymentInfo
        
        // Flow state
        private val _flowState = androidx.lifecycle.MutableLiveData<FlowState>(FlowState.IN_PROGRESS)
        val flowState: androidx.lifecycle.LiveData<FlowState> = _flowState
        
        // Current step
        private val _currentStep = androidx.lifecycle.MutableLiveData(0)
        val currentStep: androidx.lifecycle.LiveData<Int> = _currentStep
        
        fun updateCartItems(items: List<CartItem>) {
            _cartItems.value = items
        }
        
        fun updateShippingInfo(info: ShippingInfo) {
            _shippingInfo.value = info
            _currentStep.value = 1
        }
        
        fun updatePaymentInfo(info: PaymentInfo) {
            _paymentInfo.value = info
            _currentStep.value = 2
        }
        
        fun completeFlow(orderId: String) {
            _flowState.value = FlowState.COMPLETED
            // Clear cart or perform completion actions
        }
        
        fun cancelFlow() {
            _flowState.value = FlowState.CANCELLED
            // Cleanup or navigate away
        }
        
        fun canGoToStep(step: Int): Boolean {
            return when (step) {
                0 -> true // Cart - always accessible
                1 -> _cartItems.value?.isNotEmpty() == true // Shipping - needs cart
                2 -> _shippingInfo.value != null // Payment - needs shipping
                3 -> _paymentInfo.value != null // Review - needs payment
                else -> false
            }
        }
    }
    
    // Data classes
    data class CartItem(val id: String, val name: String, val price: Double, val quantity: Int)
    data class ShippingInfo(val name: String, val address: String, val city: String, val zipCode: String)
    data class PaymentInfo(val cardNumber: String, val expiry: String, val cvv: String)
    
    // Flow state enum
    enum class FlowState {
        IN_PROGRESS, COMPLETED, CANCELLED, FAILED
    }
    
    // Flow with ViewModel
    @Composable
    fun CheckoutFlowWithViewModel(
        viewModel: CheckoutViewModel = androidx.lifecycle.viewmodel.compose.viewModel()
    ) {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Observe flow state
        val flowState by viewModel.flowState.observeAsState(FlowState.IN_PROGRESS)
        val currentStep by viewModel.currentStep.observeAsState(0)
        
        // Handle flow completion
        androidx.compose.runtime.LaunchedEffect(flowState) {
            when (flowState) {
                FlowState.COMPLETED -> {
                    // Navigate to success or home
                    navController.navigate("home") {
                        popUpTo("home") { inclusive = true }
                    }
                }
                FlowState.CANCELLED -> {
                    // Navigate away
                    navController.popBackStack("home", false)
                }
                else -> { /* Continue flow */ }
            }
        }
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "cart"
        ) {
            // Cart
            androidx.navigation.compose.composable("cart") {
                CartScreen(
                    items = viewModel.cartItems.value ?: emptyList(),
                    onProceed = { navController.navigate("shipping") },
                    onCancel = { viewModel.cancelFlow() }
                )
            }
            
            // Shipping
            androidx.navigation.compose.composable("shipping") {
                ShippingScreen(
                    onSubmit = { info ->
                        viewModel.updateShippingInfo(info)
                        navController.navigate("payment")
                    },
                    onBack = { navController.popBackStack() }
                )
            }
            
            // Payment
            androidx.navigation.compose.composable("payment") {
                PaymentScreen(
                    onSubmit = { info ->
                        viewModel.updatePaymentInfo(info)
                        navController.navigate("review")
                    },
                    onBack = { navController.popBackStack() }
                )
            }
            
            // Review
            androidx.navigation.compose.composable("review") {
                ReviewScreen(
                    cartItems = viewModel.cartItems.value ?: emptyList(),
                    shippingInfo = viewModel.shippingInfo.value,
                    paymentInfo = viewModel.paymentInfo.value,
                    onPlaceOrder = {
                        viewModel.completeFlow("ORDER-123")
                    },
                    onBack = { navController.popBackStack() }
                )
            }
        }
    }
    
    @Composable
    fun CartScreen(
        items: List<CartItem>,
        onProceed: () -> Unit,
        onCancel: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize().padding(16.dp)) {
            androidx.compose.material3.Text("Cart: ${items.size} items")
            androidx.compose.material3.Button(onClick = onProceed) { androidx.compose.material3.Text("Continue") }
            androidx.compose.material3.TextButton(onClick = onCancel) { androidx.compose.material3.Text("Cancel") }
        }
    }
    
    @Composable
    fun ShippingScreen(
        onSubmit: (ShippingInfo) -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize().padding(16.dp)) {
            androidx.compose.material3.Text("Shipping")
            androidx.compose.material3.Button(onClick = { onSubmit(ShippingInfo("Test", "123 St", "City", "12345")) }) { androidx.compose.material3.Text("Continue") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun PaymentScreen(
        onSubmit: (PaymentInfo) -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize().padding(16.dp)) {
            androidx.compose.material3.Text("Payment")
            androidx.compose.material3.Button(onClick = { onSubmit(PaymentInfo("123456789", "12/25", "123")) }) { androidx.compose.material3.Text("Continue") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun ReviewScreen(
        cartItems: List<CartItem>,
        shippingInfo: ShippingInfo?,
        paymentInfo: PaymentInfo?,
        onPlaceOrder: () -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize().padding(16.dp)) {
            androidx.compose.material3.Text("Review")
            androidx.compose.material3.Button(onClick = onPlaceOrder) { androidx.compose.material3.Text("Place Order") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
}
```

## Section 4: Guided User Journeys

Creating guided experiences that walk users through specific tasks with contextual help and validation.

```kotlin
/**
 * Guided User Journeys
 * 
 * Guided journeys:
 * - Onboarding experiences
 * - First-time setup
 * - Tutorial flows
 * - Feature discovery
 */
class GuidedUserJourneys {
    
    // Onboarding flow
    @Composable
    fun OnboardingFlow() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Pager for swipeable onboarding
        val pagerState = androidx.compose.foundation.pager.rememberPagerState(pageCount = { 4 })
        
        // Onboarding pages
        val pages = listOf(
            OnboardingPage(
                title = "Welcome",
                description = "Welcome to our app! Let's show you around.",
                icon = androidx.compose.material.icons.Icons.Default.Home
            ),
            OnboardingPage(
                title = "Features",
                description = "Discover all the amazing features we offer.",
                icon = androidx.compose.material.icons.Icons.Default.Star
            ),
            OnboardingPage(
                title = "Notifications",
                description = "Stay updated with important notifications.",
                icon = androidx.compose.material.icons.Icons.Default.Notifications
            ),
            OnboardingPage(
                title = "Get Started",
                description = "You're all set! Let's get started.",
                icon = androidx.compose.material.icons.Icons.Default.CheckCircle
            )
        )
        
        // Column layout
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Skip button
            androidx.compose.foundation.layout.Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp),
                horizontalArrangement = androidx.compose.foundation.layout.Arrangement.End
            ) {
                androidx.compose.material3.TextButton(
                    onClick = {
                        // Complete onboarding
                        navController.navigate("main") {
                            popUpTo("onboarding") { inclusive = true }
                        }
                    }
                ) {
                    androidx.compose.material3.Text("Skip")
                }
            }
            
            // Page content
            androidx.compose.foundation.pager.HorizontalPager(
                state = pagerState,
                modifier = Modifier
                    .weight(1f)
                    .fillMaxWidth()
            ) { page ->
                OnboardingPageContent(page = pages[page])
            }
            
            // Page indicators
            androidx.compose.foundation.layout.Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp),
                horizontalArrangement = androidx.compose.foundation.layout.Arrangement.Center
            ) {
                repeat(pages.size) { index ->
                    val isSelected = pagerState.currentPage == index
                    androidx.compose.foundation.layout.size(8.dp),
                    androidx.compose.foundation.background(
                        if (isSelected) androidx.compose.material3.MaterialTheme.colorScheme.primary
                        else androidx.compose.material3.MaterialTheme.colorScheme.outline,
                        shape = androidx.compose.foundation.shape.CircleShape
                    ),
                    androidx.compose.foundation.layout.Spacer(modifier = Modifier.width(4.dp))
                }
            }
            
            // Navigation buttons
            androidx.compose.foundation.layout.Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp),
                horizontalArrangement = androidx.compose.foundation.layout.Arrangement.SpaceBetween
            ) {
                // Back button (only show if not on first page)
                if (pagerState.currentPage > 0) {
                    androidx.compose.material3.TextButton(
                        onClick = {
                            androidx.compose.runtime.LaunchedEffect(Unit) {
                                pagerState.animateScrollToPage(pagerState.currentPage - 1)
                            }
                        }
                    ) {
                        androidx.compose.material3.Text("Back")
                    }
                } else {
                    // Spacer for alignment
                    androidx.compose.foundation.layout.Spacer(modifier = Modifier.width(1.dp))
                }
                
                // Next/Get Started button
                androidx.compose.material3.Button(
                    onClick = {
                        if (pagerState.currentPage < pages.size - 1) {
                            androidx.compose.runtime.LaunchedEffect(Unit) {
                                pagerState.animateScrollToPage(pagerState.currentPage + 1)
                            }
                        } else {
                            // Complete onboarding
                            navController.navigate("main") {
                                popUpTo("onboarding") { inclusive = true }
                            }
                        }
                    }
                ) {
                    androidx.compose.material3.Text(
                        if (pagerState.currentPage < pages.size - 1) "Next" else "Get Started"
                    )
                }
            }
        }
    }
    
    // Onboarding page data
    data class OnboardingPage(
        val title: String,
        val description: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    )
    
    @Composable
    fun OnboardingPageContent(page: OnboardingPage) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(32.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            // Icon
            androidx.compose.material3.Icon(
                page.icon,
                contentDescription = page.title,
                modifier = Modifier.size(120.dp),
                tint = androidx.compose.material3.MaterialTheme.colorScheme.primary
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(32.dp))
            
            // Title
            androidx.compose.material3.Text(
                page.title,
                style = androidx.compose.material3.MaterialTheme.typography.headlineLarge,
                textAlign = androidx.compose.ui.text.style.TextAlign.Center
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            
            // Description
            androidx.compose.material3.Text(
                page.description,
                style = androidx.compose.material3.MaterialTheme.typography.bodyLarge,
                textAlign = androidx.compose.ui.text.style.TextAlign.Center,
                color = androidx.compose.material3.MaterialTheme.colorScheme.onSurfaceVariant
            )
        }
    }
    
    // Multi-step setup flow
    @Composable
    fun SetupFlow() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Track completed steps
        var completedSteps by androidx.compose.runtime.remember { 
            androidx.compose.runtime.mutableStateOf(setOf<String>()) 
        }
        
        // Steps in setup
        val steps = listOf(
            SetupStep("profile", "Profile Setup", "Set up your profile"),
            SetupStep("preferences", "Preferences", "Customize your experience"),
            SetupStep("notifications", "Notifications", "Configure notifications"),
            SetupStep("security", "Security", "Secure your account")
        )
        
        // Vertical pager for step-by-step
        val pagerState = androidx.compose.foundation.pager.rememberPagerState(pageCount = { steps.size })
        
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Progress
            androidx.compose.material3.LinearProgressIndicator(
                progress = { (pagerState.currentPage + 1).toFloat() / steps.size },
                modifier = Modifier.fillMaxWidth()
            )
            
            // Steps list
            androidx.compose.foundation.layout.Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp),
                horizontalArrangement = androidx.compose.foundation.layout.Arrangement.SpaceEvenly
            ) {
                steps.forEachIndexed { index, step ->
                    val isCompleted = step.route in completedSteps
                    val isCurrent = index == pagerState.currentPage
                    
                    androidx.compose.foundation.layout.Column(
                        horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally
                    ) {
                        // Step indicator
                        androidx.compose.foundation.layout.size(24.dp),
                        androidx.compose.foundation.background(
                            when {
                                isCompleted -> androidx.compose.material3.MaterialTheme.colorScheme.primary
                                isCurrent -> androidx.compose.material3.MaterialTheme.colorScheme.primaryContainer
                                else -> androidx.compose.material3.MaterialTheme.colorScheme.surfaceVariant
                            },
                            shape = androidx.compose.foundation.shape.CircleShape
                        ),
                        androidx.compose.foundation.layout.Box(contentAlignment = androidx.compose.ui.Alignment.Center) {
                            androidx.compose.material3.Text(
                                if (isCompleted) "✓" else "${index + 1}",
                                style = androidx.compose.material3.MaterialTheme.typography.labelSmall
                            )
                        }
                        
                        androidx.compose.material3.Spacer(modifier = Modifier.height(4.dp))
                        
                        androidx.compose.material3.Text(
                            step.title,
                            style = androidx.compose.material3.MaterialTheme.typography.labelSmall
                        )
                    }
                }
            }
            
            // Step content
            androidx.compose.foundation.pager.VerticalPager(
                state = pagerState,
                modifier = Modifier
                    .weight(1f)
                    .fillMaxWidth()
            ) { page ->
                SetupStepContent(
                    step = steps[page],
                    onComplete = { stepRoute ->
                        completedSteps = completedSteps + stepRoute
                        if (page < steps.size - 1) {
                            androidx.compose.runtime.LaunchedEffect(Unit) {
                                pagerState.animateScrollToPage(page + 1)
                            }
                        } else {
                            // Complete setup
                            navController.navigate("main") {
                                popUpTo("setup") { inclusive = true }
                            }
                        }
                    },
                    onSkip = {
                        if (page < steps.size - 1) {
                            androidx.compose.runtime.LaunchedEffect(Unit) {
                                pagerState.animateScrollToPage(page + 1)
                            }
                        }
                    }
                )
            }
        }
    }
    
    data class SetupStep(
        val route: String,
        val title: String,
        val description: String
    )
    
    @Composable
    fun SetupStepContent(
        step: SetupStep,
        onComplete: (String) -> Unit,
        onSkip: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text(
                step.title,
                style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            
            androidx.compose.material3.Text(
                step.description,
                style = androidx.compose.material3.MaterialTheme.typography.bodyMedium
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            
            androidx.compose.material3.Button(onClick = { onComplete(step.route) }) {
                androidx.compose.material3.Text("Continue")
            }
            
            androidx.compose.material3.TextButton(onClick = onSkip) {
                androidx.compose.material3.Text("Skip")
            }
        }
    }
}
```

## Example: Complete Flow Navigation App

Full implementation with complete flow navigation system.

```kotlin
/**
 * Complete Flow Navigation App
 * 
 * Full implementation showing:
 * - Complete checkout flow
 * - Step indicators
 * - State management
 * - Completion handling
 */
class CompleteFlowNavigation {
    
    // Routes
    object Routes {
        const val CART = "cart"
        const val SHIPPING = "shipping"
        const val PAYMENT = "payment"
        const val REVIEW = "review"
        const val CONFIRMATION = "confirmation"
    }
    
    // Flow steps
    data class CheckoutStep(
        val route: String,
        val title: String,
        val number: Int
    )
    
    // Main checkout flow
    @Composable
    fun CheckoutFlow() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Flow ViewModel
        val viewModel: CheckoutFlowViewModel = androidx.lifecycle.viewmodel.compose.viewModel()
        
        // Steps
        val steps = listOf(
            CheckoutStep(Routes.CART, "Cart", 1),
            CheckoutStep(Routes.SHIPPING, "Shipping", 2),
            CheckoutStep(Routes.PAYMENT, "Payment", 3),
            CheckoutStep(Routes.REVIEW, "Review", 4),
            CheckoutStep(Routes.CONFIRMATION, "Done", 5)
        )
        
        // Current route
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        // Current step index
        val currentStepIndex = steps.indexOfFirst { it.route == currentRoute }.coerceAtLeast(0)
        
        // Column layout
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Top app bar
            androidx.compose.material3.TopAppBar(
                title = { 
                    androidx.compose.material3.Text("Checkout - Step ${currentStepIndex + 1}/${steps.size}") 
                },
                navigationIcon = {
                    if (currentStepIndex > 0 && currentRoute != Routes.CONFIRMATION) {
                        androidx.compose.material3.IconButton(onClick = { navController.popBackStack() }) {
                            androidx.compose.material3.Icon(
                                androidx.compose.material.icons.Icons.Default.ArrowBack,
                                contentDescription = "Back"
                            )
                        }
                    }
                }
            )
            
            // Step indicator
            StepProgressIndicator(
                steps = steps.map { it.title },
                currentStep = currentStepIndex
            )
            
            // NavHost
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = Routes.CART,
                modifier = Modifier.weight(1f)
            ) {
                // Cart
                androidx.navigation.compose.composable(Routes.CART) {
                    CartStepContent(
                        itemCount = viewModel.cartItemCount,
                        total = viewModel.cartTotal,
                        onProceed = { navController.navigate(Routes.SHIPPING) },
                        onCancel = { /* Handle cancel */ }
                    )
                }
                
                // Shipping
                androidx.navigation.compose.composable(Routes.SHIPPING) {
                    ShippingStepContent(
                        shippingInfo = viewModel.shippingInfo,
                        onSubmit = { info ->
                            viewModel.updateShippingInfo(info)
                            navController.navigate(Routes.PAYMENT)
                        },
                        onBack = { navController.popBackStack() }
                    )
                }
                
                // Payment
                androidx.navigation.compose.composable(Routes.PAYMENT) {
                    PaymentStepContent(
                        paymentInfo = viewModel.paymentInfo,
                        onSubmit = { info ->
                            viewModel.updatePaymentInfo(info)
                            navController.navigate(Routes.REVIEW)
                        },
                        onBack = { navController.popBackStack() }
                    )
                }
                
                // Review
                androidx.navigation.compose.composable(Routes.REVIEW) {
                    ReviewStepContent(
                        cartItems = viewModel.cartItems,
                        shippingInfo = viewModel.shippingInfo,
                        paymentInfo = viewModel.paymentInfo,
                        onPlaceOrder = {
                            viewModel.placeOrder()
                            navController.navigate(Routes.CONFIRMATION)
                        },
                        onBack = { navController.popBackStack() }
                    )
                }
                
                // Confirmation
                androidx.navigation.compose.composable(Routes.CONFIRMATION) {
                    ConfirmationStepContent(
                        orderId = viewModel.orderId,
                        onDone = {
                            navController.navigate("home") {
                                popUpTo("home") { inclusive = true }
                            }
                        }
                    )
                }
            }
        }
    }
    
    // Step progress indicator
    @Composable
    fun StepProgressIndicator(
        steps: List<String>,
        currentStep: Int
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp)
        ) {
            // Progress bar
            androidx.compose.material3.LinearProgressIndicator(
                progress = { (currentStep + 1).toFloat() / steps.size },
                modifier = Modifier.fillMaxWidth()
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            
            // Step labels
            androidx.compose.foundation.layout.Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = androidx.compose.foundation.layout.Arrangement.SpaceBetween
            ) {
                steps.forEachIndexed { index, title ->
                    androidx.compose.material3.Text(
                        title,
                        style = androidx.compose.material3.MaterialTheme.typography.labelSmall,
                        color = when {
                            index <= currentStep -> androidx.compose.material3.MaterialTheme.colorScheme.primary
                            else -> androidx.compose.material3.MaterialTheme.colorScheme.outline
                        }
                    )
                }
            }
        }
    }
    
    @Composable
    fun CartStepContent(
        itemCount: Int,
        total: Double,
        onProceed: () -> Unit,
        onCancel: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Your Cart", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Text("${itemCount} items - Total: $${String.format("%.2f", total)}")
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            androidx.compose.material3.Button(onClick = onProceed) { androidx.compose.material3.Text("Continue to Shipping") }
            androidx.compose.material3.TextButton(onClick = onCancel) { androidx.compose.material3.Text("Cancel") }
        }
    }
    
    @Composable
    fun ShippingStepContent(
        shippingInfo: ShippingData?,
        onSubmit: (ShippingData) -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Shipping Address", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.OutlinedTextField(value = "", onValueChange = {}, label = { androidx.compose.material3.Text("Name") }, modifier = Modifier.fillMaxWidth())
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            androidx.compose.material3.OutlinedTextField(value = "", onValueChange = {}, label = { androidx.compose.material3.Text("Address") }, modifier = Modifier.fillMaxWidth())
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.Button(onClick = { onSubmit(ShippingData("Test", "123 St", "City", "12345")) }) { androidx.compose.material3.Text("Continue to Payment") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun PaymentStepContent(
        paymentInfo: PaymentData?,
        onSubmit: (PaymentData) -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize().padding(16.dp)) {
            androidx.compose.material3.Text("Payment Method", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.Button(onClick = { onSubmit(PaymentData("1234567890", "12/25", "123")) }) { androidx.compose.material3.Text("Review Order") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun ReviewStepContent(
        cartItems: List<CartItemData>,
        shippingInfo: ShippingData?,
        paymentInfo: PaymentData?,
        onPlaceOrder: () -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize().padding(16.dp)) {
            androidx.compose.material3.Text("Review Your Order", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.Button(onClick = onPlaceOrder) { androidx.compose.material3.Text("Place Order") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun ConfirmationStepContent(
        orderId: String,
        onDone: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Icon(
                androidx.compose.material.icons.Icons.Default.CheckCircle,
                contentDescription = "Success",
                modifier = Modifier.size(80.dp),
                tint = androidx.compose.material3.MaterialTheme.colorScheme.primary
            )
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            androidx.compose.material3.Text("Order Placed!", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            androidx.compose.material3.Text("Order ID: ${orderId}")
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            androidx.compose.material3.Button(onClick = onDone) { androidx.compose.material3.Text("Continue Shopping") }
        }
    }
    
    // ViewModel
    class CheckoutFlowViewModel : androidx.lifecycle.ViewModel() {
        var cartItemCount by androidx.compose.runtime.remember { androidx.compose.runtime.mutableIntStateOf(3) }
        var cartTotal by androidx.compose.runtime.remember { androidx.compose.runtime.mutableDoubleStateOf(99.99) }
        var shippingInfo by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf<ShippingData?>(null) }
        var paymentInfo by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf<PaymentData?>(null) }
        val cartItems by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(listOf<CartItemData>()) }
        var orderId by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf("") }
        
        fun updateShippingInfo(info: ShippingData) { shippingInfo = info }
        fun updatePaymentInfo(info: PaymentData) { paymentInfo = info }
        fun placeOrder() { orderId = "ORD-${System.currentTimeMillis()}" }
    }
    
    data class ShippingData(val name: String, val address: String, val city: String, val zip: String)
    data class PaymentData(val cardNumber: String, val expiry: String, val cvv: String)
    data class CartItemData(val name: String, val price: Double, val quantity: Int)
}
```

## Output Statement Results

**Flow Navigation Types:**
- Checkout: Cart -> Shipping -> Payment -> Review -> Confirmation
- Onboarding: Welcome -> Features -> Setup -> Complete
- Registration: Email -> Profile -> Verify -> Complete
- Setup: Step 1 -> Step 2 -> Step 3 -> Done

**Flow Components:**
- Step indicator: Visual progress
- Navigation: Forward/backward flow
- State preservation: ViewModel holding flow data
- Completion: Handle flow end

**State Management:**
- ViewModel: Flow-wide state
- LiveData/StateFlow: Reactive updates
- SavedStateHandle: State preservation
- Flow completion: Handled in UI layer

**Best Practices:**
- Clear step indicators: Show progress
- Back navigation: Allow going back
- State preservation: Keep data across steps
- Completion handling: Handle flow end

## Cross-References

- See: [01_Jetpack_Navigation_Basics.md](../01_Navigation_Architecture/01_Jetpack_Navigation_Basics.md)
- See: [02_Navigation_Compose.md](../01_Navigation_Architecture/02_Navigation_Compose.md)
- See: [01_Bottom_Navigation.md](./01_Bottom_Navigation.md)
- See: [05_Custom_Navigation.md](./05_Custom_Navigation.md)

## End of Flow Navigation Guide