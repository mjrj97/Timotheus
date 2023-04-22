<p align="center">
  <img style="width: 75%" src="https://mjrj.dk/img/software/screenshots/TimotheusEnglish.png" alt="Timotheus screenshot"/>
</p>

# Description :memo:
The purpose of this project is to create software that can manage a small association's calendar, data storage, consent forms etc. in a GDPR compliant fashion (or atleast to its best ability). The program is perfectly suited for organizations, that needs to securely share files internally without using services like Dropbox, Google Drive, and OneDrive, that has been shown to comprise with the GDPR laws. The program can also help handle a calendar/schedule, and manage a list of consent forms. The project is primarily aimed towards small associations e.g. the Danish LM and IM, that handle sensitive data (names, ages, addresses etc.).

# Features :fire:
:heavy_check_mark: Platform-Independent (Windows, macOS)<br/>
:heavy_check_mark: Calendar editor with publishing functionality<br/>
:heavy_check_mark: File synchronization using SFTP<br/>
:heavy_check_mark: English and Danish localization<br/>
:heavy_check_mark: GDPR consent form management<br/>
:x: Group Management<br/>
:x: Accounting

# How to use :rocket:
The latest version can be downloaded from [Martin's website](https://mjrj.dk/Home/Software/0). The program revolves around the **key file**. This key file is used to save the last opened calendar, people file, usernames, password etc. When the user opens or save fx. the calendar, the key file saves the path to the calendar, so it loads the correct file. When creating a new key file, it should be placed in an empty folder, and the calendar and consent forms are required to be in the same folder (or subfolders).

If the calendar is connected to a remote calendar, the key can also save the username and password. The key is encrypted with a password, which is chosen when the key is saved. This password is required to open (decrypt) the key on subsequent uses.
</br></br>
There are plans to create YouTube tutorials for different use cases, but these are not in the work at the moment.

# Contribution :building_construction:
You are welcome to contribute to the project!</br>
Send an e-mail to martin.jensen.1997@hotmail.com for further information.
</br></br>
Microsoft Visual Studio with the [Avalonia extension](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaVS) is highly recommended since it adds an editor to VS, which allows you to see the UI while you edit and add components.
</br></br>
All commits should be on their own branch before being merged into the pre-release branch. On the pre-release branch the program is thoroughly tested before being pulled to the master branch with a version number (Pull requests to the master are done by Martin). The program is being used by an association to handle sensitive data, so quality and security is a priority! :lock:
</br></br>
This project has taken a lot of effort, so any monetary contribution is greatly appreciated. The money will first and foremost go to pay for the $99 / 779 kr. Apple Developer License, so the macOS version can keep getting updates.</br></br>
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/donate/?hosted_button_id=XUQ6GLJNQFGCC)

# Libraries :card_file_box:
[Avalonia](https://github.com/AvaloniaUI/Avalonia) 0.10.15</br>
[FluentFTP](https://github.com/robinrodricks/FluentFTP) 37.0.3</br>
[iCal.Net](https://github.com/rianjs/ical.net) 4.2.0</br>
[PdfSharpCore](https://github.com/ststeiger/PdfSharpCore) 1.3.30</br>
[SSH.NET](https://github.com/sshnet/SSH.NET) 2020.0.1
