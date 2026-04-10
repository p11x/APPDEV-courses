/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Delegates and Events - Event Basics Part 2
 * FILE      : EventBasics_Part2.cs
 * PURPOSE   : Advanced event patterns including custom event
 *            accessors, event conventions, and best practices
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._03_Advanced_OOP._02_Delegates_Events
{
    class EventBasics_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Event Basics Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Custom Event Accessors
            // ═══════════════════════════════════════════════════════════

            // Events can have custom add/remove accessors
            // Provides control over how handlers are added/removed
            // Useful for validation or logging

            // ── EXAMPLE 1: Basic Custom Accessors ───────────────────────
            Console.WriteLine("--- Custom Event Accessors ---");
            
            var publisher = new CustomAccessorPublisher();
            
            // Subscribe - goes through custom add accessor
            publisher.ValueChanged += (oldVal, newVal) =>
                Console.WriteLine($"  Value changed: {oldVal} -> {newVal}");
            
            publisher.SetValue(100);
            publisher.SetValue(200);

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Event Validation with Accessors
            // ═══════════════════════════════════════════════════════════

            // Custom accessors allow validation before subscription
            // Can prevent memory leaks with weak references
            // Can throttle or batch subscriptions

            // ── EXAMPLE 1: Validation in Accessors ───────────────────────
            Console.WriteLine("\n--- Validation in Accessors ---");
            
            var validator = new ValidatingPublisher();
            
            // This will be accepted
            validator.DataChanged += (data) => Console.WriteLine($"  Handler 1: {data}");
            
            // This will be accepted
            validator.DataChanged += (data) => Console.WriteLine($"  Handler 2: {data}");
            
            // Try to add null - would be rejected in custom accessor
            // validator.DataChanged += null;  // Would fail if we added validation
            
            validator.SetData("Test Data");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Event Conventions
            // ╌══════════════════════════════════════════════════════════

            // .NET conventions for events:
            // - Use EventHandler or EventHandler<T> delegate
            // - EventArgs-derived class for event data
            // - Naming: On[Event] for raising method
            // - Protected virtual for raising method

            // ── EXAMPLE 1: Standard .NET Event Pattern ───────────────────
            Console.WriteLine("\n--- Standard .NET Event Pattern ---");
            
            var document = new Document();
            
            // Subscribe using EventHandler pattern
            document.Saved += (sender, e) =>
                Console.WriteLine($"  Document saved: {e.FileName}");
            document.Modified += (sender, e) =>
                Console.WriteLine($"  Document modified: {e.FileName}");
            
            document.Save("report.docx");
            document.Modify();

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Protected Virtual Raise Method
            // ═══════════════════════════════════════════════════════════

            // Convention: protected virtual On[EventName] method
            // Allows derived classes to raise event
            // Follows standard .NET patterns

            // ── EXAMPLE 1: OnEventName Pattern ────────────────────────────
            Console.WriteLine("\n--- OnEventName Pattern ---");
            
            var button = new ModernButton();
            button.Clicked += (sender, e) =>
                Console.WriteLine($"  Button clicked at {e.ClickTime}");
            
            button.Click();  // Internally calls OnClicked

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Event-Driven Property Pattern
            // ═══════════════════════════════════════════════════════════

            // Properties can raise events when value changes
            // Common pattern in UI frameworks

            // ── EXAMPLE 1: Property with Change Event ────────────────────
            Console.WriteLine("\n--- Property Change Events ---");
            
            var person = new Person();
            person.NameChanged += (oldName, newName) =>
                Console.WriteLine($"  Name changed: '{oldName}' -> '{newName}'");
            
            person.Name = "Alice";
            person.Name = "Bob";
            person.Name = "Alice";  // No change, no event

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Event Cancellation Support
            // ═══════════════════════════════════════════════════════════

            // Some events support cancellation
            // Use CancelEventArgs for this pattern

            // ── EXAMPLE 1: Cancellation Events ───────────────────────────
            Console.WriteLine("\n--- Cancellation Events ---");
            
            var formCloser = new FormCloser();
            formCloser.Closing += (sender, e) =>
            {
                Console.WriteLine($"  Closing form: {e.FormName}");
                if (e.FormName == "Unsaved")
                {
                    e.Cancel = true;  // Cancel the close
                    Console.WriteLine("  Close cancelled!");
                }
            };
            
            formCloser.Close("Saved");   // Will close
            formCloser.Close("Unsaved"); // Will be cancelled

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World: Validation System
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Form Validation Events ───────────────────────
            Console.WriteLine("\n--- Real-World: Form Validation ---");
            
            var form = new InputForm();
            
            form.Validating += (sender, e) =>
            {
                if (string.IsNullOrEmpty(e.Value))
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Value cannot be empty";
                }
                else if (e.Value.Length < 3)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Value must be at least 3 characters";
                }
            };
            
            form.Validate("");       // Invalid
            form.Validate("AB");     // Invalid
            form.Validate("ABC");    // Valid

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Real-World: State Machine
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: State Transitions ──────────────────────────────
            Console.WriteLine("\n--- Real-World: State Machine ---");
            
            var order = new Order();
            
            order.StateChanged += (oldState, newState) =>
                Console.WriteLine($"  Order {oldState} -> {newState}");
            
            order.Submit();      // Pending -> Submitted
            order.Process();     // Submitted -> Processing
            order.Ship();        // Processing -> Shipped
            order.Complete();    // Shipped -> Delivered

            Console.WriteLine("\n=== Event Basics Part 2 Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom Event Accessors
    // ═══════════════════════════════════════════════════════════

    class CustomAccessorPublisher
    {
        // Declare event with custom accessors
        public event Action<int, int> ValueChanged
        {
            add
            {
                Console.WriteLine($"  Adding handler (count: {value.GetInvocationList()?.Length ?? 0})");
            }
            remove
            {
                Console.WriteLine($"  Removing handler");
            }
        }

        private int _value;

        public void SetValue(int newValue)
        {
            int oldValue = _value;
            _value = newValue;
            ValueChanged?.Invoke(oldValue, newValue);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Validation in Event Accessors
    // ═══════════════════════════════════════════════════════════

    class ValidatingPublisher
    {
        private const int MaxHandlers = 5;
        private int _handlerCount;

        public event Action<string> DataChanged
        {
            add
            {
                if (_handlerCount >= MaxHandlers)
                {
                    Console.WriteLine("  Cannot add more handlers (limit reached)");
                    return;
                }
                _handlerCount++;
            }
            remove
            {
                _handlerCount--;
            }
        }

        public void SetData(string data)
        {
            Console.WriteLine($"  Setting data: {data}");
            DataChanged?.Invoke(data);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Standard .NET Event Pattern
    // ═══════════════════════════════════════════════════════════

    class DocumentEventArgs : EventArgs
    {
        public string FileName { get; }
        
        public DocumentEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }

    class Document
    {
        private string _fileName = "";

        // Use EventHandler<T> for standard pattern
        public event EventHandler<DocumentEventArgs> Saved;
        public event EventHandler<DocumentEventArgs> Modified;

        public void Save(string fileName)
        {
            _fileName = fileName;
            OnSaved();
        }

        public void Modify()
        {
            OnModified();
        }

        // Convention: protected virtual OnEventName
        protected virtual void OnSaved()
        {
            Saved?.Invoke(this, new DocumentEventArgs(_fileName));
        }

        protected virtual void OnModified()
        {
            Modified?.Invoke(this, new DocumentEventArgs(_fileName));
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Modern Button with EventArgs
    // ═══════════════════════════════════════════════════════════

    class ClickEventArgs : EventArgs
    {
        public DateTime ClickTime { get; }
        
        public ClickEventArgs()
        {
            ClickTime = DateTime.Now;
        }
    }

    class ModernButton
    {
        public event EventHandler<ClickEventArgs> Clicked;

        public void Click()
        {
            OnClicked();
        }

        protected virtual void OnClicked()
        {
            Clicked?.Invoke(this, new ClickEventArgs());
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Property Change Events
    // ═══════════════════════════════════════════════════════════

    class Person
    {
        private string _name = "";

        public event Action<string, string> NameChanged;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    string oldName = _name;
                    _name = value;
                    NameChanged?.Invoke(oldName, value);
                }
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Cancellation Events
    // ═══════════════════════════════════════════════════════════

    class CancelEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public string FormName { get; }

        public CancelEventArgs(string formName)
        {
            FormName = formName;
            Cancel = false;
        }
    }

    class FormCloser
    {
        public event EventHandler<CancelEventArgs> Closing;

        public void Close(string formName)
        {
            var args = new CancelEventArgs(formName);
            Closing?.Invoke(this, args);
            
            if (!args.Cancel)
            {
                Console.WriteLine($"  Form '{formName}' closed");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Form Validation
    // ═══════════════════════════════════════════════════════════

    class ValidationEventArgs : EventArgs
    {
        public string Value { get; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public ValidationEventArgs(string value)
        {
            Value = value;
            IsValid = true;
            ErrorMessage = "";
        }
    }

    class InputForm
    {
        public event EventHandler<ValidationEventArgs> Validating;

        public void Validate(string value)
        {
            var args = new ValidationEventArgs(value);
            Validating?.Invoke(this, args);
            
            if (args.IsValid)
            {
                Console.WriteLine($"  '{value}' is VALID");
            }
            else
            {
                Console.WriteLine($"  '{value}' is INVALID: {args.ErrorMessage}");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: State Machine
    // ═══════════════════════════════════════════════════════════

    enum OrderState
    {
        Pending,
        Submitted,
        Processing,
        Shipped,
        Delivered
    }

    class Order
    {
        private OrderState _state = OrderState.Pending;

        public event Action<OrderState, OrderState> StateChanged;

        public void Submit()
        {
            ChangeState(OrderState.Submitted);
        }

        public void Process()
        {
            ChangeState(OrderState.Processing);
        }

        public void Ship()
        {
            ChangeState(OrderState.Shipped);
        }

        public void Complete()
        {
            ChangeState(OrderState.Delivered);
        }

        private void ChangeState(OrderState newState)
        {
            if (_state != newState)
            {
                var oldState = _state;
                _state = newState;
                StateChanged?.Invoke(oldState, newState);
            }
        }
    }
}
