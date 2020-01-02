# SportTracks Weight Exporter

Simple tool for exporting weight data from the [SportTracks 3](https://www.zonefivesoftware.com/sporttracks/support/updates.php) desktop application.

Since the application has no built-in export functionality and uses a proprietary file format, the tool uses [FlaUI](https://github.com/FlaUI/FlaUI) automation library to copy the values from the application user interface and save them to a CSV file in the tool directory.

SportTracks 3 must be already running when the tool is launched. Don't use any other applications while the export process is running to not interfere with the keyboard commands being sent to the application.
