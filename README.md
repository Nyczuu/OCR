# OCR

Simple console app responsible for scanning images.
Application detects rectangles and relations between them, as a result of the operation both result image and json file will be created.
Each application run will create results with GUID-named subfolder, containing folders for each scanned image.
To run the appliction you can pass path to folder as first command line parameter.
If no path specified, application will look for the images inside TestData folder which should be in the same directory as application.
