# APPDEV Courses - Windows Task Scheduler Removal Script

$TaskName = "APPDEV-AutoCommit"

Write-Host "Removing Windows Task Scheduler task..." -ForegroundColor Yellow

try {
    Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false -ErrorAction Stop
    Write-Host ""
    Write-Host "[OK] Task removed from Task Scheduler" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "Task '$TaskName' not found or already removed." -ForegroundColor Yellow
}
