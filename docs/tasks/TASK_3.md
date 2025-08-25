# Task A.1.7: Code Coverage Setup with Coverlet

## **Objective**
Integrate code coverage collection using Coverlet.Collector across all test projects to measure test coverage in CI.

## **Scope**
**SINGLE TASK** - Only code coverage setup. No benchmarks, no serialization interfaces yet.

## **Requirements**

### **Coverlet Integration**
- [x] Add `coverlet.collector` package to all test projects
- [x] Configure `Directory.Build.props` for coverage settings
- [x] Update CI workflow to collect coverage data
- [x] Generate coverage reports in Cobertura format
- [x] Upload coverage reports as CI artifacts

### **Configuration Updates**
- [x] Update `.github/workflows/ci.yml` to include coverage collection
- [x] Configure coverage output directory and format
- [x] Ensure coverage data is collected for all target frameworks

## **Acceptance Criteria**

### **Package Integration**
- [x] `coverlet.collector` package added to all test projects
- [x] Package version is consistent across all test projects
- [x] No package conflicts or dependency issues
- [x] Package references are properly configured in `.csproj` files

### **Build Configuration**
- [x] `Directory.Build.props` contains coverage configuration
- [x] `CollectCoverage` property set to `true`
- [x] `CoverletOutputFormat` set to `cobertura`
- [x] Coverage output path configured correctly
- [x] Coverage collection working for both Debug and Release

### **Coverage Collection**
- [x] Coverage data collected during test execution
- [x] Coverage collection works for both `net9.0` and `netstandard2.1` frameworks
- [x] Coverage files generated in Cobertura format (`.xml`)
- [x] Coverage data includes all main projects (not test projects)
- [x] No coverage collection errors or warnings

### **CI Integration**
- [x] CI workflow updated to include coverage collection
- [x] `--collect:"XPlat Code Coverage"` flag added to test command
- [x] Coverage reports uploaded as separate artifacts
- [x] Coverage artifacts named appropriately (`coverage-reports`)
- [x] Coverage data available for download after workflow completion

### **Output Validation**
- [x] Coverage files generated in expected directory (`TestResults/`)
- [x] Coverage files contain valid Cobertura XML format
- [x] Coverage data includes line coverage information
- [x] Coverage data includes branch coverage information
- [x] Coverage reports are human-readable and parseable

### **Performance Impact**
- [x] Test execution time increased by < 50% due to coverage collection
- [x] No memory issues during coverage collection
- [x] Coverage collection doesn't cause test failures
- [x] CI pipeline completion time remains reasonable

### **Build Stability**
- [x] All existing builds continue to work
- [x] No new build warnings or errors introduced
- [x] Coverage configuration doesn't break existing functionality
- [x] Both Debug and Release configurations work correctly

## **Technical Implementation**

### **Directory.Build.props Addition**
```xml
<!-- Add to Directory.Build.props -->
<PropertyGroup>
  <CollectCoverage>true</CollectCoverage>
  <CoverletOutputFormat>cobertura</CoverletOutputFormat>
</PropertyGroup>
```

### **CI Workflow Update**
```yaml
# Update in .github/workflows/ci.yml
- name: Test with Coverage
  run: dotnet test --configuration Release --no-build --collect:"XPlat Code Coverage" --logger "trx;LogFileName=test_results.trx" --results-directory ./TestResults

- name: Upload Coverage Reports
  uses: actions/upload-artifact@v4
  with:
    name: coverage-reports
    path: |
      **/TestResults/**/coverage.cobertura.xml
    if-no-files-found: ignore
```

## **Success Criteria**
- [x] Coverlet.Collector added to all test projects
- [x] Coverage data collected during CI runs
- [x] Cobertura format reports generated
- [x] Coverage reports uploaded as artifacts
- [x] No build errors or warnings
- [x] Coverage collection doesn't significantly slow down tests

## **Dependencies**
- [x] TASK_2 completed (CI/CD pipeline working)
- [x] Coverlet.Collector NuGet package
- [x] Existing test project structure

## **Out of Scope**
- ❌ Coverage reporting tools (ReportGenerator)
- ❌ Coverage thresholds and alerts
- ❌ Advanced coverage analysis
- ❌ Benchmark integration
- ❌ Serialization interfaces

## **Estimated Effort**
- **Total**: 1-2 hours ✅ **COMPLETED**

## **Next Steps After Completion**
- **TASK_4**: Benchmark job setup
- **TASK_5**: Core serialization interfaces
- **TASK_6**: Advanced coverage reporting (optional)

## **Notes**
- ✅ Focus on getting basic coverage working - **COMPLETED**
- ✅ Ensure coverage doesn't break existing CI pipeline - **COMPLETED**
- ✅ Coverage data should be available for manual review - **COMPLETED**
- ✅ Keep configuration simple and maintainable - **COMPLETED**

## **Status: ✅ COMPLETED**
**TASK_3 has been successfully implemented with all acceptance criteria met. Code coverage infrastructure is now in place and working correctly in both local development and CI environments.**
