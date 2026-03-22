/*
 * SUB TOPIC: Date and Time API in Java
 * 
 * DEFINITION:
 * The Date and Time API (java.time) was introduced in Java 8 to provide comprehensive support for 
 * date and time operations. It includes classes like LocalDate, LocalTime, LocalDateTime, ZonedDateTime, 
 * Duration, Period, and formatting/parsing utilities.
 * 
 * FUNCTIONALITIES:
 * 1. LocalDate - Date only (year, month, day)
 * 2. LocalTime - Time only (hour, minute, second)
 * 3. LocalDateTime - Date and time together
 * 4. ZonedDateTime - Date/time with timezone
 * 5. Duration - Time-based amount (hours, minutes, seconds)
 * 6. Period - Date-based amount (years, months, days)
 * 7. DateTimeFormatter - Format and parse dates
 */

import java.time.*; // Import java.time classes
import java.time.format.*; // Import formatting classes
import java.time.temporal.*; // Import temporal classes

public class Example41 {
    public static void main(String[] args) {
        
        // Topic Explanation: LocalDate
        
        // Get current date
        System.out.println("=== LocalDate ===");
        LocalDate today = LocalDate.now();
        System.out.println("Today: " + today);
        
        // Create specific date
        LocalDate birthday = LocalDate.of(2000, 1, 15);
        System.out.println("Birthday: " + birthday);
        
        // Get date components
        System.out.println("Year: " + today.getYear());
        System.out.println("Month: " + today.getMonth());
        System.out.println("Day of month: " + today.getDayOfMonth());
        
        // Add/subtract days
        LocalDate tomorrow = today.plusDays(1);
        LocalDate yesterday = today.minusDays(1);
        System.out.println("Tomorrow: " + tomorrow);
        System.out.println("Yesterday: " + yesterday);
        
        // Topic Explanation: LocalTime
        
        System.out.println("\n=== LocalTime ===");
        LocalTime now = LocalTime.now();
        System.out.println("Current time: " + now);
        
        // Create specific time
        LocalTime meetingTime = LocalTime.of(14, 30); // 2:30 PM
        System.out.println("Meeting time: " + meetingTime);
        
        // Get time components
        System.out.println("Hour: " + now.getHour());
        System.out.println("Minute: " + now.getMinute());
        System.out.println("Second: " + now.getSecond());
        
        // Add/subtract time
        LocalTime later = now.plusHours(2);
        System.out.println("2 hours later: " + later);
        
        // Topic Explanation: LocalDateTime
        
        System.out.println("\n=== LocalDateTime ===");
        LocalDateTime nowWithTime = LocalDateTime.now();
        System.out.println("Now: " + nowWithTime);
        
        // Create specific date-time
        LocalDateTime examDateTime = LocalDateTime.of(2024, 6, 15, 9, 0);
        System.out.println("Exam: " + examDateTime);
        
        // Formatting dates
        System.out.println("\n=== DateTime Formatting ===");
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd/MM/yyyy");
        String formatted = today.format(formatter);
        System.out.println("Formatted: " + formatted);
        
        DateTimeFormatter timeFormatter = DateTimeFormatter.ofPattern("hh:mm a");
        String timeFormatted = now.format(timeFormatter);
        System.out.println("Time formatted: " + timeFormatted);
        
        // Parsing dates
        System.out.println("\n=== Parsing Dates ===");
        LocalDate parsedDate = LocalDate.parse("25/12/2024", formatter);
        System.out.println("Parsed: " + parsedDate);
        
        // Duration - Time-based amount
        System.out.println("\n=== Duration ===");
        Duration dur = Duration.between(LocalTime.of(9, 0), LocalTime.of(17, 30));
        System.out.println("Work hours: " + dur.toHours() + " hours");
        System.out.println("In minutes: " + dur.toMinutes() + " minutes");
        
        // Period - Date-based amount
        System.out.println("\n=== Period ===");
        LocalDate startDate = LocalDate.of(2024, 1, 1);
        LocalDate endDate = LocalDate.of(2024, 12, 31);
        
        Period period = Period.between(startDate, endDate);
        System.out.println("Year: " + period.getYears());
        System.out.println("Months: " + period.getMonths());
        System.out.println("Days: " + period.getDays());
        
        // Real-time Example 1: Age Calculator
        System.out.println("\n=== Example 1: Age Calculator ===");
        LocalDate birthDate = LocalDate.of(2000, 5, 15);
        LocalDate currentDate = LocalDate.now();
        
        Period age = Period.between(birthDate, currentDate);
        System.out.println("Age: " + age.getYears() + " years, " + age.getMonths() + " months, " + age.getDays() + " days");
        
        // Real-time Example 2: Days until event
        System.out.println("\n=== Example 2: Days Until Event ===");
        LocalDate eventDate = LocalDate.of(2024, 12, 25); // Christmas
        long daysUntil = ChronoUnit.DAYS.between(currentDate, eventDate);
        System.out.println("Days until Christmas: " + daysUntil);
        
        // Real-time Example 3: Working hours calculation
        System.out.println("\n=== Example 3: Working Hours ===");
        LocalTime startWork = LocalTime.of(9, 0);
        LocalTime endWork = LocalTime.of(18, 0);
        
        Duration workDay = Duration.between(startWork, endWork);
        System.out.println("Work day: " + workDay.toHours() + " hours");
        
        // Subtract break time
        Duration breakTime = Duration.ofMinutes(60);
        Duration actualWork = workDay.minus(breakTime);
        System.out.println("Actual work: " + actualWork.toHours() + " hours");
        
        // Real-time Example 4: Subscription expiry check
        System.out.println("\n=== Example 4: Subscription Check ===");
        LocalDate subscriptionStart = LocalDate.now().minusDays(30);
        LocalDate subscriptionEnd = subscriptionStart.plusDays(30);
        
        System.out.println("Subscription started: " + subscriptionStart);
        System.out.println("Subscription ends: " + subscriptionEnd);
        
        boolean isActive = subscriptionEnd.isAfter(LocalDate.now());
        System.out.println("Subscription active: " + isActive);
        
        // Days remaining
        long daysRemaining = ChronoUnit.DAYS.between(LocalDate.now(), subscriptionEnd);
        System.out.println("Days remaining: " + daysRemaining);
        
        // Real-time Example 5: Meeting scheduler
        System.out.println("\n=== Example 5: Meeting Scheduler ===");
        LocalDateTime meeting = LocalDateTime.of(2024, 3, 20, 14, 30);
        
        DateTimeFormatter meetingFormatter = DateTimeFormatter.ofPattern("EEEE, MMMM dd, yyyy 'at' hh:mm a");
        System.out.println("Meeting scheduled for: " + meeting.format(meetingFormatter));
        
        // Check if meeting is in future
        boolean isFuture = meeting.isAfter(LocalDateTime.now());
        System.out.println("Is in future: " + isFuture);
        
        // Real-time Example 6: Timezone handling
        System.out.println("\n=== Example 6: Timezone ===");
        
        // Get current time in different zones
        ZoneId newYork = ZoneId.of("America/New_York");
        ZoneId london = ZoneId.of("Europe/London");
        ZoneId tokyo = ZoneId.of("Asia/Tokyo");
        
        ZonedDateTime nowNY = ZonedDateTime.now(newYork);
        ZonedDateTime nowLondon = ZonedDateTime.now(london);
        ZonedDateTime nowTokyo = ZonedDateTime.now(tokyo);
        
        System.out.println("New York: " + nowNY.format(DateTimeFormatter.ofPattern("HH:mm")));
        System.out.println("London: " + nowLondon.format(DateTimeFormatter.ofPattern("HH:mm")));
        System.out.println("Tokyo: " + nowTokyo.format(DateTimeFormatter.ofPattern("HH:mm")));
        
        // Bonus: Date comparison
        System.out.println("\n=== Bonus: Date Comparison ===");
        LocalDate date1 = LocalDate.of(2024, 1, 15);
        LocalDate date2 = LocalDate.of(2024, 1, 20);
        LocalDate date3 = LocalDate.of(2024, 1, 15);
        
        System.out.println(date1 + " is before " + date2 + ": " + date1.isBefore(date2));
        System.out.println(date1 + " is after " + date2 + ": " + date1.isAfter(date2));
        System.out.println(date1 + " is equal to " + date3 + ": " + date1.isEqual(date3));
    }
}
