MySQLCompare
============

An application that helps compare schemas between two MySQL databases, and generates SQL to bring them to parity.

Currently available as a WPF / C# application for Windows XP / 7 / 8 (Desktop) with plans for a Mac OS X version in the future.

Licensed under GPL.

Installation / Configuration
----------------------------

To use, download the application executable package from the "Downloads" section, and unzip wherever you would like to run the application from. It's completely portable, and needs no formal installation or uninstallation.

To add or modify MySQL connection profiles, open "Settings / Profiles.xml" with any text or XML editor, and enter your connection details following the examples provided (the example profiles are safe to remove).

Then, launch the application by double-clicking "MySQLCompare.exe".

Usage
-----

Use by dragging and dropping any profile from the "Profiles" panel to the "Left" and "Right" profile slots at the top. Then, click ">|< Compare Schema" to see comparison results.

In the results panel, fields missing from the left profile appear left-justified, fields missing from the right table appear right-justified, and tables missing from the right profile appear with a red title.

Important Notice
----------------

SQL is generated in the bottom panel that aids in bringing the right profile into parity with the left. However, this project is still very much a work in progress, and I highly recommend manually verifying all SQL statements generated before running them against your databases. These statements must be manually run against your SQL database -- there is currently no support for automatically running these commands from within the application, by design.

Using the Source
----------------

To build and run the source in Visual Studio, you'll need to install the MySQL ADO.NET connector (http://www.mysql.com/products/connector/).

The application uses a "Josh Smith" style MVVM structure for the most part. If you have ideas, suggestions, or pull requests, I'd love to see them!

Informal Roadmap
----------------

Planning to flesh out the UI to look a bit more polished, and still deciding whether to create an "in app" settings screen to replace the external XML file (personally, I find external settings files helpful quite often). Also would like to add the ability to compare not just the schema, but the actual data of selected tables, as well.

Of equal priority is building a native Cocoa / OS X version for Mac users like myself. :)