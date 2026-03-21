#!/usr/bin/env python3
"""
Auto-commit file watcher for APPDEV-courses repository.
Monitors changes and automatically commits and pushes to GitHub.
"""

import os
import subprocess
import time
import json
import logging
from datetime import datetime
from pathlib import Path
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler, FileSystemEvent

CONFIG_FILE = "watcher_config.json"
LOG_FILE = "auto_commit.log"

logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[
        logging.FileHandler(LOG_FILE),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)


def load_config():
    """Load configuration from JSON file."""
    try:
        with open(CONFIG_FILE, 'r') as f:
            return json.load(f)
    except FileNotFoundError:
        logger.warning(f"Config file {CONFIG_FILE} not found, using defaults")
        return {
            "watched_directory": ".",
            "debounce_seconds": 3600,
            "ignored_patterns": [".git", "__pycache__", "*.pyc", ".DS_Store", "auto_commit.log"],
            "branch": "master",
            "auto_push": True,
            "commit_message_format": "Auto-commit: {event} - {filename} - {timestamp}"
        }


def should_ignore(filepath: str, ignored_patterns: list) -> bool:
    """Check if file should be ignored based on patterns."""
    filepath = filepath.replace("\\", "/")
    parts = filepath.split("/")
    
    for pattern in ignored_patterns:
        if pattern.startswith("*"):
            if any(part.endswith(pattern[1:]) for part in parts):
                return True
        elif pattern.endswith("/"):
            if any(part == pattern.rstrip("/") for part in parts):
                return True
        else:
            if pattern in parts:
                return True
    return False


def run_git_command(command: list, cwd: str = ".") -> tuple:
    """Run a git command and return success status and output."""
    try:
        result = subprocess.run(
            command,
            cwd=cwd,
            capture_output=True,
            text=True,
            timeout=30
        )
        return result.returncode == 0, result.stdout, result.stderr
    except subprocess.TimeoutExpired:
        return False, "", "Command timed out"
    except Exception as e:
        return False, "", str(e)


def git_add():
    """Stage all changes."""
    success, stdout, stderr = run_git_command(["git", "add", "."])
    if success:
        logger.info("✅ Staged all changes")
        print("✅ Staged all changes")
    else:
        logger.error(f"❌ Error staging: {stderr}")
        print(f"❌ Error staging: {stderr}")
    return success


def git_commit(filename: str, event_type: str, config: dict) -> bool:
    """Commit staged changes."""
    timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    message = config["commit_message_format"].format(
        event=event_type,
        filename=filename,
        timestamp=timestamp
    )
    
    success, stdout, stderr = run_git_command(["git", "commit", "-m", message])
    if success:
        logger.info(f"✅ Committed: {filename}")
        print(f"✅ Committed: {filename}")
    else:
        if "nothing to commit" in stderr.lower():
            logger.info("Nothing to commit")
            return False
        logger.error(f"❌ Error committing: {stderr}")
        print(f"❌ Error committing: {stderr}")
        return False
    return True


def git_push(branch: str = "master") -> bool:
    """Push changes to remote."""
    success, stdout, stderr = run_git_command(["git", "push", "origin", branch])
    if success:
        logger.info("🚀 Pushed to master")
        print("🚀 Pushed to master")
    else:
        logger.error(f"❌ Error pushing: {stderr}")
        print(f"❌ Error pushing: {stderr}")
        return False
    return True


class AutoCommitHandler(FileSystemEventHandler):
    """Handler for file system events."""
    
    def __init__(self, config: dict):
        self.config = config
        self.pending_changes = []
        self.last_processed = time.time()
    
    def on_any_event(self, event: FileSystemEvent):
        """Handle any file system event."""
        if event.is_directory:
            return
        
        filepath = event.src_path
        if should_ignore(filepath, self.config["ignored_patterns"]):
            return
        
        event_type = event.event_type
        filename = os.path.basename(filepath)
        
        logger.info(f"Detected: {event_type} - {filename}")
        print(f"📁 Detected: {event_type} - {filename}")
        
        self.pending_changes.append({
            "filename": filename,
            "event_type": event_type,
            "time": time.time()
        })
        self.last_processed = time.time()
    
    def process_pending(self):
        """Process pending changes after debounce period."""
        if not self.pending_changes:
            return
        
        current_time = time.time()
        debounce = self.config["debounce_seconds"]
        
        if current_time - self.last_processed < debounce:
            return
        
        if len(self.pending_changes) > 0:
            filename = self.pending_changes[-1]["filename"]
            event_type = self.pending_changes[-1]["event_type"]
            
            self.pending_changes = []
            
            print(f"\n🔄 Processing changes...")
            
            if not git_add():
                return
            
            if not git_commit(filename, event_type, self.config):
                return
            
            if self.config.get("auto_push", True):
                if not git_push(self.config.get("branch", "master")):
                    return
            
            print("✨ All done!\n")


def main():
    """Main function to start the file watcher."""
    config = load_config()
    
    watched_dir = config.get("watched_directory", ".")
    branch = config.get("branch", "master")
    
    print("\n" + "="*50)
    print("🚀 APPDEV Courses Auto-Commit Watcher")
    print("="*50)
    print(f"📂 Watching: {os.path.abspath(watched_dir)}")
    print(f"🌿 Branch: {branch}")
    print(f"⏱️  Debounce: {config.get('debounce_seconds', 3)} seconds")
    print("-"*50)
    print("Press Ctrl+C to stop the watcher")
    print("="*50 + "\n")
    
    logger.info(f"Starting watcher on {watched_dir}")
    
    event_handler = AutoCommitHandler(config)
    observer = Observer()
    observer.schedule(event_handler, watched_dir, recursive=True)
    observer.start()
    
    try:
        while True:
            time.sleep(1)
            event_handler.process_pending()
    except KeyboardInterrupt:
        print("\n\n🛑 Stopping watcher...")
        observer.stop()
        observer.join()
        print("✅ Watcher stopped")


if __name__ == "__main__":
    main()
