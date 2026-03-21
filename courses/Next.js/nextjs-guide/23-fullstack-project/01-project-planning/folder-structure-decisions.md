# Folder Structure Decisions

## What You'll Learn
- Project structure
- Organization choices
- Best practices

## Prerequisites
- Understanding of App Router

## Do I Need This Right Now?
Good structure makes projects maintainable.

## Project Structure

```
task-manager/
├── app/
│   ├── (auth)/           # Auth routes (login, register)
│   │   ├── login/
│   │   └── register/
│   ├── (dashboard)/      # Protected dashboard routes
│   │   └── dashboard/
│   ├── api/              # API routes
│   ├── layout.tsx        # Root layout
│   └── page.tsx          # Home page
├── components/           # React components
├── lib/                  # Utilities
│   ├── auth.ts          # Auth configuration
│   └── db.ts            # Database client
├── prisma/              # Database schema
└── public/               # Static assets
```

## Summary
- Group by feature where possible
- Keep components and lib separate
- Use route groups for auth
