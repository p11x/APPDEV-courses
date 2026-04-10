# Test Reporting

## Learning Objectives

1. Understanding test reporting fundamentals
2. Generating comprehensive test reports
3. Using code coverage tools
4. Creating custom test reports
5. Analyzing test results
6. Implementing test metrics

## Prerequisites

- Test execution knowledge
- Gradle build system
- Coverage tools basics

## Core Concepts

### Test Reporting Overview

Test reporting provides insights into test execution:
- **Test results**: Pass/fail/skip status
- **Execution time**: How long tests took
- **Code coverage**: What code was tested
- **Metrics**: Test quality indicators

### Coverage Tools

- **JaCoCo**: Java Code Coverage
- **Emma**: Coverage tool (legacy)
- **Cobertura**: Line/branch coverage

## Code Examples

### Standard Example: JaCoCo Configuration

```groovy
// build.gradle

plugins {
    id 'jacoco'
}

jacoco {
    toolVersion = "0.8.7"
}

task jacocoTestReport(type: JacocoReport) {
    dependsOn 'testDebugUnitTest'
    
    reports {
        xml.enabled = true
        html.enabled = true
        csv.enabled = false
        
        xml.destination file("${buildDir}/reports/jacoco/jacoco.xml")
        html.destination file("${buildDir}/reports/jacoco/html")
    }
    
    doFirst {
        classDirectories = fileTree('app/build/intermediates/javac/debug/classes')
    }
    
    sourceDirectories = files('app/src/main/java')
    executionData = fileTree('app/build/test-results/')
}

task jacocoTestVerification(type: JacocoCoverageVerification) {
    dependsOn 'jacocoTestReport'
    
    violationRules {
        rule {
            element = 'CLASS'
            
            limit {
                counter = 'LINE'
                minimum = 0.75
            }
        }
        
        rule {
            element = 'METHOD'
            
            limit {
                counter = 'METHOD'
                minimum = 0.80
            }
        }
        
        rule {
            element = 'BRANCH'
            
            limit {
                counter = 'BRANCH'
                minimum = 0.70
            }
        }
    }
}
```

### Real-World Example: Custom Test Reports

```kotlin
import org.junit.platform.launcher.*
import org.junit.platform.launcher.listeners.*
import java.text.SimpleDateFormat

class CustomTestReport : TestExecutionListener {
    
    private val results = mutableListOf<TestResult>()
    private var startTime: Long = 0
    
    override fun testPlanExecutionStarted(testPlan: TestPlan) {
        startTime = System.currentTimeMillis()
        println("Test execution started: ${testPlan.tests}")
    }
    
    override fun testPlanExecutionFinished(testPlan: TestPlan, result: TestExecutionResult) {
        val duration = System.currentTimeMillis() - startTime
        
        val passed = results.count { it.status == TestResult.Status.PASSED }
        val failed = results.count { it.status == TestResult.Status.FAILED }
        val skipped = results.count { it.status == TestResult.Status.SKIPPED }
        
        println("""
            Test Execution Report
            ==================
            Total Tests: ${results.size}
            Passed: $passed
            Failed: $failed
            Skipped: $skipped
            Duration: ${duration}ms
            
            Pass Rate: ${(passed * 100) / results.size}%
        """.trimIndent())
        
        saveReport(results, duration)
    }
    
    override fun executionFinished(testIdentifier: TestIdentifier, result: TestExecutionResult) {
        val testResult = TestResult(
            testId = testIdentifier.uniqueId,
            name = testIdentifier.displayName,
            status = when (result.status) {
                TestExecutionResult.Status.SUCCESSFUL -> TestResult.Status.PASSED
                TestExecutionResult.Status.FAILED -> TestResult.Status.FAILED
                TestExecutionResult.Status.SKIPPED -> TestResult.Status.SKIPPED
            },
            duration = System.currentTimeMillis() - startTime
        )
        
        results.add(testResult)
        
        if (result.status == TestExecutionResult.Status.FAILED) {
            result.exception?.let {
                println("FAILED: ${testIdentifier.displayName}")
                println("Error: ${it.message}")
            }
        }
    }
    
    private fun saveReport(results: List<TestResult>, duration: Long) {
        val reportFile = File("test-report-${System.currentTimeMillis()}.json")
        
        val json = buildJson(results, duration)
        reportFile.writeText(json)
    }
    
    private fun buildJson(results: List<TestResult>, duration: Long): String {
        return """
            {
                "timestamp": "${SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(Date())}",
                "duration": $duration,
                "results": ${results.map { it.toJson() }}
            }
        """.trimIndent()
    }
}
```

### Real-World Example: Coverage Analysis

```kotlin
import org.jacoco.core.analysis.*
import org.jacoco.core.data.*
import java.io.*

class CoverageAnalysis {
    
    fun analyzeCoverage(report: CoverageReport) {
        // Total coverage
        val totalInstruction = report.instructionCounter.covered
        val totalInstructionMissed = report.instructionCounter.missed
        val instructionCoverage = totalInstruction.toDouble() / 
            (totalInstruction + totalInstructionMissed)
        
        // Line coverage
        val totalLine = report.lineCounter.covered
        val totalLineMissed = report.lineCounter.missed
        val lineCoverage = totalLine.toDouble() / (totalLine + totalLineMissed)
        
        // Branch coverage
        val totalBranch = report.branchCounter.covered
        val totalBranchMissed = report.branchCounter.missed
        val branchCoverage = totalBranch.toDouble() / 
            (totalBranch + totalBranchMissed)
        
        // Method coverage
        val methodCovered = report.methodCounter.covered
        val methodMissed = report.methodCounter.missed
        val methodCoverage = methodCovered.toDouble() / (methodCovered + methodMissed)
        
        // Class coverage
        val classCovered = report.classCounter.covered
        val classMissed = report.classCounter.missed
        val classCoverage = classCovered.toDouble() / (classCovered + classMissed)
        
        print("""
            Coverage Analysis
            ==============
            Instructions: ${(instructionCoverage * 100).toInt()}%
            Lines: ${(lineCoverage * 100).toInt()}%
            Branches: ${(branchCoverage * 100).toInt()}%
            Methods: ${(methodCoverage * 100).toInt()}%
            Classes: ${(classCoverage * 100).toInt()}%
        """.trimIndent())
    }
    
    fun findUncoveredCode(executionData: ExecutionDataStore): List<String> {
        val uncoveredMethods = mutableListOf<String>()
        
        for ((classPath, classData) in executionData) {
            for ((methodId, _) in classData.methodHits) {
                if (methodId.hits == 0) {
                    uncoveredMethods.add("${classPath.name}.${methodId.name}")
                }
            }
        }
        
        return uncoveredMethods
    }
    
    fun generateHtmlReport(
        executionData: ExecutionDataStore,
        sourceFiles: Map<String, File>
    ): String {
        val analyzer = CoverageAnalyzer(executionData)
        
        val coverage = analyzer.analyze(sourceFiles)
        
        val html = StringBuilder()
        html.append("<html><body>")
        
        for ((source, coverageData) in coverage) {
            html.append("<h3>$source</h3>")
            html.append("<pre>")
            
            for ((lineNum, lineHits) in coverageData.lines) {
                val prefix = if (lineHits > 0) "✓" else "✗"
                html.append("$prefix $lineNum: ${coverageData.getSource(lineNum)}")
            }
            
            html.append("</pre>")
        }
        
        html.append("</body></html>")
        
        return html.toString()
    }
}
```

### Real-World Example: Test Metrics Dashboard

```kotlin
class TestMetricsDashboard {
    
    data class TestMetrics(
        val totalTests: Int,
        val passed: Int,
        val failed: Int,
        val skipped: Int,
        val duration: Long,
        val coverage: Double,
        val codeQualityScore: Int
    )
    
    private val history = mutableListOf<TestMetrics>()
    
    fun recordMetrics(metrics: TestMetrics) {
        history.add(metrics)
        
        // Save to database
        saveToDatabase(metrics)
    }
    
    fun generateTrendReport(): String {
        if (history.size < 2) return "Insufficient data"
        
        val recent = history.takeLast(10)
        
        val avgDuration = recent.map { it.duration }.average()
        val avgCoverage = recent.map { it.coverage }.average()
        val successRate = recent.map { it.passed.toDouble() / it.totalTests }
            .average()
        
        return buildString {
            appendLine("Test Metrics Trend")
            appendLine("=" * 30)
            appendLine("Average Duration: ${avgDuration}ms")
            appendLine("Average Coverage: ${(avgCoverage * 100).toInt()}%")
            appendLine("Success Rate: ${(successRate * 100).toInt()}%")
            appendLine("")
            appendLine("Recent History:")
            
            for ((index, metrics) in recent.withIndex()) {
                appendLine("Build ${index + 1}: ${metrics.passed}/${metrics.totalTests} passed (${metrics.duration}ms)")
            }
        }
    }
    
    fun generateAlertRules(): List<AlertRule> {
        return listOf(
            AlertRule(
                name = "Failing tests increased",
                condition = { 
                    val recent = history.takeLast(3)
                    recent.map { it.failed }.sum() > 0
                },
                severity = Severity.HIGH
            ),
            AlertRule(
                name = "Coverage decreased",
                condition = {
                    val last = history.lastOrNull() ?: return@AlertRule false
                    val average = history.map { it.coverage }.average()
                    last.coverage < average - 0.1
                },
                severity = Severity.MEDIUM
            ),
            AlertRule(
                name = "Tests taking too long",
                condition = {
                    history.lastOrNull()?.duration ?: 0 > 300000  // 5 minutes
                },
                severity = Severity.LOW
            )
        )
    }
}
```

### Output Results

```
Test Reporting Results
==================

Test Execution: COMPLETED
- Total Tests: 312
- Passed: 310
- Failed: 0
- Skipped: 2
- Duration: 124567ms

Coverage (JaCoCo):
- Instructions: 82% (15432/18842)
- Branches: 74% (342/462)
- Lines: 85% (1254/1476)
- Methods: 88% (456/518)
- Classes: 91% (86/94)

Test Quality:
- Code Coverage: 82%
- Test Success Rate: 99%
- Average Test Duration: 399ms
- Risk: LOW

Trend Analysis:
- Coverage trend: STABLE (+1% from last build)
- Test duration: IMPROVED (-5% from last build)
- Passing tests: STABLE
```

## Best Practices

1. **Generate reports always**: On every build
2. **Use multiple formats**: HTML, XML, JSON
3. **Track trends**: Over time and builds
4. **Set thresholds**: Minimum coverage
5. **Integrate with alerts**: Notify on failures
6. **Include code quality**: Lint, detekt
7. **Review reports**: Regular review
8. **Act on insights**: Improve tests

## Common Pitfalls

**Pitfall 1: No coverage tracking**
- **Problem**: Unknown test coverage
- **Solution**: Configure JaCoCo

**Pitfall 2: Large coverage gaps**
- **Problem**: Low coverage
- **Solution**: Add more tests

**Pitfall 3: Irrelevant coverage**
- **Problem**: Coverage on generated code
- **Solution**: Exclude generated code

**Pitfall 4: Report not reviewed**
- **Problem**: Insights not used
- **Solution**: Review regularly

**Pitfall 5: No trends tracked**
- **Problem**: Can't see progress
- **Solution**: Store history

## Troubleshooting Guide

**Issue: "Coverage not generated"**
1. Check JaCoCo plugin is applied
2. Verify test task ran
3. Check output directory exists

**Issue: "Missing classes in report"**
1. Configure source directories
2. Check class paths
3. Verify execution data

**Issue: "Coverage too low"**
1. Add more tests
2. Use code coverage as goal
3. Focus on core code

## Advanced Tips

**Tip 1: Combined reports**
```groovy
// Merge multiple test runs
jacocoMerge(inputFiles: files('app/build/**/*.exec')) {
    outputFile = file('merged.exec')
}
```

**Tip 2: SonarQube integration**
```groovy
sonarProperties {
    property "sonar.jacoco.reportPaths", "app/build/reports/jacoco/jacocoTestReport.xml"
}
```

**Tip 3: Custom exclusions**
```groovy
jacoco {
    excludes = ['**/R.class', '**/R$*.class']
}
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/04_Test_Utilities.md
See: 08_TESTING/02_Advanced_Testing/02_Performance_Testing.md
See: 08_TESTING/02_Advanced_Testing/04_Continuous_Testing.md
See: 01_SETUP_ENVIRONMENT/01_IDE_Installation_and_Configuration/04_Gradle_Configuration.md