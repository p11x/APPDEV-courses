# Early Development of R

## Learning Objectives

- Understand the origins of R from the S language
- Trace the initial creation of R at the University of Auckland
- Recognize the key motivations behind R's development

## Theoretical Background

### The S Language Heritage

R traces its heritage to the S language, developed by John Chambers and colleagues at Bell Labs starting in 1976. S was revolutionary in its approach to statistical computing:

- **Interactive Environment**: S was designed as an interactive environment for data exploration and analysis
- **Object-Oriented Philosophy**: Introduced the concept that "everything that exists is an object"
- **Statistical Focus**: Built specifically for statistical computing and graphical techniques
- **Extensibility**: Allowed users to extend functionality through functions and packages

### The Creation of R

R was created by **Ross Ihaka** and **Robert Gentleman** at the University of Auckland, New Zealand, in 1993. The name "R" has dual origins:

1. The shared first letter of the creators' names (Ross and Robert)
2. A play on the name of the S language (R being the letter before S alphabetically)

### Initial Release

- **1993**: R conceived and developed at University of Auckland
- **1995**: Initial public release of R
- **1997**: R Core Group established, CRAN (Comprehensive R Archive Network) created

## Step-by-Step Explanation

### Motivation for Creating R

The creators developed R for several reasons:

1. **Educational Needs**: Needed a statistical environment for teaching statistics at university
2. **Open Source Desire**: Wanted a free, open-source alternative to S and SAS
3. **Platform Independence**: Desired a language that could run on multiple operating systems
4. **Modernization**: Wished to improve upon S with new features and better design

### The Open Source Decision

Ross Ihaka and Robert Gentleman made a pivotal decision to release R as open source under the GNU General Public License. This decision was fundamental to R's success because it:

- Allowed global collaboration on language development
- Enabled researchers to share statistical methods freely
- Created an incentive for the community to contribute packages
- Made R accessible to everyone regardless of budget

## Code Examples

### Checking S Language Influence

```r
# Demonstrating S language influence on R
# Both S and R share similar syntax and paradigms

# Create a vector (fundamental data structure in both S and R)
x <- c(1, 2, 3, 4, 5)

# Apply a function (functional programming roots from S)
mean(x)
sum(x)

# Statistical functions built-in
summary(x)

# The object-oriented nature: everything is an object
class(x)  # "numeric" - x is an object with a class
mode(x)   # "numeric" - x has a storage mode
```

**Output:**
```
[1] 3
[1] 15
   Min. 1st Qu.  Median    Mean 3rd Qu.    Max. 
      1       2       3       3       4       5 
[1] "numeric"
[1] "numeric"
```

**Comments:**
- `c()` function creates vectors (same in S and R)
- Statistical functions are first-class citizens
- Object system allows introspection of any variable

## Best Practices

1. **Understand the heritage**: Knowing R's S language roots helps understand design decisions
2. **Appreciate open source**: The GNU GPL license enabled R's ecosystem growth

## Further Reading

- R FAQ: https://cran.r-project.org/doc/FAQ/R-FAQ.html
- Ross Ihaka's papers on R development
