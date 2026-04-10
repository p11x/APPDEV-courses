# DESCRIPTION File in R Packages

## Learning Objectives

- Understand DESCRIPTION file structure
- Learn required fields
- Specify dependencies
- Document package metadata

## Theory

The DESCRIPTION file is the package's metadata file. It contains package name, version, description, author information, dependencies, and licensing details. It follows RFC 822-style format with field: value pairs.

## Step-by-Step

### Required Fields

```
Package: mypackage
Title: My Package Title
Version: 0.1.0
Description: A short description of what the package does.
License: MIT
Author: Your Name your@email.com [aut, cre]
Maintainer: Your Name your@email.com [cre]
```

### Common Fields

```
Depends: R (>= 3.5.0)
Imports: 
    dplyr,
    ggplot2
Suggests: 
    testthat
URL: https://github.com/yourname/mypackage
BugReports: https://github.com/yourname/mypackage/issues
Encoding: UTF-8
```

## Code Examples

### Complete DESCRIPTION

```
Package: mypackage
Title: My Package for Data Analysis
Version: 0.1.0
Authors@R: person("John", "Doe", email = "john@example.com", 
                  role = c("aut", "cre"))
Description: A package for analyzing data with statistical methods.
License: MIT + file LICENSE
Depends: R (>= 4.0.0)
Imports: 
    dplyr (>= 1.0.0),
    ggplot2 (>= 3.4.0),
    tidyr,
    rlang
Suggests: 
    testthat (>= 3.0.0),
    knitr,
    covr
URL: https://github.com/yourname/mypackage
BugReports: https://github.com/yourname/mypackage/issues
Encoding: UTF-8
RoxygenNote: 7.2.3
Collate: 
    'utils.R'
    'analysis.R'
```

### Versioning

```
Version: 0.1.0.9000
# 0.1.0 - major.minor.patch
# 0.1.0.9000 - development version
```

### Licensing

```r
# MIT license - most common
License: MIT

# GPL-2 or GPL-3
License: GPL (>= 2)

# Apache 2.0
License: Apache (>= 2.0)

# CC0 for public domain
License: CC0

# Proprietary - specify
License: file LICENSE
```

## Best Practices

1. **Update Version**: Increment for each release.

2. **Specify Versions**: Min version for dependencies.

3. **Document All Authors**: Include contributions.

4. **License**: Include license file for file-based.

5. **Check**: Use devtools::check() for errors.

## Exercises

1. Create package skeleton.

2. Edit DESCRIPTION file.

3. Add dependencies.

4. Add authors.

5. Check package.

## Additional Resources

- [R Packages Book](https://r-pkgs.org/)
- [Writing R Extensions](https://cran.r-project.org/doc/manuals/R-exts.html)
- [DESCRIPTION Format](https://cran.r-project.org/web/packages/policies.html)