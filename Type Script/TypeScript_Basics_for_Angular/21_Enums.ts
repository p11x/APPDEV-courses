/**
 * Enums in TypeScript
 * 
 * Enums allow defining named constants. TypeScript supports numeric and string enums.
 * 
 * Angular Connection: Used for:
 * - Route definitions
 * - User roles
 * - Application states
 * - Configuration options
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== ENUMS ==========\n");

// ============================================
// NUMERIC ENUMS
// ============================================

enum Direction {
    Up,    // 0
    Down,  // 1
    Left,  // 2
    Right  // 3
}

console.log("1. Numeric enums:");
console.log("   Direction.Up:", Direction.Up);
console.log("   Direction.Down:", Direction.Down);
console.log("   Direction[0]:", Direction[0]);

// Custom numeric values
enum StatusCode {
    Success = 200,
    Created = 201,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    ServerError = 500
}

console.log("\n2. Custom numeric values:");
console.log("   StatusCode.Success:", StatusCode.Success);
console.log("   StatusCode.NotFound:", StatusCode.NotFound);

// ============================================
// STRING ENUMS
// ============================================

enum UserRole {
    Admin = 'ADMIN',
    User = 'USER',
    Guest = 'GUEST'
}

console.log("\n3. String enums:");
console.log("   UserRole.Admin:", UserRole.Admin);
console.log("   UserRole.User:", UserRole.User);

function checkPermission(role: UserRole): void {
    if (role === UserRole.Admin) {
        console.log('   Full access');
    } else if (role === UserRole.User) {
        console.log('   Limited access');
    } else {
        console.log('   No access');
    }
}

checkPermission(UserRole.Admin);
checkPermission(UserRole.User);
checkPermission(UserRole.Guest);

// ============================================
// HETEROGENEOUS ENUMS (MIXED)
// ============================================

enum MixedEnum {
    No = 0,
    Yes = 'YES'
}

console.log("\n4. Heterogeneous enum:");
console.log("   MixedEnum.No:", MixedEnum.No);
console.log("   MixedEnum.Yes:", MixedEnum.Yes);

// ============================================
// CONST ENUMS
// ============================================

// const enums are inlined at compile time
const enum HttpMethod {
    GET = 'GET',
    POST = 'POST',
    PUT = 'PUT',
    DELETE = 'DELETE'
}

console.log("\n5. Const enums:");
console.log("   HttpMethod.GET:", HttpMethod.GET);

// ============================================
// ENUM WITH METHODS
// ============================================

enum LogLevel {
    Debug = 'DEBUG',
    Info = 'INFO',
    Warn = 'WARN',
    Error = 'ERROR'
}

const currentLevel = LogLevel.Info;

function shouldLog(level: LogLevel): boolean {
    const levels = [LogLevel.Debug, LogLevel.Info, LogLevel.Warn, LogLevel.Error];
    return levels.indexOf(level) >= levels.indexOf(currentLevel);
}

console.log("\n6. Enum usage:");
console.log("   shouldLog(LogLevel.Debug):", shouldLog(LogLevel.Debug));
console.log("   shouldLog(LogLevel.Error):", shouldLog(LogLevel.Error));

// ============================================
// REVERSE MAPPING
// ============================================

enum DirectionReverse {
    North = 1,
    South = 2,
    East = 3,
    West = 4
}

console.log("\n7. Reverse mapping (numeric only):");
console.log("   DirectionReverse.North:", DirectionReverse.North);
console.log("   DirectionReverse[1]:", DirectionReverse[1]);

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Route types
enum RoutePath {
    Home = '/',
    Users = '/users',
    Products = '/products',
    Login = '/login',
    Dashboard = '/dashboard'
}

console.log("Route paths:");
console.log("   RoutePath.Home:", RoutePath.Home);
console.log("   RoutePath.Users:", RoutePath.Users);

// Application state
enum AppState {
    Loading = 'LOADING',
    Ready = 'READY',
    Error = 'ERROR',
    Offline = 'OFFLINE'
}

let currentState: AppState = AppState.Loading;

function handleStateChange(state: AppState): void {
    console.log(`   State changed to: ${state}`);
    
    switch (state) {
        case AppState.Loading:
            console.log('   Showing spinner...');
            break;
        case AppState.Ready:
            console.log('   Rendering content...');
            break;
        case AppState.Error:
            console.log('   Showing error message...');
            break;
        case AppState.Offline:
            console.log('   Showing offline indicator...');
            break;
    }
}

console.log("\nState handling:");
handleStateChange(AppState.Loading);
handleStateChange(AppState.Ready);

// Event types
enum ButtonEvent {
    Click = 'CLICK',
    Hover = 'HOVER',
    Focus = 'FOCUS',
    Blur = 'BLUR'
}

function trackEvent(event: ButtonEvent, element: string): void {
    console.log(`   Tracking ${event} on ${element}`);
}

console.log("\nEvent tracking:");
trackEvent(ButtonEvent.Click, 'submit-btn');
trackEvent(ButtonEvent.Focus, 'input-field');

// Form validation state
enum ValidationState {
    Valid = 'VALID',
    Invalid = 'INVALID',
    Pending = 'PENDING',
    Pristine = 'PRISTINE'
}

interface FormFieldState {
    state: ValidationState;
    message?: string;
}

function validateField(field: FormFieldState): void {
    const icon = field.state === ValidationState.Valid ? '✓' : '✗';
    console.log(`   ${icon} ${field.state}${field.message ? `: ${field.message}` : ''}`);
}

console.log("\nForm validation:");
validateField({ state: ValidationState.Valid });
validateField({ state: ValidationState.Invalid, message: 'Required field' });

console.log("\n========== SUMMARY ==========");
console.log("Enums:");
console.log("- Use 'enum' keyword");
console.log("- Numeric: auto-increment by default");
console.log("- String: explicit values");
console.log("- Const: inlined at compile time");
console.log("- Reverse mapping for numeric enums");
console.log("\nAngular Usage:");
console.log("- Route definitions");
console.log("- User roles and permissions");
console.log("- Application states");
console.log("- Event types");
console.log("- Form validation states");
console.log("\nBest Practices:");
console.log("- Use string enums for most cases");
console.log("- Use const enums for performance");
console.log("- Prefer enums over magic numbers");
console.log("================================\n");
