/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Real-World Examples Part 2
 * FILE      : DelegatesEvents_RealWorld_Part2.cs
 * PURPOSE   : More real-world applications including command
 *            pattern, mediator pattern, and pub-sub messaging
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class DelegatesEvents_RealWorld_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Examples Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Command Pattern
            // ═══════════════════════════════════════════════════════════

            // Command pattern encapsulates requests as objects
            // Enables parameterization, queuing, and undo
            // Delegates provide a simplified implementation

            // ── EXAMPLE 1: Simple Command Pattern ────────────────────────
            Console.WriteLine("--- Command Pattern: Simple Commands ---");
            
            var remote = new RemoteControl();
            
            // Add commands
            remote.AddCommand(() => Console.WriteLine("  TV On"));
            remote.AddCommand(() => Console.WriteLine("  Lights On"));
            remote.AddCommand(() => Console.WriteLine("  AC On"));
            
            // Execute all commands
            Console.WriteLine("  Pressing all buttons:");
            remote.ExecuteCommands();

            // ── EXAMPLE 2: Command with Undo ───────────────────────────────
            Console.WriteLine("\n--- Command Pattern: With Undo ---");
            
            var editor = new TextEditor();
            
            // Execute commands with undo capability
            editor.Execute(new InsertTextCommand(editor, "Hello "));
            editor.Execute(new InsertTextCommand(editor, "World"));
            Console.WriteLine($"  Text: '{editor.Text}'");
            
            editor.Undo();
            Console.WriteLine($"  After undo: '{editor.Text}'");
            
            editor.Undo();
            Console.WriteLine($"  After another undo: '{editor.Text}'");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Mediator Pattern
            // ═══════════════════════════════════════════════════════════

            // Mediator centralizes communication between objects
            // Reduces coupling between components
            // Components communicate through mediator

            // ── EXAMPLE 1: Chat Room Mediator ────────────────────────────
            Console.WriteLine("\n--- Mediator Pattern: Chat Room ---");
            
            var chatRoom = new ChatRoomMediator();
            
            var alice = new ChatUser("Alice", chatRoom);
            var bob = new ChatUser("Bob", chatRoom);
            var charlie = new ChatUser("Charlie", chatRoom);
            
            alice.Send("Hello everyone!");
            bob.Send("Hi Alice!");
            charlie.Send("Hey guys!");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Publish-Subscribe Pattern
            // ═══════════════════════════════════════════════════════════

            // Pub-sub decouples publishers from subscribers
            // Subscribers subscribe to specific topics
            // Message broker handles routing

            // ── EXAMPLE 1: Message Broker ────────────────────────────────
            Console.WriteLine("\n--- Publish-Subscribe Pattern ---");
            
            var broker = new MessageBroker();
            
            // Subscribe to topics
            broker.Subscribe("news", msg => Console.WriteLine($"  [News] {msg}"));
            broker.Subscribe("sports", msg => Console.WriteLine($"  [Sports] {msg}"));
            broker.Subscribe("tech", msg => Console.WriteLine($"  [Tech] {msg}"));
            
            // Subscribe to multiple topics
            broker.Subscribe(new[] { "news", "tech" }, msg =>
                Console.WriteLine($"  [News+Tech] Important: {msg}"));
            
            // Publish messages
            broker.Publish("news", "Breaking: Big news story");
            broker.Publish("sports", "Team wins championship");
            broker.Publish("tech", "New phone released");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Strategy Pattern Variants
            // ═══════════════════════════════════════════════════════════

            // Strategy pattern with delegates for validation
            // Runtime selection of algorithms

            // ── EXAMPLE 1: Validation Strategies ────────────────────────
            Console.WriteLine("\n--- Strategy Pattern: Validation ---");
            
            var formValidator = new FormValidator();
            
            // Email validation strategy
            formValidator.SetStrategy(fields => 
                fields["email"]?.Contains("@") == true ? null : "Invalid email");
            
            // Validate with current strategy
            var result = formValidator.Validate(new Dictionary<string, string>
            {
                { "email", "test@example.com" }
            });
            Console.WriteLine($"  Result: {result}");
            
            // Change to required field strategy
            formValidator.SetStrategy(fields =>
            {
                foreach (var field in fields)
                {
                    if (string.IsNullOrEmpty(field.Value))
                        return $"{field.Key} is required";
                }
                return null;
            });
            
            result = formValidator.Validate(new Dictionary<string, string>
            {
                { "email", "" }
            });
            Console.WriteLine($"  Result: {result}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Reactive Extensions Style
            // ═══════════════════════════════════════════════════════════

            // LINQ-style event processing
            // Filter, transform, combine events

            // ── EXAMPLE 1: Event Filtering ───────────────────────────────
            Console.WriteLine("\n--- Reactive Style: Event Filtering ---");
            
            var sensor = new TemperatureSensor();
            
            // Filter and transform events
            sensor.TemperatureChanged += temp =>
            {
                if (temp > 30)
                    Console.WriteLine($"  HOT: {temp}°C");
                else if (temp < 10)
                    Console.WriteLine($"  COLD: {temp}°C");
            };
            
            // Simulate temperature changes
            sensor.UpdateTemperature(25);
            sensor.UpdateTemperature(32);
            sensor.UpdateTemperature(15);
            sensor.UpdateTemperature(8);
            sensor.UpdateTemperature(28);

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Dependency Injection with Delegates
            // ═══════════════════════════════════════════════════════════

            // Use Func delegates for lazy dependency resolution
            // Service locator pattern with delegates

            // ── EXAMPLE 1: Service Locator ──────────────────────────────
            Console.WriteLine("\n--- Service Locator with Delegates ---");
            
            var container = new ServiceContainer();
            
            // Register services
            container.Register<ILogger>( () => new ConsoleLogger());
            container.Register<IDatabase>(() => new MockDatabase());
            container.Register<IAuthService>(() => new AuthService(
                container.Resolve<IDatabase>()));
            
            // Resolve and use services
            var logger = container.Resolve<ILogger>();
            logger.Log("Application started");
            
            var auth = container.Resolve<IAuthService>();
            auth.Login("user", "pass");

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Workflow Engine
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Workflow with Steps ───────────────────────────
            Console.WriteLine("\n--- Real-World: Workflow Engine ---");
            
            var workflow = new WorkflowEngine();
            
            // Define workflow steps
            workflow.AddStep(new ValidationStep());
            workflow.AddStep(new ProcessingStep());
            workflow.AddStep(new NotificationStep());
            
            // Execute workflow
            workflow.Execute(new WorkflowContext { Data = "Test Data" });

            Console.WriteLine("\n=== Real-World Examples Part 2 Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Command Pattern: Remote Control
    // ═══════════════════════════════════════════════════════════

    class RemoteControl
    {
        private List<Action> _commands = new List<Action>();

        public void AddCommand(Action command)
        {
            _commands.Add(command);
        }

        public void ExecuteCommands()
        {
            foreach (var command in _commands)
            {
                command();
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Command Pattern: Text Editor with Undo
    // ═══════════════════════════════════════════════════════════

    interface ICommand
    {
        void Execute();
        void Undo();
    }

    class TextEditor
    {
        public string Text { get; set; } = "";
    }

    class InsertTextCommand : ICommand
    {
        private TextEditor _editor;
        private string _text;

        public InsertTextCommand(TextEditor editor, string text)
        {
            _editor = editor;
            _text = text;
        }

        public void Execute()
        {
            _editor.Text += _text;
        }

        public void Undo()
        {
            _editor.Text = _editor.Text.Substring(0, _editor.Text.Length - _text.Length);
        }
    }

    class TextEditorCommands
    {
        private TextEditor _editor;
        private Stack<ICommand> _history = new Stack<ICommand>();

        public TextEditorCommands(TextEditor editor)
        {
            _editor = editor;
        }

        public void Execute(ICommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var command = _history.Pop();
                command.Undo();
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Mediator Pattern: Chat Room
    // ═══════════════════════════════════════════════════════════

    interface IChatMediator
    {
        void SendMessage(string message, ChatUser sender);
    }

    class ChatRoomMediator : IChatMediator
    {
        private List<ChatUser> _users = new List<ChatUser>();

        public void AddUser(ChatUser user)
        {
            _users.Add(user);
        }

        public void SendMessage(string message, ChatUser sender)
        {
            foreach (var user in _users)
            {
                if (user != sender)
                {
                    user.Receive(message, sender.Name);
                }
            }
        }
    }

    class ChatUser
    {
        public string Name { get; }
        private IChatMediator _mediator;

        public ChatUser(string name, IChatMediator mediator)
        {
            Name = name;
            _mediator = mediator;
            if (_mediator is ChatRoomMediator room)
                room.AddUser(this);
        }

        public void Send(string message)
        {
            Console.WriteLine($"  {Name} sends: {message}");
            _mediator.SendMessage(message, this);
        }

        public void Receive(string message, string sender)
        {
            Console.WriteLine($"  {Name} received from {sender}: {message}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Publish-Subscribe: Message Broker
    // ═══════════════════════════════════════════════════════════

    class MessageBroker
    {
        private Dictionary<string, List<Action<string>>> _subscribers = new Dictionary<string, List<Action<string>>>();

        public void Subscribe(string topic, Action<string> handler)
        {
            if (!_subscribers.ContainsKey(topic))
            {
                _subscribers[topic] = new List<Action<string>>();
            }
            _subscribers[topic].Add(handler);
        }

        public void Subscribe(string[] topics, Action<string> handler)
        {
            foreach (var topic in topics)
            {
                Subscribe(topic, handler);
            }
        }

        public void Publish(string topic, string message)
        {
            if (_subscribers.ContainsKey(topic))
            {
                foreach (var handler in _subscribers[topic])
                {
                    handler(message);
                }
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Strategy Pattern: Form Validator
    // ═══════════════════════════════════════════════════════════

    class FormValidator
    {
        private Func<Dictionary<string, string>, string> _strategy;

        public void SetStrategy(Func<Dictionary<string, string>, string> strategy)
        {
            _strategy = strategy;
        }

        public string Validate(Dictionary<string, string> fields)
        {
            return _strategy?.Invoke(fields) ?? "Valid";
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Reactive Style: Temperature Sensor
    // ═══════════════════════════════════════════════════════════

    class TemperatureSensor
    {
        public event Action<int> TemperatureChanged;

        public void UpdateTemperature(int temp)
        {
            TemperatureChanged?.Invoke(temp);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Service Locator
    // ═══════════════════════════════════════════════════════════

    interface ILogger
    {
        void Log(string message);
    }

    interface IDatabase
    {
        void Query(string sql);
    }

    interface IAuthService
    {
        void Login(string user, string pass);
    }

    class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"  [LOG] {message}");
    }

    class MockDatabase : IDatabase
    {
        public void Query(string sql) => Console.WriteLine($"  [DB] Executing: {sql}");
    }

    class AuthService : IAuthService
    {
        private IDatabase _db;

        public AuthService(IDatabase db)
        {
            _db = db;
        }

        public void Login(string user, string pass)
        {
            _db.Query($"SELECT * FROM Users WHERE username='{user}'");
            Console.WriteLine($"  [AUTH] User '{user}' logged in");
        }
    }

    class ServiceContainer
    {
        private Dictionary<Type, Func<object>> _services = new Dictionary<Type, Func<object>>();

        public void Register<T>(Func<T> factory) where T : class
        {
            _services[typeof(T)] = () => factory();
        }

        public T Resolve<T>() where T : class
        {
            return _services.ContainsKey(typeof(T)) 
                ? _services[typeof(T)]() as T 
                : null;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Workflow Engine
    // ═══════════════════════════════════════════════════════════

    class WorkflowContext
    {
        public string Data { get; set; }
        public bool IsValid { get; set; } = true;
        public string Result { get; set; }
    }

    interface IWorkflowStep
    {
        void Execute(WorkflowContext context);
    }

    class ValidationStep : IWorkflowStep
    {
        public void Execute(WorkflowContext context)
        {
            Console.WriteLine($"  Step 1: Validating '{context.Data}'");
            context.IsValid = !string.IsNullOrEmpty(context.Data);
        }
    }

    class ProcessingStep : IWorkflowStep
    {
        public void Execute(WorkflowContext context)
        {
            Console.WriteLine($"  Step 2: Processing '{context.Data}'");
            context.Result = $"Processed: {context.Data.ToUpper()}";
        }
    }

    class NotificationStep : IWorkflowStep
    {
        public void Execute(WorkflowContext context)
        {
            Console.WriteLine($"  Step 3: Notifying with result '{context.Result}'");
        }
    }

    class WorkflowEngine
    {
        private List<IWorkflowStep> _steps = new List<IWorkflowStep>();

        public void AddStep(IWorkflowStep step)
        {
            _steps.Add(step);
        }

        public void Execute(WorkflowContext context)
        {
            foreach (var step in _steps)
            {
                if (!context.IsValid)
                {
                    Console.WriteLine("  Workflow aborted - validation failed");
                    break;
                }
                step.Execute(context);
            }
        }
    }
}
