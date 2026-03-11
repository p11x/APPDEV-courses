// MultithreadingDemo - Demonstrates Java Multithreading
// Important for concurrent backend operations

public class MultithreadingDemo {
    
    // Thread using Runnable interface
    static class MyRunnable implements Runnable {
        private String name;
        
        public MyRunnable(String name) {
            this.name = name;
        }
        
        public void run() {
            for (int i = 1; i <= 3; i++) {
                System.out.println(name + ": " + i);
                try {
                    Thread.sleep(500);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }
    
    // Thread using Thread class
    static class MyThread extends Thread {
        private String name;
        
        public MyThread(String name) {
            this.name = name;
        }
        
        public void run() {
            for (int i = 1; i <= 3; i++) {
                System.out.println(name + ": " + i);
                try {
                    Thread.sleep(500);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== MULTITHREADING DEMO ===\n");
        
        // Method 1: Using Runnable
        System.out.println("--- Using Runnable ---");
        Runnable r1 = new MyRunnable("Thread-1");
        Thread t1 = new Thread(r1);
        t1.start();
        
        // Method 2: Using Thread class
        System.out.println("--- Using Thread Class ---");
        Thread t2 = new MyThread("Thread-2");
        t2.start();
        
        // Lambda (Java 8+)
        System.out.println("--- Using Lambda ---");
        Thread t3 = new Thread(() -> {
            for (int i = 1; i <= 3; i++) {
                System.out.println("Thread-3: " + i);
                try { Thread.sleep(500); } catch (Exception e) {}
            }
        });
        t3.start();
        
        // Join threads
        try {
            t1.join();
            t2.join();
            t3.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        
        System.out.println("\n--- Thread Methods ---");
        Thread mainThread = Thread.currentThread();
        System.out.println("Main thread: " + mainThread.getName());
        System.out.println("Thread count: " + Thread.activeCount());
        
        // Synchronization
        System.out.println("\n--- Synchronization ---");
        Counter counter = new Counter();
        Thread t4 = new Thread(() -> {
            for (int i = 0; i < 1000; i++) counter.increment();
        });
        Thread t5 = new Thread(() -> {
            for (int i = 0; i < 1000; i++) counter.increment();
        });
        t4.start();
        t5.start();
        try {
            t4.join();
            t5.join();
        } catch (InterruptedException e) {}
        System.out.println("Counter: " + counter.getCount());
    }
}

// Counter with synchronization
class Counter {
    private int count = 0;
    
    public synchronized void increment() {
        count++;
    }
    
    public synchronized int getCount() {
        return count;
    }
}
