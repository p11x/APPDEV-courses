@echo off
REM APPDEV Courses Auto-Commit Watcher Startup Script (Windows)

echo.
echo 🚀 Starting APPDEV Courses Auto-Commit Watcher...
echo.

REM Check if virtual environment exists
if exist venv (
    echo 📦 Activating virtual environment...
    call venv\Scripts\activate.bat
)

REM Install requirements
echo 📥 Installing dependencies...
pip install -r requirements.txt

if %ERRORLEVEL% NEQ 0 (
    echo ❌ Failed to install dependencies
    exit /b 1
)

echo ✅ Dependencies installed successfully
echo.
echo ▶️  Starting file watcher...
echo Press Ctrl+C to stop
echo.

REM Run the watcher
python auto_commit.py
