# Introduction to Version Control

## What You'll Learn

- What version control is and why it matters
- The difference between centralized and distributed systems
- Why Git has become the industry standard
- Key vocabulary you'll use daily
- How version control fits into web development

## Prerequisites

None—this is a conceptual introduction for those new to version control.

## What Is Version Control?

Imagine you're writing a document. Every time you make a major change, you save a new version:

```
v1.txt     → First draft
v2.txt     → Added introduction
v3.txt     → Fixed typos
v4.txt     → Added conclusion
v5.txt     → Final version
```

Now imagine doing this for a web application with thousands of files, multiple developers, over months of work. That's impossible to manage manually.

**Version control** is a system that:
- Tracks every change to your code
- Lets you see who made what change
- Allows you to revert to any previous version
- Enables multiple people to work on the same project
- Helps you merge changes without losing work

## Types of Version Control Systems

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    VERSION CONTROL SYSTEM TYPES                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  CENTRALIZED (CVCS):                                                        │
│  ┌─────────────────┐                                                       │
│  │   Central       │                                                       │
│  │   Server        │◄────── Client A                                       │
│  │                 │◄────── Client B                                       │
│  │                 │◄────── Client C                                       │
│  └─────────────────┘                                                       │
│                                                                             │
│  Examples: CVS, Subversion (SVN)                                           │
│  Pros: Simple, single source of truth                                     │
│  Cons: No offline work, single point of failure                            │
│                                                                             │
│  DISTRIBUTED (DVCS):                                                        │
│  ┌─────────────────┐    ┌─────────────────┐                              │
│  │   Central       │    │   Local         │                              │
│  │   Server        │◄──▶│   Repository    │                              │
│  └─────────────────┘    └─────────────────┘                              │
│                              │                                            │
│                              ▼                                            │
│                        ┌─────────────────┐                              │
│                        │   Local         │                              │
│                        │   Repository    │                              │
│                        └─────────────────┘                              │
│                                                                             │
│  Examples: Git, Mercurial, Bazaar                                          │
│  Pros: Full offline work, multiple backups, flexible workflows           │
│  Cons: More complex, steeper learning curve                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Why Git?

Git is the dominant version control system today:

| Feature | Why It Matters |
|---------|-----------------|
| Distributed | Work offline, work anywhere |
| Speed | Blazing fast, even for large projects |
| Branching | Cheap, easy, encouraged |
| Security | Cryptographically verified |
| Community | Massive ecosystem, countless resources |

Git was created by Linus Torvalds in 2005 for Linux kernel development. It was designed to handle the massive scale of the Linux kernel—thousands of contributors, millions of lines of code.

## Key Vocabulary

These terms will become second nature, but here's your starting vocabulary:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GIT VOCABULARY                                            │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  REPOSITORY (REPO)                                                         │
│  The entire project—code, history, configuration.                         │
│  Think: A folder that remembers everything.                               │
│                                                                             │
│  COMMIT                                                                    │
│  A snapshot of your code at a point in time.                             │
│  Think: A save point with a description.                                  │
│                                                                             │
│  BRANCH                                                                    │
│  A separate line of development.                                          │
│  Think: A parallel universe where you're trying something new.           │
│                                                                             │
│  MERGE                                                                      │
│  Combining changes from one branch into another.                          │
│  Think: Bringing your parallel work back together.                       │
│                                                                             │
│  PULL                                                                       │
│  Getting changes from a remote repository.                                │
│  Think: Downloading updates from the team.                               │
│                                                                             │
│  PUSH                                                                      │
│  Sending your changes to a remote repository.                            │
│  Think: Uploading your work to share with the team.                      │
│                                                                             │
│  FORK                                                                       │
│  Creating your own copy of someone else's repository.                    │
│  Think: Taking a project and making it your own.                         │
│                                                                             │
│  PULL REQUEST (PR)                                                         │
│  Requesting that your changes be merged into a project.                  │
│  Think: Submitting your work for review and inclusion.                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## How Git Thinks

Here's the mental model that will help you understand Git:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GIT'S THREE STATES                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌─────────────────────────────────────────────────────────────┐           │
│  │                                                             │           │
│  │  WORKING DIRECTORY                                         │           │
│  │  Your actual files on disk                                │           │
│  │  (the files you're editing right now)                      │           │
│  │                                                             │           │
│  └───────────────────────┬─────────────────────────────────┘           │
│                          │                                                  │
│                          │ git add                                         │
│                          ▼                                                 │
│  ┌─────────────────────────────────────────────────────────────┐           │
│  │                                                             │           │
│  │  STAGING AREA (INDEX)                                      │           │
│  │  What will go in your next commit                          │           │
│  │  (like a loading dock—items ready to ship)                  │           │
│  │                                                             │           │
│  └───────────────────────┬─────────────────────────────────┘           │
│                          │                                                  │
│                          │ git commit                                      │
│                          ▼                                                 │
│  ┌─────────────────────────────────────────────────────────────┐           │
│  │                                                             │           │
│  │  GIT DIRECTORY (.git)                                      │           │
│  │  The database storing all commits                          │           │
│  │  (like a vault—permanent storage)                          │           │
│  │                                                             │           │
│  └─────────────────────────────────────────────────────────────┘           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Version Control in Web Development

Why does a web developer need version control? Let me count the ways:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHY WEB DEVELOPERS NEED GIT                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ BACKUP: Your code is safely stored in multiple places                 │
│                                                                             │
│  ✅ COLLABORATION: Multiple developers work on the same project           │
│                    without overwriting each other's work                   │
│                                                                             │
│  ✅ EXPERIMENTATION: Try new features in branches without                 │
│                      breaking the working code                             │
│                                                                             │
│  ✅ HISTORY: See exactly what changed, when, and why                      │
│                                                                             │
│  ✅ ROLLBACK: Instantly revert to a working version if something breaks   │
│                                                                             │
│  ✅ CODE REVIEW: Teammates can review changes before merging               │
│                                                                             │
│  ✅ DEPLOYMENT: Connect to deployment pipelines that run tests             │
│                 and deploy your code automatically                         │
│                                                                             │
│  ✅ OPEN SOURCE: Contribute to and benefit from open source projects      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## The Basic Workflow

Here's what your typical day looks with Git:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    TYPICAL GIT WORKFLOW                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. START DAY                                                              │
│     git pull  ← Get latest changes from team                              │
│                                                                             │
│  2. WORK ON TASK                                                          │
│     Write code, make changes                                               │
│                                                                             │
│  3. SAVE WORK                                                             │
│     git add -A   ← Stage your changes                                     │
│     git commit   ← Save a snapshot                                         │
│                                                                             │
│  4. SHARE WORK                                                            │
│     git push   ← Upload to shared repository                              │
│                                                                             │
│  5. INTEGRATE                                                            │
│     Create PR ← Request to merge your work                                │
│                                                                             │
│  Repeat!                                                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Git vs GitHub

A common source of confusion:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GIT VS GITHUB                                            │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  GIT                                                                       │
│  • The version control system                                             │
│  • Command-line tool (git)                                               │
│  • Runs on your computer                                                  │
│  • Manages code history locally                                          │
│                                                                             │
│  GITHUB                                                                    │
│  • A website/service that hosts Git repositories                         │
│  • Provides a web interface for Git                                       │
│  • Adds collaboration features (PRs, issues, projects)                   │
│  • Cloud storage for your code                                           │
│                                                                             │
│  Analogy:                                                                 │
│  Git is like a camera (the tool)                                          │
│  GitHub is like Instagram (a platform to share photos)                   │
│                                                                             │
│  Alternatives to GitHub:                                                  │
│  • GitLab (gitlab.com)                                                   │
│  • Bitbucket (bitbucket.org)                                             │
│  • Gitea (self-hosted)                                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- Version control tracks all changes to your code
- Git is distributed, fast, and designed for collaboration
- Key concepts: repository, commit, branch, merge, pull, push, fork, PR
- Git has three states: working directory, staging area, repository
- GitHub hosts Git repositories and adds collaboration features

## Next Steps

→ Continue to `02-git-basics.md` to learn essential Git commands you'll use daily.
