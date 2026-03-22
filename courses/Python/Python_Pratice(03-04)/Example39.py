# Example39.py
# Topic: Comprehensive Docstrings and Annotations - Real-World Examples

# This file demonstrates real-world applications of docstrings and type annotations
# in practical scenarios like APIs, data processing, and libraries.


from typing import Optional, Dict, List, Any, Tuple, Callable
import json


# Real-world Example 1: REST API Endpoint with Full Documentation
class UserAPI:
    """REST API handler for user management operations.
    
    This class provides methods for creating, reading, updating,
    and deleting user resources in the system.
    
    Attributes:
        base_url: Base URL for API endpoints.
        version: API version number.
    
    Example
    -------
    >>> api = UserAPI()
    >>> users = api.get_users()
    >>> user = api.get_user(123)
    """
    
    def __init__(self, base_url: str = "https://api.example.com", version: str = "v1"):
        """Initialize the User API client.
        
        Args:
            base_url: Base URL for API endpoints.
            version: API version string.
        """
        self.base_url = base_url
        self.version = version
    
    def get_users(self, limit: int = 10, offset: int = 0) -> List[Dict[str, Any]]:
        """Retrieve a list of users with pagination.
        
        Args:
            limit: Maximum number of users to return (default: 10).
            offset: Number of users to skip (default: 0).
            
        Returns:
            List of user dictionaries containing id, name, and email.
            
        Raises:
            ValueError: If limit is negative or offset is negative.
            ConnectionError: If API is unreachable.
            
        Example
        -------
        >>> api = UserAPI()
        >>> users = api.get_users(limit=5)
        >>> for user in users:
        ...     print(user['name'])
        """
        if limit < 0 or offset < 0:
            raise ValueError("Limit and offset must be non-negative")
        
        # Simulated response
        return [
            {"id": i + offset, "name": f"User {i + offset}", "email": f"user{i + offset}@example.com"}
            for i in range(min(limit, 100))
        ]
    
    def get_user(self, user_id: int) -> Optional[Dict[str, Any]]:
        """Get a specific user by ID.
        
        Args:
            user_id: Unique identifier of the user.
            
        Returns:
            User dictionary if found, None if not found.
            
        Example
        -------
        >>> api = UserAPI()
        >>> user = api.get_user(42)
        >>> if user:
        ...     print(user['email'])
        """
        if user_id <= 0:
            raise ValueError("user_id must be positive")
        
        # Simulated response
        return {"id": user_id, "name": f"User {user_id}", "email": f"user{user_id}@example.com"}
    
    def create_user(self, name: str, email: str, **kwargs: str) -> Dict[str, Any]:
        """Create a new user in the system.
        
        Args:
            name: Full name of the user.
            email: Email address of the user.
            **kwargs: Additional optional fields (phone, address, etc.)
            
        Returns:
            Dictionary containing the created user's information.
            
        Raises:
            ValueError: If name or email is invalid.
            
        Example
        -------
        >>> api = UserAPI()
        >>> new_user = api.create_user("John Doe", "john@example.com", phone="555-1234")
        >>> print(new_user['id'])
        """
        if not name or not name.strip():
            raise ValueError("Name cannot be empty")
        if not email or "@" not in email:
            raise ValueError("Invalid email address")
        
        user_id = 999  # Simulated ID
        return {
            "id": user_id,
            "name": name,
            "email": email,
            **kwargs
        }

api = UserAPI()
print("=== User API Demo ===")    # === User API Demo ===
users = api.get_users(limit=3)
print(f"Fetched {len(users)} users")    # Fetched 3 users

new_user = api.create_user("Alice Smith", "alice.smith@example.com", department="Engineering")
print(f"Created user: {new_user['name']} in {new_user.get('department', 'N/A')}")    # Created user: Alice Smith in Engineering


# Real-world Example 2: Data Pipeline with Comprehensive Documentation
class DataPipeline:
    """A data processing pipeline with configurable stages.
    
    This class manages a multi-stage data transformation pipeline
    where each stage can filter, transform, or aggregate data.
    
    Attributes:
        name: Name of the pipeline.
        stages: List of processing stages.
    
    Example
    -------
    >>> pipeline = DataPipeline("etl_pipeline")
    >>> pipeline.add_stage(filter_stage)
    >>> pipeline.add_stage(transform_stage)
    >>> results = pipeline.run(input_data)
    """
    
    def __init__(self, name: str):
        """Initialize the data pipeline.
        
        Args:
            name: Identifier for this pipeline.
        """
        self.name = name
        self.stages: List[Callable[[List[Dict[str, Any]]], List[Dict[str, Any]]]] = []
        self.metadata: Dict[str, Any] = {}
    
    def add_stage(
        self,
        stage_func: Callable[[List[Dict[str, Any]]], List[Dict[str, Any]]],
        name: Optional[str] = None,
        description: Optional[str] = None
    ) -> None:
        """Add a processing stage to the pipeline.
        
        Args:
            stage_func: Function that transforms input data.
            name: Optional name for this stage.
            description: Optional description of what the stage does.
            
        Raises:
            TypeError: If stage_func is not callable.
        """
        if not callable(stage_func):
            raise TypeError("stage_func must be callable")
        
        self.stages.append(stage_func)
        stage_name = name or stage_func.__name__
        self.metadata[stage_name] = {
            "description": description or stage_func.__doc__ or "",
            "function": stage_func
        }
    
    def run(self, input_data: List[Dict[str, Any]]) -> Tuple[bool, List[Dict[str, Any]]]:
        """Execute the pipeline on input data.
        
        Args:
            input_data: List of data records to process.
            
        Returns:
            Tuple of (success, processed_data). If success is False,
            processed_data contains error messages.
            
        Example
        -------
        >>> pipeline = DataPipeline("my_pipeline")
        >>> pipeline.add_stage(lambda data: [d for d in data if d['active']])
        >>> success, results = pipeline.run(data)
        """
        current_data = input_data
        
        for i, stage in enumerate(self.stages):
            try:
                current_data = stage(current_data)
            except Exception as e:
                return (False, [{"stage": i, "error": str(e)}])
        
        return (True, current_data)
    
    def get_documentation(self) -> str:
        """Generate documentation for this pipeline.
        
        Returns:
            Markdown-formatted documentation string.
        """
        lines = [
            f"# Pipeline: {self.name}",
            "",
            "## Stages",
            ""
        ]
        
        for name, info in self.metadata.items():
            lines.append(f"### {name}")
            lines.append("")
            if info["description"]:
                lines.append(info["description"])
            else:
                lines.append("*No description available*")
            lines.append("")
        
        return "\n".join(lines)


def filter_active_users(data: List[Dict[str, Any]]) -> List[Dict[str, Any]]:
    """Filter out inactive user records.
    
    Args:
        data: List of user dictionaries with 'active' field.
        
    Returns:
        List containing only active users.
    """
    return [record for record in data if record.get("active", False)]


def add_processed_timestamp(data: List[Dict[str, Any]]) -> List[Dict[str, Any]]:
    """Add a timestamp to each processed record.
    
    Args:
        data: List of data records.
        
    Returns:
        Data records with 'processed_at' field added.
    """
    import datetime
    timestamp = datetime.datetime.now().isoformat()
    return [{**record, "processed_at": timestamp} for record in data]


pipeline = DataPipeline("user_processing")
pipeline.add_stage(
    filter_active_users,
    name="filter_active",
    description="Remove inactive user records from the dataset"
)
pipeline.add_stage(
    add_processed_timestamp,
    name="add_timestamp",
    description="Add processing timestamp to each record"
)

input_data = [
    {"id": 1, "name": "Alice", "active": True},
    {"id": 2, "name": "Bob", "active": False},
    {"id": 3, "name": "Carol", "active": True}
]

success, result = pipeline.run(input_data)
print("\n=== Data Pipeline Demo ===")    # (blank line)
print(f"Success: {success}")    # Success: True
print(f"Processed records: {len(result)}")    # Processed records: 2


# Real-world Example 3: Library with Type Hints and Docstrings
class RateLimiter:
    """Token bucket rate limiter for API request throttling.
    
    This implementation uses the token bucket algorithm to limit
    the rate of operations per time window.
    
    Attributes:
        rate: Number of tokens per time window.
        capacity: Maximum bucket capacity.
        tokens: Current number of tokens available.
    
    Example
    -------
    >>> limiter = RateLimiter(rate=10, capacity=20)
    >>> if limiter.allow_request():
    ...     make_api_call()
    """
    
    def __init__(self, rate: float, capacity: int):
        """Initialize the rate limiter.
        
        Args:
            rate: Number of tokens added per second.
            capacity: Maximum number of tokens in the bucket.
        """
        self.rate = rate
        self.capacity = capacity
        self.tokens = float(capacity)
        self.last_update = 0.0
    
    def allow_request(self, cost: float = 1.0) -> bool:
        """Check if a request should be allowed.
        
        Args:
            cost: Token cost of the request (default: 1.0).
            
        Returns:
            True if request is allowed, False if rate limited.
            
        Example
        -------
        >>> limiter = RateLimiter(10, 5)
        >>> for i in range(10):
        ...     print(limiter.allow_request(), end=" ")
        True True True True True False False False False False
        """
        import time
        now = time.time()
        elapsed = now - self.last_update
        
        # Add tokens based on elapsed time
        self.tokens = min(self.capacity, self.tokens + elapsed * self.rate)
        self.last_update = now
        
        if self.tokens >= cost:
            self.tokens -= cost
            return True
        
        return False
    
    def get_wait_time(self, cost: float = 1.0) -> float:
        """Get time to wait before request can be processed.
        
        Args:
            cost: Token cost of the request.
            
        Returns:
            Seconds to wait for tokens to become available.
        """
        if self.tokens >= cost:
            return 0.0
        return (cost - self.tokens) / self.rate


limiter = RateLimiter(rate=2.0, capacity=5)
print("\n=== Rate Limiter Demo ===")    # (blank line)
results = [limiter.allow_request() for _ in range(7)]
print(f"Requests allowed: {sum(results)} of 7")    # Requests allowed: 5 of 7 (varies)
print(f"Wait time: {limiter.get_wait_time():.2f} seconds")    # Wait time: 0.00 seconds (varies)


# Real-world Example 4: Configuration Parser with Docstrings
class ConfigParser:
    """Configuration file parser supporting multiple formats.
    
    This class handles parsing of configuration files in various
    formats (JSON, YAML-like) and provides validation.
    
    Attributes:
        config: Parsed configuration dictionary.
        schema: Validation schema for configuration.
    
    Example
    -------
    >>> parser = ConfigParser()
    >>> config = parser.parse('{"app": "myapp", "debug": true}')
    >>> print(config['app'])
    myapp
    """
    
    def __init__(self, schema: Optional[Dict[str, type]] = None):
        """Initialize the configuration parser.
        
        Args:
            schema: Optional dictionary defining expected config types.
        """
        self.config: Dict[str, Any] = {}
        self.schema = schema or {}
    
    def parse(self, config_string: str, format: str = "json") -> Dict[str, Any]:
        """Parse a configuration string.
        
        Args:
            config_string: Raw configuration string.
            format: Format of the config (default: "json").
            
        Returns:
            Parsed configuration dictionary.
            
        Raises:
            ValueError: If config string is invalid.
            TypeError: If format is unsupported.
            
        Example
        -------
        >>> parser = ConfigParser()
        >>> config = parser.parse('{"port": 8080}')
        >>> config['port']
        8080
        """
        if format.lower() == "json":
            try:
                self.config = json.loads(config_string)
            except json.JSONDecodeError as e:
                raise ValueError(f"Invalid JSON: {e}")
        else:
            raise TypeError(f"Unsupported format: {format}")
        
        self._validate()
        return self.config
    
    def _validate(self) -> None:
        """Validate configuration against schema.
        
        Raises:
            TypeError: If configuration doesn't match schema.
        """
        for key, expected_type in self.schema.items():
            if key in self.config:
                if not isinstance(self.config[key], expected_type):
                    raise TypeError(
                        f"Config '{key}' should be {expected_type.__name__}, "
                        f"got {type(self.config[key]).__name__}"
                    )
    
    def get(self, key: str, default: Any = None) -> Any:
        """Get a configuration value.
        
        Args:
            key: Configuration key (supports dot notation for nested).
            default: Default value if key not found.
            
        Returns:
            Configuration value or default.
            
        Example
        -------
        >>> parser = ConfigParser()
        >>> parser.parse('{"database": {"host": "localhost"}}')
        >>> parser.get("database.host")
        localhost
        """
        keys = key.split(".")
        value = self.config
        
        for k in keys:
            if isinstance(value, dict) and k in value:
                value = value[k]
            else:
                return default
        
        return value


schema = {
    "app": str,
    "port": int,
    "debug": bool
}

parser = ConfigParser(schema)
config = parser.parse('{"app": "MyApp", "port": 8080, "debug": true}')
print("\n=== Config Parser Demo ===")    # (blank line)
print(f"App: {parser.get('app')}")    # App: MyApp
print(f"Port: {parser.get('port')}")    # Port: 8080
print(f"Debug: {parser.get('debug')}")    # Debug: True
