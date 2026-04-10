# Check and Build Process

## Learning Objectives

- Run R CMD check
- Fix common issues
- Build and install packages
- Submit to CRAN

## Theory

R CMD check verifies package correctness. It checks documentation, tests, code style, and more. Build creates a package tarball. A successful check with no errors is required before CRAN submission.

## Step-by-Step

### Running Check

```r
library(devtools)

# Development check
check()

# Check and rebuild
check(build = TRUE)
```

### R CMD Check Output

```
0 errors | 0 warnings | 0 notes
```

is the goal. Fix errors first, then warnings, then notes.

### Building Package

```r
# Build source package
build()

# Build binary package
build(binary = TRUE)

# Install package
install()
```

## Code Examples

### Common Check Solutions

```r
# Fix missing exports
#' @export in roxygen2

# Fix checks: NOTE about RoxygenNote
roxygenise()

# Fix test failures
# Update tests or code

# Fix missing documentation
# Add roxygen2 to all exported functions
```

### Pre-Check Checklist

```r
# Run these before check:
devtools::check()  # Regular check
covr::package_coverage()  # Test coverage
lintr::lint_package()  # Code style

# Build package
devtools::build()
```

### Running Full Check

```r
# Full process
roxygenise()
devtools::test()
devtools::check()

# With all extra checks
devtools::check(
  args = c("--as-cran"),
  env_vars = c(LANG = "en_US.UTF-8")
)
```

## Best Practices

1. **Check Often**: Check during development.

2. **Fix Errors**: Zero errors required.

3. **Minimize Notes**: Clear all notes if possible.

4. **Test Coverage**: Aim for high coverage.

5. **Cross-Platform**: Test on multiple platforms.

## Exercises

1. Run devtools::check().

2. Fix check errors.

3. Remove warnings.

4. Clear notes.

5. Build ready package.

## Additional Resources

- [R CMD check](https://cran.r-project.org/doc/manuals/R-exts.html#Checking)
- [Package Development](https://r-pkgs.org/)
- [CRAN Submission](https://cran.r-project.org/web/packages/policies.html)