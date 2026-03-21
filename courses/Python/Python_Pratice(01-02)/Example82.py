# Example82.py
# Topic: Pattern Matching — Match Statement Basics

# Match statement (Python 3.10+) - supercharged switch

# === Basic match syntax ===
# match subject:
#     case pattern:
#         code

status = "active"

match status:
    case "pending":
        print("Waiting...")
    case "active":
        print("Running!")
    case "completed":
        print("Done!")
    case _:
        print("Unknown status")

# === Match with different values ===
command = "start"

match command:
    case "start":
        print("Starting process...")
    case "stop":
        print("Stopping process...")
    case "restart":
        print("Restarting...")
    case _:
        print("Unknown command")

# === Match with numbers ===
http_code = 200

match http_code:
    case 200:
        print("OK - Success")
    case 404:
        print("Not Found")
    case 500:
        print("Server Error")
    case _:
        print("Unknown code")

# === Real-world: Order status ===
order_status = "shipped"

match order_status:
    case "pending":
        print("Order is being processed")
    case "paid":
        print("Payment confirmed")
    case "shipped":
        print("Order has been shipped")
    case "delivered":
        print("Order delivered!")
    case _:
        print("Unknown order status")

# === Match in a function ===
def get_status_message(status):
    match status:
        case "open":
            return "Ticket is open"
        case "in_progress":
            return "Ticket is being worked on"
        case "closed":
            return "Ticket is closed"
        case _:
            return "Unknown status"


print(get_status_message("open"))
print(get_status_message("in_progress"))
