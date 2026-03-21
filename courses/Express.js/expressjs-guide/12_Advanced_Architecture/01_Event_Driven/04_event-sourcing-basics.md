# Event Sourcing Basics

## 📌 What You'll Learn

- What event sourcing is and how it differs from CRUD
- Understanding the event store as an append-only log
- Replaying events to reconstruct state
- When to use event sourcing and when to avoid it

## 🧠 Concept Explained (Plain English)

Traditional applications store the current state — like a photo of your data at a moment in time. Every time something changes, you overwrite the old value.

**Event sourcing** flips this: instead of storing the current state, you store every change as a sequence of events. The current state is derived by replaying all events from the beginning.

Think of it like a bank account statement:
- **Traditional**: "Current balance: $500"
- **Event sourcing**: "+$100, -$50, +$200, -$75, +$325 = $500"

If you have all the events, you can always calculate the current balance. You can also go back in time ("what was the balance on January 15th?") or replay events to debug issues.

**Key concepts:**
- **Event Store**: A database that only appends (like Redis lists or a specialized DB)
- **Aggregate**: The entity being tracked (e.g., Order, Account)
- **Command**: A request to change state (e.g., "place order")
- **Event**: A record of something that happened (e.g., "order placed")

**Benefits:**
- Complete audit trail
- Easy debugging with event replay
- Temporal queries (state at any point in time)
- Scalability (append-only is fast)

**Challenges:**
- Learning curve for team
- Event schema changes over time
- More complex than simple CRUD

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// ============================================
// Simple In-Memory Event Store (use Redis in production)
// ============================================

class EventStore {
  constructor() {
    this.events = [];
  }
  
  // Append an event
  append(event) {
    const storedEvent = {
      id: this.events.length + 1,
      ...event,
      timestamp: new Date().toISOString()
    };
    this.events.push(storedEvent);
    return storedEvent;
  }
  
  // Get all events for an aggregate
  getEvents(aggregateId) {
    return this.events.filter(e => e.aggregateId === aggregateId);
  }
  
  // Get all events (for rebuilding state)
  getAllEvents() {
    return [...this.events];
  }
  
  // Get events since a version (for replay)
  getEventsSince(aggregateId, version) {
    return this.events.filter(
      e => e.aggregateId === aggregateId && e.version > version
    );
  }
  
  // Clear (for testing)
  clear() {
    this.events = [];
  }
}

const eventStore = new EventStore();


// ============================================
// Event Types
// ============================================

const EventTypes = {
  ACCOUNT_CREATED: 'AccountCreated',
  DEPOSIT: 'Deposit',
  WITHDRAWAL: 'Withdrawal',
  TRANSFER_SENT: 'TransferSent',
  TRANSFER_RECEIVED: 'TransferReceived'
};


// ============================================
// Aggregate: Bank Account
// ============================================

class BankAccount {
  constructor(accountId) {
    this.id = accountId;
    this.balance = 0;
    this.version = 0;
  }
  
  // Apply an event to get new state
  apply(event) {
    switch (event.type) {
      case EventTypes.ACCOUNT_CREATED:
        this.balance = 0;
        break;
      case EventTypes.DEPOSIT:
        this.balance += event.amount;
        break;
      case EventTypes.WITHDRAWAL:
        this.balance -= event.amount;
        break;
      case EventTypes.TRANSFER_SENT:
        this.balance -= event.amount;
        break;
      case EventTypes.TRANSFER_RECEIVED:
        this.balance += event.amount;
        break;
    }
    this.version++;
  }
  
  // Reconstruct from events
  static fromEvents(events) {
    const account = new BankAccount(events[0]?.aggregateId || 'unknown');
    events.forEach(event => account.apply(event));
    return account;
  }
}


// ============================================
// Command Handlers (create events)
// ============================================

function createAccount(accountId, ownerName) {
  // Business logic: validate
  if (!ownerName) {
    throw new Error('Owner name required');
  }
  
  // Create event
  const event = eventStore.append({
    type: EventTypes.ACCOUNT_CREATED,
    aggregateId: accountId,
    aggregateType: 'BankAccount',
    ownerName,
    version: 0
  });
  
  return event;
}

function deposit(accountId, amount) {
  // Get current state
  const events = eventStore.getEvents(accountId);
  const account = BankAccount.fromEvents(events);
  
  // Business logic: validate
  if (amount <= 0) {
    throw new Error('Deposit amount must be positive');
  }
  
  // Create event
  const event = eventStore.append({
    type: EventTypes.DEPOSIT,
    aggregateId: accountId,
    aggregateType: 'BankAccount',
    amount,
    version: account.version + 1
  });
  
  return event;
}

function withdraw(accountId, amount) {
  const events = eventStore.getEvents(accountId);
  const account = BankAccount.fromEvents(events);
  
  // Business logic: check sufficient funds
  if (account.balance < amount) {
    throw new Error('Insufficient funds');
  }
  
  const event = eventStore.append({
    type: EventTypes.WITHDRAWAL,
    aggregateId: accountId,
    aggregateType: 'BankAccount',
    amount,
    version: account.version + 1
  });
  
  return event;
}

function transfer(fromId, toId, amount) {
  const fromEvents = eventStore.getEvents(fromId);
  const fromAccount = BankAccount.fromEvents(fromEvents);
  
  // Business logic
  if (fromAccount.balance < amount) {
    throw new Error('Insufficient funds for transfer');
  }
  
  if (fromId === toId) {
    throw new Error('Cannot transfer to same account');
  }
  
  // Two events for a transfer (atomic in real systems)
  eventStore.append({
    type: EventTypes.TRANSFER_SENT,
    aggregateId: fromId,
    amount,
    toId,
    version: fromAccount.version + 1
  });
  
  const toEvents = eventStore.getEvents(toId);
  const toAccount = BankAccount.fromEvents(toEvents);
  
  eventStore.append({
    type: EventTypes.TRANSFER_RECEIVED,
    aggregateId: toId,
    amount,
    fromId,
    version: toAccount.version + 1
  });
  
  return { fromId, toId, amount };
}


// ============================================
// Express Routes (API)
// ============================================

app.use(express.json());

// Get account state (reconstruct from events)
app.get('/api/accounts/:id', (req, res) => {
  const accountId = req.params.id;
  const events = eventStore.getEvents(accountId);
  
  if (events.length === 0) {
    return res.status(404).json({ error: 'Account not found' });
  }
  
  const account = BankAccount.fromEvents(events);
  
  res.json({
    accountId: account.id,
    balance: account.balance,
    version: account.version,
    events: events.map(e => ({
      type: e.type,
      amount: e.amount,
      timestamp: e.timestamp
    }))
  });
});

// Get account history (audit trail)
app.get('/api/accounts/:id/history', (req, res) => {
  const events = eventStore.getEvents(req.params.id);
  
  res.json({
    accountId: req.params.id,
    eventCount: events.length,
    events
  });
});

// Create account
app.post('/api/accounts', (req, res) => {
  const { accountId, ownerName } = req.body || {};
  
  try {
    const event = createAccount(accountId, ownerName);
    res.status(201).json({ 
      message: 'Account created', 
      event 
    });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Deposit
app.post('/api/accounts/:id/deposit', (req, res) => {
  const { amount } = req.body || {};
  
  try {
    const event = deposit(req.params.id, amount);
    res.json({ message: 'Deposit successful', event });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Withdraw
app.post('/api/accounts/:id/withdraw', (req, res) => {
  const { amount } = req.body || {};
  
  try {
    const event = withdraw(req.params.id, amount);
    res.json({ message: 'Withdrawal successful', event });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Transfer
app.post('/api/transfer', (req, res) => {
  const { fromId, toId, amount } = req.body || {};
  
  try {
    const result = transfer(fromId, toId, amount);
    res.json({ message: 'Transfer successful', ...result });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Get all events (for debugging)
app.get('/api/events', (req, res) => {
  res.json({ events: eventStore.getAllEvents() });
});

// Time travel: get balance at a point in time
app.get('/api/accounts/:id/at-time', (req, res) => {
  const { timestamp } = req.query;
  const cutoffTime = new Date(timestamp);
  
  const allEvents = eventStore.getEvents(req.params.id);
  const eventsAtTime = allEvents.filter(
    e => new Date(e.timestamp) <= cutoffTime
  );
  
  const account = BankAccount.fromEvents(eventsAtTime);
  
  res.json({
    accountId: req.params.id,
    balance: account.balance,
    atTime: cutoffTime,
    eventsIncluded: eventsAtTime.length
  });
});

// Reset (for testing)
app.post('/api/reset', (req, res) => {
  eventStore.clear();
  res.json({ message: 'Event store cleared' });
});


app.get('/health', (req, res) => {
  res.json({ 
    status: 'ok',
    totalEvents: eventStore.getAllEvents().length
  });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log('\n📝 Example usage:');
  console.log('1. Create account: POST /api/accounts {accountId: "acc-1", ownerName: "John"}');
  console.log('2. Deposit: POST /api/accounts/acc-1/deposit {amount: 100}');
  console.log('3. Check balance: GET /api/accounts/acc-1');
  console.log('4. Check history: GET /api/accounts/acc-1/history');
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 12-35 | `EventStore` class | Simple append-only event storage |
| 40-48 | Event types | Constants for event names |
| 52-78 | `BankAccount` aggregate | Applies events to reconstruct state |
| 84-93 | `createAccount` | Creates AccountCreated event |
| 96-109 | `deposit` | Creates Deposit event |
| 112-127 | `withdraw` | Creates Withdrawal event (with validation) |
| 130-153 | `transfer` | Creates two events for atomic transfer |
| 170-183 | Account state endpoint | Rebuilds state from events |
| 186-193 | History endpoint | Full audit trail |
| 196-206 | Create route | HTTP handler for creating accounts |
| 209-230 | Transfer routes | Deposit/withdraw handlers |
| 236-253 | Time travel query | Get state at any point in time |

## ⚠️ Common Mistakes

### 1. Using event sourcing for everything

**What it is**: Applying event sourcing to simple CRUD operations.

**Why it happens**: Over-enthusiasm about new patterns.

**How to fix it**: Use event sourcing when you need audit trails, temporal queries, or complex business workflows. Use regular CRUD for simple data.

### 2. Not handling event schema evolution

**What it is**: Old events can't be applied after code changes.

**Why it happens**: Event schemas change over time without migration paths.

**How to fix it**: Use event versioning and upgraders, or design events carefully upfront.

### 3. Storing too much in events

**What it is**: Large payload in events, performance issues.

**Why it happens**: Copying entire objects instead of deltas.

**How to fix it**: Store minimal data in events, reference documents in object storage.

## ✅ Quick Recap

- Event sourcing stores changes as a sequence of events instead of current state
- Current state is reconstructed by replaying all events
- Provides complete audit trail and temporal queries
- Good for domains with complex workflows and audit requirements
- Event store is append-only — never modify or delete events
- Use aggregates to apply events and rebuild state

## 🔗 What's Next

Now move to the Design Patterns section to learn about [CQRS Pattern](./../02_Design_Patterns/01_cqrs-pattern.md).
