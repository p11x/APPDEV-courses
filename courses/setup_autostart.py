#!/usr/bin/env python3
"""
Unified Auto-Start Setup Script
Detects OS and runs the appropriate setup script for auto-starting the watcher.
"""

import os
import sys
import subprocess
import platform
from pathlib import Path

def get_repo_path():
    """Get the full path to the repository."""
    return Path(__file__).parent.resolve()

def detect_os():
    """Detect the operating system."""
    system = platform.system().lower()
    if system == "windows":
        return "windows"
    elif system == "darwin":
        return "mac"
    elif system == "linux":
        return "linux"
    else:
        return None

def run_script(script_path):
    """Run a shell script and return its success status."""
    try:
        result = subprocess.run(
            ["bash", str(script_path)],
            cwd=get_repo_path(),
            capture_output=False
        )
        return result.returncode == 0
    except Exception as e:
        print(f"❌ Error running script: {e}")
        return False

def run_powershell(script_path):
    """Run a PowerShell script and return its success status."""
    try:
        result = subprocess.run(
            ["powershell", "-ExecutionPolicy", "Bypass", "-File", str(script_path)],
            cwd=get_repo_path(),
            capture_output=False
        )
        return result.returncode == 0
    except Exception as e:
        print(f"❌ Error running script: {e}")
        return False

def main():
    repo_path = get_repo_path()
    os_name = detect_os()
    
    print("\n" + "="*50)
    print("🔧 APPDEV Courses - Auto-Start Setup")
    print("="*50)
    print("")
    print(f"🖥️  OS Detected: {os_name.capitalize() if os_name else 'Unknown'}")
    print(f"📁 Repo Path: {repo_path}")
    print("")
    
    if not os_name:
        print("❌ Unsupported operating system")
        print("Supported: Windows, Linux, Mac")
        sys.exit(1)
    
    success = False
    
    if os_name == "windows":
        script = repo_path / "setup_windows_service.ps1"
        if script.exists():
            print("▶️  Running Windows setup script...")
            success = run_powershell(script)
        else:
            print(f"❌ Script not found: {script}")
    
    elif os_name == "linux":
        script = repo_path / "setup_linux_service.sh"
        if script.exists():
            print("▶️  Running Linux setup script...")
            success = run_script(script)
        else:
            print(f"❌ Script not found: {script}")
    
    elif os_name == "mac":
        script = repo_path / "setup_mac_service.sh"
        if script.exists():
            print("▶️  Running Mac setup script...")
            success = run_script(script)
        else:
            print(f"❌ Script not found: {script}")
    
    print("")
    if success:
        print("="*50)
        print("✅ Auto-start service configured")
        print("🔁 Watcher will run on every system boot")
        print("📄 Logs saved to: auto_commit.log")
        print("="*50)
        print("")
        print("⛔ To stop/remove the service, run:")
        if os_name == "windows":
            print("   powershell remove_windows_service.ps1")
        elif os_name == "linux":
            print("   bash remove_linux_service.sh")
        elif os_name == "mac":
            print("   bash remove_mac_service.sh")
    else:
        print("❌ Setup failed")
        sys.exit(1)

if __name__ == "__main__":
    main()
