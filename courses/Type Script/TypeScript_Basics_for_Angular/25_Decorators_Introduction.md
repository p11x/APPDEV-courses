# Decorators Introduction

## What Are Decorators?

Decorators are a special kind of declaration that can be attached to classes, methods, properties, or parameters. They use the `@` symbol followed by the decorator name and are placed immediately before the item being decorated. Decorators are a fundamental part of Angular's architecture.

In TypeScript, decorators are enabled by setting `experimentalDecorators` to `true` in your `tsconfig.json` file. They were finalized in TypeScript 5.0 but have been available as an experimental feature for years.

## How Decorators Work

A decorator is essentially a function that gets called at runtime with information about the decorated item. There are several types of decorators:

- **Class decorators**: Applied to classes
- **Method decorators**: Applied to class methods
- **Property decorators**: Applied to class properties  
- **Parameter decorators**: Applied to function parameters

Here's a simple example of a class decorator:

```typescript
function Log(target: Function) {
    console.log(`Class ${target.name} was defined`);
}

@Log
class MyClass {
    // Class definition
}
```

## Decorators in Angular

Angular uses decorators extensively to define application components, services, and other Angular-specific constructs. Here are the most common Angular decorators:

### @Component
The `@Component` decorator marks a class as an Angular component and provides configuration metadata:

```typescript
@Component({
    selector: 'app-user-list',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.component.css']
})
export class UserListComponent {
    // Component logic
}
```

### @Injectable
The `@Injectable` decorator marks a class as available for dependency injection:

```typescript
@Injectable({
    providedIn: 'root'
})
export class UserService {
    getUsers() {
        // Service logic
    }
}
```

### @Input and @Output
These decorators mark properties as component inputs and outputs:

```typescript
export class UserComponent {
    @Input() user: User;
    @Output() userSelected = new EventEmitter<User>();
}
```

## Key Points to Remember

- Decorators use the `@` symbol and are placed before the decorated item
- TypeScript requires `experimentalDecorators` in tsconfig.json
- Angular decorators provide metadata to the Angular framework
- Decorators are functions that receive information about the decorated item
- Multiple decorators can be applied to the same item

## Connection to Angular

Decorators are fundamental to Angular development because they:

- Define components, services, directives, and pipes
- Configure dependency injection
- Mark properties for data binding
- Define routing and module organization

Understanding decorators is essential for understanding how Angular applications are structured and how the framework provides its powerful features through declarative code.
