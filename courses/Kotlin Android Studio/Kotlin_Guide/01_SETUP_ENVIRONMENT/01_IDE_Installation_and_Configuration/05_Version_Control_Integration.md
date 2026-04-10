# Version Control Integration

## Learning Objectives

1. Understanding version control fundamentals with Git
2. Integrating Git with Android Studio
3. Managing code branches and merging
4. Handling collaborative development workflows
5. Configuring Git for Android projects

## Section 1: Version Control Overview

Version control is essential for Android development:
- Track changes over time
- Collaborate with teams
- Manage different versions
- Maintain code history
- Enable parallel development

Git is the most widely used VCS for Android projects.

```kotlin
object VersionControlOverview {
    const val GIT_VERSION = "2.43.0"
    const val GITHUB_CLI_VERSION = "2.49.0"
    const val GIT_LFS_VERSION = "3.5.0"
}
```

## Section 2: Git Setup and Configuration

Initial Git configuration for Android development.

```kotlin
class GitSetup {
    private val gitConfig = mutableMapOf<String, String>()
    
    fun configureGit(): Map<String, String> {
        return mapOf(
            "user.name" to "Your Name",
            "user.email" to "your.email@example.com",
            "core.editor" to "code --wait",
            "core.autocrlf" to "input",
            "core.filemode" to "false",
            "core.longpaths" to "true",
            "alias.st" to "status",
            "alias.co" to "checkout",
            "alias.br" to "branch",
            "alias.ci" to "commit",
            "alias.df" to "diff",
            "alias.log" to "log --oneline --graph --all"
        )
    }
    
    fun configureGlobalIgnore(): String {
        return """
# Global .gitignore for Android development
# Download from: https://github.com/github/gitignore/blob/master/Android.gitignore

# Built application files
*.apk
*.ap_
*.aab

# Files for the ART/Dalvik VM
*.dex

# Java class files
*.class

# Generated files
bin/
gen/
out/
build/
target/

# Gradle files
.gradle/
build.gradle
gradle.properties

# Android Studio / IntelliJ
.idea/
*.iml

# Local configuration (sdk path, etc)
local.properties

# Log Files
*.log

# Android
proguard-rules.pro
*/proguard-rules.pro

# Keystore files
*.jks
*.keystore

# Signing configs
signing.properties

# External native builder, Native libc
CMakeLists.txt

# Pre-built native libraries
*.so

# OS-specific files
.DS_Store
Thumbs.db
        """.trimIndent()
    }
    
    fun initRepository(): String {
        return """# Initialize Git repository
git init
git add .
git commit -m "Initial commit"

# Create remote repository
git remote add origin https://github.com/username/repo.git
git push -u origin master"""
    }
}
```

## Section 3: Android Studio Git Integration

Android Studio provides built-in Git integration.

```kotlin
class AndroidStudioGitIntegration {
    
    enum class GitMenuOptions {
        COMMIT,
        PUSH,
        PULL,
        FETCH,
        BRANCH,
        MERGE,
        STASH,
        RESET,
        DIFF,
        LOG
    }
    
    fun getMenuActions(): Map<GitMenuOptions, String> {
        return mapOf(
            GitMenuOptions.COMMIT to "VCS > Git > Commit",
            GitMenuOptions.PUSH to "VCS > Git > Push",
            GitMenuOptions.PULL to "VCS > Git > Pull",
            GitMenuOptions.FETCH to "VCS > Git > Fetch",
            GitMenuOptions.BRANCH to "VCS > Git > Branches",
            GitMenuOptions.MERGE to "VCS > Git > Merge",
            GitMenuOptions.STASH to "VCS > Git > Stash Changes",
            GitMenuOptions.RESET to "VCS > Git > Reset HEAD",
            GitMenuOptions.DIFF to "VCS > Git > Compare Files",
            GitMenuOptions.LOG to "VCS > Git > Show History"
        )
    }
    
    fun enableVersionControl(): String {
        return """# Enable version control in Android Studio
# 1. VCS > Enable Version Control Integration
# 2. Select Git
# 3. Click OK

# Or use command line:
git init
# File > Settings > Version Control > Git > Enable"""
    }
}
```

## Section 4: Branch Management

Effective branching strategies for Android development.

```kotlin
class BranchManagement {
    
    enum class BranchStrategy(
        val branches: List<String>,
        val purpose: String
    ) {
        SIMPLE(listOf("main", "develop", "feature/*", "hotfix/*"), "Simple development"),
        GITFLOW(listOf("main", "develop", "feature/*", "release/*", "hotfix/*", "support/*"), "Git Flow workflow"),
        RELEASE(listOf("main", "production", "feature/*", "release/*"), "Release-based workflow")
    }
    
    fun getRecommendedStrategy(): BranchStrategy {
        return BranchStrategy.GITFLOW
    }
    
    fun createFeatureBranch(name: String): String {
        return """# Create new feature branch
git checkout -b feature/$name
# Or use Android Studio: VCS > Git > Branches > New Branch"""
    }
    
    fun mergeFeatureBranch(branchName: String): String {
        return """# Merge feature branch
git checkout develop
git merge feature/$branchName
git branch -d feature/$branchName"""
    }
    
    fun resolveMergeConflicts(): List<String> {
        return listOf(
            "Accept Current Change",
            "Accept Incoming Change",
            "Accept Both Changes",
            "Compare Changes"
        )
    }
}
```

## Section 5: GitHub and Remote Collaboration

Managing remote repositories and collaboration.

```kotlin
class RemoteCollaboration {
    
    enum class RemoteType {
        ORIGIN,      // Main remote
        UPSTREAM,    // Upstream repository
        OTHER        // Additional remotes
    }
    
    fun configureRemote(): String {
        return """# Add remote repositories
git remote add origin https://github.com/user/repo.git
git remote add upstream https://github.com/upstream/repo.git

# List remotes
git remote -v

# Fetch from remote
git fetch origin"""
    }
    
    fun createPullRequest(): String {
        return """# Create Pull Request workflow
# 1. Push your branch
git push -u origin feature/my-feature

# 2. Create PR on GitHub (web or CLI)
gh pr create --title "Feature Description" --body "PR Details"

# 3. Or use GitHub Desktop"""
    }
    
    fun forkWorkflow(): String {
        return """# Fork Workflow
# 1. Fork repository on GitHub

# 2. Clone your fork
git clone https://github.com/your-username/repo.git

# 3. Add upstream
git remote add upstream https://github.com/original/repo.git

# 4. Keep fork updated
git fetch upstream
git merge upstream/main"""
    }
}
```

## Common Pitfalls and Solutions

**Pitfall 1: Large files causing slow commits**
- Use Git LFS for large binary files
- Configure .gitignore properly
- Remove cached large files

**Pitfall 2: Merge conflicts in Gradle files**
- Use semantic merge for Gradle files
- Communicate with team members
- Review base version before merging

**Pitfall 3: Accidental commit to wrong branch**
- Use git reset to move commits
- Create new branch from commit
- Use git reflog to recover

**Pitfall 4: Git repository corruption**
- Clone fresh copy from remote
- Use git reflog
- Run git fsck

**Pitfall 5: Large repository size**
- Use Git LFS
- Shallow clone with depth
- Remove history with filter-branch

## Best Practices

1. Commit often with meaningful messages
2. Use .gitignore for generated files
3. Create feature branches for new features
4. Review code before committing
5. Use merge request workflow
6. Keep commits atomic
7. Write good commit messages
8. Sync with remote regularly
9. Use tags for releases
10. Backup remote repositories

## Troubleshooting Guide

**Issue: "refusing to merge unrelated histories"**
1. Use: git merge --allow-unrelated-histories
2. Or rebase instead

**Issue: "detached HEAD" state**
1. Create branch from current HEAD
2. Checkout a branch to save changes
3. Commit changes

**Issue: "failed to push some refs"**
1. Pull first: git pull --rebase
2. Resolve any conflicts
3. Push again

## Advanced Tips and Tricks

**Tip 1: Use Git hooks**
- Pre-commit hooks for linting
- Pre-push hooks for testing
- Commit message validation

**Tip 2: Use bisect for debugging**
- Binary search for bugs
- Automate with script

**Tip 3: Use worktree for multiple branches**
- Work on multiple branches
- Faster switching

**Tip 4: Use stash for temporary changes**
- Save incomplete work
- Switch branches quickly

**Tip 5: Configure Git aliases**
- Faster command line
- Custom commands

## Example 1: Android Project Git Workflow

```kotlin
class AndroidProjectWorkflow {
    fun initializeProject(): Unit {
        println("Starting Android project setup...")
        println("Step 1: Initializing Git repository...")
        println("Step 2: Creating .gitignore...")
        println("Step 3: Creating initial commit...")
        println("Step 4: Adding remote...")
        println("Step 5: Creating develop branch...")
        println("Android project ready for development!")
    }
    
    fun dailyWorkflow(): String {
        return """Daily Development Workflow:
1. git checkout develop
2. git pull origin develop
3. git checkout -b feature/my-feature
4. // Make changes
5. git add .
6. git commit -m "Description"
7. git push -u origin feature/my-feature
8. Create Pull Request"""
    }
    
    fun releaseWorkflow(): String {
        return """Release Workflow:
1. git checkout develop
2. git pull origin develop
3. git checkout -b release/v1.0.0
4. // Update version and build
5. git commit -m "Release v1.0.0"
6. git checkout main
7. git merge release/v1.0.0
8. git tag v1.0.0
9. git push origin main --tags"""
    }
}
```

## Example 2: Team Collaboration Workflow

```kotlin
class TeamCollaborationWorkflow {
    fun onboarding(): String {
        return """Team Onboarding:
1. Clone repository: git clone <repo-url>
2. Add remote: git remote add upstream <upstream-url>
3. Configure identity
4. Install pre-commit hooks
5. Read contribution guidelines"""
    }
    
    fun contributeChange(): String {
        return """Contribution Process:
1. Sync with upstream: git fetch upstream && git merge upstream/develop
2. Create feature branch from develop
3. Make changes following code style
4. Run tests locally
5. Create commit withConventional Commits message
6. Push to fork
7. Create Pull Request
8. Address review feedback
9. Merge after approval"""
    }
    
    fun codeReviewProcess(): String {
        return """Code Review Process:
1. Reviewer receives PR notification
2. Review code changes
3. Add comments for improvements
4. Approve or request changes
5. Make final decisions"""
    }
}
```

## Example 3: GitHub Actions Integration

```kotlin
class GitHubActionsIntegration {
    fun getWorkflowFile(): String {
        return """name: Android CI

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Set up JDK 17
      uses: actions/setup-java@v3
      with:
        java-version: '17'
        distribution: 'temurin'
    
    - name: Cache Gradle packages
      uses: actions/cache@v3
      with:
        path: ~/.gradle/caches
        key: \${{ runner.os }}-gradle-\${{ hashFiles('**/*.gradle*') }}
    
    - name: Grant execute permission for gradlew
      run: chmod +x gradlew
    
    - name: Build with Gradle
      run: ./gradlew build
    
    - name: Run tests
      run: ./gradlew test
        """.trimIndent()
    }
    
    fun createRelease(): String {
        return """# Create Release with GitHub Actions
# 1. Tag release: git tag v1.0.0
# 2. Push tag: git push origin v1.0.0
# 3. Actions builds and creates APK
# 4. Download from Actions artifacts"""
    }
}
```

## Output Statement Results

Version Control Configuration Complete:
- Git Version: 2.43.0
- Initialized Git repository
- Configured .gitignore
- Set up Git LFS

Branch Structure:
- main (production)
- develop (development)
- feature/* (features)
- release/* (releases)
- hotfix/* (hotfixes)

Team Workflows:
- Fork and clone
- Feature branches
- Pull requests
- Code reviews
- CI/CD integration

GitHub Actions:
- CI pipeline configured
- Build checks
- Test execution
- APK generation

## Cross-References

See: 01_IDE_Installation_and_Configuration/01_Android_Studio_Setup.md
See: 01_IDE_Installation_and_Configuration/04_Gradle_Configuration.md
See: 07_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 10_DEPLOYMENT/01_App_Distribution/04_Release_Management.md
