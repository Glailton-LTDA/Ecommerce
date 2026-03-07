# Code Coverage Setup Guide

## GitHub Actions Workflow with Code Coverage

The workflow in `.github/workflows/dotnet-tests.yml` has been configured to:

### 1. **Run Tests with Code Coverage**
- Uses Coverlet (already installed in your Tests.csproj) to collect coverage data
- Generates OpenCover format XML files
- Stores coverage in the `./coverage/` directory

### 2. **Upload Coverage to Codecov**
- Automatically uploads coverage reports to [Codecov.io](https://codecov.io)
- Provides visual coverage reports and trend tracking
- Integrates with pull requests to show coverage changes

### 3. **Test Results Artifacts**
- Stores test results as artifacts for easy review
- Can be downloaded from GitHub Actions run summary

## Setup Instructions

### Step 1: Enable Codecov Integration (Optional but Recommended)

1. Go to https://codecov.io
2. Sign in with your GitHub account
3. Grant permissions to access your repository
4. Your repository will be automatically set up

### Step 2: View Coverage Reports

**In Codecov Dashboard:**
- Visit your Codecov project page
- See coverage percentage, trends, and file-by-file breakdown
- Coverage badges available for your README

**In GitHub:**
- Coverage reports appear in Pull Request comments
- Shows coverage changes compared to the base branch

### Step 3: Add Coverage Badge to README (Optional)

Add this to your `README.md`:
```markdown
[![codecov](https://codecov.io/gh/Glailton-LTDA/Ecommerce/branch/main/graph/badge.svg)](https://codecov.io/gh/Glailton-LTDA/Ecommerce)
```

## How It Works

When you push code or create a pull request:

1. ✅ Tests run with coverage collection
2. 📊 Coverage data is generated (OpenCover XML format)
3. 📤 Coverage is uploaded to Codecov
4. 📝 Coverage report appears in the GitHub Actions summary
5. 📧 Codecov comments on PRs with coverage impact

## View Local Coverage (Optional)

To generate coverage reports locally:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./coverage/
```

## Customization

You can modify the workflow to:
- Change coverage thresholds
- Generate HTML reports
- Fail CI if coverage drops
- Add multiple coverage formats

Edit `.github/workflows/dotnet-tests.yml` to customize.
