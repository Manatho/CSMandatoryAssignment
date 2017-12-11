# CSMandatoryAssignment

Requires: Visual Studio 2017

## Startup
- Open AssignmentPresentation.sln
- Rebuild the project
- Run all tests
- Set AssignmentNewPresentation as the startup project
- Start the project.

## Codes
The accepted serialcodes range from 0 to 99

## Files
Two files will be created and saved in \AssignmentPresentation\AssignmentNewPresentation\Files:
- Serialcodes.dat
- Submmissions.dat

## Known issues
Submiting dates with above the year 9999 will cause the Form to pass along an empty Date object and thus failing domain side validation.