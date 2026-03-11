// DateTimeAPIDemo - Demonstrates Java Date/Time API (java.time)
// Important for handling dates in backend applications

import java.time.*;
import java.time.format.*;

public class DateTimeAPIDemo {
    
    public static void main(String[] args) {
        System.out.println("=== DATE/TIME API DEMO ===\n");
        
        // LocalDate - date only
        System.out.println("--- LocalDate ---");
        LocalDate today = LocalDate.now();
        System.out.println("Today: " + today);
        System.out.println("Year: " + today.getYear());
        System.out.println("Month: " + today.getMonth());
        System.out.println("Day of month: " + today.getDayOfMonth());
        
        // Specific date
        LocalDate birthday = LocalDate.of(1990, 5, 15);
        System.out.println("Birthday: " + birthday);
        
        // LocalTime - time only
        System.out.println("\n--- LocalTime ---");
        LocalTime now = LocalTime.now();
        System.out.println("Now: " + now);
        System.out.println("Hour: " + now.getHour());
        System.out.println("Minute: " + now.getMinute());
        
        // LocalDateTime - date and time
        System.out.println("\n--- LocalDateTime ---");
        LocalDateTime dateTime = LocalDateTime.now();
        System.out.println("DateTime: " + dateTime);
        
        // ZonedDateTime - with timezone
        System.out.println("\n--- ZonedDateTime ---");
        ZonedDateTime zonedNow = ZonedDateTime.now();
        System.out.println("Zoned: " + zonedNow);
        
        ZonedDateTime tokyo = ZonedDateTime.now(ZoneId.of("Asia/Tokyo"));
        System.out.println("Tokyo: " + tokyo);
        
        // Date manipulation
        System.out.println("\n--- Date Manipulation ---");
        LocalDate nextWeek = today.plusDays(7);
        System.out.println("Next week: " + nextWeek);
        
        LocalDate lastMonth = today.minusMonths(1);
        System.out.println("Last month: " + lastMonth);
        
        // Period - difference between dates
        System.out.println("\n--- Period ---");
        LocalDate start = LocalDate.of(2024, 1, 1);
        LocalDate end = LocalDate.now();
        Period period = Period.between(start, end);
        System.out.println("Days since Jan 1: " + period.getDays());
        System.out.println("Months: " + period.getMonths());
        
        // Duration - difference between times
        System.out.println("\n--- Duration ---");
        LocalTime startTime = LocalTime.of(9, 0);
        LocalTime endTime = LocalTime.of(17, 30);
        Duration duration = Duration.between(startTime, endTime);
        System.out.println("Work hours: " + duration.toHours());
        
        // Formatting
        System.out.println("\n--- Formatting ---");
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd/MM/yyyy");
        System.out.println("Formatted: " + today.format(formatter));
        
        DateTimeFormatter custom = DateTimeFormatter.ofPattern("EEEE, MMMM dd, yyyy");
        System.out.println("Full date: " + today.format(custom));
        
        // Parsing
        System.out.println("\n--- Parsing ---");
        String dateStr = "2024-12-25";
        LocalDate parsed = LocalDate.parse(dateStr);
        System.out.println("Parsed: " + parsed);
        
        // Instant - timestamp
        System.out.println("\n--- Instant ---");
        Instant instant = Instant.now();
        System.out.println("Current instant: " + instant);
        System.out.println("Epoch millis: " + instant.toEpochMilli());
        
        // Angular Integration
        System.out.println("\n=== ANGULAR INTEGRATION ===");
        System.out.println("LocalDateTime -> ISO 8601 String");
        System.out.println("ZonedDateTime -> ISO 8601 with timezone");
        System.out.println("JSON format: '2024-03-15T10:30:00'");
    }
}
