# Timezone Handling

## What You'll Learn

- Why timezone handling is tricky in Node.js
- How to use Intl.DateTimeFormat for timezone-aware formatting
- How to schedule cron jobs in specific timezones
- How DST (Daylight Saving Time) affects schedules
- How the Temporal API improves timezone handling

## The Timezone Problem

A server in UTC runs `cron.schedule('0 9 * * *', job)`. This runs at 9 AM UTC. But your users are in New York — they expect it at 9 AM Eastern (which is 14:00 UTC in winter, 13:00 UTC in summer due to DST).

## Formatting with Intl

```js
// timezone-format.js — Format dates in different timezones

import cron from 'node-cron';

// Intl.DateTimeFormat — built-in timezone-aware formatting
function formatInTimezone(date, timezone) {
  return new Intl.DateTimeFormat('en-US', {
    timeZone: timezone,
    dateStyle: 'full',
    timeStyle: 'long',
  }).format(date);
}

const now = new Date();

console.log('UTC:        ', formatInTimezone(now, 'UTC'));
console.log('New York:   ', formatInTimezone(now, 'America/New_York'));
console.log('London:     ', formatInTimezone(now, 'Europe/London'));
console.log('Tokyo:      ', formatInTimezone(now, 'Asia/Tokyo'));
console.log('Sydney:     ', formatInTimezone(now, 'Australia/Sydney'));

// Get current hour in a specific timezone
function getHourInTimezone(timezone) {
  const parts = new Intl.DateTimeFormat('en-US', {
    timeZone: timezone,
    hour: 'numeric',
    hour12: false,
  }).formatToParts(new Date());

  return parseInt(parts.find((p) => p.type === 'hour').value);
}

console.log('\nCurrent hour:');
console.log('  New York:', getHourInTimezone('America/New_York'));
console.log('  Tokyo:   ', getHourInTimezone('Asia/Tokyo'));
```

## Cron with Timezone

```js
// timezone-cron.js — Cron jobs in specific timezones

import cron from 'node-cron';

// 9 AM Eastern — runs at 14:00 UTC in EST, 13:00 UTC in EDT
cron.schedule('0 9 * * *', () => {
  const time = new Intl.DateTimeFormat('en-US', {
    timeZone: 'America/New_York',
    timeStyle: 'medium',
  }).format(new Date());
  console.log(`[${time} ET] Daily report sent`);
}, { timezone: 'America/New_York' });

// 9 AM London — runs at 09:00 GMT, 08:00 UTC in BST
cron.schedule('0 9 * * *', () => {
  const time = new Intl.DateTimeFormat('en-US', {
    timeZone: 'Europe/London',
    timeStyle: 'medium',
  }).format(new Date());
  console.log(`[${time} GMT] UK report sent`);
}, { timezone: 'Europe/London' });

console.log('Timezone-specific jobs scheduled');
```

## DST Edge Cases

```js
// dst.js — Handle DST transitions

// Problem: 2:30 AM does not exist on the spring-forward day
// In the US, clocks jump from 1:59 AM to 3:00 AM

// What happens to a job scheduled at 2:30 AM?
// node-cron handles this — it skips the non-existent time

// Problem: 1:30 AM happens TWICE on the fall-back day
// A job at 1:30 AM runs once (the first occurrence)

// Best practice: avoid scheduling during the DST transition hour (2-3 AM)
// Schedule at :00 or :30 outside the transition window

// Safe times (no DST ambiguity):
cron.schedule('0 3 * * *', () => {
  // 3 AM is safe — always exists
}, { timezone: 'America/New_York' });

// Unsafe time (DST transition):
// cron.schedule('30 2 * * *', () => {
//   // 2:30 AM may not exist on spring-forward day
// }, { timezone: 'America/New_York' });
```

## Temporal API (Preview)

The **Temporal** API (Stage 3 proposal) provides better timezone handling:

```js
// temporal-preview.js — Temporal API for timezone handling (future)
// Requires Node.js with --harmony flag or polyfill

// Note: Temporal is not yet in Node.js LTS — this is a preview

// With Temporal, timezone handling is explicit:
// const zoned = Temporal.Now.zonedDateTimeISO('America/New_York');
// const nyTime = zoned.toString();
// const tokyoTime = zoned.withTimeZone('Asia/Tokyo').toString();

// For now, use Intl.DateTimeFormat as shown above
```

## Common Mistakes

### Mistake 1: Assuming Server Timezone

```js
// WRONG — uses server's local timezone
const hour = new Date().getHours();  // Server might be in UTC

// CORRECT — use Intl for timezone-aware values
const hour = new Intl.DateTimeFormat('en-US', {
  timeZone: 'America/New_York',
  hour: 'numeric',
  hour12: false,
}).format(new Date());
```

### Mistake 2: Scheduling During DST Transition

```js
// WRONG — 2:30 AM does not exist on spring-forward day
cron.schedule('30 2 10 3 *', job, { timezone: 'America/New_York' });

// CORRECT — schedule outside the DST transition hour
cron.schedule('0 3 10 3 *', job, { timezone: 'America/New_York' });
```

### Mistake 3: Hardcoding UTC Offset

```js
// WRONG — offset changes with DST
const NY_OFFSET = -5;  // EST only — EDT is -4
const nyHour = (utcHour + NY_OFFSET) % 24;

// CORRECT — let Intl handle DST automatically
const nyHour = new Intl.DateTimeFormat('en-US', {
  timeZone: 'America/New_York',
  hour: 'numeric',
  hour12: false,
}).format(new Date());
```

## Try It Yourself

### Exercise 1: Multi-Timezone Display

Create a function that prints the current time in 5 different timezones. Verify they differ correctly.

### Exercise 2: User Timezone Scheduling

Given a user's timezone (e.g., "Asia/Tokyo"), schedule a notification for 9 AM in their timezone.

### Exercise 3: DST Test

Schedule a job for 2:30 AM on the DST transition date. Observe whether it runs, skips, or errors.

## Next Steps

You understand timezone handling. For using BullMQ for scheduled jobs, continue to [BullMQ Repeatable](../queue-cron/01-bullmq-repeatable.md).
