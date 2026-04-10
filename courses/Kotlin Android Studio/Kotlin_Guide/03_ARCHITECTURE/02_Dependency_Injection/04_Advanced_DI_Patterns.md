# Advanced DI Patterns

## Learning Objectives

1. Understanding advanced DI patterns
2. Using scopes and custom scopes
3. Testing with DI
4. Multi-module DI setup

## Advanced Patterns

```kotlin
object AdvancedDIPatterns {
    
    // Custom scope
    @javax.inject.Scope
    @Retention(androidx.annotation.AnnotationRetention.RUNTIME)
    annotation class ActivityScope
    
    // Activity-scoped component
    @ActivityScope
    @dagger.Component
    interface ActivityComponent {
        fun inject(activity: MainActivity)
    }
    
    // Subcomponents
    @dagger.Subcomponent
    interface ViewModelComponent {
        fun viewModel(): MyViewModel
    }
    
    // Multi-module setup
    @dagger.Module(subcomponents = [ViewModelComponent::class])
    class ViewModelModule
}
```