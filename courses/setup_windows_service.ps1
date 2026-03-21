# APPDEV Courses - Windows Task Scheduler Setup Script
# Run as Administrator for best results

$TaskName = "APPDEV-AutoCommit"
$RepoPath = $PSScriptRoot
$PythonExe = "pythonw.exe"
$ScriptPath = Join-Path $RepoPath "auto_commit.py"

# Find pythonw.exe in PATH or common locations
$PythonPaths = @(
    "pythonw.exe",
    "C:\Python312\pythonw.exe",
    "C:\Python311\pythonw.exe",
    "C:\Python310\pythonw.exe",
    "C:\Python39\pythonw.exe",
    "$env:USERPROFILE\AppData\Local\Programs\Python\Python312\pythonw.exe",
    "$env:USERPROFILE\AppData\Local\Programs\Python\Python311\pythonw.exe",
    "$env:USERPROFILE\AppData\Local\Programs\Python\Python310\pythonw.exe",
    "C:\Program Files\Python312\pythonw.exe",
    "C:\Program Files\Python311\pythonw.exe"
)

$PythonW = $null
foreach ($path in $PythonPaths) {
    $expandedPath = [System.Environment]::ExpandEnvironmentVariables($path)
    if (Test-Path $expandedPath) {
        $PythonW = $expandedPath
        break
    }
}

# If not found, try to find pythonw
if (-not $PythonW) {
    $pythonWich = Get-Command pythonw.exe -ErrorAction SilentlyContinue
    if ($pythonWich) {
        $PythonW = $pythonWich.Source
    } else {
        Write-Host "❌ pythonw.exe not found. Installing watchdog with regular python..." -ForegroundColor Red
        $PythonW = "python.exe"
    }
}

Write-Host "Using Python: $PythonW" -ForegroundColor Cyan

# Create the task action
$Action = New-ScheduledTaskAction -Execute $PythonW -Argument "`"$ScriptPath`"" -WorkingDirectory $RepoPath

# Create the triggers (at startup and at logon)
$Trigger1 = New-ScheduledTaskTrigger -AtStartup
$Trigger2 = New-ScheduledTaskTrigger -AtLogOn

# Create the principal (run whether user is logged on or not)
$Principal = New-ScheduledTaskPrincipal -UserId $env:USERNAME -LogonType S4U -RunLevel Highest

# Create settings
$Settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable -RunOnlyIfNetworkAvailable:$false -DontStopOnIdleEnd -RestartOnFailure -RestartInterval (New-TimeSpan -Minutes 1) -MaxRestartCount 3

# Register the task
try {
    # Remove existing task if present
    Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false -ErrorAction SilentlyContinue
    
    Register-ScheduledTask -TaskName $TaskName -Action $Action -Trigger $Trigger1, $Trigger2 -Principal $Principal -Settings $Settings -Description "APPDEV Courses Auto-Commit File Watcher" -Force
    
    Write-Host ""
    Write-Host "✅ Task 'APPDEV-AutoCommit' registered in Windows Task Scheduler" -ForegroundColor Green
    Write-Host "🔁 It will auto-start on every system boot" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Task Details:" -ForegroundColor Yellow
    Write-Host "  - Name: $TaskName"
    Write-Host "  - Path: $ScriptPath"
    Write-Host "  - Working Directory: $RepoPath"
    Write-Host "  - Python: $PythonW"
    Write-Host ""
    Write-Host "To check status: Get-ScheduledTask -TaskName '$TaskName' | Get-ScheduledTaskInfo" -ForegroundColor Gray
    Write-Host "To remove: .\remove_windows_service.ps1" -ForegroundColor Gray
    
} catch {
    Write-Host "❌ Error creating task: $_" -ForegroundColor Red
    exit 1
}
