// AngularComponentDemo - Demonstrates OOP for Angular Component Structure
// Use case: Modeling Angular components in Java backend

public class AngularComponentDemo {
    // Component properties (fields)
    private String componentName;
    private String template;
    private String[] inputs;
    private String[] outputs;
    private boolean isActive;
    
    // Constructor
    public AngularComponentDemo(String name) {
        this.componentName = name;
        this.template = "<div>Default Template</div>";
        this.inputs = new String[] {};
        this.outputs = new String[] {};
        this.isActive = true;
    }
    
    // Component initialization
    public void initialize(String template, String[] inputs, String[] outputs) {
        this.template = template;
        this.inputs = inputs;
        this.outputs = outputs;
        System.out.println("Component initialized: " + componentName);
    }
    
    // Input binding methods
    public void setInput(String key, String value) {
        System.out.println("Input bound: " + key + " = " + value);
    }
    
    // Output event methods
    public String emitEvent(String eventName, Object data) {
        System.out.println("Event emitted: " + eventName);
        return "Event(" + eventName + ", " + data + ")";
    }
    
    // Template rendering
    public String render() {
        return "Rendering: " + componentName + " with template:\n" + template;
    }
    
    // Lifecycle hook
    public void onInit() {
        System.out.println(componentName + " - ngOnInit called");
    }
    
    public void onDestroy() {
        System.out.println(componentName + " - ngOnDestroy called");
    }
    
    public static void main(String[] args) {
        System.out.println("=== OOP FOR ANGULAR COMPONENTS ===\n");
        
        // Create component
        AngularComponentDemo userList = new AngularComponentDemo("UserListComponent");
        
        // Initialize with inputs/outputs
        userList.initialize(
            "<ul><li *ngFor='let user of users'>{{user.name}}</li></ul>",
            new String[] {"users", "title"},
            new String[] {"userSelected", "userDeleted"}
        );
        
        // Simulate lifecycle
        userList.onInit();
        
        // Bind inputs
        userList.setInput("title", "User List");
        
        // Emit events
        userList.emitEvent("userSelected", "{id: 1}");
        
        // Render
        System.out.println("\n" + userList.render());
        
        // Cleanup
        userList.onDestroy();
        
        System.out.println("\n=== OOP MAPPING ===");
        System.out.println("Java Class -> TypeScript Class");
        System.out.println("Constructor -> Component constructor");
        System.out.println("Methods -> Component methods");
        System.out.println("Fields -> Component properties");
    }
}
