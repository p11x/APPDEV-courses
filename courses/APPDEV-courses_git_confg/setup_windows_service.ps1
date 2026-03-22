# APPDEV Courses - Windows Task Scheduler Setup Script
# Uses schtasks.exe for more reliable task creation

$TaskName = "APPDEV-AutoCommit"
$RepoPath = $PSScriptRoot
$ScriptPath = Join-Path $RepoPath "auto_commit.py"

# Try to find Python
$PythonCmd = $null

# Check common Python locations
$PythonLocs = @(
    "python.exe",
    "pythonw.exe",
    "C:\Python312\python.exe",
    "C:\Python311\python.exe",
    "C:\Python310\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python312\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python311\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python310\python.exe"
)

foreach ($loc in $PythonLocs) {
    $expanded = [System.Environment]::ExpandEnvironmentVariables($loc)
    if (Test-Path $expanded) {
        $PythonCmd = $expanded
        break
    }
}

if (-not $PythonCmd) {
    $PythonCmd = "python.exe"
}

Write-Host "Using Python: $PythonCmd" -ForegroundColor Cyan

# Remove existing task first
$existing = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
if ($existing) {
    Write-Host "Removing existing task..." -ForegroundColor Yellow
    Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false
}

# Create task using schtasks (more reliable)
$schtasksArgs = @(
    "/Create",
    "/TN", $TaskName,
    "/TR", "`"$PythonCmd`" `"$ScriptPath`"",
    "/SC", "ONLOGON",
    "/RL", "LIMITED",
    "/F"
)

$output = schtasks.exe $schtasksArgs 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "[OK] Task 'APPDEV-AutoCommit' registered in Windows Task Scheduler" -ForegroundColor Green
    Write-Host "[INFO] It will auto-start on every user login" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Task Details:" -ForegroundColor Yellow
    Write-Host "  - Name: $TaskName"
    Write-Host "  - Path: $ScriptPath"
    Write-Host "  - Working Directory: $RepoPath"
    Write-Host "  - Python: $PythonCmd"
    Write-Host ""
    Write-Host "To check status: schtasks /Run /TN '$TaskName'" -ForegroundColor Gray
    Write-Host "To remove: .\remove_windows_service.ps1" -ForegroundColor Gray
} else {
    Write-Host "[ERROR] Failed to create task: $output" -ForegroundColor Red
    
    # Try alternative - create a startup shortcut instead
    Write-Host ""
    Write-Host "[INFO] Trying alternative: Creating Startup shortcut..." -ForegroundColor Yellow
    
    $StartupFolder = [System.Environment]::GetFolderPath("Startup")
    $ShortcutPath = Join-Path $StartupFolder "APPDEV-AutoCommit.lnk"
    
    try {
        $WshShell = New-Object -ComObject WScript.Shell
        $Shortcut = $WshShell.CreateShortcut($ShortcutPath)
        $Shortcut.TargetPath = $PythonCmd
        $Shortcut.Arguments = "`"$ScriptPath`""
        $Shortcut.WorkingDirectory = $RepoPath
        $Shortcut.Description = "APPDEV Courses Auto-Commit Watcher"
        $Shortcut.Save()
        
        Write-Host ""
        Write-Host "[OK] Startup shortcut created!" -ForegroundColor Green
        Write-Host "[INFO] The watcher will start on every login" -ForegroundColor Cyan
        Write-Host "[INFO] Shortcut location: $ShortcutPath" -ForegroundColor Gray
    } catch {
        Write-Host "[ERROR] Could not create startup shortcut: $_" -ForegroundColor Red
    }
}
