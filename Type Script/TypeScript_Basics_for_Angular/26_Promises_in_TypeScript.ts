/**
 * Promises in TypeScript
 * 
 * Promises represent eventual completion or failure of async operations.
 * Angular HttpClient returns Promises for HTTP requests.
 * 
 * Angular Connection: Used for:
 * - HttpClient requests
 * - Async operations
 * - Handling API responses
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== PROMISES ==========\n");

// ============================================
// CREATING PROMISES
// ============================================

// Create a promise
const myPromise = new Promise<string>((resolve, reject) => {
    // Simulate async operation
    setTimeout(() => {
        const success = true;
        
        if (success) {
            resolve('Operation completed successfully!');
        } else {
            reject(new Error('Operation failed'));
        }
    }, 100);
});

console.log("1. Creating promises:");
myPromise
    .then(result => console.log('   Result:', result))
    .catch(error => console.log('   Error:', error.message));

// ============================================
// PROMISE STATES
// ============================================

// Promise has three states: pending, fulfilled, rejected
const promise1 = new Promise((resolve) => {
    setTimeout(() => resolve('Done!'), 50);
});

const promise2 = new Promise((_, reject) => {
    setTimeout(() => reject(new Error('Failed!')), 50);
});

console.log("\n2. Promise states:");
promise1.then(result => console.log('   Fulfilled:', result));

promise2
    .catch(error => console.log('   Rejected:', error.message));

// ============================================
// THEN, CATCH, FINALLY
// ============================================

const chainedPromise = new Promise<number>((resolve) => {
    resolve(10);
});

// Chain operations
chainedPromise
    .then(value => {
        console.log("\n3. Chaining:");
        console.log('   Initial:', value);
        return value * 2;
    })
    .then(value => {
        console.log('   After first transformation:', value);
        return value + 5;
    })
    .then(value => {
        console.log('   After second transformation:', value);
    })
    .catch(error => {
        console.log('   Error:', error);
    })
    .finally(() => {
        console.log('   Finally block executed');
    });

// ============================================
// PROMISE.ALL
// ============================================

const promiseA = Promise.resolve('Result A');
const promiseB = Promise.resolve('Result B');
const promiseC = Promise.resolve('Result C');

console.log("\n4. Promise.all:");
Promise.all([promiseA, promiseB, promiseC])
    .then(results => {
        console.log('   All results:', results);
    });

// ============================================
// PROMISE.RACE
// ============================================

const fastPromise = new Promise(resolve => 
    setTimeout(() => resolve('Fast!'), 50)
);
const slowPromise = new Promise(resolve => 
    setTimeout(() => resolve('Slow!'), 200)
);

console.log("\n5. Promise.race:");
Promise.race([fastPromise, slowPromise])
    .then(result => console.log('   First to complete:', result));

// ============================================
// PROMISE.THEN
// ============================================

const promiseWithType = Promise.resolve<string>('typed result');

console.log("\n6. Generic Promise types:");
promiseWithType
    .then((result: string) => console.log('   Result:', result))
    .catch((error: Error) => console.log('   Error:', error.message));

// ============================================
// ANGULAR EXAMPLES
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Simulating HttpClient (Angular returns Promises/Observables)
interface HttpResponse<T> {
    data: T;
    status: number;
}

// Mock HttpClient
class MockHttpClient {
    get<T>(url: string): Promise<HttpResponse<T>> {
        return new Promise((resolve) => {
            setTimeout(() => {
                const mockData = { message: 'Mock response' } as T;
                resolve({ data: mockData, status: 200 });
            }, 100);
        });
    }
}

const http = new MockHttpClient();

// Using promises with HttpClient
console.log("HttpClient usage:");
http.get<{ message: string }>('/api/users')
    .then(response => {
        console.log('   Status:', response.status);
        console.log('   Data:', response.data);
    })
    .catch(error => {
        console.log('   Error:', error.message);
    });

// Async service pattern
class DataService {
    private http = new MockHttpClient();
    
    async fetchData(): Promise<{ data: string }> {
        const response = await this.http.get<{ data: string }>('/api/data');
        return response.data;
    }
    
    fetchWithErrorHandling(): Promise<{ message: string }> {
        return this.http.get<{ message: string }>('/api/data')
            .then(response => {
                console.log('   Fetched:', response.data);
                return response.data;
            })
            .catch(error => {
                console.log('   Error caught:', error.message);
                throw error;
            });
    }
}

console.log("\nService pattern:");
const dataService = new DataService();
dataService.fetchWithErrorHandling()
    .then(result => console.log('   Final result:', result.message));

// Multiple concurrent requests
interface User {
    id: number;
    name: string;
}

function fetchUser(id: number): Promise<User> {
    return Promise.resolve({ id, name: `User ${id}` });
}

console.log("\nConcurrent requests:");
Promise.all([
    fetchUser(1),
    fetchUser(2),
    fetchUser(3)
]).then(users => {
    console.log('   All users:', users);
});

console.log("\n========== SUMMARY ==========");
console.log("Promises:");
console.log("- Represent async operations");
console.log("- Have three states: pending, fulfilled, rejected");
console.log("- Use .then() for success handling");
console.log("- Use .catch() for error handling");
console.log("- Use .finally() for cleanup");
console.log("\nMethods:");
console.log("- Promise.all(): wait for all");
console.log("- Promise.race(): first to complete");
console.log("- Promise.resolve(): create resolved promise");
console.log("- Promise.reject(): create rejected promise");
console.log("\nAngular Usage:");
console.log("- HttpClient returns Promises (or Observables)");
console.log("- Can use .then()/.catch() or async/await");
console.log("- Important for API calls");
console.log("================================\n");
