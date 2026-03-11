/**
 * Environment Verification Script
 * This script helps verify that all required tools are properly installed
 * 
 * Run this with: node verify-environment.js
 */

// Function to execute system commands
const { execSync } = require('child_process');

const tools = [
  { name: 'Node.js', command: 'node -v', required: true },
  { name: 'npm', command: 'npm -v', required: true },
  { name: 'Angular CLI', command: 'ng version', required: true },
  { name: 'Java', command: 'java -version', required: true },
  { name: 'Maven', command: 'mvn -version', required: true },
  { name: 'Git', command: 'git --version', required: true }
];

console.log('===========================================');
console.log('  Environment Verification Script');
console.log('===========================================\n');

let allPassed = true;

tools.forEach(tool => {
  try {
    // For commands that might output to stderr, we redirect
    let output = '';
    try {
      output = execSync(tool.command, { encoding: 'utf8', stdio: 'pipe' });
    } catch (e) {
      output = e.stdout || e.message;
    }
    
    // Clean up output (remove extra newlines)
    output = output.trim().split('\n')[0];
    
    console.log(`✅ ${tool.name}: ${output}`);
  } catch (error) {
    if (tool.required) {
      console.log(`❌ ${tool.name}: NOT FOUND (Required)`);
      allPassed = false;
    } else {
      console.log(`⚠️  ${tool.name}: NOT FOUND (Optional)`);
    }
  }
});

console.log('\n===========================================');
if (allPassed) {
  console.log('✅ All required tools are installed!');
  console.log('===========================================');
  console.log('\nYou are ready to start your full-stack journey!');
} else {
  console.log('❌ Some required tools are missing!');
  console.log('===========================================');
  console.log('\nPlease install the missing tools before proceeding.');
  process.exit(1);
}

// Additional information about installed packages
console.log('\n===========================================');
console.log('  Additional Information');
console.log('===========================================');

try {
  const npmVersion = execSync('npm -v', { encoding: 'utf8' }).trim();
  console.log(`npm version: ${npmVersion}`);
  
  // Check if Angular CLI is installed globally
  try {
    const ngVersion = execSync('ng version', { encoding: 'utf8' });
    const ngMatch = ngVersion.match(/Angular CLI:\s*(\d+\.\d+\.\d+)/);
    if (ngMatch) {
      console.log(`Angular CLI: v${ngMatch[1]}`);
    }
  } catch (e) {
    // Angular CLI might not be in PATH
  }
} catch (e) {
  // Ignore errors for additional info
}

console.log('\n===========================================');
console.log('  Next Steps');
console.log('===========================================');
console.log('1. Install SQL Server (if not already installed)');
console.log('2. Install Postman (for API testing)');
console.log('3. Install VS Code extensions');
console.log('4. Proceed to Section 2: Introduction to Full-Stack Development');
console.log('===========================================\n');
