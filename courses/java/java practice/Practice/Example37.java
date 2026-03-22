/*
 * SUB TOPIC: Multithreading in Java
 * 
 * DEFINITION:
 * Multithreading is the ability of a CPU to execute multiple threads concurrently. A thread is the 
 * smallest unit of execution. Java provides built-in support for multithreading through the Thread 
 * class and Runnable interface, enabling parallel processing and better resource utilization.
 * 
 * FUNCTIONALITIES:
 * 1. Creating threads using Thread class
 * 2. Creating threads using Runnable interface
 * 3. Thread lifecycle (NEW, RUNNABLE, BLOCKED, TERMINATED)
 * 4. Thread synchronization with synchronized keyword
 * 5. Thread communication with wait(), notify(), notifyAll()
 * 6. Thread priorities
 */

public class Example37 {
    
    // Thread using Thread class
    static class MyThread extends Thread {
        private String name;
        
        public MyThread(String name) {
            this.name = name;
        }
        
        @Override
        public void run() {
            for (int i = 1; i <= 5; i++) {
                System.out.println(name + " - Count: " + i);
                try {
                    sleep(100); // Pause thread execution
                } catch (InterruptedException e) {
                    System.out.println(e.getMessage());
                }
            }
        }
    }
    
    // Thread using Runnable interface
    static class MyRunnable implements Runnable {
        private String taskName;
        
        public MyRunnable(String taskName) {
            this.taskName = taskName;
        }
        
        @Override
        public void run() {
            System.out.println(taskName + " started");
            for (int i = 1; i <= 3; i++) {
                System.out.println(taskName + " processing: " + i);
                try {
                    Thread.sleep(200);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            System.out.println(taskName + " completed");
        }
    }
    
    public static void main(String[] args) {
        
        // Topic Explanation: Creating Threads
        
        // Method 1: Using Thread class
        System.out.println("=== Thread using Thread Class ===");
        MyThread thread1 = new MyThread("Thread-1");
        thread1.start(); // Start the thread
        
        // Method 2: Using Runnable interface
        System.out.println("\n=== Thread using Runnable ===");
        Thread thread2 = new Thread(new MyRunnable("Task-A"));
        thread2.start();
        
        // Main thread continues
        System.out.println("\nMain thread executing...");
        
        try {
            thread1.join(); // Wait for thread1 to complete
            thread2.join(); // Wait for thread2 to complete
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        
        System.out.println("\nAll threads completed!");
        
        // Thread Priorities
        System.out.println("\n=== Thread Priorities ===");
        Thread highPriority = new Thread(() -> System.out.println("High priority running"));
        Thread lowPriority = new Thread(() -> System.out.println("Low priority running"));
        
        highPriority.setPriority(Thread.MAX_PRIORITY); // 10
        lowPriority.setPriority(Thread.MIN_PRIORITY); // 1
        
        System.out.println("High priority: " + highPriority.getPriority());
        System.out.println("Low priority: " + lowPriority.getPriority());
        
        // Real-time Example 1: Download Manager simulation
        System.out.println("\n=== Example 1: Download Manager ===");
        
        class Downloader implements Runnable {
            private String fileName;
            
            public Downloader(String fileName) {
                this.fileName = fileName;
            }
            
            @Override
            public void run() {
                System.out.println("Downloading " + fileName + " started...");
                for (int i = 1; i <= 10; i++) {
                    try {
                        Thread.sleep(100);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                    System.out.println(fileName + ": " + (i * 10) + "%");
                }
                System.out.println(fileName + " download completed!");
            }
        }
        
        Thread d1 = new Thread(new Downloader("video.mp4"));
        Thread d2 = new Thread(new Downloader("document.pdf"));
        
        d1.start();
        d2.start();
        
        // Real-time Example 2: Online Chat Server
        System.out.println("\n=== Example 2: Chat Messages ===");
        
        class ChatMessage {
            private String message;
            
            public synchronized void sendMessage(String msg) {
                this.message = msg;
                System.out.println("Sent: " + msg);
                notify(); // Notify waiting thread
            }
            
            public synchronized String receiveMessage() {
                try {
                    wait(); // Wait for message
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                return message;
            }
        }
        
        ChatMessage chat = new ChatMessage();
        
        Thread sender = new Thread(() -> {
            chat.sendMessage("Hello!");
            chat.sendMessage("How are you?");
        });
        
        Thread receiver = new Thread(() -> {
            String msg = chat.receiveMessage();
            System.out.println("Received: " + msg);
        });
        
        // Real-time Example 3: Bank Account - Synchronized
        System.out.println("\n=== Example 3: Bank Account ===");
        
        class BankAccount {
            private double balance;
            
            public BankAccount(double initialBalance) {
                this.balance = initialBalance;
            }
            
            public synchronized void deposit(double amount) {
                balance += amount;
                System.out.println("Deposited: " + amount + ", New Balance: " + balance);
            }
            
            public synchronized void withdraw(double amount) {
                if (amount <= balance) {
                    balance -= amount;
                    System.out.println("Withdrew: " + amount + ", New Balance: " + balance);
                } else {
                    System.out.println("Insufficient funds!");
                }
            }
            
            public synchronized double getBalance() {
                return balance;
            }
        }
        
        BankAccount account = new BankAccount(1000);
        
        Thread t1 = new Thread(() -> account.deposit(500));
        Thread t2 = new Thread(() -> account.withdraw(300));
        Thread t3 = new Thread(() -> account.deposit(200));
        
        t1.start();
        t2.start();
        t3.start();
        
        // Real-time Example 4: Task Scheduler simulation
        System.out.println("\n=== Example 4: Task Scheduler ===");
        
        class TaskScheduler {
            public void scheduleTask(String task, int delay) {
                try {
                    System.out.println("Scheduling: " + task);
                    Thread.sleep(delay);
                    System.out.println("Executing: " + task);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
        
        TaskScheduler scheduler = new TaskScheduler();
        
        Thread task1 = new Thread(() -> scheduler.scheduleTask("Send Email", 100));
        Thread task2 = new Thread(() -> scheduler.scheduleTask("Generate Report", 200));
        Thread task3 = new Thread(() -> scheduler.scheduleTask("Update Database", 50));
        
        task1.start();
        task2.start();
        task3.start();
        
        // Real-time Example 5: Producer-Consumer Problem
        System.out.println("\n=== Example 5: Producer-Consumer ===");
        
        class SharedQueue {
            private java.util.Queue<String> queue = new java.util.LinkedList<>();
            private int capacity = 3;
            
            public synchronized void produce(String item) {
                while (queue.size() == capacity) {
                    try {
                        wait();
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
                queue.add(item);
                System.out.println("Produced: " + item);
                notifyAll();
            }
            
            public synchronized String consume() {
                while (queue.isEmpty()) {
                    try {
                        wait();
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
                String item = queue.poll();
                System.out.println("Consumed: " + item);
                notifyAll();
                return item;
            }
        }
        
        SharedQueue buffer = new SharedQueue();
        
        Thread producer = new Thread(() -> {
            for (int i = 1; i <= 5; i++) {
                buffer.produce("Item-" + i);
            }
        });
        
        Thread consumer = new Thread(() -> {
            for (int i = 1; i <= 5; i++) {
                buffer.consume();
            }
        });
        
        // Real-time Example 6: Thread Pool simulation
        System.out.println("\n=== Example 6: Thread Pool ===");
        
        class SimpleThreadPool {
            public void executeTasks(int numTasks) {
                System.out.println("Thread pool created with 3 threads");
                System.out.println("Executing " + numTasks + " tasks\n");
                
                for (int i = 1; i <= numTasks; i++) {
                    final int taskId = i;
                    new Thread(() -> {
                        System.out.println("Task-" + taskId + " started by " + Thread.currentThread().getName());
                        try {
                            Thread.sleep(100);
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                        System.out.println("Task-" + taskId + " completed");
                    }).start();
                }
            }
        }
        
        SimpleThreadPool pool = new SimpleThreadPool();
        pool.executeTasks(6);
        
        try {
            Thread.sleep(1000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }
}
