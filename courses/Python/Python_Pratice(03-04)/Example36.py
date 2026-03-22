# Example36.py
# Topic: Docstrings - Basic and Various Styles

# This file demonstrates different docstring styles including Google, NumPy,
# and reStructuredText formats.


# Google Style Docstring
def calculate_rectangle_area(length: float, width: float) -> float:
    """Calculate the area of a rectangle.
    
    Args:
        length: The length of the rectangle in units.
        width: The width of the rectangle in units.
        
    Returns:
        The area of the rectangle in square units.
    """
    return length * width

area = calculate_rectangle_area(5.0, 3.0)
print(f"Rectangle area: {area}")    # Rectangle area: 15.0


# Google Style with Raises
def divide_numbers(dividend: float, divisor: float) -> float:
    """Divide two numbers and return the result.
    
    Args:
        dividend: The number to be divided.
        divisor: The number to divide by (must not be zero).
        
    Returns:
        The result of the division.
        
    Raises:
        ValueError: If divisor is zero.
    """
    if divisor == 0:
        raise ValueError("Divisor cannot be zero")
    return dividend / divisor

result = divide_numbers(10.0, 2.0)
print(f"Division result: {result}")    # Division result: 5.0


# Google Style with Example
def greet_user(name: str, greeting: str = "Hello") -> str:
    """Greet a user with a custom greeting.
    
    Args:
        name: The name of the user to greet.
        greeting: The greeting word to use (default: "Hello").
        
    Returns:
        A greeting message string.
        
    Example:
        >>> greet_user("Alice")
        'Hello, Alice!'
        >>> greet_user("Bob", "Hi")
        'Hi, Bob!'
    """
    return f"{greeting}, {name}!"

print(greet_user("Alice"))          # Hello, Alice!
print(greet_user("Bob", "Hi"))      # Hi, Bob!


# NumPy Style Docstring
def calculate_mean(numbers: list[float]) -> float:
    """
    Calculate the arithmetic mean of a list of numbers.
    
    Parameters
    ----------
    numbers : list[float]
        A list of numeric values.
        
    Returns
    -------
    float
        The arithmetic mean of the input numbers.
        
    See Also
    --------
    calculate_median : Calculate the median value.
    calculate_std : Calculate the standard deviation.
    
    Examples
    --------
    >>> calculate_mean([1, 2, 3, 4, 5])
    3.0
    """
    if not numbers:
        return 0.0
    return sum(numbers) / len(numbers)

mean = calculate_mean([10, 20, 30, 40, 50])
print(f"Mean: {mean}")    # Mean: 30.0


# NumPy Style with Multiple Parameters
def find_statistics(data: list[float]) -> dict[str, float]:
    """
    Find statistical measures for a dataset.
    
    Parameters
    ----------
    data : list[float]
        A list of numeric values.
        
    Returns
    -------
    dict[str, float]
        Dictionary containing 'mean', 'median', 'std', 'min', and 'max'.
        
    Notes
    -----
    The standard deviation is calculated using the population formula.
    """
    sorted_data = sorted(data)
    n = len(data)
    
    mean_val = sum(data) / n
    
    # Calculate standard deviation
    variance = sum((x - mean_val) ** 2 for x in data) / n
    std_val = variance ** 0.5
    
    mid = n // 2
    if n % 2 == 0:
        median = (sorted_data[mid - 1] + sorted_data[mid]) / 2
    else:
        median = sorted_data[mid]
    
    return {
        "mean": mean_val,
        "median": median,
        "std": std_val,
        "min": min(data),
        "max": max(data)
    }

stats = find_statistics([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
print(f"Statistics: {stats}")
# Statistics: {'mean': 5.5, 'median': 5.5, 'std': 2.87..., 'min': 1, 'max': 10}


# reStructuredText Style Docstring
def validate_email(email: str) -> bool:
    """Validate an email address format.
    
    :param email: The email address to validate.
    :type email: str
    :returns: True if email is valid, False otherwise.
    :rtype: bool
    :raises ValueError: If email is empty.
    
    .. note::
        This is a basic validation. For production use,
        consider using a dedicated email validation library.
        
    .. warning::
        Does not verify if the email actually exists.
    """
    if not email:
        raise ValueError("Email cannot be empty")
    return "@" in email and "." in email.split("@")[1]

print(f"Valid email: {validate_email('user@example.com')}")    # True
print(f"Invalid email: {validate_email('userexample.com')}")  # False


# reStructuredText Style with Returns
def format_currency(amount: float, currency: str = "USD") -> str:
    """Format a number as currency.
    
    :param amount: The numeric amount.
    :type amount: float
    :param currency: The currency code (default: USD).
    :type currency: str
    :returns: Formatted currency string.
    :rtype: str
    
    .. versionadded:: 1.0
       Added support for multiple currency codes.
    """
    symbols = {"USD": "$", "EUR": "€", "GBP": "£", "JPY": "¥"}
    symbol = symbols.get(currency, currency + " ")
    return f"{symbol}{amount:,.2f}"

print(format_currency(1234.56))       # $1,234.56
print(format_currency(999.99, "EUR"))  # €999.99


# Real-life Example 1: Employee class with Google-style docstring
class Employee:
    """Represents an employee in the company.
    
    Attributes:
        employee_id: Unique identifier for the employee.
        name: Full name of the employee.
        department: Department where the employee works.
        salary: Annual salary in USD.
    """
    
    def __init__(self, employee_id: int, name: str, department: str, salary: float):
        """Initialize an Employee instance.
        
        Args:
            employee_id: Unique identifier for the employee.
            name: Full name of the employee.
            department: Department where the employee works.
            salary: Annual salary in USD.
        """
        self.employee_id = employee_id
        self.name = name
        self.department = department
        self.salary = salary
    
    def get_annual_bonus(self, performance_rating: float) -> float:
        """Calculate the annual bonus based on performance.
        
        Args:
            performance_rating: Rating from 1.0 to 5.0.
            
        Returns:
            Bonus amount in USD.
            
        Raises:
            ValueError: If rating is outside 1.0-5.0 range.
        """
        if not 1.0 <= performance_rating <= 5.0:
            raise ValueError("Performance rating must be between 1.0 and 5.0")
        return self.salary * (performance_rating / 10)
    
    def __str__(self) -> str:
        """Return string representation of the employee."""
        return f"Employee({self.employee_id}): {self.name}, {self.department}"

emp = Employee(101, "Alice Johnson", "Engineering", 85000)
print(emp)                                        # Employee(101): Alice Johnson, Engineering
print(f"Bonus: ${emp.get_annual_bonus(4.5):.2f}")  # Bonus: $38250.00


# Real-life Example 2: Database connection with NumPy-style docstring
class DatabaseConnection:
    """Manages database connections and queries.
    
    Parameters
    ----------
    host : str
        Database server hostname or IP address.
    port : int
        Database server port number.
    database : str
        Name of the database to connect to.
    username : str, optional
        Database username (default: None).
    password : str, optional
        Database password (default: None).
        
    Attributes
    ----------
    connected : bool
        Whether the connection is currently active.
    query_count : int
        Number of queries executed.
        
    Example
    -------
    >>> db = DatabaseConnection("localhost", 5432, "mydb")
    >>> db.connect()
    >>> results = db.execute("SELECT * FROM users")
    """
    
    def __init__(self, host: str, port: int, database: str, username: str = None, password: str = None):
        self.host = host
        self.port = port
        self.database = database
        self.username = username
        self.password = password
        self.connected = False
        self.query_count = 0
    
    def connect(self) -> bool:
        """Establish connection to the database.
        
        Returns
        -------
        bool
            True if connection successful, False otherwise.
        """
        self.connected = True
        return True
    
    def execute(self, query: str) -> list[dict]:
        """Execute a SQL query.
        
        Parameters
        ----------
        query : str
            SQL query string.
            
        Returns
        -------
        list[dict]
            Query results as list of dictionaries.
        """
        self.query_count += 1
        return [{"status": "simulated", "query": query}]

db = DatabaseConnection("localhost", 5432, "production", "admin", "secret")
db.connect()
result = db.execute("SELECT * FROM users")
print(f"Connected: {db.connected}, Queries: {db.query_count}")
# Connected: True, Queries: 1


# Real-life Example 3: API client with reStructuredText docstring
class APIClient:
    """HTTP client for making API requests.
    
    :ivar base_url: Base URL for all API requests.
    :ivar timeout: Request timeout in seconds.
    :ivar headers: Default headers for requests.
    :type headers: dict
    
    .. versionchanged:: 2.0
       Added support for custom headers.
       
    .. versionadded:: 1.5
       Added timeout parameter.
    """
    
    def __init__(self, base_url: str, timeout: int = 30):
        """Initialize the API client.
        
        :param base_url: Base URL for the API.
        :type base_url: str
        :param timeout: Request timeout in seconds.
        :type timeout: int
        """
        self.base_url = base_url
        self.timeout = timeout
        self.headers = {"Content-Type": "application/json"}
    
    def get(self, endpoint: str, params: dict = None) -> dict:
        """Make a GET request to the API.
        
        :param endpoint: API endpoint path.
        :type endpoint: str
        :param params: Query parameters.
        :type params: dict, optional
        :returns: Response data as dictionary.
        :rtype: dict
        """
        return {
            "url": f"{self.base_url}/{endpoint}",
            "method": "GET",
            "params": params or {}
        }
    
    def post(self, endpoint: str, data: dict) -> dict:
        """Make a POST request to the API.
        
        :param endpoint: API endpoint path.
        :type endpoint: str
        :param data: Request body data.
        :type data: dict
        :returns: Response data as dictionary.
        :rtype: dict
        """
        return {
            "url": f"{self.base_url}/{endpoint}",
            "method": "POST",
            "data": data
        }

client = APIClient("https://api.example.com")
response = client.get("users", {"page": 1})
print(f"GET {response['url']}")    # GET https://api.example.com/users
