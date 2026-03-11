/**
 * Async/Await in TypeScript
 * 
 * Async/await provides cleaner syntax for working with Promises.
 * It makes async code look and behave more like synchronous code.
 * 
 * Angular Connection: Used extensively for:
 * - HTTP requests
 * - Async operations
 * - Service methods
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== ASYNC/AWAIT ==========\n");

// ============================================
// BASIC ASYNC FUNCTION
// ============================================

async function fetchData(): Promise<string> {
    // Simulate async operation
    return new Promise(resolve => {
        setTimeout(() => resolve('Data fetched!'), 100);
    });
}

async function main(): Promise<void> {
    const data = await fetchData();
    console.log("1. Basic async/await:");
    console.log('   Data:', data);
}

main();

// ============================================
// ASYNC WITH ERROR HANDLING
// ============================================

async function fetchWithError(shouldFail: boolean): Promise<string> {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            if (shouldFail) {
                reject(new Error('Failed to fetch'));
            } else {
                resolve('Success!');
            }
        }, 50);
    });
}

async function handleErrors(): Promise<void> {
    console.log("\n2. Error handling:");
    
    try {
        const result = await fetchWithError(false);
        console.log('   Success:', result);
    } catch (error) {
        console.log('   Error:', (error as Error).message);
    }
    
    // Try with error
    try {
        await fetchWithError(true);
    } catch (error) {
        console.log('   Caught error:', (error as Error).message);
    }
}

handleErrors();

// ============================================
// AWAIT WITH PROMISE.ALL
// ============================================

async function fetchUser(id: number): Promise<{ id: number; name: string }> {
    return new Promise(resolve => {
        setTimeout(() => resolve({ id, name: `User ${id}` }), 50);
    });
}

async function fetchAllUsers(): Promise<void> {
    console.log("\n3. Promise.all with await:");
    
    const users = await Promise.all([
        fetchUser(1),
        fetchUser(2),
        fetchUser(3)
    ]);
    
    console.log('   Users:', users);
}

fetchAllUsers();

// ============================================
// ASYNC/AWAIT IN CLASSES
// ============================================

class ApiClient {
    private baseUrl = 'https://api.example.com';
    
    async get<T>(endpoint: string): Promise<T> {
        // Simulate request
        return new Promise(resolve => {
            setTimeout(() => {
                resolve({ data: endpoint } as T);
            }, 50);
        });
    }
    
    async post<T, B>(endpoint: string, body: B): Promise<T> {
        return new Promise(resolve => {
            setTimeout(() => {
                // Simple return for demo
                resolve({ ...body, id: Math.random() } as unknown as T);
            }, 50);
        });
    }
}

async function useApiClient(): Promise<void> {
    console.log("\n4. Async methods in class:");
    
    const client = new ApiClient();
    
    // GET request
    const response = await client.get<{ data: string }>('/users');
    console.log('   GET response:', response);
    
    // POST request
    const created = await client.post<{ id: number }, { name: string }>(
        '/users',
        { name: 'John' }
    );
    console.log('   POST response:', created);
}

useApiClient();

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Angular-style service
interface UserItem {
    id: number;
    name: string;
    email: string;
}

class UserServiceAsync {
    private api = new ApiClient();
    
    // Async method to get users
    async getUsers(): Promise<UserItem[]> {
        console.log('   Fetching users...');
        
        // Simulate API call
        return new Promise(resolve => {
            setTimeout(() => {
                const users: UserItem[] = [
                    { id: 1, name: 'Alice', email: 'alice@example.com' },
                    { id: 2, name: 'Bob', email: 'bob@example.com' }
                ];
                resolve(users);
            }, 100);
        });
    }
    
    // Async method with error handling
    async getUser(id: number): Promise<UserItem | null> {
        try {
            const users = await this.getUsers();
            return users.find(u => u.id === id) ?? null;
        } catch (error) {
            console.log('   Error fetching user:', (error as Error).message);
            return null;
        }
    }
    
    // Async method to create user
    async createUser(user: Omit<UserItem, 'id'>): Promise<UserItem> {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                if (!user.name) {
                    reject(new Error('Name is required'));
                    return;
                }
                
                const newUser: UserItem = {
                    id: Math.random(),
                    ...user
                };
                resolve(newUser);
            }, 100);
        });
    }
}

console.log("Angular service pattern:");
const userServiceAsync = new UserServiceAsync();

userServiceAsync.getUsers().then(users => {
    console.log('   Users:', users);
});

userServiceAsync.getUser(1).then(user => {
    console.log('   User 1:', user);
});

userServiceAsync.createUser({ name: 'Charlie', email: 'charlie@example.com' })
    .then(newUser => {
        console.log('   Created user:', newUser);
    });

// Async lifecycle hook simulation
class ComponentWithAsync {
    async ngOnInit(): Promise<void> {
        console.log("\nComponent lifecycle:");
        console.log('   ngOnInit called');
        
        const data = await this.loadData();
        console.log('   Loaded data:', data);
    }
    
    private async loadData(): Promise<string> {
        return new Promise(resolve => {
            setTimeout(() => resolve('Component data'), 50);
        });
    }
}

const componentAsync = new ComponentWithAsync();
componentAsync.ngOnInit();

console.log("\n========== SUMMARY ==========");
console.log("Async/Await:");
console.log("- 'async' keyword makes function return Promise");
console.log("- 'await' pauses until Promise resolves");
console.log("- Cleaner than .then()/.catch() chains");
console.log("- Error handling with try/catch");
console.log("\nWith Promise.all:");
console.log("- Run promises concurrently");
console.log("- await Promise.all([...])");
console.log("\nAngular Usage:");
console.log("- Service methods");
console.log("- HTTP requests");
console.log("- Lifecycle hooks");
console.log("- Form submissions");
console.log("================================\n");
