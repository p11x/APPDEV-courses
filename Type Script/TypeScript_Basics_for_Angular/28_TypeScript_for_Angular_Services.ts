/**
 * TypeScript for Angular Services
 * 
 * This file demonstrates how to create well-typed Angular services
 * using TypeScript features.
 * 
 * Angular Services: Use TypeScript for:
 * - Proper typing of data models
 * - Generic service methods
 * - HTTP response types
 * - Dependency injection
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== ANGULAR SERVICES ==========\n");

// ============================================
// DATA MODELS (INTERFACES)
// ============================================

// User model - used throughout the app
interface UserModel {
    id: number;
    username: string;
    email: string;
    roles: UserRole[];
    createdAt: Date;
    profile?: UserProfile;
}

interface UserProfile {
    avatar: string;
    bio: string;
    location: string;
}

enum UserRole {
    Admin = 'ADMIN',
    User = 'USER',
    Guest = 'GUEST'
}

// API Response wrapper
interface ApiResponse<T> {
    data: T;
    message: string;
    status: number;
}

// Paginated response
interface PaginatedResponse<T> {
    items: T[];
    total: number;
    page: number;
    pageSize: number;
    totalPages: number;
}

console.log("1. Data models defined:");
console.log("   UserModel: interface defined");
console.log("   ApiResponse<T>: Generic interface");
console.log("   PaginatedResponse<T>: For list endpoints");

// ============================================
// SERVICE WITH GENERICS
// ============================================

// Generic repository interface
interface Repository<T> {
    findAll(): Promise<T[]>;
    findById(id: number): Promise<T | null>;
    create(item: Omit<T, 'id'>): Promise<T>;
    update(id: number, item: Partial<T>): Promise<T>;
    delete(id: number): Promise<void>;
}

// User service implementing the repository
class UserService implements Repository<UserModel> {
    // Simulated data store
    private users: UserModel[] = [
        {
            id: 1,
            username: 'john_doe',
            email: 'john@example.com',
            roles: [UserRole.User],
            createdAt: new Date('2023-01-01'),
            profile: {
                avatar: '/avatars/1.jpg',
                bio: 'Software developer',
                location: 'New York'
            }
        },
        {
            id: 2,
            username: 'jane_smith',
            email: 'jane@example.com',
            roles: [UserRole.Admin, UserRole.User],
            createdAt: new Date('2023-02-15')
        }
    ];
    
    // GET /users - returns all users
    async findAll(): Promise<UserModel[]> {
        // In real app: return this.http.get<UserModel[]>('/api/users');
        return new Promise(resolve => {
            setTimeout(() => resolve([...this.users]), 100);
        });
    }
    
    // GET /users/:id - returns single user
    async findById(id: number): Promise<UserModel | null> {
        return new Promise(resolve => {
            const user = this.users.find(u => u.id === id) ?? null;
            setTimeout(() => resolve(user), 50);
        });
    }
    
    // POST /users - create new user
    async create(item: Omit<UserModel, 'id'>): Promise<UserModel> {
        return new Promise(resolve => {
            const newUser: UserModel = {
                ...item,
                id: Math.max(...this.users.map(u => u.id)) + 1
            };
            this.users.push(newUser);
            setTimeout(() => resolve(newUser), 100);
        });
    }
    
    // PUT /users/:id - update user
    async update(id: number, item: Partial<UserModel>): Promise<UserModel> {
        return new Promise((resolve, reject) => {
            const index = this.users.findIndex(u => u.id === id);
            if (index === -1) {
                reject(new Error('User not found'));
                return;
            }
            this.users[index] = { ...this.users[index], ...item };
            setTimeout(() => resolve(this.users[index]), 100);
        });
    }
    
    // DELETE /users/:id - delete user
    async delete(id: number): Promise<void> {
        return new Promise((resolve, reject) => {
            const index = this.users.findIndex(u => u.id === id);
            if (index === -1) {
                reject(new Error('User not found'));
                return;
            }
            this.users.splice(index, 1);
            setTimeout(() => resolve(), 50);
        });
    }
    
    // Additional methods specific to UserService
    async findByEmail(email: string): Promise<UserModel | null> {
        return new Promise(resolve => {
            const user = this.users.find(u => u.email === email) ?? null;
            setTimeout(() => resolve(user), 50);
        });
    }
    
    async findByRole(role: UserRole): Promise<UserModel[]> {
        return new Promise(resolve => {
            const users = this.users.filter(u => u.roles.includes(role));
            setTimeout(() => resolve(users), 50);
        });
    }
}

console.log("\n2. UserService created:");
const userService = new UserService();

// ============================================
// USING THE SERVICE
// ============================================

async function demonstrateService(): Promise<void> {
    console.log("\n3. Service methods:");
    
    // Find all users
    const allUsers = await userService.findAll();
    console.log("   findAll():", allUsers.length, 'users');
    
    // Find by ID
    const user1 = await userService.findById(1);
    console.log("   findById(1):", user1?.username);
    
    // Find by email
    const userByEmail = await userService.findByEmail('jane@example.com');
    console.log("   findByEmail('jane@example.com'):", userByEmail?.username);
    
    // Find by role
    const admins = await userService.findByRole(UserRole.Admin);
    console.log("   findByRole(Admin):", admins.length, 'admins');
    
    // Create user
    const newUser = await userService.create({
        username: 'new_user',
        email: 'new@example.com',
        roles: [UserRole.User],
        createdAt: new Date()
    });
    console.log("   create():", newUser.username, 'with id', newUser.id);
    
    // Update user
    const updated = await userService.update(1, {
        profile: { avatar: '/new-avatar.jpg', bio: 'Updated bio', location: 'LA' }
    });
    console.log("   update():", updated.profile?.bio);
    
    // Delete user
    await userService.delete(newUser.id);
    console.log("   delete(): user removed");
}

demonstrateService();

// ============================================
// SERVICE WITH HTTP-LIKE PATTERN
// ============================================

class HttpService {
    // Simulate HTTP get with generic response
    async get<T>(url: string): Promise<ApiResponse<T>> {
        return {
            data: {} as T,
            message: 'Success',
            status: 200
        };
    }
    
    // Simulate HTTP post
    async post<T, B>(url: string, body: B): Promise<ApiResponse<T>> {
        return {
            data: {} as T,
            message: 'Created',
            status: 201
        };
    }
}

class TypedUserService {
    private http = new HttpService();
    
    // Fully typed HTTP calls
    async getUsers(): Promise<ApiResponse<UserModel[]>> {
        return this.http.get<UserModel[]>('/api/users');
    }
    
    async getUser(id: number): Promise<ApiResponse<UserModel>> {
        return this.http.get<UserModel>(`/api/users/${id}`);
    }
    
    async createUser(user: Omit<UserModel, 'id'>): Promise<ApiResponse<UserModel>> {
        return this.http.post<UserModel, Omit<UserModel, 'id'>>('/api/users', user);
    }
}

console.log("\n4. Typed HTTP service:");
const typedService = new TypedUserService();
typedService.getUsers().then(response => {
    console.log('   Response status:', response.status);
});

console.log("\n========== SUMMARY ==========");
console.log("Angular Services:");
console.log("- Define interfaces for data models");
console.log("- Use generics for reusable methods");
console.log("- Implement Repository pattern");
console.log("- Type HTTP responses properly");
console.log("\nTypeScript Features Used:");
console.log("- Interfaces for models");
console.log("- Generics for reusable services");
console.log("- Enums for constants");
console.log("- async/await for async methods");
console.log("- Omit<T, K> and Partial<T> utility types");
console.log("\nBenefits:");
console.log("- Type safety throughout the app");
console.log("- Better IDE autocompletion");
console.log("- Easier refactoring");
console.log("- Self-documenting code");
console.log("================================\n");
