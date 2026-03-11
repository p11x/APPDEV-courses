/**
 * Angular Preparation Example
 * 
 * This comprehensive example demonstrates how TypeScript features
 * come together in Angular development.
 * 
 * It simulates an Angular architecture showing:
 * - User interface
 * - UserService with typed methods
 * - Generic service patterns
 * - Async data fetching
 * - Component structure
 * 
 * Each TypeScript feature is commented to show its contribution to Angular.
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== COMPREHENSIVE ANGULAR EXAMPLE ==========\n");

// ============================================
// 1. USER INTERFACE - Defines the data model
// ============================================

// Interface defines the shape of our data
// Angular Connection: Used for @Input types, HTTP responses, form models
interface User {
    id: number;
    username: string;
    email: string;
    // Optional property - might not always be present
    avatar?: string;
    // Readonly - shouldn't be modified
    readonly createdAt: Date;
}

// Enum for user roles - common in Angular
enum UserRole {
    Admin = 'ADMIN',
    User = 'USER',
    Guest = 'GUEST'
}

// Type alias for union - flexible typing
type UserWithRole = User & { roles: UserRole[] };

console.log("1. User Interface:");
console.log("   Using: interface, optional properties, readonly, enum, type alias");

// ============================================
// 2. GENERIC API RESPONSE - Reusable type
// ============================================

// Generic interface - works with any data type
// Angular Connection: HttpClient responses are typed this way
interface ApiResponse<T> {
    data: T;
    status: number;
    message: string;
}

// Generic for paginated responses
interface PaginatedResult<T> {
    items: T[];
    total: number;
    page: number;
    pageSize: number;
}

console.log("2. API Response Types:");
console.log("   Using: Generic interfaces for reusable type-safe responses");

// ============================================
// 3. USER SERVICE - Core Angular Service Pattern
// ============================================

/**
 * UserService - Simulates an Angular @Injectable service
 * 
 * TypeScript Features Demonstrated:
 * - Class structure
 * - Generic methods
 * - Async/await
 * - Private/public access modifiers
 * - Constructor parameter properties
 */
class UserService {
    // Simulated data store - private for encapsulation
    private users: User[] = [];
    
    // Constructor with dependency injection simulation
    // Angular Connection: Services are injected via constructor in Angular
    constructor() {
        // Initialize with mock data
        this.initializeMockData();
    }
    
    // Private method - internal only
    private initializeMockData(): void {
        this.users = [
            {
                id: 1,
                username: 'john_doe',
                email: 'john@example.com',
                avatar: '/avatars/1.jpg',
                createdAt: new Date('2023-01-15')
            },
            {
                id: 2,
                username: 'jane_smith',
                email: 'jane@example.com',
                createdAt: new Date('2023-03-20')
            }
        ];
    }
    
    // Generic method - returns typed array
    // Angular Connection methods return typed Promises/O: Servicebservables
    async getUsers(): Promise<User[]> {
        // Simulate API delay with async/await
        return new Promise(resolve => {
            setTimeout(() => resolve([...this.users]), 100);
        });
    }
    
    // Method with union type parameter
    async getUser(idOrEmail: number | string): Promise<User | null> {
        return new Promise(resolve => {
            let user: User | null = null;
            
            if (typeof idOrEmail === 'number') {
                user = this.users.find(u => u.id === idOrEmail) ?? null;
            } else {
                user = this.users.find(u => u.email === idOrEmail) ?? null;
            }
            
            setTimeout(() => resolve(user), 50);
        });
    }
    
    // Method using Omit utility type - common in Angular for create operations
    async createUser(userData: Omit<User, 'id' | 'createdAt'>): Promise<User> {
        return new Promise(resolve => {
            const newUser: User = {
                ...userData,
                id: Math.max(...this.users.map(u => u.id)) + 1,
                createdAt: new Date()
            };
            this.users.push(newUser);
            setTimeout(() => resolve(newUser), 100);
        });
    }
    
    // Method using Partial for updates - Angular pattern
    async updateUser(id: number, updates: Partial<User>): Promise<User | null> {
        return new Promise((resolve, reject) => {
            const index = this.users.findIndex(u => u.id === id);
            if (index === -1) {
                resolve(null);
                return;
            }
            this.users[index] = { ...this.users[index], ...updates };
            setTimeout(() => resolve(this.users[index]), 100);
        });
    }
    
    // Delete method
    async deleteUser(id: number): Promise<boolean> {
        return new Promise(resolve => {
            const index = this.users.findIndex(u => u.id === id);
            if (index === -1) {
                resolve(false);
                return;
            }
            this.users.splice(index, 1);
            setTimeout(() => resolve(true), 50);
        });
    }
}

console.log("\n3. UserService:");
console.log("   Using: Class, async/await, generics, private/public modifiers");

// ============================================
// 4. ANGULAR COMPONENT SIMULATION
// ============================================

/**
 * UserListComponent - Simulates an Angular Component
 * 
 * TypeScript Features:
 * - Class with properties and methods
 * - Type annotations
 * - Arrow functions for event handlers
 * - Getters for computed properties
 */
class UserListComponent {
    // Component properties with types - like @Input
    title: string = 'User Management';
    isLoading: boolean = false;
    errorMessage: string = '';
    
    // Component state - private
    private users: User[] = [];
    private filterText: string = '';
    
    // Getter for computed property
    // Angular Connection: Computed values in templates
    get userCount(): number {
        return this.users.length;
    }
    
    // Getter with filter
    get filteredUsers(): User[] {
        if (!this.filterText) return this.users;
        return this.users.filter(u => 
            u.username.toLowerCase().includes(this.filterText.toLowerCase()) ||
            u.email.toLowerCase().includes(this.filterText.toLowerCase())
        );
    }
    
    // Arrow function as event handler
    // Angular Connection: Template event handlers
    onSearch = (searchText: string): void => {
        this.filterText = searchText;
    };
    
    // Lifecycle hook simulation - async method
    async ngOnInit(): Promise<void> {
        this.isLoading = true;
        
        try {
            const userService = new UserService();
            this.users = await userService.getUsers();
        } catch (error) {
            this.errorMessage = 'Failed to load users';
        } finally {
            this.isLoading = false;
        }
    }
    
    // Event handler method
    onUserClick(user: User): void {
        console.log(`   User clicked: ${user.username}`);
    }
    
    // Delete handler with confirmation type
    async onDeleteUser(id: number): Promise<void> {
        const userService = new UserService();
        const success = await userService.deleteUser(id);
        
        if (success) {
            this.users = this.users.filter(u => u.id !== id);
        }
    }
}

console.log("4. UserListComponent:");
console.log("   Using: Class, getters, arrow functions, async/await, types");

// ============================================
// 5. DEMONSTRATION
// ============================================

async function demonstrateAngularPatterns(): Promise<void> {
    console.log("\n5. Running Demonstration:");
    
    // Using the service
    const service = new UserService();
    
    // Get all users
    const users = await service.getUsers();
    console.log('\n   All users:');
    users.forEach(u => console.log(`   - ${u.username} (${u.email})`));
    
    // Get single user
    const user1 = await service.getUser(1);
    console.log('\n   User by ID 1:');
    console.log(`   - ${user1?.username}`);
    
    // Create user
    const newUser = await service.createUser({
        username: 'new_user',
        email: 'new@example.com',
        avatar: '/avatars/new.jpg'
    });
    console.log('\n   Created user:');
    console.log(`   - ${newUser.username} with ID ${newUser.id}`);
    
    // Using the component
    const component = new UserListComponent();
    component.onSearch('john');
    console.log('\n   Filtered users (searching "john"):');
    component.filteredUsers.forEach(u => console.log(`   - ${u.username}`));
}

demonstrateAngularPatterns();

// ============================================
// 6. TYPE SCRIPT FEATURES SUMMARY
// ============================================

console.log("\n========== SUMMARY ==========");
console.log("\nTypeScript Features Used:");
console.log("1. Interfaces - Data models (User, ApiResponse)");
console.log("2. Enums - Constants (UserRole)");
console.log("3. Type aliases - Union types (UserWithRole)");
console.log("4. Classes - Services and Components");
console.log("5. Generics - Reusable types (ApiResponse<T>)");
console.log("6. async/await - Async operations");
console.log("7. Access modifiers - Encapsulation");
console.log("8. Getters - Computed properties");
console.log("9. Arrow functions - Event handlers");
console.log("10. Optional properties - Flexible types");
console.log("11. Readonly - Immutable properties");
console.log("12. Omit/Partial - Utility types");
console.log("\nAngular Patterns Demonstrated:");
console.log("- @Injectable service pattern");
console.log("- @Input/@Output type definitions");
console.log("- Lifecycle hooks (ngOnInit)");
console.log("- HTTP service methods");
console.log("- Component event handlers");
console.log("- Template computed properties");
console.log("\n================================\n");
